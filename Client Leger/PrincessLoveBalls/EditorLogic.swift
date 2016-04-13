//
//  EditorLogic.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//



import Foundation
import SpriteKit

class EditorLogic{
    
    init(){
        _modifySpriteManager = ModifySpriteManager(spriteContainer: _spriteContainer)
        _sounds = SoundPlayer()
    }
    
    func setGameScene(gameScene: GameScene){
        if !_initialized{
            _gameScene = gameScene
            _zoneDeJeu = _gameScene?._zoneDeJeu
            _initialized = true
            _modifySpriteManager!.setGameScene(gameScene)
        }
    }
    
    func setModeLabel(label: UILabel){
        _modeLabel = label
    }
    
    // This function catches the start of the touch event and decides what to do with it
    func touchesBegan(touches: Set<UITouch>, withEvent event: UIEvent?){
        var touchesPositions = [CGPoint]()
        for touch in  touches{
            touchesPositions.append(touch.locationInNode(_zoneDeJeu!))
        }
        _modifySpriteManager!.touchesBegan(touchesPositions)
        
        switch(_touchStatus){
        // En mode selection, on sélectionne chaque noeud touché, s'il n'y en a aucun, tous les objets sont déselectionnés
        case TouchStatus.SELECTION:
            var touchedANode = false
            for touch in touches{
                let positionInScene = touch.locationInNode(_zoneDeJeu!)
                var touchedNode = _zoneDeJeu!.nodeAtPoint(positionInScene)
                
                if var name = touchedNode.name
                {
                    if (name == "arrow"){
                        name = (touchedNode.parent?.name)!
                        touchedNode = touchedNode.parent!
                    }
                    if _spriteContainer.getAllCurrentNames().contains(name)
                    {
                        let sprite = touchedNode as! Sprite
                        if(sprite.isSelected()){
                            sprite.selected(false)
                        } else {
                            sprite.selected(true)
                        }
                        
                        
                        touchedANode = true
                    }
                }
            }
            
            if !touchedANode{
                _spriteContainer.unselectAll()
            }
            break
        
        case TouchStatus.DELETE:
            for touch in touches{
                let positionInScene = touch.locationInNode(_zoneDeJeu!)
                var touchedNode = _zoneDeJeu!.nodeAtPoint(positionInScene)
                
                if let portal = touchedNode as? Portal{
                    if let name = portal.name
                    {
                        if _spriteContainer.getAllCurrentNames().contains(name)
                        {
                            portal.removeFromParent()
                            removeSprite(portal)
                            let sibling = _spriteContainer.getSpriteByName(portal.getPairedPortal()) as? Portal
                            sibling!.removeFromParent()
                            removeSprite(sibling!)
                            
                        }
                    }
                } else {
                    if var name = touchedNode.name
                    {
                        if (name == "arrow"){
                            name = (touchedNode.parent?.name)!
                            touchedNode = touchedNode.parent!
                        }
                        if _spriteContainer.getAllCurrentNames().contains(name)
                        {
                            let sprite = touchedNode as! Sprite
                            sprite.removeFromParent()
                            removeSprite(sprite)
                            
                        }
                    }
                }
            }
            break
            
        case TouchStatus.ADD:
            let positionInScene = touches.first!.locationInNode(_zoneDeJeu!)
            if touchInZoneDeJeu(positionInScene){
                _spriteContainer.unselectAll()
                let sprite = self.createSprite(_spriteToAdd) //WILL DEPEND ON SELECTED SPRITE
                sprite.position = positionInScene
                sprite.selected(true)
                _gameScene?.addSprite(sprite)
                if (isPositionValid(sprite)) {
                    if(_spriteToAdd == SpriteToAdd.MUR){
                        
                    } else if(_spriteToAdd == SpriteToAdd.PORTAIL){
                        if(_portalInProgress == false){
                            _tempPortal = sprite as? Portal
                            _portalInProgress = true
                        } else {
                            _portalInProgress = false
                            _tempPortal?.setPairedPortal(sprite.name!)
                            let _tempSprite:Portal = (sprite as? Portal)!
                            _tempSprite.setPairedPortal(_tempPortal!.name!)
                            _tempPortal = nil
                        }
                        _touchStatus = TouchStatus.MOVEMENT
                        _previousTouchStatus = TouchStatus.ADD
                    } else {
                        _touchStatus = TouchStatus.MOVEMENT
                        _previousTouchStatus = TouchStatus.ADD
                    }

                } else {
                    sprite.removeFromParent()
                    removeSprite(sprite)
                }
            }
            
            break
            
        //Copie des objets
        case TouchStatus.COPY:
            //Portal stuff
            var tempPortals:[Portal] = []
            for sprite in _spriteContainer.getSelectedSprite(){
                if let portal = sprite as? Portal{
                    let portalCopy = portal.copySprite() as? Portal
                    tempPortals.append(portalCopy!)
                    
                    if let tempSibling = _spriteContainer.getSpriteByName(portal.getPairedPortal()) as? Portal{
                        if(!tempSibling.isSelected()){
                            let siblingCopy = tempSibling.copySprite() as? Portal
                            tempPortals.append(siblingCopy!)
                        }
                    }
                }
            }
            
            for sprite in _spriteContainer.getSelectedSprite(){
                if let portal = sprite as? Portal{
                    portal.selected(false)
                }
            }
            
            for var i:Int = 0; i<tempPortals.count;i+=2{
                tempPortals[i].setPairedPortal(tempPortals[i+1].name!)
                tempPortals[i+1].setPairedPortal(tempPortals[i].name!)
                
                tempPortals[i].selected(true)
                tempPortals[i+1].selected(true)
                
                _spriteContainer.addSprite(tempPortals[i])
                _spriteContainer.addSprite(tempPortals[i+1])
                
                _gameScene?.addSprite(tempPortals[i])
                _gameScene?.addSprite(tempPortals[i+1])
            }
                
                
            //Other stuff
            var tempSprites: [Sprite] = []
            for sprite in _spriteContainer.getSelectedSprite(){
                if(sprite.dynamicType != Portal.self){
                    tempSprites.append(sprite.copySprite())
                    sprite.selected(false)
                }
            }
            for sprite in tempSprites{
                sprite.selected(true)
                _spriteContainer.addSprite(sprite)
                _gameScene?.addSprite(sprite)
            }
            
            //Prepare for movement
            _modeLabel!.text = "Déplacement"
            _touchStatus = TouchStatus.MOVEMENT
            break
            
            
        // En mode qui n'est pas sélection, on modifie les propriétés des objets
        default:
            
            break
        }
    }
    
