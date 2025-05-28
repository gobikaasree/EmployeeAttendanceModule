using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeAttendance
{
    public partial class Attendance : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (TimeSpan.Parse(txtTimeOut.Text) <= TimeSpan.Parse(txtTimeIn.Text))
            {
                lblMessage.Text = "Time Out must be after Time In.";
                return;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "INSERT INTO EmployeeAttendance (EmployeeID, EmployeeName, AttendanceDate, TimeIn, TimeOut, Remarks) " +
                               "VALUES (@EmployeeID, @EmployeeName, @Date, @TimeIn, @TimeOut, @Remarks)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text);
                cmd.Parameters.AddWithValue("@EmployeeName", txtEmployeeName.Text);
                cmd.Parameters.AddWithValue("@Date", DateTime.Parse(txtDate.Text));
                cmd.Parameters.AddWithValue("@TimeIn", TimeSpan.Parse(txtTimeIn.Text));
                cmd.Parameters.AddWithValue("@TimeOut", TimeSpan.Parse(txtTimeOut.Text));
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            ClearFields();
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM EmployeeAttendance", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAttendance.DataSource = dt;
                gvAttendance.DataBind();
            }
        }

        private void ClearFields()
        {
            txtEmployeeID.Text = "";
            txtEmployeeName.Text = "";
            txtDate.Text = "";
            txtTimeIn.Text = "";
            txtTimeOut.Text = "";
            txtRemarks.Text = "";
            lblMessage.Text = "";
        }

        protected void gvAttendance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAttendance.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gvAttendance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAttendance.EditIndex = -1;
            LoadData();
        }

        protected void gvAttendance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvAttendance.Rows[e.RowIndex];
            int id = Convert.ToInt32(gvAttendance.DataKeys[e.RowIndex].Value);

            string employeeID = ((TextBox)row.Cells[0].Controls[0]).Text;
            string employeeName = ((TextBox)row.Cells[1].Controls[0]).Text;
            DateTime date = DateTime.Parse(((TextBox)row.Cells[2].Controls[0]).Text);
            TimeSpan timeIn = TimeSpan.Parse(((TextBox)row.Cells[3].Controls[0]).Text);
            TimeSpan timeOut = TimeSpan.Parse(((TextBox)row.Cells[4].Controls[0]).Text);
            string remarks = ((TextBox)row.Cells[5].Controls[0]).Text;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "UPDATE EmployeeAttendance SET EmployeeID=@EmployeeID, EmployeeName=@EmployeeName, AttendanceDate=@Date, TimeIn=@TimeIn, TimeOut=@TimeOut, Remarks=@Remarks WHERE AttendanceID=@ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@TimeIn", timeIn);
                cmd.Parameters.AddWithValue("@TimeOut", timeOut);
                cmd.Parameters.AddWithValue("@Remarks", remarks);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            gvAttendance.EditIndex = -1;
            LoadData();
        }

        protected void gvAttendance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvAttendance.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "DELETE FROM EmployeeAttendance WHERE AttendanceID=@ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            LoadData();
        }
    }
}