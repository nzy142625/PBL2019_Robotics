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
        private VideoCapture capture;
        BeautoRoverlib br = new BeautoRoverlib();
        static int count = 0;
        public Form1()
        {
            // コンポーネント初期化
            // 组件初始化
            InitializeComponent();
            // カメラ映像を取得
            // 获取摄像头影像
            capture = VideoCapture.FromCamera(1);
            // カメラデバイスが正常にオープンしたか確認
            // 确认是否正常获取摄像头
            //textBox1.Text = Convert.ToString(capture.IsOpened());
        }

        // スタートバートンを押す処理
        // Start按钮按下事件
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            // 画面の高さを設定する
            // 设定画面高度
            capture.Set(CaptureProperty.FrameHeight, 240);
            // 画面の幅を設定する
            // 设定画面宽度
            capture.Set(CaptureProperty.FrameWidth, 320);
            textBox3.Text = Convert.ToString(capture.Fps);
            // タイマー処理開始
            // timer开始
            timer1.Start();
            // シリアルポートを開く
            // 打开串口
            textBox1.Text = br.OpenCOMPort("COM4");
            // スタートバートン無効化
            // Start按钮无效化
            buttonStart.Enabled = false;
            // ストーブバートン有効化
            // Stop按钮有效化
            buttonStop.Enabled = true;
        }

        // ストーブバートンを押す処理
        // Stop按钮按下事件
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            // タイマー処理停止
            // timer停止
            timer1.Stop();
            // シリアルポートを閉じる
            // 关闭串口
            textBox1.Text = br.Close();
            // スタートバートン有効化
            // Start按钮有效化
            buttonStart.Enabled = true;
            // ストーブバートン無効化
            // Stop按钮无效化
            buttonStop.Enabled = false;
        }

        // タイマー処理
        // timer处理
        private void Timer1_Tick(object sender, EventArgs e)
        {
            // 画像のオブジェクトを作る
            // 创建图像对象
            Mat srcImg;
            // 取得した映像をMatオブジェクトに転換
            // 捕获帧转换为Mat对象
            srcImg = capture.RetrieveMat();
            
            // オブジェクトがnullでない場合
            // 对象不为null时
            if (srcImg != null)
            {
                // メモリ解放
                // 清理内存
                if (GC.GetTotalMemory(false) > 600000)
                {
                    GC.Collect();
                }

                Point2f nowCtr, befCtr = new Point2f(-1, -1);
                float nowRad, befRad = 0;

                ImageProcessing(srcImg, out nowCtr, out nowRad);

                if (nowCtr != null && befCtr != null)
                {
                    //textBox2.Text = Convert.ToString((int)nowCtr.X) + "," + Convert.ToString((int)nowCtr.Y);
                    //textBox3.Text = Convert.ToString(nowRad);

                    if (count == 10)
                    {
                        count = 0;
                        befCtr = nowCtr;
                        befRad = nowRad;
                        //RobotControl(nowCtr, nowRad, befCtr, befRad);
                    }

                    count++;
                }
                else
                {
                    textBox1.Text = "error";
                }
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
