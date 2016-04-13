//////////////////////////////////////////////////////////////////////////////
/// @file ClasseSons.h
/// @author Nicolas et Jerome
/// @date 20015-03-31
/// @version 1.0 
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __SONS_CLASSESONS_H_
#define __SONS_CLASSESONS_H_


#include "../../Commun/Externe/FMOD/include/fmod.hpp"
#include <map>

enum TYPE_SON 
{
	TYPE_SON_COLLISION_MUR,
	TYPE_SON_COLLISION_BUTOIR_CIRCULAIRE, 
	TYPE_SON_COLLISION_BUTOIR_TRIANGULAIRE,
	TYPE_SON_COLLISION_CIBLE,
	TYPE_SON_PALETTE, 
	TYPE_SON_COMPRESSION, 
	TYPE_SON_RELACHEMENT,
	TYPE_SON_PORTAIL, 
	TYPE_SON_TROU, 
	TYPE_SON_NOUVELLE_BILLE, 
	TYPE_SON_BILLE_GRATUITE, 
	TYPE_SON_PROCHAIN_NIVEAU, 
	TYPE_SON_PARTIE_GAGNEE,
	TYPE_SON_MUSIQUE,
	TYPE_SON_GAME_OVER, 
	TYPE_SON_DEMARRER_PARTIE,
	TYPE_SON_POWER_UP
};



typedef FMOD::Sound* Son;

class ClasseSons
{
public:
	static ClasseSons* obtenirInstance();
	static void bloquerSons();

	void creerSon(Son *son, const char* fichier);
	void jouerSon(TYPE_SON son, bool loop);
	void ClasseSons::jouerSon(TYPE_SON son, bool loop, int i);
	void relacherSon(TYPE_SON son);
	void arreterSons();
	void ClasseSons::arreterSon(int index);

	static const char* CHEMIN_TYPE_SON_COLLISION_MUR;
	static const char* CHEMIN_TYPE_SON_COLLISION_BUTOIR_CIRCULAIRE;
	static const char* CHEMIN_TYPE_SON_COLLISION_BUTOIR_TRIANGULAIRE;
	static const char* CHEMIN_TYPE_SON_COLLISION_CIBLE;
	static const char* CHEMIN_TYPE_SON_PALETTE;
	static const char* CHEMIN_TYPE_SON_COMPRESSION;
	static const char* CHEMIN_TYPE_SON_RELACHEMENT;
	static const char* CHEMIN_TYPE_SON_PORTAIL;
	static const char* CHEMIN_TYPE_SON_TROU;
	static const char* CHEMIN_TYPE_SON_NOUVELLE_BILLE;
	static const char* CHEMIN_TYPE_SON_BILLE_GRATUITE;
	static const char* CHEMIN_TYPE_SON_PROCHAIN_NIVEAU;
	static const char* CHEMIN_TYPE_SON_PARTIE_GAGNEE;
	static const char* CHEMIN_TYPE_SON_MUSIQUE;
	static const char* CHEMIN_TYPE_SON_GAME_OVER;
	static const char* CHEMIN_TYPE_SON_DEMARRER_PARTIE;
	static const char* CHEMIN_TYPE_SON_POWER_UP;
private:
	/******************************************************************************
	VARIABLE QUI ACTIVE LES SONS
	*******************************************************************************/
	static bool activerSons;
	int channel;


	ClasseSons();
	static ClasseSons* instance_;
	FMOD::ChannelGroup* groupe_;
	std::map<TYPE_SON, Son> sons_;
	FMOD::System* systeme_;
};

#endif //__SONS_CLASSESONS_H_