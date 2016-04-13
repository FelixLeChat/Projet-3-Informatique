#ifndef __ARBRE_NOEUDS_NOEUDButoirCirculaire_H__
#define __ARBRE_NOEUDS_NOEUDButoirCirculaire_H__

#include "NoeudAbstrait.h"

class NoeudButoirCirculaire : public NoeudAbstrait{
public:
	/// Constructeur.
	NoeudButoirCirculaire(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudButoirCirculaire();

	/// Accepter le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Effectuer le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	/// executer une collision avec une bille
	virtual void executerCollision(NoeudBille* bille);
	///obtient les informations de la collision
	virtual double obtenirEnfoncement(NoeudBille* bille);

	void assignerAllumer(bool allume);
	bool estAllume() const;
private:
	double obtenirRayon();
	double rayon_;
	/// Indique si le butoir est allume
	bool allumer;
	/// Represente de le delai avant que le butoir ne se ferme
	float delai;

};

#endif //__ARBRE_NOEUDS_NOEUDButoirCirculaire_H__