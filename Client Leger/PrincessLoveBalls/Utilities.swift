//
//  Utilities.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-10.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import UIKit
import SystemConfiguration

public class Utilities{
    
    static let textEditorWidth : CGFloat = 519
    static let textEditorHeight : CGFloat = 922
    
    static func posiOStoClientLourd(pos:CGPoint) -> CGPoint{
        var newPos = CGPoint(x: 0,y: 0)
        
        newPos.x = pos.x*(90/textEditorWidth)
        newPos.y = pos.y*(160/textEditorHeight)
        
        return newPos
    }
    
    static func posClientLourdToiOS(pos:CGPoint) -> CGPoint{
        var newPos = CGPoint(x: 0,y: 0)
        
        newPos.x = pos.x*(textEditorWidth/90)
        newPos.y = pos.y*(textEditorHeight/160)
        
        return newPos
    }
    
    static func lengthMuriOStoClientLourd(length : CGFloat) -> CGFloat{
        return (length/textEditorWidth)*90.0/8
    }
    
    static func lengthMurClientLourdToiOS(length : CGFloat) -> CGFloat{
        return (length/90.0)*textEditorWidth*8
    }
    
    static func degToRad(angle:CGFloat) -> CGFloat{
        return (angle/360.0)*2.0*CGFloat(M_PI)
    }
    
    static func radToDeg(angle:CGFloat) -> CGFloat{
        return angle * CGFloat(360.0) / CGFloat(2*M_PI)
    }
    /*
    let string = "[ {\"name\": \"John\", \"age\": 21}, {\"name\": \"Bob\", \"age\": 35} ]"
    
    func JSONParseArray(string: String) -> [AnyObject]{
        if let data = string.dataUsingEncoding(NSUTF8StringEncoding){
            
            do{
                
                if let array = try NSJSONSerialization.JSONObjectWithData(data, options: NSJSONReadingOptions.MutableContainers)  as? [AnyObject] {
                    return array
                }
            }catch{
                
                print("error"
                //handle errors here
                
            }
        }
        return [AnyObject]()
    }
    
    
    
    for element: AnyObject in JSONParseArray(string) {
    let name = element["name"] as? String
    let age = element["age"] as? Int
    print("Name: \(name), Age: \(age)")
    }*/
    
    static func getJSONStringFromString(jsonMessage : String) -> [String : String]{
        var json : [String: String] = [:]
        
        if let data = jsonMessage.dataUsingEncoding(NSUTF8StringEncoding){
            
            do{
                
                let object = try NSJSONSerialization.JSONObjectWithData(data, options: .AllowFragments)
                if let dictionary = object as? [String: AnyObject] {
                    for key in dictionary.keys{
                        json[key] = dictionary[key] as? String
                    }
                }
            }catch{
                print("error")
            }
        }
        
        return json
    }
    
    // Can only be built using Utilities.buildJSONString
    class JSONString{
        private init(string:String){
            self.jsonString = string
        }
        
        var jsonString : String?
    }
    
    static func buildJSONString(json : [String:String]) -> JSONString{
        var jsonString = "{"
        if(json.keys.count != 0){
            for property in json.keys{
                jsonString += "\"" + property + "\" : "
                jsonString += "\"" + json[property]! + "\","
            }
            
            jsonString.removeAtIndex(jsonString.endIndex.predecessor())
            jsonString += "}"
        }
        else{
            //print("JSON string built :", "{\"\" : \"\"}")
            return JSONString(string : "{\"\" : \"\"}")
        }
        //print("JSON string built :", jsonString)
        return JSONString(string: jsonString)
    }
    
    static func backgroundThread(delay: Double = 0.0, background: (() -> Void)? = nil, completion: (() -> Void)? = nil) {
        dispatch_async(dispatch_get_global_queue(Int(QOS_CLASS_USER_INITIATED.rawValue), 0)) {
            if(background != nil){ background!(); }
            
            let popTime = dispatch_time(DISPATCH_TIME_NOW, Int64(delay * Double(NSEC_PER_SEC)))
            dispatch_after(popTime, dispatch_get_main_queue()) {
                if(completion != nil){ completion!(); }
            }
        }
    }
    
