<%@ Page Language="C#" AutoEventWireup="True" %>

<%@ Register TagPrefix="demo" TagName="TopNav" Src="~/sitecore modules/Web/SearchDemo/TopNav.ascx" %>
<%@ Register TagPrefix="demo" TagName="Header" Src="~/sitecore modules/Web/SearchDemo/Header.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Welcome to Sitecore.Search extension demo!</title>
    <link href="Demo.css" rel="Stylesheet" />
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="js/bootstrap.js" type="text/javascript"></script>
</head>
<body data-spy="scroll" data-target=".subnav" data-offset="50">
    <demo:Header runat="server" />
    <demo:TopNav runat="server" />
</body>
</html>
