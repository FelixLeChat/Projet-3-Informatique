//
//  ChatWebSocket.swift
//  SimpleChat
//
//  Created by Alex Gagne on 2016-01-11.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class ChatWebSocket : WebSocketDelegate {
    
    init(chatManager:ChatManager){
        self.chatManager = chatManager
    }
    
    func connectServer(){
        _socket = WebSocket(url: NSURL(string: "ws://ec2-52-90-46-132.compute-1.amazonaws.com/Websocket/WsChatHandler.ashx")!)
        _socket!.delegate = self
        _socket!.connect()
    }
    
    func addChatChannel(channelName : String){
        writeJSON(Utilities.buildJSONString(["Message" : "", "CanalId" : "", "CanalName" :  channelName, "UserToken" : WebSession.token!]))
    }

    func writeMessage(text : String, channelID : String){
        writeJSON(Utilities.buildJSONString(["Message" : text, "CanalId" : channelID, "CanalName" :  "", "UserToken" : WebSession.token!]))
    }
    
    func startChatChannel(){
        writeJSON(Utilities.buildJSONString(["Message" : "", "CanalId" : "", "CanalName" : "General", "UserToken" : WebSession.token!]))
    }
    
    private func writeJSON(valueToSend : Utilities.JSONString){
        //print("Text sent: " + valueToSend.jsonString!)
        _socket?.writeString(valueToSend.jsonString!)
    }
    
    //********************--Internal Methods--*********************//
    
    func websocketDidConnect(socket: WebSocket) {
        print("websocket is connected")
        
        if(!_isConnected){
            _isConnected = true
            chatManager?.isConnected()
        }
    }
    
    func websocketDidDisconnect(socket: WebSocket, error: NSError?) {
        print("websocket is disconnected: \(error?.localizedDescription)")
        if(_isConnected){
            _socket!.connect()
        }
    }
    
    func websocketDidReceiveMessage(socket: WebSocket, text: String) {
        chatManager?.receivedNewChatText(text)
        //print("Text received: " + text)
    }
    
    func websocketDidReceiveData(socket: WebSocket, data: NSData) {
        print("got some data: \(data.length)")
        print("Not supposed to happen")
    }
    
    
    //*******************--Member Variables--*********************//
    
    private var _socket : WebSocket?
    var _isConnected: Bool = false
    private var _username: String?
    
    private weak var chatManager : ChatManager?
}