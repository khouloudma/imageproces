using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
//using DirectShowLib;
using Emgu.CV.Features2D;
using System.Diagnostics;



namespace Exercice_Image
{
    public partial class Form1 : Form
    {
        // Create new stopwatch.
        Stopwatch stopwatch = new Stopwatch();

          public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();
           pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }
        Image<Bgr, Byte> ImageFrame;          //    
                                              Image<Bgr, Single> ImageColor;      //

        Capture capture;        //  takes images from camera as image frames
        private bool captureInProgress; //  checks if capture is executing

        //Image<Bgr, byte> img;
        //Image<Bgr, float> fimg;
        Image<Gray, Byte> ImageGray;      //il faut ajouter "opencv_imgproc220.dll"
        Image<Lab, Single> ImageLab;
        

        //public object ImageHsv { get; private set; }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // OK : ouvrir image depuis disque
                string FNom = openFileDialog1.FileName;
                listBox1.Items.Add(FNom.ToString());

                Image<Bgr, byte> img = new Image<Bgr, byte>(new Bitmap(@FNom));
                pictureBox1.Image = img.ToBitmap();


                img = img.Rotate(25, new Bgr(10, 10, 100));
                pictureBox2.Image = img.ToBitmap();

            }
        }
        //==========================================================================
        private void ProcessFrame(object sender, EventArgs arg)
        {
            ImageFrame = capture.QueryFrame();

            ImageColor = ImageFrame.Convert<Bgr, Single>() / 255; 	

		//SepareMask(ImageColor, trackBar1.Value, trackBar2.Value);
            	//textBox1.Text = " TBar(" + trackBar1.Value + "%, " + trackBar2.Value + "%)";

            //ImageHsv = ImageFrame.Convert<Hsv, byte>();   		
            
            ImageLab = ImageFrame.Convert<Lab, Single>() / 255;     	
            
            ImageGray = ImageFrame.Convert<Gray, byte>();

            //ImageGray = GetRMask(ImageFrame, trackBar1.Value, trackBar2.Value);
            // ImageFrame = ImageGray.Convert<Bgr, byte>();
            //           Image<Gray, Byte> smallGrayFrame = ImageGray.PyrDown();
            //            Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
            //            Image<Gay, Byte> cannyFrame = smoothedGrayFrame.Canny(new Gray(100), new Gray(60));
            //            pictureBox2.Image = cannyFrame.ToBitmap();
            //ImageColor = ImageFrame.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);
            pictureBox2.Image = ImageGray.ToBitmap();

            pictureBox4.Image = ImageFrame.ToBitmap();
            pictureBox3.Image = ImageLab.ToBitmap();

            // ImageColor.Canny(new Bgr(100, 100, 100), new Bgr(60, 60, 60));

            ImageGray = ImageFrame.Convert<Gray, byte>();

            Image<Gray, Byte> cannyFrame = ImageGray.Canny(new Gray(100), new Gray(60));
            pictureBox1.Image = cannyFrame.ToBitmap();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            #region if capture is not created, create it now
            if (capture == null)
            {
                try
                {
                    
                    capture = new Emgu.CV.Capture(0);
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            #endregion

            if (capture != null)
            {
                if (captureInProgress)
                {  //if camera is getting frames then stop the capture and set button Text
                   // "Start" for resuming capture

                    button2.Text = "Start!"; //
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    //if camera is NOT getting frames then start the capture and set button
                    // Text to "Stop" for pausing capture

                    button2.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }
        //======================================================================
        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

       
    } //class fin
} // namespace

