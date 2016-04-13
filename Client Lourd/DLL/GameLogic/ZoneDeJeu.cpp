#include "ZoneDeJeu.h"

#include "Affichage/Affichage.h"
#include "Arbre/Visiteur/VisiteurReinit.h"
#include "Arbre/Visiteur/VisiteurPositionButoirs.h"
#include "Configuration/Config.h"
#include "Arbre/Noeuds/NoeudMur.h"
#include "Arbre/Noeuds/NoeudPortail.h"	
#include "Arbre/Noeuds/Palette/NoeudPalette.h"	
#include "Sons/ClasseSons.h"
#include <tinyxml2.h>
#include <algorithm>
#include "Arbre/Visiteur/VisiteurCiblesDetruites.h"
#include "Arbre/Visiteur/VisiteurPowerUpVisibles.h"

ZoneDeJeu::ZoneDeJeu(string path)
{
	pointButoirCercle_ = 10;
	pointButoirTriangle_ = 20;
	pointCible_ = 20;
	pointBilleGratuite_ = 1000;
	pointCampagne_ = 100;
	difficulte_ = 1;
	path_ = path;
}

ZoneDeJeu::~ZoneDeJeu()
{
	if (arbre_ != nullptr)
		delete arbre_;
}

void ZoneDeJeu::animer(double dt)
{
	double dtMin = dt/2;
	double tempsTotal = 0;
	double epsilon = 0.01;
	//trouver le dt a prendre
	for (int i = 0; i < billesEnJeu_.size(); i++)
	{
		billesEnJeu_[i]->maximiserVitesse();
		double dtDeBille = billesEnJeu_[i]->obtenirRayon() / glm::max(epsilon, glm::length(billesEnJeu_[i]->obtenirVitesse()));

		if (dtDeBille < dtMin)
			dtMin = dtDeBille;
	}

	while (tempsTotal < dt-0.0001)
	{
		conteurDeRecursion = 0;
		dtMin = glm::min(dtMin, dt - tempsTotal);
		tempsTotal += animerRecursif(dtMin);
	}

	auto it = billesEnJeu_.begin();
	//Ajout des forces constantes
	while (it != billesEnJeu_.end()) 
	{
		(*it)->finaliserAnimation(float(dt));
		(*it)->assignerVitesse((*it)->obtenirVitesse()*0.99);
		(*it)->appliquerForce(glm::dvec2(0, -7 * 9.81));
		quadTree_.appliquerForcesConstantes((*it));
		it++;
	}
}

double ZoneDeJeu::animerRecursif(double dt)
{
	// Mise à jour des objets.
	arbre_->animer(float(dt));
	//Calculer les collisions
	quadTree_.retirerBilles();

	//Remodelage du quadtree
	if (billesEnJeu_.size() != 0)
	{
		if (billesAEffacer_.size() != 0) 
		{
			while (billesAEffacer_.size() != 0) 
			{
				NoeudBille * bille = billesAEffacer_.back();
				billesAEffacer_.pop_back();
				vector<NoeudBille *>::iterator it = billesEnJeu_.begin();
				while (it != billesEnJeu_.end() && (*it) != bille) it++;
				if ((*it) == bille)
				{
					billesEnJeu_.erase(it);
				}
				arbre_->effacer(bille);
			}

			quadTree_ = QuadTree();
			quadTree_.setLimites(arbre_->chercher("zonedejeu")->obtenirBornes());
			quadTree_.ajouter(dynamic_cast<NoeudComposite *>(arbre_->chercher("zonedejeu")));
		}
	}

	vector<NoeudBille *>::iterator it = billesEnJeu_.begin();
	while (it != billesEnJeu_.end()) {
		quadTree_.ajouter(*it);
		it++;
	}

	//Obtenir l'enfoncement maximal
	it = billesEnJeu_.begin();
	double epsilon = 0.2;
	NoeudAbstrait* objetDeCollision = nullptr;
	double enfoncementMax = 0;
	NoeudBille* billeACollision = nullptr;
	while (it != billesEnJeu_.end()) 
	{
		auto resultatEnfoncement = quadTree_.obtenirEnfoncement((*it));
		if (resultatEnfoncement.second != nullptr && resultatEnfoncement.first > enfoncementMax)
		{
			enfoncementMax = resultatEnfoncement.first;
			objetDeCollision = resultatEnfoncement.second;
			billeACollision = (*it);
		}
		it++;
	}

	if (objetDeCollision != nullptr)
	{
		if (enfoncementMax < epsilon 
			|| objetDeCollision->obtenirType() == ArbreRenduINF2990::NOM_PORTAIL 
			|| conteurDeRecursion > 6)
		{
			objetDeCollision->executerCollision(billeACollision);
		}
		else
		{
			conteurDeRecursion++;
			arbre_->animer(float(-dt));
			return animerRecursif(dt / 2.0);
		}
	}
	return dt;
}

