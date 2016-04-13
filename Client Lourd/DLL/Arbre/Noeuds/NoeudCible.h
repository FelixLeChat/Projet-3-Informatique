#ifndef __ARBRE_NOEUDS_NOEUDCible_H__
#define __ARBRE_NOEUDS_NOEUDCible_H__
#include "Event/IEventSubscriber.h"
#include "Arbre/Noeuds/NoeudAbstrait.h"


class NoeudCible : public NoeudAbstrait, public IEventSubscriber{
public:
	/// Constructeur.
	NoeudCible(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudCible();

	/// Accepte le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	/// Test et execute une collision avec une bille
	void executerCollision(NoeudBille* bille);
	double obtenirEnfoncement(NoeudBille* bille);
	///Reinitialise le ressort
	void reinitialiser();
	/// Calcule les bornes de la table de jeu
	virtual void calculerBornes();

	void update(IEvent* e);

	bool obtenirEstActif() const;

private:
	void executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4);
	bool actif_;

	///Quatre points composant le mur
	glm::dvec3 points[4];
};

#endif //__ARBRE_NOEUDS_NOEUDCible_H__