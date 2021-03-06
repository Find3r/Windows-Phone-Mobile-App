﻿using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pineable.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private MobileServiceUser user;
        
        public Login()
        {
            this.InitializeComponent();
        }

        private async System.Threading.Tasks.Task<bool> Authenticate()
        {
            string message = null;
            bool success = false;

            // This sample uses the Facebook provider.
            var PROVIDER = "Facebook";


            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource(PROVIDER).FirstOrDefault();
            }
            catch (Exception)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }

            if (credential != null)
            {
                // Create a user from the stored credentials.
                user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

                // Set the user from the stored credentials.
                App.MobileService.CurrentUser = user;

                // Consider adding a check to determine if the token is 
                // expired, as shown in this post: http://aka.ms/jww5vp.

                success = true;
            }
            else
            {
                try
                {
                    // Login with the identity provider.
                    user = await App.MobileService
                        .LoginAsync(MobileServiceAuthenticationProvider.Facebook);

                    // Create and store the user credentials.
                    credential = new PasswordCredential(PROVIDER,
                        user.UserId, user.MobileServiceAuthenticationToken);
                    vault.Add(credential);

                    user = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    user.MobileServiceAuthenticationToken = credential.Password;

                    // Set the user from the stored credentials.
                    App.MobileService.CurrentUser = user;

                    success = true;

                }
                catch (MobileServiceInvalidOperationException ex)
                {
                    message = "Debe iniciar sesión para utilizar la aplicación";
                }
            }


  
                // verificamos si existe algún mensaje para desplegar
                if (message != null)
                {
                    var dialog = new MessageDialog(message);
                    dialog.Commands.Add(new UICommand("OK"));
                    await dialog.ShowAsync();
                }
            return success;

        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void btnFacebook_Tapped(object sender, TappedRoutedEventArgs e)
        {
            progressBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
            await Authenticate();
            progressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            if (!Frame.Navigate(typeof(PivotPage), Helper.Helper.GetUserId(user.UserId)))
            {
                //throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

    }
}

