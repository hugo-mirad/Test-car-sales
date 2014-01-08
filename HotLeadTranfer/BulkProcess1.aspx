<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BulkProcess1.aspx.cs" Inherits="BulkProcess1" %>

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
    function ClosePopup() {
            $find('<%= mdlCarFeature.ClientID%>').hide();
            return false;
        }
         function ClosePopup1() {
            $find('<%= MPCardescr.ClientID%>').hide();
            return false;
        }
        
         function ClosePopup2() {
            $find('<%= ModelSalesno.ClientID%>').hide();
            return false;
        }
          function ClosePopup3() {
            $find('<%= ModelQCno.ClientID%>').hide();
            return false;
        }
         function ClosePopup4() {
            $find('<%= ModelPaymtno.ClientID%>').hide();
            return false;
        }
         function ClosePopup5() {
            $find('<%= mdlPopAdl.ClientID%>').hide();
            return false;
        }
          function ClosePopup9() {
            $find('<%= mdlPicdata.ClientID%>').hide();
            return false;
        }
        
          function ClosePopup10() {
            $find('<%= Mdlpicdesc.ClientID%>').hide();
            return false;
        }
        
           function ClosePopup15() {
            $find('<%= MdlQcProcessta.ClientID%>').hide();
            return false;
        }
         function ClosePopup16() {
            $find('<%=  modelbulkQCProc.ClientID%>').hide();
            return false;
        }
          function ClosePopup17() {
            $find('<%=  modelbulkQCProc.ClientID%>').hide();
            return false;
        }
           function ClosePopup18() {
            $find('<%=  modelbulkQCProc.ClientID%>').hide();
            return false;
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
                         return valid;     
                    }  
                    if (document.getElementById('<%= txtPaymentAmountInPop.ClientID%>').value.trim().length < 1)
                    {
                        alert("Please enter amount paid"); 
                        valid=false;
                        document.getElementById('txtPaymentAmountInPop').focus();  
                        return valid;               
                    }  
                     if (document.getElementById('<%= txtPaymentAmountInPop.ClientID%>').value.trim()!=document.getElementById('<%= hdnPophdnAmount.ClientID%>').value)
                    {
                        alert("Amount entered not match with amount need to be paid now"); 
                        valid=false;
                        document.getElementById('txtPaymentAmountInPop').focus();  
                        return valid;               
                    }  
                    if (document.getElementById('<%= txtPaytransID.ClientID%>').value.trim().length < 1)
                    {
                        alert("Please enter transaction id"); 
                        valid=false;
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
                         return valid;     
                    } 
                    if (document.getElementById('<%= txtPaymentNewNotes.ClientID%>').value.trim().length < 1)
                    {
                        alert("Please enter payment notes"); 
                        valid=false;
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
            $find('<%= mdlCarFeature.ClientID%>').hide();
            return false;
        }
         function ClosePopup1() {
            $find('<%= MPCardescr.ClientID%>').hide();
            return false;
        }
          function ClosePopup2() {
            $find('<%= ModelSalesno.ClientID%>').hide();
            return false;
        }
          function ClosePopup3() {
            $find('<%= ModelQCno.ClientID%>').hide();
            return false;
        }
          function ClosePopup4() {
            $find('<%= ModelPaymtno.ClientID%>').hide();
            return false;
        }
         function ClosePopup5() {
            $find('<%= mdlPopAdl.ClientID%>').hide();
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
         function poptastic2(url)
      {var carid=$.trim($('#lblSaleID').text());
    var ur = url+carid
	newwindow=window.open(ur,'name','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,height=600,width=540');
	    if (window.focus) {newwindow.focus()}
    }

    </script>
    <script type="text/javascript">
// Select/Deselect checkboxes based on header checkbox
function SelectAll(headerchk) { 
    /*
    var gvcheck = document.getElementById('grdWarmLeadInfo');
    var i;
    //Condition to check header checkbox selected or not if that is true checked all checkboxes
    if (headerchk.checked) {     
        for (i = 0; i < gvcheck.rows.length; i++) {
            var inputs = gvcheck.rows[i].getElementsByTagName('input');
            inputs[0].checked = true;
        }
    }
    //if condition fails uncheck all checkboxes in gridview
    else {
        for (i = 0; i < gvcheck.rows.length; i++) {
            var inputs = gvcheck.rows[i].getElementsByTagName('input');
            inputs[0].checked = false;
        }
    }
    */
}


// CHK on change Change row color
$(function(){

    $('#chkall').live('change', function(){
        var chkString = '';
        var counter1 = 0;
        if ($(this).is(':checked')) {    
             $('#grdWarmLeadInfo tr input[type=checkbox]').each(function(){
                $(this).prop('checked',true);
                 $(this).parent().parent().addClass('act');
                 chkString += $.trim($(this).parent().next().children('a').text())+','; 
                 counter1++;           
             })                            
        } else {   
            $('#grdWarmLeadInfo tr input[type=checkbox]').each(function(){
                $(this).prop('checked',false);
                 $(this).parent().parent().removeClass('act');                
             })                  
            
        } 
       $('#hdncheck').val(chkString);
       if(counter1 > 1 ){
            $('.bulk').show();
       }else{
            $('.bulk').hide();
       }
       
       
        //console.log($('#hdncheck').val())
    });
    
    
    


    $('#grdWarmLeadInfo tr input[type=checkbox]').live('change', function(){
           
                var chkString = '';
                var counter1 = 0;           
                if ($(this).is(':checked')) {                   
                    $(this).parent().parent().addClass('act');                  
                } else {                   
                    $('#chkall').prop('checked',false);
                    $(this).parent().parent().removeClass('act');
                } 
                //$('#hdncheck').val(chkString);
                
                $('#grdWarmLeadInfo input').each(function(){
                    if ($(this).is(':checked')) { 
                        chkString += $.trim($(this).parent().next().children('a').text())+',';
                        $('#hdncheck').val(chkString);
                        counter1++;
                     }
                     
                })
                
                if(counter1 > 1 ){
                    $('.bulk').show();
               }else{
                    $('.bulk').hide();
               }
                
           
           
           
    })
    
   
    
    
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
                    <h4>
                    </h4>
                    <h4>
                    </h4>
                </h4>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="headder">
        <table style="width: 100%">
        <tr>
            <td style="width: 260px;">
                <a>
                    <img src="images/logo2.png" /></a>
            </td>
            <td>
                <h1 style="border-bottom: none; padding-top: 5px;">
                    UNITED CAR EXCHANGE <span>Bulk Process</span></h1>
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
                                    <asp:LinkButton ID="lnkbtnQCReport" runat="server" Text="QC Report" PostBackUrl="~/QCReport.aspx"></asp:LinkButton>
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
            </tr>
        </table>
        <div class="clear">
            &nbsp;
        </div>
    </div>
    <div>
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
                            <table style="width:100%">
                                <tr>
                                    <td style="padding-bottom:20px;">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <table style=" padding: 10px; border-collapse: initial; border: #ccc 1px solid;"
                                        cellpadding="0" cellspacing="0">
                                       
                                        <tr>
                                        <td style="width:110px; padding:0">
                                            Center<br />
                                                <asp:DropDownList ID="ddlCenters" runat="server">
                                                </asp:DropDownList>
                                        </td>
                                            <td style="padding: 0; width: 125px">
                                             
                                                    QC
                                                    <br />
                                                    <asp:DropDownList ID="ddlQCStatus" runat="server" 
                                                        >
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>QCOpen</asp:ListItem>
                                                    <asp:ListItem>QCDonePayOpen</asp:ListItem>
                                                    </asp:DropDownList>
                                                    
                                            </td>
                                            <td style="padding: 0; width: 110px">
                                           Payment<br />
                                           <asp:DropDownList ID="ddlPaymentsta" runat="server" >
                                           <asp:ListItem Value="4">All</asp:ListItem>
                                           <asp:ListItem Value="4">Open</asp:ListItem>
                                                                <asp:ListItem Value="1">FullyPaid</asp:ListItem>
                                                                <asp:ListItem Value="7">PartialPaid</asp:ListItem>
                                                                <asp:ListItem Value="8">NoPayDue</asp:ListItem>
                                                                <asp:ListItem Value="3">Pending</asp:ListItem>
                                                                <asp:ListItem Value="5">Returned</asp:ListItem>
                                                                <asp:ListItem Value="2">Reject</asp:ListItem>
                                           </asp:DropDownList>
                                            </td>
                                            
                                            <td style="padding: 0; width: 300px; display:none;">
                                             Process Type<br />
                                           <asp:DropDownList ID="DropDownList1" runat="server" >
                                           <asp:ListItem>All</asp:ListItem>
                                           <asp:ListItem>QCProcess</asp:ListItem>
                                           <asp:ListItem>PaymentProcess</asp:ListItem>
                                           </asp:DropDownList>
                                            </td>
                                            <td style="padding: 0; width: 134px">
                                            Transfer<br />
                                           <asp:DropDownList ID="ddl_transtst" runat="server" >
                                           <asp:ListItem Value="0">All</asp:ListItem>
                                           <asp:ListItem Value="1">Moved</asp:ListItem>
                                           <asp:ListItem Value="2">Ready to move</asp:ListItem>
                                           <asp:ListItem Value="3">Not ready</asp:ListItem>
                                           </asp:DropDownList>
                                            </td>
                                           
                                            <td style="padding: 0; width: 170px">
                                               SortBy<br />                                              
                                               <asp:DropDownList ID="ddl_sortby" runat="server">
                                               <asp:ListItem>SaleDate</asp:ListItem>
                                               <asp:ListItem>Sale Id</asp:ListItem>
                                               <asp:ListItem>Agent</asp:ListItem>
                                               <asp:ListItem>Verifier</asp:ListItem>
                                               <asp:ListItem>Sale Voice File</asp:ListItem>
                                                <asp:ListItem>Sale Package</asp:ListItem>
                                               <asp:ListItem>No.of Cars</asp:ListItem>
                                               <asp:ListItem>Payment type</asp:ListItem>
                                               <asp:ListItem>Customer phone</asp:ListItem>
                                                <asp:ListItem>Customer first name</asp:ListItem>
                                               <asp:ListItem>Customer last name</asp:ListItem>
                                               <asp:ListItem>Customer city</asp:ListItem>
                                               <asp:ListItem>Package</asp:ListItem>
                                               <asp:ListItem>Make</asp:ListItem>
                                               <asp:ListItem>QC Status</asp:ListItem>
                                               <asp:ListItem>Payment status</asp:ListItem>
                                               <asp:ListItem>Transfer status</asp:ListItem>
                                               <asp:ListItem>Payment process date</asp:ListItem>
                                               <asp:ListItem>QC process date</asp:ListItem>
                                               <asp:ListItem>Transfer date</asp:ListItem>
                                               <asp:ListItem>CarID</asp:ListItem>
                                                
                                               
                                               </asp:DropDownList>
                                               
                                            </td>
                                            
                                             <td>
                                                
                                               <asp:Button id="rfsh" runat="server" Text="Refresh" CssClass="g-button g-button-submit"   OnClientClick="return ValidateData();" style=" margin-top:14px; " onclick="rfsh_Click"/>
                                               
                                               
                                            </td>
                                          
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                                    <td style=" text-align:right " >
                                            <b>
                                            <span>Today's Sales:</span>
                                            <asp:Label ID="Lbl_dialstat" runat="server" Text="tblStat"></asp:Label>
                                            </b>
                                            </td>
                                </tr>
                            </table>
                        </td>
                        
                        
                    </tr>
                    <tr style="padding:0;" >
                        <td style="padding:0">
                            <table style="padding:0; width:100%;">
                                <tr>
                                    <td align="left" style="width: 20%; padding:0">
                                        <%-- <h2 style="margin: 0; padding: 0;">
                                            Sale(s)</h2>--%>
                                        <asp:Label ID="lblResHead" runat="server" ></asp:Label>
                                    </td>
                                    <td align="left" style="padding:0">
                                        <asp:UpdatePanel ID="UpdtpnldatResCounts" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblRes" Font-Size="12px" Font-Bold="true" ForeColor="Black" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                      <td align="left" style="width: 40%; padding:0 20px 0 0; height:30px;">
                                      <asp:Button id="qcprocess" Text="QC Process" runat="server"/>&nbsp;
                                      <asp:Button id="qcreject" Text="QC Reject" runat="server"/>&nbsp;
                                      <asp:Button id="qcreturn" Text="QC Return" runat="server"/>&nbsp;
                                      <asp:Button id="PProcess" Text="P.Process" runat="server"/>&nbsp;
                                      <asp:Button id="PReject" Text="P.Reject" runat="server"/>&nbsp;
                                      <asp:Button id="Preturn" Text="P.Return" runat="server"/>&nbsp;
                                      <div class="bulk" style="display:none;">
                                    <asp:Label ID="lblbulktyp" runat="server" Text="Process Type:" ></asp:Label> 
                                      <asp:DropDownList ID="ddltyprocs" runat="server" >
                                     <asp:ListItem>QC process</asp:ListItem>
                                     <asp:ListItem>Payment Process</asp:ListItem>
                                     </asp:DropDownList>&nbsp;
                                     <asp:LinkButton ID="lnkbulkPr1" runat="server" Text="BulkProcess" OnClick="lnkbulkPr_Click"></asp:LinkButton> 
                                     </div>
                                     </td>
                                    <td align="right" style="width: 20%; padding:0 20px 0 0">
                                     Last refresh: <asp:Label ID="lblRefreshTime" runat="server" ></asp:Label>
                                       
                                    </td>
                                     <td align="right" style="width: 135px; padding:0">
                                        <asp:Label ID="lblResCount" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; padding-top:0">
                        <asp:HiddenField id="hdncheck" runat="server"/>
                        
                            <div style="width: 100%;" id="divCarresults" runat="server">
                                <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                    <asp:UpdatePanel ID="UpdtpnlHeader" runat="server">
                                        <ContentTemplate>
                                            <table class="grid1 " cellpadding="0" cellspacing="0" style="position: absolute;
                                                top: 2px; width: 1220px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed" >
                                                 <td style="width:30px" align="left">
                                                        <asp:CheckBox ID="chkall" runat="server" onclick="javascript:SelectAll(this);"></asp:CheckBox>
                                                        
                                                    </td>
                                                
                                                    <td style="width:60px" align="left">
                                                        <asp:LinkButton ID="lnkCarIDSort" runat="server" Text="Sale ID " OnClick="lnkCarIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td style="width:170px"align="left">
                                                        <asp:LinkButton ID="lnkSaleDateSort" runat="server" Text="Sale Data" OnClick="lnkSaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left" style="width:200px">
                                                      
                                                        <asp:LinkButton ID="lnkAgentSort" runat="server" Text="Payment Details" OnClick="lnkAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    
                                                    <td  align="left" style="width:200px">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnQCStatus" runat="server" Text="Customer Details"
                                                            OnClick="lnkbtnQCStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td  align="left" style="width:170px">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnPaymentStatus" runat="server" Text="Vehicle Details"
                                                            OnClick="lnkbtnPaymentStatus_Click"></asp:LinkButton>
                                                    </td>
                                                   
                                                    <td align="left" style="width:100px" >
                                                        <%--Year--%>
                                                        <asp:LinkButton ID="lnkYearSort" runat="server" Text="Notes"
                                                            OnClick="lnkYearSort_Click"></asp:LinkButton>
                                                    </td>
                                                    
                                                      <td  align="left"  >
                                                        <%--Year--%>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Status"
                                                            ></asp:LinkButton>
                                                    </td>
                                                    
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="width: auto; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; ">
                                    <asp:Panel ID="pnl1" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdPnlGrid" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="txthdnSortOrder" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="txthdnSortColumnId" type="hidden" runat="server" enableviewstate="true" />
                                                <%--<span style="color: rgb(34, 34, 34); font-family: Consolas, 'Lucida Console', monospace; font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none;">
                                               </span>--%>
                                                
                                                 <asp:GridView Width="1220px" ID="grdWarmLeadInfo" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1 autoWordBreak" AutoGenerateColumns="False"  GridLines="None" 
                                                    ShowHeader="false" OnRowDataBound="grdWarmLeadInfo_RowDataBound" OnRowCommand="grdWarmLeadInfo_RowCommand">
                                                    <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle CssClass="headder" />
                                                    <PagerSettings Position="Top" />
                                                    <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                    <RowStyle CssClass="row1" />
                                                    <AlternatingRowStyle CssClass="row2" />
                                                    <Columns>
                                                  
                                                  <%--  For Checkbox --%>
                                                     <asp:TemplateField>
                                                            <ItemTemplate>
                                                               <asp:CheckBox id="chk1" runat="server"/>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="30px" Height="90px"/>
                                                        </asp:TemplateField>
                                                        
                                                       <%-- FGor SaleId--%>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCarID" runat="server" Text='<%# Eval("carid")%>' CommandArgument='<%# Eval("carid")%>'
                                                                    CommandName="EditSale"></asp:LinkButton>
                                                            </ItemTemplate>
                                                           <ItemStyle HorizontalAlign="Left" Width="60px" Height="90px"/>
                                                        </asp:TemplateField>
                                                        
                                                        <%--Fopr SaleData--%>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                               <asp:Label ID="lblSaleData" runat="server" ></asp:Label><br />
                                                               <asp:Label ID="lblAgent" runat="server" ></asp:Label><br />
                                                                <asp:Label ID="lblVerifier" runat="server" ></asp:Label><br />
                                                               <asp:LinkButton ID="lnkvoicefile" runat="server"></asp:LinkButton><br />
                                                                <asp:Label ID="lblPackage" runat="server" ></asp:Label><br />
                                                                <asp:Label ID="lblCTotaCars" runat="server" ></asp:Label>
                                                               
                                                            </ItemTemplate>
                                                           <ItemStyle HorizontalAlign="Left" Width="170px" Height="90px"/>
                                                        </asp:TemplateField>
                                                        
                                                      <%--For PaymentDetails--%>
                                                         <asp:TemplateField>
                                                            <ItemTemplate>
                                                        
                                                                  <asp:Label ID="lblType" runat="server"></asp:Label><br />
                                                                   <asp:Label ID="lblSecurity" runat="server"></asp:Label><br />
                                                                    <asp:Label ID="lblName" runat="server"></asp:Label><br />
                                                                     <asp:Label ID="lblAdd1" runat="server"></asp:Label><br />
                                                                      <asp:Label ID="lblCity" runat="server"></asp:Label><br />
                                                                       <asp:Label ID="lblToday" runat="server"></asp:Label>
                                                                      
                                                            </ItemTemplate>
                                                             <ItemStyle HorizontalAlign="Left" Width="200px" Height="90px"/>
                                                        </asp:TemplateField>
                                                        
                                                        <%--For CustomerDetails--%>
                                                         <asp:TemplateField>
                                                            <ItemTemplate>
                                                                  <asp:Label ID="lblCusPhn" runat="server"></asp:Label><a href="http://www.whitepages.com/">Check</a><br />
                                                                  <asp:Label ID="lblCName" runat="server"></asp:Label><br />
                                                                  <asp:Label ID="lblcAdd" runat="server"></asp:Label><br />
                                                                  <asp:Label ID="lblCCity" runat="server"></asp:Label><br />
                                                                  <asp:Label ID="LblCmail" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                          <ItemStyle HorizontalAlign="Left" Width="200px" Height="90px"/>
                                                        </asp:TemplateField>
                                                        
                                                       <%--For VehicleDetails--%>
                                                         <asp:TemplateField>
                                                            <ItemTemplate>
                                                                  <asp:Label ID="LblTitle" runat="server"></asp:Label><br />
                                                                    <asp:Label ID="LblPriMil" runat="server"></asp:Label><br />
                                                                      <asp:Label ID="lblMakemodl" runat="server"></asp:Label><br />
                                                                      <asp:LinkButton ID="lblfet" runat="server" Text="Features"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                        CommandName="CarAddFeat"></asp:LinkButton> /
                                                                                                          <asp:LinkButton ID="lblAddFer" runat="server" Text="Addl Features" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                        CommandName="CarFea"></asp:LinkButton><br />
                                                                                                           <asp:LinkButton ID="lblDescr" runat="server" Text="Description" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                        CommandName="CarDesctiption"></asp:LinkButton>  <br />
                                                                                                             <asp:LinkButton ID="lblpictres" runat="server" Text="PicData" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                        CommandName="PicData"></asp:LinkButton> /
                                                                                                          <asp:LinkButton ID="lblpicDesc" runat="server" Text="Descript Data"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                        CommandName="PicDescd"></asp:LinkButton>
                                                                                                          
                                                                        </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="170px" Height="90px"/>
                                                        </asp:TemplateField>
                                                        
                                                        <%--For Notes--%>
                                                         <asp:TemplateField>
                                                            <ItemTemplate>
                                                                 <asp:LinkButton ID="lblnotels" runat="server" Text="SalesNotes" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                CommandName="SaleNote"></asp:LinkButton><br />
                                                                                             <asp:LinkButton ID="lblQcNotes" runat="server" Text="QCNotes" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                CommandName="QCNote"></asp:LinkButton><br />
                                                                                             <asp:LinkButton ID="lblPaynotes" runat="server" Text="Pmnt Notes" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "carid")%>'
                                                                CommandName="PaymentNote"></asp:LinkButton><br />
                                                                                             <asp:LinkButton ID="lblpayhist" runat="server" Text="Pmnt History" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PostingID")%>'
                                                                CommandName="PaymentHistory"></asp:LinkButton><br />
                                                                 <asp:LinkButton ID="leaddetails" runat="server" Text="Lead Details" Visible="false"></asp:LinkButton><br />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        
                                                        <%--For Status--%>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                 <asp:Label ID="lblQcSatus" runat="server"></asp:Label>
                                                                 <asp:LinkButton ID="QCProcessta" runat="server" Text="Process"  CommandArgument='<%# Eval("postingID")%>'
                                                                    CommandName="QCProcesstas" Enabled="true"></asp:LinkButton><br />
                                                                 Pmnt : <asp:Label ID="lb1PmtSta" runat="server"></asp:Label>
                                                                 <asp:LinkButton ID="lbpmtpprcs" Text="Process" runat="server" Visible="false"  CommandArgument='<%# Eval("postingID")%>'
                                                                    CommandName="PaymentProcess" Enabled="true"></asp:LinkButton><br />
                                                                Transfer : <b><asp:LinkButton ID="lblTransfr" runat="server" CommandArgument='<%# Eval("postingID")%>'
                                                                    CommandName="MoveSmartz"></asp:LinkButton></b><br />
                                                                  <%--<asp:Label ID="lblQcPer" runat="server"></asp:Label>--%>
                                                                 <%-- <asp:LinkButton ID="lbpmntQc" Text="Process" runat="server" Visible="false"></asp:LinkButton>--%>
                                                                 
                                                            </ItemTemplate>
                                                           <ItemStyle HorizontalAlign="Left"  Height="90px"/>
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
                                <div>
                                <table>
                                <tr>
                                <td align="center">
                                <asp:Label ID="lblsearchdetails" runat="server" Text ="Search Criteria" Visible="false"></asp:Label>
                                </td>
                                </tr>
                                </table>
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
                                                            <td colspan="2">
                                                                <h5 style="font-size: 15px; margin: 0; float: left; width: 130px;">
                                                                    <b>Paypal Details</b></h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Payment trans ID</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPaypalTranID" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <b>Paypal account email</b>
                                                            </td>
                                                            <td>
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
                                                                <asp:ListItem Value="4">All</asp:ListItem>
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
                                                            <asp:TextBox ID="txtPaymentAmountInPop" runat="server" MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
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
                                            <td style="padding-top: 15px;">
                                                <div style="width: 240px; margin: 0 auto;">
                                                    <asp:Button ID="btnUpdate" OnClientClick="return ValidateUpdate();" runat="server"
                                                        Text="Update" CssClass="g-button g-button-submit" OnClick="btnUpdate_Click" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnCancelUpdate" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup();" />
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
                &nbsp;<asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                &nbsp;<asp:UpdatePanel ID="UpdatePanel3" runat="server">
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
                &nbsp;<asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblAddAnotherCarAlertError" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnAddAnotherCarNo" class="btn" runat="server" Text="No" />&nbsp;
            <asp:Button ID="btnAddAnotherCarYes" class="btn" runat="server" Text="Yes" OnClick="btnAddAnotherCarYes_Click" />
        </div>
    </div>
    
    
   <%--hi--%>
   
   <!--Model pop up for Car Features-->

    <asp:HiddenField ID="lblfet" runat="server" />
   <cc1:ModalPopupExtender ID="mdlCarFeature" runat="server" PopupControlID="tblUpdate1"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblfet" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate1" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 360px; margin-top: 110px; width: 390px">
            <h4>
                Car Features
                       </h4>
            <div class="dat" style="padding: 0 0 0 2px; height: 320px; width:99%; overflow-y:scroll; overflow-x:hidden">
                <table id="Table1" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 99%; margin: 0 auto; ">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad gridView ">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                   
                                                        <tr>
                                                         <td style="width: 30%;">
                                                            <b>CarId:</b> &nbsp;
                                                            <asp:Label ID="CarID" runat="server"></asp:Label>
                                                        </td>
                                                        </tr>
                                                         <tr>
                                                        <td style="width: 30%;">
                                                            <b>Comfort:</b> &nbsp;
                                                            <asp:Label ID="lblcomfrt" runat="server"></asp:Label>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td style="width: 30%;">
                                                            <b>Seats:</b> &nbsp;
                                                            <asp:Label ID="lblseats" runat="server"></asp:Label>
                                                            
                                                        </td>
                                                        </tr>
                                                         <tr>
                                                        <td style="width: 30%;">
                                                            <b>Safety:</b> &nbsp;
                                                            <asp:Label ID="lblsafety" runat="server"></asp:Label>
                                                            
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td style="width: 30%;">
                                                            <b>SoundSystem:</b> &nbsp;
                                                            <asp:Label ID="lblSound" runat="server"></asp:Label>
                                                        </td>
                                                        </tr>
                                                         <tr>
                                                        <td style="width: 30%;">
                                                            <b>Windows:</b> &nbsp;
                                                            <asp:Label ID="lblwindows" runat="server"></asp:Label>
                                                        </td>
                                                        </tr>
                                                         <tr>
                                                        <td style="width: 30%;">
                                                            <b>Other:</b> &nbsp;
                                                            <asp:Label ID="lblothers" runat="server"></asp:Label>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td style="width: 30%;">
                                                            <b>New:</b> &nbsp;
                                                            <asp:Label ID="lblnew" runat="server"></asp:Label>
                                                        </td>
                                                        </tr>
                                                          <tr>
                                                        <td style="width: 30%;">
                                                            <b>Specials:</b> &nbsp;
                                                            <asp:Label ID="lblspecls" runat="server"></asp:Label>
                                                        </td>
                                                        </tr>
                                                       <br />
                                                        <tr>
                                                        <td align="center">
                                                        <br />
                                                         <asp:Button ID="Button1" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup();" />
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <!--pop up end-->
 
 
 
 <!--Model pop up for Car Description-->
   
    <asp:HiddenField ID="lblDescr" runat="server" />
   <cc1:ModalPopupExtender ID="MPCardescr" runat="server" PopupControlID="tblUpdate2"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblDescr" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate2" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
                Car Description
                       </h4>
            <div class="dat" style="padding: 0 0 0 6px;  height: 280px; width:99%;">
                <table id="Table3" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 95%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                    <td >
                                                    <asp:TextBox ID="txtcardesc"  runat="server" TextMode="MultiLine" Width="100%" Rows="10" ></asp:TextBox>
                                                    </td>
                                                    </tr>
                                                     <tr>
                                                     <td align="center"><br />
                                                         <asp:Button ID="Button2" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup1();" />
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <!--pop up end-->
    
    <!--payment-->
    <cc1:ModalPopupExtender ID="mdepAlertRejectThere" runat="server" PopupControlID="divRejectThere"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnRejectThere" OkControlID="btnRejectThereNo">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnRejectThere" runat="server" />
    <div id="divRejectThere" class="alert" style="display: none">
        <h4 id="H7">
            Alert
           
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblRejectThereError" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="clear">
                </div>
            </p>
            <asp:Button ID="btnRejectThereNo" class="btn" runat="server" Text="No" />
            &nbsp;
            <asp:Button ID="btnRejectThereYes" class="btn" runat="server" Text="Yes" OnClick="btnRejectThereYes_Click" />
        </div>
    </div>
     <!--payment end-->
     
     <!-- Hist -->
     <cc1:ModalPopupExtender ID="mdepalertMoveSmartz" runat="server" PopupControlID="divdraftPhone"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdndraftPhNo">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdndraftPhNo" runat="server" />
    <div id="divdraftPhone" class="alert" style="display: none">
        <h4 id="H2">
            Alert
            <%--<asp:Button ID="btnDiv" class="cls" runat="server" Text="" BorderWidth="0" />--%>
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMoveSmartz" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="clear">
                </div>
            </p>
            <asp:Button ID="btnMoveSmartzNo" class="btn" runat="server" Text="No" OnClick="btnMoveSmartzNo_Click" />
            &nbsp;
            <asp:Button ID="btnMoveSmartzYes" class="btn" runat="server" Text="Yes" OnClick="MoveSmartz_Click" />
        </div>
    </div>
    
    <!-- Another Model-->
        <cc1:ModalPopupExtender ID="mpealteruser" runat="server" PopupControlID="AlertUser"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAlertuser">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAlertuser" runat="server" />
    <div id="AlertUser" class="alert" style="display: none">
        <h4 id="H1">
            Alert
            <%--<asp:Button ID="BtnCls" class="cls" runat="server" Text="" BorderWidth="0" OnClick="btnNo_Click" />--%>
            <!-- <div class="cls"> </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="updpnlMsgUser1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErr" runat="server"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:RadioButton ID="rdbtnPmntPending" runat="server" Text="Pending" CssClass="noLM"
                GroupName="PmntStatus" Checked="true" />
            <asp:RadioButton ID="rdbtnPmntReject" runat="server" Text="Reject" CssClass="noLM"
                GroupName="PmntStatus" />
            <asp:RadioButton ID="rdbtnPmntReturned" runat="server" Text="Returned" CssClass="noLM"
                GroupName="PmntStatus" />
            &nbsp;
            <asp:Button ID="Button3" class="btn" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        </div>
    </div>
    
    <!-- End -->
    
    
     <!-- Sales Notes Model-->
      <asp:HiddenField ID="lblsales" runat="server" />
   <cc1:ModalPopupExtender ID="ModelSalesno" runat="server" PopupControlID="tblUpdate3"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblsales" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate3" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 340px; margin-top: 90px; width: 370px">
            <h4>
               SalesNote:
                       </h4>
            <div class="dat" style="padding: 0 0 0 6; overflow: scroll; height: 300px; width:320px;">
                <table id="Table4" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 90%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                    <tr>
                                                    <td  >
                                                    SalesNote:
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    <asp:Label ID="lblsalesnot" runat="server" ></asp:Label>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btncancel" runat="server" CssClass="g-button g-button-submit" 
                                                                OnClientClick="return ClosePopup2();" Text="Close" />
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <!--pop up end-->
     
    
    <!-- End -->
    
    
     <!-- Sales Notes Model-->
      <asp:HiddenField ID="lblQcNotes" runat="server" />
   <cc1:ModalPopupExtender ID="ModelQCno" runat="server" PopupControlID="tblUpdate4"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblQcNotes" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate4" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               QCNotes:
                       </h4>
            <div class="dat" style="padding: 0 0 0 6; overflow: scroll; height: 280px; width:300px;">
                <table id="Table5" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 90%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                    <tr>
                                                    <td  rowspan="4">
                                                    SalesNote:
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    <asp:Label ID="lblQcNotesty" runat="server" ></asp:Label>
                                                    </td>
                                                    </tr>
                                                     <tr>
                                                     <td>
                                                         <asp:Button ID="BtnQCNote" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup3();" />
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <!--pop up end-->
    
     
     <!-- payment Notes Model-->
      <asp:HiddenField ID="lblPaynotes" runat="server" />
   <cc1:ModalPopupExtender ID="ModelPaymtno" runat="server" PopupControlID="tblUpdate5"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblPaynotes" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate5" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               PaymentNotes:
                       </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width:300px;">
                <table id="Table6" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 96%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" >
                                                    <tr>
                                                    <td>
                                                        <b>Payment Notes:</b><br />
                                                        <asp:Label ID="lblpaynotes1" runat="server" ></asp:Label>
                                                    </td>
                                                    </tr>
                                                   
                                                     <tr>
                                                     <td style="text-align:center"><br />
                                                         <asp:Button ID="btnpaym" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup4();" />
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <!--pop up end-->
    
    <!-- Payment History-->
    <cc1:ModalPopupExtender ID="mpeaSalesData" runat="server" PopupControlID="tblUpdate11"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblpayhist" CancelControlID="BtnClsSendRegMail" >
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="lblpayhist" runat="server" />
    <div id="tblUpdate11" class="PopUpHolder" style="display: none;"  >
        <div class="main" style="width: 1120px" >
            <h4>
                Payment Transaction History
                <asp:LinkButton ID="BtnClsSendRegMail" runat="server" Text="Close" BorderWidth="0"
                    CssClass="close"></asp:LinkButton>
                <!-- <div class="cls">
            </div> -->
            </h4>
            <div class="dat" style="padding: 0 10px; width: 98%;">
                <asp:UpdatePanel ID="updtpnlHistory" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%; margin: 10px 0;">
                            <tr>
                                <td style="vertical-align: top;">
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <b>Sale ID:&nbsp;<asp:Label ID="lblPayTransSaleID" runat="server"></asp:Label></b>
                                            </div>
                                            <div style="width: 100%">
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div style="width: 100%;" id="divresults" runat="server">
                                        <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                <ContentTemplate>
                                                    <table class="grid1 gridBoldHed" cellpadding="0" cellspacing="0" style="position: absolute;
                                                        top: 2px; padding-top: 2px; width: 1070px; background: #fff;">
                                                        <tr class="tbHed">
                                                            <td width="110px" align="left">
                                                                Trans Dt
                                                            </td>
                                                            <td align="left" width="90px">
                                                                Trans By
                                                            </td>
                                                            <td width="80px" align="left">
                                                                Card Type
                                                            </td>
                                                            <td width="60px" align="left">
                                                                Card #
                                                            </td>
                                                            <td width="110px" align="left">
                                                                First Name
                                                            </td>
                                                            <td width="110px" align="left">
                                                                Last Name
                                                            </td>
                                                            <td width="110px" align="left">
                                                                Address
                                                            </td>
                                                            <td width="80px" align="left">
                                                                City
                                                            </td>
                                                            <td width="50px" align="left">
                                                                State
                                                            </td>
                                                            <td width="55px" align="left">
                                                                Zip
                                                            </td>
                                                            <td width="70px" align="left">
                                                                Amount
                                                            </td>
                                                            <td align="left">
                                                                Result
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div style="width: 1090px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                            border: #ccc 1px solid; height: 300px">
                                            <asp:Panel ID="Panel1" Width="100%" runat="server">
                                                <asp:UpdatePanel ID="updtpanelGridPopup" runat="server">
                                                    <ContentTemplate>
                                                        <input style="width: 91px" id="Hidden1" type="hidden" runat="server" enableviewstate="true" />
                                                        <input style="width: 40px" id="Hidden2" type="hidden" runat="server" enableviewstate="true" />
                                                        <asp:GridView Width="1070px" ID="grdIntroInfo" runat="server" CellSpacing="0" CellPadding="0"
                                                            CssClass="grid1" AutoGenerateColumns="False" GridLines="None" ShowHeader="false"
                                                            OnRowDataBound="grdIntroInfo_RowDataBound">
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
                                                                        <asp:Label ID="lblTransDt" runat="server" Text='<%# Bind("PayTryDatetime", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransBy" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"AgentUserName"),11)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransCardType" runat="server" Text='<%# Eval("CardType")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransCardNum" runat="server"></asp:Label>
                                                                        <asp:HiddenField ID="hdnTransCardNum" runat="server" Value='<%# Eval("CardNumber")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransFirstName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"CCFirstName"),15)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransLastName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"CCLastName"),15)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransAddress" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"Address"),15)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransCity" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"City"),10)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransState" runat="server" Text='<%# Eval("State")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransZip" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"Zip"),5)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="55px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransAmount" runat="server" Text='<%# Eval("Amount")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransResult" runat="server" Text='<%# Eval("Result")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdIntroInfo" EventName="Sorting" />
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
            <div class="clear">
                &nbsp;</div>
        </div>
        <div class="clear">
            &nbsp;</div>
    </div>
    
     <!-- Vehicle Adnl Features Model-->
         <asp:HiddenField ID="lblAddFer" runat="server" />
   <cc1:ModalPopupExtender ID="mdlPopAdl" runat="server" PopupControlID="tblUpdate6"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblAddFer" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate6" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 520px; margin-top: 70px; width: 970px">
            <h4>Car Additional Features   <asp:LinkButton ID="BtnClsSendRegMail2" runat="server" Text="Close" BorderWidth="0"
                    CssClass="close"></asp:LinkButton></h4>
            <!-- --------------------------------------------------  -->
            
            <div class="popupForm" id="infoV">
                                <fieldset>
                                    <!-- <legend>Vehicle Information</legend>  -->
                                    <table style="width: 100%;">
                                    
                                        <tr>
                                            <td style="width: 330px;">
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Make:</strong>
                                                      <asp:UpdatePanel ID="updtMake" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox id="ddlMake" runat="server" ></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                            <td style="width: 330px">
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Model:</strong>
                                                    <asp:UpdatePanel ID="updtpnlModel" runat="server">
                                                        <ContentTemplate>
                                                           <asp:TextBox ID="ddlModel" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Year:</strong>
                                                     <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                   <asp:TextBox ID="ddlYear1" runat="server"></asp:TextBox>
                                                   </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding: 0;">
                                                <table style="padding: 0; width: 99%;">
                                                    <tr>
                                                        <td style="width: 270px">
                                                            <h4 class="h4">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 45px">Price $:</strong>
                                                                <%--  <input type="text" style="width: 212px" class="sample4" />--%>
                                                                 <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                        <ContentTemplate>

                                                                <asp:TextBox ID="txtAskingPrice" runat="server" MaxLength="6" class="sample4" Enabled="false"></asp:TextBox>
                                                                </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                            </h4>
                                                        </td>
                                                        <td style="width: 190px">
                                                            <h4 class="h4">
                                                                <strong style="width: 40px">Mileage:</strong>
                                                                <%-- <input type="text" style="width: 119px" class="sample4" />--%>
                                                                 <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                        <ContentTemplate>
                                                                <asp:TextBox ID="txtMileage" runat="server" MaxLength="6" class="sample4" Enabled="false"></asp:TextBox>
                                                                  </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                            </h4>
                                                        </td>
                                                        <td valign="bottom">
                                                            <h4 class="h4 noB">
                                                             <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                        <ContentTemplate>
                                                                <strong style="width: 50px">Cylinders:</strong><span style="font-weight: bold">
                                                                    <asp:RadioButton ID="rdbtnCylinders1" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><span class="featNon">3</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders2" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><span class="featNon">4</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders3" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><span class="featNon">5</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders4" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><span class="featNon">6</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders5" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><span class="featNon">7</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders6" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><span class="featNon">8</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders7" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Checked="true" Enabled="false" /><span class="featNon">NA</span>
                                                                </span>
                                                                  </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 65px">Body style:</strong>
                                                    <%-- <input type="text" style="width: 245px" />--%>
                                                    <%-- <asp:DropDownList ID="ddlBodyStyle" runat="server" onchange="ChangeValuesHidden()"
                                                        Enabled="false">
                                                    </asp:DropDownList>--%>
                                                     <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                        <ContentTemplate>
                                                    <asp:TextBox ID="txtBodyStyle" runat="server" Enabled="false"></asp:TextBox>
                                                      </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 80px">Exterior color:</strong>
                                                    <%--<input type="text" style="width: 224px" />--%>
                                                    <%-- <asp:DropDownList ID="ddlExteriorColor" runat="server" onchange="ChangeValuesHidden()"
                                                        Enabled="false">
                                                    </asp:DropDownList>--%>
                                                    <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                                        <ContentTemplate>
                                                    <asp:TextBox ID="txtExteriorColor" runat="server" Enabled="false"></asp:TextBox>
                                                      </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 80px">Interior color:</strong>
                                                    <%--<input type="text" style="width: 170px" />--%>
                                                    <%--<asp:DropDownList ID="ddlInteriorColor" runat="server" onchange="ChangeValuesHidden()"
                                                        Enabled="false">
                                                    </asp:DropDownList>--%>
                                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                        <ContentTemplate>
                                                    <asp:TextBox ID="txtInteriorColor" runat="server" Enabled="false"></asp:TextBox>
                                                     </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding: 0">
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <h4 class="h4 noB">
                                                             <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                        <ContentTemplate>
                                                                <strong style="width: 45px">Transmission:</strong><span style="font-weight: bold">
                                                                    <asp:RadioButton ID="rdbtnTrans1" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Enabled="false" /><span class="featNon">Auto</span>
                                                                    <asp:RadioButton ID="rdbtnTrans2" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Enabled="false" /><span class="featNon">Manual</span>
                                                                    <asp:RadioButton ID="rdbtnTrans3" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Enabled="false" /><span class="featNon">Tiptronic</span>
                                                                    <asp:RadioButton ID="rdbtnTrans4" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Checked="true" Enabled="false" /><span class="featNon">NA</span>
                                                                </span>
                                                                 </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <h4 class="h4 noB">
                                                                <strong style="width: 45px">Doors:</strong><span>
                                                                    <%-- <input type="radio" class="noLM" />
                                                            2
                                                            <input type="radio" />
                                                            3
                                                            <input type="radio" />
                                                            4
                                                            <input type="radio" />
                                                            5</span></h4>--%>
                                                             <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                                                        <ContentTemplate>
                                                                    <asp:RadioButton ID="rdbtnDoor2" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">2</span>
                                                                    <asp:RadioButton ID="rdbtnDoor3" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">3</span>
                                                                    <asp:RadioButton ID="rdbtnDoor4" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">4</span>
                                                                    <asp:RadioButton ID="rdbtnDoor5" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">5</span>
                                                                    <asp:RadioButton ID="rdbtnDoor6" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Checked="true" Enabled="false" /><span class="featNon">NA</span> </span>
                                                                         </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding: 0;">
                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <h4 class="h4 noB">
                                                              <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                        <ContentTemplate>
                                                                <strong style="width: 65px">Drive train:</strong><span style="font-weight: bold">
                                                                    <asp:RadioButton ID="rdbtnDriveTrain1" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><span class="featNon">2WD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain2" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><span class="featNon">FWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain3" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><span class="featNon">AWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain4" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><span class="featNon">RWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain5" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Checked="true" Enabled="false" /><span class="featNon">NA</span>
                                                                </span>
                                                                 </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <h4 class="h4">
                                                                <strong>VIN #:</strong>
                                                                  <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                        <ContentTemplate>
                                                                <%--<input type="text" style="width: 409px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtVin" runat="server" Style="width: 409px" MaxLength="20" Enabled="false"></asp:TextBox>
                                                                  </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <h4 class="h4 noB">
                                                  <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                                        <ContentTemplate>
                                                    <strong style="width: 55px">Fuel type:</strong><span style="font-weight: bold">
                                                        <asp:RadioButton ID="rdbtnFuelType1" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><span class="featNon">Diesel</span>
                                                        <asp:RadioButton ID="rdbtnFuelType2" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><span class="featNon">Petrol</span>
                                                        <asp:RadioButton ID="rdbtnFuelType3" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><span class="featNon">Hybrid</span>
                                                        <asp:RadioButton ID="rdbtnFuelType4" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><span class="featNon">Electric</span>
                                                        <asp:RadioButton ID="rdbtnFuelType5" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><span class="featNon">Gasoline</span>
                                                        <asp:RadioButton ID="rdbtnFuelType6" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><span class="featNon">E-85</span>
                                                        <asp:RadioButton ID="rdbtnFuelType7" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><span class="featNon">Gasoline-Hybrid</span>
                                                        <asp:RadioButton ID="rdbtnFuelType8" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Checked="true" Enabled="false" /><span class="featNon">NA</span> </span>
                                                              </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding: 0;">
                                                <h4 class="h4 noB">
                                                    <strong style="width: 45px">Condition:</strong><span style="font-weight: bold">
                                                        <%-- <input type="radio" class="noLM" />
                                                Excellent
                                                <input type="radio" />
                                                Very good
                                                <input type="radio" />
                                                Good
                                                <input type="radio" />
                                                Fair
                                                <input type="radio" />
                                                Poor
                                                <input type="radio" />
                                                Parts or salvage--%>
                                                
                                                        <asp:RadioButton ID="rdbtnCondition1" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><span class="featNon">Excellent</span>
                                                        <asp:RadioButton ID="rdbtnCondition2" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><span class="featNon">Very good</span>
                                                        <asp:RadioButton ID="rdbtnCondition3" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><span class="featNon">Good</span>
                                                        <asp:RadioButton ID="rdbtnCondition4" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><span class="featNon">Fair</span>
                                                        <asp:RadioButton ID="rdbtnCondition5" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><span class="featNon">Poor</span>
                                                        <asp:RadioButton ID="rdbtnCondition6" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><span class="featNon">Parts or salvage</span>
                                                        <asp:RadioButton ID="rdbtnCondition7" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Checked="true" Enabled="false" /><span class="featNon">NA</span>
                                                    </span>
                                                </h4>
                                            </td>
                                        </tr>

                                    </table>
                                </fieldset>
                            </div>
            
            <!-- --------------------------------------------------  -->
            
            
        </div>
    </div>
    <!-- End -->
    
    
    <!-- PicData -->
    <asp:HiddenField ID="lblpictres" runat="server" />
   <cc1:ModalPopupExtender ID="mdlPicdata" runat="server" PopupControlID="tblUpdate7"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblpictres" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate7" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
                Picture Data
                       </h4>
            <div class="dat" style="padding: 0 0 0 6px;  height: 280px; width:99%;">
                <table id="Table8" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 95%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                    <td >
                                                    <asp:TextBox ID="txtpicdata"  runat="server" TextMode="MultiLine" Width="100%" Rows="10" ></asp:TextBox>
                                                    </td>
                                                    </tr>
                                                     <tr>
                                                     <td align="center"><br />
                                                         <asp:Button ID="Button4" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup9();" />
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <!-- Picdataend -->
    
    
    <!-- PicDesc -->
      <asp:HiddenField ID="lblpicDesc" runat="server" />
   <cc1:ModalPopupExtender ID="Mdlpicdesc" runat="server" PopupControlID="tblUpdate8"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblpicDesc" CancelControlID="btnCancelUpdate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate8" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
                Picture Data
                       </h4>
            <div class="dat" style="padding: 0 0 0 6px;  height: 280px; width:99%;">
                <table id="Table9" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 95%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                    <td >
                                                    <asp:TextBox ID="txtpicdesc"  runat="server" TextMode="MultiLine" Width="100%" Rows="10" ></asp:TextBox>
                                                    </td>
                                                    </tr>
                                                     <tr>
                                                     <td align="center"><br />
                                                         <asp:Button ID="Button5" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup10();" />
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
  
     <!-- Sales Notes Model-->
      <asp:HiddenField ID="QCProcessta" runat="server" />
   <cc1:ModalPopupExtender ID="MdlQcProcessta" runat="server" PopupControlID="tblUpdate12"
        BackgroundCssClass="ModalPopupBG" TargetControlID="QCProcessta">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate12" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               QC Process Status:
                       </h4>
            <div class="dat" style="padding: 0 0 0 6; overflow: scroll; height: 280px; width:300px;">
                <table id="Table10" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 90%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                <tr><td>
                                               SaleId: <asp:TextBox ID="qccarid" runat="server" ></asp:TextBox></td></tr>
                                                    <tr>
                                                    <td>
                                                    QC Status:  <asp:DropDownList ID="DropDownList2" runat="server" Font-Size="14px" Font-Bold="true"
                                                    ForeColor="#2B4BB1">
                                                    <asp:ListItem Value="0">Select QC Status</asp:ListItem>
                                                    <asp:ListItem Value="1">QC Approved</asp:ListItem>
                                                    <asp:ListItem Value="2">QC Reject</asp:ListItem>
                                                    <asp:ListItem Value="3">QC Pending</asp:ListItem>
                                                    <asp:ListItem Value="4">QC Returned</asp:ListItem>
                                                    </asp:DropDownList>
                                                    </td>
                                                    </tr>
                                                     <tr>
                                                    <td>
                                                    QC Old Notes: <asp:TextBox ID="txtOldQcNotes" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    QC Notes: <asp:TextBox ID="txtQCNotes" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    </tr>
                                                   
                                                     <tr>
                                                     
                                                     <td>
                                                         <asp:Button ID="Button6" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Update"  OnClick="Updatesta_Click" />
                                                        </td>
                                                        <td>
                                                         <asp:Button ID="Button7" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup15();"/>
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
                           
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <!--pop up end-->
    
    <!--  QCBulk -->
    
    <cc1:ModalPopupExtender ID="modelbulkQCProc" runat="server" PopupControlID="tblUpdate13"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lnkbulkPr" CancelControlID="BtnClsSendRegMail1" >
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="lnkbulkPr" runat="server" />
    <div id="tblUpdate13" class="PopUpHolder" style="display: none;"  >
        <div class="main" style="width: 1120px" >
            <h4>
                Bulk QC Process <asp:Label id="lblerrormsg" runat="server" Visible="false"></asp:Label>
                <asp:LinkButton ID="BtnClsSendRegMail1" runat="server" Text="Close" BorderWidth="0"
                    CssClass="close"></asp:LinkButton>
                <!-- <div class="cls">
            </div> -->
            </h4>
            <div class="dat" style="padding: 0 10px; width: 98%;">
                <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%; margin: 10px 0;">
                            <tr>
                                <td style="vertical-align: top;">
                                    <div style="width: 100%;" id="div1" runat="server">
                                       <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                <ContentTemplate>
                                                    <table class="grid1 gridBoldHed" cellpadding="0" cellspacing="0" style="position: absolute;
                                                        top: 2px; padding-top: 2px; width: 1070px; background: #fff;">
                                                        <tr class="tbHed">
                                                         <td width="14px" align="left">
                                                                
                                                            </td>
                                                            <td width="50px" align="left">
                                                                Sales Id
                                                            </td>
                                                            <td width="120px" align="left">
                                                                Sale Date
                                                            </td>
                                                            <td width="106px" align="left">
                                                                Agent/Verifier
                                                            </td>
                                                            <td width="66px" align="left">
                                                            Cust.Name
                                                            </td>
                                                            <td width="120px" align="left">
                                                            QC Status
                                                            </td>
                                                            <td width="160px" align="left">
                                                            Desired Status
                                                            </td>
                                                            <td width="190px" align="left">
                                                            Notes to enter
                                                            </td>
                                                            <td>
                                                           Results
                                                            </td>
                                                            
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div style="width: 1090px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                            border: #ccc 1px solid; height: 300px">
                                            <asp:Panel ID="Panel2" Width="100%" runat="server">
                                                <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                    <ContentTemplate>
                                                        <input style="width: 91px" id="Hidden3" type="hidden" runat="server" enableviewstate="true" />
                                                        <input style="width: 40px" id="Hidden4" type="hidden" runat="server" enableviewstate="true" />
                                                        <asp:GridView Width="1070px" ID="BulkQc" runat="server" CellSpacing="0" CellPadding="0"
                                                            CssClass="grid1" AutoGenerateColumns="False" GridLines="None" ShowHeader="false"
                                                           >
                                                            <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                            <HeaderStyle CssClass="headder" />
                                                            <PagerSettings Position="Top" />
                                                            <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                            <RowStyle CssClass="row1" />
                                                            <AlternatingRowStyle CssClass="row2" />
                                                            <Columns>
                                                           
                                                                                <asp:BoundField DataField="RowNumber" HeaderText="Row Number" />
                                         <asp:TemplateField HeaderText="Header 1" >
                                          <HeaderTemplate>
                                                                <asp:LinkButton ID="SalesId" runat="server"  Text="Category" />
                                                            </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="saleid" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header 2">
                                        <ItemTemplate>
                                        <asp:Label ID="saledate" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header 3">
                                        <ItemTemplate>
                                        <asp:Label ID="Agent" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Header 4">
                                        <ItemTemplate>
                                        <asp:Label ID="Customername" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Header 5">
                                        <ItemTemplate>
                                        <asp:Label ID="QCStatus" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header 6">
                                        <ItemTemplate>
                                             <asp:DropDownList ID="desrdqcstat" runat="server" >
                                            <asp:ListItem Value="0">Select QC Status</asp:ListItem>
                                            <asp:ListItem Value="1">QC Approved</asp:ListItem>
                                            <asp:ListItem Value="2">QC Reject</asp:ListItem>
                                            <asp:ListItem Value="3">QC Pending</asp:ListItem>
                                            <asp:ListItem Value="4">QC Returned</asp:ListItem>
                                             </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Header 7">
                                        <ItemTemplate>
                                        <asp:TextBox ID="Notestoresult" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header 8">
                                        <ItemTemplate>
                                        <asp:TextBox ID="result" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdIntroInfo" EventName="Sorting" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <tr>
                                                
                                                <td style="width: 91px" align="center"><asp:Button id="btnproceed1" runat="server"  CssClass="g-button g-button-submit"  Text="Proceed" OnClick="btnproceed1_Click" />
                                                &nbsp; <asp:Button id="btnOk1" runat="server" Text="OK"  CssClass="g-button g-button-submit"  OnClientClick="return ClosePopup18();" Visible="false"/>
                                                &nbsp; <asp:Button id="btncancel1" runat="server" Text="Cancel"  CssClass="g-button g-button-submit"  OnClientClick="return ClosePopup17();"/></td>
                                                </tr>
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
            <div class="clear">
                &nbsp;</div>
        </div>
        <div class="clear">
            &nbsp;</div>
    </div>
    <!--pop up end-->
    <!-- QC Bulk end -->
    
    </form>
</body>
</html>
