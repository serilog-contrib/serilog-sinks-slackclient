namespace SlackClient

open FSharp.Data

type ChatPostMessageResponse = 
    JsonProvider<""" {"ok":true,"channel":"XXXXXXXXX","ts":"1459354769.000004","message":{"text":"This is a test message!","username":"bot","type":"message","subtype":"bot_message","ts":"1459354769.000004"}, "error": "not_authed" } """>

module SlackClient = 

    let internal ChatPostMessageRequest (token:string, channelId:string, msg:string) = ("https://slack.com/api/chat.postMessage?token=" + token + "&channel=" + channelId + "&text=" + msg + "&pretty=1")
        
    let public SendMessage (token:string, channelId:string, message:string) = ChatPostMessageResponse.Load(ChatPostMessageRequest (token, channelId, message))
