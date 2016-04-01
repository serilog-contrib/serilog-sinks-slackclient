using System.Collections.Generic;

namespace Serilog.Sinks.Slack
{
    /// <summary>
    /// Slack channel.
    /// </summary>
    public class SlackChannel
    {
        /// <summary>
        /// Slack Channel Id
        /// </summary>
        public readonly string ChannelId;

        /// <summary>
        /// Token that allows Slack authentication. To manage tokens go to https://api.slack.com/tokens
        /// </summary>
        public readonly string Token;

        /// <summary>
        /// WebHook Uri that allows Slack Webhooks. Incoming Webhooks are a simple way to post messages 
        /// from external sources into Slack. They make use of normal HTTP requests with a JSON payload, 
        /// which includes the message and a few other optional details described later.
        /// Message Attachments(https://api.slack.com/docs/attachments) can also be used in Incoming Webhooks 
        /// to display richly-formatted messages that stand out from regular chat messages.
        /// </summary>
        public readonly string WebHookUri;

        /// <summary>
        /// Flag to show if the sink is using Slack webhooks. True for using webhooks, false for using channel id and token.
        /// </summary>
        public readonly bool UsesWebhooks = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Serilog.Sinks.Slack.SlackChannel"/> class.
        /// </summary>
        public SlackChannel(string webhookUri)
        {
            WebHookUri = webhookUri;
            UsesWebhooks = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Serilog.Sinks.Slack.SlackChannel"/> class.
        /// </summary>
        /// <param name="channelId">Channel identifier.</param>
        /// <param name="token">Token.</param>
        public SlackChannel(string channelId, string token)
        {
            ChannelId = channelId;
            Token = token;
            UsesWebhooks = false;
        }
    }

    /// <summary>
    /// Slack channel collection.
    /// </summary>
    public class SlackChannelCollection : List<SlackChannel>
    {
    }
}
