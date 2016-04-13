#ifndef __GAMELOGIC_PARTIE_BASE_H__
#define __GAMELOGIC_PARTIE_BASE_H__

#include "../Event/IEventSubscriber.h"
#include "../Joueurs/IJoueurManager.h"
#include "../GameLogic/ZoneDeJeu.h"

class PartieBase : public IEventSubscriber
{
public:
	PartieBase() {}

	PartieBase(string chemin);
	virtual ~PartieBase();

	virtual void afficher();
	/// Démarrage de la partie
	virtual bool demarrerPartie();
	/// Lancer de la bille
	virtual NoeudBille* lancerBille();
	/// Gère la perte d'une bille
	virtual void perdreBille();
	/// Reinitialise la partie
	virtual void reinitialiser();
	/// Gère les changements liés au temps de la partie
	virtual void tick(double dt);

	virtual bool obtenirEstTerminee() const;

	virtual void update(IEvent* e);

	virtual int obtenirScoreJoueur(int joueur) const;
	virtual int obtenirBillesRestantesJoueur(int joueur_num) const;

	ZoneDeJeu* obtenirZoneDeJeu() const;
	virtual int obtenirMeilleurPointage();
	virtual int obtenirMonPointage();
	virtual int obtenirMonNbBilles();
	void reassignerPalettes() const;

	//Pour afficher les informations des campagnes
	virtual bool obtenirRondeGagnee() const;
	virtual int obtenirDifficulteZone() const;
	virtual int obtenirPointsAAtteindre() const;
	string obtenirNomZone() const;
	virtual bool obtenirEstGagnee();
protected:

	ZoneDeJeu* zoneCourante_;
	double tempsAvantProchaineBille_;			// Le temps restant avant l'obtention d'une seconde bille
	bool estTerminee_;
	int nbBillesEnJeu_;
	IJoueurManager* joueurManager_;
};

#endif // !__GAMELOGIC_PARTIE_BASE_H__
