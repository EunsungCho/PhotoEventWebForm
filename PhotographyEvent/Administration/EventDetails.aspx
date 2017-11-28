<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="EventDetails.aspx.cs" Inherits="PhotographyEvent.Administration.EventDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#txtStartDate").datepicker({
                dateFormat: "dd/mm/yy"
            });
            $("#txtToDate").datepicker({
                dateFormat: "dd/mm/yy"
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $("#chClose").click(function (e) {
                if ($(this).is(':checked')) {
                    if (!confirm('Do you really want to close this event?')) {
                        e.preventDefault();
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    if (!confirm('Are you suer you cancel to close this event?')) {
                        e.preventDefault();
                        return false;
                    } else {
                        return true;
                    }
                }
                
            })
        })

        function popup(eventId, userId) {
            if (!window.focus) return true;
            var href = 'ViewPhoto.aspx?eid=' + eventId + '&uid=' + userId;
            window.open(href, 'ViewPhoto', 'width=1000,height=600,scrollbars=yes');
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
                <td>
                    <asp:TextBox ID="txtTitle" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Event Rule:</td>
                <td>
                    <asp:TextBox ID="txtRule" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Date:</td>
                <td>
                    <asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static"></asp:TextBox>&nbsp; ~ &nbsp;<asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Preview Image:</td>
                <td>
                    <span style="text-align: left;">
                        <asp:Button ID="btnShowImage" runat="server" Text="Show" />                       
                    </span>
                    <span style="text-align: right;">
                        <asp:FileUpload ID="upImage" runat="server" />
                        <asp:Button ID="btnReplace" runat="server" Text="Replace" OnClick="btnReplace_Click" ValidationGroup="uploadIamge" />
                        <asp:RequiredFieldValidator ID="rfvUploadImage" runat="server" ValidationGroup="uploadImage" ControlToValidate="upImage" ErrorMessage="No files to replace." ForeColor="Red"></asp:RequiredFieldValidator>
                    </span>
                </td>
            </tr>
            <tr>
                <td>Close Event:</td>
                <td>
                    <asp:CheckBox ID="chClose" runat="server" OnCheckedChanged="chClose_CheckedChanged" ClientIDMode="Static" AutoPostBack="true" /></td>
            </tr>
            <tr style="height: 50px;">
                <td colspan="2" style="text-align: right; vertical-align: text-top;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    <asp:HiddenField ID="hdnWinner" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Participants:</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
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
                                    <asp:TemplateField HeaderText="Set Winner">
                                        <ItemTemplate>
                                            <asp:Button ID="btnWinner" runat="server" Text="Set" CommandArgument='<%# Eval("UserId") %>' CommandName="WINNER" />                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <%--<asp:GridView ID="gvParticipants" runat="server" AutoGenerateColumns="false" DataKeyNames="EventId, UserID" OnRowCommand="gvParticipants_RowCommand" OnRowDataBound="gvParticipants_RowDataBound">
                        <Columns>
                            
                            <asp:BoundField DataField="userId" HeaderText="User ID" />
                            <asp:BoundField DataField="firstName" HeaderText="Name" />
                            <asp:BoundField DataField="PhotoTitle" HeaderText="Title" />
                            <asp:TemplateField HeaderText="Photo">
                                <itemtemplate>
                                            <asp:ImageButton ID="btnShowImg" runat="server" CommandName="SHOW" CommandArgument='<%# string.Format("{0}|{1}", Eval("EventId"), Eval("UserId")) %>' />
                                        </itemtemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NoOfVotes" HeaderText="No of Votes" />
                            <asp:TemplateField HeaderText="Winner">
                                <itemtemplate>
                                            <asp:Button ID="btnWinner" runat="server" Text="Set" CommandArgument='<%# Eval("UserId") %>' CommandName="WINNER" />
                                        </itemtemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
