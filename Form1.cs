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
            // コンポーネント初期化
            // 组件初始化
            InitializeComponent();
            // カメラ映像を取得
            // 获取摄像头影像
            capture = VideoCapture.FromCamera(0);
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
            // タイマー処理開始
            // timer开始
            timer1.Start();
            // シリアルポートを開く
            // 打开串口
            textBox1.Text = br.OpenCOMPort("COM6");
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
            // 元画像と処理画像のオブジェクトを作る
            // 创建原图像及处理图像对象
            Mat srcImg, procImg;
            // 取得した映像をMatオブジェクトに転換
            // 捕获帧转换为Mat对象
            srcImg = capture.RetrieveMat();
            // 処理オブジェクトにコピー
            // 复制帧
            procImg = srcImg.Clone();

            // オブジェクトがnullでない場合
            // 对象不为null时
            if (procImg != null)
            {
                // メモリ解放
                // 清理内存
                if (GC.GetTotalMemory(false) > 600000)
                {
                    GC.Collect();
                }

                // 映像をBGR空間からHSV空間に変更
                // 将影像从BGR空间转换到HSV空间
                procImg = procImg.CvtColor(ColorConversionCodes.BGR2HSV);
                // グレースケール化
                // 灰度处理
                procImg = procImg.CvtColor(ColorConversionCodes.BGR2GRAY);
                // メディアンフィルタリング
                // 中值滤波
                procImg = procImg.MedianBlur(15);
                // 二値化
                // 二值化
                procImg = procImg.Threshold(140, 255, ThresholdTypes.Binary);
                // モルフォロジー変換要素を作る
                // 创建形态学结构元素
                Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(15, 15));
                // ノイズ除去するためオープニング処理
                // 开运算去除噪点
                procImg = procImg.MorphologyEx(MorphTypes.Open, element);
                // 二値化映像を表示
                // 显示二值化图像
                pictureBox1.Image = procImg.ToBitmap();

                // 輪郭オブジェクトを作る
                // 创建轮廓对象
                MatOfPoint[] contours;
                // 輪郭検出
                // 轮廓检测
                contours = procImg.FindContoursAsMat(RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);
                // 輪郭存在する場合
                // 如果存在轮廓
                if (0 < contours.GetLength(0))
                {
                    // 輪郭により外接円の円心と半径を求める
                    // 求外接圆圆心及半径
                    contours[0].MinEnclosingCircle(out Point2f center, out float radius);
                    //indicate center & radius
                    textBox2.Text = center.ToString();
                    textBox3.Text = radius.ToString();
                    // 元画像に外接円を描画
                    // 在原图像上绘制外接圆
                    srcImg.Circle((Point)center, (int)radius, Scalar.Green);

                    Point2f mc = center;

                    //controll BeautoRover
                    if (center.X < 50)
                    {
                        textBox1.Text = br.TurnLeft();
                    }
                    else if (center.X > 270)
                    {
                        textBox1.Text = br.TurnRight();
                    }
                    else if (radius < 25)
                    {
                        textBox1.Text = br.Back();
                    }
                    else
                    {
                        textBox1.Text = br.Forward();
                    }
                }
                else {
                    br.Stop();
                }

                // 元画像を表示
                // 显示原图像
                pictureBox2.Image = srcImg.ToBitmap();
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
