using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FacePractice
{
    class PicListData
    {

        private BitmapImage _ImageData;

        public PicListData(BitmapImage bitmapImage)
        {
            _ImageData = bitmapImage;
        }

        public BitmapImage ImageData
        {
            get { return this._ImageData; }
            set { this._ImageData = value; }
        }

    }
}
