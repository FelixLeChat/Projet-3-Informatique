#ifndef __GAMELOGIC_PARTIE_CAMPAGNE_COMPETITIVE_H__
#define __GAMELOGIC_PARTIE_CAMPAGNE_COMPETITIVE_H__
////////////////////////////////////////////////
/// @file   PartieCampagneCompetitive.h
/// @author Nicolas Blais, Jérôme Daigle
/// @date   2014-03-17
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////

#include "PartieRapideCompetitive.h"

class PartieCampagneCompetitive : public PartieRapideCompetitive
{
public:

	PartieCampagneCompetitive(vector<string> playerIds, vector<string> paths, int nb_ai, bool estHost, string matchId, bool estRejoin = false);
	~PartieCampagneCompetitive();

	void analyserPointage(int playerNum) override;
	bool demarrerPartie() override;
	void tick(double dt) override;
	void afficher() override;

	void update(IEvent* e) override;




	bool obtenirRondeGagnee() const override;
	int obtenirDifficulteZone() const override;
	int obtenirPointsAAtteindre() const override;


private:
	void afficherInformation() const;

	bool rondeGagnee;
	double tempsAffichage_;
	int partieActuelle_;
	vector<ZoneDeJeu*> zones_;
	vector<int> pointageDerniereZone_;
};
#endif // !__GAMELOGIC_PARTIE_CAMPAGNE_COMPETITIVE_H__
