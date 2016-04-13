///////////////////////////////////////////////////////////////////////////////
/// @file NoeudComposite.h
/// @author DGI-INF2990
/// @date 2007-01-25
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDCOMPOSITE_H__
#define __ARBRE_NOEUDS_NOEUDCOMPOSITE_H__


#include "NoeudAbstrait.h"

#include <vector>


///////////////////////////////////////////////////////////////////////////
/// @class NoeudComposite
/// @brief Implantation d'un noeud du patron composite qui peut posséder
///        des enfants.
///
///        Cette classe implante les différentes fonctions relatives aux
///        enfants, comme l'ajout, le retrait, la recherche, etc.
///
/// @author DGI-2990
/// @date 2007-01-24
///////////////////////////////////////////////////////////////////////////
class NoeudComposite : public NoeudAbstrait
{
public:
	/// Constructeur.
	NoeudComposite(
		const std::string& type = std::string{ "" }
      );
   /// Destructeur.
   virtual ~NoeudComposite();


   // Interface d'un noeud

   /// Calcule la profondeur de l'arbre sous le noeud courant.
   virtual unsigned int calculerProfondeur() const;

   /// Vide le noeud de ses enfants.
   virtual void vider();
   /// Efface le noeud passé en paramètre.
   virtual void effacer( const NoeudAbstrait* noeud );

   /// Accepter les visiteurs
   virtual void accepterVisiteur(VisiteurAbstrait*);

   /// Cherche un noeud par le type (sur un noeud constant).
   virtual const NoeudAbstrait* chercher( const std::string& typeNoeud ) const;
   /// Cherche un noeud par le type.
   virtual NoeudAbstrait* chercher( const std::string& typeNoeud );
   /// Cherche un noeud enfant selon l'indice (sur un noeud constant).
   virtual const NoeudAbstrait* chercher( unsigned int indice ) const;
   /// Cherche un noeud enfant selon l'indice.
   virtual NoeudAbstrait* chercher( unsigned int indice );

   /// Ajoute un noeud enfant.
   virtual bool ajouter( NoeudAbstrait* enfant );
   /// Obtient le nombre d'enfants du noeud.
   virtual unsigned int obtenirNombreEnfants() const;

   // Changer la sélection du noeud: on prend la version de la classe de
   // base.
   // virtual void inverserSelection();
   /// Efface les enfants sélectionnés.
   virtual void effacerSelection();
   /// Efface les enfants étampes
   virtual void effacerEtampes();
   /// Retire les étampes
   virtual void deEtamperTout();


   /// Sélectionne tous les enfants de même que le noeud.
   virtual void selectionnerTout();
   /// Désélectionne tous les enfants de même que le noeud.
   virtual void deselectionnerTout();
   /// Vérifier si le noeud ou un de ses enfants est sélectionné.
   virtual bool selectionExiste() const;

   /// Change le mode d'affichage des polygones.
   virtual void changerModePolygones( bool estForce );
   /// Assigne le mode d'affichage des polygones.
   virtual void assignerModePolygones( GLenum modePolygones );
   // Affiche le noeud: on prend la version de la classe de base.
   // virtual void afficher() const;
   /// Affiche le noeud de manière concrète.
   virtual void afficherConcret() const;
   /// Anime le noeud.
   virtual void animer( float dt );


protected:
   /// Le choix du conteneur pour les enfants.
	using conteneur_enfants = std::vector<NoeudAbstrait*>;
   /// La liste des enfants.
   conteneur_enfants enfants_;
   

private:
   /// Constructeur copie déclaré privé mais non défini pour éviter le
   /// constructeur copie généré par le compilateur.
   NoeudComposite(const NoeudComposite&);

};


#endif // __ARBRE_NOEUDS_NOEUDCOMPOSITE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
