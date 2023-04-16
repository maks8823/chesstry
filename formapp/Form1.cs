using System;
using System.Timers;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace formapp
{
    public partial class Form1 : Form
    {
        System.Timers.Timer myTimer;
        private const int BOARD_SIZE = 8;
        private const int BOTTOM_MARGIN = 25;
        private int width;
        private int height;
        private bool clicked = false;
        private Rectangle[] rectangles;

        public Form1()
        {
            InitializeComponent();
            width = this.ClientSize.Width / BOARD_SIZE;
            height = (this.ClientSize.Height - BOTTOM_MARGIN) / BOARD_SIZE;
            rectangles = new Rectangle[BOARD_SIZE * BOARD_SIZE];
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    int index = row * BOARD_SIZE + col;
                    rectangles[index] = new Rectangle(col * width, row * height, width, height);
                }
            }
            
            myTimer = new System.Timers.Timer();
            myTimer.Interval = 1000;
            myTimer.Elapsed += new ElapsedEventHandler(Timer_update);
            myTimer.Start();
        }
        static void Timer_update(object sender, ElapsedEventArgs e)
        {
            if (myform == null || myform.IsDisposed)
            {
                return;
            }

            Form1.myform.Invoke(new MethodInvoker(delegate
            {
                myform.Text = DateTime.Now.ToString();
            }));
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Отрисовываем шахматную доску, используя сохраненные координаты клеток
            for (int i = 0; i < rectangles.Length; i++)
            {
                if ((i / BOARD_SIZE + i % BOARD_SIZE) % 2 == 0)
                {
                    e.Graphics.FillRectangle(Brushes.PeachPuff, rectangles[i]);
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.Sienna, rectangles[i]);
                }
            }
            if (clicked)
            {
                try
                {
                    Image atlas = Properties.Resources.chess;
                    Bitmap bitmap = new Bitmap(atlas);
                    // Определение размера фрагментов
                    int fragmentWidth = atlas.Width / 6; // 6 - количество фигур в строке
                    int fragmentHeight = atlas.Height / 2; // 2 - количество строк

                    // Создание массива фрагментов для белых и черных фигур
                    RectangleF[] whiteFigures = new RectangleF[6];
                    RectangleF[] blackFigures = new RectangleF[6];
                    for (int i = 0; i < 6; i++)
                    {
                        whiteFigures[i] = new RectangleF(i * fragmentWidth, 0, fragmentWidth, fragmentHeight);
                        blackFigures[i] = new RectangleF(i * fragmentWidth, fragmentHeight, fragmentWidth, fragmentHeight);
                    }
                    Graphics g = e.Graphics;
                    for (int i = 0; i < rectangles.Length; i++)
                    {
                        if (i == 0 || i == 7 || i == 56 || i == 63)
                        {
                            // Рисуем ладью
                            if (i <= 7)
                            {
                                g.DrawImage(bitmap, rectangles[i], whiteFigures[4], GraphicsUnit.Pixel);
                            }
                            else
                            {
                                g.DrawImage(bitmap, rectangles[i], blackFigures[4], GraphicsUnit.Pixel);
                            }
                        }
                        else if (i == 1 || i == 6 || i == 57 || i == 62)
                        {
                            // Рисуем коня
                            if (i <= 7)
                            {
                                g.DrawImage(bitmap, rectangles[i], whiteFigures[3], GraphicsUnit.Pixel);
                            }
                            else
                            {
                                g.DrawImage(bitmap, rectangles[i], blackFigures[3], GraphicsUnit.Pixel);
                            }
                        }
                        else if (i == 2 || i == 5 || i == 58 || i == 61)
                        {
                            // Рисуем слона
                            if (i <= 7)
                            {
                                g.DrawImage(bitmap, rectangles[i], whiteFigures[2], GraphicsUnit.Pixel);
                            }
                            else
                            {
                                g.DrawImage(bitmap, rectangles[i], blackFigures[2], GraphicsUnit.Pixel);
                            }
                        }
                        else if (i == 3 || i == 59)
                        {
                            // Рисуем ферзя
                            if (i <= 7)
                            {
                                g.DrawImage(bitmap, rectangles[i], whiteFigures[1], GraphicsUnit.Pixel);
                            }
                            else
                            {
                                g.DrawImage(bitmap, rectangles[i], blackFigures[1], GraphicsUnit.Pixel);
                            }
                        }
                        else if (i == 4 || i == 60)
                        {
                            // Рисуем короля
                            if (i <= 7)
                            {
                                g.DrawImage(bitmap, rectangles[i], whiteFigures[0], GraphicsUnit.Pixel);
                            }
                            else
                            {
                                g.DrawImage(bitmap, rectangles[i], blackFigures[0], GraphicsUnit.Pixel);
                            }
                        }
                        else if (i >= 8 && i <= 15)
                        {
                            // Рисуем белую пешку
                            g.DrawImage(bitmap, rectangles[i], whiteFigures[5], GraphicsUnit.Pixel);
                        }
                        else if (i >= 48 && i <= 55)
                        {
                            // Рисуем черную пешку
                            g.DrawImage(bitmap, rectangles[i], blackFigures[5], GraphicsUnit.Pixel);
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found");
                }

                
            }
        }
        private void RedrawFrame()
        {
            var r = new Rectangle(
                0, 0, this.Width, this.Height);
            this.Invalidate(r);
        }
        public void timerstop()
        {
            myTimer.Stop();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerstop();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            timerstop();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clicked = true;
            RedrawFrame();
        }
    }
}
