#include "NoeudButoirTriangulaireDroit.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "AideCollision.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "NoeudBille.h"
#include "Configuration/Config.h"

#include "Event/EventManager.h"
#include "Event/CollisionEvent.h"
#include "Sons/ClasseSons.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudButoirTriangulaireDroit::NoeudButoirTriangulaireDroit(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudButoirTriangulaireDroit::NoeudButoirTriangulaireDroit(const string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudButoirTriangulaireDroit::~NoeudButoirTriangulaireDroit()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudButoirTriangulaireDroit::~NoeudButoirTriangulaireDroit()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirTriangulaireDroit::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudButoirTriangulaireDroit::afficherConcret() const
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
void NoeudButoirTriangulaireDroit::animer(float temps)
{

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirTriangulaireDroit::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudButoirTriangulaireDroit::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudButoirTriangulaireDroit(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirTriangulaireDroit::calculerBornes()
///
/// Cette fonction calcule les bornes de l'objet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudButoirTriangulaireDroit::calculerBornes()
{
	if (modele_ != nullptr){
		double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
			agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
			0.0, 0.0, agrandissement_.z, 0.0,
			positionRelative_.x, positionRelative_.y, 0, 1.0 };

		bornes = utilitaire::calculerBoiteEnglobante(*modele_, transformation);

		glm::dvec3 tempSegment[3][2] = { { glm::dvec3(3.33418, 15.574027, 0), glm::dvec3(-6.751180, -9.707367, 0) },
		{ glm::dvec3(-4.987365, -11.471173, 0), glm::dvec3(5.294025, -6.385822, 0) },
		{ glm::dvec3(6.191180, -5.292633, 0), glm::dvec3(6.220000, 14.999998, 0) }
		};
		glm::dvec3 tempCercles[3] = { glm::dvec3(4.70809, 14.93, 0), glm::dvec3(4.70809, -4.99, 0), glm::dvec3(-5.25, -9.94, 0) };

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				segments[i][j] = utilitaire::appliquerMatrice(tempSegment[i][j], transformation);
			}
		}
		for (int i = 0; i < 3; i++)
		{
			cercles[i] = utilitaire::appliquerMatrice(tempCercles[i], transformation);
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirTriangulaireDroit::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
double NoeudButoirTriangulaireDroit::obtenirEnfoncement(NoeudBille* bille)
{
	bornes.coinMax.z = 0;
	bornes.coinMin.z = 0;
	double rayon = 1.52*agrandissement_.y;
	double enfoncementMax = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[0][0], segments[0][1]);
	double enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[1][0], segments[1][1]);

	if (enfoncement > enfoncementMax)
	{
		enfoncementMax = enfoncement;
	}
	enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[2][0], segments[2][1]);
	if (enfoncement > enfoncementMax)
	{
		enfoncementMax = enfoncement;
	}
	enfoncement = obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), cercles[0], rayon);
	if (enfoncement > enfoncementMax)
	{
		enfoncementMax = enfoncement;
	}
	enfoncement = obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), cercles[1], rayon);
	if (enfoncement > enfoncementMax)
	{
		enfoncementMax = enfoncement;
	}
	enfoncement = obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), cercles[2], rayon);
	if (enfoncement > enfoncementMax)
	{
		enfoncementMax = enfoncement;
	}
	return enfoncementMax;
}

void NoeudButoirTriangulaireDroit::executerCollision(NoeudBille* bille)
{
	bornes.coinMax.z = 0;
	bornes.coinMin.z = 0;
	double rayon = 1.52*agrandissement_.y;
	executerCollisionAvecMur(bille, segments[0][0], segments[0][1], 50);
	executerCollisionAvecMur(bille, segments[1][0], segments[1][1], 0);
	executerCollisionAvecMur(bille, segments[2][0], segments[2][1], 0);
	executerCollisionAvecCercle(bille, cercles[0], rayon, 0);
	executerCollisionAvecCercle(bille, cercles[1], rayon, 0);
	executerCollisionAvecCercle(bille, cercles[2], rayon, 0);
}


void NoeudButoirTriangulaireDroit::executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, double forceAdditionnelle)
{
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionSegment(point1, point2, bille->obtenirPositionRelative(), rayonBille, true);
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		
		glm::dvec2 vitesse = bille->obtenirVitesse();
		glm::dvec2 normale = glm::normalize(glm::dvec2(test.direction.x, test.direction.y));

		if (forceAdditionnelle != 0 && Config::obtenirInstance()->getForceRebond())
		{
			bille->setCollision(true);
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_COLLISION_BUTOIR_TRIANGULAIRE, false);
		}
		else
		{
			bille->setCollision(true, true);
			forceAdditionnelle = 0;
		}

		double forceSupp = Config::obtenirInstance()->getForceRebond() ? forceAdditionnelle : 0;
		bille->assignerVitesse(glm::normalize(normale)*(2.0*std::abs(glm::dot(normale, vitesse)) + forceSupp) + bille->obtenirVitesse());

		glm::dvec2 enfoncementVec = glm::normalize(normale)*test.enfoncement*1.1;
		bille->assignerPositionRelative(bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0));

		if (forceAdditionnelle != 0 && Config::obtenirInstance()->getForceRebond())
			EventManager::GetInstance()->throwEvent(&CollisionEvent(obtenirType(), bille->obtenirFacteurPointage(), bille->obtenirPlayerNum()));
	}
}
	///Collision avec cercle
void NoeudButoirTriangulaireDroit::executerCollisionAvecCercle(NoeudBille* bille, glm::dvec3 position, double rayon, double forceAdditionnelle)
{
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(bille->obtenirPositionRelative().x, bille->obtenirPositionRelative().y), rayonBille, glm::dvec2(position.x, position.y), rayon);
	glm::dvec2 vitesse = bille->obtenirVitesse();
	glm::dvec2 normale = -glm::normalize(glm::dvec2(test.direction.x, test.direction.y));
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		bille->setCollision(true, true);
		bille->assignerVitesse(glm::normalize(normale)*std::abs(2.0*glm::dot(normale, vitesse)) + bille->obtenirVitesse());

		glm::dvec2 enfoncementVec = -glm::normalize(normale)*test.enfoncement;
		bille->assignerPositionRelative(bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0));
	}
}
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
