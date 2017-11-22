<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PhotographyEvent.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table>
            <tr>
                <td>User ID:</td>
                <td>
                    <asp:TextBox ID="txtUserId" runat="server" Width="200px"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="User Id required." ControlToValidate="txtUserId"></asp:RequiredFieldValidator>
                </td>
                <td rowspan="2"><asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /></td>
            </tr>
            <tr>
                <td>Password:</td>
                <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Password is requred." ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td></td><td><asp:HyperLink ID="hplCreateNew" runat="server" NavigateUrl="~/CreateNewAccount.aspx" Text="Create User ID"></asp:HyperLink></td>
            </tr>
            <tr><td colspan="3"><asp:Label ID="lblWarnning" runat="server" ForeColor="Red"></asp:Label></td></tr>
        </table>
    </div>
</asp:Content>
