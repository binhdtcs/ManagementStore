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
using Tulpep.NotificationWindow;
namespace Management_Store
{
    public partial class Form1 : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            
            NotifyCriticalItems();


            //cn.Open();
            //MessageBox.Show("Đã kết nối");
            
           
        }

        public void NotifyCriticalItems()
        {
            string critical = "";
            cn.Open();
            cm = new SqlCommand("select count(*) from vwCriticalItems", cn);
            string count = cm.ExecuteScalar().ToString();
            cn.Close();


            int i = 0;
            cn.Open();
            cm = new SqlCommand("select * from vwCriticalItems", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                critical += i + ". " + dr["pdesc"].ToString() + Environment.NewLine;
            }
            dr.Close();
            cn.Close();

            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.error;
            popup.TitleText = count + " CÁC MẶT HÀNG QUAN TRỌNG";
            popup.ContentText = critical;
            popup.Popup();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            frmBrandList frm = new frmBrandList();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            frmCategoryList frm = new frmCategoryList();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.LoadCategory();
            frm.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmProductList frm = new frmProductList();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.LoadRecords();
            frm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmRecords frm = new frmRecords();  
            frm.TopLevel = false;
            frm.LoadCriticalItems();
            frm.LoadInventory();
            frm.CancelledOrders();
            frm.LoadStockInHistory();
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnStockIn_Click_1(object sender, EventArgs e)
        {
            frmStockIn frm = new frmStockIn();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            //frmPOS frm = new frmPOS();
            //frm.ShowDialog();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmUserAccount frm = new frmUserAccount();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            panel4.BringToFront();
            frm.Show();
        }

        private void btnSalesHistory_Click(object sender, EventArgs e)
        {
            frmSoldItems frm = new frmSoldItems();
            frm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("ĐĂNG XUẤT KHỎI ỨNG DỤNG","XÁC NHẬN", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmSecurity f  = new frmSecurity(); 
             
                f.ShowDialog();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmStore f = new frmStore();
            f.LoadRecords();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyDashboard();
        }

        public void MyDashboard()
        {
            frmDashboard f = new frmDashboard();
            f.TopLevel = false;
            panel4.Controls.Add(f);
            f.lblDailySales.Text = dbcon.DailySales().ToString("#,##0.00");
            f.lblProduct.Text = dbcon.ProductLine().ToString("#,##0");
            f.lblStockOnHand.Text = dbcon.StockOnHand().ToString("#,##0");
            f.lblCritical.Text = dbcon.CriticalItems().ToString("#,##0");
            f.BringToFront();
            f.Show();
        }

        private void btnAdjustment_Click(object sender, EventArgs e)
        {
            frmStockAdjustment f = new frmStockAdjustment(this);
            f.LoadRecords();
            f.txtUser.Text = lblname.Text;
            f.ReferenceNo();
            f.ShowDialog();
        }
    }
}
