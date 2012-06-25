<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.FieldSearchDemoPage"
    MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" CodeBehind="FieldSearchDemoPage.aspx.cs" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>
        Scenario: Search for specific field value
    </h2>
    <p>
    </p>
</asp:Content>
<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <table>
        <tr>
            <td>
                Field Name
            </td>
            <td>
                <asp:TextBox ID="FieldName" Width="300px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Field Value
            </td>
            <td>
                <asp:TextBox ID="FieldValue" Width="300px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Checkbox ID="Partial" Text="Partial Search" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
