using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random random = new Random();

        private void Button_Click(object sender, EventArgs e)
        {
            var clickedButton = (Button)sender;

            AddButtonToPanel(clickedButton);

            this.Controls.Remove(clickedButton);

            EnableButton();

            RemoveButtons();
        }


        private void AddButtonToPanel(Button Button)
        {
            int sameImageCount = 0; // 相同图片的数量
            List<Button> sameImageButtons = new List<Button>(); // 存储相同图片的 Button
            int count = panel.Controls.Count;
            MemoryStream ms = new MemoryStream();
            Button.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String secondBitmap = Convert.ToBase64String(ms.ToArray());
            // 遍历 panel 中的 Button
            for (int i = count - 1; i <= count && i >= 0; i--)
            {
                Button existButton = panel.Controls[i] as Button;

                if (existButton != null)
                {
                    //判断两张图片是否一致的两种方法
                    ms.Position = 0;
                    existButton.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    String firstBitmap = Convert.ToBase64String(ms.ToArray());
                    if (secondBitmap.Equals(firstBitmap))
                    {
                        sameImageCount++;
                        if (sameImageButtons.Count == 0)
                        {
                            sameImageButtons.Add(Button);
                        }
                        int index = panel.Controls.IndexOf(existButton);
                        panel.Controls.Add(Button);
                        sameImageButtons.Add(existButton);
                        Button.Location = new Point(existButton.Location.X + 50, 0);
                        panel.Controls.SetChildIndex(Button, index + 1);
                    }
                }
            }
            if (sameImageCount == 2)
            {
                foreach (Button button in sameImageButtons)
                {
                    panel.Controls.Remove(button);
                }
                sameImageButtons.Clear();
                // 对 panel 中的 Button 进行重新排序
                for (int j = 0; j < panel.Controls.Count; j++)
                {
                    Button button = panel.Controls[j] as Button;
                    if (button != null)
                    {
                        button.Location = new Point((j % 7) * 50, 0);
                    }
                }
                return;
            }
            if (!panel.Controls.Contains(Button))
            {
                panel.Controls.Add(Button);
                Button.Location = new Point((count % 7) * 50, 0);
            }

            //对 panel 中的 Button 进行重新排序
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                Button button = panel.Controls[i] as Button;
                if (button != null)
                {
                    button.Location = new Point((i % 7) * 50, 0);
                }
            }
            if (panel.Controls.Count == 7)
            {
                MessageBox.Show("游戏结束！");
                //Form1.ActiveForm.Close();
                return;
            }
        }
        private void RemoveButtons()
        {

        }
        public Bitmap GetRandomButtonBackground()
        {
            List<Image> images = new List<Image>();
            for (int i = 0; i < 16; i++)
            {
                string imageFileName = @"C:\Users\苏梦\Desktop\3sheeps-master\图片素材\" + i + ".png";
                Image _image = Image.FromFile(imageFileName);
                images.Add(_image);
            }
            int imageIndex = random.Next(0, images.Count);
            // 复制图片来创建新的按钮背景
            Image image = images[imageIndex];
            Bitmap buttonBackground = new Bitmap(image);
            return buttonBackground;
        }

        private void EnableButton()
        {
            foreach (Button but in this.Controls.OfType<Button>())
            {
                but.Enabled = true;
            }

            foreach (Button btn1 in this.Controls.OfType<Button>())
            {
                foreach (Button btn2 in this.Controls.OfType<Button>())
                {
                    if (btn1.TabIndex > btn2.TabIndex)
                    {
                        // 检查是否有重叠
                        Rectangle rect1 = new Rectangle(btn1.Location, btn1.Size);
                        Rectangle rect2 = new Rectangle(btn2.Location, btn2.Size);
                        if (rect1.IntersectsWith(rect2))
                        {
                            // 比较 TabIndex
                            btn1.Enabled = false;
                            break;
                        }
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            List<diepai1> list1 = new List<diepai1>();

            int[] array1 = new int[360];
            array1 = Enumerable.Range(0, 360)
                        .OrderBy(n => (new Random(n).Next()))
                        .ToArray<int>();
            
            for (int i = 0; i < array1.Length / 3; i++)
            {
                Bitmap buttonBackground = GetRandomButtonBackground();
                for (int j = 0; j < 3; j++)
                {
                    var pai = new diepai1 { bitmap = buttonBackground, index = array1[i * 3 + j] };
                    list1.Add(pai);
                }
            }
            var pailist1 = list1.OrderBy(x => x.index).ToArray();
            int layers = random.Next(1,10);//随机生成图片层数
            for (int i = 0; i < layers; i++)
            {
                int offsetX = 25 * random.Next(-1, 2);
                int offsetY = 25 * random.Next(-1, 2);
                int len = random.Next(0, 13) * 3;//限制不显示的图片为3的倍数，防止不能通关
                int l = 0;//计数没有显示的图片数目
                /*
                 lx,ly防止其因为图片不显示而跳过图片
                 */
                int lx = 0;
                int ly = 0;
                for (int x = 0; x < 6; x++)
                {
                    lx++;
                    //int oy = random.Next(0, 6);
                    for (int y = 0; y < 6; y++)
                    {
                        int Mask = random.Next(0, 2);//判断这个图片显示与否,生成随机的图像
                        if (l < len)
                        {
                            l++;
                            if (Mask == 0)
                            {
                                continue;
                            }
                        }
                        /*if (Mask == 0)
                        {
                            continue;
                        }*/
                        Button but1 = new Button();
                        but1.Location = new System.Drawing.Point(x: 100 + 50 * x + offsetX, y: 100 + 50 * y + offsetY);
                        but1.Size = new System.Drawing.Size(width: 50, height: 50);
                        but1.Image = pailist1[i * 36 + lx * 6 + ly].bitmap;
                        but1.BackgroundImageLayout = ImageLayout.Zoom;
                        but1.TabIndex = i;
                        but1.Text = Convert.ToString("");
                        this.Controls.Add(but1);
                        but1.Click += new EventHandler(Button_Click);
                        ly ++;
                    }
                }
            }

            List<diepai2> list = new List<diepai2>();
            int size = random.Next(20, 48);
            int[] array2 = new int[size];
            array2 = Enumerable.Range(0, size)
                        .OrderBy(n => (new Random(n).Next()))
                        .ToArray<int>();
            for (int i = 0; i < array2.Length / 3; i++)
            {
                Bitmap buttonBackground = GetRandomButtonBackground();
                for (int j = 0; j < 3; j++)
                {
                    var pai = new diepai2 { bitmap = buttonBackground, index = array2[i * 3 + j] };
                    list.Add(pai);
                }
            }

            var pailist2 = list.OrderByDescending(x => x.index).ToArray();
            foreach (var item in pailist2)
            {
                Button but2 = new Button();
                but2.Location = new System.Drawing.Point(x: 100 + (48 - size) * 2 + 5 * item.index, y: 430);
                but2.Size = new System.Drawing.Size(width: 50, height: 50);
                but2.Image = item.bitmap;
                this.Controls.Add(but2);
                but2.Click += new EventHandler(Button_Click);
            }
            EnableButton();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear(); 
            InitializeComponent();
            Form1_Load(sender, e);
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    class diepai1
    {
        public Bitmap bitmap;
        public int index;
    }
    class diepai2
    {
        public Bitmap bitmap;
        public int index;
    }

}




