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
    public partial class frmSecurity : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmSecurity()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           if(MessageBox.Show("THOÁT KHỎI ỨNG DỤNG","XÁC NHẬN", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string _username="", _role="", _name="";
            try
            {
                bool found = false;
                cn.Open();
                cm = new SqlCommand("Select * from tblUser where username = @username and password = @password", cn);
                cm.Parameters.AddWithValue("@username", txtUser.Text);
                cm.Parameters.AddWithValue("@password", txtPass.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;   
                    _username = dr["username"].ToString();
                    _role = dr["role"].ToString();
                    _name = dr["name"].ToString();
                }
                else
                {
                    found = false;
                }
                dr.Close();
                cn.Close();

                if (found == true)
                {

                    if (_role == "Thu ngân")
                    {
                        MessageBox.Show("Chào mừng " + _name + "!", "ĐĂNG NHẬP THÀNH CÔNG", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                        txtPass.Clear();
                        txtUser.Clear();
                        this.Hide();
                        frmPOS frm = new frmPOS(this);
                        frm.lblUser.Text = _username;
                        frm.lblName.Text = _name + " | " + _role;
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Chào mừng " + _name + "!", "ĐĂNG NHẬP THÀNH CÔNG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPass.Clear();
                        txtUser.Clear();
                        this.Hide();
                        Form1 frm = new Form1();
                        frm.lblname.Text = _name;
                     
                        frm.lblRole.Text = _role;
                        frm.MyDashboard();
                        frm.ShowDialog();
                    }

                }
                else {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "ĐĂNG NHẬP THẤT BẠI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
             catch (Exception ex) {
                cn.Close();
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    
    }
}
