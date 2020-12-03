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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace IT_Kho
{
    public partial class Taikhoan : DevExpress.XtraEditors.XtraUserControl
    {
        public Taikhoan()
        {
            InitializeComponent();
        }
        private static Taikhoan _instance;

        public static Taikhoan Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Taikhoan();
                return _instance;
            }
        }
        private void hien()
        {
            try
            {
                string sql = "select NhanVien.* from NhanVien";




                gridControl1.DataSource = Connect.getTable(sql);

            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void Taikhoan_Load(object sender, EventArgs e)
        {
            hien();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "manv").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "tennv").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "username").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "password").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "quyen").ToString() == "")
            {
                // chuỗi thông báo lỗi
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form nhập!!";
            }
            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng

                string manv = gridView1.GetRowCellValue(e.RowHandle, "manv").ToString();
                string tennv = gridView1.GetRowCellValue(e.RowHandle, "tennv").ToString();
                string username = gridView1.GetRowCellValue(e.RowHandle, "username").ToString();
                string pass = gridView1.GetRowCellValue(e.RowHandle, "password").ToString();
                string quyen = gridView1.GetRowCellValue(e.RowHandle, "quyen").ToString();



                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {

                        string insert = "insert into NhanVien values('" + manv + "','" + tennv + "','" + username + "','" + pass + "','" + quyen + "' )";
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
                        string update = "update NhanVien set manv = '" + manv + "', tennv = '" + tennv + "',username = '" + username + "',password = '" + pass + "',quyen = '" + quyen + "' ";
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

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                string manv = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "manv").ToString();
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    GridView view = sender as GridView;
                    view.DeleteRow(view.FocusedRowHandle);
                    try
                    {
                        string delete = "delete from Nhap where sohd ='" + manv + "' ";
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
        // tạo 1 biến
        bool indicatorIcon = true;
        // code hiển thị CODE STT 
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
