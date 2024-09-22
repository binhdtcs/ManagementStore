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
    public partial class frmProductList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string stitle = "Simple POS system";
        public frmProductList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct(this);
            frm.btnSave.Enabled = true;
            frm.btnUpdate.Enabled = false;
            frm.LoadBrand();
            frm.LoadCategory();
            frm.ShowDialog();
        }

        public void LoadRecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("Select p.pcode,p.pdesc,b.brand,c.category,p.price,p.qty from tblProduct as p inner join tblBrand as b on b.id = p.bid inner join tblCategory as c on c.id = p.cid where p.pdesc like '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            cn.Close();
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName  = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName == "Edit")
            {
                frmProduct frm = new frmProduct(this);
                frm.btnSave.Enabled = false;
                frm.btnUpdate.Enabled = true; 
                frm.txtPcode.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                frm.txtPdesc.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                frm.txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                frm.cboBrand.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(); 
                frm.cboCategory.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                
                frm.ShowDialog();
            }
            else
            {
                if(MessageBox.Show("Bạn chắc chắn muốn xóa không?","Delete Record",MessageBoxButtons.YesNo ,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblproduct where pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'",cn);
                    cm.ExecuteNonQuery(); 
                    cn.Close();
                    LoadRecords();
                }
            }
        }
    }
}
