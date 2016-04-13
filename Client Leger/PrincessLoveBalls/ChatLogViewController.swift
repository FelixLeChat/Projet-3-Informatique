//
//  ChatLogViewController.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-17.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit
import Foundation

class ChatLogViewController : UIViewController, UITextFieldDelegate{
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    init(parent superController : ChatViewController, name: String){
        super.init(nibName: nil, bundle : nil)
        self.superController = superController
        self.name = name
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.view = ChatLogView(parent: self)
    }
    
    func textFieldShouldReturn(sender: UITextField) -> Bool {
        let chatLogView = sender.superview as! ChatLogView
        if(chatLogView.chatEntryField.text != ""){
            superController?.writeMessage(chatLogView.chatEntryField.text!, channelName: self.name)
            chatLogView.chatEntryField.text = ""
            return true
        }
        return false
    }
    
    @IBAction func sendButtonTapped(button : UIButton){
        let chatLogView = button.superview as! ChatLogView
        if(chatLogView.chatEntryField.text != ""){
            superController?.writeMessage(chatLogView.chatEntryField.text!, channelName: self.name)
            chatLogView.chatEntryField.text = ""
        }
    }
    
    @IBAction func closeChannelButtonTapped(button : UIButton){
        superController?.closeCurrentChannel()
    }
    
    func updateLog(message : String){
        chatLog.addLineOfChat(message)
        let chatLogView = self.view as! ChatLogView
        chatLogView.chatLogBox.text = chatLog.getChatText()
    }
    
    func createChannelButtonTapped(button : UIButton){
        let chatLogView = button.superview as! ChatLogView
        if(chatLogView.createChannelField.text != ""){
            superController?.createChannel(chatLogView.createChannelField.text!)
            chatLogView.createChannelField.text = ""
        }
    }
    
    func backButtonTapped(button : UIButton){
        superController?.getBackToMainMenu()
    }
    
    func clearLog(){
        chatLog.clearLog()
    }
    
    var name : String = ""
    weak var superController : ChatViewController?
    var chatLog : ChatLog = ChatLog()
}
