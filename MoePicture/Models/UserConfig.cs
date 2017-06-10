using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MoePicture.Models
{
    public class UserConfig : ObservableObject
    {
        #region Properties

        public enum rating { safe, all };

        private rating _raing;
        private bool _isPaneOpen;
        private ObservableCollection<string> _myTags = new ObservableCollection<string>();

        public string Path { get => ApplicationData.Current.LocalCacheFolder.Path; }
        public rating Raing { get => _raing; set { Set(ref _raing, value); } }
        public bool IsPaneOpen { get => _isPaneOpen; set { Set(ref _isPaneOpen, value); } }
        public ObservableCollection<string> MyTags { get => _myTags; set { Set(ref _myTags, value); } }

        #endregion Properties
    }
}
