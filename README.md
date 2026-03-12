# 🎬 VideoFrame Processor Consumer

Este repositório contém o serviço consumidor Kafka responsável pelo processamento de edições de vídeos, desenvolvido para o hackathon FIAP SOAT. O serviço consome mensagens de um tópico Kafka, processa a edição solicitada (atualmente extração de frames) e notifica o resultado via um tópico de notificação.

## 🏃 Integrantes do grupo

- **Jeferson dos Santos Gomes** - RM 362669
- **Jamison dos Santos Gomes** - RM 362671
- **Alison da Silva Cruz** - RM 362628

## 👨‍💻 Tecnologias Utilizadas

- **.NET 10 (C# 14)** – Worker Service com `BackgroundService`
- **Apache Kafka (Confluent.Kafka)** – mensageria para consumo e produção de eventos
- **AWS S3 (AWSSDK.S3)** – armazenamento de vídeos e resultados processados
- **FFmpeg** – extração de frames de vídeos
- **Serilog** – logging estruturado
- **Docker & Docker Compose** – containerização e orquestração local

## 🏗️ Arquitetura

O projeto segue **Clean Architecture** com as seguintes camadas:

| Camada | Projeto | Responsabilidade |
|---|---|---|
| **Domain** | `Domain` | Entidades, enums, use cases e interfaces de gateways |
| **Adapter** | `Adapter` | Controllers, presenters, DTOs e implementações de gateways |
| **Infrastructure** | `Infrastructure` | Clientes externos (S3, Kafka, FFmpeg, API), providers |
| **Consumer** | `Consumer` | Worker Service – ponto de entrada e consumo Kafka |

### Strategy Pattern por `EditType`

O serviço utiliza o padrão **Strategy** para resolver o caso de uso correto com base no `EditType` da mensagem recebida. Isso permite adicionar novos tipos de edição no futuro sem alterar a lógica existente.

| EditType | Implementação | Descrição |
|---|---|---|
| `Frame` | `FrameEditUseCase` | Extrai frames do vídeo e gera um arquivo ZIP |
| *(futuro)* | *(nova implementação de `IEditUseCase`)* | Novos tipos de edição podem ser adicionados |

## 📨 Mensagem Kafka (Entrada)

O serviço consome mensagens do tópico configurado na variável `KAFKA_CONSUMER_TOPIC`. A mensagem deve seguir o seguinte formato JSON:

```json
{
  "editId": "string",
  "userId": "string",
  "userName": "string",
  "userRecipient": "string",
  "videoPath": "string",
  "editType": "Frame",
  "notificationTargets": [
    {
      "channel": "Email",
      "target": "user@example.com"
    }
  ]
}
```

### Descrição dos campos

| Campo | Tipo | Descrição |
|---|---|---|
| `editId` | `string` | Identificador único da edição |
| `userId` | `string` | Identificador do usuário solicitante |
| `userName` | `string` | Nome do usuário solicitante |
| `userRecipient` | `string` | Destinatário da notificação |
| `videoPath` | `string` | Caminho do vídeo no storage (S3) |
| `editType` | `string` | Tipo de edição a ser realizada (`Frame`) |
| `notificationTargets` | `array` | Lista de destinos para notificação do resultado |
| `notificationTargets[].channel` | `string` | Canal de notificação (`Webhook`, `Email`) |
| `notificationTargets[].target` | `string` | Endereço/URL de destino da notificação |

### Fluxo de processamento

1. **Consumo** – O Worker consome a mensagem do tópico Kafka
2. **Resolução** – O `EditUseCaseResolver` identifica o use case pelo `EditType`
3. **Download** – Obtém a URL de download do vídeo no S3
4. **Processamento** – Executa a edição (ex.: extração de frames via FFmpeg)
5. **Upload** – Faz upload do resultado (ZIP) para o S3
6. **Notificação** – Publica mensagem de sucesso ou erro no tópico de notificação (`NOTIFICATION_TOPIC`)

## 🌐 Variáveis de Ambiente

O serviço utiliza as seguintes variáveis para configuração:

| Variável | Descrição |
|---|---|
| `DOTNET_ENVIRONMENT` | Define o ambiente em que a aplicação está rodando (`Development`, `Staging`, `Production`) |
| `KAFKA_HOST` | Endereço do broker Kafka |
| `KAFKA_CONSUMER_GROUP` | Nome do consumer group Kafka |
| `KAFKA_CONSUMER_TOPIC` | Nome do tópico Kafka para consumo de mensagens de edição |
| `NOTIFICATION_TOPIC` | Nome do tópico Kafka para produção de notificações |
| `S3BucketBaseUrl` | URL base do serviço S3 (ou compatível, ex.: MinIO) |
| `S3BucketUser` | Usuário/Access Key para autenticação no S3 |
| `S3BucketPassword` | Senha/Secret Key para autenticação no S3 |
| `S3BucketName` | Nome do bucket S3 utilizado para armazenamento |
| `VideoFrameApiUrl` | URL da API VideoFrame para atualização de status das edições |

## 👤 Convenções

- A mensagem Kafka utiliza serialização JSON com `camelCase`.
- O serviço é stateless – não possui banco de dados próprio.
- Configurações de ambiente estão em `appsettings.json` / `appsettings.Development.json`.
- O Worker realiza commit manual no Kafka após processamento bem-sucedido.
- Em caso de erro no processamento, a mensagem é logada e o consumo continua (sem commit).

## 🐳 Docker

### Build da imagem

```bash
docker build -t videoframe-processor-consumer .
```

### Executar com Docker Compose

```bash
docker-compose up -d
```

> **Nota:** O Dockerfile instala o `ffmpeg` no container, necessário para a extração de frames.