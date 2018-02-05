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

        public enum RatingType { safe, all };

        private RatingType rating;
        private ObservableCollection<string> myTags = new ObservableCollection<string>();

        public string SaveFolderlToken { get; set; }
        public string CacheFolderToken { get; set; }
        public RatingType Rating { get => rating; set { Set(ref rating, value); } }
        public ObservableCollection<string> MyTags { get => myTags; set { Set(ref myTags, value); } }

        #endregion Properties
    }
}
