# Projeto de API usando C# e .NET 7

## DevEncurtaUrl - Jornada .NET Direto ao Ponto

Foi desenvolvida uma API REST completa de encurtamento de links.

## Tecnologias e práticas utilizadas
- ASP.NET Core com .NET 7
- Entity Framework Core
- SQLite / In-Memory database
- Swagger
- Injeção de Dependência
- Programação Orientada a Objetos
- Logs com Serilog
- Clean Code
- Publicação

## Funcionalidades
- Cadastro, Listagem, Detalhes, Atualização e Exclusão de Links
- Redirecionamento de Links Encurtados

###

![alt text](https://raw.githubusercontent.com/samuel-oldra/DevEncurtaUrl.API/main/README_IMGS/swagger_ui.png)

## Comandos básicos
```
dotnet new gitignore
dotnet new webapi -o DevGames.API
dotnet build
dotnet run
dotnet watch run
dotnet test
dotnet publish
```

## Tool Entity Framework Core (migrations)
```
dotnet tool install --global dotnet-ef
```

## Migrations
```
dotnet ef migrations add InitialMigration -o Persistence/Migrations
dotnet ef database update
```