void ZoneDeJeu::perdreBille(NoeudBille* bille)
{
	billesAEffacer_.push_back(bille);
}


void ZoneDeJeu::perdreBille(int billeId)
{
	auto current = billesEnJeu_.begin();
	while(current != billesEnJeu_.end() && (*current)->obtenirId() != billeId)
	{
		++current;
	}
	if (current != billesEnJeu_.end() && !any_of(billesAEffacer_.begin(), billesAEffacer_.end(), [&](NoeudBille* bille) {return bille == (*current); }))
	{
		billesAEffacer_.push_back(*current);
	}
}



////////////////////////////////////////////////////////////////////////
///
/// @fn void ajouterBille(NoeudBille *bille)
///
/// Cette fonction est appelee par les parties pour ajouter des billes à l'animation
///
/// @param[in]
///
////////////////////////////////////////////////////////////////////////
NoeudBille* ZoneDeJeu::lancerBille(bool locale)
{
	vector<NoeudAbstrait*> genBille;
	NoeudAbstrait* zoneJeu = arbre_->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU);
	for (unsigned int i = 0; i < zoneJeu->obtenirNombreEnfants(); i++)
	{
		if (zoneJeu->chercher(i)->obtenirType() == ArbreRenduINF2990::NOM_GENERATEURBILLE)
		{
			genBille.push_back(zoneJeu->chercher(i));
		}
	}

	NoeudAbstrait * lanceur = genBille[rand() % genBille.size()];
	glm::dvec3 pos = lanceur->obtenirPositionRelative();
	double angle = lanceur->obtenirRotation() - 45;

	glm::dvec2 vitesse = glm::dvec2(40 * cos(utilitaire::DEG_TO_RAD(angle)), 40 * sin(utilitaire::DEG_TO_RAD(angle)));

	NoeudBille* bille = dynamic_cast < NoeudBille *>(arbre_->creerNoeud(ArbreRenduINF2990::NOM_BILLE));
	bille->assignerPositionRelative(pos);
	bille->assignerVitesse(vitesse);


	zoneJeu->ajouter(bille);

	billesEnJeu_.push_back(bille);

	if (Config::obtenirInstance()->getGenBille() && Config::obtenirInstance()->getDebog())
	{
		utilitaire::timeStamp();
		cout << " - Nouvelle bille: x: " << pos[0] << "  y: " << pos[1] << endl;
	}
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_NOUVELLE_BILLE, false);

	bille->assignerLocale(locale);

	return bille;

}

NoeudBille* ZoneDeJeu::lancerBille(int id, glm::dvec3 pos, glm::dvec2 vitesse, bool locale, int playerNum)
{

	NoeudAbstrait* zoneJeu = arbre_->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU);
	NoeudBille* bille = dynamic_cast < NoeudBille *>(arbre_->creerNoeud(ArbreRenduINF2990::NOM_BILLE));
	bille->assignerPositionRelative(pos);
	bille->assignerVitesse(vitesse);
	bille->assignerId(id);
	bille->assignerLocale(locale);
	bille->assignerPlayerNum(playerNum);

	zoneJeu->ajouter(bille);

	billesEnJeu_.push_back(bille);

	if (Config::obtenirInstance()->getGenBille() && Config::obtenirInstance()->getDebog())
	{
		utilitaire::timeStamp();
		cout << " - Nouvelle bille: x: " << pos[0] << "  y: " << pos[1] << endl;
	}
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_NOUVELLE_BILLE, false);
	return bille;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::reinitialiser()
