<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.NumericRangeDemoPage"
   CodeBehind="NumericRangeDemoPage.aspx.cs" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" %>

   <asp:Content ContentPlaceHolderID="header" runat="server">
   <h2>
      Scenario: Numeric Range Search
   </h2>
   <p>
       Search for numeric ranges and refine by location, language, template, full text query
   </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="mainPH" runat="server">
   <h4>
      Numeric Range Parameter
   </h4>
   <table>
      <tr>
         <td>
            Range1:
         </td>
         <td>
            <asp:TextBox ID="NumericFieldName1TextBox" Text="<first field name>" Width="100%"
               runat="server" />
         </td>
      </tr>
      <tr>
         <td colspan="2">
            From:
            <asp:TextBox ID="NumericStart1TextBox" runat="server" />
            To:
            <asp:TextBox ID="NumericEnd1TextBox" runat="server" />
         </td>
      </tr>
      <tr>
         <td>
            Inner Condition:
         </td>
         <td>
            <asp:DropDownList ID="InnerNumericRangeConditionList" runat="server">
               <asp:ListItem Text="AND" Value="0" Selected="True" />
               <asp:ListItem Text="OR" Value="2" />
               <asp:ListItem Text="NOT" Value="1" />
            </asp:DropDownList>
         </td>
      </tr>
      <tr>
         <td>
            Range2:
         </td>
         <td>
            <asp:TextBox ID="NumericFieldName2TextBox" Text="<second field name>" runat="server" />
         </td>
      </tr>
      <tr>
         <td colspan="2">
            From:
            <asp:TextBox ID="NumericStart2TextBox" runat="server" />
            To:
            <asp:TextBox ID="NumericEnd2TextBox" runat="server" />
         </td>
      </tr>

   </table>
</asp:Content>
