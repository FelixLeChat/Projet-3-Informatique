//
//  TutorielViewController.swift
//  PrincessLoveBalls
//
//  Created by Guillaume Lavoie-Harvey on 2016-04-10.
//  Copyright © 2016 Alex Gagne. All rights reserved.
//

import UIKit

class TutorielViewController: UIViewController {
    
    @IBOutlet var buttonFrame: UIView!
    @IBOutlet var imageFrame: UIView!
    @IBOutlet var imageView: UIImageView!
    @IBOutlet var textLabel: UILabel!
    @IBOutlet var transparentTextFrame: UIView!
    @IBOutlet var nextButton: UIButton!
    @IBOutlet var backButton: UIButton!
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.view.backgroundColor = UIColor(patternImage: UIImage(named: "background_tile")!)
        transparentTextFrame.layer.zPosition = -1
        transparentTextFrame.layer.borderWidth = 4
        transparentTextFrame.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        transparentTextFrame.layer.cornerRadius = 20
        
        imageFrame.layer.zPosition = -1
        imageFrame.layer.borderWidth = 4
        imageFrame.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        imageFrame.layer.cornerRadius = 20
        
        buttonFrame.layer.zPosition = -1
        buttonFrame.layer.borderWidth = 4
        buttonFrame.layer.borderColor = UIColor(red: 0.733, green: 0.204, blue: 0.424, alpha: 1).CGColor
        buttonFrame.layer.cornerRadius = 20
        
        backButton.setImage(UIImage(named: "Insert_Button_gray"), forState: UIControlState.Disabled)
        
        nextButton.setImage(UIImage(named: "Extract_Button_gray"), forState: UIControlState.Disabled)
        
        updateText(_currentIndex)
        updateImage(_currentIndex)

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBAction func nextButtonPush(sender: UIButton) {
        _currentIndex++
        updateImage(_currentIndex)
        updateText(_currentIndex)
    }
    
    @IBAction func backButtonPush(sender: UIButton) {
        _currentIndex--
        updateImage(_currentIndex)
        updateText(_currentIndex)    }
    
