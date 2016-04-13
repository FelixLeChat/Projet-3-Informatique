//
//  Portal.swift
//  PrincessLoveBalls
//
//  Created by Guillaume Lavoie-Harvey on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class Portal : Sprite{
    
    init(){
        super.init(imageNamed: Portal._normalImage)
        // TODO changer le nom pour portail
        name = "portal_" + String(Portal._nextId)
        Portal._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:Portal = Portal()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: Portal._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: Portal._normalImage)
        }
    }
    
    override func setScale(scale: CGFloat) {
        //Nothing, portals cannot scale
    }
    
    override func scale(scaleFactor: CGFloat) {
        //Nothing, portals cannot scale
    }
    
    func setPairedPortal(pairedPortalID: String){
        _pairedPortal = pairedPortalID
    }
    
    func getPairedPortal() ->String{
        return _pairedPortal
    }
    
    private var _pairedPortal:String = ""
    private static var _nextId:Int = 0
    private static var _normalImage:String = "portal"
    private static var _selectedImage:String = "portal_selected"
    
}