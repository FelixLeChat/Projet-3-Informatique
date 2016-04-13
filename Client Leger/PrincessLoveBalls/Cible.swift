//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class Cible : Sprite{
    
    init(){
        super.init(imageNamed: Cible._normalImage)
        name = "cible_" + String(Cible._nextId)
        Cible._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:Cible = Cible()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: Cible._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: Cible._normalImage)
        }
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "cible"
    private static var _selectedImage:String = "cible_selected"
    
}