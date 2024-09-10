using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HowTo_DetectLongButtonPress;

public partial class MainWindow : Window
{
    bool IsLeftButtonDown = false;
    Stopwatch leftButtonDownTimer = new();
    Brush originalBackground;
    Brush originalForeground;
    string originalText;

    public MainWindow()
    {
        InitializeComponent();
        originalBackground = tbTest.Background;
        originalForeground = tbTest.Foreground;
        originalText = tbTest.Text;
    }

    private void LongPressMethod()
    {
        Dispatcher.Invoke(() => tbTest.Background = Brushes.Green);
        Dispatcher.Invoke(() => tbTest.Foreground = Brushes.White);
        Dispatcher.Invoke(() => tbTest.Text = "Long press");
    }

    private void IsLongPress(long timeDown)
    {
        bool longPress = false;
        leftButtonDownTimer.Start();
        while (IsLeftButtonDown)
        {
            if (leftButtonDownTimer.ElapsedMilliseconds >= timeDown)
            {
                longPress = true;
                break;
            }
        }
        IsLeftButtonDown = false;
        leftButtonDownTimer.Stop();
        leftButtonDownTimer.Reset();
        if (longPress)
        {
            LongPressMethod();
        }
    }

    private void tbTest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        IsLeftButtonDown = true;
        Task.Run(() => IsLongPress(500));// Milliseconds
    }

    private void tbTest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        IsLeftButtonDown = false;
        tbTest.Background = originalBackground;
        tbTest.Foreground = originalForeground;
        tbTest.Text = originalText;
    }

    private void tbTest_MouseLeave(object sender, MouseEventArgs e)
    {
        // Consider this as button up
        tbTest_MouseLeftButtonUp(sender, null);
    }
}