//
//  TouchManager.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-11.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

//COMMENTS: En ce moment SELECTION ne gère pas la sélection multiple non-simultanée

import Foundation
import SpriteKit


class ModifySpriteManager{
    
    init(spriteContainer:SpriteContainer){
        _spriteContainer = spriteContainer
        
    }
    
    func setGameScene(gameScene: GameScene){
        _gameScene = gameScene
        _zoneDeJeu = _gameScene?._zoneDeJeu
    }
    
    func touchesBegan(startTouchedPosition: [CGPoint]){
        _startPosition = startTouchedPosition
        _lastPosition = startTouchedPosition
    }
    
    // Modifies sprite depending on the status of touch
    func touchesMoved(newTouchedPosition: [CGPoint], touchStatus: TouchStatus, numTouches: Int){
        let sprites = _spriteContainer?.getSelectedSprite()
        
        if (sprites!.count == 0 || sprites ==  nil){
            return
        }
        
        if(_startPosition.count != numTouches){
            self.touchesBegan(newTouchedPosition)
            _deltaInitialized = false
            _deltaPos = [CGPoint](count: sprites!.count, repeatedValue: CGPoint())
            _firstInit = false
        }
        
        switch(touchStatus){        
            
        case TouchStatus.MOVEMENT:
            var startSprites:[[CGFloat]] = [[CGFloat]]()
            for sprite in sprites!{
                startSprites.append([sprite.position.x, sprite.position.y, sprite.zRotation, sprite.xScale, sprite.yScale, sprite._scale])
            }
            
            
            var centers:[CGPoint] = []
            var i:Int = 0
            if(!_firstInit){
                _deltaPos = [CGPoint](count: sprites!.count, repeatedValue: CGPoint())
                _firstInit = true
            }
            for sprite in sprites!{
                if(numTouches == 1){ //MOVEMENT
                    if(!_deltaInitialized){
                        _deltaPos[i].x = sprite.position.x - _lastPosition[0].x
                        _deltaPos[i].y = sprite.position.y - _lastPosition[0].y
                    }
                    
                    var newPos = CGPoint()
                    newPos.x = newTouchedPosition[0].x + _deltaPos[i].x
                    newPos.y = newTouchedPosition[0].y + _deltaPos[i].y
                    sprite.place(newPos)
                }
                i++
                
                if(numTouches >= 2){
                    //SCALE
                    let scaleFactor = distance(newTouchedPosition[0],p2: newTouchedPosition[1])/distance(_lastPosition[0],p2: _lastPosition[1])
                    sprite.scale(scaleFactor)
                    
                    //ROTATE
                    centers.append(sprite.position)
                }
            }
            _deltaInitialized = true
            
            var xCenter:CGFloat = 0
            var yCenter:CGFloat = 0
            for point in centers {
                xCenter += point.x
                yCenter += point.y
            }
            let center:CGPoint = CGPoint(x: xCenter/CGFloat(centers.count), y: yCenter/CGFloat(centers.count))
            
            for sprite in sprites! {
                if(numTouches >= 2){
                    let oldOrientation = Vector2(_lastPosition[0].x - _lastPosition[1].x, _lastPosition[0].y - _lastPosition[1].y)
                    let newOrientation = Vector2(newTouchedPosition[0].x - newTouchedPosition[1].x, newTouchedPosition[0].y - newTouchedPosition[1].y)
                    let angleToRotate = oldOrientation.angleWith(newOrientation)
                    sprite.rotateRad(angleToRotate)
                    if(sprites!.count >= 2){
                        //Rotate node around center
                        let newPos = rotateObjectAroundPoint(sprite.position, p2: center, rotationAngle: angleToRotate)
                        sprite.place(newPos)
                    }
                }
            }
            
            //Verify if move was valid
            var valid:Bool = true
            for sprite in sprites!{
                if !isMoveValid(sprite){
                    valid = false
                    break
                }
            }
            if !valid{
                for var i:Int = 0; i<sprites!.count;i++ {
                    sprites![i].position.x = startSprites[i][0]
                    sprites![i].position.y = startSprites[i][1]
                    sprites![i].zRotation = startSprites[i][2]
                    sprites![i].xScale = startSprites[i][3]
                    sprites![i].yScale = startSprites[i][4]
                    sprites![i]._scale = startSprites[i][5]
                }
            }
            
            
            break
            
        case TouchStatus.ADD://NEEDS TO BE FIXED
            if(_gameScene?._editorLogic._spriteToAdd == SpriteToAdd.MUR){
                //Position
                let newX = (newTouchedPosition[0].x + _startPosition[0].x)/2
                let newY = (newTouchedPosition[0].y + _startPosition[0].y)/2
                let newPos:CGPoint = CGPoint(x: newX, y: newY)
                let oldPos = sprites![0].position
                let oldScale = sprites![0]._scale
                let oldAngle = sprites![0].zRotation
                
                sprites![0].place(newPos)
                
                
                
                let scale = self.distance(newTouchedPosition[0],p2: _startPosition[0])+1
                sprites![0].setScale(scale)
                
                //Rotation
                let newOrientation = Vector2(newTouchedPosition[0].x - _startPosition[0].x, newTouchedPosition[0].y - _startPosition[0].y)
                let angleToRotate = newOrientation.angleWith(Vector2(CGFloat(1),CGFloat(0)))
                sprites![0].zRotation = -angleToRotate
                
                
                if !isMoveValid(sprites![0]){
                    sprites![0].place(oldPos)
                    sprites![0].setScale(oldScale)
                    sprites![0].zRotation = oldAngle
                }
            }
            break
        
        default:
            break
        }
        
        _lastPosition = newTouchedPosition
    }
    
