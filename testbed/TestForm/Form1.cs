using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TiledMap;
using FlatRedBall.Content;
using FlatRedBall.Content.AI.Pathfinding;
using FlatRedBall.Content.Math.Geometry;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tmxButton_Click(object sender, EventArgs e)
        {
            tmxFileDialog.ShowDialog();
        }

        private void tmxFiledialogOk(object sender, CancelEventArgs e)
        {
            tmxFilename.Text = tmxFileDialog.FileName;
        }

        

        private void tmxDestinationButton_Click(object sender, EventArgs e)
        {
            DialogResult result = tmxFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tmxDestinationFolder.Text = tmxFolderDialog.SelectedPath;
            }
        }

        private void tmxConvertToScnx_Click(object sender, EventArgs e)
        {
            TiledMapSave save = TiledMapSave.FromFile(tmxFilename.Text);
            SpriteEditorScene ses = save.ToSpriteEditorScene();
            string pathtosave = tmxDestinationFolder.Text + GetFilename(tmxFilename.Text) + ".scnx";
            ses.Save(pathtosave);
        }

        private void tmxConvertToNntx_Click(object sender, EventArgs e)
        {
            TiledMapSave save = TiledMapSave.FromFile(tmxFilename.Text);
            NodeNetworkSave nns = save.ToNodeNetworkSave();
            string pathtosave = tmxDestinationFolder.Text + GetFilename(tmxFilename.Text) + ".nntx";
            nns.Save(pathtosave);
        }

        private void tmxConvertToShcx_Click(object sender, EventArgs e)
        {
            TiledMapSave save = TiledMapSave.FromFile(tmxFilename.Text);
            ShapeCollectionSave scs = save.ToShapeCollectionSave();
            string pathtosave = tmxDestinationFolder.Text + GetFilename(tmxFilename.Text) + ".shcx";
            scs.Save(pathtosave);
        }

        private static string GetFilename(string filepath)
        {
            return filepath.Substring(filepath.LastIndexOf("\\") + 1).Replace(".tmx", "");
        }

        private void tmxCSVButton_Click(object sender, EventArgs e)
        {
            TiledMapSave save = TiledMapSave.FromFile(tmxFilename.Text);
            string csv = save.ToCSVString();
            System.IO.File.WriteAllText(tmxDestinationFolder.Text + GetFilename(tmxFilename.Text) + ".csv", csv);
        }





    }
}
