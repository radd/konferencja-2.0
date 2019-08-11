using Konferencja.Class;
using Konferencja.Query;
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
    /// Interaction logic for StartPanel.xaml
    /// </summary>
    public partial class StartPanel : UserControl
    {
        MainWindow main;
        public StartPanel(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            LoadConferences();
        }

        private void LoadConferences()
        {
            ConferenceQuery confQuery = new ConferenceQuery();
            List<Conference> confs = confQuery.getConferences();
            foreach (Conference c in confs)
                confsListView.Items.Add(c);
        }

        private void confsListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Conference conf = ((ListViewItem)sender).Content as Conference;
            if (conf != null)
            {
                ConferenceInfo panel = new ConferenceInfo(main, conf);
                TopBarAnimation topBarAnimation = new TopBarAnimation(main);
                topBarAnimation.Collapse(panel);
            }
        }


        private void addConf_Click(object sender, RoutedEventArgs e)
        {
            ConferenceAdd startPanel = new ConferenceAdd(main);
            TopBarAnimation topBarAnimation = new TopBarAnimation(main);
            topBarAnimation.Collapse(startPanel);
        }
    }
}
