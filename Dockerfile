FROM node:18 as build-vue
WORKDIR /app/frontend
COPY ./Frontend/package*.json ./
RUN npm install

COPY ./Frontend/ .
RUN ls -la node_modules/.bin/
RUN npm run build

# Backend

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ./Backend/*.sln ./
COPY ./Backend/TimeFlow.API/*.csproj ./TimeFlow.API/
COPY ./Backend/TimeFlow.DAL/*.csproj ./TimeFlow.DAL/
COPY ./Backend/TimeFlow.DL/*.csproj ./TimeFlow.DL/
COPY ./Backend/TimeFlow.Tests/*.csproj ./TimeFlow.Tests/
RUN dotnet restore

COPY Backend/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-vue /app/frontend/dist ./wwwroot


EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "TimeFlow.API.dll", "http://+:80"]