    static func writeToFile(filename: String, content: String) -> Bool{
        
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.AllDomainsMask, true).first {
            let path = dir.stringByAppendingPathComponent(filename);
            
            //writing
            do {
                try content.writeToFile(path, atomically: false, encoding: NSUTF8StringEncoding)
            }
            catch {
                /* error handling here */
                return false
            }
            return true
        }
        return false
    }
    
    static func readFromFile(filename : String) -> String{
        
        if let dir : NSString = NSSearchPathForDirectoriesInDomains(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomainMask.AllDomainsMask, true).first {
            let path = dir.stringByAppendingPathComponent(filename);

            do {
                let content = try String(contentsOfURL: NSURL(fileURLWithPath: path), encoding: NSUTF8StringEncoding)
                return content
            } catch{
                print("error loading from url \(path)")
                return ""
            }
        }
        return ""
    }
    
    static func NSDataToArrayAnyObject(data : NSData) -> Array<AnyObject>{
        var array : Array<AnyObject> = []
        
        do {
            let json = try NSJSONSerialization.JSONObjectWithData(data, options: NSJSONReadingOptions())
            guard let jsonWTF = json as? Array<AnyObject>
                else{
                    print("Invalid data, please check connection. Check if client lourd is connected on the same profile.")
                    return array
            }
            array = jsonWTF
        } catch {
            print(error)
        }
        
        return array
    }
    
    class Date{
        
        // Must conform to the following format:
        // yyyy-MM-dd/hh:mm:ss
        init(timeString : String){
            self.year = Int(timeString.substringWithRange(Range<String.Index>(start: timeString.startIndex, end: timeString.startIndex.advancedBy(4))))
            self.month = Int(timeString.substringWithRange(Range<String.Index>(start: timeString.startIndex.advancedBy(5), end: timeString.startIndex.advancedBy(7))))
            self.day = Int(timeString.substringWithRange(Range<String.Index>(start: timeString.startIndex.advancedBy(8), end: timeString.startIndex.advancedBy(10))))
            self.hour = Int(timeString.substringWithRange(Range<String.Index>(start: timeString.startIndex.advancedBy(11), end: timeString.startIndex.advancedBy(13))))
            self.minute = Int(timeString.substringWithRange(Range<String.Index>(start: timeString.startIndex.advancedBy(14), end: timeString.startIndex.advancedBy(16))))
            self.seconds = Int(timeString.substringWithRange(Range<String.Index>(start: timeString.startIndex.advancedBy(17), end: timeString.startIndex.advancedBy(19))))            
        }
        
        init(year : Int, month : Int, day : Int, hour : Int, minute : Int, seconds: Int){
            self.year = year
            self.month = month
            self.day = day
            self.hour = hour
            self.minute = minute
            self.seconds = seconds
        }
        
        func isBiggerThan(other:Date) -> Bool{
            if(self.year > other.year){
                return true
            }
            else if(self.year == other.year){
                if(self.month > other.month){
                    return true
                }
                else if(self.month == other.month){
                    if(self.day > other.day){
                        return true
                    }
                    else if(self.day == other.day){
                        if(self.hour > other.hour){
                            return true
                        }
                        else if(self.hour == other.hour){
                            if(self.minute > other.minute){
                                return true
                            }
                            else if(self.minute == other.minute){
                                if(self.seconds > other.seconds){
                                    return true
                                }
                            }
                        }
                    }
                }
            }
            return false
        }
        
        func toString() -> String{
            return (String(year!) + "-" + String(month!) + "-" + String(day!) + "/" + String(hour!) + ":" + String(minute!) + ":" + String(seconds!))
        }
        
        private var year: Int?
        private var month: Int?
        private var day: Int?
        private var hour: Int?
        private var minute: Int?
        private var seconds: Int?
    }
    
    /*public class Reachability {
        
        class func isConnectedToNetwork() -> Bool {
            
            var Status:Bool = false
            let url = NSURL(string: "http://google.com/")
            let request = NSMutableURLRequest(URL: url!)
            request.HTTPMethod = "HEAD"
            request.cachePolicy = NSURLRequestCachePolicy.ReloadIgnoringLocalAndRemoteCacheData
            request.timeoutInterval = 10.0
            let session = NSURLSession.sharedSession()
            
            session.dataTaskWithRequest(request, completionHandler: {(data, response, error) in
                print("data \(data)")
                print("response \(response)")
                print("error \(error)")
                
                if let httpResponse = response as? NSHTTPURLResponse {
                    print("httpResponse.statusCode \(httpResponse.statusCode)")
                    if httpResponse.statusCode == 200 {
                        Status = true
                    }
                }
                
            }).resume()
            
            
            return Status
        }
        
    }*/
    
}


extension NSData{
    var hashvalue : Int{
        get{
            let datastring = NSString(data:self, encoding:NSUTF8StringEncoding)
            return (datastring?.hashValue)!
        }
    }
}
