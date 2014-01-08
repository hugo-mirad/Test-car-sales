<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewDealerSale.aspx.cs" Inherits="NewDealerSale" %>

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
	
	
	$(function(){	
	    	
		$('.hid').next('div.hidden').hide();				
		
		
		$('.sample4').numeric();		
	})	
    </script>

    <script type="text/javascript" language="javascript">
    
      function ValidateSavedData()
      {
         var valid = true;         
            if(document.getElementById('<%=ddlSaleStatus.ClientID%>').value =="1")  
            {
                 if (document.getElementById('<%= txtDealerShipName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerShipName.ClientID%>').focus();
                alert("Enter dealership name");
                document.getElementById('<%=txtDealerShipName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerShipName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }               
            
               if (document.getElementById('<%= txtCity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCity.ClientID%>').focus();
                alert("Enter customer city");
                document.getElementById('<%=txtCity.ClientID%>').value = ""
                document.getElementById('<%=txtCity.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }   
            if(document.getElementById('<%=ddlLocationState.ClientID%>').value =="0")
            {
                alert("Please select customer state"); 
                valid=false;
                document.getElementById('ddlLocationState').focus();  
                return valid;               
            }        
             if(document.getElementById('<%=txtZip.ClientID%>').value.trim().length > 0)
             {
                  var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtZip.ClientID%>').value);             
                   if (isValid == false)
                   {
                         document.getElementById('<%= txtZip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                     document.getElementById('<%=txtZip.ClientID%>').value = ""
                    document.getElementById('<%=txtZip.ClientID%>').focus()
                    valid = false;  
                     return valid;      
                   }                                   
             }     
              if (document.getElementById('<%= txtDealerContactName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerContactName.ClientID%>').focus();
                alert("Enter contact name");
                document.getElementById('<%=txtDealerContactName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerContactName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }                   
              if (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtContactPhone.ClientID%>').focus();
                alert("Enter contact phone number");
                document.getElementById('<%=txtContactPhone.ClientID%>').value = ""
                document.getElementById('<%=txtContactPhone.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }    
             if((document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtContactPhone.ClientID%>').focus();
                document.getElementById('<%=txtContactPhone.ClientID%>').value = "";
                alert("Enter valid phone number");
                valid = false; 
                 return valid;     
              } 
              
            if((document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length < 10)) {
            document.getElementById('<%= txtContactMobileNumber.ClientID%>').focus();
            document.getElementById('<%=txtContactMobileNumber.ClientID%>').value = "";
            alert("Enter valid mobile number");
            valid = false; 
            return valid;                

            } 
               if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
             {               
                document.getElementById('<%=txtEmail.ClientID%>').value = ""
                document.getElementById('<%=txtEmail.ClientID%>').focus()
                valid = false;               
                return valid;     
             } 
                  
            if(document.getElementById('<%= rdbtnPayAmex.ClientID%>').checked == true)
            {               
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 15)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "3")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Amex card number");
                valid = false; 
                return valid;  
                }
                }  
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 4)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }    
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                    if (isValid == false)
                    {
                    document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                    document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                    document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                    valid = false;  
                    return valid;      
                    }                                   
                }  
           }
           if(document.getElementById('<%= rdbtnPayVisa.ClientID%>').checked == true)
            {                  
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "4")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Visa card number");
                valid = false; 
                return valid;  
                }
                }    
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;  
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }    
            
             if(document.getElementById('<%= rdbtnPayMasterCard.ClientID%>').checked == true)
            {               
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "5")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Master card number");
                valid = false; 
                return valid;  
                }
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;  
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }       
              if(document.getElementById('<%= rdbtnPayDiscover.ClientID%>').checked == true)
            {
               
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "6")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Discover card number");
                valid = false; 
                return valid;  
                }
                }               
                
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }               
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }       
            if(document.getElementById('<%= rdbtnInvoice.ClientID%>').checked == true)
            {                  
               
                 if(document.getElementById('<%= rdbtnInvoiceEmail.ClientID%>').checked == true)
                 {
                    if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
                    {               
                    document.getElementById('<%=txtEmail.ClientID%>').value = ""
                    document.getElementById('<%=txtEmail.ClientID%>').focus()
                    valid = false;               
                    return valid;     
                    }      
                 }                  
                if(document.getElementById('<%=txtInvoiceZip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtInvoiceZip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtInvoiceZip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtInvoiceZip.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceZip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  
            }    
            
             if(document.getElementById('<%= rdbtnPayCheck.ClientID%>').checked == true)
            {
                    if((document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.trim().length < 4)) {
                    document.getElementById('<%= txtAccNumberForCheck.ClientID%>').focus();
                    document.getElementById('<%=txtAccNumberForCheck.ClientID%>').value = "";
                    alert("Enter valid account number");
                    valid = false; 
                    return valid; 
                    } 
                    if((document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.trim().length < 9)) {
                    document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').focus();
                    document.getElementById('<%=txtRoutingNumberForCheck.ClientID%>').value = "";
                    alert("Enter valid routing number");
                    valid = false; 
                    return valid; 
                    }                                                          
            }     
            if(document.getElementById('<%=txtPDAmountNow.ClientID%>').value != "")        
            {
                    var string = $('#ddlPackage option:selected').text();
                    var p =string.split('$');
                    var pp = p[1].split(')');
                    //alert(pp[0]);
                    //pp[0] = parseInt(pp[0]);
                    pp[0] = parseFloat(pp[0]);
                    var selectedPack = pp[0].toFixed(2);
                    
                    var EnterAmt = parseFloat($('#txtPDAmountNow').val());

                    if(EnterAmt > selectedPack){
                    document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
                    alert("Amount more than selected package price");
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
                    valid = false; 
                    return valid;     
                    } 
                    if(EnterAmt < selectedPack){
                    document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
                    alert("Amount less than selected package price");
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
                    valid = false; 
                    return valid;     
                    }   
            }                            
            document.getElementById('<%= ddlTargetDate.ClientID%>').value = "0";
            document.getElementById('<%= ddlCallbackDate.ClientID%>').value = "0";
            $find('<%= MPEUpdate.ClientID%>').show();
             valid = false; 
            return valid;  
           
            }
            if(document.getElementById('<%=ddlSaleStatus.ClientID%>').value =="2")  
            {
                if(document.getElementById('<%= ddlPackage.ClientID%>').value == "0") {
                document.getElementById('<%= ddlPackage.ClientID%>').focus();
                alert("Select package");                 
                document.getElementById('<%=ddlPackage.ClientID%>').focus()
                valid = false;            
                 return valid;     
                }  
              if(document.getElementById('<%=ddlPromotionOptions.ClientID%>').value =="1")  
                {
                    document.getElementById('<%= ddlPromotionOptions.ClientID%>').focus();
                    alert("Select promotion option");                 
                    document.getElementById('<%=ddlPromotionOptions.ClientID%>').focus()
                    valid = false;            
                     return valid;     
                }
             if (document.getElementById('<%= txtDealerShipName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerShipName.ClientID%>').focus();
                alert("Enter dealership name");
                document.getElementById('<%=txtDealerShipName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerShipName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }                
             if((document.getElementById('<%= txtPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtPhone.ClientID%>').focus();
                document.getElementById('<%=txtPhone.ClientID%>').value = "";
                alert("Enter valid phone number");
                valid = false; 
                 return valid;   
              } 
               if (document.getElementById('<%= txtCity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCity.ClientID%>').focus();
                alert("Enter customer city");
                document.getElementById('<%=txtCity.ClientID%>').value = ""
                document.getElementById('<%=txtCity.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }   
            if(document.getElementById('<%=ddlLocationState.ClientID%>').value =="0")
            {
                alert("Please select customer state"); 
                valid=false;
                document.getElementById('ddlLocationState').focus();  
                return valid;               
            } 
        if (document.getElementById('<%= txtZip.ClientID%>').value.trim().length < 1) {
            document.getElementById('<%= txtZip.ClientID%>').focus();
            alert("Enter zipcode");
            document.getElementById('<%=txtZip.ClientID%>').value = ""
            document.getElementById('<%=txtZip.ClientID%>').focus()                
            valid = false;
            return valid;     
            }   
             if(document.getElementById('<%=txtZip.ClientID%>').value.trim().length > 0)
             {
                  var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtZip.ClientID%>').value);             
                   if (isValid == false)
                   {
                         document.getElementById('<%= txtZip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                     document.getElementById('<%=txtZip.ClientID%>').value = ""
                    document.getElementById('<%=txtZip.ClientID%>').focus()
                    valid = false;  
                     return valid;      
                   }                                   
             }     
              if (document.getElementById('<%= txtDealerContactName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerContactName.ClientID%>').focus();
                alert("Enter contact name");
                document.getElementById('<%=txtDealerContactName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerContactName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }    
               
              if (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtContactPhone.ClientID%>').focus();
                alert("Enter contact phone number");
                document.getElementById('<%=txtContactPhone.ClientID%>').value = ""
                document.getElementById('<%=txtContactPhone.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }    
             if((document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtContactPhone.ClientID%>').focus();
                document.getElementById('<%=txtContactPhone.ClientID%>').value = "";
                alert("Enter valid phone number");
                valid = false; 
                 return valid;                
            
              }   
                if((document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtContactMobileNumber.ClientID%>').focus();
                document.getElementById('<%=txtContactMobileNumber.ClientID%>').value = "";
                alert("Enter valid mobile number");
                valid = false; 
                 return valid;                
            
              } 
               if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
             {               
                document.getElementById('<%=txtEmail.ClientID%>').value = ""
                document.getElementById('<%=txtEmail.ClientID%>').focus()
                valid = false;               
                return valid;     
             }        
            if(document.getElementById('<%= rdbtnPayAmex.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder first name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 15)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "3")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Amex card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 4)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }   
               
                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }
           if(document.getElementById('<%= rdbtnPayVisa.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                   
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "4")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Visa card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }   
                
                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }    
            
             if(document.getElementById('<%= rdbtnPayMasterCard.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                   
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "5")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Master card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }   
             

                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }       
              if(document.getElementById('<%= rdbtnPayDiscover.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                   
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "6")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Discover card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                } 
                

                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }       
            if(document.getElementById('<%= rdbtnInvoice.ClientID%>').checked == true)
            {                   
                if(document.getElementById('<%= txtAttentionTo.ClientID%>').value.length < 1) {
                document.getElementById('<%= txtAttentionTo.ClientID%>').focus();
                alert("Enter invoice attention to");
                document.getElementById('<%=txtAttentionTo.ClientID%>').value = ""
                document.getElementById('<%=txtAttentionTo.ClientID%>').focus()
                valid = false;            
                return valid;     
                }  
                 if(document.getElementById('<%= rdbtnInvoiceEmail.ClientID%>').checked == true)
                 {
                    if (document.getElementById('<%= txtEmail.ClientID%>').value.trim().length < 1) {
                        document.getElementById('<%= txtEmail.ClientID%>').focus();
                        alert("Enter customer email");
                        document.getElementById('<%=txtEmail.ClientID%>').value = ""
                        document.getElementById('<%=txtEmail.ClientID%>').focus()                
                        valid = false;
                         return valid;     
                     } 
                    if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
                    {               
                    document.getElementById('<%=txtEmail.ClientID%>').value = ""
                    document.getElementById('<%=txtEmail.ClientID%>').focus()
                    valid = false;               
                    return valid;     
                    }      
                 }  
                if (document.getElementById('<%= txtInvoiceBillingname.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtInvoiceBillingname.ClientID%>').focus();
                alert("Enter billing name");
                document.getElementById('<%=txtInvoiceBillingname.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceBillingname.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                      if (document.getElementById('<%= tyxtInvoiceAddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= tyxtInvoiceAddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=tyxtInvoiceAddress.ClientID%>').value = ""
                document.getElementById('<%=tyxtInvoiceAddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtInvoiceCity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtInvoiceCity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtInvoiceCity.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceCity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlInvoiceState.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlInvoiceState').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtInvoiceZip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtInvoiceZip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtInvoiceZip.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceZip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtInvoiceZip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtInvoiceZip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtInvoiceZip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtInvoiceZip.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceZip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  
            }    
            
             if(document.getElementById('<%= rdbtnPayCheck.ClientID%>').checked == true)
            {
                      if(document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtAccNumberForCheck.ClientID%>').focus();
                    alert("Enter account number");
                    document.getElementById('<%=txtAccNumberForCheck.ClientID%>').value = ""
                    document.getElementById('<%=txtAccNumberForCheck.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }  
                      if((document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.trim().length < 4)) {
                    document.getElementById('<%= txtAccNumberForCheck.ClientID%>').focus();
                    document.getElementById('<%=txtAccNumberForCheck.ClientID%>').value = "";
                    alert("Enter valid account number");
                    valid = false; 
                    return valid; 
                    } 
                     if(document.getElementById('<%=ddlAccType.ClientID%>').value =="0")
                    {
                    alert("Please select account type"); 
                    valid=false;
                    document.getElementById('ddlAccType').focus();  
                    return valid;               
                    }   
                    if(document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').focus();
                    alert("Enter routing number");
                    document.getElementById('<%=txtRoutingNumberForCheck.ClientID%>').value = ""
                    document.getElementById('<%=txtRoutingNumberForCheck.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }  
                      if((document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.trim().length < 9)) {
                    document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').focus();
                    document.getElementById('<%=txtRoutingNumberForCheck.ClientID%>').value = "";
                    alert("Enter valid routing number");
                    valid = false; 
                    return valid; 
                    } 
                     if(document.getElementById('<%= txtCustNameForCheck.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtCustNameForCheck.ClientID%>').focus();
                    alert("Enter account holder name");
                    document.getElementById('<%=txtCustNameForCheck.ClientID%>').value = ""
                    document.getElementById('<%=txtCustNameForCheck.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }                                           
            }
            
             if(document.getElementById('<%=ddlContractDate.ClientID%>').value =="0")
            {
            alert("Please select contract date"); 
            valid=false;
            document.getElementById('ddlContractDate').focus();  
            return valid;               
            } 
            if(document.getElementById('<%=ddlContractStatus.ClientID%>').value =="0")
            {
            alert("Please select contract status"); 
            valid=false;
            document.getElementById('ddlContractStatus').focus();  
            return valid;               
            } 
            var string = $('#ddlPackage option:selected').text();
            var p =string.split('$');
            var pp = p[1].split(')');
            //alert(pp[0]);
            //pp[0] = parseInt(pp[0]);
            pp[0] = parseFloat(pp[0]);
            var selectedPack = pp[0].toFixed(2);
            
            var stringpr = $('#ddlPromotionOptions option:selected').text();
            var ppr =stringpr.split('$');
            var pppr = ppr[1].split(')');
            //alert(pp[0]);
            //pp[0] = parseInt(pp[0]);
            pppr[0] = parseFloat(pppr[0]);
            var selectedPackpromo = pppr[0].toFixed(2);
            

            var EnterAmt = parseFloat($('#txtPDAmountNow').val());

            if(document.getElementById('<%= txtPDAmountNow.ClientID%>').value.trim().length < 1) {
            document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
            alert("Enter amount being paid now");
            document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
            document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
            valid = false;            
            return valid;     
            }    
           
            if(EnterAmt > selectedPackpromo){
            document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
            alert("Amount more than selected promotion price");
            document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
            document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
            valid = false; 
            return valid;     
            } 
            if(EnterAmt < selectedPackpromo){
            document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
            alert("Amount less than selected promotion price");
            document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
            document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
            valid = false; 
            return valid;     
            }                   
            }
            if(document.getElementById('<%=ddlSaleStatus.ClientID%>').value =="3")  
            {
              if(document.getElementById('<%= ddlPackage.ClientID%>').value == "0") {
                document.getElementById('<%= ddlPackage.ClientID%>').focus();
                alert("Select package");                 
                document.getElementById('<%=ddlPackage.ClientID%>').focus()
                valid = false;            
                 return valid;     
                }  
              if(document.getElementById('<%=ddlPromotionOptions.ClientID%>').value !="1")  
                {
                    alert("Promotions not consider if it is a sale.");        
                }
             if (document.getElementById('<%= txtDealerShipName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerShipName.ClientID%>').focus();
                alert("Enter dealership name");
                document.getElementById('<%=txtDealerShipName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerShipName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }                
             if((document.getElementById('<%= txtPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtPhone.ClientID%>').focus();
                document.getElementById('<%=txtPhone.ClientID%>').value = "";
                alert("Enter valid phone number");
                valid = false; 
                 return valid;   
              } 
               if (document.getElementById('<%= txtCity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCity.ClientID%>').focus();
                alert("Enter customer city");
                document.getElementById('<%=txtCity.ClientID%>').value = ""
                document.getElementById('<%=txtCity.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }   
            if(document.getElementById('<%=ddlLocationState.ClientID%>').value =="0")
            {
                alert("Please select customer state"); 
                valid=false;
                document.getElementById('ddlLocationState').focus();  
                return valid;               
            } 
        if (document.getElementById('<%= txtZip.ClientID%>').value.trim().length < 1) {
            document.getElementById('<%= txtZip.ClientID%>').focus();
            alert("Enter zipcode");
            document.getElementById('<%=txtZip.ClientID%>').value = ""
            document.getElementById('<%=txtZip.ClientID%>').focus()                
            valid = false;
            return valid;     
            }   
             if(document.getElementById('<%=txtZip.ClientID%>').value.trim().length > 0)
             {
                  var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtZip.ClientID%>').value);             
                   if (isValid == false)
                   {
                         document.getElementById('<%= txtZip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                     document.getElementById('<%=txtZip.ClientID%>').value = ""
                    document.getElementById('<%=txtZip.ClientID%>').focus()
                    valid = false;  
                     return valid;      
                   }                                   
             }     
              if (document.getElementById('<%= txtDealerContactName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerContactName.ClientID%>').focus();
                alert("Enter contact name");
                document.getElementById('<%=txtDealerContactName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerContactName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }    
               
              if (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtContactPhone.ClientID%>').focus();
                alert("Enter contact phone number");
                document.getElementById('<%=txtContactPhone.ClientID%>').value = ""
                document.getElementById('<%=txtContactPhone.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }    
             if((document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtContactPhone.ClientID%>').focus();
                document.getElementById('<%=txtContactPhone.ClientID%>').value = "";
                alert("Enter valid phone number");
                valid = false; 
                 return valid;                
            
              }   
                if((document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtContactMobileNumber.ClientID%>').focus();
                document.getElementById('<%=txtContactMobileNumber.ClientID%>').value = "";
                alert("Enter valid mobile number");
                valid = false; 
                 return valid;                
            
              } 
               if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
             {               
                document.getElementById('<%=txtEmail.ClientID%>').value = ""
                document.getElementById('<%=txtEmail.ClientID%>').focus()
                valid = false;               
                return valid;     
             }        
            if(document.getElementById('<%= rdbtnPayAmex.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder first name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 15)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "3")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Amex card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 4)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }   
               
                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }
           if(document.getElementById('<%= rdbtnPayVisa.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                   
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "4")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Visa card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }   
                
                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }    
            
             if(document.getElementById('<%= rdbtnPayMasterCard.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                   
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "5")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Master card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                }   
             

                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }       
              if(document.getElementById('<%= rdbtnPayDiscover.ClientID%>').checked == true)
            {
                if (document.getElementById('<%= txtCardholderName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderName.ClientID%>').focus();
                alert("Enter card holder name");
                document.getElementById('<%=txtCardholderName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderName.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                   
                if (document.getElementById('<%= txtCardholderLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCardholderLastName.ClientID%>').focus();
                alert("Enter card holder last name");
                document.getElementById('<%=txtCardholderLastName.ClientID%>').value = ""
                document.getElementById('<%=txtCardholderLastName.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();
                alert("Enter card number");
                document.getElementById('<%=CardNumber.ClientID%>').value = ""
                document.getElementById('<%=CardNumber.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                return valid;              

                }           
                var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
                if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
                {
                CCNum = CCNum.charAt(0);
                if(CCNum != "6")
                {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("This is not a Discover card number");
                valid = false; 
                return valid;  
                }
                }               
                if(document.getElementById('<%=ExpMon.ClientID%>').value =="0")
                {
                alert("Please select the expiration month"); 
                valid=false;
                document.getElementById('ExpMon').focus();  
                return valid;               
                }
                if(document.getElementById('<%=CCExpiresYear.ClientID%>').value =="0")
                {
                alert("Please select the expiration year"); 
                valid=false;
                document.getElementById('CCExpiresYear').focus();  
                return valid;               
                }  
                if (document.getElementById('<%= cvv.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= cvv.ClientID%>').focus();
                alert("Enter cvv number");
                document.getElementById('<%=cvv.ClientID%>').value = ""
                document.getElementById('<%=cvv.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                return valid;              

                } 
                

                if (document.getElementById('<%= txtbillingaddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingaddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=txtbillingaddress.ClientID%>').value = ""
                document.getElementById('<%=txtbillingaddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtbillingcity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingcity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtbillingcity.ClientID%>').value = ""
                document.getElementById('<%=txtbillingcity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlbillingstate.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlbillingstate').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtbillingzip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  

           }       
            if(document.getElementById('<%= rdbtnInvoice.ClientID%>').checked == true)
            {                   
                if(document.getElementById('<%= txtAttentionTo.ClientID%>').value.length < 1) {
                document.getElementById('<%= txtAttentionTo.ClientID%>').focus();
                alert("Enter invoice attention to");
                document.getElementById('<%=txtAttentionTo.ClientID%>').value = ""
                document.getElementById('<%=txtAttentionTo.ClientID%>').focus()
                valid = false;            
                return valid;     
                }  
                 if(document.getElementById('<%= rdbtnInvoiceEmail.ClientID%>').checked == true)
                 {
                    if (document.getElementById('<%= txtEmail.ClientID%>').value.trim().length < 1) {
                        document.getElementById('<%= txtEmail.ClientID%>').focus();
                        alert("Enter customer email");
                        document.getElementById('<%=txtEmail.ClientID%>').value = ""
                        document.getElementById('<%=txtEmail.ClientID%>').focus()                
                        valid = false;
                         return valid;     
                     } 
                    if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
                    {               
                    document.getElementById('<%=txtEmail.ClientID%>').value = ""
                    document.getElementById('<%=txtEmail.ClientID%>').focus()
                    valid = false;               
                    return valid;     
                    }      
                 }  
                if (document.getElementById('<%= txtInvoiceBillingname.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtInvoiceBillingname.ClientID%>').focus();
                alert("Enter billing name");
                document.getElementById('<%=txtInvoiceBillingname.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceBillingname.ClientID%>').focus()                
                valid = false;
                return valid;     
                } 
                      if (document.getElementById('<%= tyxtInvoiceAddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= tyxtInvoiceAddress.ClientID%>').focus();
                alert("Enter billing address");
                document.getElementById('<%=tyxtInvoiceAddress.ClientID%>').value = ""
                document.getElementById('<%=tyxtInvoiceAddress.ClientID%>').focus()                
                valid = false;
                return valid;     
                }    
                if (document.getElementById('<%= txtInvoiceCity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtInvoiceCity.ClientID%>').focus();
                alert("Enter city");
                document.getElementById('<%=txtInvoiceCity.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceCity.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=ddlInvoiceState.ClientID%>').value =="0")
                {
                alert("Please select state"); 
                valid=false;
                document.getElementById('ddlInvoiceState').focus();  
                return valid;               
                } 
                if (document.getElementById('<%= txtInvoiceZip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtInvoiceZip.ClientID%>').focus();
                alert("Enter zipcode");
                document.getElementById('<%=txtInvoiceZip.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceZip.ClientID%>').focus()                
                valid = false;
                return valid;     
                }   
                if(document.getElementById('<%=txtInvoiceZip.ClientID%>').value.trim().length > 0)
                {
                var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtInvoiceZip.ClientID%>').value);             
                if (isValid == false)
                {
                document.getElementById('<%= txtInvoiceZip.ClientID%>').focus();
                alert("Please enter valid zipcode");
                document.getElementById('<%=txtInvoiceZip.ClientID%>').value = ""
                document.getElementById('<%=txtInvoiceZip.ClientID%>').focus()
                valid = false;  
                return valid;      
                }                                   
                }  
            }    
            
             if(document.getElementById('<%= rdbtnPayCheck.ClientID%>').checked == true)
            {
                      if(document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtAccNumberForCheck.ClientID%>').focus();
                    alert("Enter account number");
                    document.getElementById('<%=txtAccNumberForCheck.ClientID%>').value = ""
                    document.getElementById('<%=txtAccNumberForCheck.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }  
                      if((document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtAccNumberForCheck.ClientID%>').value.trim().length < 4)) {
                    document.getElementById('<%= txtAccNumberForCheck.ClientID%>').focus();
                    document.getElementById('<%=txtAccNumberForCheck.ClientID%>').value = "";
                    alert("Enter valid account number");
                    valid = false; 
                    return valid; 
                    } 
                     if(document.getElementById('<%=ddlAccType.ClientID%>').value =="0")
                    {
                    alert("Please select account type"); 
                    valid=false;
                    document.getElementById('ddlAccType').focus();  
                    return valid;               
                    }   
                    if(document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').focus();
                    alert("Enter routing number");
                    document.getElementById('<%=txtRoutingNumberForCheck.ClientID%>').value = ""
                    document.getElementById('<%=txtRoutingNumberForCheck.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }  
                      if((document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').value.trim().length < 9)) {
                    document.getElementById('<%= txtRoutingNumberForCheck.ClientID%>').focus();
                    document.getElementById('<%=txtRoutingNumberForCheck.ClientID%>').value = "";
                    alert("Enter valid routing number");
                    valid = false; 
                    return valid; 
                    } 
                     if(document.getElementById('<%= txtCustNameForCheck.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtCustNameForCheck.ClientID%>').focus();
                    alert("Enter account holder name");
                    document.getElementById('<%=txtCustNameForCheck.ClientID%>').value = ""
                    document.getElementById('<%=txtCustNameForCheck.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }                                           
            }
            
             if(document.getElementById('<%=ddlContractDate.ClientID%>').value =="0")
            {
            alert("Please select contract date"); 
            valid=false;
            document.getElementById('ddlContractDate').focus();  
            return valid;               
            } 
            if(document.getElementById('<%=ddlContractStatus.ClientID%>').value =="0")
            {
            alert("Please select contract status"); 
            valid=false;
            document.getElementById('ddlContractStatus').focus();  
            return valid;               
            } 
            var string = $('#ddlPackage option:selected').text();
            var p =string.split('$');
            var pp = p[1].split(')');
            //alert(pp[0]);
            //pp[0] = parseInt(pp[0]);
            pp[0] = parseFloat(pp[0]);
            var selectedPack = pp[0].toFixed(2);


            var EnterAmt = parseFloat($('#txtPDAmountNow').val());

            if(document.getElementById('<%= txtPDAmountNow.ClientID%>').value.trim().length < 1) {
            document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
            alert("Enter amount being paid now");
            document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
            document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
            valid = false;            
            return valid;     
            }    

            if(EnterAmt > selectedPack){
            document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
            alert("Amount more than selected package price");
            document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
            document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
            valid = false; 
            return valid;     
            } 
            if(EnterAmt < selectedPack){
            document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
            alert("Amount less than selected package price");
            document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
            document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
            valid = false; 
            return valid;     
            }                   
               
            }
                                    
            return valid;
      }
     
     function PhoneOnblur()
     {
           if((document.getElementById('<%= txtPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length < 10)) {                           
                alert("Enter valid phone number");
                document.getElementById('<%= txtPhone.ClientID%>').focus();  
                valid = false; 
                 return valid;              
            
            } 
          
           if((document.getElementById('<%= txtPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length == 10)) {
              var phone = document.getElementById('<%= txtPhone.ClientID%>').value;
               formatted = phone.substr(0, 3) + '-' + phone.substr(3, 3) + '-' + phone.substr(6,4);                
                document.getElementById('<%=txtPhone.ClientID%>').value = formatted;               
                valid = true; 
                 return valid;                
            
            }   
                      
     }
    
      function FaxOnblur()
     {
           if((document.getElementById('<%= txtFax.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtFax.ClientID%>').value.trim().length < 10)) {                           
                alert("Enter valid fax number");
                document.getElementById('<%= txtFax.ClientID%>').focus();  
                valid = false; 
                 return valid;              
            
            } 
          
           if((document.getElementById('<%= txtFax.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtFax.ClientID%>').value.trim().length == 10)) {
              var phone = document.getElementById('<%= txtFax.ClientID%>').value;
               formatted = phone.substr(0, 3) + '-' + phone.substr(3, 3) + '-' + phone.substr(6,4);                
                document.getElementById('<%=txtFax.ClientID%>').value = formatted;               
                valid = true; 
                 return valid;                
            
            }   
                      
     }
       function ContactPhoneOnblur()
     {
           if((document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length < 10)) {                           
                alert("Enter valid phone number");
                document.getElementById('<%= txtContactPhone.ClientID%>').focus();  
                valid = false; 
                 return valid;              
            
            }           
           if((document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactPhone.ClientID%>').value.trim().length == 10)) {
              var phone = document.getElementById('<%= txtContactPhone.ClientID%>').value;
               formatted = phone.substr(0, 3) + '-' + phone.substr(3, 3) + '-' + phone.substr(6,4);                
                document.getElementById('<%=txtContactPhone.ClientID%>').value = formatted;               
                valid = true; 
                 return valid;              
            
            }   
                      
     }
        function EmailOnblur()
     {           
               if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
             {                           
                document.getElementById('<%=txtEmail.ClientID%>').focus()
                valid = false;               
                return valid;     
            }     
                       
     }
         function InvoiceEmailOnblur()
     {           
               if ((document.getElementById('<%=txtInvoiceEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtInvoiceEmail.ClientID%>').value.trim()) == false) )
             {                           
                document.getElementById('<%=txtInvoiceEmail.ClientID%>').focus()
                valid = false;               
                return valid;     
            }     
                       
     }
       function ContactMobileOnblur()
     {
           if((document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length < 10)) {                           
                alert("Enter valid phone number");
                document.getElementById('<%= txtContactMobileNumber.ClientID%>').focus();  
                valid = false; 
                 return valid;              
            
            } 
          
           if((document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtContactMobileNumber.ClientID%>').value.trim().length == 10)) {
              var phone = document.getElementById('<%= txtContactMobileNumber.ClientID%>').value;
               formatted = phone.substr(0, 3) + '-' + phone.substr(3, 3) + '-' + phone.substr(6,4);                
                document.getElementById('<%=txtContactMobileNumber.ClientID%>').value = formatted;               
                valid = true; 
                 return valid;                
            
            }   
                      
     }
     function CreditCardOnblur()
     {
         if(document.getElementById('<%= rdbtnPayAmex.ClientID%>').checked == true)
         {
           if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 15)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                 return valid;              
            
            }           
            var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
            if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
            {
                CCNum = CCNum.charAt(0);
                if(CCNum != "3")
                {
                 document.getElementById('<%= CardNumber.ClientID%>').focus();             
                 alert("This is not a Amex card number");
                 valid = false; 
                 return valid;  
                }
            }
         }
         
         if(document.getElementById('<%= rdbtnPayVisa.ClientID%>').checked == true)
         {
           if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                 return valid;              
            
            }           
            var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
            if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
            {
                CCNum = CCNum.charAt(0);
                if(CCNum != "4")
                {
                 document.getElementById('<%= CardNumber.ClientID%>').focus();             
                 alert("This is not a Visa card number");
                 valid = false; 
                 return valid;  
                }
            }
         }
          if(document.getElementById('<%= rdbtnPayMasterCard.ClientID%>').checked == true)
         {
           if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                 return valid;              
            
            }           
            var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
            if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
            {
                CCNum = CCNum.charAt(0);
                if(CCNum != "5")
                {
                 document.getElementById('<%= CardNumber.ClientID%>').focus();             
                 alert("This is not a Master card number");
                 valid = false; 
                 return valid;  
                }
            }
         }
         if(document.getElementById('<%= rdbtnPayDiscover.ClientID%>').checked == true)
         {
           if((document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= CardNumber.ClientID%>').value.trim().length != 16)) {
                document.getElementById('<%= CardNumber.ClientID%>').focus();             
                alert("Enter valid card number");
                valid = false; 
                 return valid;              
            
            }           
            var CCNum = document.getElementById('<%= CardNumber.ClientID%>').value.trim();
            if(document.getElementById('<%= CardNumber.ClientID%>').value.trim().length > 0) 
            {
                CCNum = CCNum.charAt(0);
                if(CCNum != "6")
                {
                 document.getElementById('<%= CardNumber.ClientID%>').focus();             
                 alert("This is not a Discover card number");
                 valid = false; 
                 return valid;  
                }
            }
         }
         
                      
     }
     
     function CVVOnblur()
     {
         if(document.getElementById('<%= rdbtnPayAmex.ClientID%>').checked == true)
         {
           if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 4)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                 return valid;              
            
            }                      
         }
         
         if(document.getElementById('<%= rdbtnPayVisa.ClientID%>').checked == true)
         {
           if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                 return valid;              
            
            }     
         }
          if(document.getElementById('<%= rdbtnPayMasterCard.ClientID%>').checked == true)
         {
            if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                 return valid;              
            
            }  
         }
         if(document.getElementById('<%= rdbtnPayDiscover.ClientID%>').checked == true)
         {
            if((document.getElementById('<%= cvv.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= cvv.ClientID%>').value.trim().length != 3)) {
                document.getElementById('<%= cvv.ClientID%>').focus();             
                alert("Enter valid cvv number");
                valid = false; 
                 return valid;              
            
            }  
         }
         
                      
     }
     
     function ZipOnblur()
     {
          if(document.getElementById('<%=txtZip.ClientID%>').value.trim().length > 0)
             {
                  var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtZip.ClientID%>').value);             
                   if (isValid == false)
                   {
                         document.getElementById('<%= txtZip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                     document.getElementById('<%=txtZip.ClientID%>').value = ""
                    document.getElementById('<%=txtZip.ClientID%>').focus()
                    valid = false;  
                     return valid;      
                   }                                   
             }  
                      
     }
     function billingZipOnblur()
     {
          if(document.getElementById('<%=txtbillingzip.ClientID%>').value.trim().length > 0)
             {
                  var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtbillingzip.ClientID%>').value);             
                   if (isValid == false)
                   {
                         document.getElementById('<%= txtbillingzip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                     document.getElementById('<%=txtbillingzip.ClientID%>').value = ""
                    document.getElementById('<%=txtbillingzip.ClientID%>').focus()
                    valid = false;  
                     return valid;      
                   }                                   
             }  
                      
     }
     function txtInvoiceZip()
     {
          if(document.getElementById('<%=txtInvoiceZip.ClientID%>').value.trim().length > 0)
             {
                  var isValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test(document.getElementById('<%=txtInvoiceZip.ClientID%>').value);             
                   if (isValid == false)
                   {
                         document.getElementById('<%= txtInvoiceZip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                     document.getElementById('<%=txtInvoiceZip.ClientID%>').value = ""
                    document.getElementById('<%=txtInvoiceZip.ClientID%>').focus()
                    valid = false;  
                     return valid;      
                   }                                   
             }  
                      
     }
     
      function PhoneOnfocus()
     {           
              var phone = document.getElementById('<%= txtPhone.ClientID%>').value;
               formatted =phone.replace("-","");
               formatted =formatted.replace("-","");
                document.getElementById('<%=txtPhone.ClientID%>').value = formatted;            
                       
     }
   
      function FaxOnfocus()
     {           
              var phone = document.getElementById('<%= txtFax.ClientID%>').value;
               formatted =phone.replace("-","");
               formatted =formatted.replace("-","");
                document.getElementById('<%=txtFax.ClientID%>').value = formatted;            
                       
     }
     function ContactPhoneOnfocus()
     {           
              var phone = document.getElementById('<%= txtContactPhone.ClientID%>').value;
               formatted =phone.replace("-","");
               formatted =formatted.replace("-","");
                document.getElementById('<%=txtContactPhone.ClientID%>').value = formatted;            
                       
     }
    function ContactMobileOnfocus()
     {           
              var phone = document.getElementById('<%= txtContactMobileNumber.ClientID%>').value;
               formatted =phone.replace("-","");
               formatted =formatted.replace("-","");
                document.getElementById('<%=txtContactMobileNumber.ClientID%>').value = formatted;            
                       
     }
   
              
    </script>

    <script type="text/javascript" language="javascript">
    function CopySellerInfo()
        {
         
            var valid=true;   
                if (document.getElementById('<%= txtDealerContactName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerContactName.ClientID%>').focus();
                alert("Enter customer name");
                document.getElementById('<%=txtDealerContactName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerContactName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }           
             else if (document.getElementById('<%= txtAddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtAddress.ClientID%>').focus();
                alert("Enter customer address");
                document.getElementById('<%=txtAddress.ClientID%>').value = ""
                document.getElementById('<%=txtAddress.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }    
             else if (document.getElementById('<%= txtCity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCity.ClientID%>').focus();
                alert("Enter customer city");
                document.getElementById('<%=txtCity.ClientID%>').value = ""
                document.getElementById('<%=txtCity.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }   
            else if(document.getElementById('<%=ddlLocationState.ClientID%>').value =="0")
            {
                alert("Please select customer state"); 
                valid=false;
                document.getElementById('ddlLocationState').focus();  
                return valid;               
            } 
            else if (document.getElementById('<%= txtZip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtZip.ClientID%>').focus();
                alert("Enter zip");
                document.getElementById('<%=txtZip.ClientID%>').value = ""
                document.getElementById('<%=txtZip.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }  
            else if((document.getElementById('<%=txtZip.ClientID%>').value.trim().length > 0) && (document.getElementById('<%=txtZip.ClientID%>').value.trim().length < 5))
             {          

                    document.getElementById('<%= txtZip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                    document.getElementById('<%=txtZip.ClientID%>').value = ""
                    document.getElementById('<%=txtZip.ClientID%>').focus()
                    valid = false;  
                    return valid;      
                                                     
             }   
              else
              {
                
                 document.getElementById('<%= txtCardholderName.ClientID%>').value =  document.getElementById('<%= txtDealerContactName.ClientID%>').value;                
                 document.getElementById('<%= txtCardholderLastName.ClientID%>').value =  document.getElementById('<%= txtDealerContactName.ClientID%>').value;
                 document.getElementById('<%= txtbillingaddress.ClientID%>').value =  document.getElementById('<%= txtAddress.ClientID%>').value;
                 document.getElementById('<%= txtbillingcity.ClientID%>').value =  document.getElementById('<%= txtCity.ClientID%>').value;
                 document.getElementById('<%= ddlbillingstate.ClientID%>').value =  document.getElementById('<%= ddlLocationState.ClientID%>').value;                 
                 document.getElementById('<%= txtbillingzip.ClientID%>').value =  document.getElementById('<%= txtZip.ClientID%>').value;
              }             
              return valid;      
        } 
          
         function CopySellerInfoForCheck()
        {
         
            var valid=true;   
                if (document.getElementById('<%= txtDealerContactName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerContactName.ClientID%>').focus();
                alert("Enter customer name");
                document.getElementById('<%=txtDealerContactName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerContactName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }   
              else
              {
                
                 document.getElementById('<%= txtCustNameForCheck.ClientID%>').value =  document.getElementById('<%= txtDealerContactName.ClientID%>').value;                            
                           
              }             
              return valid;      
        } 
         function CopySellerInfoForInvoice()
        {
         
            var valid=true;   
                if (document.getElementById('<%= txtDealerContactName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtDealerContactName.ClientID%>').focus();
                alert("Enter customer name");
                document.getElementById('<%=txtDealerContactName.ClientID%>').value = ""
                document.getElementById('<%=txtDealerContactName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }           
             else if (document.getElementById('<%= txtAddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtAddress.ClientID%>').focus();
                alert("Enter customer address");
                document.getElementById('<%=txtAddress.ClientID%>').value = ""
                document.getElementById('<%=txtAddress.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }    
             else if (document.getElementById('<%= txtCity.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtCity.ClientID%>').focus();
                alert("Enter customer city");
                document.getElementById('<%=txtCity.ClientID%>').value = ""
                document.getElementById('<%=txtCity.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }   
            else if(document.getElementById('<%=ddlLocationState.ClientID%>').value =="0")
            {
                alert("Please select customer state"); 
                valid=false;
                document.getElementById('ddlLocationState').focus();  
                return valid;               
            } 
            else if (document.getElementById('<%= txtZip.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtZip.ClientID%>').focus();
                alert("Enter zip");
                document.getElementById('<%=txtZip.ClientID%>').value = ""
                document.getElementById('<%=txtZip.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }  
            else if((document.getElementById('<%=txtZip.ClientID%>').value.trim().length > 0) && (document.getElementById('<%=txtZip.ClientID%>').value.trim().length < 5))
             {          

                    document.getElementById('<%= txtZip.ClientID%>').focus();
                    alert("Please enter valid zipcode");
                    document.getElementById('<%=txtZip.ClientID%>').value = ""
                    document.getElementById('<%=txtZip.ClientID%>').focus()
                    valid = false;  
                    return valid;      
                                                     
             }   
              else
              {
                
                 document.getElementById('<%= txtInvoiceBillingname.ClientID%>').value =  document.getElementById('<%= txtDealerContactName.ClientID%>').value;                                 
                 document.getElementById('<%= tyxtInvoiceAddress.ClientID%>').value =  document.getElementById('<%= txtAddress.ClientID%>').value;
                 document.getElementById('<%= txtInvoiceCity.ClientID%>').value =  document.getElementById('<%= txtCity.ClientID%>').value;
                 document.getElementById('<%= ddlInvoiceState.ClientID%>').value =  document.getElementById('<%= ddlLocationState.ClientID%>').value;                 
                 document.getElementById('<%= txtInvoiceZip.ClientID%>').value =  document.getElementById('<%= txtZip.ClientID%>').value;
              }             
              return valid;      
        } 
             
          
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
    
    
    
    
    
    
    $('#txtPDAmountNow').live('focus',function(){
        if($('#ddlPackage option:selected').text() == 'Select'){ 
            $('#ddlPackage').focus();
            alert('Select package');
        }
    });
     function OnchangeDropdown(){          
            if($('#ddlPackage option:selected').text() != 'Select'){  
               var string = $('#ddlPackage option:selected').text();
                var p =string.split('$');
                var pp = p[1].split(')');
                //alert(pp[0]);
                //pp[0] = parseInt(pp[0]);
                pp[0] = parseFloat(pp[0]);
                var selectedPack = pp[0].toFixed(2);
                selectedPack = parseFloat(selectedPack); 
                $('#txtPDAmountNow').val(selectedPack);
                           
                }else{
                
                 $('#txtTotalAmount').val('');                 
                }                            
                      
            }           
    /*
    $('#txtPDAmountNow').live('keydown', function(){
        //console.log($(this).val())
        $(this).val($(this).val().toString().replace(/^[0-9]\./g, ',').replace(/\./g, ''));
    });
    */
    
    
             
    $('#txtPDAmountNow').live('blur', function(){           
            if($('#txtPDAmountNow').val().length>0 && ($('#ddlPackage option:selected').text() != 'Select')){   
                var curr = parseFloat($('#txtPDAmountNow').val());
                curr = curr.toFixed(2)         
                var string = $('#ddlPackage option:selected').text();
                var p =string.split('$');
                var pp = p[1].split(')');
                //alert(pp[0]);
                //pp[0] = parseInt(pp[0]);
                pp[0] = parseFloat(pp[0]);
                var selectedPack = pp[0].toFixed(2);
                selectedPack = parseFloat(selectedPack); 
                
                if(selectedPack < curr){
                    alert('Entered amount can not be graterthen selected package..')
                     document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
                }                          
                      
            }            
    });
    
    
   
   
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
                    <ul class="menu2">
                        <li><span style="font-size: 13px; font-weight: bold; cursor: pointer; color: #FFC50F">
                            Menu &nabla;</span>
                            <ul>
                                <li>
                                    <asp:LinkButton ID="lnkTicker" runat="server" Text="Sales Ticker"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnCentralReport" runat="server" Text="Center report" OnClick="lnkbtnCentralReport_Click"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkIntromail" runat="server" Text="Intromail" OnClick="lnkIntromail_Click"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAdmin" runat="server" Text="User mgmt" OnClick="lnkbtnAdmin_Click"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnReports" runat="server" Text="My report" OnClick="lnkbtnReports_Click"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnTransfers" runat="server" Text="Transfers" PostBackUrl="~/LiveTransfers.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnNewSale" runat="server" Text="New sale" OnClick="lnkbtnNewSale_Click"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnMyDealerRep" runat="server" Text="My dealer report" OnClick="lnkbtnMyDealerRep_Click"
                                        Enabled="false"></asp:LinkButton>
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
                            <asp:DropDownList ID="ddlSaleStatus" runat="server" Font-Size="14px" Font-Bold="true"
                                ForeColor="#2B4BB1">
                                <asp:ListItem Value="1">Prospect</asp:ListItem>
                                <asp:ListItem Value="2">Promotional</asp:ListItem>
                                <asp:ListItem Value="3">Sale</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="btnSave" runat="server" CssClass="g-button g-button-submit" Text="Save"
                                OnClick="btnSave_Click" OnClientClick="return ValidateSavedData();" />
                            &nbsp;
                            <asp:Button ID="btnAbandon" runat="server" CssClass="g-button g-button-submit" Text="Abandon"
                                OnClick="btnAbandon_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td width="50%">
                    <h4 class="h4">
                        <span class="star" style="color: Red">*</span><strong style="width: 110px;">Dealer Coordinator:</strong>
                        <asp:DropDownList ID="ddlSaleAgent" runat="server" onchange="return OnchangeDropdown()">
                        </asp:DropDownList>
                    </h4>
                </td>
                <td>
                    <h4 class="h4">
                        <span class="star" style="color: Red">*</span><strong style="width: 45px;">Package:</strong>
                        <%-- <input type="text" style="width: 245px" />--%>
                        <asp:DropDownList ID="ddlPackage" runat="server">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="11">Dealer Compass Light ($99.99)</asp:ListItem>
                            <asp:ListItem Value="10">Dealer Compass Package ($200.00)</asp:ListItem>
                        </asp:DropDownList>
                    </h4>
                </td>
            </tr>
            <tr>
                <td>
                    <h4 class="h4">
                        <strong style="width: 75px;">Lead Source:</strong>
                        <asp:TextBox ID="txtLeadSource" runat="server" MaxLength="50"></asp:TextBox>
                    </h4>
                </td>
                <td>
                    <h4 class="h4">
                        <strong style="width: 110px;">Promotion Option:</strong>
                        <asp:DropDownList ID="ddlPromotionOptions" runat="server">
                        </asp:DropDownList>
                    </h4>
                </td>
            </tr>
            <tr>
                <td>
                    <h4 class="h4">
                        <strong style="width: 70px;">Lead Agent:</strong>
                        <asp:TextBox ID="txtLeadAgent" runat="server" MaxLength="50"></asp:TextBox>
                    </h4>
                </td>
                <td style="text-align: right">
                    &nbsp;
                </td>
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
                                                <asp:TextBox ID="txtDealerShipName" runat="server" MaxLength="30"></asp:TextBox>
                                            </h4>
                                        </td>
                                        <td>
                                            <h4 class="h4">
                                                <strong style="width: 50px">Phone#:</strong>
                                                <%-- <input type="text" style="width: 394px" />--%>
                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                    onblur="return PhoneOnblur();" onfocus="return PhoneOnfocus();"></asp:TextBox>
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
                                                                onblur="return FaxOnblur();" onfocus="return FaxOnfocus();"></asp:TextBox>
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
                                                            <asp:TextBox ID="txtWebAddress" runat="server" MaxLength="100"></asp:TextBox>
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
                                                            <asp:TextBox ID="txtDealerLicenseNumber" runat="server" MaxLength="60"></asp:TextBox>
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
                                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="40"></asp:TextBox>
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
                                                            <asp:TextBox ID="txtCity" runat="server" MaxLength="40"></asp:TextBox>
                                                        </h4>
                                                    </td>
                                                    <td style="width: 120px;">
                                                        <h4 class="h4">
                                                            <span class="star" style="color: Red">*</span><strong>State:</strong>
                                                            <%-- <input type="text" style="width: 63px" />--%>
                                                            <asp:DropDownList ID="ddlLocationState" runat="server" Style="width: 100px">
                                                            </asp:DropDownList>
                                                        </h4>
                                                    </td>
                                                    <td>
                                                        <h4 class="h4">
                                                            <span class="star" style="color: Red">*</span><strong>Zip:</strong>
                                                            <%--<input type="text" style="width: 74px" class="sample4" />--%>
                                                            <asp:TextBox ID="txtZip" runat="server" Style="width: 74px" MaxLength="5" class="sample4"
                                                                onkeypress="return isNumberKey(event)" onblur="return ZipOnblur();"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtDealerContactName" runat="server" MaxLength="30"></asp:TextBox>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 50px">Job title:</strong>
                                                    <%-- <input type="text" style="width: 394px" />--%>
                                                    <asp:TextBox ID="txtDealerJobTitle" runat="server" MaxLength="30"></asp:TextBox>
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
                                                                    onblur="return ContactPhoneOnblur();" onfocus="return ContactPhoneOnfocus();"></asp:TextBox>
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
                                                                    onblur="return ContactMobileOnblur();" onfocus="return ContactMobileOnfocus();"></asp:TextBox>
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
                                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="60" onblur="return EmailOnblur();"></asp:TextBox>
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
                                                                    runat="server" Checked="true" /><span class="featNon">Yes</span>
                                                                <asp:RadioButton ID="rdbtnWebsiteNo" CssClass="noLM" Text="" GroupName="Website"
                                                                    runat="server" /><span class="featNon">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Preferred addresses:</strong><br />
                                                            <asp:TextBox ID="txtPreferredAddress" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%"></asp:TextBox>
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
                                                                    runat="server" Checked="true" /><span class="featNon">Yes</span>
                                                                <asp:RadioButton ID="rdbtnCarsPromotionNo" CssClass="noLM" Text="" GroupName="CarsPromotion"
                                                                    runat="server" /><span class="featNon">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Where to get initial cars from?:</strong><br />
                                                            <asp:TextBox ID="txtGetCarsFrom" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%"></asp:TextBox>
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
                                                                    Checked="true" /><span class="featNon">Yes</span>
                                                                <asp:RadioButton ID="rdbtnLeadsNo" CssClass="noLM" Text="" GroupName="Leads" runat="server" /><span
                                                                    class="featNon">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Whom to send it?:</strong><br />
                                                            <asp:TextBox ID="txtLeadsToSend" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%"></asp:TextBox>
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
                                                                    Checked="true" /><span class="featNon">Yes</span>
                                                                <asp:RadioButton ID="rdbtnOthersNo" CssClass="noLM" Text="" GroupName="Others" runat="server" /><span
                                                                    class="featNon">No</span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <strong>Others notes::</strong><br />
                                                            <asp:TextBox ID="txtOthersNotes" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="80%"></asp:TextBox>
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
                                                <asp:Repeater ID="RepeterSurvey" runat="server">
                                                    <ItemTemplate>
                                                        <div>
                                                            <strong>
                                                                <asp:Label ID="lblSurveyQuestions" runat="server" Text='<%# Eval("SurveyQuestion") %>'></asp:Label></strong>
                                                            <asp:TextBox ID="txtSurveyQuestionAnswers" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="100%" Style="margin-bottom: 15px;"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnSurveyQuestionID" runat="server" Value='<%# Eval("SurveyQuestionID") %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Repeater ID="RepeaterSurveyEdit" runat="server" Visible="false">
                                                    <ItemTemplate>
                                                        <div>
                                                            <strong>
                                                                <asp:Label ID="lblSurveyQuestions" runat="server" Text='<%# Eval("SurveyQuestion") %>'></asp:Label></strong>
                                                            <asp:TextBox ID="txtSurveyQuestionAnswers" TextMode="MultiLine" runat="server" MaxLength="300"
                                                                Width="100%" Style="margin-bottom: 15px;" Text='<%# Eval("SurveyQuestionAnswer") %>'></asp:TextBox>
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
                                        height: 75px; resize: none;" TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false"></asp:TextBox>
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
                                                                    runat="server" OnCheckedChanged="rdbtnPayVisa_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayMasterCard" CssClass="noLM" Text="Mastercard" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayMasterCard_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayDiscover" CssClass="noLM" Text="Discover" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayDiscover_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayAmex" CssClass="noLM" Text="Amex" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayAmex_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnInvoice" CssClass="noLM" Text="Invoice" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnInvoice_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayCheck" CssClass="noLM" Text="Check" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayCheck_CheckedChanged" AutoPostBack="true" />
                                                        </h4>
                                                        <div class="h4 noB" style="left: 878px; top: -29px; display: inline-block; z-index: 11;
                                                            width: 200px; height: auto; padding: 0;">
                                                            <%--  <div class="date2">
                                                        <strong>Post date</strong></div>--%>
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
                                                                            <asp:LinkButton ID="lnkbtnCopySellerInfo" runat="server" Text="Copy name & address from dealer information"
                                                                                OnClientClick="return CopySellerInfo();" Style="color: Blue; text-decoration: underline;"></asp:LinkButton>
                                                                        </h5>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 135px">Card Holder
                                                                                First Name</strong>
                                                                            <asp:HiddenField ID="CardType" runat="server" />
                                                                            <asp:TextBox ID="txtCardholderName" runat="server" MaxLength="25" />
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 65px">Last Name</strong>
                                                                            <asp:TextBox ID="txtCardholderLastName" runat="server" MaxLength="25" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 77px">Credit Card
                                                                                #</strong>
                                                                            <asp:TextBox runat="server" ID="CardNumber" MaxLength="16" onkeypress="return isNumberKey(event)"
                                                                                onblur="return CreditCardOnblur();" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 65px">Expiry Date</strong>
                                                                            <asp:DropDownList ID="ExpMon" Style="width: 130px;" runat="server">
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
                                                                            </asp:DropDownList>
                                                                            /
                                                                            <asp:DropDownList ID="CCExpiresYear" Style="width: 120px" runat="server">
                                                                            </asp:DropDownList>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">CVV#</strong>
                                                                            <asp:TextBox ID="cvv" MaxLength="4" runat="server" onkeypress="return isNumberKey(event)"
                                                                                onblur="return CVVOnblur();" />
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
                                                                            <asp:TextBox ID="txtbillingaddress" runat="server" MaxLength="40"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">City</strong>
                                                                            <asp:TextBox ID="txtbillingcity" runat="server" MaxLength="40"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 45%; display: inline-block; float: left; margin-right: 10px;">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">State</strong>
                                                                                <asp:DropDownList ID="ddlbillingstate" runat="server" Style="width: 120px">
                                                                                </asp:DropDownList>
                                                                            </h4>
                                                                        </div>
                                                                        <div style="width: 45%; display: inline-block; float: left">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">ZIP</strong>
                                                                                <asp:TextBox ID="txtbillingzip" runat="server" Style="width: 74px" MaxLength="5"
                                                                                    class="sample4" onkeypress="return isNumberKey(event)" onblur="return billingZipOnblur();"></asp:TextBox>
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
                                                                            <asp:LinkButton ID="lnkbtnCopyCheckName" runat="server" Text="Copy name from dealer information"
                                                                                OnClientClick="return CopySellerInfoForCheck();" Style="color: Blue; text-decoration: underline;"></asp:LinkButton>
                                                                        </h5>
                                                                        <table style="width: 80%;">
                                                                            <tr>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 125px">Account holder
                                                                                            name</strong>
                                                                                        <asp:TextBox ID="txtCustNameForCheck" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 60px">Account #</strong>
                                                                                        <asp:TextBox ID="txtAccNumberForCheck" runat="server" MaxLength="20"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <strong style="width: 67px">Bank name:</strong>
                                                                                        <asp:TextBox ID="txtBankNameForCheck" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 60px">Routing #</strong>
                                                                                        <asp:TextBox ID="txtRoutingNumberForCheck" runat="server" MaxLength="9"></asp:TextBox>
                                                                                    </h4>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h4 class="h4">
                                                                                        <span class="star" style="color: Red">*</span><strong style="width: 76px">Account type</strong>
                                                                                        <asp:DropDownList ID="ddlAccType" runat="server">
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
                                                                        <h5 style="font-size: 12px; font-weight: normal; margin: 0; display: inline-block">
                                                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Copy name & address from dealer information"
                                                                                OnClientClick="return CopySellerInfoForInvoice();" Style="color: Blue; text-decoration: underline;"></asp:LinkButton>
                                                                        </h5>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 77px">Attention
                                                                                To </strong>
                                                                            <asp:TextBox runat="server" ID="txtAttentionTo" MaxLength="100" />
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="star" style="color: Red">*</span><strong>Send the Invoice by</strong>
                                                                        <h4 class="h4 noB" style="margin-left: 80px; width: 430px;">
                                                                            <asp:RadioButton ID="rdbtnInvoiceEmail" CssClass="noLM" Style="width: 40px" Text=""
                                                                                GroupName="InvoiceSend" runat="server" Checked="true" /><span class="featNon" style="width: 50px;">Email</span>
                                                                            <strong></strong>
                                                                            <asp:TextBox ID="txtInvoiceEmail" runat="server" Style="border: #ccc 1px solid;"
                                                                                MaxLength="60" onblur="return InvoiceEmailOnblur();"></asp:TextBox><br />
                                                                        </h4>
                                                                        <h4 class="h4 noB" style="margin-left: 80px; margin-top: 6px;">
                                                                            <asp:RadioButton ID="rdbtnInvoicePostal" Style="width: 40px" CssClass="noLM" Text=""
                                                                                GroupName="InvoiceSend" runat="server" /><span class="featNon" style="display: inline">Postal</span>
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
                                                                            <asp:TextBox ID="txtInvoiceBillingname" runat="server" MaxLength="30"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 45px">Address</strong>
                                                                            <asp:TextBox ID="tyxtInvoiceAddress" runat="server" MaxLength="40"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h4 class="h4">
                                                                            <span class="star" style="color: Red">*</span><strong style="width: 40px">City</strong>
                                                                            <asp:TextBox ID="txtInvoiceCity" runat="server" MaxLength="40"></asp:TextBox>
                                                                        </h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 45%; display: inline-block; float: left; margin-right: 10px;">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">State</strong>
                                                                                <asp:DropDownList ID="ddlInvoiceState" runat="server" Style="width: 120px">
                                                                                </asp:DropDownList>
                                                                            </h4>
                                                                        </div>
                                                                        <div style="width: 45%; display: inline-block; float: left">
                                                                            <h4 class="h4">
                                                                                <span class="star" style="color: Red">*</span><strong style="width: 40px">ZIP</strong>
                                                                                <asp:TextBox ID="txtInvoiceZip" runat="server" Style="width: 74px" MaxLength="5"
                                                                                    class="sample4" onkeypress="return isNumberKey(event)" onblur="return billingZipOnblur();"></asp:TextBox>
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
                                                                <asp:DropDownList ID="ddlPaymentDate" runat="server" onchange="ChangeValuesHidden()"
                                                                    Width="120px">
                                                                </asp:DropDownList>
                                                            </h4>
                                                        </td>
                                                        <td style="width: 100px; vertical-align: bottom">
                                                            <h4 class="h4 non">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 55px">Amount $</strong>
                                                                <asp:TextBox ID="txtPDAmountNow" runat="server" MaxLength="6" onkeypress="return isNumberKeyWithDot(event)"
                                                                    onkeyup="return ChangeValuesHidden()" Width="200px"></asp:TextBox>
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
                                                                <asp:TextBox ID="txtVoicefileConfirmNo" runat="server" MaxLength="30"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td width="40%">
                                                            <h4 class="h4">
                                                                <strong style="width: 150px; font-size: 15px;">Voice file Location:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:DropDownList ID="ddlVoiceFileLocation" runat="server">
                                                                </asp:DropDownList>
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
                                                                <asp:DropDownList ID="ddlContractDate" runat="server" onchange="ChangeValuesHidden()">
                                                                </asp:DropDownList>
                                                            </h4>
                                                        </td>
                                                        <td width="40%">
                                                            <h4 class="h4">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 150px; font-size: 15px;">
                                                                    Contract status:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:DropDownList ID="ddlContractStatus" runat="server">
                                                                    <asp:ListItem Value="0">None</asp:ListItem>
                                                                    <asp:ListItem Value="1">Received</asp:ListItem>
                                                                    <asp:ListItem Value="2">Not Received</asp:ListItem>
                                                                </asp:DropDownList>
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
                                    </table>
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
    </form>
</body>
</html>
