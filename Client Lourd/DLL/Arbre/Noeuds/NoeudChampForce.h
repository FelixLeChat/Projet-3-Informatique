#ifndef __ARBRE_NOEUDS_CHAMPFORCE_H__
#define __ARBRE_NOEUDS_CHAMPFORCE_H__

#include "NoeudComposite.h"


class NoeudChampForce : public NoeudComposite {
public:
	/// Constructeur.
	NoeudChampForce(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudChampForce();

	/// Accepte le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	/// Test et execute une collision avec une bille
	void appliquerForceConstante(NoeudBille* bille);

	virtual void calculerBornes();
	virtual const double obtenirRotation();
	virtual void assignerRotation(double rotation);

private:
	void collision(NoeudBille* bille);
	double force_;
	///Quatre points composant le champ
	glm::dvec3 points[4];
};

#endif //__ARBRE_NOEUDS_CHAMPFORCE_H__