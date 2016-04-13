#include "NoeudPortail.h"
#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "NoeudBille.h"
#include "Configuration/Config.h"
#include <AideCollision.h>
#include "Sons/ClasseSons.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPortail::NoeudPortail(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPortail::NoeudPortail(const std::string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
	frere = nullptr;
	actif_ = true;
	rayon_ = 0;
	bille_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPortail::~NoeudPortail()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPortail::~NoeudPortail()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::afficherConcret() const
{
	// Sauvegarde de la matrice.
	glPushMatrix();

	if (Config::obtenirInstance()->getLimitesPortails()  && Config::obtenirInstance()->getDebog())
	{
		double rayonPortail = utilitaire::calculerCylindreEnglobant(*modele_).rayon*agrandissement_.x;

		glColor3f(1, 0, 0);
		glBegin(GL_LINE_LOOP);

		for (int i = 0; i < 360; i++)
		{
			double degInRad = utilitaire::DEG_TO_RAD(i);
			glVertex3d(cos(degInRad)*(rayonPortail * 4), sin(degInRad)*(rayonPortail * 4), 1);
		}

		glEnd();
	}

	// Affichage du modèle.
	liste_->dessiner();

	// Restauration de la matrice.
	glPopMatrix();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::animer(float temps)
///
/// Cette fonction effectue l'animation du noeud pour un certain
/// intervalle de temps.
///
/// @param[in] temps : Intervalle de temps sur lequel faire l'animation.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::animer(float temps)
{

}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudPortail(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPortail* NoeudPortail::obtenirFrere()
///
/// Cette fonction obtient le frère du noeud portail.
///
///
/// @return le frère.
///
////////////////////////////////////////////////////////////////////////
NoeudPortail* NoeudPortail::obtenirFrere()
{
	return frere;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::assignerFrere(NoeudPortail* noeud)
///
/// Cette fonction assigne le frère à un noeud portail.
///
/// @param[in] noeud : le noeud frère qu'on va assigner
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::assignerFrere(NoeudPortail* noeud)
{
	frere = noeud;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::executerCollision(NoeudBille* bille)
{
	
	if (actif_)
	{
		double distance = glm::distance(positionRelative_, bille->obtenirPositionRelative());
		if (distance < bille->obtenirRayon() + obtenirRayon())
		{
			bille->assignerPositionRelative(frere->obtenirPositionRelative());
			bille->setCollision(true);
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_PORTAIL, false);
			frere->desactiver(bille);
		}
	}
}

double NoeudPortail::obtenirEnfoncement(NoeudBille * bille)
{
	if (actif_)
		return obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), positionRelative_, obtenirRayon());
	else
		return 0;
}

void NoeudPortail::appliquerForceConstante(NoeudBille * bille)
{
	if (!actif_)
	{
		if (glm::distance(positionRelative_, bille_->obtenirPositionRelative()) > bille_->obtenirRayon() + obtenirRayon() * 4)
		{
			reinitialiser();
		}
	}
	else
	{
		double rayonBille = bille->obtenirRayon();
		double distance = glm::distance(positionRelative_, bille->obtenirPositionRelative());
		aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(bille->obtenirPositionRelative().x, bille->obtenirPositionRelative().y), rayonBille, glm::dvec2(positionRelative_.x, positionRelative_.y), obtenirRayon() * 4);
		if (test.type != aidecollision::COLLISION_AUCUNE)
		{
			bille->appliquerForce(glm::normalize(glm::dvec2(test.direction.x, test.direction.y))*2000.0 / glm::max(pow(obtenirRayon() * 3 - test.enfoncement, 2), 0.75));
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::desactiver()
///
/// Cette fonction desactive un portail
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::desactiver(NoeudBille* bille)
{
	actif_ = false;
	bille_ = bille;
}



////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::reinitialiser()
///
/// Cette fonction reinitialise un portail
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::reinitialiser()
{
	actif_ = true;
	bille_ = nullptr;
}

double NoeudPortail::obtenirRayon()
{
	if (rayon_ == 0)
		rayon_ = utilitaire::calculerSphereEnglobante(*modele_).rayon;
	return rayon_*agrandissement_.x;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
