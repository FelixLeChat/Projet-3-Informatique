///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudZoneDeJeu.h
/// @author Julien Gascon-Samson
/// @date 2011-05-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_UsineNoeudZoneDeJeu_H__
#define __ARBRE_USINES_UsineNoeudZoneDeJeu_H__


#include "UsineNoeud.h"
#include "Arbre/Noeuds/NoeudZoneDeJeu.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudZoneDeJeu
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type NoeudZoneDeJeu.
///
/// @author Julien Gascon-Samson
/// @date 2011-05-19
///////////////////////////////////////////////////////////////////////////
class UsineNoeudZoneDeJeu : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudZoneDeJeu(const std::string& nom);

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;


};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudZoneDeJeu::UsineNoeudZoneDeJeu(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres..
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudZoneDeJeu::UsineNoeudZoneDeJeu(const std::string& nom)
: UsineNoeud(nom, std::string{ "media/table.obj" })
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudZoneDeJeu::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudZoneDeJeu::creerNoeud() const
{
	auto noeud = new NoeudZoneDeJeu{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	return noeud;
}


#endif // __ARBRE_USINES_UsineNoeudZoneDeJeu_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
