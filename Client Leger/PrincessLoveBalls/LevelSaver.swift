//
//  LevelSaver.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

// TODO

// Envoyer le fichier sur le serveur

import Foundation

class LevelSaver {
    
    static func saveLevel(spriteContainer: SpriteContainer, sendToServer: Bool) -> String{
        if(!isLevelValid(spriteContainer)){
            return "_______________INVALID_LEVEL____________"
        }
        
        // Save properties
        
        let xmlString = getXMLStringForContainer(spriteContainer)
        
        //print(xmlString)
        saveLevel(GameProperties.LevelName, xmlString: xmlString, level : GameProperties.levelLevel, sendToServer: sendToServer)
        
        return xmlString
    }
    
    static func saveLevel(levelName : String, xmlString : String, level : Int, sendToServer: Bool){
        
        var file = ""
        
        // if connected, save to user folder and save to server
        if(WebSession.isConnected){
            file = "Levels/" + WebSession.userHashId! + "/" + GameProperties.LevelName + ".xml"
            
            if(GameProperties.wasLoadedFromServer && GameProperties.updateServer){
                HTTPLevelManager.updateLevel(levelName, xmlLevelString : xmlString, level : level)
                GameProperties.wasLoadedFromServer = false
                GameProperties.updateServer = false
            }
            else{
                HTTPLevelManager.saveLevel(levelName, xmlLevelString : xmlString, level : level)
            }
        }
        // If not connected, save to anonymous folder
        else{
            if(GameProperties.levelComesFromPublicOffline){
                GameProperties.levelComesFromPublicOffline = false
                
                if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
                    
                    let dataPathForLevels = dir.stringByAppendingPathComponent("Levels")
                    let dataPathForPublic = dir.stringByAppendingPathComponent("Levels/Public")
                    let dataPathForModifiedOfflineLevels = dir.stringByAppendingPathComponent("Levels/Public/ModifiedOfflineLevels.txt")
                    
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
                    var keyToSave = ""
                    
                    for key in hashToNameArray.keys{
                        if(hashToNameArray[key] == GameProperties.LevelName){
                            keyToSave = key
                        }
                    }
                    
                    if(keyToSave != ""){
                        let keyFilename = keyToSave
                        keyToSave += "/"
                        
                        // If the directory "ModifiedOfflineLevels" exists
                        isDir = false
                        if(fileManager.fileExistsAtPath(dataPathForModifiedOfflineLevels, isDirectory: &isDir)){
                            
                            var savedLevels = ""
                            
                            //reading
                            do {
                                savedLevels = try String(contentsOfFile: dataPathForModifiedOfflineLevels)
                            }
                            catch {/* error handling here */
                                GameProperties.levelComesFromPublicOffline = false
                            }
                            
                            if(savedLevels == ""){
                                //writing
                                do {
                                    try keyToSave.writeToFile(dataPathForModifiedOfflineLevels, atomically: false, encoding: NSUTF8StringEncoding)
                                }
                                catch {/* error handling here */
                                    GameProperties.levelComesFromPublicOffline = false
                                }
                            }
                            else{
                                savedLevels += keyToSave
                                
                                //writing
                                do {
                                    try savedLevels.writeToFile(dataPathForModifiedOfflineLevels, atomically: false, encoding: NSUTF8StringEncoding)
                                }
                                catch {/* error handling here */
                                    GameProperties.levelComesFromPublicOffline = false
                                }
                            }
                            
                        }
                        else{
                                //writing
                                do {
                                    try keyToSave.writeToFile(dataPathForModifiedOfflineLevels, atomically: false, encoding: NSUTF8StringEncoding)
                                }
                                catch {/* error handling here */
                                    GameProperties.levelComesFromPublicOffline = false
                                }
                        }
                        let dataPathForPublicOfflineMap = dir.stringByAppendingPathComponent("Levels/Public/" + keyFilename + ".xml")
                        
                        //writing
                        do {
                            try xmlString.writeToFile(dataPathForPublicOfflineMap, atomically: false, encoding: NSUTF8StringEncoding)
                        }
                        catch {/* error handling here */
                            GameProperties.levelComesFromPublicOffline = false
                            print("Could not update file :" + keyFilename + ".xml")
                        }
                    }
                }
            }
            else{
                file = "Levels/Anonymous/" + GameProperties.LevelName + ".xml"
                savePNGImageForAnonymous(GameProperties.LevelName)
            }
        }
        
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            
            let dataPathForLevels = dir.stringByAppendingPathComponent("Levels")
            let dataPathForAnonymous = dir.stringByAppendingPathComponent("Levels/Anonymous")
            
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
            
