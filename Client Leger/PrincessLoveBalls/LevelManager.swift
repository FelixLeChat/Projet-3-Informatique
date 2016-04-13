//
//  LevelManager.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-04-04.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class LevelManager{
    
    static func getLevelName(hashID: String) -> String{
        return ""
    }
    
    static func getLevelHash(levelName : String) -> String{
      return ""
    }
    
    static func getLevelsByName() -> [String]{
        return []
    }
    
    static func canWriteLevel(levelName : String) -> Bool{
        return GameProperties.LevelName != levelName
    }
    
    // returns the name of levels that have changed or empty array
    static func updateMapsFromServer(contents : NSData){
        
        var json: Array<AnyObject>! = []
        
        do {
            json = try NSJSONSerialization.JSONObjectWithData(contents, options: NSJSONReadingOptions()) as? Array
        }
        catch {
            print(error)
        }
        
        
        if(json != nil){
            for var i:Int = 0; i < json.count; i++ {
                if let item = json[i] as? [String: AnyObject]{
                    // Si le hash de content diffère de celui que l'on a trouvé
                    let name = item["Name"] as! String
                    
                    // Si le niveau trouvé est le même que celui en train d'être modifié
                    if(name == GameProperties.LevelName){
                        let lastUpdateDate = HTTPLevelManager.getUpdateTimeFromServer(item["HashId"] as! String)
                        //print("Last update " + lastUpdateDate.toString())
                        //print("Now " + lastLoadTime!.toString())
                        if(lastUpdateDate.isBiggerThan(lastLoadTime!)){
                            print("Level on server was modified")
                            Notifications.callNotifications("Level on server was modified!", category: "Server Level")
                            GameProperties.wasModifiedByServer = true
                            setLoadTime(lastUpdateDate)
                        }
                    }
                }
            }
        }
    }
    
    static func setLoadTime(date: Utilities.Date){
        lastLoadTime = date
    }
    
    static func getAllPublicMapsOffline(){
        
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            
            let dataPathForModifiedOfflineLevels = dir.stringByAppendingPathComponent("Levels/Public/ModifiedOfflineLevels.txt")
            let fileManager = NSFileManager.defaultManager()
            
            // If the directory "ModifiedOfflineLevels" exists
            var isDir : ObjCBool = false
            if(fileManager.fileExistsAtPath(dataPathForModifiedOfflineLevels, isDirectory: &isDir)){
                var savedLevels = ""
                
                //reading
                do {
                    savedLevels = try String(contentsOfFile: dataPathForModifiedOfflineLevels)
                }
                catch {/* error handling here */
                    GameProperties.levelComesFromPublicOffline = false
                }
                
                if(savedLevels != ""){
                    let levels = savedLevels.characters.split("/")
                    for level in levels{
                        let dataPathForModifiedOfflineLevel = dir.stringByAppendingPathComponent("Levels/Public/" + String(level) + ".xml")
                        
                        var xmlString = ""
                        
                        //reading
                        do {
                            xmlString = try String(contentsOfFile: dataPathForModifiedOfflineLevel)
                        }
                        catch{
                        }
                        
                        if(xmlString != ""){
                            let pathHashToName = dir.stringByAppendingPathComponent("Levels/Public/MapHashIdToName.txt")
                            
                            var hashToName = ""
                            var hashToNameArray : [String : String] = [:]
                            
                            //reading
                            do {
                                hashToName = try String(contentsOfFile: pathHashToName, encoding: NSUTF8StringEncoding)
                            }
                            catch {/* error handling here */
                                print("Couldn't read from hashToName file")
                            }
                            
                            if(hashToName != ""){
                                
                                let separateLevels = hashToName.characters.split("/")
                                
                                for level in separateLevels{
                                    var hashToNameSplit = level.split(":")
                                    hashToNameArray[String(hashToNameSplit[hashToNameSplit.startIndex])] = String(hashToNameSplit[hashToNameSplit.startIndex.advancedBy(1)])
                                }
                            }
                            var nameToSave = ""
                            
                            for key in hashToNameArray.keys{
                                if(key == String(level)){
                                    nameToSave = hashToNameArray[key]!
                                }
                            }
                            
                            if (nameToSave != ""){
                                print("xmlString sent: " + xmlString)
                                HTTPLevelManager.updateLevel(nameToSave, xmlLevelString: xmlString, level: LevelLoader.getLevel(xmlString))
                            }
                        }
                    }
                    //writing
                    do {
                        try "".writeToFile(dataPathForModifiedOfflineLevels, atomically: false, encoding: NSUTF8StringEncoding)
                    }
                    catch {/* error handling here */
                        GameProperties.levelComesFromPublicOffline = false
                    }
                }
            }
        }
        
        let publicLevels = HTTPLevelManager.getAllPublicLevels()
        
        var mapHashIdToName : [String:String] = [:]
        for level in publicLevels{
            let name = level["Name"]
            let mapHashId = level["HashId"]
            let content = level["Content"]
            
            mapHashIdToName[mapHashId!] = name!
            
            if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
                
                let dataPathForLevels = dir.stringByAppendingPathComponent("Levels")
                let dataPathForPublic = dir.stringByAppendingPathComponent("Levels/Public")
                
                let fileManager = NSFileManager.defaultManager()
                
                // If the directory "Levels" doesn't exist, create it
                var isDir : ObjCBool = true
                if(!(fileManager.fileExistsAtPath(dataPathForLevels, isDirectory: &isDir))){
                    do {
                        try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForLevels, withIntermediateDirectories: false, attributes: nil)
                    } catch let error as NSError {
                        print(error.localizedDescription);
                    }
                }
                
                // If the directory "Levels/Public" doesn't exist, create it
                isDir = true
                if(!(fileManager.fileExistsAtPath(dataPathForPublic, isDirectory: &isDir))){
                    do {
                        try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForPublic, withIntermediateDirectories: false, attributes: nil)
                    } catch let error as NSError {
                        print(error.localizedDescription);
                    }
                }
                
                let file = "Levels/Public/" + mapHashId! + ".xml"
                
                let path = dir.stringByAppendingPathComponent(file)
                
                //writing
                do {
                    try content!.writeToFile(path, atomically: false, encoding: NSUTF8StringEncoding)
                }
                catch {/* error handling here */
                    print("Couldn't write a public level")
                }
            }
        }
        
        var stringToWrite = ""
        
        for key in mapHashIdToName.keys{
            stringToWrite += key + ":" + mapHashIdToName[key]! + "/"
        }
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            
            let file = "Levels/Public/MapHashIdToName.txt"
            
            let path = dir.stringByAppendingPathComponent(file)
            
            //writing
            do {
                try stringToWrite.writeToFile(path, atomically: false, encoding: NSUTF8StringEncoding)
            }
            catch {/* error handling here */
                print("Couldn't the mapHashId to name text file")
            }
        }
        
        
    }

    /*
    static func saveAllUserMapsLocally(levelsJSON: Array<AnyObject>){
            
        createUpdateTimeFolderIfNotExists()
        
        let userUpdateTimeExists = doesUpdateTimeFileExistsForUser()
        
        createLevelsFolderIfNotExists()
        
        var userUpdateTime : Dictionary<String, Utilities.Date> = [:]
        
        // Go through all the levels from the server
        for var i:Int = 0; i < levelsJSON.count; i++ {
            if let item = levelsJSON[i] as? [String: AnyObject]{
                let name = item["Name"] as! String
                let mapHashId = item["HashId"] as! String
                let serverUpdateTime = Utilities.Date(timeString: item["UpdateTime"] as! String)
                
                if(userUpdateTimeExists){
                    let jsonFromUpdateTimeFile = getJSONForUpdateTime()
                    
                    
                    // Si le updateTime du serveur est plus récent
                    if (serverUpdateTime.isBiggerThan(jsonFromUpdateTimeFile[mapHashId]!)){
                        updateFileFromServer(mapHashId, name : name)
                        
                        userUpdateTime[mapHashId] = serverUpdateTime
                    }
                }
                else{
                    updateFileFromServer(mapHashId, name : name)
                    
                    userUpdateTime[mapHashId] = serverUpdateTime
                }
            }
        }
        
        writeNewTimeToUpdateTimeFile(userUpdateTime)
    }
    
    private static func updateFileFromServer(mapHashId : String, name : String){
        let xmlString = HTTPLevelManager.getLevelContent(mapHashId)
        
        writeLevelInItsFile(name, xmlString: xmlString)
    }
    
    
    private static func doesUpdateTimeFileExistsForUser() -> Bool{
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            
            let dataPathForUpdateTime = dir.stringByAppendingPathComponent("updateTime/" + WebSession.userHashId! + ".json")
            let fileManager = NSFileManager.defaultManager()
            
            //Let's look if the lastUpdateTime of every level file exists for user
            var isDir : ObjCBool = false
            if(!(fileManager.fileExistsAtPath(dataPathForUpdateTime, isDirectory: &isDir))){
                return true
            }
            return false
        }
        return false
    }
    
    private static func createLevelsFolderIfNotExists(){
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            let dataPathForLevelContent = dir.stringByAppendingPathComponent("Levels/" + WebSession.userHashId!)
            let fileManager = NSFileManager.defaultManager()
            
            // If the directory "Levels/[userHashId]" doesn't exist, create it
            var isDir : ObjCBool = true
            if(!(fileManager.fileExistsAtPath(dataPathForLevelContent, isDirectory: &isDir))){
                do {
                    try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForLevelContent, withIntermediateDirectories: false, attributes: nil)
                } catch let error as NSError {
                    print(error.localizedDescription);
                }
            }
        }
    }
    
    private static func createUpdateTimeFolderIfNotExists(){
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            
            let dataPathForUpdateTimeFolder = dir.stringByAppendingPathComponent("updateTime")
            let fileManager = NSFileManager.defaultManager()
            
            // If the directory "updateTime" doesn't exist, create it
            var isDir : ObjCBool = true
            if(!(fileManager.fileExistsAtPath(dataPathForUpdateTimeFolder, isDirectory: &isDir))){
                do {
                    try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForUpdateTimeFolder, withIntermediateDirectories: false, attributes: nil)
                } catch let error as NSError {
                    print(error.localizedDescription);
                }
            }
        }
    }
    
    private static func writeLevelInItsFile(levelName : String, xmlString : String){
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            let file = "Levels/" + WebSession.userHashId! + "/" + levelName + ".xml"
            let path = dir.stringByAppendingPathComponent(file)
            
            //writing
            do {                
                try xmlString.writeToFile(path, atomically: false, encoding: NSUTF8StringEncoding)
            }
            catch {/* error handling here */
                print("Writing level " + levelName + " failed")
            }
        }

    }
    
    private static func getJSONForUpdateTime() -> [String:Utilities.Date]{
        var updateTime : [String:Utilities.Date] = [:]
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            
            let dataPathForUpdateTime = dir.stringByAppendingPathComponent("updateTime/" + WebSession.userHashId! + ".json")
        
            let jsonData = NSData(contentsOfFile:dataPathForUpdateTime)
            
            do{
                if let jsonDict = try NSJSONSerialization.JSONObjectWithData(jsonData!, options : NSJSONReadingOptions.MutableContainers) as? NSDictionary{
                    for key in jsonDict.allKeys{
                        updateTime[key as! String] = Utilities.Date(timeString: jsonDict[key as! String] as! String)
                    }
                }
            }
            catch{
                print("Cannot read from update time file")
            }
        }
        
        return updateTime
    }
    
    private static func writeNewTimeToUpdateTimeFile(mapHashIdToDateTime : Dictionary<String, Utilities.Date>){
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            let dataPathForUpdateTime = dir.stringByAppendingPathComponent("updateTime/" + WebSession.userHashId! + ".json")
            
            var jsonTimeString : [String:String] = [:]
            
            for key in mapHashIdToDateTime.keys{
                jsonTimeString[key] = mapHashIdToDateTime[key]?.toString()
            }
            
            let json = Utilities.buildJSONString(jsonTimeString)
            
            do {
                try json.jsonString?.writeToFile(dataPathForUpdateTime, atomically: false, encoding: NSUTF8StringEncoding)
            }
            catch {
                print("Writing update time failed")
            }
        }
    }*/
    
    private static var lastLoadTime : Utilities.Date?
    
}

/*
Update Zone:
Http Post - api/zones/update
Level(int), Content(string), HashId(string)

get my levels :
Http Get - api/zones/all avec token
return List MapModel :
public int Id { get; set; }
public string Name { get; set; }
public string CreatorhashId { get; set; }
public string Content { get; set; }
public int Level { get; set; }
public string HashId { get; set; }
public string CreationDate { get; set; }
public DateTime UpdateTime { get; set; }

new Map :
Http Post - api/zones/new
Besoin:
Name (string), Content (string), Level(int)
*/