#ifndef __AFFICHAGE_H__
#define __AFFICHAGE_H__

#include <gl/glew.h>
#include "AideGL.h"
#include "BoiteEnvironnement.h"
#include "Vue/Vue.h"
#include "../Event/IEventSubscriber.h"
#include <vector>

class Affichage : public IEventSubscriber
{
public:
	Affichage();
	~Affichage();
	static Affichage* obtenirInstance();
private:
	static Affichage* instance_;

	/// Boite englobante
	utilitaire::BoiteEnvironnement * skyBox;
	/// Poignée ("handle") vers la fenêtre où l'affichage se fait.
	HWND  hWnd_{ nullptr };
	/// Poignée ("handle") vers le contexte OpenGL.
	HGLRC hGLRC_{ nullptr };
	/// Poignée ("handle") vers le "device context".
	HDC   hDC_{ nullptr };

	/// Vue courante de la scène.
	vue::Vue* vue_{ nullptr };

	GLuint progNuanceur_ = 0;


	//Lumieres en fonctions

	bool lAmbianteAllum_ = true;
	bool lDirectAllum_ = true;
	bool lSpotAllum_ = true;


	bool openGlGenere_ = false;

	const int NB_BILLES_MAX = 8; //Va etre 8 un jour, il va falloir modifier les nuanceurs pour que ca marche

public:
	void initAffichageFrame() const;
	void afficherLumieresGlobales() const;
	void afficherLumieresBillesEtButoirs(vector<glm::dvec3> posBilles, vector<glm::dvec3> posButoirs) const;
	void afficherSkyBox() const;
	void finaliserAffichageFrame() const;

	void changerCamera(bool orbite);

	void reinitLumiere();

	void intialiserOpenGL(HWND hwnd);
	void intialiserOpenGL();

	/// Libère le contexte OpenGL.
	void libererOpenGL();

	GLuint obtenirProgNuanceur();
	vue::Vue* obtenirVue();

	void convertirClotureAVirtuelle(int x, int y1, const math::Plan3D& plan, glm::dvec3& pos);
	void convertirClotureAVirtuelle(int x, int y, glm::dvec3& positionVirtuelle);

	void update(IEvent* e) override;
};


#endif // !__AFFICHAGE_H__
