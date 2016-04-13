///////////////////////////////////////////////////////////////////////////////
/// @file UsineNoeud.h
/// @author Martin Bisson
/// @date 2007-01-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_USINES_USINENOEUD_H__
#define __ARBRE_USINES_USINENOEUD_H__
#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"

class NoeudAbstrait;

///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeud
/// @brief Classe de base abstraite des usines qui seront utilisées pour
///        créer les différents noeuds de l'arbre de rendu.
///
/// @author Martin Bisson
/// @date 2007-01-28
///////////////////////////////////////////////////////////////////////////
class UsineNoeud
{
public:
   /// Destructeur vide déclaré virtuel pour les classes dérivées.
   inline virtual ~UsineNoeud() {
	   liste_.storageRelacher();
   }

   /// Fonction à surcharger pour la création d'un noeud.
   virtual NoeudAbstrait* creerNoeud() const = 0;

   /// Retourne le nom associé à l'usine
   inline const std::string& obtenirNom() const;


protected:
   /// Constructeur qui prend le nom associé à l'usine.
	UsineNoeud(const std::string& nomUsine, const std::string& nomModele) : nom_(nomUsine) {
		modele_.charger(nomModele);
		liste_ = modele::opengl_storage::OpenGL_Liste{ &modele_ };
		liste_.storageCharger();
	}

   /// Modèle 3D correspondant à ce noeud.
   modele::Modele3D modele_;
   /// Storage pour le dessin du modèle
   modele::opengl_storage::OpenGL_Liste liste_;

private:
   /// Le nom associé à l'usine
   std::string nom_;
};




////////////////////////////////////////////////////////////////////////
///
/// @fn inline const std::string& UsineNoeud::obtenirNom() const
///
/// Cette fonction retourne une chaîne représentante le nom associé à
/// l'usine.
///
/// @return Le nom associé à l'usine.
///
////////////////////////////////////////////////////////////////////////
inline const std::string& UsineNoeud::obtenirNom() const
{
   return nom_;
}


#endif // __ARBRE_USINES_USINENOEUD_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
