<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.ComboSearchDemoPage"
    CodeBehind="ComboSearchDemoPage.aspx.cs" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>Scenario: Combo Search
    </h2>
    <p>
        Search for numeric and date ranges and refine by location, language, template, full text query
    </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <label>Base Condition:</label>
    <asp:DropDownList ID="BaseConditionList" runat="server">
        <asp:ListItem Text="AND" Value="0" Selected="True" />
        <asp:ListItem Text="OR" Value="2" />
        <asp:ListItem Text="NOT" Value="1" />
    </asp:DropDownList>
    <hr />
    <h5>Date Range Parameter</h5>
    <h6>First Range:</h6>
    <label>Field 1 Name:</label>
    <asp:TextBox ID="DateFieldName1TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>From:</label>
    <asp:TextBox ID="DateStartDate1TextBox" placeholder="range start" CssClass="input-xlarge datepicker" runat="server" />
    <label>To:</label>
    <asp:TextBox ID="DateEndDate1TextBox" placeholder="range end" CssClass="input-xlarge datepicker" runat="server" />
    <h6>Second Range (optional):</h6>
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
    <hr />
    <h5>Numeric Range Parameter</h5>
    <h6>First Range:</h6>
    <label>Field 1 Name:</label>
    <asp:TextBox ID="NumericFieldName1TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>From:</label>
    <asp:TextBox ID="NumericStart1TextBox" placeholder="range start" CssClass="input-xlarge" runat="server" />
    <label>To:</label>
    <asp:TextBox ID="NumericEnd1TextBox" placeholder="range end" CssClass="input-xlarge" runat="server" />
    <h6>Second Range (optional):</h6>
    <label>Inner Condition:</label>
    <asp:DropDownList ID="InnerNumericRangeConditionList" runat="server">
        <asp:ListItem Text="AND" Value="0" Selected="True" />
        <asp:ListItem Text="OR" Value="2" />
        <asp:ListItem Text="NOT" Value="1" />
    </asp:DropDownList>
    <label>Field 2 Name:</label>
    <asp:TextBox ID="NumericFieldName2TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>From:</label>
    <asp:TextBox ID="NumericStart2TextBox" placeholder="range start" CssClass="input-xlarge" runat="server" />
    <label>To:</label>
    <asp:TextBox ID="NumericEnd2TextBox" placeholder="range end" CssClass="input-xlarge" runat="server" />

</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" runat="server">
    <script>
        $(function () {
            $(".datepicker").datepicker();
        });
    </script>
</asp:Content>
