using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TmxEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadTMXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = LoadFile("tmx");

            ProjectManager.Self.LoadTiledMapSave(fileName);
        }

        

        private string LoadFile(string extension)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*." + extension + ")|*." + extension + "";
            var result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void saveTMXAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = SaveFile("tmx");

            ProjectManager.Self.SaveTiledMapSave(fileName);
        }

        private string SaveFile(string extension)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "(*." + extension + ")|*." + extension + "";
            var result = fileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void loadTilesetPropertiesFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = LoadFile("tmx");

            ProjectManager.Self.LoadTilesetFrom(fileName);
        }
    }
}
