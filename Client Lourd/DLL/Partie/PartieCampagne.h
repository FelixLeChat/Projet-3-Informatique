#ifndef __GAMELOGIC_PARTIE_CAMPAGNE_H__
#define __GAMELOGIC_PARTIE_CAMPAGNE_H__
////////////////////////////////////////////////
/// @file   PartieCampagne.h
/// @author Nicolas Blais, Jérôme Daigle
/// @date   2014-03-17
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////

#include "PartieRapide.h"

class PartieCampagne : public PartieRapide
{
public:

	/// Constructeur
	PartieCampagne(int nbJoueurs, bool humain, string chemin);
	PartieCampagne( vector<string> playerIds, vector<string> paths, int nb_ai, bool estHost, string matchId, bool estRejoin = false);
	~PartieCampagne();
	///Méthodes virtuelles

	/// Méthode virtuelle d'analyse du pointage
	virtual void analyserPointage();
	/// Méthode virtuelle d'obtention du nom du fichier XML
	virtual string obtenirNomFichierXml(int i);
	/// Méthode virtuelle de réinitialisation de la partie
	virtual void reinitialiser();
	/// Méthode virtuelle de démarrage de la partie
	virtual bool demarrerPartie();
	virtual void tick(double dt);

	virtual void afficher();

	//Fonctions appellees uniquement pour PartieCampagne

	/// Méthode virtuelle d'envoi du nom du fichier
	virtual  bool envoyerNomFichier(char* nomFichier, int longueur);
	/// Méthode virtuelle d'affichage des informations de la partie
	virtual void afficherInformation();
	/// Méthode virtuelle de tri de parties par difficulté
	virtual void finReceptionNomFichiers();
	/// Méthode virtuelle donnant le nombre de joueurs et leur nature humaine
	virtual void recevoirJoueursHumain(int nbJoueurs, bool humain);

	bool obtenirEstHumain();
	int obtenirNbFichiers();
	int obtenirNbJoueurs();

	void update(IEvent* e);
	void toutSynchroniser(SyncAllEvent* SyncEvent) override;
	void envoiSynchronisationGlobale() override;


	bool obtenirRondeGagnee() const override;
	int obtenirDifficulteZone() const override;
	int obtenirPointsAAtteindre() const override;

private:
	/// Méthode d'obtention de la réussite ou non du chargement du fichier XML 
	bool XMLCampagneBienLu;
	/// Méthode d'assignation d'un nom de fichier à l'arbre
	string nomFichierCampagne;
	/// Méthode d'écriture sur XML
	bool ecrireFichierCampagneXML(string cheminFichier);
	/// Méthode de lecture sur XML
	bool lireFichierCampagneXML(string cheminFichier);

	//Puisque partie campagne recoit les noms de fichiers, il faut vider le vieil arbre s'il n,en a pas encore recu (donc le savoir via nombreFichiersRecus == 0)
	int nombreFichiersRecus = 0;
	/// La partie actuelle
	int partieActuelle_;
	/// Si la ronde actuelle est gagnée
	bool rondeGagnee;

	double tempsAffichage_;

	//Config de campage
	int nbJoueurs_;
	bool humain_;


	vector<ZoneDeJeu*> zones_;

};
#endif // !__GAMELOGIC_PARTIE_CAMPAGNE_H__
