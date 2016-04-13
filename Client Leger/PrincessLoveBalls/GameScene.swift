//
//  GameScene.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright (c) 2016 Alex Gagne. All rights reserved.
//

import AVFoundation
import SpriteKit

class GameScene: SKScene {
    
    override func didMoveToView(view: SKView) {
        
        _editorLogic.setGameScene(self)
        
        let zoneWidth = self.size.width
        let zoneHeight = self.size.height //zoneWidth*(16/9.0)
        
        _zoneDeJeu.position = CGPoint(x:self.size.width - zoneWidth/2, y:self.size.height - zoneHeight/2)
        _zoneDeJeu.size = CGSize(width: zoneWidth, height: zoneHeight)
        _zoneDeJeu.color = UIColor(red: 0.8, green: 0.5, blue: 0.7, alpha: 1.0)
        _zoneDeJeu.zPosition = -1
        _zoneDeJeu.anchorPoint = CGPoint(x: 0.5, y:0.5)
        self.addChild(_zoneDeJeu)
        
    }
    
    func addSprite(sprite: Sprite) {
        _zoneDeJeu.addChild(sprite as SKNode)
        _editorLogic.addSprite(sprite)
    }
    
    func deleteSprite(sprite: Sprite) {
        _editorLogic.removeSprite(sprite)
    }
    
    func clearSprites(){
        _zoneDeJeu.removeAllChildren()
    }
    
    // On laisse EditorLogic s'occuper de la logique après la détection d'évènement
    override func touchesBegan(touches: Set<UITouch>, withEvent event: UIEvent?) {
        _editorLogic.touchesBegan(touches, withEvent: event!)        
    }
    override func touchesMoved(touches: Set<UITouch>, withEvent event: UIEvent?) {
        _editorLogic.touchesMoved(touches, withEvent: event!)
    }
    override func touchesEnded(touches: Set<UITouch>, withEvent event: UIEvent?) {
        _editorLogic.touchesEnded(touches, withEvent: event!)
    }
   
    override func update(currentTime: CFTimeInterval) {
        /* Called before each frame is rendered */
    }
    
    
    var _editorLogic: EditorLogic = EditorLogic()
    var _zoneDeJeu: SKSpriteNode = SKSpriteNode()
    //var _boundingBox: [SKSpriteNode] = [SKSpriteNode]() //Up, Left, Down, Right
    
}
