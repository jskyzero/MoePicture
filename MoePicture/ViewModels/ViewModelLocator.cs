using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
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
        //private const string shellKey = nameof(Views.Shell);
        //public static string ShellKey => shellKey;

        public UserConfigVM ConfigVM => ServiceLocator.Current.GetInstance<UserConfigVM>();
        public ShellVM ShellVM => ServiceLocator.Current.GetInstance<ShellVM>();
        public PictureItemsVM PicturesVM => ServiceLocator.Current.GetInstance<PictureItemsVM>();

        static ViewModelLocator()
        {
            //SimpleIoc.Default.Register<INavigationService>(() => nav);
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //var nav = new NavigationService();
            //nav.Configure(ShellKey, typeof(Views.Shell));
            SimpleIoc.Default.Register<Services.ConfigService>();
            SimpleIoc.Default.Register<UserConfigVM>();
            SimpleIoc.Default.Register<PictureItemsVM>();
            SimpleIoc.Default.Register<ShellVM>();
        }
    }
}
