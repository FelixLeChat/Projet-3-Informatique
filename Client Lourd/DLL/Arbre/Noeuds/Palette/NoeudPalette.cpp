///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPalette.cpp
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-03-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudPalette.h"
#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "Arbre/Noeuds/NoeudBille.h"

#include "Sons/ClasseSons.h"
#include <AideCollision.h>
#include <Event/EventManager.h>
#include <Event/PaletteStateSync.h>

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPalette::NoeudPalette(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPalette::NoeudPalette(const std::string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }
{
	rotAction = 0;
	boutonAppuye = false;
	aRecalculer = true;
	billeDansZone_ = false;
	playerNum_ = 0;
	EventManager::GetInstance()->subscribe(this, PALETTESTATESYNC);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPalette::~NoeudPalette()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPalette::~NoeudPalette()
{
	EventManager::GetInstance()->unsubscribe(this, PALETTESTATESYNC);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPalette::animer(float temps)
///
/// Cette fonction effectue l'animation du noeud pour un certain
/// intervalle de temps.
///
/// @param[in] temps : Intervalle de temps sur lequel faire l'animation.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPalette::animer(float temps)
{
	if (boutonAppuye)
	{
		if (rotAction != 60)
		{
			if (rotAction > 60)
			{
				rotAction = 60;
			}
			else
			{
				rotAction += temps * 720;
			}
			calculerPoints();
		}
		else if (aRecalculer)
		{
			calculerPoints();
			aRecalculer = false;
		}

	}
	else if (rotAction != 0)
	{
		aRecalculer = true;
		if (rotAction < 0)
			rotAction = 0;
		else
			rotAction -= temps * 240;
		calculerPoints();
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPalette::activer()
///
/// Cette fonction active la palette
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPalette::activer()
{
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_PALETTE, false);
	boutonAppuye = true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPalette::calculerPoints()
///
/// Cette fonction ne fait rien
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPalette::calculerPoints()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPalette::desactiver()
///
/// Cette fonction desactive la palette
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPalette::desactiver()
{
	boutonAppuye = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPalette::collisionAvecMur(NoeudBille* bille, double dt, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4, double forceAdditionnelle)
///
/// Cette fonction teste et execute une collision avec les deuc
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
/// @param[in] point1 : Premier point du premier segment.
/// @param[in] point2 : Second point du premier segment.
/// @param[in] point3 : Premier point du second segment.
/// @param[in] point4 : Second point du second segment.
/// @param[in] forceAdditionnelle : Force supplementaire a appliquer sur la bille
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPalette::executerCollisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4, double forceAdditionnelle)
{
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionSegment(point1, point2, bille->obtenirPositionRelative(), rayonBille, false);
	aidecollision::DetailsCollision test2 = aidecollision::calculerCollisionSegment(point3, point4, bille->obtenirPositionRelative(), rayonBille, false);
	glm::dvec2 vitesse = bille->obtenirVitesse();
	glm::dvec3 segment = point2 - point1;
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		segment = point2 - point1;
		if (glm::cross(segment, test.direction).z < 0 && !boutonAppuye && rotAction > 0 && vitesse.y>0) 
		{
			bille->assignerPositionRelative(bille->obtenirPositionRelative() - 2.0*test.direction*test.enfoncement);
			test = aidecollision::calculerCollisionSegment(point3, point4, bille->obtenirPositionRelative(), rayonBille, false);
			segment = point4 - point3;
			forceAdditionnelle = -forceAdditionnelle;
		}
	}
	else if (test2.type != aidecollision::COLLISION_AUCUNE)
	{
		test = test2;
		segment = point4 - point3;
		if (glm::cross(segment, test.direction).z < 0 && boutonAppuye && rotAction < 60 && vitesse.y<0) 
		{
			bille->assignerPositionRelative(bille->obtenirPositionRelative() - 2.0*test.direction*test.enfoncement);
			test = aidecollision::calculerCollisionSegment(point1, point2, bille->obtenirPositionRelative(), rayonBille, false);
			segment = point2 - point1;
		}
		else
		{
			forceAdditionnelle = -forceAdditionnelle;
		}
	}
	glm::dvec2 normale = glm::normalize(glm::dvec2(test.direction.x, test.direction.y));


	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		bille->setCollision(true, true);

		double ajoutVitesse = std::abs(2 * glm::dot(normale, vitesse));
		glm::dvec2 ajoutAVitesse = normale*(ajoutVitesse + forceAdditionnelle);

		bille->assignerVitesse(ajoutAVitesse + bille->obtenirVitesse());
		glm::dvec2 enfoncementVec = glm::normalize(normale)*test.enfoncement*1.1;
		glm::dvec3 nouvellePos = bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0);
		bille->assignerPositionRelative(nouvellePos);

		bille->assignerDernierFrappeur(playerNum_);

	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPalette::collisionAvecCercle(NoeudBille* bille, double dt, glm::dvec3 position, double rayon, double forceAdditionnelle)
///
/// Cette fonction teste et execute une collision avec un segment de droite
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
/// @param[in] position : Centre du cercle
/// @param[in] rayon : Rayon du cercle
/// @param[in] forceAdditionnelle : Force supplementaire a appliquer sur la bille
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPalette::executerCollisionAvecCercle(NoeudBille* bille, glm::dvec3 position, double rayon, double forceAdditionnelle)
{
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(bille->obtenirPositionRelative().x, bille->obtenirPositionRelative().y), rayonBille, glm::dvec2(position.x, position.y), rayon);
	glm::dvec2 vitesse = bille->obtenirVitesse();
	glm::dvec2 normale = -glm::normalize(glm::dvec2(test.direction.x, test.direction.y));
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		bille->setCollision(true, true);


		if (forceAdditionnelle < 0)
			forceAdditionnelle = 0;
		glm::dvec3 directionPalette = glm::normalize(glm::dvec3(cercles[0].y - cercles[1].y, cercles[1].x - cercles[0].x, 0));
		forceAdditionnelle = abs(glm::dot(test.direction, directionPalette))*forceAdditionnelle;

		bool forceSupp = rotAction == 60 && boutonAppuye || !boutonAppuye && rotAction == 0;
		double longueurVitesse = std::abs(glm::dot(normale, vitesse));
		glm::dvec2 ajoutAVitesse = normale*(2 * longueurVitesse + (forceSupp ? forceAdditionnelle : 0));

		bille->assignerVitesse(ajoutAVitesse + bille->obtenirVitesse());
		glm::dvec2 enfoncementVec = glm::normalize(normale)*test.enfoncement;
		glm::dvec3 nouvellePos = bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0);
		bille->assignerPositionRelative(nouvellePos);
		bille->assignerDernierFrappeur(playerNum_);
	}
}


void NoeudPalette::reinitialiser()
{
	rotAction = 0;
	desactiver();
}

bool NoeudPalette::aBilleAFrapper() const
{
	return billeDansZone_;
}

void NoeudPalette::assignerPlayerNum(int playerNum)
{
	playerNum_ = playerNum;
}

int NoeudPalette::obtenirPlayerNum() const
{
	return playerNum_;
}

void NoeudPalette::assignerFantome(bool fantome)
{
	fantome_ = fantome;
}

double NoeudPalette::obtenirRotationPalette() const
{
	return rotAction;
}

void NoeudPalette::assignerRotationPalette(double rot_action)
{
	rotAction = rot_action;
}

bool NoeudPalette::obtenirEstActif() const
{
	return boutonAppuye;
}

void NoeudPalette::assignerEstActif(bool bouton_appuye)
{
	boutonAppuye = bouton_appuye;
}

bool NoeudPalette::obtenirEstDeDroite()
{
	return false;
}

void NoeudPalette::update(IEvent* e)
{
	if(e->getType() == PALETTESTATESYNC)
	{
		auto ev = static_cast<PaletteStateSync *>(e);
		if(ev->player_num() == playerNum_ && ev->de_droite() == obtenirEstDeDroite())
		{
			assignerAgrandissement(glm::dvec3(ev->scale()));
			assignerRotationPalette(ev->rotation());
			assignerEstActif(ev->actif());
			calculerPoints();
		}
	}
}

glm::fvec3 NoeudPalette::obtenirCouleurPalette() const
{
	switch(playerNum_)
	{
	case 0:
		return glm::fvec3(1.0, 1.0, 1.0);
	case 1:
		return glm::fvec3(0, 1.0, 1.0);
	case 2:
		return glm::fvec3(0.2, 0.2, 1.0);
	case 3:
		return glm::fvec3(1.0, 1.0, 0.2);

	}
	return glm::fvec3(1.0, 1.0, 1.0);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
