using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace GitBuildUtility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                filePath.Text = fbd.SelectedPath;
            }
            else
            {
                filePath.Text = "";
            }
        }

        private void build_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/c sh '" + scriptPath.Text + "' " + textBox1.Text + " " + textBox2.Text;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.WorkingDirectory = filePath.Text;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    string err = process.StandardError.ReadToEnd();
                    if(!err.Equals(""))
                    {
                        Console.WriteLine(err);
                        Application.Exit();
                    }
                    
                    process.WaitForExit();
                    MessageBox.Show("Build genration succeed!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); 
                }
                
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                scriptPath.Text = openFileDialog1.FileName;
            }
            else
            {
                scriptPath.Text = "";
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
        public bool validate()
        {

            if (filePath.Text.Equals(""))
            {
                MessageBox.Show("Please select local git repository!", "Error");
                return false;
            }
            if (scriptPath.Text.Equals(""))
            {
                MessageBox.Show("Please select script path. !", "Error");
                return false;
            }
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Please enter starting commit !", "Error");
                return false;
            }
            if (textBox2.Text.Equals(""))
            {
                MessageBox.Show("Please enter last commit!", "Error");
                return false;
            }
            return true;
        }
    }
}
