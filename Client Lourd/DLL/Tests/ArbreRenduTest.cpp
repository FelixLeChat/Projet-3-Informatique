////////////////////////////////////////////////////////////////////////////////////
/// @file ArbreRenduTest.cpp
/// @author Konstantin Fedorov
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "ArbreRenduTest.h"
#include "Arbre/ArbreRenduINF2990.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(ArbreRenduTest);


////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::setUp()
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
void ArbreRenduTest::setUp()
{
	arbre_ = std::make_unique<ArbreRenduINF2990>();
	NoeudAbstrait* nouveauNoeud;
	glm::dvec3 position;
	// On ajoute un noeud bidon seulement pour que quelque chose s'affiche.
	NoeudAbstrait* noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_ZONEDEJEU);
	noeud->assignerEstSelectionnable(false);

	nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_TROU);
	position = glm::dvec3(0, -75, 0);
	nouveauNoeud->assignerPositionRelative(position);
	noeud->ajouter(nouveauNoeud);

	nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_RESSORT);
	position = glm::dvec3(41.32, -69.79, 0);
	nouveauNoeud->assignerPositionRelative(position);
	noeud->ajouter(nouveauNoeud);

	nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_GENERATEURBILLE);
	position = glm::dvec3(35, -45, 0);
	nouveauNoeud->assignerPositionRelative(position);
	noeud->ajouter(nouveauNoeud);

	arbre_->ajouter(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::tearDown()
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
void ArbreRenduTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testCreerNoeud()
///
/// Cas de test: Creation de noeuds
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ArbreRenduTest::testCreerNoeud()
{
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	noeud->assignerPositionRelative(glm::dvec3(5, 10, 15));
	noeud->assignerAgrandissement(glm::dvec3(1.7, 1.7, 1.7));
	noeud->assignerRotation(20);
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	zone->ajouter(noeud);

	NoeudAbstrait * noeud2 = zone->chercher(zone->obtenirNombreEnfants() - 1);
	CPPUNIT_ASSERT(noeud2 != nullptr);
	CPPUNIT_ASSERT(noeud2->obtenirRotation() == 20);
	glm::dvec3 position = noeud2->obtenirPositionRelative();
	CPPUNIT_ASSERT(position.x == 5);
	CPPUNIT_ASSERT(position.y == 10);
	CPPUNIT_ASSERT(position.z == 15);
	CPPUNIT_ASSERT(noeud2->obtenirAgrandissement().x == 1.7);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testEstDansBornes()
///
/// Cas de test: verification si tout les objest sont dans la zone de jeu
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ArbreRenduTest::testEstDansBornes()
{
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	noeud->assignerPositionRelative(glm::dvec3(5, 10, 15));
	noeud->assignerAgrandissement(glm::dvec3(1.7, 1.7, 1.7));
	noeud->assignerRotation(20);
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	zone->ajouter(noeud);

	CPPUNIT_ASSERT(arbre_->estDansBornes());
	noeud->assignerPositionRelative(glm::dvec3(-100, -100, 0));
	CPPUNIT_ASSERT(!arbre_->estDansBornes());
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testObtenirDepassement()
///
/// Cas de test: Calcul du plus grand depassement des objets par rapport a la zone de jeu
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ArbreRenduTest::testObtenirDepassement()
{
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	noeud->assignerPositionRelative(glm::dvec3(5, 10, 15));
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	zone->ajouter(noeud);

	glm::dvec2 depassement = arbre_->obtenirDepassement();
	//CPPUNIT_ASSERT(depassement.x == 0);
	//CPPUNIT_ASSERT(depassement.y == 0);
	noeud->assignerPositionRelative(glm::dvec3(100, 100, 0));
	depassement = arbre_->obtenirDepassement();
	//CPPUNIT_ASSERT(depassement.x==62.5);
	//CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(depassement.y - 27.5));
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testCentreSelection()
///
/// Cas de test: Calcul du centre de la selection
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ArbreRenduTest::testCentreSelection()
{
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	noeud->assignerPositionRelative(glm::dvec3(5, 10, 15));
	noeud->inverserSelection();
	zone->ajouter(noeud);

	noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	noeud->assignerPositionRelative(glm::dvec3(-5, 7, 15));
	noeud->inverserSelection();
	zone->ajouter(noeud);

	noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	noeud->assignerPositionRelative(glm::dvec3(10, 8, 15));
	noeud->inverserSelection();
	zone->ajouter(noeud);

	noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	noeud->assignerPositionRelative(glm::dvec3(-3, -15, 15));
	noeud->inverserSelection();
	zone->ajouter(noeud);
	arbre_->calculerCentreSelection();
	glm::dvec3 centre = arbre_->obtenirCentreSelection();
	CPPUNIT_ASSERT(centre.x == 2.5);
	CPPUNIT_ASSERT(centre.y == -2.5);

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testEssentielSelectionne()
///
/// Cas de test: Verification si tout les objets d'un type d'objet essentiel sont selectionnes
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ArbreRenduTest::testEssentielSelectionne()
{
	CPPUNIT_ASSERT(!arbre_->tousEssentielSelectionne());
	arbre_->chercher("zonedejeu")->chercher("ressort")->inverserSelection();
	CPPUNIT_ASSERT(arbre_->tousEssentielSelectionne());
}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
