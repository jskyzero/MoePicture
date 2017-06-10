using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoePicture.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        private const string shellKey = nameof(Views.Shell);

        public static string ShellKey => shellKey;

        public UserConfigViewModel ConfigViewModel => ServiceLocator.Current.GetInstance<UserConfigViewModel>();
        public PictureItemsViewModel PicturesViewModel => ServiceLocator.Current.GetInstance<PictureItemsViewModel>();

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(ShellKey, typeof(Views.Shell));
            SimpleIoc.Default.Register<INavigationService>(() => nav);

            SimpleIoc.Default.Register<UserConfigServer>();
            SimpleIoc.Default.Register<UserConfigViewModel>();
            SimpleIoc.Default.Register<PictureItemsViewModel>();
        }
    }
}
