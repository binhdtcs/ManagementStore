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
    public partial class frmRecords : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmRecords()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void LoadRecord()
        {
            int i = 0;
            cn.Open();
            dataGridView1.Rows.Clear();
            cm = new SqlCommand("select top 10 pcode,pdesc,sum(qty) as qty from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'Đã bán' group by pcode,pdesc oder by qty desc", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["qty"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void CancelledOrders()
        {
            int i = 0;
            dataGridView5.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwcancelledorder where sdate between '" + dateTimePicker5.Value.ToString() + "' and '" + dateTimePicker6.Value.ToString() + "'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView5.Rows.Add(i, dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["total"].ToString(), dr["sdate"].ToString(), dr["voidby"].ToString(), dr["cancelledby"].ToString(), dr["reason"].ToString(), dr["action"].ToString());
            }
            dr.Close();
            cn.Close() ;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select c.pcode,c.pdesc,c.price,sum(qty) as tot_qty,sum(c.disc) as tot_disc, sum(c.total) as total from tblcart as c inner join tblproduct as p on c.pcode =p.pcode where status like 'Đã bán' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "' group by c.pcode,p.pdesc,c.price", cn);
                cm.ExecuteNonQuery();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), Double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["tot_qty"].ToString(), dr["tot_disc"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                cn.Close();

                String x;

                cn.Open();

                cm = new SqlCommand("select isnull(sum(total),0) from tblcart where status like 'Đã bán' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'", cn);
                lblTotal.Text = Double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadInventory()
        {
            int i = 0;
            dataGridView4.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select p.pcode, p.barcode, p.pdesc, b.brand, c.category, p.price, p.qty, p.reorder from tblProduct as p inner join tblbrand as b on p.bid=b.id inner join tblcategory as c on p.cid=c.id", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView4.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdesc"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["reorder"].ToString(), dr["qty"].ToString());
            }
            dr.Close ();
            cn.Close();
        }

        public void LoadCriticalItems()
        {
            try
            {
                dataGridView3.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select * from vwCriticalItems", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView3.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.LoadReport();
            frm.ShowDialog();
        }

   
        private void button2_Click(object sender, EventArgs e)
        {
            CancelledOrders();
        }

        public void LoadStockInHistory()
        {
            int i = 0;
            dataGridView6.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockin where cast(sdate as date) between '" + dateTimePicker8.Value.ToShortDateString() + "' and '" + dateTimePicker7.Value.ToShortDateString() + "' and status like 'Hoàn tất'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView6.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadStockInHistory();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport f = new frmInventoryReport();
            f.LoadTopSelling("select top 10 pcode,pdesc,sum(qty) as qty from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'Đã bán' group by pcode,pdesc oder by qty desc", "Từ :" + dateTimePicker1.Value.ToString() + " đến: " + dateTimePicker2.Value.ToString());
            f.ShowDialog();

        }

 

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport f = new frmInventoryReport();
            f.LoadSoldItems("select c.pcode,c.pdesc,c.price,sum(qty) as tot_qty,sum(c.disc) as tot_disc, sum(c.total) as total from tblcart as c inner join tblProduct as p on c.pcode =p.pcode where status like 'Đã bán' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "' group by c.pcode,p.pdesc,c.price", "Từ : " + dateTimePicker4.Value.ToString() + " đến: " + dateTimePicker3.Value.ToString());
            f.ShowDialog();
        }
    }
}
