//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class ButoirCirculaire : Sprite{
    
    init(){
        super.init(imageNamed: ButoirCirculaire._normalImage)
        name = "butoirCercle_" + String(ButoirCirculaire._nextId)
        ButoirCirculaire._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:ButoirCirculaire = ButoirCirculaire()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: ButoirCirculaire._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: ButoirCirculaire._normalImage)
        }
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "butoir_circulaire"
    private static var _selectedImage:String = "butoir_circulaire_selected"
    
}