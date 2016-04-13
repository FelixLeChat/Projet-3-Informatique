#ifndef __ARBRE_NOEUDS_NOEUDButoirTriangulaireGauche_H__
#define __ARBRE_NOEUDS_NOEUDButoirTriangulaireGauche_H__

#include "NoeudAbstrait.h"


class NoeudButoirTriangulaireGauche : public NoeudAbstrait{
public:
	/// Constructeur.
	NoeudButoirTriangulaireGauche(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudButoirTriangulaireGauche();

	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	///Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	/// executer une collision avec une bille
	virtual void executerCollision(NoeudBille* bille);
	///obtient les informations de la collision
	virtual double obtenirEnfoncement(NoeudBille* bille);
	/// Calcule les bornes de la table de jeu
	virtual void calculerBornes();
private:
	/// Les points des trois segments de droite du butoir
	glm::dvec3 segments[3][2];
	/// Les centres des trois cercles du butoir
	glm::dvec3 cercles[3];
	///Collision avec segment
	void executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, double forceAdditionnelle);
	///Collision avec cercle
	void executerCollisionAvecCercle(NoeudBille* bille, glm::dvec3 position, double rayon, double forceAdditionnelle);

};

#endif //__ARBRE_NOEUDS_NOEUDButoirTriangulaireGauche_H__