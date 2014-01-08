<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCReport.aspx.cs" Inherits="QCReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>..:: Car Sales System ::..</title>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <link href="css/css.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript" src="js/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="js/emulatetab.joelpurra.js"></script>

    <script type="text/javascript" src="js/plusastab.joelpurra.js"></script>

    <link href="css/css.css" rel="stylesheet" type="text/css" />

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
          function ValidateUpdate() {
         $('#UpdateProgress1').show();
            var valid = true;
            if((document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="1")||(document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="7")||(document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="8"))
            {
                if(document.getElementById('<%=hdnPophdnAmount.ClientID%>').value != "0")
                {
                    if(document.getElementById('<%= ddlPaymentDate.ClientID%>').value == "0") {
                        document.getElementById('<%= ddlPaymentDate.ClientID%>').focus();
                        alert("Select payment date");                 
                        document.getElementById('<%=ddlPaymentDate.ClientID%>').focus()
                        valid = false; 
                         $('#UpdateProgress1').hide();           
                         return valid;     
                    }  
                    if (document.getElementById('<%= txtPaymentAmountInPop.ClientID%>').value.trim().length < 1)
                    {
                        alert("Please enter amount paid"); 
                        valid=false;
                         $('#UpdateProgress1').hide();       
                        document.getElementById('txtPaymentAmountInPop').focus();  
                        return valid;               
                    }  
                     if (document.getElementById('<%= txtPaymentAmountInPop.ClientID%>').value.trim()!=document.getElementById('<%= hdnPophdnAmount.ClientID%>').value.trim())
                    {
                        alert("Amount entered not match with amount need to be paid now"); 
                        valid=false;
                         $('#UpdateProgress1').hide();       
                        document.getElementById('txtPaymentAmountInPop').focus();  
                        return valid;               
                    }  
                    if (document.getElementById('<%= txtPaytransID.ClientID%>').value.trim().length < 1)
                    {
                        alert("Please enter transaction id"); 
                        valid=false;
                         $('#UpdateProgress1').hide();       
                        document.getElementById('txtPaytransID').focus();  
                        return valid;               
                    }
                }
            } 
            
             if(document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="2")
            {
                    if(document.getElementById('<%= ddlPayCancelReason.ClientID%>').value == "0") {
                        document.getElementById('<%= ddlPayCancelReason.ClientID%>').focus();
                        alert("Select payment reject reason");                 
                        document.getElementById('<%=ddlPayCancelReason.ClientID%>').focus()
                        valid = false;   
                         $('#UpdateProgress1').hide();                
                         return valid;     
                    } 
                    if (document.getElementById('<%= txtPaymentNewNotes.ClientID%>').value.trim().length < 1)
                    {
                        alert("Please enter payment notes"); 
                        valid=false;
                         $('#UpdateProgress1').hide();       
                        document.getElementById('txtPaymentNewNotes').focus();  
                        return valid;               
                    }                                 
            } 
            
           
            
            
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
        function ClosePopup() {
            $find('<%= MPEUpdate.ClientID%>').hide();
            return false;
        }
	     function PayInfoChanges() {
	     debugger;
	        if(document.getElementById('<%=hdnPopPayType.ClientID%>').value =="6")
	        {
	            document.getElementById('<%=divTransID.ClientID%>').style.display = "none";
	            document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "none";
	            document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "none";
	        }
	        else
	        {	        
	            if((document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="1")||(document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="7")||(document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="8"))
	            {
                    document.getElementById('<%=divTransID.ClientID%>').style.display = "block";
	                document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "block";
	                document.getElementById('<%=divReason.ClientID%>').style.display = "none";
	                document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "block";
	                if(document.getElementById('<%=hdnPophdnAmount.ClientID%>').value =="0")
	                {
	                    document.getElementById('<%=divTransID.ClientID%>').style.display = "none";
	                    document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "none";	                    
	                    document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "none";
	                }
                }
                else
                {
                    document.getElementById('<%=divTransID.ClientID%>').style.display = "none";
	                document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "none";
	                document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "none";
	                if(document.getElementById('<%=ddlPaymentStatus.ClientID%>').value =="2")
	                {
	                     document.getElementById('<%=divReason.ClientID%>').style.display = "block";
	                }
	                else
	                {
	                    document.getElementById('<%=divReason.ClientID%>').style.display = "none";
	                }
                }
            }
            return false;
        }
 function isNumberKey(evt)
         {
         debugger
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
         function isNumberKeyWithDot(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;

            return true;
        }


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
            <td style="width: 260px;">
                <a>
                    <img src="images/logo2.png" /></a>
            </td>
            <td>
                <h1 style="border-bottom: none; padding-top: 5px;">
                    UNITED CAR EXCHANGE <span>QC Report</span></h1>
            </td>
            <td style="width: 490px; padding-top: 10px;">
                <div class="loginStat">
                    Welcome &nbsp;<asp:Label ID="lblUserName" runat="server" Visible="false"></asp:Label>
                    <br />
                    <ul class="menu2">
                        <li><span style="font-size: 13px; font-weight: bold; cursor: pointer; color: #FFC50F">
                            Menu &nabla;</span>
                            <ul>
                                <li>
                                    <asp:LinkButton ID="lnkTicker" runat="server" Text="Sales Ticker"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnIPAddress" runat="server" Text="IP Address" PostBackUrl="~/IPAddress.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAllCentersReport" runat="server" Text="Centers report"
                                        PostBackUrl="~/AllCentersReport.aspx" Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnBulkReport" runat="server" Text="Bulk Process" PostBackUrl="~/BulkProcess.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAddCenters" runat="server" Text="Centers Mgmt" PostBackUrl="~/AddNewCenters.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAllusersmgmnt" runat="server" Text="User Mgmt" PostBackUrl="~/AllUsersManagement.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnSalesreport" runat="server" Text="Sales Report" PostBackUrl="~/CarSalesReportNew.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnLeadsAssign" runat="server" Text="Leads Assign" PostBackUrl="~/LeadAssign.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnLeadsDownLoad" runat="server" Text="Leads Download" PostBackUrl="~/LeadDownLoad.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnQCReportFroDealer" runat="server" Text="Dealer QC Report"
                                        PostBackUrl="~/QCReportForDealer.aspx"></asp:LinkButton>
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
    </div>
    <div style="height: 10px;">
    </div>
    <div class="main" style="width: 90%; min-width: 1100px; margin: 0 10px 10px 10px">
        <asp:UpdatePanel ID="updtpnltblGrdcar" runat="server">
            <ContentTemplate>
                <table style="width: 100%; display: block;" id="tblGrdcar" runat="server">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <table style="width: auto; padding: 10px; border-collapse: initial; border: #ccc 1px solid; float:left; height:61px;"
                                        cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding: 0; width: 290px; vertical-align:middle">
                                                <asp:RadioButton ID="rdbtnQCOpen" runat="server" Text="QC Open" GroupName="QCReport"
                                                    CssClass="noLM" Style="margin-left: 0;" />
                                                <asp:RadioButton ID="rdbtnQCDonepayopen" Text="QC Done pay open" runat="server" GroupName="QCReport" />
                                                <asp:RadioButton ID="rdbtnAll" runat="server" Text="All" GroupName="QCReport" Checked="true"
                                                    OnCheckedChanged="rdbtnAll_CheckedChanged" />
                                            </td>
                                            <td style="text-align: left; padding-right: 10px; width: 119px; vertical-align:middle">
                                                Center:
                                                <asp:DropDownList ID="ddlCenters" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="vertical-align:middle" >
                                                <asp:UpdatePanel ID="updbtnSearch" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnGenerate" runat="server" CssClass="g-button g-button-submit" Text="Generate"
                                                            OnClientClick="return ValidateData();" OnClick="btnGenerate_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 410px; padding: 10px; border-collapse: collapse; border: #ccc 1px solid; float:left; height:61px; margin-left:15px;">
                                        <tr>
                                            <td style="vertical-align:middle; padding-left:10px;" >
                                                search:
                                            </td>
                                            <td style="vertical-align:middle">
                                                <asp:DropDownList ID="ddlQCSearch" runat="server">
                                                    <asp:ListItem>SaleID</asp:ListItem>
                                                   <%-- <asp:ListItem>Sale Date</asp:ListItem>--%>
                                                    <asp:ListItem>Phone</asp:ListItem>
                                                   <asp:ListItem>Name</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="vertical-align:middle">
                                                <asp:TextBox ID="txtQCSearch" runat="server"></asp:TextBox>
                                            </td>
                                            <td style="vertical-align:middle; padding-right:10px;">
                                                <asp:Button ID="BtnQCSearch" runat="server" Text="QC Search" CssClass="g-button g-button-submit" OnClick="BtnQCSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 40%;">
                                        <%-- <h2 style="margin: 0; padding: 0;">
                                            Sale(s)</h2>--%>
                                        <asp:Label ID="lblResHead" runat="server"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdtpnldatResCounts" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblRes" Font-Size="12px" Font-Bold="true" ForeColor="Black" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" style="width: 40%;">
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
                                                top: 2px; width: 1518px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed">
                                                    <td width="60" align="left">
                                                        <asp:LinkButton ID="lnkCarIDSort" runat="server" Text="Sale ID &darr; &uarr;" OnClick="lnkCarIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkSaleDateSort" runat="server" Text="Sale Dt &#8657" OnClick="lnkSaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="160" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkAgentSort" runat="server" Text="Agent &darr; &uarr;" OnClick="lnkAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        Voice Record #
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnQCStatus" runat="server" Text="QC Status &darr; &uarr;"
                                                            OnClick="lnkbtnQCStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnPaymentStatus" runat="server" Text="Pmnt Status &darr; &uarr;"
                                                            OnClick="lnkbtnPaymentStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="240" align="left">
                                                        Smartz St
                                                    </td>
                                                    <td align="left">
                                                        <%--Year--%>
                                                        <asp:LinkButton ID="lnkYearSort" runat="server" Text="Year/Make/Model &darr; &uarr;"
                                                            OnClick="lnkYearSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkPackageSort" runat="server" Text="Package &darr; &uarr;" OnClick="lnkPackageSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkNameSort" runat="server" Text="Name &darr; &uarr;" OnClick="lnkNameSort_Click"></asp:LinkButton>
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
                                <div style="width: 1540px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 470px">
                                    <asp:Panel ID="pnl1" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdPnlGrid" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="txthdnSortOrder" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="txthdnSortColumnId" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1518px" ID="grdWarmLeadInfo" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                    ShowHeader="false" OnRowDataBound="grdWarmLeadInfo_RowDataBound" OnRowCommand="grdWarmLeadInfo_RowCommand">
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
                                                                <asp:LinkButton ID="lnkCarID" runat="server" Text='<%# Eval("carid")%>' CommandArgument='<%# Eval("postingID")%>'
                                                                    CommandName="EditSale"></asp:LinkButton>
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
                                                                <asp:HiddenField ID="hdnAgentCenterID" runat="server" Value='<%# Eval("AgentCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVoiceRecord" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"VoiceRecord"),11)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQCStatus" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnQCStatusName" runat="server" Value='<%# Eval("QCStatusName")%>' />
                                                                <asp:HiddenField ID="hdnQCStatusID" runat="server" Value='<%# Eval("QCStatusID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnPaymentStatus" runat="server" CommandArgument='<%# Eval("postingID")%>'
                                                                    CommandName="EditPayInfo"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnPSID1Status" runat="server" Value='<%# Eval("PSStatusID1")%>' />
                                                                <asp:HiddenField ID="hdnPSID1StatusName" runat="server" Value='<%# Eval("PSStatusName1")%>' />
                                                                <asp:HiddenField ID="hdnPSAmount" runat="server" Value='<%# Eval("Amount1") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnMoveSmartz" runat="server" CommandArgument='<%# Eval("postingID")%>'
                                                                    CommandName="MoveSmartz"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnSmartzStatus" runat="server" Value='<%# Eval("SmartzStatus")%>' />
                                                                <asp:HiddenField ID="hdnSmartzCarID" runat="server" Value='<%# Eval("SmartzCarID")%>' />
                                                                <asp:HiddenField ID="hdnSmartzMovedDate" runat="server" Value='<%# Eval("SmartzMovedDate")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="240px" />
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
                <div style="height: 20px;">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField ID="btnOpen" runat="server" />
    <cc1:ModalPopupExtender ID="MPEUpdate" runat="server" PopupControlID="tblUpdate"
        BackgroundCssClass="ModalPopupBG" TargetControlID="btnOpen" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 520px; margin-top: 70px; width: 650px">
            <h4>
                Update Payment Details
            </h4>
            <div class="dat" style="padding: 0 0 0 3; overflow: scroll; height: 480px;">
                <table id="Table2" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 100%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="updPnlUser" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                    <tr>
                                                        <td style="width: 30%;">
                                                            <b>Sale ID</b> &nbsp;
                                                            <asp:Label ID="lblpaymentPopSaleID" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <b>Phone</b> &nbsp;
                                                            <asp:Label ID="lblPayInfoPhone" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b>Email</b> &nbsp;
                                                            <asp:Label ID="lblPayInfoEmail" runat="server"></asp:Label>
                                                            <asp:HiddenField ID="hdnPopPayType" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <b>Voice file confirmation #</b> &nbsp;
                                                            <asp:Label ID="lblPayInfoVoiceConfNo" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 30%">
                                                            <b>Payment date</b> &nbsp;
                                                            <asp:Label ID="lblPoplblPayDate" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <b>Amount</b> &nbsp;
                                                            <asp:Label ID="lblPoplblPayAmount" runat="server"></asp:Label>
                                                            <asp:HiddenField ID="hdnPophdnAmount" runat="server" />
                                                        </td>
                                                        <td>
                                                            <b>Package</b> &nbsp;
                                                            <asp:Label ID="lblPoplblPackage" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="trPopPDData" runat="server">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 30%">
                                                            <b>PD Date</b> &nbsp;
                                                            <asp:Label ID="lblPDDateForPop" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b>Amount</b> &nbsp;
                                                            <asp:Label ID="lblPDAmountForPop" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <!-- Credit Card Start  -->
                                                <div id="divcard" runat="server" style="display: block; height: auto; min-height: auto;
                                                    max-height: auto; margin-bottom: 15px;">
                                                    <!-- 
                                                <table>
                                                    <tr>
                                                        <td style="width:49%;">
                                                            <table>
                                                                
                                                            </table>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                                -->
                                                    <table border="0" cellpadding="4" cellspacing="4" style="width: 99%; margin: 15px 0;
                                                        float: left;">
                                                        <tr>
                                                            <td colspan="5">
                                                                <h5 style="font-size: 15px; margin: 0; float: left; width: 130px;">
                                                                    <b>Card Details</b></h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Card Type</b>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblCCCardType" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Card Holder First Name</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCardHolderName" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width: 50px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <b>Last Name</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCardHolderLastName" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>Credit Card #</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCCNumber" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <b>Expiry Date</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCCExpiryDate" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>CVV#</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCvv" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <b>Address</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBillingAddress" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>City</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBillingCity" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td colspan="2">
                                                                <div style="width: 80px; display: inline-block; float: left; margin-right: 10px;">
                                                                    <b>State &nbsp;</b>
                                                                    <asp:Label ID="lblBillingState" runat="server"></asp:Label>
                                                                </div>
                                                                <div style="width: 120px; display: inline-block; float: left">
                                                                    <b>ZIP &nbsp;</b>
                                                                    <asp:Label ID="lblBillingZip" runat="server"></asp:Label>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <!-- Credit Card End  -->
                                                <div class="clear">
                                                    &nbsp;</div>
                                                <!-- check Start  -->
                                                <div id="divcheck" runat="server" style="display: none; height: auto; min-height: auto;
                                                    max-height: auto">
                                                    <table border="0" cellpadding="4" cellspacing="4" style="width: 99%; margin: 15px 0;
                                                        float: left;">
                                                        <tr>
                                                            <td colspan="5">
                                                                <h5 style="font-size: 15px; margin: 0; float: left; width: 130px;">
                                                                    <b>Check Details</b></h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Account holder name</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAccHolderName" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width: 50px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <b>Bank name</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBankName" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Account type</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAccType" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width: 50px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <b>Account #</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAccNumber" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Routing #</b>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblRouting" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <!-- check End  -->
                                                <div class="clear">
                                                    &nbsp;</div>
                                                <!-- paypal Start  -->
                                                <div id="divpaypal" runat="server" style="display: none; height: auto; min-height: auto;
                                                    max-height: auto;">
                                                    <table border="0" cellpadding="4" cellspacing="4" style="width: 99%; margin: 15px  0;
                                                        float: left;">
                                                        <tr>
                                                            <td colspan="5">
                                                                <h5 style="font-size: 15px; margin: 0; float: left; width: 130px;">
                                                                    <b>Paypal Details</b></h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Payment trans ID</b>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblPaypalTranID" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Paypal account email</b>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblPaypalEmail" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="clear">
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 21%;">
                                                            <b>Payment status</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPaymentStatus" runat="server" onchange="return PayInfoChanges();">
                                                                <asp:ListItem Value="4">Open</asp:ListItem>
                                                                <asp:ListItem Value="1">FullyPaid</asp:ListItem>
                                                                <asp:ListItem Value="7">PartialPaid</asp:ListItem>
                                                                <asp:ListItem Value="8">NoPayDue</asp:ListItem>
                                                                <asp:ListItem Value="3">Pending</asp:ListItem>
                                                                <asp:ListItem Value="5">Returned</asp:ListItem>
                                                                <asp:ListItem Value="2">Reject</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="divTransID" runat="server" style="display: none;">
                                                <!-- Credit Card Start  -->
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 21%;">
                                                            <b>Transaction ID</b>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPaytransID" runat="server" MaxLength="30"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="divPaymentDate" runat="server" style="display: none;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 21%;">
                                                            <b>Payment Date</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPaymentDate" runat="server" CssClass="input1">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="divPaymentAmount" runat="server" style="display: none;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 21%;">
                                                            <b>Amount</b>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPaymentAmountInPop" runat="server" MaxLength="6" onkeypress="return isNumberKeyWithDot(event)"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="divReason" runat="server" style="display: none;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 21%;">
                                                            <b>Reason</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPayCancelReason" runat="server" CssClass="input1">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="vertical-align: top; width: 21%;">
                                                            <b>Old Notes</b> &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPaymentNotes" runat="server" MaxLength="1000" Style="width: 200px;
                                                                height: 45px; resize: none;" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="vertical-align: top; width: 21%;">
                                                            <b>Notes</b> &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPaymentNewNotes" runat="server" MaxLength="1000" Style="width: 200px;
                                                                height: 45px; resize: none;" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                    <!-- paypal End  -->
                                    <div class="clear">
                                        &nbsp;</div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="updtpnlBtns" runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr align="center">
                                            <td colspan="4" style="padding-top: 15px;">
                                                <div style="width: 240px; margin: 0 auto;">
                                                    <asp:Button ID="btnUpdate" OnClientClick="return ValidateUpdate();" runat="server"
                                                        Text="Update" CssClass="g-button g-button-submit" OnClick="btnUpdate_Click" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnCancelUpdate" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Cancel" OnClientClick="return ClosePopup();" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mdepAlertExists" runat="server" PopupControlID="divExists"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnExists" OkControlID="btnExustCls"
        CancelControlID="btnOk">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnExists" runat="server" />
    <div id="divExists" class="alert" style="display: none">
        <h4 id="H2">
            Alert
            <asp:Button ID="btnExustCls" class="cls" runat="server" Text="" BorderWidth="0" />
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorExists" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnOk" class="btn" runat="server" Text="Ok" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mpealteruserUpdated" runat="server" PopupControlID="AlertUserUpdated"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAlertuserUpdated">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAlertuserUpdated" runat="server" />
    <div id="AlertUserUpdated" class="alert" style="display: none">
        <h4 id="H3">
            Alert
            <asp:Button ID="BtnClsUpdated" class="cls" runat="server" Text="" BorderWidth="0"
                OnClick="btnYesUpdated_Click" />
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErrUpdated" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnYesUpdated" class="btn" runat="server" Text="Ok" OnClick="btnYesUpdated_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="MdepAddAnotherCarAlert" runat="server" PopupControlID="divAddAnotherCarAlert"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAddAnotherCarAlert" OkControlID="btnAddAnotherCarNo">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAddAnotherCarAlert" runat="server" />
    <div id="divAddAnotherCarAlert" class="alert" style="display: none">
        <h4 id="H7">
            Alert
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblAddAnotherCarAlertError" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnAddAnotherCarNo" class="btn" runat="server" Text="No" />&nbsp;
            <asp:Button ID="btnAddAnotherCarYes" class="btn" runat="server" Text="Yes" OnClick="btnAddAnotherCarYes_Click" />
        </div>
    </div>
    </form>
</body>
</html>
