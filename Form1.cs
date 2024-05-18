using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаб5
{
    public partial class Form1 : Form
    {
        bool allowclick = false;  //переменная, которая определяет, можно ли кликать на картинки
        PictureBox firstGuess; //переменная, которая будет хранить первую картинку, на которую кликнул пользователь.
        Random rnd = new Random(); 
        Timer clickTimer = new Timer(); // переменная, для создания задержки
        int time = 60;
        Timer timer = new Timer { Interval = 1000 }; // таймер для обратного отсчета
        public Form1()
        {
            InitializeComponent();
        }
        private PictureBox[] pictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }
        private static IEnumerable<Image> images
        {
            get
            {
                return new Image[]
                {
                    Properties.Resources.img1,
                    Properties.Resources.img2,
                    Properties.Resources.img3,
                    Properties.Resources.img4,
                    Properties.Resources.img5,
                    Properties.Resources.img6,
                    Properties.Resources.img7,
                    Properties.Resources.img8,
                };
            }
        }
        private bool messageShown = false;

        private void startGametimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time--;
                if (time < 0 && !messageShown)
                {
                    MessageBox.Show("Время вышло!");
                    messageShown = true;
                    timer.Stop();
                    ResetImages();
                }
                var ssTime = TimeSpan.FromSeconds(time);
                label1.Text = "00: " + time.ToString();
            };
        }

        private void ResetImages()
        {
            foreach (var pic in pictureBoxes) 
            {
                pic.Tag = null;
                pic.Visible = true; //делает каждый PictureBox видимым
            }
            HideImages(); //скрывает все изображения
            setRandomImages();
            time = 60;
            if (!messageShown)
            {
                timer.Start();
            }
        }
        private void HideImages() //скрывает изображение, устанавливая "рубашки"
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Image = Properties.Resources.shirt;
            }
        }

        private PictureBox getFreeSlot()
        {
            var freePictureBoxes = new List<PictureBox>();
            foreach (var pb in pictureBoxes)
            {
                if (pb.Tag == null)
                {
                    freePictureBoxes.Add(pb);
                }
            }
            if (freePictureBoxes.Count > 0)
            {
                int num = rnd.Next(0, freePictureBoxes.Count);
                Console.WriteLine(num);
                return freePictureBoxes[num];
            }
            else
            {
                return null;
            }
        }



        private void SetPictureBoxSizeMode()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void setRandomImages()
        {
            Array.ForEach(pictureBoxes, pictureBox => pictureBox.Tag = null);
            foreach (var image in images)
            {
                AssignImageToFreeSlot(image);
                AssignImageToFreeSlot(image);
            }
            SetPictureBoxSizeMode();
        }

        private void AssignImageToFreeSlot(object image)
        {
            var freeSlot = getFreeSlot();
            if (freeSlot != null)
            {
                freeSlot.Tag = image;
            }
        }

        private void CLICKTIMER_TICK (object sender, EventArgs e)
        {
            HideImages();

            allowclick = true;
            clickTimer.Stop();
        }

        private void clickImage(object sender, EventArgs e)
        {
           
                if (!allowclick) return;
                var pic = (PictureBox)sender;
                if (firstGuess == null)
                {
                    firstGuess = pic;
                    pic.Image = (Image)pic.Tag;
                    return;
                }
                pic.Image = (Image)pic.Tag;
                if (pic.Image == firstGuess.Image && pic != firstGuess)
                {
                    pic.Visible = firstGuess.Visible = false;
                    {
                        firstGuess = pic;
                    }
                    HideImages();
                }
                else
                {
                    allowclick = false;
                    clickTimer.Start();
                }
                firstGuess = null;
                if (pictureBoxes.Any(p => p.Visible)) return;
                timer.Stop(); // Остановите таймер
                MessageBox.Show("Ты выиграл! Игра закончена.");
                button1.Enabled = true;
                allowclick = false;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            allowclick = true;
            setRandomImages();
            HideImages();
            startGametimer();
            clickTimer.Interval = 1000;
            clickTimer.Tick += CLICKTIMER_TICK;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
