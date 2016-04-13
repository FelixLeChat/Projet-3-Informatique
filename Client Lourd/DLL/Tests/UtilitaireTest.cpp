////////////////////////////////////////////////////////////////////////////////////
/// @file UtilitaireTest.cpp
/// @author Konstantin Fedorov
/// @date 2015-02-15
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "UtilitaireTest.h"
#include "Utilitaire.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(UtilitaireTest);


////////////////////////////////////////////////////////////////////////
///
/// @fn void UtilitaireTest::setUp()
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
void UtilitaireTest::setUp()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void UtilitaireTest::tearDown()
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
void UtilitaireTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void UtilitaireTest::testAjustementRapport()
///
/// Cas de test: Ajustement rapport d'aspect
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void UtilitaireTest::testAppliqueMatrice()
{
	glm::dvec3 vecteur(6, 8, 20);
	double mat[16]{
		 2*cos(utilitaire::DEG_TO_RAD(90)), 2*sin(utilitaire::DEG_TO_RAD(90)), 0, 0,
			3*-sin(utilitaire::DEG_TO_RAD(90)),  3*cos(utilitaire::DEG_TO_RAD(90)), 0, 0,
		0,0,1.5,0,
		0,0,0,1};
	vecteur = utilitaire::appliquerMatrice(vecteur, mat);
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur.x + 24));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur.y - 12));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur.z - 30));
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
