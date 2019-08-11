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
    /// Interaction logic for ConferenceInformation.xaml
    /// </summary>
    public partial class ConferenceInformation : UserControl
    {
        ConferenceInfo info;
        Conference conf;

        public ConferenceInformation(ConferenceInfo info, Conference conf)
        {
            InitializeComponent();
            this.info = info;
            this.conf = conf;

            LoadSpeakers();
            LoadInfo();


        }

        private void LoadSpeakers()
        {
            RelationshipQuery relaQuery = new RelationshipQuery();
            List<Speaker> speakers = relaQuery.getSpeakers(conf.ID);
            conf.setSpeakers(speakers);
        }

        private void LoadInfo()
        {
            confName_TextBlock.Text = conf.Name;
            confTime_TextBlock.Text = String.Format("{0} - {1}", conf.From.ToString("H:mm"), conf.To.ToString("H:mm"));
            string date = conf.Date.ToString("dddd, d MMMM yyyy");
            confDate_TextBlock.Text = date.First().ToString().ToUpper() + date.Substring(1);
            confVenue_TextBlock.Text = conf.Venue;
            speakersCount_TextBlock.Text = String.Format("Liczba prelegentów: {0}", conf.getSpeakersCount());

            LoadLogo();

            foreach (Speaker s in conf.getSpeakers())
                speakers_ListView.Items.Add(s);
        }

        private void LoadLogo()
        {
            BitmapImage bitmap = new BitmapImage();
            bool load = true;
            try
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(System.AppDomain.CurrentDomain.BaseDirectory + conf.Logo);
                bitmap.EndInit();
                confLogo.Source = bitmap;
            }
            catch
            {
                load = false;
            }

            if (!load)
            {
                bitmap = null;
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/puste_logo.jpeg", UriKind.Absolute);
                bitmap.EndInit();
                confLogo.Source = bitmap;
            }


        }

        private void SpeakerEdit_Click(object sender, RoutedEventArgs e)
        {
            IconButton btn = sender as IconButton;
            if (btn != null)
            {
                Speaker conf = btn.DataContext as Speaker;
                if (conf != null)
                {
                    SpeakerEdit startPanel = new SpeakerEdit(info, conf);
                    info.infoWrap.Children.Clear();
                    info.infoWrap.Children.Add(startPanel);
                    Console.WriteLine("" + conf.Name);
                }


            }

        }

        private void SpeakerDelete_Click(object sender, RoutedEventArgs e)
        {
            IconButton btn = sender as IconButton;
            if (btn != null)
            {
                Speaker speaker = btn.DataContext as Speaker;
                if (speaker != null)
                {
                    SpeakerDelete startPanel = new SpeakerDelete(info, speaker);
                    info.infoWrap.Children.Clear();
                    info.infoWrap.Children.Add(startPanel);
                }


            }
        }

        private void newSpeaker_Click(object sender, RoutedEventArgs e)
        {
            SpeakerAdd startPanel = new SpeakerAdd(info);
            info.infoWrap.Children.Clear();
            info.infoWrap.Children.Add(startPanel);
        }

        private void startConf_Click(object sender, RoutedEventArgs e)
        {
            ConfWindow cw = new ConfWindow(conf.ID);
            cw.Show();
            info.main.Close();
        }
    }
}
