////////////////////////////////////////////////////////////////////////////////////
/// @file BancTests.cpp
/// @author Julien Gascon-Samson
/// @date 2011-07-16
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "BancTests.h"

// Inclusions cppunit pour l'exécution des tests
#include <cppunit/CompilerOutputter.h>
#include <cppunit/extensions/TestFactoryRegistry.h>
#include <cppunit/ui/text/TestRunner.h>

SINGLETON_DECLARATION_CPP(BancTests);

////////////////////////////////////////////////////////////////////////
///
/// @fn bool BancTests::executer()
///
/// Cette fonction exécute l'ensemble des tests unitaires définis.
/// La sortie de l'exécution des tests se fait dans la console standard
/// d'erreurs 'cerr'. Cette fonction ajuste également le format de
/// sortie pour correspondre à celui de Visual Studio afin d'intégrer
/// l'exécution des tests au processus de compilation ("Post Build Event").
///
/// @return true si l'exécution de tous les tests a réussi, sinon false.
///
////////////////////////////////////////////////////////////////////////
bool BancTests::executer()
{
	// Obtenir la suite de tests à haut niveau
	CppUnit::Test *suite{ CppUnit::TestFactoryRegistry::getRegistry().makeTest() };

	// Obtient un environnement d'exécution de tests qui imprime les résultats
	// dans une console (cout, cerr, fichier, etc.) et ajoute la suite de tests
	// de base à l'environnement.
	// (Notez qu'il est aussi possible d'obtenir un environnement qui affiche
	// les résultats des tests au sein d'une interface graphique QT ou MFC.
	// Consultez la documentation cppunit pour plus d'informations)
	CppUnit::TextUi::TestRunner runner;
	runner.addTest(suite);

	// Indique que nous souhaitons formatter la sortie selon un format qui
	// s'apparente à la sortie d'un compilateur (MSVC++), et que nous
	// souhaitons que la sortie soit réalisée dans le canal standard cerr.
	// Cela permettra à Visual Studio d'interpréter la sortie de cppunit,
	// d'indiquer les erreurs trouvées par les tests et leur numéro de ligne
	// en tant qu'erreurs survenant au niveau du processus de compilation.
	runner.setOutputter(new CppUnit::CompilerOutputter{ &runner.result(),
		std::cerr });
	// Exécuter les tests
	return runner.run();
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
