//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class ButoirTriangulaireGauche : Sprite{
    
    init(){
        super.init(imageNamed: ButoirTriangulaireGauche._normalImage)
        name = "butoirTriangleGauche_" + String(ButoirTriangulaireGauche._nextId)
        ButoirTriangulaireGauche._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:ButoirTriangulaireGauche = ButoirTriangulaireGauche()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: ButoirTriangulaireGauche._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: ButoirTriangulaireGauche._normalImage)
        }
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "butoir_triangulaire_g"
    private static var _selectedImage:String = "butoir_triangulaire_g_selected"
    
}