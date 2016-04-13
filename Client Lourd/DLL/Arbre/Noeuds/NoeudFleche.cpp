#include "NoeudFleche.h"

#include <GL/gl.h>

#include "Modele3D.h"
#include "NoeudBille.h"



////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudFleche::NoeudFleche(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudFleche::NoeudFleche(const string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudFleche::~NoeudFleche()
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudFleche::~NoeudFleche()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudFleche::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudFleche::afficherConcret() const
{
	// Sauvegarde de la matrice.
	glPushMatrix();

	// Affichage du modèle.
	liste_->dessiner();

	// Restauration de la matrice.
	glPopMatrix();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCube::animer(float temps)
///
/// Cette fonction effectue l'animation du noeud pour un certain
/// intervalle de temps.
///
/// @param[in] temps : Intervalle de temps sur lequel faire l'animation.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudFleche::animer(float temps)
{}

/// Changer la sélection du noeud.
bool NoeudFleche::inverserSelection()
{
	return parent_->inverserSelection();
}

void NoeudFleche::afficherSelectionne() const
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudFleche::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudFleche::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	//visiteur->traiterNoeudFleche(this);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
