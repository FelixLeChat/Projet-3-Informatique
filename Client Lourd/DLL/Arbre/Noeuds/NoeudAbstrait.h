///////////////////////////////////////////////////////////////////////////////
/// @file NoeudAbstrait.h
/// @author DGI-INF2990
/// @date 2007-01-24
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDABSTRAIT_H__
#define __ARBRE_NOEUDS_NOEUDABSTRAIT_H__


#include <windows.h>
#include <gl/glew.h>
#include <GL/gl.h>
#include "glm\glm.hpp"
#include "Arbre/Visiteur/VisiteurAbstrait.h"

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"

class NoeudBille;
/// D�clarations avanc�es pour contenir un pointeur vers un mod�le3D et sa liste
/// d'affichage
/*namespace modele{
	class Modele3D;
	namespace opengl_storage{
		class OpenGL_Liste;
	}
}*/

///////////////////////////////////////////////////////////////////////////
/// @class NoeudAbstrait
/// @brief Classe de base du patron composite utilis�e pour cr�er l'arbre
///        de rendu.
///
///        Cette classe abstraite comprend l'interface de base que doivent
///        implanter tous les noeuds pouvant �tre pr�sent dans l'arbre de
///        rendu.
///
/// @author DGI-2990
/// @date 2007-01-24
///////////////////////////////////////////////////////////////////////////
class NoeudAbstrait
{
public:
	/// Constructeur.
	NoeudAbstrait(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudAbstrait();

	/// Obtient le parent de ce noeud.
	inline NoeudAbstrait* obtenirParent();

	/// Obtient le parent de ce noeud (version constante).
	inline const NoeudAbstrait* obtenirParent() const;

	/// Assigne le parent de ce noeud.
	inline void assignerParent(NoeudAbstrait* parent);

	/// Obtient la position relative du noeud.
	inline const glm::dvec3& obtenirPositionRelative() const;

	/// Assigne la position relative du noeud.
	virtual void assignerPositionRelative(const glm::dvec3& positionRelative);

	/// Obtient la taille d'un noeud
	inline const glm::dvec3& obtenirAgrandissement() const;

	/// Assigne la taille d'un noeud
	void assignerAgrandissement(const glm::dvec3& taille);

	/// Obtient la rotation d'un noeud
	virtual const double obtenirRotation();

	/// Assigne la rotation d'un noeud
	virtual void assignerRotation(double rotation);

	///Accepter les visiteurs
	virtual void accepterVisiteur(VisiteurAbstrait*);

	/// Obtient le type du noeud.
	inline const std::string& obtenirType() const;

	/// �crit l'�tat de l'affichage du du noeud.
	inline void assignerAffiche(bool affiche);
	/// V�rifie si le noeud se fait afficher.
	inline bool estAffiche() const;

	/// �crit l'�tat de la s�lection du noeud.
	inline void assignerSelection(bool selectionne);
	/// V�rifie si le noeud est s�lectionn�.
	inline bool estSelectionne() const;

	/// Assigne une �tampe
	inline void assignerEtampe(bool etampe);
	/// V�rifie si un noeud est une �tampe
	inline bool estEtampe() const;
	/// �crit si le noeud peut �tre s�lectionn� ou non.
	inline void assignerEstSelectionnable(bool selectionnable);
	/// V�rifie si le noeud est s�lectionnable.
	inline bool estSelectionnable() const;
	/// �crit si le noeud peut �tre enregistr� ou non.
	inline void assignerEstEnregistrable(bool enregistrable);
	/// V�rifie si le noeud est enregistrable.
	inline bool estEnregistrable() const;

	/// Assigne le mod�le3D et la liste d'affichage du noeud courant
	inline void assignerObjetRendu(modele::Modele3D const* modele, modele::opengl_storage::OpenGL_Liste const* liste);

	///affiche une boite contenant l'objet selectionne
	virtual void afficherSelectionne() const;

	// Interface d'un noeud

	/// Calcule la profondeur de l'arbre sous le noeud courant.
	virtual unsigned int calculerProfondeur() const;

	/// Vide le noeud de ses enfants.
	virtual void vider();
	/// Efface le noeud pass� en param�tre.
	virtual void effacer(const NoeudAbstrait* noeud);

	/// Cherche un noeud par le type (sur un noeud constant).
	virtual const NoeudAbstrait* chercher(const std::string& typeNoeud) const;
	/// Cherche un noeud par le type.
	virtual NoeudAbstrait* chercher(const std::string& typeNoeud);
	/// Cherche un noeud enfant selon l'indice (sur un noeud constant).
	virtual const NoeudAbstrait* chercher(unsigned int indice) const;
	/// Cherche un noeud enfant selon l'indice.
	virtual NoeudAbstrait* chercher(unsigned int indice);

	/// Ajoute un noeud enfant.
	virtual bool ajouter(NoeudAbstrait* enfant);
	/// Obtient le nombre d'enfants du noeud.
	virtual unsigned int obtenirNombreEnfants() const;

	/// Changer la s�lection du noeud.
	virtual bool inverserSelection();
	/// Efface les enfants s�lectionn�s.
	virtual void effacerSelection();
	/// S�lectionne tous les enfants de m�me que le noeud.
	virtual void selectionnerTout();
	/// D�s�lectionne tous les enfants de m�me que le noeud.
	virtual void deselectionnerTout();
	/// Retire toutes les �tampes
	virtual void deEtamperTout();
	/// Efface toutes les �tampes
	virtual void effacerEtampes();
	/// V�rifier si le noeud ou un de ses enfants est s�lectionn�.
	virtual bool selectionExiste() const;

	/// Change le mode d'affichage des polygones.
	virtual void changerModePolygones(bool estForce);
	/// Assigne le mode d'affichage des polygones.
	virtual void assignerModePolygones(GLenum modePolygones);
	/// Affiche le noeud.
	virtual void afficher() const;
	/// Affiche le noeud de mani�re concr�te.
	virtual void afficherConcret() const;
	/// Anime le noeud.
	virtual void animer(float dt);

	///Retourne les bornes xmin xmax ymin ymax de la boite englobante
	virtual utilitaire::BoiteEnglobante obtenirBornes();
	/// Calcule les bornes de la table de jeu
	virtual void calculerBornes();

	/// executer une collision avec une bille
	virtual void executerCollision(NoeudBille* bille);
	///obtient les informations de la collision
	virtual double obtenirEnfoncement(NoeudBille* bille);

	virtual void appliquerForceConstante(NoeudBille* bille) {};

protected:
	/// Type du noeud.
	std::string      type_;

	/// Mode d'affichage des polygones.
	GLenum           modePolygones_{ GL_FILL };

	/// Position relative du noeud.
	glm::dvec3       positionRelative_;

	/// Taille de l'objet
	glm::dvec3		 agrandissement_{ 1, 1, 1 };

	/// Rotation de l'objet
	double			 rotation_{ 0 };

	/// Vrai si on doit afficher le noeud.
	bool             affiche_{ true };

	/// S�lection du noeud.
	bool             selectionne_{ false };
	/// L'�tampe du noeud
	bool			etampe_{false};

	/// Vrai si le noeud est s�lectionnable.
	bool             selectionnable_{ true };

	/// D�termine si l'objet peut �tre sauvegard� en XML.
	bool             enregistrable_{ true };

	/// Pointeur vers le parent.
	NoeudAbstrait*   parent_{ nullptr };

	/// Mod�le 3D correspondant � ce noeud.
	modele::Modele3D const* modele_;
	/// Storage pour le dessin du mod�le
	modele::opengl_storage::OpenGL_Liste const* liste_;

	///Bornes du rectangle de l'objet
	utilitaire::BoiteEnglobante bornes;

	///Retourne la norme de l'enfoncement entre deux cercles
	double obtenirEnfoncementCercle(glm::dvec3 pos1, double rayon1, glm::dvec3 pos2, double rayon2);
	///Retourne la norme de l'enfoncement entre un cercle et un segment
	double obtenirEnfoncementSegment(glm::dvec3 pos1, double rayon1, glm::dvec3 point1, glm::dvec3 point2);
};




////////////////////////////////////////////////////////////////////////
///
/// @fn inline NoeudAbstrait* NoeudAbstrait::obtenirParent()
///
/// Cette fonction retourne le pointeur vers le parent de ce noeud.
///
/// @return Le pointeur vers le parent.
///
////////////////////////////////////////////////////////////////////////
inline NoeudAbstrait* NoeudAbstrait::obtenirParent()
{
	return parent_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline const NoeudAbstrait* NoeudAbstrait::obtenirParent() const
///
/// Cette fonction retourne le pointeur constant vers le parent de ce noeud.
///
/// @return Le pointeur constant vers le parent.
///
////////////////////////////////////////////////////////////////////////
inline const NoeudAbstrait* NoeudAbstrait::obtenirParent() const
{
	return parent_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerParent( NoeudAbstrait* parent )
///
/// Cette fonction assigne le parent du noeud afin qu'il soit possible
/// de remonter dans l'arbre.
///
/// @param[in] parent : Le parent du noeud.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerParent(NoeudAbstrait* parent)
{
	parent_ = parent;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline const glm::dvec3& NoeudAbstrait::obtenirPositionRelative() const
///
/// Cette fonction retourne la position relative du noeud par rapport
/// � son parent.
///
/// @return La position relative.
///
////////////////////////////////////////////////////////////////////////
inline const glm::dvec3& NoeudAbstrait::obtenirPositionRelative() const
{
	return positionRelative_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline const glm::dvec3& NoeudAbstrait::obtenirAgrandissement() const
///
/// Cette fonction retourne un vecteur repr�sentant la valeur de la mise � l'�chelle sur chaque axe.
///
/// @return Le type du noeud.
///
////////////////////////////////////////////////////////////////////////
inline const glm::dvec3& NoeudAbstrait::obtenirAgrandissement() const
{
	return agrandissement_;
}




////////////////////////////////////////////////////////////////////////
///
/// @fn inline const std::string& NoeudAbstrait::obtenirType() const
///
/// Cette fonction retourne une cha�ne repr�sentante le type du noeud.
///
/// @return Le type du noeud.
///
////////////////////////////////////////////////////////////////////////
inline const std::string& NoeudAbstrait::obtenirType() const
{
	return type_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerAffiche( bool affiche )
///
/// Cette fonction permet d'assigner si le noeud se fait afficher ou
/// non lorsque l'arbre de rendu se fait afficher.  Elle permet donc
/// de temporairement suspendre ou activer l'affichage d'un noeud.
///
/// @param affiche : L'�tat affich� ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerAffiche(bool affiche)
{
	affiche_ = affiche;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estAffiche() const
///
/// Cette fonction retourne l'�tat que le noeud se fait afficher ou non.
///
/// @return L'�tat affich� ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estAffiche() const
{
	return affiche_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerSelection( bool selectionne )
///
/// Cette fonction permet d'assigner l'�tat d'�tre s�lectionn� ou non du noeud.
///
/// @param selectionne : L'�tat s�lectionn� ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerSelection(bool selectionne)
{
	// Un objet non s�lectionnable n'est jamais s�lectionn�.
	selectionne_ = (selectionne && selectionnable_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estSelectionne() const
///
/// Cette fonction retourne l'�tat d'�tre s�lectionn� ou non du noeud.
///
/// @return L'�tat s�lectionn� ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estSelectionne() const
{
	// Un objet non s�lectionnable n'est jamais s�lectionn�.
	return (selectionne_ && selectionnable_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerEtampe( bool etampe )
///
/// Cette fonction permet d'assigner l'�tat d'�tre etampe ou non du noeud.
///
/// @param selectionne : L'�tat s�lectionn� ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerEtampe(bool etampe)
{
	// Un objet non s�lectionnable n'est jamais s�lectionn�.
	etampe_ = etampe;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estEtampe() const
///
/// Cette fonction retourne l'�tat d'�tre etampe ou non du noeud.
///
/// @return L'�tat etampe ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estEtampe() const
{
	return etampe_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerEstSelectionnable( bool selectionnable )
///
/// Cette fonction permet d'assigner l'�tat d'�tre s�lectionnable ou non du noeud.
///
/// @param selectionnable : L'�tat s�lectionnable ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerEstSelectionnable(bool selectionnable)
{
	selectionnable_ = selectionnable;
	selectionne_ = selectionne_ && selectionnable_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estSelectionnable() const
///
/// Cette fonction retourne l'�tat d'�tre s�lectionnable ou non du noeud.
///
/// @return L'�tat s�lectionnable ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estSelectionnable() const
{
	return selectionnable_;
}


////////////////////////////////////////////////////////////////////////

///
/// @fn inline void NoeudAbstrait::assignerEstEnregistrable( bool enregistrable )
///
/// Cette fonction permet d'assigner l'�tat d'�tre enregistrable ou non du noeud.
///
/// @param enregistrable : L'�tat enregistrable ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerEstEnregistrable(bool enregistrable)
{
	enregistrable_ = enregistrable;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estEnregistrable() const
///
/// Cette fonction retourne l'�tat d'�tre enregistrable en XML ou non du
/// noeud.
///
/// @return L'�tat enregistrable ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estEnregistrable() const
{
	return enregistrable_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerObjetRendu(modele::Modele3D* modele, modele::opengl_storage::OpenGL_Liste* liste)
///
/// Cette fonction assigne l'objet de rendu au mod�le, c'est-�-dire son
/// mod�le 3D et sa liste d'affichage
///
/// @param modele : le mod�le 3D
/// @param liste : la liste d'affichage OpenGL
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerObjetRendu(modele::Modele3D const* modele, modele::opengl_storage::OpenGL_Liste const* liste)
{
	modele_ = modele;
	liste_ = liste;
	calculerBornes();

}

inline void NoeudAbstrait::executerCollision(NoeudBille* bille)
{

}

inline double NoeudAbstrait::obtenirEnfoncement(NoeudBille* bille)
{
	return 0;
}


#endif // __ARBRE_NOEUDS_NOEUDABSTRAIT_H__




///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
