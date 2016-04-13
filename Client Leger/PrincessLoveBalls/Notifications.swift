//
//  Notifications.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-24.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class Notifications{
    static func callNotifications (message : String, category : String){
        let notification = UILocalNotification()
        notification.alertBody = message // text that will be displayed in the notification
        notification.fireDate = NSDate().dateByAddingTimeInterval(1) // todo item due date (when notification will be fired)
        notification.soundName = UILocalNotificationDefaultSoundName // play default sound
        notification.userInfo = ["UUID": UUID, ] // assign a unique identifier to the notification so that we can retrieve it later
        notification.category = category
        UIApplication.sharedApplication().scheduleLocalNotification(notification)
        
        UUID++
    }
    
    static var UUID = 0
}