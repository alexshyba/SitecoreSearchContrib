<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.FieldSearchDemoPage"
    MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" CodeBehind="FieldSearchDemoPage.aspx.cs" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>Scenario: Search for specific field value</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <label>Field Name:</label>
    <asp:TextBox ID="FieldName" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>Field Value:</label>
    <asp:TextBox ID="FieldValue" placeholder="field value" CssClass="input-xlarge" runat="server" />
    <label class="checkbox">
        <input type="checkbox" id="Partial" runat="server"/> Partial Search
    </label>
</asp:Content>
