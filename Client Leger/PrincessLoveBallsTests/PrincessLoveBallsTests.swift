//
//  PrincessLoveBallsTests.swift
//  PrincessLoveBallsTests
//
//  Created by Alex Gagne on 2016-02-08.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import XCTest
@testable import PrincessLoveBalls

class PrincessLoveBallsTests: XCTestCase {
    
    override func setUp() {
        super.setUp()
        // Put setup code here. This method is called before the invocation of each test method in the class.
    }
    
    override func tearDown() {
        // Put teardown code here. This method is called after the invocation of each test method in the class.
        super.tearDown()
    }
    
    func testExample() {
        // This is an example of a functional test case.
        // Use XCTAssert and related functions to verify your tests produce the correct results.
    }
    
    func testPerformanceExample() {
        // This is an example of a performance test case.
        self.measureBlock {
            // Put the code you want to measure the time of here.
        }
    }
    
    func testJSON1(){
        let testString1 = Utilities.buildJSONString([:])
        //print("\n",testString1.jsonString)
        XCTAssert(testString1.jsonString == "{\"\" : \"\"}")
    }
    
    func testJSON2(){
        let testString2 = Utilities.buildJSONString(["test1":"test2"])
        //print("\n", testString2.jsonString)
        XCTAssert(testString2.jsonString == "{\"test1\" : \"test2\"}")
    }
    
    func testJSON3(){
        let testString3 = Utilities.buildJSONString(["test1":"test2", "test3" : "test4"])
        //print("\n", testString3.jsonString)
        XCTAssert(testString3.jsonString == "{\"test3\" : \"test4\",\"test1\" : \"test2\"}")
        
    }
    
    /*static func posiOStoClientLourd(pos:CGPoint) -> CGPoint{
        var newPos = CGPoint(x: 0,y: 0)
        
        newPos.x = pos.x*(90/UIScreen.mainScreen().bounds.width) - 45
        newPos.y = pos.y*(160/UIScreen.mainScreen().bounds.height) - 80
        
        return newPos
    }
    
    static func posClientLourdToiOS(pos:CGPoint) -> CGPoint{
        var newPos = CGPoint(x: 0,y: 0)
        
        newPos.x = pos.x*(UIScreen.mainScreen().bounds.width/90) + UIScreen.mainScreen().bounds.width/2
        newPos.y = pos.y*(UIScreen.mainScreen().bounds.height/160) + UIScreen.mainScreen().bounds.width/2
        
        return newPos
    }
    
    static func lengthMuriOStoClientLourd(length : CGFloat) -> CGFloat{
        return (length/UIScreen.mainScreen().bounds.width)*90.0/8
    }
    
    static func lengthMurClientLourdToiOS(length : CGFloat) -> CGFloat{
        return (length/90.0)*UIScreen.mainScreen().bounds.width*8
    }
    
    static func degToRad(angle:CGFloat) -> CGFloat{
        return (angle/360)*2*CGFloat(M_PI)
    }
    
    static func radToDeg(angle:CGFloat) -> CGFloat{
        return ((angle%2*CGFloat(M_PI))/(2*CGFloat(M_PI)))*360
    }*/
    
    func createTime1(){
        let dateString = "2016-12-15/12:13:14"
        let date = Utilities.Date(timeString : dateString)
        
        XCTAssert(date.toString() == dateString)
    }
    
    func posiOStoClientLourd1(){
        XCTAssert(true)
    }
    
    func posiOStoClientLourd2(){
        XCTAssert(true)
    }
    
    func posiOStoClientLourd3(){
        XCTAssert(true)
    }
    
    func posClientLourdToiOS1(){
        XCTAssert(true)
    }
    
    func posClientLourdToiOS2(){
        XCTAssert(true)
    }
    
    func posClientLourdToiOS3(){
        XCTAssert(true)
    }
    
    func lengthMuriOStoClientLourd1(){
        XCTAssert(true)
    }
    
    func lengthMuriOStoClientLourd2(){
        XCTAssert(true)
    }
    
    func lengthMuriOStoClientLourd3(){
        XCTAssert(true)
    }
    
    func degToRad1(){
        XCTAssert(true)
    }
    
    func degToRad2(){
        XCTAssert(true)
    }
    
    func degToRad3(){
        XCTAssert(true)
    }
    
    func radToDeg1(){
        XCTAssert(true)
    }
    
    func radToDeg2(){
        XCTAssert(true)
    }
    
    func radToDeg3(){
        XCTAssert(true)
    }

}











