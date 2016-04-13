

#ifndef __ARBRE_NOEUDS_NoeudTrou_H__
#define __ARBRE_NOEUDS_NoeudTrou_H__

#include "NoeudAbstrait.h"


class NoeudTrou : public NoeudAbstrait{
public:
	/// Constructeur.
	NoeudTrou(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudTrou();


	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);
	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Test et exécute les collisions
	void executerCollision(NoeudBille* bille);
	double obtenirEnfoncement(NoeudBille* bille);

private:
	double obtenirRayon();
	double rayon_;

};

#endif //__ARBRE_NOEUDS_NoeudTrou_H__