using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string imag_path = path_string.Text;
            BitmapImage imag = new BitmapImage(); 
            imag.BeginInit();
            imag.UriSource = new Uri(imag_path, UriKind.Absolute); 
            imag.EndInit();

            orig_imag.Source = imag;

            red_imag.Source = ApplyColorFilter(imag, "Red");
            blue_imag.Source = ApplyColorFilter(imag, "Blue");
            green_imag.Source = ApplyColorFilter(imag, "Green");
        }

        private BitmapSource ApplyColorFilter(BitmapImage original, string color)
        {
            var bit_map = new FormatConvertedBitmap(original, PixelFormats.Bgra32, null, 0);
            int len = bit_map.PixelWidth * 4;
            byte[] pixelData = new byte[len * bit_map.PixelHeight];
            //המערך בעצם בנוי כך שכל 4 תאים מייצגים פיקסל, בסדר כחול,ירוק,אדום,שקיפות
            bit_map.CopyPixels(pixelData, len, 0);

            for (int i = 0; i < pixelData.Length; i += 4)//אותה לולאה רק צבע שונה
            {//קפיצות של 4 בלולאה כי בפורמט של ביט מאפ כל פיקסל מאופיין ב4 ערכים אדום ירוק כחול, ושקיפות
                switch (color)
                {
                    /*
                  * [i]=כחול
                  * [i+1]=ירוק
                  * [i+2]=אדום
                  */
                    case "Red":
                        pixelData[i] = 0;       
                        pixelData[i + 1] = 0;   
                        break;
                    case "Green":
                        pixelData[i] = 0;       
                        pixelData[i + 2] = 0;   
                        break;
                    case "Blue":
                        pixelData[i + 1] = 0;   
                        pixelData[i + 2] = 0;   
                        break;
                }
            }

            //ליצור תמונה מהמערך החדש
            var filteredBitmap = BitmapSource.Create(
                bit_map.PixelWidth,
                bit_map.PixelHeight,
                bit_map.DpiX,
                bit_map.DpiY,
                PixelFormats.Bgra32,
                null,
                pixelData,
                len);

            return filteredBitmap;
        }
    }
}
