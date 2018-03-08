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
    public partial class Form3 : Form
    {
        private PrincipalComponentAnalysis pca;
        private DistanceNetwork som_network;
        private SOMLearning som_learning;

        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Images Files (*.jpg, *jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                Bitmap img1 = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = img1;





            }
            Bitmap img = new Bitmap(pictureBox1.Image);

            img = Data.getInstance().preprocessing(img);

            double[] input = new double[10 * 10];
            ImageToArray converter = new ImageToArray(0, 1);

            converter.Convert(img, out input);

            double[] input_from_pca = pca.Transform(input);
            som_network.Compute(input_from_pca);
            double winner = som_network.GetWinner();

            for (int i = 0; i < Data.getInstance().images.Count; i++)
            {
                Bitmap img2 = Data.getInstance().preprocessing(Data.getInstance().images[i]);

                double[] input2 = new double[10 * 10];

                ImageToArray converter2 = new ImageToArray(0, 1);
                converter2.Convert(img2, out input2);

                double[] input_from_pca2 = pca.Transform(input2);
                som_network.Compute(input_from_pca2);
                double winner2 = som_network.GetWinner();

                if (winner == winner2)
                {
                    imageList1.Images.Add(Data.getInstance().images[i]);
                    listView1.Items.Add(Data.getInstance().filenames[i], imageList1.Images.Count - 1);
                    
                }
            }
            MessageBox.Show("Find Simillar Item Success");
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            double[][] input_data = new double[Data.getInstance().images.Count][];
            double[][] output_data = new double[Data.getInstance().images.Count][];
            int max = Data.getInstance().classes.Count - 1;
            int min = 0;

            for (int i = 0; i < Data.getInstance().images.Count; i++)
            {
                Bitmap img = Data.getInstance().preprocessing(Data.getInstance().images[i]);

                ImageToArray converter = new ImageToArray(0, 1);
                converter.Convert(img, out input_data[i]);

                output_data[i] = new double[1];
                output_data[i][0] = Data.getInstance().classindex[i];
                output_data[i][0] = 0 + (output_data[i][0] - min) * (1 - 0) / (max - min);
            }

            pca = new PrincipalComponentAnalysis();
            pca.Method = PrincipalComponentMethod.Center;
            pca.Learn(input_data);

            double[][] input_from_pca = pca.Transform(input_data);

            int a = 0;
            int output_count = 0 ;
            while (a < Data.getInstance().classes.Count)
            {
                output_count = a * a;
                a++;
            }

            som_network = new DistanceNetwork(input_from_pca[0].Count(), output_count);
            som_learning = new SOMLearning(som_network);

            double max_iter = 1000000;
            double max_err = 0.000001;

            for (int i = 0; i < max_iter; i++)
            {
                double error = som_learning.RunEpoch(input_from_pca);
                if (error < max_err) break;
            }

           
        }
    }
}
