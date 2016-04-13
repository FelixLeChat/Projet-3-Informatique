//
//  ChatLog.swift
//  SimpleChat
//
//  Created by Alex Gagne on 2016-01-11.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class ChatLog{
    
    func addLineOfChat(lineOfChat:String){
        if !canAddToLog(){
            _textLog.removeFirst()
        }
        _textLog.append(lineOfChat)
    }
    
    func getChatText() ->String {
        var chatText = ""
        
        for line in _textLog{
            chatText += (line + "\n")
        }
        
        return chatText
    }
    
    func clearLog(){
        _textLog = [String]()
    }
    
    func getNumberOfChatLines() -> Int{
        return _textLog.count
    }
    
    //*********************--Member Methods--**********************//
    private func canAddToLog() -> Bool{
        return _textLog.count <= MAX_NUMBER_OF_LOGS
    }
    
    //*******************--Member Variables--*********************//
    
    private let MAX_NUMBER_OF_LOGS = 100
    
    private var _textLog = [String]()
    
}