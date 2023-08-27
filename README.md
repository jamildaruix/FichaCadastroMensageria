# **M03S08**

Construção de uma aplicação para utilização de mensageria utilizando RabbitMQ

# **Objetivo**

Receber requisições pela API, inserir no RabbitMQ em uma fila e depois enviar para um worker processar os dados


# **Visual Studio**

Abra o visual studio e selecione a opção Criar um novo Projeto

![Alt text](/images/image3.png)

selecione o template API chamada ASP.NET CORE WEB API

![Alt text](/images/image4.png)

Seleciona o diretório, mude o nome da solucão e mude o nome do projeto

Ex.
1. Diretório  `C:\<SeuRepo>`
2. Nome da solução `FichaCadastroSln` 
3. Nome do projeto `FichaCadastroAPI` 
4. Nome do projeto `FichaCadastroWorker`
5. Nome do projeto `FichaCadastroRabbitMQ`

![Alt text](/images/image5.png)

Configuração do projeto

![Alt text](/images/image6.png)

Criar o projeto

![Alt text](/images/image7.png)

# **VS Code**

Criar uma `solução` e os `projetos` tipo api pelo VS CODE

![Alt text](/images/image1.png)

## Passo 1: Criando uma pasta chamada `FichaCadastroSln` e acessar o diretório

Abra o terminal (ou prompt de comando) no local que fica seus repositórios e execute os comandos.

```bash
mkdir FichaCadastroMensageriaSln
cd FichaCadastroMensageriaSln
```

## Passo 2: Criando a `Solução` e o `Projeto`

Dentro do terminal aberto execute os comandos

Comando                                                             | Detalhe
| :---                                                              | :---
dotnet new sln -n [NomeSln]                                         | Cria um arquivo .sln para referenciar os projetos 
dotnet new webapi -n [NomeApi]                                      | Cria uma pasta (folder) e adiciona os arquivos do projeto web api
dotnet new worker -n [NomeWorkService]                              | Cria uma pasta (folder) e adiciona os arquivos do projeto console do tipo work service
dotnet new classlib -n [NomeClassLib]                               | Cria uma pasta (folder) e adiciona os arquivos do projeto de class lib  
dotnet sln [NomeSln] add [FolderNomeApi]/[NomeApi]                  | Referencia (coloca o caminho da pasta do projeto) o projeto (.csproj) dentro da solução (.sln)
dotnet sln [NomeSln] add [FolderNomeWorkService]/[NomeWorkService]  | Referencia (coloca o caminho da pasta do projeto) o projeto (.csproj) dentro da solução (.sln)
dotnet sln [NomeSln] add [FolderNomeClassLib]/[NomeClassLib]        | Referencia (coloca o caminho da pasta do projeto) o projeto (.csproj) dentro da solução (.sln)
dotnet add [FolderNomeApi]/[NomeApi].csproj reference [FolderNomeClassLib]/[NomeClassLib].csproj | Referencia (coloca o caminho da pasta do projeto) o projeto (.csproj) dentro do projeto (.csproj)

```bash
dotnet new sln -n FichaCadastroSln
dotnet new webapi -n FichaCadastroAPI
dotnet new webapi -n FichaCadastroWorkService
dotnet new webapi -n FichaCadastroRabbitMQ

dotnet sln FichaCadastroSln.sln add FichaCadastroAPI/FichaCadastroAPI.csproj
dotnet sln FichaCadastroSln.sln add FichaCadastroWorkService/FichaCadastroWorkService.csproj
dotnet sln FichaCadastroSln.sln add FichaCadastroRabbitMQ/FichaCadastroRabbitMQ.csproj

dotnet add FichaCadastroAPI/FichaCadastroAPI.csproj reference FichaCadastroRabbitMQ/FichaCadastroRabbitMQ.csproj
dotnet add FichaCadastroWorkService/FichaCadastroWorkService.csproj reference FichaCadastroRabbitMQ/FichaCadastroRabbitMQ.csproj
```

# **Pacote nugets usados no projeto**

