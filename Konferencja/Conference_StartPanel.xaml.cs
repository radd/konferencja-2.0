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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Konferencja
{
    /// <summary>
    /// Interaction logic for Conference_StartPanel.xaml
    /// </summary>
    public partial class Conference_StartPanel : UserControl
    {
        ConfWindow main;
        Conference conf;
        public Conference_StartPanel(ConfWindow main, Conference conf)
        {
            InitializeComponent();
            this.main = main;
            this.conf = conf;

            LoadInfo();
        }

        private void LoadInfo()
        {
            LoadLogo();
            confName.Text = conf.Name;
            string date = conf.Date.ToString("dddd, d MMMM yyyy");
            confDate.Text = date.First().ToString().ToUpper() + date.Substring(1);
            confTime.Text = String.Format("{0} - {1}", conf.From.ToString("H:mm"), conf.To.ToString("H:mm"));
            confVenue.Text = conf.Venue;
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
                logoPanel.Visibility = Visibility.Collapsed;


        }


        private void speakerList_Click(object sender, RoutedEventArgs e)
        {
            Conference_SpeakerListPanel startPanel = new Conference_SpeakerListPanel(main, conf);
            //Conference_LecturePanel startPanel = new Conference_LecturePanel(this, null);
            main.wrap.Children.Clear();
            main.wrap.Children.Add(startPanel);
        }

        private void startConf_Click(object sender, RoutedEventArgs e)
        {
            Conference_LecturePanel startPanel = new Conference_LecturePanel(main, conf);
            main.wrap.Children.Clear();
            main.wrap.Children.Add(startPanel);
        }




        int down1, down2, open1, open2;
        private void footerAnim()
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.From = footer.Margin;
            anim.To = new Thickness(0, 0, 0, 0);
            anim.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            anim.Completed += (s, e) =>
            {
                open2 = 1;
            };
            footer.BeginAnimation(StackPanel.MarginProperty, anim);
        }


        private void footerAnim2(int ease)
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.From = footer.Margin;
            anim.To = new Thickness(0, 0, 0, -60);
            if (ease == 1)
                anim.BeginTime = TimeSpan.FromSeconds(1);
            anim.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            anim.Completed += (s, e) =>
            {
                open2 = 0;
            };
            footer.BeginAnimation(StackPanel.MarginProperty, anim);
        }

        private void footer_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (down2 == 0)
            {
                footerAnim();
                down2 = 1;
            }
        }

        private void footer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (down2 == 1)
            {
                footerAnim2(open2);
                down2 = 0;
            }
        }

    }
}
