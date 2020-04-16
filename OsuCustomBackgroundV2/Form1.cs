using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace OsuCustomBackgroundV2
{
    public partial class Form1 : Form
    {

        string osuPath;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            button3.Hide();
        }

        void PopulateTreeView()
        {

            string[] files = Directory.GetFiles(osuPath);

            foreach(string file in files)
            {
                if(file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".png"))
                {
                    string[] image = file.Split('\\');
                    treeView1.Nodes.Add(image[image.Length - 1]);
                }
                else
                {
                    MessageBox.Show("No valid image files found");
                    button1.Show();
                    button2.Show();
                    button3.Hide();
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (Directory.Exists(fbd.SelectedPath + @"\Data\" + @"bg\"))
                {
                    osuPath = fbd.SelectedPath + @"\Data\" + @"bg\";
                    //MessageBox.Show("Path set succesfully");
                    PopulateTreeView();
                    button1.Hide();
                    button2.Hide();
                    button3.Show();
                }
                else
                {
                    if(fbd.SelectedPath != "")
                    {
                        MessageBox.Show("Could not locate the seasonal background directory");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(Environment.CurrentDirectory + @"\Data\" + @"bg\"))
            {
                osuPath = Environment.CurrentDirectory + @"\Data\" + @"bg\";
                //MessageBox.Show("Path set succesfully");
                PopulateTreeView();
                button1.Hide();
                button2.Hide();
                button3.Show();
            }
            else
            {
                MessageBox.Show("Could not locate the seasonal background directory");
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!Directory.Exists(osuPath))
            {
                MessageBox.Show("Path does not exist");
                treeView1.Nodes.Clear();
                button1.Show();
                button2.Show();
                button3.Hide();
            }
            if(File.Exists(osuPath + treeView1.SelectedNode.Text))
            pictureBox1.BackgroundImage = new Bitmap(osuPath + treeView1.SelectedNode.Text);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.ShiftKey)
            {
                if (treeView1.Nodes.Count > 0)
                {
                    treeView1.Nodes[treeView1.SelectedNode.Index].Checked = !treeView1.Nodes[treeView1.SelectedNode.Index].Checked;
                }
            }

            if (e.KeyCode == Keys.A)
            {
                bool everythingSelected = true;
                if (treeView1.Nodes.Count > 0)
                {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if(node.Checked == false)
                        {
                            everythingSelected = false;
                        }
                    }

                    foreach(TreeNode node in treeView1.Nodes)
                    {
                        node.Checked = !everythingSelected;
                    }
                }
            }
        }

        void clearSelection()
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                node.Checked = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(osuPath))
            {
                MessageBox.Show("Path does not exist");
                treeView1.Nodes.Clear();
                button1.Show();
                button2.Show();
                button3.Hide();
            }

            if(treeView1.Nodes.Count < 1)
            {
                MessageBox.Show("Invalid action");
                return;
            }

            int checkedItems = 0;

            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Checked)
                {
                    checkedItems++;
                }
            }

            if(checkedItems < 1)
            {
                return;
            }


            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "*.png|*.jpg";
                DialogResult result = ofd.ShowDialog();

                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.Checked)
                    {
                        File.Move(osuPath + node.Text, osuPath + node.Text + ".old");
                        File.Copy(ofd.FileName, osuPath + node.Text);
                        File.Delete(osuPath + node.Text + ".old");
                    }

                }

                clearSelection();
            }
        }

    }
}
