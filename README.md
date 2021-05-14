[![.NET Build](https://github.com/ginomessmer/faq-discord-bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ginomessmer/faq-discord-bot/actions/workflows/dotnet.yml)
[![ko-fi](https://img.shields.io/badge/%E2%98%95-buy%20me%20a%20coffee-orange)](https://ko-fi.com/P5P72WHKK)

# FAQ Discord Bot
A simple Discord bot that serves as a FAQ conversational partner. Powered by Microsoft Azure QnA Maker.

## Configuration
### Required
- `ConnectionStrings:DefaultDbContext`: Connection string for the PostgreSQL database (e.g. `Host=postgres_db;Database=faqs;Username=postgres;Password=postgres`).
- `ConnectionStrings:DiscordBotToken`: Self explanatory.
- `ConnectionStrings:QnaServiceEndpoint`: QnA Service Endpoint (e.g. `https://<...>.cognitiveservices.azure.com`).
- `QnaMaker:SubscriptionKey`: QnA subscription key.
- `QnaMaker:KnowledgeBaseId`: QnA knowledge base ID.

### Optional
- `ApplicationInsights:InstrumentationKey`: Instrumentation key for Application Insights.
- `Bot:StatusMessage`: The bot's status message.
- `Bot:CultureName`: Locale (default: system language).


## QnA Maker Setup
[QnA Maker](https://www.qnamaker.ai/) is a conversational bot service that offers automatic extraction from semi-structured content.
## Run
> TODO
### Docker
Pass the required configuration as environment variables: `docker run ginomessmer/faq-discord-bot -e Provider= ...`

### Docker Compose
> TODO
```yml
version: '3.8'

services:
  bot:
    image: ghcr.io/ginomessmer/faq-discord-bot:latest
    networks:
      - postgres_default
    environment:
      ConnectionStrings:DefaultDbContext: Host=postgres_db;Database=faqs;Username=postgres;Password=postgres
      ConnectionStrings:DiscordBotToken: 
      ConnectionStrings:QnaServiceEndpoint: https://<...>.cognitiveservices.azure.com
      QnaMaker:SubscriptionKey: 
      QnaMaker:KnowledgeBaseId: 
      ApplicationInsights:InstrumentationKey:
      Bot:CultureName: en-us
```

## Contribute
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/P5P72WHKK)

You are free to create discussions, issues and pull-requests. 
