<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllCentersReport1.aspx.cs"
    Inherits="AllCentersReport1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>..:: Car Sales System ::..</title>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <link href="css/css.css" rel="stylesheet" type="text/css" />

    <script src="js/overlibmws.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript" src="js/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="js/emulatetab.joelpurra.js"></script>

    <script type="text/javascript" src="js/plusastab.joelpurra.js"></script>

    <script type='text/javascript' language="javascript" src='js/jquery.alphanumeric.pack.js'></script>

    <script src="js/jquery.formatCurrency-1.4.0.js" type="text/javascript"></script>

    <script src="Static/JS/calendar.js" type="text/javascript"></script>

    <link href="Static/Css/calender.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">window.history.forward(1);</script>

    <script type="text/javascript" language="javascript">
    
       function poptastic(url)
{
	newwindow=window.open(url,'name','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,height=420,width=300');
	    if (window.focus) {newwindow.focus()}
    }
    
     function isNumberKeyForDt(evt) {	

                    var charCode = (evt.which) ? evt.which : event.keyCode
                    if (charCode > 31 && (charCode < 48 || charCode > 57)&& charCode != 47)
                    return false;
                    return true;
                    }
    
    
        function ValidateData() {
            var valid = true;
            var today = new Date();
            var month = today.getMonth() + 1
            var day = today.getDate()
            var year = today.getFullYear()
            today = month + "/" + day + "/" + year
            var today = new Date(today);
            var SDate = document.getElementById('<%= txtStartDate.ClientID %>').value;
            var EDate = document.getElementById('<%= txtEndDate.ClientID %>').value;
            var endDate = new Date(EDate);
            var startDate = new Date(SDate);
            var Startmonth = startDate.getMonth() + 1
            var Startday = startDate.getDate()
            var Startyear = startDate.getFullYear()
            startDate = Startmonth + "/" + Startday + "/" + Startyear
            var startDate = new Date(startDate);

            var Endmonth = endDate.getMonth() + 1
            var Endday = endDate.getDate()
            var Endyear = endDate.getFullYear()
            var oneDay = 24 * 60 * 60 * 1000;

            endDate = Endmonth + "/" + Endday + "/" + Endyear

            var endDate = new Date(endDate);

            var ValidOldData = Math.abs((startDate.getTime() - today.getTime()) / (oneDay));
            var ValidDates = Math.abs((startDate.getTime() - endDate.getTime()) / (oneDay));
            
          
            if (SDate == '') {
                alert("Please enter start date");

                valid = false;
                return valid;
            }
            if (EDate == '') {

                alert("Please enter end date");
                valid = false;
                return valid;
            }
            var dtFromDt = document.getElementById('<%=txtStartDate.ClientID%>').value;
            if (isDate(dtFromDt) == false) {
                document.getElementById('<%=txtStartDate.ClientID%>').focus();
                valid = false;
                return valid;
            }

            var dtTodt = document.getElementById('<%=txtEndDate.ClientID%>').value;
            if (isDate(dtTodt) == false) {
                document.getElementById('<%=txtEndDate.ClientID%>').focus();
                valid = false;
                return valid;
            }                   
            
            if (SDate != '' && EDate != '' && startDate > endDate) {
                alert("Start date is greater than end date");
                valid = false;
                return valid;
            }
            if (startDate > today) {
                alert("Start date should not be greater Than current date");
                valid = false;
                return valid;
            }
            if (endDate > today) {

                alert("End date should not be greater than current date");
                valid = false;
                return valid;
            }
            if (ValidOldData >= 365) {
                alert("Report can be generated for maximum of one year prior. Please change the dates and resubmit again");
                document.getElementById("<%=txtStartDate.ClientID%>").focus();
                valid = false;
                return valid;
            }
            return valid;
        }


        var dtCh = "/";
        var Chktoday = new Date();
        var minYear = Chktoday.getFullYear() - 1;
        var maxYear = Chktoday.getFullYear();

        function isInteger(s) {
            var i;
            for (i = 0; i < s.length; i++) {
                // Check that current character is number.
                var c = s.charAt(i);
                if (((c < "0") || (c > "9"))) return false;
            }
            // All characters are numbers.
            return true;
        }

        function stripCharsInBag(s, bag) {
            var i;
            var returnString = "";
            // Search through string's characters one by one.
            // If character is not in bag, append to returnString.
            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);
                if (bag.indexOf(c) == -1) returnString += c;
            }
            return returnString;
        }

        function daysInFebruary(year) {
            // February has 29 days in any year evenly divisible by four,
            // EXCEPT for centurial years which are not also divisible by 400.
            return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
        }
        function DaysArray(n) {
            for (var i = 1; i <= n; i++) {
                this[i] = 31
                if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                if (i == 2) { this[i] = 29 }
            }
            return this
        }

        function isDate(dtStr) {
            var daysInMonth = DaysArray(12)
            var pos1 = dtStr.indexOf(dtCh)
            var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
            var strMonth = dtStr.substring(0, pos1)
            var strDay = dtStr.substring(pos1 + 1, pos2)
            var strYear = dtStr.substring(pos2 + 1)
            strYr = strYear
            if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
            if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
            for (var i = 1; i <= 3; i++) {
                if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
            }
            month = parseInt(strMonth)
            day = parseInt(strDay)
            year = parseInt(strYr)
            if (pos1 == -1 || pos2 == -1) {
                alert("The date format should be : mm/dd/yyyy")
                return false
            }
            if (strMonth.length < 1 || month < 1 || month > 12) {
                alert("Please enter a valid month")
                return false
            }
            if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                alert("Please enter a valid day")
                return false
            }

            if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                //alert("Enter only these years "+minYear+" "+maxYear+" to get data");		
                alert("Report can be generated for maximum of one year prior. Please change the dates and resubmit again");
                return false
            }
            if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                alert("Please enter a valid date")
                return false
            }
            return true
        }
    
    </script>

    <script type="text/javascript">
 function pageLoad()
   { 
      //InitializeTimer();      
   }
   
    var ssTime,TimerID;
   function  InitializeTimer()
   {  
     WebService.sessionGet(onsuccessGet,onError);      
   }
     function onsuccessGet(result)
     {
      ssTime=result; 
      ssTime=parseInt(ssTime)*60000;
     
      TimerDec(ssTime);
     }
   
  
   
   function  TimerDec(ssTime)
   {
   
     ssTime=ssTime-1000;
   
    TimerID=setTimeout(function(){TimerDec(ssTime);},1000);
      
    if(ssTime==60000)
    {      
     SessionInc();     
    }
     
   }
  
    
    function SessionInc()//Increase the session time
    {
     debugger    
      ssTime=parseInt("<%= Session.Timeout %>");     
      WebService.sessionSet(ssTime,onsuccessInc,onError);//call webservice to set the session variable
       ssTime=(parseInt(ssTime)-2)*60000;       
       TimerDec(ssTime);     
    }
    
    function onsuccessInc(result)
    {
     
    }    

     function onError(exception, userContext, methodName)
     {
       try 
       {
        //window.location.href='error.aspx';
        strMessage = strMessage + 'ErrorType: ' + exception._exceptionType + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
        strMessage = strMessage + 'Message: ' + exception._message + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
        strMessage = strMessage + 'Stack Trace: ' + exception._stackTrace + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
        strMessage = strMessage + 'Status Code: ' + exception._statusCode + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
        strMessage = strMessage + 'Timed Out: ' + exception._timedOut;
        ///alert(strMessage);
      } catch (ex) {}
     return false;
   }



    </script>

