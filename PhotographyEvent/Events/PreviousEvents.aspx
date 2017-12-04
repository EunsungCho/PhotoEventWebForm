<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PreviousEvents.aspx.cs" Inherits="PhotographyEvent.Events.PreviousEvents" %>

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
                                <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" OnClick="btnRetrieve_Click" />
                            </div>
                            <div>
                                <asp:GridView ID="gvPrevEvents" runat="server" AutoGenerateColumns="false" DataKeyNames="eventId" AllowPaging="True">
                                    <Columns>
                                        <asp:BoundField DataField="RowNo" HeaderText="No" />
                                        <%--<asp:BoundField DataField="EventName" HeaderText="Event Title" />--%>
                                        <asp:HyperLinkField DataNavigateUrlFields="eventId" DataNavigateUrlFormatString="EventDetails.aspx?eid={0}" DataTextField="EventName" HeaderText="Event Title" />
                                        <asp:BoundField DataField="EventDate" HeaderText="Date" />
                                        <asp:BoundField DataField="WinnerName" HeaderText="Winner" />
                                        <asp:BoundField DataField="UserCount" HeaderText="No of Participants" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

        </div>
    </section>
    <!--    <div>Previous Events</div>
    <div>
        
    </div> -->
</asp:Content>
