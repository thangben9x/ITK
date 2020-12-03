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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid;

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
        // tạo 1 biến
        // STT
        bool indicatorIcon = true;

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    string sText = (e.RowHandle + 1).ToString();
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString(sText, e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = sText;
                }
                if (!indicatorIcon)
                    e.Info.ImageIndex = -1;

                if (e.RowHandle == GridControl.InvalidRowHandle)
                {
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString("STT", e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = "STT";
                }
            }
            catch
            {
                XtraMessageBox.Show("Lỗi cột STT");
            }
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());
            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }
        // code hiển thị CODE STT 

    }
}
