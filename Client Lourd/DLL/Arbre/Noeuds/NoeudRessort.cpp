#include "NoeudRessort.h"


#include "Utilitaire.h"

#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "NoeudBille.h"
#include <algorithm>
#include "../../Sons/ClasseSons.h"
#include <AideCollision.h>

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudRessort::NoeudRessort(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudRessort::NoeudRessort(const std::string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
	compression = 1;
	enCompression = false;
	enDecompression = false;
	hauteurTrouve = false;
	hauteurBase = 0;
	collisionCettePhase = false;
	delaiSon = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudRessort::~NoeudRessort()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudRessort::~NoeudRessort()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::afficherConcret() const
{
	// Sauvegarde de la matrice.
	glPushMatrix();

	glTranslated(0, -(1 - compression)/2*hauteurBase, 0);
	glScaled(1, compression, 1);
	
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
void NoeudRessort::animer(float temps)
{
	if (enCompression)
	{
		if (compression <= 0.2)
		{
			compression = 0.2;
		}
		else
		{
			delaiSon -= temps;
			compression -= temps / 2;
			if (delaiSon <= 0)
			{
				ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_COMPRESSION, false);
				delaiSon = 0.3;
			}
		}
		calculerBornes();
	}
	else if (enDecompression)
	{
		if (compression < 1)
		{
			compression += temps * std::max(1-compression, 0.25)*8;
		}
		else
		{
			compression = 1;
			enDecompression = false;
			collisionCettePhase = false;
		}
		calculerBornes();
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::activer()
///
/// Cette fonction active le ressort
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::activer()
{
	enCompression = true;
	enDecompression = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::desactiver()
///
/// Cette fonction desactive le ressort
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::desactiver()
{
	ClasseSons::obtenirInstance()->arreterSon(1);
	enCompression = false;
	enDecompression = true;
	collisionCettePhase = false;
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_RELACHEMENT, false);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::reinitialiser()
///
/// Cette fonction reinitialise le ressort
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::reinitialiser()
{
	compression = 1;
	enCompression = false;
	enDecompression = false;
	collisionCettePhase = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::calculerBornes()
///
/// Cette fonction calcule les bornes de l'objet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::calculerBornes()
{
	if (modele_ != nullptr){
		if (!hauteurTrouve)
		{
			double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
				agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
				0.0, 0.0, agrandissement_.z, 0.0,
				positionRelative_.x, positionRelative_.y , 0, 1.0 };

			bornes = utilitaire::calculerBoiteEnglobante(*modele_, transformation);
			hauteurBase =20;
			hauteurTrouve = true;

			segmentsBase[0] = glm::dvec3(bornes.coinMin.x, bornes.coinMax.y, 0);
			segmentsBase[1] = glm::dvec3(bornes.coinMax.x, bornes.coinMax.y, 0);
			segmentsBase[2] = glm::dvec3(bornes.coinMax.x, bornes.coinMin.y, 0);
			segmentsBase[3] = glm::dvec3(bornes.coinMin.x, bornes.coinMin.y, 0);
		}
		else
		{
			calculerSegments();
			
			bornes.coinMax.x = std::max(segments[0].x, std::max(segments[1].x, std::max(segments[2].x, segments[3].x)));
			bornes.coinMax.y = std::max(segments[0].y, std::max(segments[1].y, std::max(segments[2].y, segments[3].y)));
			bornes.coinMin.x = std::min(segments[0].x, std::min(segments[1].x, std::min(segments[2].x, segments[3].x)));
			bornes.coinMin.y = std::min(segments[0].y, std::min(segments[1].y, std::min(segments[2].y, segments[3].y)));
		}
	}
	
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::calculerSegments()
///
/// Cette fonction calcule les segments de l'objet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::calculerSegments()
{
	double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
		agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y*compression*cos(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
		0.0, 0.0, agrandissement_.z, 0.0,
		positionRelative_.x, positionRelative_.y - (1 - compression) / 2 * hauteurBase, 0, 1.0 };

	segments[0] = utilitaire::appliquerMatrice(segmentsBase[0], transformation);
	segments[1] = utilitaire::appliquerMatrice(segmentsBase[1], transformation);
	segments[2] = utilitaire::appliquerMatrice(segmentsBase[2], transformation);
	segments[3] = utilitaire::appliquerMatrice(segmentsBase[3], transformation);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudRessort(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::executerCollision(NoeudBille* bille)
{
	bornes.coinMax.z = 0;
	bornes.coinMin.z = 0;
	double force = 0;
	if (enDecompression)
		force = (1 - compression)*1000;

	collisionAvecMur(bille, segments[0], segments[1], force, true);
	collisionAvecMur(bille, segments[3], segments[0], 0, false);
	collisionAvecMur(bille, segments[1], segments[2], 0, false);
	collisionAvecMur(bille, segments[2], segments[3], 0, false);
	
}

double NoeudRessort::obtenirEnfoncement(NoeudBille* bille)
{
	double enfoncementMax = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[0], segments[1]);
	double enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[1], segments[2]);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[2], segments[3]);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[3], segments[0]);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	return enfoncementMax;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRessort::collisionAvecMur(NoeudBille* bille, double dt, glm::dvec3 point1, glm::dvec3 point2, double forceSup, bool topDuRessort)
///
/// Cette fonction teste et execute une collision avec un segment de droite
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
/// @param[in] point1 : Premier point du segment.
/// @param[in] point2 : Second point du segment.
/// @param[in] forceSup : Force supplementaire a appliquer sur la bille
/// @param[in] topDuRessort : Indique s'il s'agit du dessus du ressort
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRessort::collisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, double forceSup, bool topDuRessort)
{
	double rayonBille = bille->obtenirRayon();
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionSegment(point1, point2, bille->obtenirPositionRelative(), rayonBille, true);
	
	if (test.type != aidecollision::COLLISION_AUCUNE)
	{
		glm::dvec2 vitesse = bille->obtenirVitesse();
		glm::dvec2 normale = glm::normalize(glm::dvec2(test.direction.x, test.direction.y));
		if (topDuRessort)
		{
			glm::dvec3 segment = point2 - point1;
			if (glm::cross(segment, test.direction).z < 0)
			{
				normale = -normale;
				bille->assignerPositionRelative(bille->obtenirPositionRelative() - 2.0*test.direction*test.enfoncement);
			}
			
		}
		
		if (!collisionCettePhase)
		{
			double forceSupplementaire = enDecompression ? forceSup : 0;
			vitesse = normale*std::abs(2 * glm::dot(normale, vitesse) + forceSupplementaire);
			if (topDuRessort && !enDecompression)
			{
				if (!enCompression)
				{
					vitesse *= 0.9;
				}
				else
				{ 
					vitesse *= 0.6;
				}
			}
			else if (topDuRessort)
			{
				collisionCettePhase = true;
			}

			bille->setCollision(true, true);

			bille->assignerVitesse(vitesse + bille->obtenirVitesse());
		}
		glm::dvec2 enfoncementVec = normale*(test.enfoncement + 0.1);
		glm::dvec3 nouvellePos = bille->obtenirPositionRelative() + glm::dvec3(enfoncementVec.x, enfoncementVec.y, 0);
		bille->assignerPositionRelative(nouvellePos);
	}
}
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
