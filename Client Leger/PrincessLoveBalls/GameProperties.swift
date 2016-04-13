//
//  GameProperties.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-03-07.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation

public class GameProperties {
    
    static func clear(){
        LevelName = ""
        levelLevel = 1
        PointButoirCercle = 10
        PointButoirTriangle = 20
        PointCible = 20
        PointCampagne = 100
        PointBilleGratuite = 1000
        Difficulte = 1
        wasModifiedByServer = false
        wasLoadedFromServer = false
        updateServer = false        
    }
    
    static var LevelName = ""
    static var levelLevel = 1
    static var PointButoirCercle = 10
    static var PointButoirTriangle = 20
    static var PointCible = 20
    static var PointCampagne = 100
    static var PointBilleGratuite = 1000
    static var Difficulte = 1
    static var wasModifiedByServer = false
    static var wasLoadedFromServer = false
    static var updateServer = false
    static var levelComesFromPublicOffline = false
}