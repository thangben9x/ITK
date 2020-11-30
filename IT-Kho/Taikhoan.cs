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
    }
}
