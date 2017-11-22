<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="PhotographyEvent.Test.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="testGrid" runat="server" AllowPaging="True" OnPageIndexChanged="testGrid_PageIndexChanged" OnPageIndexChanging="testGrid_PageIndexChanging"></asp:GridView>
        </div>
        <div>
            <asp:Button ID="btnDataCreate" runat="server" Text="Create Data" OnClick="btnDataCreate_Click" /><br />
            <asp:Button ID="btnCreateUser" runat="server" Text="Create User" OnClick="btnCreateUser_Click" /><br />
            <asp:Button ID="btnAuth" runat="server" Text="Authenticate User" OnClick="btnAuth_Click" /><br />
            <asp:Button ID="btnCreateEvent" runat="server" Text="Create Event" OnClick="btnCreateEvent_Click" /><br />
            <asp:FileUpload ID="upImage" runat="server" /><asp:Button ID="btnReplace" runat="server" OnClick="btnReplace_Click" /><br />
        </div>
    </form>
</body>
</html>
