//
//  SoundPlayer.swift
//  PrincessLoveBalls
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import Foundation
import AVFoundation

class SoundPlayer{
    
    init(){
        getAllSounds()
    }    
    
    // Prend tous les sons dans les fichiers et les associés à un audioplayer
    func getAllSounds(){
        let fm = NSFileManager.defaultManager()
        let path = NSBundle.mainBundle().resourcePath!
        let items = try! fm.contentsOfDirectoryAtPath(path)
            
        for item in items{
            // Add different file formats if other than .mp3
            if item.containsString(".mp3"){
                var name = item.componentsSeparatedByString(".")
                
                // name[0] is the name of the file and name[1] is the format of the file
                let sound = NSURL(fileURLWithPath: NSBundle.mainBundle().pathForResource(name[0], ofType: name[1])!)
                var audioPlayer = AVAudioPlayer()
                
                do {
                    audioPlayer = try AVAudioPlayer(contentsOfURL: sound)
                }catch _ {
                    // Do nothing if sound is missing
                }
                _audioPlayers[item] = audioPlayer
            }
        }
    }
    
    func playSound(soundName: String){
        _audioPlayers[soundName]!.play()
    }
    
    func stopSound(soundName: String){
        _audioPlayers[soundName]!.stop()
    }
    
    var _audioPlayers: [String: AVAudioPlayer] = [:]
    
}