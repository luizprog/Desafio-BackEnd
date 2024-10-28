@echo off
dotnet restore --force-evaluate
dotnet build -c Release
docker-compose down --volumes --remove-orphans
docker-compose up --build --force-recreate
pause
