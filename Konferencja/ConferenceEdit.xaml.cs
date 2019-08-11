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
    /// Interaction logic for ConferenceEdit.xaml
    /// </summary>
    public partial class ConferenceEdit : UserControl
    {
        ConferenceInfo info;
        Conference conf;

        public ConferenceEdit(ConferenceInfo info, Conference conf)
        {
            InitializeComponent();
            this.info = info;
            this.conf = conf;

            LoadConferenceInfo();
        }

        private void LoadConferenceInfo()
        {
            confName.Text = conf.Name;
            datePicker.SelectedDate = conf.Date;
            timePickerStart.setTime(conf.From);
            timePickerEnd.setTime(conf.To);
            confVenue.Text = conf.Venue;
            confLogo.Text = System.AppDomain.CurrentDomain.BaseDirectory + conf.Logo;
            LoadLogo();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            string output = "";
            try
            {
                PrepareData();
                EditConference();
                saveButton.IsEnabled = false;
                output = "Zapisano";
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
            if (confName.Text != "")
                conf.Name = confName.Text;
            else
                throw new Exception("Pole jest wymagane");

            if (datePicker.Text != "")
                conf.Date = datePicker.SelectedDate.Value.Date;

            conf.From = timePickerStart.getTime();
            conf.To = timePickerEnd.getTime();

            if (confVenue.Text != "")
                conf.Venue = confVenue.Text;
            else
                throw new Exception("Pole jest wymagane");

            if (confLogo.Text != "" && conf.Logo != confLogo.Text)
            {
                string newLogoPath = String.Format("{0}{1}conf_{2}_logo{3}", System.AppDomain.CurrentDomain.BaseDirectory, Database.IMGPATH, conf.ID, System.IO.Path.GetExtension(confLogo.Text));
                string newLogoName = String.Format("{0}conf_{1}_logo{2}", Database.IMGPATH, conf.ID, System.IO.Path.GetExtension(confLogo.Text));

                System.IO.File.Copy(confLogo.Text, newLogoPath, true);
                conf.Logo = newLogoName;
            }
            else
                conf.Logo = "";



        }

        private void EditConference()
        {
            ConferenceQuery confQuery = new ConferenceQuery();
            confQuery.setConference(conf);
        }

        public void OpenPositiveMessage(string output)
        {
            MessagePanel messagePanel = new MessagePanel();
            messagePanel.messageText.Text = output;
            messagePanel.button.Click += new RoutedEventHandler(PositiveButton_Click);
            info.messageWrap.Children.Add(messagePanel);
            info.messageWrap.Visibility = Visibility.Visible;

        }

        public void PositiveButton_Click(object sender, RoutedEventArgs e)
        {
            info.messageWrap.Visibility = Visibility.Collapsed;
            info.messageWrap.Children.Clear();

            ConferenceInfo panel = new ConferenceInfo(info.main, conf);
            info.main.wrap.Children.Clear();
            info.main.wrap.Children.Add(panel);
        }

        public void OpenNegativeMessage(string output)
        {
            MessagePanel messagePanel = new MessagePanel();
            messagePanel.messageText.Text = output;
            messagePanel.button.Click += new RoutedEventHandler(NegativeButton_Click);
            info.messageWrap.Children.Add(messagePanel);
            info.messageWrap.Visibility = Visibility.Visible;

        }

        public void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            info.messageWrap.Visibility = Visibility.Collapsed;
            info.messageWrap.Children.Clear();

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConferenceInformation panel = new ConferenceInformation(info, info.conf);
            info.infoWrap.Children.Clear();
            info.infoWrap.Children.Add(panel);
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
                confLogoImage.Source = bitmap;
            }
            catch
            {
                load = false;
            }
            finally
            {
                bitmap = null;
            }

            if (!load)
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/puste_logo.jpeg", UriKind.Absolute);
                bitmap.EndInit();
                confLogoImage.Source = bitmap;
            }


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
