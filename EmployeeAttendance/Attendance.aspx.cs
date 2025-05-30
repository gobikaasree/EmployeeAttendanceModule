using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;



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
                ReportViewer1.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(txtDate.Text, out DateTime attendanceDate))
            {
                lblMessage.Text = "Please enter a valid date.";
                return;
            }

            if (!TimeSpan.TryParse(txtTimeIn.Text, out TimeSpan timeIn) ||
                !TimeSpan.TryParse(txtTimeOut.Text, out TimeSpan timeOut))
            {
                lblMessage.Text = "Please enter valid time values.";
                return;
            }

            if (timeOut <= timeIn)
            {
                lblMessage.Text = "Time Out must be after Time In.";
                return;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"INSERT INTO EmployeeAttendance 
                                (EmployeeID, EmployeeName, AttendanceDate, TimeIn, TimeOut, Remarks) 
                                 VALUES 
                                (@EmployeeID, @EmployeeName, @Date, @TimeIn, @TimeOut, @Remarks)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text.Trim());
                cmd.Parameters.AddWithValue("@EmployeeName", txtEmployeeName.Text.Trim());
                cmd.Parameters.AddWithValue("@Date", attendanceDate);
                cmd.Parameters.AddWithValue("@TimeIn", timeIn);
                cmd.Parameters.AddWithValue("@TimeOut", timeOut);
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());

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

            string employeeID = ((TextBox)row.Cells[0].Controls[0]).Text.Trim();
            string employeeName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string dateText = ((TextBox)row.Cells[2].Controls[0]).Text;
            string timeInText = ((TextBox)row.Cells[3].Controls[0]).Text;
            string timeOutText = ((TextBox)row.Cells[4].Controls[0]).Text;
            string remarks = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();

            if (!DateTime.TryParse(dateText, out DateTime date) ||
                !TimeSpan.TryParse(timeInText, out TimeSpan timeIn) ||
                !TimeSpan.TryParse(timeOutText, out TimeSpan timeOut))
            {
                lblMessage.Text = "Invalid date or time format.";
                return;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"UPDATE EmployeeAttendance 
                                 SET EmployeeID=@EmployeeID, EmployeeName=@EmployeeName, AttendanceDate=@Date, 
                                     TimeIn=@TimeIn, TimeOut=@TimeOut, Remarks=@Remarks 
                                 WHERE AttendanceID=@ID";
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
                SqlCommand cmd = new SqlCommand("DELETE FROM EmployeeAttendance WHERE AttendanceID=@ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            LoadData();
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = true;

            if (!DateTime.TryParse(txtFromDate.Text, out DateTime fromDate) ||
                !DateTime.TryParse(txtToDate.Text, out DateTime toDate))
            {
                lblMessage.Text = "Please enter valid From and To dates.";
                ReportViewer1.Visible = false;
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        @"SELECT EmployeeID, EmployeeName, AttendanceDate AS [Date], TimeIn, TimeOut, Remarks 
                          FROM EmployeeAttendance 
                          WHERE AttendanceDate BETWEEN @From AND @To", con);
                    da.SelectCommand.Parameters.AddWithValue("@From", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@To", toDate);

                    DataSet ds = new DataSet();
                    da.Fill(ds, "AttendanceTable");

                    if (ds.Tables["AttendanceTable"].Rows.Count == 0)
                    {
                        lblMessage.Text = "No records found for the selected date range.";
                        ReportViewer1.Visible = false;
                        return;
                    }

                    ReportViewer1.Reset();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/EmployeeAttendanceReport.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();

                    ReportDataSource rds = new ReportDataSource("DataSet1", ds.Tables["AttendanceTable"]);
                    ReportViewer1.LocalReport.DataSources.Add(rds);

                    ReportViewer1.LocalReport.Refresh();
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred while generating the report: " + ex.Message;
            }
        }
    }
}