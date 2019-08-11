using Konferencja.Class;
using Konferencja.Query;
using Microsoft.Win32;
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
    /// Interaction logic for ConferenceAdd.xaml
    /// </summary>
    public partial class ConferenceAdd : UserControl
    {
        MainWindow main;
        public ConferenceAdd(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            NewConference();
        }

        private void NewConference()
        {
            string output = "";
            try
            {
                PrepareData();
                AddConference();
                saveButton.IsEnabled = false;
                output = "Dodano";
                OpenPositiveMessage(output);
            }
            catch (Exception e)
            {
                output = e.Message;
                OpenNegativeMessage(output);
            }

        }

        private void PrepareData()
        {
            if (confName.Text == "")
                throw new Exception("Pole jest wymagane");

            if (datePicker.Text == "")
                throw new Exception("Podaj date");

            if (confVenue.Text == "")
                throw new Exception("Pole jest wymagane");

        }

        private void AddConference()
        {
            ConferenceQuery confQuery = new ConferenceQuery();
            int confID = confQuery.addConference(confName.Text, datePicker.SelectedDate.Value.Date, timePickerStart.getTime(), timePickerEnd.getTime(), confVenue.Text, confLogo.Text);

            if (confLogo.Text != "")
            {
                string newLogoPath = String.Format("{0}{1}conf_{2}_logo{3}", System.AppDomain.CurrentDomain.BaseDirectory, Database.IMGPATH, confID, System.IO.Path.GetExtension(confLogo.Text));
                string newLogoName = String.Format("{0}conf_{1}_logo{2}", Database.IMGPATH, confID, System.IO.Path.GetExtension(confLogo.Text));

                System.IO.File.Copy(confLogo.Text, newLogoPath, true);

                Conference conf = confQuery.getConference(confID);
                conf.Logo = newLogoName;
                confQuery.setConference(conf);
            }


        }

        public void OpenPositiveMessage(string output)
        {
            MessagePanel messagePanel = new MessagePanel();
            messagePanel.messageText.Text = output;
            messagePanel.button.Click += new RoutedEventHandler(PositiveButton_Click);
            messageWrap.Children.Add(messagePanel);
            messageWrap.Visibility = Visibility.Visible;

        }

        public void PositiveButton_Click(object sender, RoutedEventArgs e)
        {
            messageWrap.Visibility = Visibility.Collapsed;
            messageWrap.Children.Clear();

            StartPanel panel = new StartPanel(main);
            TopBarAnimation topBarAnimation = new TopBarAnimation(main);
            topBarAnimation.Expand(panel);
        }

        public void OpenNegativeMessage(string output)
        {
            MessagePanel messagePanel = new MessagePanel();
            messagePanel.messageText.Text = output;
            messagePanel.button.Click += new RoutedEventHandler(NegativeButton_Click);
            messageWrap.Children.Add(messagePanel);
            messageWrap.Visibility = Visibility.Visible;

        }

        public void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            messageWrap.Visibility = Visibility.Collapsed;
            messageWrap.Children.Clear();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            StartPanel panel = new StartPanel(main);
            TopBarAnimation topBarAnimation = new TopBarAnimation(main);
            topBarAnimation.Expand(panel);
        }

        private void Undo_IconButton_Click(object sender, RoutedEventArgs e)
        {
            StartPanel startPanel = new StartPanel(main);
            TopBarAnimation topBarAnimation = new TopBarAnimation(main);
            topBarAnimation.Expand(startPanel);
        }

        private void browseLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = confLogo.Text != "" ? System.IO.Path.GetDirectoryName(confLogo.Text) : "C:\\";
            dlg.FileName = confLogo.Text;
            dlg.Filter = "Image files .jpg .jpeg .png|*.jpg; *.jpeg; *.png|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                confLogo.Text = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                confLogoImage.Source = bitmap;
            }
        }


    }
}
