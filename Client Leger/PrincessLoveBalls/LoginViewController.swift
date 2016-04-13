//
//  LoginViewController.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-10.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit
import Foundation

class LoginViewController: UIViewController, UITextFieldDelegate {
    
    
    @IBOutlet var anotherTransparentShit: UIView!
    @IBOutlet var emptyUsernamePasswordLabel: UILabel!
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.view.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        bannerImage.image = UIImage(named: "banner")
        transparentViewFrame.layer.zPosition = -1
        transparentViewFrame.layer.borderWidth = 4
        transparentViewFrame.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentViewFrame.layer.cornerRadius = 20
        
        anotherTransparentShit.layer.zPosition = -1
        anotherTransparentShit.layer.borderWidth = 4
        anotherTransparentShit.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        anotherTransparentShit.layer.cornerRadius = 20
        
        loginButton.setImage(UIImage(named: "Login_Button"), forState: UIControlState.Normal)
        
        usernameTextField.delegate = self
        passwordTextField.delegate = self
        
    }
    
    @IBAction func loginButton(sender: UIButton) {
        self.login()
    }
    
    func textFieldShouldReturn(sender: UITextField) -> Bool {
        self.login()
        return true
    }
    
    private func login(){
        wrongPasswordLabel.hidden = true
        emptyUsernamePasswordLabel.hidden = true
        if(usernameTextField.text != "" && passwordTextField.text != ""){
            if(loginManager.connectToServer(usernameTextField.text!, password: passwordTextField.text!)){
                //Connection Succesful
                passwordTextField.text = ""
                usernameTextField.text = ""
                print("login successful")
                
                performSegueWithIdentifier("AfterLogin", sender: self)
            }
            else{
                wrongPasswordLabel.hidden = false
                passwordTextField.text = ""
            }
        } else {
            emptyUsernamePasswordLabel.hidden = false
            passwordTextField.text = ""
        }
    }
    
    @IBAction func bypassButton(sender: AnyObject) {
        if(loginManager.connectToServer("Alex", password: "wtf")){
            print("login successful")
        } else {
            print("login fail")
        }
    }
    
    @IBAction func disconnectPlayer(segue:UIStoryboardSegue) {
        WebSession.clear()
    }


    private var loginManager : LoginManager = LoginManager()
    
    @IBOutlet weak var usernameTextField: UITextField!
    @IBOutlet weak var passwordTextField: UITextField!
    @IBOutlet weak var wrongPasswordLabel: UILabel!
    @IBOutlet var loginButton: UIButton!
    @IBOutlet var bannerImage: UIImageView!
    @IBOutlet var transparentViewFrame: UIView!
}

