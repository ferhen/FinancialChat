# FinancialChat
This repo contains the code for a simple browser-based chat application using .NET Core

![Demo](/images/demo.gif)

![Chatrooms](/images/chatrooms.png)

![FinancialRoom](/images/financialroom.png)

# Run
This is a simple applications using .NET Core, Angualar, SQL Server and RabbitMQ. To run the project, we're using Docker with Docker Compose. For startup use the following command:
```
docker-compose up --build -d
```
The application will be exposed on `port 8000`. The startup process take some time to boot up RabbitMQ and apply the database migrations.

# Features
- [x] Real time message exchange between users
- [x] Multiple chat rooms to choose from
- [x] Financial bot shows stock quotes with `/stock=stock_code` command