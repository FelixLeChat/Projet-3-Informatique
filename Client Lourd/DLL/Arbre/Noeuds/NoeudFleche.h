#ifndef __ARBRE_NOEUDS_FLECHE_H__
#define __ARBRE_NOEUDS_FLECHE_H__

#include "NoeudAbstrait.h"


class NoeudFleche : public NoeudAbstrait {
public:
	/// Constructeur.
	NoeudFleche(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudFleche();

	/// Accepte le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	bool inverserSelection() override;

	void afficherSelectionne() const override;

};

#endif //__ARBRE_NOEUDS_FLECHE_H__