using SavingPpm;
using System.IO;
using System.Printing.IndexedProperties;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media.Imaging;

namespace Clustering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Circle
    {
        public int x;
        public int y;
        public int radius;
        public Colors color;
        public Circle(int x, int y, int radius, SavingPpm.Colors color) {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.color = color;
        }
    }
    public partial class MainWindow : Window {
        public Colors[] means_colors={
                Colors.TEAL,
             Colors.GOLD,
             Colors.SALMON,  
             Colors.BLACK,
             Colors.PURPLE,
             Colors.GREEN,
        };
        public const int width = 800;
        public const int height = 800;
        public Pixent canvas;
        public List<Circle> cluster_circles = new List<Circle>();
        public List<Circle> means = new List<Circle>();
        private int CalcDistance(int x1, int y1, int x2, int y2) {
            return (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
        public void GenerateCluster(int ammount, int cx, int cy, int radius, Colors color) {
            
            var temp = new List<Circle>();
            for (int i = 0; i < ammount; i++) {
                Random rnd = new Random();
                int x = rnd.Next(200) - 100;
                int y = rnd.Next(200) - 100;
                canvas.FillCircle(cx + x, cy + y, radius, color);
                temp.Add(new Circle(cx + x, cy + y, radius, color));
            }
            cluster_circles.AddRange(temp);
        }
        public void GenerateMeans(int ammount, int radius) {
            if (ammount >= means_colors.Length) ammount = 6;
            int color_pointer = 0;
            for (int i = 0; i < ammount; i++) {
                Random rnd = new ();
                int x = rnd.Next(width-radius*2)+radius;
                int y = rnd.Next(height-radius*2)+radius;
                canvas.FillCircle(x, y, radius, means_colors[color_pointer]);
                means.Add(new Circle(x,y,radius, means_colors[color_pointer]));
                color_pointer++;
            }
        }
        public void Assignment() {
            foreach(Circle point in cluster_circles) {
                int[] distances = new int[means.Count];
                for(int i=0;i<means.Count;i++) {
                    Circle mean = means[i];
                    distances[i] = CalcDistance(point.x, point.y, mean.x, mean.y);
                }
                point.color=means[Array.IndexOf(distances, distances.Min())].color;
            }
        }
        public void PrintAllData() {
            foreach (Circle point in cluster_circles) {
                canvas.FillCircle(point.x, point.y,point.radius,point.color);
            }
            foreach (Circle mean in means) {
                canvas.FillCircle(mean.x, mean.y, mean.radius, mean.color);
            }
        }
        public MainWindow() {
            canvas = new Pixent(width, height);
            canvas.FillBackGround(Colors.LIGHTGREEN);
            GenerateCluster(20, 400, 400, 10, Colors.BLUE);
            GenerateCluster(20, 200, 200, 10, Colors.BLUE);
            GenerateCluster(20, 600, 200, 10, Colors.BLUE);
            GenerateMeans(3,20);
            canvas.SaveAsPng("first.png");

            Assignment();
            canvas.FillBackGround(Colors.LIGHTGREEN);
            PrintAllData();
            canvas.SaveAsPng("second.png");

            InitializeComponent();
            var path = Path.Combine(Environment.CurrentDirectory, "first.png");
            BitmapImage image = new BitmapImage(new Uri(path));
            image1_test.Source = image;

            path = Path.Combine(Environment.CurrentDirectory, "second.png");
            image = new BitmapImage(new Uri(path));
            image2_test.Source = image;
        }
    }
}