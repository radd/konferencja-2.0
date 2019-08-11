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
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
        }


        public DateTime getTime()
        {
            if (hoursTextBox.Text != "" && minutesTextBox.Text != "")
            {
                string time = String.Format("{0}:{1}", hoursTextBox.Text, minutesTextBox.Text);
                DateTime dt;
                if (DateTime.TryParse(time, out dt))
                    return dt;
                else
                    throw new Exception("Nieprawidłowy format czasu");
            }
            else
                throw new Exception("Nieprawidłowy format czasu");
        }

        public void setTime(DateTime dt)
        {
            hoursTextBox.Text = dt.ToString("HH");
            minutesTextBox.Text = dt.ToString("mm");
        }

        private void hoursTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            int hours = 0;
            TextBox hoursTextBox = sender as TextBox;

            if (hoursTextBox != null)
            {
                if (hoursTextBox.Text.Length <= 2)
                {
                    if (Int32.TryParse(hoursTextBox.Text, out hours) && hours >= 0 && hours <= 24)
                    {
                        if (hoursTextBox.Text.Length == 2)
                            minutesTextBox.Focus();
                    }
                    else
                        hoursTextBox.Text = "";

                }
                else
                {
                    hoursTextBox.Text = "";
                    minutesTextBox.Focus();
                }
            }


        }

        private void hoursTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox hourTextBox = sender as TextBox;

            if (hoursTextBox != null)
                hoursTextBox.Text = "";
        }

        private void hoursTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox hoursTextBox = sender as TextBox;

            if (hoursTextBox != null)
                if (hoursTextBox.Text.Length == 1)
                    hoursTextBox.Text = "0" + hoursTextBox.Text;
        }

        private void minutesTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            int minutes = 0;
            TextBox minutesTextBox = sender as TextBox;

            if (minutesTextBox != null)
            {
                if (minutesTextBox.Text.Length <= 2)
                {
                    if (!(Int32.TryParse(minutesTextBox.Text, out minutes) && minutes >= 0 && minutes <= 59))
                        minutesTextBox.Text = "";
                }
                else
                    minutesTextBox.Text = "";
            }

        }

        private void minutesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox minutesTextBox = sender as TextBox;

            if (minutesTextBox != null)
                minutesTextBox.Text = "";
        }

        private void minutesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox minutesTextBox = sender as TextBox;

            if (minutesTextBox != null)
                if (minutesTextBox.Text.Length == 1)
                    minutesTextBox.Text = "0" + minutesTextBox.Text;
        }
    }
}

