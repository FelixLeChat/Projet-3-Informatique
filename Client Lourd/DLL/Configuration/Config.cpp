////////////////////////////////////////////////////////////////////////////////////
/// @file Config.cpp
/// @author Ghislaine Menaceur, Blanche Paiement
/// @date 2015-03-10
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "Config.h"
#include <iostream>
#include <tinyxml2.h>

SINGLETON_DECLARATION_CPP(Config);

////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setPg1(const int i)
///
/// Cette fonction assigne la touche de contrôle de la palette gauche du joueur 1
///
/// @param[in] i : la touche de contrôle de la palette gauche du joueur 1.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setPg1(const int i)
{
	pg1_ = i;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setPd1(const int i)
///
/// Cette fonction assigne la touche de contrôle de la palette de droite du joueur 1
///
/// @param[in] i : la touche de contrôle de la palette de droite du joueur 1
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setPd1(const int i)
{
	pd1_ = i;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setPg2(const int i)
///
/// Cette fonction assigne la touche de contrôle de la palette de gauche du joueur 2 
///
/// @param[in] i : la touche de contrôle de la palette de gauche du joueur 2
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setPg2(const int i)
{
	pg2_ = i;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setPd2(const int i)
///
/// Cette fonction assigne la touche de contrôle de la palette de droite du joueur 2
///
/// @param[in] i : la touche de contrôle de la palette de droite du joueur 2
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setPd2(const int i)
{
	pd2_ = i;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setRes(const int i)
///
/// Cette fonction assigne la touche de contrôle des ressorts
///
/// @param[in] i : la touche de contrôle des ressorts
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setRes(const int i)
{
	res_ = i;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setNbBilles(const int i)
///
/// Cette fonction assigne le nombre de billes par partie
///
/// @param[in] i : le nombre de billes par partie
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setNbBilles(const int i)
{
	nbBilles_ = i;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setMode2Billes(const bool b)
///
/// Cette fonction assigne l'activation ou non du mode deux billes
///
/// @param[in] i : l'activation ou non du mode deux billes
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setMode2Billes(const bool b)
{
	mode2Billes_ = b;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setForceRebond(const bool b)
///
/// Cette fonction assigne l'activation ou non de la force de rebond sur les butoirs
///
/// @param[in] i : l'activation ou non de la force de rebond sur les butoirs
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setForceRebond(const bool b)
{
	forceRebond_ = b;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setDebog(const bool b)
///
/// Cette fonction assigne l'activation ou non de l'affichage de débogage
///
/// @param[in] b : l'activation ou non de l'affichage de débogage
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setDebog(const bool b)
{
	debog_ = b;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setGenBille(const bool b)
///
/// Cette fonction assigne l'activation ou non de l'affichage de la génération de bille
///
/// @param[in] b : l'activation ou non de l'affichage de la génération de bille
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setGenBille(const bool b)
{
	genBille_ = b;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setVitBilles(const bool b)
///
/// Cette fonction assigne l'activation ou non de l'affichage de la vitesse des billes après collision
///
/// @param[in] b : l'activation ou non de l'affichage de la vitesse des billes après collision
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::setVitBilles(const bool b)
{
	vitBilles_ = b;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setEclairage(const bool b)
///
/// Cette fonction assigne l'activation ou non de l'affichage de l'activation ou non de l'éclairage 
///
/// @param[in] b : l'activation ou non de l'affichage de l'activation ou non de l'éclairage 
///
/// @return Aucune.
///
//////////////////////////////////////////////////////////////////////// 
void Config::setEclairage(const bool b)
{
	eclairage_ = b;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::setLimitesPortails(const bool b)
///
/// Cette fonction assigne l'activation ou non de l'affichage de la limite d'attraction des portails
///
/// @param[in] b : l'activation ou non de l'affichage de la limite d'attraction des portails 
///
/// @return Aucune.
///
//////////////////////////////////////////////////////////////////////// 
void Config::setLimitesPortails(const bool b)
{
	limitesPortails_ = b;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const int Config::getPg1()
///
/// Cette fonction obtient la touche de contrôle de la palette gauche du joueur 1
///
/// @return la touche de contrôle de la palette gauche du joueur 1
///
////////////////////////////////////////////////////////////////////////
const int Config::getPg1()
{
	return pg1_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const int Config::getPd1()
///
/// Cette fonction obtient la touche de contrôle de la palette droite du joueur 1
///
/// @return la touche de contrôle de la palette droite du joueur 1
///
////////////////////////////////////////////////////////////////////////
const int Config::getPd1()
{
	return pd1_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const int Config::getPg2()
///
/// Cette fonction obtient la touche de contrôle de la palette gauche du joueur 2
///
/// @return la touche de contrôle de la palette gauche du joueur 2
///
////////////////////////////////////////////////////////////////////////
const int Config::getPg2()
{
	return pg2_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const int Config::getPd2()
///
/// Cette fonction obtient la touche de contrôle de la palette droite du joueur 2
///
/// @return la touche de contrôle de la palette droite du joueur 2
///
////////////////////////////////////////////////////////////////////////
const int Config::getPd2()
{
	return pd2_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const int Config::getRes()
///
/// Cette fonction obtient la touche de contrôle des ressorts
///
/// @return la touche de contrôle des ressorts
///
////////////////////////////////////////////////////////////////////////
const int Config::getRes()
{
	return res_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const int Config::getNbBilles()
///
/// Cette fonction obtient le nombre de billes dans la partie
///
/// @return le nombre de billes dans la partie
///
////////////////////////////////////////////////////////////////////////
const int Config::getNbBilles()
{
	return nbBilles_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const bool Config::getMode2Billes()
///
/// Cette fonction obtient si le mode deux billes est activé
///
/// @return si le mode deux billes est activé
///
////////////////////////////////////////////////////////////////////////
const bool Config::getMode2Billes()
{
	return mode2Billes_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const bool Config::getForceRebond()
///
/// Cette fonction obtient si la force de rebond est ajouté aux butoirs
///
/// @return si la force de rebond est ajouté aux butoirs
///
////////////////////////////////////////////////////////////////////////
const bool Config::getForceRebond()
{
	return forceRebond_;
}
////////////////////////////////////////////////////////////////////////
///
/// @fn const bool Config::getDebog()
///
/// Cette fonction obtient si l'affichage de débogage est activé ou non
///
/// @return si l'affichage de débogage est activé ou non
///
////////////////////////////////////////////////////////////////////////
const bool Config::getDebog()
{
	return debog_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const bool Config::getGenBille()
///
/// Cette fonction obtient si l'affichage de génération de bille est activé ou non
///
/// @return si l'affichage de génération de bille est activé ou non
///
////////////////////////////////////////////////////////////////////////
const bool Config::getGenBille()
{
	return genBille_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const bool Config::getVitBilles()
///
/// Cette fonction obtient si l'affichage de la vitesse des billes après collision est activé ou non
///
/// @return si l'affichage de la vitesse des billes après collision est activé ou non
///
////////////////////////////////////////////////////////////////////////
const bool Config::getVitBilles()
{
	return vitBilles_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const bool Config::getEclairage()
///
/// Cette fonction obtient si l'affichage de l'activation ou non de l'éclairage  est activé ou non
///
/// @return si l'affichage de l'activation ou non de l'éclairage  est activé ou non
///
////////////////////////////////////////////////////////////////////////
const bool Config::getEclairage()
{
	return eclairage_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const bool Config::getLimitesPortails()
///
/// Cette fonction obtient si l'affichage de la limite d'attraction des portails est activé ou non
///
/// @return si l'affichage de la limite d'attraction des portails est activé ou non
///
////////////////////////////////////////////////////////////////////////
const bool Config::getLimitesPortails()
{
	return limitesPortails_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::creerConfig ( TiXmlNode& node ) const
///
/// Cette fonction écrit les valeurs de la configuration dans un élément XML.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
bool Config::creerConfig(char* nomFichier, int longueur) const
{
	std::string nomFichierString = nomFichier;
	nomFichierString = nomFichierString.substr(0, longueur);

	//////////////////////////////////////////////////////////////////
	/// Création du fichier XML
	////////////////////////////////////////////////////////////////
	tinyxml2::XMLDocument doc;
	tinyxml2::XMLNode * Racine = doc.NewElement("Racine");
	doc.InsertFirstChild(Racine);

	///////////////////////////////////////////////////////////////
	/// Enregistrement des propriétés
	///////////////////////////////////////////////////////////////////////
	tinyxml2::XMLElement * configuration = doc.NewElement("Configuration");

	tinyxml2::XMLElement * touchesControle = doc.NewElement("TouchesControle");

	tinyxml2::XMLElement * pg1 = doc.NewElement("pg1");
	pg1->SetText(pg1_);
	touchesControle->InsertEndChild(pg1);

	tinyxml2::XMLElement * pd1 = doc.NewElement("pd1");
	pd1->SetText(pd1_);
	touchesControle->InsertEndChild(pd1);

	tinyxml2::XMLElement * pg2 = doc.NewElement("pg2");
	pg2->SetText(pg2_);
	touchesControle->InsertEndChild(pg2);

	tinyxml2::XMLElement * pd2 = doc.NewElement("pd2");
	pd2->SetText(pd2_);
	touchesControle->InsertEndChild(pd2);

	tinyxml2::XMLElement * res = doc.NewElement("res");
	res->SetText(res_);
	touchesControle->InsertEndChild(res);

	configuration->InsertEndChild(touchesControle);

	tinyxml2::XMLElement * nbBilles = doc.NewElement("nbBilles");
	nbBilles->SetText(nbBilles_);
	configuration->InsertEndChild(nbBilles);

	tinyxml2::XMLElement * mode2Billes = doc.NewElement("mode2Billes");
	mode2Billes->SetText(mode2Billes_);
	configuration->InsertEndChild(mode2Billes);

	tinyxml2::XMLElement * forceRebond = doc.NewElement("forceRebond");
	forceRebond->SetText(forceRebond_);
	configuration->InsertEndChild(forceRebond);

	tinyxml2::XMLElement * debogage = doc.NewElement("debogage");

	tinyxml2::XMLElement * debog = doc.NewElement("debogActivation");
	debog->SetText(debog_);
	debogage->InsertEndChild(debog);

	tinyxml2::XMLElement * debogBille = doc.NewElement("debogBille");
	debogBille->SetText(genBille_);
	debogage->InsertEndChild(debogBille);

	tinyxml2::XMLElement * debogCollision = doc.NewElement("debogCollision");
	debogCollision->SetText(vitBilles_);
	debogage->InsertEndChild(debogCollision);

	tinyxml2::XMLElement * debogEclairage = doc.NewElement("debogEclairage");
	debogEclairage->SetText(eclairage_);
	debogage->InsertEndChild(debogEclairage);

	tinyxml2::XMLElement * debogPortail = doc.NewElement("debogPortail");
	debogPortail->SetText(limitesPortails_);
	debogage->InsertEndChild(debogPortail);

	configuration->InsertEndChild(debogage);

	Racine->InsertEndChild(configuration);

	/////////////////////////////////////
	/// Sauvegarde du fichier XML
	////////////////////////////////////

	doc.SaveFile(nomFichierString.c_str());
	return true;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::lireConfig( const TiXmlNode& node )
///
/// Cette fonction lit les valeurs de la configuration à partir d'un élément
/// XML.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
bool Config::lireConfig(char* nomFichier, int longueur, bool tout)
{
	std::string nomFichierString = nomFichier;
	nomFichierString = nomFichierString.substr(0, longueur);

	tinyxml2::XMLDocument doc;

	// Lire à partir du fichier
	doc.LoadFile(nomFichierString.c_str());

	tinyxml2::XMLNode * racine = doc.FirstChild();
	if (racine == nullptr)
	{
		//std::cout << "Probleme de lecture xml fichier Config.cpp";
		lireConfig("données\\configDefaut.xml", 25, true);
		return false;
	}
	
	tinyxml2::XMLElement * config = racine->FirstChildElement("Configuration");
	if (config == nullptr){ std::cout << "Erreur de lecture1"; return false;	}
	
	tinyxml2::XMLElement * touches = config->FirstChildElement("TouchesControle");
	if (touches == nullptr){ std::cout << "Erreur de lecture2"; return false; }
	
	tinyxml2::XMLElement * pg1 = touches->FirstChildElement("pg1");
	if (pg1 == nullptr){ std::cout << "Erreur de lecture3"; return false; }
	pg1->QueryIntText(&pg1_);

	tinyxml2::XMLElement * pd1 = touches->FirstChildElement("pd1");
	if (pd1 == nullptr){ std::cout << "Erreur de lecture4"; return false; }
	pd1->QueryIntText(&pd1_);

	tinyxml2::XMLElement * pg2 = touches->FirstChildElement("pg2");
	if (pg2 == nullptr){ std::cout << "Erreur de lecture5"; return false; }
	pg2->QueryIntText(&pg2_);

	tinyxml2::XMLElement * pd2 = touches->FirstChildElement("pd2");
	if (pd2 == nullptr){ std::cout << "Erreur de lecture6"; return false; }
	pd2->QueryIntText(&pd2_);

	tinyxml2::XMLElement * res = touches->FirstChildElement("res");
	if (res == nullptr){ std::cout << "Erreur de lecture7"; return false; }
	res->QueryIntText(&res_);

	if (tout == true)
	{
		tinyxml2::XMLElement * nbBilles = config->FirstChildElement("nbBilles");
		if (nbBilles == nullptr){ std::cout << "Erreur de lecture8"; return false; }
		nbBilles->QueryIntText(&nbBilles_);

		tinyxml2::XMLElement * mode2Billes = config->FirstChildElement("mode2Billes");
		if (mode2Billes == nullptr){ std::cout << "Erreur de lecture9"; return false; }
		mode2Billes->QueryBoolText(&mode2Billes_);

		tinyxml2::XMLElement * forceRebond = config->FirstChildElement("forceRebond");
		if (forceRebond == nullptr){ std::cout << "Erreur de lecture10"; return false; }
		forceRebond->QueryBoolText(&forceRebond_);

		tinyxml2::XMLElement * debogage = config->FirstChildElement("debogage");
		if (debogage == nullptr){ std::cout << "Erreur de lecture11"; return false; }

		tinyxml2::XMLElement * debogActivation = debogage->FirstChildElement("debogActivation");
		if (debogActivation == nullptr){ std::cout << "Erreur de lecture12"; return false; }
		debogActivation->QueryBoolText(&debog_);

		tinyxml2::XMLElement * debogBille = debogage->FirstChildElement("debogBille");
		if (debogBille == nullptr){ std::cout << "Erreur de lecture13"; return false; }
		debogBille->QueryBoolText(&genBille_);

		tinyxml2::XMLElement * debogCollision = debogage->FirstChildElement("debogCollision");
		if (debogCollision == nullptr){ std::cout << "Erreur de lecture14"; return false; }
		debogCollision->QueryBoolText(&vitBilles_);

		tinyxml2::XMLElement * debogEclairage = debogage->FirstChildElement("debogEclairage");
		if (debogEclairage == nullptr){ std::cout << "Erreur de lecture15"; return false; }
		debogEclairage->QueryBoolText(&eclairage_);

		tinyxml2::XMLElement * debogPortail = debogage->FirstChildElement("debogPortail");
		if (debogPortail == nullptr){ std::cout << "Erreur de lecture16"; return false; }
		debogPortail->QueryBoolText(&limitesPortails_);
	}
		
	return true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Config::mettreAJourConfiguration(int paletteGJ1, int paletteDJ1, int paletteGJ2, int paletteDJ2, int ressort, int nbBille, bool mode2Bille, bool forceRebond, vector<bool> debog)
///
/// Cette fonction met à jour la configuration du jeu.
///
/// @param[in] paletteGJ1 : la touche de contrôle de la palette gauche du joueur 1
/// @param[in] paletteDJ1 : la touche de contrôle de la palette droite du joueur 1
/// @param[in] paletteGJ2 : la touche de contrôle de la palette gauche du joueur 2
/// @param[in] paletteDJ2 : la touche de contrôle de la palette droite du joueur 2
/// @param[in] ressort : la touche de contrôle du ressort
/// @param[in] nbBille : le nombre de billes par partie
/// @param[in] mode2Bille : l'activation ou non du mode deux billes
/// @param[in] forceRebond : l'activation ou non de la force de rebond supplémentaire des butoirs
/// @param[in] debog : l'activation ou non de l'affichage de débogage
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Config::mettreAJourConfiguration(int paletteGJ1, int paletteDJ1, int paletteGJ2, int paletteDJ2, int ressort, int nbBille, bool mode2Bille, bool forceRebond, bool debog, bool genBille, bool collision, bool eclairage, bool limitePortail)
{
	setPg1(paletteGJ1);
	setPd1(paletteDJ1);
	setPg2(paletteGJ2);
	setPd2(paletteDJ2);
	setRes(ressort);
	setNbBilles(nbBille);
	setMode2Billes(mode2Bille);
	setForceRebond(forceRebond);
	setDebog(debog);
	setGenBille(genBille);
	setVitBilles(collision);
	setEclairage(eclairage);
	setLimitesPortails(limitePortail);

	creerConfig("données\\config.xml", 19);

}

void Config::toucheDefaut()
{
	lireConfig("données\\configDefaut.xml", 25, false);
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
