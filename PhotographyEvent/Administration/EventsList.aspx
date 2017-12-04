<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="EventsList.aspx.cs" Inherits="PhotographyEvent.Administration.EventsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#txtFromDate").datepicker({
                dateFormat: "dd/mm/yy"
            });
            $("#txtToDate").datepicker({
                dateFormat: "dd/mm/yy"
            });
        });
    </script>
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
                                <asp:Button ID="btnRetieve" runat="server" Text="Retrieve" OnClick="btnRetieve_Click" />
                            </div>
                            <div>
                                <asp:GridView ID="gvEventsList" runat="server" DataKeyNames="EventId" AllowPaging="True" AutoGenerateColumns="False" Width="70%">
                                    <Columns>
                                        <asp:BoundField DataField="RowNo" HeaderText="No" />
                                        <asp:HyperLinkField DataNavigateUrlFields="EventId" DataNavigateUrlFormatString="EventDetails.aspx?eventid={0}" DataTextField="EventName" HeaderText="Event Title" />
                                        <asp:BoundField DataField="EventDate" HeaderText="Date" />
                                        <asp:BoundField DataField="Winner" HeaderText="Winner" />
                                        <asp:BoundField DataField="NoParticipants" HeaderText="No of Participants" />
                                        <asp:BoundField DataField="IsClosed" HeaderText="Is Closed" />

                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="border: solid; width: 70%;">
                                <table style="width: 100%;">
                                    <tr>
                                        <th style="width: 20%;">Event Title:</th>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtTitle" runat="server" Width="100%"></asp:TextBox></td>

                                    </tr>
                                    <tr>
                                        <th>Rule:</th>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtRule" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox></td>

                                    </tr>
                                    <tr>
                                        <th>Date:</th>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            ~
                                        <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <th>Preview Image:</th>
                                        <td>
                                            <asp:FileUpload ID="upPrevImage" runat="server" />
                                        </td>
                                        <td style="text-align: center;"></td>
                                    </tr>
                                </table>
                                <div>
                                    <asp:Button ID="btnSave" runat="server" Text="Add New Event" OnClick="btnSave_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

        </div>
    </section>
    <!--
    <div>Events List</div>
    -->
</asp:Content>
