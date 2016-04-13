////////////////////////////////////////////////
/// @file   ClasseSons.cpp
/// @author Nicolas Blais, Jérôme Daigle
/// @date   2015-04-09
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////

#include "ClasseSons.h"

ClasseSons* ClasseSons::instance_ = nullptr;
bool ClasseSons::activerSons = true;

const char* ClasseSons::CHEMIN_TYPE_SON_COLLISION_MUR{ "Sons/sonPalette.wav" };
const char* ClasseSons::CHEMIN_TYPE_SON_COLLISION_BUTOIR_CIRCULAIRE{ "Sons/ting.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_COLLISION_BUTOIR_TRIANGULAIRE{ "Sons/ting.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_COLLISION_CIBLE{ "Sons/ting.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_PALETTE{ "Sons/sonPalette.wav" };
const char* ClasseSons::CHEMIN_TYPE_SON_COMPRESSION{ "Sons/spring.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_RELACHEMENT{ "Sons/sonRessort.wav" };
const char* ClasseSons::CHEMIN_TYPE_SON_PORTAIL{ "Sons/magic.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_TROU{ "Sons/ouh.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_NOUVELLE_BILLE{ "Sons/nouvelle_bille.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_BILLE_GRATUITE{ "Sons/free_ball.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_PROCHAIN_NIVEAU{ "Sons/applaudissement.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_PARTIE_GAGNEE{ "Sons/won.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_MUSIQUE{ "Sons/LetItGo_Multi.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_GAME_OVER{ "Sons/witch.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_DEMARRER_PARTIE{ "Sons/magic_long.mp3" };
const char* ClasseSons::CHEMIN_TYPE_SON_POWER_UP{ "Sons/PowerUp.mp3" };

////////////////////////////////////////////////////////////////////////
///
/// @fn ClasseSons::ClasseSons(bool b)
///
/// Constructeur
///
/// @param[in] b : booléen
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
ClasseSons::ClasseSons()
{
	channel = 2;
	if (System_Create(&systeme_) != FMOD_OK){
		return;
	}

	int driverCount = 0;
	systeme_->getNumDrivers(&driverCount);
	
	if (driverCount == 0){
		return;
	}
	if (activerSons)
	{
		systeme_->init(36, FMOD_INIT_NORMAL, nullptr); //initialise avec 36 channels différents pour 36 sons
		systeme_->createChannelGroup("groupe", &groupe_);

		Son son;
		creerSon(&son, CHEMIN_TYPE_SON_COLLISION_MUR);
		sons_[TYPE_SON_COLLISION_MUR] = son;
		
		Son son2;
		creerSon(&son2, CHEMIN_TYPE_SON_COLLISION_BUTOIR_CIRCULAIRE);
		sons_[TYPE_SON_COLLISION_BUTOIR_CIRCULAIRE] = son2;
		
		Son son3;
		creerSon(&son3, CHEMIN_TYPE_SON_COLLISION_BUTOIR_TRIANGULAIRE);
		sons_[TYPE_SON_COLLISION_BUTOIR_TRIANGULAIRE] = son3;
		
		Son son4;
		creerSon(&son4, CHEMIN_TYPE_SON_COLLISION_CIBLE);
		sons_[TYPE_SON_COLLISION_CIBLE] = son4;
		
		Son son5;
		creerSon(&son5, CHEMIN_TYPE_SON_PALETTE);
		sons_[TYPE_SON_PALETTE] = son5;

		Son son6;
		creerSon(&son6, CHEMIN_TYPE_SON_COMPRESSION);
		sons_[TYPE_SON_COMPRESSION] = son6;

		Son son7;
		creerSon(&son7, CHEMIN_TYPE_SON_RELACHEMENT);
		sons_[TYPE_SON_RELACHEMENT] = son7;

		Son son8;
		creerSon(&son8, CHEMIN_TYPE_SON_PORTAIL);
		sons_[TYPE_SON_PORTAIL] = son8;

		Son son9;
		creerSon(&son9, CHEMIN_TYPE_SON_TROU);
		sons_[TYPE_SON_TROU] = son9;

		Son son10;
		creerSon(&son10, CHEMIN_TYPE_SON_NOUVELLE_BILLE);
		sons_[TYPE_SON_NOUVELLE_BILLE] = son10;

		Son son11;
		creerSon(&son11, CHEMIN_TYPE_SON_BILLE_GRATUITE);
		sons_[TYPE_SON_BILLE_GRATUITE] = son11;

		Son son12;
		creerSon(&son12, CHEMIN_TYPE_SON_PROCHAIN_NIVEAU);
		sons_[TYPE_SON_PROCHAIN_NIVEAU] = son12;

		Son son13;
		creerSon(&son13, CHEMIN_TYPE_SON_PARTIE_GAGNEE);
		sons_[TYPE_SON_PARTIE_GAGNEE] = son13;

		Son son14;
		creerSon(&son14, CHEMIN_TYPE_SON_MUSIQUE);
		sons_[TYPE_SON_MUSIQUE] = son14;

		Son son15;
		creerSon(&son15, CHEMIN_TYPE_SON_GAME_OVER);
		sons_[TYPE_SON_GAME_OVER] = son15;

		Son son16;
		creerSon(&son16, CHEMIN_TYPE_SON_DEMARRER_PARTIE);
		sons_[TYPE_SON_DEMARRER_PARTIE] = son16;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ClasseSons* ClasseSons::obtenirInstance()
///
/// Cette fonction permet d'obtenir une instance de ClasseSons
///
/// @return Un pointeur vers un objet ClasseSons
///
////////////////////////////////////////////////////////////////////////
ClasseSons* ClasseSons::obtenirInstance()
{
	if (instance_ == nullptr)
	{
		instance_ = new ClasseSons();
	}
	return instance_;
}

void ClasseSons::bloquerSons()
{
	activerSons = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ClasseSons::creerSon(Son *son, const char* fichier)
///
/// Cette fonction permet de créer un son
///
/// @param[in] son : le type de son
/// @param[in] fichier : le nom du fichier
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ClasseSons::creerSon(Son *son, const char* fichier)
{
	if (activerSons)
	{
		systeme_->createSound(fichier, FMOD_HARDWARE, 0, son);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ClasseSons::jouerSon(TYPE_SON son, bool loop = false)
///
/// Cette fonction permet de jouer un son
///
/// @param[in] son : le type de son
/// @param[in] loop : si on veut que le son joue en boucle (à faux par défaut)
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ClasseSons::jouerSon(TYPE_SON son, bool loop = false)
{
	if (activerSons)
	{
		if (!loop){
			sons_[son]->setMode(FMOD_LOOP_OFF);
		}
		else {
			sons_[son]->setMode(FMOD_LOOP_NORMAL);
			sons_[son]->setLoopCount(-1);
		}
		systeme_->playSound(FMOD_CHANNELINDEX (channel), sons_[son], false, 0);
		channel++;
		if (channel == 36)
		{
			channel = 2;
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ClasseSons::jouerSon(TYPE_SON son, bool loop = false, int index = 0)
///
/// Cette fonction permet de jouer un son
///
/// @param[in] son : le type de son
/// @param[in] loop : si on veut que le son joue en boucle (à faux par défaut)
/// @param[in] index : l'index de la chaîne désirée
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ClasseSons::jouerSon(TYPE_SON son, bool loop, int index)
{
	if (activerSons)
	{
		sons_[son]->setMode(FMOD_LOOP_NORMAL);
		sons_[son]->setLoopCount(-1);
		systeme_->playSound(FMOD_CHANNELINDEX(index), sons_[son], false, 0);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ClasseSons::arreterSons()
///
/// Cette fonction permet d'arrêter un son
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ClasseSons::arreterSons()
{
	if (activerSons)
	{
		for (int i = 0; i < 36; i++)
		{
			FMOD::Channel* pChannel = nullptr;
			FMOD_RESULT res = systeme_->getChannel(i, &pChannel);

			if (res == FMOD_OK && pChannel)
			{
				pChannel->stop();
			}
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ClasseSons::arreterSon(int index)
///
/// Cette fonction permet d'arrêter un son
///
/// @param[in] index : l'index de la chaîne désirée
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ClasseSons::arreterSon(int index)
{
	if (activerSons)
	{
		
		FMOD::Channel* pChannel = nullptr;
		FMOD_RESULT res = systeme_->getChannel(index, &pChannel);

		if (res == FMOD_OK && pChannel)
		{
			pChannel->stop();
		}
		
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ClasseSons::relacherSon(TYPE_SON son)
///
/// Cette fonction permet de relâcher un son
///
/// @param[in] son : Le type de son désiré
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ClasseSons::relacherSon(TYPE_SON son)
{
	if (activerSons)
	{
		sons_[son]->release();
	}
}