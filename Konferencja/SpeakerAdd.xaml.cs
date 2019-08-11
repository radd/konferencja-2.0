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
    /// Interaction logic for SpeakerAdd.xaml
    /// </summary>
    public partial class SpeakerAdd : UserControl, IMessageControl
    {
        ConferenceInfo info;

        public SpeakerAdd(ConferenceInfo info)
        {
            InitializeComponent();
            this.info = info;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            NewSpeaker();
        }

        private void NewSpeaker()
        {
            string output = "";
            try
            {
                PrepareData();
                AddSpeaker();
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
            if (speakerName.Text == "")
                throw new Exception("Pole jest wymagane");

            if (speakerTitle.Text == "")
                throw new Exception("Pole jest wymagane");
        }

        private void AddSpeaker()
        {
            SpeakerQuery speakerQuery = new SpeakerQuery();
            int speakerID = speakerQuery.addSpeaker(info.conf.ID, speakerName.Text, speakerTitle.Text, timePickerStart.getTime(), timePickerEnd.getTime());
            if (speakerID != 0)
            {
                RelationshipQuery relaQuery = new RelationshipQuery();
                relaQuery.addRelationship(info.conf.ID, speakerID);
            }

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
