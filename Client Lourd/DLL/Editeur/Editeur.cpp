#include "Editeur.h"
#include "Utilitaire.h"
#include "Plan3D.h"
#include "../Arbre/Noeuds/NoeudPortail.h"
#include "../Arbre/Noeuds/NoeudMur.h"

#include "../Arbre/Visiteur/VisiteurDeplacement.h"
#include "../Arbre/Visiteur/VisiteurMiseAEchelle.h"
#include "../Arbre/Visiteur/VisiteurRotation.h"
#include "../Arbre/Visiteur/VisiteurNiveauCarte.h"

#include "../Affichage/Affichage.h"
#include "EtatOpenGL.h"
#include <Vue/VueOrtho.h>

Editeur::Editeur(): zone_(nullptr)
{
}

Editeur::~Editeur()
{
}

int Editeur::selectionner(int x, int y, int longueur, int largeur, bool ajout)
{
	glUseProgram(0);
	auto arbre = zone_->obtenirArbre();

	glInitNames();
	GLint* viewport = new GLint[4];
	glGetIntegerv(GL_VIEWPORT, viewport);

	glMatrixMode(GL_PROJECTION);
	glPushMatrix();
	glLoadIdentity();
	EtatOpenGL gl = EtatOpenGL();

	gluPickMatrix(x, viewport[3] - y, longueur, largeur, viewport);
	auto vue_ = Affichage::obtenirInstance()->obtenirVue();
	vue_->appliquerProjection(true);
	vue_->appliquerCamera();

	glMatrixMode(GL_MODELVIEW);

	int nbSel = arbre->chercherSelection(x, y, ajout, largeur == 3 && longueur == 3);

	glMatrixMode(GL_PROJECTION);
	glPopMatrix();
	glMatrixMode(GL_MODELVIEW);

	NoeudComposite* noeud = dynamic_cast<NoeudComposite *>(arbre->chercher("zonedejeu"));
	int max = noeud->obtenirNombreEnfants();
	int i;
	for (i = 0; i < max && !noeud->chercher(i)->estSelectionne(); i++)
	{
	}
	if (i != max)
		noeudSel_ = noeud->chercher(i);

	glUseProgram(Affichage::obtenirInstance()->obtenirProgNuanceur());
	return nbSel;
}

bool Editeur::creerObjet(const string objetACreer, int x, int y) const
{
	bool cree;
	glm::dvec3 positionVirtuelle;

	auto arbre = zone_->obtenirArbre();

	const glm::dvec3 normale = glm::dvec3(0, 0, 1);
	const glm::dvec3 pointDuPlan = glm::dvec3(0, 0, 0);
	const math::Plan3D plan = math::Plan3D(normale, pointDuPlan);
	Affichage::obtenirInstance()->convertirClotureAVirtuelle(x, y, plan, positionVirtuelle);

	NoeudAbstrait* noeud;
	noeud = arbre->creerNoeud(objetACreer);
	noeud->assignerPositionRelative(positionVirtuelle);

	arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->ajouter(noeud);
	cree = true;
	if (!arbre->estDansBornes(noeud))
	{
		arbre->effacer(noeud);
		cree = false;
	}
	else if (objetACreer == ArbreRenduINF2990::NOM_PORTAIL)
	{
		NoeudPortail* portail = dynamic_cast<NoeudPortail *>(noeud);
		NoeudAbstrait* frere = arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->chercher(arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->obtenirNombreEnfants() - 2);
		if (frere->obtenirType() == ArbreRenduINF2990::NOM_PORTAIL)
		{
			NoeudPortail* portailFrere = dynamic_cast<NoeudPortail *>(frere);
			if (portailFrere->obtenirFrere() == nullptr)
			{
				portail->assignerFrere(portailFrere);
				portailFrere->assignerFrere(portail);
			}
		}
	}

	return cree;
}

void Editeur::supprimerSelection()
{
	auto arbre = zone_->obtenirArbre();
	if (!arbre->tousEssentielSelectionne())
	{
		arbre->effacerSelection();
		noeudSel_ = arbre;
	}
}

void Editeur::annulerCreation() const
{
	auto arbre = zone_->obtenirArbre();
	NoeudAbstrait* noeud = arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU);
	NoeudAbstrait* dernierCree = noeud->chercher(noeud->obtenirNombreEnfants() - 1);
	if (dernierCree->obtenirType() == "mur" && !(dynamic_cast<NoeudMur *>(dernierCree))->obtenirCreationTerminee() || (dernierCree->obtenirType() == "portail" && (dynamic_cast<NoeudPortail *>(dernierCree))->obtenirFrere() == nullptr))
		arbre->effacer(dernierCree);
}


