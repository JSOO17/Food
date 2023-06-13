#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://*:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Foods.Infrastructure.API/Foods.Infrastructure.API.csproj", "Foods.Infrastructure.API/"]
COPY ["Foods.Application/Foods.Application.csproj", "Foods.Application/"]
COPY ["Foods.Domain/Foods.Domain.csproj", "Foods.Domain/"]
COPY ["Foods.Infrastructure.Data/Foods.Infrastructure.Data.csproj", "Foods.Infrastructure.Data/"]
RUN dotnet restore "Foods.Infrastructure.API/Foods.Infrastructure.API.csproj"
COPY . .
WORKDIR "/src/Foods.Infrastructure.API"
RUN dotnet build "Foods.Infrastructure.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Foods.Infrastructure.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Foods.Infrastructure.API.dll"]