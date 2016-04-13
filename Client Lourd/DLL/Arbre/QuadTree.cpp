///////////////////////////////////////////////////////////////////////////
/// @file QuadTree.cpp
/// @author Konstantin Fedorov et Jeremie Gagne
/// @date 2015-02-24
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#include "QuadTree.h"


const int QuadTree::NBMAXOBJETS = 20;

////////////////////////////////////////////////////////////////////////
///
/// @fn QuadTree::QuadTree()
///
/// Ne fait qu'initialiser les variables membres de la classe.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
QuadTree::QuadTree()
{
	aEnfants = false;
	nbObjets = 0;
	parent = nullptr;
}


QuadTree::QuadTree(QuadTree *  p)
{
	aEnfants = false;
	nbObjets = 0;
	parent = p;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn QuadTree::QuadTree()
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
QuadTree::~QuadTree()
{
	if (aEnfants)
	{
		delete[] enfants;
		enfants = 0;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void QuadTree::setLimites(utilitaire::BoiteEnglobante bornes)
///
///	Assigne les nouvelles limites de la zone
///
/// @param[in] bornes : nouvelles limites de la zone
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void QuadTree::setLimites(utilitaire::BoiteEnglobante bornes)
{
	limites = bornes;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool QuadTree::ajouter(NoeudAbstrait* noeud)
///
///	Ajoute un noeud
///
/// @param[in] noeud : noeud a ajouter
/// @return true si l'ajout s'est bien exécuté.
///
////////////////////////////////////////////////////////////////////////
bool QuadTree::ajouter(NoeudAbstrait* noeud)
{
	if (noeud->obtenirType() == "portail" || noeud->obtenirType() == "zonedejeu")
	{
		listeObjets.insert(std::pair<std::string, NoeudAbstrait*>(noeud->obtenirType(),noeud));
		++nbObjets;
		return true;
	}
	else if (estDansBornes(noeud))
	{
		if (!aEnfants)
		{
			listeObjets.insert(std::pair<std::string, NoeudAbstrait*>(noeud->obtenirType(), noeud));
			if (++nbObjets > NBMAXOBJETS)
				diviser();
		}
		else
		{
			bool ajoute = false;
			for (int i = 0; i < 4 && !ajoute; i++)
				ajoute = enfants[i].ajouter(noeud);
			if (!ajoute)
			{
				listeObjets.insert(std::pair<std::string, NoeudAbstrait*>(noeud->obtenirType(), noeud));
				nbObjets++;
			}
		}
		return true;
	}
	else if (estSurBornes(noeud) && parent == nullptr)
	{
		listeObjets.insert(std::pair<std::string, NoeudAbstrait*>(noeud->obtenirType(), noeud));
		++nbObjets;
		return true;
	}
	else
		return false;
	

}
////////////////////////////////////////////////////////////////////////
///
/// @fn void QuadTree::ajouter(NoeudComposite* noeud)
///
///	Ajoute les enfants d'un noeud composite
///
/// @param[in] noeud : noeud contenant les enfants a ajouter.
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void QuadTree::ajouter(NoeudComposite* noeud)
{
	for (unsigned int i = 0; i < noeud->obtenirNombreEnfants(); i++)
	{
		NoeudAbstrait* n = noeud->chercher(i);
		if (n->obtenirNombreEnfants() != 0)
		{
			ajouter(static_cast<NoeudComposite*>(n));
		}
		else
		{
			ajouter(n);
		}
	}
		
	ajouter((NoeudAbstrait*)noeud);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void QuadTree::retirerBilles()
///
///	Retire toutes les billes du quadtree
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void QuadTree::retirerBilles()
{
	
	nbObjets -= static_cast<int>(listeObjets.erase("bille"));

	if (aEnfants)
	{
		int sommeObjets = nbObjets;
		for (int i = 0; i < 4; i++){
			enfants[i].retirerBilles();
			sommeObjets += enfants[i].obtenirNbObjets();
		}
		

		if (sommeObjets < NBMAXOBJETS)
			fusionnerEnfants();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn int QuadTree::obtenirNbObjets()
///
/// Donne le nombre d'objets dans la zone n'incluant pas les enfants
///
/// @return Le nombre d'objets contenu dans cette zone
///
////////////////////////////////////////////////////////////////////////
int QuadTree::obtenirNbObjets()
{
	return nbObjets;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void QuadTree::fusionnerEnfants()
///
/// Fusionne les zones enfants dans la zone courante.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void QuadTree::fusionnerEnfants()
{
	aEnfants = false;

	for (int i = 0; i < 4; i++){
		listeObjets.insert(enfants[i].listeObjets.begin(), enfants[i].listeObjets.end());

		nbObjets += enfants[i].nbObjets;
		enfants[i].listeObjets.clear();
	}
	delete[] enfants;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void QuadTree::diviser()
///
/// Divise la zone en sous-zone enfants de la zone courante.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void QuadTree::diviser()
{
	aEnfants = true;
	enfants = new QuadTree[4];
	enfants[0].parent = this;
	enfants[1].parent = this;
	enfants[2].parent = this;
	enfants[3].parent = this;
	glm::dvec3 pointMilieu((limites.coinMax.x - limites.coinMin.x) / 2 + limites.coinMin.x, (limites.coinMax.y - limites.coinMin.y) / 2 + limites.coinMin.y, limites.coinMin.z);
	enfants[0].setLimites(utilitaire::BoiteEnglobante{ pointMilieu, limites.coinMax });
	enfants[1].setLimites(utilitaire::BoiteEnglobante{ glm::dvec3(limites.coinMin.x, pointMilieu.y, pointMilieu.z), glm::dvec3(pointMilieu.x, limites.coinMax.y, pointMilieu.z) });
	enfants[2].setLimites(utilitaire::BoiteEnglobante{ limites.coinMin, pointMilieu});
	enfants[3].setLimites(utilitaire::BoiteEnglobante{ glm::dvec3(pointMilieu.x, limites.coinMin.y, pointMilieu.z), glm::dvec3(limites.coinMax.x, pointMilieu.y, pointMilieu.z) });
	
	std::multimap<std::string, NoeudAbstrait* >::iterator it;
	int max = nbObjets;
	for (int i = 0; i < max; i++)
	{
		it = listeObjets.begin();
		NoeudAbstrait * noeudCourant = it->second;
		listeObjets.erase(it);
		nbObjets--;
		ajouter(noeudCourant);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void QuadTree::executerCollision(NoeudBille* bille, double dt)
///
///	Execute les collisions dans la zone et les zones enfants qui contiennent la bille
///
/// @param[in] bille : bille effectuant les collisions
/// @param[in] dt : intervalle de temps
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////

pair<double, NoeudAbstrait*> QuadTree::obtenirEnfoncement(NoeudBille* bille)
{
	auto enfoncementMax = pair<double, NoeudAbstrait*>(0, nullptr);
	if (bille!=nullptr &&estSurBornes(bille))
	{
		if (aEnfants)
		{
			for (int i = 0; i < 4; i++)
			{
				auto enfoncement = enfants[i].obtenirEnfoncement(bille);
				if (enfoncementMax.first < enfoncement.first)
					enfoncementMax = enfoncement;
			}
		}
		std::multimap<std::string, NoeudAbstrait* >::iterator it = listeObjets.begin();
		while (bille != nullptr && it != listeObjets.end())
		{
			auto enfoncement = it->second->obtenirEnfoncement(bille);
			if (enfoncementMax.first < enfoncement)
				enfoncementMax = pair<double, NoeudAbstrait*>(enfoncement, it->second);
			it++;
		}
	}
	return enfoncementMax;
}

void QuadTree::appliquerForcesConstantes(NoeudBille* bille)
{
	if (bille != nullptr && bille->estLocale() && estSurBornes(bille))
	{
		if (aEnfants)
		{
			for (int i = 0; i < 4; i++)
			{
				enfants[i].appliquerForcesConstantes(bille);
			}
		}
		std::multimap<std::string, NoeudAbstrait* >::iterator it = listeObjets.begin();
		while (bille != nullptr && it != listeObjets.end())
		{
			it->second->appliquerForceConstante(bille);
			it++;
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool QuadTree::estDansBornes(NoeudAbstrait * noeud)
///
///	Vérifie si le noeud abstrait est contenu dans la zone à tester
///
/// @param[in] noeud : noeud à tester
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
bool QuadTree::estDansBornes(NoeudAbstrait * noeud)
{
	utilitaire::BoiteEnglobante test = noeud->obtenirBornes();
	if (test.coinMin.x < limites.coinMin.x || test.coinMin.y < limites.coinMin.y || test.coinMax.x > limites.coinMax.x || test.coinMax.y > limites.coinMax.y)
		return false;
	return true;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool QuadTree::estSurBornes(NoeudAbstrait * noeud)
///
///	Vérifie si le noeud abstrait est contenu sur les limites de la zone
///
/// @param[in] noeud : noeud à tester
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
bool QuadTree::estSurBornes(NoeudAbstrait * noeud)
{
	utilitaire::BoiteEnglobante test = noeud->obtenirBornes();
	if (test.coinMin.x > limites.coinMax.x || test.coinMin.y > limites.coinMax.y || test.coinMax.x < limites.coinMin.x || test.coinMax.y < limites.coinMin.y)
		return false;
	return true;
}
