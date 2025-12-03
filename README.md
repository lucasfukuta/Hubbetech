# Hubbetech

O **Hubbetech** é um sistema de gestão corporativa desenvolvido utilizando a arquitetura **Blazor WebAssembly Hosted** com .NET 8. O projeto visa facilitar o gerenciamento de demandas, equipamentos, funcionários e links úteis da empresa, integrando um frontend interativo com uma API robusta.

## Tecnologias Utilizadas

O projeto está dividido em três camadas principais: **Client**, **Server** e **Shared**.

### Backend (Server)

  * **Framework:** .NET 8 (ASP.NET Core Web API)
  * **Banco de Dados:** SQLite (via Entity Framework Core)
  * **ORM:** Entity Framework Core
  * **Autenticação:** ASP.NET Core Identity com JWT Bearer
  * **Documentação da API:** Swagger / OpenAPI

### Frontend (Client)

  * **Framework:** Blazor WebAssembly
  * **Linguagem:** C\# / Razor
  * **Armazenamento Local:** Blazored.LocalStorage
  * **Estilização:** Bootstrap (inferido pela estrutura padrão do Blazor)

### Shared

  * Biblioteca de classes compartilhada contendo DTOs (Data Transfer Objects), Enums e Modelos de Autenticação utilizados tanto pelo Client quanto pelo Server.

-----

## Funcionalidades

Com base na estrutura do banco de dados e controladores, o sistema possui as seguintes funcionalidades principais:

1.  **Autenticação e Autorização:**

      * Login e Registro de usuários via API.
      * Uso de JSON Web Tokens (JWT) para segurança.
      * Atribuição automática da role "Funcionario" para novos registros.
      * Redirecionamento automático para login ao acessar a raiz do site.

2.  **Gestão de Demandas (Dashboard Ágil):**

      * Criação e acompanhamento de tarefas/demandas.
      * Propriedades incluem Título, Descrição, Status e Atribuição de Usuário.

3.  **Gestão de Equipamentos:**

      * Controle de inventário de equipamentos da empresa (Baseado no `DbSet<Equipamento>`).

4.  **Gestão de Funcionários:**

      * Administração de usuários e perfis (Baseado no `ApplicationUser`).

5.  **Links da Empresa:**

      * Centralização de links úteis corporativos (Baseado no `DbSet<LinkEmpresa>`).

-----

## Estrutura do Projeto

  * **`Hubbetech.Client`**: Aplicação Single Page Application (SPA) que roda no navegador do usuário.
      * `Pages/`: Contém as interfaces visuais (Home, Login, etc.).
      * `Services/`: Lógica de comunicação com a API (ex: `AuthService`).
      * `Providers/`: Provedores de estado, como o `ApiAuthenticationStateProvider`.
  * **`Hubbetech.Server`**: API RESTful que gerencia o banco de dados e a autenticação.
      * `Controllers/`: Endpoints da API (`AuthController`, `DemandasController`, etc.).
      * `Data/`: Contexto do banco de dados (`ApplicationDbContext`) e Seeder.
      * `Models/`: Entidades do banco de dados.
  * **`Hubbetech.Shared`**: Contratos de dados compartilhados.
      * `Dtos/`: Objetos de transferência de dados (`DemandaDto`, `LoginRequest`, etc.).
      * `Enums/`: Enumerações como `StatusDemanda`.

-----

## Como Executar o Projeto

### Pré-requisitos

  * [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.

### Passos para Instalação

1.  **Clonar o repositório:**

    ```bash
    git clone https://github.com/seu-usuario/hubbetech.git
    cd hubbetech
    ```

2.  **Configurar o Banco de Dados:**
    O projeto utiliza SQLite. As migrações já estão configuradas. Navegue até a pasta do servidor e aplique as migrações (se necessário) ou execute o projeto para que o `DataSeeder` inicialize os dados.

    ```bash
    cd Hubbetech.Server
    dotnet ef database update
    ```

3.  **Executar a Aplicação:**
    Como é uma solução "Hosted", você deve executar o projeto **Server**. Ele servirá tanto a API quanto os arquivos estáticos do Client.

    ```bash
    dotnet run --project Hubbetech.Server
    ```

4.  **Acessar:**

      * O frontend estará disponível geralmente em `https://localhost:7142` ou `http://localhost:5190` (conforme configuração de CORS).
      * O Swagger da API pode ser acessado em `/swagger`.

-----

## Configuração de Segurança

O projeto utiliza uma chave secreta para a geração de tokens JWT. No ambiente de desenvolvimento, uma chave padrão é utilizada (`SuperSecretKey12345678901234567890`). Para produção, certifique-se de alterar essa chave no `appsettings.json` ou através de variáveis de ambiente.

-----

## Contribuição

1.  Faça um Fork do projeto.
2.  Crie uma Branch para sua Feature (`git checkout -b feature/MinhaFeature`).
3.  Faça o Commit de suas mudanças (`git commit -m 'Adiciona MinhaFeature'`).
4.  Faça o Push para a Branch (`git push origin feature/MinhaFeature`).
5.  Abra um Pull Request.
