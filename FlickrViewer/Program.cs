using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlickrViewer
{
    static class Program
    {
        /*Question 2

        You are asked to use Parallel class to modify FlickrViewer app 
        by adding image resizing functionality. More specifically, 
        resize found image(s) and save these resized image locally. 
        You can find the details about how to resize the image  
        from https://www.andrewhoefling.com/Home/post/basic-image-manipulation-in-c-sharp

         */
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FickrViewerForm());
        }
    }
}