    // Catches when the touched is moved
    func touchesMoved(touches: Set<UITouch>, withEvent event: UIEvent?){
        if _touchStatus == TouchStatus.SELECTION{
            
        } else {
            var positionInScene = [CGPoint]()
            for touch in  touches{
                 positionInScene.append(touch.locationInNode(_zoneDeJeu!))
            }
            _modifySpriteManager!.touchesMoved(positionInScene, touchStatus: _touchStatus, numTouches: touches.count)
        }
    }
    
    func touchesEnded(touches: Set<UITouch>, withEvent event: UIEvent?) {
        switch(_touchStatus){
        case TouchStatus.MOVEMENT:
            if(_previousTouchStatus == TouchStatus.ADD){
                _touchStatus = TouchStatus.ADD
                _previousTouchStatus = TouchStatus.NONE
            }
            break
        default:
            break
        }
        
        _modifySpriteManager!.touchesEnded()
        let action = SKAction.moveByX(0, y: 0, duration: 0)
        _zoneDeJeu?.runAction(action)
        
    }
    
    func addSprite(sprite: Sprite){
        _spriteContainer.addSprite(sprite)
    }
    
    func removeSprite(sprite: Sprite) {
        _spriteContainer.delete(sprite)
    }
    
    func changeTouchStatus(newStatus: TouchStatus){
        _touchStatus = newStatus
        if(newStatus != TouchStatus.ADD && _portalInProgress ){
            _tempPortal?.removeFromParent()
            removeSprite(_tempPortal!)
            _portalInProgress = false
        }
    }
    
    func changeObjectToAdd(newObject: SpriteToAdd){
        _spriteToAdd = newObject
        if(_spriteToAdd != SpriteToAdd.PORTAIL && _portalInProgress){
            _tempPortal?.removeFromParent()
            removeSprite(_tempPortal!)
            _portalInProgress = false
        }
    }
    
    func saveLevel() -> String{
        
        GameProperties.levelLevel = 0
        for sprite in _spriteContainer.getSprites(){
            if(GameProperties.levelLevel < sprite.getLevel()){
                GameProperties.levelLevel = sprite.getLevel()
            }
        }
        
        return LevelSaver.saveLevel(_spriteContainer, sendToServer: true)
    }
    
    func loadLevel(xmlString : String, levelName : String){
        _spriteContainer.clear()
        _gameScene?.clearSprites()
        let tempSpriteContainer = _levelLoader.loadLevelWithString(xmlString, levelName : levelName)
        for sprite in tempSpriteContainer.getSprites(){
            _gameScene?.addSprite(sprite)
        }
    }
    
    func getSpriteContainer() -> SpriteContainer{
        return _spriteContainer
    }
    
    func setSpriteContainer(spriteContainer : SpriteContainer){
        _spriteContainer = spriteContainer
    }
    
