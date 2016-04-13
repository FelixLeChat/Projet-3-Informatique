//
//  SpriteContainer.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

class SpriteContainer{
    
    func addSprite(sprite:Sprite){
        _sprites[sprite.name!] = sprite
    }
    
    func getSprites() -> [Sprite]{
        var sprites : [Sprite] = []
        for sprite in _sprites{
            sprites.append(sprite.1)
        }
        
        return sprites
    }
    
    func getSelectedSprite() -> [Sprite]{
        var selectedSprites: [Sprite] = []
        for sprite in _sprites{
            if sprite.1.isSelected(){
                selectedSprites.append(sprite.1)
            }
        }
        return selectedSprites
    }
    
    func getAllCurrentNames() -> [String]{
        var names: [String] = []
        
        for sprite in _sprites{
            names.append(sprite.0)
        }
        return names
    }
    
    func unselectAll(){
        for sprite in _sprites{
            sprite.1.selected(false)
        }
    }
    
    func delete(sprite:Sprite){
        _sprites.removeValueForKey(sprite.name!)
    }
    
    func getSpriteByName(name:String)->Sprite?{
        for sprite in _sprites{
            if(sprite.1.name == name){
                return sprite.1
            }
        }
        return nil
    }
    
    func clear(){
        _sprites = [:]
    }
    
    var _sprites: [String: Sprite] = [:]
}