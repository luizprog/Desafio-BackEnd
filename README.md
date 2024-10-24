## Ambiente de desenvolvimento Ubuntu / Debian
# Passo 1
 Garantir que dotnet e docker estejam instalados na maquina de desenvolvimento

# Passo 2
 executar o seguinte bash na raiz do projeto
bash:# sh settingUpDocker.sh 

Após este comando a api, o postgres e o rabbitmq devem ficar disponiveis para uso

BackEnd: http://localhost:5000/swagger/index.html
Postgres: localhost:5432
RabbitMQ: http://localhost:15672
