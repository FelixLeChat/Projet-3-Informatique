using FrontEnd.Game.Wrap;

namespace FrontEnd.Game.Config.Common
{
    public interface IGameConfig
    {
        /// <summary>
        /// Load default config
        /// </summary>
        void InitConfig();

        /// <summary>
        /// Change the windows to show the config
        /// </summary>
        void GotoConfigWindows();

        /// <summary>
        /// Check if all requirements are respected
        /// </summary>
        /// <returns></returns>
        bool Validate();

        /// <summary>
        /// Create the IGame object ready to lauch
        /// </summary>
        /// <returns></returns>
        IGame CreateGame();

        /// <summary>
        /// Hack
        /// </summary>
        void PreSetup();

        /// <summary>
        /// Setup the C++ side and the openGL (the windows need to be created)
        /// </summary>
        void Setup();
    }
}