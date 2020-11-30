using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IT_Kho
{
    public partial class Form1 : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!container.Controls.Contains(NVTCC.Instance))
            {
                container.Controls.Add(NVTCC.Instance);
                NVTCC.Instance.Dock = DockStyle.Fill;
                NVTCC.Instance.BringToFront();
            }
            NVTCC.Instance.BringToFront();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!container.Controls.Contains(Xuatkho.Instance))
            {
                container.Controls.Add(Xuatkho.Instance);
                Xuatkho.Instance.Dock = DockStyle.Fill;
                Xuatkho.Instance.BringToFront();
            }
            Xuatkho.Instance.BringToFront();
        }

        public static string mp = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "select *from  PhongBan";
            lookUpEdit1.Properties.DataSource = Connect.getTable(sql);
            lookUpEdit1.Properties.DisplayMember = "tenphong";
            lookUpEdit1.Properties.ValueMember = "maphong";
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            mp = lookUpEdit1.EditValue.ToString();
            XtraForm1 x = new XtraForm1();
            x.Show();
        }

        private void accordionControlElement7_Click(object sender, EventArgs e)
        {
            if (!container.Controls.Contains(Fix_Update.Instance))
            {
                container.Controls.Add(Fix_Update.Instance);
                Fix_Update.Instance.Dock = DockStyle.Fill;
                Fix_Update.Instance.BringToFront();
            }
            Fix_Update.Instance.BringToFront();
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {
            if (!container.Controls.Contains(Ton.Instance))
            {
                container.Controls.Add(Ton.Instance);
                Ton.Instance.Dock = DockStyle.Fill;
                Ton.Instance.BringToFront();
            }
            Ton.Instance.BringToFront();
        }

        private void accordionControlElement17_Click(object sender, EventArgs e)
        {
            if (!container.Controls.Contains(ThuHoi.Instance))
            {
                container.Controls.Add(ThuHoi.Instance);
                ThuHoi.Instance.Dock = DockStyle.Fill;
                ThuHoi.Instance.BringToFront();
            }
            ThuHoi.Instance.BringToFront();
        }

        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            if (!container.Controls.Contains(TonKi.Instance))
            {
                container.Controls.Add(TonKi.Instance);
                TonKi.Instance.Dock = DockStyle.Fill;
                TonKi.Instance.BringToFront();
            }
            TonKi.Instance.BringToFront();
        }

        private void accordionControlElement9_Click(object sender, EventArgs e)
        {
            if (!container.Controls.Contains(VatTu.Instance))
            {
                container.Controls.Add(VatTu.Instance);
                VatTu.Instance.Dock = DockStyle.Fill;
                VatTu.Instance.BringToFront();
            }
            VatTu.Instance.BringToFront();
        }

        private void accordionControlElement10_Click(object sender, EventArgs e)
        {
            if (!container.Controls.Contains(Taikhoan.Instance))
            {
                container.Controls.Add(Taikhoan.Instance);
                Taikhoan.Instance.Dock = DockStyle.Fill;
                Taikhoan.Instance.BringToFront();
            }
            Taikhoan.Instance.BringToFront();
        }

        private void barLargeButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!container.Controls.Contains(Nhapmoi.Instance))
            {
                container.Controls.Add(Nhapmoi.Instance);
                Nhapmoi.Instance.Dock = DockStyle.Fill;
                Nhapmoi.Instance.BringToFront();
            }
            Nhapmoi.Instance.BringToFront();
        }

        private void barLargeButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!container.Controls.Contains(NVTCC.Instance))
            {
                container.Controls.Add(NVTCC.Instance);
                NVTCC.Instance.Dock = DockStyle.Fill;
                NVTCC.Instance.BringToFront();
            }
            NVTCC.Instance.BringToFront();
        }
    }
}
