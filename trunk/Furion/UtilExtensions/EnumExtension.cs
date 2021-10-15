using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Furion.UtilExtensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="e">枚举</param>
        /// <returns>枚举描述</returns>
        public static string GetEnumDesc(this Enum e)
        {
            try
            {
                var enumInfo = e.GetType().GetField(e.ToString());
                var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return enumAttributes.Length > 0 ? enumAttributes[0].Description : enumInfo.Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="e">枚举</param>
        /// <returns>枚举描述</returns>
        public static string GetEnumDesc(this FieldInfo e)
        {
            try
            {
                var enumAttributes = (DescriptionAttribute[])e.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return enumAttributes.Length > 0 ? enumAttributes[0].Description : e.Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获得枚举字段的名称。
        /// </summary>
        /// <returns></returns>
        public static string GetName(this Enum thisValue)
        {
            return Enum.GetName(thisValue.GetType(), thisValue);
        }

        /// <summary>
        /// 获得枚举字段的值。
        /// </summary>
        /// <returns></returns>
        public static T GetValue<T>(this Enum thisValue)
        {
            return (T)Enum.Parse(thisValue.GetType(), thisValue.ToString());
        }
    }
}
