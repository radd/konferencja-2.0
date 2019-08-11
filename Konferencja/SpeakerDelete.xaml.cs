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
    /// Interaction logic for SpeakerDelete.xaml
    /// </summary>
    public partial class SpeakerDelete : UserControl, IMessageControl
    {
        Speaker speaker;
        ConferenceInfo info;
        public SpeakerDelete(ConferenceInfo info, Speaker speaker)
        {
            InitializeComponent();
            this.speaker = speaker;
            this.info = info;
            OpenPositiveMessage("Czy chcesz usunąć \"" + speaker.Name + "\"? ");
        }

        public void OpenPositiveMessage(string output)
        {
            MessageTwoPanel messagePanel = new MessageTwoPanel();
            messagePanel.messageText.Text = output;
            messagePanel.ok.Click += new RoutedEventHandler(PositiveButton_Click);
            messagePanel.cancel.Click += new RoutedEventHandler(NegativeButton_Click);
            info.messageWrap.Children.Add(messagePanel);
            info.messageWrap.Visibility = Visibility.Visible;
        }

        public void OpenNegativeMessage(string output)
        {
            throw new NotImplementedException();
        }

        public void PositiveButton_Click(object sender, RoutedEventArgs e)
        {
            SpeakerQuery speakerQuery = new SpeakerQuery();
            speakerQuery.Delete(speaker.ID);

            info.messageWrap.Visibility = Visibility.Collapsed;
            info.messageWrap.Children.Clear();

            ConferenceInfo panel = new ConferenceInfo(info.main, info.conf);
            info.main.wrap.Children.Clear();
            info.main.wrap.Children.Add(panel);

        }

        public void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            info.messageWrap.Visibility = Visibility.Collapsed;
            info.messageWrap.Children.Clear();
            ConferenceInfo panel = new ConferenceInfo(info.main, info.conf);
            info.main.wrap.Children.Clear();
            info.main.wrap.Children.Add(panel);
        }
    }
}
