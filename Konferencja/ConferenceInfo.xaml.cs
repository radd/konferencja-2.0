using Konferencja.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Konferencja
{
    /// <summary>
    /// Interaction logic for ConferenceInfo.xaml
    /// </summary>
    public partial class ConferenceInfo : UserControl
    {
        public MainWindow main;
        public Conference conf;

        public ConferenceInfo(MainWindow main, Conference conf)
        {
            InitializeComponent();
            this.main = main;
            this.conf = conf;

            ConferenceInformation startPanel = new ConferenceInformation(this, conf);
            infoWrap.Children.Clear();
            infoWrap.Children.Add(startPanel);

            LoadInfo();

        }

        private void LoadInfo()
        {
            confName_TextBlock.Text = conf.Name;
            confInfo_TextBlock.Text = String.Format("{0}, {1}", conf.GetDate, conf.Venue);

        }


        private void Undo_IconButton_Click(object sender, RoutedEventArgs e)
        {
            StartPanel startPanel = new StartPanel(main);

            TopBarAnimation topBarAnimation = new TopBarAnimation(main);
            topBarAnimation.Expand(startPanel);
        }

        private void ConfInfo_IconButton_Click(object sender, RoutedEventArgs e)
        {
            ConferenceInformation startPanel = new ConferenceInformation(this, conf);
            infoWrap.Children.Clear();
            infoWrap.Children.Add(startPanel);
        }

        private void ConfEdit_IconButton_Click(object sender, RoutedEventArgs e)
        {
            ConferenceEdit startPanel = new ConferenceEdit(this, conf);
            infoWrap.Children.Clear();
            infoWrap.Children.Add(startPanel);
        }

        private void ConfStart_IconButton_Click(object sender, RoutedEventArgs e)
        {
            ConfWindow cw = new ConfWindow(conf.ID);
            cw.Show();
            main.Close();
        }

        private void addConf_Click(object sender, RoutedEventArgs e)
        {
            ConferenceAdd panel = new ConferenceAdd(main);
            main.wrap.Children.Clear();
            main.wrap.Children.Add(panel);
        }

        private void confDelete_IconButton_Click(object sender, RoutedEventArgs e)
        {
            ConferenceDelete startPanel = new ConferenceDelete(this, conf);
            infoWrap.Children.Clear();
            infoWrap.Children.Add(startPanel);
        }
    }
}
