

#ifndef __ARBRE_NOEUDS_NOEUDZONEDEJEU_H__
#define __ARBRE_NOEUDS_NOEUDZONEDEJEU_H__

#include "NoeudComposite.h"


class NoeudZoneDeJeu : public NoeudComposite{
public:
	/// Constructeur.
	NoeudZoneDeJeu(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudZoneDeJeu();


	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);
	/// executer une collision avec une bille
	virtual void executerCollision(NoeudBille* bille);
	///obtient les informations de la collision
	virtual double obtenirEnfoncement(NoeudBille* bille);

private:
	/// Collision avec segment
	void executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2);
};

#endif //__ARBRE_NOEUDS_NOEUDZONEDEJEU_H__