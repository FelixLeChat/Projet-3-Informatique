//
//  WebManager.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-14.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class WebSession{
    
    static func clear(){
        username = ""
        token = ""
        password = ""
        isConnected = false
        level = 0
        userHashId = ""
    }
    
    static var username : String?
    
    static var token : String?
    
    static var password : String?
    
    static var isConnected : Bool = false
    
    static var level : Int = 0
    
    static var userHashId : String?
    
}