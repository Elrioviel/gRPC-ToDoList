FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ToDoListApp.csproj ./
RUN dotnet restore ToDoListApp.csproj

COPY . .
RUN dotnet build ToDoListApp.csproj -c Release -o /app/build
RUN dotnet publish ToDoListApp.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ToDoListApp.dll"]