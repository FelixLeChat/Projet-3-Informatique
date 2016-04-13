//
//  GameViewController.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright (c) 2016 Alex Gagne. All rights reserved.
//

import UIKit
import SpriteKit

class GameViewController: UIViewController {
    
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.view.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        transparentLabelContainer.layer.zPosition = -1
        transparentLabelContainer.layer.borderWidth = 4
        transparentLabelContainer.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentLabelContainer.layer.cornerRadius = 20
        
        objectsTransparentZone.layer.zPosition = -1
        objectsTransparentZone.layer.borderWidth = 4
        objectsTransparentZone.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        objectsTransparentZone.layer.cornerRadius = 20
        
        toolsTransparentZone.layer.zPosition = -1
        toolsTransparentZone.layer.borderWidth = 4
        toolsTransparentZone.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        toolsTransparentZone.layer.cornerRadius = 20
        
        openLoadTransparentZone.layer.zPosition = -1
        openLoadTransparentZone.layer.borderWidth = 4
        openLoadTransparentZone.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        openLoadTransparentZone.layer.cornerRadius = 20
        
        if(WebSession.level < 1){
            champForceButton.setImage(UIImage(named: "ChampDeForceGris"), forState: UIControlState.Disabled)
            champForceButton.enabled = false
        }
        
        if(WebSession.level < 2){
            troneButton.setImage(UIImage(named: "PlateauDArgentGris"), forState: UIControlState.Disabled)
            troneButton.enabled = false
        }
        
        if let scene = GameScene(fileNamed:"GameScene") {
            _gameScene = scene
            
            _gameScene?.size = CGSize(width: 519, height: 922)
            
            // Configure the view
            
            let skView = editorView as! SKView
            //skView.showsFPS = true
            //skView.showsNodeCount = true
            
            /* Sprite Kit applies additional optimizations to improve rendering performance */
            skView.ignoresSiblingOrder = true
            
            
            
            /* Set the scale mode to scale to fit the window */
            scene.scaleMode = .AspectFill
            skView.presentScene(scene)
        }
        
