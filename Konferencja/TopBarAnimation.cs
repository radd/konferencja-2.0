using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Konferencja
{
    class TopBarAnimation
    {
        MainWindow main;

        public TopBarAnimation(MainWindow main)
        {
            this.main = main;
        }


        public void Collapse(object panel)
        {
            HideWrapCollapse(panel);
        }

        private void HideWrapCollapse(object panel)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation wrapAnim = new DoubleAnimation();
            wrapAnim.From = main.wrap.Opacity;
            wrapAnim.To = 0;
            wrapAnim.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            wrapAnim.Completed += (s, e) =>
            {
                ShowWrapCollapse(panel);
            };

            ThicknessAnimation wrapAnim2 = new ThicknessAnimation();
            wrapAnim2.From = main.wrap.Margin;
            wrapAnim2.To = new Thickness(0, -140, 0, 0);
            wrapAnim2.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation logo2Anim = new DoubleAnimation();
            logo2Anim.From = main.logo2.Opacity;
            logo2Anim.To = 0;
            logo2Anim.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            ThicknessAnimation topBarAnim = new ThicknessAnimation();
            topBarAnim.From = main.topBar.Padding;
            topBarAnim.To = new Thickness(0, 0, 0, 140);
            topBarAnim.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            storyboard.Children.Add(logo2Anim);
            storyboard.Children.Add(topBarAnim);
            storyboard.Children.Add(wrapAnim2);
            storyboard.Children.Add(wrapAnim);

            Storyboard.SetTargetName(wrapAnim, main.wrap.Name);
            Storyboard.SetTargetProperty(wrapAnim, new PropertyPath(DockPanel.OpacityProperty));
            Storyboard.SetTargetName(wrapAnim2, main.wrap.Name);
            Storyboard.SetTargetProperty(wrapAnim2, new PropertyPath(DockPanel.MarginProperty));
            Storyboard.SetTargetName(logo2Anim, main.logo2.Name);
            Storyboard.SetTargetProperty(logo2Anim, new PropertyPath(Image.OpacityProperty));
            Storyboard.SetTargetName(topBarAnim, main.topBar.Name);
            Storyboard.SetTargetProperty(topBarAnim, new PropertyPath(Border.PaddingProperty));
            storyboard.Begin(main);
        }

        private void ShowWrapCollapse(object panel)
        {
            main.wrap.Children.Clear();
            main.wrap.Children.Add((UIElement)panel);

            Storyboard storyboard = new Storyboard();

            DoubleAnimation wrapAnim = new DoubleAnimation();
            wrapAnim.From = main.wrap.Opacity;
            wrapAnim.To = 1;
            wrapAnim.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            storyboard.Children.Add(wrapAnim);

            Storyboard.SetTargetName(wrapAnim, main.wrap.Name);
            Storyboard.SetTargetProperty(wrapAnim, new PropertyPath(DockPanel.OpacityProperty));

            storyboard.Begin(main);

        }


        public void Expand(object panel)
        {
            HideWrapExpand(panel);
        }

        private void HideWrapExpand(object panel)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation wrapAnim = new DoubleAnimation();
            wrapAnim.From = main.wrap.Opacity;
            wrapAnim.To = 0;
            wrapAnim.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            wrapAnim.Completed += (s, e) =>
            {
                ShowWrapExpand(panel);
            };

            ThicknessAnimation wrapAnim2 = new ThicknessAnimation();
            wrapAnim2.From = main.wrap.Margin;
            wrapAnim2.To = new Thickness(0, -60, 0, 0);
            wrapAnim2.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation logo2Anim = new DoubleAnimation();
            logo2Anim.From = main.logo2.Opacity;
            logo2Anim.To = 1;
            logo2Anim.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            ThicknessAnimation topBarAnim = new ThicknessAnimation();
            topBarAnim.From = main.topBar.Padding;
            topBarAnim.To = new Thickness(0, 0, 0, 60);
            topBarAnim.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            storyboard.Children.Add(logo2Anim);
            storyboard.Children.Add(topBarAnim);
            storyboard.Children.Add(wrapAnim2);
            storyboard.Children.Add(wrapAnim);

            Storyboard.SetTargetName(wrapAnim, main.wrap.Name);
            Storyboard.SetTargetProperty(wrapAnim, new PropertyPath(DockPanel.OpacityProperty));
            Storyboard.SetTargetName(wrapAnim2, main.wrap.Name);
            Storyboard.SetTargetProperty(wrapAnim2, new PropertyPath(DockPanel.MarginProperty));
            Storyboard.SetTargetName(logo2Anim, main.logo2.Name);
            Storyboard.SetTargetProperty(logo2Anim, new PropertyPath(Image.OpacityProperty));
            Storyboard.SetTargetName(topBarAnim, main.topBar.Name);
            Storyboard.SetTargetProperty(topBarAnim, new PropertyPath(Border.PaddingProperty));

            storyboard.Begin(main);
        }

        private void ShowWrapExpand(object panel)
        {
            main.wrap.Children.Clear();
            main.wrap.Children.Add((UIElement)panel);

            Storyboard storyboard = new Storyboard();

            DoubleAnimation wrapAnim = new DoubleAnimation();
            wrapAnim.From = main.wrap.Opacity;
            wrapAnim.To = 1;
            wrapAnim.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            storyboard.Children.Add(wrapAnim);
            Storyboard.SetTargetName(wrapAnim, main.wrap.Name);
            Storyboard.SetTargetProperty(wrapAnim, new PropertyPath(DockPanel.OpacityProperty));

            storyboard.Begin(main);

        }

    }
}
