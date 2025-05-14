using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace ЛАБА_ТВИМС__1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chartForHitogramms.ChartAreas[0].AxisX.LabelStyle.Format = "F2";
        }

        private double Max;
        private double Min;
        private int initNumOfValuesReg = 0;
        private int initNumOfValuesNorm = 0;
        private int initNumOfValuesLinVal = 0;
        private int initNumOfBars = 0;
        private double[] regularDistrib;
        private double[] normalDistrib;
        double[] sample;
        double[] sample_Dist;
        double[] xi;
        double[] counts;

        private void button1_Click(object sender, EventArgs e)
        {
            int numOfvalues = (int)this.numericUpDown1.Value;
            int numOfBars = (int)this.numericUpDown2.Value;

            if (CheckForNewParametersReg(numOfvalues) || regularDistrib == null)
                regularDistrib = CreateArrayForRegularDistib(numOfvalues);

            ShowChart(regularDistrib, numOfvalues, numOfBars);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int numOfvalues = (int)this.numericUpDown1.Value;
            int numOfBars = (int)this.numericUpDown2.Value;

            if (CheckForNewParametersNorm(numOfvalues) || normalDistrib == null)
                normalDistrib = CreateArrayForNormalDistrib(numOfvalues);

            ShowChart(normalDistrib, numOfvalues, numOfBars);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            regularDistrib = null;
            regularDistrib = CreateArrayForRegularDistib((int)this.numericUpDown1.Value);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            normalDistrib = null;
            normalDistrib = CreateArrayForNormalDistrib((int)this.numericUpDown1.Value);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            int numOfvalues = (int)this.numericUpDown1.Value;
            int numOfBars = (int)this.numericUpDown2.Value;

            sample = null;
            xi = null;
            counts = null;

            xi = CreateBordersForLin(numOfBars);
            sample = CreateSampleForLin(numOfvalues, numOfBars);
            counts = CreateChecksBetweenBorders(numOfBars, sample, xi);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            chartForHitogramms.Series[0].Points.Clear();

            int numOfvalues = (int)this.numericUpDown1.Value;
            int numOfBars = (int)this.numericUpDown2.Value;

            if (CheckForNewParametersLin(numOfvalues, numOfBars))
            {
                xi = CreateBordersForLin(numOfBars);
                sample = CreateSampleForLin(numOfvalues, numOfBars);
                counts = CreateChecksBetweenBorders(numOfBars, sample, xi);
            }

            for (int i = 0; i < numOfBars; i++)
            {
                double binWidth = xi[i + 1] - xi[i];
                double height = Math.Round((counts[i] / (binWidth * (double)numOfvalues)) / 1.5, 2); // плотность!
                chartForHitogramms.Series[0].Points.AddXY($"{Math.Round(xi[i], 2)} - {Math.Round(xi[i + 1], 2)}", height);
            }
        }
                private double[] CreateBordersForLin(int numOfBars)
        {
            double[] xi = null;
            xi = new double[numOfBars + 1];
            for (int i = 0; i <= numOfBars; i++)
                xi[i] = 2 * Math.Sqrt((double)i / numOfBars); // xi = 2√(i/n)

            return xi;
        }
        private double[] CreateSampleForLin(int numOfvalues, int numOfBars)
        {
            Random rand = new Random();

            double[] sample = new double[numOfvalues];

            int idx = 0, idx_1 = 0;

            while (idx < numOfvalues)
            {
                double x = 2 * rand.NextDouble();
                double y = rand.NextDouble();
                if (y <= (Math.Pow(x, 2) / 4))
                {
                    sample[idx] = x;
                    idx++;
                }
            }
            Array.Sort(sample);

            return sample;
        }
        private double[] CreateChecksBetweenBorders(int numOfBars, double[] sample, double[] xi)
        {
            double[] counts = new double[numOfBars];
            // Шаг 3: Подсчёт количества элементов в интервалах
            foreach (double x in sample)
            {
                for (int i = 0; i < numOfBars; i++)
                {
                    if (x >= xi[i] && (i == numOfBars - 1 ? x <= xi[i + 1] : x < xi[i + 1]))
                    {
                        counts[i]++;
                        break;
                    }
                }
            }

            return counts;
        }
                private double[] CreateArrayForRegularDistib(int numberOfvalues)
        {
            double[] massive = new double[numberOfvalues];
            Random rand = new Random();

            for (int i = 0; i < numberOfvalues; i++)
                massive[i] = Math.Round(rand.NextDouble(), 3);     // Получение случайного числа в радиусе от 0 до 1

            Array.Sort(massive);
            return massive;
        }
        private double[] CreateArrayForNormalDistrib(int numberOfvalues)
        {
            double[] massive = new double[numberOfvalues];
            double sum;
            Random rand = new Random();

            for (int i = 0; i < numberOfvalues; i++)
            {
                sum = 0;
                for (int j = 0; j < 20; j++)
                    sum += rand.NextDouble();
                sum = Math.Round(sum, 3);
                massive[i] = sum;
            }

            Array.Sort(massive);
            return massive;
        }
        private bool CheckForNewParametersLin(int numOfvalues, int numOfbars)
        {
            if (initNumOfValuesLinVal != numOfvalues || initNumOfValuesLinBar != numOfbars)
            {
                initNumOfValuesLinVal = numOfvalues;
                initNumOfValuesLinBar = numOfbars;
                return true;
            }
            return false;
        }
        private bool CheckForNewParametersReg(int numOfvalues)
        {
            if (initNumOfValuesReg != numOfvalues)
            {
                initNumOfValuesReg = numOfvalues;
                return true;
            }
            return false;
        }
        private bool CheckForNewParametersNorm(int numOfvalues)
        {
            if (initNumOfValuesNorm != numOfvalues)
            {
                initNumOfValuesNorm = numOfvalues;
                return true;
            }
            return false;
        }
        private void ShowChart(double[] massive, int numOfvalues, int numOfBars)
        {
            chartForHitogramms.Series[0].Points.Clear();

            Min = massive[0];
            Max = massive[numOfvalues - 1];

            double step = (Max - Min) / numOfBars;
            double Xvalue = 0;

            double[] Yvalue = new double[numOfBars];

            int idx = 0;

            for (int i = 0; i < numOfBars; i++)
            {
                double lowborder = Min + i * step;
                double highborder = Min + (i + 1) * step;

                while (idx < numOfvalues)
                {
                    if (massive[idx] >= lowborder && massive[idx] < highborder)
                    {
                        Yvalue[i]++;
                        idx++;
                    }
                    else break;
                }
                Yvalue[i] = Math.Round(Yvalue[i] / (numOfvalues * step), 2);
            }

            for (int i = 0; i < numOfBars; i++)
            {
                Xvalue += step;
                chartForHitogramms.Series[0].Points.AddXY($"{Xvalue} - {Xvalue + step}", Yvalue[i]);
            }
        }
    }
}
