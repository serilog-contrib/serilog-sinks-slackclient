# Serilog.Sinks.Slack

----------

[![Build status](https://ci.appveyor.com/api/projects/status/x87gr2jgnjvow6oo/branch/master?svg=true)](https://ci.appveyor.com/project/marcio-azevedo/serilog-sinks-slack/branch/master)
[![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.SlackClient.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.SlackClient/)
[![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.Slack.Core.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.Slack.Core/)

A Serilog sink that writes events as messages to [Slack](https://slack.com). Slack client was developed in [F#](http://fsharp.org/).

**Package** - [Serilog.Sinks.SlackClient](https://www.nuget.org/packages/Serilog.Sinks.SlackClient/)
| **Platforms** - .NET 4.5.1

### Contributors needed! ###

**.NET Core now supported** - Just download this package [![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.Slack.Core.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.Slack.Core/) (only incoming webhooks are supported in this package)

You'll need to have a Channel Id and a Token on your Slack team to be able to send messages. To manage tokens go to [Slack Tokens](https://api.slack.com/tokens/). In the example shown, the Channel Id in use is `XXXXXXXXX` and the token is `yyyyyyyyyyyy`.

```csharp
var log = new LoggerConfiguration()
    .WriteTo.Slack("XXXXXXXXX", "yyyyyyyyyyyy", LevelAlias.Maximum)
    .CreateLogger();
log.Fatal("This is an fatal message!");
```

Example:

![Simple Message](/assets/message01.png)

Other option is to use [incoming webhooks](https://api.slack.com/incoming-webhooks):

```csharp
var log = new LoggerConfiguration()
	.WriteTo.Slack("https://hooks.slack.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX")
	.CreateLogger();
log.Fatal("This is an fatal message!");
```

Example:

![Simple Message](/assets/message02.png)

To change the default renderization pass a delegate as an additional parameter:

```csharp
var log = new LoggerConfiguration()
	.WriteTo.Slack("https://hooks.slack.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX",
                (LogEvent l) => l.RenderMessage())
	.CreateLogger();
```

For more information about the default render function take a look to:

```csharp
string RenderMessage(LogEvent logEvent)
```

at [SlackSink.cs](/src/Serilog.Sinks.Slack/Sinks/Slack/SlackSink.cs)
