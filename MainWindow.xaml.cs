using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using SavingPpm;
namespace Clustering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void GenerateCluster(ref Pixent canvas, int size) {

        }
        public MainWindow() {
            Pixent canvas = new Pixent(800,800);
            canvas.FillBackGround(SavingPpm.Colors.LIGHTGREEN);
            canvas.SaveAsPng("result.png");
            InitializeComponent();
            var path = Path.Combine(Environment.CurrentDirectory, "result.png");
            BitmapImage image = new BitmapImage(new Uri(path));
            image_test.Source = image;
        }
    }
}