void Editeur::deplacerSelection(int x1, int x2, int y1, int y2, bool force) const
{
	auto arbre = zone_->obtenirArbre();
	glm::dvec3 positionVirtuelle1;
	glm::dvec3 positionVirtuelle2;
	const glm::dvec3 normale = glm::dvec3(0, 0, 1);
	const glm::dvec3 pointDuPlan = glm::dvec3(0, 0, 0);
	const math::Plan3D plan = math::Plan3D(normale, pointDuPlan);
	Affichage::obtenirInstance()->convertirClotureAVirtuelle(x1, y1, plan, positionVirtuelle1);
	Affichage::obtenirInstance()->convertirClotureAVirtuelle(x2, y2, plan, positionVirtuelle2);
	VisiteurDeplacement visiteur = VisiteurDeplacement(positionVirtuelle1.x - positionVirtuelle2.x, positionVirtuelle1.y - positionVirtuelle2.y, force);
	arbre->accepterVisiteur(&visiteur);

	if (!force && !arbre->estDansBornes())
	{
		glm::dvec2 depasse = arbre->obtenirDepassement();
		VisiteurDeplacement visiteurUndo = VisiteurDeplacement(-depasse.x, -depasse.y);
		arbre->accepterVisiteur(&visiteurUndo);
	}
}

void Editeur::redimensionnerSelection(int y1, int y2) const
{
	auto arbre = zone_->obtenirArbre();
	VisiteurMiseAEchelle visiteur = VisiteurMiseAEchelle((y1 - y2) * 0.01);
	arbre->accepterVisiteur(&visiteur);

	if (!arbre->estDansBornes())
	{
		VisiteurMiseAEchelle visiteurUndo = VisiteurMiseAEchelle((y2 - y1) * 0.01);
		arbre->accepterVisiteur(&visiteurUndo);
	}
}

void Editeur::rotaterSelection(int y1, int y2) const
{
	auto arbre = zone_->obtenirArbre();
	VisiteurRotation visiteur = VisiteurRotation(y2 - y1, arbre->obtenirCentreSelection());
	arbre->accepterVisiteur(&visiteur);

	if (!arbre->estDansBornes())
	{
		VisiteurRotation visiteurUndo = VisiteurRotation(-(y2 - y1), arbre->obtenirCentreSelection());
		arbre->accepterVisiteur(&visiteurUndo);
	}
}


bool Editeur::initDuplication(int x, int y) const
{
	auto arbre = zone_->obtenirArbre();
	arbre->calculerCentreSelection();
	glm::dvec3 centreSelect = arbre->obtenirCentreSelection();
	glm::dvec3 nouveauCentre;
	Affichage::obtenirInstance()->convertirClotureAVirtuelle(x, y, nouveauCentre);
	NoeudComposite* noeud = dynamic_cast<NoeudComposite *>(arbre->chercher("zonedejeu"));
	int max = noeud->obtenirNombreEnfants();
	for (int i = 0; i < max; i++)
	{
		NoeudAbstrait* enfant = noeud->chercher(i);
		//VÉRIFIER SI PORTAIL FRÈRE
		if (enfant->estSelectionne())
		{
			if (enfant->obtenirType() == "portail")
			{
				NoeudPortail* enfantPortail = dynamic_cast<NoeudPortail *>(enfant);
				if (enfantPortail->obtenirFrere()->estSelectionne())
				{
					NoeudPortail* frere = enfantPortail->obtenirFrere();
					enfantPortail->inverserSelection();
					frere->inverserSelection();
					NoeudPortail* enfantCopie = dynamic_cast<NoeudPortail *>(arbre->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL));
					NoeudPortail* frereCopie = dynamic_cast<NoeudPortail *>(arbre->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL));

					enfantCopie->assignerPositionRelative(enfantPortail->obtenirPositionRelative());
					enfantCopie->assignerAgrandissement(enfantPortail->obtenirAgrandissement());
					enfantCopie->assignerRotation(enfantPortail->obtenirRotation());
					enfantCopie->assignerEtampe(true);

					frereCopie->assignerPositionRelative(frere->obtenirPositionRelative());
					frereCopie->assignerAgrandissement(frere->obtenirAgrandissement());
					frereCopie->assignerRotation(frere->obtenirRotation());
					frereCopie->assignerEtampe(true);

					enfantCopie->assignerFrere(frereCopie);
					frereCopie->assignerFrere(enfantCopie);

					noeud->ajouter(frereCopie);
					noeud->ajouter(enfantCopie);
				}
				else
				{
					enfant->inverserSelection();
				}
			}
			else
			{
				NoeudAbstrait* copie = arbre->creerNoeud(enfant->obtenirType());
				copie->assignerPositionRelative(enfant->obtenirPositionRelative());
				copie->assignerAgrandissement(enfant->obtenirAgrandissement());
				copie->assignerRotation(enfant->obtenirRotation());
				copie->assignerEtampe(true);
				if (enfant->obtenirType() == "mur")
				{
					(dynamic_cast<NoeudMur *>(copie))->assignerLongueur((dynamic_cast<NoeudMur *>(enfant))->obtenirLongueur());
				}
				noeud->ajouter(copie);
			}
		}
	}
	VisiteurDeplacement visiteur = VisiteurDeplacement(nouveauCentre.x - centreSelect.x, nouveauCentre.y - centreSelect.y, true);
	arbre->accepterVisiteur(&visiteur);

	return true;
}

