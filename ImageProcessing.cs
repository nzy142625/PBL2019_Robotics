using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using BeautoRover;

namespace PBL2019_Robotics
{
	public partial class Form1:Form
	{
        public const int MIN_H = 170;
        public const int MIN_S = 100;
        public const int MIN_V = 100;
        public const int MAX_H = 10;
        public const int MAX_S = 255;
        public const int MAX_V = 255;
        private void ImageProcessing(Mat srcImg)
        {
            Mat procImg;
            // 処理オブジェクトにコピー
            // 复制帧
            procImg = srcImg.Clone();
            // 映像をBGR空間からHSV空間に変更
            // 将影像从BGR空间转换到HSV空间
            procImg = procImg.CvtColor(ColorConversionCodes.BGR2HSV);

            //procImg = procImg.CvtColor(ColorConversionCodes.BGR2GRAY);
            
            for(int i = 0; i < 240; i++)
                for(int j = 0; j < 320; j++)
                {
                    if(procImg.At<Vec3b>(i, j)[0] < MIN_H && procImg.At<Vec3b>(i, j)[0] > MAX_H)
                    {
                        procImg.Set<Vec3b>(i, j,new Vec3b(procImg.At<Vec3b>(i, j)[0],0,0));
                    }
                }  
            textBox1.Text = Convert.ToString(procImg.At<Vec3b>(120, 160)[0]);
            textBox2.Text = Convert.ToString(procImg.At<Vec3b>(120, 160)[1]);
            textBox3.Text = Convert.ToString(procImg.At<Vec3b>(120, 160)[2]);

            procImg = procImg.CvtColor(ColorConversionCodes.BGR2GRAY);


            //pictureBox1.Image = procImg.ToBitmap();

            // メディアンフィルタリング
            // 中值滤波
            procImg = procImg.MedianBlur(25);

            
            // 二値化
            // 二值化
            // グレースケール化
            // 灰度处理
            //Scalar minRed = new Scalar(MIN_H, MIN_S, MIN_V);
            //Scalar maxRed = new Scalar(MAX_H, MAX_S, MAX_V);

            //Cv2.InRange(procImg, minRed, maxRed, procImg);
            
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
                // 元画像に外接円を描画
                // 在原图像上绘制外接圆
                srcImg.Circle((Point)center, (int)radius, Scalar.Green);
            }

            srcImg.Circle(new Point(160, 120), 1, Scalar.Green);
            // 元画像を表示
            // 显示原图像
            pictureBox2.Image = srcImg.ToBitmap();
        }
	}
}