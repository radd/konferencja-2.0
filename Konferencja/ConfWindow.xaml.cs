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
using System.Windows.Shapes;

namespace Konferencja
{
    /// <summary>
    /// Interaction logic for ConfWindow.xaml
    /// </summary>
    public partial class ConfWindow : Window
    {
        Conference conf;
        public ConfWindow(int confID)
        {
            InitializeComponent();

            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            conf = GetConference(confID);
            Conference_StartPanel startPanel = new Conference_StartPanel(this, conf);
           
            wrap.Children.Add(startPanel);
        }

        private Conference GetConference(int confID)
        {
            ConferenceQuery confQuery = new ConferenceQuery();
            return confQuery.getConference(confID);
        }

        int down1, down2, open1, open2;
        
        private void topBarAnim()
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.From = topBar.Margin;
            anim.To = new Thickness(0, 0, 0, 0);
            anim.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            anim.Completed += (s, e) =>
            {
                open1 = 1;
            };
            topBar.BeginAnimation(StackPanel.MarginProperty, anim);
        }

        private void topBarAnim2(int ease)
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.From = topBar.Margin;
            anim.To = new Thickness(0, -20, 0, 0);
            if (ease == 1)
                anim.BeginTime = TimeSpan.FromSeconds(1);
            anim.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            anim.Completed += (s, e) =>
            {
                open1 = 0;
            };
            topBar.BeginAnimation(StackPanel.MarginProperty, anim);
        }


        private void topBar_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (down1 == 0)
            {
                topBarAnim();
                down1 = 1;
            }
        }

        private void topBar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (down1 == 1)
            {
                topBarAnim2(open1);
                down1 = 0;
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void window_Click(object sender, RoutedEventArgs e)
        {
            if (WindowStyle == WindowStyle.None)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
            }

        }


    }
}
