using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;

namespace Input_Studios_Wave
{
    public partial class TabContent : UserControl
    {
        public TabContent()
        {
            InitializeComponent();
            DataContext = this;

            Browser.LifeSpanHandler = new CustomLifeSpanHandler(this);
        }

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public bool IsBrowserNavigating
        {
            get { return (bool)GetValue(IsBrowserNavigatingProperty); }
            set { SetValue(IsBrowserNavigatingProperty, value); }
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(TabContent), new PropertyMetadata(""));

        public static readonly DependencyProperty IsBrowserNavigatingProperty =
            DependencyProperty.Register("IsBrowserNavigating", typeof(bool), typeof(TabContent), new PropertyMetadata(false));

        private void Browser_Initialized(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Url))
            {
                Browser.Address = "https://www.bing.com";
            }
            else
            {
                Browser.Address = Url;
            }
        }

        private void OnUrlTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NavigateToUrl();
            }
        }

        private void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToUrl();
        }

        private void NavigateToUrl()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                Browser.Address = Url;
            }
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            Browser.Reload();
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoBack)
            {
                Browser.Back();
            }
        }

        private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoForward)
            {
                Browser.Forward();
            }
        }

        private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            IsBrowserNavigating = false;
        }

        public class CustomLifeSpanHandler : ILifeSpanHandler
        {
            private readonly TabContent _tabContent;

            public CustomLifeSpanHandler(TabContent tabContent)
            {
                _tabContent = tabContent;
            }

            public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
            }

            public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
            }

            public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _tabContent.IsBrowserNavigating = true;

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null)
                    {
                        var newTab = mainWindow.AddNewTab(targetUrl);
                        newTab.Browser.Address = targetUrl;
                    }
                });

                newBrowser = null;
                return true;
            }
        }
    }
}
