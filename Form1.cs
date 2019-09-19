using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using BeautoRover;

namespace PBL2019_Robotics
{
    public partial class Form1 : Form
    {
        // カメラを取得するオブジェクトを作る
        // 创建视频捕获对象
        private VideoCapture capture;
        // ロボットを動くオブジェクトを作る
        // 创建小车动作对象
        BeautoRoverlib br = new BeautoRoverlib();
        public Form1()
        {
            // 初期化
            // 初始化
            InitializeComponent();
            // カメラ映像を取得する
            // 获取摄像头影像
            capture = VideoCapture.FromCamera(0);
            // カメラデバイスが正常にオープンしたか確認
            // 确认是否正常获取摄像头
            //textBox1.Text = Convert.ToString(capture.IsOpened());
        }

        // スタートバートンを押す
        // Start按钮按下事件
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            // 画面の高さを設定する
            // 设定画面高度
            capture.Set(CaptureProperty.FrameHeight, 240);
            // 画面の幅を設定する
            // 设定画面宽度
            capture.Set(CaptureProperty.FrameWidth, 320);
            // タイマー処理開始
            // timer开始
            timer1.Start();
            // シリアルポートを開く
            // 打开串口
            //textBox1.Text = br.OpenCOMPort("COM4");
            // スタートバートンを無効にする
            // Start按钮无效化
            buttonStart.Enabled = false;
            // ストーブバートンを有効にする
            // Stop按钮有效化
            buttonStop.Enabled = true;
        }

        // ストーブバートンを押す
        // Stop按钮按下事件
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            // タイマー処理停止
            // timer停止
            timer1.Stop();
            // シリアルポートを閉じる
            // 关闭串口
            //textBox1.Text = br.Close();
            // スタートバートンを有効にする
            // Start按钮有效化
            buttonStart.Enabled = true;
            // ストーブバートンを無効にする
            // Stop按钮无效化
            buttonStop.Enabled = false;
        }

        // タイマー処理
        // timer处理
        private void Timer1_Tick(object sender, EventArgs e)
        {
            // 映像をBGR空間からHSV空間に変更し，Matオブジェクトを作る
            // 创建Mat对象，将摄像头影像从BGR空间转换到HSV空间
            Mat img = capture.RetrieveMat().CvtColor(ColorConversionCodes.BGR2HSV);
            // オブジェクトがnullかどうかを判断
            // 对象null判断
            if (img != null)
            {
                // 映像を表示する
                // 显示摄像头影像
                pictureBox1.Image = img.ToBitmap();
                // メモリクリア
                // 清理内存
                /*if (GC.GetTotalMemory(false) > 600000)
                {
                    GC.Collect();
                }*/
                int sumPixelY = 0, sumPixelX = 0, count = 0;
                int minX = 320, minY = 240, maxX = 0, maxY = 0;
                for (int y = 0; y < img.Height; y++)
                    for (int x = 0; x < img.Width; x++)
                    {
                        byte h = img.At<Vec3b>(y, x)[0];
                        byte s = img.At<Vec3b>(y, x)[1];
                        byte v = img.At<Vec3b>(y, x)[2];

                        //if (r > 80 && b < 80 && g < 80)
                        if ((h < 180 && h > 160) && (s > 80) && (v < 220 && v > 50))
                        {
                            img.Set<Vec3b>(y, x, new Vec3b(255, 255, 255));
                            sumPixelY += y;
                            sumPixelX += x;
                            if (x < minX)
                                minX = x;
                            if (y < minY)
                                minY = y;
                            if (x > maxX)
                                maxX = x;
                            if (y > maxY)
                                maxY = y;
                            count++;
                        }
                        else
                        {
                            img.Set<Vec3b>(y, x, new Vec3b(0, 0, 0));
                        }
                        //textBox2.Text = Convert.ToString(capture.Get(CaptureProperty.Fps));
                    }
                Point center = new Point(sumPixelX / count, sumPixelY / count);
                int r = ((maxX - minX) + (maxY - minY)) / 4;
                img.Circle(center, r, Scalar.Green, 3);

                textBox1.Text = Convert.ToString(img.At<Vec3b>(img.Height / 2, img.Width / 2)[0]);
                textBox2.Text = Convert.ToString(img.At<Vec3b>(img.Height / 2, img.Width / 2)[1]);
                textBox3.Text = Convert.ToString(img.At<Vec3b>(img.Height / 2, img.Width / 2)[2]);

                // 処理した画像を表示
                // 显示处理后影像
                pictureBox2.Image = img.ToBitmap();
            }
            else
            {
                // オブジェクトがnullであればストーブ
                // 对象为null停止timer
                timer1.Stop();
            }

        }

    }
}

