

#ifndef __ARBRE_NOEUDS_NoeudGenerateurBille_H__
#define __ARBRE_NOEUDS_NoeudGenerateurBille_H__

#include "NoeudAbstrait.h"


class NoeudGenerateurBille : public NoeudAbstrait{
public:
	/// Constructeur.
	NoeudGenerateurBille(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudGenerateurBille();

	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

};

#endif //__ARBRE_NOEUDS_NoeudGenerateurBille_H__