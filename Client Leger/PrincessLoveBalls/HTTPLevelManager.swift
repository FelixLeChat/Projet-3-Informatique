//
//  HTTPLevelManager.swift
//  PrincessLoveBalls
//
//  Created by Guillaume Lavoie-Harvey on 2016-03-17.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class HTTPLevelManager{
    
    static func getLevelList() -> NSData{
        let uploadResult = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/public", headers : ["Authorization":WebSession.token!])
        return uploadResult.content!
    }
    
    static func getLevelImage(mapHashId:String) -> NSData{
        let uploadResult = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/image/" + mapHashId, headers : ["Authorization":WebSession.token!])        
                
        if(uploadResult.statusCode! == 500 || uploadResult.statusCode! == 400 ){
            print("Image not found")
            let defaultImg = UIImage(named: "black")
            return UIImagePNGRepresentation(defaultImg!)!
        }

        return uploadResult.content!
    }
    
    static func getLevelContent(mapHashID : String) -> String{
        let uploadResult = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/search/" + mapHashID, headers : ["Authorization":WebSession.token!])
        var levelListJson : Dictionary<String, AnyObject> = [:]
        
        do {
            levelListJson = try NSJSONSerialization.JSONObjectWithData(uploadResult.content!, options: NSJSONReadingOptions()) as! Dictionary<String, AnyObject>
        } catch {
            print(error)
        }
        
        return levelListJson["Content"] as! String
    }
    
    /*/api/zones/new/ */
    
    // Name
    // Content -> XML
    // Level -> 0, 1, ou 2, dépendament du niveau requis pour avoir le niveau
    // Returns maphashid
    static func saveLevel(levelName : String, xmlLevelString : String, level : Int) -> String{
        //level upload
        let uploadResult = Just.post("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/new/", data: ["Name":levelName, "Content":xmlLevelString, "Level":level] , headers: ["Authorization":WebSession.token!]).text!
        
        let hashID = String(uploadResult.characters.dropFirst().dropLast(1))
        // image upload
        sendImage(hashID)
        
        return hashID
    }
    
    
    /*
    Content (string, optional),
    Level (integer, optional),
    HashId (string, optional)*/
    
    static func updateLevel(levelName : String, xmlLevelString : String, level : Int) -> String{
        
        var levelListJson : Array<AnyObject> = []
        
        let levelsJSON = HTTPLevelManager.getLevelList()
        
        do {
            let json = try NSJSONSerialization.JSONObjectWithData(levelsJSON, options: NSJSONReadingOptions())
            guard let jsonWTF = json as? Array<AnyObject>
                else{
                    print("Invalid Data for levels")
                    return "Invalid Data for levels"
            }
            levelListJson = jsonWTF
        } catch {
            print(error)
        }
        
        for var i:Int = 0; i < levelListJson.count; i++ {
            if let item = levelListJson[i] as? [String: AnyObject]{
                //print(item)
                if (item["Name"] as! String) == levelName{
                    return Just.post("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/update/", data: ["Content":xmlLevelString, "Level":level, "HashId" : item["HashId"]!] , headers: ["Authorization":WebSession.token!]).text!
                }                
            }
        }

        print("Invalid Data for levels")
        return "Invalid Data for levels"
        
    }
    
    /* /api/zones/image/mapHashID */
    //
    
    static func saveImage(mapHashID : String, image : NSData){
        Just.post("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/image/" + mapHashID, data: ["":image] , headers: ["Authorization":WebSession.token!])
    }
    
    static func hasNewLevelOnServer() -> Bool{        
        if levelListHash == nil{
            let file = "levelListHash.txt"
            
            if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.AllDomainsMask, true).first {
                let path = dir.stringByAppendingPathComponent(file);
                
                //reading
                do {
                    let hashString = try NSString(contentsOfFile: path, encoding: NSUTF8StringEncoding)
                    levelListHash  = Int(hashString as String)
                }
                catch {/* error handling here */}
            }
        }
        
        
        let levels = self.getLevelList()
        if levels.hashvalue == levelListHash{
            return false
        }
        else{
            let levelsJSON = HTTPLevelManager.getLevelList()
            var levelListJson : Array<AnyObject> = Array<AnyObject>()
            
            do {
                let json = try NSJSONSerialization.JSONObjectWithData(levelsJSON, options: NSJSONReadingOptions())
                guard let jsonWTF = json as? Array<AnyObject>
                    else{
                        print("Invalid data, please check connection. Check if client lourd is connected on the same profile.")
                        return false
                    }
                levelListJson = jsonWTF
            } catch {
                print(error)
            }

            if(numberOfLevelsOnServer != nil){
                if(levelListJson.count != numberOfLevelsOnServer){
                    let hashToWrite = String(levels.hashvalue)
                    
                    Utilities.writeToFile("levelListHash.txt", content: hashToWrite)
                    
                    levelListHash = levels.hashvalue
                    
                    return true
                }
                else{
                    return false
                }
            }
            else{
                numberOfLevelsOnServer = levelListJson.count
                return false
            }
            
        }
    }
    
    static func updateMapsFromServer(){
        LevelManager.updateMapsFromServer(getLevelList())
    }
    
    static func getUpdateTimeFromServer(mapHashID : String) -> Utilities.Date{
        let uploadResult = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/search/" + mapHashID, headers : ["Authorization":WebSession.token!])
        var levelListJson : Dictionary<String, AnyObject> = [:]
        
        do {
            levelListJson = try NSJSONSerialization.JSONObjectWithData(uploadResult.content!, options: NSJSONReadingOptions()) as! Dictionary<String, AnyObject>
        } catch {
            print(error)
        }
        
        let time = levelListJson["UpdateTime"] as! String
        
        let year = Int(time.substringWithRange(Range<String.Index>(start: time.startIndex, end: time.startIndex.advancedBy(4))))
        let month = Int(time.substringWithRange(Range<String.Index>(start: time.startIndex.advancedBy(5), end: time.startIndex.advancedBy(7))))
        let day = Int(time.substringWithRange(Range<String.Index>(start: time.startIndex.advancedBy(8), end: time.startIndex.advancedBy(10))))
        let hour = Int(time.substringWithRange(Range<String.Index>(start: time.startIndex.advancedBy(11), end: time.startIndex.advancedBy(13))))
        let minute = Int(time.substringWithRange(Range<String.Index>(start: time.startIndex.advancedBy(14), end: time.startIndex.advancedBy(16))))
        let seconds = Int(time.substringWithRange(Range<String.Index>(start: time.startIndex.advancedBy(17), end: time.startIndex.advancedBy(19))))
        
        return Utilities.Date(year:year!, month:month!, day:day!, hour:hour!, minute:minute!, seconds:seconds!)
    }
    
    private static func sendImage(hashID : String){
        var image : UIImage?
        
        if (imageLevel == nil){
            image = UIImage(named: "black")
        }
        else{
            image = imageLevel
        }
        
        let data : NSData = UIImagePNGRepresentation(image!)!
        
        print(hashID)

        let stringUrl = "http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/image/" + hashID
        
        let url = NSURL(string : stringUrl.stringByAddingPercentEncodingWithAllowedCharacters(NSCharacterSet.URLQueryAllowedCharacterSet())!)
        let request = NSMutableURLRequest(URL: url!)
        request.HTTPMethod = "POST"
        request.HTTPBody = data
        request.addValue(WebSession.token!, forHTTPHeaderField: "Authorization")
        let task = NSURLSession.sharedSession().dataTaskWithRequest(request) { data, response, error in
            guard error == nil && data != nil else {                                                          // check for fundamental networking error
                print("error=\(error)")
                return
            }
            
            if let httpStatus = response as? NSHTTPURLResponse where httpStatus.statusCode != 200 {           // check for http errors
                /*print("statusCode should be 200, but is \(httpStatus.statusCode)")
                print("response = \(response)")*/
            }
            
            //let responseString = NSString(data: data!, encoding: NSUTF8StringEncoding)
            //print("responseString = \(responseString)")
        }
        task.resume()
    }
    
    static func getAllPublicLevels() -> [[String:String]]{
        let uploadResult = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/allpublic", headers : ["Authorization":WebSession.token!]).content!
        
        
        var publicLevels : [[String:String]] = [[:]]
        publicLevels.removeFirst()
        var levelListJson : Array<AnyObject> = []
        
        do {
            let json = try NSJSONSerialization.JSONObjectWithData(uploadResult, options: NSJSONReadingOptions())
            guard let jsonWTF = json as? Array<AnyObject>
                else{
                    print("Invalid data, please check connection. Check if client lourd is connected on the same profile.")
                    return [[:]]
            }
            levelListJson = jsonWTF
        } catch {
            print(error)
            return [[:]]
        }
        
        for var i:Int = 0; i < levelListJson.count; i++ {
            if let item = levelListJson[i] as? [String: AnyObject]{
                //print(item)
                let levelContent = Just.get("http://ec2-52-90-46-132.compute-1.amazonaws.com/api/zones/search/" + (item["HashId"] as! String), headers : ["Authorization":WebSession.token!]).content!
                
                var level : Dictionary<String, AnyObject> = [:]
                
                do {
                    level = try NSJSONSerialization.JSONObjectWithData(levelContent, options: NSJSONReadingOptions()) as! Dictionary<String, AnyObject>
                } catch {
                    print(error)
                }
                
                publicLevels.append(["Name":(item["Name"] as! String), "HashId":(item["HashId"] as! String), "Content":(level["Content"] as! String)])

            }
        }

        
        
        
        return publicLevels
    }
    
    /*static func loadAllLevelsOnLogin(){
        let levelData = getLevelList()
        var levelsJSON : Array<AnyObject> = []
        
        do {
            let json = try NSJSONSerialization.JSONObjectWithData(levelData, options: NSJSONReadingOptions())
            guard let jsonWTF = json as? Array<AnyObject>
                else{
                    print("Invalid data, please check connection. Check if client lourd is connected on the same profile.")
                    return
            }
            levelsJSON = jsonWTF
        } catch {
            print(error)
        }
        
        LevelManager.saveAllUserMapsLocally(levelsJSON)
    }*/
    
    static var imageLevel : UIImage?
    
    private static var levelListHash : Int?
    private static var numberOfLevelsOnServer : Int?
    
}