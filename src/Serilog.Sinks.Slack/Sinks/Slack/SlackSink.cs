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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
        /// The Slack bot name.
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// URL to an image to use as the icon for this message.
        /// </summary>
        private readonly string _iconUrl;

        /// <summary>
        ///     Construct a sink posting to the specified Slack Channel.
        /// </summary>
        /// <param name="channelId">Slack Channel Id.</param>
        /// <param name="token">Token that allows Slack authentication. To manage tokens go to https://api.slack.com/tokens.</param>
        /// <param name="formatProvider">FormatProvider to apply to <see cref="LogEvent.RenderMessage(IFormatProvider)"/>.</param>
        /// <param name="username">Optional bot name</param>
        /// <param name="iconUrl">Optional URL to an image to use as the icon for this message.</param>
        public SlackSink(string channelId, string token,
                               IFormatProvider formatProvider, string username = null, string iconUrl = null)
        {
            if (string.IsNullOrWhiteSpace(channelId))
                throw new ArgumentNullException("channelId");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            FormatProvider = formatProvider;
            Channels.Add(new SlackChannel(channelId, token));
            _username = username;
            _iconUrl = iconUrl;
        }

        /// <summary>
        ///     Construct a sink posting to the specified Slack Channel.
        /// </summary>
        /// <param name="channels">Slack Channel list.</param>
        /// <param name="renderMessageImplementation">Optional delegate to build json to send to slack webhook. By default uses <see cref="RenderMessage"/>.</param>
        /// <param name="formatProvider">FormatProvider to apply to <see cref="LogEvent.RenderMessage(IFormatProvider)"/>.</param>
        /// <param name="username">Optional bot name</param>
        /// <param name="iconUrl">Optional URL to an image to use as the icon for this message.</param>
        public SlackSink(SlackChannelCollection channels,
            SlackSink.RenderMessageMethod renderMessageImplementation,
                               IFormatProvider formatProvider, string username = null, string iconUrl = null)
        {
            if (channels == null)
                throw new ArgumentNullException("channels");

            FormatProvider = formatProvider;
            Channels = channels;
            RenderMessageImplementation = renderMessageImplementation ?? RenderMessage;
            _username = username;
            _iconUrl = iconUrl;

            if (Channels.Count == 0)
                SelfLog.WriteLine("There are 0 Slack channels defined. Slack sink will not send messages.");
        }

        /// <summary>
        ///     Construct a sink posting to the specified Slack Channel.
        /// </summary>
        /// <param name="webhookUri">WebHook Uri that allows Slack Incoming Webhooks (https://api.slack.com/incoming-webhooks).</param>
        /// <param name="renderMessageImplementation">Optional delegate to build json to send to slack webhook. By default uses <see cref="RenderMessage"/>.</param>
        /// <param name="formatProvider">FormatProvider to apply to <see cref="LogEvent.RenderMessage(IFormatProvider)"/>.</param>
        public SlackSink(string webhookUri,
            SlackSink.RenderMessageMethod renderMessageImplementation,
                               IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(webhookUri))
                throw new ArgumentNullException("webhookUri");

            FormatProvider = formatProvider;
            Channels.Add(new SlackChannel(webhookUri));
            RenderMessageImplementation = renderMessageImplementation ?? RenderMessage;
            ;

            if (Channels.Count == 0)
                SelfLog.WriteLine("There are 0 Slack channels defined. Slack sink will not send messages.");
          
        }

        /// <summary>
        /// Delegate to allow overriding of the RenderMessage method.
        /// </summary>
        public delegate string RenderMessageMethod(LogEvent input);

        /// <summary>
        /// RenderMessage method that will transform LogEvent into a Slack message.
        /// </summary>
        protected RenderMessageMethod RenderMessageImplementation = RenderMessage;

        #region ILogEventSink implementation

        public void Emit(LogEvent logEvent)
        {
            foreach (var item in Channels)
            {
                // FormatProvider overrides default behaviour
                var message = (FormatProvider != null) ? logEvent.RenderMessage(FormatProvider) : RenderMessageImplementation(logEvent);

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

        protected static string RenderMessage(LogEvent logEvent)
        {
            dynamic body = new ExpandoObject();
            body.text = logEvent.RenderMessage();
            body.attachments = WrapInAttachment(logEvent).ToArray();

            return Newtonsoft.Json.JsonConvert.SerializeObject(body);
        }

        protected void SendMessageWithChannelIdAndToken(string token, string channelId, string message)
        {
            SelfLog.WriteLine("Trying to send message to channelId '{0}' with token '{1}': '{2}'.", channelId, token, message);

            var sendMessageResult = SlackClient.SlackClient.SendMessage(token, channelId, message, _username, _iconUrl);
            if (sendMessageResult != null)
            {
                SelfLog.WriteLine("Message sent to channelId '{0}' with token '{1}': '{2}'.", channelId, token, sendMessageResult.JsonValue.ToString());
            }
        }

        protected void SendMessageWithWebHooks(string webhookUri, string message)
        {
            SelfLog.WriteLine("Trying to send message to webhook '{0}': '{1}'.", webhookUri, message);

            if (message != null)
            {
                var sendMessageResult = SlackClient.SlackClient.SendMessageViaWebhooks(webhookUri, message);
                if (sendMessageResult != null)
                {
                    SelfLog.WriteLine("Message sent to webhook '{0}': '{1}'.", webhookUri, sendMessageResult);
                }
            }
        }

        protected static string GetAttachmentColor(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Information:
                    return "#5bc0de";
                case LogEventLevel.Warning:
                    return "#f0ad4e";
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    return "#d9534f";
                default:
                    return "#777";
            }
        }

        protected static object CreateAttachmentField(string title, string value, bool @short = true)
        {
            return new { title, value, @short };
        }

        protected static object WrapInAttachment(Exception ex)
        {
            return new
            {
                title = "Exception",
                fallback = string.Format("Exception: {0} \n {1}", ex.Message, ex.StackTrace),
                color = GetAttachmentColor(LogEventLevel.Fatal),
                fields = new[]
                {
                    CreateAttachmentField("Message", ex.Message),
                    CreateAttachmentField("Type", "`"+ex.GetType().Name+"`"),
                    CreateAttachmentField("Stack Trace", "```"+ex.StackTrace+"```", false)
                },
                mrkdwn_in = new[] { "fields" }
            };
        }

        protected static IEnumerable<dynamic> WrapInAttachment(LogEvent log)
        {
            var result = new List<dynamic>
            {
                new
                {
                    fallback = string.Format("[{0}]{1}", log.Level, log.RenderMessage()),
                    color = GetAttachmentColor(log.Level),
                    fields = new[]
                    {
                        CreateAttachmentField("Level", log.Level.ToString()),
                        CreateAttachmentField("Timestamp", log.Timestamp.ToString())
                    }
                }
            };

            if (log.Exception != null)
                result.Add(WrapInAttachment(log.Exception));

            return result;
        }
    }
}
