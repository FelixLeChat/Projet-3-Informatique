#ifndef __ARBRE_NOEUDS_NOEUDRESSORT_H__
#define __ARBRE_NOEUDS_NOEUDRESSORT_H__

#include "NoeudAbstrait.h"


class NoeudRessort : public NoeudAbstrait{
public:
	/// Constructeur.
	NoeudRessort(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudRessort();

	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);
	/// Teste et execute la collision avec une bille
	virtual void executerCollision(NoeudBille* bille);
	virtual double obtenirEnfoncement(NoeudBille* bille);
	/// Appele lorsque le bouton est appuye
	void activer();
	/// Appele lorsque le bouton est relache
	void desactiver();
	/// Reinitialise le ressort
	void reinitialiser();
private:
	/// Collision avec segment
	void collisionAvecMur(NoeudBille* bille, glm::dvec3 point1, glm::dvec3 point2, double forceSup, bool topDuRessort);
	/// Indique si le ressort est en compression
	bool enCompression;
	/// Indique si le ressort est en decompression
	bool enDecompression;
	/// La compression du ressort
	double compression;
	/// La hauteur de base du ressort
	double hauteurBase;
	///Segments du ressort
	glm::dvec3 segments[4];
	/// Segments de base du ressort
	glm::dvec3 segmentsBase[4];
	/// Indique si la hauteur de base a ete calculee
	bool hauteurTrouve;
	/// calcule des bornes du ressort
	void calculerBornes();
	/// Indique si la collision est dans la meme phase de decompression
	bool collisionCettePhase;

	void calculerSegments();
	double delaiSon;

};

#endif //__ARBRE_NOEUDS_NOEUDRESSORT_H__