using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
using ClasesBase;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para Ingresando.xaml
    /// </summary>
    public partial class Ingresando : Window
    {
        DispatcherTimer timer;
        int progreso = 0;

        SoundPlayer player;

        public Ingresando()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player = new SoundPlayer(@".\audio\LOADING.wav");
            player.Load();
            player.PlayLooping();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);  // velocidad de carga
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            progreso++;
            progressBar.Value = progreso;

            if (progreso >= 100)
            {
                timer.Stop();
                player.Stop();
                // ABRE LOGIN AUTOMÁTICAMENTE
                WinWelcome login = new WinWelcome();
                login.Show();

                this.Close(); // cierra la ventana "Ingresando"
            }
        }
    }
}