        Utilities.backgroundThread(background: {
            while(WebSession.isConnected){
                sleep(5)
                if(WebSession.isConnected){
                    HTTPLevelManager.updateMapsFromServer()
                }
            }
        })
        
    }
    
    override func viewWillLayoutSubviews() {
        super.viewWillLayoutSubviews()
        _gameScene?._editorLogic.setModeLabel(editModeLabel)
        
    }

    override func shouldAutorotate() -> Bool {
        return false
    }

    override func supportedInterfaceOrientations() -> UIInterfaceOrientationMask {
        if UIDevice.currentDevice().userInterfaceIdiom == .Phone {
            return .AllButUpsideDown
        } else {
            return .All
        }
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject!) {
        if (segue.identifier == "saveSegue") {
            let svc = segue.destinationViewController as! SaveViewController
            
            svc.level = _gameScene?._editorLogic.getSpriteContainer()
        }
    }
    
    override func shouldPerformSegueWithIdentifier(identifier: String, sender: AnyObject!) -> Bool {
        if identifier == "saveSegue" {
            
            if (!(_gameScene!._editorLogic.isLevelValid())) {
                
                let alert = UIAlertController(title: "Niveau invalide", message: "Un niveau doit contenir au moins un ressort, un générateur et un trou", preferredStyle: UIAlertControllerStyle.Alert)
                let okButton = UIAlertAction(title: "Ok", style: .Default) { (alert: UIAlertAction!) -> Void in
                    // Do something after pushing button
                }
                alert.addAction(okButton)
                presentViewController(alert, animated: true, completion: nil)
                
                return false
            }
                
            else {
                return true
            }
        }
        
        // by default, transition
        return true
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Release any cached data, images, etc that aren't in use.
    }

    override func prefersStatusBarHidden() -> Bool {
        return true
    }
    
    
    @IBAction func DeleteButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.DELETE)
        self.editModeLabel.text = "Suppression"
    }
    
    @IBAction func MovementButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.MOVEMENT)
        editModeLabel.text = "Déplacement"
    }
    
    @IBAction func AddButton(sender: UIButton) {
        self.editModeLabel.text = "Ajout d'objet: Palette droite (J1)"
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.PALETTE_DROITE_J1)
    }
    
    @IBAction func SelectButton(sender: UIButton) {
        _gameScene?._editorLogic.unselectAllSprites()
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.SELECTION)
        editModeLabel.text = "Sélection"
    }
    
    @IBAction func CopyButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.COPY)
        editModeLabel.text = "Duplication"
    }
    
    @IBAction func AddWallButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.MUR)
        editModeLabel.text = "Ajout d'objet: Mur"
    }
    
    @IBAction func AddPortalButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.PORTAIL)
        editModeLabel.text = "Ajout d'objet: Portail"
    }
    
    @IBAction func AddButoirCirculaireButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.BUTOIR_CIRCULAIRE)
        editModeLabel.text = "Ajout d'objet: Butoir circulaire"
    }
    
    @IBAction func AddTroneButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.TRONE)
        editModeLabel.text = "Ajout d'objet: Plateau d'argent"
    }
    
    @IBAction func AddGenerateurButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.GENERATEUR)
        editModeLabel.text = "Ajout d'objet: Générateur de billes"
    }
    
    @IBAction func AddRessortButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.RESSORT)
        editModeLabel.text = "Ajout d'objet: Ressort"
    }
    
    @IBAction func AddPaletteGaucheButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.PALETTE_GAUCHE_J1)
        editModeLabel.text = "Ajout d'objet: Palette gauche (J1)"
    }
    
    @IBAction func AddChampDeForceButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.CHAMP_FORCE)
        editModeLabel.text = "Ajout d'objet: Champ de force"
    }
    
    @IBAction func AddTrouButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.TROU)
        editModeLabel.text = "Ajout d'objet: Trou"
    }
    
    @IBAction func AddButoirTriangulaireButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.BUTOIR_TRIANGULAIRE_DROIT)
        editModeLabel.text = "Ajout d'objet: Butoir triangulaire (droit)"
    }
    
    @IBAction func AddCibleButton(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.CIBLE)
        editModeLabel.text = "Ajout d'objet: Cible"
    }
    
    @IBAction func AddPaletteGaucheJ2(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.PALETTE_GAUCHE_J2)
        editModeLabel.text = "Ajout d'objet: Palette gauche (J2)"
    }
    @IBAction func AddPaletteDroiteJ2(sender: UIButton) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.PALETTE_DROITE_J2)
        editModeLabel.text = "Ajout d'objet: Palette droite (J2)"
    }
    
    @IBAction func AddButoirTriangulaireGaucheAction(sender: AnyObject) {
        _gameScene?._editorLogic.changeTouchStatus(TouchStatus.ADD)
        _gameScene?._editorLogic.changeObjectToAdd(SpriteToAdd.BUTOIR_TRIANGULAIRE_GAUCHE)
        editModeLabel.text = "Ajout d'objet: Butoir triangulaire (gauche)"
    }

    @IBAction func SaveButton(sender: UIButton) {
        UIGraphicsBeginImageContext(self.editorView.frame.size)
        
        self.editorView.drawViewHierarchyInRect(self.editorView.bounds, afterScreenUpdates: true)
        
        let image = UIGraphicsGetImageFromCurrentImageContext()
        UIGraphicsEndImageContext()
        
        HTTPLevelManager.imageLevel = image
        LevelSaver.imageLevelAnonymous = image
    }
    
    
    //Unwind segue
    @IBAction func cancelLoadLevel(segue:UIStoryboardSegue) {
        
    }
    
    @IBAction func loadLevel(segue:UIStoryboardSegue) {
        _gameScene?._editorLogic.loadLevel(xmlLevelString!, levelName: levelName!)
    }
    
    @IBAction func cancelSaveLevel(segue:UIStoryboardSegue) {
    }
    
    @IBAction func savedLevel(segue:UIStoryboardSegue) {
    }
    
    @IBOutlet var editorView: UIView!
    
    @IBOutlet var editModeLabel: UILabel!
    @IBOutlet var transparentLabelContainer: UIView!
    @IBOutlet var openLoadTransparentZone: UIView!
    
    @IBOutlet var objectsTransparentZone: UIView!
    @IBOutlet var toolsTransparentZone: UIView!
    
    @IBOutlet var champForceButton: UIButton!
    @IBOutlet var troneButton: UIButton!
    
    var xmlLevelString : String?
    var levelName : String?
    
    weak var _gameScene: GameScene?
    
    

}
