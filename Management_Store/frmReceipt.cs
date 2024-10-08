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
using Microsoft.Reporting.WinForms;
namespace Management_Store
{
    public partial class frmReceipt : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        frmPOS f;
        string store = "Căn tin Rạp xiếc";
        string address = "Phường 3, Quận Gò Vấp, TP.Hồ Chí Minh";


        public frmReceipt(frmPOS frm)
        {
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
            f = frm;
            this.KeyPreview = true;
        }

        private void frmReceipt_Load(object sender, EventArgs e)
        {
            
            this.reportViewer1.RefreshReport();
        }

        public void LoadReport(string pcash, string pchange)
        {
            ReportDataSource rptDataSource;
            try
            {
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report1.rdlc";
                
                this.reportViewer1.LocalReport.DataSources.Clear();
                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("select c.id, c.transno, c.pcode,c.price,c.qty,c.disc,c.total,c.sdate,c.status,p.desc from tblcart as c inner join tblProduct as p on p.pcode = c.pcode where transno like '" + f.lblTransno.Text + "'", cn);
                da.Fill(ds.Tables["dtSold"]);
                cn.Close();

                ReportParameter pVatable = new ReportParameter("pVatable",f.lblVatable.Text);
                ReportParameter pVat = new ReportParameter("pVat", f.lblVat.Text);
                ReportParameter pDiscount = new ReportParameter("pDiscount", f.lblDiscount.Text);
                ReportParameter pTotal = new ReportParameter("pTotal", f.lblTotal.Text);
                ReportParameter pCash = new ReportParameter("pCash", pcash);
                ReportParameter pChange = new ReportParameter("pChange", pchange);
                ReportParameter pStore = new ReportParameter("pStore", store);
                ReportParameter pAddress = new ReportParameter("pAddress", address);
                ReportParameter pTransaction = new ReportParameter("pTransaction", "Hóa đơn #: " + f.lblTransno.Text);
                ReportParameter pCashier = new ReportParameter("pCashier", f.lblUser.Text);
                
                reportViewer1.LocalReport.SetParameters(pVatable);
                reportViewer1.LocalReport.SetParameters(pVat);
                reportViewer1.LocalReport.SetParameters(pDiscount);
                reportViewer1.LocalReport.SetParameters(pTotal);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pChange);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pTransaction);
                reportViewer1.LocalReport.SetParameters(pCashier);



                rptDataSource = new ReportDataSource("Dataset1", ds.Tables["dtSold"]);    
                reportViewer1.LocalReport.DataSources.Add(rptDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void frmReceipt_Load_1(object sender, EventArgs e)
        {

            this.reportViewer2.RefreshReport();
        }

        private void frmReceipt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

 
    }
}
