#ifndef __GAMELOGIC_ZONEDEJEU_H__
#define __GAMELOGIC_ZONEDEJEU_H__

#include <vector>
#include "Arbre/Noeuds/NoeudBille.h"
#include "Arbre/QuadTree.h"
#include "Arbre/ArbreRenduINF2990.h"
#include "Arbre/Visiteur/VisiteurObtenirControles.h"
#include "Event/NewBallEvent.h"
#include "Event/CibleSyncEvent.h"
#include "Event/PowerUpSyncEvent.h"

using namespace std;

class ZoneDeJeu
{
public:
	ZoneDeJeu() = default;
	ZoneDeJeu(string path);
	~ZoneDeJeu();

	void animer(double dt);
	
	void perdreBille(NoeudBille* bille);
	void perdreBille(int billeId);
	NoeudBille* lancerBille(bool locale = true);
	NoeudBille* lancerBille(int id, glm::dvec3 pos, glm::dvec2 vitesse, bool locale, int playerNum = 0);
	void reinitialiser();
	void afficher() const;

	ArbreRenduINF2990* obtenirArbre();
	VisiteurObtenirControles obtenirControles();



	/// Assigner le pointage du butoir circulaire
	void setPointButoirCercle(const int);
	/// Assigner le pointage du butoir triangulaire
	void setPointButoirTriangle(const int);
	/// Assigner le pointage des cibles
	void setPointCible(const int);
	/// Assigner le pointage nécessaire pour obtenir une bille gratuite
	void setPointBilleGratuite(const int);
	/// Assigner le pointage pour avancer d'un niveau en mode campagne
	void setPointCampagne(const int);
	/// Assigner le niveau de difficulté
	void setDifficulte(const int);


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

	string obtenirChemin() const;

	bool load();

	bool saveZoneDeJeu(string path);

	vector<NoeudPalette*> dupliquerPalettes(vector<NoeudPalette*> palettes);

	//Pour Sync Global
	vector<NewBallEvent> obtenirEtatBilles() const;
	vector<CibleSyncEvent> obtenirEtatCibles() const;
	vector<PowerUpSyncEvent> obtenirEtatPowerUps() const;

	//Fait en sorte que les billes des gens disconnectes soient locales si host
	void assignerBillesLocales(int player_num, bool b) const;
private:
	ArbreRenduINF2990* arbre_{ nullptr };
	string path_;


	/// Contient les pointeurs vers les billes en jeu a effacer
	vector<NoeudBille *> billesAEffacer_;
	/// Contient les pointeurs vers les billes en jeu
	vector<NoeudBille *> billesEnJeu_;


	/// Propriétés
	/// Le pointage d'un butoir circulaire
	int pointButoirCercle_;
	/// Le pointage d'un butoir triangulaire
	int pointButoirTriangle_;
	/// Le pointage d'une cible
	int pointCible_;
	/// Le pointage nécessaire pour obtenir une bille gratuite
	int pointBilleGratuite_;
	/// Le pointage pour avancer d'un niveau en mode campagne
	int pointCampagne_;
	/// Le niveau de difficulté
	int difficulte_;

	/// QuadTree gerant avec quels objets les billes doivent tester les collisions
	QuadTree quadTree_;
	///Fonction recursive servant a avoir une physique de karité. Retourne le dt reelement utilise
	double animerRecursif(double dt);
	///Recursion maximal
	int conteurDeRecursion;
};




#endif //__GAMELOGIC_ZONEDEJEU_H__