////////////////////////////////////////////////////////////////////////////////////
/// @file NoeudCompositeTest.cpp
/// @author Konstantin Fedorov
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "NoeudCompositeTest.h"
#include "Arbre/arbreRenduINF2990.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(NoeudCompositeTest);


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCompositeTest::setUp()
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
void NoeudCompositeTest::setUp()
{
	arbre_ = std::make_unique<ArbreRenduINF2990>();
	NoeudAbstrait* nouveauNoeud;
	glm::dvec3 position;
	// On vide l'arbre
	arbre_->vider();

	// On ajoute un noeud bidon seulement pour que quelque chose s'affiche.
	NoeudAbstrait* noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_ZONEDEJEU);
	noeud->assignerEstSelectionnable(false);

	nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_TROU);
	position = glm::dvec3(0, -75, 0);
	nouveauNoeud->assignerPositionRelative(position);
	noeud->ajouter(nouveauNoeud);

	nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_RESSORT);
	position = glm::dvec3(42, -70, 0);
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
/// @fn void NoeudCompositeTest::tearDown()
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
void NoeudCompositeTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCompositeTest::testChercher()
///
/// Cas de test: Recherche des noeuds de base
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void NoeudCompositeTest::testChercher()
{
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	zone->ajouter(noeud);
	CPPUNIT_ASSERT(zone != nullptr);
	NoeudAbstrait* noeudCherche = zone->chercher(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	CPPUNIT_ASSERT(noeudCherche == noeud);
	CPPUNIT_ASSERT(noeudCherche->obtenirParent() == zone);
	noeudCherche = zone->chercher(1);
	CPPUNIT_ASSERT(noeudCherche != nullptr);
	CPPUNIT_ASSERT(noeudCherche->obtenirParent() == zone);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCompositeTest::testEstDansBornes()
///
/// Cas de test: effacer un noeud
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void NoeudCompositeTest::testEffacer()
{
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	zone->ajouter(noeud);
	CPPUNIT_ASSERT(noeud != nullptr);
	CPPUNIT_ASSERT(noeud->obtenirParent() == zone);
	int nbEnfants = zone->obtenirNombreEnfants();
	zone->effacer(noeud);
	CPPUNIT_ASSERT(zone->chercher(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE)==nullptr);
	CPPUNIT_ASSERT(zone->obtenirNombreEnfants() == nbEnfants - 1);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCompositeTest::testVider()
///
/// Cas de test: effacer tout les enfants
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void NoeudCompositeTest::testVider()
{
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	zone->ajouter(noeud);
	zone->vider();
	CPPUNIT_ASSERT(zone->obtenirNombreEnfants() == 0);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCompositeTest::testEffacerSelection()
///
/// Cas de test: Effacer tout les selectionnes
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void NoeudCompositeTest::testEffacerSelection()
{
	NoeudAbstrait * noeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	NoeudAbstrait * zone = arbre_->chercher("zonedejeu");
	zone->ajouter(noeud);
	noeud->inverserSelection();
	zone->chercher("ressort")->inverserSelection();
	zone->effacerSelection();
	CPPUNIT_ASSERT(zone->obtenirNombreEnfants() == 2);
	CPPUNIT_ASSERT(zone->chercher("ressort") == nullptr);
	CPPUNIT_ASSERT(zone->chercher("ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE") == nullptr);
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
