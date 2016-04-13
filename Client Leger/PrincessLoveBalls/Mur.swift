//
//  Mur.swift
//  PrincessLoveBalls
//
//  Created by Guillaume Lavoie-Harvey on 2016-02-22.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class Mur : Sprite{
    
    init(){
        super.init(imageNamed: Mur._normalImage)
        name = "mur_" + String(Mur._nextId)
        Mur._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:Mur = Mur()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: Mur._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: Mur._normalImage)
        }
    }
    
    override func scale(scaleFactor: CGFloat){
        _scale *= scaleFactor
        self.xScale = _scale
    }
    
    override func setScale(scale: CGFloat) {
        _scale = scale
        self.xScale = scale
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "mur"
    private static var _selectedImage:String = "mur_selected"
    
}