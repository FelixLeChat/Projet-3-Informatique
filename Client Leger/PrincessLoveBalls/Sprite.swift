//
//  Sprite.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//
import SpriteKit
import Foundation

class Sprite: SKSpriteNode{
    
    init(imageNamed: String) {
        let texture = SKTexture(imageNamed: imageNamed)// Creating a texture corresponding to the image
        super.init(texture: texture, color: UIColor.clearColor(), size: texture.size())
           
    }
    
    override init(texture: SKTexture!, color: UIColor, size: CGSize) {
        super.init(texture: texture, color: color, size: size)
    }

    // Apparently required because... what? Must ask Steve Jobs
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    func scale(scaleFactor: CGFloat){
        _scale *= scaleFactor
        if _scale < 0.5 {
            _scale = 0.5
        }
        if _scale > 2 {
            _scale = 2
        }
        
        self.xScale = _scale
        self.yScale = _scale
    }
    
    func move(addPos: CGPoint){
        self.position.x += addPos.x
        self.position.y += addPos.y
    }
    
    func place(newPos: CGPoint){
        self.position.x = newPos.x
        self.position.y = newPos.y
    }
    
    func rotateRad(addRotation: CGFloat){
        self.zRotation += addRotation
    }
    
    func rotateDeg(addRotation: CGFloat){
        rotateRad(Utilities.degToRad(addRotation))
    }
    
    func isSelected() -> Bool{
        return _selected
    }
    
    func selected(selected:Bool){
        _selected = selected
    }
    
    func getScale() -> CGFloat{
        return _scale
    }
    
    //Méthode de copie devant être réimplémentée dans chaque Sprite
    func copySprite() -> Sprite{
        preconditionFailure("This method must be overridden")
    }
    
    func getLevel() -> Int{
        return 0
    }

    
    var _scale: CGFloat = 1.0
    var _selected: Bool = false
}