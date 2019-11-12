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
    public partial class Form1 : Form
    {
        public const float LEFT = 110;
        public const float RIGHT = 210;
        public const float OFFSET = 5;
        //public const int TURN_
        public const int TIME_SLEEP = 5;

        //private String TurnLeft()
        //{ 
        //    for(int i = 0; i < )
        //}
        private void RobotControl(Point2f nowCtr, float nowRad, Point2f befCtr, float befRad)
        {
            if (nowCtr == null || befCtr == null)
            {
                textBox1.Text = "null";
                return;
            }
            if (nowCtr == new Point2f(-1, -1) || nowRad == 0)
            {
                textBox1.Text = br.Stop();
            }
            if (nowCtr.X <= RIGHT && nowCtr.X >= LEFT)
            {
                if (OFFSET > Math.Abs(befRad - nowRad))
                {
                    textBox1.Text = br.Stop();
                }
                else
                {
                    if (nowRad < befRad)
                        textBox1.Text = br.Forward();
                    else
                        textBox1.Text = br.Back();
                }
            }
            else if (nowCtr.X < LEFT)
            {
                if (OFFSET < befRad - nowRad)
                {
                    textBox1.Text = br.TurnLeft();
                    System.Threading.Thread.Sleep(15);
                    textBox1.Text = br.Forward();
                }
                else
                {
                    textBox1.Text = br.TurnLeft();
                    //textBox1.Text = 
                }
            }
            else
            {
                if (OFFSET < befRad - nowRad)
                {
                    textBox1.Text = br.TurnRight();
                    System.Threading.Thread.Sleep(15);
                    textBox1.Text = br.Forward();
                }
                else
                {
                    textBox1.Text = br.TurnRight();
                }
            }

        }
    }
}