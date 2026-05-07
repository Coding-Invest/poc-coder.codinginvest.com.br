# Coder.codinginvest.com

Este projeto é uma prova de conceito (POC) que foi descontinuado devido a priorizações internas. Ele está sendo tornado público como uma contribuição para a comunidade do GitHub, exatamente como está. Desenvolvedores são incentivados a contribuir, melhorar e adaptar o código conforme necessário.

Uma aplicação de chatbot baseada em arquitetura de microsserviços, utilizando o protocolo MCP (Model Context Protocol) para integração com IA generativa.

## Visão Geral

Esta solução divide-se em quatro camadas principais: Web (frontend Angular), Client (API gateway), Server (servidor MCP) e Domain (biblioteca compartilhada). Projetada para fornecer uma interface de chat em tempo real com automação de tarefas via ferramentas MCP.

## Funcionalidades

- **Interface de Chat em Tempo Real**: Frontend Angular com chat minimizável e histórico dinâmico.
- **Integração com IA Generativa**: Uso do protocolo MCP para comunicação com modelos de IA.
- **Servidor MCP**: Implementa ferramentas para automação, como leitura/escrita de arquivos e operações Git via JSON-RPC.
- **Arquitetura Modular**: Separação clara de responsabilidades com projetos modulares em .NET 8.0 e Angular 20.

## Tecnologias Utilizadas

- **Frontend**: Angular 20, RxJS, TypeScript
- **Backend**: ASP.NET Core 8.0, C#
- **Comunicação**: JSON-RPC 2.0, Fetch API, Server-Sent Events
- **Outros**: NewtonSoft.Json, Docker, Git

## Known Issues

- Sistema de refresh tokens não finalizado
- Ferramentas de build de Angular não finalizadas
- Controle de gasto de tokens não finalizado

## Instalação

1. Clone o repositório:
   ```
   git clone https://github.com/Coding-Invest/poc-coder.codinginvest.com.git
   cd poc-coder.codinginvest.com
   ```

2. Instale dependências do .NET:
   ```
   dotnet restore
   ```

3. Instale dependências do Angular:
   ```
   cd Web
   npm install
   cd ..
   ```

4. Configure as credenciais no `appsettings.json` dos projetos Client e Server.

   **Para Client/appsettings.json**:
   - `LLMBearerToken`: Obtenha sua chave da API x.ai em https://x.ai/api e substitua "YOUR_XAI_API_KEY_HERE".
   - `Authentication`: Configure seu email, senha e jwt secret para autenticação (ex.: "your-email@example.com", "your-password", "your-jwt-secret-key").

   **Para Server/appsettings.json**:
   - `Git`: Configure o repositório Git e Personal Access Token do GitHub (ex.: "https://github.com/your-username/your-repo.git", "YOUR_GITHUB_PAT_HERE").
   - `Smtp`: Configure o email e senha do app para envio de emails (ex.: "your-email@gmail.com", "your-app-password").
   - `Google`: Configure a API key e Cx para Google Search (ex.: "YOUR_GOOGLE_API_KEY_HERE", "YOUR_GOOGLE_CX_HERE").

5. Build a solução:
   ```
   dotnet build
   ```

## Infraestrutura com Terraform

A pasta Infrastructure contém arquivos Terraform para provisionar a infraestrutura na Azure usando Container Apps para hospedar o Client e Server.

- `main.tf`: Define recursos como Resource Group, Container App Environment, Storage, Container Apps para client e server.
- `variables.tf`: Variáveis para localização, imagens, ACR, etc.
- `providers.tf`: Configuração do provider Azure.
- `backend.tf`: Backend para armazenar estado do Terraform no Azure Storage.

Para usar:
1. Copie `variables.tfvars.example` para `variables.tfvars` e preencha com suas credenciais Azure (subscription_id, tenant_id, client_id, client_secret).
2. Execute:
   ```
   terraform init
   terraform plan -var-file=variables.tfvars
   terraform apply -var-file=variables.tfvars
   ```

## Uso

1. Execute o projeto Client (hospeda o frontend):
   ```
   dotnet run --project Client
   ```

2. Acesse o aplicativo em `http://localhost:5000` (ou porta configurada).

3. Interaja com o chatbot através da interface web.

## Arquitetura

### Web (Frontend Angular)
- Aplicação standalone responsável pela UI do chatbot.
- Usa signals e RxJS para reatividade.
- Componentes como `ChatbotComponent` para lógica de chat.

### Client (API Gateway)
- ASP.NET Core API que serve como gateway entre frontend e backend.
- Hospeda a aplicação Web e expõe endpoints para IA via x.ai API e MCP.
- Padrões: API Gateway, Mediator, Dependency Injection.

### Server (Servidor MCP)
- Implementa o servidor MCP com ferramentas para automação.
- Serviços como `FileReadService`, `GitAddService`, etc.
- Protocolo JSON-RPC 2.0.

### Domain (Biblioteca Compartilhada)
- Define entidades compartilhadas, como protocolos MCP e instruções para IA.
- Classes como `Tools`, `ProtocolRequest`.

Para mais detalhes, consulte a documentação interna em `Web/src/app/documentation.component.html`.

## Contribuição

Desenvolvedores são bem-vindos para contribuir com melhorias, correções de bugs e novas funcionalidades.

1. Fork o projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -am 'Adiciona nova feature'`).
4. Push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

## Licença
MIT

## Considerações Gerais

- **Segurança**: Use certificados autoassinados para desenvolvimento; configure CORS adequadamente.
- **Melhorias Futuras**: Adicionar testes unitários, logging avançado e validação de entrada.
- **Compatibilidade**: Desenvolvido com .NET 8.0 e Angular 20; verifique versões de dependências.