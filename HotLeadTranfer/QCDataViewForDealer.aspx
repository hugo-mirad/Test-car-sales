<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCDataViewForDealer.aspx.cs"
    Inherits="QCDataViewForDealer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>..:: Car Sales System ::..</title>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link href="css/ddsmoothmenu.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript" src="js/jquery-1.7.min.js"></script>

    <script src="Static/JS/CarsJScript.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/emulatetab.joelpurra.js"></script>

    <script type="text/javascript" src="js/plusastab.joelpurra.js"></script>

    <script type='text/javascript' language="javascript" src='js/jquery.alphanumeric.pack.js'></script>

    <script src="js/jquery.formatCurrency-1.4.0.js" type="text/javascript"></script>

    <script src="js/ddsmoothmenu.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">window.history.forward(1);</script>

    <script type="text/javascript" language="javascript">
//<![CDATA[
        function poptastic(url)
        {
	    newwindow=window.open(url,'name','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,height=420,width=300');
	        if (window.focus) {newwindow.focus()}
        }
	JoelPurra.PlusAsTab.setOptions({
		// Use enter instead of plus
		// Number 13 found through demo at
		// http://api.jquery.com/event.which/
		
		key: 13
	});
        /*
	$("form")
			.submit(simulateSubmitting);

	function simulateSubmitting(event)
	{
		event.preventDefault();

		if (confirm("Simulating that the form has been submitted.\n\nWould you like to reload the page?"))
		{
			location.reload();
		}

		return false;
	}
	*/
//]]>
    </script>

    <script type="text/javascript" language="javascript">
        //-------------------------- Agents Centers Info END ------------------------------------------
