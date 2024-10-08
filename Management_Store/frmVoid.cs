using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Management_Store
{
    public partial class frmVoid : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        frmCancelDetails f;
        public frmVoid(frmCancelDetails frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;    
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void SaveCancelOrder(string user)
        {
            cn.Open();
            cm = new SqlCommand("insert into tblcancel (transno, pcode, price, qty, sdate, voidby, cancelledby, reason, action)values(@transno, @pcode, @price, @qty, @sdate, @voidby, @cancelledby, @reason, @action)", cn);
            cm.Parameters.AddWithValue("@transno", f.txtTransNo.Text);
            cm.Parameters.AddWithValue("@pcode", f.txtPCode.Text);
            cm.Parameters.AddWithValue("@price",double.Parse(f.txtPrice.Text));
            cm.Parameters.AddWithValue("@qty",int.Parse(f.txtCancelQty.Text));
            cm.Parameters.AddWithValue("@sdate", DateTime.Now);
            cm.Parameters.AddWithValue("@voidby", user);
            cm.Parameters.AddWithValue("@cancelledby", f.txtCancel.Text);
            cm.Parameters.AddWithValue("@reason", f.txtReason.Text);
            cm.Parameters.AddWithValue("@action", f.cboAction.Text);

            cm.ExecuteNonQuery();
            cn.Close();
        }

        public void UpdateData(string sql)
        {
            cn.Open();
            cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtPass.Text != String.Empty)
                {
                    string user;
                    cn.Open();
                    cm = new SqlCommand("select * from tbluser where username = @username and password = @password", cn);
                    cm.Parameters.AddWithValue("@username",txtUser.Text);
                    cm.Parameters.AddWithValue("@password", txtUser.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        user = dr["username"].ToString();
                        dr.Close();
                        cn.Close();

                        SaveCancelOrder(user);
                        if (f.cboAction.Text == "Yes")
                        {
                            UpdateData("update tblproduct set qty=qty + " + int.Parse(f.txtCancelQty.Text) + " where pcode = '" + f.txtPCode.Text + "'");
                        }

                        UpdateData("update tblcart set qty = qty - " + int.Parse(f.txtCancelQty.Text) + " where id like '" + f.txtID.Text + "'");

                        MessageBox.Show("Giao dịch đặt hàng đã hủy thành công!","Hủy đặt hàng",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        this.Dispose();
                        f.RefreshList();
                        f.Dispose();
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            catch (Exception ex) { 
                cn.Close();
                MessageBox.Show(ex.Message,"Cảnh báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        
    }
}
