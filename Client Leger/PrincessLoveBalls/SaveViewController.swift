//
//  SaveViewController.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-31.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import UIKit

class SaveViewController : UIViewController{
    
    
    override func viewDidLoad(){
        super.viewDidLoad()
        
        self.view.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        
        transparentFrame.layer.zPosition = -1
        transparentFrame.layer.borderWidth = 4
        transparentFrame.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentFrame.layer.cornerRadius = 20
        
        pointsBilleGratuiteTextField.text = String(GameProperties.PointBilleGratuite)
        pointsCibleTextField.text = String(GameProperties.PointCible)
        pointButoirTriangulaire.text = String(GameProperties.PointButoirTriangle)
        pointButoirCirculaire.text = String(GameProperties.PointButoirCercle)
        difficulyTextField.text = String(GameProperties.Difficulte)
        levelNameTextField.text = GameProperties.LevelName
    }
    
    // Pour repasser le niveau à gameViewController en annulant
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject!) {
        if (segue.identifier == "cancelSaveLevel") {
            let gvc = segue.destinationViewController as! GameViewController
            
            gvc._gameScene?._editorLogic.setSpriteContainer(level!)
        }
        if ( segue.identifier == "savedLevel") {
            let gvc = segue.destinationViewController as! GameViewController
            
            gvc._gameScene?._editorLogic.setSpriteContainer(level!)
        }
    }
    
    
    @IBAction func cancelButton(sender: UIButton) {
        performSegueWithIdentifier("cancelSaveLevel", sender: self)
    }
    
    @IBAction func saveButton(sender: UIButton) {
        if(!GameProperties.wasModifiedByServer || (levelNameTextField.text != "" && GameProperties.LevelName != levelNameTextField.text)){
            if (levelNameTextField.text != ""){
                
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
                
                if (GameProperties.wasLoadedFromServer && GameProperties.LevelName != levelNameTextField.text!){
                    GameProperties.updateServer = false
                }
                else if(GameProperties.wasLoadedFromServer){
                    GameProperties.updateServer = true
                }
                
                GameProperties.LevelName = levelNameTextField.text!
                
                if (isPropertyValid(pointsBilleGratuiteTextField.text!)){
                    GameProperties.PointBilleGratuite = propertyToInt(pointsBilleGratuiteTextField.text!)
                }
                else{
                    okAlertBox("Donnée invalide", message: "Le nombre de point pour une bille gratuite ne doit contenir que des chiffres et être inférieur à 1000000.")
                    return
                }
                if (isPropertyValid(pointsCibleTextField.text!)){
                    GameProperties.PointCible = propertyToInt(pointsCibleTextField.text!)
                }
                else{
                    okAlertBox("Donnée invalide", message: "Le nombre de point d'une cible ne doit contenir que des chiffres et être inférieur à 1000000.")
                    return
                }
                if (isPropertyValid(pointButoirTriangulaire.text!)){
                    GameProperties.PointButoirTriangle = propertyToInt(pointButoirTriangulaire.text!)
                }
                else{
                    okAlertBox("Donnée invalide", message: "Le nombre de point d'un butoir triangulaire ne doit contenir que des chiffres et être inférieur à 1000000.")
                    return
                }
                if (isPropertyValid(pointButoirCirculaire.text!)){
                    GameProperties.PointButoirCercle = propertyToInt(pointButoirCirculaire.text!)
                }
                else{
                    okAlertBox("Donnée invalide", message: "Le nombre de point d'un butoir circulaire ne doit contenir que des chiffres et être inférieur à 1000000.")
                    return
                }
                if (isPropertyValid(difficulyTextField.text!)){
                    GameProperties.Difficulte = propertyToInt(difficulyTextField.text!)
                }
                else{
                    okAlertBox("Donnée invalide", message: "La difficulté ne doit contenir que des chiffres et être inférieur à 1000000.")
                    return
                }
                
                xmlString = LevelSaver.saveLevel(level!, sendToServer: true)
        
                GameProperties.wasModifiedByServer = false
        
                performSegueWithIdentifier("savedLevel", sender: self)
            }
            else{
                okAlertBox("Nom invalide", message: "Un niveau doit avoir un nom")
            }
        }
        else{
            if(GameProperties.LevelName == levelNameTextField.text){
                let alert = UIAlertController(title: "Niveau modifié", message: "La version du niveau sur le serveur est plus récente. Voulez-vous écraser la version sur le serveur?", preferredStyle: UIAlertControllerStyle.Alert)
                let yesButton = UIAlertAction(title: "Oui", style: .Default) { (alert: UIAlertAction!) -> Void in
                    GameProperties.wasModifiedByServer = false
                    self.saveButton(sender)
                }
                let noButton = UIAlertAction(title: "Non", style: .Default) { (alert: UIAlertAction!) -> Void in
                    let alert = UIAlertController(title: "Changer le nom", message: "Veuillez changer le nom", preferredStyle: UIAlertControllerStyle.Alert)
                    let okButton = UIAlertAction(title: "Ok", style: .Default) { (alert: UIAlertAction!) -> Void in
                        // Do something after pushing button
                    }
                    alert.addAction(okButton)
                    self.presentViewController(alert, animated: true, completion: nil)
                }
                alert.addAction(yesButton)
                alert.addAction(noButton)
                presentViewController(alert, animated: true, completion: nil)
            }
        }
        
    }
    
    private func isPropertyValid(property : String) -> Bool{
        let returnValue = (property != "" && property.rangeOfCharacterFromSet(inverseSet) == nil && property.stringByTrimmingCharactersInSet(inverseSet).characters.count <= 6)
        
        return returnValue
    }
    
    private func propertyToInt(property : String) -> Int{
        return Int(property.stringByTrimmingCharactersInSet(inverseSet))!
    }
    
    private func okAlertBox(title : String, message : String){
        let alert = UIAlertController(title: title, message: message, preferredStyle: UIAlertControllerStyle.Alert)
        let okButton = UIAlertAction(title: "Ok", style: .Default) { (alert: UIAlertAction!) -> Void in
            // Do nothing after pushing button
        }
        alert.addAction(okButton)
        presentViewController(alert, animated: true, completion: nil)
    }
    
    @IBOutlet weak var pointButoirTriangulaire: UITextField!
    @IBOutlet weak var pointButoirCirculaire: UITextField!
    @IBOutlet weak var pointsBilleGratuiteTextField: UITextField!
    @IBOutlet weak var pointsCibleTextField: UITextField!
    @IBOutlet weak var difficulyTextField: UITextField!
    @IBOutlet weak var levelNameTextField: UITextField!
    
    @IBOutlet var transparentFrame: UIView!
    
    
    private let inverseSet = NSCharacterSet(charactersInString:"0123456789").invertedSet
    
    
    var level : SpriteContainer?
    
    private var xmlString : String?
}