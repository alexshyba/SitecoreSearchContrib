<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.NumericRangeDemoPage"
    CodeBehind="NumericRangeDemoPage.aspx.cs" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>Scenario: Numeric Range Search
    </h2>
    <p>
        Search for numeric ranges and refine by location, language, template, full text query
    </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <h5>First Range:</h5>
    <label>Field 1 Name:</label>
    <asp:TextBox ID="NumericFieldName1TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>From:</label>
    <asp:TextBox ID="NumericStart1TextBox" placeholder="range start" CssClass="input-xlarge" runat="server" />
    <label>To:</label>
    <asp:TextBox ID="NumericEnd1TextBox" placeholder="range end" CssClass="input-xlarge" runat="server" />
    <h5>Second Range (optional):</h5>
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
