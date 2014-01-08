<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCDataView.aspx.cs" Inherits="QCDataView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>..:: Car Sales System ::..</title>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <link href="css/css.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript" src="js/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="js/emulatetab.joelpurra.js"></script>

    <script type="text/javascript" src="js/plusastab.joelpurra.js"></script>

    <script type='text/javascript' language="javascript" src='js/jquery.alphanumeric.pack.js'></script>

    <script src="js/jquery.formatCurrency-1.4.0.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">window.history.forward(1);</script>

    <script>
//<![CDATA[

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
  
    function ClosePopup9() {
            $find('<%= MdlPopAgentUpdate.ClientID%>').hide();
            return false;
        }
        
         function ClosePopup10() {
            $find('<%= MdlPopVerifier.ClientID%>').hide();
            return false;
        }
         function ClosePopup22() {
            $find('<%=  MdlPopLeadDetails.ClientID%>').hide();
            return false;
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
				if($('#ddlMake option:selected').val() != 0){
					str += $('#ddlMake option:selected').text()+"-";				
				}
				else
				{
				    str += "Unspecified"+"-";
				}
				if($('#ddlModel option:selected').val() != 0){
					str += $('#ddlModel option:selected').text()+"-";				
				}
				else
				{
				    str += "Unspecified"+"-";
				}
				if($('#ddlYear option:selected').val() != 0){
					str += $('#ddlYear option:selected').text()+"-";
				}
				else
				{
				    str += "Unspecified"+"-";
				}
				if($('#txtAskingPrice').val().length>0){
					str += "<span class='price11'>"+$('#txtAskingPrice').val()+"</span>-";
				}
				else
				{
				    str += "Unspecified"+"-";
				}				
				if($('#txtMileage').val().length>1){
					str += $('#txtMileage').val();
				}
				else
				{
				   str += "Unspecified";
				}	
				if(($('#ddlMake option:selected').val() == 0) &&($('#ddlModel option:selected').val() == 0)&&($('#ddlYear option:selected').val() == 0)&&($('#txtAskingPrice').val().length<1)&&($('#txtMileage').val().length<1))			
				{
				   str = "";
				}				
				if($('#Vinfo').next('div.hidden').is(':visible')){				
					$('#Vinfo label').empty().append(str);	
					$('.price11').formatCurrency();
				}else{
					$('#Vinfo label').empty()
				}	
			}
			$(this).next('div.hidden').slideToggle();
		});		
       
       
    }
    
    // Generating Unique Array 
    function unique1(list) {
      var result = [];
      $.each(list, function(i, e) {
        if ($.inArray(e, result) == -1) result.push(e);
      });
      return result;
    }
 //-------------------------- Agents Centers Info END ------------------------------------------
    </script>

    <script type="text/javascript" language="javascript">	
	
	$(function(){		    	
		$('.hid').next('div.hidden').hide();
		
		var str = '';
				if($('#ddlMake option:selected').val() != 0){
					str += $('#ddlMake option:selected').text()+"-";				
				}
				else
				{
				    str += "Unspecified"+"-";
				}
				if($('#ddlModel option:selected').val() != 0){
					str += $('#ddlModel option:selected').text()+"-";				
				}
				else
				{
				    str += "Unspecified"+"-";
				}
				if($('#ddlYear option:selected').val() != 0){
					str += $('#ddlYear option:selected').text()+"-";
				}
				else
				{
				    str += "Unspecified"+"-";
				}
				if($('#txtAskingPrice').val().length>0){
					str += "<span class='price11'>"+$('#txtAskingPrice').val()+"</span>-";
				}
				else
				{
				    str += "Unspecified"+"-";
				}				
				if($('#txtMileage').val().length>1){
					str += $('#txtMileage').val();
				}
				else
				{
				   str += "Unspecified";
				}	
				if(($('#ddlMake option:selected').val() == 0) &&($('#ddlModel option:selected').val() == 0)&&($('#ddlYear option:selected').val() == 0)&&($('#txtAskingPrice').val().length<1)&&($('#txtMileage').val().length<1))			
				{
				   str = "";
				}				
								
				$('#Vinfo label').empty().append(str);	
				$('.price11').formatCurrency();
				
		
		$('.sample4').numeric();	
		
		
		$('#txtAskingPrice').live('blur',function(){
		    if($('#txtAskingPrice').val().length >2){
		         $('#txtAskingPrice').formatCurrency({ symbol: '' });
		    } 
		});
		
		$('#txtAskingPrice').live('focus',function(){
		    if($('#txtAskingPrice').val().length >2){
		        var text = $('#txtAskingPrice').val();
		         //text = text.substring(1);
		         text = text.replace(',','');
		         $('#txtAskingPrice').val(text);
		           //alert(text)
		    }  
		    
		});
		
		$('#txtMileage').live('blur',function(){
		    if($('#txtMileage').val().length >0){
		         $('#txtMileage').formatCurrency({ symbol: '' });
		         $('#txtMileage').val($('#txtMileage').val()+' mi')
		    } 
		});
		
		$('#txtMileage').live('focus',function(){
		    if($('#txtMileage').val().length >0){
		        var text = $('#txtMileage').val();
		         //text = text.substring(1);
		         text = text.replace(' mi','');
		         text = text.replace(',','');
		         $('#txtMileage').val(text);
		           //alert(text)
		    }  
		    
		});
		
		
		
		
		
			
	})	
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

    function RejectQCValidation()
	{
        if (document.getElementById('<%= txtQCNotes.ClientID%>').value.trim().length < 1)
        {
        alert("Please enter qc notes"); 
        valid=false;
        document.getElementById('txtQCNotes').focus();  
        return valid;               
        }
	}
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
         function PayInfoChanges() {
	     debugger;      
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
        function poptastic2(url)
      {var carid=$.trim($('#lblSaleID').text());
    var ur = url+carid
	newwindow=window.open(ur,'name','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,height=600,width=540');
	    if (window.focus) {newwindow.focus()}
    }
    
    </script>

</head>
<body>
    <form id="form1" runat="server" data-plus-as-tab="true">
    <asp:ScriptManager ID="SM" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/WebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <asp:UpdateProgress ID="Progress" runat="server" AssociatedUpdatePanelID="updtpnlmain"
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
            <td style="width: 380px;">
                <a>
                    <img src="images/logo2.png" /></a>
            </td>
            <td>
                <h1 style="border-bottom: none; padding-top: 5px;">
                    UNITED CAR EXCHANGE <span>QC Data View</span></h1>
            </td>
            <td style="width: 380px; padding-top: 10px;">
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
    <div class="main">
        <asp:UpdatePanel ID="updtpnlmain" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <h4 class="h4">
                                            <strong style="width: 45px;">Sale ID:</strong>
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
                                    <td>
                                        <h4 class="h4">
                                            <strong style="width: 45px;">Agent:</strong>
                                            <%-- <input type="text" style="width: 245px" />--%>
                                            <asp:Label ID="lblSaleAgent" runat="server"></asp:Label>
                                            <asp:LinkButton id="lnkAgentUpdate" runat="server" Text="Agent" OnClick="lnkAgentUpdate_Click"></asp:LinkButton>
                                        </h4>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <h4 class="h4">
                                            <strong style="width: 45px;">Verifier:</strong>
                                            <%-- <input type="text" style="width: 245px" />--%>
                                            <asp:Label ID="lblVerifierName" runat="server"></asp:Label>
                                            <asp:LinkButton id="lnkVerifierName" runat="server" Text="Verifier" OnClick="lnkVerifierName_Click"></asp:LinkButton>
                                        </h4>
                                    </td>
                                    <td>
                                        <h4 class="h4">
                                            <strong style="width: 60px;">Location:</strong>
                                            <%-- <input type="text" style="width: 245px" />--%>
                                            <asp:Label ID="lblVerifierLocation" runat="server"></asp:Label>
                                        </h4>
                                    </td>
                                    <td>
                                        <h4 class="h4">
                                            <strong style="width: 65px;">QC Status:</strong>
                                            <%-- <input type="text" style="width: 245px" />--%>
                                            <asp:Label ID="lblQCStatus" runat="server"></asp:Label>
                                        </h4>
                                    </td>
                                    <td>
                                        <h4 class="h4">
                                            <strong style="width: 100px;">Payment Status:</strong>
                                            <%-- <input type="text" style="width: 245px" />--%>
                                            <asp:Label ID="lblPaymentStatusView" runat="server"></asp:Label>
                                        </h4>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="300px" >
                            <h4 class="h4">
                                <strong style="width: 60px;">Package:</strong>
                                <%-- <input type="text" style="width: 245px" />--%>
                                <%-- <asp:DropDownList ID="ddlPackage" runat="server" onchange="return OnchangeDropdown()"
                                    Enabled="false">
                                </asp:DropDownList>--%>
                                <asp:Label ID="lblPackage" runat="server"></asp:Label>&nbsp; <asp:Label ID="lbldiscountpacka" runat="server" Text="Discount"></asp:Label>
                            </h4>
                        </td>
                       
                        <td style="text-align: right">
                            <asp:UpdatePanel ID="updtpnlSave" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlQCStatus" Visible="false" runat="server" Font-Size="14px" Font-Bold="true"
                                        ForeColor="#2B4BB1">
                                        <asp:ListItem Value="0">Select QC Status</asp:ListItem>
                                        <asp:ListItem Value="1">QC Approved</asp:ListItem>
                                        <asp:ListItem Value="2">QC Reject</asp:ListItem>
                                        <asp:ListItem Value="3">QC Pending</asp:ListItem>
                                        <asp:ListItem Value="4">QC Returned</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;
                                  <%--  <asp:Button ID="btnQCUpdate" runat="server" CssClass="g-button g-button-submit" Text="QC Update"
                                        OnClientClick="return QCValidation();" OnClick="btnQCUpdate_Click" />--%>
                                    <asp:Button ID="btnQCUpdate" runat="server" CssClass="g-button g-button-submit" Text="QC Update"
                                          OnClientClick="poptastic2('QCCkeckList1.aspx?CarId=');" />
                                    &nbsp;
                                    <asp:Button ID="btnEdit" runat="server" CssClass="g-button g-button-submit" Text="Edit"
                                        OnClick="btnEdit_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnQCBack" runat="server" CssClass="g-button g-button-submit" Text="Close"
                                        OnClick="BtnClsUpdated_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnMovedToSmartz" runat="server" CssClass="g-button g-button-submit"
                                        Text="Move to Smartz" OnClick="MoveSmartz_Click" />
                                         <asp:Button ID="btnQcVeri" runat="server" CssClass="g-button g-button-submit"
                                        Text="QC Verifier" Visible="false" OnClientClick="poptastic2('QCCkeckList1.aspx?CarId=');" />
                                 
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table style="margin: 0 auto; width: 100%;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="5" style="padding-top: 0px;">
                            <h2 class="h200">
                                Seller Information</h2>
                            <fieldset>
                                <!-- <legend>Seller Information</legend>  -->
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <h4 class="h4">
                                                <span class="star" style="color: Red">*</span><strong style="width: 65px">First name:</strong>
                                                <%-- <input type="text" style="width: 380px" />--%>
                                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                            </h4>
                                        </td>
                                        <td>
                                            <h4 class="h4">
                                                <span class="star" style="color: Red">*</span> <strong style="width: 65px">Last name:</strong>
                                                <%--<input type="text" style="width: 380px" />--%>
                                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="30" Enabled="false"  ></asp:TextBox>
                                                <span><asp:LinkButton ID="lnkleadlines" text="LeadDetails" runat="server" OnClick="leaddetails_Click"></asp:LinkButton></span>
                                            </h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <h4 class="h4">
                                                            <span class="star" style="color: Red">*</span><strong style="width: 50px">Phone#:</strong>
                                                            <%-- <input type="text" style="width: 394px" />--%>
                                                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                                onblur="return PhoneOnblur();" onfocus="return PhoneOnfocus();" Enabled="false" style="width:100px"></asp:TextBox>
                                                                <asp:ImageButton id="PhoneMatch" runat="server" ImageUrl="images/icon-phone.png"  OnClick="PhoneMatch_Click"
                                                                    CommandName="GoogleAddressMatch" title="Google Search" />
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
                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">Email:</strong>
                                                            <%-- <input type="text" style="width: 406px" />--%>
                                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="60" onblur="return EmailOnblur();"
                                                                Enabled="false"></asp:TextBox>
                                                            <asp:CheckBox ID="chkbxEMailNA" runat="server" Text="NA" Font-Bold="true" Enabled="false" />
                                                        </h4>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <h4 class="h4">
                                                <span class="star" style="color: Red">*</span><strong style="width: 50px">Address:</strong>
                                                <%--<input type="text" style="width: 392px" />--%>
                                                <asp:TextBox ID="txtAddress" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                            </h4>
                                        </td>
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
                                                            <%--  <asp:DropDownList ID="ddlLocationState" runat="server" Style="width: 100px" Enabled="false">
                                                            </asp:DropDownList>--%>
                                                            <asp:TextBox ID="lblLocationState" runat="server" MaxLength="40" Enabled="false"></asp:TextBox>
                                                            <%--<asp:Label ID="lblLocationState" runat="server"></asp:Label>--%>
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
                            <h2 class="h200 hid" id="Vinfo">
                                Vehicle Information
                                <label class="selected">
                                </label>
                                <div class="close">
                                </div>
                            </h2>
                            <div class="hidden" id="infoV">
                                <fieldset>
                                    <!-- <legend>Vehicle Information</legend>  -->
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 330px;">
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Make:</strong>
                                                    <%--  <select style="width: 277px;">
                                                <option>Select</option>
                                            </select>--%>
                                                    <asp:UpdatePanel ID="updtMake" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlMake" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMake_SelectedIndexChanged"
                                                                onchange="ChangeValuesHidden()" Enabled="false">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                            <td style="width: 330px">
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Model:</strong>
                                                    <asp:UpdatePanel ID="updtpnlModel" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlModel" runat="server" onchange="ChangeValuesHidden()" Enabled="false">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Year:</strong>
                                                    <asp:DropDownList ID="ddlYear" runat="server" Enabled="false">
                                                    </asp:DropDownList>
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
                                                                <asp:TextBox ID="txtAskingPrice" runat="server" MaxLength="6" class="sample4" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td style="width: 190px">
                                                            <h4 class="h4">
                                                                <strong style="width: 40px">Mileage:</strong>
                                                                <%-- <input type="text" style="width: 119px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtMileage" runat="server" MaxLength="6" class="sample4" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td valign="bottom">
                                                            <h4 class="h4 noB">
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
                                                    <asp:TextBox ID="txtBodyStyle" runat="server" Enabled="false"></asp:TextBox>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 80px">Exterior color:</strong>
                                                    <%--<input type="text" style="width: 224px" />--%>
                                                    <%-- <asp:DropDownList ID="ddlExteriorColor" runat="server" onchange="ChangeValuesHidden()"
                                                        Enabled="false">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtExteriorColor" runat="server" Enabled="false"></asp:TextBox>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 80px">Interior color:</strong>
                                                    <%--<input type="text" style="width: 170px" />--%>
                                                    <%--<asp:DropDownList ID="ddlInteriorColor" runat="server" onchange="ChangeValuesHidden()"
                                                        Enabled="false">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtInteriorColor" runat="server" Enabled="false"></asp:TextBox>
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding: 0">
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <h4 class="h4 noB">
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
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <h4 class="h4">
                                                                <strong>VIN #:</strong>
                                                                <%--<input type="text" style="width: 409px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtVin" runat="server" Style="width: 409px" MaxLength="20" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <h4 class="h4 noB">
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200 hid">
                                Vehicle Features<div class="close">
                                </div>
                            </h2>
                            <div class="hidden" id="feat">
                                <fieldset>
                                    <!-- <legend>Vehicle Features</legend>  -->
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 90px;">
                                                <strong>Comfort:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <asp:CheckBox ID="chkFeatures51" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">A/C</span>
                                                <asp:CheckBox ID="chkFeatures1" runat="server" Enabled="false" class="noLM" />
                                                <span class="featNon">A/C: Front</span>
                                                <asp:CheckBox ID="chkFeatures2" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">A/C: Rear</span>
                                                <asp:CheckBox ID="chkFeatures3" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Cruise control</span>
                                                <asp:CheckBox ID="chkFeatures4" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Navigation system</span>
                                                <asp:CheckBox ID="chkFeatures5" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Power locks</span>
                                                <asp:CheckBox ID="chkFeatures6" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Power steering</span>
                                                <br />
                                                <asp:CheckBox ID="chkFeatures7" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Remote keyless entry</span>
                                                <asp:CheckBox ID="chkFeatures8" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">TV/VCR</span>
                                                <asp:CheckBox ID="chkFeatures31" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Remote start</span>
                                                <asp:CheckBox ID="chkFeatures33" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Tilt</span>
                                                <asp:CheckBox ID="chkFeatures35" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Rearview camera</span>
                                                <asp:CheckBox ID="chkFeatures36" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Power mirrors</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Seats:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <%-- <input type="checkbox" class="noLM" />
                                        Bucket seats
                                        <input type="checkbox" />
                                        Leather interior
                                        <input type="checkbox" />
                                        Memory seats
                                        <input type="checkbox" />
                                        Power seats
                                        <input type="checkbox" />
                                        Heated seats
                                        <input type="checkbox" />
                                        Vinyl interior
                                        <input type="checkbox" />
                                        Cloth interior--%>
                                                <asp:CheckBox ID="chkFeatures9" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Bucket seats</span>
                                                <asp:CheckBox ID="chkFeatures11" runat="server" Enabled="false" />
                                                <span class="featNon">Memory seats</span>
                                                <asp:CheckBox ID="chkFeatures12" runat="server" Enabled="false" />
                                                <span class="featNon">Power seats</span>
                                                <asp:CheckBox ID="chkFeatures32" runat="server" Enabled="false" />
                                                <span class="featNon">Heated seats</span>
                                                <br />
                                                <b style="color: #222; display: inline-block; margin-left: 163px;">Interior</b>
                                                :
                                                <asp:RadioButton ID="rdbtnLeather" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" Enabled="false" /><span class="featNon">Leather</span>
                                                <asp:RadioButton ID="rdbtnVinyl" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" Enabled="false" /><span class="featNon">Vinyl</span>
                                                <asp:RadioButton ID="rdbtnCloth" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" Enabled="false" /><span class="featNon">Cloth</span>
                                                <asp:RadioButton ID="rdbtnInteriorNA" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" Checked="true" Enabled="false" /><span class="featNon">NA</span>
                                                <%--<asp:CheckBox ID="chkFeatures10" runat="server" class="noLM" />
                                        Leather interior
                                        <asp:CheckBox ID="chkFeatures37" runat="server" class="noLM" />
                                        Vinyl interior
                                        <asp:CheckBox ID="chkFeatures38" runat="server" class="noLM" />
                                        Cloth interior--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Safety:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <%-- <input type="checkbox" class="noLM" />
                                        Airbag: Driver
                                        <input type="checkbox" />
                                        Airbag: Passenger
                                        <input type="checkbox" />
                                        Airbag: Side
                                        <input type="checkbox" />
                                        Alarm
                                        <input type="checkbox" />
                                        Anti-lock brakes
                                        <input type="checkbox" />
                                        Fog lights
                                        <input type="checkbox" />
                                        Power brakes--%>
                                                <asp:CheckBox ID="chkFeatures13" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Airbag: Driver</span>
                                                <asp:CheckBox ID="chkFeatures14" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Airbag: Passenger</span>
                                                <asp:CheckBox ID="chkFeatures15" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Airbag: Side</span>
                                                <asp:CheckBox ID="chkFeatures16" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Alarm</span>
                                                <asp:CheckBox ID="chkFeatures17" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Anti-lock brakes</span>
                                                <asp:CheckBox ID="chkFeatures18" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Fog lights</span>
                                                <asp:CheckBox ID="chkFeatures39" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Power brakes</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Sound System:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <%--<input type="checkbox" class="noLM" />
                                        Cassette radio
                                        <input type="checkbox" />
                                        CD changer
                                        <input type="checkbox" />
                                        CD player
                                        <input type="checkbox" />
                                        Premium sound
                                        <input type="checkbox" />
                                        AM-FM
                                        <input type="checkbox" />
                                        DVD--%>
                                                <asp:CheckBox ID="chkFeatures19" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Cassette radio</span>
                                                <asp:CheckBox ID="chkFeatures20" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">CD changer</span>
                                                <asp:CheckBox ID="chkFeatures21" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">CD player</span>
                                                <asp:CheckBox ID="chkFeatures22" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Premium sound</span>
                                                <asp:CheckBox ID="chkFeatures34" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">AM/FM</span>
                                                <asp:CheckBox ID="chkFeatures40" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">DVD</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>New:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <%--<input type="checkbox" class="noLM" />
                                        Battery
                                        <input type="checkbox" />
                                        Tires
                                        <input type="checkbox" />
                                        Rotors
                                        <input type="checkbox" />
                                        Brakes--%>
                                                <asp:CheckBox ID="chkFeatures44" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Battery</span>
                                                <asp:CheckBox ID="chkFeatures45" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Tires</span>
                                                <asp:CheckBox ID="chkFeatures52" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Rotors</span>
                                                <asp:CheckBox ID="chkFeatures53" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Brakes</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Windows:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <%--<input type="checkbox" class="noLM" />
                                        Power windows
                                        <input type="checkbox" />
                                        Rear window defroster
                                        <input type="checkbox" />
                                        Rear window wiper
                                        <input type="checkbox" />
                                        Tinted glass--%>
                                                <asp:CheckBox ID="chkFeatures23" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Power windows</span>
                                                <asp:CheckBox ID="chkFeatures24" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Rear window defroster</span>
                                                <asp:CheckBox ID="chkFeatures25" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Rear window wiper</span>
                                                <asp:CheckBox ID="chkFeatures26" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Tinted glass</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Others:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <%--<input type="checkbox" class="noLM" />
                                        Alloy wheels
                                        <input type="checkbox" />
                                        Sunroof
                                        <input type="checkbox" />
                                        Panoramic roof
                                        <input type="checkbox" />
                                        Moon roof
                                        <input type="checkbox" />
                                        Third row seats
                                        <input type="checkbox" />
                                        Tow package
                                        <input type="checkbox" />
                                        Dashboard wood--%>
                                                <asp:CheckBox ID="chkFeatures27" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Alloy wheels</span>
                                                <asp:CheckBox ID="chkFeatures28" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Sunroof</span>
                                                <asp:CheckBox ID="chkFeatures41" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Panoramic roof</span>
                                                <asp:CheckBox ID="chkFeatures42" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Moonroof</span>
                                                <asp:CheckBox ID="chkFeatures29" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Third row seats</span>
                                                <asp:CheckBox ID="chkFeatures30" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Tow package</span>
                                                <br />
                                                <asp:CheckBox ID="chkFeatures43" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Dashboard wood frame</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="fDevider">
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Specials:</strong>
                                            </td>
                                            <td style="font-weight: bold; color: #666;">
                                                <%-- <input type="checkbox" class="noLM" />
                                        Garage kept
                                        <input type="checkbox" />
                                        Non smoking
                                        <input type="checkbox" />
                                        Records & receipts kept
                                        <input type="checkbox" />
                                        Well maintained
                                        <input type="checkbox" />
                                        Regular oil changes--%>
                                                <asp:CheckBox ID="chkFeatures46" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Garage kept</span>
                                                <asp:CheckBox ID="chkFeatures47" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Non smoking</span>
                                                <asp:CheckBox ID="chkFeatures48" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Records/Receipts kept</span>
                                                <asp:CheckBox ID="chkFeatures49" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Well maintained</span>
                                                <asp:CheckBox ID="chkFeatures50" runat="server" class="noLM" Enabled="false" />
                                                <span class="featNon">Regular oil changes</span>
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
                                Vehicle Description
                                <div class="close">
                                </div>
                            </h2>
                            <div class="hidden">
                                <fieldset style="height: 80px;">
                                    <!-- <legend>Vehicle Description</legend>  -->
                                    <%--  <textarea style="width: 99%; height: 75px; resize: none;"></textarea>--%>
                                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="1000" Style="width: 99%;
                                        height: 75px; resize: none;" TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false"
                                        Enabled="false"></asp:TextBox>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding-top: 5px;">
                            <h2 class="h200 hid">
                                Sale Notes<div class="close">
                                </div>
                            </h2>
                            <div class="hidden" style="height: 142px; max-height: 142px;">
                                <fieldset style="width: 70%; float: left; height: 110px;">
                                    <!-- <legend>Sale Notes</legend>  -->
                                    <%--<textarea style="width: 99%; height: 75px; resize: none;"></textarea>--%>
                                    <asp:TextBox ID="txtSaleNotes" runat="server" TextMode="MultiLine" MaxLength="1000"
                                        Style="width: 99%; height: 105px; resize: none;" CssClass="textAr" data-plus-as-tab="false"
                                        Enabled="false"> </asp:TextBox>
                                </fieldset>
                                <table class="coll">
                                    <tr>
                                        <%--<td>
                                    <input type="checkbox">
                                </td>
                                <td>
                                    Take images from Craigslist
                                </td>--%>
                                        <td>
                                            <strong>Source of photos </strong>
                                            <br />
                                            <h4 class="h4">
                                                <%--<asp:DropDownList ID="ddlPhotosSource" runat="server" onchange="ChangeValuesHidden()"
                                                    Enabled="false">
                                                </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtPhotosSource" runat="server" Enabled="false"></asp:TextBox>
                                            </h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td>
                                    <input type="checkbox">
                                </td>
                                <td>
                                    Take description from Craigslist
                                </td>--%>
                                        <td>
                                            <strong>Source of description</strong>
                                            <br />
                                            <h4 class="h4">
                                                <%-- <asp:DropDownList ID="ddlDescriptionSource" runat="server" Enabled="false">
                                                </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtDescriptionSource" runat="server" Enabled="false"></asp:TextBox>
                                            </h4>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear">
                                    &nbsp;</div>
                            </div>
                            <div class="clear">
                                &nbsp;</div>
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
                                                                    runat="server" AutoPostBack="true" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayMasterCard" CssClass="noLM" Text="Mastercard" GroupName="PayType"
                                                                    runat="server" AutoPostBack="true" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayDiscover" CssClass="noLM" Text="Discover" GroupName="PayType"
                                                                    runat="server" AutoPostBack="true" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayAmex" CssClass="noLM" Text="Amex" GroupName="PayType"
                                                                    runat="server" AutoPostBack="true" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayPaypal" CssClass="noLM" Text="Paypal" GroupName="PayType"
                                                                    runat="server" AutoPostBack="true" Enabled="false" />
                                                                <asp:RadioButton ID="rdbtnPayCheck" CssClass="noLM" Text="Check" GroupName="PayType"
                                                                    runat="server" AutoPostBack="true" Enabled="false" />
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
                                                                        <h5 style="font-size: 12px; font-weight: normal; margin: 0; display: inline-block">
                                                                            <asp:LinkButton ID="lnkbtnCopySellerInfo" runat="server" Text="Copy name & address from Seller Information"
                                                                                OnClientClick="return CopySellerInfo();" Style="color: Blue; text-decoration: underline;"
                                                                                Enabled="false"></asp:LinkButton>
                                                                        </h5>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 135px">Card Holder
                                                                                First Name</strong>
                                                                            <asp:HiddenField ID="CardType" runat="server" />
                                                                            <asp:TextBox ID="txtCardholderName" runat="server" MaxLength="25" Enabled="false" />
                                                                            <strong style="width: 65px">Last Name</strong>
                                                                            <asp:TextBox ID="txtCardholderLastName" runat="server" MaxLength="25" Enabled="false" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 85px">Credit Card
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
                                                                            <%-- <asp:DropDownList ID="ExpMon" Style="width: 130px;" runat="server" Enabled="false">
                                                                                <asp:ListItem Value="0" Text="Select Month"></asp:ListItem>
                                                                                <asp:ListItem Value="01" Text="01"></asp:ListItem>
                                                                                <asp:ListItem Value="02" Text="02"></asp:ListItem>
                                                                                <asp:ListItem Value="03" Text="03"></asp:ListItem>
                                                                                <asp:ListItem Value="04" Text="04"></asp:ListItem>
                                                                                <asp:ListItem Value="05" Text="05"></asp:ListItem>
                                                                                <asp:ListItem Value="06" Text="06"></asp:ListItem>
                                                                                <asp:ListItem Value="07" Text="07"></asp:ListItem>
                                                                                <asp:ListItem Value="08" Text="08"></asp:ListItem>
                                                                                <asp:ListItem Value="09" Text="09"></asp:ListItem>
                                                                                <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                                                                <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                                                                <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                                                            </asp:DropDownList>--%>
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
                                                                           &nbsp;&nbsp; <asp:ImageButton id="AddressMatch" runat="server" ImageUrl="images/icon-directory.png"  OnClick="AddressMatch_Click"
                                                                    CommandName="AddressMatch" title="Address Search" />
                                                                            <asp:TextBox ID="txtbillingaddress" runat="server" MaxLength="40" Enabled="false" Width="250px" ></asp:TextBox>
                                                                            
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">City</strong>
                                                                              <asp:ImageButton id="GoogleAddressMatch" runat="server" ImageUrl="images/icon-google_location.png" OnClick="GoogleAddressMatch_Click"
                                                                    CommandName="GoogleAddressMatch" title="Google Search" />&nbsp;
                                                                            <asp:TextBox ID="txtbillingcity" runat="server" MaxLength="40" Enabled="false" Width="43%"></asp:TextBox>
                                                                            
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 45%; display: inline-block; float: left; margin-right: 10px;">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">State</strong>
                                                                                <%--<asp:DropDownList ID="ddlbillingstate" runat="server" Style="width: 120px" Enabled="false">
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
                                                                        <h5 style="font-size: 12px; font-weight: normal; margin: 0; display: inline-block">
                                                                            <asp:LinkButton ID="lnkbtnCopyCheckName" runat="server" Text="Copy name from Seller Information"
                                                                                OnClientClick="return CopySellerInfoForCheck();" Style="color: Blue; text-decoration: underline;"
                                                                                Enabled="false"></asp:LinkButton>
                                                                        </h5>
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
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h5 style="display: inline-block; margin: 0; font-size: 15px;">
                                                                            Paypal Details</h5>
                                                                        <div id="Div1" runat="server" style="width: 80%;">
                                                                            <table>
                                                                                <%-- <tr>
                                                                            <td>
                                                                                <h4 class="h4">
                                                                                    <span class="star" style="color: Red">*</span><strong style="width: 90px">Payment date:</strong>
                                                                                  
                                                                                    <asp:DropDownList ID="ddlPaymentdate" runat="server" onchange="ChangeValuesHidden()"
                                                                                        Width="195px">
                                                                                    </asp:DropDownList>
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4 class="h4">
                                                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px">Amount:</strong>
                                                                                  
                                                                                    <asp:TextBox ID="txtPayAmount" runat="server" MaxLength="6" onkeypress="return isNumberKeyWithDot(event)"
                                                                                        onkeyup="return ChangeValuesHidden()"></asp:TextBox>
                                                                                </h4>
                                                                            </td>
                                                                        </tr>--%>
                                                                                <tr>
                                                                                    <td>
                                                                                        <h4 class="h4">
                                                                                            <span class="star" style="color: Red">*</span><strong style="width: 100px">Payment trans
                                                                                                ID:</strong>
                                                                                            <%-- <input type="text" style="width: 245px" />--%>
                                                                                            <asp:TextBox ID="txtPaytransID" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                                                                        </h4>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <h4 class="h4">
                                                                                            <span class="star" style="color: Red">*</span><strong style="width: 140px">Paypal account
                                                                                                email:</strong>
                                                                                            <%-- <input type="text" style="width: 245px" />--%>
                                                                                            <asp:TextBox ID="txtpayPalEmailAccount" runat="server" onblur="return PaypalEmailblur();"
                                                                                                Enabled="false"></asp:TextBox>
                                                                                        </h4>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="clear">
                                                                            &nbsp;</div>
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
                                                            <b>Today's Payment </b>
                                                        </td>
                                                        <td style="width: 100px; vertical-align: bottom">
                                                            <h4 class="h4">
                                                                <strong style="width: 35px">Date</strong>
                                                                <asp:TextBox ID="txtPaymentDate" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td style="width: 100px; vertical-align: bottom">
                                                            <h4 class="h4 non">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 55px">Amount $</strong>
                                                                <asp:TextBox ID="txtPDAmountNow" runat="server" MaxLength="6" onkeypress="return isNumberKeyWithDot(event)"
                                                                    onkeyup="return ChangeValuesHidden()" Enabled="false" Width="200px"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 0; margin: 0; vertical-align: middle">
                                                            <asp:CheckBox ID="chkboxlstPDsale" runat="server" CssClass="noLM" onclick="return OnchangeDropdown()"
                                                                Enabled="false" />
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <b>Post Dated Payment</b>
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <h4 class="h4 ">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 35px">Date</strong>
                                                                <asp:TextBox ID="txtPDDate" runat="server" ReadOnly="true" Enabled="false" ForeColor="Red"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <h4 class="h4 non">
                                                                <strong style="width: 55px">Amount $</strong>
                                                                <asp:TextBox ID="txtPDAmountLater" runat="server" MaxLength="6" onkeypress="return isNumberKeyWithDot(event)"
                                                                    onkeyup="return ChangeValuesHidden()" ReadOnly="true" Enabled="false" Width="200px"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <h4 class="h4">
                                                                <strong style="width: 40px">Total $</strong>
                                                                <asp:TextBox ID="txtTotalAmount" ReadOnly="true" runat="server" Enabled="false"></asp:TextBox>
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
                                                        <td width="35%">
                                                            <h4 class="h4">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 175px; font-size: 15px;">Voice
                                                                    file confirmation #:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:TextBox ID="txtVoicefileConfirmNo" runat="server" MaxLength="30" Enabled="false"></asp:TextBox>
                                                            </h4>
                                                        </td>
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
                                                        <td width="100%" colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td width="35%">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 150px; font-size: 15px;">
                                                                                            Voice file Location:</strong>
                                                                                        <%-- <input type="text" style="width: 245px" />--%>
                                                                                        <%-- <asp:DropDownList ID="ddlVoiceFileLocation" runat="server" Enabled="false">
                                                                                        </asp:DropDownList>--%>
                                                                                        <asp:TextBox ID="txtVoiceFileLocation" runat="server" Enabled="false"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td id="divReason" runat="server" style="display: none;">
                                                                        <div style="width: 500px; float: right; margin: 0 auto 10px auto; clear: both; text-align: right">
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td style="width: 21%; padding-left: 110px;">
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
                <table width="100%">
                    <tr>
                        <td width="400px">
                            &nbsp;
                        </td>
                        <td style="text-align: right">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <%-- <asp:DropDownList ID="ddlQCStatus" runat="server">
                                        <asp:ListItem Value="0">Select QC Status</asp:ListItem>
                                        <asp:ListItem Value="1">QC Pass</asp:ListItem>
                                        <asp:ListItem Value="2">QC Fail</asp:ListItem>
                                        <asp:ListItem Value="3">QC Pending</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Button ID="btnQCUpdate" runat="server" CssClass="g-button g-button-submit" Text="QC Update"
                                        OnClientClick="return QCValidation();" />
                                    &nbsp;
                                    <asp:Button ID="btnEdit2" runat="server" CssClass="g-button g-button-submit" Text="Edit"
                                        OnClick="btnEdit_Click" />
                                    &nbsp;
                                    <%--<asp:Button ID="btnQCPass2" runat="server" CssClass="g-button g-button-submit" Text="QC Pass"
                                        OnClick="btnQCPass_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnQCReject2" runat="server" CssClass="g-button g-button-submit"
                                        Text="QC Fail" OnClick="btnQCFail_Click" OnClientClick="return RejectQCValidation();" />
                                    &nbsp;
                                    <asp:Button ID="btnQCPending2" runat="server" CssClass="g-button g-button-submit"
                                        Text="QC Pending" OnClick="btnQCPending_Click" />
                                    &nbsp;--%>
                                    <%--<asp:Button ID="btnQCBack2" runat="server" CssClass="g-button g-button-submit" Text="Close"
                                        OnClick="BtnClsUpdated_Click" />--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
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
            <asp:Button ID="btnUpdate" class="btn" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        </div>
    </div>
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
    <cc1:ModalPopupExtender ID="mdepAlertExists" runat="server" PopupControlID="divExists"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnExists">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnExists" runat="server" />
    <div id="divExists" class="alert" style="display: none">
        <h4 id="H4">
            Alert
            <asp:Button ID="btnExustCls" class="cls" runat="server" Text="" BorderWidth="0" OnClick="btnNo_Click" />
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorExists" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnOk" class="btn" runat="server" Text="Ok" OnClick="btnNo_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mpealteruserUpdatedSmartz" runat="server" PopupControlID="AlertUserUpdatedSmartz"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAlertuserUpdatedSmartz">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAlertuserUpdatedSmartz" runat="server" />
    <div id="AlertUserUpdatedSmartz" class="alert" style="display: none">
        <h4 id="H5">
            Alert
            <asp:Button ID="btnYesUpdatedSmartz1" class="cls" runat="server" Text="" BorderWidth="0"
                OnClick="btnYesUpdatedSmartz1_Click" />
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErrUpdatedSmartz" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnYesUpdatedSmartz2" class="btn" runat="server" Text="Ok" OnClick="btnYesUpdatedSmartz1_Click" />
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
     <cc1:ModalPopupExtender ID="MdepAddAnotherCarAlert" runat="server" PopupControlID="divAddAnotherCarAlert"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAddAnotherCarAlert" OkControlID="btnAddAnotherCarNo">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAddAnotherCarAlert" runat="server" />
    <div id="divAddAnotherCarAlert" class="alert" style="display: none">
        <h4 id="H8">
            Alert
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblAddAnotherCarAlertError" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnAddAnotherCarNo" class="btn" runat="server" Text="No" />&nbsp;
            <asp:Button ID="btnAddAnotherCarYes" class="btn" runat="server" Text="Yes" OnClick="btnAddAnotherCarYes_Click" />
        </div>
    </div>
    
    <!-- Agent Update -->
     <cc1:ModalPopupExtender ID="MdlPopAgentUpdate" runat="server" PopupControlID="tblUpdate13"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lnkAgentUpdate1">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="lnkAgentUpdate1" runat="server" />
    <div id="tblUpdate13" class="PopUpHolder" style="display: none;"  >
        <div class="main" style="width: 350px" >
            <h4>
               Agent Update
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
                                              <b> SaleId:</b></td>
                                               <td> <asp:Label ID="qccarid" runat="server" ></asp:Label></td>
                                               </tr>
                                               <tr>
                                               <td>
                                               <b> Agent: </b>
                                                </td>
                                                <td><asp:Label ID="lblactualAgent" runat="server" ></asp:Label></td></tr>
                                               </td>
                                               </tr>
                                                    <tr>
                                                    <td>
                                                  Change Agent </td>
                                                  <td>
                                                  <asp:DropDownList ID="ddlAgentUpdate" runat="server" Font-Size="14px" Font-Bold="true"
                                                    ForeColor="#2B4BB1">
                                                    </asp:DropDownList>
                                                    </td>
                                                    </tr>
                                                
                                                     <tr>
                                                     
                                                     <td>
                                                         <asp:Button ID="AgentUdfate" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Update"  OnClick="AgentUdfate_Click" />
                                                        </td>
                                                        <td>
                                                         <asp:Button ID="Button7" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup9();"/>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td><br /></td>
                                                        <td><br /></td>
                                                        </tr>
                                                        
                                                        <tr>
                                                       <td><br /></td>
                                                       <td><br /></td>
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
            <div class="clear">
                &nbsp;</div>
        </div>
        <div class="clear">
            &nbsp;</div>
    </div>
    
    
    
     <!-- Verifier Update -->
     <cc1:ModalPopupExtender ID="MdlPopVerifier" runat="server" PopupControlID="tblUpdate14"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lnkVerifierName1">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="lnkVerifierName1" runat="server" />
    <div id="tblUpdate14" class="PopUpHolder" style="display: none;"  >
        <div class="main" style="width: 350px" >
            <h4>
               Agent Update
            </h4>
      
            <div class="dat" style="padding: 0 0 0 6; overflow: scroll; height: 280px; width:300px;">
                <table id="Table1" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 90%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                <tr><td>
                                              <b> SaleId:</b></td>
                                               <td> <asp:Label ID="lblVerSaleId" runat="server" ></asp:Label></td>
                                               </tr>
                                               <tr>
                                               <td>
                                               <b> Verifier: </b>
                                                </td>
                                                <td><asp:Label ID="lblVerName" runat="server" ></asp:Label></td></tr>
                                               </td>
                                               </tr>
                                                    <tr>
                                                    <td>
                                                  Change Verifier </td>
                                                  <td>
                                                  <asp:DropDownList ID="ddlVerifierUpdate" runat="server" Font-Size="14px" Font-Bold="true"
                                                    ForeColor="#2B4BB1">
                                                    </asp:DropDownList>
                                                    </td>
                                                    </tr>
                                                
                                                     <tr>
                                                     
                                                     <td>
                                                         <asp:Button ID="VUpdate" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Update"  OnClick="VUpdate_Click" />
                                                        </td>
                                                        <td>
                                                         <asp:Button ID="Button2" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Close" OnClientClick="return ClosePopup10();"/>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td><br /></td>
                                                        <td><br /></td>
                                                        </tr>
                                                        
                                                        <tr>
                                                       <td><br /></td>
                                                       <td><br /></td>
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
            <div class="clear">
                &nbsp;</div>
        </div>
        <div class="clear">
            &nbsp;</div>
    </div>
    
     <!-- Leads Details form -->
   <cc1:ModalPopupExtender ID="MDLPOPLeadsPhn" runat="server" PopupControlID="tblUpdate61"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnLeadsPhn"  >
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="HdnLeadsPhn" runat="server" />
    <div id="tblUpdate61" class="PopUpHolder" style="display: none;"  >
        <div class="main" style="height: 160px; margin-top: 70px; width: 250px">
            <h4>
            Enter Phone Number
                <!-- <div class="cls">
            </div> -->
            </h4>
            <div class="dat" style="padding: 0 0 0 3; height: 120px;">
                <table id="Table7" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 100%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                    <tr>
                                                        <td style="width: 100px;">
                                                            <b>Phone #:</b> &nbsp;
                                                        </td>
                                                        <td style="width: 200px;">
                                                            <asp:TextBox ID="txtLoadPhone" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox>
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
                            <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr align="center">
                                            <td colspan="4" style="padding-top: 15px;">
                                                <div style="width: 240px; margin: 0 auto;">
                                                    <asp:Button ID="btnPhoneOk" runat="server" Text="Ok" CssClass="g-button g-button-submit"
                                                      OnClick="btnPhoneOk_Click"  OnClientClick="return ValidatePhone();"  />
                                                       <asp:Button ID="Button8" CssClass="g-button g-button-submit" runat="server"
                                                   Text="Close" OnClientClick="return ClosePopup21();"/>
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
   
   <!-- end -->
      <!-- Leads Se4arch form -->
   <cc1:ModalPopupExtender ID="MdlPopLeadDetails" runat="server" PopupControlID="tblUpdate62"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnLeadDetails"  >
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnLeadDetails" runat="server" />
    <div id="tblUpdate62" class="PopUpHolder" style="display: none;"  >
        <div class="main" style="height: 600px; margin-top: 70px; width: 750px">
            <h4>
               Lead Details Form
                <!-- <div class="cls">
            </div> -->
            </h4>
            <div class="dat" style="padding: 0 0 0 3; height: 120px;">
                <table id="Table11" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 100%; margin: 0 auto;">
                    <tr>
                        <td style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel42" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                    <tr>
                                                        <td style="width: 200px;">
                                                            <b>Phone #:</b> <asp:label ID="txtLeadPhnDeta" runat="server" MaxLength="10" ></asp:label>
                                                        </td>
                                                    </tr>
                                                  <tr>
                                                   <td style="width: 200px;">
                                                            <b>Lead Date :</b> <asp:label ID="lblLeaddate" runat="server" MaxLength="10" ></asp:label>
                                                        </td>
                                                  </tr>
                                                     <tr>
                                                       <td style="width: 200px;">
                                                            <b>Price:</b> <asp:label ID="lblLeadPrice" runat="server" MaxLength="10" ></asp:label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                      <td style="width: 200px;">
                                                            <b>Model:</b> <asp:label ID="lblLeadModel" runat="server" MaxLength="10" ></asp:label>
                                                        </td>
                                                    </tr>
                                                      <tr>
                                                       <td style="width: 200px;">
                                                            <b>State:</b> <asp:label ID="lblLeadState" runat="server" MaxLength="10" ></asp:label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                       <td style="width: 200px;">
                                                            <b>City:</b> <asp:label ID="lblLeadCity" runat="server" MaxLength="10" ></asp:label>
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                       <td style="width: 200px;" colspan="2">
                                                            <b> CusEmailId:</b> <asp:label ID="lblLeadEmail" runat="server" MaxLength="10" ></asp:label>
                                                        </td>
                                                         
                                                    </tr>
                                                      <tr>
                                                       <td style="width: 200px;" colspan="2">
                                                            <b> Description:</b> <asp:label ID="lblDescriptin" runat="server" MaxLength="300" ></asp:label>
                                                        </td>
                                                         
                                                    </tr>
                                                      <tr>
                                                       <td style="width: 200px;" colspan="2">
                                                            <b> URL:</b><asp:LinkButton ID="lnkLeadURL" runat="server" Text=""></asp:LinkButton>
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
                            <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr align="center">
                                            <td colspan="4" style="padding-top: 15px;">
                                                <div style="width: 240px; margin: 0 auto;">
                                                   
                                                       <asp:Button ID="Button11" CssClass="g-button g-button-submit" runat="server"
                                                   Text="Close" OnClientClick="return ClosePopup22();"/>
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
   
   <!-- end -->
    </form>
</body>
</html>
