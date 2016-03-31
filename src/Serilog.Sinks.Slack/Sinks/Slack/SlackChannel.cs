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
        public string ChannelId { get; set; }

        /// <summary>
        /// Token that allows Slack authentication. To manage tokens go to https://api.slack.com/tokens
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Serilog.Sinks.Slack.SlackChannel"/> class.
        /// </summary>
        public SlackChannel()
        {
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
        }
    }

    /// <summary>
    /// Slack channel collection.
    /// </summary>
    public class SlackChannelCollection : List<SlackChannel>
    {
    }
}
