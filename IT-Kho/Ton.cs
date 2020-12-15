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
    public partial class Ton : DevExpress.XtraEditors.XtraUserControl
    {
        public Ton()
        {
            InitializeComponent();
        }

        private void Ton_Load(object sender, EventArgs e)
        {
            hien();
        }
        //Hiện thị data lên gridcontrol
        private void hien()
        {
            try
            {
                string sql = "select MaVT, TenVT, dvt, Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton from (select model as MaVT, tensp as TenVT, dvt as dvt, 0 as Nhap, 0 as Xuat From VatTu union Select N.model as MaVT, H.tensp as TenVT, H.dvt as dvt, Sum(N.slnhap) as Nhap, 0 as Xuat  From Nhap N, VatTu H Where N.model = H.model  Group By N.model, H.tensp, H.dvt having SUM(N.slnhap) > 0 union Select X.model as MaVT, H.tensp as TenVT, H.dvt as dvt, 0 as Nhap, Sum(X.slxuat) as Xuat   From Xuat X, VatTu H Where X.model = H.model Group By X.model, H.tensp, H.dvt having SUM(X.slxuat) > 0 ) as hangton Group by MaVT, TenVT, dvt";
                gridControl1.DataSource = Connect.getTable(sql);
                chart1.DataSource = Connect.getTable(sql);
                chart1.ChartAreas["ChartArea1"].AxisY.Title = "Tổng Số Lượng Nhập Xuất";
                chart1.ChartAreas["ChartArea1"].AxisX.Title = "Mã Model";
                chart1.Series["Tổng Xuất Kho"].XValueMember = "MaVT";
                chart1.Series["Tổng Xuất Kho"].YValueMembers = "tongxuat";
                chart1.Series["Tổng Nhập Kho"].YValueMembers = "tongnhhap";
                chart1.Series["Tồn Kho"].YValueMembers = "Ton";
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL");
            }
        }
        //CHuyển hướng sang trang  Tồn kho
        private static Ton _instance;

        public static Ton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Ton();
                return _instance;
            }
        }
        // tạo 1 biến
        bool indicatorIcon = true;
        // code hiển thị CODE STT 
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
    }
}
