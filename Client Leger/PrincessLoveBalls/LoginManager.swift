//
//  LoginManager.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-14.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class LoginManager{
    func connectToServer(username: String, password: String) ->Bool{
        WebSession.username = username
        WebSession.password = password
        WebSession.token = String(httpLogin.connectToServer(username, password: password).characters.dropFirst().dropLast(1))
        
        
        WebSession.isConnected = (WebSession.token != "LOGIN ERROR")
        if(WebSession.isConnected){
            WebSession.level = httpLogin.getUserLevel(username)
        }
        
        print("level:",WebSession.level)
        return WebSession.isConnected
    }
    
    private var httpLogin : HTTPLogin = HTTPLogin()
}
