////////////////////////////////////////////////////////////////////////////////////
/// @file ProjectionOrthoTest.cpp
/// @author Konstantin Fedorov
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "ProjectionOrthoTest.h"
#include "Vue/ProjectionOrtho.h"
#include "Utilitaire.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(ProjectionOrthoTest);


////////////////////////////////////////////////////////////////////////
///
/// @fn void ProjectionOrthoTest::setUp()
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
void ProjectionOrthoTest::setUp()
{
	projection = std::make_unique<vue::ProjectionOrtho>(0,400,0,800,0,200,10,2000,1.1,-10,10,-10,10);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ProjectionOrthoTest::tearDown()
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
void ProjectionOrthoTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ProjectionOrthoTest::testAjustementRapport()
///
/// Cas de test: Ajustement rapport d'aspect
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ProjectionOrthoTest::testAjustementRapport()
{
	double xMin, xMax, yMin, yMax;
	projection->obtenirCoordonneesFenetreVirtuelle(xMin, xMax, yMin, yMax);
	CPPUNIT_ASSERT(xMin == -10);
	CPPUNIT_ASSERT(xMax ==  10);
	CPPUNIT_ASSERT(yMin == -20);
	CPPUNIT_ASSERT(yMax ==  20);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ProjectionOrthoTest::testRedimensionnement()
///
/// Cas de test: Redimensionnement de la fenetre
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ProjectionOrthoTest::testRedimensionnement()
{
	projection->redimensionnerFenetre(glm::ivec2(0, 0), glm::ivec2(600, 1000));
	double xMin, xMax, yMin, yMax;
	projection->obtenirCoordonneesFenetreVirtuelle(xMin, xMax, yMin, yMax);
	CPPUNIT_ASSERT(xMin == -15);
	CPPUNIT_ASSERT(xMax == 15);
	CPPUNIT_ASSERT(yMin == -25);
	CPPUNIT_ASSERT(yMax == 25);
	int xMin2, xMax2, yMin2, yMax2;
	projection->obtenirCoordonneesCloture(xMin2, xMax2, yMin2, yMax2);
	CPPUNIT_ASSERT(xMin2 == 0);
	CPPUNIT_ASSERT(xMax2 == 600);
	CPPUNIT_ASSERT(yMin2 == 0);
	CPPUNIT_ASSERT(yMax2 == 1000);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ProjectionOrthoTest::testZoomSimple()
///
/// Cas de test: Zoom In et zoom out simple
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ProjectionOrthoTest::testZoomSimple()
{
	
	projection->zoomerIn();
	double xMin, xMax, yMin, yMax;
	projection->obtenirCoordonneesFenetreVirtuelle(xMin, xMax, yMin, yMax);
	CPPUNIT_ASSERT(xMin == -9);
	CPPUNIT_ASSERT(xMax == 9);
	CPPUNIT_ASSERT(yMin == -18);
	CPPUNIT_ASSERT(yMax == 18);
	projection->zoomerOut();
	projection->obtenirCoordonneesFenetreVirtuelle(xMin, xMax, yMin, yMax);
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(xMin + 10));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(xMax - 10));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(yMin +20));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(yMax - 20));
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ProjectionOrthoTest::testZoomRect()
///
/// Cas de test: Zoomer a l'aide du rectangle elastique
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void ProjectionOrthoTest::testZoomRect()
{
	projection->zoomerIn(glm::ivec2(0, 800), glm::ivec2(200, 600));
	double xMin, xMax, yMin, yMax;
	projection->obtenirCoordonneesFenetreVirtuelle(xMin, xMax, yMin, yMax);
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(xMin + 10));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(xMax));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(yMin + 25));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(yMax + 5));
	projection->zoomerOut(glm::ivec2(0, 800), glm::ivec2(200, 400));
	projection->obtenirCoordonneesFenetreVirtuelle(xMin, xMax, yMin, yMax);
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(xMin + 10));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(xMax - 10));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(yMin + 25));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(yMax - 15));
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
