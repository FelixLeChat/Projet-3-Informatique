#include "NoeudPlateauDArgent.h"

#include "Utilitaire.h"

#include <GL/gl.h>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include "../../../Event/PowerUpSyncEvent.h"
#include "../../../Reseau/NetworkManager.h"
#include "../../../Event/EventManager.h"

//TODO: Faire un modèle pour le plateau et chaque power-up (balle glowy avec des couleurs)
//TODO: Faire un bouton pour ajouter des plateaux

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTrone::NoeudTrone(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPlateauDArgent::NoeudPlateauDArgent(const string& typeNoeud)
	: NoeudComposite{ typeNoeud }
{
	intervalle_ = 15;
	tempsDepuisPowerUpObtenu_ = 0;
	powerUpActif_ = PowerUpType::NB_TYPES;

	powerUpDisponible_ = false;
	if(!NetworkManager::getInstance()->isHost())
	{
		EventManager::GetInstance()->subscribe(this, POWERUPSYNC);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTrone::~NoeudTrone()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPlateauDArgent::~NoeudPlateauDArgent()
{
	EventManager::GetInstance()->unsubscribe(this, POWERUPSYNC);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTrone::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPlateauDArgent::afficherConcret() const
{
	NoeudComposite::afficherConcret();
	// Sauvegarde de la matrice.
	glPushMatrix();

	// Affichage du modèle.
	liste_->dessiner();

	// Restauration de la matrice.
	glPopMatrix();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCube::animer(float temps)
///
/// Cette fonction effectue l'animation du noeud pour un certain
/// intervalle de temps.
///
/// @param[in] temps : Intervalle de temps sur lequel faire l'animation.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPlateauDArgent::animer(float temps)
{
	NoeudComposite::animer(temps);
	if (NetworkManager::getInstance()->isHost())
	{
		if (!powerUpDisponible_)
		{
			tempsDepuisPowerUpObtenu_ += temps;
			if (tempsDepuisPowerUpObtenu_ >= intervalle_)
			{
				creerPowerUp();
			}
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTrone::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPlateauDArgent::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudPlateauDArgent(this);
}


void NoeudPlateauDArgent::activerPowerUp(PowerUpType choixPowerUp)
{
	powerUpActif_ = choixPowerUp;

	switch (choixPowerUp)
	{
	case PowerUpType::DOUBLE_TAILE_BILLE:
		((NoeudPowerUp*)chercher("billePowerup"))->activer();
		break;
	case PowerUpType::DOUBLE_TAILLE_PALETTES:
		((NoeudPowerUp*)chercher("palettePowerup"))->activer();
		break;
	case PowerUpType::DOUBLE_PTS:
		((NoeudPowerUp*)chercher("doublePowerup"))->activer();
		break;
	case PowerUpType::MOITIE_PTS:
		((NoeudPowerUp*)chercher("demiPowerup"))->activer();
		break;
	default:
		cout << "Panic" << endl;
		break;
	}
}

void NoeudPlateauDArgent::creerPowerUp()
{
	powerUpDisponible_ = true;
	PowerUpType choixPowerUp = PowerUpType(rand() % PowerUpType::NB_TYPES);

	activerPowerUp(choixPowerUp);
	NetworkManager::getInstance()->SyncPowerUp(positionRelative_.x, positionRelative_.y, choixPowerUp);
}

void NoeudPlateauDArgent::donnerPowerUp()
{
	powerUpDisponible_ = false;
	tempsDepuisPowerUpObtenu_ = 0;
	powerUpActif_ = PowerUpType::NB_TYPES;

	if(NetworkManager::getInstance()->isHost())
		NetworkManager::getInstance()->SyncPowerUp(positionRelative_.x, positionRelative_.y, PowerUpType::NB_TYPES);
}

void NoeudPlateauDArgent::update(IEvent* e)
{
	if(e->getType() == POWERUPSYNC)
	{
		auto sync = static_cast<PowerUpSyncEvent *>(e);
		if (sync->pos_x() == positionRelative_.x && sync->pos_y() == positionRelative_.y)
		{
			if(powerUpDisponible_ == false && sync->power_up()!= PowerUpType::NB_TYPES)
				activerPowerUp(sync->power_up());
			else
			{
				powerUpDisponible_ = false;
				powerUpActif_ = PowerUpType::NB_TYPES;
				tempsDepuisPowerUpObtenu_ = 0;
				((NoeudPowerUp*)chercher("billePowerup"))->desactiver();
				((NoeudPowerUp*)chercher("palettePowerup"))->desactiver();
				((NoeudPowerUp*)chercher("doublePowerup"))->desactiver();
				((NoeudPowerUp*)chercher("demiPowerup"))->desactiver();
			}
		}
	}
}

void NoeudPlateauDArgent::reinitialiser()
{
	intervalle_ = 15;
	tempsDepuisPowerUpObtenu_ = 0;
	powerUpDisponible_ = false;

	for (int i = 0; i < obtenirNombreEnfants(); i++)
	{
		((NoeudPowerUp*)chercher(i))->desactiver();
	}
}

PowerUpType NoeudPlateauDArgent::obtenirTypePowerUp() const
{
	return powerUpActif_;
}
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
