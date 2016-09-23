﻿using SpyCameraIOT.Models;
using SpyCameraIOT.Util;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpyCameraIOT.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserSelection : Page
    {
        public UserSelection()
        {
            this.InitializeComponent();
            Loaded += UserSelection_Loaded;
        }

        private void UserSelection_Loaded(object sender, RoutedEventArgs e)
        {
            if (AccountHelper.AccountList.Count == 0)
            {
                //If there are no accounts navigate to the LoginPage
                this.Frame.Navigate(typeof(Frames.fHome));
            }

            UserListView.ItemsSource = AccountHelper.AccountList;
            UserListView.SelectionChanged += UserSelectionChanged;
        }

        /// <summary>
        /// Function called when an account is selected in the list of accounts
        /// Navigates to the Login page and passes the chosen account
        /// </summary>
        private void UserSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (((ListView)sender).SelectedValue != null)
            {
                Account account = (Account)((ListView)sender).SelectedValue;
                if (account != null)
                {
                    //Debug.WriteLine("Account " + account.Username + " selected!");
                }
                this.Frame.Navigate(typeof(Frames.fHome));
            }
        }

        /// <summary>
        /// Function called when the "+" button is clicked to add a new user.
        /// Navigates to the Login page with nothing filled out
        /// </summary>
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Frames.fAccount));
        }
    }
}
