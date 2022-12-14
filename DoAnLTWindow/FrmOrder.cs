using DoAnLTWindow.DAO;
using DoAnLTWindow.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTWindow
{
    public partial class FrmOrder : Form
    {
        public FrmOrder()
        {
            InitializeComponent();        }

        private void FrmOrder_Load(object sender, EventArgs e)
        {
            dtOrderDate.Value = DateTime.Now;
            cbbTableName.DataSource = TableDAO.Instance.loadTableList();
            cbbTableName.DisplayMember = "Name";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int id = (int)DataProvider.Instance.ExecuteScalar("SELECT ID FROM BAN_AN WHERE TEN = N'" + cbbTableName.Text + "'");
            string order_time = dtOrderDate.Text + " " + nbudHour.Value + ":" + nbudMinute.Value;
            try
            {
                DataProvider.Instance.ExecuteNonQuery(String.Format("EXEC INSERT_ORDERED {0}, N'{1}', '{2}', '{3}'", id, txtOrderName.Text, txtNumber.Text, order_time));
            }
            catch
            {
                MessageBox.Show("Vui Lòng Nhập Đúng Thông Tin!!");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataProvider.Instance.ExecuteNonQuery("UPDATE BAN_AN SET TRANGTHAI2 = 0 WHERE TEN = N'" + cbbTableName.Text + "'");
        }

        private void cbbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable pTable = DataProvider.Instance.ExecuteQuery("SELECT * FROM BAN_AN WHERE TEN = N'" + cbbTableName.Text + "'");
            if (pTable.Rows.Count > 0)
            {
                DataRow row = pTable.Rows[0];
                if ((int)row["TRANGTHAI2"] != 0)
                {
                    row = DataProvider.Instance.ExecuteQuery("EXEC GET_ORDERED_TABLE " + (int)row["ID"]).Rows[0];
                    txtNumber.Text = row["Number"].ToString();
                    txtOrderName.Text = row["ORDERS_NAME"].ToString();
                    string[] test0 = row["ORDER_TIME"].ToString().Split(' ');
                    dtOrderDate.Text = test0[0];
                    test0 = test0[1].Split(':');
                    nbudHour.Text = test0[0];
                    nbudMinute.Text = test0[1];
                }
            }
        }
    }
}