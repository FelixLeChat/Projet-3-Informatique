using System.Drawing;
using Models;

namespace FrontEnd.ProfileHelper
{
    public static class EnumToImage
    {
        public static Bitmap GetAvatarBitmap(EnumsModel.PrincessAvatar avatar)
        {
            switch (avatar)
            {
                case EnumsModel.PrincessAvatar.Default:
                    return Properties.Resources._default;
                case EnumsModel.PrincessAvatar.Ariel:
                    return Properties.Resources.ariel;
                case EnumsModel.PrincessAvatar.Belle:
                    return Properties.Resources.belle;
                case EnumsModel.PrincessAvatar.Cinder:
                    return Properties.Resources.cinder;
                case EnumsModel.PrincessAvatar.Frog:
                    return Properties.Resources.frog;
                case EnumsModel.PrincessAvatar.Mulan:
                    return Properties.Resources.mulan;
                case EnumsModel.PrincessAvatar.Poca:
                    return Properties.Resources.poca;
                case EnumsModel.PrincessAvatar.Ray:
                    return Properties.Resources.ray;
                case EnumsModel.PrincessAvatar.Sleep:
                    return Properties.Resources.sleep;
                case EnumsModel.PrincessAvatar.Snow:
                    return Properties.Resources.snow;
                case EnumsModel.PrincessAvatar.Jasmine:
                    return Properties.Resources.Jasmine;
                default:
                    return Properties.Resources.placeholder;
            }
        }

        public static Bitmap GetRankBitmap(EnumsModel.PrincessTitle title)
        {
            switch (title)
            {
                case EnumsModel.PrincessTitle.Lady:
                    return Properties.Resources.CrownLady;
                case EnumsModel.PrincessTitle.Duchess:
                    return Properties.Resources.CrownDuchess;
                case EnumsModel.PrincessTitle.Princess:
                    return Properties.Resources.CrownPrincess;
                case EnumsModel.PrincessTitle.Queen:
                    return Properties.Resources.CrownQueen;
                case EnumsModel.PrincessTitle.MASTER:
                    return Properties.Resources.MASTER;
                default:
                    return Properties.Resources.placeholder;
            }
        }
    }
}