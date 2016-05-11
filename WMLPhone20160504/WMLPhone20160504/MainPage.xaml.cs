using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace WMLPhone20160504
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MediaPlayer _mediaPlayer;
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //imgSplash.Source = new BitmapImage(new Uri("ms-appx:///"+Constants.mainPageSpashScreenPath, UriKind.Absolute));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.
            _mediaPlayer = BackgroundMediaPlayer.Current;
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// THis function navigates the main screen to the MapViewScreen Main
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //AudioPlayer objAudioPlayer = new AudioPlayer();
                var message = new ValueSet
                {
                   {
                      "Play",
                       //"http://media.ch9.ms/ch9/a092/c301b08c-1acc-4f2c-b636-61f4e917a092/3-159.mp3"
                       Constants.startSoundPath
                   }
                 };
                BackgroundMediaPlayer.SendMessageToBackground(message);

                //await Task.Delay(TimeSpan.FromSeconds(3)); // set your desired delay
                Frame.Navigate(typeof(SplashScreen));
                //Frame.Navigate(typeof(MapScreenMain));
            }
            catch (Exception ex)
            {
                Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog("Error Message:\n" + ex.Message + "\n" + "Data:" + ex.Data);
                var dialogtask = await dialog.ShowAsync();
            }
        }

    }
}
