///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudPaletteGaucheJ2.h
/// @author Julien Gascon-Samson
/// @date 2011-05-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_UsineNoeudPaletteGaucheJ2_H__
#define __ARBRE_USINES_UsineNoeudPaletteGaucheJ2_H__


#include "UsineNoeud.h"
#include "Arbre/Noeuds/Palette/NoeudPaletteGaucheJ2.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudPaletteGaucheJ2
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type NoeudPaletteGaucheJ2.
///
/// @author Julien Gascon-Samson
/// @date 2011-05-19
///////////////////////////////////////////////////////////////////////////
class UsineNoeudPaletteGaucheJ2 : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudPaletteGaucheJ2(const std::string& nom);

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;
};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudPaletteGaucheJ2::UsineNoeudPaletteGaucheJ2(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres..
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudPaletteGaucheJ2::UsineNoeudPaletteGaucheJ2(const std::string& nom)
: UsineNoeud(nom, std::string{ "media/PaletteGaucheJ2.obj" })
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudPaletteGaucheJ2::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudPaletteGaucheJ2::creerNoeud() const
{
	auto noeud = new NoeudPaletteGaucheJ2{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	return noeud;
}


#endif // __ARBRE_USINES_UsineNoeudPaletteGaucheJ2_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
