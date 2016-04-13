///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudPowerUpGrossissementBille.h
/// @author Jeremie Gagne
/// @date 2016-03-10
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_NOEUDPOWERUPGROSSEBILLE_H__
#define __ARBRE_USINES_NOEUDPOWERUPGROSSEBILLE_H__


#include "UsineNoeud.h"
#include "../Noeuds/PowerUp/NoeudPowerUpGrossissementBille.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudPowerUpGrossissementBille
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type NoeudTrone.
///
/// @author Jeremie Gagne
/// @date 2016-03-10
///////////////////////////////////////////////////////////////////////////
class UsineNoeudPowerUpGrossissementBille : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudPowerUpGrossissementBille(const std::string& nom);

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;


};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudPowerUpGrossissementBille::UsineNoeudPowerUpGrossissementBille(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres..
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudPowerUpGrossissementBille::UsineNoeudPowerUpGrossissementBille(const std::string& nom)
	: UsineNoeud(nom, std::string{ "media/PowerUp_GrosseBille.obj" })//
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudPowerUpGrossissementBille::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudPowerUpGrossissementBille::creerNoeud() const
{
	auto noeud = new NoeudPowerUpGrossissementBille{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	return noeud;
}


#endif // __ARBRE_USINES_NOEUDPOWERUPGROSSEBILLE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

