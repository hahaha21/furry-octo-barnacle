using Accord.Imaging.Converters;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Statistics.Analysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projekLabANN
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            predict.Enabled = false;
        }
 
        public ActivationNetwork bpnn_net;
        public BackPropagationLearning bpnn_learn;

        private void train_Click(object sender, EventArgs e)
        {
            double[][] input = new double[Data.getInstance().images.Count()][];
            double[][] output = new double[Data.getInstance().images.Count()][];
            int max = Data.getInstance().classes.Count() - 1;
            int min = 0;

            for (int i = 0; i < Data.getInstance().images.Count(); i++)
            {
                Bitmap img = Data.getInstance().preprocessing(Data.getInstance().images[i]);
                ImageToArray converter = new ImageToArray(0, 1);
                converter.Convert(img, out input[i]);

                output[i] = new double[1];
                output[i][0] = Data.getInstance().classindex[i];
                output[i][0] = 0 + (output[i][0] - min) * (1 - 0) / (max - min);
            }

            bpnn_net = new ActivationNetwork(new SigmoidFunction(), 100, 4, 1);
            bpnn_learn = new BackPropagationLearning(bpnn_net);

            double max_iter = 1000000 ;
            double max_err = 0.000001;

            for (double i = 0; i < max_iter; i++)
            {
                double err = bpnn_learn.RunEpoch(input, output);
                if (err < max_err) break;
            }

            predict.Enabled = true;
            MessageBox.Show("Computing BPNN Success");
        }

        private void predict_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Images Files (*.jpg, *jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                Bitmap img1 = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = img1;

                  


                
            }
            Bitmap img = new Bitmap(pictureBox1.Image);
            img = Data.getInstance().preprocessing(img);

            double[] input = new double[100];

            ImageToArray convert = new ImageToArray(0, 1);
            convert.Convert(img, out input);

            double[] output = bpnn_net.Compute(input);

            int result = Convert.ToInt32((Math.Round(output[0]) - 0) * (Data.getInstance().classes.Count - 1 - 0) / (1 - 0) + 0);

        


            textBox1.Text = Data.getInstance().classes[result];

            MessageBox.Show("Prediction Success");
        }
    }
}
