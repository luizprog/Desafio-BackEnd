## Ambiente de desenvolvimento Ubuntu / Debian
# Passo 1
# Garantir que dotnet, docker e docker-compose estejam instalados na maquina de desenvolvimento

# Passo 2
# Ir ate a pasta raiz do projeto
bash:# cd {local de download do projeto}/Desafio-BackEnd/LocacaoDesafioBackEnd

# Passo 3
# executar o seguinte bash na raiz do projeto
bash:# sh settingUpDocker.sh 

Após este comando a api, o postgres e o rabbitmq devem ficar disponiveis para uso

BackEnd: http://localhost:5000/swagger/index.html
Postgres: localhost:5432
RabbitMQ: http://localhost:15672


# Monitorar rabbitmq
docker ps
docker exec -it nome_do_container bash
rabbitmq-plugins enable rabbitmq_management
http://localhost:15672
# Se necessário
rabbitmqctl add_user nome_usuario senha
rabbitmqctl set_user_tags nome_usuario administrator
exit
docker restart nome_do_container
