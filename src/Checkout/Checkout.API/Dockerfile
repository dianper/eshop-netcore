#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Checkout/Checkout.API/Checkout.API.csproj", "src/Checkout/Checkout.API/"]
COPY ["src/Checkout/Checkout.Application/Checkout.Application.csproj", "src/Checkout/Checkout.Application/"]
COPY ["src/Checkout/Checkout.Core/Checkout.Core.csproj", "src/Checkout/Checkout.Core/"]
COPY ["src/Checkout/Checkout.Infrastructure/Checkout.Infrastructure.csproj", "src/Checkout/Checkout.Infrastructure/"]
RUN dotnet restore "src/Checkout/Checkout.API/Checkout.API.csproj"
COPY . .
WORKDIR "/src/src/Checkout/Checkout.API"
RUN dotnet build "Checkout.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.API.dll"]
