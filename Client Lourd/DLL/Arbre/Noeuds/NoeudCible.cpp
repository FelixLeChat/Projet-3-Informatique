#include "NoeudCible.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "NoeudBille.h"
#include "../../Event/EventManager.h"
#include "../../Event/CollisionEvent.h"
#include <AideCollision.h>
#include "../../Sons/ClasseSons.h"
#include "../../Reseau/NetworkManager.h"
#include "../../Event/CibleSyncEvent.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudCible::NoeudCible(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudCible::NoeudCible(const string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
	actif_ = true;
	EventManager::GetInstance()->subscribe(this, CIBLESYNC);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudCible::~NoeudCible()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudCible::~NoeudCible()
{
	EventManager::GetInstance()->unsubscribe(this, CIBLESYNC);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCible::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudCible::afficherConcret() const
{
	if (actif_)
	{
		// Sauvegarde de la matrice.
		glPushMatrix();

		// Affichage du modèle.
		liste_->dessiner();

		// Restauration de la matrice.
		glPopMatrix();
	}
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
void NoeudCible::animer(float temps)
{

}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCible::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudCible::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudCible(this);
}


double NoeudCible::obtenirEnfoncement(NoeudBille* bille)
{
	double enfoncementMax = 0;
	if (actif_)
	{
		enfoncementMax = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), points[0], points[1]);

		double enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), points[1], points[2]);
		if (enfoncementMax < enfoncement)
			enfoncementMax = enfoncement;
		enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), points[2], points[3]);
		if (enfoncementMax < enfoncement)
			enfoncementMax = enfoncement;
		enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), points[3], points[0]);
		if (enfoncementMax < enfoncement)
			enfoncementMax = enfoncement;

	}
	return enfoncementMax;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCible::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudCible::executerCollision(NoeudBille* bille)
{
	if (actif_)
	{
		bornes.coinMax.z = 0;
		bornes.coinMin.z = 0;
		executerCollisionAvecMur(bille, points[0], points[1], points[2], points[3]);
		executerCollisionAvecMur(bille, points[1], points[2], points[3], points[0]);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCible::collisionAvecMur(NoeudBille* bille, double dt, glm::dvec3 point1, glm::dvec3 point2)
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
void NoeudCible::executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4 )
{
	if (actif_)
	{
		double rayonBille = bille->obtenirRayon();
		aidecollision::DetailsCollision test = aidecollision::calculerCollisionSegment(point1, point2, bille->obtenirPositionRelative(), rayonBille, true);
		aidecollision::DetailsCollision test2 = aidecollision::calculerCollisionSegment(point3, point4, bille->obtenirPositionRelative(), rayonBille, true);
		if (test.type != aidecollision::COLLISION_AUCUNE)
		{
			if (test2.type != aidecollision::COLLISION_AUCUNE && test2.enfoncement > test.enfoncement)
			{
				test = test2;
			}
		}
		else
		{
			test = test2;
		}


		if (test.type != aidecollision::COLLISION_AUCUNE)
		{
			bille->setCollision(true);
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_COLLISION_CIBLE, false);
			glm::dvec2 vitesse = bille->obtenirVitesse();
			glm::dvec2 normale = glm::normalize(glm::dvec2(test.direction.x, test.direction.y));

			bille->assignerVitesse(glm::normalize(normale)*std::abs(2 * glm::dot(normale, vitesse)) + bille->obtenirVitesse());

			glm::dvec2 enfoncementVec = glm::normalize(normale)*test.enfoncement;
			glm::dvec3 nouvellePos = bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0);
			bille->assignerPositionRelative(nouvellePos);

			NetworkManager::getInstance()->SyncCible(positionRelative_.x, positionRelative_.y);
			if (bille->estLocale())
			{
				actif_ = false;
				EventManager::GetInstance()->throwEvent(&CollisionEvent(obtenirType(), bille->obtenirFacteurPointage(), bille->obtenirPlayerNum()));
			}
		}
	}
}

///Reinitialise le ressort
void NoeudCible::reinitialiser()
{
	actif_ = true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCible::calculerBornes()
///
/// Cette fonction calcule les bornes de la table de jeu.
///
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudCible::calculerBornes()
{
	if (modele_ != nullptr)
	{
		double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.x *  sin(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
			agrandissement_.y * -sin(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
			0.0, 0.0, agrandissement_.z, 0.0,
			positionRelative_.x, positionRelative_.y, 0, 1.0 };

		bornes = utilitaire::calculerBoiteEnglobante(*modele_, transformation);

		glm::dvec3 tempSegment[4] = { glm::dvec3(-1, -3, 0), glm::dvec3(-1, 3, 0),
			glm::dvec3(1, 3, 0), glm::dvec3(1, -3, 0) };

		for (int i = 0; i < 4; i++)
		{
			points[i] = utilitaire::appliquerMatrice(tempSegment[i], transformation);
		}
	}
}

void NoeudCible::update(IEvent* e)
{
	if(actif_ && e->getType()==CIBLESYNC)
	{
		auto syncEvent = static_cast<CibleSyncEvent *>(e);
		if(syncEvent->pos_x() == positionRelative_.x && syncEvent->pos_y() == positionRelative_.y)
		{
			actif_ = false;
		}
	}
}

bool NoeudCible::obtenirEstActif() const
{
	return actif_;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
