//
//  FriendList.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-24.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class FriendList{
    func setFriendList(friends : [[String:String]]){
        self.friends = friends
    }
    
    func getFriendsByName() -> [String]{
        var friendNames : [String] = []
        for friend in friends{
            friendNames.append(friend["Username"]!)
        }
        return friendNames
    }
    
    func getFriendHash(name: String) -> String?{
        for friend in friends{
            if(friend["Username"] == name){
                return friend["HashId"]
            }
        }
        return nil
    }
    
    var friends : [[String:String]] = [[:]]
}