using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Konferencja
{
    interface IMessageControl
    {
        void OpenPositiveMessage(string output);
        void OpenNegativeMessage(string output);
        void PositiveButton_Click(object sender, RoutedEventArgs e);
        void NegativeButton_Click(object sender, RoutedEventArgs e);
    }
}
