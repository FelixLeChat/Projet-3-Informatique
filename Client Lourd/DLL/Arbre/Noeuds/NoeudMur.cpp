#include "NoeudMur.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "NoeudBille.h"
#include <AideCollision.h>


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudMur::NoeudMur(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudMur::NoeudMur(const std::string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
	premierPointAssigne_ = false;
	rotation_ = 0;
	longueur_ = 1;
	creationTerminee = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudMur::~NoeudMur()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudMur::~NoeudMur()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::afficherConcret() const
{
	// Sauvegarde de la matrice.
	glPushMatrix();
	
	glScaled(1, longueur_, 1);
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
void NoeudMur::animer(float temps)
{

}

////////////////////////////////////////////////////////////////////////
///
/// @fn double NoeudMur::obtenirLongueur()
///
/// Cette fonction obtient la longueur du mur.
///
///
/// @return la longueur du mur.
///
////////////////////////////////////////////////////////////////////////
double NoeudMur::obtenirLongueur()
{
	return longueur_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::assignerLongueur(double longueur)
///
/// Cette fonction assigne la longueur du mur.
///
/// @param[in] longueur : la longueur du mur.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::assignerLongueur(double longueur)
{
	longueur_ = longueur;
	calculerBornes();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::assignerPositionRelative(const glm::dvec3& positionRelative)
///
/// Cette fonction permet d'assigner la position relative du noeud par
/// rapport à son parent.
///
/// @param positionRelative : La position relative.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::assignerPositionRelative(const glm::dvec3& positionRelative)
{
	NoeudAbstrait::assignerPositionRelative(positionRelative);
	if (!premierPointAssigne_){
		premierPoint_.x = positionRelative_.x;
		premierPoint_.y = positionRelative_.y;
		premierPointAssigne_ = !premierPointAssigne_;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param visiteur : le visiteur à accepter.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudMur(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::assignerDeuxiemePoint(double x, double y)
///
/// Cette fonction assigne le deuxième point du mur.
///
/// @param x : La position en x.
/// @param y : La position en y.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::assignerDeuxiemePoint(double x, double y)
{
	if (x < (obtenirParent()->obtenirBornes().coinMax.x)
		&& x >(obtenirParent()->obtenirBornes().coinMin.x)
		&& y < (obtenirParent()->obtenirBornes().coinMax.y)
		&& y >(obtenirParent()->obtenirBornes().coinMin.y))
	{
		deuxiemePoint_.x = x;
		deuxiemePoint_.y = y;
		deuxiemePoint_.z = 0;

		positionRelative_.x = (premierPoint_.x + deuxiemePoint_.x) / 2;
		positionRelative_.y = (premierPoint_.y + deuxiemePoint_.y) / 2;
	}
	if (premierPoint_.x == deuxiemePoint_.x && premierPoint_.y == deuxiemePoint_.y)
		rotation_ = 1;
	else
		rotation_ = utilitaire::RAD_TO_DEG(asin((deuxiemePoint_.x - premierPoint_.x) / sqrt(pow((deuxiemePoint_.x - premierPoint_.x), 2) + pow((deuxiemePoint_.y - premierPoint_.y), 2))));

	if (deuxiemePoint_.y > premierPoint_.y){
		rotation_ += 2 * (90 - rotation_);
	}

	utilitaire::BoiteEnglobante boite = utilitaire::calculerBoiteEnglobante(*modele_);
	double hauteurBase = boite.coinMax.y - boite.coinMin.y;
	longueur_ = abs(sqrt(pow((deuxiemePoint_.x - premierPoint_.x), 2) + pow((deuxiemePoint_.y - premierPoint_.y), 2)) / hauteurBase);
	
	calculerBornes();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::dvec3 NoeudMur::obtenirDeuxiemePoint()
///
/// Cette fonction obtient le deuxième point du mur.
///
///
/// @return le deuxième point du mur.
///
////////////////////////////////////////////////////////////////////////
glm::dvec3 NoeudMur::obtenirDeuxiemePoint(){
	return deuxiemePoint_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::calculerBornes()
///
/// Cette fonction calcule les bornes de la table de jeu.
///
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::calculerBornes()
{
	double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.x *  sin(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
		longueur_*agrandissement_.y * -sin(utilitaire::DEG_TO_RAD(rotation_)), longueur_*agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
		0.0, 0.0, agrandissement_.z, 0.0,
		positionRelative_.x, positionRelative_.y, 0, 1.0 };

	bornes = utilitaire::calculerBoiteEnglobante(*modele_, transformation);

	if (creationTerminee)
	{
		glm::dvec3 tempSegment[4] = {  glm::dvec3(-1, -4, 0), glm::dvec3(-1, 4, 0) ,
		 glm::dvec3(1, 4, 0), glm::dvec3(1, -4, 0) };

		for (int i = 0; i < 4; i++)
		{
			points[i] = utilitaire::appliquerMatrice(tempSegment[i], transformation);
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool NoeudMur::obtenirCreationTerminee()
///
/// Cette fonction obtient si la creation du mur est terminer ou non
///
///
/// @return le deuxième point du mur.
///
////////////////////////////////////////////////////////////////////////
bool NoeudMur::obtenirCreationTerminee()
{
	return creationTerminee;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::terminerCreation()
///
/// Cette fonction termine la creation du mur
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::terminerCreation()
{
	creationTerminee = true;
	calculerBornes();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::executerCollision(NoeudBille* bille)
{
	executerCollisionAvecSegments(bille, points[0], points[1], points[2], points[3]);
	executerCollisionAvecSegments(bille, points[1], points[2], points[3], points[0]);
}

double NoeudMur::obtenirEnfoncement(NoeudBille* bille)
{
	double enfoncementMax = 0;
	bornes.coinMax.z = 0;
	bornes.coinMin.z = 0;

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
	return enfoncementMax;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::collisionAvecSegments(NoeudBille* bille, double dt, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4)
///
/// Cette fonction teste et execute une collision avec deux segments de
/// droites parallele
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
/// @param[in] point1 : Premier point du premier segment.
/// @param[in] point2 : Second point du premier segment.
/// @param[in] point3 : Premier point du second segment.
/// @param[in] point4 : Second point du second segment.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::executerCollisionAvecSegments(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, glm::dvec3 point3, glm::dvec3 point4)
{
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionSegment(point1, point2, bille->obtenirPositionRelative(), rayonBille, true);
	aidecollision::DetailsCollision test2 = aidecollision::calculerCollisionSegment(point3, point4, bille->obtenirPositionRelative(), rayonBille, true);
	glm::dvec3 segment = point2 - point1;
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		if (test2.type != aidecollision::COLLISION_AUCUNE && test2.enfoncement > test.enfoncement){
			test = test2;
			segment = point4 - point3;
		}
	}
	else
	{
		test = test2;
		segment = point4 - point3;
	}
	

	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		bille->setCollision(true, true);
		glm::dvec2 vitesse = bille->obtenirVitesse();
		glm::dvec2 normale = glm::normalize(glm::dvec2(test.direction.x, test.direction.y));

		bille->assignerVitesse(glm::normalize(normale)*std::abs(2.0*glm::dot(normale, vitesse)) + bille->obtenirVitesse());
		glm::dvec2 enfoncementVec = glm::normalize(normale)*test.enfoncement*1.1;
		glm::dvec3 nouvellePos = bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0);
		bille->assignerPositionRelative(nouvellePos);
	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