///
/// Cette fonction réinitialise la scène à un état "vide".
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ZoneDeJeu::reinitialiser()
{
	billesEnJeu_.clear();
	billesAEffacer_.clear();
	NoeudAbstrait * noeud;
	while ((noeud =arbre_->chercher(ArbreRenduINF2990::NOM_BILLE)) != nullptr){
		arbre_->effacer(noeud);
	}
	VisiteurReinit visiteur = VisiteurReinit();
	arbre_->accepterVisiteur(&visiteur);
	arbre_->deselectionnerTout();
	arbre_->deEtamperTout();

	quadTree_ = QuadTree();
	quadTree_.setLimites(arbre_->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->obtenirBornes());
	quadTree_.ajouter(dynamic_cast<NoeudComposite *>(arbre_->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)));

}


void ZoneDeJeu::afficher() const
{
	vector<glm::dvec3> posBilles;
	for (int i = 0; i < billesEnJeu_.size(); i++)
	{
		posBilles.push_back(billesEnJeu_[i]->obtenirPositionRelative());
	}
	VisiteurPositionButoirs vis = VisiteurPositionButoirs();
	arbre_->accepterVisiteur(&vis);
	Affichage::obtenirInstance()->afficherLumieresBillesEtButoirs(posBilles,vis.obtenirPositions());
	arbre_->afficher();
}

ArbreRenduINF2990* ZoneDeJeu::obtenirArbre() 
{
	return arbre_;
}


void ZoneDeJeu::setPointButoirCercle(const int i)
{
	pointButoirCercle_ = i;
}

void ZoneDeJeu::setPointButoirTriangle(const int i)
{
	pointButoirTriangle_ = i;
}

void ZoneDeJeu::setPointCible(const int i)
{
	pointCible_ = i;
}

void ZoneDeJeu::setPointBilleGratuite(const int i)
{
	pointBilleGratuite_ = i;
}

void ZoneDeJeu::setPointCampagne(const int i)
{
	pointCampagne_ = i;
}

void ZoneDeJeu::setDifficulte(const int i)
{
	difficulte_ = i;
}


int ZoneDeJeu::getPointButoirCercle() const
{
	return pointButoirCercle_;
}

int ZoneDeJeu::getPointButoirTriangle() const
{
	return pointButoirTriangle_;
}

int ZoneDeJeu::getPointCible() const
{
	return pointCible_;
}

int ZoneDeJeu::getPointBilleGratuite() const
{
	return pointBilleGratuite_;
}

int ZoneDeJeu::getPointCampagne() const
{
	return pointCampagne_;
}

int ZoneDeJeu::getDifficulte() const
{
	return difficulte_;
}

string ZoneDeJeu::obtenirChemin() const 
{
	return path_;
}

vector<NoeudPalette*> ZoneDeJeu::dupliquerPalettes(vector<NoeudPalette*> palettes)
{
	auto zdj = arbre_->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU);
	auto nouveauxNoeuds = vector<NoeudPalette*>();
	for each(NoeudPalette* p in palettes)
	{
		auto nNoeud = arbre_->creerNoeud(p->obtenirType());
		nNoeud->assignerPositionRelative(p->obtenirPositionRelative());
		nNoeud->assignerRotation(p->obtenirRotation());
		nNoeud->assignerAgrandissement(p->obtenirAgrandissement());
		zdj->ajouter(nNoeud);
		quadTree_.ajouter(nNoeud);
		nouveauxNoeuds.push_back(static_cast<NoeudPalette*>(nNoeud));
	}
	return nouveauxNoeuds;
}

vector<NewBallEvent> ZoneDeJeu::obtenirEtatBilles() const
{
	vector<NewBallEvent> billes;
	for each(auto bille in billesEnJeu_)
	{
		billes.push_back(NewBallEvent(bille));
	}
	return billes;
}

vector<CibleSyncEvent> ZoneDeJeu::obtenirEtatCibles() const
{

	auto vis = VisiteurCiblesDetruites();
	arbre_->accepterVisiteur(&vis);
	return vis.getPositionsCibles();
}

