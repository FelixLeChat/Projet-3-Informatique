#include "NoeudButoirCirculaire.h"
#include "NoeudBille.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "Configuration/Config.h"
#include "Affichage/Affichage.h"
#include "Event/EventManager.h"
#include "Event/CollisionEvent.h"
#include <AideCollision.h>
#include "Sons/ClasseSons.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudButoirCirculaire::NoeudButoirCirculaire(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudButoirCirculaire::NoeudButoirCirculaire(const string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
	delai = 0;
	allumer = false;
	rayon_ = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudButoirCirculaire::~NoeudButoirCirculaire()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudButoirCirculaire::~NoeudButoirCirculaire()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirCirculaire::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudButoirCirculaire::afficherConcret() const
{
	// Sauvegarde de la matrice.
	glPushMatrix();

	if (Config::obtenirInstance()->getForceRebond() && allumer){
		glEnable(GL_LIGHT6);
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "butoirAllume"), true);
		GLfloat position[4] = { GLfloat(positionRelative_.x), GLfloat(positionRelative_.y), GLfloat(positionRelative_.z + 20), 0.5f };

		GLfloat lumAmbiante[]
			= { 1.0, 1.0, 1.0, 1.0 };
		GLfloat lumDiffuse[]
			= { 1.0, 1.0, 1.0, 1.0 };
		GLfloat lumSpeculaire[]
			= { 1.0, 1.0, 1.0, 1.0 };
		glLightfv(GL_LIGHT6, GL_AMBIENT, lumAmbiante);
		glLightfv(GL_LIGHT6, GL_DIFFUSE, lumDiffuse);
		glLightfv(GL_LIGHT6, GL_SPECULAR, lumSpeculaire);

		glLightfv(GL_LIGHT6, GL_POSITION ,position);
	}
	// Affichage du modèle.
	liste_->dessiner();
	if (Config::obtenirInstance()->getForceRebond() && allumer)
	{
		glDisable(GL_LIGHT6);
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "butoirAllume"), false);
	}
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
void NoeudButoirCirculaire::animer(float temps)
{

	if (allumer)
	{
		delai -= temps;
		if (delai <= 0){
			delai = 0;
			allumer = false;
		}
	}

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirCirculaire::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudButoirCirculaire::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudButoirCirculaire(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirCirculaire::assignerAllumer(bool allume)
///
/// Cette fonction accepte change l'éclairage du butoir
///
/// @param[in] bool : Allumer ou non.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudButoirCirculaire::assignerAllumer(bool allume)
{
	allumer = allume;
}

bool NoeudButoirCirculaire::estAllume() const
{
	return allumer;
}

double NoeudButoirCirculaire::obtenirRayon()
{
	if (rayon_ == 0)
		rayon_ = utilitaire::calculerSphereEnglobante(*modele_).rayon;
	return rayon_*agrandissement_.x;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudButoirCirculaire::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
double NoeudButoirCirculaire::obtenirEnfoncement(NoeudBille* bille)
{
	return obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), positionRelative_, obtenirRayon());
}
	
void NoeudButoirCirculaire::executerCollision(NoeudBille* bille)
{

	double rayonButoir = obtenirRayon();
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(bille->obtenirPositionRelative().x, bille->obtenirPositionRelative().y), rayonBille, glm::dvec2(positionRelative_.x, positionRelative_.y), rayonButoir);

	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		glm::dvec2 vitesse = bille->obtenirVitesse();
		glm::dvec2 normale = -glm::normalize(glm::dvec2(test.direction.x, test.direction.y));
		double normeVitesse = std::abs(glm::dot(normale, vitesse));
		bille->setCollision(true);
		ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_COLLISION_BUTOIR_CIRCULAIRE, false);

		double forceSupp = Config::obtenirInstance()->getForceRebond() ? 10 : 0;

		bille->assignerVitesse(normale*(2.0*normeVitesse + forceSupp) + bille->obtenirVitesse());

		glm::dvec2 enfoncementVec = -glm::normalize(normale)*test.enfoncement;
		bille->assignerPositionRelative(bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0));
		delai = 0.5;

		if (Config::obtenirInstance()->getForceRebond())
		{
			EventManager::GetInstance()->throwEvent(&CollisionEvent(obtenirType(), bille->obtenirFacteurPointage(), bille->obtenirPlayerNum()));
			if (!allumer)
				allumer = true;
		}
			
	}

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
