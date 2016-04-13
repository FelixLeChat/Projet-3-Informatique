#include "NoeudZoneDeJeu.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "NoeudBille.h"
#include <AideCollision.h>

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudZoneDeJeu::NoeudZoneDeJeu(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudZoneDeJeu::NoeudZoneDeJeu(const std::string& typeNoeud)
: NoeudComposite{ typeNoeud }
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudZoneDeJeu::~NoeudZoneDeJeu()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudZoneDeJeu::~NoeudZoneDeJeu()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudZoneDeJeu::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudZoneDeJeu::afficherConcret() const
{
	// Appel à la version de la classe de base pour l'affichage des enfants.
	NoeudComposite::afficherConcret();
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
void NoeudZoneDeJeu::animer(float temps)
{
	// Appel à la version de la classe de base pour l'animation des enfants.
	NoeudComposite::animer(temps);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudZoneDeJeu::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudZoneDeJeu::executerCollision(NoeudBille* bille)
{
	bornes.coinMax.z = 0;
	bornes.coinMin.z = 0;
	executerCollisionAvecMur(bille, bornes.coinMax,glm::dvec3(bornes.coinMax.x, bornes.coinMin.y,0));
	executerCollisionAvecMur(bille, glm::dvec3(bornes.coinMin.x, bornes.coinMax.y, 0), bornes.coinMax);
	executerCollisionAvecMur(bille, bornes.coinMin, glm::dvec3(bornes.coinMin.x, bornes.coinMax.y, 0));
	executerCollisionAvecMur(bille, glm::dvec3(bornes.coinMax.x, bornes.coinMin.y, 0), bornes.coinMin);
}

double NoeudZoneDeJeu::obtenirEnfoncement(NoeudBille* bille)
{
	bornes.coinMax.z = 0;
	bornes.coinMin.z = 0;
	double enfoncementMax = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), bornes.coinMax, glm::dvec3(bornes.coinMax.x, bornes.coinMin.y, 0));
	double enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), glm::dvec3(bornes.coinMin.x, bornes.coinMax.y, 0), bornes.coinMax);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), bornes.coinMin, glm::dvec3(bornes.coinMin.x, bornes.coinMax.y, 0));
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), glm::dvec3(bornes.coinMax.x, bornes.coinMin.y, 0), bornes.coinMin);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	return enfoncementMax;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudZoneDeJeu::collisionAvecMur(NoeudBille* bille, double dt, glm::dvec3 point1, glm::dvec3 point2)
///
/// Cette fonction teste et execute une collision avec un segment de droite
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
/// @param[in] point1 : Premier point du segment.
/// @param[in] point2 : Second point du segment.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudZoneDeJeu::executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2)
{
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionSegment(point1, point2, bille->obtenirPositionRelative(), rayonBille, true);
	
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		bille->setCollision(true, true);
		glm::dvec2 vitesse = bille->obtenirVitesse();
		glm::dvec2 normale = glm::normalize(glm::dvec2(test.direction.x, test.direction.y));

		bille->assignerVitesse(glm::normalize(normale)*std::abs(2*glm::dot(normale, vitesse)) + bille->obtenirVitesse());

		glm::dvec2 enfoncementVec = glm::normalize(normale)*test.enfoncement*1.1;
		glm::dvec3 nouvellePos = bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0);
		bille->assignerPositionRelative(nouvellePos);

	}
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
