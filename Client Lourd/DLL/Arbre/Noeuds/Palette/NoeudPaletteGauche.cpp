///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPalettegauche.cpp
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-03-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudPaletteGauche.h"


#include "Utilitaire.h"
#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "Arbre/Noeuds/NoeudBille.h"
#include <algorithm>
#include "Reseau/NetworkManager.h"
#include "Affichage/Affichage.h"


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteGauche::NoeudPaletteGauche(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteGauche::NoeudPaletteGauche(const std::string& typeNoeud)
	: NoeudPalette{ typeNoeud }
{
	rotAction = 0;
	boutonAppuye = false;
	aRecalculer = true;
	bornesBaseTrouvees = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteGauche::~NoeudPaletteGauche()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteGauche::~NoeudPaletteGauche()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteGauche::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteGauche::afficherConcret() const
{

	// Sauvegarde de la matrice.
	glPushMatrix();

	glRotated(rotAction, 0, 0, 1);

	if (NetworkManager::getInstance()->estJeuEnLigne())
	{
		auto couleur = obtenirCouleurPalette();
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "changerCouleur"), 1);
		glUniform4f(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "couleurForcee"), couleur[0], couleur[1], couleur[2], 0.3);
	}
	glTranslated(0, 0, -0.05*playerNum_); // fix z-fighting
	if (fantome_)
	{
		glBlendFunc(GL_SRC_ALPHA, GL_ONE);
	}
	// Affichage du modèle.
	liste_->dessiner();

	if (fantome_)
	{
		glBlendFunc(GL_ONE, GL_ZERO);
	}

	glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "changerCouleur"), 0);
	// Restauration de la matrice.
	glPopMatrix();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteGauche::calculerBornes()
///
/// Cette fonction calcule les bornes de l'objet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteGauche::calculerBornes()
{
	if (!bornesBaseTrouvees && modele_ != nullptr)
	{
		bornes = utilitaire::calculerBoiteEnglobante(*modele_);
		bornesBaseTrouvees = true;

		segmentsBase[0] = glm::dvec3(bornes.coinMin.x, bornes.coinMax.y, 0);
		segmentsBase[1] = glm::dvec3(bornes.coinMax.x, bornes.coinMax.y, 0);
		segmentsBase[2] = glm::dvec3(bornes.coinMax.x, bornes.coinMin.y, 0);
		segmentsBase[3] = glm::dvec3(bornes.coinMin.x, bornes.coinMin.y, 0);
	}
	if (modele_ != nullptr){
		calculerPoints();
		calculerSegments();
	}
}



////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteGauche::calculerPoints()
///
/// Cette fonction calcule les points de l'objet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteGauche::calculerPoints(){
	double rot = rotation_ + rotAction;
	double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
		agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
		0.0, 0.0, agrandissement_.z, 0.0,
		positionRelative_.x, positionRelative_.y, 0, 1.0 };

	glm::dvec3 tempSegment[2][2] = { { glm::dvec3(0, 3, 0), glm::dvec3(18, 0.9, 0) },
	{ glm::dvec3(0, -3, 0), glm::dvec3(18, -0.9, 0) }
	};
	glm::dvec3 tempCercles[2] = { glm::dvec3(0, 0, 0), glm::dvec3(18, 0, 0) };

	for (int i = 0; i < 2; i++)
	{
		for (int j = 0; j < 2; j++)
		{
			segments[i][j] = utilitaire::appliquerMatrice(tempSegment[i][j], transformation);
		}
	}
	for (int i = 0; i < 2; i++)
	{
		cercles[i] = utilitaire::appliquerMatrice(tempCercles[i], transformation);
	}
	rayons[0] = agrandissement_.y * 3;
	rayons[1] = agrandissement_.y*0.9;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteGauche::calculerSegments()
