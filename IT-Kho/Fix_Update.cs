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

namespace IT_Kho
{
    public partial class Fix_Update : DevExpress.XtraEditors.XtraUserControl
    {
        public Fix_Update()
        {
            InitializeComponent();
        }
        private static Fix_Update _instance;

        public static Fix_Update Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Fix_Update();
                return _instance;
            }
        }
        private void LoadData()
        {
            string sql = "select YeuCau.tenmay, stt, thietbinc, chitietnc, ngaync, lydo, trangthai, YeuCau.ghichu, nguoinc, PC.chumay, PC.maphong from YeuCau inner join PC on PC.tenmay = YeuCau.tenmay order by ngaync desc";
            gridControl1.DataSource = Connect.getTable(sql);
        }
        private void Fix_Update_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "tenmay").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "thietbinc").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "chitietnc").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "ngaync").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "lydo").ToString() == ""
                    || gridView1.GetRowCellValue(e.RowHandle, "trangthai").ToString() == "")

            {
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form !!";
            }

            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string tenmay = gridView1.GetRowCellValue(e.RowHandle, "tenmay").ToString();
                string thietbinc = gridView1.GetRowCellValue(e.RowHandle, "thietbinc").ToString();
                string chitietnc = gridView1.GetRowCellValue(e.RowHandle, "chitietnc").ToString();
                string ngaync = gridView1.GetRowCellValue(e.RowHandle, "ngaync").ToString();
                string lydo = gridView1.GetRowCellValue(e.RowHandle, "lydo").ToString();
                string trangthai = gridView1.GetRowCellValue(e.RowHandle, "trangthai").ToString();
                string nguoinc = gridView1.GetRowCellValue(e.RowHandle, "nguoinc").ToString();
                string ghichu = gridView1.GetRowCellValue(e.RowHandle, "ghichu").ToString();
                string stt = gridView1.GetRowCellValue(e.RowHandle, "stt").ToString();

                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into YeuCau values('" + tenmay + "',N'" + thietbinc + "',N'" + chitietnc + "','" + Convert.ToDateTime(ngaync).ToString("MM/dd/yyyy") + "',N'" + lydo + "',N'" + trangthai + "',N'" + ghichu + "',N'" + nguoinc + "')";
                        Connect.Query(insert);
                        LoadData();
                        if(trangthai == "Đã nâng cấp")
                        {
                            XtraMessageBox.Show("Add danh sách thành công! Hãy update lại cấu hình máy bạn vừa ADD");
                        }
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                    }
                }
                else 
                {
                    try
                    {
                        string update = "update YeuCau set tenmay = '" + tenmay + "', thietbinc = '" + thietbinc + "', chitietnc = N'" + chitietnc + "',ngaync = '" + Convert.ToDateTime(ngaync).ToString("MM/dd/yyyy") + "', lydo = N'" + lydo + "', trangthai =N'" + trangthai + "' ,ghichu=N'" + ghichu + "', nguoinc = N'" + nguoinc + "'where stt = '" + stt + "' ";
                        Connect.Query(update);
                        LoadData();
                        if (trangthai == "Đã nâng cấp")
                        {
                            XtraMessageBox.Show("Cập nhật thành công! Hãy update lại cấu hình máy bạn vừa ADD");
                        }
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                    }
                }
            }
            else
            {
                DialogResult tb = XtraMessageBox.Show(sErr, "Lỗi trong quá trình Xuất!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (tb == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                // nếu cột không có giá trị thì trả về....
                if (view == null) return;

                if (e.Column.Caption != "Tên máy") return;

                string sql2 = @"select chumay, maphong from PC where tenmay = '" + view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString() + "' ";
                DataTable tb = Connect.getTable(sql2);

                view.SetRowCellValue(e.RowHandle, view.Columns[8], "");
                string cellValue4 = "" + tb.Rows[0]["chumay"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[8]).ToString();
                view.SetRowCellValue(e.RowHandle, view.Columns[8], cellValue4);

                view.SetRowCellValue(e.RowHandle, view.Columns[10], "");
                string cellValue = "" + tb.Rows[0]["maphong"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
                view.SetRowCellValue(e.RowHandle, view.Columns[10], cellValue);

            }
            catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra !!");
            }

        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                string stt = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "stt").ToString();
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    try
                    {
                        string delete = "delete from YeuCau where stt ='" + stt + "'";
                        Connect.Query(delete);
                        LoadData();
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                    }
                }
                else
                {
                    LoadData();
                }
            }
        }
    }
}
