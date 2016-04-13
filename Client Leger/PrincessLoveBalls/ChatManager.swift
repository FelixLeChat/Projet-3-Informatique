//
//  ChatManager.swift
//  SimpleChat
//
//  Created by Alex Gagne on 2016-01-11.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class ChatManager {
    var _chatWebSocket : ChatWebSocket?
    
    private var channels : [String:String] = [:]
    
    weak var _viewController : ChatViewController?
    
    init(viewControl:ChatViewController){
        self._viewController = viewControl
        _chatWebSocket = ChatWebSocket(chatManager: self)
        _chatWebSocket?.connectServer()
    }
    
    func isConnected(){        
        _chatWebSocket?.startChatChannel()
    }
    
    func writeMessage(text:String, channelName : String){
        _chatWebSocket?.writeMessage(text, channelID : getCanalIDByName(channelName))
    }
    
    func receivedNewChatText(text:String){
        //Need to separate into message and channel name
        let json = Utilities.getJSONStringFromString(text)
        
        guard let canalId = json["CanalId"]
            
        else{
            // If the json is invalid
            return
        }
        let message = json["Message"]
        let canalName = json["CanalName"]
        
        /*print("What I actually received")
        print(message, " ", canalId, " ", canalName)
        print("\n\n")*/
        
        guard let _ = message
        
        else{
            if(canalName != ""  && canalId != "" ){
                // New Canal Request
                //print("Created channel: " + canalId + "\n Channel name: " + canalName!)
                channels[json["CanalId"]!] = canalName
                _viewController?.addNewChatChannel(canalName!)
            }
            return
        }
        if(message != "" && canalId != ""){
            // New message received
            //print("Received message: " + message! + "\n on channel: " + getCanalNameByID(canalId))
            _viewController?.updateLog(message!, channelName: getCanalNameByID(canalId))
        }
        
    }
    
    func createChannel(channelName:String){
        _chatWebSocket?.addChatChannel(channelName)
    }
    
    func getCanalNameByID(id : String) -> String{
        return channels[id]!
    }
    
    func getCanalIDByName(name : String) -> String{
        let keys = (channels as NSDictionary).allKeysForObject(name) as! [String]
        return keys[0]
    }
    
    func connectServer(){
        self._chatWebSocket?.connectServer()
    }
}