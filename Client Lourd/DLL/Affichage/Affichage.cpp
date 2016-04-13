#include "Affichage.h"
#include <iostream>

#include "FreeImage.h"
#include "OpenGL_Nuanceur.h"
#include "VueOrtho.h"
#include "VueOrbite.h"
#include "glm/gtc/type_ptr.hpp"
#include "../Event/EventManager.h"
#include "../Event/KeyPressEvent.h"
#include "../Configuration/Config.h"
#include "Utilitaire.h"

Affichage* Affichage::instance_{ nullptr };

Affichage::Affichage()
{
	EventManager::GetInstance()->subscribe(this, INPUTEVENT);
}


Affichage::~Affichage()
{
	EventManager::GetInstance()->unsubscribe(this, INPUTEVENT);
}

Affichage* Affichage::obtenirInstance()
{
	if (instance_ == nullptr)
		instance_ = new Affichage();

	return instance_;
}

void Affichage::initAffichageFrame() const
{
	// Efface l'ancien rendu
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
	glUniform1i(glGetUniformLocation(progNuanceur_, "changerCouleur"), 0);

	// Positionne la caméra
	vue_->appliquerProjection();
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	vue_->appliquerCamera();
	afficherSkyBox();
	afficherLumieresGlobales();
}

void Affichage::afficherLumieresGlobales() const
{
	// Positionner la lumière.
	glm::vec4 position2{ 0, 0, 1, 0 };
	glLightfv(GL_LIGHT0, GL_POSITION, glm::value_ptr(position2));



	GLfloat lumiAmbiant2[] = { 0.2f, 0.2f, 0.2f, 1.0f };
	glLightfv(GL_LIGHT1, GL_AMBIENT, lumiAmbiant2);
	GLfloat lumiDiffuse[] = { 0.9f, 0.9f, 0.9f, 1.0f };
	glLightfv(GL_LIGHT1, GL_DIFFUSE, lumiDiffuse);
	GLfloat lumiSpecular[] = { 0.5f, 0.5f, 0.5f, 1.0f };
	glLightfv(GL_LIGHT1, GL_SPECULAR, lumiSpecular);
	GLfloat position[] = { 1.0f, 1.0f, 2.0f, 0.0f };
	glLightfv(GL_LIGHT1, GL_POSITION, position);

}

void Affichage::finaliserAffichageFrame() const
{
	glActiveTexture(GL_TEXTURE0);
	glUniform1i(glGetUniformLocation(progNuanceur_, "laTexture"), 0);

	// Échange les tampons pour que le résultat du rendu soit visible.
	SwapBuffers(hDC_);
}

void Affichage::afficherLumieresBillesEtButoirs(vector<glm::dvec3> posBilles, vector<glm::dvec3> posButoirs) const
{

	//spot billes
	GLfloat lumiDiffuse2[] = { 0.2f, 0.2f, 0.2f, 1.0f };
	glLightfv(GL_LIGHT2, GL_DIFFUSE, lumiDiffuse2);
	GLfloat lumiSpecular2[] = { 0.5f, 0.5f, 0.5f, 1.0f };
	glLightfv(GL_LIGHT2, GL_SPECULAR, lumiSpecular2);
	GLfloat spotDirection[] = { 0.0, -2.0, -5.0, 0.0 };
	glLightfv(GL_LIGHT2, GL_SPOT_DIRECTION, spotDirection);
	GLfloat cutoff = 30.0;
	glLightfv(GL_LIGHT2, GL_SPOT_CUTOFF, &cutoff);

	//spot butoirs
	GLfloat lumiDiffuse3[] = { 0.2f, 0.2f, 0.5f, 1.0f };
	glLightfv(GL_LIGHT3, GL_DIFFUSE, lumiDiffuse3);
	GLfloat lumiSpecular3[] = { 0.0f, 0.0f, 0.0f, 0.0f };
	glLightfv(GL_LIGHT3, GL_SPECULAR, lumiSpecular3);
	GLfloat spotDirection3[] = { 0.0f, 0.0f, -5.0f, 0.0f };
	glLightfv(GL_LIGHT3, GL_SPOT_DIRECTION, spotDirection3);
	GLfloat cutoff3 = 30.0;
	glLightfv(GL_LIGHT3, GL_SPOT_CUTOFF, &cutoff3);

	glUniform1i(glGetUniformLocation(progNuanceur_, "nbBillesEnJeu"), posBilles.size());
	for (int i = 0; i < NB_BILLES_MAX; i++)
	{
		string item = "sp[" + to_string(i) + "]";
		glUniform1i(glGetUniformLocation(progNuanceur_, item.c_str()), false);
	}

	auto transformMat = glm::fmat4();
	glGetFloatv(GL_MODELVIEW_MATRIX, glm::value_ptr(transformMat));
	if (lSpotAllum_ )
	{
		for (int i = 0; i <  posBilles.size(); i++)
		{
			glm::dvec3 pos = posBilles[i];
			glm::fvec4 position = glm::fvec4( float(pos.x), float(pos.y) + 8, float(pos.z) + 20.0f, 1.0f );
			string itemPosition = "spPos[" + to_string(i) + "]";
			auto transformedPos = transformMat * position;
			glUniform4f(glGetUniformLocation(progNuanceur_, itemPosition.c_str()), transformedPos.x, transformedPos.y, transformedPos.z, transformedPos.w);
		}
	}

	if(Config::obtenirInstance()->getForceRebond())
	{
		glUniform1i(glGetUniformLocation(progNuanceur_, "nbButoirsAllumes"), posButoirs.size());

		for (int i = 0; i < posButoirs.size(); i++)
		{
			glm::dvec3 pos = posButoirs[i];
			glm::fvec4 position = glm::fvec4(float(pos.x), float(pos.y), float(pos.z) + 20.0f, 1.0f);
			string itemPosition = "butPos[" + to_string(i) + "]";
			auto transformedPos = transformMat * position;
			glUniform4f(glGetUniformLocation(progNuanceur_, itemPosition.c_str()), transformedPos.x, transformedPos.y, transformedPos.z, transformedPos.w);
		}
	}
	else
	{
		glUniform1i(glGetUniformLocation(progNuanceur_, "nbButoirsAllumes"), 0);
	}
}

