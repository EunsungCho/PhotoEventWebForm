<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="EventDetails.aspx.cs" Inherits="PhotographyEvent.Administration.EventDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function popup(eventId, userId) {
            if (!window.focus) return true;
            var href = 'ViewPhoto.aspx?eid=' + eventId + '&uid=' + userId;
            window.open(href, 'ViewPhoto', 'width=800,height=600,scrollbars=yes');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>Event Details</div>
    <div>
        <table style="width: 70%;">
            <tr>
                <td>Event Title:</td>
                <td><asp:TextBox ID="txtTitle" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Event Rule:</td>
                <td><asp:TextBox ID="txtRule" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Date:</td>
                <td><asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>&nbsp; ~ &nbsp;<asp:TextBox ID="txtToDate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Preview Image:</td>
                <td>
                    <span style="text-align:left;"><asp:Button ID="btnShowImage" runat="server" Text="Show" /></span>
                    <span style="text-align:right;"><asp:FileUpload ID="upImage" runat="server" /><asp:Button ID="btnReplace" runat="server" Text="Replace" OnClick="btnReplace_Click" /></span>
                </td>
            </tr>
            <tr>
                <td>Close Event:</td>
                <td><asp:CheckBox ID="chClose" runat="server" /></td>
            </tr>
            <tr style="height: 50px;">
                <td colspan="2" style="text-align: right; vertical-align: text-top;"><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            </tr>
            <tr>
                <td>Participants:</td>
                <td>
                    <asp:GridView ID="gvParticipants" runat="server" AutoGenerateColumns="false" DataKeyNames="EventId, UserID" OnRowCommand="gvParticipants_RowCommand" OnRowDataBound="gvParticipants_RowDataBound">
                        <Columns>
                            <%--<asp:HyperLinkField DataNavigateUrlFields="userId" DataNavigateUrlFormatString="UserDetails.aspx" DataTextField="userId" HeaderText="User ID" />--%>
                            <asp:BoundField DataField="userId" HeaderText="User ID" />
                            <asp:BoundField DataField="firstName" HeaderText="Name" />
                            <asp:BoundField DataField="PhotoTitle" HeaderText="Title" />
                            <asp:TemplateField HeaderText="Photo">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnShowImg" runat="server" CommandName="SHOW" CommandArgument='<%# string.Format("{0}|{1}", Eval("EventId"), Eval("UserId")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NoOfVotes" HeaderText="No of Votes" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <%--<asp:Button ID="btnTest" runat="server" Text="Popup" OnClientClick="return popup(1, 1);" />--%>
    </div>
</asp:Content>