///
/// Cette fonction calcule les segments de l'objet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteGauche::calculerSegments()
{
	glm::dvec3 segmentsBorne[4];
	double rot = rotation_;
	double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
		agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
		0.0, 0.0, agrandissement_.z, 0.0,
		positionRelative_.x, positionRelative_.y, 0, 1.0 };


	segmentsBorne[0] = utilitaire::appliquerMatrice(segmentsBase[0], transformation);
	segmentsBorne[1] = utilitaire::appliquerMatrice(segmentsBase[1], transformation);
	segmentsBorne[2] = utilitaire::appliquerMatrice(segmentsBase[2], transformation);
	segmentsBorne[3] = utilitaire::appliquerMatrice(segmentsBase[3], transformation);

	bornes.coinMax.x = std::max(segmentsBorne[0].x, std::max(segmentsBorne[1].x, std::max(segmentsBorne[2].x, segmentsBorne[3].x)));
	bornes.coinMax.y = std::max(segmentsBorne[0].y, std::max(segmentsBorne[1].y, std::max(segmentsBorne[2].y, segmentsBorne[3].y)));
	bornes.coinMin.x = std::min(segmentsBorne[0].x, std::min(segmentsBorne[1].x, std::min(segmentsBorne[2].x, segmentsBorne[3].x)));
	bornes.coinMin.y = std::min(segmentsBorne[0].y, std::min(segmentsBorne[1].y, std::min(segmentsBorne[2].y, segmentsBorne[3].y)));

	rot = rotation_ + 60;
	double transformation2[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
		agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
		0.0, 0.0, agrandissement_.z, 0.0,
		positionRelative_.x, positionRelative_.y, 0, 1.0 };


	segmentsBorne[0] = utilitaire::appliquerMatrice(segmentsBase[0], transformation2);
	segmentsBorne[1] = utilitaire::appliquerMatrice(segmentsBase[1], transformation2);
	segmentsBorne[2] = utilitaire::appliquerMatrice(segmentsBase[2], transformation2);
	segmentsBorne[3] = utilitaire::appliquerMatrice(segmentsBase[3], transformation2);

	bornes.coinMax.x = std::max(bornes.coinMax.x, std::max(segmentsBorne[0].x, std::max(segmentsBorne[1].x, std::max(segmentsBorne[2].x, segmentsBorne[3].x))));
	bornes.coinMax.y = std::max(bornes.coinMax.y, std::max(segmentsBorne[0].y, std::max(segmentsBorne[1].y, std::max(segmentsBorne[2].y, segmentsBorne[3].y))));
	bornes.coinMin.x = std::min(bornes.coinMin.x, std::min(segmentsBorne[0].x, std::min(segmentsBorne[1].x, std::min(segmentsBorne[2].x, segmentsBorne[3].x))));
	bornes.coinMin.y = std::min(bornes.coinMin.y, std::min(segmentsBorne[0].y, std::min(segmentsBorne[1].y, std::min(segmentsBorne[2].y, segmentsBorne[3].y))));

	if (((int)rot) / 90 != ((int)rotation_) / 90)
	{

		rot = (((int)rot) / 90) * 90;
		double transformation3[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
			agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rot)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rot)), 0.0, 0.0,
			0.0, 0.0, agrandissement_.z, 0.0,
			positionRelative_.x, positionRelative_.y, 0, 1.0 };


		segmentsBorne[0] = utilitaire::appliquerMatrice(segmentsBase[0], transformation3);
		segmentsBorne[1] = utilitaire::appliquerMatrice(segmentsBase[1], transformation3);
		segmentsBorne[2] = utilitaire::appliquerMatrice(segmentsBase[2], transformation3);
		segmentsBorne[3] = utilitaire::appliquerMatrice(segmentsBase[3], transformation3);

		bornes.coinMax.x = std::max(bornes.coinMax.x, std::max(segmentsBorne[0].x, std::max(segmentsBorne[1].x, std::max(segmentsBorne[2].x, segmentsBorne[3].x))));
		bornes.coinMax.y = std::max(bornes.coinMax.y, std::max(segmentsBorne[0].y, std::max(segmentsBorne[1].y, std::max(segmentsBorne[2].y, segmentsBorne[3].y))));
		bornes.coinMin.x = std::min(bornes.coinMin.x, std::min(segmentsBorne[0].x, std::min(segmentsBorne[1].x, std::min(segmentsBorne[2].x, segmentsBorne[3].x))));
		bornes.coinMin.y = std::min(bornes.coinMin.y, std::min(segmentsBorne[0].y, std::min(segmentsBorne[1].y, std::min(segmentsBorne[2].y, segmentsBorne[3].y))));
	}

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteDroit::executerCollision(NoeudBille* bille, double dt)
///
/// Cette fonction teste et execute une collision avec l'objet
///
/// @param[in] bille : La bille avec laquelle sera teste la collision
/// @param[in] dt : Intervalle de temps sur lequel faire la collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteGauche::executerCollision(NoeudBille* bille)
{
	if (!NetworkManager::getInstance()->estCompetitif() || bille->obtenirPlayerNum() == playerNum_)
	{
		double forceFrappe = 0;
		double dist = glm::length(positionRelative_ - bille->obtenirPositionRelative());
		if (rotAction < 60 && boutonAppuye)
			forceFrappe = dist*2;
		else if (rotAction > 0 && !boutonAppuye)
			forceFrappe = -dist;
	
		executerCollisionAvecMur(bille, segments[0][0], segments[0][1], segments[1][1], segments[1][0], forceFrappe);
		executerCollisionAvecCercle(bille, cercles[1], rayons[1], forceFrappe);
		executerCollisionAvecCercle(bille, cercles[0], rayons[0], 0);
	}
	
}

double NoeudPaletteGauche::obtenirEnfoncement(NoeudBille* bille) 
{
	if(NetworkManager::getInstance()->estCompetitif() && bille->obtenirPlayerNum() != playerNum_)
	{
		return 0;
	}

	// Check pour animer palette
	billeDansZone_ = false;
	glm::dvec3 pos = bille->obtenirPositionRelative();
	glm::dvec2 vitesse = bille->obtenirVitesse();
	if (utilitaire::DANS_INTERVALLE(pos.x, bornes.coinMin.x, bornes.coinMax.x) && utilitaire::DANS_INTERVALLE(pos.y, bornes.coinMin.y, bornes.coinMax.y))
	{
		glm::dvec3 mur = segments[0][1] - segments[0][0];
		glm::dvec3 directionABille{ pos - segments[0][0] };
		if (glm::cross(mur, directionABille).z > 0)
		{
			billeDansZone_ = true;
		}
	}

	double enfoncementMax = obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), cercles[0], rayons[0]);
	double enfoncement = obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), cercles[1], rayons[1]);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[0][0], segments[0][1]);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	enfoncement = obtenirEnfoncementSegment(bille->obtenirPositionRelative(), bille->obtenirRayon(), segments[1][0], segments[1][1]);
	if (enfoncementMax < enfoncement)
		enfoncementMax = enfoncement;
	return enfoncementMax;
}

bool NoeudPaletteGauche::obtenirEstDeDroite()
{
	return false;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////