#ifndef __ARBRE_NOEUDS_TRONE_H__
#define __ARBRE_NOEUDS_TRONE_H__

#include "Arbre/Noeuds/NoeudComposite.h"
#include "NoeudPowerUp.h"
#include "../../../Event/PowerUpSyncEvent.h"


class NoeudPlateauDArgent : public NoeudComposite, public IEventSubscriber
{
public:
	/// Constructeur.
	NoeudPlateauDArgent(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPlateauDArgent();

	/// Accepte le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	void activerPowerUp(PowerUpType choixPowerUp);
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	void donnerPowerUp();

	void update(IEvent* e);
	void reinitialiser();
	PowerUpType obtenirTypePowerUp() const;
private:
	void creerPowerUp();

	double intervalle_;
	double tempsDepuisPowerUpObtenu_;

	bool powerUpDisponible_;

	PowerUpType powerUpActif_;
};

#endif //__ARBRE_NOEUDS_TRONE_H__