#ifndef __ARBRE_NOEUDS_NOEUDPORTAIL_H__
#define __ARBRE_NOEUDS_NOEUDPORTAIL_H__

#include "NoeudAbstrait.h"


class NoeudPortail : public NoeudAbstrait{
public:
	/// Constructeur.
	NoeudPortail(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPortail();

	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Affiche le véritable rendu d'un objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);
	/// Obtient le frère du noeud portail
	NoeudPortail* obtenirFrere();
	/// Assigne un frère au noeud portail
	void assignerFrere(NoeudPortail *);
	/// Teste et execute la collision avec une bille
	void executerCollision(NoeudBille* bille);
	double obtenirEnfoncement(NoeudBille* bille);
	virtual void appliquerForceConstante(NoeudBille* bille);

	void desactiver(NoeudBille* bille);
	
	/// Reinitialise le portail 
	void reinitialiser();

private:
	/// Pointeur vers le portail frere
	NoeudPortail * frere;
	/// Compteur pour la desactivation du portail
	bool actif_;
	double rayon_;
	NoeudBille* bille_;

	double obtenirRayon();
};

#endif //__ARBRE_NOEUDS_NOEUDPORTAIL_H__