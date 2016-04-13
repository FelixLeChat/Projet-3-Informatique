///////////////////////////////////////////////////////////////////////////
/// @file UsineNoeudTrone.h
/// @author Jeremie Gagne
/// @date 2016-03-10
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_NOEUDPLATEAU_H__
#define __ARBRE_USINES_NOEUDPLATEAU_H__


#include "UsineNoeud.h"
#include "Arbre/Noeuds/PowerUp/NoeudPlateauDArgent.h"
#include "UsineNoeudPowerUpDemiPoint.h"
#include "UsineNoeudPowerUpDoublePoint.h"
#include "UsineNoeudPowerUpGrossissementBille.h"
#include "UsineNoeudPowerUpGrossissementPalette.h"


///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeudTrone
/// @brief Classe qui représente une usine capable de créer des noeuds de
///        type NoeudTrone.
///
/// @author Jeremie Gagne
/// @date 2016-03-10
///////////////////////////////////////////////////////////////////////////
class UsineNoeudPlateauDArgent : public UsineNoeud
{
public:
	/// Constructeur par paramètres.
	inline UsineNoeudPlateauDArgent(const std::string& nom);

	inline virtual ~UsineNoeudPlateauDArgent() 
	{
		liste_.storageRelacher();
		delete usineDemiPoint_;
		delete usineDoublePoint_;
		delete usineGrosseBille_;
		delete usineGrossePalette_;
	}

	/// Fonction à surcharger pour la création d'un noeud.
	inline virtual NoeudAbstrait* creerNoeud() const;

private:
	UsineNoeudPowerUpDemiPoint* usineDemiPoint_;
	UsineNoeudPowerUpDoublePoint* usineDoublePoint_;
	UsineNoeudPowerUpGrossissementBille* usineGrosseBille_;
	UsineNoeudPowerUpGrossissementPalette* usineGrossePalette_;

};


////////////////////////////////////////////////////////////////////////
///
/// @fn UsineNoeudTrone::UsineNoeudTrone(const std::string& nom)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres..
///
/// @param[in] nom   : Le nom de l'usine qui correspond au type de noeuds créés.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
inline UsineNoeudPlateauDArgent::UsineNoeudPlateauDArgent(const std::string& nom)
	: UsineNoeud(nom, std::string{ "media/PlateauDArgent.obj" })//
{
	usineDemiPoint_ = new UsineNoeudPowerUpDemiPoint("demiPowerup");
	usineDoublePoint_ = new UsineNoeudPowerUpDoublePoint("doublePowerup");
	usineGrosseBille_ = new UsineNoeudPowerUpGrossissementBille("billePowerup");
	usineGrossePalette_ = new UsineNoeudPowerUpGrossissementPalette("palettePowerup");
}




////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* UsineNoeudPlateauDArgent::creerNoeud() const
///
/// Cette fonction retourne un noeud nouvellement créé du type produit
/// par cette usine, soit une araignée.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* UsineNoeudPlateauDArgent::creerNoeud() const
{
	auto noeud = new NoeudPlateauDArgent{ obtenirNom() };
	noeud->assignerObjetRendu(&modele_, &liste_);
	noeud->ajouter(usineGrossePalette_->creerNoeud());
	noeud->ajouter(usineGrosseBille_->creerNoeud());
	noeud->ajouter(usineDoublePoint_->creerNoeud());
	noeud->ajouter(usineDemiPoint_->creerNoeud());
	
	return noeud;
}


#endif // __ARBRE_USINES_UsineNoeudTrone_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
