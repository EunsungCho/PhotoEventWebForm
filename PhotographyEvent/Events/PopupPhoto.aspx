<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupPhoto.aspx.cs" Inherits="PhotographyEvent.Events.PopupPhoto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblPhotoTitle" runat="server"></asp:Label><br />
            <asp:Image ID="imgPhoto" runat="server" />
        </div>
    </form>
</body>
</html>
