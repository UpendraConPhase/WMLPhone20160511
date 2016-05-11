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
using Windows.UI;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WMLPhone20160504
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapScreenMain : Page
    {
        public MapScreenMain()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        async private void Web_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Web.Source = new Uri("ms-appx-web:///MAP.html");
                //Web.Source = new Uri("ms-appx-web:///HTMLPages/MapView.html");
                // Web.Source = new Uri("ms-appx-web:///HTMLPages/RouteMap.html");
                Web.Source = new Uri(Constants.browserMapPage);//RouteMapLocalScript
                string[] latlong = new string[] { "23.027193", "72.507470" };
                string[,] polypoint = new string[,] {
                    {"22.986786","72.494863" },
                    {"23.027193","72.507470" },
                    {"23.038695","72.511836" }
                };
                List<string[]> l = new List<string[]>();
                l.Add(new string[] { "22.986786", "72.494863" });
                l.Add(new string[] { "23.027193", "72.507470" });
                l.Add(new string[] { "23.038695", "72.511836" });

                //var x = Web.InvokeScriptAsync("eval", new string[] { "var objLatitude=23.027193; var onjLongitude=72.507470;" });
                Web.LoadCompleted += (object s, NavigationEventArgs e1) =>
                {
                    //Web.IsScriptEnabled = true;
                    var x = Web.InvokeScriptAsync("SetLatLong", latlong);
                    x = Web.InvokeScriptAsync("addPolylineToMapFromScript", l[0]);
                    x = Web.InvokeScriptAsync("addPolylineToMapFromScript", l[1]);
                    x = Web.InvokeScriptAsync("addPolylineToMapFromScript", l[2]);
                    x = Web.InvokeScriptAsync("DrawMapFromScript", new string[] { "" });
                    //var clientHeight = Web.InvokeScriptAsync("eval", new string[] { "document.body.clientHeight.toString();" });
                };

            }
            catch (Exception ex)
            {
                Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog("Error Message:\n" + ex.Message + "\n" + "Data:" + ex.Data);
                var dialogtask = await dialog.ShowAsync();
            }
        }

        async private void Web_ScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                if (e.Value.StartsWith("AddGuess"))
                {
                    string[] arg = e.Value.Split('~');
                    string lat = arg[1];
                    string lng = arg[2];
                    string[] objLatLong = new string[2];
                    objLatLong[0] = lat;
                    objLatLong[1] = lng;
                    Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog("Add Guess ?");


                    dialog.Commands.Add(new UICommand("Yes"));
                    dialog.Commands.Add(new UICommand("No"));
                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;
                    var dialogtask = await dialog.ShowAsync();
                    if (dialogtask.Label.Equals("Yes"))
                    {
                        var x = Web.InvokeScriptAsync("setPin", objLatLong);

                    }
                    if (dialogtask.Label.Equals("No"))
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog("Error Message:\n" + ex.Message + "\n" + "Data:" + ex.Data);
                var dialogtask = await dialog.ShowAsync();
            }
        }
    }
}
