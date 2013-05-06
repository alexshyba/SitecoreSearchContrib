<%@ Page Language="C#" AutoEventWireup="True" Inherits="scSearchContrib.Demo.RelationSearchDemoPage" MasterPageFile="~/sitecore modules/Web/SearchDemo/Demo.Master" CodeBehind="RelationSearchDemoPage.aspx.cs" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
   <h2>
      Scenario: Search for relations
   </h2>
   <p>
       Search for relations and refine by location, language, template, full text query
   </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="mainPH" runat="server">
    <label>Find relations to:</label>
    <asp:TextBox ID="RelatedIdsTextBox" placeholder="GUID of the item" CssClass="input-xlarge" runat="server" />
</asp:Content>

