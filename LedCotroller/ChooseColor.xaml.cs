using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace LedCotroller
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ChooseColor : Page
    {
        List<Color> colors = new List<Color>();
        public ChooseColor()
        {
            this.InitializeComponent();
            colors = Rofl();
        }
        private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            var p = ((MainPage)((Frame)Window.Current.Content).Content).ActiveLeds;
            foreach (var item in p)
            {
                item.Color = args.NewColor;
            }
            
        }



        private List<Color> Rofl()
        {
            List<Color> colors = new List<Color>();
            IEnumerable<PropertyInfo> props = typeof(Colors).GetTypeInfo().DeclaredProperties.Where(x => x.GetMethod.IsStatic && x.GetMethod.IsPublic);
            foreach (PropertyInfo info in props)
            {
                colors.Add((Color)info.GetValue(null));
            }
            return colors;
        }

        private void StyledGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = ((MainPage)((Frame)Window.Current.Content).Content).ActiveLeds;
            foreach (var item in p)
            {
                item.IsVideoMode = false;
                item.Color = (Color)e.AddedItems.FirstOrDefault();
            }
        }
    }
}
