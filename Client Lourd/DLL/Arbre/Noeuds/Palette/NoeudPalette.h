///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPalette.h
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-03-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDPALETTE_H__
#define __ARBRE_NOEUDS_NOEUDPALETTE_H__

#include "Arbre/Noeuds/NoeudAbstrait.h"
#include "Event/IEventSubscriber.h"

///////////////////////////////////////////////////////////////////////////
/// @class NoeudPalette
/// @brief Classe de base des palettes
///
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-03-21
///////////////////////////////////////////////////////////////////////////
class NoeudPalette : public NoeudAbstrait, public IEventSubscriber{
public:
	/// Constructeur.
	NoeudPalette(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPalette();

	/// Anime le noeud.
	virtual void animer(float dt);
	
	/// Appele lorsque le bouton est appuye
	void activer();
	/// Appele lorsque le bouton est relache
	void desactiver();
	/// Reinitialise la palette
	void reinitialiser();
	///
	bool aBilleAFrapper() const;

	void assignerPlayerNum(int playerNum);
	int obtenirPlayerNum() const;
	void assignerFantome(bool fantome);


	double obtenirRotationPalette() const;
	void assignerRotationPalette(double rot_action);

	bool obtenirEstActif() const;
	void assignerEstActif(bool bouton_appuye);

	virtual bool obtenirEstDeDroite();
	void update(IEvent* e);
protected:
	glm::fvec3 obtenirCouleurPalette() const;

	///Les points des deux segments de droite de la palette
	glm::dvec3 segments[2][2];
	/// Le centre des deux cercles de la palette
	glm::dvec3 cercles[2];
	/// Le rayon des cercles de la palette
	double rayons[2];
	///Collision avec segment
	void executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4, double forceAdditionnelle);
	///Collision avec cercle
	void executerCollisionAvecCercle(NoeudBille* bille, glm::dvec3 position, double rayon, double forceAdditionnelle);

	/// calcule les points des composants de la palette
	virtual void calculerPoints();
	//Rotation additionnelle du a l'action du joueur
	double rotAction;
	//Est-ce que le bouton est appuye
	bool boutonAppuye;

	bool aRecalculer;

	bool billeDansZone_;

	bool fantome_;

	int playerNum_;
};

#endif //__ARBRE_NOEUDS_NOEUDPALETTE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
