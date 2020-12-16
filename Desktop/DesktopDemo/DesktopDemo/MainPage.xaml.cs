using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DesktopDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
    private TelemetryClient tc;
        public MainPage()
        {
            this.InitializeComponent();

            tc = new TelemetryClient();
            // Alternative to setting ikey in config file:
            tc.InstrumentationKey = "3de41c8b-2293-4f66-9001-507a8fa467de";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new Exception("manually exception thrown!");
            }
            catch(Exception ex)
            {
                tc.TrackException(ex);
                Console.WriteLine("catched");
            }
          
        }
    }
}
