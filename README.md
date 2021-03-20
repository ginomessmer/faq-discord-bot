[![.NET Build](https://github.com/ginomessmer/faq-discord-bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ginomessmer/faq-discord-bot/actions/workflows/dotnet.yml)
[![ko-fi](https://img.shields.io/badge/%E2%98%95-buy%20me%20a%20coffee-orange)](https://ko-fi.com/P5P72WHKK)

# FAQ Bot for Discord
A simple Discord bot that serves as a FAQ conversational partner.

## Configuration
### Required
- `Provider`: The FAQ provider implementation (see below). Can be `Lucene`, `QnaMaker`.
- `ConnectionStrings:DiscordBotToken`: Self explanatory.

### Optional
- `Bot:StatusMessage`: The bot's status message.

## Providers
### Local powered by Lucene (`Lucene`)
The bot indexes text documents in `data/sources` during start up. 

### QnA Maker (`QnaMaker`)
[QnA Maker](https://www.qnamaker.ai/) is a conversational bot service that offers automatic extraction from semi-structured content.
#### Required Configuration
Make sure to supply these configuration entries.
- `QnaMaker:SubscriptionKey`: The subscription key for the managed QnA Maker service.
- `QnaMaker:KnowledgeBaseId`: The knowledge base that serves as the QnA source.
- `ConnectionStrings:QnaServiceEndpoint`: The service's endpoint, e.g. `https://*.cognitiveservices.azure.com`.

## Run
### With Docker
Pass the required configuration as environment variables: `docker run ginomessmer/faq-discord-bot -e Provider= ...`

## Contribute
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/P5P72WHKK)

You are free to create discussions, issues and pull-requests. 