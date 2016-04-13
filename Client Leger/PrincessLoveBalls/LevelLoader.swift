//
//  LevelLoader.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

/*
<Racine>
<ListeObjets NbObjets="31">
    <Objet>
        <Type>trou</Type>
        <Position X="0" Y="-75" Z="0"/>
        <Rotation>0</Rotation>
        <Agrandissement X="1" Y="1" Z="1"/>
    </Objet>
    <Objet>
        <Type>portail</Type>
        <Position X="41.289999999999999" Y="-69.790000000000006" Z="0"/>
        <Rotation>0</Rotation>
        <Agrandissement X="1" Y="1" Z="1"/>
        <Frere>
            <Position X="17.809999465942383" Y="-28.409999847412109" Z="0"/>
            <Rotation>0</Rotation>
            <Agrandissement X="1" Y="1" Z="1"/>
        </Frere>
    </Objet>
*/

class LevelLoader{
    
    func loadLevelWithString(xmlString : String, levelName : String) -> SpriteContainer{
        do{
            
            //print(xmlString)
            
            if let xmlData = xmlString.dataUsingEncoding(NSUTF8StringEncoding){
                let document =  try AEXMLDocument(xmlData :xmlData)
                print(document.xmlString)
                GameProperties.LevelName = levelName
                return loadLevelWithAEXMLDocument(document)
            }
        }
        catch{
            print("error : Invalid XML")
        }
        return SpriteContainer()
    }
    
    func loadLevelWithFilename(filename : String) -> SpriteContainer {

        let level = readXMLFromFile(filename)
        GameProperties.LevelName = filename
        return loadLevelWithAEXMLDocument(level)
    }
    
