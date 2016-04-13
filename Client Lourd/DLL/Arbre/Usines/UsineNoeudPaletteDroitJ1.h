///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudPaletteDroitJ1.h
/// @author Julien Gascon-Samson
/// @date 2011-05-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_UsineNoeudPaletteDroitJ1_H__
#define __ARBRE_USINES_UsineNoeudPaletteDroitJ1_H__


#include "UsineNoeud.h"
#include "Arbre/Noeuds/Palette/NoeudPaletteDroitJ1.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudPaletteDroitJ1
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type NoeudPaletteDroitJ1.
///
/// @author Julien Gascon-Samson
/// @date 2011-05-19
///////////////////////////////////////////////////////////////////////////
class UsineNoeudPaletteDroitJ1 : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudPaletteDroitJ1(const std::string& nom);

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;
};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudPaletteDroitJ1::UsineNoeudPaletteDroitJ1(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres..
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudPaletteDroitJ1::UsineNoeudPaletteDroitJ1(const std::string& nom)
: UsineNoeud(nom, std::string{ "media/PaletteDroitJ1.obj" })
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudPaletteDroitJ1::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudPaletteDroitJ1::creerNoeud() const
{
	auto noeud = new NoeudPaletteDroitJ1{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	return noeud;
}


#endif // __ARBRE_USINES_UsineNoeudPaletteDroitJ1_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
