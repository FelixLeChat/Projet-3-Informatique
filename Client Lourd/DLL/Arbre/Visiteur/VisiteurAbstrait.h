//////////////////////////////////////////////////////////////////////////////
/// @file VisiteurAbstrait.h
/// @author Nicolas Blais, Jérémie Gagné
/// @date 2015-02-03
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef ARBRE_VISITEUR_VISITEUR_ABSTRAIT_H_
#define ARBRE_VISITEUR_VISITEUR_ABSTRAIT_H_

class NoeudAbstrait;

class NoeudButoirCirculaire;
class NoeudButoirTriangulaireGauche;
class NoeudButoirTriangulaireDroit;

class NoeudCible;
class NoeudGenerateurBille;

class NoeudPalette;
class NoeudPaletteDroitJ1;
class NoeudPaletteDroitJ2;
class NoeudPaletteGaucheJ1;
class NoeudPaletteGaucheJ2;

class NoeudRessort;
class NoeudTrou;

class NoeudMur;
class NoeudPortail;

class NoeudChampForce;
class NoeudPlateauDArgent;

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurAbstrait
/// @brief La classe de laquelle hérite les visiteurs
///
/// @author Nicolas Blais, Jérémie Gagné
/// @date 2015-02-03
///////////////////////////////////////////////////////////////////////////
class VisiteurAbstrait {

public:
	/// Constructeur par défaut
	VisiteurAbstrait(){};
	/// Destructeur
	virtual ~VisiteurAbstrait(){};

protected :
	/// Traitement de noeud selon le visiteur
	virtual void traiterNoeud(NoeudAbstrait*){};
	
public : 
	/// Traitement de noeud selon le visiteur
	virtual void traiterNoeudButoirCirculaire(NoeudButoirCirculaire*){};
	virtual void traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche*){};
	virtual void traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit*){};

	virtual void traiterNoeudCible(NoeudCible*){};
	virtual void traiterNoeudGenerateurBille(NoeudGenerateurBille*){};

	virtual void traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1*){};
	virtual void traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2*){};
	virtual void traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1*){};
	virtual void traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2*){};

	virtual void traiterNoeudRessort(NoeudRessort*){};
	virtual void traiterNoeudTrou(NoeudTrou*){};

	virtual void traiterNoeudMur(NoeudMur*){};
	virtual void traiterNoeudPortail(NoeudPortail*){};

	virtual void traiterNoeudChampForce(NoeudChampForce*){};
	virtual void traiterNoeudPlateauDArgent(NoeudPlateauDArgent*) {};

};
#endif //ARBRE_VISITEUR_VISITEUR_ABSTRAIT_H_

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////