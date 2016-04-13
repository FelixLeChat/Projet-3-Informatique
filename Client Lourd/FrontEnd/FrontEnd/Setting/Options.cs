using System.Threading;
using System.Threading.Tasks;
using FrontEnd.Achievements;
using FrontEnd.Core;

namespace FrontEnd.Setting
{
    public class Options
    {
        private static Options _instance;
        public static Options Instance => _instance ?? (_instance = new Options());

        public bool IsLoading { get; private set; } = true;

        private Options()
        {
            var task = new Task(() =>
            {
                IsLoading = true;
                NativeFunction.creerConfig();
                GetCurrentConfigs();
                IsLoading = false;

                AchievementManager.AchieveMusicLoad();
            });
            task.Start();
        }

        // Setting Properties
        public int RightJ1 { get; set; }
        public int LeftJ1 { get; set; }
        public int RightJ2 { get; set; }
        public int LeftJ2 { get; set; }
        public int SpringKey { get; set; }
        public int BallCount { get; set; }
        public bool DoubleBallMode { get; set; }
        public bool IncrReboundForce { get; set; }
        public bool ShowDebug { get; set; }
        public bool ShowBallGeneration { get; set; }
        public bool ShowCollisionSpeed { get; set; }
        public bool ShowLighting { get; set; }
        public bool ShowPortalAttraction { get; set; }

        public void DefaultConfigs()
        {
            NativeFunction.toucheDefaut();
            GetCurrentConfigs();
        }

        public void UpdateConfigs()
        {
            NativeFunction.mettreAJourConfiguration(
                LeftJ1, RightJ1, 
                LeftJ2, RightJ2,
                SpringKey, 
                BallCount, 
                DoubleBallMode, 
                IncrReboundForce, 
                ShowDebug, 
                ShowBallGeneration, 
                ShowCollisionSpeed, 
                ShowLighting, 
                ShowPortalAttraction);
        }

        public void GetCurrentConfigs(bool def = false)
        {

            // Control
            RightJ1 = NativeFunction.getPd1();
            RightJ2 = NativeFunction.getPd2();
            LeftJ1 = NativeFunction.getPg1();
            LeftJ2 = NativeFunction.getPg2();
            SpringKey = NativeFunction.getRes();

            // Game
            BallCount = NativeFunction.getNbBilles();
            DoubleBallMode = NativeFunction.getMode2Billes();

            // Debug
            IncrReboundForce = NativeFunction.getForceRebond();
            ShowDebug = NativeFunction.getDebog();
            ShowBallGeneration = NativeFunction.getGenBille();
            ShowCollisionSpeed = NativeFunction.getVitBilles();
            ShowLighting = NativeFunction.getEclairage();
            ShowPortalAttraction = NativeFunction.getLimitesPortails();


            // check for first time
            /*if (BallCount < 0 && !def)
            {
                NativeFunction.toucheDefaut();
                GetCurrentConfigs(true);
            }*/
        }

    }
}