<details>
    <summary>RabbitMQ.Client</summary>
    <p>Link <a href="https://www.nuget.org/packages/RabbitMQ.Client">Link</a></p>
    <p>dotnet add package RabbitMQ.Client --version 6.5.0</p>
</details>

<br/>


# **Comandos git** 

## Branches

Comando                                     | Detalhe
| :---                                      | :---
git branch                                  | Lista as branches locais
git branch `feature\nome-da-branch`         | Cria uma nova branch
git checkout `feature\nome-da-branch`       | Muda para a branch especificada
git checkout -b `feature\nome-da-branch`    | Cria e muda para uma nova branch

## Commits

Comando                               | Detalhe
| :---                                | :---
git status                            | Mostra o estado atual das mudanças no diretório de trabalho
git add `exemplo.pdf`                 | Adiciona um arquivo específico para a área
git add . ou git add -A               | Adiciona todos os arquivos modificados para a área 
git commit -m "Mensagem do commit"    | Cria um novo commit com as mudanças na área
git commit -a -m "Mensagem do commit" | Adiciona automaticamente todas as alterações conhecidas ao índice e cria um commit

## Atualização e Sincronização

Comando     | Detalhe
| :---      | :---
git fetch   | Obtém informações atualizadas do repositório remoto sem incorporar as alterações no diretório de trabalho local
git pull    | Atualiza o repositório local com as alterações do repositório remoto
git push    | Envie os commits locais para o repositório remoto

## Merge e Rebase

Comando                                 | Detalhe
| :---                                  | :---
git merge  `origin/branch` ou `branch`  | Faz a fusão de uma branch na branch atual
git rebase `origin/branch` ou `branch`  | Reaplica commits em cima de outra branch

# **O projeto**

Após baixar o projeto, você pode abrir com o `Visual Studio` ou `VS Code`.

As tecnologias utilizadas:

* .Net com C#
* RabbitMQ

## Imagem RabbitMQ

Imagem criada no docker-compose.yaml do RabbitMQ

```YAML
    version: '3.4'

    services:
    rabbitmq:
        image: "rabbitmq:3-management"
        environment:
        RABBITMQ_DEFAULT_USER: "guest"
        RABBITMQ_DEFAULT_PASS: "guest"
        RABBITMQ_DEFAULT_VHOST: "/"
        restart: always
        ports:
        - "15672:15672"
        - "5672:5672"
        networks:
        - mensageria-network

    networks:
    mensageria-network:
        driver: 
        bridge
```

## Instruções para configurar o SQL Server usando Docker Compose

1. Certifique-se de ter o Docker instalado no seu sistema.

2. Navegue até o diretório onde o arquivo `docker-compose.yml` está localizado.

3. Abra o terminal na pasta e execute o seguinte comando para iniciar o SQL Server:

Comando                 | Detalhe
| :---                  | :---
docker-compose up       | Inicia o contanier e os serviços ficaram em execução no terminal
docker-compose up -d    | Inicia o contanier e os serviços ficaram em backgraoud (segundo plano) em execução
ocker-compose stop      | Para a execução do container do docker
docker-compose down     | Para e remove o container do docker
docker-compose ps       | Lista todos container's em execução
docker-compose ps -a    | Lista todos container's parados ou em execução

## Acessando

Abrir o Browser (Crhome) para acessar o RabbitMQ configurado da imagem e acessar a url http://localhost:15672/
- Usuário de acesso guest
- Senha de acesso   guest

![Alt text](/images/image9.png)

- Server name configurado no : `amqp://guest:guest@host.docker.internal:5672/` 

```CSHARP
        /// <summary>
        /// Criar uma ConnectionFactory para conectar no RabbitMQ
        /// </summary>
        /// <returns></returns>
        private static IConnectionFactory ConnectionFactory()
        {
            ConnectionFactory factory = new()
            {
                VirtualHost = "academia",
                Uri = new Uri("amqp://guest:guest@host.docker.internal:5672/")
            };

            return factory;
        }
    }
```