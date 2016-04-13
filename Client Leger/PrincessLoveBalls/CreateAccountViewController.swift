//
//  CreateAccountViewController.swift
//  PrincessLoveBalls
//
//  Created by Guillaume Lavoie-Harvey on 2016-04-11.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit

class CreateAccountViewController: UIViewController, UITextFieldDelegate {
    
    @IBOutlet var transparentShitShit: UIView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.view.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        
        
        transparentShitShit.layer.zPosition = -1
        transparentShitShit.layer.borderWidth = 4
        transparentShitShit.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentShitShit.layer.cornerRadius = 20
        
        usernameTextField.delegate = self
        passwordTextField.delegate = self
        repeatedPasswordTextField.delegate = self

        // Do any additional setup after loading the view.
    }

    @IBAction func createButton(sender: UIButton) {
        self.register()
    }
    
    private func register(){
        errorMessageLabel.hidden = true
        if(passwordTextField.text == repeatedPasswordTextField.text){
            let username = usernameTextField.text!
            let password = passwordTextField.text!
            
            let registerResult = httpLogin.registerUser(username, password:password)
            print("code:",registerResult.statusCode)
            if(registerResult.statusCode == 200){
                //Réussi
                //Sewt web session
                WebSession.username = username
                WebSession.password = password
                let token = registerResult.text! ?? ""
                WebSession.token = String(token.characters.dropFirst().dropLast(1))
                WebSession.isConnected = true
                print("token:",WebSession.token)
                WebSession.level = httpLogin.getUserLevel(username)
                
                //Clear text fields for security reasons
                usernameTextField.text = ""
                passwordTextField.text = ""
                repeatedPasswordTextField.text = ""
                
                performSegueWithIdentifier("showMainMenu", sender: self)
                
            } else {
                print("error",registerResult.text!)
                errorMessageLabel.text = registerResult.text ?? ""
                errorMessageLabel.hidden = false
                passwordTextField.text = ""
                repeatedPasswordTextField.text = ""
            }
            
        } else {
            errorMessageLabel.text = "Les mots de passe doivent être identiques"
            errorMessageLabel.hidden = false
            passwordTextField.text = ""
            repeatedPasswordTextField.text = ""
        }
        
    }
    
    func textFieldShouldReturn(sender: UITextField) -> Bool {
        self.register()
        return true
    }
    
    
    @IBOutlet var errorMessageLabel: UILabel!
    @IBOutlet var usernameTextField: UITextField!
    @IBOutlet var passwordTextField: UITextField!
    @IBOutlet var repeatedPasswordTextField: UITextField!
    let httpLogin:HTTPLogin = HTTPLogin()



}
