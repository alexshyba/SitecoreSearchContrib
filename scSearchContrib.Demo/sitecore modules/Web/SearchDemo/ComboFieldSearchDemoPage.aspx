<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.ComboFieldSearchDemoPage"
    CodeBehind="ComboFieldSearchDemoPage.aspx.cs" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <h2>Scenario: Combo Field Search
    </h2>
    <p>
        Search using two field search parameters
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
    <h5>First Field Search Parameter:</h5>
    <label>Field Name:</label>
    <asp:TextBox ID="Field1Name" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>Field Value:</label>
    <asp:TextBox ID="Field1Value" placeholder="field value" CssClass="input-xlarge" runat="server" />
    <label class="checkbox">
        <input type="checkbox" id="Partial1" runat="server" />
        Partial Search
    </label>
    <label>Condition:</label>
    <asp:DropDownList ID="FieldSearchParameter1ConditionList" runat="server">
        <asp:ListItem Text="AND" Value="0" Selected="True" />
        <asp:ListItem Text="OR" Value="2" />
        <asp:ListItem Text="NOT" Value="1" />
    </asp:DropDownList>
    <hr />

    <h5>Second Field Search Parameter:</h5>
    <label>Field Name:</label>
    <asp:TextBox ID="Field2Name" placeholder="lowercased field name" CssClass="input-xlarge" runat="server" />
    <label>Field Value:</label>
    <asp:TextBox ID="Field2Value" placeholder="field value" CssClass="input-xlarge" runat="server" />
    <label class="checkbox">
        <input type="checkbox" id="Partial2" runat="server" />
        Partial Search
    </label>
    <label>Condition:</label>
    <asp:DropDownList ID="FieldSearchParameter2ConditionList" runat="server">
        <asp:ListItem Text="AND" Value="0" Selected="True" />
        <asp:ListItem Text="OR" Value="2" />
        <asp:ListItem Text="NOT" Value="1" />
    </asp:DropDownList>

</asp:Content>
