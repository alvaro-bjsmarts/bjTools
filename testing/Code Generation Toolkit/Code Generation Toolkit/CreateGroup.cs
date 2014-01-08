using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Code_Generation_Toolkit
{
    public partial class CreateGroup : Form
    {
       

        public CreateGroup()
        {
            InitializeComponent();           
        }

        public string[] sendnames { get; set; }


        public string[] name = new string[20];
        
        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            int count = 0;

            if (sendnames != null)
            {

                for (int n = 0; n < sendnames.Count(); n++)
                {
                    if (sendnames[n] != null)
                    {
                        count++;
                    }
                }

            }
                if (count != 0)
                {
                    name[count + 1] = txtGroupName.Text;
                }
                else
                {
                    name[count] = txtGroupName.Text;
                }

                Main obj = (Main)Application.OpenForms["Main"];
                obj.fiillDdl(name, count);

            
            this.Close();
        }             
    }
}
