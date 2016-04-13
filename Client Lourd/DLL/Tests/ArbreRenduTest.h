//////////////////////////////////////////////////////////////////////////////
/// @file ArbreRenduTest.h
/// @author Konstantin Fedorov
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_ArbreRenduTEST_H
#define _TESTS_ArbreRenduTEST_H

#include <cppunit/extensions/HelperMacros.h>
#include <memory>
#include "Arbre/ArbreRenduINF2990.h"

///////////////////////////////////////////////////////////////////////////
/// @class ArbreRenduTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        méthodes de la classe ArbreRendu
///
/// @author Konstantin Fedorov
/// @date 2015-02-15
///////////////////////////////////////////////////////////////////////////
class ArbreRenduTest : public CppUnit::TestFixture
{

	// =================================================================
	// Déclaration de la suite de tests et des méthodes de tests
	//
	// Important, vous devez définir chacun de vos cas de tests à l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas exécuté !
	// =================================================================
	CPPUNIT_TEST_SUITE(ArbreRenduTest);
	CPPUNIT_TEST(testCreerNoeud);
	CPPUNIT_TEST(testEstDansBornes);
	CPPUNIT_TEST(testObtenirDepassement);
	CPPUNIT_TEST(testCentreSelection);
	CPPUNIT_TEST(testEssentielSelectionne);
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

	void testCreerNoeud();
	void testEstDansBornes();
	void testObtenirDepassement();
	void testCentreSelection();
	void testEssentielSelectionne();
	

private:
	/// Instance d'un noeud abstrait
	std::unique_ptr<ArbreRenduINF2990> arbre_;
};

#endif // _TESTS_ArbreRenduTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
