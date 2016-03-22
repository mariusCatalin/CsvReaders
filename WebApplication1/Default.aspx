<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style type="text/css">
        .auto-style1 {
            width: 22%;
            height: 133px;
            margin-bottom: 5px;
            border-style: outset;
            border-color: ActiveCaption;
                    }
 
        .myButton{
            color: black;
            border-radius: 4px;
            text-decoration:solid;
            text-shadow: 0 1px 1px hsla(0, 0%, 0%, 0.20)
            
        }
        
        .auto-style3 {
            width: 80px;
            height: 8px;
        }
        .gridView{
            margin-bottom: 5px;
            border:2px solid #456879;
	        border-radius:10px;
        }

        .auto-style5 {
            width: 230px;
            height: 8px;
        }
        .auto-style6 {
            width: 80px;
            height: 53px;
        }
        .auto-style7 {
            width: 230px;
            height: 53px;
        }
        .auto-style8 {
            width: 80px;
            height: 43px;
        }
        .auto-style9 {
            width: 160px;
            height: 43px;
            border:2px solid #456879;
	        border-radius:10px;
	

        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>

            <br />

            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <table class="auto-style1" aria-dropeffect="none" >
                <tr>
                    <td class="auto-style8">
                        <asp:Label ID="Label2" runat="server" Text="Start Date" Font-Italic="True"></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="TextBoxStartDate" runat="server" Style="height: 22px" TextMode="DateTime" OnClick="this.value=''" CssClass ="auto-style9"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="textBoxStartDate_CalendarExtender" runat="server" TargetControlID="textBoxStartDate" PopupButtonID="startDateImage" Format="MM/dd/yyyy" />
                        <asp:Image ID="startDateImage" runat="server" ImageUrl="~/Calendar-icon.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label3" runat="server" Text="End Date" Font-Italic="True"></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="TextBoxEndDate" runat="server" Style="height: 22px" OnClick="this.value=''" TextMode="DateTime" CssClass ="auto-style9"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="textBoxEndDate_CalendarExtender" runat="server" TargetControlID="textBoxEndDate" PopupButtonID="endDateImage" Format="MM/dd/yyyy" />
                        <asp:Image ID="endDateImage" runat="server" ImageUrl="~/Calendar-icon.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        <asp:Label ID="Label1" runat="server" Text="Ip" Font-Italic="True"></asp:Label>
                    </td>
                    <td class="auto-style7">
                        <asp:TextBox ID="TextBoxIp" runat="server" Style="height: 22px" CssClass ="auto-style9"></asp:TextBox>
                    </td>
                </tr>
            </table>

        </div>
        <div>

            <asp:Button ID="submitButton" runat="server" BorderStyle="Outset" OnClick="submitButton_Click" Text="Submit" Font-Bold="True" Font-Italic="False" Font-Overline="False" Height="30px" Width="99px" BorderColor="Black" CssClass="myButton" />

        </div>
        <p>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" AllowPaging="True" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" AllowSorting="true" EmptyDataText="No data was found!" ShowHeaderWhenEmpty ="true" OnPageIndexChanging="GridView1_PageIndexChanging" OnSorting="GridView1_Sorting" BorderStyle="Solid" CellSpacing="1" CssClass="gridView" >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                    <asp:BoundField DataField="LogTime" HeaderText="LogTime" SortExpression="LogTime" />
                    <asp:BoundField DataField="Action" HeaderText="Action" SortExpression="Action" />
                    <asp:BoundField DataField="FolderPath" HeaderText="FolderPath" SortExpression="FolderPath" />
                    <asp:BoundField DataField="Filename" HeaderText="Filename" SortExpression="Filename" />
                    <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                    <asp:BoundField DataField="IpAdress" HeaderText="IpAdress" SortExpression="IpAdress" />
                    <asp:BoundField DataField="XferSize" HeaderText="XferSize" SortExpression="XferSize" />
                    <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" />
                    <asp:BoundField DataField="AgentBrand" HeaderText="AgentBrand" SortExpression="AgentBrand" />
                    <asp:BoundField DataField="AgentVersion" HeaderText="AgentVersion" SortExpression="AgentVersion" />
                    <asp:BoundField DataField="Error" HeaderText="Error" SortExpression="Error" />
                </Columns>
                <EditRowStyle HorizontalAlign="Right" VerticalAlign="Top" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            <%--<asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:CsvDatabaseConnectionString %>" SelectCommand="selectProcedure" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="false">
                <SelectParameters>
                    <asp:ControlParameter ControlID="TextBoxStartDate" DefaultValue="" Name="startDate"  PropertyName="Text" ConvertEmptyStringToNull="true" Type="DateTime"/>
                    <asp:ControlParameter ControlID="TextBoxEndDate" DefaultValue="" Name="endDate" PropertyName="Text" ConvertEmptyStringToNull="true" Type="DateTime" />
                    <asp:ControlParameter ControlID="TextBoxIp" Name="Ip" PropertyName="Text" ConvertEmptyStringToNull="true" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>--%>
        </p>
        <p>
            &nbsp;
        </p>
    </form>
</body>
</html>
