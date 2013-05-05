<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.MultiFieldDemoPage"
   CodeBehind="MultiFieldDemoPage.aspx.cs" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
   <h2>
      Scenario: Multi field search
   </h2>
   <p>
      Search for values in multiple fields and refine by location, language, template,
      full text query
   </p>
</asp:Content>
<asp:Content ContentPlaceHolderID="mainPH" runat="server">
  <h4>
      Multi field Parameter</h4>
   Field 1:
   <asp:TextBox ID="FieldName1TextBox" runat="server" />
   <asp:TextBox ID="FieldValue1TextBox" runat="server" />
   <br />
   Inner Condition:
   <asp:DropDownList ID="ConditionList" runat="server">
      <asp:ListItem Text="AND" Value="0" Selected="True" />
      <asp:ListItem Text="OR" Value="2" />
      <asp:ListItem Text="NOT" Value="1" />
   </asp:DropDownList>
   <br />
   Field 2:
   <asp:TextBox ID="FieldName2TextBox" runat="server" />
   <asp:TextBox ID="FieldValue2TextBox" runat="server" />
   <br />
  
</asp:Content>
