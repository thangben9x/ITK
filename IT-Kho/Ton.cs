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
    public partial class Ton : DevExpress.XtraEditors.XtraUserControl
    {
        public Ton()
        {
            InitializeComponent();
        }

        private void Ton_Load(object sender, EventArgs e)
        {
            Loaddata();
        }
        private void Loaddata()
        {
            try
            {
                string sql = "select MaVT, TenVT, dvt, Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton from (select model as MaVT, tensp as TenVT, dvt as dvt, 0 as Nhap, 0 as Xuat From VatTu union Select N.model as MaVT, H.tensp as TenVT, H.dvt as dvt, Sum(N.slnhap) as Nhap, 0 as Xuat  From Nhap N, VatTu H Where N.model = H.model  Group By N.model, H.tensp, H.dvt having SUM(N.slnhap) > 0 union Select X.model as MaVT, H.tensp as TenVT, H.dvt as dvt, 0 as Nhap, Sum(X.slxuat) as Xuat   From Xuat X, VatTu H Where X.model = H.model Group By X.model, H.tensp, H.dvt having SUM(X.slxuat) > 0 ) as hangton Group by MaVT, TenVT, dvt";
                gridControl1.DataSource = Connect.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL");
            }
        }
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
    }
}