            // If the directory "Anonymous" doesn't exist, create it
            isDir = true
            if(!(fileManager.fileExistsAtPath(dataPathForAnonymous, isDirectory: &isDir))){
                do {
                    try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForAnonymous, withIntermediateDirectories: false, attributes: nil)
                } catch let error as NSError {
                    print(error.localizedDescription);
                }
            }
            
            let path = dir.stringByAppendingPathComponent(file)
            
            //writing
            do {
            try xmlString.writeToFile(path, atomically: false, encoding: NSUTF8StringEncoding)
            }
            catch {/* error handling here */}
        }
    }
    
    private static func savePNGImageForAnonymous(levelName : String){
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
            let fileManager = NSFileManager.defaultManager()
            
            let dataPathForlevels = dir.stringByAppendingPathComponent("Levels")
            
            // If the directory "Levels" doesn't exist, create it
            var isDir : ObjCBool = true
            if(!(fileManager.fileExistsAtPath(dataPathForlevels, isDirectory: &isDir))){
                do {
                    try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForlevels, withIntermediateDirectories: false, attributes: nil)
                } catch let error as NSError {
                    print(error.localizedDescription);
                }
            }
            
            let dataPathForAnonymous = dir.stringByAppendingPathComponent("Levels/Anonymous")
            
            // If the directory "Levels/Anonymous" doesn't exist, create it
            isDir = true
            if(!(fileManager.fileExistsAtPath(dataPathForAnonymous, isDirectory: &isDir))){
                do {
                    try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForAnonymous, withIntermediateDirectories: false, attributes: nil)
                } catch let error as NSError {
                    print(error.localizedDescription);
                }
            }
            
            let dataPathForAnonymousImages = dir.stringByAppendingPathComponent("Levels/Anonymous/Images")
            
            // If the directory "Levels/Anonymous/Images" doesn't exist, create it
            isDir = true
            if(!(fileManager.fileExistsAtPath(dataPathForAnonymousImages, isDirectory: &isDir))){
                do {
                    try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForAnonymousImages, withIntermediateDirectories: false, attributes: nil)
                } catch let error as NSError {
                    print(error.localizedDescription);
                }
            }
            
            let dataPathForAnonymousImage = dir.stringByAppendingPathComponent("Levels/Anonymous/Images/" + levelName + ".png")
            
            if let data = UIImagePNGRepresentation(imageLevelAnonymous!) {
                data.writeToFile(dataPathForAnonymousImage, atomically: true)
            }
        }
    }
    
    
    static func getXMLStringForContainer(spriteContainer: SpriteContainer) -> String{
        _portailsCouverts = []
        _nombreObjetEnlevé = 0
        
        let levelSaved = AEXMLDocument()
        let racine = levelSaved.addChild(name: "Racine")
        let properties = racine.addChild(name: "Proprietes")
        properties.addChild(name: "PointButoirCercle", value: String(GameProperties.PointButoirCercle))
        properties.addChild(name: "PointButoirTriangle", value: String(GameProperties.PointButoirTriangle))
        properties.addChild(name: "PointCible", value: String(GameProperties.PointCible))
        properties.addChild(name: "PointCampagne", value: String(GameProperties.PointCampagne))
        properties.addChild(name: "PointBilleGratuite", value: String(GameProperties.PointBilleGratuite))
        properties.addChild(name: "Difficulte", value: String(GameProperties.Difficulte))
        
        // Save all individual objects
        let sprites = spriteContainer.getSprites()
        
        let listeObjets = racine.addChild(name: "ListeObjets", attributes: ["NbObjets":String(sprites.count)])
        
        GameProperties.levelLevel = 0
        
        for sprite in sprites{
            saveObject(sprite, racine: listeObjets, spriteContainer: spriteContainer)
            if(sprite.getLevel() > GameProperties.levelLevel){
            }
        }
        
        racine["ListeObjets"].attributes = ["NbObjets":String(sprites.count - _nombreObjetEnlevé)]
        
        var xmlString = levelSaved.xmlString.stringByReplacingOccurrencesOfString("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>", withString: "")
        xmlString.removeAtIndex(xmlString.startIndex)
        return xmlString
    }
    
    private static func saveObject(sprite: Sprite, racine: AEXMLElement, spriteContainer: SpriteContainer){
        if let mur = sprite as? Mur{
            let objet = racine.addChild(name: "Objet")
            objet.addChild(name: "Type", value: mur.name?.componentsSeparatedByString("_")[0])
            
            let clientLourdPos = Utilities.posiOStoClientLourd(mur.position)
            
            objet.addChild(name: "Position", attributes: ["X" : String(clientLourdPos.x), "Y" : String(clientLourdPos.y), "Z" : "0"])
            objet.addChild(name: "Rotation", value: String(Utilities.radToDeg(mur.zRotation + CGFloat(M_PI/2))))
            objet.addChild(name: "Agrandissement", attributes: ["X" : "1", "Y" : "1", "Z" : "1"])
            objet.addChild(name: "LongueurMur", value : String(Utilities.lengthMuriOStoClientLourd(mur._scale)))
        }
        else if let champForce = sprite as? ChampForce{
            let objet = racine.addChild(name: "Objet")
            
            let clientLourdPos = Utilities.posiOStoClientLourd(champForce.position)
            
            objet.addChild(name: "Type", value: champForce.name?.componentsSeparatedByString("_")[0])
            objet.addChild(name: "Position", attributes: ["X" : String(clientLourdPos.x), "Y" : String(clientLourdPos.y), "Z" : "0"])
            objet.addChild(name: "Rotation", value: String(Utilities.radToDeg(champForce._arrow.zRotation)))
            objet.addChild(name: "Agrandissement", attributes: ["X" : String(champForce._scale), "Y" : String(champForce._scale), "Z" : String(champForce._scale)])
        }
        else if let portal = sprite as? Portal{
            if(portal.getPairedPortal() != ""){
                if(!_portailsCouverts.contains(portal.name!) && !_portailsCouverts.contains(portal.getPairedPortal())){
                    let objet = racine.addChild(name: "Objet")
                    
                    let clientLourdPos = Utilities.posiOStoClientLourd(portal.position)
                    
                    objet.addChild(name: "Type", value: "portail")
                    objet.addChild(name: "Position", attributes: ["X" : String(clientLourdPos.x), "Y" : String(clientLourdPos.y), "Z" : "0"])
                    objet.addChild(name: "Rotation", value: String(Utilities.radToDeg(portal.zRotation)))
                    objet.addChild(name: "Agrandissement", attributes: ["X" : String(portal._scale), "Y" : String(portal._scale), "Z" : String(portal._scale)])
                
                    let frere = objet.addChild(name: "Frere")
                    
                    let clientLourdPosPaired = Utilities.posiOStoClientLourd(spriteContainer.getSpriteByName(portal.getPairedPortal())!.position)
                    
                    frere.addChild(name: "Position", attributes: ["X" : String(clientLourdPosPaired.x), "Y" : String(clientLourdPosPaired.y), "Z" : "0"])
                    frere.addChild(name: "Rotation", value: String(Utilities.radToDeg(spriteContainer.getSpriteByName(portal.getPairedPortal())!.zRotation)))
                    let scale = spriteContainer.getSpriteByName(portal.getPairedPortal())!._scale
                    frere.addChild(name: "Agrandissement", attributes: ["X" : String(scale), "Y" : String(scale), "Z" : String(scale)])
                
                    _portailsCouverts.append(portal.name!)
                    _portailsCouverts.append(portal.getPairedPortal())

                }
                
            }
            else
            {
                _nombreObjetEnlevé++
            }
        } else {
            let objet = racine.addChild(name: "Objet")
            
            let clientLourdPos = Utilities.posiOStoClientLourd(sprite.position)
            
            objet.addChild(name: "Type", value: sprite.name?.componentsSeparatedByString("_")[0])
            objet.addChild(name: "Position", attributes: ["X" : String(clientLourdPos.x), "Y" : String(clientLourdPos.y), "Z" : "0"])
            objet.addChild(name: "Rotation", value: String(Utilities.radToDeg(sprite.zRotation)))
            objet.addChild(name: "Agrandissement", attributes: ["X" : String(sprite._scale), "Y" : String(sprite._scale), "Z" : String(sprite._scale)])
        }
    }
    
    static func isLevelValid(spriteContainer: SpriteContainer) -> Bool{
        var trou = false
        var ressort = false
        var generateur = false
        for sprite in spriteContainer.getSprites(){
            if sprite.dynamicType == Trou.self{
                trou = true
            }
            
            if sprite.dynamicType == Ressort.self{
                ressort = true
            }
            
            if sprite.dynamicType == GenerateurDeBille.self{
                generateur = true
            }
        }
        
        return (trou && generateur && ressort)
    }
    
    static var imageLevelAnonymous : UIImage?
    
    private static var _portailsCouverts: [String] = []
    private static var _nombreObjetEnlevé = 0
}

/*
<PointButoirCercle>10</PointButoirCercle>
<PointButoirTriangle>20</PointButoirTriangle>
<PointCible>20</PointCible>
<PointCampagne>100</PointCampagne>
<PointBilleGratuite>1000</PointBilleGratuite>
<Difficulte>1</Difficulte>
*/