using System.Collections.Generic;
using System.Drawing;
using Models;

namespace FrontEnd.UserControl.Tutorial
{
    public class TutorialHelper
    {
        public static void StartEditorTutorial()
        {
            var tutorialList = new List<Bitmap>()
            {
                Properties.Resources._1,
                Properties.Resources._2,
                Properties.Resources._3,
                Properties.Resources._4,
                Properties.Resources._5,
                Properties.Resources._6,
                Properties.Resources._7,
                Properties.Resources._8,
                Properties.Resources._9,
                Properties.Resources._10,
                Properties.Resources._11,
                Properties.Resources._12,
                Properties.Resources._13,
                Properties.Resources._14,
                Properties.Resources._15,
                Properties.Resources._16,
                Properties.Resources._17,
                Properties.Resources._18,
                Properties.Resources._19,
                Properties.Resources._20,
                Properties.Resources._21,
                Properties.Resources._22,
                Properties.Resources._23,
                Properties.Resources._24,
                Properties.Resources._25,
                Properties.Resources._26,
                Properties.Resources._27,
                Properties.Resources._28,
                Properties.Resources._29,
                Properties.Resources._30,
                Properties.Resources._31,
                Properties.Resources._32,
                Properties.Resources._33,
                Properties.Resources._34,
                Properties.Resources._35,
                Properties.Resources._36,
                Properties.Resources._37,
                Properties.Resources._38,
                Properties.Resources._39,
                Properties.Resources._40,
                Properties.Resources._41,
                Properties.Resources._42,
                Properties.Resources._43,
                Properties.Resources._44,
                Properties.Resources._45,
                Properties.Resources._45a,
                Properties.Resources._46,
                Properties.Resources._47,
                Properties.Resources._48,
                Properties.Resources._49,
                Properties.Resources._50
            };

            var win = new TutorialWindow(tutorialList, EnumsModel.TutorialType.Editor);
            win.ShowDialog();
        }


        public static void StartGameTutorial()
        {
            var tutorialList = new List<Bitmap>()
            {
                Properties.Resources._101,
                Properties.Resources._102,
                Properties.Resources._103,
                Properties.Resources._104,
                Properties.Resources._105,
                Properties.Resources._106,
                Properties.Resources._107,
                Properties.Resources._108,
                Properties.Resources._109,
                Properties.Resources._109a,
                Properties.Resources._110,
                Properties.Resources._111,
                Properties.Resources._112,
                Properties.Resources._113,
                Properties.Resources._114,
                Properties.Resources._115,
                Properties.Resources._116,
                Properties.Resources._117,
                Properties.Resources._118,
                Properties.Resources._119,
                Properties.Resources._120,
                Properties.Resources._121,
                Properties.Resources._122,
                Properties.Resources._123,
                Properties.Resources._124,
                Properties.Resources._125,
                Properties.Resources._126
            };

            var win = new TutorialWindow(tutorialList, EnumsModel.TutorialType.Game);
            win.ShowDialog();
        }
    }
}