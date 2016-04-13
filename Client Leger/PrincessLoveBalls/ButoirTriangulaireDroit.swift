//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class ButoirTriangulaireDroit : Sprite{
    
    init(){
        super.init(imageNamed: ButoirTriangulaireDroit._normalImage)
        name = "butoirTriangleDroit_" + String(ButoirTriangulaireDroit._nextId)
        ButoirTriangulaireDroit._nextId++
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:ButoirTriangulaireDroit = ButoirTriangulaireDroit()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.zRotation = self.zRotation
        
        return newSprite
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: ButoirTriangulaireDroit._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: ButoirTriangulaireDroit._normalImage)
        }
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "butoir_triangulaire_d"
    private static var _selectedImage:String = "butoir_triangulaire_d_selected"
    
}