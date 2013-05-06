<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.DateRangeDemoPage"
    CodeBehind="DateRangeDemoPage.aspx.cs" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>Scenario: Date Range Search
    </h2>
    <p>
        Search for date ranges and refine by location, language, template, full text query
    </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <h5>First Range:</h5>
    <label>Field 1 Name:</label>
    <asp:TextBox ID="DateFieldName1TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>From:</label>
    <asp:TextBox ID="DateStartDate1TextBox" placeholder="range start" CssClass="input-xlarge datepicker" runat="server" />
    <label>To:</label>
    <asp:TextBox ID="DateEndDate1TextBox" placeholder="range end" CssClass="input-xlarge datepicker" runat="server" />
    <h5>Second Range (optional):</h5>
    <label>Inner Condition:</label>
    <asp:DropDownList ID="InnerDateRangeConditionList" runat="server">
        <asp:ListItem Text="AND" Value="0" Selected="True" />
        <asp:ListItem Text="OR" Value="2" />
        <asp:ListItem Text="NOT" Value="1" />
    </asp:DropDownList>
    <label>Field 2 Name:</label>
    <asp:TextBox ID="DateFieldName2TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>From:</label>
    <asp:TextBox ID="DateStartDate2TextBox" placeholder="range start" CssClass="input-xlarge datepicker" runat="server" />
    <label>To:</label>
    <asp:TextBox ID="DateEndDate2TextBox" placeholder="range end" CssClass="input-xlarge datepicker" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="scripts" runat="server">
      <script>
        $(function () {
            $(".datepicker").datepicker();
        });
    </script>
</asp:Content>
