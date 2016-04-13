#ifndef __GAMELOGIC_PARTIE_RAPIDE_H__
#define __GAMELOGIC_PARTIE_RAPIDE_H__
////////////////////////////////////////////////
/// @file   PartieRapide.h
/// @author Nicolas Blais, Jérôme Daigle
/// @date   2014-03-17
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////
#include "PartieBase.h"
#include "../Event/SyncAllEvent.h"
#include <glm/detail/type_mat.hpp>
#include <glm/detail/type_mat.hpp>

class PartieRapide : public PartieBase
{

public:
	/// Constructeur
	PartieRapide() = default;
	PartieRapide(int nbJoueurs, bool humain, string chemin);
	PartieRapide(vector<string> playerIds, string zone, int nb_ai, bool estHost, string matchId, bool estRejoin = false);
	virtual ~PartieRapide();

	virtual bool demarrerPartie();
	NoeudBille* lancerBille();
	virtual void perdreBille();
	/// Méthode d'analyse du pointage
	virtual void analyserPointage();

	virtual void update(IEvent * e);
	void afficherPointage() const;
	virtual void afficher();

	virtual void tick(double dt);

	/// Méthode d'obtention de fin de partie
	bool obtenirEstGagnee() override;

	int obtenirScoreJoueur(int joueur) const override;
	int obtenirBillesRestantesJoueur(int joueur_num) const override;
	int obtenirMonPointage() override;
	int obtenirMonNbBilles() override;
	virtual void reinitialiser();

	void lancerBilleEnLigne(int id, glm::dvec3 pos, glm::dvec2 vitesse, double scale);

	virtual void toutSynchroniser(SyncAllEvent* SyncEvent);
	virtual void envoiSynchronisationGlobale();
protected:
	int pointage_;
	int nbBillesReserve_;
	int pointageDerniereBille_;
	bool estGagnee_;

	//VariableTemp de test online
	bool estHost_;
	bool partieEnLigne_;
	string matchId_;
	bool aSynchroniser_;
};
#endif // !__GAMELOGIC_PARTIE_RAPIDE_H__
