<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master"
    Inherits="scSearchContrib.Demo.SortDemoPage" CodeBehind="SortDemoPage.aspx.cs" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>
        Scenario: Basic Search with <i>Sorting</i>
    </h2>
    <p>
        Search, <strong>sort</strong> and refine by location, language, template, full text query
    </p>
</asp:Content>
<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <table>
        <tr>
            <td>
                Sort by:
            </td>
            <td>
                <asp:TextBox ID="SortByTextBox" Width="300px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Descending:
            </td>
            <td>
                <asp:CheckBox ID="DescendingCheckBox" Text="Descending" Checked="true" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