    func isLevelValid() -> Bool{
        return LevelSaver.isLevelValid(_spriteContainer)
    }
    
    func getLevelToXML() -> String{       
        return LevelSaver.getXMLStringForContainer(_spriteContainer)
    }
    
    private func createSprite(toAdd: SpriteToAdd) -> Sprite{
        switch(toAdd){
        case SpriteToAdd.PALETTE_DROITE_J1:
            return PaletteDroiteJ1()
        case SpriteToAdd.MUR:
            return Mur()
        case SpriteToAdd.PORTAIL:
            return Portal()
        case SpriteToAdd.BUTOIR_CIRCULAIRE:
            return ButoirCirculaire()
        case SpriteToAdd.BUTOIR_TRIANGULAIRE_DROIT:
            return ButoirTriangulaireDroit()
        case SpriteToAdd.BUTOIR_TRIANGULAIRE_GAUCHE:
            return ButoirTriangulaireGauche()
        case SpriteToAdd.CIBLE:
            return Cible()
        case SpriteToAdd.CHAMP_FORCE:
            return ChampForce()
        case SpriteToAdd.RESSORT:
            return Ressort()
        case SpriteToAdd.TROU:
            return Trou()
        case SpriteToAdd.TRONE:
            return Trone()
        case SpriteToAdd.GENERATEUR:
            return GenerateurDeBille()
        case SpriteToAdd.PALETTE_GAUCHE_J1:
            return PaletteGaucheJ1()
        case SpriteToAdd.PALETTE_GAUCHE_J2:
            return PaletteGaucheJ2()
        case SpriteToAdd.PALETTE_DROITE_J2:
            return PaletteDroiteJ2()
        default:
            return Sprite()
        }
    }
    
    private func isPositionValid(node: Sprite)->Bool{
        var xTexture = abs(node._scale*cos(node.zRotation)*node.texture!.size().width) + abs(node._scale*sin(node.zRotation)*node.texture!.size().height)
        var yTexture = abs(node._scale*sin(node.zRotation)*node.texture!.size().width) + abs(node._scale*cos(node.zRotation)*node.texture!.size().height)
        
        if node.dynamicType == Mur.self{
            xTexture = abs(node._scale*cos(node.zRotation)*node.texture!.size().width) + abs(sin(node.zRotation)*node.texture!.size().height)
            yTexture = abs(node._scale*sin(node.zRotation)*node.texture!.size().width) + abs(cos(node.zRotation)*node.texture!.size().height)
        }
        return (node.position.x + xTexture/2 < _zoneDeJeu!.size.width/2 && node.position.x - xTexture/2 > -_zoneDeJeu!.size.width/2 && node.position.y + yTexture/2 < _zoneDeJeu!.size.height/2 && node.position.y - yTexture/2 > -_zoneDeJeu!.size.height/2)
        
    }
    
    private func touchInZoneDeJeu(position: CGPoint)->Bool{
        return(position.x < (_zoneDeJeu?.size.width)!/2 && position.x > -(_zoneDeJeu?.size.width)!/2 && position.y < (_zoneDeJeu?.size.height)!/2 && position.y > -(_zoneDeJeu?.size.height)!/2)
    }
    
    func unselectAllSprites(){
        _spriteContainer.unselectAll()
    }

    
    private var _portalInProgress = false
    private var _tempPortal:Portal?
    
    private var _soundPlayer : SoundPlayer = SoundPlayer()
    private var _spriteContainer: SpriteContainer = SpriteContainer()
    private var _modifySpriteManager: ModifySpriteManager?
    private var _levelLoader : LevelLoader = LevelLoader()
    
    var _initialized: Bool = false
    
    weak var _gameScene: GameScene?
    weak var _zoneDeJeu: SKSpriteNode?
    weak var _modeLabel: UILabel?
    
    var _touchStatus: TouchStatus = TouchStatus.SELECTION
    var _previousTouchStatus: TouchStatus = TouchStatus.NONE
    var _spriteToAdd: SpriteToAdd = SpriteToAdd.NONE
    private var _sounds:SoundPlayer
}

enum TouchStatus{
    case SELECTION
    case COPY
    case MOVEMENT
    case ADD
    case DELETE
    case NONE
}

enum SpriteToAdd{
    case PALETTE_DROITE_J1
    case PALETTE_DROITE_J2
    case PALETTE_GAUCHE_J1
    case PALETTE_GAUCHE_J2
    case BUTOIR_TRIANGULAIRE_DROIT
    case BUTOIR_TRIANGULAIRE_GAUCHE
    case BUTOIR_CIRCULAIRE
    case CIBLE
    case TROU
    case GENERATEUR
    case RESSORT
    case PORTAIL
    case TRONE
    case CHAMP_FORCE
    case MUR
    case NONE
}