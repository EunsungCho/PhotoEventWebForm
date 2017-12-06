<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="AccountInfo.aspx.cs" Inherits="PhotographyEvent.Events.AccountInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="container">
        <div class="wrap-container zerogrid">

            <!-----------------content-box-2-------------------->
            <section class="content-box">
                <div class="row wrap-box">
                    <div class="box-text bg-3 col-full">
                        <div>
                            <div>
                                <table>
                                    <tr>
                                        <td style="text-align: right;">User ID:</td>
                                        <td>
                                            <asp:Label ID="lblUserId" runat="server"></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Password:</td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" ValidationGroup="password"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Retype:</td>
                                        <td>
                                            <asp:TextBox ID="txtRetypePass" runat="server" TextMode="Password"></asp:TextBox>&nbsp;<asp:Button ID="btnChngPass" runat="server" Text="Change" OnClick="btnChngPass_Click" ValidationGroup="password" /></td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvRepass" runat="server" ControlToValidate="txtRetypePass" ValidationGroup="password" ErrorMessage="Retype password."></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvPass" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtRetypePass" ErrorMessage="Type password correctly again." ValidationGroup="password"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Email Address:</td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email address is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Please Enter Valid Email ID" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">First Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

        </div>
    </section>

    <!--<div>Account Information</div>
   -->
</asp:Content>
