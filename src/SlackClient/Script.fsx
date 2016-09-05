// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

//#r @"../../packages/FSharp.Core.4.0.0.1/lib/net40/FSharp.Core.dll";;
#r @"../../packages/FSharp.Data.2.3.2/lib/net40/FSharp.Data.dll";;

#load "SlackClient.fs"
open SlackClient
open System.Net
open FSharp.Core
open FSharp.Data
open FSharp.Data.HttpRequestHeaders

let apiToken = "yyyy-13773446566-12344567-12344567-23rf23rf23r"

(*
let channelListRequest (token:string) = "https://slack.com/api/channels.list?token=" + token + "&pretty=1"
type ChannelListResponse = JsonProvider<""" {"ok":true,"channels":[{"id":"XXXXXXXXX","name":"bots","is_channel":true,"created":1459252554,"creator":"U0DNSJWGY","is_archived":false,"is_general":false,"is_member":true,"members":["U0DNSJWGY"],"topic":{"value":"","creator":"","last_set":0},"purpose":{"value":"Channel to test Slack API and develop integrations with apps.","creator":"U0DNSJWGY","last_set":1459252554},"num_members":1},{"id":"C0DNMCR6Z","name":"general","is_channel":true,"created":1446556159,"creator":"U0DNSJWGY","is_archived":false,"is_general":true,"is_member":true,"members":["U0DNSJWGY","U0DSSCUE6"],"topic":{"value":"Company-wide announcements and work-based matters","creator":"","last_set":0},"purpose":{"value":"This channel is for team-wide communication and announcements. All team members are in this channel.","creator":"","last_set":0},"num_members":2},{"id":"C0EDNC892","name":"potential-competitors","is_channel":true,"created":1447367127,"creator":"U0DNSJWGY","is_archived":false,"is_general":false,"is_member":true,"members":["U0DNSJWGY","U0DSSCUE6"],"topic":{"value":"","creator":"","last_set":0},"purpose":{"value":"channel dedicated to competitiveness intelligence","creator":"U0DNSJWGY","last_set":1447367127},"num_members":2},{"id":"C0DNNMYCB","name":"random","is_channel":true,"created":1446556159,"creator":"U0DNSJWGY","is_archived":false,"is_general":false,"is_member":true,"members":["U0DNSJWGY","U0DSSCUE6"],"topic":{"value":"Non-work banter and water cooler conversation","creator":"","last_set":0},"purpose":{"value":"A place for non-work-related flimflam, faffing, hodge-podge or jibber-jabber you'd prefer to keep out of more focused work-related channels.","creator":"","last_set":0},"num_members":2}],"error": "not_authed"} """>
let channelList = ChannelListResponse.Load(channelListRequest apiToken)
let botChannelId = channelList.Channels.[0].Id
*)
let botChannelId = "XXXXXXXXXX"
let webhookUri = "https://hooks.slack.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX"

let testMessage = "`This%20is%20a%20test%20message!`"
let testMessage1 = "`This is a test message!`"
let testMessage2 = "This is a test message!"

//let sendMessageResult = SlackClient.SendMessage (apiToken, botChannelId, testMessage1)
//sendMessageResult.Ok

let testMessageWebHooks = """ {"text": "*Level*                *Timestamp*\n\nVerbose         31/03/2016 10:00:00 ", "username": "mybot", "icon_url": "https://slack.com/img/icons/app-57.png", "icon_emoji": ":ghost:", "attachments":[{"fallback":"New open task [Urgent]: ","pretext":"New open task [Urgent]: ","color":"#D00000","fields":[{"title":"Notes","value":"This is much easier than I thought it would be.","short":false}]}]} """
let testMessageWebHooks1 = """ {"attachments":[{"fallback":"New open task [Urgent]: ","pretext":"New open task [Urgent]: ","color":"#D00000","fields":[{"title":"Notes","value":"This is much easier than I thought it would be.","short":false}]}]} """
let testMessageWebHooks2 = """ {"text":"*Level*                *Timestamp *\n\nFatal         04/01/2016 16:49:56 ","attachments":[{"fallback":"Exception catched at Main.: ","pretext":"Exception catched at Main.: ","color":"#d9534f","fields":[{"title":"Stack Trace","value":"`   at Serilog.Sinks.Slack.TestConsoleApp.Program.Main(String[] args) in Y:Projectsserilog-sinks-slacksrcSerilog.Sinks.Slack.TestConsoleAppProgram.cs:line 23`","short":false}]}]} """

let Request = SlackClient.SendMessageViaWebhooks (webhookUri, testMessageWebHooks2)
