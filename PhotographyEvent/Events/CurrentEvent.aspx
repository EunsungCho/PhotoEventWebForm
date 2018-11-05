<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CurrentEvent.aspx.cs" Inherits="PhotographyEvent.Events.CurrentEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="container">
        <div class="wrap-container zerogrid">

            <!-----------------content-box-2-------------------->
            <section class="content-box">
                <div class="row wrap-box">
                    <!--Start Box-->
                    <div class="box-text bg-3 col-full">
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
                                            <asp:UpdatePanel ID="upanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvEventUsers" runat="server" AutoGenerateColumns="false" DataKeyNames="EventId, UserId" OnRowCommand="gvEventUsers_RowCommand" OnRowDataBound="gvEventUsers_RowDataBound">
                                                        <EmptyDataTemplate>
                                                            There is no current event Participants now.
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
                                                    <asp:Label ID="lblVoteResponse" runat="server"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvEventUsers" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnUpdateTitle" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <%--<asp:GridView ID="gvEventUsers" runat="server" AutoGenerateColumns="false" DataKeyNames="EventId, UserId" OnRowCommand="gvEventUsers_RowCommand" OnRowDataBound="gvEventUsers_RowDataBound">
                                                <EmptyDataTemplate>
                                                    There is no current event Participants now.
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
                                            </asp:GridView>--%>
                                            <br />
                                            <%--<asp:Label ID="lblVoteResponse" runat="server"></asp:Label>--%>
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
                                                <asp:UpdatePanel ID="upanelTitle" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtPhotoTitle" runat="server" Width="400px"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblTitleResult" runat="server" Text=""></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnUpdateTitle" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <%--<asp:TextBox ID="txtPhotoTitle" runat="server" Width="400px"></asp:TextBox>--%>
                                                <asp:Button ID="btnUpdateTitle" runat="server" Text="Modify" OnClick="btnUpdateTitle_Click" />&nbsp;&nbsp;<%--<asp:Label ID="lblTitleResult" runat="server" Text=""></asp:Label>--%>
                                            </td>
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
                    </div>
                </div>
            </section>

        </div>
    </section>

    <!--   <div>Current Event</div>
   
    -->
</asp:Content>
