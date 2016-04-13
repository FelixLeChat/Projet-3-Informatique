///////////////////////////////////////////////////////////////////////////////
/// @file NoeudBille.h
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-02-24
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDBILLE_H__
#define __ARBRE_NOEUDS_NOEUDBILLE_H__

#include "NoeudAbstrait.h"
#include "Event/IEventSubscriber.h"

///////////////////////////////////////////////////////////////////////////
/// @class NoeudBille
/// @brief Cette classe represente une bille
///
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-02-24
///////////////////////////////////////////////////////////////////////////
class NoeudBille : public NoeudAbstrait, public IEventSubscriber
{
public:
	static int nextBilleId;
	/// Constructeur.
	NoeudBille(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudBille();

	/// Accepter le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	glm::fvec3 obtenirCouleurPalette() const;
	/// Effectuer le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);
	/// Applique une force sur la bille
	void appliquerForce(glm::dvec2 force);
	/// Retourne le rayon de la bille dépendemment du modele et de l'agrandissement
	double obtenirRayon();
	/// Donne le vecteur vitesse courant de la bille
	glm::dvec2 obtenirVitesse();
	/// Donne la collision a la bille
	void setCollision(bool coll, bool avecMur = false);
	/// Teste et execute la collision avec une bille
	void executerCollision(NoeudBille* bille);
	double obtenirEnfoncement(NoeudBille* bille);
	/// assigne une vitesse a la bille
	void assignerVitesse(glm::dvec2 vit);
	void finaliserAnimation(float duree);
	void maximiserVitesse();
	void assignerId(int billeId);
	void assignerLocale(bool locale);
	void update(IEvent* e);
	int obtenirId();
	double obtenirFacteurPointage();
	void assignerFacteurPointage(double facteur);
	int obtenirPlayerNum();
	void assignerPlayerNum(int playerNum);
	void assignerDernierFrappeur(int playerNum);
	int obtenirDernierFrappeur();
	bool estLocale();
	void assignerFantome(bool b);

	virtual void calculerBornes();
private:
	/// L'acceleration de la bille
	glm::dvec2 acceleration;
	/// La vitesse de la bille
	glm::dvec2 vitesse;

	bool collision = false;

	double rotationX;
	//Temps depuis la derniere collision avec un mur
	double derniereCollMur;

	double rayon_;

	int billeId_;
	bool locale_;
	int playerNum_;

	double facteurPointage_;
	//todo ajouter playerId si pas locale_
	float tempsSync_;

	int dernierFrappeurNum_;

	bool fantome_;
};

#endif //__ARBRE_NOEUDS_NOEUDBILLE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////