void Affichage::afficherSkyBox() const
{

	GLfloat color[] = { 1.0f, 1.0f, 1.0f, 1.0f };

	glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, color);
	glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, color);
	glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, color);
	glMaterialfv(GL_FRONT_AND_BACK, GL_POSITION, color);


	skyBox->afficher(glm::dvec3(0, 0, 0), 300);
}

void Affichage::reinitLumiere()
{
	lAmbianteAllum_ = true;
	lSpotAllum_ = true;
	lDirectAllum_ = true;
	glUniform1i(glGetUniformLocation(progNuanceur_, "lumiereAmbiante"), lAmbianteAllum_);
	glUniform1i(glGetUniformLocation(progNuanceur_, "lumiereDirectionnelle"), lDirectAllum_);
	glUniform1i(glGetUniformLocation(progNuanceur_, "spots"), lSpotAllum_);
}

void Affichage::changerCamera(bool orbite)
{
	if (orbite)
	{
		delete vue_;
		vue_ = vue_ = new vue::VueOrbite{
			vue::Camera{
				glm::dvec3(0, 0, 200), glm::dvec3(0, 0, 0),
				glm::dvec3(0, 1, 0), glm::dvec3(0, 1, 0),
				progNuanceur_ },
				vue::ProjectionOrbite{
					0, 668, 0, 651,
					1, 2000, 20, 150, 1.25,
					50
				}
		};
		vue_->deplacerXY(glm::dvec2(0, 0));
	}
	else
	{
		delete vue_;
		vue_ = new vue::VueOrtho{
			vue::Camera{
				glm::dvec3(0, 0, 200), glm::dvec3(0, 0, 0),
				glm::dvec3(0, 1, 0), glm::dvec3(0, 1, 0),
				progNuanceur_ },
				vue::ProjectionOrtho{
					0, 668, 0, 651,
					1, 1000, 20, 1000, 1.25,
					-100, 100, -100, 100 }
		};
	}
}

