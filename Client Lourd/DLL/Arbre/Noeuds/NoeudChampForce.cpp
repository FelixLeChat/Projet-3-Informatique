#include "NoeudChampForce.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "NoeudBille.h"
#include <AideCollision.h>
#include "../../Sons/ClasseSons.h"


//TODO: Faire un bouton pour ajouter des champs de force

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudChampForce::NoeudChampForce(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudChampForce::NoeudChampForce(const string& typeNoeud)
	: NoeudComposite{ typeNoeud }
{
	force_ = 80;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudChampForce::~NoeudChampForce()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudChampForce::~NoeudChampForce()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudChampForce::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudChampForce::afficherConcret() const
{
	NoeudComposite::afficherConcret();
	// Sauvegarde de la matrice.
	glPushMatrix();
	glBlendFunc(GL_ONE, GL_ONE_MINUS_SRC_ALPHA);
	// Affichage du modèle.
	liste_->dessiner();
	glBlendFunc(GL_ONE, GL_ZERO);

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
void NoeudChampForce::animer(float temps)
{}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudChampForce::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudChampForce::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudChampForce(this);
}

const double NoeudChampForce::obtenirRotation()
{
	return enfants_[0]->obtenirRotation();
}

void NoeudChampForce::assignerRotation(double rotation)
{
	enfants_[0]->assignerRotation(rotation);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudChampForce::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudChampForce::appliquerForceConstante(NoeudBille* bille)
{
	collision(bille);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudChampForce::collisionAvecMur(NoeudBille* bille, double dt, glm::dvec3 point1, glm::dvec3 point2)
///
/// Cette fonction teste et execute une collision avec un segment de droite
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudChampForce::collision(NoeudBille* bille)
{
	glm::dvec3 posBille = bille->obtenirPositionRelative();
	double rayonBille = bille->obtenirRayon();
	double minX = points[0].x;
	double maxX = points[2].x;
	double minY = points[0].y;
	double maxY = points[2].y;
	if (posBille.x < maxX + rayonBille && posBille.x > minX - rayonBille 
		&& posBille.y < maxY + rayonBille && posBille.y > minY - rayonBille)
	{
		bille->appliquerForce(glm::dvec2(cos(utilitaire::DEG_TO_RAD(enfants_[0]->obtenirRotation())), sin(utilitaire::DEG_TO_RAD(enfants_[0]->obtenirRotation())))*force_);
	}
}

void NoeudChampForce::calculerBornes()
{
	if (modele_ != nullptr)
	{
		double transformation[16] = { agrandissement_.x, 0.0, 0.0, 0.0,
			0.0, agrandissement_.y, 0.0, 0.0,
			0.0, 0.0, agrandissement_.z, 0.0,
			positionRelative_.x, positionRelative_.y, 0, 1.0 };

		bornes = utilitaire::calculerBoiteEnglobante(*modele_, transformation);

		glm::dvec3 tempSegment[4] = { glm::dvec3(-8, -8, 0), glm::dvec3(-8, 8, 0),
			glm::dvec3(8, 8, 0), glm::dvec3(8, -8, 0) };

		for (int i = 0; i < 4; i++)
		{
			points[i] = utilitaire::appliquerMatrice(tempSegment[i], transformation);
		}
	}
}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
