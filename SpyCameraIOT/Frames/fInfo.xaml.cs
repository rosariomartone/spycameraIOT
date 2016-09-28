﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

using SpyCameraIOT.IOT;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Microsoft.Azure.Devices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpyCameraIOT.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class fInfo : Page
    {
        static DeviceClient deviceClient;

        public fInfo()
        {
            this.InitializeComponent();
        }

        private void tbDeviceInfo_Loaded(object sender, RoutedEventArgs e)
        {
            getInfo();

            setInfo();
        }

        private async void setInfo()
        {
            await IOTMessages.SendCloudToDeviceMessageAsync("MESSAGE FROM IOT HUB: Message received.");
        }

        private async void getInfo()
        {
            tbDeviceInfo.Text = App.IsMobile;

            IOTMessages.SendDeviceToCloudMessagesAsync("ATTENTION: Intrusion detected!!! A small video has been recorded and it is visible on the shared folder.");

            while (true)
            {
                deviceClient = DeviceClient.Create(App.IOTUrl, new DeviceAuthenticationWithRegistrySymmetricKey(App.deviceIDMobile, App.deviceKeyMobile));

                Microsoft.Azure.Devices.Client.Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                string mess = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                alertDialog(mess);

                await deviceClient.CompleteAsync(receivedMessage);
            }

            List<Device> list = await IOTMessages.GetDevicesList();
        }
        private async void alertDialog(string message)
        {
            var dialog = new ContentDialog()
            {
                Title = "SpyCameraIOT: WARNING",
                MaxWidth = this.ActualWidth
            };

            var panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
            });

            var cb = new CheckBox
            {
                Content = "Agree"
            };

            cb.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = dialog,
                Path = new PropertyPath("IsPrimaryButtonEnabled"),
                Mode = BindingMode.TwoWay,
            });

            panel.Children.Add(cb);
            dialog.Content = panel;

            dialog.PrimaryButtonText = "OK";
            dialog.IsPrimaryButtonEnabled = false;
            dialog.SecondaryButtonText = "Cancel";

            var result = await dialog.ShowAsync();
        }
    }
}
