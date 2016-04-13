//////////////////////////////////////////////////////////////////////////////
/// @file NoeudCompositeTest.h
/// @author Konstantin Fedorov
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_NoeudCompositeTEST_H
#define _TESTS_NoeudCompositeTEST_H

#include <cppunit/extensions/HelperMacros.h>
#include <memory>
#include "Arbre/Noeuds/NoeudComposite.h"
#include "Arbre/ArbreRenduINF2990.h"

///////////////////////////////////////////////////////////////////////////
/// @class NoeudCompositeTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        méthodes de la classe NoeudComposite
///
/// @author Konstantin Fedorov
/// @date 2015-02-15
///////////////////////////////////////////////////////////////////////////
class NoeudCompositeTest : public CppUnit::TestFixture
{

	// =================================================================
	// Déclaration de la suite de tests et des méthodes de tests
	//
	// Important, vous devez définir chacun de vos cas de tests à l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas exécuté !
	// =================================================================
	CPPUNIT_TEST_SUITE(NoeudCompositeTest);
	CPPUNIT_TEST(testChercher);
	CPPUNIT_TEST(testEffacer);
	CPPUNIT_TEST(testVider);
	CPPUNIT_TEST(testEffacerSelection);
	CPPUNIT_TEST_SUITE_END();

public:

	// =================================================================
	// Méthodes pour initialiser et 'finaliser' la suite de tests
	// =================================================================

	/// Traitement à effectuer pour initialiser cette suite de tests
	void setUp();

	/// Traitement à effectuer pour 'finaliser' cette suite de tests
	void tearDown();


	// =================================================================
	// Définissez ici les différents cas de tests...
	// =================================================================

	void testChercher();
	void testEffacer();
	void testVider();
	void testEffacerSelection();

private:
	/// Instance d'un noeud abstrait
	std::unique_ptr<ArbreRenduINF2990> arbre_;
};

#endif // _TESTS_NoeudCompositeTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
