<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="EmployeeAttendance.Attendance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Attendance</title>
    <style>
    body {
    font-family: 'Roboto', sans-serif;
    background-color: #f9f9f9;
    font-size: 18px; /* Base font size */
}

    h2, h3 {
        color: #333;
        font-size: 28px;
        text-align: center;
    }

    .form-container {
        margin: 30px auto;
        padding: 30px;
        max-width: 900px;
        background-color: #fff;
        border-radius: 12px;
        box-shadow: 0 0 12px rgba(0, 0, 0, 0.1);
    }

    label {
        font-size: 20px;
        font-weight: 500;
    }

    table {
        border-collapse: collapse;
        width: 100%;
        max-width: 900px;
        font-size: 18px;
    }

    td {
        padding: 12px;
        vertical-align: top;
    }

    input, textarea {
        width: 100%;
        padding: 10px;
        font-size: 18px;
        box-sizing: border-box;
        border: 1px solid #ccc;
        border-radius: 6px;
    }

    input[type="submit"],
    .btn {
        background-color: #007BFF;
        color: white;
        padding: 12px 24px;
        font-size: 18px;
        border: none;
        border-radius: 8px;
        cursor: pointer;
        margin-top: 10px;
        transition: background-color 0.3s ease;
    }

    input[type="submit"]:hover,
    .btn:hover {
        background-color: #0056b3;
    }
    .table {
        border: 1px solid #ccc;
        width: 100%;
        font-size: 18px;
    }
    .table th {
        background-color: #f2f2f2;
        padding: 12px;
        text-align: left;
        font-size: 20px;
    }

    .table td {
        border-top: 1px solid #ccc;
        padding: 12px;
        font-size: 18px;
    }

    .action-link {
        font-size: 18px;
        margin-right: 10px;
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
        <div style="margin: 20px;">
            <h2>Employee Attendance Form</h2>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <table>
                <tr>
                    <td>Employee ID:</td>
                    <td><asp:TextBox ID="txtEmployeeID" runat="server" required="required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Employee Name:</td>
                    <td><asp:TextBox ID="txtEmployeeName" runat="server" required="required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Date:</td>
                    <td><asp:TextBox ID="txtDate" runat="server" TextMode="Date" required="required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Time In:</td>
                    <td><asp:TextBox ID="txtTimeIn" runat="server" TextMode="Time" required="required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Time Out:</td>
                    <td><asp:TextBox ID="txtTimeOut" runat="server" TextMode="Time" required="required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Remarks:</td>
                    <td><asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <asp:HiddenField ID="hfAttendanceID" runat="server" />
                    </td>
                </tr>
            </table>

            <hr />

            <h3>Attendance Records</h3>
            <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="False" CssClass="table"
                OnRowEditing="gvAttendance_RowEditing"
                OnRowCancelingEdit="gvAttendance_RowCancelingEdit"
                OnRowUpdating="gvAttendance_RowUpdating"
                OnRowDeleting="gvAttendance_RowDeleting"
                DataKeyNames="AttendanceID">
                <Columns>
                    <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" />
                    <asp:BoundField DataField="AttendanceDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="TimeIn" HeaderText="Time In" />
                    <asp:BoundField DataField="TimeOut" HeaderText="Time Out" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
