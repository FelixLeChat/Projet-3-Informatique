//
//  WebLogin.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-14.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class HTTPLogin{
    func connectToServer(username: String, password: String) -> String{
        let uploadResult = Just.post("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/user/login", json : ["Facebookid" : "","Username" :  username,"Password" : password])
        //print(uploadResult.text ?? "")
        if(uploadResult.statusCode >= 400){
            return " LOGIN ERROR " //DO NOT REMOVE THE SPACES, THEY MATTER  #SpaceLivesMatter
        }
        return uploadResult.text ?? ""
    }
    
    
    func registerUser(username:String, password:String)->HTTPResult{
        let registerResult = Just.post("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/user/register", json : ["Facebookid" : "", "Username" :  username,"Password" : password])
        
        return registerResult
    }
    
    
    
    func getUserLevel(username: String) -> Int{
        
        var userHash:String = String()
        
        let allUsers = Just.post("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/profile/all", headers : ["Authorization":WebSession.token!])
        print("request 1 code", allUsers.statusCode)
        do {
            let json = try NSJSONSerialization.JSONObjectWithData(allUsers.content!, options: NSJSONReadingOptions()) as! [AnyObject]
            for(var i = 0; i < json.count; i++){
                let user = json[i] as? [String:AnyObject]
                if(user!["Username"] as? String == WebSession.username){
                    userHash = (user!["HashId"] as? String)!
                    WebSession.userHashId = (user!["HashId"] as? String)!
                }
            }
        } catch {
            return -1
        }
        
        
        let levelResult = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/profile/user/" + userHash, headers : ["Authorization":WebSession.token!])
        print("request 2 code", levelResult.statusCode)
        do {
            let json = try NSJSONSerialization.JSONObjectWithData(levelResult.content!, options: NSJSONReadingOptions()) as! [String:AnyObject]
            print("json",json)
            let level = json["Level"] as? Int
            return level!
        } catch {
            return -1
        }
    }
    
}