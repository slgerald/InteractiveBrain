using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InteractiveBrain
{
    //From https://stackoverflow.com/questions/4800597/wpf-detect-image-click-only-on-non-transparent-portion
    public class OpaqueClickableImage : Image
    {
        public OpaqueClickableImage() { }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            var source = (BitmapSource)Source;
            var x = (int)(hitTestParameters.HitPoint.X / ActualWidth * source.PixelWidth);
            var y = (int)(hitTestParameters.HitPoint.Y / ActualHeight * source.PixelHeight);
            if (x == source.PixelWidth)
                x--;
            if (y == source.PixelHeight)
                y--;
            var pixels = new byte[4];
            source.CopyPixels(new Int32Rect(x, y, 1, 1), pixels, 4, 0);
            if (pixels[3] < 1) return null;
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

    }
}
