using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSForm2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 32;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        long tenBillion = 10000000000;

        decimal Gauss(decimal num)
        {
            return (num * (num + 1)) / 2;
        }

        decimal Sum(long num)
        {
            decimal sum = 0;
            for (long i = 1; i <= num; i++)
            {
                sum += i;
            }

            return sum;
        }

        void DoSum(int i)
        {
            var watch = Stopwatch.StartNew();
            decimal sum = Sum(tenBillion);
            watch.Stop();
            string message = (i + 1) + ". thread = " + sum + " Elapsed Time = " + watch.ElapsedMilliseconds + "\n";
            dataGridView1.Rows[i].Cells[0].Value = i + 1;
            dataGridView1.Rows[i].Cells[1].Value = sum;
            dataGridView1.Rows[i].Cells[2].Value = watch.ElapsedMilliseconds;
        }

        public void CreateChart()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row!=null)
                {
                    chart1.Series["Threads"].Points.AddXY((row.Index) + 1, row.Cells[2].Value);
                    chart1.ChartAreas[0].AxisX.Title = "Threads";
                    chart1.ChartAreas[0].AxisY.Title = "Elapsed Time (ms)";
                }
                
            }
            //chart1.Series["Series1"].Points.AddXY(1, 700);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Text += ("Gauss Result = " + Gauss(tenBillion) + "\n");
            var watch_total = Stopwatch.StartNew();
            Thread[] threads = new Thread[32];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => DoSum(i));
                threads[i].Start();
                threads[i].Join();
                CreateChart();

            }
            watch_total.Stop();
            label2.Text = " Total Elapsed Time = " + watch_total.ElapsedMilliseconds + " ms";
            //Console.WriteLine(" Total Elapsed Time = " + watch_total.ElapsedMilliseconds + "\n");
            //CreateChart();
        }
    }
}
