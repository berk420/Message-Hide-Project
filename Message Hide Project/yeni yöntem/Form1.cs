using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yeni_yöntem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region Uniqe random value method
        Random rastgele = new Random();
        List<int> usedValues = new List<int>();
        public int UniqueRandomInt(int min, int max)
        {
            int val = rastgele.Next(min, max);
            while (usedValues.Contains(val))
            {
                val = rastgele.Next(min, max);
            }
            return val;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            #region our message has been parsed into bits to hide inside the picture
            string message_to_hide = "Hi";
            byte[] message_convert_to_bit = new byte[message_to_hide.Length * 8];
            int count_message_bit = 0;
            for (int i = 0; i < message_to_hide.Length; i++)
            {
                byte h = (byte)message_to_hide[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((h & 1) == 1)
                    {
                        message_convert_to_bit[count_message_bit] = (byte)(message_convert_to_bit[count_message_bit] | 1);
                    }
                    else
                    {
                        message_convert_to_bit[count_message_bit] = (byte)(message_convert_to_bit[count_message_bit] & 0);
                    }
                    h >>= 1;
                    listBox13.Items.Add(message_convert_to_bit[count_message_bit]);

                    count_message_bit++;

                }
            }
            #endregion

            #region printed the original image to the screen
            Bitmap img = new Bitmap(@"C:\Users\berkg\Desktop\Lena.tiff");
            pictureBox1.Image = img;
            #endregion

            #region took the bit-equivalent of the blue pixels in the image and assigned it to the two-dimensional array
            int width = img.Width;
            int height = img.Height;
            byte[] bites_1D_Blue = new byte[width * height * 8];
            byte[,] bites_2D_Blue = new byte[width * height, 8];
            int count = 0;
            int count2 = 0;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    count2++;
                    Color pixel = img.GetPixel(j, i);//img.GetPixel(height value, width value)
                    label1.Text = "Blue pixel value";
                    listBox1.Items.Add(pixel.B);
                    byte h = (byte)pixel.B;
                    for (int k = 0; k < 8; k++)
                    {
                        if ((h & 1) == 1)
                        {
                            bites_1D_Blue[count] = (byte)(bites_1D_Blue[count] | 1);
                        }
                        else
                        {
                            bites_1D_Blue[count] = (byte)(bites_1D_Blue[count] & 0);
                        }
                        h >>= 1;
                        count++;
                    }
                }
            }


            //burada blue pikselleri İŞLEM YAPACAĞIMIZ DİZİYE KOYDUK 
            int count3 = 0;

            for (int m = 0; m < count2; m++)
            {
                for (int n = 0; n <8 ; n++)
                {
                    bites_2D_Blue[m, n] = bites_1D_Blue[count3];
                    count3++;
                }
                
            }
            #endregion

            #region printed the blue pixels on the screen
            string print_blue;
            byte ındex_print_blue = 0;
            for (int i = 0; i < width*height; i++)
            {
                print_blue = bites_2D_Blue[i, 7].ToString() + bites_2D_Blue[i, 6].ToString() + bites_2D_Blue[i, 5].ToString() + bites_2D_Blue[i, 4].ToString() + bites_2D_Blue[i, 3].ToString() + bites_2D_Blue[i, 2].ToString() + bites_2D_Blue[i, 1].ToString() + bites_2D_Blue[i, 0].ToString();
                ındex_print_blue = Convert.ToByte(print_blue, 2);
                listBox4.Items.Add(print_blue);
            }
            #endregion

            #region randomly put the bits of our message into the bit values of the blue pixels
            int count_for_hidding = 0;
            int key = 0;
            int[] key_arr = new int[count_message_bit];
            for (int i = 0; i < count2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (j ==0  && (count_message_bit - 1) != count_for_hidding)
                    {
                                              key=UniqueRandomInt(0, count2);
                        key_arr[count_for_hidding] = key;
                        listBox2.Items.Add(key_arr[count_for_hidding]);
                        bites_2D_Blue[key, j] = message_convert_to_bit[count_for_hidding];
                        count_for_hidding++;
                    }
                }
            }
            #endregion

            #region wrote the bits of the blue pixels where the message is hidden to the screen
            string print_blue_after_hiding;
            byte ındex_print_blue_after_hiding = 0;
            for (int i = 0; i < width*height; i++)
            {
                print_blue_after_hiding = bites_2D_Blue[i, 7].ToString() + bites_2D_Blue[i, 6].ToString() + bites_2D_Blue[i, 5].ToString() + bites_2D_Blue[i, 4].ToString() + bites_2D_Blue[i, 3].ToString() + bites_2D_Blue[i, 2].ToString() + bites_2D_Blue[i, 1].ToString() + bites_2D_Blue[i, 0].ToString();
                ındex_print_blue_after_hiding = Convert.ToByte(print_blue_after_hiding, 2);
                listBox7.Items.Add(print_blue_after_hiding);
            }
            #endregion

            #region bits that the message is hidden are converted to bytes for printing on the screen
            string add_byte_to_string = "";
            string[] piksel_bit_string_arr = new string[width * height];
            for (int i = 0; i < width * height; i++)
            {
                for (int j = 8-1; j>-1; j--)
                {
                    add_byte_to_string += bites_2D_Blue[i, j].ToString();
                    if (j==0)
                    {
                        piksel_bit_string_arr[i] = add_byte_to_string;
                        add_byte_to_string = "";
                    }
                }
            }
            byte[] after_hiding_pixel_byte = new byte[width * height];
            for (int i = 0; i < after_hiding_pixel_byte.Length; ++i)
            {
                after_hiding_pixel_byte[i] = Convert.ToByte(piksel_bit_string_arr[i].Substring(8 * 0,8),2);
                listBox10.Items.Add(after_hiding_pixel_byte[i]);
            }

            #endregion

            #region printing the hidden byte data as a photo

            int[,] img_print = new int[width, height];
            int hide_message_count = 0;
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(j, i);
                    byte[] bgra = new byte[] { (byte)after_hiding_pixel_byte[hide_message_count], (byte)pixel.G, (byte)pixel.R, (byte)pixel.A };
                    img_print[i, j] = BitConverter.ToInt32(bgra, 0);
                    hide_message_count++;
                }

            }
            Bitmap bitmap;
            unsafe
            {
                fixed (int* intPtr = &img_print[0, 0])
                {
                    bitmap = new Bitmap(width, height, width*4, PixelFormat.Format32bppRgb, new IntPtr(intPtr));
                }
            }
            pictureBox2.Image = bitmap;
            #endregion
        }

       
    }
}


//if you want to retrive the hidden message as a string message
//it is very simple with bitwise
