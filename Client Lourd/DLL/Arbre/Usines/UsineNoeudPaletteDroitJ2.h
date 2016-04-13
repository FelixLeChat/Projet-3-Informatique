///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudPaletteDroitJ2.h
/// @author Julien Gascon-Samson
/// @date 2011-05-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_UsineNoeudPaletteDroitJ2_H__
#define __ARBRE_USINES_UsineNoeudPaletteDroitJ2_H__


#include "UsineNoeud.h"
#include "Arbre/Noeuds/Palette/NoeudPaletteDroitJ2.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudPaletteDroitJ2
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type NoeudPaletteDroitJ2.
///
/// @author Julien Gascon-Samson
/// @date 2011-05-19
///////////////////////////////////////////////////////////////////////////
class UsineNoeudPaletteDroitJ2 : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudPaletteDroitJ2(const std::string& nom);

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;
};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudPaletteDroitJ2::UsineNoeudPaletteDroitJ2(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres..
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudPaletteDroitJ2::UsineNoeudPaletteDroitJ2(const std::string& nom)
: UsineNoeud(nom, std::string{ "media/PaletteDroitJ2.obj" })
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudPaletteDroitJ2::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudPaletteDroitJ2::creerNoeud() const
{
	auto noeud = new NoeudPaletteDroitJ2{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	return noeud;
}


#endif // __ARBRE_USINES_UsineNoeudPaletteDroitJ2_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
