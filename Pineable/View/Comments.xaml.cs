﻿using Pineable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Comments : Page
    {
        NewCustom OBJ_NOTICIA;
        List<CommentCustom> lstComentarios = new List<CommentCustom> ();

        public Comments()
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
            NewCustom objNoticia = e.Parameter as NewCustom;

            if (objNoticia != null)
            {
                OBJ_NOTICIA = objNoticia;
                verificarConexion();

            }
        }

        private async void verificarConexion()
        {
            cargarDatos();

            if (App.NetworkAvailable)
            {
                //Hay conexión a Internet
                progressRing.IsActive = true;

                cargarDatos();
                //progressRing.IsActive = false;

            }
            else
            {
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("Verfique la conexión a Internet");
                await info.ShowAsync();
            }
        }

        private void cargarDatos()
        {
             lstComentarios = new List<CommentCustom>();

            for (int i = 0; i < 15; i++)
            {
                lstComentarios.Add(new CommentCustom() {Id = i.ToString(),Date = DateTime.Now,IdNew = "1",IdUser = "1", UserName = "Nombre usuario" , Description = "Ejemplo Comentario", UserPictureURL = "ms-appx:///Assets/user.png" });

            }

            lstvComentarios.ItemsSource = lstComentarios;

            progressRing.IsActive = false;
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            // mostramos la barra de carga
            progressRing.IsActive = true;

            //IMobileServiceTable<Comentario> tableComentario = App.MobileService.GetTable<Comentario>();
            Comment objComentarioNuevo = new Comment();

            // establecemos el id de la noticia
            objComentarioNuevo.IdNew = OBJ_NOTICIA.Id;

            // establecemos el id del usuario, en este caso obtenemos este id mediante el que se encuentra autenticado
            //objComentarioNuevo.IdUser = Helper.Helper.GetUserId(App.MobileService.CurrentUser.UserId);
            objComentarioNuevo.IdUser = "1";

            // establecemos la descripción
            objComentarioNuevo.Description = txtComment.Text.Trim();

            // limpiamos el textbox
            txtComment.Text = "";

            // obtenmos la fecha
            objComentarioNuevo.Date = DateTime.Now;

            // agregamos el comentario
            //await tableComentario.InsertAsync(objComentarioNuevo);
            

            // recargamos
            //CargarComentarios();

            // ocultamos la barra de carga
            progressRing.IsActive = false;
        }
    }
}
