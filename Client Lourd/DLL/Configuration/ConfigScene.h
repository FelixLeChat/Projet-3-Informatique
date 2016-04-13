//////////////////////////////////////////////////////////////////////////////
/// @file ConfigScene.h
/// @author Jean-François Pérusse
/// @date 2007-01-10
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __CONFIGURATION_CONFIGSCENE_H__
#define __CONFIGURATION_CONFIGSCENE_H__


#include "Singleton.h"
#include "tinyxml2.h"

///////////////////////////////////////////////////////////////////////////
/// @class ConfigScene
/// @brief Les variables de configuration de la classe CScene.
///        C'est une classe singleton.
///
/// @author Jean-François Pérusse
/// @date 2007-01-10
///////////////////////////////////////////////////////////////////////////
class ConfigScene : public Singleton<ConfigScene>
{
   SINGLETON_DECLARATION_CLASSE(ConfigScene);

public:
	

   /// Créer le DOM avec les valeurs.
	void creerDOM(tinyxml2::XMLDocument& document) const;

   /// Lire les valeurs du DOM.
	void lireDOM(tinyxml2::XMLDocument const& document);

   /// Nombre de calculs par image.
   static int CALCULS_PAR_IMAGE;

};


#endif // __CONFIGURATION_CONFIGSCENE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