    func touchesEnded(){
        _startPosition = [CGPoint]()
        _lastPosition = [CGPoint]()
        _deltaInitialized = false
        _firstInit = false
        _deltaPos = [CGPoint]()
    }
    
    private func distance(p1: CGPoint, p2: CGPoint) -> CGFloat{
        let xDist:CGFloat = (p2.x - p1.x)
        let yDist:CGFloat = (p2.y - p1.y)
        let distance:CGFloat = sqrt((xDist * xDist) + (yDist * yDist))
        return distance
    }
    
    private func rotateObjectAroundPoint(p1: CGPoint, p2: CGPoint, rotationAngle: CGFloat) ->CGPoint{
        let relX = p1.x - p2.x
        let relY = p1.y - p2.y
        
        let newX = relX*cos(rotationAngle) - relY*sin(rotationAngle)
        let newY = relX*sin(rotationAngle) + relY*cos(rotationAngle)

        return CGPoint(x: newX + p2.x,y: newY + p2.y)
    }
    
    private func isMoveValid(node: Sprite)->Bool{
        var xTexture = abs(node._scale*cos(node.zRotation)*node.texture!.size().width) + abs(node._scale*sin(node.zRotation)*node.texture!.size().height)
        var yTexture = abs(node._scale*sin(node.zRotation)*node.texture!.size().width) + abs(node._scale*cos(node.zRotation)*node.texture!.size().height)
        
        if node.dynamicType == Mur.self{
            xTexture = abs(node._scale*cos(node.zRotation)*node.texture!.size().width) + abs(sin(node.zRotation)*node.texture!.size().height)
            yTexture = abs(node._scale*sin(node.zRotation)*node.texture!.size().width) + abs(cos(node.zRotation)*node.texture!.size().height)
        }
        
        return (node.position.x + xTexture/2 < _zoneDeJeu!.size.width/2 && node.position.x - xTexture/2 > -_zoneDeJeu!.size.width/2 && node.position.y + yTexture/2 < _zoneDeJeu!.size.height/2 && node.position.y - yTexture/2 > -_zoneDeJeu!.size.height/2)
    }
    
    private var _startPosition: [CGPoint] = [CGPoint]()
    private var _lastPosition: [CGPoint] = [CGPoint]()
    private var _copyStarted: Bool = false
    private var _deltaPos = [CGPoint]()
    private var _deltaInitialized:Bool = false
    private var _firstInit:Bool = false
    
    weak var _spriteContainer: SpriteContainer?
    weak var _gameScene: GameScene?
    weak var _zoneDeJeu: SKSpriteNode?
    
}