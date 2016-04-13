#include "NoeudTrou.h"
#include "Utilitaire.h"
#include <GL/gl.h>
#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "../Visiteur/VisiteurAbstrait.h"
#include "NoeudBille.h"
#include "../../Event/BallLostEvent.h"
#include "../../Event/EventManager.h"
#include <AideCollision.h>
#include "../../Sons/ClasseSons.h"
#include "../../Reseau/NetworkManager.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTrou::NoeudTrou(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudTrou::NoeudTrou(const string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
	rayon_ = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTrou::~NoeudTrou()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudTrou::~NoeudTrou()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTrou::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTrou::afficherConcret() const
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
void NoeudTrou::animer(float temps)
{

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTrou::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTrou::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudTrou(this);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTrou::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTrou::executerCollision(NoeudBille* bille)
{

	double rayonTrou = obtenirRayon();
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(bille->obtenirPositionRelative().x, bille->obtenirPositionRelative().y), rayonBille, glm::dvec2(positionRelative_.x, positionRelative_.y), rayonTrou);
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_TROU, false);

		if (bille->estLocale()) {
			EventManager::GetInstance()->throwEvent(&BallLostEvent(bille));
			NetworkManager::getInstance()->perdreBille(bille->obtenirId(), bille->obtenirPlayerNum());
		}
	}
}

double NoeudTrou::obtenirEnfoncement(NoeudBille* bille)
{

	if (!bille->estLocale()) {
		return 0;
	}
	return obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), positionRelative_, obtenirRayon());
}

double NoeudTrou::obtenirRayon()
{
	if (rayon_ == 0)
		rayon_ = utilitaire::calculerSphereEnglobante(*modele_).rayon;
	return rayon_*agrandissement_.x;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
