# Identity Service
Microserviço de autenticação baseado no [ASP.Net Identity Server](http://identityserver.io/).

## Geração da Imagem
O serviço está todo configurado para execução com Docker. O branch ***master*** foca em Windows, mas o branch ***linux*** permite a execução em Linux e macOS.

##### Dockerfile
Para compilar o Dockerfile, basta executar o comando na pasta do Dockerfile
```sh
docker build . -t identityservice:latest
```

##### Docker Hub
Em breve

## Utilização
O Identity Service é pensado para a proteção de uma API, onde varios clientes podem consumí-la de maneira segura. Atualmente o serviço está configurado para Android, iOS, Web e Swagger, mas isso é totalmente configurável (e futuramente é esperado que os clientes possam ser configurados em tempo de execução). As configurações são passadas para dentro do container através de variáveis de ambiente (parâmetro **-e** quando se executa o comando **docker run**). Como são vários parâmetros, é criado um arquivo **identity-var.env** que permite a declaração de todos eles, e ainda a execução no **docker-compose**:

```sh
identityservice:
    image: identityservice:latest
    env_file:
    - identity-var.env
```

Existe também o arquivo **secrets.example.json** que mostra como utilizar o Identity Service como aplicação ASP.Net Core sem a necessidade de um ambiente Docker. Quase todos os parâmetros são autoexplicativos, menos o parâmetro **APIEnableScopePerClaim**. Ele é responsável por informar à API se os escopos (scopes) serão transformados em Claims ou não. 

Vale notar também que o Identity Service utiliza as bibliotecas do [SendGrid](https://sendgrid.com/) para envio de e-mail, tornando necessária a existência de uma conta nesse serviço. Caso seja necessário, é possível substituir a dependência de SendGrid