////////////////////////////////////////////////////////////////////////////////////
/// @file PartieCampagne.cpp
/// @author Nicolas Blais, Jérôme Daigle
/// @date 2015-03-17
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "PartieCampagne.h"
#include "EtatOpenGL.h"
#include "../Affichage/Affichage.h"
#include <FTGL/ftgl.h>
#include "../Sons/ClasseSons.h"
#include <tinyxml2.h>
#include "../Configuration/Config.h"
#include "../Event/EventManager.h"
#include "../Joueurs/JoueurManagerLocal.h"
#include "../Joueurs/JoueurManagerEnLigne.h"
#include "../Reseau/NetworkManager.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn PartieCampagne::PartieCampagne(int nbJoueurs, bool humain, std::string chemin, int longueurChemin)
///
/// Constructeur de campagne offline
///
/// @param[in] nbJoueurs : le nombre de joueurs.
/// @param[in] humain : si le deuxième joueur est humain.
/// @param[in] chemin : le chemin du fichier.
/// @param[in] longueurChemin : la longueur du nom du chemin du fichier.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

PartieCampagne::PartieCampagne(int nbJoueurs, bool humain, string chemin){
	nomFichierCampagne = chemin;
	estTerminee_ = false;
	estGagnee_ = false;
	pointage_ = 0;
	partieActuelle_ = 0;
	rondeGagnee = false;


	XMLCampagneBienLu = lireFichierCampagneXML(nomFichierCampagne);
	if (!XMLCampagneBienLu){
		cout << "Erreur de chargement du XML (PartieCampagne.cpp)\n";
		zones_.push_back(new ZoneDeJeu("zones\\zoneJeuDefaut.xml"));
		zones_.push_back(new ZoneDeJeu("zones\\zoneJeuDefaut.xml"));
		humain_ = true;
		nbJoueurs_ = 1;
	}

	partieEnLigne_ = false;

	EventManager::GetInstance()->subscribe(this, TIMEEVENT);
	EventManager::GetInstance()->subscribe(this, BALLLOST);
	EventManager::GetInstance()->subscribe(this, COLLISIONEVENT);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn PartieCampagne::PartieCampagne(int nbJoueurs, bool humain, std::string chemin, int longueurChemin)
///
/// Constructeur de campagne enligne
///
/// @param[in] nbJoueurs : le nombre de joueurs.
/// @param[in] humain : si le deuxième joueur est humain.
/// @param[in] chemin : le chemin du fichier.
/// @param[in] longueurChemin : la longueur du nom du chemin du fichier.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
PartieCampagne::PartieCampagne( vector<string> playerIds, vector<string> paths, int nb_ai, bool estHost, string matchId, bool estRejoin)
{
	estTerminee_ = false;
	estGagnee_ = false;
	pointage_ = 0;
	rondeGagnee = false;
	partieEnLigne_ = true;
	estHost_ = estHost;
	partieActuelle_ = 0;

	joueurManager_ = new JoueurManagerEnLigne(playerIds, nb_ai, true,estHost_);
	for each(auto path in paths)
	{
		auto zone = new ZoneDeJeu(path);
		zone->load();
		zones_.push_back(zone);
	}
	zoneCourante_ = zones_[0];
	matchId_ = matchId;

	auto controles = zoneCourante_->obtenirControles();
	joueurManager_->assignerControles(controles, zoneCourante_);


	if (estRejoin)
	{
		EventManager::GetInstance()->subscribe(this, TIMEEVENT);
		EventManager::GetInstance()->subscribe(this, SYNCALL);
		NetworkManager::getInstance()->requestGlobalSync();
	}
	else
	{
		EventManager::GetInstance()->subscribe(this, BALLLOST);
		EventManager::GetInstance()->subscribe(this, COLLISIONEVENT);
		EventManager::GetInstance()->subscribe(this, NEWBALL);
		EventManager::GetInstance()->subscribe(this, SCORESYNC);
		EventManager::GetInstance()->subscribe(this, TIMEEVENT);
		if (estHost_)
		{
			EventManager::GetInstance()->subscribe(this, SYNCALLREQUEST);
			EventManager::GetInstance()->subscribe(this, DISCONNECTEVENT);
		}
	}
}


PartieCampagne::~PartieCampagne(){
	EventManager::GetInstance()->unsubscribe(this, TIMEEVENT);
	EventManager::GetInstance()->unsubscribe(this, BALLLOST);
	EventManager::GetInstance()->unsubscribe(this, COLLISIONEVENT);
	EventManager::GetInstance()->unsubscribe(this, NEWBALL);
	EventManager::GetInstance()->unsubscribe(this, SCORESYNC);
	EventManager::GetInstance()->unsubscribe(this, SYNCALL);
	EventManager::GetInstance()->unsubscribe(this, SYNCALLREQUEST);
	EventManager::GetInstance()->unsubscribe(this, DISCONNECTEVENT);

	for(int i= 0; i<zones_.size(); i++)
	{
		if (i != partieActuelle_)
			delete zones_[i];
	}

}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool Partie::demarrerPartie()
///
/// Cette fonction permet de démarrer une partie.
///
/// @param[in] Aucun.
///
/// @return Si une partie est démarrée.
///
////////////////////////////////////////////////////////////////////////
bool PartieCampagne::demarrerPartie()
{
	tempsAvantProchaineBille_ = 5;
	tempsAffichage_ = 5;
	rondeGagnee = false;
	return PartieRapide::demarrerPartie();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn PartieCampagne::lireFichierCampagneXML(std::string cheminFichier, int longueurChemin)
///
/// Lire le fichier XML pour creer la campagne
///
/// @param[in] cheminFichier : le chemin menant au fichier
/// @param[in] longueurChemin : la longueur du chemin
///
/// @return bool
///
////////////////////////////////////////////////////////////////////////
bool PartieCampagne::lireFichierCampagneXML(string cheminFichier){
	tinyxml2::XMLDocument doc;

	// Lire à partir du fichier
	doc.LoadFile(cheminFichier.c_str());


	//nomPartie_ = cheminFichier;

	tinyxml2::XMLNode * racine = doc.FirstChild();
	if (racine == nullptr){
		//std::cout << "Erreur (1) de lecture du fichier XML de campagne.";
		return false;
	}
	tinyxml2::XMLElement * pFichiers = racine->FirstChildElement("Fichiers");
	if (pFichiers == nullptr){cout << "Erreur (2) de lecture du fichier XML de campagne";	return false;}
	float nbFichier;
	pFichiers->QueryFloatAttribute("nbFichiers", &nbFichier);

	int i = 0;
	tinyxml2::XMLElement * pNomFichier = pFichiers->FirstChildElement("nomFichier");
	do{
		if (pNomFichier == nullptr){ cout << "Erreur (3) de lecture du fichier XML de campagne";	return false; }
		const char* c = pNomFichier->GetText();
		zones_.push_back(new ZoneDeJeu(string(c)));

		i++;
		pNomFichier = pNomFichier->NextSiblingElement("nomFichier");
	} while (i < nbFichier);
	tinyxml2::XMLElement * pNbJoueur = racine->FirstChildElement("NbJoueur");
	if (pNbJoueur == nullptr){ cout << "Erreur (4) de lecture du fichier XML de campagne";	return false; }
	
	pNbJoueur->QueryIntText(&nbJoueurs_);

	tinyxml2::XMLElement * pHumain = racine->FirstChildElement("humain");
	if (pHumain == nullptr){ cout << "Erreur (5) de lecture du fichier XML de campagne";	return false; }
	int j;
	pHumain->QueryIntText(&j);
	//1 = humain, 0 = AI
	humain_ = (j == 1);
	return true;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn PartieCampagne::ecrireFichierCampagneXML(std::string cheminFichier, int longueurChemin)
///
/// Ecrit le fichier XML pour creer la campagne
///
/// @param[in] cheminFichier : le chemin menant au fichier
/// @param[in] longueurChemin : la longueur du chemin
///
/// @return bool
///
////////////////////////////////////////////////////////////////////////
bool PartieCampagne::ecrireFichierCampagneXML(string cheminFichier){
	
	tinyxml2::XMLDocument doc;
	tinyxml2::XMLNode * pRacine = doc.NewElement("Racine");
	doc.InsertFirstChild(pRacine);

	int nbArbres = int(zones_.size());

	tinyxml2::XMLElement * pFichiers = doc.NewElement("Fichiers");
	pFichiers->SetAttribute("nbFichiers", nbArbres);

	for (int i = 0; i < nbArbres; i++){
		tinyxml2::XMLElement * pNomFichier = doc.NewElement("nomFichier");
		string s = zones_[i]->obtenirChemin();
		pNomFichier->SetText(s.c_str());

		pFichiers->InsertEndChild(pNomFichier);

	}
	pRacine->InsertEndChild(pFichiers);

	tinyxml2::XMLElement * pNbJoueur = doc.NewElement("NbJoueur");
	pNbJoueur->SetText(nbJoueurs_);
	pRacine->InsertEndChild(pNbJoueur);

	tinyxml2::XMLElement * pHumain = doc.NewElement("humain");
	pHumain->SetText(humain_);
	pRacine->InsertEndChild(pHumain);

	/////////////////////////////////////
	/// Sauvegarde du fichier XML
	////////////////////////////////////

	doc.SaveFile(cheminFichier.c_str());

	return true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn std::string obtenirNomFichierXml(int i)
///
/// Renvoie le nom du fichier avec l'indice indique dans le vecteur pour la variable contenant les noms des fichiers xml.
///
/// @param[in] cheminFichier : le chemin menant au fichier
/// @param[in] longueurChemin : la longueur du chemin
///
/// @return bool
///
////////////////////////////////////////////////////////////////////////
string PartieCampagne::obtenirNomFichierXml(int i)
{
	return zones_[i]->obtenirChemin();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool envoyerNomFichier(char* nomFichier, int longueur)
///
/// Fonction appellee depuis c# pour donner un nom de fichier a charger dans l'arbre.
///
/// @param[in] nomFichier : le chemin menant au fichier
/// @param[in] longueur : la longueur du chemin
///
/// @return bool : Réussite ou echec du chargement
///
////////////////////////////////////////////////////////////////////////
bool PartieCampagne::envoyerNomFichier(char* nomFichier, int longueur)
{
	string s = nomFichier;
	s = s.substr(0, longueur);


	//Si c'est le premier fichier recu, on clear l'arbre.
	if (nombreFichiersRecus == 0)
		zones_.clear();

	auto zone = new ZoneDeJeu(s);
	bool reussi = zone->load();

	if (!reussi)
	{
		delete zone;
		zone = new ZoneDeJeu("zones\\zoneJeuDefault.xml");
	}
	zones_.push_back(zone);
	nombreFichiersRecus++;
	return reussi;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void PartieCampagne::analyserPointage()
///
/// Analyse du pointage, s'il y a assez de points pour obtenir une nouvelle bille
/// 
/// @param[in] Aucun.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////

void PartieCampagne::analyserPointage()
{
	int pointageActuel = pointage_ - pointageDerniereBille_ + (pointageDerniereBille_ % (zoneCourante_->getPointBilleGratuite()));
	if (pointage_ >= zoneCourante_->getPointCampagne())
	{
		if (partieActuelle_ < zones_.size() - 1)
		{
			rondeGagnee = true;
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_PROCHAIN_NIVEAU, false);
		}
		else
		{
			estTerminee_ = true;
			estGagnee_ = true;
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_PARTIE_GAGNEE, false);
		}
		return;
	
	}
	else if (pointage_ >= zoneCourante_->getPointBilleGratuite())
	{
		while (pointageActuel >= zoneCourante_->getPointBilleGratuite())
		{
			pointageActuel -= zoneCourante_->getPointBilleGratuite();
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_BILLE_GRATUITE, false);
			nbBillesReserve_++;
		}
		if ((!partieEnLigne_|| estHost_) && nbBillesEnJeu_ == 1 && Config::obtenirInstance()->getMode2Billes())
		{
			lancerBille();
		}
	}
	pointageDerniereBille_ = pointage_;
	return;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void PartieCampagne::finReceptionNomFichiers()
///
/// Fonction appellee par c# lorsqu'il a fini d'envoyer des noms de fichiers pour
/// faire un tri par difficulte des niveaux et enregistrer le xml campagne
///
////////////////////////////////////////////////////////////////////////
void PartieCampagne::finReceptionNomFichiers()
{
	for (unsigned int i = 0; i < zones_.size(); i++){
		for (unsigned int j = 0; j < zones_.size() - i -1; j++){
			if (zones_[j]->getDifficulte() > zones_[j + 1]->getDifficulte()){
				swap(zones_[j], zones_[j + 1]);
			}
		}
	}

	zoneCourante_ = zones_[0];
	joueurManager_->assignerControles(zoneCourante_->obtenirControles(),zoneCourante_);
	partieActuelle_ = 0;
	ecrireFichierCampagneXML("données\\campagneDefaut.xml");

}

/////////////////////////////////////////////////////////////////////////
/// @fn void recevoirJoueursHumain(bool nbJoueurs, bool humain);
///
///	 Fonction appellee lors du debut d'une partie campagne pour dire au c++ 
///  le nb de joueur et si le 2eme joueur sera un humain ou AI
///
///  @param[in] nbJoueurs : Nombre de joueurs (true = 1, false = 2)
///  @param[in] humain : Le second joueur est un: (true = humain, false = AI)
///
////////////////////////////////////////////////////////////////////////
void PartieCampagne::recevoirJoueursHumain(int nbJoueurs, bool humain)
{
	joueurManager_ = new JoueurManagerLocal(nbJoueurs, !humain);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Partie::afficherInformation(glm::ivec2 dimClo)
///
/// Affiche les informations de la partie
///
/// @param[in] dimClo : les dimensions de la cloture
///
/// @return Rien
///
////////////////////////////////////////////////////////////////////////

void PartieCampagne::afficherInformation()
{

	if (tempsAffichage_ > 0)
	{

		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), false);
		int cloture[4];
		glGetIntegerv(GL_VIEWPORT, cloture);
		double centreX = cloture[0] + cloture[2] / 2.0;
		double centreY = cloture[1] + cloture[3] / 2.0;

		glMatrixMode(GL_PROJECTION);
		glPushMatrix();
		glLoadIdentity();

		glOrtho(cloture[0], cloture[0]+ cloture[2], cloture[1], cloture[1] + cloture[3], -1, 1);

		glMatrixMode(GL_MODELVIEW);
		glPushMatrix();
		glLoadIdentity();

		glDisable(GL_DEPTH_TEST);


		glColor3f(1, 0, 0);
		glBegin(GL_QUADS);
		glVertex2f(float(centreX) - 160, float(centreY) + 75);
		glVertex2f(float(centreX) - 160, float(centreY) - 75);
		glVertex2f(float(centreX) + 160, float(centreY) - 75);
		glVertex2f(float(centreX) + 160, float(centreY) + 75);
		glEnd();
		glColor3f(1, 1, 1);



		EtatOpenGL etat;
		FTPixmapFont font = FTPixmapFont("C:/Windows/Fonts/Arial.ttf");
		FTPoint coordNom(centreX - 130, centreY + 40, 50);
		FTPoint coordDiff(centreX - 130, centreY + 10, 50);
		FTPoint coordPointage(centreX - 130, centreY - 20, 50);
		FTPoint coordPointage2(centreX - 130, centreY - 45, 50);
		if (font.Error()){
		cout << "Probleme de police dans partieCampagne.cpp" << endl;
		return;
		}

		//glPushAttrib(GL_ALL_ATTRIB_BITS);
		glLoadIdentity();
		glColor3f(0, 1, 0);
		font.FaceSize(20);

		char nom[255];
		char difficulte[255];
		char pointage[255];		
		glEnable(GL_BLEND);

		auto nomFichier =  zoneCourante_->obtenirChemin();
		nomFichier = nomFichier.substr(nomFichier.find_last_of("\\")+1);

		sprintf_s(nom, "Nom de la partie: %s", nomFichier.c_str());
		font.Render(nom, -1, coordNom);
		sprintf_s(difficulte, "Niveau de difficulte: %d ", zoneCourante_->getDifficulte());
		font.Render(difficulte, -1, coordDiff);
		sprintf_s(pointage, "Pointage necessaire pour");
		font.Render(pointage, -1, coordPointage);
		sprintf_s(pointage, "passer au prochain niveau: %d", zoneCourante_->getPointCampagne());
		font.Render(pointage, -1, coordPointage2);

		//glPopAttrib();
		glPopMatrix();
		glMatrixMode(GL_PROJECTION);
		glPopMatrix();
		glEnable(GL_DEPTH_TEST);
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), true);
	}
	
}
////////////////////////////////////////////////////////////////////////
///
/// @fn bool PartieCampagne::reinitialiser()
///
/// Cette fonction permet de démarrer une partie.
///
/// @param[in] Aucun.
///
/// @return Si une partie est démarrée.
///
////////////////////////////////////////////////////////////////////////

void PartieCampagne::reinitialiser()
{
	if (!partieEnLigne_)
	{
		for each(auto zone in zones_)
		{
			zone->reinitialiser();
		}

		partieActuelle_ = 0;
		estGagnee_ = false;
		estTerminee_ = false;
		zoneCourante_ = zones_[0];

		joueurManager_->assignerControles(zoneCourante_->obtenirControles(), zoneCourante_);
		rondeGagnee = false;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void  PartieCampagne::tick(double dt)
///
/// Cette fonction gere les changements liés au temps de la partie
///
/// @param[in] dt = temps écoulé
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void PartieCampagne::tick(double dt){
	if (rondeGagnee)
	{
		zoneCourante_=zones_[++partieActuelle_];
		joueurManager_->assignerControles(zoneCourante_->obtenirControles(),zoneCourante_);
		rondeGagnee = false;
		zoneCourante_->reinitialiser();
		demarrerPartie();
	}
	if (tempsAvantProchaineBille_ > 0)
		tempsAvantProchaineBille_ -= dt;
	if (tempsAffichage_ > 0)
		tempsAffichage_ -= dt;

	if (tempsAvantProchaineBille_ <= 0 
		&& tempsAffichage_ <= 0
		&& nbBillesReserve_ > 0 
		&& (nbBillesEnJeu_ == 0 || (nbBillesEnJeu_ == 1 && Config::obtenirInstance()->getMode2Billes()))){
		lancerBille();
	}
	if (partieEnLigne_)
	{
		NetworkManager::getInstance()->SyncReceivedMessages();
	}
	zoneCourante_->animer(dt);
}

void PartieCampagne::update(IEvent* e)
{
	PartieRapide::update(e);
}

void PartieCampagne::toutSynchroniser(SyncAllEvent* SyncEvent)
{
	partieActuelle_ = SyncEvent->zone_num();
	zoneCourante_ = zones_[partieActuelle_];
	joueurManager_->assignerControles(zoneCourante_->obtenirControles(), zoneCourante_);

	PartieRapide::toutSynchroniser(SyncEvent);
}

void PartieCampagne::envoiSynchronisationGlobale()
{
	auto zoneNum = partieActuelle_;
	auto pointage = vector<int>(1, pointage_);
	auto nbBillesRest = vector<int>(1, nbBillesReserve_);
	auto billes = zoneCourante_->obtenirEtatBilles();
	auto cibles = zoneCourante_->obtenirEtatCibles();
	auto powerUps = zoneCourante_->obtenirEtatPowerUps();
	auto palettes = static_cast<JoueurManagerEnLigne *> (joueurManager_)->obtenirEtatPalettes();

	//obtenir effets de scale

	NetworkManager::getInstance()->SyncAll(zoneNum, pointage, nbBillesRest, billes, cibles, powerUps, palettes);
}

bool PartieCampagne::obtenirRondeGagnee() const
{
	return tempsAffichage_ > 0;
}

int PartieCampagne::obtenirDifficulteZone() const
{
	return zoneCourante_->getDifficulte();
}

int PartieCampagne::obtenirPointsAAtteindre() const
{
	return zoneCourante_->getPointCampagne();
}


void PartieCampagne::afficher()
{
	PartieRapide::afficher();
	//afficherInformation();
}


bool PartieCampagne::obtenirEstHumain()
{
	return humain_;
}
int PartieCampagne::obtenirNbFichiers()
{
	return (int)zones_.size();
}
int PartieCampagne::obtenirNbJoueurs()
{
	return nbJoueurs_;
}

