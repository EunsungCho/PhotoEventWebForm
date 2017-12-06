<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="EventDetails.aspx.cs" Inherits="PhotographyEvent.Events.EventDetails" %>

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
                                                There were no participants in this event.
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:BoundField DataField="FirstName" HeaderText="Name" />
                                                <asp:BoundField DataField="PhotoTitle" HeaderText="Title" />
                                                <asp:TemplateField HeaderText="Photo">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnPhoto" runat="server" CommandArgument='<%//# string.Format("{0}|{1}", Eval("EventId"), Eval("UserId")) %>' CommandName="PHOTO" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="score" HeaderText="Score" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </div>
            </section>

        </div>
    </section>
    <!--<div>Event Details</div>
    <div>
       
    </div>
    -->
</asp:Content>
