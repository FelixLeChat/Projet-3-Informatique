#ifndef __ARBRE_NOEUDS_NOEUDMUR_H__
#define __ARBRE_NOEUDS_NOEUDMUR_H__

#include "NoeudAbstrait.h"
#include "glm\glm.hpp"


class NoeudMur : public NoeudAbstrait{
public:
	/// Constructeur.
	NoeudMur(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudMur();

	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Assigne la position relative du noeud par rapport a son parent
	virtual void assignerPositionRelative(const glm::dvec3&);
	/// Assigne le deuxième point du mur
	void assignerDeuxiemePoint(double x, double y);
	/// Obtient le deuxième point du mur
	glm::dvec3 obtenirDeuxiemePoint();
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);
	/// Calcule les bornes de la table de jeu
	virtual void calculerBornes();
	/// Obtient la longueur du mur
	double obtenirLongueur();
	/// Assigne la longueur du mur
	void assignerLongueur(double longueur);
	/// Teste et execute la collision avec une bille
	void executerCollision(NoeudBille* bille);
	double obtenirEnfoncement(NoeudBille* bille);
	/// Indique si la creation est terminee
	bool obtenirCreationTerminee();
	/// Termine la creation d'un mur
	void terminerCreation();


private:
	/// Le premier point du mur
	glm::dvec3 premierPoint_;
	/// Le deuxième point du mur
	glm::dvec3 deuxiemePoint_;

	///Quatre points composant le mur
	glm::dvec3 points[4];
	/// La longueur du mur
	double longueur_;
	/// Si le point en cours est le premier point assigné ou pas
	bool premierPointAssigne_;
	///  True si la creation est terminee
	bool creationTerminee;
	/// Execute une collision en tenant compte des quatres segments
	void executerCollisionAvecSegments(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4);
};
#endif //__ARBRE_NOEUDS_NOEUDMUR_H__