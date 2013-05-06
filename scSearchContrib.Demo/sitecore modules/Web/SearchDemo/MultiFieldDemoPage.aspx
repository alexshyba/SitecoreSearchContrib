<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.MultiFieldDemoPage"
    CodeBehind="MultiFieldDemoPage.aspx.cs" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>Scenario: Multi field search
    </h2>
    <p>
        Search for values in multiple fields and refine by location, language, template,
      full text query
    </p>
</asp:Content>
<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <h5>First Field:</h5>
    <label>Field1 Name:</label>
    <asp:TextBox ID="FieldName1TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>Field1 Value:</label>
    <asp:TextBox ID="FieldValue1TextBox" placeholder="field value" CssClass="input-xlarge" runat="server" />
    <h5>Second Field (optional):</h5>
    <label>Inner Condition:</label>
    <asp:DropDownList ID="ConditionList" runat="server">
        <asp:ListItem Text="AND" Value="0" Selected="True" />
        <asp:ListItem Text="OR" Value="2" />
        <asp:ListItem Text="NOT" Value="1" />
    </asp:DropDownList>
    <label>Field2 Name:</label>
    <asp:TextBox ID="FieldName2TextBox" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>Field2 Value:</label>
    <asp:TextBox ID="FieldValue2TextBox" placeholder="field value" CssClass="input-xlarge" runat="server" />
</asp:Content>
