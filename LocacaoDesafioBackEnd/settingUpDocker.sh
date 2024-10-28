dotnet restore --force-evaluate
dotnet build -c Release
sudo docker-compose down --volumes --remove-orphans
sudo docker-compose up --build --force-recreate