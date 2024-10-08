using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Management_Store
{
    public partial class frmChangePassword : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        DBConnection db = new DBConnection();
        frmPOS f;
        public frmChangePassword(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(db.MyConnection());
            f = frm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string _oldpass = db.GetPassword(f.lblUser.Text);
                if (_oldpass != txtOld.Text)
                {
                    MessageBox.Show("Mật khẩu cũ không chính xác", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(txtNew.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Mật khẩu nhập lại không chính xác", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MessageBox.Show("Bạn muốn thay đổi mật khẩu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new SqlCommand("Update tbluser set password=@password where username=@username", cn);
                        cm.Parameters.AddWithValue("@password",txtNew.Text);
                        cm.Parameters.AddWithValue("@username",f.lblUser.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Mật khẩu mới lưu lại thành công!", "Lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

       
    }
}
