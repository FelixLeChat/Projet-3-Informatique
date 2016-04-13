////////////////////////////////////////////////
/// @file   NoeudAbstrait.cpp
/// @author DGI-2990
/// @date   2007-01-24
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////


#include <iostream>
#include "NoeudAbstrait.h"
#include "Utilitaire.h"
#include "AideCollision.h"
#include "Affichage/Affichage.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait::NoeudAbstrait(const std::string& type)
///
/// Ne fait qu'initialiser les variables membres de la classe.
///
/// @param[in] type               : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait::NoeudAbstrait(
	const string& type //= std::string{ "" }
	) :
	type_(type)
{
	modele_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait::~NoeudAbstrait()
///
/// Destructeur vide déclaré virtuel pour les classes dérivées.  La
/// libération des afficheurs n'est pas la responsabilité de cette
/// classe.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait::~NoeudAbstrait()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn unsigned int NoeudAbstrait::calculerProfondeur() const
///
/// Cette fonction calcule la profondeur de l'arbre incluant le noeud
/// courant ainsi que tous ses enfants.
///
/// Cette fonction retourne toujours 1 pour un noeud sans enfant.
///
/// @return La profondeur de l'arbre sous ce noeud, donc toujours 1.
///
////////////////////////////////////////////////////////////////////////
unsigned int NoeudAbstrait::calculerProfondeur() const
{
	return 1;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::vider()
///
/// Cette fonction vide le noeud de tous ses enfants.
///
/// Cette fonction ne fait rien pour un noeud sans enfant, elle ne fait
/// donc rien dans cette implantation par défaut de la classe de base.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::vider()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::effacer( const NoeudAbstrait* noeud )
///
/// Cette fonction efface le noeud s'il fait partie des enfants de
/// ce noeud.
///
/// Cette fonction ne fait rien pour un noeud sans enfant, elle ne fait
/// donc rien dans cette implantation par défaut de la classe de base.
///
/// @param[in] noeud : Le noeud à effacer.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::effacer(const NoeudAbstrait* noeud)
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn const NoeudAbstrait* NoeudAbstrait::chercher( const std::string& typeNoeud ) const
///
/// Cette fonction cherche un noeud d'un type donné parmi le noeud
/// lui-même et ses enfants.
///
/// Elle retourne donc le noeud lui-même si son type est celui passé en
/// paramètre, ou 0 sinon.
///
/// @param[in] typeNoeud : Le type du noeud à trouver.
///
/// @return Le pointeur vers le noeud s'il est trouvé.
///
////////////////////////////////////////////////////////////////////////
const NoeudAbstrait* NoeudAbstrait::chercher(const string& typeNoeud) const
{
	if (typeNoeud == type_)
		return this;
	else
		return nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* NoeudAbstrait::chercher( const std::string& typeNoeud )
///
/// Cette fonction cherche un noeud d'un type donné parmi le noeud
/// lui-même et ses enfants.
///
/// Elle retourne donc le noeud lui-même si son type est celui passé en
/// paramètre, ou 0 sinon.
///
/// @param[in] typeNoeud : Le type du noeud à trouver.
///
/// @return Le pointeur vers le noeud s'il est trouvé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* NoeudAbstrait::chercher(const string& typeNoeud)
{
	if (typeNoeud == type_)
		return this;
	else
		return nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn const NoeudAbstrait* NoeudAbstrait::chercher( unsigned int indice ) const
///
/// Cette fonction cherche le i-ème enfant d'un noeud.
///
/// Elle retourne toujours 0 pour la classe de base, car cette
/// dernière ne possède pas d'enfant.
///
/// @param[in] indice : L'indice du noeud à trouver.
///
/// @return Le pointeur vers le noeud s'il est trouvé.
///
////////////////////////////////////////////////////////////////////////
const NoeudAbstrait* NoeudAbstrait::chercher(unsigned int indice) const
{
	return nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAbstrait* NoeudAbstrait::chercher( unsigned int indice )
///
/// Cette fonction cherche le i-ème enfant d'un noeud.
///
/// Elle retourne toujours 0 pour la classe de base, car cette
/// dernière ne possède pas d'enfant.
///
/// @param[in] indice : L'indice du noeud à trouver.
///
/// @return Le pointeur vers le noeud s'il est trouvé.
///
////////////////////////////////////////////////////////////////////////
NoeudAbstrait* NoeudAbstrait::chercher(unsigned int indice)
{
	return nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool NoeudAbstrait::ajouter(NoeudAbstrait* enfant)
///
/// Cette fonction ajoute un enfant à ce noeud.
///
/// Elle retourne toujours faux et ne fait rien, car ce type de noeud
/// abstrait ne peut pas avoir d'enfant.
///
/// @param[in] enfant: Le noeud à ajouter.
///
/// @return Vrai si l'ajout a bien été effectué, faux autrement.
///
////////////////////////////////////////////////////////////////////////
bool NoeudAbstrait::ajouter(NoeudAbstrait* enfant)
{
	return false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn unsigned int NoeudAbstrait::obtenirNombreEnfants() const
///
/// Cette fonction retourne le nombre d'enfants de ce noeud.
///
/// Elle retourne toujours 0, car ce type de noeud abstrait ne peut pas
/// avoir d'enfant.
///
/// @return Vrai si l'ajout a bien été effectué, faux autrement.
///
////////////////////////////////////////////////////////////////////////
unsigned int NoeudAbstrait::obtenirNombreEnfants() const
{
	return 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool NoeudAbstrait::inverserSelection()
///
/// Cette fonction inverse l'état de sélection de ce noeud.
///
/// @return si le noeud est selectionné ou pas.
///
////////////////////////////////////////////////////////////////////////
bool NoeudAbstrait::inverserSelection()
{
	if (selectionnable_)
		selectionne_ = !selectionne_;

	return selectionne_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::effacerSelection()
///
/// Cette fonction efface les noeuds qui sont sélectionnés parmi les
/// enfants de ce noeud.
///
/// Elle ne fait rien, car ce type de noeud abstrait ne peut pas avoir
/// d'enfant.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::effacerSelection()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::effacerEtampes()
///
/// Cette fonction efface les noeuds qui sont des étampes parmi les
/// enfants de ce noeud.
///
/// Elle ne fait rien, car ce type de noeud abstrait ne peut pas avoir
/// d'enfant.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::effacerEtampes()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::selectionnerTout()
///
/// Cette fonction sélectionne le noeud et ses enfants.
///
/// Elle ne fait que sélectionner le noeud pour cette classe, car ce
/// type de noeud abstrait ne peut pas avoir d'enfants.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::selectionnerTout()
{
	assignerSelection(true);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::deselectionnerTout()
///
/// Cette fonction désélectionne le noeud et ses enfants.
///
/// Elle ne fait que désélectionner le noeud pour cette classe, car ce
/// type de noeud abstrait ne peut pas avoir d'enfants.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::deselectionnerTout()
{
	selectionne_ = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::deEtamperTout()
///
/// Cette fonction enlève les étampes.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::deEtamperTout()
{
	etampe_ = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool NoeudAbstrait::selectionExiste() const
///
/// Cette fonction vérifie si le noeud ou un de ses enfants est
/// sélectionné.
///
/// Elle ne fait que regarder si le noeud est sélectionné, car ce type
/// de noeud abstrait ne peut pas avoir d'enfants.
///
/// @return Vrai s'il existe un noeud sélectionné, faux autrement.
///
////////////////////////////////////////////////////////////////////////
bool NoeudAbstrait::selectionExiste() const
{
	return selectionne_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::changerModePolygones( bool estForce )
///
/// Cette fonction change le mode de rendu des polygones du noeud s'il
/// est sélectionné ou si on le force.
///
/// @param[in] estForce: Vrai si on veut changer le mode même si le
///                      noeud n'est pas sélectionné.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::changerModePolygones(bool estForce)
{
	if ((estForce) || (estSelectionne())) {
		if (modePolygones_ == GL_FILL)
			modePolygones_ = GL_LINE;
		else if (modePolygones_ == GL_LINE)
			modePolygones_ = GL_POINT;
		else
			modePolygones_ = GL_FILL;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::assignerModePolygones( GLenum modePolygones )
///
/// Cette fonction assigne le mode de rendu des polygones du noeud.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::assignerModePolygones(GLenum modePolygones)
{
	// Le mode par défaut si on passe une mauvaise valeur est GL_FILL
	if ((modePolygones != GL_FILL) &&
		(modePolygones != GL_LINE) &&
		(modePolygones != GL_POINT)) {
		modePolygones = GL_FILL;
	}

	modePolygones_ = modePolygones;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::afficher() const
///
/// Cette fonction affiche le noeud comme tel.
///
/// Elle consiste en une template method (dans le sens du patron de
/// conception, et non les template C++) qui effectue ce qui est
/// généralement à faire pour l'affichage, c'est-à-dire:
/// - Mise en pile de la matrice de transformation
/// - Translation du noeud pour qu'il soit à sa position relative
/// - Utilisation du mode d'affichage des polygones
/// - ...
/// - Restauration de l'état.
///
/// L'affichage comme tel est confié à la fonction afficherConcret(),
/// appelée par la fonction afficher().
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::afficher() const
{
	if (affiche_) {
		glPushMatrix();
		glPushAttrib(GL_CURRENT_BIT | GL_POLYGON_BIT);
		afficherSelectionne();

		// La translation de la position relative
		glTranslated(
			positionRelative_[0], positionRelative_[1], positionRelative_[2]
			);

		// Assignation du mode d'affichage des polygones
		glPolygonMode(GL_FRONT_AND_BACK, modePolygones_);


		glRotated(rotation_, 0, 0, 1);

		glScaled(agrandissement_.x, agrandissement_.y, agrandissement_.z);

		// Affichage concret

		afficherConcret();

		// Restauration
		glPopAttrib();
		glPopMatrix();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::assignerPositionRelative(const glm::dvec3& positionRelative)
///
/// Cette fonction permet d'assigner la position relative du noeud par
/// rapport à son parent.
///
/// @param positionRelative : La position relative.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::assignerPositionRelative(const glm::dvec3& positionRelative)
{
	positionRelative_ = positionRelative;
	calculerBornes();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn double NoeudAbstrait::obtenirRotation()
///
/// Cette fonction retourne la valeur de l'angle de rotation.
///
/// @return L'angle de rotation de l'objet
///
////////////////////////////////////////////////////////////////////////
const double NoeudAbstrait::obtenirRotation()
{
	return rotation_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::assignerRotation(double rotation)
///
/// Cette fonction assigne un nouvel angle de rotation.
///
/// @return Rien.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::assignerRotation(double rotation)
{
	rotation_ = rotation;
	if (rotation_ < 0)
	{
		rotation_ += ((int)rotation_ / -360 + 1) * 360;
	}
	else if (rotation_ > 360)
	{
		rotation_ += ((int)rotation_ / -360) * 360;
	}
	calculerBornes();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction permet d'accepter un visiteur.
///
/// @param visiteur : le visiteur à accepter.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::accepterVisiteur(VisiteurAbstrait* visiteur){}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::assignerAgrandissement(const glm::dvec3& agrandissement)
///
/// Cette fonction permet d'assigner l'aggrandissement du noeud.
///
/// @param agrandissement : l'agrandissement du noeud.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::assignerAgrandissement(const glm::dvec3& agrandissement)
{
	agrandissement_ = agrandissement;
	calculerBornes();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.  Elle est
/// appelée par la template method (dans le sens du patron de conception,
/// et non des template C++) afficher() de la classe de base.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::afficherConcret() const
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::animer( float dt )
///
/// Cette fonction effectue l'animation du noeud pour un certain
/// intervalle de temps.
///
/// Elle ne fait rien pour cette classe et vise à être surcharger par
/// les classes dérivées.
///
/// @param[in] dt : Intervalle de temps sur lequel faire l'animation.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::animer(float dt)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::afficherSelectionne() const
///
/// Cette fonction affiche une boite autour de l'objet selectionne
///
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::afficherSelectionne() const
{
	if (selectionne_){
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), false);
		glm::dvec3 xMin, xMax, yMin, yMax, zMin, zMax;
		utilitaire::obtenirPointsExtremes(modele_->obtenirNoeudRacine(), xMin, xMax, yMin, yMax, zMin, zMax);


		glColor3d(0, 0.7, 0);
		glLineWidth(2);
		glBegin(GL_LINE_LOOP);
		glVertex3d(bornes.coinMin.x - 0.5, bornes.coinMin.y - 0.5, 2);
		glVertex3d(bornes.coinMax.x + 0.5, bornes.coinMin.y - 0.5, 2);
		glVertex3d(bornes.coinMax.x + 0.5, bornes.coinMax.y + 0.5, 2);
		glVertex3d(bornes.coinMin.x - 0.5, bornes.coinMax.y + 0.5, 2);
		glEnd();
		glLineWidth(1);
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), true);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn utilitaire::BoiteEnglobante NoeudAbstrait::obtenirBornes()
///
/// Cette fonction permet d'obtenir les bornes de l'objet
///
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
utilitaire::BoiteEnglobante NoeudAbstrait::obtenirBornes()
{
	return bornes;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstrait::calculerBornes() 
///
/// Cette fonction calcule les bornes de la table de jeu.
///
///
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAbstrait::calculerBornes(){
	
	if (modele_ != nullptr){
		double transformation[16] = { agrandissement_.x * cos(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y *  sin(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
			agrandissement_.x * -sin(utilitaire::DEG_TO_RAD(rotation_)), agrandissement_.y*cos(utilitaire::DEG_TO_RAD(rotation_)), 0.0, 0.0,
			0.0, 0.0, agrandissement_.z, 0.0,
			positionRelative_.x, positionRelative_.y, -200.0, 1.0 };

		bornes = utilitaire::calculerBoiteEnglobante(*modele_, transformation);
	}
}

double NoeudAbstrait::obtenirEnfoncementCercle(glm::dvec3 pos1, double rayon1, glm::dvec3 pos2, double rayon2)
{
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionCercle(glm::dvec2(pos1.x, pos1.y), rayon1, glm::dvec2(pos2.x, pos2.y), rayon2);
	return test.type == aidecollision::COLLISION_AUCUNE ? 0 : test.enfoncement;
}

double NoeudAbstrait::obtenirEnfoncementSegment(glm::dvec3 pos1, double rayon1, glm::dvec3 point1, glm::dvec3 point2)
{
	aidecollision::DetailsCollision test = aidecollision::calculerCollisionSegment(point1, point2, pos1, rayon1, true);
	return test.type == aidecollision::COLLISION_AUCUNE ? 0 : test.enfoncement;
}


////////////////////////////////////////////////
/// @}
////////////////////////////////////////////////
