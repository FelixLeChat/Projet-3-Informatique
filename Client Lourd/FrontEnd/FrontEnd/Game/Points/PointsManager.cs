using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Threading;
using FrontEnd.Achievements;
using FrontEnd.Stats;
using FrontEnd.UserControl.Winform;
using Application = System.Windows.Application;

namespace FrontEnd.Game.Points
{
    class PointsManager
    {
        public int  CurPlayerNum { get; set; }

        private int _ballsPlayer1;
        public int BallsPlayer1
        {
            get { return _ballsPlayer1; }
            set
            {
                _ballsPlayer1 = value;
                if (BallLabel1 != null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        BallLabel1.Text = _ballsPlayer1.ToString();
                    }));
            }
        }

        private int _ballsPlayer2;
        public int BallsPlayer2
        {
            get { return _ballsPlayer2; }
            set
            {
                _ballsPlayer2 = value;
                if (BallLabel2 != null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        BallLabel2.Text = _ballsPlayer2.ToString();
                    }));
            }
        }

        private int _ballsPlayer3;
        public int BallsPlayer3
        {
            get { return _ballsPlayer3; }
            set
            {
                _ballsPlayer3 = value;
                if (BallLabel3 != null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        BallLabel3.Text = _ballsPlayer3.ToString();
                    }));
            }
        }

        private int _ballsPlayer4;
        public int BallsPlayer4
        {
            get { return _ballsPlayer1; }
            set
            {
                _ballsPlayer4 = value;
                if (BallLabel4 != null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        BallLabel4.Text = _ballsPlayer4.ToString();
                    }));
            }
        }

        private int _pointPlayer1;
        public int PointPlayer1
        {
            get { return _pointPlayer1; }
            set
            {
                _pointPlayer1 = value;
                if(PointJoueur1Label != null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        PointJoueur1Label.Text =  _pointPlayer1.ToString();
                    }));
            }
        }

        private int _pointPlayer2;
        public int PointPlayer2
        {
            get { return _pointPlayer2; }
            set
            {
                _pointPlayer2 = value;
                if(PointJoueur2Label!=null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        PointJoueur2Label.Text = _pointPlayer2.ToString();
                    }));
            }
        }

        private int _pointPlayer3;
        public int PointPlayer3
        {
            get { return _pointPlayer3; }
            set
            {
                _pointPlayer3 = value;
                if(PointJoueur3Label !=null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        PointJoueur3Label.Text = _pointPlayer3.ToString();
                    }));
            }
        }

        private int _pointPlayer4;
        public int PointPlayer4
        {
            get { return _pointPlayer4; }
            set
            {
                _pointPlayer4 = value;
                if (PointJoueur4Label != null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        PointJoueur4Label.Text = _pointPlayer4.ToString();
                    }));
            }
        }
        public Label PointJoueur1Label { get; set; }
        public Label PointJoueur2Label { get; set; }
        public Label PointJoueur3Label { get; set; }
        public Label PointJoueur4Label { get; set; }


        public Label BallLabel1 { get; set; }
        public Label BallLabel2 { get; set; }
        public Label BallLabel3 { get; set; }
        public Label BallLabel4 { get; set; }

        private IntegratedOpenGl.Mode _gameMode;
        private bool _isOnline;
        private bool _isFriendZone;
        private readonly Stopwatch Timer = new Stopwatch();

        public PointsManager(int playerPos)
        {
            PointPlayer1 = 0;
            PointPlayer2 = 0;
            PointPlayer3 = 0;
            PointPlayer4 = 0;
            CurPlayerNum = playerPos;
        }

        /// <summary>
        /// Need to be called only on the point manager of the player's points
        /// </summary>
        public void StartGame(IntegratedOpenGl.Mode mode, bool isOnline = false, bool isFriendZone = false, bool isCompe = false, int numbJoueurs = 0)
        {
            _gameMode = mode;
            _isOnline = isOnline;
            _isFriendZone = isFriendZone;
            Timer.Start();
            // Stats
            StatsManager.AddGamePlayed();

            if(isOnline)
                AchievementManager.AchieveFirstOnlineGame();
            if(_isFriendZone)
                AchievementManager.AchievePlayFriendMap();

            // Label visibility
            if (isCompe)
            {
                PointJoueur1Label.Visible = true;
                BallLabel1.Visible = true;

                if (numbJoueurs > 1)
                {
                    PointJoueur2Label.Visible = true;
                    BallLabel2.Visible = true;
                }
                if (numbJoueurs > 2)
                {
                    PointJoueur3Label.Visible = true;
                    BallLabel3.Visible = true;
                }
                if (numbJoueurs > 3)
                {
                    PointJoueur4Label.Visible = true;
                    BallLabel4.Visible = true;
                }
            }
            else
            {
                if (PointJoueur2Label != null)
                    PointJoueur2Label.Visible = false;
                if (PointJoueur3Label != null)
                    PointJoueur3Label.Visible = false;
                if (PointJoueur4Label != null)
                    PointJoueur4Label.Visible = false;


                if (PointJoueur1Label != null)
                    PointJoueur1Label.Visible = true;


                if (BallLabel2 != null)
                    BallLabel2.Visible = false;
                if (BallLabel3 != null)
                    BallLabel3.Visible = false;
                if (BallLabel4 != null)
                    BallLabel4.Visible = false;


                if (BallLabel1 != null)
                    BallLabel1.Visible = true;
            }

        }

        public void Endgame(bool isWin = false)
        {
            Timer.Stop();
            var myPoints = GetMyPoints();

            // Stats
            StatsManager.AddGameTime((int)(Timer.ElapsedMilliseconds/1000));
            StatsManager.AddGamePoints(myPoints);
            if(isWin)
                StatsManager.AddGameWon();

            // Achievement
            if (myPoints >= 10000)
            {
                AchievementManager.Achieve10000GamePoints();

                if(_gameMode == IntegratedOpenGl.Mode.ModePartieRapide)
                    AchievementManager.Achieve10000FastGamePoints();
            }
            if (isWin && _isOnline)
                AchievementManager.AchieveFirstOnlineGameWon();
        }

        public int GetMyPoints()
        {
            int myPoints;
            switch (CurPlayerNum)
            {
                case 0:
                    myPoints = PointPlayer1;
                    break;
                case 1:
                    myPoints = PointPlayer2;
                    break;
                case 2:
                    myPoints = PointPlayer3;
                    break;
                case 3:
                    myPoints = PointPlayer4;
                    break;
                default:
                    myPoints = 0;
                    break;
            }

            return myPoints;
        }
    }
}
