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
    <label>Sort by:</label>
    <asp:TextBox ID="SortByTextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label class="checkbox">
        <input type="checkbox" id="DescendingCheckBox" checked="True" runat="server" ClientIDMode="Static" /> Descending
    </label>
</asp:Content>