    private func loadLevelWithAEXMLDocument( document: AEXMLDocument) -> SpriteContainer{        
        for prop in document.root["Proprietes"].children{
            switch(prop.name){
            case "PointButoirCercle":
                GameProperties.PointButoirCercle = prop.intValue
            case "PointButoirTriangle":
                GameProperties.PointButoirTriangle = prop.intValue
            case "PointCible":
                GameProperties.PointCible = prop.intValue
            case "PointCampagne":
                GameProperties.PointCampagne = prop.intValue
            case "PointBilleGratuite":
                GameProperties.PointBilleGratuite = prop.intValue
            case "Difficulte":
                GameProperties.Difficulte = prop.intValue
            default: break
            }
        }
        
        let spriteContainer = SpriteContainer()
        
        let children = document.root["ListeObjets"].children
        
        for sprites in children{
                var sprite = Sprite()
                //for object in sprites.all!{
                    for prop in sprites.children{
                        switch(prop.name){
                        case "Type":
                            //print(prop.value!)
                            switch(prop.value!){
                            case "paletteDroitJ1":
                                sprite = PaletteDroiteJ1()
                                
                            case "paletteDroitJ2":
                                sprite = PaletteDroiteJ2()
                                
                            case "portail":
                                sprite = Portal()
                                
                            case "mur":
                                sprite = Mur()
                                
                            case "paletteGaucheJ1":
                                sprite = PaletteGaucheJ1()
                                
                            case "paletteGaucheJ2":
                                sprite = PaletteGaucheJ2()
                                
                            case "butoirTriangleGauche":
                                sprite = ButoirTriangulaireGauche()
                                
                            case "butoirCercle":
                                sprite = ButoirCirculaire()
                                
                            case "cible":
                                sprite = Cible()
                                
                            case "trou":
                                sprite = Trou()
                                
                            case "generateurbille":
                                sprite = GenerateurDeBille()
                                
                            case "ressort":
                                sprite = Ressort()
                                
                            case "plateauDArgent":
                                sprite = Trone()
                                
                            case "champForce":
                                sprite = ChampForce()
                                print("champForce")
                                
                            case "butoirTriangleDroit":
                                sprite = ButoirTriangulaireDroit()
                            default:
                                break
                            }
                        case "Position":
                            sprite.place(Utilities.posClientLourdToiOS(CGPoint(x: Double(prop.attributes["X"]!)!, y: Double(prop.attributes["Y"]!)!)))
                        case "Rotation":
                            if(sprite.name!.containsString("mur_")){
                                sprite.rotateDeg(CGFloat(Double(prop.value!)! - 90))
                            }                            
                            else{
                                sprite.rotateDeg(CGFloat(Double(prop.value!)!))
                            }
                        case "Agrandissement":
                            sprite.scale(CGFloat(Double(prop.attributes["X"]!)!))
                        case "LongueurMur":
                            sprite.scale(Utilities.lengthMurClientLourdToiOS(CGFloat(Double(prop.value!)!)))
                        case "Frere":
                            let pairedPortal = Portal()
                            for pairedProp in sprites["Frere"].children{
                                switch(pairedProp.name){
                                case "Position":
                                    pairedPortal.place(Utilities.posClientLourdToiOS(CGPoint(x: Double(pairedProp.attributes["X"]!)!, y: Double(pairedProp.attributes["Y"]!)!)))
                                case "Rotation":
                                    pairedPortal.rotateDeg(CGFloat(Double(pairedProp.value!)!))
                                case "Agrandissement":
                                    pairedPortal.scale(CGFloat(Double(pairedProp.attributes["X"]!)!))
                                default:
                                    break
                                }
                            }
                            pairedPortal.setPairedPortal(sprite.name!)
                            let otherPortal = sprite as! Portal
                            otherPortal.setPairedPortal(pairedPortal.name!)
                            spriteContainer.addSprite(pairedPortal)
                        default: break
                       // }
                    }
                spriteContainer.addSprite(sprite)
            }
        }
        
        let currentTime = NSDate()
        
        let calendar = NSCalendar.currentCalendar()
        
        var components = calendar.components( NSCalendarUnit.Year, fromDate: currentTime)
        let year = Int(components.year)
        components = calendar.components(NSCalendarUnit.Month, fromDate: currentTime)
        let month = Int(components.month)
        components = calendar.components(NSCalendarUnit.Day, fromDate: currentTime)
        let day = Int(components.day)
        components = calendar.components(NSCalendarUnit.Hour, fromDate: currentTime)
        let hour = Int(components.hour)
        components = calendar.components(NSCalendarUnit.Minute, fromDate: currentTime)
        let minute = Int(components.minute)
        components = calendar.components(NSCalendarUnit.Second, fromDate: currentTime)
        let seconds = Int(components.second)
        
        let date = Utilities.Date(year: year, month: month, day: day, hour: hour, minute: minute, seconds: seconds)
        
        //print(date.toString())
        LevelManager.setLoadTime(date)
        return spriteContainer
    }

    private func URLForResource(fileName: String, withExtension: String) -> NSURL {
        let bundle = NSBundle()
        return bundle.URLForResource(fileName, withExtension: withExtension)!
    }
    
    func xmlDocumentFromURL(url: NSURL) -> AEXMLDocument {
        var xmlDocument = AEXMLDocument()
        
        if let data = NSData(contentsOfURL: url) {
            do {
                xmlDocument = try AEXMLDocument(xmlData: data)
            } catch {
                print(error)
            }
        }
        
        return xmlDocument
    }
    
    private func readXMLFromFile(filename: String) -> AEXMLDocument {
        let url = URLForResource(filename, withExtension: "xml")
        return xmlDocumentFromURL(url)
    }
    
    static func getLevel(xmlString : String) -> Int{
        var level = 0
        if let xmlData = xmlString.dataUsingEncoding(NSUTF8StringEncoding){
            do{
                let document = try AEXMLDocument(xmlData :xmlData)
                
                let children = document.root["ListeObjets"].children
                
                for sprites in children{
                    for prop in sprites.children{
                        switch(prop.name){
                        case "Type":
                            switch(prop.value!){
                                
                            case "plateauDArgent":
                                level = 1
                                
                            case "champForce":
                                level = 2
                            default:
                                break
                            }
                        default:
                            break
                        }
                    }
                }
            }
            catch{
                print("invalid XML String")
                return 0
            }
            
                    }
        
        return level
    }
}
