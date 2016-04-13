//
//  ChatLogView.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-17.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit
import Foundation

class ChatLogView : UIView{
    
    init(parent: ChatLogViewController){
        super.init(frame: CGRect(x: 0, y: 0, width: UIScreen.mainScreen().bounds.width, height: UIScreen.mainScreen().bounds.height))
        
        self.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        
        // On change le font pour qu'il soit le type par défaut, mais un peu plus grand
        chatLogBox.editable = false
        chatLogBox.font = UIFont(name: (chatEntryField.font?.fontName)!, size: 18)
        chatLogBox.alpha = 0.7
        chatLogBox.layer.borderWidth = 4
        chatLogBox.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        chatLogBox.layer.cornerRadius = 20
        
        chatEntryField.placeholder = "Start Chatting here!"
        chatEntryField.borderStyle = UITextBorderStyle.RoundedRect
        chatEntryField.autocorrectionType = UITextAutocorrectionType.No
        chatEntryField.keyboardType = UIKeyboardType.Default
        chatEntryField.returnKeyType = UIReturnKeyType.Done
        chatEntryField.clearButtonMode = UITextFieldViewMode.WhileEditing;
        chatEntryField.contentVerticalAlignment = UIControlContentVerticalAlignment.Center
        chatEntryField.font = UIFont(name: (chatEntryField.font?.fontName)!, size: 18)
        chatEntryField.delegate = parent
        
        sendTextButton.setImage(UIImage(named:"Send_Button"), forState: UIControlState.Normal)
        sendTextButton.frame = CGRect(x: 600, y: 622, width: 150, height: 68)
        sendTextButton.addTarget(parent, action: "sendButtonTapped:", forControlEvents: UIControlEvents.TouchUpInside)
        
        backButton.setImage(UIImage(named: "Back_Button"), forState: UIControlState.Normal)
        backButton.frame = CGRect(x: 605, y: 40, width: 150, height: 68)
        backButton.addTarget(parent, action: "backButtonTapped:", forControlEvents: UIControlEvents.TouchUpInside)
        
        closeChannelButton.setImage(UIImage(named: "Close_Button2"), forState: UIControlState.Normal)
        closeChannelButton.frame = CGRect(x: 615, y: 521, width: 130, height: 59)
        closeChannelButton.addTarget(parent, action: "closeChannelButtonTapped:", forControlEvents: UIControlEvents.TouchUpInside)
        
        buttonCreateChannel.setImage(UIImage(named:"Join_Button"), forState: UIControlState.Normal)
        buttonCreateChannel.frame = CGRect(x: 615, y: 430, width: 130, height: 59)
        buttonCreateChannel.addTarget(parent, action: "createChannelButtonTapped:", forControlEvents: UIControlEvents.TouchUpInside)
        
        createChannelField.placeholder = "Channel Name"
        createChannelField.borderStyle = UITextBorderStyle.RoundedRect
        createChannelField.autocorrectionType = UITextAutocorrectionType.No
        createChannelField.keyboardType = UIKeyboardType.Default
        createChannelField.returnKeyType = UIReturnKeyType.Done
        createChannelField.clearButtonMode = UITextFieldViewMode.WhileEditing;
        createChannelField.contentVerticalAlignment = UIControlContentVerticalAlignment.Center
        createChannelField.font = UIFont(name: (chatEntryField.font?.fontName)!, size: 18)
        
        transparentBox1.alpha = 0.6
        transparentBox1.backgroundColor = UIColor.whiteColor()
        transparentBox1.layer.zPosition = -1
        transparentBox1.layer.borderWidth = 4
        transparentBox1.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentBox1.layer.cornerRadius = 20
        
        transparentBox2.alpha = 0.6
        transparentBox2.backgroundColor = UIColor.whiteColor()
        transparentBox2.layer.zPosition = -1
        transparentBox2.layer.borderWidth = 4
        transparentBox2.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentBox2.layer.cornerRadius = 20
        
        addSubview(transparentBox1)
        addSubview(transparentBox2)
        //addSubview(transparentBox3)
        addSubview(chatLogBox)
        addSubview(chatEntryField)
        addSubview(sendTextButton)        
        addSubview(createChannelField)
        addSubview(backButton)
        addSubview(buttonCreateChannel)
        addSubview(closeChannelButton)
        
    }

    // Required because Steve Jobs has no idea what he is doing
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    var chatLogBox : UITextView = UITextView(frame: CGRect(x: 40, y: 40, width: 550, height: 550))
    
    var chatEntryField : UITextField = UITextField(frame: CGRect(x: 40, y: 622, width: 550, height: 68))
    var sendTextButton : UIButton = UIButton(type: UIButtonType.Custom)
    
    var createChannelField : UITextField = UITextField(frame: CGRect(x: 615, y: 379, width: 130, height: 44))
    var buttonCreateChannel : UIButton = UIButton(type: UIButtonType.Custom)
    
    var backButton : UIButton = UIButton(type: UIButtonType.Custom)
    
    var closeChannelButton : UIButton = UIButton(type : UIButtonType.Custom)
    
    var transparentBox1 : UIView = UIView(frame: CGRect(x: 605, y: 369, width: 150, height: 130))
    var transparentBox2 : UIView = UIView(frame: CGRect(x: 605, y: 511, width: 150, height: 79))
}




