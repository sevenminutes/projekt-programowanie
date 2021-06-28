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
using System.Windows.Threading;

namespace Pacman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool Up, Down, Left, Right; 
        bool noUp, noDown, noLeft, noRight;
        int speed = 9;
        int Speed = 8;
        int ghostMove = 150;
        int currentGhostStep;
        int score = 0;
        DispatcherTimer Timer = new DispatcherTimer();
        Rect pacmanHitBox;

        public MainWindow()
        {
            InitializeComponent();
            Game();
        }

        private void canvaskd(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && noLeft == false)
            {
                
                Right = Up = Down = false; 
                noRight = noUp = noDown = false;

                Left = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Right && noRight == false)
            {
                
                noLeft = noUp = noDown = false; 
                Left = Up = Down = false; 

                Right = true; 

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2); 

            }

            if (e.Key == Key.Up && noUp == false)
            {
                
                noRight = noDown = noLeft = false; 
                Right = Down = Left = false; 

                Up = true; 

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2); 
            }

            if (e.Key == Key.Down && noDown == false)
            {
                
                noUp = noLeft = noRight = false; 
                Up = Left = Right = false; 

                Down = true; 

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }
        }
        private void Game()
        {
           

            game.Focus();

            Timer.Tick += GameLoop;
            Timer.Interval = TimeSpan.FromMilliseconds(15);
            Timer.Start();
            currentGhostStep = ghostMove;

            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("C:/Users/Bartosz/source/repos/Pacman/Pacman/pacman_photos/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("C:/Users/Bartosz/source/repos/Pacman/Pacman/pacman_photos/red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("C:/Users/Bartosz/source/repos/Pacman/Pacman/pacman_photos/orange.jpg"));
            orangeGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("C:/Users/Bartosz/source/repos/Pacman/Pacman/pacman_photos/pink.jpg"));
            pinkGuy.Fill = pinkGhost;


        }
        private void GameLoop(object sender, EventArgs e)
        {

            txtScore.Content = "Score: " + score; 

            if (Right)
            { 
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (Left)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (Up)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (Down)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }

            if (Down && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                Down = false;
            }
            if (Up && Canvas.GetTop(pacman) < 1)
            {
                noUp = true;
                Up = false;
            }
            if (Left && Canvas.GetLeft(pacman) - 10 < 1)
            {
                noLeft = true;
                Left = false;
            }
            if (Right && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                Right = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height); 

            foreach (var x in game.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); 
                if ((string)x.Tag == "wall")
                {
                    if (Left == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        Left = false;
                    }
                    if (Right == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        Right = false;
                    }
                    if (Down == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        Down = false;
                    }
                    if (Up == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        Up = false;
                    }
                }
                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;
                    }
                }

                if ((string)x.Tag == "ghost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver("Duszek Cię złapał!");
                    }

                    if (x.Name.ToString() == "orangeGuy")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - Speed);

                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + Speed);
                    }

                    currentGhostStep--;

                    if (currentGhostStep < 1)
                    {

                        currentGhostStep = ghostMove;
                        Speed = -Speed;
                    }
                }
            }

            if (score == 116)
            {
                GameOver("Brawo, zebrałeś wszystkie pieniążki!");
            }
        }
        private void GameOver(string message)
        {
            Timer.Stop();
            MessageBox.Show(message, "Stop");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
