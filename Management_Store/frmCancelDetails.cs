using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Management_Store
{
    public partial class frmCancelDetails : Form
    {
        frmSoldItems f;
        public frmCancelDetails(frmSoldItems frm)
        {
            InitializeComponent();
            f = frm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

      

        private void cboAction_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((cboAction.Text != String.Empty) && (txtQty.Text != String.Empty) && (txtReason.Text != String.Empty))
                {
                    if(int.Parse(txtQty.Text) >= int.Parse(txtCancelQty.Text))
                    {
                        frmVoid f = new frmVoid(this);
                        f.ShowDialog();
                    }
                        
                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void RefreshList()
        {
            f.LoadRecord();
        }

       
    }
}
