//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class Trone : Sprite{
    
    init(){
        super.init(imageNamed: Trone._normalImage)
        name = "trone_" + String(Trone._nextId)
        Trone._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:Trone = Trone()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: Trone._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: Trone._normalImage)
        }
    }
    
    override func getLevel() -> Int{
        return 2
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "trone"
    private static var _selectedImage:String = "trone_selected"
    
}