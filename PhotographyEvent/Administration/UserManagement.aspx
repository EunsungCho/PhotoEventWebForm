<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="PhotographyEvent.Administration.UserManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>User Management</div>
    <div>
        <div>
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" DataKeyNames="userId" OnRowCommand="gvUsers_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="User ID">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnUserId" runat="server" Text='<%# Eval("userId") %>' CommandArgument='<%#Eval("userId") %>' />
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:BoundField DataField="firstName" HeaderText="User Name" />
                    <asp:BoundField DataField="emailAddress" HeaderText="Email" />
                    <asp:BoundField DataField="regDate" HeaderText="Date of Register" />
                </Columns>
            </asp:GridView>
        </div>
        <br /><br />
        <div>
            <asp:UpdatePanel ID="upDetails" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="RowCommand" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="lblUser" runat="server"></asp:Label><br />
                    <table>
                        <tr>
                            <td>Won Event:</td>
                            <td style="width: 30%;">
                                <asp:GridView ID="gvWonEvent" runat="server" AutoGenerateColumns="false">
                                    <EmptyDataTemplate>No won events</EmptyDataTemplate>
                                    <Columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="EventId" DataNavigateUrlFormatString="EventDetails.aspx?eventid={0}" HeaderText="Event Title" DataTextField="EventName" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td>Participant Event:</td>
                            <td style="width: 30%;">
                                <asp:GridView ID="gvParticipant" runat="server" AutoGenerateColumns="false">
                                    <EmptyDataTemplate>No participated events</EmptyDataTemplate>
                                    <Columns>
                                        <asp:HyperLinkField DataNavigateUrlFields="EventId" DataNavigateUrlFormatString="EventDetails.aspx?eventid={0}" HeaderText="Event Title" DataTextField="EventName"/>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
