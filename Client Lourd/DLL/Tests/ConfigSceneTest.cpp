////////////////////////////////////////////////////////////////////////////////////
/// @file ConfigSceneTest.cpp
/// @author Julien Gascon-Samson
/// @date 2011-07-16
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "ConfigSceneTest.h"
#include "Configuration/ConfigScene.h"
#include "Application/FacadeModele.h"

// Enregistrement de la suite de tests au sein du registre
//CPPUNIT_TEST_SUITE_REGISTRATION(ConfigSceneTest);

////////////////////////////////////////////////////////////////////////
///
/// @fn void ConfigSceneTest::setUp()
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
void ConfigSceneTest::setUp()
{
	// Nous pourrions initialiser l'objet, mais puisqu'il s'agit d'un singleton,
	// aucune initialisation n'est requise.
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ConfigSceneTest::tearDown()
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
void ConfigSceneTest::tearDown()
{
	// Nous pourrions libérer l'objet, mais puisqu'il s'agit d'un singleton,
	// aucune libération n'est requise.
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ConfigSceneTest::testSauvegardeChargement()
///
/// Cas de test: sauvegarde et chargement XML de la configuration
/// Modifier la valeur CALCULS_PAR_IMAGE, enregistrer la configuration,
/// restaurer la valeur CALCULS_PAR_IMAGE, charger la configuration,
/// s'assurer que la valeur sauvegardée a bien été restaurée du fichier
/// XML, restaurer la valeur par défaut.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ConfigSceneTest::testSauvegardeChargement()
{
	// On définit une autre valeur pour les attributs
	ConfigScene::CALCULS_PAR_IMAGE = 20;

	// On sauvegarde le XML...
	FacadeModele::obtenirInstance()->enregistrerConfiguration();

	// On remet les valeurs par défaut
	ConfigScene::CALCULS_PAR_IMAGE = 50;

	// On charge le XML...
	FacadeModele::obtenirInstance()->chargerConfiguration();

	// On vérifie si les valeurs de test sont celles qui ont bien été chargées
	CPPUNIT_ASSERT(ConfigScene::CALCULS_PAR_IMAGE == 20);

	// On réaffecte les valeurs par défaut
	ConfigScene::CALCULS_PAR_IMAGE = 50;

	// On resauvegarde le XML
	FacadeModele::obtenirInstance()->enregistrerConfiguration();
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
