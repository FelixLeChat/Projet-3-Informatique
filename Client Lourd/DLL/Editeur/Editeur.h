#ifndef __EDITEUR_H__
#define __EDITEUR_H__
#include "../GameLogic/ZoneDeJeu.h"

class Editeur
{
public:
	Editeur();
	~Editeur();

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
	int selectionner(int x, int y, int longueur, int largeur, bool ajout);

/// Supprime la sélection
	void supprimerSelection();

/// Initialise la duplication
	bool initDuplication(int x, int y) const;

/// Termine la duplication
	void finirDuplication() const;

//finit la creation d'un mur
	void finirCreationMur() const;

/// Annuler la creation d'un mur ou d'un portail
	void annulerCreation() const;

/// Assigne la position relative à la sélection
	void assignerSelPosition(double x, double y) const;


/// Méthodes d'obtention des modifications effectuées sur les objets
	double obtenirEchelleSelection() const;
	double obtenirRotationSelection() const;
	double obtenirPosXSel() const;
	double obtenirPosYSel() const;

	bool positionDansBornes(int x, int y) const;

	void assignerZoneDeJeu(ZoneDeJeu* zone);

	bool enregistrerFichierXML(char* nomFichier, int longueur) const;

	void mettreAJourProprietes(int pointButoirCercle, int pointButoirTriangle, int pointCible, int pointBilleGratuite, int pointCampagne, int difficulte) const;


/// Obtenir le pointage d'un butoir circulaire
	int getPointButoirCercle() const;
/// Obtenir le pointage d'un butoir triangulaire
	int getPointButoirTriangle() const;
/// Obtenir le pointage d'une cible
	int getPointCible() const;
/// Obtenir le pointage nécessaire pour obtenir une bille gratuite
	int getPointBilleGratuite() const;
/// Obtenir le pointage pour avancer d'un niveau en mode campagne
	int getPointCampagne() const;
/// Obtenir le niveau de difficulté
	int getDifficulte() const;

	int getNiveauCarte() const;

private:
	ZoneDeJeu* zone_;
/// Le noeud sélectionné
	NoeudAbstrait * noeudSel_{ nullptr };
};

#endif //__EDITEUR_H__