void Editeur::finirDuplication() const
{
	auto arbre = zone_->obtenirArbre();
	if (!arbre->estDansBornes(false))
	{
		arbre->effacerEtampes();
		arbre->deselectionnerTout();
	}
	else
	{
		arbre->deEtamperTout();
	}
}

void Editeur::assignerSelPosition(double x, double y) const
{
	auto arbre = zone_->obtenirArbre();
	glm::dvec3 pos = noeudSel_->obtenirPositionRelative();
	noeudSel_->assignerPositionRelative(glm::dvec3(x, y, pos.z));
	if (!arbre->estDansBornes(noeudSel_))
		noeudSel_->assignerPositionRelative(pos);
}

void Editeur::murFantome(int x, int y) const
{
	auto arbre = zone_->obtenirArbre();

	glm::dvec3 positionVirtuelle;
	Affichage::obtenirInstance()->convertirClotureAVirtuelle(x, y, positionVirtuelle);
	NoeudMur* mur = dynamic_cast<NoeudMur *>(arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->chercher(arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU)->obtenirNombreEnfants() - 1));
	glm::dvec3 ancientPoint = mur->obtenirDeuxiemePoint();
	mur->assignerDeuxiemePoint(positionVirtuelle.x, positionVirtuelle.y);

	if (!arbre->estDansBornes(mur))
	{
		mur->assignerDeuxiemePoint(ancientPoint.x, ancientPoint.y);
	}
}

void Editeur::finirCreationMur() const
{
	auto arbre = zone_->obtenirArbre();
	NoeudAbstrait* noeud = arbre->chercher(ArbreRenduINF2990::NOM_ZONEDEJEU);
	NoeudAbstrait* dernierCree = noeud->chercher(noeud->obtenirNombreEnfants() - 1);
	if (dernierCree->obtenirType() == "mur")
		dynamic_cast<NoeudMur*>(dernierCree)->terminerCreation();
}

void Editeur::assignerCentreSelection() const
{
	zone_->obtenirArbre()->calculerCentreSelection();
}


bool Editeur::positionDansBornes(int x, int y) const
{
	auto arbre = zone_->obtenirArbre();
	glm::dvec3 position;
	//vue_->convertirClotureAVirtuelle(x, y, position);
	NoeudAbstrait* zoneJeu = arbre->chercher("zonedejeu");
	utilitaire::BoiteEnglobante boite = zoneJeu->obtenirBornes();
	return (position.x <= boite.coinMax.x && position.y <= boite.coinMax.y && position.x >= boite.coinMin.x && position.y >= boite.coinMin.y);
}


double Editeur::obtenirEchelleSelection() const
{
	return noeudSel_->obtenirAgrandissement().y;
}

double Editeur::obtenirRotationSelection() const
{
	return noeudSel_->obtenirRotation();
}

double Editeur::obtenirPosXSel() const
{
	return noeudSel_->obtenirPositionRelative().x;
}

double Editeur::obtenirPosYSel() const
{
	return noeudSel_->obtenirPositionRelative().y;
}

void Editeur::assignerZoneDeJeu(ZoneDeJeu* zone)
{
	zone_ = zone;
}

bool Editeur::enregistrerFichierXML(char* nomFichier, int longueur) const
{
	return zone_->saveZoneDeJeu(string(nomFichier, longueur));
}

void Editeur::mettreAJourProprietes(int pointButoirCercle, int pointButoirTriangle, int pointCible, int pointBilleGratuite, int pointCampagne, int difficulte) const
{
	zone_->setPointButoirCercle(pointButoirCercle);
	zone_->setPointButoirTriangle(pointButoirTriangle);
	zone_->setPointCible(pointCible);
	zone_->setPointBilleGratuite(pointBilleGratuite);
	zone_->setPointCampagne(pointCampagne);
	zone_->setDifficulte(difficulte);
}


int Editeur::getPointButoirCercle() const
{
	return zone_->getPointButoirCercle();
}

int Editeur::getPointButoirTriangle() const
{
	return zone_->getPointButoirTriangle();
}

int Editeur::getPointCible() const
{
	return zone_->getPointCible();
}

int Editeur::getPointBilleGratuite() const
{
	return zone_->getPointBilleGratuite();
}

int Editeur::getPointCampagne() const
{
	return zone_->getPointCampagne();
}

int Editeur::getDifficulte() const
{
	return zone_->getDifficulte();
}

int Editeur::getNiveauCarte() const
{
	VisiteurNiveauCarte visiteur = VisiteurNiveauCarte();
	zone_->obtenirArbre()->accepterVisiteur(&visiteur);
	return visiteur.obtenirNiveau();
}
