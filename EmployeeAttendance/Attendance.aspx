<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="EmployeeAttendance.Attendance" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Attendance</title>
        <style>
    body {
        font-family: 'Roboto', sans-serif;
        background-color: #f9f9f9;
        font-size: 14px; /* Reduced base font size */
    }

    h2, h3 {
        color: #333;
        font-size: 22px; /* Smaller heading */
        text-align: left;
    }

    .form-container {
        margin: 20px auto;
        padding: 20px;
        max-width: 800px; /* Slightly smaller container */
        background-color: #fff;
        border-radius: 10px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.08);
    }

    label {
        font-size: 16px;
        font-weight: 500;
    }

    table {
        border-collapse: collapse;
        width: 100%;
        max-width: 800px;
        font-size: 14px;
    }

    td {
        padding: 8px;
        vertical-align: top;
    }

    input, textarea {
        width: 100%;
        padding: 8px;
        font-size: 14px;
        box-sizing: border-box;
        border: 1px solid #ccc;
        border-radius: 5px;
    }

    input[type="submit"],
    .btn {
        background-color: #007BFF;
        color: white;
        padding: 10px 20px;
        font-size: 14px;
        border: none;
        border-radius: 6px;
        cursor: pointer;
        margin-top: 8px;
        transition: background-color 0.3s ease;
    }

    input[type="submit"]:hover,
    .btn:hover {
        background-color: #0056b3;
    }

    .table {
        border: 1px solid #ccc;
        width: 100%;
        font-size: 14px;
    }

    .table th {
        background-color: #f2f2f2;
        padding: 10px;
        text-align: left;
        font-size: 16px;
    }

    .table td {
        border-top: 1px solid #ccc;
        padding: 10px;
        font-size: 14px;
    }

    .action-link {
        font-size: 14px;
        margin-right: 8px;
        color: #007BFF;
        text-decoration: none;
    }

    .action-link:hover {
        text-decoration: underline;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div style="margin: 20px; max-width: none;">
            <h2>Employee Attendance Form</h2>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />

            <table style="width: 100%;">
                <tr><td>Employee ID:</td><td><asp:TextBox ID="txtEmployeeID" runat="server" /></td></tr>
                <tr><td>Employee Name:</td><td><asp:TextBox ID="txtEmployeeName" runat="server" /></td></tr>
                <tr><td>Date:</td><td><asp:TextBox ID="txtDate" runat="server" TextMode="Date" /></td></tr>
                <tr><td>Time In:</td><td><asp:TextBox ID="txtTimeIn" runat="server" TextMode="Time" /></td></tr>
                <tr><td>Time Out:</td><td><asp:TextBox ID="txtTimeOut" runat="server" TextMode="Time" /></td></tr>
                <tr><td>Remarks:</td><td><asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" /></td></tr>
                <tr><td colspan="2" style="text-align:center;"><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn" /></td></tr>
            </table>

            <h3>Attendance Records</h3>
            <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="False" DataKeyNames="AttendanceID"
                OnRowEditing="gvAttendance_RowEditing" OnRowCancelingEdit="gvAttendance_RowCancelingEdit"
                OnRowUpdating="gvAttendance_RowUpdating" OnRowDeleting="gvAttendance_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
                    <asp:BoundField DataField="EmployeeName" HeaderText="Name" />
                    <asp:BoundField DataField="AttendanceDate" HeaderText="Date" 
    DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                    <asp:BoundField DataField="TimeIn" HeaderText="Time In" />
                    <asp:BoundField DataField="TimeOut" HeaderText="Time Out" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>

            <hr />
            <h3>Generate Attendance Report</h3>
            <table style="width: 1353px;">
                <tr><td>From Date:</td><td><asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" /></td></tr>
                <tr><td>To Date:</td><td><asp:TextBox ID="txtToDate" runat="server" TextMode="Date" /></td></tr>
                <tr><td colspan="2" style="text-align:center;">
                    <asp:Button ID="btnViewReport" runat="server" Text="View Report" CausesValidation="false" OnClick="btnViewReport_Click" />
                </td></tr>
            </table>

           
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1353px">
            </rsweb:ReportViewer>
            </div>


    </form>
</body>
</html>
