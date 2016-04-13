//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class PaletteGaucheJ1 : Sprite{
    
    init(){
        super.init(imageNamed: PaletteGaucheJ1._normalImage)
        name = "paletteGaucheJ1_" + String(PaletteGaucheJ1._nextId)
        PaletteGaucheJ1._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:PaletteGaucheJ1 = PaletteGaucheJ1()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: PaletteGaucheJ1._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: PaletteGaucheJ1._normalImage)
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
    private static var _normalImage:String = "palette_g"
    private static var _selectedImage:String = "palette_g_selected"
    
}