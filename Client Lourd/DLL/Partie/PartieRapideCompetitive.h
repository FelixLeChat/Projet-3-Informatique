#ifndef __GAMELOGIC_PARTIE_RAPIDE_COMPETITIVE_H__
#define __GAMELOGIC_PARTIE_RAPIDE_COMPETITIVE_H__
////////////////////////////////////////////////
/// @file   PartieRapideCompetitive.h
/// @author Nicolas Blais, Jérôme Daigle
/// @date   2014-03-17
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////
#include "PartieBase.h"
#include <queue>

class SyncAllEvent;

class PartieRapideCompetitive : public PartieBase
{

public:
	/// Constructeur
	PartieRapideCompetitive() = default;
	PartieRapideCompetitive(vector<string> playerIds, string zone, int nb_ai, bool estHost, string matchId, bool estRejoin = false);
	virtual ~PartieRapideCompetitive();

	virtual bool demarrerPartie();

	void checkEndGame();
	virtual void perdreBille(int player_num);
	NoeudBille* lancerBille();
	/// Méthode d'analyse du pointage
	virtual void analyserPointage(int playerNum);

	virtual void update(IEvent * e);
	virtual void afficher();

	virtual void tick(double dt);

	void reinitialiser() override;

	/// Méthode d'obtention de fin de partie
	bool obtenirEstGagnee() override;
	int obtenirJoueurGagnant() const;


	int obtenirScoreJoueur(int joueur) const override;
	int obtenirBillesRestantesJoueur(int joueur_num) const override;

	void lancerBilleEnLigne(int id, glm::dvec3 pos, glm::dvec2 vitesse, int playerNum);

	int obtenirMonPointage() override;
	int obtenirMeilleurPointage() override;
	int obtenirMonNbBilles() override;



	virtual void toutSynchroniser(SyncAllEvent* SyncEvent);
	virtual void envoiSynchronisationGlobale();
protected:
	vector<int> pointage_;
	vector<int> nbBillesReserve_;
	vector<int> pointageDernierAjout_;
	vector<int> nbBillesEnJeu_;

	vector<string> playerIds_;
	queue<int> billesALancer_;
	bool estGagnee_;

	//VariableTemp de test online
	bool estHost_;
	string matchId_;
	int nbJoueurs_;
	int localPlayerNum_;
	int gagnant_;
};
#endif // !__GAMELOGIC_PARTIE_RAPIDE_COMPETITIVE_H__
