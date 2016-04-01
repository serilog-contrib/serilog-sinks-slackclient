# Serilog.Sinks.Slack

----------

[![Build status](https://ci.appveyor.com/api/projects/status/x87gr2jgnjvow6oo/branch/master?svg=true)](https://ci.appveyor.com/project/marcio-azevedo/serilog-sinks-slack/branch/master)

A Serilog sink that writes events as messages to [Slack](https://slack.com). Slack client was developed in [F#](http://fsharp.org/).

**Package** - [Serilog.Sinks.SlackClient](https://www.nuget.org/packages/Serilog.Sinks.SlackClient/)
| **Platforms** - .NET 4.5.1

You'll need to have a Channel Id and a Token on your Slack team to be able to send messages. To manage tokens go to [Slack Tokens](https://api.slack.com/tokens/). In the example shown, the Channel Id in use is `XXXXXXXXX` and the token is `yyyyyyyyyyyy`.

```csharp
var log = new LoggerConfiguration()
    .WriteTo.Slack("XXXXXXXXX", "yyyyyyyyyyyy", LevelAlias.Maximum)
    .CreateLogger();
log.Fatal("This is an fatal message!");
```

Example:

![Simple Message](/assets/message01.png)

