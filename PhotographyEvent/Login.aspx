<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PhotographyEvent.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <section id="container">
        <div class="wrap-container zerogrid">

            <!-----------------content-box-2-------------------->
            <section class="content-box box-2">
                <div class="row wrap-box">
                    <!--Start Box-->
                    <div class="col-1-2">
                        <div class="box-text bg-2">
                            User ID:
                            <br />
                            <asp:TextBox ID="txtUserId" runat="server" Width="200px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="User Id required." ControlToValidate="txtUserId"></asp:RequiredFieldValidator>
                            <br />

                            Password:
                            <br />
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Password is requred." ControlToValidate="txtPassword"></asp:RequiredFieldValidator><br />

                            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                            <br />

                            <asp:HyperLink ID="hplCreateNew" runat="server" NavigateUrl="~/CreateNewAccount.aspx" Text="Create User ID"></asp:HyperLink><br />
                            <asp:Label ID="lblWarnning" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </div>

                    <div class="col-1-2">
                        <div class="box-text bg-3">
                            <div class="heading">
                                <span class="prefix">We</span>
                                <h2>Photography event</h2>
                            </div>
                            <div class="content">
                                <p>Massive Dynamic has over 10 years of experience in Design, Technology and Marketing. We take pride in delivering Intelligent Designs and Engaging Experiences for clients all over the World.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

        </div>
    </section>

</asp:Content>
