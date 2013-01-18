using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TmxEditor.UI
{
    public partial class NewPropertyWindow : Form
    {
        #region Fields

        public string ResultName
        {
            get
            {
                return NameTextBox.Text;
            }
        }


        public string ResultType
        {
            get
            {
                return TypeComboBox.Text;
            }
        }

        #endregion


        public NewPropertyWindow()
        {
            InitializeComponent();

            FillComboBoxWithTypes();
            StartPosition = FormStartPosition.Manual;


            Location = new Point(MousePosition.X - Width/2, MousePosition.Y-Height/2);
        }

        private void FillComboBoxWithTypes()
        {
            TypeComboBox.Items.Add("string");
            TypeComboBox.Items.Add("float");
            TypeComboBox.Items.Add("int");
            TypeComboBox.Items.Add("bool");

        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
