using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace IT_Kho
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();
        }
        string map = "";


        private void XtraForm1_Load(object sender, EventArgs e)
        {
            map = Form1.mp;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                string sql = "select tenmay, ram, chip, hdd, ssd, manhinh, tenuser, chumay, ghichu, maphong from PC where maphong ='" + map + "'";
                gridControl1.DataSource = Connect.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL!!");
            }
        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "tenmay").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "ram").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "chip").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "hdd").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "manhinh").ToString() == ""
                    || gridView1.GetRowCellValue(e.RowHandle, "tenuser").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "chumay").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "maphong").ToString() == "")

            {
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form !!";
            }

            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string tenmay = gridView1.GetRowCellValue(e.RowHandle, "tenmay").ToString();
                string ram = gridView1.GetRowCellValue(e.RowHandle, "ram").ToString();
                string chip = gridView1.GetRowCellValue(e.RowHandle, "chip").ToString();
                string hdd = gridView1.GetRowCellValue(e.RowHandle, "hdd").ToString();
                string ssd = gridView1.GetRowCellValue(e.RowHandle, "ssd").ToString();
                string manhinh = gridView1.GetRowCellValue(e.RowHandle, "manhinh").ToString();
                string tenuser = gridView1.GetRowCellValue(e.RowHandle, "tenuser").ToString();
                string chumay = gridView1.GetRowCellValue(e.RowHandle, "chumay").ToString();
                string ghichu = gridView1.GetRowCellValue(e.RowHandle, "ghichu").ToString();
                string maphong = gridView1.GetRowCellValue(e.RowHandle, "maphong").ToString();

                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into PC values('" + tenmay + "','" + ram + "','" + chip + "','" + hdd + "','" + ssd + "',N'" + manhinh + "','" + tenuser + "',N'" + chumay + "',N'" + ghichu + "','" + maphong + "')";
                        Connect.Query(insert);
                        LoadData();
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
                        string update = "update PC set ram = '" + ram + "', chip = '" + chip + "', hdd = '" + hdd + "',ssd = '" + ssd + "', manhinh = N'" + manhinh + "', tenuser =N'" + tenuser + "' ,ghichu=N'" + ghichu + "', chumay = N'" + chumay + "' where tenmay = '" + tenmay + "' ";
                        Connect.Query(update);
                        LoadData();
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
            map = Form1.mp;
            try
            {
                GridView view = sender as GridView;
                if (view == null) return;

                if (e.Column.Caption != "Tên máy") return;
                view.SetRowCellValue(e.RowHandle, view.Columns[9], "");
                string cellvalue = "" + map + "" + view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
                view.SetRowCellValue(e.RowHandle, view.Columns[9], cellvalue);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                string tenmay = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "tenmay").ToString();
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    try
                    {
                        string delete = "delete from PC where tenmay ='" + tenmay + "'";
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