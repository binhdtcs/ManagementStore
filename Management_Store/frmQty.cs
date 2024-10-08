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
    public partial class frmQty : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        private String pcode;
        private double price;
        private int qty;
        private String transno;
        string stitle = "Simple POS system";
        frmPOS fpos;
        public frmQty(frmPOS frmpos)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            fpos = frmpos;
            
        }
        public void ProductDetails(String pcode, double price, String transno, int qty)
        {
            this.pcode = pcode;
            this.price = price;
            this.transno = transno;
            this.qty = qty;
        }

        private void frmQty_Load(object sender, EventArgs e)
        {
            txtQty.Text = "0";
            
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13) && (txtQty.Text != String.Empty))
            {
                String id="";
                int cart_qty=0;
                bool found = false;

                cn.Open();
                cm = new SqlCommand("Select * from tblcart where transno = @transno and pcode = @pcode", cn);
                cm.Parameters.AddWithValue("@transno", fpos.lblTransno.Text);
                cm.Parameters.AddWithValue("@pcode", pcode);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                    id = dr["id"].ToString();
                    cart_qty = int.Parse(dr["qty"].ToString());
                }
                else { found = false; }
                dr.Close();
                cn.Close();
                
                if(found == true)
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Không thể tiếp tục. Số lượng còn lại trong kho là " + qty, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    cn.Open();
                    cm = new SqlCommand("update tblcart set qty = (qty + " + int.Parse(txtQty.Text) + ") where id = '"+ id + "'", cn);                  
                    cm.ExecuteNonQuery();
                    cn.Close();


                    fpos.txtSearch.Clear();
                    fpos.txtSearch.Focus();
                    fpos.LoadCart();
                    this.Dispose();

                }
                else
                {
                    if (qty < int.Parse(txtQty.Text))
                    {
                        MessageBox.Show("Không thể tiếp tục. Số lượng còn lại trong tay là " + qty, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    cn.Open();
                    cm = new SqlCommand("insert into tblcart (transno, pcode, price, qty, sdate, cashier)values(@transno, @pcode, @price, @qty, @sdate, @cashier)", cn);
                    cm.Parameters.AddWithValue("@transno", transno);
                    cm.Parameters.AddWithValue("@pcode", pcode);
                    cm.Parameters.AddWithValue("@price", price);
                    cm.Parameters.AddWithValue("@qty", int.Parse(txtQty.Text));
                    cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                    cm.Parameters.AddWithValue("@cashier", fpos.lblUser.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();


                    fpos.txtSearch.Clear();
                    fpos.txtSearch.Focus();
                    fpos.LoadCart();
                    this.Dispose();
                }    

            }
        
        }

      
    }
}
