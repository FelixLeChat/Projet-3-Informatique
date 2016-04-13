////////////////////////////////////////////////
/// @file   FacadeInterfaceNative.h
/// @author INF2990
/// @date   2014-08-16
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////
#ifndef __FACADE_INTERFACE_NATIVE_H__
#define __FACADE_INTERFACE_NATIVE_H__

extern "C" {

	//Sons
	__declspec(dllexport) void bloquerSons(); //Empeche les sons de jouer
	__declspec(dllexport) void arreterSons(); // Arrete les sons courants
	
	//Events
	__declspec(dllexport) void connexion(char* userToken, int token_ength, char* userId, int userId_length);
	__declspec(dllexport) void deconnexion();
	__declspec(dllexport) void animer(double temps);
	__declspec(dllexport) void appuyerBouton(int keycode);
	__declspec(dllexport) void dessinerOpenGL();
	__declspec(dllexport) void relacherBouton(int keycode);

	//Edition
	__declspec(dllexport) void annulerCreation();
	__declspec(dllexport) void assignerCentreSelection();
	__declspec(dllexport) void assignerSelPosition(double x, double y);
	__declspec(dllexport) void chargerFichierXML(char* nomFichier, int longueur);
	__declspec(dllexport) bool creerObjet(char* objet, int longueur, int x, int y);
	__declspec(dllexport) void deplacerSelection(int x1, int x2, int y1, int y2, bool force);
	__declspec(dllexport) void enregistrerFichierXML(char* nomFichier, int longueur);
	__declspec(dllexport) void finirCreationMur();
	__declspec(dllexport) void finirDuplication();
	__declspec(dllexport) bool getForceRebond();
	__declspec(dllexport) bool initialiserDuplication(int x, int y);
	__declspec(dllexport) void initRectangle(int dx, int dy);
	__declspec(dllexport) void miseAJourRectangle(int posOrigX, int posOrigY, int posPrecX, int posPrecY, int posX, int posY);
	__declspec(dllexport) void murFantome(int x, int y);
	__declspec(dllexport) double obtenirEchelleSelection();
	__declspec(dllexport) double obtenirPosXSel();
	__declspec(dllexport) double obtenirPosYSel();
	__declspec(dllexport) double obtenirRotationSelection();
	__declspec(dllexport) bool positionDansBornes(double x, double y);
	__declspec(dllexport) void reinitialiser();
	__declspec(dllexport) void redimensionnerSelection(int y1, int y2);
	__declspec(dllexport) void rotaterSelection(int y1, int y2);
	__declspec(dllexport) int selectionner(int x, int y, int longueur, int hauteur, bool ajout);
	__declspec(dllexport) void supprimerSelection();
	__declspec(dllexport) void terminerRectangle(int posOrigX, int posOrigY, int posX, int posY);
	__declspec(dllexport) int getNiveauCarte();

	__declspec(dllexport) int getPointBilleGratuite();
	__declspec(dllexport) int getPointButoirCercle();
	__declspec(dllexport) int getPointButoirTriangle();
	__declspec(dllexport) int getPointCampagne();
	__declspec(dllexport) int getPointCible();
	__declspec(dllexport) void mettreAJourProprietes(int collisionButoirCirculaire, int collisionButoirTriangulaire, int collisionCible, int pointBilleGratuite, int pointChangerNiveau, int difficulte);

	//Config
	__declspec(dllexport) void basculerDebug();
	__declspec(dllexport) bool creerConfig();
	__declspec(dllexport) bool getDebog();
	__declspec(dllexport) int getDifficulte();
	__declspec(dllexport) bool getEclairage();
	__declspec(dllexport) bool getGenBille();
	__declspec(dllexport) bool getLimitesPortails();
	__declspec(dllexport) bool getMode2Billes();
	__declspec(dllexport) int getNbBilles();
	__declspec(dllexport) int getPg1();
	__declspec(dllexport) int getPg2();
	__declspec(dllexport) int getPd1();
	__declspec(dllexport) int getPd2();
	__declspec(dllexport) int getRes();
	__declspec(dllexport) bool getVitBilles();
	__declspec(dllexport) void mettreAJourConfiguration(int paletteGJ1, int paletteDJ1, int paletteGJ2, int paletteDJ2, int ressort, int nbBille, bool mode2Bille, bool forceRebond, bool debog, bool genBille, bool collision, bool eclairage, bool limitePortail);
	__declspec(dllexport) void toucheDefaut(); // Reset la config

	__declspec(dllexport) int  obtenirAffichagesParSeconde();

	//Vue
	__declspec(dllexport) void cameraOrbite();
	__declspec(dllexport) void cameraOrtho();
	__declspec(dllexport) void deplacerCamera(double dx, double dy);
	__declspec(dllexport) void deplacerCameraInt(int dx, int dy);
	__declspec(dllexport) void redimensionnerFenetre(int largeur, int hauteur);
	__declspec(dllexport) void zoomIn();
	__declspec(dllexport) void zoomInRect(int posX1, int posY1, int posX2, int posY2);
	__declspec(dllexport) void zoomOut();
	__declspec(dllexport) void zoomOutRect(int posX1, int posY1, int posX2, int posY2);

	//Parties
	__declspec(dllexport) bool demarrerPartie();
	__declspec(dllexport) bool obtenirEstHumain(); // TODO changer cette shit
	__declspec(dllexport) bool  obtenirEstGagnee();
	__declspec(dllexport) bool obtenirEstTerminee();
	__declspec(dllexport) int obtenirNombreFichiers();
	__declspec(dllexport) bool obtenirNombreJoueur();

	__declspec(dllexport) int obtenirNbBillesJoueur(int joueur); 
	__declspec(dllexport) int obtenirPointageJoueur(int joueur);
	__declspec(dllexport) int obtenirMonPointage();
	__declspec(dllexport) int obtenirMonNbBilles();
	__declspec(dllexport) int obtenirMeilleurPointage();


	//Parties locales
	//{
	__declspec(dllexport) void ouvrirPartieCampagne(int, bool, char*);
	__declspec(dllexport) void ouvrirPartieRapide(int, bool, char*, int);
	__declspec(dllexport) void ouvrirPartieTest(char*, int);
	// Pour campagne, a modifier
	//{
	__declspec(dllexport) void obtenirFichier(char *str, int len, int i); 
	__declspec(dllexport) bool envoyerNomFichier(char* nomFichier, int longueur); // Pour campagnes, appels successifs pour chaque partie Todo: Changer en array
	__declspec(dllexport) void finReceptionNomFichiers();
	__declspec(dllexport) void transmettreJoueursHumain(bool nbJoueurs, bool humain);
	//}
	//}

	//Parties en ligne
	__declspec(dllexport) void creerPartieSimpleEnLigne(bool estCoop, int nbJoueurs, int nbAi, char* matchId, int matchId_length, char** playerIds, int* playerIdLength, char* zonePath, int zonePathLength, bool estHost, bool estRejoin = false);
	__declspec(dllexport) void creerPartieCampagneEnLigne(bool estCoop, int nbJoueurs, int nbAi, char* matchId, int matchId_length, char** playerIds, int* playerIdLength, int nbZones, char** zonePath, int* zonePathLength, bool estHost, bool estRejoin = false);
	

	// TODO 
	__declspec(dllexport) void quitterPartie();
	//si partie est en cours et que le joueur la quitte puis tente de la rejoindre
	__declspec(dllexport) void rejoindrePartieCampagne(bool estCoop, int nbJoueurs, int nbAi, char* matchId, int matchId_length, char** playerIds, int* playerIdLength, char* zonePath, int zonePathLength, bool estHost);
	__declspec(dllexport) void rejoindrePartieSimple(bool estCoop, int nbJoueurs, int nbAi, char* matchId, int matchId_length, char** playerIds, int* playerIdLength, int nbZones, char** zonePath, int* zonePathLength, bool estHost);


	//Tests
	__declspec(dllexport) bool executerTests();

	//Initialisation et finition
	__declspec(dllexport) void initialiserOpenGL(int * handle, char*, int);
	__declspec(dllexport) void libererOpenGL();
	__declspec(dllexport) void libererModele();


	

	__declspec(dllexport) bool obtenirRondeGagnee();
	__declspec(dllexport) int obtenirDifficulteZone();
	__declspec(dllexport) int obtenirPointsAAtteindre();
	__declspec(dllexport) void obtenirNomZone(char* nom);
	

}

#endif // __FACADE_INTERFACE_NATIVE_H__