vector<PowerUpSyncEvent> ZoneDeJeu::obtenirEtatPowerUps() const
{
	auto vis = VisiteurPowerUpVisibles();
	arbre_->accepterVisiteur(&vis);
	return vis.obtenirPowerUps();
}

void ZoneDeJeu::assignerBillesLocales(int player_num, bool b) const
{
	for each(auto bille in billesEnJeu_)
	{
		if(bille->obtenirPlayerNum() == player_num)
		{
			bille->assignerLocale(b);
		}
	}
}

bool ZoneDeJeu::load()
{
	if (arbre_ != nullptr)
	{
		delete arbre_;
	}

	arbre_ = new ArbreRenduINF2990;
	arbre_->initialiser();
	reinitialiser();

	string nomFichierString = path_;

	tinyxml2::XMLDocument doc;

	int pointButoirCercle = 0;
	int pointButoirTriangle = 0;
	int pointCible = 0;
	int pointCampagne = 0;
	int pointBilleGratuite = 0;
	int difficulte = 0;

	// Lire à partir du fichier
	doc.LoadFile(nomFichierString.c_str());

	tinyxml2::XMLNode * racine = doc.FirstChild();
	if (racine == nullptr)
	{
		cout << "Probleme de lecture du fichier XML";
		return false;
	}

	//////////////////////////////////////////////////////////
	/// Chargement des propriétés
	/////////////////////////////////////////////////////////////

	tinyxml2::XMLElement * pProprietes = racine->FirstChildElement("Proprietes");
	if (pProprietes == nullptr){ cout << "Erreur de lecture1"; return false; }
	tinyxml2::XMLElement * pPoint = pProprietes->FirstChildElement("PointButoirCercle");
	if (pPoint == nullptr){ cout << "Erreur de lecture2"; return false; }
	pPoint->QueryIntText(&pointButoirCercle);

	pPoint = pProprietes->FirstChildElement("PointButoirTriangle");
	if (pPoint == nullptr){ cout << "Erreur de lecture3"; return false; }
	pPoint->QueryIntText(&pointButoirTriangle);

	pPoint = pProprietes->FirstChildElement("PointCible");
	if (pPoint == nullptr){ cout << "Erreur de lecture4"; return false; }
	pPoint->QueryIntText(&pointCible);

	pPoint = pProprietes->FirstChildElement("PointCampagne");
	if (pPoint == nullptr){ cout << "Erreur de lecture5"; return false; }
	pPoint->QueryIntText(&pointCampagne);

	pPoint = pProprietes->FirstChildElement("PointBilleGratuite");
	if (pPoint == nullptr){ cout << "Erreur de lecture6"; return false; }
	pPoint->QueryIntText(&pointBilleGratuite);

	pPoint = pProprietes->FirstChildElement("Difficulte");
	if (pPoint == nullptr){ cout << "Erreur de lecture7"; return false; }
	pPoint->QueryIntText(&difficulte);

	//Decommenter la ligne suivante pour verifier que les propries sont bien lues
	//std::cout << "pointButoirCercle" << pointButoirCercle << "pointButoirTriangle" << pointButoirTriangle << "PointCible" << pointCible << "PointCampagne" << pointCampagne << "PointBilleGratuite" 
	//	<< pointBilleGratuite << "Difficulte" << difficulte;

	//////////////////////////////////////////////////////////
	/// Chargement des objets
	//////////////////////////////////////////////////////////

	int nbEnfant = 0;

	tinyxml2::XMLElement * pListeObjets = racine->FirstChildElement("ListeObjets");
	if (pListeObjets == nullptr){ cout << "Erreur de lecture8"; return false; }
	pListeObjets->QueryIntAttribute("NbObjets", &nbEnfant);

	//On initialise les tableaux avec le nbEnfant lu.
	const char** typeEnfants = new const char*[nbEnfant]();
	float** positionEnfants = new float*[nbEnfant]();
	float** agrandissementEnfants = new float*[nbEnfant]();
	for (int j = 0; j < nbEnfant; j++)
	{
		positionEnfants[j] = new float[3];
		agrandissementEnfants[j] = new float[3];
	}
	float* rotationEnfants = new float[nbEnfant]();
	float* longueur = new float[nbEnfant]();//Uniquement pour les murs


	int i = 0;
	tinyxml2::XMLElement * pObjet = pListeObjets->FirstChildElement("Objet");
	do {
		if (pObjet == nullptr){ cout << "Erreur de lecture9"; return false; }
		//std::cout << "Allo" << std::endl;
		tinyxml2::XMLElement * pType = pObjet->FirstChildElement("Type");
		if (pType == nullptr){ cout << "Erreur de lecture10"; return false; }
		const char* c = pType->GetText();
		typeEnfants[i] = c;

		tinyxml2::XMLElement * pPosition = pObjet->FirstChildElement("Position");
		if (pPosition == nullptr){ cout << "Erreur de lecture11"; return false; }
		pPosition->QueryFloatAttribute("X", &positionEnfants[i][0]);
		pPosition->QueryFloatAttribute("Y", &positionEnfants[i][1]);
		pPosition->QueryFloatAttribute("Z", &positionEnfants[i][2]);

		tinyxml2::XMLElement * pRotation = pObjet->FirstChildElement("Rotation");
		if (pRotation == nullptr){ cout << "Erreur de lecture12"; return false; }
		pRotation->QueryFloatText(&rotationEnfants[i]);

		tinyxml2::XMLElement * pAgrandissement = pObjet->FirstChildElement("Agrandissement");
		if (pAgrandissement == nullptr){ cout << "Erreur de lecture13"; return false; }
		pAgrandissement->QueryFloatAttribute("X", &agrandissementEnfants[i][0]);
		pAgrandissement->QueryFloatAttribute("Y", &agrandissementEnfants[i][1]);
		pAgrandissement->QueryFloatAttribute("Z", &agrandissementEnfants[i][2]);

		//Si c'est un mur, on doit obtenir la longueur du mur.

		if (strcmp(typeEnfants[i], "mur") == 0)
		{
			tinyxml2::XMLElement * pLongueur = pObjet->FirstChildElement("LongueurMur");
			if (pLongueur == nullptr){ cout << "Erreur de lecture14"; return false; }
			pLongueur->QueryFloatText(&longueur[i]);
		}
		//std::cout << "Type: " << typeEnfants[i] << " Position: X " << positionEnfants[i][0] << " Y" << positionEnfants[i][1] << " Z" << positionEnfants[i][2] << " Rotation: " << rotationEnfants[i] <<
		//	"Agrandissement: X " << agrandissementEnfants[i][0] << "Y " << agrandissementEnfants[i][1] << "Z " << agrandissementEnfants[i][2];
		//Si c'est un portail, on lit le frere du portail
		if (strcmp(typeEnfants[i], "portail") == 0)
		{
			i++;

			typeEnfants[i] = "portail";
			tinyxml2::XMLElement * pFrere = pObjet->FirstChildElement("Frere");
			if (pObjet == nullptr){ cout << "Erreur de lecture15"; return false; }
			tinyxml2::XMLElement * pPositionf = pFrere->FirstChildElement("Position");
			if (pPositionf == nullptr){ cout << "Erreur de lecture15"; return false; }
			pPositionf->QueryFloatAttribute("X", &positionEnfants[i][0]);
			pPositionf->QueryFloatAttribute("Y", &positionEnfants[i][1]);
			pPositionf->QueryFloatAttribute("Z", &positionEnfants[i][2]);

			tinyxml2::XMLElement * pRotationf = pFrere->FirstChildElement("Rotation");
			if (pRotationf == nullptr){ cout << "Erreur de lecture16"; return false; }
			pRotationf->QueryFloatText(&rotationEnfants[i]);

			tinyxml2::XMLElement * pAgrandissementf = pFrere->FirstChildElement("Agrandissement");
			if (pAgrandissementf == nullptr){ cout << "Erreur de lecture17"; return false; }
			pAgrandissementf->QueryFloatAttribute("X", &agrandissementEnfants[i][0]);
			pAgrandissementf->QueryFloatAttribute("Y", &agrandissementEnfants[i][1]);
			pAgrandissementf->QueryFloatAttribute("Z", &agrandissementEnfants[i][2]);

			//std::cout << "Type: " << typeEnfants[i] << " Position: X " << positionEnfants[i][0] << " Y" << positionEnfants[i][1] << " Z" << positionEnfants[i][2] << " Rotation: " << rotationEnfants[i] <<
			//	"Agrandissement: X " << agrandissementEnfants[i][0] << "Y " << agrandissementEnfants[i][1] << "Z " << agrandissementEnfants[i][2];
		}

		pObjet = pObjet->NextSiblingElement("Objet");
		i++;

	} while (i < nbEnfant);

	//On clear la zone de jeu existante et met ceux qu'on vient de lire.
	NoeudAbstrait* n = arbre_->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU);
	n->vider();

	for (int j = 0; j < nbEnfant; j++)
	{
		NoeudAbstrait* nouveauNoeud;

		if (strcmp(typeEnfants[j], "trou") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_TROU);
		}
		else if (strcmp(typeEnfants[j], "ressort") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_RESSORT);
		}
		else if (strcmp(typeEnfants[j], "generateurbille") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_GENERATEURBILLE);
		}
		else if (strcmp(typeEnfants[j], "butoirCercle") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE);
		}
		else if (strcmp(typeEnfants[j], "butoirTriangleGauche") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREGAUCHE);
		}
		else if (strcmp(typeEnfants[j], "butoirTriangleDroit") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREDROIT);
		}
		else if (strcmp(typeEnfants[j], "cible") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_CIBLE);
		}
		else if (strcmp(typeEnfants[j], "paletteGaucheJ1") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_PALETTEGAUCHEJ1);
		}
		else if (strcmp(typeEnfants[j], "paletteDroitJ1") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_PALETTEDROITJ1);
		}
		else if (strcmp(typeEnfants[j], "paletteGaucheJ2") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_PALETTEGAUCHEJ2);
		}
		else if (strcmp(typeEnfants[j], "paletteDroitJ2") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_PALETTEDROITJ2);
		}
		else if (strcmp(typeEnfants[j], "portail") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL);
		}
		else if (strcmp(typeEnfants[j], "mur") == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_MUR);
			dynamic_cast<NoeudMur *>(nouveauNoeud)->assignerLongueur(longueur[j]);
			dynamic_cast<NoeudMur *>(nouveauNoeud)->terminerCreation();
		}
		else if (strcmp(typeEnfants[j], ArbreRenduINF2990::NOM_CHAMPFORCE.c_str()) == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_CHAMPFORCE);
		}
		else if (strcmp(typeEnfants[j], ArbreRenduINF2990::NOM_PLATEAUDARGENT.c_str()) == 0)
		{
			nouveauNoeud = arbre_->creerNoeud(ArbreRenduINF2990::NOM_PLATEAUDARGENT);
		}
		else
		{
			cout << "Erreur dans le type dans le fichier XML.";
			return false;
		}

		nouveauNoeud->assignerPositionRelative(glm::dvec3(positionEnfants[j][0], positionEnfants[j][1], positionEnfants[j][2]));
		nouveauNoeud->assignerAgrandissement(glm::dvec3(agrandissementEnfants[j][0], agrandissementEnfants[j][1], agrandissementEnfants[j][2]));
		nouveauNoeud->assignerRotation(rotationEnfants[j]);

		if (strcmp(typeEnfants[j], "portail") == 0){
			j++;
			NoeudAbstrait* nouveauNoeud1 = arbre_->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL);
			nouveauNoeud1->assignerPositionRelative(glm::dvec3(positionEnfants[j][0], positionEnfants[j][1], positionEnfants[j][2]));
			nouveauNoeud1->assignerAgrandissement(glm::dvec3(agrandissementEnfants[j][0], agrandissementEnfants[j][1], agrandissementEnfants[j][2]));
			nouveauNoeud1->assignerRotation(rotationEnfants[j]);

			dynamic_cast<NoeudPortail *>(nouveauNoeud1)->assignerFrere(dynamic_cast<NoeudPortail *>(nouveauNoeud));
			dynamic_cast<NoeudPortail *>(nouveauNoeud)->assignerFrere(dynamic_cast<NoeudPortail *>(nouveauNoeud1));
			n->ajouter(nouveauNoeud1);
		}

		n->ajouter(nouveauNoeud);
		pointBilleGratuite_ = pointBilleGratuite;
		pointButoirCercle_ = pointButoirCercle;
		pointButoirTriangle_ = pointButoirTriangle;
		pointCible_ = pointCible;
		pointCampagne_ = pointCampagne;
		difficulte_ = difficulte;
			
	}

	delete typeEnfants;
	for (int k = 0; k < nbEnfant; k++)
	{
		delete positionEnfants[k];
		delete agrandissementEnfants[k];
	}
	delete positionEnfants;
	delete agrandissementEnfants;
	delete rotationEnfants;
	delete longueur;

	reinitialiser();
	return true;
}

