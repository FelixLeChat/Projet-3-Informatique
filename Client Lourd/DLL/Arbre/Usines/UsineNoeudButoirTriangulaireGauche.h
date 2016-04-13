///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudButoirTriangulaireGauche.h
/// @author Julien Gascon-Samson
/// @date 2011-05-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_UsineNoeudButoirTriangulaireGauche_H__
#define __ARBRE_USINES_UsineNoeudButoirTriangulaireGauche_H__


#include "UsineNoeud.h"
#include "Arbre/Noeuds/NoeudButoirTriangulaireGauche.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudButoirTriangulaireGauche
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type NoeudButoirTriangulaireGauche.
///
/// @author Julien Gascon-Samson
/// @date 2011-05-19
///////////////////////////////////////////////////////////////////////////
class UsineNoeudButoirTriangulaireGauche : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudButoirTriangulaireGauche(const std::string& nom);

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;
};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudButoirTriangulaireGauche::UsineNoeudButoirTriangulaireGauche(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres..
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudButoirTriangulaireGauche::UsineNoeudButoirTriangulaireGauche(const std::string& nom)
: UsineNoeud(nom, std::string{ "media/ButoirTriangulaireGauche.obj" })//
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudButoirTriangulaireGauche::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudButoirTriangulaireGauche::creerNoeud() const
{
	auto noeud = new NoeudButoirTriangulaireGauche{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	return noeud;
}


#endif // __ARBRE_USINES_UsineNoeudButoirTriangulaireGauche_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
