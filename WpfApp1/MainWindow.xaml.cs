using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        ConsoleContent dc = new ConsoleContent();
        Controller controller = new Controller();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = dc;
            Loaded += MainWindow_Loaded;
            rbuttonVideo.IsChecked = true;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InputBlock.Focus();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            var check = controller.verifyURL(txtUrl);
            if (check)
            {
                dc.ConsoleInput = "Enterd url is valid, starting download";
                showOutput();
                
                if (rbuttonVideo.IsChecked == true)
                {
                    var video = controller.getVideo(txtUrl);

                    if (video != null)
                    {
                        dc.ConsoleInput = "Video succesfully downloaded choose a destination folder";
                        showOutput();
                        controller.saveVideo(video);
                        dc.ConsoleInput = "File succesfully saved to ...";
                        showOutput();
                    }
                }
                else
                {
                    var audio = controller.getAudio(txtUrl);
                    if (audio != null)
                    {
                        dc.ConsoleInput = "Video succesfully downloaded choose a destination folder";
                        showOutput();
                        controller.saveAudio(audio);
                        dc.ConsoleInput = "File succesfully saved to ...";
                        showOutput();
                    }
                }
            }
            else { dc.ConsoleInput = "Error: Entered url is not a valid YouTube url."; showOutput(); }
        }

        public void showOutput()
        {
            dc.RunCommand();
            InputBlock.Focus();
            Scroller.ScrollToBottom();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TitleBar_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.MainWindow.DragMove();
        }

        private void rbuttonAudio_GotFocus(object sender, RoutedEventArgs e)
        {
            dc.ConsoleInput = "File output changed to .mp3";
            showOutput();
        }

        private void rbuttonVideo_GotFocus(object sender, RoutedEventArgs e)
        {
            dc.ConsoleInput = "File output changed to .mp4";
            showOutput();
        }
    }
}
