///////////////////////////////////////////////////////////////////////////////
/// @file FacadeModele.cpp
/// @author Martin Bisson
/// @date 2007-05-22
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

// Commentaire Doxygen mis sur la première page de la documentation Doxygen.
/**

@mainpage Projet intégrateur de deuxième année -- INF2990

*/

#include "FacadeModele.h"

#include "Configuration/ConfigScene.h"
#include "CompteurAffichage.h"
#include "Affichage/Affichage.h"
#include "Sons/ClasseSons.h"
#include "Reseau/NetworkManager.h"

#include "Partie/PartieRapide.h"
#include "Partie/PartieCampagne.h"
#include "Partie/PartieRapideCompetitive.h"
#include "Partie/PartieCampagneCompetitive.h"

using namespace std;

/// Pointeur vers l'instance unique de la classe.
FacadeModele* FacadeModele::instance_{nullptr};

/// Chaîne indiquant le nom du fichier de configuration du projet.
const string FacadeModele::FICHIER_CONFIGURATION{"données\\configuration.xml"};

////////////////////////////////////////////////////////////////////////
///
/// @fn FacadeModele* FacadeModele::obtenirInstance()
///
/// Cette fonction retourne un pointeur vers l'instance unique de la
/// classe.  Si cette instance n'a pas été créée, elle la crée.  Cette
/// création n'est toutefois pas nécessairement "thread-safe", car
/// aucun verrou n'est pris entre le test pour savoir si l'instance
/// existe et le moment de sa création.
///
/// @return Un pointeur vers l'instance unique de cette classe.
///
////////////////////////////////////////////////////////////////////////
FacadeModele* FacadeModele::obtenirInstance()
{
	if (instance_ == nullptr)
	{
		instance_ = new FacadeModele;
	}

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::libererInstance()
{
	delete instance_;
	instance_ = nullptr;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn FacadeModele::~FacadeModele()
///
/// Ce destructeur libère les objets du modèle.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
FacadeModele::~FacadeModele()
{
	if (editeur_ != nullptr)
		delete editeur_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::initialiserOpenGL(HWND hWnd, char* s)
///
/// Cette fonction permet d'initialiser le contexte OpenGL.  Elle crée
/// un contexte OpenGL sur la fenêtre passée en paramètre, initialise
/// FreeImage (utilisée par le chargeur de modèles) et assigne des 
/// paramètres du contexte OpenGL.
///
/// @param[in] hWnd : La poignée ("handle") vers la fenêtre à utiliser.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::initialiserOpenGL(HWND hWnd, char* s, int l)
{
	string cheminabsolu = s;
	cheminabsolu = cheminabsolu.substr(0, l);
	CHEMIN_ABSOLU = string(cheminabsolu + "\\");

	// Initialisation de la configuration
	chargerConfiguration();
	editeur_ = new Editeur();

	Affichage::obtenirInstance()->intialiserOpenGL(hWnd);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::initialiserOpenGL(HWND hWnd)
///
///
/// @param[in] hWnd : La poignée ("handle") vers la fenêtre à utiliser.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::initialiserPourTests() const
{
	// Création de l'arbre de rendu.  À moins d'être complètement certain
	// d'avoir une bonne raison de faire autrement, il est plus sage de créer
	// l'arbre après avoir créé le contexte OpenGL.
	/*arbre_ = new ArbreRenduINF2990;
	arbre_->initialiser();
	noeudSel_ = arbre_;*/ // TODO

	// Initialisation de la configuration
	chargerConfiguration();
	Affichage::obtenirInstance()->intialiserOpenGL();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::chargerConfiguration() const
///
/// Cette fonction charge la configuration à partir d'un fichier XML si
/// ce dernier existe.  Sinon, le fichier de configuration est généré à
/// partir de valeurs par défaut directement dans le code.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::chargerConfiguration() const
{
	// Vérification de l'existance du ficher

	// Si le fichier n'existe pas, on le crée.
	if (!utilitaire::fichierExiste(FICHIER_CONFIGURATION))
	{
		enregistrerConfiguration();
	}
	// si le fichier existe on le lit
	else
	{
		tinyxml2::XMLDocument document;

		// Lire à partir du fichier de configuration
		document.LoadFile(FICHIER_CONFIGURATION.c_str());

		// On lit les différentes configurations.
		ConfigScene::obtenirInstance()->lireDOM(document);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::enregistrerConfiguration() const
///
/// Cette fonction génère un fichier XML de configuration à partir de
/// valeurs par défaut directement dans le code.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::enregistrerConfiguration() const
{
	tinyxml2::XMLDocument document;
	// Écrire la déclaration XML standard...
	document.NewDeclaration(R"(?xml version="1.0" standalone="yes"?)");

	// On enregistre les différentes configurations.
	ConfigScene::obtenirInstance()->creerDOM(document);

	// Écrire dans le fichier
	document.SaveFile(FICHIER_CONFIGURATION.c_str());
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::libererOpenGL()
///
/// Cette fonction libère le contexte OpenGL et désinitialise FreeImage.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::libererOpenGL()
{
	utilitaire::CompteurAffichage::libererInstance();

	// On libère les instances des différentes configurations.
	ConfigScene::libererInstance();

	Affichage::obtenirInstance()->libererOpenGL();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::libererPourTests()
///
/// Cette fonction libère le contexte OpenGL pour les tests
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::libererPourTests()
{
	utilitaire::CompteurAffichage::libererInstance();

	// On libère les instances des différentes configurations.
	ConfigScene::libererInstance();

	Affichage::obtenirInstance()->libererOpenGL();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::afficher() const
///
/// Cette fonction affiche le contenu de la scène.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::afficher() const
{
	glEnable(GL_BLEND);
	glBlendFunc(GL_ONE, GL_ZERO);
	//afficher
	auto afficheur = Affichage::obtenirInstance();
	afficheur->initAffichageFrame();
	if (partie_ != nullptr)
	{
		partie_->afficher();
	}
	afficheur->finaliserAffichageFrame();


	// Compte de l'affichage
	utilitaire::CompteurAffichage::obtenirInstance()->signalerAffichage();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::reinitialiser()
///
/// Cette fonction réinitialise la scène à un état "vide".
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::reinitialiser() const
{
	partie_->reinitialiser();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::arreterSons()
///
/// Cette fonction arrete les sons en cours
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::arreterSons() const
{
	ClasseSons::obtenirInstance()->arreterSons();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool FacadeModele::chargerFichierXML(char* nomFichier, int longueur) 
///
/// Cette fonction charge un fichier XML si
/// ce dernier existe.
///
/// @param[in] nomfichier : le nom du fichier sous lequel enregistrer.
/// @param[in] longueur : la longueur du nom du fichier pour la conversion en string.
///
/// @return Si le chargement a été fait avec succès ou non.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::chargerFichierXML(char* nomFichier, int longueur)
{
	delete partie_;
	partie_ = new PartieBase(string(nomFichier, longueur));
	editeur_->assignerZoneDeJeu(partie_->obtenirZoneDeJeu());
	return true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool FacadeModele::enregistrerFichierXML(char* nomFichier, int longueur) 
///
/// Cette fonction génère un fichier XML.
///
/// @param[in] nomFichier : le nom du fichier sous lequel enregistrer.
/// @param[in] longueur : la longueur du nom du fichier pour la conversion en string.
///
/// @return Si l'enregistrement s'est effectué avec succès ou non.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::enregistrerFichierXML(char* nomFichier, int longueur) const
{
	return editeur_->enregistrerFichierXML(nomFichier, longueur);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::mettreAJourProprietes(int pointButoirCercle, int pointButoirTriangle, int pointCible, int pointBilleGratuite, int pointCampagne, int difficulte) 
///
/// Cette fonction met à jour les propriétés du jeu.
///
/// @param[in] pointButoirCercle : le pointage d'un butoir circulaire.
/// @param[in] pointButoirTriangle : le pointage d'un butoir triangulaire.
/// @param[in] pointCible : le pointage d'une cible.
/// @param[in] pointBilleGratuite : le pointage nécessaire pour recevoir une bille gratuite.
/// @param[in] pointCampagne : le pointage d'une campagne.
/// @param[in] difficulte : le niveau de difficulté.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::mettreAJourProprietes(int pointButoirCercle, int pointButoirTriangle, int pointCible, int pointBilleGratuite, int pointCampagne, int difficulte) const
{
	editeur_->mettreAJourProprietes(pointButoirCercle, pointButoirTriangle, pointCible, pointBilleGratuite, pointCampagne, difficulte);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn const int FacadeModele::getPointButoirCercle() 
///
/// Cette fonction obtient le pointage d'un butoir circulaire.
///
/// @return le pointage d'un butoir circulaire.
///
////////////////////////////////////////////////////////////////////////
int FacadeModele::getPointButoirCercle() const
{
	return editeur_->getPointButoirCercle();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const int FacadeModele::getPointButoirTriangle() 
///
/// Cette fonction obtient le pointage d'un butoir triangulaire.
///
/// @return le pointage d'un butoir triangulaire.
///
////////////////////////////////////////////////////////////////////////
int FacadeModele::getPointButoirTriangle() const
{
	return editeur_->getPointButoirTriangle();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const int FacadeModele::getPointCible() 
///
/// Cette fonction obtient le pointage d'une cible.
///
/// @return le pointage d'une cible.
///
////////////////////////////////////////////////////////////////////////
int FacadeModele::getPointCible() const
{
	return editeur_->getPointCible();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const int FacadeModele::getPointBilleGratuite() 
///
/// Cette fonction obtient le pointage nécessaire pour recevoir une bille gratuite.
///
/// @return le pointage nécessaire pour recevoir une bille gratuite.
///
////////////////////////////////////////////////////////////////////////
int FacadeModele::getPointBilleGratuite() const
{
	return editeur_->getPointBilleGratuite();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn const int FacadeModele::getPointCampagne() 
///
/// Cette fonction obtient le pointage nécessaire pour passer au 
/// prochain niveau lors du mode campagne.
///
/// @return Le pointage nécessaire pour passer au 
/// prochain niveau lors du mode campagne.
///
////////////////////////////////////////////////////////////////////////
int FacadeModele::getPointCampagne() const
{
	return editeur_->getPointCampagne();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const int FacadeModele::getDifficulte() 
///
/// Cette fonction obtient le niveau de difficulté.
///
/// @return Le niveau de difficulté.
///
////////////////////////////////////////////////////////////////////////
int FacadeModele::getDifficulte() const
{
	return editeur_->getDifficulte();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn std::string FacadeModele::obtenirCheminAbsolu()
///
/// Cette fonction obtient le chemin absolu
///
///
/// @return Le chemin absolu
///
////////////////////////////////////////////////////////////////////////
string FacadeModele::obtenirCheminAbsolu() const
{
	return CHEMIN_ABSOLU;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ouvrirPartieRapide 
///
/// Cette fonction est appelee lors de la creation d'une fenetre en mode jeu pour que openGL s'affiche bien.
///
/// @param[in] nbJoueurs : Nombre de joueurs
/// @param[in] humain : Si le 2eme joueur sera un humain ou une AI
/// @param[in] chemin : Chemin vers le fichier a ouvrir
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::ouvrirPartieRapide(int nbJoueurs, bool humain, string chemin)
{
	delete partie_;
	partie_ = new PartieRapide(nbJoueurs, humain, chemin);
	Affichage::obtenirInstance()->reinitLumiere();
	NetworkManager::getInstance()->jeuHorsLigne();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ouvrirPartieCampagne 
///
/// Cette fonction est appelee lors de la creation d'une fenetre en mode campagne pour que openGL s'affiche bien.
///
/// @param[in] nbJoueurs : Nombre de joueurs
/// @param[in] humain : Si le 2eme joueur sera un humain ou une AI
/// @param[in] chemin : Chemin vers le fichier a ouvrir
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::ouvrirPartieCampagne(int nbJoueurs, bool humain, string chemin)
{
	delete partie_;
	partie_ = new PartieCampagne(nbJoueurs, humain, chemin);
	Affichage::obtenirInstance()->reinitLumiere();
	NetworkManager::getInstance()->jeuHorsLigne();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ouvrirPartieTest
///
/// Cette fonction est appelee lors de la creation d'une fenetre d'editeur (mode test) pour que openGL s'affiche bien.
///
/// @param[in] chemin : Chemin vers le fichier a ouvrir
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::ouvrirPartieTest(string chemin)
{
	delete partie_;
	partie_ = new PartieBase(chemin);
	editeur_->assignerZoneDeJeu(partie_->obtenirZoneDeJeu());

	NetworkManager::getInstance()->jeuHorsLigne();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void demarrerPartie
///
///Démarre une partie
///
/// @param[in] aucun
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::demarrerPartie() const
{
	return partie_->demarrerPartie();
}

string FacadeModele::obtenirNomZone() const
{
	return partie_->obtenirNomZone();
}

void FacadeModele::changerCamera(bool orbite)
{
	Affichage::obtenirInstance()->changerCamera(orbite);
}

#pragma region Edition

////////////////////////////////////////////////////////////////////////
///
/// @fn int FacadeModele::selectionner(int x, int y, int longueur, int largeur, bool ajout)
///
/// Cette fonction effectue la sélection d'un objet.
///
/// @param[in] x : la position centrale en x de la zone à sélectionner.
/// @param[in] y : la position centrale en y de la zone à sélectionner.
/// @param[in] longueur : la longueur de la zone à sélectionner.
/// @param[in] largeur : la largeur de la zone à sélectionner.
/// @param[in] ajout : spécifie si l'objet à sélectionner est ajouter ou non
///					   à la sélection courante.
///
/// @return le nombre d'objets sélectionnés.
///
////////////////////////////////////////////////////////////////////////
int FacadeModele::selectionner(int x, int y, int longueur, int largeur, bool ajout) const
{
	return editeur_->selectionner(x, y, longueur, largeur, ajout);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool FacadeModele::creerObjet(std::string objetACreer, int x, int y)
///
/// Cette fonction crée un objet à une position précise.
///
/// @param[in] objetACreer : le nom de l'objet à créer.
/// @param[in] x : position en x de l'objet à créer.
/// @param[in] y : position en y de l'objet à créer.
///
/// @return Si l'objet a été créé ou non.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::creerObjet(const string objetACreer, int x, int y) const
{
	auto succes =  editeur_->creerObjet(objetACreer, x, y);
	if(succes && ArbreRenduINF2990::estNomDePalette(objetACreer))
	{
		partie_->reassignerPalettes();
	}
	return succes;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::supprimerSelection()
///
/// Cette fonction supprime la sélection.
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::supprimerSelection() const
{
	editeur_->supprimerSelection();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::annulerCreation()
///
/// Cette fonction annule la creation d'un mur ou d'un portail.
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::annulerCreation() const
{
	editeur_->annulerCreation();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::murFantome(int x, int y)
///
/// Cette fonction gère le deuxième clic lors de la création d'un mur. 
///
/// @param[in] x : position en x du deuxième clic lors de la création d'un mur.
/// @param[in] y : position en y du deuxième clic lors de la création d'un mur.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::murFantome(int x, int y) const
{
	editeur_->murFantome(x, y);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::deplacerSelection(int x1, int x2, int y1, int y2, bool force)
///
/// Cette fonction déplace la sélection d'une position spécifique à 
/// une autre à l'intérieur dze la zone bornée ou non.
///
/// @param[in] x1 : position initiale en x de la sélection.
/// @param[in] x2 : position finale en x de la sélection.
/// @param[in] y1 : position initiale en y de la sélection.
/// @param[in] y2 : position finale en y de la sélection.
/// @param[in] force : déplacement limité par les bornes de la table ou non.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::deplacerSelection(int x1, int x2, int y1, int y2, bool force) const
{
	editeur_->deplacerSelection(x1, x2, y1, y2, force);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::redimensionnerSelection(int y1, int y2)
///
/// Ctte fonction permet de redimensionner la sélection selon un
/// ration calculé avec les positions initiale et finale en y de la 
/// sélection et en faisant appel à un visiteur.
///
/// @param[in] y1 : position initiale en y de la sélection.
/// @param[in] y2 : position finale en y de la sélection.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::redimensionnerSelection(int y1, int y2) const
{
	editeur_->redimensionnerSelection(y1, y2);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::rotaterSelection(int y1, int y2)
///
/// Cette fonction effectue la rotation de la sélection en faisant 
/// appel à un visiteur et en calculant un ration selon les 
/// positions initiale et finale en y de la sélection.
///
/// @param[in] y1 : position initiale en y de la sélection.
/// @param[in] y2 : position finale en y de la sélection.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::rotaterSelection(int y1, int y2) const
{
	editeur_->rotaterSelection(y1, y2);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::assignerCentreSelection()
///
/// Cette fonction sert a actualiser le centre de la selection
/// pour l'affichage
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::assignerCentreSelection() const
{
	editeur_->assignerCentreSelection();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool FacadeModele::initDuplication(int x, int y) 
///
/// Cette fonction initialise la duplication d'un objet.
///
/// @param[in] x : la position en x de l'objet à dupliquer.
/// @param[in] y : la position en y de l'objet à dupliquer.
///
/// @return Si la duplication a été effectuée avec succès ou non.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::initDuplication(int x, int y) const
{
	return editeur_->initDuplication(x, y);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn const double FacadeModele::obtenirEchelleSelection() 
///
/// Cette fonction obtient l'échelle d'agrandissement de l'objet courant.
///
/// @return L'échelle d'agrandissement de l'objet courant.
///
////////////////////////////////////////////////////////////////////////
double FacadeModele::obtenirEchelleSelection() const
{
	return editeur_->obtenirEchelleSelection();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const double FacadeModele::obtenirRotationSelection() 
///
/// Cette fonction obtient le degré de rotation de l'objet courant.
///
/// @return Le degré de rotation de l'objet courant.
///
////////////////////////////////////////////////////////////////////////
double FacadeModele::obtenirRotationSelection() const
{
	return editeur_->obtenirRotationSelection();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const double FacadeModele::obtenirPosXSel() 
///
/// Cette fonction obtient la position relative en x de la sélection.
///
/// @return La position relative en x de la sélection.
///
////////////////////////////////////////////////////////////////////////
double FacadeModele::obtenirPosXSel() const
{
	return editeur_->obtenirPosXSel();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const double FacadeModele::obtenirPosYSel() 
///
/// Cette fonction obtient la position relative en y de la sélection.
///
/// @return la position relative en y de la sélection.
///
////////////////////////////////////////////////////////////////////////
double FacadeModele::obtenirPosYSel() const
{
	return editeur_->obtenirPosYSel();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::finirDuplication() 
///
/// Cette fonction termine la duplication en retirant les étampes et 
/// enlevant la sélection des objets à dupliquer.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::finirDuplication() const
{
	editeur_->finirDuplication();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::assignerSelPosition(double x, double y) 
///
/// Cette fonction assigne la position relative à la sélection.
///
/// @param[in] x : la position en x.
/// @param[in] y : la position en y.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::assignerSelPosition(double x, double y) const
{
	editeur_->assignerSelPosition(x, y);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::finirCreationMur()
///
/// Cette fonction finit la création d'un mur
///
///
/// @return Le chemin absolu
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::finirCreationMur() const
{
	editeur_->finirCreationMur();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool FacadeModele::positionDansBornes(double x, double y) 
///
/// Cette fonction vérifie si la position donnée est dans les bornes de la table de jeu, 
/// pour l'affichage du curseur.
///
/// @param[in] x : la position en x.
/// @param[in] y : la position en y.
///
/// @return Si la position donnée est dans les bornes de la table de jeu.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::positionDansBornes(int x, int y) const
{
	return editeur_->positionDansBornes(x, y);
}

#pragma endregion


#pragma region Campagneshit
//A deplacer dans le C# ou au moins 


////////////////////////////////////////////////////////////////////////
///
/// @fn void obtenirFichiers
///
/// Retourne le nom du fichier xml d'une campagne avec l'id specifie.
/// Est appellee par MenuPrincipalParametrePartieCampagne pour avoir la liste des fichiers xml lus.
///
/// @param[in] keycode : le code de la touche
///
////////////////////////////////////////////////////////////////////////
string FacadeModele::obtenirFichierCampagne(int i) const
{
	return static_cast<PartieCampagne *>(partie_)->obtenirNomFichierXml(i);
}


bool FacadeModele::getEstHumainCampagne() const
{
	return static_cast<PartieCampagne *>(partie_)->obtenirEstHumain();
}

int FacadeModele::getNbFichiersCampagne() const
{
	return static_cast<PartieCampagne *>(partie_)->obtenirNbFichiers();
}

bool FacadeModele::getNbJoueursCampagne() const
{
	return static_cast<PartieCampagne *>(partie_)->obtenirNbJoueurs() == 2;
}

bool FacadeModele::envoyerNomFichierCampagne(char* nomFichier, int longueur) const
{
	return static_cast<PartieCampagne *>(partie_)->envoyerNomFichier(nomFichier, longueur);
}

bool FacadeModele::obtenirPartieEstTerminee() const
{
	return partie_->obtenirEstTerminee();
}

bool FacadeModele::obtenirPartieEstGagnee() const
{
	return partie_->obtenirEstGagnee();
}

void FacadeModele::finReceptionNomFichiersCampagne() const
{
	static_cast<PartieCampagne *>(partie_)->finReceptionNomFichiers();
}

void FacadeModele::assignerTypeJoueursCampagne(int nbJoueurs, bool humain) const
{
	static_cast<PartieCampagne *>(partie_)->recevoirJoueursHumain(nbJoueurs, humain);
}

bool FacadeModele::obtenirRondeGagnee() const
{
	return partie_->obtenirRondeGagnee();
}

int FacadeModele::obtenirDifficulteZone() const
{
	return partie_->obtenirDifficulteZone();
}

int FacadeModele::obtenirPointsAAtteindre() const
{
	return partie_->obtenirPointsAAtteindre();
}
#pragma endregion
void FacadeModele::creerPartieCampagneEnLigne(bool estCoop, vector<string> playerIds, vector<string> zonePaths, int nbAi, bool estHost, string matchId, bool estRejoin)
{
	delete partie_;
	NetworkManager::getInstance()->joinMatch(matchId, estHost, !estCoop);
	//new partie campagne
	if (estCoop)
		partie_ = new PartieCampagne(playerIds, zonePaths, nbAi, estHost, matchId, estRejoin);
	else
		partie_ = new PartieCampagneCompetitive(playerIds, zonePaths, nbAi, estHost, matchId, estRejoin);
}

void FacadeModele::creerPartieSimpleEnLigne(bool estCoop, vector<string> playerIds, string zone, int nb_ai, bool estHost, string matchId, bool estRejoin)
{
	delete partie_;
	NetworkManager::getInstance()->joinMatch(matchId, estHost, !estCoop);
	if (estCoop)
		partie_ = new PartieRapide(playerIds, zone, nb_ai, estHost, matchId, estRejoin);
	else
		partie_ = new PartieRapideCompetitive(playerIds, zone, nb_ai, estHost, matchId, estRejoin);
}

int FacadeModele::obtenirPointageJoueur(int joueurNum) const
{
	return partie_->obtenirScoreJoueur(joueurNum);
}

int FacadeModele::obtenirMonPointage() const
{
	return partie_->obtenirMonPointage();
}

int FacadeModele::obtenirMonNbBille() const
{
	return partie_->obtenirMonNbBilles();
}

int FacadeModele::obtenirMeilleurPointage() const
{
	return partie_->obtenirMeilleurPointage();
}

int FacadeModele::obtenirBillesRestantesJoueur(int joueurNum) const
{
	return static_cast<PartieRapide*>(partie_)->obtenirBillesRestantesJoueur(joueurNum);
}

int FacadeModele::obtenirNiveauCarte() const
{
	return editeur_->getNiveauCarte();
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

