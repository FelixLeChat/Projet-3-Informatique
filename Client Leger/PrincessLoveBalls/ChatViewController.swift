//
//  ChatViewController.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-10.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit
import Foundation

class ChatViewController: UITabBarController, UITabBarControllerDelegate{
   
    override func viewDidLoad() {
        super.viewDidLoad()
        delegate = self
    }
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.friends = []
        chatManager = ChatManager(viewControl: self)
    }
    
    func addNewChatChannel(name : String){
        
        if let controls = self.viewControllers{
            for control in controls{
                if let chatControl = control as? ChatLogViewController{
                    if(chatControl.name == name){
                        return
                    }
                }
            }
        }
        
        let chatLog = ChatLogViewController(parent: self, name: name)
        
        chatLog.tabBarItem = UITabBarItem(
            title: chatLog.name,
            image: nil,
            tag: currentTag)
        
        currentTag++
        
        let attributes = [NSFontAttributeName:UIFont(name: "Helvetica", size: 18) as AnyObject!]
        chatLog.tabBarItem.setTitleTextAttributes(attributes, forState: .Normal)
        
        var controllers = []
        guard var _controllers = self.viewControllers
            
        else
        {
            controllers = [chatLog]
            addChannelToView(controllers as! [UIViewController])
            return
        }
        
        _controllers.append(chatLog as UIViewController)
        addChannelToView(_controllers)
    }
    
    private func addChannelToView(controllers : [UIViewController]){
        self.setViewControllers(controllers, animated: true)
    }
    
    func writeMessage(message : String, channelName: String){
        chatManager?.writeMessage(message, channelName: channelName)
        messageSent = true
    }
    
    func createChannel(channelName : String){
        chatManager?.createChannel(channelName)
    }
    
    func updateLog(message : String, channelName : String){
        for chatLogViewController in self.viewControllers as! [ChatLogViewController]{
            if(chatLogViewController.name == channelName){
                chatLogViewController.updateLog(message)
            }
        }
        if(!messageSent){
            Notifications.callNotifications("Nouveau message dans le canal " + channelName + "!", category: "Chat Message")
        }
        messageSent = false
    }
    
    func getBackToMainMenu(){
        for chatLogViewController in self.viewControllers as! [ChatLogViewController]{
            chatLogViewController.clearLog()
        }
        performSegueWithIdentifier("getBackFromChat", sender: self)
    }
    
    func closeCurrentChannel(){
        if let chatControl = self.selectedViewController as? ChatLogViewController{
            if(chatControl.name != "General"){
                var controllers = self.viewControllers
                controllers?.removeAtIndex(self.selectedIndex)
                addChannelToView(controllers!)
            }
            else{
                let alert = UIAlertController(title: "Action invalide", message: "On ne peut pas fermer le canal Général", preferredStyle: UIAlertControllerStyle.Alert)
                let okButton = UIAlertAction(title: "Ok", style: .Default) { (alert: UIAlertAction!) -> Void in
                    // Do nothing after pushing button
                }
                alert.addAction(okButton)
                presentViewController(alert, animated: true, completion: nil)
            }
        }
    }
    
    private var friends : [String]?
    
    private var chatManager : ChatManager?
    private var currentTag : Int = 1
    
    private var messageSent : Bool = false
    
}