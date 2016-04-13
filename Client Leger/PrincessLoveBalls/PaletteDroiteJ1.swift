//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class PaletteDroiteJ1 : Sprite{
    
    init(){
        super.init(imageNamed: PaletteDroiteJ1._normalImage)
        // TODO nom des palettes peut éventuellement changer du côté client lourd
        name = "paletteDroitJ1_" + String(PaletteDroiteJ1._nextId)
        PaletteDroiteJ1._nextId++
        
    
    }

    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:PaletteDroiteJ1 = PaletteDroiteJ1()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: PaletteDroiteJ1._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: PaletteDroiteJ1._normalImage)
        }
    }
    
    override func scale(scaleFactor: CGFloat){
        //Do nothing
    }
    
    override func setScale(scale: CGFloat) {
        //Do nothing
    }
    
    var joueur = 0
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "palette_d"
    private static var _selectedImage:String = "palette_d_selected"
    
}