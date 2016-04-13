//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-10.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class PaletteGaucheJ2 : Sprite{
    
    init(){
        super.init(imageNamed: PaletteGaucheJ2._normalImage)
        name = "paletteGaucheJ2_" + String(PaletteGaucheJ2._nextId)
        PaletteGaucheJ2._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:PaletteGaucheJ2 = PaletteGaucheJ2()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: PaletteGaucheJ2._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: PaletteGaucheJ2._normalImage)
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
    private static var _normalImage:String = "palette_g_j2"
    private static var _selectedImage:String = "palette_g_j2_selected"
    
}