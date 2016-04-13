///////////////////////////////////////////////////////////////////////////////
/// @file NoeudBille.cpp
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-02-24
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudBille.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include <iostream>
#include "Configuration/Config.h"
#include "Sons/ClasseSons.h"
#include <AideCollision.h>

#include "Event/EventManager.h"
#include "Event/BallSync.h"
#include "Reseau/NetworkManager.h"
#include "Event/BallScaleSyncEvent.h"
#include <Affichage/Affichage.h>


int NoeudBille::nextBilleId{ 0 };
////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudBille::NoeudBille(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudBille::NoeudBille(const std::string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
	vitesse = glm::dvec2(0, -200);
	acceleration = glm::dvec2(0, 0);
	rotationX = 0;
	derniereCollMur = 1;
	rayon_ = 0;
	locale_ = true;
	billeId_ = nextBilleId++;
	facteurPointage_ = 1;
	tempsSync_ = 0;
	playerNum_ = 0;
	fantome_ = false;
	dernierFrappeurNum_ = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudBille::~NoeudBille()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudBille::~NoeudBille()
{
	if (!locale_)
	{
		EventManager::GetInstance()->unsubscribe(this, BALLSYNC);
		EventManager::GetInstance()->unsubscribe(this, BALLSCALESYNC);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBille::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBille::afficherConcret() const
{
	// Sauvegarde de la matrice.
	glPushMatrix();
	glTranslated(0, 0, 2);
	double angleRot = atan(vitesse.y / vitesse.x);
	if (vitesse.x < 0)
		angleRot += utilitaire::PI;
	angleRot -= utilitaire::PI/2.0;
	glRotated(utilitaire::RAD_TO_DEG(angleRot), 0, 0, 1);
	glRotated(rotationX, 1, 0, 0);

	if (NetworkManager::getInstance()->estCompetitif())
	{
		auto couleur = obtenirCouleurPalette();
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "changerCouleur"), 1);
		glUniform4f(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "couleurForcee"), couleur[0], couleur[1], couleur[2], 0.3);
	}
	// Affichage du modèle.
	if (fantome_)
		glBlendFunc(GL_ONE, GL_ONE);
	liste_->dessiner();

	if (fantome_)
		glBlendFunc(GL_ONE, GL_ZERO);

	glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "changerCouleur"), 0);
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
void NoeudBille::animer(float temps)
{		
	vitesse.x += acceleration.x * temps;
	vitesse.y += acceleration.y * temps;
	maximiserVitesse();
	
	rotationX-= abs(glm::length(vitesse))*temps/(2*obtenirRayon()*utilitaire::PI)*360;
	if (rotationX < 0)
		rotationX += 360;

	positionRelative_.x += vitesse.x *temps;
	positionRelative_.y += vitesse.y *temps;

	calculerBornes();

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBille::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBille::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	//visiteur->traiterNoeudBille(this);
}

