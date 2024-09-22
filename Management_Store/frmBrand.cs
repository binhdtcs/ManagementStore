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
    public partial class frmBrand : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        frmBrandList frmlist;
        public frmBrand(frmBrandList flist)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmlist = flist;
        }

        private void frmThuongHieu_Load(object sender, EventArgs e)
        {

        }

        private void Clear()
        {
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtBrand.Clear();
            txtBrand.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn lưu thương hiệu này không?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblBrand(Brand)VALUEs(@brand)", cn);
                    cm.Parameters.AddWithValue("@brand", txtBrand.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Lưu thành công");
                    Clear();
                    frmlist.LoadRecords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Bạn có muốn cập nhật thương hiệu này không?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblbrand set brand = @brand where id like '" + lblID.Text + "'",cn);
                    cm.Parameters.AddWithValue("@brand", txtBrand.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Cập nhật thành công");
                    Clear();
                    frmlist.LoadRecords();
                    this.Dispose();
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
