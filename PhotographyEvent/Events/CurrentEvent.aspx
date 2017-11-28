<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CurrentEvent.aspx.cs" Inherits="PhotographyEvent.Events.CurrentEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>Current Event</div>
    <div>
        <div>
            <table>
                <tr>
                    <td style="vertical-align: top;">
                        <div>
                            <asp:Image ID="imgPreview" runat="server" Width="400px" />
                        </div>
                        <div>
                            <asp:Label ID="lblRule" runat="server" Width="400px"></asp:Label>
                        </div>
                    </td>
                    <td>
                        <asp:GridView ID="gvEventUsers" runat="server" AutoGenerateColumns="false" DataKeyNames="EventId, UserId" OnRowCommand="gvEventUsers_RowCommand" OnRowDataBound="gvEventUsers_RowDataBound">
                            <EmptyDataTemplate>
                                There is no current event now.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField DataField="FirstName" HeaderText="Name" />
                                <asp:BoundField DataField="PhotoTitle" HeaderText="Title" />
                                <asp:TemplateField HeaderText="Photo">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnPhoto" runat="server" CommandArgument='<%# string.Format("{0}|{1}", Eval("EventId"), Eval("UserId")) %>' CommandName="PHOTO" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="score" HeaderText="Score" />
                                <asp:TemplateField HeaderText="Vote">
                                    <ItemTemplate>
                                        <asp:Button ID="btnVote" runat="server" Text="Vote" CommandArgument='<%# string.Format("{0}|{1}", Eval("EventId"), Eval("UserId")) %>' CommandName="VOTE" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserIdToVote" HeaderText="VoteId" Visible="false" />
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Label ID="lblVoteResponse" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div>Participation</div>
        <div>
            <asp:Panel ID="pnUpload" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>Photo Title: </td>
                        <td>
                            <asp:TextBox ID="txtPhotoTitle" runat="server" Width="400px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:FileUpload ID="fuPhoto" runat="server" Width="400px" /><asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /></td>
                    </tr>
                </table>
                <div>
                    <asp:Label ID="lblResult" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
