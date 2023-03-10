using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LedControllerAndroid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : TabbedPage
    {
        public StartPage()
        {
            InitializeComponent();
            colors = Rofl();
        }
        List<Color> colors = new List<Color>();
        private List<Color> Rofl()
        {
            List<Color> colors = new List<Color>();
            IEnumerable<FieldInfo> fields = typeof(Color).GetTypeInfo().DeclaredFields.Where(x => x.IsPublic && x.IsStatic);
            foreach (FieldInfo info in fields)
            {
                colors.Add((Color)info.GetValue(null));
            }
            return colors;
        }
    }
}