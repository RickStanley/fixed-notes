using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace App2
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool DropDownOpen { get; set; } = false;
        private DropDownButton OpenDropDown { get; set; } = null;
        private TextBlock HiddenTextBlock { get; set; } = null;

        public ObservableCollection<Note> Notes { get; }
            = new ObservableCollection<Note>();

        public MainPage()
        {
            this.InitializeComponent();
            //230, 185, 5
            this.Notes.Add(new Note() { Content = "Hello World!!", Color = new SolidColorBrush(new Color() { A = 255, R = 230, G = 185, B = 0,}) });

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 400));
            ApplicationView.PreferredLaunchViewSize = new Size(300, 400);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            Window.Current.Activated += Current_Activated;

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Set active window colors
            titleBar.ForegroundColor = Colors.White;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 42, 42, 42);
            titleBar.ButtonPressedForegroundColor = Colors.Gray;
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 32, 32, 32);

            // Set inactive window colors
            titleBar.InactiveForegroundColor = Colors.Gray;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Gray;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                TitleBarAddButton.Opacity = 0.5;
                TitleBarSettingsButton.Opacity = 0.5;
            } else
            {
                TitleBarSettingsButton.Opacity = 1.0;
                TitleBarAddButton.Opacity = 1.0;
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            TitleBarAddButton.Margin = new Thickness(coreTitleBar.SystemOverlayLeftInset, 0, 0, 0);
            TitleBarSettingsButton.Margin = new Thickness(0, 0, coreTitleBar.SystemOverlayRightInset, 0);
            AppTitleBar.Height = coreTitleBar.Height;
        }

        private async void Button_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var parameters = new NotePageParams();
            parameters.Note = (e.OriginalSource as FrameworkElement).DataContext as Note;

            await OpenPageAsWindowAsync(typeof(NotePage), parameters);
        }

        /// <summary>
        /// Opens a page given the page type as a new window.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private async Task<bool> OpenPageAsWindowAsync(Type t, NotePageParams notePageParams)
        {
            var view = CoreApplication.CreateNewView();
            int id = 0;

            await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(t, notePageParams);
                Window.Current.Content = frame;
                Window.Current.Activate();
                id = ApplicationView.GetForCurrentView().Id;
            });

            return await ApplicationViewSwitcher.TryShowAsStandaloneAsync(id);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
            }
        }

        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Button button = sender as Button;
            DropDownButton dropDownButton = button.FindName("DropDown") as DropDownButton;
            TextBlock date = button.FindName("Date") as TextBlock;
            dropDownButton.Visibility = Visibility.Visible;
            date.Visibility = Visibility.Collapsed;
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Button button = sender as Button;
            DropDownButton dropDownButton = button.FindName("DropDown") as DropDownButton;
            TextBlock date = button.FindName("Date") as TextBlock;
            if (DropDownOpen)
            {
                HiddenTextBlock = date;
                OpenDropDown = dropDownButton;
                return;
            }
            dropDownButton.Visibility = Visibility.Collapsed;
            date.Visibility = Visibility.Visible;
        }

        private void MenuFlyout_Opening(object sender, object e)
        {
            DropDownOpen = true;
        }

        private void MenuFlyout_Closed(object sender, object e)
        {
            DropDownOpen = false;
            HiddenTextBlock.Visibility = Visibility.Visible;
            OpenDropDown.Visibility = Visibility.Collapsed;
            HiddenTextBlock = null;
            OpenDropDown = null;
        }

    }
}