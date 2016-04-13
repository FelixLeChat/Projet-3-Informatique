#pragma once
#include "../Joueurs/IJoueur.h"
#include "../Arbre/Visiteur/VisiteurObtenirControles.h"
#include "../GameLogic/ZoneDeJeu.h"
class IJoueurManager
{
public:
	IJoueurManager(){}

	virtual ~IJoueurManager()
	{
		for each(auto joueur in joueurs_)
		{
			delete joueur;
			joueur = nullptr;
		}
		joueurs_.clear();
	}

	virtual void assignerControles(VisiteurObtenirControles visiteur, ZoneDeJeu* zone){}
	string obtenirId(int player_num)
	{
			return joueurs_[player_num]->obtenirId();
	}

	bool estLocal(int player_num) { return joueurs_[player_num]->estLocal(); }

	int obtenirPlayerNum(string user_id)
	{
		int i = 0;
		while (i < joueurs_.size() && joueurs_[i]->obtenirId() != user_id) i++;
		return i;
	}

	virtual void reconnect(string cs){}
protected:
	vector<IJoueur* > joueurs_;
};