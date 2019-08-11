using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Konferencja
{
    public class IconButton : Button
    {
        public IconButton() : base()
        {


        }

        public static readonly DependencyProperty IconProperty =
              DependencyProperty.Register("Icon", typeof(String),
              typeof(IconButton), new UIPropertyMetadata(""));

        public String Icon
        {
            get { return (String)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

    }
}
