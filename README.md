## Ambiente de desenvolvimento Ubuntu / Debian
# Passo 1
# Garantir que dotnet, docker e docker-compose estejam instalados na maquina de desenvolvimento

# Passo 2
# Ir ate a pasta raiz do projeto
bash:# cd {local de download do projeto}/Desafio-BackEnd/LocacaoDesafioBackEnd

# Passo 3
# executar o seguinte bash na raiz do projeto
bash:# sh settingUpDocker.sh 

# Após este comando a api, o postgres e o rabbitmq devem ficar disponiveis para uso


## Ip e porta para acessar cada um dos servicos
BackEnd: http://localhost:5000/swagger/index.html
Postgres: localhost:5432
RabbitMQ: http://localhost:15672


# Autenticar e autorizar uso da API
![Screenshot from 2024-10-29 15 21 59](https://github.com/user-attachments/assets/b06bcdcb-2ae1-4cd9-85b1-d1807b8872f3)
![2024-10-29_16-14](https://github.com/user-attachments/assets/65792a00-b066-4547-956b-4d4c9f9b52af)
![2024-10-29_16-15](https://github.com/user-attachments/assets/8b4725bb-bd52-481f-8ea0-699f9fa7db8d)
![2024-10-29_16-16](https://github.com/user-attachments/assets/5dd0482e-9166-417d-ae10-29804a8ad714)
![2024-10-29_16-16_1](https://github.com/user-attachments/assets/ef1dc124-89d6-46f2-bc9e-2b25d84f2bca)




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