bool ZoneDeJeu::saveZoneDeJeu(string path)
{

	string nomFichierString = path;
	path_ = path;
	
	NoeudAbstrait* n = arbre_->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU);
	int nbEnfant = n->obtenirNombreEnfants();

	////////////////////////////////////////////////////////////////////
	/// Vérification des prérequis pour l'enregistrement de fichier
	//////////////////////////////////////////////////////////////////
	int nbTrou = 0;
	int nbGenerateurBille = 0;
	int nbRessort = 0;

	for (int i = 0; i < nbEnfant; i++)
	{
		NoeudAbstrait * n1 = n->chercher(i);
		if (n1->obtenirType() == "trou")
			nbTrou++;
		else if (n1->obtenirType() == "ressort")
			nbRessort++;
		else if (n1->obtenirType() == "generateurbille")
			nbGenerateurBille++;
	}

	if (nbTrou == 0 || nbRessort == 0 || nbGenerateurBille == 0)
	{
		return false;
	}

	//////////////////////////////////////////////////////////////////
	/// Création du fichier XML
	////////////////////////////////////////////////////////////////
	tinyxml2::XMLDocument doc;
	tinyxml2::XMLNode * pRacine = doc.NewElement("Racine");
	doc.InsertFirstChild(pRacine);

	///////////////////////////////////////////////////////////////
	/// Enregistrement des propriétés
	///////////////////////////////////////////////////////////////////////
	tinyxml2::XMLElement * pProprietes = doc.NewElement("Proprietes");

	tinyxml2::XMLElement * pPoint = doc.NewElement("PointButoirCercle");
	pPoint->SetText(pointButoirCercle_);
	pProprietes->InsertEndChild(pPoint);

	pPoint = doc.NewElement("PointButoirTriangle");
	pPoint->SetText(pointButoirTriangle_);
	pProprietes->InsertEndChild(pPoint);

	pPoint = doc.NewElement("PointCible");
	pPoint->SetText(pointCible_);
	pProprietes->InsertEndChild(pPoint);

	pPoint = doc.NewElement("PointCampagne");
	pPoint->SetText(pointCampagne_);
	pProprietes->InsertEndChild(pPoint);

	pPoint = doc.NewElement("PointBilleGratuite");
	pPoint->SetText(pointBilleGratuite_);
	pProprietes->InsertEndChild(pPoint);

	pPoint = doc.NewElement("Difficulte");
	pPoint->SetText(difficulte_);
	pProprietes->InsertEndChild(pPoint);

	pRacine->InsertEndChild(pProprietes);

	//////////////////////////////////////////////////////////////////////////
	/// Enregistrement des objets
	////////////////////////////////////////////////////////////////////////
	string type;
	glm::dvec3 position;
	double rotation;
	glm::dvec3 agrandissement;

	tinyxml2::XMLElement * pElements = doc.NewElement("ListeObjets");
	tinyxml2::XMLElement * pElement;
	tinyxml2::XMLElement * pType;
	tinyxml2::XMLElement * pPosition;
	tinyxml2::XMLElement * pRotation;
	tinyxml2::XMLElement * pAgrandissement;

	pElements->SetAttribute("NbObjets", nbEnfant);
	n->selectionnerTout();
	for (int i = 0; i < nbEnfant; i++)
	{
		if (n->chercher(i)->estSelectionne())
		{
			n->chercher(i)->inverserSelection();

			pElement = doc.NewElement("Objet");

			NoeudAbstrait* n1 = n->chercher(i);
			type = n1->obtenirType();
			if (type == "trou")
				nbTrou++;
			else if (type == "ressort")
				nbRessort++;
			else if (type == "generateurbille")
				nbGenerateurBille++;
			position = n1->obtenirPositionRelative();
			rotation = n1->obtenirRotation();
			agrandissement = n1->obtenirAgrandissement();

			pType = doc.NewElement("Type");
			pType->SetText(type.c_str());
			pElement->InsertEndChild(pType);

			pPosition = doc.NewElement("Position");
			pPosition->SetAttribute("X", ((int)(position[0] * 100)) / 100.0);
			pPosition->SetAttribute("Y", ((int)(position[1] * 100)) / 100.0);
			pPosition->SetAttribute("Z", ((int)(position[2] * 100)) / 100.0);
			pElement->InsertEndChild(pPosition);

			pRotation = doc.NewElement("Rotation");
			pRotation->SetText(rotation);
			pElement->InsertEndChild(pRotation);

			pAgrandissement = doc.NewElement("Agrandissement");
			pAgrandissement->SetAttribute("X", agrandissement[0]);
			pAgrandissement->SetAttribute("Y", agrandissement[1]);
			pAgrandissement->SetAttribute("Z", agrandissement[2]);
			pElement->InsertEndChild(pAgrandissement);

			//Argument supplémentaire pour les murs
			if (type == "mur")
			{
				tinyxml2::XMLElement * pLongueurMur = doc.NewElement("LongueurMur");
				pLongueurMur->SetText(dynamic_cast<NoeudMur *>(n1)->obtenirLongueur());
				pElement->InsertEndChild(pLongueurMur);
			}

			//Si c'est un portail, on enregistre son frere.
			if (type == "portail")
			{
				NoeudPortail* nf = dynamic_cast<NoeudPortail *>(n->chercher(i))->obtenirFrere();
				nf->inverserSelection();

				string type1;
				glm::dvec3 position1;
				double rotation1;
				glm::dvec3 agrandissement1;

				tinyxml2::XMLElement * pFrere = doc.NewElement("Frere");
				tinyxml2::XMLElement * pPosition1;
				tinyxml2::XMLElement * pRotation1;
				tinyxml2::XMLElement * pAgrandissement1;

				position1 = nf->obtenirPositionRelative();
				rotation1 = nf->obtenirRotation();
				agrandissement1 = nf->obtenirAgrandissement();

				pPosition1 = doc.NewElement("Position");
				pPosition1->SetAttribute("X", position1[0]);
				pPosition1->SetAttribute("Y", position1[1]);
				pPosition1->SetAttribute("Z", position1[2]);
				pFrere->InsertEndChild(pPosition1);

				pRotation1 = doc.NewElement("Rotation");
				pRotation1->SetText(rotation1);
				pFrere->InsertEndChild(pRotation1);

				pAgrandissement1 = doc.NewElement("Agrandissement");
				pAgrandissement1->SetAttribute("X", agrandissement1[0]);
				pAgrandissement1->SetAttribute("Y", agrandissement1[1]);
				pAgrandissement1->SetAttribute("Z", agrandissement1[2]);
				pFrere->InsertEndChild(pAgrandissement1);

				pElement->InsertEndChild(pFrere);

				//On decremente de 1 puisque le frere est un objet mais qu'il est traite
				i--;
			}

			pElements->InsertEndChild(pElement);
		}
	}

	pRacine->InsertEndChild(pElements);

	/////////////////////////////////////
	/// Sauvegarde du fichier XML
	////////////////////////////////////

	doc.SaveFile(nomFichierString.c_str());
	return true;
}


VisiteurObtenirControles ZoneDeJeu::obtenirControles(){
	VisiteurObtenirControles visiteur;

	arbre_->accepterVisiteur(&visiteur);

	return visiteur;
}