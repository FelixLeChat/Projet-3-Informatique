//
//  MainMenuViewController.swift
//  PrincessLoveBalls
//
//  Created by Guillaume Lavoie-Harvey on 2016-02-11.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit

class MainMenuViewController: UIViewController {

    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Do any additional setup after loading the view.
        self.view.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        titleImage.image = UIImage(named: "banner")
        EditorButton.setImage(UIImage(named: "Editor_Button"), forState: UIControlState.Normal)
        ProfileButton.setImage(UIImage(named: "Profile_Button"), forState: UIControlState.Normal)
        ChatButton.setImage(UIImage(named: "StartChat_Button"), forState: UIControlState.Normal)
        DisconnectButton.setImage(UIImage(named: "Logout_Button"), forState: UIControlState.Normal)
        
        transparentFrame.layer.zPosition = -1
        transparentFrame.layer.borderWidth = 4
        transparentFrame.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentFrame.layer.cornerRadius = 20
        
        if(WebSession.isConnected){
            LevelManager.getAllPublicMapsOffline()           
        }
        else{
            //Setup for offline session
            
            ProfileButton.setImage(UIImage(named:"Profile_Button_gray"), forState: UIControlState.Disabled)
            ProfileButton.enabled = false
            
            ChatButton.setImage(UIImage(named:"StartChat_Button_gray"), forState: UIControlState.Disabled)
            ChatButton.enabled = false
            
            DisconnectButton.setImage(UIImage(named:"Back_Button"), forState: UIControlState.Normal)
        }
        
        
        // Start a thread verifying if there is a new level on the server        
        Utilities.backgroundThread(background: {
            while(WebSession.isConnected){
                sleep(5)
                if(WebSession.isConnected){
                    if(HTTPLevelManager.hasNewLevelOnServer()){
                        print("New level on server")
                        Notifications.callNotifications("New Level on server!", category: "Server Level")
                    }
                }
            }
        })        
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated
    }
    
    @IBAction func editorButtonTapped(sender: UIButton) {
        /*if(!WebSession.isConnected){
            let alert = UIAlertController(title: "Connexion invalide", message: "Vous n'êtes pas connecté ou vous avez été déconnecté du serveur", preferredStyle: UIAlertControllerStyle.Alert)
            let okButton = UIAlertAction(title: "Retour au menu précédent", style: .Default) { (alert: UIAlertAction!) -> Void in
                // Do something after pushing button
            }
            alert.addAction(okButton)
            presentViewController(alert, animated: true, completion: nil)
            performSegueWithIdentifier("disconnectPlayer", sender: self)
        }*/
    }
    @IBAction func chatButtonTapped(sender: UIButton) {
        if(!WebSession.isConnected){
            let alert = UIAlertController(title: "Connexion invalide", message: "Vous n'êtes pas connecté ou vous avez été déconnecté du serveur", preferredStyle: UIAlertControllerStyle.Alert)
            let okButton = UIAlertAction(title: "Retour au menu précédent", style: .Default) { (alert: UIAlertAction!) -> Void in
                // Do something after pushing button
            }
            alert.addAction(okButton)
            presentViewController(alert, animated: true, completion: nil)
            performSegueWithIdentifier("disconnectPlayer", sender: self)
        }
    }
    @IBAction func webButtonTapped(sender: UIButton) {
        if(!WebSession.isConnected){
            let alert = UIAlertController(title: "Connexion invalide", message: "Vous n'êtes pas connecté ou vous avez été déconnecté du serveur", preferredStyle: UIAlertControllerStyle.Alert)
            let okButton = UIAlertAction(title: "Retour au menu précédent", style: .Default) { (alert: UIAlertAction!) -> Void in
                // Do something after pushing button
            }
            alert.addAction(okButton)
            presentViewController(alert, animated: true, completion: nil)
            performSegueWithIdentifier("disconnectPlayer", sender: self)
        }
    }
    
 
    
    @IBAction func cancelToPlayersViewController(segue:UIStoryboardSegue) {
        GameProperties.clear()
    }
    
    @IBAction func getBackFromChat(segue:UIStoryboardSegue){
        
    }
    
    @IBOutlet weak var EditorButton: UIButton!
    @IBOutlet var ChatButton: UIButton!
    @IBOutlet var ProfileButton: UIButton!
    @IBOutlet var DisconnectButton: UIButton!
    @IBOutlet var titleImage: UIImageView!
    @IBOutlet var transparentFrame: UIView!

}
