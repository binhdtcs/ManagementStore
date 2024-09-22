using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Management_Store
{
    public partial class frmStockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string stitle = "Simple POS system";
        public frmStockIn()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        public void LoadProduct()
        {
            int i = 0;
            cn.Open();
            cm = new SqlCommand("Select pcode, pdesc, price from tblproduct where pdesc like '%" + txtSearch.Text + "%' order by pdesc",cn );
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i,dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void frmStockIn_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "colSelect") { 
            
                if(MessageBox.Show("Thêm sản phẩm?",stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("insert into tblstockin(refno, pcode, sdate, stockinby)values(@refno, @pcode, @sdate, @stockinby)", cn);// * from tblproduct where pcode like '" +  + "'", cn);
                    cm.Parameters.AddWithValue("@refno",txtRefNo.Text);
                    cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cm.Parameters.AddWithValue("@sdate", dt1.Value);
                    cm.Parameters.AddWithValue("@stockinby",txtBy.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    MessageBox.Show("Thêm thành công",stitle,MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
               

            }    
        }
        public void LoadStockIn()
        {
            dataGridView2.Rows.Clear();
            cn.Open();

            cn.Close() ;
        }
    }
}
