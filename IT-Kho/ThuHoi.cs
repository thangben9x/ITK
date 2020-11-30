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
    public partial class ThuHoi : DevExpress.XtraEditors.XtraUserControl
    {
        public ThuHoi()
        {
            InitializeComponent();
        }

        private void ThuHoi_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private static ThuHoi _instance;

        public static ThuHoi Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ThuHoi();
                return _instance;
            }
        }
        private void LoadData()
        {
            try
            {
                string sql = "select stt, ThuHoi.tenmay, chumay, maphong, tbthuhoi, model, lydoth, ThuHoi.ghichu from ThuHoi inner join PC on PC.tenmay = ThuHoi.tenmay order by stt desc";
                gridControl1.DataSource = Connect.getTable(sql);
            }
            catch
            {
                MessageBox.Show("Không thể kết nối tới CSDL!!");
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                
                GridView view = sender as GridView;
                string sql2 = @"select chumay, maphong from PC where tenmay = '" + view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString() + "' ";
                DataTable tb = Connect.getTable(sql2);
                if (view == null) return;

                if (e.Column.Caption != "Tên máy") return;
                view.SetRowCellValue(e.RowHandle, view.Columns[6], "");
                string cellValue4 = "" + tb.Rows[0]["chumay"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                view.SetRowCellValue(e.RowHandle, view.Columns[6], cellValue4);

                view.SetRowCellValue(e.RowHandle, view.Columns[7], "");
                string cellValue = "" + tb.Rows[0]["maphong"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[7]).ToString();
                view.SetRowCellValue(e.RowHandle, view.Columns[7], cellValue);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                string stt = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "stt").ToString();
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    try
                    {
                        string delete = "delete from ThuHoi where stt ='" + stt + "'";
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

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "tenmay").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "tbthuhoi").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "model").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "lydoth").ToString() == "")
            {
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form !!";
            }

            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string tenmay = gridView1.GetRowCellValue(e.RowHandle, "tenmay").ToString();
                string tbthuhoi = gridView1.GetRowCellValue(e.RowHandle, "tbthuhoi").ToString();
                string stt = gridView1.GetRowCellValue(e.RowHandle, "stt").ToString();
                string model = gridView1.GetRowCellValue(e.RowHandle, "model").ToString();
                string lydoth = gridView1.GetRowCellValue(e.RowHandle, "lydoth").ToString();
                string ghichu = gridView1.GetRowCellValue(e.RowHandle, "ghichu").ToString();

                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into ThuHoi values('" + tenmay + "',N'" + tbthuhoi + "','" + model + "',N'" + lydoth + "',N'" + ghichu + "')";
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
                        string update = "update ThuHoi set tenmay = '" + tenmay + "', tbthuhoi = N'" + tbthuhoi + "', model = '" + model + "',lydoth = N'" + lydoth + "', ghichu = N'" + ghichu + "' where stt = '" + stt + "' ";
                        Connect.Query(update);
                        LoadData();
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                    }
                }
            }
        }
    }
}