//    $(window).load(function(){
//        TransfersInfoBinding();
//    })
  function TransfersInfoBinding(){
        youfunction()
        $(window).load(function(){
            //alert('Ok')
            youfunction()
        });            
  }
  
    
    function youfunction(){
        //alert('ok')
           $('#feat input[type=radio]').each(function(){
            if($(this).is(':checked')){
                $(this).parent().next('span').addClass('featAct')  
            }    
            
        });
        
        
         $('#infoV input[type=radio]').each(function(){
            if($(this).is(':checked')){
                $(this).parent().next('span').addClass('featAct')  
            }  
        });
        
        
        
         $('#feat input[type=checkbox]').each(function(){              
                if($(this).is(':checked')){
                    $(this).parent().next('span').addClass('featAct')  
                }  
        });
        
        
          $('#feat input[type=checkbox]').each(function(){
            $(this).click(function(){
                if($(this).parent().hasClass('noLM')){
                    $(this).parent().next('span').toggleClass('featAct')
                }else{
                    $(this).next('span').toggleClass('featAct')
                }
                if($(this).is(':checked')){
                    $(this).parent().next('span').addClass('featAct')  
                }  
            })
        });
       
        
        
         $('#feat input[type=radio]').each(function(){
            $(this).click(function(){
              // var name = $(this).attr('name');
               $('#feat input[type=radio]').each(function(){
                    //if(name != $(this).attr('name')){
                        $(this).parent().next('span').removeClass('featAct')
                    //}
               });
               $(this).parent().next('span').addClass('featAct')             
               
            })
        });
        
        
         $('#infoV input[type=radio]').each(function(){
            $(this).click(function(){
               var name = $(this).attr('name');
               $('#infoV input[type=radio]').each(function(){
                    if(name == $(this).attr('name')){
                        $(this).parent().next('span').removeClass('featAct')
                    }
               })  
               $(this).parent().next('span').addClass('featAct')     
            })
        });
         $('.hid').click(function(){			
			if($(this).attr('id') == 'Vinfo'){
				var str = '';							
				if($('#Vinfo').next('div.hidden').is(':visible')){				
					$('#Vinfo label').empty().append(str);						
				}else{
					$('#Vinfo label').empty()
				}	
			}
			$(this).next('div.hidden').slideToggle();
		});		
    }
 //-------------------------- Agents Centers Info END ------------------------------------------
    </script>

    <script type="text/javascript" language="javascript">
 function QCValidation()
	{
	
        if(document.getElementById('<%=ddlQCStatus.ClientID%>').value =="0")
        {
        alert("Please select qc status"); 
        valid=false;
        document.getElementById('ddlQCStatus').focus();  
        return valid;               
        }
        if(document.getElementById('<%=ddlQCStatus.ClientID%>').value =="2")
        {
             if (document.getElementById('<%= txtQCNotes.ClientID%>').value.trim().length < 1)
            {
            alert("Please enter qc notes"); 
            valid=false;
            document.getElementById('txtQCNotes').focus();  
            return valid;               
            }   
        }
	}
    </script>

    <script type="text/javascript" language="javascript">
	
	
	$(function(){	
	    	
		$('.hid').next('div.hidden').hide();				
		
		
		$('.sample4').numeric();		
	})	
    </script>

    <script type="text/javascript" language="javascript">
     function echeck(str) {
            var at = "@"
            var dot = "."
            var lat = str.indexOf(at)
            var lstr = str.length
            var ldot = str.indexOf(dot)
            if (str.indexOf(at) == -1) {
                alert("Enter valid email")
                return false
            }

            if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
                alert("Enter valid email")
                return false
            }

            if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
                alert("Enter valid email")
                return false
            }

            if (str.indexOf(at, (lat + 1)) != -1) {
                alert("Enter valid email")
                return false
            }

            if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
                alert("Enter valid email")
                return false
            }

            if (str.indexOf(dot, (lat + 2)) == -1) {
                alert("Enter valid email")
                return false
            }

            if (str.indexOf(" ") != -1) {
                alert("Enter valid email")
                return false
            }

            return true
        }
 function ChangeValuesHidden()
      {
       document.getElementById("<%=hdnChange.ClientID%>").value ="1";
      } 
       function ChangeValues()
       {
         var hidden = document.getElementById("<%=hdnChange.ClientID%>").value ;
         if( hidden == '1')
         {
           var answer = confirm("If you move out of this page, changes will be permanently lost. Are you sure you want to move out of this page?")
           if (answer)
           {
              return true;
//              window.location.href = "CustomerView.aspx ";  
           }
           else           
           {
              return false;
           }
         }
       }    
    </script>

    <script type="text/javascript" language="javascript">
       function isNumberKey(evt)
         {
         debugger
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isNumberKeyWithDot(evt)
         {
         debugger
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;

            return true;
        }
         function isNumberKeyWithDashForZip(evt)
         {
         debugger
         
            var charCode = (evt.which) ? evt.which : event.keyCode         
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 45)
                return false;

            return true;
        }
         function isNumberKeyForDt(evt)
              {	
	    
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)&& charCode != 47)
                return false;
            return true;
        }
          function isKeyNotAcceptSpace(evt)
          {		    
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 32)
                return false;
            return true;
        }
    </script>

    <script type="text/javascript" language="javascript">   
        
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
                alert("Please enter a valid year");
                return false
            }
            if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                alert("Please enter a valid date")
                return false
            }
            return true
        }
    
    function PayInfoChanges() {
	         
            if(document.getElementById('<%=ddlPmntStatus.ClientID%>').value =="2")
            {
                 document.getElementById('<%=divReason.ClientID%>').style.display = "block";
            }
            else
            {
                document.getElementById('<%=divReason.ClientID%>').style.display = "none";
            }
            return false;
        }
       function ValidateDealerCode()
       {
            var valid = true;  
             if (document.getElementById('<%= txtDealerCode.ClientID%>').value.trim()=="1") {
                document.getElementById('<%= txtDealerCode.ClientID%>').focus();
                alert("Enter dealer code");
                document.getElementById('<%=txtDealerCode.ClientID%>').value = ""
                document.getElementById('<%=txtDealerCode.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }
              if (document.getElementById('<%= txtDealerCode.ClientID%>').value.trim().length < 3) {
                  document.getElementById('<%= txtDealerCode.ClientID%>').focus();
                  alert("Enter dealer code must be greater than 3 characters");
                  document.getElementById('<%=txtDealerCode.ClientID%>').value = ""
                  document.getElementById('<%=txtDealerCode.ClientID%>').focus()
                  valid = false;
                  return valid;
              } 
         return valid;     
       }
        function PmntValidation()
	    {
    	    valid=true;
            if(document.getElementById('<%=ddlPmntStatus.ClientID%>').value =="0")
            {
            alert("Please select payment status"); 
            valid=false;
            document.getElementById('ddlPmntStatus').focus();  
            return valid;               
            }
            if(document.getElementById('<%=ddlPmntStatus.ClientID%>').value =="2")
            {
                if(document.getElementById('<%= ddlPayCancelReason.ClientID%>').value == "0") {
                        document.getElementById('<%= ddlPayCancelReason.ClientID%>').focus();
                        alert("Select payment reject reason");                 
                        document.getElementById('<%=ddlPayCancelReason.ClientID%>').focus()
                        valid = false;            
                         return valid;     
                    } 
                 if (document.getElementById('<%= txtPaymentNotesNew.ClientID%>').value.trim().length < 1)
                {
                alert("Please enter payment notes"); 
                valid=false;
                document.getElementById('txtPaymentNotesNew').focus();  
                return valid;               
                }   
            }
            return valid; 
	    }
    </script>

</head>
<body>
    <form id="form1" runat="server" data-plus-as-tab="true">
    <asp:ScriptManager ID="scrptmgr" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/WebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <asp:UpdateProgress ID="Progress" runat="server" AssociatedUpdatePanelID="updtpnlSave"
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
            <td style="width: 250px;">
                <a>
                    <img src="images/logo2.png" /></a>
            </td>
            <td>
                <h1 style="border-bottom: none; padding-top: 5px;">
                    UNITED CAR EXCHANGE <span>Dealer Sales Order Form</span></h1>
            </td>
            <td style="width: 520px; padding-top: 10px;">
                <div class="loginStat">
                    Welcome &nbsp;<asp:Label ID="lblUserName" runat="server" Visible="false"></asp:Label>
                    <br />
                    <asp:LinkButton ID="lnkBtnLogout" runat="server" Text="Logout" Visible="false" OnClick="lnkBtnLogout_Click"></asp:LinkButton></div>
            </td>
        </table>
        <div class="clear">
            &nbsp;
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnChange" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="height: 10px;">
    </div>
    <div class="main" style="border: #ccc 1px solid; padding: 10px; background: rgb(253, 243, 234)">
        <table width="100%">
            <tr>
                <td width="400px">
                    &nbsp;
                </td>
                <td style="text-align: right">
                    <asp:UpdatePanel ID="updtpnlSave" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlQCStatus" runat="server" Font-Size="14px" Font-Bold="true"
                                ForeColor="#2B4BB1">
                                <asp:ListItem Value="0">Select QC Status</asp:ListItem>
                                <asp:ListItem Value="1">QC Approved</asp:ListItem>
                                <asp:ListItem Value="2">QC Reject</asp:ListItem>
                                <asp:ListItem Value="3">QC Pending</asp:ListItem>
                                <asp:ListItem Value="4">QC Returned</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="btnQCUpdate" runat="server" CssClass="g-button g-button-submit" Text="QC Update"
                                OnClientClick="return QCValidation();" OnClick="btnQCUpdate_Click" />
                            &nbsp;
                            <asp:Button ID="btnEdit" runat="server" CssClass="g-button g-button-submit" Text="Edit"
                                OnClick="btnEdit_Click" />
                            &nbsp;
                            <asp:Button ID="btnClose" runat="server" CssClass="g-button g-button-submit" Text="Close"
                                OnClick="btnClose_Click" />
                            &nbsp;
                            <asp:Button ID="btnMovedToSmartz" runat="server" CssClass="g-button g-button-submit"
                                Text="Move to Smartz" OnClick="MoveSmartz_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <table width="100%">
                    <tr>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 55px;">Dealer ID:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblSaleID" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 60px;">Sale Date:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblSaleDate" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 45px;">Location:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblLocation" runat="server"></asp:Label>
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 45px;">Coordinator:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblSaleAgent" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 75px;">Lead Source:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblLeadSource" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 70px;">Lead Agent:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblLeadAgent" runat="server"></asp:Label>
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 60px;">Target Date:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblTargetDate" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 60px;">Callback Date:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblCallbackDate" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 45px;">Promotion:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblPromotionOption" runat="server"></asp:Label>
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 60px;">QC Status:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblQCStatus" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 95px;">Payment Status:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblPaymentStatus" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 45px;">Package:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblPackage" runat="server"></asp:Label>
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4 class="h4">
                                <strong style="width: 80px;">Dealer Status:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <asp:Label ID="lblSaleStatus" runat="server"></asp:Label>
                            </h4>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </tr>
        </table>
        <asp:UpdatePanel ID="updtpnl" runat="server">
            <ContentTemplate>
                <table style="margin: 0 auto; width: 100%;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="5" style="padding-top: 0px;">
                            <h2 class="h200">
                                Dealer Information</h2>
                            <fieldset>
                                <!-- <legend>Seller Information</legend>  -->
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <h4 class="h4">
                                                <span class="star" style="color: Red">*</span><strong style="width: 100px">Dealership
                                                    name:</strong>
                                                <%-- <input type="text" style="width: 380px" />--%>
                                                <asp:TextBox ID="txtDealerShipName" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                            </h4>
                                        </td>
                                        <td>
                                            <h4 class="h4">
                                                <strong style="width: 50px">Phone#:</strong>
                                                <%-- <input type="text" style="width: 394px" />--%>
                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                    onblur="return PhoneOnblur();" onfocus="return PhoneOnfocus();" Enabled="false"></asp:TextBox>
                                            </h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <h4 class="h4">
                                                            <strong style="width: 40px">Fax#:</strong>
                                                            <%-- <input type="text" style="width: 394px" />--%>
                                                            <asp:TextBox ID="txtFax" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                                onblur="return FaxOnblur();" onfocus="return FaxOnfocus();" Enabled="false"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <h4 class="h4">
                                                            <strong style="width: 80px">Web Address:</strong>
                                                            <%-- <input type="text" style="width: 406px" />--%>
                                                            <asp:TextBox ID="txtWebAddress" runat="server" MaxLength="100" Enabled="false"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <h4 class="h4">
                                                            <strong style="width: 95px">Dealer license#:</strong>
                                                            <%-- <input type="text" style="width: 406px" />--%>
                                                            <asp:TextBox ID="txtDealerLicenseNumber" runat="server" MaxLength="60" Enabled="false"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <h4 class="h4">
                                                            <strong style="width: 50px">Address:</strong>
                                                            <%--<input type="text" style="width: 392px" />--%>
                                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0;">
                                            <table style="padding: 0; width: 100%;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 220px;">
                                                        <h4 class="h4">
                                                            <span class="star" style="color: Red">*</span><strong style="width: 30px">City:</strong>
                                                            <%-- <input type="text" style="width: 171px" />--%>
                                                            <asp:TextBox ID="txtCity" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                    <td style="width: 120px;">
                                                        <h4 class="h4">
                                                            <span class="star" style="color: Red">*</span><strong>State:</strong>
                                                            <%-- <input type="text" style="width: 63px" />--%>
                                                            <%--<asp:DropDownList ID="ddlLocationState" runat="server" Style="width: 100px">
                                                            </asp:DropDownList>--%>
                                                            <asp:TextBox ID="txtLocationState" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                    <td>
                                                        <h4 class="h4">
                                                            <span class="star" style="color: Red">*</span><strong>Zip:</strong>
                                                            <%--<input type="text" style="width: 74px" class="sample4" />--%>
                                                            <asp:TextBox ID="txtZip" runat="server" Style="width: 74px" MaxLength="5" class="sample4"
                                                                onkeypress="return isNumberKey(event)" onblur="return ZipOnblur();" Enabled="false"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200 hid">
                                Dealer Contact Information
                                <div class="close">
                                </div>
                            </h2>
                            <div class="hidden">
                                <fieldset>
                                    <!-- <legend>Seller Information</legend>  -->
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 50%;">
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 85px">Contact name:</strong>
                                                    <%-- <input type="text" style="width: 380px" />--%>
                                                    <asp:TextBox ID="txtDealerContactName" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 50px">Job title:</strong>
                                                    <%-- <input type="text" style="width: 394px" />--%>
                                                    <asp:TextBox ID="txtDealerJobTitle" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <h4 class="h4">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">Phone#:</strong>
                                                                <%-- <input type="text" style="width: 394px" />--%>
                                                                <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                                    onblur="return ContactPhoneOnblur();" onfocus="return ContactPhoneOnfocus();"
                                                                    Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <h4 class="h4">
                                                                <strong style="width: 40px">Mobile#:</strong>
                                                                <%-- <input type="text" style="width: 394px" />--%>
                                                                <asp:TextBox ID="txtContactMobileNumber" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                                    onblur="return ContactMobileOnblur();" onfocus="return ContactMobileOnfocus();"
                                                                    Enabled="false"></asp:TextBox>
                                                            </h4>
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
                                                            <h4 class="h4">
                                                                <strong style="width: 40px">Email:</strong>
                                                                <%-- <input type="text" style="width: 406px" />--%>
                                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="60" onblur="return EmailOnblur();"
                                                                    Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200 hid">
                                Product<div class="close">
                                </div>
                            </h2>
                            <div class="hidden" id="Div2">
                                <fieldset>
                                    <!-- <legend>Vehicle Features</legend>  -->
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 95px; padding-top: 10px;">
                                                <strong>Website:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <h4 class="h4 noB">
                                                                <asp:RadioButton ID="rdbtnWebsiteYes" CssClass="noLM" Text="" GroupName="Website"
                                                                    runat="server" Checked="true" Enabled="false" /><span class="featNon" enabled="false">Yes</span>
                                                                <asp:RadioButton ID="rdbtnWebsiteNo" CssClass="noLM" Text="" GroupName="Website"
                                                                    runat="server" Enabled="false" /><span class="featNon" enabled="false">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Preferred addresses:</strong><br />
                                                            <asp:TextBox ID="txtPreferredAddress" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <strong>Cars promotion:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <h4 class="h4 noB">
                                                                <asp:RadioButton ID="rdbtnCarsPromotionYes" CssClass="noLM" Text="" GroupName="CarsPromotion"
                                                                    runat="server" Checked="true" Enabled="false" /><span class="featNon">Yes</span>
                                                                <asp:RadioButton ID="rdbtnCarsPromotionNo" CssClass="noLM" Text="" GroupName="CarsPromotion"
                                                                    runat="server" Enabled="false" /><span class="featNon">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Where to get initial cars from?:</strong><br />
                                                            <asp:TextBox ID="txtGetCarsFrom" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <strong>Leads:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <h4 class="h4 noB">
                                                                <asp:RadioButton ID="rdbtnLeadsYes" CssClass="noLM" Text="" GroupName="Leads" runat="server"
                                                                    Checked="true" Enabled="false" /><span class="featNon">Yes</span>
                                                                <asp:RadioButton ID="rdbtnLeadsNo" CssClass="noLM" Text="" GroupName="Leads" runat="server"
                                                                    Enabled="false" /><span class="featNon">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Whom to send it?:</strong><br />
                                                            <asp:TextBox ID="txtLeadsToSend" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <strong>Others:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <h4 class="h4 noB">
                                                                <asp:RadioButton ID="rdbtnOthersYes" CssClass="noLM" Text="" GroupName="Others" runat="server"
                                                                    Checked="true" Enabled="false" /><span class="featNon">Yes</span>
                                                                <asp:RadioButton ID="rdbtnOthersNo" CssClass="noLM" Text="" GroupName="Others" runat="server"
                                                                    Enabled="false" /><span class="featNon">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Others notes::</strong><br />
                                                            <asp:TextBox ID="txtOthersNotes" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200 hid">
                                Survey<div class="close">
                                </div>
                            </h2>
                            <div class="hidden" id="feat">
                                <fieldset>
                                    <!-- <legend>Vehicle Features</legend>  -->
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Repeater ID="RepeaterSurveyEdit" runat="server" Visible="false">
                                                    <ItemTemplate>
                                                        <div>
                                                            <strong>
                                                                <asp:Label ID="lblSurveyQuestions" runat="server" Text='<%# Eval("SurveyQuestion") %>'></asp:Label></strong>
                                                            <asp:TextBox ID="txtSurveyQuestionAnswers" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="100%" Style="margin-bottom: 15px;" Enabled="false" Text='<%# Eval("SurveyQuestionAnswer") %>'></asp:TextBox>
                                                            <asp:HiddenField ID="hdnSurveyQuestionID" runat="server" Value='<%# Eval("SurveyQuestionID") %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200 hid">
                                Sale Notes
                                <div class="close">
                                </div>
                            </h2>
                            <div class="hidden">
                                <fieldset style="height: 80px;">
                                    <!-- <legend>Vehicle Description</legend>  -->
                                    <%--  <textarea style="width: 99%; height: 75px; resize: none;"></textarea>--%>
                                    <asp:TextBox ID="txtSaleNotes" runat="server" MaxLength="1000" Style="width: 99%;
                                        height: 75px; resize: none;" TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false"
                                        Enabled="false"></asp:TextBox>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200 hid">
                                Payment Details<div class="close">
                                </div>
                            </h2>
                            <div class="hidden">
                                <fieldset style="position: relative; z-index: 9; height: auto; padding-bottom: 20px;
                                    margin-bottom: 20px;">
                                    <!-- <legend>Payment Details</legend>   -->
                                    <table style="width: 99%; position: relative; z-index: 10; margin: 0">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="updtpnlPaymentDetails" runat="server">
                                                    <ContentTemplate>
                                                        <h4 class="h4 noB">
                                                            <strong style="width: 70px">Pay method:</strong><span style="font-weight: bold">
                                                                <asp:RadioButton ID="rdbtnPayVisa" CssClass="noLM" Text="Visa" Checked="true" GroupName="PayType"
                                                                    runat="server" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayMasterCard" CssClass="noLM" Text="Mastercard" GroupName="PayType"
                                                                    runat="server" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayDiscover" CssClass="noLM" Text="Discover" GroupName="PayType"
                                                                    runat="server" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayAmex" CssClass="noLM" Text="Amex" GroupName="PayType"
                                                                    runat="server" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnInvoice" CssClass="noLM" Text="Invoice" GroupName="PayType"
                                                                    runat="server" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayCheck" CssClass="noLM" Text="Check" GroupName="PayType"
                                                                    runat="server" Enabled="false" />
                                                        </h4>
                                                        <div class="h4 noB" style="left: 800px; top: -19px; display: inline-block; z-index: 11;
                                                            width: 200px; height: auto; padding: 0;">
                                                            <asp:LinkButton ID="lnkbtnPaymentHistory" runat="server" Text="Payment History" OnClick="lnkbtnPaymentHistory_Click"></asp:LinkButton>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <!-- Credit Card Start  -->
                                                        <div id="divcard" runat="server">
                                                            <table border="0" cellpadding="4" cellspacing="4" style="width: 55%; margin: 0 30px 0 0;
                                                                float: left;">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <h5 style="font-size: 15px; margin: 0; float: left; width: 130px;">
                                                                            Card Details</h5>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 135px">Card Holder
                                                                                First Name</strong>
                                                                            <asp:HiddenField ID="CardType" runat="server" />
                                                                            <asp:TextBox ID="txtCardholderName" runat="server" MaxLength="25" Enabled="false" />
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 65px">Last Name</strong>
                                                                            <asp:TextBox ID="txtCardholderLastName" runat="server" MaxLength="25" Enabled="false" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 77px">Credit Card
                                                                                #</strong>
                                                                            <asp:TextBox runat="server" ID="CardNumber" MaxLength="16" onkeypress="return isNumberKey(event)"
                                                                                onblur="return CreditCardOnblur();" Enabled="false" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 65px">Expiry Date</strong>
                                                                            <asp:TextBox ID="txtExpMon" runat="server" Enabled="false" Width="30px" />
                                                                            /
                                                                            <%--  <asp:DropDownList ID="CCExpiresYear" Style="width: 120px" runat="server" Enabled="false">
                                                                            </asp:DropDownList>--%>
                                                                            <asp:TextBox ID="txtCCExpiresYear" runat="server" Enabled="false" Width="120px" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">CVV#</strong>
                                                                            <asp:TextBox ID="cvv" MaxLength="4" runat="server" onkeypress="return isNumberKey(event)"
                                                                                onblur="return CVVOnblur();" Enabled="false" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 37%; margin: 0; float: right">
                                                                <%--  <tr>
                                                            <td colspan="2">
                                                                <h5 style="font-size: 15px; margin: 0; display: inline-block">
                                                                    Billing Address</h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 70px;">
                                                                <h4 class="h4">
                                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px">Name</strong>
                                                                    <asp:TextBox ID="txtBillingname" runat="server" MaxLength="30"></asp:TextBox>
                                                                </h4>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <h4 class="h4">
                                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px">Phone</strong>
                                                                    <asp:TextBox ID="txtbillingPhone" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                                        onblur="return billingPhoneOnblur();" onfocus="return billingPhoneOnfocus();"></asp:TextBox>
                                                                </h4>
                                                            </td>
                                                        </tr>--%>
                                                                <tr>
                                                                    <td colspan="2">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 45px">Address</strong>
                                                                            <asp:TextBox ID="txtbillingaddress" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">City</strong>
                                                                            <asp:TextBox ID="txtbillingcity" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 45%; display: inline-block; float: left; margin-right: 10px;">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">State</strong>
                                                                                <%--      <asp:DropDownList ID="ddlbillingstate" runat="server" Style="width: 120px">
                                                                                </asp:DropDownList>--%>
                                                                                <asp:TextBox ID="txtbillingstate" runat="server" Enabled="false" Width="120px"></asp:TextBox>
                                                                            </h4>
                                                                        </div>
                                                                        <div style="width: 45%; display: inline-block; float: left">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">ZIP</strong>
                                                                                <asp:TextBox ID="txtbillingzip" runat="server" Style="width: 74px" MaxLength="5"
                                                                                    class="sample4" onkeypress="return isNumberKey(event)" onblur="return billingZipOnblur();"
                                                                                    Enabled="false"></asp:TextBox>
                                                                            </h4>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <!-- Credit Card End  -->
                                                        <div class="clear">
                                                            &nbsp;</div>
                                                        <!-- check Start  -->
                                                        <div id="divcheck" runat="server" style="display: none;">
                                                            <table border="0" cellpadding="4" cellspacing="4" style="width: 98%; margin: 0;">
                                                                <tr>
                                                                    <td>
                                                                        <h5 style="display: inline-block; margin: 0; font-size: 15px;">
                                                                            Check Details</h5>
                                                                        <table style="width: 80%;">
                                                                            <tr>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 125px">Account holder
                                                                                            name</strong>
                                                                                        <asp:TextBox ID="txtCustNameForCheck" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 60px">Account #</strong>
                                                                                        <asp:TextBox ID="txtAccNumberForCheck" runat="server" MaxLength="20" Enabled="false"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <strong style="width: 67px">Bank name:</strong>
                                                                                        <asp:TextBox ID="txtBankNameForCheck" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 60px">Routing #</strong>
                                                                                        <asp:TextBox ID="txtRoutingNumberForCheck" runat="server" MaxLength="9" Enabled="false"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 76px">Account type</strong>
                                                                                        <asp:DropDownList ID="ddlAccType" runat="server" Enabled="false">
                                                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                                                            <asp:ListItem Value="1">CHECKING</asp:ListItem>
                                                                                            <asp:ListItem Value="2">SAVINGS</asp:ListItem>
                                                                                            <asp:ListItem Value="3">BUSINESSCHECKING</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </h4>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div style="width: 45%; display: inline-block; float: left; margin-right: 10px;">
                                                                        </div>
                                                                        <div style="width: 45%; display: inline-block; float: left">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <!-- check End  -->
                                                        <div class="clear">
                                                            &nbsp;</div>
                                                        <!-- paypal Start  -->
                                                        <div id="divpaypal" runat="server" style="display: none;">
                                                            <table border="0" cellpadding="4" cellspacing="4" style="width: 55%; margin: 0 30px 0 0;
                                                                float: left;">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <h5 style="font-size: 15px; margin: 0; float: left; width: 130px;">
                                                                            Invoice Details</h5>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 77px">Attention
                                                                                To </strong>
                                                                            <asp:TextBox runat="server" ID="txtAttentionTo" MaxLength="100" Enabled="false" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="star" style="color: Red">*</span><strong>Send the Invoice by</strong>
                                                                        <h4 class="h4 noB" style="margin-left: 80px; width: 430px;">
                                                                            <asp:RadioButton ID="rdbtnInvoiceEmail" CssClass="noLM" Style="width: 40px" Text=""
                                                                                GroupName="InvoiceSend" runat="server" Checked="true" Enabled="false" /><span class="featNon"
                                                                                    style="width: 50px;">Email</span> <strong></strong>
                                                                            <asp:TextBox ID="txtInvoiceEmail" runat="server" Style="border: #ccc 1px solid;"
                                                                                MaxLength="60" onblur="return InvoiceEmailOnblur();" Enabled="false"></asp:TextBox><br />
                                                                        </h4>
                                                                        <h4 class="h4 noB" style="margin-left: 80px; margin-top: 6px;">
                                                                            <asp:RadioButton ID="rdbtnInvoicePostal" Style="width: 40px" CssClass="noLM" Text=""
                                                                                GroupName="InvoiceSend" runat="server" Enabled="false" /><span class="featNon" style="display: inline">Postal</span>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 37%; margin: 0; float: right">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <h5 style="font-size: 15px; margin: 0; display: inline-block">
                                                                            Billing Address</h5>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 70px;">
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">Name</strong>
                                                                            <asp:TextBox ID="txtInvoiceBillingname" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 45px">Address</strong>
                                                                            <asp:TextBox ID="tyxtInvoiceAddress" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">City</strong>
                                                                            <asp:TextBox ID="txtInvoiceCity" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 45%; display: inline-block; float: left; margin-right: 10px;">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">State</strong>
                                                                                <asp:TextBox ID="txtInvoiceState" runat="server" Enabled="false" Width="120px"></asp:TextBox>
                                                                            </h4>
                                                                        </div>
                                                                        <div style="width: 45%; display: inline-block; float: left">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">ZIP</strong>
                                                                                <asp:TextBox ID="txtInvoiceZip" runat="server" Style="width: 74px" MaxLength="5"
                                                                                    class="sample4" onkeypress="return isNumberKey(event)" onblur="return billingZipOnblur();"
                                                                                    Enabled="false"></asp:TextBox>
                                                                            </h4>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <!-- paypal End  -->
                                                        <div class="clear">
                                                            &nbsp;</div>
                                                        <!-- Post Date End  -->
                                                        <!-- Post Date End  -->
                                                        <div class="clear">
                                                            &nbsp;</div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td colspan="2" style="padding: 10px 0 0 0">
                                                            <h5 style="font-size: 15px; margin: 0; padding: 0">
                                                                Payment Schedule</h5>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 9px;">
                                                        </td>
                                                        <td style="width: 130px; vertical-align: bottom">
                                                            <b>Schedule Payment </b>
                                                        </td>
                                                        <td style="width: 100px; vertical-align: bottom">
                                                            <h4 class="h4">
                                                                <strong style="width: 35px">Date</strong>
                                                                <asp:TextBox ID="txtPaymentDate" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td style="width: 100px; vertical-align: bottom">
                                                            <h4 class="h4 non">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 55px">Amount $</strong>
                                                                <asp:TextBox ID="txtPDAmountNow" runat="server" MaxLength="6" onkeypress="return isNumberKeyWithDot(event)"
                                                                    onkeyup="return ChangeValuesHidden()" Width="200px" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="40%">
                                                            <h4 class="h4">
                                                                <strong style="width: 175px; font-size: 15px;">Voice file confirmation #:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:TextBox ID="txtVoicefileConfirmNo" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td width="40%">
                                                            <h4 class="h4">
                                                                <strong style="width: 150px; font-size: 15px;">Voice file Location:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:TextBox ID="txtVoiceFileLocation" runat="server" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td style="padding: 0;">
                                                            <div style="width: 100px; float: right; margin: 0 auto 10px auto; clear: both; text-align: right">
                                                                <%--<input type="submit" name="btnSale" value="Process" onclick="return ValidateSavedData();"
                                                                    id="btnprocess" class="g-button g-button-submit">--%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="40%">
                                                            <h4 class="h4">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 175px; font-size: 15px;">Contract
                                                                    sign date :</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:TextBox ID="txtContractDate" runat="server" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                            </h4>
                                                        </td>
                                                        <td width="40%">
                                                            <h4 class="h4">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 150px; font-size: 15px;">
                                                                    Contract status:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:TextBox ID="txtContractStatus" runat="server" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td style="padding: 0;">
                                                            <div style="width: 100px; float: right; margin: 0 auto 10px auto; clear: both; text-align: right">
                                                                <%--<input type="submit" name="btnSale" value="Process" onclick="return ValidateSavedData();"
                                                                    id="btnprocess" class="g-button g-button-submit">--%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 0;">
                                                <div style="width: 500px; float: right; margin: 0 auto 10px auto; clear: both; text-align: right">
                                                    <%--<input type="submit" name="btnSale" value="Process" onclick="return ValidateSavedData();"
                                                                    id="btnprocess" class="g-button g-button-submit">--%>
                                                    <asp:DropDownList ID="ddlPmntStatus" runat="server" Font-Size="14px" Font-Bold="true"
                                                        ForeColor="#2B4BB1" onchange="return PayInfoChanges();" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Select Pmnt Status</asp:ListItem>
                                                        <asp:ListItem Value="3">Pending</asp:ListItem>
                                                        <asp:ListItem Value="1">FullyPaid</asp:ListItem>
                                                        <asp:ListItem Value="5">Returned</asp:ListItem>
                                                        <asp:ListItem Value="2">Reject</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                    <asp:Button ID="btnPmntUpdate" runat="server" CssClass="g-button g-button-submit"
                                                        Text="Pmnt Update" OnClientClick="return PmntValidation();" OnClick="btnPmntUpdate_Click" />
                                                    &nbsp;
                                                    <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="g-button g-button-submit"
                                                        Visible="true" Enabled="false" OnClick="btnProcess_Click" />
                                                    &nbsp;
                                                    <asp:Button ID="btnCheckProcess" runat="server" Text="Process" CssClass="g-button g-button-submit"
                                                        Visible="true" Enabled="false" OnClick="btnCheckProcess_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="divReason" runat="server" style="display: none;">
                                                <div style="width: 500px; float: right; margin: 0 auto 10px auto; clear: both; text-align: right">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 30%; padding-left: 10px;">
                                                                <b>Reason</b>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlPayCancelReason" runat="server" CssClass="input1" Font-Size="14px"
                                                                    Font-Bold="true" ForeColor="#2B4BB1">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200" style="margin-top: 0">
                                Payment Notes
                                <div class="close">
                                </div>
                            </h2>
                            <div class="hidden">
                                <fieldset style="height: 150px;">
                                    <!-- <legend>Vehicle Description</legend>  -->
                                    <%--  <textarea style="width: 99%; height: 75px; resize: none;"></textarea>--%>
                                    <asp:TextBox ID="txtPaymentNotes" runat="server" Style="width: 99%; height: 65px;
                                        resize: none;" TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false"
                                        Enabled="false"></asp:TextBox>
                                    <div style="height: 5px;">
                                        &nbsp;</div>
                                    <asp:TextBox ID="txtPaymentNotesNew" runat="server" MaxLength="1000" Style="width: 99%;
                                        height: 45px; resize: none;" TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false"></asp:TextBox>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200" style="margin-top: 0">
                                QC Notes
                                <div class="close">
                                </div>
                            </h2>
                            <div class="hidden">
                                <fieldset style="height: 150px;">
                                    <!-- <legend>Vehicle Description</legend>  -->
                                    <%--  <textarea style="width: 99%; height: 75px; resize: none;"></textarea>--%>
                                    <asp:TextBox ID="txtOldQcNotes" runat="server" Style="width: 99%; height: 65px; resize: none;"
                                        TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false" Enabled="false"></asp:TextBox>
                                    <div style="height: 5px;">
                                        &nbsp;</div>
                                    <asp:TextBox ID="txtQCNotes" runat="server" MaxLength="1000" Style="width: 99%; height: 45px;
                                        resize: none;" TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false"></asp:TextBox>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- <table width="100%">
            <tr>
                <td width="400px">
                    &nbsp;
                </td>
                <td style="text-align: right">
                    <asp:UpdatePanel ID="updatePnlSave2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSaleStatus2" runat="server" Font-Size="14px" Font-Bold="true"
                                ForeColor="#2B4BB1">
                                <asp:ListItem Value="1">Prospect</asp:ListItem>
                                <asp:ListItem Value="2">Promotional</asp:ListItem>
                                <asp:ListItem Value="3">Sale</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="btnSave2" runat="server" CssClass="g-button g-button-submit" Text="Save"
                                OnClick="btnSave2_Click" OnClientClick="return ValidateSavedData();"/>
                            &nbsp;
                            <asp:Button ID="btnAbandon2" runat="server" CssClass="g-button g-button-submit" Text="Abandon"
                                OnClick="btnAbandon_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <br />
                </td>
            </tr>
        </table>--%>
    </div>
    <asp:HiddenField ID="btnOpen" runat="server" />
    <cc1:ModalPopupExtender ID="MPEUpdate" runat="server" PopupControlID="tblUpdate"
        BackgroundCssClass="ModalPopupBG" TargetControlID="btnOpen" OkControlID="btnPhoneCancel">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 180px; margin-top: 70px; width: 400px">
            <h4>
                Enter Information
                <!-- <div class="cls">
            </div> -->
            </h4>
            <div class="dat" style="padding: 0 0 0 3; height: 120px;">
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
                                                        <td style="width: 120px;">
                                                            <b>Target Signup Date:</b> &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTargetDate" runat="server" onchange="ChangeValuesHidden()"
                                                                Width="120px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>Callback Date:</b> &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCallbackDate" runat="server" onchange="ChangeValuesHidden()"
                                                                Width="120px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
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
                            <table style="width: 100%;">
                                <tr align="center">
                                    <td colspan="4" style="padding-top: 15px;">
                                        <div style="width: 240px; margin: 0 auto;">
                                            <asp:Button ID="btnProspectSave" runat="server" Text="Save" CssClass="g-button g-button-submit"
                                                OnClientClick="return ValidatePhone();" OnClick="btnProspectSave_Click" />
                                            <asp:Button ID="btnPhoneCancel" runat="server" Text="Cancel" CssClass="g-button g-button-submit" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                OnClick="BtnClsUpdated_Click" />
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
            <asp:Button ID="btnYesUpdated" class="btn" runat="server" Text="Ok" OnClick="BtnClsUpdated_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mdepAlertRejectThere" runat="server" PopupControlID="divRejectThere"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnRejectThere" OkControlID="btnRejectThereNo">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnRejectThere" runat="server" />
    <div id="divRejectThere" class="alert" style="display: none">
        <h4 id="H7">
            Alert
            <%--<asp:Button ID="btnDiv" class="cls" runat="server" Text="" BorderWidth="0" />--%>
            <!-- <div class="cls">
            </div> -->
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
    <cc1:ModalPopupExtender ID="mdepalertMoveSmartz" runat="server" PopupControlID="divdraftPhone"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdndraftPhNo">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdndraftPhNo" runat="server" />
    <div id="divdraftPhone" class="alert" style="display: none">
        <h4 id="H1">
            Alert
            <%--<asp:Button ID="btnDiv" class="cls" runat="server" Text="" BorderWidth="0" />--%>
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
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
    <cc1:ModalPopupExtender ID="mpealteruser" runat="server" PopupControlID="AlertUser"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAlertuser">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAlertuser" runat="server" />
    <div id="AlertUser" class="alert" style="display: none">
        <h4 id="H4">
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
            <asp:Button ID="btnUpdate" class="btn" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mpeaSalesData" runat="server" PopupControlID="divViewregisterMail"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnViewregisterMail" CancelControlID="BtnClsSendRegMail">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnViewregisterMail" runat="server" />
    <div id="divViewregisterMail" class="PopUpHolder">
        <div class="main" style="width: 1120px">
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
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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
                                            <asp:UpdatePanel ID="UpdtpnlHeader" runat="server">
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
                                    <div style="width: 100%;" id="divCheckResults" runat="server">
                                        <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
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
                                                                Acc Type
                                                            </td>
                                                            <td width="130px" align="left">
                                                                Acc #
                                                            </td>
                                                            <td width="130px" align="left">
                                                                Acc Holder Name
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
                                            <asp:Panel ID="Panel2" Width="100%" runat="server">
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                    <ContentTemplate>
                                                        <input style="width: 91px" id="Hidden3" type="hidden" runat="server" enableviewstate="true" />
                                                        <input style="width: 40px" id="Hidden4" type="hidden" runat="server" enableviewstate="true" />
                                                        <asp:GridView Width="1070px" ID="grdCheckResults" runat="server" CellSpacing="0"
                                                            CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                            ShowHeader="false" OnRowDataBound="grdCheckResults_RowDataBound">
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
                                                                        <asp:Label ID="lblCheckTransDt" runat="server" Text='<%# Bind("PayTryDatetime", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransBy" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"AgentUserName"),11)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransAccType" runat="server" Text='<%# Eval("AccountType")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransAccNum" runat="server"></asp:Label>
                                                                        <asp:HiddenField ID="hdnCheckTransAccNum" runat="server" Value='<%# Eval("AccountNumber")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="130px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckAccHolderName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"AccountHolderName"),15)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="130px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransAddress" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"Address"),15)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransCity" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"City"),10)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransState" runat="server" Text='<%# Eval("State")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransZip" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"Zip"),5)%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="55px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransAmount" runat="server" Text='<%# Eval("Amount")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCheckTransResult" runat="server" Text='<%# Eval("Result")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdCheckResults" EventName="Sorting" />
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
    <cc1:ModalPopupExtender ID="mdepNoTransHis" runat="server" PopupControlID="divNoTransHIs"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnNoTransHis" OkControlID="btnNotransClose1"
        CancelControlID="btnNotransClose2">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnNoTransHis" runat="server" />
    <div id="divNoTransHIs" class="alert" style="display: none">
        <h4 id="H6">
            Alert
            <asp:Button ID="btnNotransClose1" class="cls" runat="server" Text="" BorderWidth="0" />
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblNotransError" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnNotransClose2" class="btn" runat="server" Text="Ok" />
        </div>
    </div>
    <asp:HiddenField ID="hdnEnterDEalerCode" runat="server" />
    <cc1:ModalPopupExtender ID="MdepEnterDEalerCode" runat="server" PopupControlID="divEnterDEalerCode"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnEnterDEalerCode" OkControlID="btnEnterDealerCodeCancel">
    </cc1:ModalPopupExtender>
    <div id="divEnterDEalerCode" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 180px; margin-top: 70px; width: 400px">
            <h4>
                Enter Dealer Code
                <!-- <div class="cls">
            </div> -->
            </h4>
            <div class="dat" style="padding: 0 0 0 3; height: 120px;">
                <table id="Table1" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 100%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                    <tr>
                                                        <td style="width: 120px;">
                                                            <b>Dealer Code:</b> &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDealerCode" runat="server" MaxLength="8"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
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
                            <table style="width: 100%;">
                                <tr align="center">
                                    <td colspan="4" style="padding-top: 15px;">
                                        <div style="width: 240px; margin: 0 auto;">
                                            <asp:Button ID="btnPopMovetoSmartz" runat="server" Text="Move to Smartz" CssClass="g-button g-button-submit"
                                                OnClientClick="return ValidateDealerCode();" OnClick="btnPopMovetoSmartz_Click" />
                                            <asp:Button ID="btnEnterDealerCodeCancel" runat="server" Text="Cancel" CssClass="g-button g-button-submit" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div class="clearFix">
                    &nbsp</div>
            </div>
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mdepDealerExists" runat="server" PopupControlID="divDealerExists"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnDealerExists">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnDealerExists" runat="server" />
    <div id="divDealerExists" class="alert" style="display: none">
        <h4 id="H5">
            Alert
            <asp:Button ID="btnDealerExistsClose" class="cls" runat="server" Text="" BorderWidth="0" OnClick="btnDealerExistsOK_Click"/>
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorDealerExists" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnDealerExistsOK" class="btn" runat="server" Text="Ok" OnClick="btnDealerExistsOK_Click" />
        </div>
    </div>
    </form>
</body>
</html>
