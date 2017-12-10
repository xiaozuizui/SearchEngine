<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainWindow.aspx.cs" Inherits="SearchApp.MainWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>LittleMM</title>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/SC.js" type="text/javascript"></script>
    <link href="Style/index.css" rel="stylesheet" type="text/css" />
   
    <script type="text/javascript">
        function doclick(action) {
            $("#action").val(action);
            $("#form1").submit();
        }
    </script>
</head>
<body>

    <form id="form1" runat="server" action="MainWindow.aspx">

       <div>
        <h1>Littlemm Search</h1>
        <hr />

        </div>

        <div id="SearchIndex">
            
            搜索内容：<input type="text" name="content" class="input_text" value="<%=txtContent %>" />
            <input type="submit" value="搜索"  onclick="doclick('SearchIndex')"/>
        
            
    </div>

    </form>


</body>
</html>
