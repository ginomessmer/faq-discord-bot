[![.NET Build](https://github.com/ginomessmer/faq-discord-bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ginomessmer/faq-discord-bot/actions/workflows/dotnet.yml)
[![ko-fi](https://img.shields.io/badge/%E2%98%95-buy%20me%20a%20coffee-orange)](https://ko-fi.com/P5P72WHKK)

# FAQ Bot for Discord
A simple Discord bot that serves as a FAQ partner based on Microsoft QnA Maker.

## Run
### With Docker
```sh
docker run ginomessmer/faq-discord-bot -e QnaMaker:SubscriptionKey= ...
```

Pass all required (*) configuration entries as environment variables. 

### With Docker Compose
TODO

## Configuration
- `QnaMaker:SubscriptionKey`*: The subscription key for the managed QnA Maker service.
- `QnaMaker:KnowledgeBaseId`*: The knowledge base that serves as the QnA source.
- `ConnectionStrings:DiscordBotToken`*: Self explanatory.
- `ConnectionStrings:QnaServiceEndpoint`*: The service's endpoint, e.g. `https://*.cognitiveservices.azure.com`.

## Contribute
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/P5P72WHKK)

You are free to create discussions, issues and pull-requests. 