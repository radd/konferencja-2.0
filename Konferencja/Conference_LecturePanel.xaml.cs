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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Konferencja
{
    /// <summary>
    /// Interaction logic for Conference_LecturePanel.xaml
    /// </summary>
    public partial class Conference_LecturePanel : UserControl
    {
        ConfWindow main;
        Conference conf;
        DispatcherTimer timer;
        bool active;
        int nextIndex, duration, sound;
        enum Direction { FIRST, PREVIOUS, NEXT, INDEXOF }

        public Conference_LecturePanel(ConfWindow main, Conference conf)
        {
            InitializeComponent();
            this.main = main;
            this.conf = conf;

            LoadSpeakers();
            LoadInfo();
            loadTimer();
            nextSpeaker(Direction.FIRST);
        }

        private void LoadInfo()
        {
            LoadLogo();
            confName_TextBlock.Text = conf.Name;
            confInfo_TextBlock.Text = String.Format("{0}, {1}", conf.GetDate, conf.Venue);

            foreach (Speaker s in conf.getSpeakers())
                speakersListView.Items.Add(s);
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

        private void LoadSpeakers()
        {
            RelationshipQuery relaQuery = new RelationshipQuery();
            List<Speaker> speakers = relaQuery.getSpeakers(conf.ID);
            conf.setSpeakers(speakers);
        }

        private void loadTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 1);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (duration > 0)
            {
                duration--;
                progressBar.Value++;

                if (duration == 300 && sound == 0)
                    OpenSound();

            }
            else
            {
                timer.Stop();
                nextSpeaker(Direction.NEXT);
                ChangeActive(false);
            }

        }

        private void OpenSound()
        {
            sound++;
            MediaPlayer mp = new MediaPlayer();
            string path = String.Format("{0}sounds\\sound.mp3", System.AppDomain.CurrentDomain.BaseDirectory);
            mp.Open(new Uri(path));
            mp.Play();
        }


        private void nextSpeaker(Direction s)
        {
            if (s == Direction.NEXT)
                nextIndex++;
            else if (s == Direction.PREVIOUS)
                nextIndex--;
            else if (s == Direction.FIRST)
                nextIndex = 0;

            if (nextIndex < 0)
                nextIndex = 0;
            else if (nextIndex > conf.getSpeakersCount() - 1)
                nextIndex = conf.getSpeakersCount() - 1;
            else
                getSpeaker();

            ButtonChange();
        }

        private void ButtonChange()
        {
            if (conf.getSpeakersCount() == 1)
            {
                previous.Opacity = 0.6;
                next.Opacity = 0.6;
            }
            else if (nextIndex == 0)
                previous.Opacity = 0.6;
            else if (nextIndex == conf.getSpeakersCount() - 1)
                next.Opacity = 0.6;
            else
            {
                previous.Opacity = 1;
                next.Opacity = 1;
            }
        }



        private void getSpeaker()
        {
            Speaker speaker = conf.getSpeakers()[nextIndex];
            duration = getDuration(speaker.From, speaker.To);
            loadSpeakerInfo(speaker);
        }

        private int getDuration(DateTime from, DateTime to)
        {
            TimeSpan ts = to - from;
            return (int)ts.TotalSeconds;
        }


        private void loadSpeakerInfo(Speaker speaker)
        {
            speakerName.Text = speaker.Name;
            speakerTitle.Text = speaker.Title;

            TimeSpan ts = new TimeSpan(0, 0, duration);
            if (ts.Hours > 0)
            {
                from.Content = "0:00:00";
                to.Content = ts.ToString(@"h\:mm\:ss");
            }

            else
            {
                from.Content = "00:00";
                to.Content = ts.ToString(@"mm\:ss");
            }

            progressBar.Maximum = duration;
            progressBar.Value = 0;
            sound = 0;

        }

        private void startStop_Click(object sender, RoutedEventArgs e)
        {
            if (active)
            {
                timer.Stop();
                ChangeActive(false);
            }
            else
            {
                timer.Start();
                ChangeActive(true);
            }

        }

        private void ChangeActive(bool t)
        {
            if (t)
            {
                active = true;
                startStop.Content = "Stop";
            }
            else
            {
                active = false;
                startStop.Content = "Start";
            }


        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (!active)
            {
                timer.Stop();
                nextSpeaker(Direction.NEXT);
            }

        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (!active)
            {
                timer.Stop();
                nextSpeaker(Direction.PREVIOUS);
            }
        }




        private void endConf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow cw = new MainWindow();
            cw.Show();
            timer.Stop();
            timer = null;
            main.Close();
        }

        private void speakersListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Speaker speaker = ((ListViewItem)sender).Content as Speaker;
            if (speaker != null && !active)
            {
                nextIndex = conf.getSpeakers().IndexOf(speaker);
                nextSpeaker(Direction.INDEXOF);
            }
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



        private void sidebarAnim()
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.From = sidebar.Margin;
            anim.To = new Thickness(0, 0, 0, 0);
            anim.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            anim.Completed += (s, e) =>
            {
                open1 = 1;
            };
            sidebar.BeginAnimation(Grid.MarginProperty, anim);
        }


        private void sidebarAnim2(int ease)
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.From = sidebar.Margin;
            anim.To = new Thickness(-350, 0, 0, 0);
            if (ease == 1)
                anim.BeginTime = TimeSpan.FromSeconds(1);
            anim.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            anim.Completed += (s, e) =>
            {
                open1 = 0;
            };
            sidebar.BeginAnimation(Grid.MarginProperty, anim);
        }

        private void sidebar_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (down1 == 0)
            {
                sidebarAnim();
                down1 = 1;
            }
        }

        private void sidebar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (down1 == 1)
            {
                sidebarAnim2(open1);
                down1 = 0;
            }
        }

    }
}
