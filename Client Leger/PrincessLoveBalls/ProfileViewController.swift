//
//  ProfileViewController.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-31.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class ProfileViewController : UIViewController
{
    @IBOutlet var backButton: UIButton!
    @IBOutlet weak var webView: UIWebView!
    
    override func awakeFromNib()
    {
        super.awakeFromNib()
    }
    
    override func viewDidLoad()
        
    {
        super.viewDidLoad()
        
        backButton.setImage(UIImage(named: "Back_Button"), forState: UIControlState.Normal)
        self.view.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        
        if(WebSession.isConnected){
            let url = NSURL (string: "http://ec2-52-90-46-132.compute-1.amazonaws.com/Account/connect?userToken=" + WebSession.token!)
            let requestObj = NSURLRequest(URL: url!)
            webView.loadRequest(requestObj)
        }
        else{
            performSegueWithIdentifier("ReturnToMainMenu", sender: self)
        }
        
    }
}
