////////////////////////////////////////////////////////////////////////////////////
/// @file VisiteursTest.cpp
/// @author Jérémie Gagné
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "VisiteursTest.h"
#include "../Arbre/Visiteur/VisiteurDeplacement.h"
#include "../Arbre/Visiteur/VisiteurRotation.h"
#include "../Arbre/Visiteur/VisiteurMiseAEchelle.h"


// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(VisiteursTest);


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteursTest::setUp()
///
/// Effectue l'initialisation préalable à l'exécution de l'ensemble des
/// cas de tests de cette suite de tests (si nécessaire).
/// 
/// Si certains objets doivent être construits, il est conseillé de le
/// faire ici.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteursTest::setUp()
{
	arbre = std::make_unique<ArbreRenduINF2990>();

	NoeudAbstrait* nouveauNoeud;
	glm::dvec3 position;
	// On vide l'arbre
	arbre->vider();

	// On ajoute un noeud bidon seulement pour que quelque chose s'affiche.
	NoeudAbstrait* noeud = arbre->creerNoeud(ArbreRenduINF2990::NOM_ZONEDEJEU);
	noeud->assignerEstSelectionnable(false);

	nouveauNoeud = arbre->creerNoeud(ArbreRenduINF2990::NOM_TROU);
	position = glm::dvec3(0, -75, 0);
	nouveauNoeud->assignerPositionRelative(position);
	noeud->ajouter(nouveauNoeud);

	nouveauNoeud = arbre->creerNoeud(ArbreRenduINF2990::NOM_RESSORT);
	position = glm::dvec3(42, -70, 0);
	nouveauNoeud->assignerPositionRelative(position);
	noeud->ajouter(nouveauNoeud);

	nouveauNoeud = arbre->creerNoeud(ArbreRenduINF2990::NOM_GENERATEURBILLE);
	position = glm::dvec3(35, -45, 0);
	nouveauNoeud->assignerPositionRelative(position);
	noeud->ajouter(nouveauNoeud);

	arbre->ajouter(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteursTest::tearDown()
///
/// Effectue les opérations de finalisation nécessaires suite à l'exécution
/// de l'ensemble des cas de tests de cette suite de tests (si nécessaire).
/// 
/// Si certains objets ont été alloués à l'initialisation, ils doivent être
/// désalloués, et il est conseillé de le faire ici.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteursTest::tearDown()
{
	arbre->vider();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteursTest::testDeplacement()
///
/// Cas de test: Visiteur de deplacement
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteursTest::testDeplacement()
{
	arbre->selectionnerTout();
	int dx = -10;
	int dy = 20;

	glm::dvec3 positionRessort = arbre->chercher(ArbreRenduINF2990::NOM_RESSORT)->obtenirPositionRelative();
	glm::dvec3 positionTrou = arbre->chercher(ArbreRenduINF2990::NOM_TROU)->obtenirPositionRelative();
	VisiteurDeplacement visiteur(dx, dy);

	arbre->accepterVisiteur(&visiteur);

	glm::dvec3 nouvellePositionRessort = arbre->chercher(ArbreRenduINF2990::NOM_RESSORT)->obtenirPositionRelative();
	glm::dvec3 nouvellePositionTrou = arbre->chercher(ArbreRenduINF2990::NOM_TROU)->obtenirPositionRelative();

	CPPUNIT_ASSERT((nouvellePositionRessort.x - positionRessort.x) == dx);
	CPPUNIT_ASSERT((nouvellePositionRessort.y - positionRessort.y) == dy);
	CPPUNIT_ASSERT((nouvellePositionTrou.x - positionTrou.x) == dx);
	CPPUNIT_ASSERT((nouvellePositionTrou.y - positionTrou.y) == dy);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteursTest::testRotation()
///
/// Cas de test: Visiteur de rotation
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteursTest::testRotation()
{
	int dy = 90;
	NoeudAbstrait* butoir = arbre->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	butoir->assignerPositionRelative(glm::dvec3(10, 10, 10));
	if (!butoir->estSelectionne())
		butoir->inverserSelection();

	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(butoir);

	double rotationInitiale = butoir->obtenirRotation();

	VisiteurRotation visiteur(dy, butoir->obtenirPositionRelative());
	arbre->accepterVisiteur(&visiteur);

	double rotationFinale = butoir->obtenirRotation();

	CPPUNIT_ASSERT((rotationFinale - rotationInitiale) == dy);


	NoeudAbstrait* butoir2 = arbre->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	glm::dvec3 positionInitial(-10, -20, 10);
	butoir2->assignerPositionRelative(positionInitial);
	if (!butoir2->estSelectionne())
		butoir2->inverserSelection();

	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(butoir2);

	rotationInitiale = butoir2->obtenirRotation();
	glm::dvec3 centreSelection = arbre->obtenirCentreSelection();

	VisiteurRotation rotationMultiple(dy, centreSelection);
	arbre->accepterVisiteur(&rotationMultiple);

	rotationFinale = butoir2->obtenirRotation();
	glm::dvec3 positionFinale = butoir2->obtenirPositionRelative();

	CPPUNIT_ASSERT((rotationFinale - rotationInitiale) == dy);
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(positionFinale.x - 20));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(positionFinale.y + 10));
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteursTest::testMiseAEchelle()
///
/// Cas de test: Visiteur de mise a l'echelle
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteursTest::testMiseAEchelle()
{
	double dy = 0.5;
	NoeudAbstrait* butoir = arbre->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	NoeudAbstrait* mur = arbre->creerNoeud(ArbreRenduINF2990::NOM_MUR);
	NoeudAbstrait* ressort = arbre->creerNoeud(ArbreRenduINF2990::NOM_RESSORT);
	NoeudAbstrait* palette = arbre->creerNoeud(ArbreRenduINF2990::NOM_PALETTEDROITJ1);
	if (!butoir->estSelectionne())
		butoir->inverserSelection();
	if (!mur->estSelectionne())
		mur->inverserSelection();
	if (!ressort->estSelectionne())
		ressort->inverserSelection();
	if (!palette->estSelectionne())
		palette->inverserSelection();

	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(butoir);
	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(mur);
	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(ressort);
	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(palette);

	glm::dvec3 echelleInitialeB = butoir->obtenirAgrandissement();
	glm::dvec3 echelleInitialeM = mur->obtenirAgrandissement();
	glm::dvec3 echelleInitialeR = ressort->obtenirAgrandissement();
	glm::dvec3 echelleInitialeP = palette->obtenirAgrandissement();

	VisiteurMiseAEchelle visiteur(dy);
	arbre->accepterVisiteur(&visiteur);

	glm::dvec3 echelleFinaleB = butoir->obtenirAgrandissement();
	glm::dvec3 echelleFinaleM = mur->obtenirAgrandissement();
	glm::dvec3 echelleFinaleR = ressort->obtenirAgrandissement();
	glm::dvec3 echelleFinaleP = palette->obtenirAgrandissement();

	CPPUNIT_ASSERT((echelleFinaleB.x - echelleInitialeB.x) == dy);
	CPPUNIT_ASSERT((echelleFinaleB.y - echelleInitialeB.y) == dy);
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(echelleFinaleM.x - echelleInitialeM.x));
	CPPUNIT_ASSERT((echelleFinaleM.y - echelleInitialeM.y) == dy);
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(echelleFinaleR.x - echelleInitialeR.x));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(echelleFinaleP.x - echelleInitialeP.x));
}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
