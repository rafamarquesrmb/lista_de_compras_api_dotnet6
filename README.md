[![author](https://img.shields.io/badge/author-rafamarquesrmb-red.svg)](https://github.com/rafamarquesrmb) [![DotNET6](https://img.shields.io/badge/.NET-6.0-blue.svg)](https://dotnet.microsoft.com/) [![ASP.NET6](https://img.shields.io/badge/ASP.NET-6.0-blue.svg)](https://docs.microsoft.com/pt-br/aspnet/core/?view=aspnetcore-6.0) [![C#10](https://img.shields.io/badge/csharp-10-blue.svg)](https://docs.microsoft.com/pt-br/dotnet/csharp/) [![License-Unlicense](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

# Lista de Compras com .NET6 Web Api

<sub>Desenvolvido por [Rafael Marques](https://github.com/rafamarquesrmb)</sub>

## Sobre

Este projeto Consiste em Uma WebAPI para registro de Listas de Compras multiusuários.
Cada usuário pode possuir N listas, e cada Lista pode conter N itens. A autenticação é baseada em Tokens JWT Bearer.

Desenvolvido no .NET 6, sendo um projeto "ASP.NET Web Api". Conecta-se à um banco de Dados SQL Server.

Cada usuário só pode ver, editar e criar suas próprias listas, assim como seus itens.

Para todas as etapas, o usuário é verificado através do próprio token JWT. Por exemplo, ao criar uma lista, o relacionamento entre a lista e o usuário (atributo UserId) é atribuído pela identificação direta do Token JWT Bearer passado no Header da requisição, dessa forma, não é preciso enviar o UserId na requisição em si, o que garante maior segurança a aplicação.

## Funcionalidades disponíveis na API

### Usuários, Registro e Login

Em relação às contas de usuários, podemos:

- Registrar novo usuário através de uma rota publica
- Realizar login para receber o Token JWT, através de uma rota publica
- Alterar Senha (apenas para usuário autenticado)
- Deletar Conta (apenas para usuário autenticado)
- Receber as informações do usuário e suas Listas de Compras (apenas para usuário autenticado)
- Receber as informações do usuário, suas listas de compras e os itens das listas (apenas para usuário autenticado)

Para ficar completo e 100% apropriado para um MVP, ainda faltaria uma funcionalidade:

- Serviço de recuperar senha, através do e-mail, usando um Token para recuperação com tempo de expiração.

### Listas de Compras

Em relação as Listas de Compras, cada usuário pode criar quantas listas desejar.
Podemos:

- Criar lista
- Editar o titulo e descrição da lista
- Deletar a lista
- Receber as informações de todas as listas do usuário
- Receber as informações de uma lista do usuário buscando pelo seu ID

### Itens

Para cada Lista de Compras, podemos adicionar quantos itens desejarmos.
Podemos:

- Criar item
- Editar item
- Deletar o item da lista
- Receber as informações de todos os itens de uma Lista (através do ID da lista)
- Receber as informações de um item de uma Lista (através do ID da lista e do ID do item)

## Endpoints

Cada Controller possui como padrão o endpoint **api/[Controller]**
São 3 Controllers principais:

- **Accounts** - (Gerenciar funcionalidades referente as contas do usuário, login, registros, etc)
- **GroceryLists** - (Gerenciar funcionalidades referente as Listas de Compras do Usuário)
- **Items** - (Gerenciar funcionalidades referente aos Itens das listas de Compras do Usuário)

**Dica:** Ao inicializar o projeto em ambiente de desenvolvimento, basta acessar a rota **/swagger** para ter acesso a UI do Swagger.

### Endpoind - Accounts

| Método | Rota                              | Autorização                                    | Descrição                                                                                                                |
| ------ | --------------------------------- | ---------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------ |
| POST   | **/api/Accounts/login**           | Rota Pública                                   | Realizar o login, enviando as credenciais no corpo da requisição. Retorna um token JWT                                   |
| POST   | **/api/Accounts/register**        | Rota Pública                                   | Realizar o cadastro de um novo usuário. Retorna mensagens + Token JWT (considera que realiza o login em caso de sucesso) |
| PUT    | **/api/Accounts/changepassword**  | Necessita Autenticação (Header com JWT Bearer) | Permite que o usuário realize alteração de senha.                                                                        |
| DELETE | **/api/Accounts/deleteaccount**   | Necessita Autenticação (Header com JWT Bearer) | Deleta a conta do usuário permanentemente.                                                                               |
| GET    | **/api/Accounts/getinfo**         | Necessita Autenticação (Header com JWT Bearer) | Retorna informações sobre o usuário, incluindo suas Listas de Compras.                                                   |
| GET    | **/api/Accounts/getcompleteinfo** | Necessita Autenticação (Header com JWT Bearer) | Retorna informações sobre o usuário, incluindo suas Listas de Compras e os itens contido nestas listas.                  |

---

### Endpoind - GroceryLists

| Método | Rota                       | Autorização                                    | Descrição                                                                                                                  |
| ------ | -------------------------- | ---------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------- |
| GET    | **/api/GroceryLists/**     | Necessita Autenticação (Header com JWT Bearer) | Recebe todas as listas de Compras existentes para o usuário. (não incluso seus itens)                                      |
| GET    | **/api/GroceryLists/{id}** | Necessita Autenticação (Header com JWT Bearer) | Recebe informação sobre uma lista de Compras especificada pelo seu ID, existentes para o usuário. (não incluso seus itens) |
| POST   | **/api/GroceryLists/**     | Necessita Autenticação (Header com JWT Bearer) | Cria uma nova Lista de Compras para o usuário.                                                                             |
| PUT    | **/api/GroceryLists/{id}** | Necessita Autenticação (Header com JWT Bearer) | Permite que o usuário realize alterações no título e/ou descrição de uma lista de Compras especificada pelo ID da Lista.   |
| DELETE | **/api/GroceryLists/{id}** | Necessita Autenticação (Header com JWT Bearer) | Deleta uma lista de Compras do Usuário, especificada pelo ID da Lista                                                      |

---

### Endpoind - Items

| Método | Rota                                                 | Autorização                                    | Descrição                                                                                                         |
| ------ | ---------------------------------------------------- | ---------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| GET    | **/api/Items/{Id_Da_Lista_de_Compras}/{id_do_Item}** | Necessita Autenticação (Header com JWT Bearer) | Recebe todos os Items pertencentes a uma Lista de Compras do usuário.                                             |
| GET    | **/api/Items/{Id_Da_Lista_de_Compras}/{id_do_Item}** | Necessita Autenticação (Header com JWT Bearer) | Recebe informação sobre um item,especificado pelo seu ID, de uma lista de Compras do Usuário                      |
| POST   | **/api/Items/{Id_Da_Lista_de_Compras}**              | Necessita Autenticação (Header com JWT Bearer) | Cria um novo item para uma lista de Compras do Usuário.                                                           |
| PUT    | **/api/Items/{Id_Da_Lista_de_Compras}/{id_do_Item}** | Necessita Autenticação (Header com JWT Bearer) | Permite que o usuário realize alterações em um Item, especificado pelo seu ID, de uma lista de Compras do Usuário |
| DELETE | **/api/Items/{Id_Da_Lista_de_Compras}/{id_do_Item}** | Necessita Autenticação (Header com JWT Bearer) | Deleta um item, especificado pelo seu ID, de uma Lista de Compras do usuário.                                     |

---

## Tecnologias e Packages

O projeto foi desenvolvido com **.NET 6** na versão 6.0.101, utilizando a linguagem de Programação C#. O Banco de dados utilizado foi o **SQL Server**.

Dentre os Pacotes utilizados para o projeto, os principais são:

- [Microsoft.AspNetCore.Authentication](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication/) - v.2.2.0
- [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/) - v.6.0.1
- [SecureIdentity](https://www.nuget.org/packages/SecureIdentity/) - v.1.0.2
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/) - v.6.0.1
- [Microsoft.EntityFrameworkCore.SqlServer](Microsoft.EntityFrameworkCore.SqlServer) - v.6.0.1
- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/) - v.6.0.1
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/) - v.6.2.3

## Configurações

Para facilitar o uso das configurações básicas, disponibilizei uma classe na raiz da API: **Configuration.cs.**

Nela há dois atributos, **JwtKey** e **ConnectionString**. Ambas são definidas no **Program.cs**, utilizando as informações do **appsettings.json** do projeto.

No program.cs você pode encontrar as linhas de código que definem os atributos da Classe Configuration através das variáveis do arquivo **appsettings.json**:

```
Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
Configuration.ConnectionString = app.Configuration.GetConnectionString("DefaultConnection");
```

No **appsettings.json** você deve realizar as alterações da JwtKey e da ConnectionString do banco de Dados, como no exemplo abaixo:

```
"JwtKey": "minhachavesecreta",
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=Grocery_list;User ID=sa;Password=sa"
}
```

Recomendo utilizar uma chave segura com no mínimo 32 caracteres como JwtKey. Na ConnectionString, defina a sua string de conexão com o seu banco de dados no SQL Server.

## Contribuições e Considerações Finais

Este projeto foi criado com o intuito apenas de testar algumas coisas novas e praticar um pouco o .NET 6, além de utiliza-lo como base para ser consumido em um projeto Mobile... Portanto, foi um projeto realizado em poucas horas, sem uma arquitetura bem definida, pensando em uma API Data Driven.

No entanto, acredito que pode ser útil para programadores iniciantes, ou que estejam pensando em desenvolver algo envolvendo listas de compras. Por isso, deixo este **repositório publico** e **aceito contribuições de qualquer usuário** que deseje acrescentar funcionalidades ou melhorias no código! Porém, **mantenha a ideia de ser um projeto simples, sem muitas complexidades, de fácil compreensão para iniciantes**.

<sub>Desenvolvido por [Rafael Marques](https://github.com/rafamarquesrmb)</sub>
