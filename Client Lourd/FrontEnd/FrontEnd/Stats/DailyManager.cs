using System.Windows;
using FrontEnd.Player;
using FrontEndAccess.APIAccess;
using Models;

namespace FrontEnd.Stats
{
    public class DailyManager
    {
        public static void DailyInitialize()
        {
            if (Profile.Instance != null && !Profile.Instance.Daily.IsDone)
                MessageBox.Show("Le défis journalier est de : " + GetDailyDescription(Profile.Instance.Daily.DailyType));
        }

        public static void AchieveDaily(EnumsModel.DailyType type)
        {
            var daily = Profile.Instance?.Daily;
            if (daily != null && !daily.IsDone && daily.DailyType == type)
            {
                DailyAccess.Instance.CompleteDaily();
                daily.IsDone = true;
            }
        }

        public static string GetDailyDescription(EnumsModel.DailyType type)
        {
            var description = "";
            switch (type)
            {
                case EnumsModel.DailyType.AddFriend:
                    description = "Ajouter un nouvel Ami.";
                    break;
                case EnumsModel.DailyType.ChatOnce:
                    description = "Écrire dans le chat.";
                    break;
                case EnumsModel.DailyType.CreateCanal:
                    description = "Créer un nouveau canal de chat.";
                    break;
                case EnumsModel.DailyType.CreateMap:
                    description = "Créer une nouvele carte.";
                    break;
                case EnumsModel.DailyType.PlayCampain:
                    description = "Jouer en mode Campagne.";
                    break;
                case EnumsModel.DailyType.PlayFastGame:
                    description = "Jouer en mode partie rapide.";
                    break;
                case EnumsModel.DailyType.UpdateProfile:
                    description = "Mettre a jour votre profile.";
                    break;
            }
            return description;
        }
    }
}