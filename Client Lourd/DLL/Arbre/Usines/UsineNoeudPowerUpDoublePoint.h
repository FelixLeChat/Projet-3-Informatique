///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudTrone.h
/// @author Jeremie Gagne
/// @date 2016-03-10
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_NOEUDPOWERUPDOUBLEPOINT_H__
#define __ARBRE_USINES_NOEUDPOWERUPDOUBLEPOINT_H__


#include "UsineNoeud.h"
#include "../Noeuds/PowerUp/NoeudPowerUpDoublePoint.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudPowerUpDoublePoint
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type UsineNoeudPowerUpDoublePoint.
///
/// @author Jeremie Gagne
/// @date 2016-03-10
///////////////////////////////////////////////////////////////////////////
class UsineNoeudPowerUpDoublePoint : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudPowerUpDoublePoint(const std::string& nom);

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;


};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudPowerUpDoublePoint::UsineNoeudPowerUpDoublePoint(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudPowerUpDoublePoint::UsineNoeudPowerUpDoublePoint(const std::string& nom)
	: UsineNoeud(nom, std::string{ "media/PowerUp_DoublePts.obj" })//
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudPowerUpDoublePoint::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudPowerUpDoublePoint::creerNoeud() const
{
	auto noeud = new NoeudPowerUpDoublePoint{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	return noeud;
}


#endif // __ARBRE_USINES_NOEUDPOWERUPDOUBLEPOINT_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

