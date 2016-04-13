using System.Windows;
using FrontEnd.WinformTest;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for TestOpengl.xaml
    /// </summary>
    public partial class TestOpengl
    {
        private Integrate _openGlPanel;

        public TestOpengl()
        {
            InitializeComponent();
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            //var fenetre = new fenetreOpenGL();
            // fenetre.Show();
            _openGlPanel = new Integrate();
            wfhSample.Child = _openGlPanel;

            wfhSample.Focus();
        }

        public void Run(double elapsedTime)
        {
            _openGlPanel?.UpdateWindow();
        }
    }
}
