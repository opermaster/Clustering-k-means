﻿using SavingPpm;
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
        public List<Circle> means= new List<Circle>();
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
        public void Update() {
            foreach(Circle mean in means) {
                List<Circle> assigned_circles = cluster_circles.Where(circ => circ.color == mean.color)
                    .ToList();
                int c = assigned_circles.Count;
                int cx = assigned_circles.Select(circle => circle.x).Sum();
                int cy = assigned_circles.Select(circle => circle.y).Sum();
                mean.x = cx / c;
                mean.y = cy / c;
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
        private static BitmapSource LoadImage(string path) {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }
        public MainWindow() {
            canvas = new Pixent(width, height);
            canvas.FillBackGround(Colors.LIGHTGREEN);
            GenerateCluster(20, 400, 400, 5, Colors.BLUE);
            GenerateCluster(20, 200, 200, 5, Colors.BLUE);
            GenerateCluster(20, 600, 200, 5, Colors.BLUE);
            GenerateMeans(3,20);
            canvas.SaveAsPng("first.png");

            Assignment();
            canvas.FillBackGround(Colors.LIGHTGREEN);
            PrintAllData();
            canvas.SaveAsPng("second.png");

            Update();
            canvas.FillBackGround(Colors.LIGHTGREEN);
            PrintAllData();
            canvas.SaveAsPng("third.png");

            InitializeComponent();

            image1_test.Source = LoadImage("first.png"); 

            image2_test.Source = LoadImage("second.png");

            image3_test.Source = LoadImage("third.png");
        }

        private void Regenerate_Button_Click(object sender, RoutedEventArgs e) {
            cluster_circles = new List<Circle>();
            means= new List<Circle>();
            canvas.FillBackGround(Colors.LIGHTGREEN);

            //100,100;200,200;300,300;
            int elem_ammount = Convert.ToInt32( Cluster_Count.Text);

            string[] clusers_coords = Cluster_Coord.Text
                    .Replace(" ", "")
                    .Split(";");
            foreach (string coord in clusers_coords) {
                string[] x_y = coord.Split(",");
                int x = Convert.ToInt32(x_y[0]);
                int y = Convert.ToInt32(x_y[1]);
                GenerateCluster(elem_ammount, x, y, 5, Colors.BLUE);
            }

            int means_ammount = Convert.ToInt32(Centroids_Count.Text);

            GenerateMeans(means_ammount, 20);
            
            canvas.SaveAsPng("first.png");
            image1_test.Source = LoadImage("first.png");
        }

        private void Clustering_Button_Click(object sender, RoutedEventArgs e) {
            Assignment();
            canvas.FillBackGround(Colors.LIGHTGREEN);
            PrintAllData();
            canvas.SaveAsPng("second.png");
            Update();
            canvas.FillBackGround(Colors.LIGHTGREEN);
            PrintAllData();
            canvas.SaveAsPng("third.png");

            image2_test.Source = LoadImage("second.png");
            image3_test.Source = LoadImage("third.png"); 
        }
    }
}