void Affichage::intialiserOpenGL(HWND hWnd)
{
	hWnd_ = hWnd;
	bool succes{ aidegl::creerContexteGL(hWnd_, hDC_, hGLRC_) };
	assert(succes && "Le contexte OpenGL n'a pu être créé.");

	// Initialisation des extensions de OpenGL
	glewInit();


	// FreeImage, utilisée par le chargeur, doit être initialisée
	FreeImage_Initialise();

	// La couleur de fond
	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

	// Nuanceurs
	const char *ns = "Nuanceurs\\nuanceurSommets.glsl";
	const char *nf = "Nuanceurs\\nuanceurFragments.glsl";

	progNuanceur_ = glCreateProgram();

	opengl::Nuanceur nuSommet = opengl::Nuanceur();
	opengl::Nuanceur nuFrag = opengl::Nuanceur();

	nuSommet.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, ns);
	nuFrag.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, nf);
	if (nuSommet.obtenirHandle() != 0)
	{
		glAttachShader(progNuanceur_, nuSommet.obtenirHandle());
	}
	if (nuFrag.obtenirHandle() != 0)
	{
		glAttachShader(progNuanceur_, nuFrag.obtenirHandle());
	}

	glLinkProgram(progNuanceur_);

	if (progNuanceur_ != 0)
		glUseProgram(progNuanceur_);

	glEnable(GL_NORMALIZE);

	// Les lumières
	glEnable(GL_LIGHTING);
	//glLightModelf(GL_LIGHT_MODEL_LOCAL_VIEWER, GL_TRUE);
	glLightModeli(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);
	glEnable(GL_COLOR_MATERIAL);
	/// Pour normaliser les normales dans le cas d'utilisation de glScale[fd]
	glEnable(GL_NORMALIZE);
	GLfloat lumiAmbiant[] = { 0.6f, 0.6f, 0.65f, 1.0f };
	glLightfv(GL_LIGHT0, GL_AMBIENT, lumiAmbiant);

	//spot billes
	GLfloat lumiDiffuse2[] = { 0.2f, 0.2f, 0.2f, 1.0f };
	glLightfv(GL_LIGHT2, GL_DIFFUSE, lumiDiffuse2);
	GLfloat lumiSpecular2[] = { 0.5f, 0.5f, 0.5f, 1.0f };
	glLightfv(GL_LIGHT2, GL_SPECULAR, lumiSpecular2);
	GLfloat spotDirection[] = { 0.0, -2.0, -5.0, 0.0 };
	glLightfv(GL_LIGHT2, GL_SPOT_DIRECTION, spotDirection);
	GLfloat cutoff = 30.0;
	glLightfv(GL_LIGHT2, GL_SPOT_CUTOFF, &cutoff);

	//spot butoirs
	GLfloat lumiDiffuse3[] = { 0.2f, 0.2f, 0.5f, 1.0f };
	glLightfv(GL_LIGHT3, GL_DIFFUSE, lumiDiffuse3);
	GLfloat lumiSpecular3[] = { 0.0f, 0.0f, 0.0f, 0.0f };
	glLightfv(GL_LIGHT3, GL_SPECULAR, lumiSpecular3);
	GLfloat spotDirection3[] = { 0.0, -0, -5.0, 0.0 };
	glLightfv(GL_LIGHT3, GL_SPOT_DIRECTION, spotDirection3);
	GLfloat cutoff3 = 30.0;
	glLightfv(GL_LIGHT3, GL_SPOT_CUTOFF, &cutoff3);

	glEnable(GL_LIGHT0);
	glEnable(GL_LIGHT1);

	glUniform1i(glGetUniformLocation(progNuanceur_, "lumiereAmbiante"), lAmbianteAllum_);
	glUniform1i(glGetUniformLocation(progNuanceur_, "lumiereDirectionnelle"), lDirectAllum_);
	glUniform1i(glGetUniformLocation(progNuanceur_, "spots"), lSpotAllum_);
	glUniform1i(glGetUniformLocation(progNuanceur_, "avecShader"), true);
	glUniform1i(glGetUniformLocation(progNuanceur_, "butoirAllume"), false);

	for (int i = 0; i < NB_BILLES_MAX; i++)
	{
		glUniform1i(glGetUniformLocation(progNuanceur_, ("sp" + to_string(i + 1)).c_str()), false);
	}

	// Qualité
	glShadeModel(GL_SMOOTH);
	glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);

	// Profondeur
	glEnable(GL_DEPTH_TEST);

	// Le cull face
	glEnable(GL_CULL_FACE);
	glCullFace(GL_BACK);

	// On crée une vue par défaut.
	vue_ = new vue::VueOrtho{
		vue::Camera{
			glm::dvec3(0, 0, 200), glm::dvec3(0, 0, 0),
			glm::dvec3(0, 1, 0), glm::dvec3(0, 1, 0),
			progNuanceur_ },
			vue::ProjectionOrtho{
				0, 1951,0, 1514,
				1, 1000, 20, 1000, 1.25,
				-100, 100, -100, 100 }
	};

	skyBox = new utilitaire::BoiteEnvironnement(
		"media/skybox/left.jpg",
		"media/skybox/right.jpg",
		"media/skybox/front.jpg",
		"media/skybox/back.jpg",
		"media/skybox/bottom.jpg",
		"media/skybox/top.jpg");

	openGlGenere_ = true;
}

