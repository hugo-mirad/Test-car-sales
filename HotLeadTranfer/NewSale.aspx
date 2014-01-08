<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewSale.aspx.cs" Inherits="NewSale" %>

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
        
        
        
            var result =[];
        //$('#grdTest').onload(function(){ alert('Ok') })
        console.log($('#grdTest tr').length);
        if($('#grdTest tr').length >0 ){
        for(i=0; i<$('#grdTest tr').length; i++){
            var cc = [];
            cc.push($.trim($('#grdTest tr:eq('+i+') td:eq(0) span').text()));
            cc.push($.trim($('#grdTest tr:eq('+i+') td:eq(1) span').text()));
            result.push(cc);
        }
        //console.log(result)
        
        
        var centers = [];
        var centersUniq = [];
        var agents = [];
        var finalArray2 = [];
        var finalArray = [];
        var menu = '';  
        
        for (i=0; i<result.length; i++){       
            centersUniq.push($.trim(result[i][1]));          
        }
         
        finalArray =  result;    
        centersUniq = unique1(centersUniq);
                
        for(i=0; i<centersUniq.length; i++){
            var cent = $.trim(centersUniq[i]);            
            var dum = [];
            var counter = 0;
            for(k=0; k<finalArray.length; k++){
                if(centersUniq[i] == $.trim(finalArray[k][1])){
                    var cn = [];
                    dum.push($.trim(finalArray[k][0])) ;             
                    counter++;
                }
            }            
            var dum2 = [];
            dum2.push(cent);
            dum2.push(dum);
            dum2.push(counter);            
            finalArray2.push(dum2);       
        }
        var text = $('#lblTransferAgentsCount').text(); 
        menu = '';
        menu += "<div id='smoothmenu1' class='ddsmoothmenu'><ul><li><a href='#'>"+text+"</a><ul>";
        for(i=0;i<finalArray2.length;i++){
            menu += '<li><a href="#" >'+finalArray2[i][0]+' ('+finalArray2[i][2]+')</a>';
            if(finalArray2[i][1].length > 0){                
                for(k=0;k<finalArray2[i][1].length;k++){
                    if(k==0){menu += '<ul>';}
                    menu += '<li><a href="#" >'+finalArray2[i][1][k]+'</a></li>';
                    if(k==(finalArray2[i][1].length-1)){menu += '</ul>';}
                }                
            }
            menu += '</li>';            
        } 
      
        menu += "</ul></li></ul></div>";  
        $('#lblTransferAgentsCount').empty().append(menu);        
        $('<a href="#"></a>').remove();
        
        ddsmoothmenu.init({
	        mainmenuid: "smoothmenu1", //menu DIV id
	        orientation: 'h', //Horizontal or vertical menu: Set to "h" or "v"
	        classname: 'ddsmoothmenu', //class added to menu's outer DIV
	        //customtheme: ["#1c5a80", "#18374a"],
	        contentsource: "markup" //"markup" or ["container_id", "path_to_menu_file"]
        });     
        }  
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
		
		
		$('.sample4').numeric();	
		
		
		$('#txtAskingPrice').live('blur',function(){
		    if($('#txtAskingPrice').val().length >0){      
                 if($('#txtAskingPrice').val() == 0)
                 {
                    alert('Enter valid price');
                    $('#txtAskingPrice').focus();
                     return false;
                 }
              }
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
		     if($('#txtMileage').val()< 1000){ 
		     alert('Mileage must be greater than 1000')
		     $('#txtMileage').focus();
		     return false;
		     }   
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

    <script type="text/javascript" language="javascript">
    function CopySellerInfo()
        {
         
            var valid=true;   
                if (document.getElementById('<%= txtFirstName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtFirstName.ClientID%>').focus();
                alert("Enter customer first name");
                document.getElementById('<%=txtFirstName.ClientID%>').value = ""
                document.getElementById('<%=txtFirstName.ClientID%>').focus()                
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
                
                 document.getElementById('<%= txtCardholderName.ClientID%>').value =  document.getElementById('<%= txtFirstName.ClientID%>').value;                
                 document.getElementById('<%= txtCardholderLastName.ClientID%>').value =  document.getElementById('<%= txtLastName.ClientID%>').value;
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
                if (document.getElementById('<%= txtFirstName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtFirstName.ClientID%>').focus();
                alert("Enter customer first name");
                document.getElementById('<%=txtFirstName.ClientID%>').value = ""
                document.getElementById('<%=txtFirstName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }   
              else
              {
                
                 document.getElementById('<%= txtCustNameForCheck.ClientID%>').value =  document.getElementById('<%= txtFirstName.ClientID%>').value;                            
                           
              }             
              return valid;      
        } 
            
          
    </script>

    <script type="text/javascript" language="javascript">
    function ValidatePhone(){
            if (document.getElementById('<%= txtLoadPhone.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtLoadPhone.ClientID%>').focus();
                alert("Enter customer phone number");
                document.getElementById('<%=txtLoadPhone.ClientID%>').value = ""
                document.getElementById('<%=txtLoadPhone.ClientID%>').focus()                
                valid = false;
                 return valid;     
             }    
             if((document.getElementById('<%= txtLoadPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtLoadPhone.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtLoadPhone.ClientID%>').focus();
                document.getElementById('<%=txtLoadPhone.ClientID%>').value = "";
                alert("Enter valid phone number");
                valid = false; 
                 return valid;                
            
              }  
    }
    
     function ValidateAbandonData()
      {
         var valid = true;        
               
//              if (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length < 1) {
//                document.getElementById('<%= txtPhone.ClientID%>').focus();
//                alert("Enter customer phone number");
//                document.getElementById('<%=txtPhone.ClientID%>').value = ""
//                document.getElementById('<%=txtPhone.ClientID%>').focus()                
//                valid = false;
//                 return valid;     
//             }    
             if((document.getElementById('<%= txtPhone.ClientID%>').value.trim().length > 0) && (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length < 10)) {
                document.getElementById('<%= txtPhone.ClientID%>').focus();
                document.getElementById('<%=txtPhone.ClientID%>').value = "";
                alert("Enter valid phone number");
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
               
             return valid;
      }
    </script>

    <script type="text/javascript" language="javascript">
     function ValidateDraftData()
      {
         var valid = true;        
               
              if (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtPhone.ClientID%>').focus();
                alert("Phone number is mandatory to save");
                document.getElementById('<%=txtPhone.ClientID%>').value = ""
                document.getElementById('<%=txtPhone.ClientID%>').focus()                
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
             if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
             {               
                document.getElementById('<%=txtEmail.ClientID%>').value = ""
                document.getElementById('<%=txtEmail.ClientID%>').focus()
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
               
             return valid;
      }
    </script>

    <script type="text/javascript" language="javascript">
    
      function ValidateSavedData()
      {
         var valid = true;         
               
                if(document.getElementById('<%= ddlPackage.ClientID%>').value == "0") {
                document.getElementById('<%= ddlPackage.ClientID%>').focus();
                alert("Select package");                 
                document.getElementById('<%=ddlPackage.ClientID%>').focus()
                valid = false;            
                 return valid;     
               }  
               if (document.getElementById('<%= txtFirstName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtFirstName.ClientID%>').focus();
                alert("Enter customer first name");
                document.getElementById('<%=txtFirstName.ClientID%>').value = ""
                document.getElementById('<%=txtFirstName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              } 
                if (document.getElementById('<%= txtLastName.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtLastName.ClientID%>').focus();
                alert("Enter customer last name");
                document.getElementById('<%=txtLastName.ClientID%>').value = ""
                document.getElementById('<%=txtLastName.ClientID%>').focus()                
                valid = false;
                 return valid;     
              }   
               
              if (document.getElementById('<%= txtPhone.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtPhone.ClientID%>').focus();
                alert("Enter customer phone number");
                document.getElementById('<%=txtPhone.ClientID%>').value = ""
                document.getElementById('<%=txtPhone.ClientID%>').focus()                
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
           
                 if(document.getElementById('<%= chkbxEMailNA.ClientID%>').checked == false)
                {
                      if (document.getElementById('<%= txtEmail.ClientID%>').value.trim().length < 1) {
                        document.getElementById('<%= txtEmail.ClientID%>').focus();
                        alert("Enter customer email");
                        document.getElementById('<%=txtEmail.ClientID%>').value = ""
                        document.getElementById('<%=txtEmail.ClientID%>').focus()                
                        valid = false;
                         return valid;     
                     } 
                 }
            
              if ((document.getElementById('<%=txtEmail.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtEmail.ClientID%>').value.trim()) == false) )
             {               
                document.getElementById('<%=txtEmail.ClientID%>').value = ""
                document.getElementById('<%=txtEmail.ClientID%>').focus()
                valid = false;               
                return valid;     
             }         
              if (document.getElementById('<%= txtAddress.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtAddress.ClientID%>').focus();
                alert("Enter customer address");
                document.getElementById('<%=txtAddress.ClientID%>').value = ""
                document.getElementById('<%=txtAddress.ClientID%>').focus()                
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
             if(document.getElementById('<%=ddlMake.ClientID%>').value =="0")
            {
                alert("Please select make"); 
                valid=false;
                document.getElementById('ddlMake').focus();  
                return valid;               
            }             
             if(document.getElementById('<%=ddlModel.ClientID%>').value =="0")
            {
                alert("Please select model"); 
                valid=false;
                document.getElementById('ddlModel').focus();
                return valid;               
            } 
            if (document.getElementById('<%=ddlYear.ClientID%>').value =="0")
            {
                alert('Please select year')
                valid=false;
                document.getElementById('ddlYear').focus();  
                return valid;
            }               
              if (document.getElementById('<%= txtAskingPrice.ClientID%>').value.trim().length < 1) {
                document.getElementById('<%= txtAskingPrice.ClientID%>').focus();
                alert("Enter price");
                document.getElementById('<%=txtAskingPrice.ClientID%>').value = ""
                document.getElementById('<%=txtAskingPrice.ClientID%>').focus()                
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
            if(document.getElementById('<%= rdbtnPayPaypal.ClientID%>').checked == true)
            {                   
                    if(document.getElementById('<%= txtPaytransID.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtPaytransID.ClientID%>').focus();
                    alert("Enter Payment Trans ID");
                    document.getElementById('<%=txtPaytransID.ClientID%>').value = ""
                    document.getElementById('<%=txtPaytransID.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }    

                    if(document.getElementById('<%= txtpayPalEmailAccount.ClientID%>').value.length < 1) {
                    document.getElementById('<%= txtpayPalEmailAccount.ClientID%>').focus();
                    alert("Enter paypal account email");
                    document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').value = ""
                    document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }            
                    if ((document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').value.length > 0) && (echeck(document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').value) == false) )
                    {               
                    document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').value = ""
                    document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').focus()
                    valid = false;               
                    return valid;     
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
            
            
            if(document.getElementById('<%= txtVoicefileConfirmNo.ClientID%>').value.length < 1) {
            document.getElementById('<%= txtVoicefileConfirmNo.ClientID%>').focus();
            alert("Enter voice file confirmation #");
            document.getElementById('<%=txtVoicefileConfirmNo.ClientID%>').value = ""
            document.getElementById('<%=txtVoicefileConfirmNo.ClientID%>').focus()
            valid = false;            
            return valid;     
            }    

            if(document.getElementById('<%=ddlVoiceFileLocation.ClientID%>').value =="0")
            {
            alert("Please select voice file location"); 
            valid=false;
            document.getElementById('ddlVoiceFileLocation').focus();  
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

            if(EnterAmt > selectedPack-curr1){
            document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
            alert("Amount more than selected package price");
            document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
            document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
            valid = false; 
            return valid;     
            } 
            
             if(document.getElementById('<%= chkboxlstPDsale.ClientID%>').checked == false)
            {
              
                    if(EnterAmt < selectedPack-curr1){
                    document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
                    alert("Amount less than selected package price");
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
                    valid = false; 
                    return valid;     
                    }    
            }                 
            if(document.getElementById('<%= chkboxlstPDsale.ClientID%>').checked == true)
            {
                    var string1 = $('#ddlDiscount option:selected').val();
                    var curr1 ;
                    if(string1==0)curr1="0";
                    else  if(string1==1)curr1="25"; 
                    else  if(string1==2)curr1="50"; 
                
                    if(document.getElementById('<%=ddlPDDate.ClientID%>').value =="0")
                    {
                    alert("Please select PD date"); 
                    valid=false;
                    document.getElementById('ddlPDDate').focus();  
                    return valid;               
                    }  
                    var EnterAmt = parseFloat($('#txtPDAmountNow').val());

                    if(document.getElementById('<%= txtPDAmountNow.ClientID%>').value.trim().length < 1) {
                    document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
                    alert("Enter amount being paid now");
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').value = ""
                    document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
                    valid = false;            
                    return valid;     
                    }    

                    if(EnterAmt > selectedPack-curr1){
                    document.getElementById('<%= txtPDAmountNow.ClientID%>').focus();
                    alert("Amount more than selected package price");
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
     
      function PhoneOnfocus()
     {           
              var phone = document.getElementById('<%= txtPhone.ClientID%>').value;
               formatted =phone.replace("-","");
               formatted =formatted.replace("-","");
                document.getElementById('<%=txtPhone.ClientID%>').value = formatted;            
                       
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
     function PaypalEmailblur()
     {           
         if ((document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').value.trim().length > 0) && (echeck(document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').value.trim()) == false) )
         {                           
            document.getElementById('<%=txtpayPalEmailAccount.ClientID%>').focus()
            valid = false;               
            return valid;     
         }              
     }
     function EmailNAClick(){
     var checkbox = document.getElementById("chkbxEMailNA");
      if(checkbox.checked){        
        document.getElementById('<%= txtEmail.ClientID%>').disabled  = true;            
      }
      else
      {
         document.getElementById('<%= txtEmail.ClientID%>').disabled  = false;
      }
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
             
                  //for selected discount amount
                var string1 = $('#ddlDiscount option:selected').val();
                var curr1 ;
                if(string1==0)curr1="0";
                else  if(string1==1)curr1="25"; 
                else  if(string1==2)curr1="50"; 
                curr1=parseFloat(curr1);
                var curr2=(selectedPack-curr1);
                curr2=curr2.toFixed(2);
                $('#txtPDAmountNow').val(curr2);
                 $('#txtTotalAmount').val(curr2);
                  if(document.getElementById('<%= chkboxlstPDsale.ClientID%>').checked == true)
                  {
                    $('#txtPDAmountLater').val('0.00');
                  }
                  else
                  {
                     $('#txtPDAmountLater').val('');
                  }
                }else{
                 $('#txtPDAmountNow').val('');
                 $('#txtTotalAmount').val('');
                  if(document.getElementById('<%= chkboxlstPDsale.ClientID%>').checked == true)
                  {
                    $('#txtPDAmountLater').val('');
                  }
                   else
                  {
                     $('#txtPDAmountLater').val('');
                  }
                }                            
                      
            }           
    /*
    $('#txtPDAmountNow').live('keydown', function(){
        //console.log($(this).val())
        $(this).val($(this).val().toString().replace(/^[0-9]\./g, ',').replace(/\./g, ''));
    });
    */
    
    
             
    $('#txtPDAmountNow').live('blur', function(){
            $('#txtPDAmountLater').val('');
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
               
                //for selected discount amount
                  var string1 = $('#ddlDiscount option:selected').val();
                  var curr1 ;
                if(string1==0)curr1="0";
                else  if(string1==1)curr1="25"; 
                
               // console.log(curr1);
                   var finalpack =(selectedPack-curr1);
                   //finalpack=finalpack.toFixed(2);
               
                if(finalpack < curr){
              
                    alert('Entered amount can not be graterthen selected package..')
                     document.getElementById('<%=txtPDAmountNow.ClientID%>').focus()
                }else{
                    var value = parseFloat(selectedPack-curr-curr1);
                    value = value.toFixed(2); 
                    $('#txtPDAmountLater').val(value);
                    var amntval=selectedPack- curr1;
                    amntval=amntval.toFixed(2);
                    $('#txtTotalAmount').val(amntval);
                }                            
                      
            }            
    });
    
    function  OnchangeCheckDiscount() 
    {
    if( $('#ddlPackage option:selected').text() == 'Gold Deluxe ($199.99)')
    $('#discount').show();
    else
    {
     $('#ddlDiscount').val(0);
     $('#discount').hide();
     }
   return this
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
   }    </script>

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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updatePnlSave2"
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
                    UNITED CAR EXCHANGE <span>Sales Order Form</span></h1>
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
                                    <asp:LinkButton ID="lnkbtnDealerSale" runat="server" Text="New dealer sale" OnClick="lnkbtnDealerSale_Click"
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
        <table style="display: none" id="tblAgents" runat="server">
            <tr>
                <td>
                    <asp:GridView ID="grdTest" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblGridAgnetName" runat="server" Text='<%# Eval("AgentUFirstName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblGridAgentCenter" runat="server" Text='<%# Eval("AgentCenterCode")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td width="400px;">
                    <asp:Label ID="lblTransferAgentsCount" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="400px">
                    <h4 class="h4">
                        <span class="star" style="color: Red">*</span><strong style="width: 45px;">Package:</strong>
                        <%-- <input type="text" style="width: 245px" />--%>
                        <asp:DropDownList ID="ddlPackage" runat="server" onchange="return OnchangeCheckDiscount()">
                        </asp:DropDownList>
                    </h4>
                </td>
                <td style="text-align: right">
                    <asp:UpdatePanel ID="updtpnlSave" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnTransfer" runat="server" CssClass="g-button g-button-submit" Text="Transfer"
                                OnClientClick="return ValidateDraftData();" OnClick="btnTransfer_Click" />
                            &nbsp;
                            <asp:Button ID="btnSale" runat="server" CssClass="g-button g-button-submit" Text="Sale"
                                OnClientClick="return ValidateSavedData();" OnClick="btnSale_Click" />
                            &nbsp;
                            <asp:Button ID="btnSavedraft" runat="server" CssClass="g-button g-button-submit"
                                Text="Save as draft" OnClick="btnSavedraft_Click" OnClientClick="return ValidateDraftData();" />
                            &nbsp;
                            <asp:Button ID="btnAbandon" runat="server" CssClass="g-button g-button-submit" Text="Abandon"
                                OnClick="btnAbandon_Click" OnClientClick="return ValidateAbandonData();" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
            
                <td  width="400px;">
                <span id="discount" style="display:none;">
                    <h4 class="h4">
                        <strong style="width: 45px;">Discount:</strong>
                        <asp:DropDownList ID="ddlDiscount" runat="server" onchange="return OnchangeDropdown()">
                        </asp:DropDownList>
                    </h4>
                    </span>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="400px;">
                    <h4 class="h4">
                        <strong style="width: 45px;">Agent:</strong>
                        <asp:DropDownList ID="ddlSaleAgent" runat="server" onchange="return OnchangeDropdown()">
                        </asp:DropDownList>
                    </h4>
                </td>
                <td>
                   <h4 class="h4">
                        <strong style="width: 45px;">Verifier:</strong>
                        <asp:DropDownList ID="ddlVerifier" runat="server" onchange="return OnchangeDropdown()">
                        </asp:DropDownList>
                    </h4>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updtpnl" runat="server">
            <ContentTemplate>
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
                                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="30"></asp:TextBox>
                                            </h4>
                                        </td>
                                        <td>
                                            <h4 class="h4">
                                                <span class="star" style="color: Red">*</span> <strong style="width: 65px">Last name:</strong>
                                                <%--<input type="text" style="width: 380px" />--%>
                                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="30"></asp:TextBox>
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
                                                                onblur="return PhoneOnblur();" onfocus="return PhoneOnfocus();"></asp:TextBox>
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
                                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="60" onblur="return EmailOnblur();"></asp:TextBox>
                                                            <asp:CheckBox ID="chkbxEMailNA" runat="server" Text="NA" Font-Bold="true" onclick="return EmailNAClick();" />
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
                                                <asp:TextBox ID="txtAddress" runat="server" MaxLength="40"></asp:TextBox>
                                            </h4>
                                        </td>
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
                                                                onchange="ChangeValuesHidden()">
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
                                                            <asp:DropDownList ID="ddlModel" runat="server" onchange="ChangeValuesHidden()">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Year:</strong>
                                                    <asp:DropDownList ID="ddlYear" runat="server">
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
                                                                <asp:TextBox ID="txtAskingPrice" runat="server" MaxLength="6" class="sample4" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td style="width: 190px">
                                                            <h4 class="h4">
                                                                <strong style="width: 40px">Mileage:</strong>
                                                                <%-- <input type="text" style="width: 119px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtMileage" runat="server" MaxLength="6" class="sample4" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td valign="bottom">
                                                            <h4 class="h4 noB">
                                                                <strong style="width: 50px">Cylinders:</strong><span style="font-weight: bold">
                                                                    <asp:RadioButton ID="rdbtnCylinders1" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" /><span class="featNon">3</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders2" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" /><span class="featNon">4</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders3" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" /><span class="featNon">5</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders4" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" /><span class="featNon">6</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders5" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" /><span class="featNon">7</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders6" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" /><span class="featNon">8</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders7" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Checked="true" /><span class="featNon">NA</span> </span>
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
                                                    <asp:DropDownList ID="ddlBodyStyle" runat="server" onchange="ChangeValuesHidden()">
                                                    </asp:DropDownList>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 80px">Exterior color:</strong>
                                                    <%--<input type="text" style="width: 224px" />--%>
                                                    <asp:DropDownList ID="ddlExteriorColor" runat="server" onchange="ChangeValuesHidden()">
                                                    </asp:DropDownList>
                                                </h4>
                                            </td>
                                            <td>
                                                <h4 class="h4">
                                                    <strong style="width: 80px">Interior color:</strong>
                                                    <%--<input type="text" style="width: 170px" />--%>
                                                    <asp:DropDownList ID="ddlInteriorColor" runat="server" onchange="ChangeValuesHidden()">
                                                    </asp:DropDownList>
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
                                                                        runat="server" /><span class="featNon">Auto</span>
                                                                    <asp:RadioButton ID="rdbtnTrans2" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" /><span class="featNon">Manual</span>
                                                                    <asp:RadioButton ID="rdbtnTrans3" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" /><span class="featNon">Tiptronic</span>
                                                                    <asp:RadioButton ID="rdbtnTrans4" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Checked="true" /><span class="featNon">NA</span> </span>
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
                                                                    <asp:RadioButton ID="rdbtnDoor2" CssClass="noLM" Text="" GroupName="Doors" runat="server" /><span
                                                                        class="featNon">2</span>
                                                                    <asp:RadioButton ID="rdbtnDoor3" CssClass="noLM" Text="" GroupName="Doors" runat="server" /><span
                                                                        class="featNon">3</span>
                                                                    <asp:RadioButton ID="rdbtnDoor4" CssClass="noLM" Text="" GroupName="Doors" runat="server" /><span
                                                                        class="featNon">4</span>
                                                                    <asp:RadioButton ID="rdbtnDoor5" CssClass="noLM" Text="" GroupName="Doors" runat="server" /><span
                                                                        class="featNon">5</span>
                                                                    <asp:RadioButton ID="rdbtnDoor6" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Checked="true" /><span class="featNon">NA</span> </span>
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
                                                                        runat="server" /><span class="featNon">2WD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain2" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" /><span class="featNon">FWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain3" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" /><span class="featNon">AWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain4" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" /><span class="featNon">RWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain5" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Checked="true" /><span class="featNon">NA</span> </span>
                                                            </h4>
                                                        </td>
                                                        <td>
                                                            <h4 class="h4">
                                                                <strong>VIN #:</strong>
                                                                <%--<input type="text" style="width: 409px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtVin" runat="server" Style="width: 409px" MaxLength="20"></asp:TextBox>
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
                                                        <asp:RadioButton ID="rdbtnFuelType1" CssClass="noLM" Text="" GroupName="Fuels" runat="server" /><span
                                                            class="featNon">Diesel</span>
                                                        <asp:RadioButton ID="rdbtnFuelType2" CssClass="noLM" Text="" GroupName="Fuels" runat="server" /><span
                                                            class="featNon">Petrol</span>
                                                        <asp:RadioButton ID="rdbtnFuelType3" CssClass="noLM" Text="" GroupName="Fuels" runat="server" /><span
                                                            class="featNon">Hybrid</span>
                                                        <asp:RadioButton ID="rdbtnFuelType4" CssClass="noLM" Text="" GroupName="Fuels" runat="server" /><span
                                                            class="featNon">Electric</span>
                                                        <asp:RadioButton ID="rdbtnFuelType5" CssClass="noLM" Text="" GroupName="Fuels" runat="server" /><span
                                                            class="featNon">Gasoline</span>
                                                        <asp:RadioButton ID="rdbtnFuelType6" CssClass="noLM" Text="" GroupName="Fuels" runat="server" /><span
                                                            class="featNon">E-85</span>
                                                        <asp:RadioButton ID="rdbtnFuelType7" CssClass="noLM" Text="" GroupName="Fuels" runat="server" /><span
                                                            class="featNon">Gasoline-Hybrid</span>
                                                        <asp:RadioButton ID="rdbtnFuelType8" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Checked="true" /><span class="featNon">NA</span> </span>
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
                                                            runat="server" /><span class="featNon">Excellent</span>
                                                        <asp:RadioButton ID="rdbtnCondition2" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" /><span class="featNon">Very good</span>
                                                        <asp:RadioButton ID="rdbtnCondition3" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" /><span class="featNon">Good</span>
                                                        <asp:RadioButton ID="rdbtnCondition4" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" /><span class="featNon">Fair</span>
                                                        <asp:RadioButton ID="rdbtnCondition5" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" /><span class="featNon">Poor</span>
                                                        <asp:RadioButton ID="rdbtnCondition6" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" /><span class="featNon">Parts or salvage</span>
                                                        <asp:RadioButton ID="rdbtnCondition7" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Checked="true" /><span class="featNon">NA</span> </span>
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
                                                <asp:CheckBox ID="chkFeatures51" runat="server" class="noLM" />
                                                <span class="featNon">A/C</span>
                                                <asp:CheckBox ID="chkFeatures1" runat="server" class="noLM" />
                                                <span class="featNon">A/C: Front</span>
                                                <asp:CheckBox ID="chkFeatures2" runat="server" class="noLM" />
                                                <span class="featNon">A/C: Rear</span>
                                                <asp:CheckBox ID="chkFeatures3" runat="server" class="noLM" />
                                                <span class="featNon">Cruise control</span>
                                                <asp:CheckBox ID="chkFeatures4" runat="server" class="noLM" />
                                                <span class="featNon">Navigation system</span>
                                                <asp:CheckBox ID="chkFeatures5" runat="server" class="noLM" />
                                                <span class="featNon">Power locks</span>
                                                <asp:CheckBox ID="chkFeatures6" runat="server" class="noLM" />
                                                <span class="featNon">Power steering</span>
                                                <br />
                                                <asp:CheckBox ID="chkFeatures7" runat="server" class="noLM" />
                                                <span class="featNon">Remote keyless entry</span>
                                                <asp:CheckBox ID="chkFeatures8" runat="server" class="noLM" />
                                                <span class="featNon">TV/VCR</span>
                                                <asp:CheckBox ID="chkFeatures31" runat="server" class="noLM" />
                                                <span class="featNon">Remote start</span>
                                                <asp:CheckBox ID="chkFeatures33" runat="server" class="noLM" />
                                                <span class="featNon">Tilt</span>
                                                <asp:CheckBox ID="chkFeatures35" runat="server" class="noLM" />
                                                <span class="featNon">Rearview camera</span>
                                                <asp:CheckBox ID="chkFeatures36" runat="server" class="noLM" />
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
                                                <asp:CheckBox ID="chkFeatures9" runat="server" class="noLM" />
                                                <span class="featNon">Bucket seats</span>
                                                <asp:CheckBox ID="chkFeatures11" runat="server" />
                                                <span class="featNon">Memory seats</span>
                                                <asp:CheckBox ID="chkFeatures12" runat="server" />
                                                <span class="featNon">Power seats</span>
                                                <asp:CheckBox ID="chkFeatures32" runat="server" />
                                                <span class="featNon">Heated seats</span>
                                                <br />
                                                <b style="color: #222; display: inline-block; margin-left: 163px;">Interior</b>
                                                :
                                                <asp:RadioButton ID="rdbtnLeather" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" /><span class="featNon">Leather</span>
                                                <asp:RadioButton ID="rdbtnVinyl" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" /><span class="featNon">Vinyl</span>
                                                <asp:RadioButton ID="rdbtnCloth" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" /><span class="featNon">Cloth</span>
                                                <asp:RadioButton ID="rdbtnInteriorNA" runat="server" CssClass="noLM" GroupName="Seats"
                                                    Text="" Checked="true" /><span class="featNon">NA</span>
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
                                                <asp:CheckBox ID="chkFeatures13" runat="server" class="noLM" />
                                                <span class="featNon">Airbag: Driver</span>
                                                <asp:CheckBox ID="chkFeatures14" runat="server" class="noLM" />
                                                <span class="featNon">Airbag: Passenger</span>
                                                <asp:CheckBox ID="chkFeatures15" runat="server" class="noLM" />
                                                <span class="featNon">Airbag: Side</span>
                                                <asp:CheckBox ID="chkFeatures16" runat="server" class="noLM" />
                                                <span class="featNon">Alarm</span>
                                                <asp:CheckBox ID="chkFeatures17" runat="server" class="noLM" />
                                                <span class="featNon">Anti-lock brakes</span>
                                                <asp:CheckBox ID="chkFeatures18" runat="server" class="noLM" />
                                                <span class="featNon">Fog lights</span>
                                                <asp:CheckBox ID="chkFeatures39" runat="server" class="noLM" />
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
                                                <asp:CheckBox ID="chkFeatures19" runat="server" class="noLM" />
                                                <span class="featNon">Cassette radio</span>
                                                <asp:CheckBox ID="chkFeatures20" runat="server" class="noLM" />
                                                <span class="featNon">CD changer</span>
                                                <asp:CheckBox ID="chkFeatures21" runat="server" class="noLM" />
                                                <span class="featNon">CD player</span>
                                                <asp:CheckBox ID="chkFeatures22" runat="server" class="noLM" />
                                                <span class="featNon">Premium sound</span>
                                                <asp:CheckBox ID="chkFeatures34" runat="server" class="noLM" />
                                                <span class="featNon">AM/FM</span>
                                                <asp:CheckBox ID="chkFeatures40" runat="server" class="noLM" />
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
                                                <asp:CheckBox ID="chkFeatures44" runat="server" class="noLM" />
                                                <span class="featNon">Battery</span>
                                                <asp:CheckBox ID="chkFeatures45" runat="server" class="noLM" />
                                                <span class="featNon">Tires</span>
                                                <asp:CheckBox ID="chkFeatures52" runat="server" class="noLM" />
                                                <span class="featNon">Rotors</span>
                                                <asp:CheckBox ID="chkFeatures53" runat="server" class="noLM" />
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
                                                <asp:CheckBox ID="chkFeatures23" runat="server" class="noLM" />
                                                <span class="featNon">Power windows</span>
                                                <asp:CheckBox ID="chkFeatures24" runat="server" class="noLM" />
                                                <span class="featNon">Rear window defroster</span>
                                                <asp:CheckBox ID="chkFeatures25" runat="server" class="noLM" />
                                                <span class="featNon">Rear window wiper</span>
                                                <asp:CheckBox ID="chkFeatures26" runat="server" class="noLM" />
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
                                                <asp:CheckBox ID="chkFeatures27" runat="server" class="noLM" />
                                                <span class="featNon">Alloy wheels</span>
                                                <asp:CheckBox ID="chkFeatures28" runat="server" class="noLM" />
                                                <span class="featNon">Sunroof</span>
                                                <asp:CheckBox ID="chkFeatures41" runat="server" class="noLM" />
                                                <span class="featNon">Panoramic roof</span>
                                                <asp:CheckBox ID="chkFeatures42" runat="server" class="noLM" />
                                                <span class="featNon">Moonroof</span>
                                                <asp:CheckBox ID="chkFeatures29" runat="server" class="noLM" />
                                                <span class="featNon">Third row seats</span>
                                                <asp:CheckBox ID="chkFeatures30" runat="server" class="noLM" />
                                                <span class="featNon">Tow package</span>
                                                <br />
                                                <asp:CheckBox ID="chkFeatures43" runat="server" class="noLM" />
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
                                                <asp:CheckBox ID="chkFeatures46" runat="server" class="noLM" />
                                                <span class="featNon">Garage kept</span>
                                                <asp:CheckBox ID="chkFeatures47" runat="server" class="noLM" />
                                                <span class="featNon">Non smoking</span>
                                                <asp:CheckBox ID="chkFeatures48" runat="server" class="noLM" />
                                                <span class="featNon">Records/Receipts kept</span>
                                                <asp:CheckBox ID="chkFeatures49" runat="server" class="noLM" />
                                                <span class="featNon">Well maintained</span>
                                                <asp:CheckBox ID="chkFeatures50" runat="server" class="noLM" />
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
                                        height: 75px; resize: none;" TextMode="MultiLine" CssClass="textAr" data-plus-as-tab="false"></asp:TextBox>
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
                                        Style="width: 99%; height: 105px; resize: none;" CssClass="textAr" data-plus-as-tab="false"> </asp:TextBox>
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
                                                <asp:DropDownList ID="ddlPhotosSource" runat="server" onchange="ChangeValuesHidden()">
                                                </asp:DropDownList>
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
                                                <asp:DropDownList ID="ddlDescriptionSource" runat="server">
                                                </asp:DropDownList>
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
                                                                    runat="server" OnCheckedChanged="rdbtnPayVisa_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayMasterCard" CssClass="noLM" Text="Mastercard" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayMasterCard_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayDiscover" CssClass="noLM" Text="Discover" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayDiscover_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayAmex" CssClass="noLM" Text="Amex" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayAmex_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rdbtnPayPaypal" CssClass="noLM" Text="Paypal" GroupName="PayType"
                                                                    runat="server" OnCheckedChanged="rdbtnPayPaypal_CheckedChanged" AutoPostBack="true" />
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
                                                                            <asp:LinkButton ID="lnkbtnCopySellerInfo" runat="server" Text="Copy name & address from Seller Information"
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
                                                                            <asp:LinkButton ID="lnkbtnCopyCheckName" runat="server" Text="Copy name from Seller Information"
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
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h5 style="display: inline-block; margin: 0; font-size: 15px;">
                                                                            Paypal Details</h5>
                                                                        <div runat="server" style="width: 80%;">
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
                                                                                            <asp:TextBox ID="txtPaytransID" runat="server" MaxLength="30"></asp:TextBox>
                                                                                        </h4>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <h4 class="h4">
                                                                                            <span class="star" style="color: Red">*</span><strong style="width: 140px">Paypal account
                                                                                                email:</strong>
                                                                                            <%-- <input type="text" style="width: 245px" />--%>
                                                                                            <asp:TextBox ID="txtpayPalEmailAccount" runat="server" onblur="return PaypalEmailblur();"></asp:TextBox>
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
                                                                <asp:TextBox ID="txtPaymentDate" runat="server" ReadOnly="true"></asp:TextBox>
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
                                                    <tr>
                                                        <td style="padding: 0; margin: 0; vertical-align: middle">
                                                            <asp:CheckBox ID="chkboxlstPDsale" runat="server" CssClass="noLM" />
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <b>Post Dated Payment</b>
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <h4 class="h4 ">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 35px">Date</strong>
                                                                <asp:DropDownList ID="ddlPDDate" runat="server" onchange="ChangeValuesHidden()" Width="120px"
                                                                    ForeColor="Red">
                                                                </asp:DropDownList>
                                                            </h4>
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <h4 class="h4 non">
                                                                <strong style="width: 55px">Amount $</strong>
                                                                <asp:TextBox ID="txtPDAmountLater" runat="server" MaxLength="6" onkeypress="return isNumberKeyWithDot(event)"
                                                                    onkeyup="return ChangeValuesHidden()" Enabled="false" Width="200px"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <h4 class="h4">
                                                                <strong style="width: 40px">Total $</strong>
                                                                <asp:TextBox ID="txtTotalAmount" ReadOnly="true" runat="server"></asp:TextBox>
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
                                                                <span class="star" style="color: Red">*</span><strong style="width: 175px; font-size: 15px;">Voice
                                                                    file confirmation #:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:TextBox ID="txtVoicefileConfirmNo" runat="server" MaxLength="30"></asp:TextBox>
                                                            </h4>
                                                        </td>
                                                        <td width="40%">
                                                            <h4 class="h4">
                                                                <span class="star" style="color: Red">*</span><strong style="width: 150px; font-size: 15px;">
                                                                    Voice file Location:</strong>
                                                                <%-- <input type="text" style="width: 245px" />--%>
                                                                <asp:DropDownList ID="ddlVoiceFileLocation" runat="server">
                                                                </asp:DropDownList>
                                                            </h4>
                                                        </td>
                                                        <td style="padding: 0;">
                                                            <div style="width: 100px; float: right; margin: 0 auto 10px auto; clear: both; text-align: right">
                                                                <%--<input type="submit" name="btnSale" value="Process" onclick="return ValidateSavedData();"
                                                                    id="btnprocess" class="g-button g-button-submit">--%>
                                                                <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="g-button" Visible="true"
                                                                    Enabled="false" />
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
        <table width="100%">
            <tr>
                <td width="400px">
                    &nbsp;
                </td>
                <td style="text-align: right">
                    <asp:UpdatePanel ID="updatePnlSave2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnTransfer2" runat="server" CssClass="g-button g-button-submit"
                                Text="Transfer" OnClientClick="return ValidateDraftData();" OnClick="btnTransfer_Click" />
                            &nbsp;
                            <asp:Button ID="btnSale2" runat="server" CssClass="g-button g-button-submit" Text="Sale"
                                OnClientClick="return ValidateSavedData();" OnClick="btnSale_Click" />
                            &nbsp;
                            <asp:Button ID="btnSavedraft2" runat="server" CssClass="g-button g-button-submit"
                                Text="Save as draft" OnClick="btnSavedraft_Click" OnClientClick="return ValidateDraftData();" />
                            &nbsp
                            <asp:Button ID="btnAbandon2" runat="server" CssClass="g-button g-button-submit" Text="Abandon"
                                OnClick="btnAbandon_Click" OnClientClick="return ValidateAbandonData();" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
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
    <cc1:ModalPopupExtender ID="mdepalertdraftPhnoExist" runat="server" PopupControlID="divdraftPhone"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdndraftPhNo" OkControlID="btnDraftPhNoNo">
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
                        <asp:Label ID="lbldraftPhNoExists" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="clear">
                </div>
            </p>
            <asp:Button ID="btnDraftPhNoNo" class="btn" runat="server" Text="No" />
            &nbsp;
            <asp:Button ID="btnDraftPhNoYes" class="btn" runat="server" Text="Yes" OnClick="btnDraftPhNoYes_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mdepAlreadySale" runat="server" PopupControlID="divAlreadySale"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAlreadySale" OkControlID="btnDraftPhNoNo">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAlreadySale" runat="server" />
    <div id="divAlreadySale" class="alert" style="display: block;">
        <h4 id="H4">
            Alert
            <%--<asp:Button ID="btnDiv" class="cls" runat="server" Text="" BorderWidth="0" />--%>
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <table width="95%" style="margin: 15px auto;" id="tblAlreadySale">
                            <tr>
                                <td colspan="2">
                                    <b>System already have a sale from this phone #</b>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px;">
                                    Phone #
                                </td>
                                <td>
                                    <asp:Label ID="lblAlreadySalePhoneNumber" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    First name
                                </td>
                                <td>
                                    <asp:Label ID="lblAlreadySaleFirstName" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="clear">
                </div>
            </p>
            <asp:Button ID="Button1" class="btn" runat="server" Text="No" />
            &nbsp;
            <asp:Button ID="Button2" class="btn" runat="server" Text="Yes" />
        </div>
    </div>
    <asp:HiddenField ID="btnOpen" runat="server" />
    <cc1:ModalPopupExtender ID="MPEUpdate" runat="server" PopupControlID="tblUpdate"
        BackgroundCssClass="ModalPopupBG" TargetControlID="btnOpen" OkControlID="btnPhoneCancel">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 160px; margin-top: 70px; width: 250px">
            <h4>
                Enter Phone Number
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
                            <asp:UpdatePanel ID="updtpnlBtns" runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr align="center">
                                            <td colspan="4" style="padding-top: 15px;">
                                                <div style="width: 240px; margin: 0 auto;">
                                                    <asp:Button ID="btnPhoneOk" runat="server" Text="Ok" CssClass="g-button g-button-submit"
                                                        OnClientClick="return ValidatePhone();" OnClick="btnPhoneOk_Click" />
                                                    <asp:Button ID="btnPhoneCancel" runat="server" Text="Cancel" CssClass="g-button g-button-submit" />
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
    <cc1:ModalPopupExtender ID="mdepExistsAlertShow" runat="server" PopupControlID="divExistsAlertShow"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnExistsAlertShow">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnExistsAlertShow" runat="server" />
    <div id="divExistsAlertShow" class="alert" style="display: none">
        <h4 id="H5">
            Alert
            <asp:Button ID="btnAlertExistTop" class="cls" runat="server" Text="" BorderWidth="0"
                OnClick="btnAlertExistShow_Click" />
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblExistsAlertShow" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnAlertExistShow" class="btn" runat="server" Text="Ok" OnClick="btnAlertExistShow_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mdepDraftExistsShow" runat="server" PopupControlID="divDraftExistsShow"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnDraftExistsShow">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnDraftExistsShow" runat="server" />
    <div id="divDraftExistsShow" class="alert" style="display: none">
        <h4 id="H6">
            Alert
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblDraftExistsShow" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnDraftExistsShowNo" class="btn" runat="server" Text="No" OnClick="btnDraftExistsShowNo_Click" />&nbsp;
            <asp:Button ID="btnDraftExistsShowYes" class="btn" runat="server" Text="Yes" OnClick="btnDraftExistsShowYes_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="MdepAddAnotherCarAlert" runat="server" PopupControlID="divAddAnotherCarAlert"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAddAnotherCarAlert">
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
            <asp:Button ID="btnAddAnotherCarNo" class="btn" runat="server" Text="No" OnClick="btnAddAnotherCarNo_Click" />&nbsp;
            <asp:Button ID="btnAddAnotherCarYes" class="btn" runat="server" Text="Yes" OnClick="btnAddAnotherCarYes_Click" />
        </div>
    </div>
    <cc1:ModalPopupExtender ID="mdepAlertAfterSave" runat="server" PopupControlID="divAlertAfterSave"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnAlertAfterSave">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnAlertAfterSave" runat="server" />
    <div id="divAlertAfterSave" class="alert" style="display: none">
        <h4 id="H8">
            Alert
            <!-- <div class="cls">
            </div> -->
        </h4>
        <div class="data">
            <p>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblAlertAfterSave" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <asp:Button ID="btnAlertAfterSaveNo" class="btn" runat="server" Text="No" OnClick="btnAlertAfterSaveNo_Click" />&nbsp;
            <asp:Button ID="btnAlertAfterSaveYes" class="btn" runat="server" Text="Yes" OnClick="btnAlertAfterSaveYes_Click" />
        </div>
    </div>
    </form>
</body>
</html>