    func updateText(index:Int){
        
        switch(index){
        case 0:
            backButton.enabled = false
            textLabel.text = "Bienvenue dans le tutoriel du client iPad de Princess Love Balls!\n\nCe tutoriel vous guidera dans l'utilisation des différentes fonctions disponibles sur ce client.\n\nVous pouvez appuyer sur la flèche droite pour avancer dans le tutoriel, ou sur la flèche gauche pour revenir à l'étape précédente. Vous pouvez aussi appuyer sur \"Retour\" pour revenir au menu principal à tout moment."
        
            break
        case 1:
            backButton.enabled = true
            textLabel.text = "L'écran d'accueil vous offre plusieurs façon d'accéder à l'application.\n\nSi vous possédez un compte d'utilisateur préalablement créé, vous pouvez entrer vos information de connexion dans la zone encerclé, puis apuuyer sur \"Login\" pour vous connecter."
            break
        case 2:
            textLabel.text = "Si vous ne possédez pas de compte, vous pouvez appuyer sur \"Créer un compte\" afin de procéder à la création d'un compte utilisateur."
            break
        case 3:
            textLabel.text = "Si vous ne désirez pas vous créer de compte ou si vous n'avez pas de connexion internet, il est possible d'utiliser l'application de manière limitée en mode hors ligne."
            break
        case 4:
            textLabel.text = "L'écran de création de compte permet de vous créer un compte Princess Love Balls. Vous pouvez entrer les informations souhaitées dans les champs de texte encerclés ci-haut."
            break
        case 5:
            textLabel.text = "Une fois les information écrites, vous pouves appuyer sur \"Créer un compte\" afin de procéder à la création du compte et à la connexion sur l'application."
            break
        case 6:
            textLabel.text = "Voici le menu principal. À partir de cet écran, vous pouvez accéder aux différentes fonctions disponibles sur cette application."
            break
        case 7:
            textLabel.text = "Vous pouvez appuyer sur \"Éditeur\" pour accéder à l'éditeur de cartes.\n\nUne description détaillée des fonctions disponibles dans l'éditeur est disponible plus tard dans le tutoriel."
            break
        case 8:
            textLabel.text = "Vous pouvez aussi accéder au service de clavardage en appuyant sur le bouton approprié sur le menu principal.\n\nUne description détaillée des fontionnalités de clavardage est disponible plus tard dans le tutoriel."
            break
        case 9:
            textLabel.text = "Il est aussi possible d'accéder au présent tutoriel à partir du menu principal, afin d'obtenir de l'information sur les différentes fonctionnalités de cette application."
            break
        case 10:
            textLabel.text = "Il est possible d'accéder à son profil d'utilisateur en appuyant sur le bouton approprié du menu principal.\n\nL'interface utilisée sur cette page est la même que sur le site princessloveballs.tech. Il est possible de consulter et modifier son profil ainsi que de visualiser sa liste d'amis."
            break
        case 11:
            textLabel.text = "Il est possible de se déconnecter de son profil sur le iPad à partir du menu principal.\n\nCette action vous ramènera à la fenêtre de connexion."
            break
        case 12:
            textLabel.text = "Si vous êtes en mode hors ligne, les fonctions de clavardage et de visualisation du profil sont inacsessibles.\n\nNote: Les cartes créées en mode hors ligne ne sont ajoutées sur le serveur que lorsque l'application se reconnecte à ce dernier."
            break
        case 13:
            textLabel.text = "La fenêtre de clavardage permet de communiquer avec d'autres utilisateurs de Princess Love Balls connectés sur le client iPad ou sur le client PC.\n\nEn appuyant sur la zone de texte encerclée si haut, il est possible d'écrire un message à envoyer aux autres utilisateurs présents dans le canal utilisé."
            break
        case 14:
            textLabel.text = "Lorsque le message souhaité est écrit, vous pouvez appuyer sur le bouton \"Envoyer\" encerclé ci-haut ou sur la touche \"Done\" sur votre clavier pour envoyer votre message au canal de discussion actuel."
            break
        case 15:
            textLabel.text = "Il est possible de rejoindre un canal de discussion différent en écrivant le nom dans la zone de texte ci-haut, puis en appuyant sur \"Joindre.\""
            break
        case 16:
            textLabel.text = "Afin de visualiser la liste des canaux de discussion que vous avez rejoints, vous devez réduire le clavier en appuant sur l'icône encerclée dans le coin en bas à droite de votre clavier, encerclée ci-haut."
            break
        case 17:
            textLabel.text = "Les différents canaux rejoints sont affichés dans le bas de l'écran.\n\nIl est possible de changer de canal actif en appuyant sur le nom du canal."
            break
        case 18:
            textLabel.text = "Il est possible de quitter le canal de discussion actif en appuyant sur le bouton \"Fermer\" ci-haut.\n\nNote: Il n'est pas possible de quitter le canal général."
            break
        case 19:
            textLabel.text = "L'éditeur offre plusieurs fonctions afin de créer ou modifier des cartes.\n\nTout d'abord, il est possible d'ajouter différents objets à la zone de jeu en appuyant sur le bouton correspondant puis en appuyant sur l'endroit désiré dans la zone de jeu."
            break
        case 20:
            textLabel.text = "Il est à noter que les objets \"Plateau d'argent\" et \"Champ de force\" ne sont respectivement disponibles que pour des utilisateurs de plus haut niveau.\n\nLe champ de force requiert un utilisateur de niveau 1.\n\nLe plateau d'argent requiert un utilisateur de niveau 2."
            break
        case 21:
            textLabel.text = "Les différentes fonctions d'édition des objets existants sont disponibles dans la zone encerclée dans l'image ci-haut."
            break
        case 22:
            textLabel.text = "La fonction \"Déplacement\" permet de déplacer un ou plusieurs objets sélectionnés. Le fonctionnement détaillé de cette fonction sera expliqué plus tard dans le tutoriel."
            break
        case 23:
            textLabel.text = "La fonction sélection permet de sélectionner un ou plusieurs objets existants en appuyant sur leur image dans la zone de jeu.\n\nAppuyer sur un objet déjà sélectionné permet de le déselectionner, et appuyer sur le bouton de l'outil de sélection désélectionne automatiquement tous les objets sélectionnés."
            break
        case 24:
            textLabel.text = "L'outil de suppression permet de supprimer des objets existants sur la zone de jeu.\n\nUne fois l'outil sélectionné, il suffit simplement d'appuyer sur différents objets dans la zone de jeu afin de les supprimer."
            break
        case 25:
            textLabel.text = "L'outil de duplication permet de copier les objets sélectionnés.\n\nLorqu'un ou plusieurs objets sont sélectionnés et que l'action de duplication est sélecitonnée, glisser les objets sélectionnés sur l'écran permet de les copier à un endroit différent."
            break
        case 26:
            textLabel.text = "Il est à noter que le mode d'édition actuellement actif est toujours affiché sur l'écran, dans la zone encerclée ci-haut."
            break
        case 27:
            textLabel.text = "L'outil de déplacement permet 3 actions différentes.\n\nD'abord, en glissant un seul doit sur l'écran, il est possible de déplacer les objets sélectionnés dans la direction du mouvement."
            break
        case 28:
            textLabel.text = "Ensuite, il est possible d'effectuer un mouvement de rotation à deux doigts afin que les objets sélectionnés effectuent une rotation similaire."
            break
        case 29:
            textLabel.text = "Faire un mouvement d'éloigement avec deux doigts permet d'agrandir les objets sélectionnés.\n\nNote: Les objets \"Ressort\", \"Portail\" et \"Palettes\" ne peuvent pas changer de taille."
            break
        case 30:
            textLabel.text = "Faire un mouvement de rapprochement avec deux doigts permet de rapetisser les objets sélectionnés.\n\nNote: Les objets \"Ressort\", \"Portail\" et \"Palettes\" ne peuvent pas changer de taille."
            break
        case 31:
            textLabel.text = "Lorsque vous avez créer une zone de jeu satisfaisante, vous pouvez la sauvegarder en appuyant sur le bouton approprié en bas à droite de l'écran.\n\nNote: Une zone de jeu doit absolument contenir un générateur de billes, un trou et un ressort pour être valide.\n\nVous pouvez aussi ouvrir une carte existante en appuyant sur le bouton approprié."
            break
        case 32:
            textLabel.text = "L'écran de sauvegarde permet de modifier les informations d'une carte.\n\nLa première zone de texte permet de modifier le nom de la carte.\n\nNote: Utiliser le nom d'une carte existante modifiera cette carte sur le serveur, sauf si la version sur le serveur est plus récente."
            break
        case 33:
            textLabel.text = "Les champs de texte suivants permettent de modifier les informations relatives au pointage de la carte."
            break
        case 34:
            textLabel.text = "Une fois les informations entrées, il est possible d'apputer sur \"Sauvegarder\" pour envoyer la carte sur le serveur. Il est aussi possible à tout moment d'appuyer sur \"Annuler\" pour annuler la sauvegarde et revenir à l'éditeur."
            break
        case 35:
            nextButton.enabled = true
            textLabel.text = "L'écran de chargement permet de voir les cartes disponibles pour édition sur le serveur.\n\nVous pouvez glisser l'image vers la gauche ou la droite afin de voir les différentes zones de jeu présentes."
            break
        case 36:
            nextButton.enabled = false
            textLabel.text = "Une fois la zone de jeu désirée atteinte, vous pouvez appuyer sur \"Ouvrir\" pour charger la zone de jeu dans l'éditeur.\n\nVous avez atteint la fin du tutoriel. Bon jeu, et amusez-vous bien!"
            break
        default:
            break
            
        }
        
    }
    
    func updateImage(index:Int){
        imageView.image = UIImage(named: "tutorial"+String(_currentIndex))
    }
    
    var _currentIndex:Int = 0
    

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

}
