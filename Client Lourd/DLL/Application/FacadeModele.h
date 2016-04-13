//////////////////////////////////////////////////////////////////////////////
/// @file FacadeModele.h
/// @author DGI
/// @date 2005-06-15
/// @version 1.0 
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_FACADEMODELE_H__
#define __APPLICATION_FACADEMODELE_H__


#include <string>
#include "../Partie/PartieBase.h"
#include "../Editeur/Editeur.h"

class NoeudAbstrait;

using namespace std;

///////////////////////////////////////////////////////////////////////////
/// @class FacadeModele
/// @brief Classe qui constitue une interface (une façade) sur l'ensemble
///        du modèle et des classes qui le composent.
///
/// @author Martin Bisson
/// @date 2007-02-20
///////////////////////////////////////////////////////////////////////////
class FacadeModele
{
public:

	/// Obtient l'instance unique de la classe.
	static FacadeModele* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	/// Crée un contexte OpenGL et initialise celui-ci.
	void initialiserOpenGL(HWND hWnd, char*, int);
	/// Initialise l'arbre pour des tests
	void initialiserPourTests() const;


	/// Charge la configuration à partir d'un fichier XML.
	void chargerConfiguration() const;
	/// Enregistre la configuration courante dans un fichier XML.
	void enregistrerConfiguration() const;
	/// Charge un fichier XML.
	bool chargerFichierXML(char* nomFichier, int longueur);
	/// Enregistre un fichier XML.
	bool enregistrerFichierXML(char* nomFichier, int longueur) const;


	/// Libère le contexte OpenGL.
	void libererOpenGL();
	void libererPourTests();
	/// Affiche le contenu du modèle.
	void afficher() const;

	/// Annuler la creation d'un mur ou d'un portail
	void annulerCreation() const;

	/// Réinitialise la scène.
	void reinitialiser() const;

	// Arreter les sons joués
	void arreterSons() const;

	/// Creer un nouvel objet sur la scène
	bool creerObjet(string objetACreer, int x, int y) const;


	/// Mur fantome
	void murFantome(int x, int y) const;


	///Deplacer avec option de ne pas forcer les bornes
	void deplacerSelection(int x1, int x2, int y1, int y2, bool force) const;


	/// Redimensionner une selection
	void redimensionnerSelection(int y1, int y2) const;

	/// Rotation d'une selection
	void rotaterSelection(int y1, int y2) const;

	/// Assignation du centre de selection
	void assignerCentreSelection() const;

	///Sélectionner
	int selectionner(int x, int y, int longueur, int largeur, bool ajout) const;

	/// Mettre à jour les nouvelles propriétés
	void mettreAJourProprietes(int pointButoirCercle, int pointButoirTriangle, int pointCible, int pointBilleGratuite, int pointCampagne, int difficulte) const;

	/// Supprime la sélection
	void supprimerSelection() const;

	/// Initialise la duplication
	bool initDuplication(int x, int y) const;

	/// Termine la duplication
	void finirDuplication() const;

	///Méthodes d'obtention des différents pointages
	int getPointBilleGratuite() const;
	int getPointButoirCercle() const;
	int getPointButoirTriangle() const;
	int getPointCampagne() const;
	int getPointCible() const;

	/// Obtient le niveau de difficulté
	int getDifficulte() const;

	/// Méthodes d'obtention des modifications effectuées sur les objets
	double obtenirEchelleSelection() const;
	double obtenirRotationSelection() const;
	double obtenirPosXSel() const;
	double obtenirPosYSel() const;

	/// Assigne la position relative à la sélection
	void assignerSelPosition(double x, double y) const;
	/// Vérifie que la position passée est dans les bornes de la table de jeu
	bool positionDansBornes(int x, int y) const;
	string obtenirCheminAbsolu() const;
	//finit la creation d'un mur
	void finirCreationMur() const;


	//Fonctions appelees depuis le menu principal pour que openGL affiche la bonne zone de jeu
	void ouvrirPartieRapide(int, bool, string);
	void ouvrirPartieCampagne(int, bool, string);
	void ouvrirPartieTest(string);

	//Démarre la partie
	bool demarrerPartie() const;
	string obtenirNomZone() const;
	//change la vue
	static void changerCamera(bool orbite);

	void creerPartieCampagneEnLigne(bool estCoop, vector<string> playerIds, vector<string> zonePaths, int nbAi, bool estHost, string matchId, bool estRejoin);
	void creerPartieSimpleEnLigne(bool estCoop, vector<string> playerIds, string zonePath, int nb_ai, bool estHost, string matchId, bool estRejoin);
	int obtenirPointageJoueur(int joueurNum) const;
	int obtenirMonPointage() const;
	int obtenirMonNbBille() const;
	int obtenirMeilleurPointage() const;
	int obtenirBillesRestantesJoueur(int joueurNum) const;

	int obtenirNiveauCarte() const;


	//FONCTIONS A DEPLACER DANS LE C#
	//Fonctions appellees depuis la classe MenuPrincipalParametrePartieCampagne pour obtenir les fichiers xml charges de xml.
	string obtenirFichierCampagne(int i) const;
	bool getEstHumainCampagne() const;
	int getNbFichiersCampagne() const;
	bool getNbJoueursCampagne() const;
	bool envoyerNomFichierCampagne(char* nomFichier, int longueur) const;
	bool obtenirPartieEstTerminee() const;
	bool obtenirPartieEstGagnee() const;
	void finReceptionNomFichiersCampagne() const;
	void assignerTypeJoueursCampagne(int nbJoueurs, bool humain) const;



	//Pour afficher les informations des campagnes
	bool obtenirRondeGagnee() const;
	int obtenirDifficulteZone() const;
	int obtenirPointsAAtteindre() const;
private:
	/// Constructeur par défaut.
	FacadeModele() = default;
	/// Destructeur.
	~FacadeModele();
	/// Constructeur copie désactivé.
	FacadeModele(const FacadeModele&) = delete;
	/// Opérateur d'assignation désactivé.
	FacadeModele& operator =(const FacadeModele&) = delete;

	/// Nom du fichier XML dans lequel doit se trouver la configuration.
	static const string FICHIER_CONFIGURATION;

	/// Pointeur vers l'instance unique de la classe.
	static FacadeModele* instance_;

	Editeur* editeur_{nullptr};
	/// Partie actuelle
	PartieBase* partie_{nullptr};

	/// Contient le chemin absolu vers le .exe (transmis par le c#)
	string CHEMIN_ABSOLU;
};


#endif // __APPLICATION_FACADEMODELE_H__



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
