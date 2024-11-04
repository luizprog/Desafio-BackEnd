# Projeto LocacaoDesafioBackEnd

Este projeto é uma API para gerenciamento de locações de veículos.

---

## Ambiente de Desenvolvimento

Instruções para configurar o ambiente de desenvolvimento em diferentes sistemas operacionais.

### Pré-requisitos

Certifique-se de que as seguintes ferramentas estão instaladas:

- .NET SDK
- Docker
- Docker Compose

---

## Instalação e Configuração

### Ubuntu / Debian

#### Passo 1: Verificar a Instalação das Dependências

Certifique-se de que `dotnet`, `docker` e `docker-compose` estão instalados na máquina de desenvolvimento.

#### Passo 2: Acessar a Pasta Raiz do Projeto

Navegue até o diretório raiz do projeto:

```bash
cd {local de download do projeto}/Desafio-BackEnd/LocacaoDesafioBackEnd
```

Passo 3: Executar o Script de Configuração Docker

Na raiz do projeto, execute o seguinte comando para iniciar os serviços:

```bash
sh settingUpDocker.sh
```

Após este comando, a API, o PostgreSQL e o RabbitMQ devem ficar disponíveis para uso.


### Windows

#### Passo 1: Verificar a Instalação das Dependências

Certifique-se de que o .NET SDK, Docker e Docker Compose estão instalados.

#### Passo 2: Acessar a Pasta Raiz do Projeto

Navegue até o diretório raiz do projeto. Use o Prompt de Comando ou PowerShell:

powershell

cd {local de download do projeto}\Desafio-BackEnd\LocacaoDesafioBackEnd

#### Passo 3: Executar o Script de Configuração Docker

Na raiz do projeto, execute o seguinte comando para iniciar os serviços:

```cmd
settingUpDocker.bat
```

Após este comando, a API, o PostgreSQL e o RabbitMQ devem ficar disponíveis para uso.
Acessar os Serviços

    BackEnd: http://localhost:5000/swagger/index.html
    PostgreSQL: localhost:5432
    RabbitMQ: http://localhost:15672

### Autenticação e Autorização da API

Para autenticar e autorizar o uso da API, siga as capturas de tela abaixo:

Exemplo de Autenticação 1 Exemplo de Autenticação 2 Exemplo de Autenticação 3 Exemplo de Autenticação 4 Exemplo de Autenticação 5
Monitoramento do RabbitMQ

Para monitorar o RabbitMQ e configurar usuários, siga as instruções abaixo:

    Listar containers ativos:

    ```bash
  docker ps
  ```


Acessar o container RabbitMQ:

bash

docker exec -it nome_do_container bash

Habilitar o plugin de gerenciamento do RabbitMQ:

```bash
rabbitmq-plugins enable rabbitmq_management
```

Acessar o RabbitMQ Management:

    http://localhost:15672

(Opcional) Adicionar um usuário:

```bash
rabbitmqctl add_user nome_usuario senha
rabbitmqctl set_user_tags nome_usuario administrator
```

Sair do container e reiniciá-lo:

```bash

    exit
    docker restart nome_do_container```

