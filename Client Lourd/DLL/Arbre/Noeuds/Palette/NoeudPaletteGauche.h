///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPaletteDroit.h
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-03-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDPALETTEGAUCHE_H__
#define __ARBRE_NOEUDS_NOEUDPALETTEGAUCHE_H__

#include "NoeudPalette.h"

///////////////////////////////////////////////////////////////////////////
/// @class NoeudPaletteGauche
/// @brief Classe de base des palettes de gauche
///
/// @author Jeremie Gagne, Konstantin Fedorov
/// @date 2015-03-21
///////////////////////////////////////////////////////////////////////////
class NoeudPaletteGauche : public NoeudPalette{
public:
	/// Constructeur.
	NoeudPaletteGauche(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPaletteGauche();

	
	/// Affiche le véritable rendu de l'objet
	virtual void afficherConcret() const;
	/// Calcule les bornes de la table de jeu
	virtual void calculerBornes();
	/// Teste et execute la collision avec une bille
	virtual void executerCollision(NoeudBille* bille);
	virtual double obtenirEnfoncement(NoeudBille* bille);
	bool obtenirEstDeDroite() override;


private:

	/// bornes bases calculees ou non
	bool bornesBaseTrouvees;
	void calculerSegments();
	/// Segments de base de la palette
	glm::dvec3 segmentsBase[4];

protected:
	virtual void calculerPoints();
	
};

#endif //__ARBRE_NOEUDS_NOEUDPALETTEGAUCHE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////