<!-- Drop Menu CSS -->
    <link rel="stylesheet" href="css/dropdown-menu.css" />
   <link rel="stylesheet" href="css/dropdown-menu-skin.css" />
<!-- Drop Menu JS -->
    <script type="text/javascript" src="js/dropdown-menu.min.js"></script>
<script type="text/javascript">
$(function() {
    $('#example3').dropdown_menu();
});
</script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/WebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updtpnltblGrdcar"
        DisplayAfter="0">
        <ProgressTemplate>
            <div id="spinner">
                <h4>
                    <div>
                        Processing
                        <img src="images/loading.gif" />
                    </div>
                </h4>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="headder">
        <table style="width: 100%">
            <td style="width: 280px;">
                <a>
                    <img src="images/logo2.png" /></a>
            </td>
            <td>
                <h1 style="border-bottom: none; padding-top: 5px;">
                    UNITED CAR EXCHANGE <span>All Centers Report</span></h1>
            </td>
            <td style="width: 470px; padding-top: 10px;">
                <div class="loginStat">
                    Welcome &nbsp;<asp:Label ID="lblUserName" runat="server" Visible="false"></asp:Label>
                    <br />
                    <ul id="example3" class="dropdown-menu " data-options='{"sub_indicators":"true","drop_shadows":"true"}' >
                        <li style="background:none"><a href="#">Menu</a>
                            <ul>
                              <li>
                                    <asp:LinkButton ID="lnkTicker" runat="server" Text="Sales Ticker"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnIPAddress" runat="server" Text="IP Address" PostBackUrl="~/IPAddress.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnQCReport" runat="server" Text="QC Report" PostBackUrl="~/QCReport.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                <asp:LinkButton ID="lnkbtnBulkReport" runat="server" Text="Bulk Process" PostBackUrl="~/BulkProcess.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAddCenters" runat="server" Text="Centers Mgmt" PostBackUrl="~/AddNewCenters.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAllusersmgmnt" runat="server" Text="User Mgmt" PostBackUrl="~/AllUsersManagement.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnSalesreport" runat="server" Text="Sales Report" PostBackUrl="~/CarSalesReportNew.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnLeadsAssign" runat="server" Text="Leads Assign" PostBackUrl="~/LeadAssign.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnLeadsDownLoad" runat="server" Text="Leads Download" PostBackUrl="~/LeadDownLoad.aspx#"></asp:LinkButton>
                                </li>
                                <li>
                                    <a href="#">Dialy Sales</a>
                                    <ul>
                                         <li>
                                    <asp:LinkButton ID="lnkbtnDialySales" runat="server" Text="Daily Sales Count" PostBackUrl="~/DialySalesCount.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="linkButtonDetails" runat="server" Text="Daily Sales Details" PostBackUrl="~/DialySalesDeatails.aspx"></asp:LinkButton>
                                </li>
                                  <li>
                                    <asp:LinkButton ID="lnkbtnsalesbyweekly" runat="server" Text="Weekly Sales By Package" PostBackUrl="~/WeeklySalesByPacakage.aspx"></asp:LinkButton>
                                </li>
                                 <li>
                                    <asp:LinkButton ID="lnkbtnsalesbyweelday" runat="server" Text="Weekly Sales By Day" PostBackUrl="~/WeeklySalesByDay.aspx"></asp:LinkButton>
                                </li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkBtnLogout" runat="server" Text="Logout" Visible="false" OnClick="lnkBtnLogout_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </td>
        </table>
        <div class="clear">
            &nbsp;
        </div>
    </div>
    <div style="height: 10px;">
    </div>
    <div class="main" style="width: 90%; min-width: 1100px; margin: 0 10px 10px 10px">
        <asp:UpdatePanel ID="updtpnltblGrdcar" runat="server">
            <ContentTemplate>
                <table style="width: 100%; display: block;" id="tblGrdcar" runat="server">
                    <tr>
                        <td>
                            <table style="float: left; width: 533px; border-collapse: initial; padding: 10px 0 15px 0;
                                border: #ccc 1px solid; margin-right: 15px; height: 70px;">
                                <tr>
                                    <td style="width: 270px; padding-top: 2px;">
                                        <table style="width: 270px; float: left; border-collapse: collapse; margin-left: 0px;
                                            margin-right: 13px;">
                                            <tr>
                                                <td colspan="3" align="left" style="padding: 0">
                                                    <div style="border-bottom: 1px #666 solid; text-align: center; width: 240px; margin: 0 auto 2px auto;
                                                        font-weight: bold; padding-bottom: 2px;">
                                                        Date range</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45%; padding: 0; text-align: right">
                                                    <asp:TextBox ID="txtStartDate" runat="server" class="input1 " MaxLength="10" onkeypress="return isNumberKeyForDt(event)"
                                                        Width="70px"></asp:TextBox>&nbsp;
                                                    <img id="imgcal" runat="server" style="border-right: 0px; border-top: 0px; border-left: 0px;
                                                        border-bottom: 0px" title="Calendar Control" onclick="displayCalendar(document.forms[0].txtStartDate,'mm/dd/yyyy',this);"
                                                        alt="Calendar Control" src="images/Calender.gif" width="18" />
                                                </td>
                                                <td style="width: 26px; text-align: center; padding: 0;">
                                                    <b>to</b>
                                                </td>
                                                <td style="text-align: left; padding: 0;">
                                                    <asp:TextBox ID="txtEndDate" runat="server" class="input1 " MaxLength="10" onkeypress="return isNumberKeyForDt(event)"
                                                        Width="70px"></asp:TextBox>&nbsp;
                                                    <img id="img1" runat="server" style="border-right: 0px; border-top: 0px; border-left: 0px;
                                                        border-bottom: 0px" title="Calendar Control" onclick="displayCalendar(document.forms[0].txtEndDate,'mm/dd/yyyy',this);"
                                                        alt="Calendar Control" src="images/Calender.gif" width="18" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <strong>Center:</strong>
                                        <br />
                                        <asp:DropDownList ID="ddlCenters" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 105px; padding-top: 28px;">
                                        <div style="float: left; width: 100px;">
                                            <asp:UpdatePanel ID="updbtnSearch" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSearchMonth" runat="server" CssClass="g-button g-button-submit"
                                                        Text="Generate" OnClientClick="return ValidateData();" OnClick="btnSearchMonth_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updbtnSearch"
                                                DisplayAfter="0">
                                                <ProgressTemplate>
                                                    <div id="spinner">
                                                        <h4>
                                                            <div>
                                                                Processing
                                                                <img src="images/loading.gif" />
                                                            </div>
                                                        </h4>
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="border-collapse: collapse; float: right;
                                border-collapse: collapse; float: right; border-collapse: initial; padding: 10px;
                                border: #ccc 1px solid; height: 94px;" class="table12">
                                <tr>
                                    <td style="width: 140px;">
                                        <b>Total Sales:</b>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:Label ID="lblTotSales" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 140px;">
                                        <b>Total Verifications:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotVerif" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Total Abandon/draft(s):</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotAbandon" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <b>Total Transfers Out:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotTransfers" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="padding: 10px; border-collapse: initial; border: #ccc 1px solid; float:left">
                                <tr>
                                    <td style="width: 400px;">
                                        <asp:RadioButton ID="rdbtnSales" Text="Sales" GroupName="Option" runat="server" Checked="true"
                                            OnCheckedChanged="rdbtnSales_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rdbtnVerifications" Text="Verifications" GroupName="Option"
                                            runat="server" OnCheckedChanged="rdbtnVerifications_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rdbtnAbandon" Text="Abandons/Drafts" GroupName="Option" runat="server"
                                            OnCheckedChanged="rdbtnAbandon_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rdbtnTransfers" Text="Transfers" GroupName="Option" runat="server"
                                            OnCheckedChanged="rdbtnTransfers_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                    
                                </tr>
                            </table>
                            
                            <table style="padding: 10px; border-collapse: initial; border: #ccc 1px solid;; float:left; margin-left:15px; width:270px;">
                                <tr>
                                    <td style="vertical-align:middle; padding:3px 0">
                                      Payment: <asp:DropDownList ID="ddlpaymen" runat="server">
                                      <asp:ListItem Value="0">All</asp:ListItem>
                                      <asp:ListItem Value="1">Pening</asp:ListItem>
                                       </asp:DropDownList>
                                       &nbsp;
                                       <asp:Button ID="PaymentChange" runat="server" Text="Change" OnClick="PaymentChange_click" CssClass="g-button g-button-submit" />
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="100%">
                                                <asp:Label ID="lblResHead" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; display: block;" id="tblSales" runat="server">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 35%;">
                                        <h2 style="margin: 0; padding: 0;">
                                            Sale(s)</h2>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdtpnldatResCounts" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblRes" Font-Size="12px" Font-Bold="true" ForeColor="Black" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" style="width: 35%;">
                                        <asp:Label ID="lblResCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div style="width: 100%;" id="divCarresults" runat="server">
                                <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                    <asp:UpdatePanel ID="UpdtpnlHeader" runat="server">
                                        <ContentTemplate>
                                            <table class="grid1 " cellpadding="0" cellspacing="0" style="position: absolute;
                                                top: 2px; width: 1408px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed">
                                                    <td width="60" align="left">
                                                        <asp:LinkButton ID="lnkCarIDSort" runat="server" Text="Sale ID &darr; &uarr;" OnClick="lnkCarIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkSaleDateSort" runat="server" Text="Sale Dt &#8657" OnClick="lnkSaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="150" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkAgentSort" runat="server" Text="Agent &darr; &uarr;" OnClick="lnkAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="150" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkVerifierSort" runat="server" Text="Verifier &darr; &uarr;"
                                                            OnClick="lnkVerifierSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <%--Year--%>
                                                        <asp:LinkButton ID="lnkYearSort" runat="server" Text="Year/Make/Model &darr; &uarr;"
                                                            OnClick="lnkYearSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkPackageSort" runat="server" Text="Package &darr; &uarr;" OnClick="lnkPackageSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnQCStatus" runat="server" Text="QC Status &darr; &uarr;"
                                                            OnClick="lnkbtnQCStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="70" align="right" style="text-align: right">
                                                        <asp:LinkButton ID="lnkbtnPaid" runat="server" Text="$ Paid" Enabled="false"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="right" style="text-align: right; padding-right: 10px;">
                                                        <asp:LinkButton ID="lnkbtnPending" runat="server" Text="$ Pending" Enabled="false"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnPmntStatus" runat="server" Text="Pmnt Status &darr; &uarr;"
                                                            OnClick="lnkbtnPmntStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkNameSort" runat="server" Text="Cust Name &darr; &uarr;" OnClick="lnkNameSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="90" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkPhoneSort" runat="server" Text="Phone &darr; &uarr;" OnClick="lnkPhoneSort_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="width: 1430px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 420px">
                                    <asp:Panel ID="pnl1" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdPnlGrid" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="txthdnSortOrder" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="txthdnSortColumnId" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1408px" ID="grdWarmLeadInfo" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                    ShowHeader="false" OnRowDataBound="grdWarmLeadInfo_RowDataBound">
                                                    <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle CssClass="headder" />
                                                    <PagerSettings Position="Top" />
                                                    <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                    <RowStyle CssClass="row1" />
                                                    <AlternatingRowStyle CssClass="row2" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkCarID" runat="server" Text='<%# Eval("carid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSaleDt" runat="server" Text='<%# Bind("SaleDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgent" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnAgentID" runat="server" Value='<%# Eval("SaleAgentID")%>' />
                                                                <asp:HiddenField ID="hdnAgentName" runat="server" Value='<%# Eval("SaleAgent")%>' />
                                                                <asp:HiddenField ID="hdnAgentCenterCode" runat="server" Value='<%# Eval("AgentCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifier" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifierID" runat="server" Value='<%# Eval("SaleVerifierID")%>' />
                                                                <asp:HiddenField ID="hdnVerifierName" runat="server" Value='<%# Eval("VerifierName")%>' />
                                                                <asp:HiddenField ID="hdnVerifierCenterCode" runat="server" Value='<%# Eval("VerifierCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblYear" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnYear" runat="server" Value='<%# Eval("yearOfMake")%>' />
                                                                <asp:HiddenField ID="hdnMake" runat="server" Value='<%# Eval("make")%>' />
                                                                <asp:HiddenField ID="hdnModel" runat="server" Value='<%# Eval("model")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPackage" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPackName" runat="server" Value='<%# Eval("PackageCode")%>' />
                                                                <asp:HiddenField ID="hdnPackDiscount" runat="server" Value='<%# Eval("UCS_Discountid")%>' />
                                                                <asp:HiddenField ID="hdnPackCost" runat="server" Value='<%# Eval("Price")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQcStatus" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnQCStatusID" runat="server" Value='<%# Eval("QCStatusID")%>' />
                                                                <asp:HiddenField ID="hdnQcStatus" runat="server" Value='<%# Eval("QCStatusName")%>' />
                                                                <asp:HiddenField ID="hdnQCNotes" runat="server" Value='<%# Eval("QCNotes")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPSID1Status" runat="server" Value='<%# Eval("PSStatusID1")%>' />
                                                                <asp:HiddenField ID="hdnPSID2Status" runat="server" Value='<%# Eval("PSStatusID2")%>' />
                                                                <asp:HiddenField ID="hdnPSIDNotes" runat="server" Value='<%# Eval("PaymentNotes")%>' />
                                                                <asp:HiddenField ID="hdnAmount1" runat="server" Value='<%# Eval("Amount1")%>' />
                                                                <asp:HiddenField ID="hdnAmount2" runat="server" Value='<%# Eval("Amount2")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPending" runat="server" Style="padding-right: 10px;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPmntStatus" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPmntStatus" runat="server" Value='<%# Eval("PSStatusName1")%>' />
                                                                <asp:HiddenField ID="hdnPmntReason" runat="server" Value='<%# Eval("PaymentCancelReasonName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnSellerName" runat="server" Value='<%# Eval("sellerName")%>' />
                                                                <asp:HiddenField ID="hdnLastName" runat="server" Value='<%# Eval("LastName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPhoneNum" runat="server" Value='<%# Eval("PhoneNum")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdWarmLeadInfo" EventName="Sorting" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                                <div class="clear" style="height: 12px;">
                                    &nbsp;</div>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; display: block;" id="tblVerifies" runat="server">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 25%;">
                                        <h2 style="margin: 0; padding: 0;">
                                            Verification(s)</h2>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblVerifyRes" Font-Size="12px" Font-Bold="true" ForeColor="Black"
                                                    runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" style="width: 25%;">
                                        <asp:Label ID="lblVerifyResCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div style="width: 100%;" id="div3" runat="server">
                                <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <table class="grid1 " cellpadding="0" cellspacing="0" style="position: absolute;
                                                top: 2px; width: 1408px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed">
                                                    <td width="60" align="left">
                                                        <asp:LinkButton ID="lnkVerifyCarIDSort" runat="server" Text="Sale ID &darr; &uarr;"
                                                            OnClick="lnkVerifyCarIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkVerifySaleDateSort" runat="server" Text="Sale Dt &#8657" OnClick="lnkVerifySaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="150" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkVerifyAgentSort" runat="server" Text="Agent &darr; &uarr;"
                                                            OnClick="lnkVerifyAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="150" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkVerifyVerifierSort" runat="server" Text="Verifier &darr; &uarr;"
                                                            OnClick="lnkVerifyVerifierSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <%--Year--%>
                                                        <asp:LinkButton ID="lnkVerifyYearSort" runat="server" Text="Year/Make/Model &darr; &uarr;"
                                                            OnClick="lnkVerifyYearSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkVerifyPackageSort" runat="server" Text="Package &darr; &uarr;"
                                                            OnClick="lnkVerifyPackageSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnVerifyQCStatus" runat="server" Text="QC Status &darr; &uarr;"
                                                            OnClick="lnkbtnVerifyQCStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="70" align="right" style="text-align: right">
                                                        <asp:LinkButton ID="lnkbtnVerifyPaid" runat="server" Text="$ Paid" Enabled="false"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="right" style="text-align: right; padding-right: 10px;">
                                                        <asp:LinkButton ID="lnkbtnVerifyPending" runat="server" Text="$ Pending" Enabled="false"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnVerifyPmntStatus" runat="server" Text="Pmnt Status &darr; &uarr;"
                                                            OnClick="lnkbtnVerifyPmntStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkVerifyNameSort" runat="server" Text="Cust Name &darr; &uarr;"
                                                            OnClick="lnkVerifyNameSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="90" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkVerifyPhoneSort" runat="server" Text="Phone &darr; &uarr;"
                                                            OnClick="lnkVerifyPhoneSort_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="width: 1430px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 420px">
                                    <asp:Panel ID="Panel3" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="Hidden5" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="Hidden6" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1408px" ID="grdVerifierData" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                    ShowHeader="false" OnRowDataBound="grdVerifierData_RowDataBound">
                                                    <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle CssClass="headder" />
                                                    <PagerSettings Position="Top" />
                                                    <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                    <RowStyle CssClass="row1" />
                                                    <AlternatingRowStyle CssClass="row2" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkVerifyCarID" runat="server" Text='<%# Eval("carid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifySaleDt" runat="server" Text='<%# Bind("SaleDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyAgent" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyAgentID" runat="server" Value='<%# Eval("SaleAgentID")%>' />
                                                                <asp:HiddenField ID="hdnVerifyAgentName" runat="server" Value='<%# Eval("SaleAgent")%>' />
                                                                <asp:HiddenField ID="hdnVerifyAgentCenterCode" runat="server" Value='<%# Eval("SaleAgentCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyVerifer" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyVeriferID" runat="server" Value='<%# Eval("SaleVerifierID")%>' />
                                                                <asp:HiddenField ID="hdnVerifyVeriferName" runat="server" Value='<%# Eval("SaleVerifierName")%>' />
                                                                <asp:HiddenField ID="hdnVerifyVerifierCenterCode" runat="server" Value='<%# Eval("SaleVeriferCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyYear" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyYear" runat="server" Value='<%# Eval("yearOfMake")%>' />
                                                                <asp:HiddenField ID="hdnVerifyMake" runat="server" Value='<%# Eval("make")%>' />
                                                                <asp:HiddenField ID="hdnVerifyModel" runat="server" Value='<%# Eval("model")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyPackage" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyPackName" runat="server" Value='<%# Eval("PackageCode")%>' />
                                                                <asp:HiddenField ID="hdnVerifyPackCost" runat="server" Value='<%# Eval("Price")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyQcStatus" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyQCStatusID" runat="server" Value='<%# Eval("QCStatusID")%>' />
                                                                <asp:HiddenField ID="hdnVerifyQcStatus" runat="server" Value='<%# Eval("QCStatusName")%>' />
                                                                <asp:HiddenField ID="hdnVerifyQCNotes" runat="server" Value='<%# Eval("QCNotes")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyPaid" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyPSID1Status" runat="server" Value='<%# Eval("PSStatusID1")%>' />
                                                                <asp:HiddenField ID="hdnVerifyPSID2Status" runat="server" Value='<%# Eval("PSStatusID2")%>' />
                                                                <asp:HiddenField ID="hdnVerifyPSIDNotes" runat="server" Value='<%# Eval("PaymentNotes")%>' />
                                                                <asp:HiddenField ID="hdnVerifyAmount1" runat="server" Value='<%# Eval("Amount1")%>' />
                                                                <asp:HiddenField ID="hdnVerifyAmount2" runat="server" Value='<%# Eval("Amount2")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyPending" runat="server" Style="padding-right: 10px;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyPmntStatus" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyPmntStatus" runat="server" Value='<%# Eval("PSStatusName1")%>' />
                                                                <asp:HiddenField ID="hdnVerifyPmntReason" runat="server" Value='<%# Eval("PaymentCancelReasonName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyName" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifySellerName" runat="server" Value='<%# Eval("sellerName")%>' />
                                                                <asp:HiddenField ID="hdnVerifyLastName" runat="server" Value='<%# Eval("LastName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVerifyPhone" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnVerifyPhoneNum" runat="server" Value='<%# Eval("PhoneNum")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdVerifierData" EventName="Sorting" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                                <div class="clear" style="height: 12px;">
                                    &nbsp;</div>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; display: block;" id="tblAbandon" runat="server">
                    <tr>
                        <td align="right">
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 30%;">
                                        <h2 style="margin: 0; padding: 0;">
                                            Abandon/Draft(s)</h2>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblAbandonRes" Font-Size="12px" Font-Bold="true" ForeColor="Black"
                                                    runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="Center" style="width: 30%;">
                                        <asp:Label ID="lblAbandonResCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div style="width: 100%;" id="div1" runat="server">
                                <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table class="grid1 " cellpadding="0" cellspacing="0" style="position: absolute;
                                                top: 2px; width: 1228px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed">
                                                    <td width="60" align="left">
                                                        <asp:LinkButton ID="lnkAbandonCarIDSort" runat="server" Text="Sale ID &darr; &uarr;"
                                                            OnClick="lnkAbandonCarIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110px" align="left">
                                                        <asp:LinkButton ID="lnkAbandonSaleDateSort" runat="server" Text="Sale Dt &#8657"
                                                            OnClick="lnkAbandonSaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkAbandonCenterCode" runat="server" Text="Cen Code &darr; &uarr;"
                                                            OnClick="lnkAbandonCenterCode_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkAbandonAgentSort" runat="server" Text="Agent &darr; &uarr;"
                                                            OnClick="lnkAbandonAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <%--Year--%>
                                                        <asp:LinkButton ID="lnkAbandonYearSort" runat="server" Text="Year/Make/Model &darr; &uarr;"
                                                            OnClick="lnkAbandonYearSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkAbandonPackageSort" runat="server" Text="Package &darr; &uarr;"
                                                            OnClick="lnkAbandonPackageSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="130px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkAbandonNameSort" runat="server" Text="Cust Name &darr; &uarr;"
                                                            OnClick="lnkAbandonNameSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="90" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkAbandonPhoneSort" runat="server" Text="Phone &darr; &uarr;"
                                                            OnClick="lnkAbandonPhoneSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="130" align="left">
                                                        <%--Email--%>
                                                        <asp:LinkButton ID="lnkAbandonEmailSort" runat="server" Text="Email &darr; &uarr;"
                                                            OnClick="lnkAbandonEmailSort_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="width: 1250px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 420px">
                                    <asp:Panel ID="Panel1" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="Hidden1" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="Hidden2" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1228px" ID="grdAbandInfo" runat="server" CellSpacing="0" CellPadding="0"
                                                    CssClass="grid1" AutoGenerateColumns="False" GridLines="None" ShowHeader="false"
                                                    OnRowDataBound="grdAbandInfo_RowDataBound">
                                                    <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle CssClass="headder" />
                                                    <PagerSettings Position="Top" />
                                                    <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                    <RowStyle CssClass="row1" />
                                                    <AlternatingRowStyle CssClass="row2" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkAbandonCarID" runat="server" Text='<%# Eval("carid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAbandonSaleDt" runat="server" Text='<%# Bind("SaleDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkAbandonCenterCode" runat="server" Text='<%# Eval("AgentCenterCode")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAbandonAgent" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnAbandonAgentID" runat="server" Value='<%# Eval("SaleAgentID")%>' />
                                                                <asp:HiddenField ID="hdnAbandonAgentName" runat="server" Value='<%# Eval("SaleAgent")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAbandonYear" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnAbandonYear" runat="server" Value='<%# Eval("yearOfMake")%>' />
                                                                <asp:HiddenField ID="hdnAbandonMake" runat="server" Value='<%# Eval("make")%>' />
                                                                <asp:HiddenField ID="hdnAbandonModel" runat="server" Value='<%# Eval("model")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAbandonPackage" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnAbandonPackName" runat="server" Value='<%# Eval("PackageCode")%>' />
                                                                <asp:HiddenField ID="hdnAbandonPackCost" runat="server" Value='<%# Eval("Price")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAbandonName" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnAbandonSellerName" runat="server" Value='<%# Eval("sellerName")%>' />
                                                                <asp:HiddenField ID="hdnAbandonLastName" runat="server" Value='<%# Eval("LastName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="130px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAbandonPhone" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnAbandonPhoneNum" runat="server" Value='<%# Eval("PhoneNum")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAbandonEmail" runat="server" Text='<%# objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"email"),15)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="130px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdAbandInfo" EventName="Sorting" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                                <div class="clear" style="height: 12px;">
                                    &nbsp;</div>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; display: none;" id="tblTransfersIN" runat="server">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 25%;">
                                        <h2 style="margin: 0; padding: 0;">
                                            Transfer(s) Out</h2>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTransferRes" Font-Size="12px" Font-Bold="true" ForeColor="Black"
                                                    runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" style="width: 25%;">
                                        <asp:Label ID="lblTranferResCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div style="width: 100%;" id="div2" runat="server">
                                <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <table class="grid1 " cellpadding="0" cellspacing="0" style="position: absolute;
                                                top: 2px; width: 1328px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed">
                                                    <td width="60" align="left">
                                                        <asp:LinkButton ID="lnkbtnTransSaleID" runat="server" Text="Sale ID &darr; &uarr;"
                                                            OnClick="lnkbtnTransSaleID_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkbtnTransSaleDt" runat="server" Text="Trans Dt &#8657" OnClick="lnkbtnTransSaleDt_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="160" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnTransAgent" runat="server" Text="Agent &darr; &uarr;" OnClick="lnkbtnTransAgent_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="160" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnTransVerifier" runat="server" Text="Verifier &darr; &uarr;"
                                                            OnClick="lnkbtnTransVerifier_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnTransStatus" runat="server" Text="Status &darr; &uarr;"
                                                            OnClick="lnkbtnTransStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <%--Year--%>
                                                        <asp:LinkButton ID="lnkbtnTransYear" runat="server" Text="Year/Make/Model &darr; &uarr;"
                                                            OnClick="lnkbtnTransYear_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkbtnTransPackage" runat="server" Text="Package &darr; &uarr;"
                                                            OnClick="lnkbtnTransPackage_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkbtnTransName" runat="server" Text="Cust Name &darr; &uarr;"
                                                            OnClick="lnkbtnTransName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="90" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkbtnTransPhone" runat="server" Text="Phone &darr; &uarr;" OnClick="lnkbtnTransPhone_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="130" align="left">
                                                        <%--Email--%>
                                                        <asp:LinkButton ID="lnkbtnTransEmail" runat="server" Text="Email &darr; &uarr;" OnClick="lnkbtnTransEmail_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="width: 1350px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 420px">
                                    <asp:Panel ID="Panel2" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="Hidden3" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="Hidden4" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1328px" ID="grdTransfersIn" runat="server" CellSpacing="0" CellPadding="0"
                                                    CssClass="grid1" AutoGenerateColumns="False" GridLines="None" ShowHeader="false"
                                                    OnRowDataBound="grdTransfersIn_RowDataBound">
                                                    <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle CssClass="headder" />
                                                    <PagerSettings Position="Top" />
                                                    <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                    <RowStyle CssClass="row1" />
                                                    <AlternatingRowStyle CssClass="row2" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransSaleID" runat="server" Text='<%# Eval("carid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransSaleDt" runat="server" Text='<%# Bind("TransferDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransAgent" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnTransAgentID" runat="server" Value='<%# Eval("SaleAgentID")%>' />
                                                                <asp:HiddenField ID="hdnTransAgentName" runat="server" Value='<%# Eval("SaleAgent")%>' />
                                                                <asp:HiddenField ID="hdnTransAgentCenterCode" runat="server" Value='<%# Eval("AgentCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransVerifier" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnTransVerifierID" runat="server" Value='<%# Eval("VerifierID")%>' />
                                                                <asp:HiddenField ID="hdnTransVerifierName" runat="server" Value='<%# Eval("VerifierName")%>' />
                                                                <asp:HiddenField ID="hdnTransVerifierCenterCode" runat="server" Value='<%# Eval("VerifierCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransStatus" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnTransStatusName" runat="server" Value='<%# Eval("LeadStatusName")%>' />
                                                                <asp:HiddenField ID="hdnTransStatusID" runat="server" Value='<%# Eval("LeadStatus")%>' />
                                                                <asp:HiddenField ID="hdnTransDisposID" runat="server" Value='<%# Eval("DispositionID")%>' />
                                                                <asp:HiddenField ID="hdnTransDisposName" runat="server" Value='<%# Eval("DispositionName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransYear" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnTransYear" runat="server" Value='<%# Eval("yearOfMake")%>' />
                                                                <asp:HiddenField ID="hdnTransMake" runat="server" Value='<%# Eval("make")%>' />
                                                                <asp:HiddenField ID="hdnTransModel" runat="server" Value='<%# Eval("model")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransPackage" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnTransPackName" runat="server" Value='<%# Eval("PackageCode")%>' />
                                                                <asp:HiddenField ID="hdnTransPackCost" runat="server" Value='<%# Eval("Price")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransName" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnTransSellerName" runat="server" Value='<%# Eval("sellerName")%>' />
                                                                <asp:HiddenField ID="hdnTransLastName" runat="server" Value='<%# Eval("LastName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransPhone" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnTransPhoneNum" runat="server" Value='<%# Eval("PhoneNum")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransEmail" runat="server" Text='<%# objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"email"),15)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="130px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdTransfersIn" EventName="Sorting" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                                <div class="clear" style="height: 12px;">
                                    &nbsp;</div>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
