using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class AverageScreenColor : Page
    {
        List<Color> colors = new List<Color>();
        ScreenColor color = new ScreenColor();

        public AverageScreenColor()
        {
            this.InitializeComponent();
        }

        void AddElement()
        {
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            color.ColorChanged += Color_ColorChanged;
            await color.StartCaptureAsync();
        }

        private void Color_ColorChanged(object sender, Color e)
        {
            var p = ((MainPage)((Frame)Window.Current.Content).Content).ActiveLeds;
            foreach (var item in p)
            {
                item.IsVideoMode = true;
                item.Color = e;
            }
        }
    }
}
