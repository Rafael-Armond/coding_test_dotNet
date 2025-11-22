# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia solution (se existir) - ajuda o restore em multi-projeto
# Se sua solution tiver outro nome, ajuste aqui.
COPY ["*.sln", "./"]

# Copia os csproj de TODOS os projetos para o restore funcionar
COPY ["CreditosConstituidos.Api/CreditosConstituidos.Api.csproj", "CreditosConstituidos.Api/"]
COPY ["CreditosConstituidos.Application/CreditosConstituidos.Application.csproj", "CreditosConstituidos.Application/"]
COPY ["CreditosConstituidos.Domain/CreditosConstituidos.Domain.csproj", "CreditosConstituidos.Domain/"]
COPY ["CreditosConstituidos.Infrastructure/CreditosConstituidos.Infrastructure.csproj", "CreditosConstituidos.Infrastructure/"]
COPY ["CreditosConstituidos.Tests/CreditosConstituidos.Tests.csproj", "CreditosConstituidos.Tests/"]

# Restore (pode ser pelo sln ou direto pela Api)
# Se você não copiou o .sln acima, esse restore pela Api já basta.
RUN dotnet restore "CreditosConstituidos.Api/CreditosConstituidos.Api.csproj"

# Copia o restante da solution (código fonte)
COPY . .

# Build da API (as referências aos outros projetos agora existem)
WORKDIR "/src/CreditosConstituidos.Api"
RUN dotnet build "CreditosConstituidos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# =========================
# Publish
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/CreditosConstituidos.Api"
RUN dotnet publish "CreditosConstituidos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# =========================
# Final image
# =========================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CreditosConstituidos.Api.dll"]
