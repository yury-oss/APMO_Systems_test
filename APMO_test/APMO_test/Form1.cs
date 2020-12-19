using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace APMO_test
{
    public partial class Form1 : Form
    {
        bool stopSearching,pause = false;
        int countfiles;
        DateTime starttime;
        public Form1()
        {
           
            InitializeComponent();
            richTextBox1.Text = APMO_test.Properties.Settings.Default.path;
            textBox1.Text = APMO_test.Properties.Settings.Default.regex;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string subpath = richTextBox1.Text;
            string CurDir = System.IO.Path.GetFullPath(subpath);
            string myregex = textBox1.Text;
            char PathSeparator = '\\';
            countfiles = 0;
            label3.Text = "Время начала поиска: \n" + DateTime.Now.ToLongTimeString();
            treeView1.Nodes.Clear();
            starttime = DateTime.Now;

              OutputTreeView(CurDir, myregex, PathSeparator);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = folderBrowserDialog1.SelectedPath;
                richTextBox1.Text = filename;
            }
        }

       

         async   void OutputTreeView(string CurDir, string myregex, char PathSeparator)
        {
            DirectoryInfo DirectoryCurrent = new DirectoryInfo(CurDir);
            IEnumerable<string> allfiles = DirectoryCurrent.GetFiles(myregex, SearchOption.AllDirectories).Select(s => s.FullName.Substring(s.FullName.LastIndexOf(CurDir))).ToList();

            label6.Text = "Всего файлов: \n" +  allfiles.Count<string>().ToString();

            TreeNode LastNode = null;

            foreach (string PathToFile in allfiles)
            {
                if (stopSearching) break;

                    
                  richTextBox2.Text = System.IO.Path.GetDirectoryName(PathToFile);
                  string SubPathAgg = string.Empty;

                    foreach (string SubPath in PathToFile.Split(PathSeparator))
                    {
                    label8.Text ="Времени прошло \n" + (starttime-DateTime.Now).ToString();
                        await Task.Run(()=> {
                            while (pause)
                            {

                            }

                        });
                        countfiles++;
                        label5.Text = "Файлов найдено: \n" + countfiles.ToString();
                        await Task.Delay(1);

                        SubPathAgg += SubPath + PathSeparator;

                        TreeNode[] Nodes = treeView1.Nodes.Find(SubPathAgg, true);

                        if (Nodes.Length == 0)
                        {

                            if (LastNode == null)
                            {

                                LastNode = treeView1.Nodes.Add(SubPathAgg, SubPath);
                            }
                            else
                            {
                                LastNode = LastNode.Nodes.Add(SubPathAgg, SubPath);
                            }

                        }
                        else
                        {
                            LastNode = Nodes[0];
                        }

                    }
                
            }
                label4.Text = "Время конца поиска: \n" + DateTime.Now.ToLongTimeString();
                stopSearching = false;
                pause = false;
                button4.Text = "Пауза";

        }

       

        private void Button3_Click(object sender, EventArgs e)
        {
            stopSearching = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }
        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {
            
            APMO_test.Properties.Settings.Default.path = richTextBox1.Text;
            APMO_test.Properties.Settings.Default.regex = textBox1.Text;
            APMO_test.Properties.Settings.Default.Save();
            

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            pause = !pause;
            if (pause)
            {
            button4.Text = "Продолжить";

            }else
            {
                button4.Text = "Пауза";
            }
        }
    }
}
