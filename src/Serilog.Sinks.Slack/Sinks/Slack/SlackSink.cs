// Copyright 2013 Serilog Contributors 
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Sinks.Slack
{
    /// <summary>
    ///     Writes log events as messages to Slack Channels.
    /// </summary>
    public class SlackSink : ILogEventSink
    {
        /// <summary>
        /// The Slack channels collection.
        /// </summary>
        protected readonly SlackChannelCollection Channels = new SlackChannelCollection();

        /// <summary>
        /// The <see cref="IFormatProvider"/> used to apply to <see cref="LogEvent.RenderMessage(IFormatProvider)"/>.
        /// </summary>
        protected readonly IFormatProvider FormatProvider;

        /// <summary>
        ///     Construct a sink posting to the specified Slack Channel.
        /// </summary>
        /// <param name="channelId">Slack Channel Id.</param>
        /// <param name="token">Token that allows Slack authentication. To manage tokens go to https://api.slack.com/tokens.</param>
        /// <param name="formatProvider">FormatProvider to apply to <see cref="LogEvent.RenderMessage(IFormatProvider)"/>.</param>
        public SlackSink(string channelId, string token,
                               IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(channelId))
                throw new ArgumentNullException("channelId");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            FormatProvider = formatProvider;
            Channels.Add(new SlackChannel(channelId, token));
        }

        /// <summary>
        ///     Construct a sink posting to the specified Slack Channel.
        /// </summary>
        /// <param name="channels">Slack Channel list.</param>
        /// <param name="formatProvider">FormatProvider to apply to <see cref="LogEvent.RenderMessage(IFormatProvider)"/>.</param>
        public SlackSink(SlackChannelCollection channels,
                               IFormatProvider formatProvider)
        {
            if (channels == null)
                throw new ArgumentNullException("channels");

            FormatProvider = formatProvider;
            Channels = channels;

            if (Channels.Count == 0)
                SelfLog.WriteLine("There are 0 Slack channels defined. Slack sink will not send messages.");
        }

        #region ILogEventSink implementation

        public void Emit(LogEvent logEvent)
        {
            foreach (var item in Channels)
            {
                var message = (FormatProvider != null) ? logEvent.RenderMessage(FormatProvider) : logEvent.RenderMessage();


                if (item.UsesWebhooks)
                {
                    SendMessageWithWebHooks(item.WebHookUri, message);
                }
                else
                {
                    SendMessageWithChannelIdAndToken(item.Token, item.ChannelId, message);
                }
            }
        }

        #endregion

        protected string GetMessage(LogEvent logEvent)
        {
            var message = string.Empty;
            // """ {"text": "*Level*                *Timestamp*\n\nVerbose         31/03/2016 10:00:00 ", 
            // "username": "mybot", "icon_url": "https://slack.com/img/icons/app-57.png", "icon_emoji": ":ghost:"} """
            // "attachments":[{"fallback":"New open task [Urgent]: ","pretext":"New open task [Urgent]: ","color":"#D00000","fields":[{"title":"Notes","value":"This is much easier than I thought it would be.","short":false}]}]

            switch (logEvent.Level)
            {
                case LogEventLevel.Debug:
                    break;
                case LogEventLevel.Information:
                    break;
                case LogEventLevel.Verbose:
                    break;
                case LogEventLevel.Warning:
                    break;
                case LogEventLevel.Error:
                    break;
                case LogEventLevel.Fatal:
                    break;
                default:
                    break;
            }

            return message;
        }

        protected void SendMessageWithChannelIdAndToken(string token, string channelId, string message)
        {
            SelfLog.WriteLine("Trying to send message to channelId '{0}' with token '{1}': '{2}'.", channelId, token, message);

            var sendMessageResult = SlackClient.SlackClient.SendMessage(token, channelId, message);
            if (sendMessageResult != null)
            {
                SelfLog.WriteLine("Message sent to channelId '{0}' with token '{1}': '{2}'.", channelId, token, sendMessageResult.JsonValue.ToString());
            }
        }

        protected void SendMessageWithWebHooks(string webhookUri, string message)
        {
            SelfLog.WriteLine("Trying to send message to webhook '{0}': '{1}'.", webhookUri, message);

            var sendMessageResult = SlackClient.SlackClient.SendMessageViaWebhooks(webhookUri, message);
            if (sendMessageResult != null)
            {
                SelfLog.WriteLine("Message sent to webhook '{0}': '{1}'.", webhookUri, sendMessageResult);
            }
        }
    }
}
