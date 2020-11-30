using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace IT_Kho
{
    public partial class TonKi : DevExpress.XtraEditors.XtraUserControl
    {
        public TonKi()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Loaddt();
        }
        private void Loaddt()
        {
            try
            {
                string sql = "SELECT Tonct.model, Tonct.tensp, Tonct.Barcode ,sum(Tonct.Tondk) AS TonDau, sum(Tonct.Nhaptk) AS Nhap, sum(Tonct.Xuattk) AS Xuat, (sum(Tonct.Tondk)+sum(Tonct.Nhaptk)- sum(Tonct.Xuattk)) AS TonCuoi  FROM(Select dk.model, dk.tensp, dk.Barcode, Tondk, 0 as Nhaptk, 0 as Xuattk  From (Select a.model, a.tensp, a.Barcode, (Sum(a.Nhap) - Sum(a.Xuat)) AS Tondk  From (Select N.model, H.tensp, H.dvt as Barcode, Sum(N.slnhap) as Nhap, 0 as Xuat  From Nhap N, VatTu H Where N.model = H.model and N.ngaynhap < '" + Convert.ToDateTime(ngaybd.Text).ToString("MM/dd/yyyy") + "'  Group By N.model, H.tensp, H.dvt UNION (Select X.model, H.tensp, H.dvt as Barcode, 0 as Nhap, sum(X.slxuat) as Xuat From Xuat X, VatTu H Where X.model = H.model and X.ngayxuat < '" + Convert.ToDateTime(ngaybd.Text).ToString("MM/dd/yyyy") + "' Group By X.model, H.tensp, H.dvt)) a GROUP BY a.model, a.tensp, a.Barcode HAVING(Sum(a.Nhap - a.Xuat)) <> 0) dk Union Select model, tensp, dvt as Barcode, 0 as Tondk, 0 as Nhaptk, 0 as Xuattk From VatTu Union Select N.model, H.tensp, H.dvt as Barcode, 0 as Tondk, Sum(N.slnhap) as Nhaptk, 0 as Xuattk  From Nhap N, VatTu H Where N.model = H.model and N.ngaynhap >= '" + Convert.ToDateTime(ngaybd.Text).ToString("MM/dd/yyyy") + "' and N.ngaynhap <= '" + Convert.ToDateTime(ngaykt.Text).ToString("MM/dd/yyyy") + "'  Group By N.model, H.tensp, H.dvt Union Select X.model, H.tensp, H.dvt as Barcode, 0 as Tondk, 0 as Nhaptk, sum(X.slxuat) as Xuattk  From Xuat X, VatTu H Where X.model = H.model and X.ngayxuat >= '" + Convert.ToDateTime(ngaybd.Text).ToString("MM/dd/yyyy") + "' and X.ngayxuat <= '" + Convert.ToDateTime(ngaykt.Text).ToString("MM/dd/yyyy") + "'  Group By X.model, H.tensp, H.dvt )  AS Tonct GROUP BY Tonct.model, Tonct.tensp, Tonct.Barcode HAVING(sum(Tonct.Tondk) + sum(Tonct.Nhaptk) - sum(Tonct.Xuattk)) <> 0";
                gridControl1.DataSource = Connect.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không kết nối được tới CSDL!!");
            }
            
        }
        private static TonKi _instance;

        public static TonKi Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TonKi();
                return _instance;
            }
        }
    }
}
