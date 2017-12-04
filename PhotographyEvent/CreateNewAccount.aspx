<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CreateNewAccount.aspx.cs" Inherits="PhotographyEvent.CreateNewAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            $("#txtAdminCode").hide();
            $("#lblAdminKeyCode").hide();
            $("#chkAdmin").click(function () {
                if ($(this).is(':checked')) {
                    $("#txtAdminCode").show();
                    $("#lblAdminKeyCode").show();
                }
                else {
                    $("#txtAdminCode").hide();
                    $("#lblAdminKeyCode").hide();
                };
            });
        })

        function showSuccess() {
            alert('New account created. Please login.');
            document.location.href = 'Main.aspx';
        }

        function hideCheckIdLabel() {
            $("#lblCheckId").hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="container">
        <div class="wrap-container zerogrid">

            <!-----------------content-box-2-------------------->
            <section class="content-box">
                <div class="row wrap-box">
                    <!--Start Box-->
                    <div class="box-text bg-3 col-full">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkAdmin" runat="server" Text="Administrator<br />Registration:" ClientIDMode="Static" /></td>
                                <td>
                                    <asp:TextBox ID="txtAdminCode" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblAdminKeyCode" runat="server" Text="Input Administrator Key Code" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>User ID:</td>
                                <td>
                                    <asp:TextBox ID="txtUserId" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnCheckId" runat="server" Text="Check ID" OnClick="btnCheckId_Click" CausesValidation="false" /><asp:Label ID="lblCheckId" runat="server" ClientIDMode="Static"></asp:Label><asp:RequiredFieldValidator ID="rfvUserId" runat="server" ErrorMessage="User Id is required" ForeColor="Red" ControlToValidate="txtUserId"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdIdChecked" runat="server" Value="" ClientIDMode="Static" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                            <tr>
                                <td>Password</td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required" ForeColor="Red" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Retype Password</td>
                                <td>
                                    <asp:TextBox ID="txtRetypePassword" runat="server" TextMode="Password"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvRepass" runat="server" ErrorMessage="Retypre password here." ControlToValidate="txtRetypePassword" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvRepass" runat="server" ErrorMessage="Please retype password correctly." ControlToCompare="txtRetypePassword" ControlToValidate="txtPassword" ForeColor="Red"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Email Address:</td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Your Email address is required." ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Please Enter Valid Email ID" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button ID="btnCreateUser" runat="server" Text="Create New User ID" OnClientClick="hideCheckIdLabel()" OnClick="btnCreateUser_Click" /></td>
                                <td></td>
                            </tr>
                            <%--<tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>--%>
                        </table>
                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                    </div>
                </div>
            </section>

        </div>
    </section>

    <!--
    
    -->
</asp:Content>