glm::fvec3 NoeudBille::obtenirCouleurPalette() const
{
	switch (playerNum_)
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

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBille::appliquerForce(glm::dvec2 force)
///
/// Cette fonction applique une force a la bille
///
/// @param[in] force : la force a appliquer
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBille::appliquerForce(glm::dvec2 force)
{
	acceleration.x += force.x;
	acceleration.y += force.y;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::dvec2 NoeudBille::obtenirRayon()
///
/// Cette fonction obtient le rayon de la bille courante.
///
/// @param[in] Aucun.
///
/// @return le rayon de la bille.
///
////////////////////////////////////////////////////////////////////////
double NoeudBille::obtenirRayon()
{
	if(rayon_ == 0)
		rayon_ = utilitaire::calculerSphereEnglobante(*modele_).rayon;
	return rayon_*agrandissement_.x;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::dvec2 NoeudBille::obtenirVitesse()
///
/// Cette fonction obtient la vitesse de la bille courante.
///
/// @param[in] Aucun.
///
/// @return le vecteur vitesse de la bille.
///
////////////////////////////////////////////////////////////////////////
glm::dvec2 NoeudBille::obtenirVitesse()
{
	maximiserVitesse();
	return vitesse;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBille::assignerVitesse(glm::dvec2 vit)
///
/// Cette fonction assigne la vitesse de la bille courante.
///
/// @param[in] glm::dvec2: le vecteur vitesse de la bille.
///
/// @return Aucun. 
///
////////////////////////////////////////////////////////////////////////
void NoeudBille::assignerVitesse(glm::dvec2 vit)
{
	vitesse = vit;
}

void NoeudBille::finaliserAnimation(float duree)
{
	acceleration = glm::dvec2(0);


	if (locale_)
	{
		tempsSync_ += duree;
		if (tempsSync_ > 0.1 || collision)
		{
			tempsSync_ = 0;
			NetworkManager::getInstance()->syncBille(billeId_, rotation_, vitesse.x, vitesse.y, positionRelative_.x, positionRelative_.y);
		}
	}

	if (collision)
	{
		collision = false;

		if (Config::obtenirInstance()->getVitBilles() && collision && Config::obtenirInstance()->getDebog())
		{
			utilitaire::timeStamp();
			double vit = sqrt(pow(vitesse[0], 2) + pow(vitesse[1], 2));
			std::cout << " - Vitesse : " << vit << std::endl;
		}
	}
	if (derniereCollMur < 0.2)
		derniereCollMur += duree;
}

void NoeudBille::maximiserVitesse()
{
	if (abs(glm::length(vitesse)) > 135.0)
	{
		vitesse = glm::normalize(vitesse)*(135.0);
	}
}

void NoeudBille::assignerId(int billeId)
{
	billeId_ = billeId;
}

void NoeudBille::assignerLocale(bool locale)
{
	locale_ = locale;
	if (!locale_)
	{
		EventManager::GetInstance()->subscribe(this, BALLSYNC);
		EventManager::GetInstance()->subscribe(this, BALLSCALESYNC);
	}
	else
	{
		EventManager::GetInstance()->unsubscribe(this, BALLSYNC);
		EventManager::GetInstance()->unsubscribe(this, BALLSCALESYNC);
		
	}
}

void NoeudBille::update(IEvent* e)
{
	if(e->getType()==BALLSYNC)
	{
		auto sync = static_cast<BallSyncEvent*>(e);
		if (sync->ballId() == billeId_)
		{
			vitesse = glm::dvec2(sync->vit_x(), sync->vit_y());
			positionRelative_ = glm::dvec3(sync->pos_x(), sync->pos_y(), positionRelative_.z);
			rotation_ = sync->rot();
		}
	}
	else if(e->getType() == BALLSCALESYNC)
	{
		auto sync = static_cast<BallScaleSyncEvent*>(e);
		if(sync->ballId() == billeId_)
			assignerAgrandissement(agrandissement_* sync->agrandissement());
	}
}

int NoeudBille::obtenirId()
{
	return billeId_;
}

int NoeudBille::obtenirPlayerNum()
{
	return playerNum_;
}

void NoeudBille::assignerPlayerNum(int playerNum)
{
	playerNum_ = playerNum;
}

void NoeudBille::assignerDernierFrappeur(int playerNum)
{
	dernierFrappeurNum_ = playerNum;
}

double NoeudBille::obtenirFacteurPointage()
{
	return facteurPointage_;
}
void NoeudBille::assignerFacteurPointage(double facteur)
{
	facteurPointage_ *= facteur;
}

int NoeudBille::obtenirDernierFrappeur()
{
	return dernierFrappeurNum_;
}

bool NoeudBille::estLocale()
{
	return locale_;
}

void NoeudBille::assignerFantome(bool b)
{
	fantome_ = b;
}

void NoeudBille::calculerBornes()
{
	utilitaire::BoiteEnglobante nouvellesBornes;
	auto ray = obtenirRayon();
	nouvellesBornes.coinMin = glm::dvec3(positionRelative_.x - ray, positionRelative_.y - ray, positionRelative_.z-ray);
	nouvellesBornes.coinMax = glm::dvec3(positionRelative_.x + ray, positionRelative_.y + ray, positionRelative_.z+ray);
	bornes = nouvellesBornes;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBille::setCollision(bool coll)
///
/// Cette fonction assigne si la bille est en collision ou non.
///
/// @param[in] coll : si la bille est en collision ou non.
/// @param[in] avecMur : si la bille est en collision avec un mur ou non.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBille::setCollision(bool coll, bool avecMur)
{
	collision = coll;
	if (coll && avecMur)
	{
		if (derniereCollMur >= 0.2)
		{
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_COLLISION_MUR, false);
		}
		derniereCollMur = 0;
	} 
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBille::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBille::executerCollision(NoeudBille* bille)
{
	if (bille != this)
	{
		double rayonBilleCourante = obtenirRayon();
		double rayonBille = bille->obtenirRayon();
		aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(bille->obtenirPositionRelative().x, bille->obtenirPositionRelative().y), rayonBille, glm::dvec2(positionRelative_.x, positionRelative_.y), rayonBilleCourante);
		
		if (test.type != aidecollision::COLLISION_AUCUNE)
		{
			glm::dvec2 vB = bille->obtenirVitesse();
			glm::dvec2 vBCourant = vitesse;
			glm::dvec2 normale = -glm::normalize(glm::dvec2(test.direction.x, test.direction.y));
			collision = true;
			bille->collision = true;
			double coeffRestitution = 0.95;
			assignerVitesse(vBCourant + normale*(-(1 + coeffRestitution) / 2)*glm::dot(normale, (vBCourant - vB)));

			bille->assignerVitesse(vB - normale*(-(1 + coeffRestitution) / 2)*glm::dot(normale, (vBCourant - vB)));
			
			glm::dvec2 enfoncementVec = normale*test.enfoncement/1.8;
			bille->assignerPositionRelative(bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0));
			assignerPositionRelative(obtenirPositionRelative() - glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0));
		}
	}

}

double NoeudBille::obtenirEnfoncement(NoeudBille* bille)
{
	if (bille == this)
		return 0;
	double rayonBilleCourante = obtenirRayon();
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(bille->obtenirPositionRelative().x, bille->obtenirPositionRelative().y), rayonBille, glm::dvec2(positionRelative_.x, positionRelative_.y), rayonBilleCourante);
	return test.enfoncement;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
