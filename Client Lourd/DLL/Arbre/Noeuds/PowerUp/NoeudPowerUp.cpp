#include "NoeudPowerUp.h"
#include "NoeudPlateauDArgent.h"
#include "Sons\ClasseSons.h"

NoeudPowerUp::NoeudPowerUp(const std::string & typeNoeud)
	: NoeudAbstrait{ typeNoeud }
{
	rayon_ = 0;
	estObtenu_ = false;
	active_ = false;
	duree_ = 10;
}

NoeudPowerUp::~NoeudPowerUp()
{
}

void NoeudPowerUp::accepterVisiteur(VisiteurAbstrait* )
{}

void NoeudPowerUp::activer()
{
	active_ = true;
}

void NoeudPowerUp::desactiver()
{
	active_ = false;
}

void NoeudPowerUp::afficherConcret() const
{
	if (!estObtenu_ && active_)
	{
		// Sauvegarde de la matrice.
		glPushMatrix();

		// Affichage du modèle.
		liste_->dessiner();

		// Restauration de la matrice.
		glPopMatrix();
	}
}

void NoeudPowerUp::animer(float dt)
{
	if (estObtenu_)
	{
		tempsDepuisObtention_ += dt;
		if (tempsDepuisObtention_ >= duree_)
		{
			retirerPowerUp();
			active_ = false;
			estObtenu_ = false;
		}
	}
}

void NoeudPowerUp::executerCollision(NoeudBille * bille)
{
	if(active_ && !estObtenu_ && bille->estLocale())
		collision(bille);
}

double NoeudPowerUp::obtenirEnfoncement(NoeudBille * bille)
{
	if (active_ && !estObtenu_)
		return obtenirEnfoncementCercle(bille->obtenirPositionRelative(), bille->obtenirRayon(), parent_->obtenirPositionRelative(), obtenirRayon());
	return 0;
}

void NoeudPowerUp::afficherSelectionne() const
{
}

double NoeudPowerUp::obtenirRayon()
{
	if (rayon_ == 0)
		rayon_ = utilitaire::calculerSphereEnglobante(*modele_).rayon*agrandissement_.x;
	return rayon_;
}

void NoeudPowerUp::collision(NoeudBille* bille)
{
	double distance = glm::distance(parent_->obtenirPositionRelative(), bille->obtenirPositionRelative());
	double distanceMin = obtenirRayon() + bille->obtenirRayon();
	if (abs(distance) < distanceMin)
	{
		ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_POWER_UP, false);
		appliquerPowerUp(bille);
		((NoeudPlateauDArgent*)parent_)->donnerPowerUp();
		estObtenu_ = true;
		tempsDepuisObtention_ = 0;
	}
}
