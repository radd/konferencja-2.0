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
    /// Interaction logic for SpeakerEdit.xaml
    /// </summary>
    public partial class SpeakerEdit : UserControl, IMessageControl
    {
        ConferenceInfo info;
        Speaker speaker;

        public SpeakerEdit(ConferenceInfo info, Speaker speaker)
        {
            InitializeComponent();
            this.info = info;
            this.speaker = speaker;

            LoadSpeakerInfo();
        }

        private void LoadSpeakerInfo()
        {
            speakerName.Text = speaker.Name;
            speakerTitle.Text = speaker.Title;
            timePickerStart.setTime(speaker.From);
            timePickerEnd.setTime(speaker.To);
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
                EditSpeaker();
                saveButton.IsEnabled = false;
                output = "Zapisano";
                OpenPositiveMessage(output);
            }
            catch (Exception e)
            {
                output = e.Message;
                OpenNegativeMessage(output);
            }
            finally
            {

            }

        }

        private void PrepareData()
        {
            if (speakerName.Text != "")
                speaker.Name = speakerName.Text;
            else
                throw new Exception("Pole jest wymagane");

            if (speakerTitle.Text != "")
                speaker.Title = speakerTitle.Text;
            else
                throw new Exception("Pole jest wymagane");

            speaker.From = timePickerStart.getTime();
            speaker.To = timePickerEnd.getTime();

        }

        private void EditSpeaker()
        {
            SpeakerQuery speakerQuery = new SpeakerQuery();
            speakerQuery.setSpeaker(speaker, info.conf.ID);
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

            ConferenceInformation panel = new ConferenceInformation(info, info.conf);
            info.infoWrap.Children.Clear();
            info.infoWrap.Children.Add(panel);
            info.mainScroll.ScrollToBottom();
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
            info.mainScroll.ScrollToBottom();
        }
    }
}
