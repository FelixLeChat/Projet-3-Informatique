#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif

///////////////////////////////////////////////////////////////////////////
/// @file ArbreRenduINF2990.cpp
/// @author Martin Bisson
/// @date 2007-03-23
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#include "ArbreRenduINF2990.h"
#include "Usines/UsineNoeudZoneDeJeu.h"
#include "Usines/UsineNoeudTrou.h"
#include "Usines/UsineNoeudRessort.h"
#include "Usines/UsineNoeudGenerateurBille.h"

#include "Usines\UsineNoeudPaletteGaucheJ1.h"
#include "Usines\UsineNoeudPaletteDroitJ1.h"
#include "Usines\UsineNoeudPaletteGaucheJ2.h"
#include "Usines\UsineNoeudPaletteDroitJ2.h"

#include "Usines\UsineNoeudButoirCirculaire.h"
#include "Usines\UsineNoeudButoirTriangulaireGauche.h"
#include "Usines\UsineNoeudButoirTriangulaireDroit.h"

#include "Usines\UsineNoeudCible.h"

#include "Usines\UsineNoeudMur.h"
#include "Usines\UsineNoeudPortail.h"
#include "Usines\UsineNoeudBille.h"

#include "Usines\UsineNoeudChampForce.h"

#include "Usines\UsineNoeudPlateauDArgent.h"

//#include "../Application/FacadeModele.h"

#include "EtatOpenGL.h"


/// La chaîne représentant le type de zone de jeu
const string ArbreRenduINF2990::NOM_ZONEDEJEU{ "zonedejeu" };
/// La chaîne représentant le type de trou
const string ArbreRenduINF2990::NOM_TROU{ "trou" };
/// La chaîne représentant le type de ressort
const string ArbreRenduINF2990::NOM_RESSORT{ "ressort" };
/// La chaîne représentant le type de générateur de bille
const string ArbreRenduINF2990::NOM_GENERATEURBILLE{ "generateurbille" };
/// La chaîne représentant le type de butoir circulaire
const string ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE{ "butoirCercle" };
/// La chaîne représentant le type de butoir triangulaire gauche
const string ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREGAUCHE{ "butoirTriangleGauche" }; 
/// La chaîne représentant le type de butoir triangulaire droit
const string ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREDROIT{ "butoirTriangleDroit" }; 
/// La chaîne représentant le type de cible
const string ArbreRenduINF2990::NOM_CIBLE{ "cible" };
/// La chaîne représentant le type de palette gauche du joueur 1
const string ArbreRenduINF2990::NOM_PALETTEGAUCHEJ1{ "paletteGaucheJ1" };
/// La chaîne représentant le type de palette droite du joueur 1
const string ArbreRenduINF2990::NOM_PALETTEDROITJ1{ "paletteDroitJ1" };
/// La chaîne représentant le type de palette gauche du joueur 2
const string ArbreRenduINF2990::NOM_PALETTEGAUCHEJ2{ "paletteGaucheJ2" };
/// La chaîne représentant le type de palette droite du joueur 2
const string ArbreRenduINF2990::NOM_PALETTEDROITJ2{ "paletteDroitJ2" };
/// La chaîne représentant le type de portail
const string ArbreRenduINF2990::NOM_PORTAIL{ "portail" };
/// La chaîne représentant le type de mur
const string ArbreRenduINF2990::NOM_MUR{ "mur" };
/// La chaîne représentant le type d'une bille.
const string ArbreRenduINF2990::NOM_BILLE{ "bille" };

const string ArbreRenduINF2990::NOM_CHAMPFORCE{ "champForce" };

const string ArbreRenduINF2990::NOM_PLATEAUDARGENT{ "plateauDArgent" };

