////////////////////////////////////////////////////////////////////////////////////
/// @file NoeudPortailTest.cpp
/// @author Jérémie Gagné
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "NoeudPortailTest.h"
#include "../Arbre/Noeuds/NoeudPortail.h"
#include "../Application/FacadeModele.h"


// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(NoeudPortailTest);


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortailTest::setUp()
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
void NoeudPortailTest::setUp()
{
	arbre = make_unique<ArbreRenduINF2990>();

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
/// @fn void NoeudPortailTest::tearDown()
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
void NoeudPortailTest::tearDown()
{
	arbre->vider();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortailTest::testAnnulerCreation()
///
/// Cas de test: Annuler la creation d'un couple de portail
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortailTest::testAnnulerCreation()
{
	/*NoeudPortail* portail1 = dynamic_cast<NoeudPortail*> (arbre->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL));


	FacadeModele::obtenirInstance()->initialiserPourTests();
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(portail1);

	FacadeModele::obtenirInstance()->annulerCreation();

	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO((int)(FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_PORTAIL))));*/

	//REDO TESTS WITH EDITOR
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortailTest::testEffacerPortail()
///
/// Cas de test: Effacer un portail doit aussi effacer le frere
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortailTest::testEffacerPortail()
{
	NoeudPortail* portail1 = dynamic_cast<NoeudPortail*> (arbre->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL));
	NoeudPortail* portail2 = dynamic_cast<NoeudPortail*> (arbre->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL));

	portail1->assignerFrere(portail2);
	portail2->assignerFrere(portail1);

	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(portail1);
	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(portail2);

	arbre->deselectionnerTout();
	portail1->inverserSelection();

	arbre->effacerSelection();

	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO((int)arbre->chercher(ArbreRenduINF2990::NOM_PORTAIL)));

	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->vider();

	portail1 = dynamic_cast<NoeudPortail*> (arbre->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL));
	portail2 = dynamic_cast<NoeudPortail*> (arbre->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL));

	portail1->assignerFrere(portail2);
	portail2->assignerFrere(portail1);


	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(portail1);
	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(portail2);

	arbre->deselectionnerTout();
	portail1->inverserSelection();
	portail2->inverserSelection();

	arbre->effacerSelection();

	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO((int)arbre->chercher(ArbreRenduINF2990::NOM_PORTAIL)));

}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
