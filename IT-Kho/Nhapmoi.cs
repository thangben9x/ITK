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
    public partial class Nhapmoi : DevExpress.XtraEditors.XtraUserControl
    {
        public Nhapmoi()
        {
            InitializeComponent();
        }
        private static Nhapmoi _instance;

        public static Nhapmoi Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Nhapmoi();
                return _instance;
            }
        }
        private void hien()
        {
            try
            {
                string sql = "SELECT Nhap.masp, Nhap.slnhap, Nhap.ngaynhap, Nhap.sn, Nhap.barcode, Nhap.ncc, Nhap.ghichu, Nhap.sohd, VatTu.tensp, VatTu.dvt, VatTu.model, NhanVien.manv FROM NhanVien INNER JOIN Nhap ON NhanVien.manv = Nhap.manv INNER JOIN VatTu ON Nhap.model = VatTu.model order by sohd desc";




                gridControl1.DataSource = Connect.getTable(sql);

            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void Nhapmoi_Load(object sender, EventArgs e)
        {
            hien();
            string sql1 = "select *from NhanVien";
            repositoryItemLookUpEdit1.DataSource = Connect.getTable(sql1);
            repositoryItemLookUpEdit1.ValueMember = "manv";
            repositoryItemLookUpEdit1.DisplayMember = "tennv";
            repositoryItemLookUpEdit1.NullText = @"Chọn người nhập";
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "masp").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "slnhap").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "ngaynhap").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "manv").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "ncc").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "barcode").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "sn").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "ghichu").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "model").ToString() == "")

            {
                // chuỗi thông báo lỗi
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form nhập!!";
            }
            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng

                string model = gridView1.GetRowCellValue(e.RowHandle, "model").ToString();
                string masp = gridView1.GetRowCellValue(e.RowHandle, "masp").ToString();
                string slnhap = gridView1.GetRowCellValue(e.RowHandle, "slnhap").ToString();
                string dvt = gridView1.GetRowCellValue(e.RowHandle, "dvt").ToString();
                string ngaynhap = gridView1.GetRowCellValue(e.RowHandle, "ngaynhap").ToString();
                string manv = gridView1.GetRowCellValue(e.RowHandle, "manv").ToString();
                string ncc = gridView1.GetRowCellValue(e.RowHandle, "ncc").ToString();
                string sn = gridView1.GetRowCellValue(e.RowHandle, "sn").ToString();
                string barcode = gridView1.GetRowCellValue(e.RowHandle, "barcode").ToString();
                string ghichu = gridView1.GetRowCellValue(e.RowHandle, "ghichu").ToString();
                string sohd = gridView1.GetRowCellValue(e.RowHandle, "sohd").ToString();
                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into Nhap values('" + masp + "','" + slnhap + "','" + Convert.ToDateTime(ngaynhap).ToString("MM/dd/yyyy") + "','" + sn + "','" + barcode + "','" + ncc + "','" + ghichu + "','" + manv + "','" + model + "')";
                        Connect.Query(insert);
                        hien();
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể kết nối tới CSDL!!");
                    }
                }
                else
                {
                    try
                    {
                        string update = "update Nhap set masp = '" + masp + "', slnhap = '" + slnhap + "',ngaynhap = '" + Convert.ToDateTime(ngaynhap).ToString("MM/dd/yyyy") + "', manv = '" + manv + "', ncc ='" + ncc + "' ,ghichu='" + ghichu + "',barcode='" + barcode + "',sn='" + sn + "', model = '" + model + "'where sohd = '" + sohd + "'";
                        Connect.Query(update);
                        hien();

                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể kết nối tới CSDL!!");
                    }
                }
            }
            else
            {
                DialogResult tb = XtraMessageBox.Show(sErr, "Lỗi trong quá trình nhập!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (tb == DialogResult.OK)
                {
                    // load lại form
                    hien();
                }
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //blinding dữ liệu tương ứng từ cột mã loại khi nhập vào các cell mã vật tư, tên vt, loại từ bảng Vật tư
                GridView view = sender as GridView;
                // nếu cột không có giá trị thì trả về....
                if (view == null) return;
                {
                    switch (e.Column.Caption.ToString())
                    {
                        case "Model":
                            string sql2 = @"select tensp,dvt from VatTu where model = '" + view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString() + "' ";
                            // thực thi câu lệnh sql thành dạng bảng
                            DataTable tb = Connect.getTable(sql2);
                            //blinding dvt
                            view.SetRowCellValue(e.RowHandle, view.Columns[2], "");
                            string cellValue2 = "" + tb.Rows[0]["dvt"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[2], cellValue2);



                            //blinding tenvt
                            view.SetRowCellValue(e.RowHandle, view.Columns[1], "");
                            string cellValue1 = "" + tb.Rows[0]["tensp"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[1], cellValue1);
                            break;

                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Vui lòng kiểm tra lại dữ liệu nhập!!");
            }
        }
        //CHức năng xóa Item
        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                string sohd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "sohd").ToString();
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    GridView view = sender as GridView;
                    view.DeleteRow(view.FocusedRowHandle);
                    try
                    {
                        string delete = "delete from Nhap where sohd ='" + sohd + "' ";
                        Connect.Query(delete);
                        hien();

                    }
                    catch
                    {
                        XtraMessageBox.Show("Lỗi! Hãy thử lại!");
                        hien();
                    }
                }
                else
                {
                    hien();
                }
            }
        }
        bool indicatorIcon = true;

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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
