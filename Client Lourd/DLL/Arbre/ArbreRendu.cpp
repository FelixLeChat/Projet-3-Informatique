////////////////////////////////////////////////
/// @file   ArbreRendu.cpp
/// @author Martin Bisson
/// @date   2007-01-28
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////

#include "ArbreRendu.h"
#include "Usines/UsineNoeud.h"
#include "Noeuds/NoeudAbstrait.h"
#include "ArbreRenduINF2990.h"


////////////////////////////////////////////////////////////////////////
///
/// @fn ArbreRendu::ArbreRendu()
///
/// Ne fait qu'assigner que ce noeud n'est pas sélectionnable.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
ArbreRendu::ArbreRendu()
: NoeudComposite{ "racine" }
{
	// On ne veut pas que ce noeud soit sélectionnable.
	assignerEstSelectionnable(false);
	nbObjSel = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ArbreRendu::~ArbreRendu()
///
/// Détruit les usines des noeuds de l'arbre.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
ArbreRendu::~ArbreRendu()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* ArbreRendu::creerNoeud(const std::string& typeNouveauNoeud) const
///
/// Cette fonction permet de créer un nouveau noeud, sans l'ajouter
/// directement à l'arbre de rendu.
///
/// @param[in] typeNouveauNoeud : Le type du nouveau noeud.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* ArbreRendu::creerNoeud(
	const std::string& typeNouveauNoeud
	) const
{
	if (usines_.find(typeNouveauNoeud) == usines_.end()) {
		// Incapable de trouver l'usine
		return nullptr;
	}

	// Pour une raison que je ne comprends pas, la ligne suivante ne
	// compile pas:
	//
	// const UsineNoeud* usine = usines_[typeNouveauNoeud];
	//
	// On utilisera donc:
	const UsineNoeud* usine{ (*(usines_.find(typeNouveauNoeud))).second };

	return usine->creerNoeud();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* ArbreRendu::ajouterNouveauNoeud(const std::string& typeParent, const std::string& typeNouveauNoeud)
///
/// Cette fonction permet d'ajouter un nouveau noeud dans l'arbre de
/// rendu.
///
/// @param[in] typeParent       : Le type du parent du nouveau noeud.
/// @param[in] typeNouveauNoeud : Le type du nouveau noeud.
///
/// @return Le noeud nouvellement créé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* ArbreRendu::ajouterNouveauNoeud(
	const std::string& typeParent,
	const std::string& typeNouveauNoeud
	)
{
	NoeudAbstrait* parent{ chercher(typeParent) };
	if (parent == nullptr) {
		// Incapable de trouver le parent
		return nullptr;
	}

	NoeudAbstrait* nouveauNoeud{ creerNoeud(typeNouveauNoeud) };
	if (nouveauNoeud)
		parent->ajouter(nouveauNoeud);

	return nouveauNoeud;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn int ArbreRendu::chercherSelection(int x, int y, bool adding, bool clic)
///
/// Cette fonction détecte et compte le nombre d'objets sélectionnés.
///
/// @param[in] x : La position en x.
/// @param[in] y : La position en y.
/// @param[in] adding : Si on ajoute l'objet à la sélection ou pas.
/// @param[in] clic : Si c'est un clic.
///
/// @return Le nombre d'objets sélectionnés
///
////////////////////////////////////////////////////////////////////////
int ArbreRendu::chercherSelection(int x, int y, bool adding, bool clic)
{
	const GLsizei taille = 2000;
	GLuint tampon[taille];
	glSelectBuffer(taille, tampon);

	glRenderMode(GL_SELECT);
	NoeudComposite::afficherConcret();
	int nbObjets = glRenderMode(GL_RENDER);

	GLuint* ptr = tampon;
	
	if (!adding)
	{
		deselectionnerTout();
		nbObjSel = 0;
	}
		
	for (int j = 0; j < nbObjets && (j<1 || !clic); j++)
	{
		GLuint nbnoms = *ptr;
		++ptr;
		++ptr;
		++ptr;
		NoeudComposite * parent = this;
		for (unsigned int i = 0; i < nbnoms - 1; i++){
			parent = dynamic_cast<NoeudComposite *>(parent->chercher(*ptr++));
		}
		NoeudAbstrait* noeudSel = parent->chercher(*ptr);
		//YOLO
		if (clic || noeudSel->obtenirType() != "FLECHE" )
		{
			if (noeudSel->inverserSelection())
			{
				nbObjSel++;
			}
			else if (noeudSel->estSelectionnable())
			{
				nbObjSel--;
			}
		}
		++ptr;
	}
	return nbObjSel;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ArbreRendu::estDansBornes()
///
/// Cette fonction vérifie que la sélection se trouve à l'intérieur des bornes
/// de la table de jeu.
///
/// @return si la sélection se trouve à l'intérieur des bornes de la table
///         de jeu.
///
////////////////////////////////////////////////////////////////////////
bool ArbreRendu::estDansBornes(bool selected)
{
	NoeudComposite* zoneJeu = dynamic_cast<NoeudComposite *>(chercher("zonedejeu"));
	utilitaire::BoiteEnglobante table = zoneJeu->obtenirBornes();
	bool interieur = true;
	int nbObjs = zoneJeu->obtenirNombreEnfants();
	for (int i = 0; i < nbObjs && interieur; i++)
	{
		auto noeud = zoneJeu->chercher(i);
		if (!selected || noeud->estSelectionne())
		{
			utilitaire::BoiteEnglobante test = noeud->obtenirBornes();
			if (test.coinMin.x < table.coinMin.x || test.coinMin.y < table.coinMin.y || test.coinMax.x > table.coinMax.x || test.coinMax.y > table.coinMax.y)
				interieur = false;
		}

	}
	return interieur;


}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ArbreRendu::estDansBornes(NoeudAbstrait * noeud)
///
/// Cette fonction vérifie que la sélection se trouve à l'intérieur des bornes
/// de la table de jeu.
///
/// @param[in] noeud : le noeud dont on vérifie la position
///
/// @return si la sélection se trouve à l'intérieur des bornes de la table
///         de jeu.
///
////////////////////////////////////////////////////////////////////////
bool ArbreRendu::estDansBornes(NoeudAbstrait * noeud)
{
	NoeudComposite* zoneJeu = dynamic_cast<NoeudComposite *>(chercher("zonedejeu"));
	utilitaire::BoiteEnglobante table = zoneJeu->obtenirBornes();
	int nbObjs = zoneJeu->obtenirNombreEnfants();
	for (int i = 0; i < nbObjs ; i++)
	{
		NoeudAbstrait * noeudTest = zoneJeu->chercher(i);
		if (noeudTest == noeud)
		{
			utilitaire::BoiteEnglobante test = noeudTest->obtenirBornes();
			if (test.coinMin.x < table.coinMin.x || test.coinMin.y < table.coinMin.y || test.coinMax.x > table.coinMax.x || test.coinMax.y > table.coinMax.y)
				return false;
			else
				return true;
		}
	}
	return true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::dvec2 ArbreRendu::obtenirDepassement()
///
/// Cette fonction obtient le dépassement d'un objet par rapport 
/// à la table de jeu.
///
/// @return le dépassement de la table de jeu
///
////////////////////////////////////////////////////////////////////////
glm::dvec2 ArbreRendu::obtenirDepassement()
{
	NoeudComposite* zoneJeu = dynamic_cast<NoeudComposite *>(chercher("zonedejeu"));
	utilitaire::BoiteEnglobante table = zoneJeu->obtenirBornes();
	glm::dvec2 depassement(0,0);
	bool interieur = true;
	int nbObjs = zoneJeu->obtenirNombreEnfants();
	for (int i = 0; i < nbObjs && interieur; i++)
	{
		utilitaire::BoiteEnglobante test = zoneJeu->chercher(i)->obtenirBornes();
		if (abs(depassement.x) < (table.coinMin.x - test.coinMin.x))
			depassement.x = test.coinMin.x - table.coinMin.x;
		else if (abs(depassement.x) < test.coinMax.x - table.coinMax.x)
			depassement.x = test.coinMax.x - table.coinMax.x;

		if (abs(depassement.y) < (table.coinMin.y - test.coinMin.y))
			depassement.y = test.coinMin.y - table.coinMin.y;
		else if (abs(depassement.y) < test.coinMax.y - table.coinMax.y)
			depassement.y = test.coinMax.y - table.coinMax.y;
	}
	return depassement;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRendu::calculerCentreSelection()
///
/// Cette fonction calcule le centre de la zone de sélection
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ArbreRendu::calculerCentreSelection()
{
	if (selectionExiste())
	{
		double xmin, xmax, ymin, ymax;
		bool trouve = false;

		int i = chercher("zonedejeu")->obtenirNombreEnfants();
		for (int j = 0; j < i; j++)
		{
			NoeudAbstrait* noeud = chercher("zonedejeu")->chercher(j);
			if (noeud->estSelectionne())
			{
				noeud->calculerBornes();
				if (!trouve)
				{
					trouve = true;
					xmin = noeud->obtenirBornes().coinMin.x;
					xmax = noeud->obtenirBornes().coinMax.x;
					ymin = noeud->obtenirBornes().coinMin.y;
					ymax = noeud->obtenirBornes().coinMax.y;
				}

				if (noeud->obtenirBornes().coinMin.x < xmin)
					xmin = noeud->obtenirBornes().coinMin.x;
				if (noeud->obtenirBornes().coinMax.x > xmax)
					xmax = noeud->obtenirBornes().coinMax.x;
				if (noeud->obtenirBornes().coinMin.y < ymin)
					ymin = noeud->obtenirBornes().coinMin.y;
				if (noeud->obtenirBornes().coinMax.y > ymax)
					ymax = noeud->obtenirBornes().coinMax.y;
			}
		}
	centreSelection_ = glm::dvec3((xmin + xmax)/2, (ymin+ymax)/2, 0);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::dvec3& ArbreRendu::obtenirCentreSelection()
///
/// Cette fonction obtient le centre de la zone de sélection.
///
/// @return le centre de la zone de sélection.
///
////////////////////////////////////////////////////////////////////////
glm::dvec3& ArbreRendu::obtenirCentreSelection()
{
	return centreSelection_;

}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ArbreRendu::tousEssentielSelectionne()
///
/// Cette fonction vérifie que les éléments de bas du jeu sont sélectionnés.
///
/// @return si les éléments de bas du jeu sont sélectionnés.
///
////////////////////////////////////////////////////////////////////////
bool ArbreRendu::tousEssentielSelectionne()
{
	int nbGeneNonSel = 0;
	int nbRessNonSel = 0;
	int nbTrouNonSel = 0;

	NoeudComposite * noeud = dynamic_cast<NoeudComposite *>(chercher("zonedejeu"));
	int max = noeud->obtenirNombreEnfants();
	for (int i = 0; i < max && nbGeneNonSel*nbRessNonSel*nbTrouNonSel == 0; i++){
		NoeudAbstrait * enfant = noeud->chercher(i);
		if (!enfant->estSelectionne())
		{
			std::string nom = enfant->obtenirType();
			if (nom == "trou")
				nbTrouNonSel++;
			else if (nom == "ressort")
				nbRessNonSel++;
			else if (nom == "generateurbille")
				nbGeneNonSel++;
		}
	}
	return (nbGeneNonSel*nbRessNonSel*nbTrouNonSel == 0);

}

////////////////////////////////////////////////////////////////////////
///
/// @fn unsigned int ArbreRendu::calculerProfondeurMaximale()
///
/// Cette fonction retourne la profondeur maximale possible de l'arbre.
/// Comme lors du rendu, on effectue un glPushMatrix() pour sauvegarder
/// les transformations, ainsi qu'un glPushName() pour ajouter un nom
/// sur la pile des noms pour la sélection, la profondeur maximale de
/// l'arbre est limitée par la taille de la pile des matrices ainsi que
/// par celle de la pile des noms pour la sélection.
///
/// Cette fonction vérifie donc ces deux valeurs et retourne la plus
/// petite, c'est-à-dire celle qui limite la profondeur de l'arbre.
///
/// @return La profondeur maximale possible de l'arbre de rendu.
///
////////////////////////////////////////////////////////////////////////
unsigned int ArbreRendu::calculerProfondeurMaximale()
{
	GLint profondeurPileMatrice, profondeurPileNoms;

	glGetIntegerv(GL_MAX_MODELVIEW_STACK_DEPTH, &profondeurPileMatrice);
	glGetIntegerv(GL_MAX_NAME_STACK_DEPTH, &profondeurPileNoms);

	return (profondeurPileMatrice < profondeurPileNoms) ? profondeurPileMatrice : profondeurPileNoms;
}







////////////////////////////////////////////////
/// @}
////////////////////////////////////////////////
