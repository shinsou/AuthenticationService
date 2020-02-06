#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
# install NodeJS 13.x
# see https://github.com/nodesource/distributions/blob/master/README.md#deb
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 2222

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

# Prevent 'Warning: apt-key output should not be parsed (stdout is not a terminal)'
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=1
# install NodeJS 13.x
# see https://github.com/nodesource/distributions/blob/master/README.md#deb
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs

COPY ["AuthenticationService.WebApi/AuthenticationService.WebApi.csproj", "AuthenticationService.WebApi/"]
COPY ["AuthenticationService.Application/AuthenticationService.Application.csproj", "AuthenticationService.Application/"]
COPY ["AuthenticationService.Infrastructure/AuthenticationService.Infrastructure.csproj", "AuthenticationService.Infrastructure/"]
COPY ["AuthenticationService.Persistence/AuthenticationService.Persistence.csproj", "AuthenticationService.Persistence/"]
RUN dotnet restore "AuthenticationService.WebApi/AuthenticationService.WebApi.csproj"
COPY . .
WORKDIR "/src/AuthenticationService.WebApi"
RUN dotnet build "AuthenticationService.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AuthenticationService.WebApi.csproj" -c Release -o /app/publish

#FROM base AS final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationService.WebApi.dll"]