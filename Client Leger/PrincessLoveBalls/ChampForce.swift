//
//  PaletteDroite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import SpriteKit

class ChampForce : Sprite{
    
    init(){
        super.init(imageNamed: ChampForce._normalImage)
        name = "champForce_" + String(ChampForce._nextId)
        ChampForce._nextId++
        
        let arrowTexture = SKTexture(imageNamed: "champ_force_arrow")
        _arrow = SKSpriteNode(texture: arrowTexture, size: arrowTexture.size())
        _arrow.zPosition = 2
        _arrow.name = "arrow"
        self.addChild(_arrow)
        
    }
    
    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func copySprite() -> Sprite {
        let newSprite:ChampForce = ChampForce()
        newSprite.scale(self._scale)
        newSprite.place(self.position)
        newSprite.rotateRad(self._arrow.zRotation)
        
        return newSprite
    }
    
    override func rotateRad(addRotation: CGFloat) {
        _arrow.zRotation += addRotation
    }
    
    override func selected(selected: Bool) {
        super.selected(selected)
        if(self._selected){
            self.texture = SKTexture(imageNamed: ChampForce._selectedImage)
        } else {
            self.texture = SKTexture(imageNamed: ChampForce._normalImage)
        }
    }
    
    override func getLevel() -> Int{
        return 1
    }
    
    private static var _nextId:Int = 0
    private static var _normalImage:String = "champ_force"
    private static var _selectedImage:String = "champ_force_selected"
    
    var _arrow:SKSpriteNode = SKSpriteNode()
    
}