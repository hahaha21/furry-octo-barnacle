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
    public partial class Form1 : Form
    {
        private List<Bitmap> img_list;
        private List<string> name;
        ListViewGroup stationary;
        ListViewGroup artisan_tool;
        ListViewGroup cutlery;
        ListViewGroup cookingWare;


        public Form1()
        {
            InitializeComponent();
            img_list = new List<Bitmap>();
            name = new List<string>();
            stationary = new ListViewGroup("Stationary", HorizontalAlignment.Left);
            cutlery = new ListViewGroup("Cutlery", HorizontalAlignment.Left);
            cookingWare = new ListViewGroup("Cooking Ware", HorizontalAlignment.Left);
            artisan_tool = new ListViewGroup("Artisan Tool", HorizontalAlignment.Left);
            listView1.Groups.Add(stationary);
            listView1.Groups.Add(artisan_tool);
            listView1.Groups.Add(cookingWare);
            listView1.Groups.Add(cutlery);
            Data.getInstance().classes.Add("stationary");
            Data.getInstance().classes.Add("cutlery");
            Data.getInstance().classes.Add("artisan_tool");
            Data.getInstance().classes.Add("cookingWare");
        }

        private void browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Images Files (*.jpg, *jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 foreach(var path in openFileDialog1.FileNames)
                {
                    Bitmap img = new Bitmap(path);
                    imageList1.Images.Add(img);
                    img_list.Add(img);
                  
                   // temp.Images.Add(img);
                   
                    // imageList1.ImageSize = new Size(64, 64);
                    String filename = System.IO.Path.GetFileName(path);
                    name.Add(filename);
                    listView2.Items.Add(filename, imageList1.Images.Count-1);
                 
                    
                }
            }


        }

        private void remove_Click(object sender, EventArgs e)
        {
            if(listView2.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select data first", "",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for(int i =0 ; i <listView2.SelectedIndices.Count ; i++)
            {
                img_list.RemoveAt(listView2.SelectedIndices[i]);

                listView2.Items.RemoveAt(listView2.SelectedIndices[i]);
                


            }
          
        }

        private void upload_Click(object sender, EventArgs e)
        {
            
            if(listView2.Items.Count == 0)
            {
                MessageBox.Show("No picture to upload", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string selected = comboBox1.GetItemText(comboBox1.SelectedItem);

            for (int i = 0; i < listView2.Items.Count; i++)
            {

                imageList2.Images.Add(img_list[i]);
                //  imageList2.ImageSize = new Size(64, 64);
                ListViewItem item = new ListViewItem();
                item.Text = listView2.Items[i].Text;
                item.ImageIndex = imageList2.Images.Count - 1;
                String text = "";
                if (String.Compare(selected, "Stationary") == 0)
                {
                    item.Group = stationary;
                   
                    text = "stationary";
                }
                if (String.Compare(selected, "Cutlery") == 0)
                {
                    item.Group = cutlery;
                    
                    text = "cutlery";

                }
                if (String.Compare(selected, "Artisan Tool") == 0)
                {
                    item.Group = artisan_tool;
                  
                    text = "artisan_tool";

                }
                
                if (String.Compare(selected, "Cooking Ware") == 0)
                {
                    item.Group = cookingWare;
                  
                    text = "cookingWare";

                }

                listView1.Items.Add(item);

                Data.getInstance().images.Add(img_list[i]);
                Data.getInstance().filenames.Add(name[i]);
                Data.getInstance().classindex.Add(Data.getInstance().classes.IndexOf(text));


            }
          
            listView2.Items.Clear();
            imageList1.Images.Clear();
            img_list.Clear();
            name.Clear();

        }

        private void classify_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
            this.Hide();
        }

        private void search_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
            this.Hide();
        }
    }
}
