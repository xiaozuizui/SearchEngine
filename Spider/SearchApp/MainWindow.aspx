﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainWindow.aspx.cs" Inherits="SearchApp.MainWindow" %>

<!DOCTYPE html >

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
            console.log(1000);
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

        <div>
            <table class="table_list" cellpadding="0" cellspacing="0">
    <tbody>
    <%
        if (list != null && list.Count > 0)
        {
            %>
         <tr>
            <td>标题</td>
            <td>内容</td>
           
        </tr>
            <%
            foreach (Web.Model.Record obj in list)
            {
            %>
            <tr>
                <td><%=obj.Title%></td>
                <td><%=obj.Content%></td>
            
            </tr>
            <%
            }
            %>
                <tr><td colspan="3" style="text-align:left;">一共找到<strong><%=list.Count%></strong>条数据，共耗费<strong><%=lSearchTime%></strong>毫秒</td></tr>
            <%
        }
         %>
         </tboyd>
         <%
              if (list != null && list.Count > 0)
        {
              %>
         <tfoot>
            <tr><td colspan="2"><%=txtPageFoot %></td></tr>
         </tfoot>
         <%} %>
    </table>
        </div>
        <input type="hidden" name="action" id="action" value="default" />
    </form>


</body>
</html>
