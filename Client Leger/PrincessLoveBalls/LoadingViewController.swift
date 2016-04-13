//
//  LoadingViewController.swift
//  PrincessLoveBalls
//
//  Created by Guillaume Lavoie-Harvey on 2016-03-17.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit

class LoadingViewController: UIViewController, iCarouselDataSource, iCarouselDelegate
{
    
    private var namesList: [String] = []
    private var mapHashes: [String] = []
    
    @IBOutlet var carousel: iCarousel!
    
    override func awakeFromNib()
    {
        
        super.awakeFromNib()
        
        // If you're connected, get the levels from online
        if(WebSession.isConnected){
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
                
                let dataPathForPublic = dir.stringByAppendingPathComponent("Levels/Public")
                
                // If the directory "Levels/Public" doesn't exist, create it
                isDir = true
                if(!(fileManager.fileExistsAtPath(dataPathForPublic, isDirectory: &isDir))){
                    do {
                        try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForPublic, withIntermediateDirectories: false, attributes: nil)
                    } catch let error as NSError {
                        print(error.localizedDescription);
                    }
                }
            }
            
            
            self.getData()
        }
            // If not, get them locally
        else{
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
                
                let dataPathForPublic = dir.stringByAppendingPathComponent("Levels/Public")
                
                // If the directory "Levels/Public" doesn't exist, create it
                isDir = true
                if(!(fileManager.fileExistsAtPath(dataPathForPublic, isDirectory: &isDir))){
                    do {
                        try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForPublic, withIntermediateDirectories: false, attributes: nil)
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
                
                let dataPathAnonymous = dir.stringByAppendingPathComponent("Levels/Anonymous")
                
                let enumeratorAnon :NSDirectoryEnumerator = fileManager.enumeratorAtPath(dataPathAnonymous)!
                
                while let element = enumeratorAnon.nextObject() as? NSString {
                    if element.pathExtension == "xml" {
                        levelListJson.append(["Name":element.stringByDeletingPathExtension, "HashId" : ""])
                    }
                }
                
                let dataPathPublic = dir.stringByAppendingPathComponent("Levels/Public")
                
                let enumeratorPub :NSDirectoryEnumerator = fileManager.enumeratorAtPath(dataPathPublic)!
                
                while let element = enumeratorPub.nextObject() as? NSString {
                    
                    let pathHashToName = dir.stringByAppendingPathComponent("Levels/Public/MapHashIdToName.txt")
                    
                    var hashToName = ""
                    
                    //reading
                    do {
                        hashToName = try String(contentsOfFile: pathHashToName, encoding: NSUTF8StringEncoding)
                    }
                    catch {/* error handling here */
                        print("Couldn't read from hashToName file")
                    }
                    
                    if(hashToName != ""){
                        
                        var hashToNameArray : [String : String] = [:]
                        
                        let separateLevels = hashToName.characters.split("/")
                        
                        for level in separateLevels{
                            var hashToNameSplit = level.split(":")
                            hashToNameArray[String(hashToNameSplit[hashToNameSplit.startIndex])] = String(hashToNameSplit[hashToNameSplit.startIndex.advancedBy(1)])
                        }
                        
                        if hashToNameArray[element.stringByDeletingPathExtension] != nil{
                            let name = hashToNameArray[element.stringByDeletingPathExtension]! as String
                            if element.pathExtension == "xml" {
                                levelListJson.append(["Name":name, "HashId" : ""])
                            }
                        }
                    }
                }
            }
        }
        
        for var i:Int = 0; i < levelListJson.count; i++ {
            if let item = levelListJson[i] as? [String: AnyObject]{
                //print(item)
                let name = item["Name"]
                namesList.append(name! as! String)
                let mapHash = item["HashId"]
                mapHashes.append(mapHash! as! String)
                
            }
        }
    
    }
    
    func getData(){
        
        var gotInvalidData = true
        repeat{
            repeat{
                var levelsJSON:NSData = NSData()
                
                levelsJSON = HTTPLevelManager.getLevelList()
                
                do {                   
                    let json = try NSJSONSerialization.JSONObjectWithData(levelsJSON, options: NSJSONReadingOptions())
                    guard let jsonWTF = json as? Array<AnyObject>
                        else{
                            print("Invalid data, please check connection. Check if client lourd is connected on the same profile.")
                            break
                    }
                    levelListJson = jsonWTF
                    gotInvalidData = false
                } catch {
                    print(error)
                }
            } while(gotInvalidData)
        } while(gotInvalidData)
    }
    
    override func viewDidLoad()
    
    {
        super.viewDidLoad()
        
        carousel.type = .TimeMachine
        carousel.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        carousel.layer.zPosition = -2
        
        transparentThing.layer.zPosition = -1
        transparentThing.layer.borderWidth = 4
        transparentThing.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentThing.layer.cornerRadius = 20
        
        // If you're connected, get the levels from online
        if(WebSession.isConnected){
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
                
                let dataPathForPublic = dir.stringByAppendingPathComponent("Levels/Public")
                
                // If the directory "Levels/Public" doesn't exist, create it
                isDir = true
                if(!(fileManager.fileExistsAtPath(dataPathForPublic, isDirectory: &isDir))){
                    do {
                        try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForPublic, withIntermediateDirectories: false, attributes: nil)
                    } catch let error as NSError {
                        print(error.localizedDescription);
                    }
                }
            }
            
            self.getData()
        }
            // If not, get them locally
        else{
            if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
                let dataPathForlevels = dir.stringByAppendingPathComponent("Levels")
                
                let fileManager = NSFileManager.defaultManager()
                
                // If the directory "Levels" doesn't exist, create it
                var isDir : ObjCBool = true
                if(!(fileManager.fileExistsAtPath(dataPathForlevels, isDirectory: &isDir))){
                    do {
                        try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForlevels, withIntermediateDirectories: false, attributes: nil)
                    } catch let error as NSError {
                        print(error.localizedDescription);
                    }
                }
                
                
                let dataPathForPublic = dir.stringByAppendingPathComponent("Levels/Public")
                
                // If the directory "Levels/Public" doesn't exist, create it
                isDir = true
                if(!(fileManager.fileExistsAtPath(dataPathForPublic, isDirectory: &isDir))){
                    do {
                        try NSFileManager.defaultManager().createDirectoryAtPath(dataPathForPublic, withIntermediateDirectories: false, attributes: nil)
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
                
                let dataPath = dir.stringByAppendingPathComponent("Levels/Anonymous")
                
                let enumerator:NSDirectoryEnumerator = fileManager.enumeratorAtPath(dataPath)!
                
                while let element = enumerator.nextObject() as? NSString {
                    if element.pathExtension == "xml" {
                        let name = element.stringByDeletingPathExtension
                        levelListJson.append(["Name":name , "HashId" : ""])
                    }
                }
            }
        }
        
        for var i:Int = 0; i < levelListJson.count; i++ {
            if let item = levelListJson[i] as? [String: AnyObject]{
                //print(item)
                let name = item["Name"]
                namesList.append(name! as! String)
                let mapHash = item["HashId"]
                mapHashes.append(mapHash! as! String)
                
            }
        }

    }
    
    func numberOfItemsInCarousel(carousel: iCarousel) -> Int
    {
        return namesList.count
    }
    
    func carousel(carousel: iCarousel, viewForItemAtIndex index: Int, reusingView view: UIView?) -> UIView
    {
        var label: UILabel
        var itemView: UIImageView
        
        //create new view if no view is available for recycling
        if (view == nil)
        {
            //don't do anything specific to the index within
            //this `if (view == nil) {...}` statement because the view will be
            //recycled and used with other index values later
            itemView = UIImageView(frame:CGRect(x:0, y:0, width:384, height:683))
            itemView.image = getLevelImage(index)
            itemView.contentMode = .ScaleToFill
            //itemView.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
            
            label = UILabel(frame:itemView.bounds)
            label.backgroundColor = UIColor.clearColor()
            label.textColor = UIColor.whiteColor()
            label.textAlignment = .Center
            label.font = label.font.fontWithSize(50)
            label.tag = 1
            itemView.addSubview(label)
        }
        else
        {
            //get a reference to the label in the recycled view
            itemView = view as! UIImageView;
            label = itemView.viewWithTag(1) as! UILabel!
        }
        
        //set item label
        //remember to always set any properties of your carousel item
        //views outside of the `if (view == nil) {...}` check otherwise
        //you'll get weird issues with carousel item content appearing
        //in the wrong place in the carousel
        /*if(WebSession.isConnected){
            label.text = "\(namesList[index])"
        }
        else{
            var levelName = "\(namesList[index])"
            levelName.removeRange(Range(start: levelName.startIndex, end: levelName.startIndex.advancedBy(10)))
            label.text = levelName
        }*/
        label.text = "\(namesList[index])"
        
        return itemView
    }
    
    func carousel(carousel: iCarousel, valueForOption option: iCarouselOption, withDefault value: CGFloat) -> CGFloat
    {
        if (option == .Spacing)
        {
            return value * 1.1
        }
        return value
    }
    
    func getLevelImage(index:Int)->UIImage{
        if (WebSession.isConnected){
            let mapHash:String = mapHashes[index]
            
            let data = HTTPLevelManager.getLevelImage(mapHash)
            
            var dataString = String(data: data, encoding: NSUTF8StringEncoding)
            
            if(dataString != nil){
                dataString = dataString!.stringByReplacingOccurrencesOfString("\"", withString: "", options: NSStringCompareOptions.LiteralSearch, range: nil)
                let decodedData = NSData(base64EncodedString: String(dataString!), options: NSDataBase64DecodingOptions(rawValue: 0))
                //print(decodedData)
                let decodedimage = UIImage(data: decodedData!)
                return decodedimage! as UIImage
            } else {
                
            }
        }
        else{
            let levelNameToLoad = namesList[index]
            //levelNameToLoad.removeRange(Range(start: levelNameToLoad.startIndex, end: levelNameToLoad.startIndex.advancedBy(10)))
            
            if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.UserDomainMask, true).first {
                let dataPathForAnonymousImage = dir.stringByAppendingPathComponent("Levels/Anonymous/Images/" + levelNameToLoad + ".png")
                
                if let image = UIImage(contentsOfFile: dataPathForAnonymousImage){
                    return image
                }
                
            }
        }
        return UIImage(named: "black")!
    }
    
    @IBAction func loadButtonPressed(sender: UIButton) {
        
        if self.carousel.visibleItemViews.count > 0{
            let view = self.carousel.currentItemView
            let nameOfLevel = (view!.subviews[0] as! UILabel).text
            levelNameToLoad = nameOfLevel
            
            //print("Level to load: ", nameOfLevel)
            if(WebSession.isConnected){
                for var i:Int = 0; i < levelListJson.count; i++ {
                    if let item = levelListJson[i] as? [String: AnyObject]{
                        if nameOfLevel == (item["Name"] as! String){
                            levelToLoad = HTTPLevelManager.getLevelContent((item["HashId"] as? String)!)
                            if( levelToLoad != ""){                            
                                performSegueWithIdentifier("loadLevel", sender: self)
                                //print(levelToLoad)
                            }
                            else{
                                let alert = UIAlertController(title: "Erreur de connexion", message: "Impossible de récupérer les données de la carte", preferredStyle: UIAlertControllerStyle.Alert)
                                let okButton = UIAlertAction(title: "Ok", style: .Default) { (alert: UIAlertAction!) -> Void in
                                    // Do something after pushing button
                                }
                                alert.addAction(okButton)
                                presentViewController(alert, animated: true, completion: nil)
                            }
                        }
                    }
                }
            }
            else{
                if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.AllDomainsMask, true).first {
                    
                    var file = "Levels/Anonymous/" + levelNameToLoad! + ".xml"
                    var path = dir.stringByAppendingPathComponent(file)
                    
                    do {
                        levelToLoad = try NSString(contentsOfFile: path, encoding: NSUTF8StringEncoding) as String
                        if(levelToLoad != ""){
                            performSegueWithIdentifier("loadLevel", sender: self)
                        }
                    }
                    catch {
                        do{
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
                            
                            var hashName = ""
                            
                            
                            for key in hashToNameArray.keys{
                                if(hashToNameArray[key] == levelNameToLoad){
                                    hashName = key
                                }
                            }
                            
                            file = "Levels/Public/" + hashName + ".xml"
                            path = dir.stringByAppendingPathComponent(file)
                            levelToLoad = try NSString(contentsOfFile: path, encoding: NSUTF8StringEncoding) as String
                            if(levelToLoad != ""){
                                GameProperties.levelComesFromPublicOffline = true
                                performSegueWithIdentifier("loadLevel", sender: self)
                            }
                        }
                        catch{
                            print("Could not read level " + levelNameToLoad!)
                        }
                    }
                }
            }
        }
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject!) {
        if (segue.identifier == "loadLevel") {
            let gvc = segue.destinationViewController as! GameViewController
            
            GameProperties.wasLoadedFromServer = true
            gvc.xmlLevelString = levelToLoad
            gvc.levelName = levelNameToLoad
        }
    }
    
    private var levelListJson : Array<AnyObject> = []
    private var levelToLoad : String?
    private var levelNameToLoad : String?
    @IBOutlet var transparentThing: UIView!
}