////////////////////////////////////////////////////////////////////////
///
/// @fn ArbreRenduINF2990::ArbreRenduINF2990()
///
/// Ce constructeur crée toutes les usines qui seront utilisées par le
/// projet de INF2990et les enregistre auprès de la classe de base.
/// Il crée également la structure de base de l'arbre de rendu, c'est-à-dire
/// avec les noeuds structurants.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
ArbreRenduINF2990::ArbreRenduINF2990()
{
	// Construction des usines
	ajouterUsine(NOM_ZONEDEJEU, new UsineNoeudZoneDeJeu{ NOM_ZONEDEJEU });
	ajouterUsine(NOM_TROU, new UsineNoeudTrou{ NOM_TROU });
	ajouterUsine(NOM_RESSORT, new UsineNoeudRessort{ NOM_RESSORT });
	ajouterUsine(NOM_GENERATEURBILLE, new UsineNoeudGenerateurBille{ NOM_GENERATEURBILLE });

	ajouterUsine(NOM_PALETTEGAUCHEJ1, new UsineNoeudPaletteGaucheJ1{ NOM_PALETTEGAUCHEJ1 });
	ajouterUsine(NOM_PALETTEDROITJ1, new UsineNoeudPaletteDroitJ1{ NOM_PALETTEDROITJ1 });
	ajouterUsine(NOM_PALETTEGAUCHEJ2, new UsineNoeudPaletteGaucheJ2{ NOM_PALETTEGAUCHEJ2 });
	ajouterUsine(NOM_PALETTEDROITJ2, new UsineNoeudPaletteDroitJ2{ NOM_PALETTEDROITJ2 });

	ajouterUsine(NOM_BUTOIRCIRCULAIRE, new UsineNoeudButoirCirculaire{ NOM_BUTOIRCIRCULAIRE });
	ajouterUsine(NOM_BUTOIRTRIANGULAIREGAUCHE, new UsineNoeudButoirTriangulaireGauche{ NOM_BUTOIRTRIANGULAIREGAUCHE });
	ajouterUsine(NOM_BUTOIRTRIANGULAIREDROIT, new UsineNoeudButoirTriangulaireDroit{ NOM_BUTOIRTRIANGULAIREDROIT });
	ajouterUsine(NOM_CIBLE, new UsineNoeudCible{ NOM_CIBLE });


	ajouterUsine(NOM_MUR, new UsineNoeudMur{ NOM_MUR });
	ajouterUsine(NOM_PORTAIL, new UsineNoeudPortail{ NOM_PORTAIL });
	ajouterUsine(NOM_BILLE, new UsineNoeudBille{ NOM_BILLE });

	ajouterUsine(NOM_CHAMPFORCE, new UsineNoeudChampForce{ NOM_CHAMPFORCE });
	ajouterUsine(NOM_PLATEAUDARGENT, new UsineNoeudPlateauDArgent{ NOM_PLATEAUDARGENT });

}


////////////////////////////////////////////////////////////////////////
///
/// @fn ArbreRenduINF2990::~ArbreRenduINF2990()
///
/// Ce destructeur ne fait rien pour le moment.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
ArbreRenduINF2990::~ArbreRenduINF2990()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduINF2990::initialiser()
///
/// Cette fonction crée la structure de base de l'arbre de rendu, c'est-à-dire
/// avec les noeuds structurants (pour les objets, les murs, les billes,
/// les parties statiques, etc.)
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ArbreRenduINF2990::initialiser()
{
	// On vide l'arbre
	vider();

	// On ajoute un noeud bidon seulement pour que quelque chose s'affiche.
	NoeudAbstrait* noeud = creerNoeud(NOM_ZONEDEJEU);
	noeud->assignerEstSelectionnable(false);

	auto zoneDeJeu = static_cast<NoeudZoneDeJeu *>(noeud);
	NoeudAbstrait* nouveauNoeud = creerNoeud(NOM_TROU);
	zoneDeJeu->ajouter(nouveauNoeud);
	nouveauNoeud->assignerPositionRelative(glm::dvec3(0, -74, 0));

	NoeudAbstrait* nouveauNoeud1 = creerNoeud(NOM_RESSORT);
	zoneDeJeu->ajouter(nouveauNoeud1);
	nouveauNoeud1->assignerPositionRelative(glm::dvec3(41.32, -69.79, 0));

	NoeudAbstrait* nouveauNoeud2 = creerNoeud(NOM_GENERATEURBILLE);
	zoneDeJeu->ajouter(nouveauNoeud2);
	nouveauNoeud2->assignerPositionRelative(glm::dvec3(30, -35, 0));

	ajouter(zoneDeJeu);
}
