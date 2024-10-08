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
    public partial class frmSearchProductStockin : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string stitle = "Simple POS system";
        frmStockIn slist;
        public frmSearchProductStockin(frmStockIn flist)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            slist = flist;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadProduct()
        {
            int i = 0;
            cn.Open();
            cm = new SqlCommand("Select pcode, pdesc, price from tblproduct where pdesc like '%" + txtSearch.Text + "%' order by pdesc", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "colSelect")
            {
                if (slist.txtRefNo.Text == string.Empty) { MessageBox.Show("Vui lòng nhập vào ReferenceNo", stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); slist.txtRefNo.Focus(); return; }
                if (slist.txtBy.Text == string.Empty) { MessageBox.Show("Vui lòng nhập vào người mua", stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); slist.txtBy.Focus(); return; }
                if (MessageBox.Show("Thêm sản phẩm?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("insert into tblstockin(refno, pcode, sdate, stockinby)values(@refno, @pcode, @sdate, @stockinby)", cn);// * from tblproduct where pcode like '" +  + "'", cn);
                    cm.Parameters.AddWithValue("@refno",slist.txtRefNo.Text);
                    cm.Parameters.AddWithValue("@pcode",dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cm.Parameters.AddWithValue("@sdate",slist.dt1.Value);
                    cm.Parameters.AddWithValue("@stockinby",slist.txtBy.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    MessageBox.Show("Thêm thành công", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    slist.LoadStockIn();
                }


            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
