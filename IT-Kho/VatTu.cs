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
    public partial class VatTu : DevExpress.XtraEditors.XtraUserControl
    {
        public VatTu()
        {
            InitializeComponent();
        }
        private static VatTu _instance;

        public static VatTu Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new VatTu();
                return _instance;
            }
        }
        private void hien()
        {
            try
            {
                string sql = "select VatTu.* from VatTu";




                gridControl1.DataSource = Connect.getTable(sql);

            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void VatTu_Load(object sender, EventArgs e)
        {
            hien();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "model").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "tensp").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "dvt").ToString() == "")
            {
                // chuỗi thông báo lỗi
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form nhập!!";
            }
            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng

                string model = gridView1.GetRowCellValue(e.RowHandle, "model").ToString();
                string tensp = gridView1.GetRowCellValue(e.RowHandle, "tensp").ToString();
                string dvt = gridView1.GetRowCellValue(e.RowHandle, "dvt").ToString();

                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into VatTu values('" + model + "','" + tensp + "','" + dvt + "')";
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
                        string update = "update VatTu set model = '" + model + "', tensp = '" + tensp + "',dvt = '" + dvt + "'";
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
                string model = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "model").ToString();
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    GridView view = sender as GridView;
                    view.DeleteRow(view.FocusedRowHandle);
                    try
                    {
                        string delete = "delete from VatTu where model ='" + model + "' ";
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
    }
}
