#ifndef __ARBRE_NOEUDS_POWERUP_H__
#define __ARBRE_NOEUDS_POWERUP_H__

#include "Arbre/Noeuds/NoeudAbstrait.h"
#include "Arbre/Noeuds/NoeudBille.h"

class NoeudPowerUp : public NoeudAbstrait
{
public:
	/// Constructeur.
	NoeudPowerUp(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPowerUp();

	/// Accepte le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	/// Test et execute une collision avec une bille
	virtual void executerCollision(NoeudBille* bille);
	virtual double obtenirEnfoncement(NoeudBille* bille);
	void afficherSelectionne() const override;

	void activer();
	void desactiver();
protected:
	virtual void appliquerPowerUp(NoeudBille*) = 0;
	virtual void retirerPowerUp() = 0;

private:
	double obtenirRayon();
	void collision(NoeudBille* bille);

	bool active_;

	bool estObtenu_;
	double tempsDepuisObtention_;
	double duree_;

	double rayon_;
};

#endif //__ARBRE_NOEUDS_POWERUP_H__