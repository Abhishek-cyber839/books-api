# use aspnet to run the container
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
EXPOSE 443
EXPOSE 8000 8081

# use sdk to build the image
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app /p:UseAppHost=false 

FROM base AS final
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/blogs-api.dll" ]
CMD [ "dotnet ef migrations add InitialMigration;dotnet ef database update" ]