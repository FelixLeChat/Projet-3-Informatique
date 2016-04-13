///////////////////////////////////////////////////////////////////////////////
/// @file QuadTree.h
/// @author Konstantin Fedorov et Jeremie Gagne
/// @date 2015-02-24
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_QUADTREE_H__
#define __ARBRE_QUADTREE_H__

#include "Arbre/Noeuds/NoeudAbstrait.h"
#include "Arbre/Noeuds/NoeudComposite.h"
#include "Arbre/Noeuds/NoeudBille.h"
#include <map>


///////////////////////////////////////////////////////////////////////////
/// @class QuadTree
/// @brief Aide a la gestion des collisions
///
/// @author Konstantin Fedorov et Jeremie Gagne
/// @date 2015-02-24
///////////////////////////////////////////////////////////////////////////
class QuadTree
{
public:
	QuadTree();
	QuadTree(QuadTree* p);
	~QuadTree();

	/// Ajoute un noeud au QuadTree
	bool ajouter(NoeudAbstrait* noeud);
	/// Ajoute un noeud composite au QuadTree
	void ajouter(NoeudComposite* noeud);
	/// Retire les billes du QuadTree
	void retirerBilles();
	/// Donne le nombre d'objets dans la zone n'incluant pas les enfants
	int obtenirNbObjets();
	/// Execute les collisions dans la zone et les zones enfants qui contiennent la bille
	pair<double, NoeudAbstrait*> obtenirEnfoncement(NoeudBille* bille);
	/// Assigne les limites de la zone
	void setLimites(utilitaire::BoiteEnglobante bornes);
	/// Indique si un noeud est dans les bornes de la zone
	bool estDansBornes(NoeudAbstrait* noeud);
	/// Indique si un noeud est sur les bornes de la zone
	bool estSurBornes(NoeudAbstrait* noeud);

	void appliquerForcesConstantes(NoeudBille* bille);

private:
	/// Le parent de la zone. nullptr s'il n'y en a pas
	QuadTree* parent;
	/// Indique si la zone a des enfants
	bool aEnfants;
	/// Le nombre d'objets dans la zone n'incluant pas les enfants
	int nbObjets;
	/// La liste des objets de la zone n'incluant pas les enfants. La clef est le type du noeud
	std::multimap<std::string, NoeudAbstrait*> listeObjets;
	/// Les enfants de la zone
	QuadTree* enfants;
	/// Les limites de la zone
	utilitaire::BoiteEnglobante limites;
	/// Divise la zone en sous - zone enfants de la zone courante.
	void diviser();
	/// Fusionne les zones enfants dans la zone courante.
	void fusionnerEnfants();
	/// Indique le nombre maximal d'objet dans une zone sans enfant avant de devoir la subdiviser
	static const int NBMAXOBJETS;
};

#endif //__ARBRE_QUADTREE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

