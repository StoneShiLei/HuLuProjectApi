using Furion.JsonSerialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furion.Cache
{
    public class RedisCacheHelper
    {
        private static RedisCache _redisCache = null;
        private static RedisCacheOptions options = null;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="instanceName"></param>
        public RedisCacheHelper(string connectionString, string instanceName)
        {
            options = new RedisCacheOptions
            {
                Configuration = connectionString,
                InstanceName = instanceName
            };
            _redisCache = new RedisCache(options);

        }
        /// <summary>
        /// 初始化Redis
        /// </summary>
        public void InitRedis(string connectionString, string instanceName)
        {
            options = new RedisCacheOptions
            {
                Configuration = connectionString,
                InstanceName = instanceName
            };
            _redisCache = new RedisCache(options);
        }

        /// <summary>
        /// 添加string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="ExprireTime">过期时间 单位小时</param>
        /// <returns></returns>
        public bool SetStringValue(string key, string value, int ExprireTime = 0)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            try
            {
                _redisCache.SetString(key, value, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = ExprireTime > 0 ? DateTime.Now.AddHours(ExprireTime) : null
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取或者新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ExprireTime"></param>
        /// <returns></returns>
        public T GetOrCreate<T>(string key, Func<T> create, int ExprireTime = 0)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            try
            {
                var data = Get<T>(key);
                if (data == null)
                {
                    var result = create();
                    if (result != null)
                    {
                        SetStringValue(key, JSON.Serialize(result), ExprireTime);
                        return result;
                    }
                    else
                        return default;
                }
                return data;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 更新缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="create"></param>
        /// <param name="ExprireTime"></param>
        /// <returns></returns>
        public T Update<T>(string key, Func<T> create, int ExprireTime = 0)
        {
            if (string.IsNullOrEmpty(key)) return default;

            try
            {
                var result = create();
                if (result == null) return default;

                SetStringValue(key, JSON.Serialize(result), ExprireTime);
                return result;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 获取string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetStringValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            try
            {
                return _redisCache.GetString(key);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取数据（对象）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            string value = GetStringValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            try
            {
                var obj = JSON.Deserialize<T>(value);
                return obj;
            }
            catch (Exception)
            {
                return default;
            }
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key">键</param>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                _redisCache.Remove(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="key">键</param>
        public bool Refresh(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                _redisCache.Refresh(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间 单位小时</param>
        public bool Replace(string key, string value, int expireTime = 24)
        {
            if (Remove(key))
            {
                return SetStringValue(key, value, expireTime);
            }
            else
            {
                return false;
            }
        }
    }
}
