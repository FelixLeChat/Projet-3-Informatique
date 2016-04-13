//
//  HTTPFriendManager.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-24.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

/* Structure received*/
/*
[{"Username":"Felix1","HashId":"481621452296252202193522052321162181560215160190240112","AreFriend":true},{"Username":"kofed","HashId":"1164121111318315225239103224224612622918726104116219","AreFriend":true}]
*/

class HTTPFriendManager{
    
    func getFriendListinJSON() -> [[String:String]]{
        let uploadResult = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/friend", headers : ["Authorization":WebSession.token!])
        let json = uploadResult.json as! Array<AnyObject>
        
        var listOfFriends : [[String:String]] = [[:]]
        
        for item in json{
            if let friend = item as? [String: AnyObject]{
                //print("Username :", friend["Username"] as! String, "HashId :", friend["HashId"] as! String, "\n")
                listOfFriends.append(["Username" : friend["Username"] as! String, "HashId" : friend["HashId"] as! String])                
            }
        }
        listOfFriends.removeFirst()
        return listOfFriends
    }
}