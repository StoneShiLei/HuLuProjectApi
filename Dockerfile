FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS restore
WORKDIR /src
COPY ["trunk/HuLuProject.Web.Entry/HuLuProject.Web.Api.csproj", "HuLuProject.Web.Entry/"]
COPY ["trunk/HuLuProject.Web.Core/HuLuProject.Web.Core.csproj", "HuLuProject.Web.Core/"]
COPY ["trunk/Furion.Extras.Logging.Serilog/Furion.Extras.Logging.Serilog.csproj", "Furion.Extras.Logging.Serilog/"]
COPY ["trunk/HuLuProject.Application/HuLuProject.Application.csproj", "HuLuProject.Application/"]
COPY ["trunk/HuLuProject.Core/HuLuProject.Core.csproj", "HuLuProject.Core/"]
COPY ["trunk/Furion.Extras.DatabaseAccessor.MongoDB/Furion.Extras.DatabaseAccessor.MongoDB.csproj", "Furion.Extras.DatabaseAccessor.MongoDB/"]
COPY ["trunk/Furion.Extras.DatabaseAccessor.FreeSql/Furion.Extras.DatabaseAccessor.FreeSql.csproj", "Furion.Extras.DatabaseAccessor.FreeSql/"]
COPY ["trunk/Furion.Extras.DependencyModel.CodeAnalysis/Furion.Extras.DependencyModel.CodeAnalysis.csproj", "Furion.Extras.DependencyModel.CodeAnalysis/"]
COPY ["trunk/Furion/Furion.csproj", "Furion/"]
COPY ["trunk/Furion.Extras.Authentication.JwtBearer/Furion.Extras.Authentication.JwtBearer.csproj", "Furion.Extras.Authentication.JwtBearer/"]
COPY ["trunk/Furion.Extras.ObjectMapper.Mapster/Furion.Extras.ObjectMapper.Mapster.csproj", "Furion.Extras.ObjectMapper.Mapster/"]
RUN dotnet restore "HuLuProject.Web.Entry/HuLuProject.Web.Api.csproj"

# FROM restore AS build
COPY . .
WORKDIR "/src/trunk/HuLuProject.Web.Entry"
RUN dotnet build "HuLuProject.Web.Api.csproj" -c Release -o /app/build

FROM restore AS publish
RUN dotnet publish "HuLuProject.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HuLuProject.Web.Api.dll"]