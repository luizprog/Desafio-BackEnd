FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
RUN dotnet dev-certs https --trust
COPY . ./
RUN dotnet build -c Release -o /app
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish /app
EXPOSE 80

# Add the migration command here
ENTRYPOINT ["dotnet", "LocacaoDesafioBackEnd.dll"]

