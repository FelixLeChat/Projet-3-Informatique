////////////////////////////////////////////////////////////////////////////////////
/// @file NoeudAbstraitTest.cpp
/// @author Julien Gascon-Samson
/// @date 2011-07-16
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "NoeudAbstraitTest.h"
#include "Arbre/Noeuds/NoeudButoirCirculaire.h"
#include "Arbre/ArbreRenduINF2990.h"
#include "Utilitaire.h"

// Enregistrement de la suite de tests au sein du registre
//CPPUNIT_TEST_SUITE_REGISTRATION(NoeudAbstraitTest);

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstraitTest::setUp()
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
void NoeudAbstraitTest::setUp()
{
	noeud = std::make_unique<NoeudButoirCirculaire>( ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE );
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstraitTest::tearDown()
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
void NoeudAbstraitTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstraitTest::testPositionRelative()
///
/// Cas de test: écriture/lecture de la position relative
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstraitTest::testPositionRelative()
{
	// Premier test : on vérifie que la position initiale est un vector nul
	glm::dvec3 vecteur{ noeud->obtenirPositionRelative() };
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur[0]));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur[1]));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur[2]));

	// On modifie la position
	noeud->assignerPositionRelative(glm::dvec3{ 2.2, 3.3, 4.4 });

	// Second test : on vérifie que la position a été modifiée
	vecteur = noeud->obtenirPositionRelative();
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur[0] - 2.2));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur[1] - 3.3));
	CPPUNIT_ASSERT(utilitaire::EGAL_ZERO(vecteur[2] - 4.4));
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstraitTest::testType()
///
/// Cas de test: type de noeud
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstraitTest::testType()
{
	CPPUNIT_ASSERT(noeud->obtenirType() == ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
	CPPUNIT_ASSERT(noeud->obtenirType() != ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREDROIT);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstraitTest::testSelection()
///
/// Cas de test: définition/obtention des états de sélection du noeud
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstraitTest::testSelection()
{
	// Cas 1 : non sélectionnable, non sélectionné
	noeud->assignerEstSelectionnable(false);
	noeud->assignerSelection(false);
	CPPUNIT_ASSERT(noeud->estSelectionnable() == false);
	CPPUNIT_ASSERT(noeud->estSelectionne() == false);

	// Cas 2 : non sélectionnable, sélectionné
	noeud->assignerEstSelectionnable(false);
	noeud->assignerSelection(true);
	CPPUNIT_ASSERT(noeud->estSelectionnable() == false);
	CPPUNIT_ASSERT(noeud->estSelectionne() == false);

	// Cas 3 : sélectionnable, non sélectionné
	noeud->assignerEstSelectionnable(true);
	noeud->assignerSelection(false);
	CPPUNIT_ASSERT(noeud->estSelectionnable() == true);
	CPPUNIT_ASSERT(noeud->estSelectionne() == false);

	// Cas 4 : non sélectionnable, sélectionné
	noeud->assignerEstSelectionnable(true);
	noeud->assignerSelection(true);
	CPPUNIT_ASSERT(noeud->estSelectionnable() == true);
	CPPUNIT_ASSERT(noeud->estSelectionne() == true);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstraitTest::testEnfants()
///
/// Cas de test: s'assurer que le noeud abstrait n'a pas d'enfant
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstraitTest::testEnfants()
{
	// On vérifie qu'initialement, le noeud n'a pas d'enfant
	CPPUNIT_ASSERT(noeud->obtenirNombreEnfants() == 0);
	CPPUNIT_ASSERT(noeud->calculerProfondeur() == 1);

	// Essaie d'ajouter un noeud
	NoeudAbstrait* nouveauNoeud{ new NoeudButoirCirculaire{ ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE} };
	bool ajout{ noeud->ajouter(nouveauNoeud) };

	// L'ajout devrait avoir échoué puisqu'il s'agit d'un noeud abstrait...
	CPPUNIT_ASSERT(ajout == false);

	// Assurons-nous que le noeud ne possède pas d'enfant...
	CPPUNIT_ASSERT(noeud->obtenirNombreEnfants() == 0);
	CPPUNIT_ASSERT(noeud->calculerProfondeur() == 1);

	// Nettoyage
	delete nouveauNoeud;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
