//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class Ressort : Sprite{
    
    init(){
        super.init(imageNamed: Ressort._normalImage)
        name = "ressort_" + String(Ressort._nextId)
        Ressort._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:Ressort = Ressort()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: Ressort._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: Ressort._normalImage)
        }
    }
    
    override func scale(scaleFactor: CGFloat){
        //Do nothing
    }
    
    override func setScale(scale: CGFloat) {
        //Do nothing
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "ressort"
    private static var _selectedImage:String = "ressort_selected"
    
}