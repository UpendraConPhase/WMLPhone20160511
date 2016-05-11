using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Playback;

namespace BackgroundMusic
{
    public sealed class MusicPlayer : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private SystemMediaTransportControls objSystemMediaTransportControl;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            objSystemMediaTransportControl = SystemMediaTransportControls.GetForCurrentView();
            objSystemMediaTransportControl.IsEnabled = true;

            BackgroundMediaPlayer.MessageReceivedFromForeground += MessageReceivedFromForeground;
            BackgroundMediaPlayer.Current.CurrentStateChanged += BackgroundMediaPlayerCurrentStateChanged;

            // Associate a cancellation and completed handlers with the background task.
            taskInstance.Canceled += OnCanceled;
            taskInstance.Task.Completed += Taskcompleted;

            _deferral = taskInstance.GetDeferral();
        }

        private void MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            ValueSet valueSet = e.Data;
            foreach (string key in valueSet.Keys)
            {
                switch (key)
                {
                    case "Play":
                        //Debug.WriteLine("Starting Playback");
                        Play(valueSet[key].ToString());
                        break;
                }
            }
        }

        private void Play(string toPlay)
        {
            MediaPlayer mediaPlayer = BackgroundMediaPlayer.Current;
            mediaPlayer.AutoPlay = true;
            mediaPlayer.SetUriSource(new Uri(toPlay));

            //Update the universal volume control
            objSystemMediaTransportControl.ButtonPressed += MediaTransportControlButtonPressed;
            objSystemMediaTransportControl.IsPauseEnabled = true;
            objSystemMediaTransportControl.IsPlayEnabled = true;
            objSystemMediaTransportControl.DisplayUpdater.Type = MediaPlaybackType.Music;
            objSystemMediaTransportControl.DisplayUpdater.MusicProperties.Title = "Test Title";
            objSystemMediaTransportControl.DisplayUpdater.MusicProperties.Artist = "Test Artist";
            objSystemMediaTransportControl.DisplayUpdater.Update();
        }

        /// <summary>
        ///     The MediaPlayer's state changes, update the Universal Volume Control to reflect the correct state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void BackgroundMediaPlayerCurrentStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Playing)
            {
                objSystemMediaTransportControl.PlaybackStatus = MediaPlaybackStatus.Playing;
            }
            else if (sender.CurrentState == MediaPlayerState.Paused)
            {
                objSystemMediaTransportControl.PlaybackStatus = MediaPlaybackStatus.Paused;
            }
        }

        /// <summary>
        ///     Handle the buttons on the Universal Volume Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MediaTransportControlButtonPressed(SystemMediaTransportControls sender,
            SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    BackgroundMediaPlayer.Current.Play();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    BackgroundMediaPlayer.Current.Pause();
                    break;
            }
        }


        private void Taskcompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            BackgroundMediaPlayer.Shutdown();
            _deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // You get some time here to save your state before process and resources are reclaimed
            BackgroundMediaPlayer.Shutdown();
            _deferral.Complete();
        }
    }
}