void Affichage::intialiserOpenGL()
{
	glewInit();


	// FreeImage, utilisée par le chargeur, doit être initialisée
	FreeImage_Initialise();

	// La couleur de fond
	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

	// Les lumières
	glEnable(GL_LIGHTING);
	glLightModelf(GL_LIGHT_MODEL_LOCAL_VIEWER, GL_TRUE);
	glLightModeli(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);
	glEnable(GL_COLOR_MATERIAL);
	/// Pour normaliser les normales dans le cas d'utilisation de glScale[fd]
	glEnable(GL_NORMALIZE);
	//glEnable(GL_LIGHT0);

	// Qualité
	glShadeModel(GL_SMOOTH);
	glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);

	// Profondeur
	glEnable(GL_DEPTH_TEST);

	// Le cull face
	glEnable(GL_CULL_FACE);
	glCullFace(GL_BACK);

	// On crée une vue par défaut.
	vue_ = new vue::VueOrtho{
		vue::Camera{
			glm::dvec3(0, 0, 200), glm::dvec3(0, 0, 0),
			glm::dvec3(0, 1, 0), glm::dvec3(0, 1, 0),
			progNuanceur_ },
			vue::ProjectionOrtho{
				0, 668, 0, 651,
				1, 1000, 20, 1000, 1.25,
				-100, 100, -100, 100 }
	};
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	vue_->appliquerCamera();
	glm::vec4 position{ 0, 0, 1, 0 };
	glLightfv(GL_LIGHT0, GL_POSITION, glm::value_ptr(position));


}

void Affichage::libererOpenGL()
{
	if (hWnd_ != nullptr)
	{
		bool succes{ aidegl::detruireContexteGL(hWnd_, hDC_, hGLRC_) };
		assert(succes && "Le contexte OpenGL n'a pu être détruit.");
	}

	FreeImage_DeInitialise();
	openGlGenere_ = false;
}

GLuint Affichage::obtenirProgNuanceur()
{
	return progNuanceur_;
}

vue::Vue* Affichage::obtenirVue()
{
	return vue_;
}

void Affichage::convertirClotureAVirtuelle(int x, int y,const math::Plan3D&  plan,glm::dvec3& positionVirtuelle)
{
	vue_->convertirClotureAVirtuelle(x, y, plan, positionVirtuelle);
}


void Affichage::convertirClotureAVirtuelle(int x, int y, glm::dvec3& positionVirtuelle)
{
	vue_->convertirClotureAVirtuelle(x, y, positionVirtuelle);
}

void Affichage::update(IEvent* e)
{
	KeyPressEvent* keyEvent = (KeyPressEvent*)e;

	switch (keyEvent->getKeyCode())
	{
	case 'J':
		if (lAmbianteAllum_)
			glDisable(GL_LIGHT0);
		else
			glEnable(GL_LIGHT0);
		lAmbianteAllum_ = !lAmbianteAllum_;

		glUniform1i(glGetUniformLocation(progNuanceur_, "lumiereAmbiante"), lAmbianteAllum_);
		if (Config::obtenirInstance()->getDebog() && Config::obtenirInstance()->getEclairage())
		{
			utilitaire::timeStamp();
			cout << " - Lumiere ambiante ";
			if (lAmbianteAllum_)
				cout << "allumee\n";
			else
				cout << "eteinte\n";
		}
		break;
	case 'K':
		if (lDirectAllum_)
			glDisable(GL_LIGHT1);
		else
			glEnable(GL_LIGHT1);
		lDirectAllum_ = !lDirectAllum_;
		glUniform1i(glGetUniformLocation(progNuanceur_, "lumiereDirectionnelle"), lDirectAllum_);
		if (Config::obtenirInstance()->getDebog() && Config::obtenirInstance()->getEclairage())
		{
			utilitaire::timeStamp();
			cout << " - Lumiere directionnelle ";
			if (lDirectAllum_)
				cout << "allumee\n";
			else
				cout << "eteinte\n";
		}
		break;
	case 'L':
		if (lSpotAllum_)
		{
			glDisable(GL_LIGHT2);
		}
		else
		{
			glEnable(GL_LIGHT2);
		}
		lSpotAllum_ = !lSpotAllum_;
		glUniform1i(glGetUniformLocation(progNuanceur_, "spots"), lSpotAllum_);
		if (Config::obtenirInstance()->getDebog() && Config::obtenirInstance()->getEclairage())
		{
			utilitaire::timeStamp();
			cout << " - Lumieres spots ";
			if (lSpotAllum_)
				cout << "allumees\n";
			else
				cout << "eteintes\n";
		}
	default:
		break;
	}
}
