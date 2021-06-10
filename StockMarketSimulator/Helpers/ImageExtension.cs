using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.Helpers
{
    // This class was created using a tutorial, it is NOT my work 
    // To add images, add the image in the images folder and set the action, in properties, to embedded image
    // To make sure the content is a source type
    [ContentProperty(nameof(Source))]

    // Class used to enable local image use
    public class ImageExtension : IMarkupExtension
    {
        // IMarkupExtension is needed  
        public string Source
        {
            get; set;
        }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            var imageResource = ImageSource.FromResource(Source, typeof(ImageExtension).GetTypeInfo().Assembly);
            return imageResource;
        }
    }
}
