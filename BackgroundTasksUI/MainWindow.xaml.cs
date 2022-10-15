using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WPF_examples;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private Action? _cancel_action = null;

    async void b_start_Click(object sender, RoutedEventArgs e)
    {
        b_start.IsEnabled = false;
        b_stop.IsEnabled = true;

        var cts = new CancellationTokenSource();

        _cancel_action = () =>
        {
            b_start.IsEnabled = true;
            b_stop.IsEnabled = false;
            cts.Cancel();
        };

        var token = cts.Token;

        await Task.Run(() => background_loop(1000, token), token);

    }

    void background_loop(int number, CancellationToken token)
    {
        for (int i = 0; i < number; i++)
        {
            if (token.IsCancellationRequested)
                break;

            Dispatcher.Invoke(() =>
            {
                tb_counter.Text = i.ToString();
            });

            Thread.Sleep(100);
        }

        Dispatcher.Invoke(() =>
        {
            b_start.IsEnabled = true;
            b_stop.IsEnabled = false;
        });
    }

    private void b_stop_Click(object sender, RoutedEventArgs e)
    {
        _cancel_action?.Invoke();
    }
} // class MainWindow
