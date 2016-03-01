<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style type="text/css">
        .auto-style1 {
            width: 37%;
        }

        .auto-style2 {
            width: 80px;
        }

        .auto-style3 {
            width: 80px;
            height: 26px;
        }

        .auto-style4 {
            width: 35px;
        }

        .auto-style5 {
            width: 35px;
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>

            <br />

            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="Label2" runat="server" Text="Start Date"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Data introdusa nu este corecta" Type="Date" ControlToValidate="TextBoxStartDate" Operator="DataTypeCheck" ></asp:CompareValidator>
                        <asp:TextBox ID="TextBoxStartDate" runat="server" Style="height: 22px" TextMode="DateTime" OnTextChanged="textBoxStartDate_TextChanged"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="textBoxStartDate_CalendarExtender" runat="server" TargetControlID="textBoxStartDate" PopupButtonID="textBoxStartDate" Format="MM/dd/yyyy" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label3" runat="server" Text="End Date"></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Data introdusa nu este corecta" Type="Date" ControlToValidate="TextBoxEndDate" Operator="DataTypeCheck"></asp:CompareValidator>
                        <asp:TextBox ID="TextBoxEndDate" runat="server" Style="height: 22px" TextMode="DateTime"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="textBoxEndDate_CalendarExtender" runat="server" TargetControlID="textBoxEndDate" PopupButtonID="textBoxEndDate" Format="MM/dd/yyyy" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="Label1" runat="server" Text="Ip"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="TextBoxIp" runat="server" Style="height: 22px"></asp:TextBox>
                    </td>
                </tr>
            </table>

        </div>
        <div>

            <asp:Button ID="submitButton" runat="server" BorderStyle="Outset" OnClick="submitButton_Click" Text="Submit" Font-Bold="True" Font-Italic="False" Font-Overline="False" Height="30px" Width="70px" />

        </div>
        <p>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="true" EmptyDataText="No data was found!" ShowHeaderWhenEmpty ="true" >
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
                <EditRowStyle BackColor="#7C6F57" />
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
            <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:CsvDatabaseConnectionString %>" SelectCommand="selectProcedure" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="false">
                <SelectParameters>
                    <asp:ControlParameter ControlID="TextBoxStartDate" DefaultValue="" Name="startDate"  PropertyName="Text" ConvertEmptyStringToNull="true" Type="DateTime"/>
                    <asp:ControlParameter ControlID="TextBoxEndDate" DefaultValue="" Name="endDate" PropertyName="Text" ConvertEmptyStringToNull="true" Type="DateTime" />
                    <asp:ControlParameter ControlID="TextBoxIp" Name="Ip" PropertyName="Text" ConvertEmptyStringToNull="true" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </p>
        <p>
            &nbsp;
        </p>
    </form>
</body>
</html>
