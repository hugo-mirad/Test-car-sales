<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCCkeckList1.aspx.cs" Inherits="QCCkeckList1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="css/css2.css" rel="stylesheet" type="text/css" />
    <link href="css/inputs.css" rel="stylesheet" type="text/css" />
    <link href="Static/Css/calender.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.7.min.js" type="text/javascript"></script>

    <style>
        .pageHead
        {
            text-align: center;
        }
        .block
        {
            display: block;
            border: #999 1px solid;
            padding: 1px;
            margin-bottom: 10px;
            box-shadow: 0 2px 3px rgba(0,0,0,0.1);
        }
        .blockHead
        {
            background: #ccc;
            color: ##333;
            padding: 5px;
            margin: 0 0 10px 0;
            text-align: left;
        }
        .block .inner
        {
            margin: 10px;
        }
        .block2
        {
            margin: 10px;
            font-size: 13px;
        }
        .block2 .inner
        {
            margin: 0;
        }
        select, textarea, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input
        {
            height: 18px;
        }
        td
        {
            vertical-align: top;
            padding-bottom: 8px;
        }
         td td td
        {
            vertical-align: top;
            padding-bottom:2px;
        }
        td td table{ margin-top:-8px; background:#FFFEED; border:#FFF8D0 1px solid; width:100%; margin-bottom:5px;  }
        
        input[type=checkbox], input[type=radio]
        {
            display: inline-block;
            margin-right: 4px;
            vertical-align: top;
            margin-top: 0;
        }
        /* POPUP*/.ModalPopupBG
        {
            border: 2px;
            background-color: #dbdbdb;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .PopUpHolder .main
        {
            border: 4px solid rgba(0,0,0,0.5);
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
            -webkit-box-shadow: 0 0 18px rgba(0,0,0,0.9);
            -moz-box-shadow: 0 0 18px rgba(0,0,0,0.9);
            box-shadow: 0 0 18px rgba(0,0,0,0.9);
            width: 590px;
            padding: 5px;
            margin: 0 auto;
            font-size: 12px;
            color: #333;
            background: #fff;
            text-align: left;
            height: auto;
            margin-top: 10%;
        }
        .PopUpHolder .main .dat
        {
            padding: 0 25px;
            width: 93%;
            height: auto;
        }
        .PopUpHolder .main h4
        {
            padding: 6px;
            color: #222;
            font-size: 16px;
            height: 30px;
            line-height: 30px;
            margin: 0;
            text-align: center;
            border-bottom: #999 1px solid;
            margin-bottom: 10px;
        }
        
        .question{ color:#990000  }
        .question span, .question b{ font-size:14px; }
        
    </style>

    <script type="text/javascript">

function openpopup()
{
window.open("Ticker.aspx ")
}
 function ClosePopup15() {
            $find('<%= MdlpackageEdit.ClientID%>').hide();
            return false;
        }
        
        function ClosePopup1() {
            $find('<%= HdnEmailEdi.ClientID%>').hide();
            return false;
        }
         function ClosePopup8() {
            $find('<%= MdlPaymEdit.ClientID%>').hide();
            return false;
        }
         function ClosePopup3() {
            $find('<%= MDLVREdit.ClientID%>').hide();
            return false;
        }
          function ClosePopup2() {
            $find('<%= MDLSaleDateUpda.ClientID%>').hide();
            return false;
        }
         function ClosePopup5() {
            $find('<%= MDlPopCustNameC.ClientID%>').hide();
            return false;
        }
            function ClosePopup6() {
            $find('<%= MdlPopPhnAndAdd.ClientID%>').hide();
            return false;
        }
          function ClosePopup7() {
            $find('<%= MdlPopVehUp.ClientID%>').hide();
            return false;
        }
         function ClosePopup9() {
            $find('<%= MdlPostDate.ClientID%>').hide();
            return false;
        }
          function ClosePopup11() {
            $find('<%= MdlPAgentEdi.ClientID%>').hide();
            return false;
        }
         function ValidateUpdate() {
        
            var valid = true;
                        if(document.getElementById('<%= ddlAgntCents.ClientID%>').value == "All") {
                        document.getElementById('<%= ddlAgntCents.ClientID%>').focus();
                        alert("Please select Center and Agent name");                 
                        document.getElementById('<%=ddlAgntCents.ClientID%>').focus()
                        valid = false;            
                         return valid;     
                         
                         if(document.getElementById('<%= ddlVerifierCents.ClientID%>').value == "All") {
                        document.getElementById('<%= ddlVerifierCents.ClientID%>').focus();
                        alert("Please select Center and Verifier name");                 
                        document.getElementById('<%=ddlVerifierCents.ClientID%>').focus()
                        valid = false;            
                         return valid;     
                    }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div style="width: 550px;">
        <table style="width: 100%; border-collapse: collapse;">
            <tr>
                <td>
                    <h2 class="pageHead">
                        QC Check List</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="block">
                        <div class="inner">
                            <table style="width: 100%; border-collapse: collapse">
                                <tr>
                                    <td style="width: 76px">
                                        <b>Sale ID</b>
                                    </td>
                                    <td style="width: 120px">
                                        <asp:Label ID="lblSalesId" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 58px">
                                    </td>
                                   <td >
                                        <b>Customer</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcuName" runat="server"></asp:Label>
                                    </td>
                                  
                                </tr>
                                <tr>
                                
                                
                                    <td style="width: 111px">
                                        <b>Sale Date</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSalesDate" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                     <td>
                                        <b>Phone</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPhn" runat="server"></asp:Label>
                                    </td>
                                  
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                     <td>
                                        <b>Voice File No</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVFNo" runat="server"></asp:Label><asp:Label ID="lblVFLoc" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                  <%--<td>
                                        <b>Voice File Location</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVFLoc" runat="server"></asp:Label>
                                    </td>--%>
                                </tr>
                                <tr>
                                <td style="width: 111px"><b>Agent</b></td>
                                <td><asp:Label ID="lblAgen" runat="server" ></asp:Label>&nbsp;<asp:LinkButton ID="lnkAgent" runat="server" Text="A/V" OnClick="lnkAgent_Click" ></asp:LinkButton></td>
                                  <td style="width: 58px">
                                    </td>
                                 <td><b>Verifier</b></td>
                                <td><asp:Label ID="lblver" runat="server" ></asp:Label>&nbsp;<asp:LinkButton ID="lnkVerifie" Visible="false" runat="server"  Text="EDIT" OnClick="lnkVerifie_Click" ></asp:LinkButton></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="block">
                        <div class="inner">
                            <table style="width: 100%; border-collapse: collapse">
                                <tr>
                                    <td style="width: 380px"  >
                                       Recording:<b><asp:Label ID="lblVF" runat="server" Text="VF01"></asp:Label></b>  is available?
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="VoiceFileY" Text="" GroupName="VoiceFile" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="VoiceFileN" Text="" GroupName="VoiceFile" runat="server" />False
                                    </td>  
                                     <td>
                                      <asp:LinkButton ID="lnkRecEdt" runat="server" Text="EDIT" OnClick="lnkRecEdt_Click"></asp:LinkButton>
                                    </td>                                   
                                <tr>
                                    <td >
                                       Sale date:<b><asp:Label ID="lblDate" runat="server"></asp:Label></b>
                                    </td>
                                    <td >
                                        <asp:RadioButton ID="VoiceQualityY" Text="" GroupName="VoiceQuality" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="VoiceQualityN" Text="" GroupName="VoiceQuality" runat="server" />False
                                    </td>
                                     <td>
                                      <asp:LinkButton ID="lnkSaleDate" runat="server" Text="EDIT" OnClick="lnkSaleDate_Click"></asp:LinkButton>
                                    </td> 
                                </tr>
                                <tr>
                                    <td >
                                        Customer Name <b><asp:Label ID="lblcustname" runat="server"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="lblcustnameY" Text="" GroupName="lblcustname" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="lblcustnameN" Text="" GroupName="lblcustname" runat="server" />False
                                    </td>
                                    <td>
                                    <asp:LinkButton ID="lnkCustNameC" runat="server" Text="EDIT" OnClick="lnkCustNameC_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <!-- Phone -->
                                <tr>
                                    <td >
                                        Phone Number and Address
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="PhnAddY" Text="" GroupName="PhnAdd" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="PhnAddN" Text="" GroupName="PhnAdd" runat="server" />False
                                    </td>
                                      <td>
                                    <asp:LinkButton ID="lnkPhnAdd" runat="server" Text="EDIT" OnClick="lnkPhnAdd_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                 <td colspan="3">
                                        <table>
                                        <tr>
                                    <td colspan="3" style="padding-left: 20px; width:300px;">
                                        <b>Ph:<asp:Label ID="lblPn" runat="server"></asp:Label></b>
                                    </td>
                                    </tr>
                                   <tr>
                                    <td colspan="3" style="padding-left: 20px; width:300px;" >
                                      <b><asp:Label ID="lblAdd" runat="server" Text="Add"></asp:Label></b> 
                                    </td>
                                    </tr>
                                     <tr>
                                    <td colspan="3" style="padding-left: 20px; width:300px;" >
                                       <b><asp:Label ID="lblCity1" runat="server" ></asp:Label></b> 
                                    </td>
                                    </tr>
                                    
                                </table>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                Vehicle Info:<b><asp:Label ID="lblVehivleinfo" runat="server"></asp:Label></b>
                                </td>
                                 <td>
                                        <asp:RadioButton ID="VehiclY" Text="" GroupName="Vehicl" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="VehiclN" Text="" GroupName="Vehicl" runat="server" />False
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkVeh" runat="server" Text="EDIT" OnClick="lnkVeh_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <!-- End -->
                                <tr>
                                    <td style="width: 200px" >
                                        Package Sold<b>
                                            <asp:Label ID="lblPack" runat="server" Text="$99"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="PackageAvailableY" Text="" GroupName="PackageAvailable" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="PackageAvailableN" Text="" GroupName="PackageAvailable" runat="server" />False
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lblPackEdit" runat="server" Text="EDIT" OnClick="lblPackEdit_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="pack199">
                                    <td style="width: 200px" >
                                       Refund Policy: <b>Explained Clearly</b>
                                    </td>
                                    <td colspan="2" >
                                        <asp:RadioButton ID="Refund1Y" Text="" GroupName="Refund1" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="Refund1N" Text="" GroupName="Refund1" runat="server" />False
                                    </td>
                                </tr>
                                <tr class="pack99">
                                    <td style="width: 200px" >
                                        No Refund Policy <b>Explained Clearly</b>
                                    </td>
                                    <td colspan="2" >
                                        <asp:RadioButton ID="RefundNY" Text="" GroupName="RefundN" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="RefundNN" Text="" GroupName="RefundN" runat="server" />False
                                    </td>
                                </tr>
                                 <tr>
                                  <td >
                                        Payment Info:
                                    </td>
                                  <td >
                                        <asp:RadioButton ID="RefundY" Text="" GroupName="Refund" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="RefundN" Text="" GroupName="Refund" runat="server" />False
                                         
                                    </td>
                                    <td>
                                    <asp:LinkButton ID="lnkPaymEdit" runat="server" Text="EDIT" OnClick="lnkPaymEdit_Click" ></asp:LinkButton>
                                    </td>
                                   
                                </tr>
                                <tr class="cc1">
                                <td colspan="3" >
                                <table style="width: 100%; border-collapse: collapse">
                               
                                 <tr>
                                <td style="padding-left: 20px; width:300px;"><b><asp:Label ID="lblpaymn" runat="server" ></asp:Label> (<asp:Label ID="lblpamth" runat="server" ></asp:Label>)</b></td>
                                </tr>
                                <tr>
                                <td style="padding-left: 20px; width:300px;"><b>#<asp:Label ID="lblcrdCNo" runat="server" ></asp:Label></b></td>
                                </tr>
                                 <tr>
                                <td style="padding-left: 20px; width:300px;">CVV:<b><asp:Label ID="lblcvv" runat="server" ></asp:Label></b></td>
                                </tr>
                                 <tr>
                                <td style="padding-left: 20px; width:300px;">Exp:<b><asp:Label ID="lblExpDate" runat="server" ></asp:Label></b></td>
                                </tr>
                                 <tr>
                                <td style="padding-left: 20px; width:300px;">Name:<b><asp:Label ID="lblCHN" runat="server" ></asp:Label></b></td>
                                </tr>
                                  <tr>
                                <td style="padding-left: 20px; width:300px;"><b><asp:Label ID="lblbillAdd" runat="server" ></asp:Label></b></td>
                                </tr>
                                   <tr>
                                <td style="padding-left: 20px; width:300px;"><b><asp:Label ID="lnlbillcity" runat="server" ></asp:Label></b></td>
                                </tr>
                                </table>
                                </td>
                                
                                   
                                  
                                    
                                    <!-- end-->
                                </tr>
                               
                                <tr class="cck">
                                    <td colspan="3">
                                        <table>
                                            <!-- check -->
                                            <tr>
                                                <td style="width: 250px">
                                                    ACH Name:<b><asp:Label ID="lblACHNam" runat="server"></asp:Label></b>
                                                </td>
                                                <td >
                                                    Bank Name:<b><asp:Label ID="lblBankNam" runat="server"></asp:Label></b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Account #: <b><asp:Label ID="lblAName" runat="server" ></asp:Label></b>
                                                </td>
                                                <td>
                                                    Routing #:<b><asp:Label ID="lblRoutName" runat="server" ></asp:Label></b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Account Type:<b><asp:Label ID="lblAccTyp" runat="server"></asp:Label></b>
                                                </td>
                                            </tr>
                                            <!-- end -->
                                        </table>
                                    </td>
                                </tr>
                                
                                 <tr class="ppl">
                                    <td colspan="3">
                                        <table>
                                            <!-- check -->
                                            <tr>
                                                <td style="width: 250px">
                                                    Transaction #:<b><asp:Label ID="lblTrnas" runat="server"></asp:Label></b>
                                                </td>
                                                <td >
                                                    PayPal Email:<b><asp:Label ID="lblpayemail" runat="server"></asp:Label></b>
                                                </td>
                                            </tr>
                                           
                                        </table>
                                    </td>
                                </tr>
                                <!-- Payment Schedule -->
                                <tr>
                                    <td>
                                        Payment:Post Dated <b><asp:Label ID="lblpyschd" runat="server" ForeColor="White"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="PayScSFullPY" Text="" GroupName="PayScSFullP" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="PayScSFullPN" Text="" GroupName="PayScSFullP" runat="server" />False
                                    </td>
                                     <td>
                                    <asp:LinkButton ID="linkPyDate" runat="server" Text="EDIT" OnClick="linkPyDate_Click" ></asp:LinkButton>
                                    </td>
                                </tr>
                                
                                <tr class="pay1">
                                    <td colspan="3">
                                        <table style="width: 100%; border-collapse: collapse">
                                            <tr>
                                                <td  colspan="2" style="padding-left: 20px; width:300px;">
                                                    <b><asp:Label ID="txtPaymentDate" runat="server"></asp:Label>:$ <asp:Label ID="txttodaypayment" runat="server" ></asp:Label></b>
                                                    </td>
                                                
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="padding-left: 20px; width:300px;"><b><asp:Label ID="txtPDDate" runat="server" Text="$199"></asp:Label> <asp:Label ID="txtPDAmountLater" runat="server" ></asp:Label></b>
                                                </td>
                                            </tr>  
                                        </table>
                                    </td>
                                </tr>
                              
                                <!-- end -->
                                <tr>
                                    <td >
                                        Email: <b><asp:Label ID="lblEmail" runat="server" Text="padma@gmail.com"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="EmailY" Text="" GroupName="Email" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="EmailN" Text="" GroupName="Email" runat="server" />False
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkEmil" runat="server" Text="EDIT" OnClick="lnkEmil_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="pack199" >
                                    <td style="width: 200px" >
                                       Refund Policy: <b>Explained Clearly</b>
                                    </td>
                                    <td  colspan="2" >
                                        <asp:RadioButton ID="Refund2Y" Text="" GroupName="Refund2" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="Refund2N" Text="" GroupName="Refund2" runat="server" />False
                                    </td>
                                </tr>
                                <tr class="pack99">
                                    <td style="width: 200px" >
                                        No Refund Policy <b>Explained Clearly</b>
                                    </td>
                                    <td colspan="2" >
                                        <asp:RadioButton ID="NRefund2Y" Text="" GroupName="NRefund2" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="NRefund2N" Text="" GroupName="NRefund2" runat="server" />False
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                    Customer service access:  <b># 1-888-786-8307</b> and times mentioned
                                    </td>
                                    <td colspan="2" >
                                        <asp:RadioButton ID="custmservY" Text="" GroupName="custmserv" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="custmservN" Text="" GroupName="custmserv" runat="server" />False
                                    </td>
                                </tr>
                                <tr>
                                <td >
                                Confirmation Number issued
                                </td>
                                 <td colspan="2" >
                                        <asp:RadioButton ID="CNumIssY" Text="" GroupName="CNumIss" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="CNumIssN" Text="" GroupName="CNumIss" runat="server" />False
                                    </td>
                                </tr>
                                <tr>
                                <td> Is the customer actively participating ?</td>
                                 <td colspan="2" >
                                        <asp:RadioButton ID="custAY" Text="" GroupName="custA" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="CustAN" Text="" GroupName="custA" runat="server" />False
                                    </td>
                                </tr>
                                <tr class="pay">
                                    <td  >
                                        Did they partial payments and deposits made are not refundable ?
                                    </td>
                                   <td colspan="2" >
                                        <asp:RadioButton ID="PatyY" Text="" GroupName="PatyY" runat="server" />True&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="PatyN" Text="" GroupName="PatyY" runat="server"  />False
                                    <td></td>
                                </tr>
                                <tr>
                                <td colspan="3" style="padding-top:10px;">
                                
                                <b>QC Check List FeedBack</b> <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Style="resize: none;
                                            height: 70px; width: 99%"></asp:TextBox>
                                </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px;">
                    <div style="text-align: center">
                        <asp:Button ID="btnQuali" runat="server" Text="Qualified" OnClick="btnQuali_Click"
                            CssClass="btn btn-success" Enabled="false"  />
                        <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click"
                            CssClass="btn btn-danger" Enabled="false" />
                        <asp:Button ID="btnHold" runat="server" Text="Hold" OnClick="btnHold_Click" Enabled="true" CssClass="btn btn-warning"/>
                        <asp:Button ID="btnReturn" runat="server" Text="Return" OnClick="btnReturn_Click" CssClass="btn btn-info" Enabled="false" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <!-- PicDesc -->
    <asp:HiddenField ID="lblpicDesc" runat="server" />
    <cc1:ModalPopupExtender ID="MdlpackageEdit" runat="server" PopupControlID="tblUpdate8"
        BackgroundCssClass="ModalPopupBG" TargetControlID="lblpicDesc">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate8" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               Packages 
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                    <tr>
                    <td style="width: 120px; height: 10px;">SaleId</td>
                    <td><b><asp:Label ID="lblsaleId" runat="server"></asp:Label></b></td>
                    </tr>
                    <tr>
                        <td >
                            <b>Selected Package</b>
                        </td>
                        <td><b><asp:Label ID="SelectedPack" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                        <td>Change Package</td>
                        <td style="width: 80pxheight:10px;">
                             <asp:RadioButton ID="RadioButton3" Text="" GroupName="NRefund23" runat="server" />$99.99&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:RadioButton ID="RadioButton4" Text="" GroupName="NRefund23" runat="server" />$199.99&nbsp;&nbsp;
                              <asp:RadioButton ID="RadioButton5" Text="" GroupName="NRefund23" runat="server" />$299.99
                        </td>
                    </tr>
                    <tr><td> <br /></td></tr>
                     <tr><td> <br /></td></tr>
                    
                     <tr>
                     <td >   <asp:Button ID="BtnPacgeUpdate" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="BtnPacgeUpdate_Click"/></td>
                    <td>   <asp:Button ID="Button7" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup15();"/></td>
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!-- Edit Email -->
     <asp:HiddenField ID="HdnEmailEdi" runat="server" />
    <cc1:ModalPopupExtender ID="MdlEmailEdit" runat="server" PopupControlID="tblUpdate9"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnEmailEdi">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate9" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               Email
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:30px">Sale Id</td>
                <td><b><asp:Label ID="Label1" runat="server"></asp:Label></b></td>
                </tr>
                    <tr>
                    <td>Email:</td>
                    <td><asp:Label ID="lbloldemail" runat="server"></asp:Label></td>
                    </tr>
                    
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Email</b>
                        </td>
                        <td style="width: 80pxheight:10px;">
                             <b><asp:TextBox ID="txtEmail" runat="server" ></asp:TextBox></b>
                        </td>
                    </tr>
                     <tr>
                     <td style="width:80px">   <asp:Button ID="Button1" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="Button1_Click"/></td>
                    <td>   <asp:Button ID="Button2" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup1();"/></td>
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
      <!-- Edit Email -->
     <asp:HiddenField ID="HiddenField1" runat="server" />
    <cc1:ModalPopupExtender ID="MdpAddress" runat="server" PopupControlID="tblUpdate10"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnEmailEdi">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate10" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               Address
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:30px">Sale Id</td>
                <td><b><asp:Label ID="Label2" runat="server"></asp:Label></b></td>
                </tr>
                    <tr>
                    <td>Phone:</td>
                    <td><asp:Label ID="Label3" runat="server"></asp:Label></td>
                    </tr>
                    
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Address</b>
                        </td>
                        <td style="width: 80pxheight:10px;">
                             <b><asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox></b>
                        </td>
                    </tr>
                     <tr>
                     <td style="width:80px">   <asp:Button ID="Button3" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="Button1_Click"/></td>
                    <td>   <asp:Button ID="Button4" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup1();"/></td>
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
       <!-- Edit Email -->
     <asp:HiddenField ID="HiddenField2" runat="server" />
    <cc1:ModalPopupExtender ID="MdlPaymEdit" runat="server" PopupControlID="tblUpdate11"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnEmailEdi">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate11" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 520px; margin-top: 70px; width: 650px">
            <h4>
               Payment Info
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                                <tr>
                                <td colspan="2"><b>Sale Id</b> <asp:Label ID="lblcarP" runat="server" ></asp:Label></td>
                                </tr>
                                <tr>
                                <td style="width:200px">Card No&nbsp;<asp:Label ID="lblCarNoP" runat="server" ></asp:Label></td>
                                <td style="width:200px"><b>Change Card No</b>&nbsp;&nbsp;<asp:TextBox ID="lblCarNoPC" runat="server" ></asp:TextBox></td>
                                </tr>
                                 <tr>
                                <td ><b>CVV&nbsp;</b><asp:Label ID="lblCVVP" runat="server" ></asp:Label></td>
                                 <td ><b>Change CVV</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="lblCVVPC" runat="server" ></asp:TextBox></td>
                                </tr>
                                 <tr>
                                <td><b>Exp&nbsp;</b><asp:Label ID="lblExpP" runat="server" ></asp:Label></td>
                                 <td><b>Change Exp</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="lblExpPC" runat="server" ></asp:TextBox></td>
                                </tr>
                                 <tr>
                                <td><b>Name&nbsp;</b><asp:Label ID="lblNameP" runat="server" ></asp:Label></td>
                                 <td><b>Change Name</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="lblNamePC" runat="server" Width="100px" ></asp:TextBox>&nbsp;<asp:TextBox ID="tctLPName" runat="server" Width="100px"></asp:TextBox></td>
                                </tr>
                                  <tr>
                                <td><b>Address&nbsp;</b><asp:Label ID="lblAddP1" runat="server" ></asp:Label></td>
                                <td><b>Change Address</b>&nbsp;<asp:TextBox ID="lblAddP1C" runat="server" ></asp:TextBox></td>
                                </tr>
                                <tr>
                                <td><b>City&nbsp;</b><asp:Label ID="LblCityPP" runat="server" ></asp:Label></td>
                                 <td><b>Change City</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="LblCityPPC" runat="server" ></asp:TextBox></td>
                                </tr>
                                 <tr>
                                <td></td>
                                 <td><b>State&nbsp;</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:DropDownList ID="txtStatePP" runat="server" Width="100px" Height="25px"></asp:DropDownList>&nbsp;&nbsp;<asp:TextBox ID="txtzipii" runat="server" Width="100px"></asp:TextBox></td>
                                </tr>
                                <br />
                                 <tr>
                     <td style="width:80px">   <asp:Button ID="BtnPaymInfoUpdat" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="BtnPaymInfoUpdat_Click"/></td>
                    <td>   <asp:Button ID="Button17" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup8();"/></td>
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
       <!-- Voice Record Editing-->
     <asp:HiddenField ID="HdnVREdit" runat="server" />
    <cc1:ModalPopupExtender ID="MDLVREdit" runat="server" PopupControlID="tblUpdate14"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnVREdit">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate14" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               Voice Record
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:100px"><b>Sale Id</b></td>
                <td style="width:50px"><b><asp:Label ID="lblSaleIdR" runat="server"></asp:Label></b></td>
                </tr>
                    <tr>
                    <td><b>Voice File </b></td>
                    <td><asp:Label ID="lblVFNo1" runat="server"></asp:Label> (<asp:Label ID="lblVFLoc1" runat="server"></asp:Label>)</td>
                    </tr>
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Voice File No</b>
                        </td>
                        <td><asp:TextBox ID="lblVFNoC1" runat="server"></asp:TextBox></td>
                         
                    </tr>
                  <%--  <tr visible="false">
                   <td><b>Change Voice File Location:</b></td>
                    <td> <asp:TextBox ID="lblVFNoL1" runat="server"></asp:TextBox> </td>
                           
                      
                    </tr>--%>
                    <br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="Button5" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="Button5_Click"/>&nbsp;&nbsp; <asp:Button ID="Button6" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup3();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    <!--  End-->
    
    
    <!-- Sale Date Editing-->
    <asp:HiddenField ID="HdnSaleDate" runat="server" />
    <cc1:ModalPopupExtender ID="MDLSaleDateUpda" runat="server" PopupControlID="tblUpdate15"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnSaleDate">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate15" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
               Sale Date
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:100px"><b>Sale Id</b></td>
                <td style="width:50px"><b><asp:Label ID="lblSaleUp" runat="server"></asp:Label></b></td>
                </tr>
                    <tr>
                    <td><b>Sale Date </b></td>
                    <td><asp:Label ID="lblsaleDateU" runat="server"></asp:Label></td>
                    </tr>
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change SaleDate</b>
                        </td>
                        <td><asp:TextBox ID="txtSaleDateC" runat="server"></asp:TextBox></td>
                         
                    </tr><br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="BtnDaleDate" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="BtnDaleDate_Click"/>&nbsp;&nbsp; <asp:Button ID="Button9" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup2();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
     <!-- CyustNamee Editing-->
    <asp:HiddenField ID="HdCustName" runat="server" />
    <cc1:ModalPopupExtender ID="MDlPopCustNameC" runat="server" PopupControlID="tblUpdate16"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdCustName">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate16" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
              Customer
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:100px"><b>Sale Id</b></td>
                <td style="width:50px"><b><asp:Label ID="lblsalCus" runat="server"></asp:Label></b></td>
                </tr>
                    <tr>
                    <td><b>Customer Name</b></td>
                    <td><asp:Label ID="lblCustNC" runat="server"></asp:Label></td>
                    </tr>
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change F.Name</b>
                        </td>
                        <td><asp:TextBox ID="txtchanCusU" runat="server"></asp:TextBox></td>
                         
                    </tr>
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change L.Name</b>
                        </td>
                        <td><asp:TextBox ID="txtlatNamU" runat="server"></asp:TextBox></td>
                         
                    </tr><br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="BtncustnUpda" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="BtncustnUpda_Click"/>&nbsp;&nbsp; <asp:Button ID="Button11" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup5();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
    
     <!-- Phone and addess  Editing-->
    <asp:HiddenField ID="HdnPhoneAndAdd" runat="server" />
    <cc1:ModalPopupExtender ID="MdlPopPhnAndAdd" runat="server" PopupControlID="tblUpdate17"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnPhoneAndAdd">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate17" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 420px; margin-top: 70px; width: 550px">
            <h4>
              Phone and address
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 680px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:100px"><b>Sale Id</b></td>
                <td style="width:50px"><b><asp:Label ID="lblsalddr" runat="server"></asp:Label></b></td>
                </tr>
                    <tr>
                    <td><b>Phone</b></td>
                    <td><asp:Label ID="lblPhnAdd" runat="server"></asp:Label></td>
                    </tr>
                     <tr>
                    <td><b>Address</b></td>
                    <td><asp:Label ID="lbladdAddress" runat="server"></asp:Label></td>
                    </tr>
                     <tr>
                     <td></td>
                    <td><asp:Label ID="lbladdCity" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Phone</b>
                        </td>
                        <td><asp:TextBox ID="txtcPhnC" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Address</b>
                        </td>
                        <td><asp:TextBox ID="txtCAddress" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change City</b>
                        </td>
                        <td><asp:TextBox ID="txtCityC" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change State</b>
                        </td>
                        <td><asp:TextBox ID="txtSTateC" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Zip</b>
                        </td>
                        <td><asp:TextBox ID="TxtCZIp" runat="server"></asp:TextBox></td>
                    </tr>
                    <br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="btnCustAdd" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="btnCustAdd_Click"/>&nbsp;&nbsp; <asp:Button ID="Button13" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup6();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
    
    <!-- Vehicle   Editing-->
    <asp:HiddenField ID="HdnVehClP" runat="server" />
    <cc1:ModalPopupExtender ID="MdlPopVehUp" runat="server" PopupControlID="tblUpdate18"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnVehClP">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate18" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 350px">
            <h4>
           Vehicle Information
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:100px"><b>Sale Id</b></td>
                <td style="width:50px"><b><asp:Label ID="lblvehsale" runat="server"></asp:Label></b></td>
                </tr>
                    <tr>
                    <td><b>Make</b></td>
                    <td><asp:Label ID="lblMake" runat="server"></asp:Label></td>
                    </tr>
                     <tr>
                    <td><b>Model</b></td>
                    <td><asp:Label ID="lblMaodel" runat="server"></asp:Label></td>
                    </tr>
                     <tr>
                     <td><b>Year</b></td>
                    <td><asp:Label ID="lblYear" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Make</b>
                        </td>
                        <td><asp:DropDownList ID="txtcmake" runat="server" AutoPostBack="true" Height="22px" Width="150px" OnSelectedIndexChanged="txtcmake_SelectedIndex"></asp:DropDownList></td>
                    </tr>
                  <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Model</b>
                        </td>
                        <td><asp:DropDownList ID="txtmodel" runat="server" Height="22px" Width="150px"></asp:DropDownList></td>
                    </tr>
                     <tr>
                        <td style="width: 40px; height: 10px;">
                            <b>Change Year</b>
                        </td>
                        <td><asp:TextBox ID="txtyear" runat="server"></asp:TextBox></td>
                    </tr>
                    
                    <br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="btnUpdateVehicl1" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="btnUpdateVehicl1_Click"/>&nbsp;&nbsp; <asp:Button ID="Button15" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup7();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
     <!-- Phone and addess  Editing-->
    <asp:HiddenField ID="HdnFielPAmnt" runat="server" />
    <cc1:ModalPopupExtender ID="MdlPostDate" runat="server" PopupControlID="tblUpdate19"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnFielPAmnt">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate19" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 420px; margin-top: 70px; width: 550px">
            <h4>
          Payment Amount and Date
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 380px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:250px"><b>Sale Id</b></td>
                <td style="width:150px"><b><asp:Label ID="lblpostUp" runat="server"></asp:Label></b></td>
                </tr>
                <tr>
                <td colspan="2">
                     <b>Today's Amount</b>&nbsp;&nbsp;<asp:Label ID="lblDateUp" runat="server"></asp:Label>,$ <asp:Label ID="lblAmountUps" runat="server" ></asp:Label>
                     </td></tr>
                  <tr>
                     <td colspan="2" visible>
                     <b>Next Amount&nbsp;&nbsp;<asp:Label ID="lblPostDateUp" runat="server"></asp:Label> <asp:Label ID="lblPostAmountUpse" runat="server" ></asp:Label></b>
                     </td></tr>
                     <tr>
                     <td><b>Change Date</b></td>
                     <td><asp:TextBox ID="txtChDateUp" runat="server"></asp:TextBox></td>
                     </tr>
                      <tr>
                     <td><b>Change Amount</b></td>
                     <td><asp:TextBox ID="txtChAmntUp" runat="server"></asp:TextBox></td>
                     </tr>
                   <%--   <tr>
                     <td><b>Post Date</b></td>
                     <td><asp:TextBox ID="txtPostDateCh" runat="server"></asp:TextBox></td>
                     </tr>
                      <tr>
                     <td><b>Post Amount</b></td>
                     <td><asp:TextBox ID="txtPostAmntCh" runat="server"></asp:TextBox></td>
                     </tr>
                    --%>
                    <br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="BtnPostAmount" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="BtnPostAmount_Click"/>&nbsp;&nbsp; <asp:Button ID="Button19" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup9();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
    
      <!-- Phone and addess  Editing-->
    <asp:HiddenField ID="HdnAgenNameChang" runat="server" />
    <cc1:ModalPopupExtender ID="MdlPAgentEdi" runat="server" PopupControlID="tblUpdate20"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnAgenNameChang">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate20" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 320px; margin-top: 70px; width: 450px">
            <h4>
          Agent and Verifier 
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 380px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:250px"><b>Sale Id</b></td>
                <td style="width:150px"><b><asp:Label ID="lblAgnSale" runat="server"></asp:Label></b></td>
                </tr>
                <tr>
                <td><b>Agent Name</b></td>
               <td><asp:Label ID="lblAgnt" runat="server" ></asp:Label></td> 
                 </tr>
                  <tr>
                <td><b>Verifier Name</b></td>
               <td><asp:Label ID="lblVerfs" runat="server" ></asp:Label></td> 
                 </tr>
                    <tr>
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                <td><b>Change Agent Name</b></td>
               <td><asp:Label ID="lblChAgnName" runat="server" ></asp:Label>
               <asp:DropDownList ID="ddlAgntCents" runat="server"  Height="22px" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlAgntCents_SelectedIndexChanged"></asp:DropDownList></td>
               <td><asp:DropDownList ID="ddlagents" runat="server" Height="22px" Width="150px"></asp:DropDownList></td> 
                 </tr>
                  <tr>
                <td><b>Change Verifier Name</b></td>
                
               <td><asp:Label ID="Label4" runat="server" >
               </asp:Label><asp:DropDownList ID="ddlVerifierCents" runat="server"  Height="22px" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlVerifierCents_SelectedIndexChanged"></asp:DropDownList></td>
               <td><asp:DropDownList ID="ddlverifiers" runat="server"  Height="22px" Width="150px"></asp:DropDownList></td> 
                 </ContentTemplate>
                </asp:UpdatePanel>
                 </tr>
                    <br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="Button20"  runat="server" CssClass="g-button g-button-submit"
                           Text="Update" OnClick="Button20_Click" OnClientClick="return ValidateUpdate();" />&nbsp;&nbsp; <asp:Button ID="Button21" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup11();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
    <!-- Check  -->
       <!-- Edit Email -->
     <asp:HiddenField ID="HdnCheck" runat="server" />
    <cc1:ModalPopupExtender ID="MdlPopCheck" runat="server" PopupControlID="tblUpdate30"
        BackgroundCssClass="ModalPopupBG" TargetControlID="HdnCheck">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate30" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 420px; margin-top: 70px; width: 450px">
            <h4>
               Check Payment
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 280px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:150px">Sale Id</td>
                <td><b><asp:Label ID="chkcarid" runat="server"></asp:Label></b></td>
                <tr>
               
                <td><b>ACH Name:</b><asp:Label ID="chklabel" runat="server"></asp:Label></td>
                <td><b>Bank Name:</b><asp:Label ID="chkbanknam" runat="server"></asp:Label></td>
               
                </tr>
              <td><b>Account #:</b><asp:Label ID="chkaccno" runat="server"></asp:Label></td>
               <td><b>Routing #:<b></b><asp:Label ID="chkbankroutin" runat="server"></asp:Label></td>
               
                  </tr>
                  <tr>
                  <td>Account Type
                <asp:Label ID="chkacctyp" runat="server" Visible="false"></asp:Label>
                </td>
                <td></td>
                </tr>
               <br />
               <tr>
               
                <td colspan="2" style="width:150px">ACH Name&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtchaccnam" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                <td colspan="2">Bank Name&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtbanknam" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                <td colspan="2">Account #&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtaccno" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                <td colspan="2">Routing #&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtrou" runat="server"></asp:TextBox></td>
                  </tr>
               <tr>
                <td style="width:80px" colspan="3" align="center">   <asp:Button ID="chkupdate"  runat="server" CssClass="g-button g-button-submit"
                           Text="Update" OnClick="chkupdate_Click"  />&nbsp;&nbsp; <asp:Button ID="Button10" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup11();"/></td>
                  
                    </tr>
               </tr>
                </table>
   
            </div>
        </div>
    </div>
    
    <!--  End-->
    
     <!-- Phone and addess  Editing-->
    <asp:HiddenField ID="hdnpaypal" runat="server" />
    <cc1:ModalPopupExtender ID="MdlPPaypl" runat="server" PopupControlID="tblUpdate45"
        BackgroundCssClass="ModalPopupBG" TargetControlID="hdnpaypal">
    </cc1:ModalPopupExtender>
    <div id="tblUpdate45" class="PopUpHolder" style="display: none;">
        <div class="main" style="height: 420px; margin-top: 70px; width: 550px">
            <h4>
         PayPal Account
            </h4>
            <div class="dat" style="padding: 0 0 0 6px; height: 380px; width: 99%;">
                <div class="clearFix">
                    &nbsp</div>
                <table style="width: 100%; border-collapse: collapse;">
                <tr>
                <td style="width:250px"><b>Sale Id</b></td>
                <td style="width:150px"><b><asp:Label ID="lblpyaplsal" runat="server"></asp:Label></b></td>
                </tr>
                <tr>
                <td colspan="3">
                     <b>Transaction #</b>&nbsp;&nbsp;<asp:Label ID="lbltranpayno" runat="server"></asp:Label>
                     </td></tr>
                  <tr>
                     <td colspan="3" visible>
                     <b>Paypal Email&nbsp;&nbsp;<asp:Label ID="Label8" runat="server"></asp:Label></b>
                     </td></tr>
                     <tr>
                     <td><b>Change Transaction #</b></td>
                     <td><asp:TextBox ID="txttransano" runat="server"></asp:TextBox></td>
                     </tr>
                      <tr>
                     <td><b>Paypal Email</b></td>
                     <td><asp:TextBox ID="txtpayemail" runat="server"></asp:TextBox></td>
                     </tr>
                  
                    <br />
                     <tr>
                     <td style="width:80px" colspan="3" align="center">   <asp:Button ID="btnppayemail" CssClass="g-button g-button-submit" runat="server"
                           Text="Update" OnClick="btnppayemail_Click"/>&nbsp;&nbsp; <asp:Button ID="Button12" CssClass="g-button g-button-submit" runat="server"
                           Text="Close" OnClientClick="return ClosePopup9();"/></td>
                  
                    </tr>
                   
                </table>
            </div>
        </div>
    </div>
    
    <!--  End-->
    
    
    <!-- end -->
    

    <script type="text/javascript">
       
        var PhnAdd = false;
        var Refund = false;
        var Email = false;
        
         var ar = ['VoiceFile','VoiceQuality','lblcustname','PhnAdd',   
                    'Vehicl',
                    'PackageAvailable',
                    'Refund1',
                    'Refund',            
                    'PayScSFullP',
                    'Email',
                    'Refund2',
                    'NRefund',
                    'custmserv',
                    'CNumIss',
                    'custA']
            
        var rCount = 0;
        
         function txtData(){
            console.log('Call')
            $('#txtNotes').val('');
            if(PhnAdd == true){
                $('#txtNotes').val('Address is not correct');
            }
            
             if(Refund == true){
                $('#txtNotes').val($('#txtNotes').val()+'Payment Not correct ');
            }
            
             if(Email == true){
                $('#txtNotes').val($('#txtNotes').val()+'Email is not correct');
            }
        }
        
        
        $(window).load(function(){
             $('#txtNotes').text('');
             
             
            if($.trim($('#lblPack').text()) == "$99"){
                $('.pack199').hide();
                $('.pack99').show();
            }else{
                $('.pack199').show();
                $('.pack99').hide();   
            }
            
            $('#lblPackEdit, #lnkPaymEdit, .cck, .cc1, #lnkEmil,#lnkRecEdt,#lnkSaleDate,#lnkPhnAdd,#lnkVeh,#linkPyDate,#lnkCustNameC').hide();
            
            $('input[name=PackageAvailable]').change(function(){                
                if($(this).val() == 'PackageAvailableN'){
                    $('#lblPackEdit').show();
                }else{
                    $('#lblPackEdit').hide();
                }
            })
            
            
           
           // Text box Data Binding  --------------------------
             $('input[name=PhnAdd]').change(function(){                
                if($(this).val() == 'PhnAdd1N'){
                   PhnAdd = true;               
                }else{
                   PhnAdd = false;                
                }
                txtData();
                
            })
            
            $('input[name=Refund]').change(function(){                
                if($(this).val() == 'RefundN'){
                    $('#lnkPaymEdit').show();
                    Refund = true;
                }else{
                    $('#lnkPaymEdit').hide();
                    Refund = false;
                }
                txtData();
            })
            
            $('input[name=Email]').change(function(){                
                if($(this).val() == 'EmailN'){
                    $('#lnkEmil').show();
                    Email = true;
                }else{
                    $('#lnkEmil').hide();
                    Email = false;
                }
                txtData();
            })
             $('input[name=VoiceFile]').change(function(){                
                if($(this).val() == 'VoiceFileN'){
                    $('#lnkRecEdt').show();
                    Email = true;
                }else{
                    $('#lnkRecEdt').hide();
                    Email = false;
                }
               
            })
            
             $('input[name=VoiceQuality]').change(function(){                
                if($(this).val() == 'VoiceQualityN'){
                    $('#lnkSaleDate').show();
                    Email = true;
                }else{
                    $('#lnkSaleDate').hide();
                    Email = false;
                }
               
            })
              $('input[name=lblcustname]').change(function(){                
                if($(this).val() == 'lblcustnameN'){
                    $('#lnkCustNameC').show();
                    Email = true;
                }else{
                    $('#lnkCustNameC').hide();
                    Email = false;
                }
               
            })
             $('input[name=PhnAdd]').change(function(){                
                if($(this).val() == 'PhnAddN'){
                    $('#lnkPhnAdd').show();
                    Email = true;
                }else{
                    $('#lnkPhnAdd').hide();
                    Email = false;
                }
               
            })
             $('input[name=Vehicl]').change(function(){                
                if($(this).val() == 'VehiclN'){
                    $('#lnkVeh').show();
                    Email = true;
                }else{
                    $('#lnkVeh').hide();
                    Email = false;
                }
               
            })
             $('input[name=PayScSFullP]').change(function(){                
                if($(this).val() == 'PayScSFullPN'){
                    $('#linkPyDate').show();
                    Email = true;
                }else{
                    $('#linkPyDate').hide();
                    Email = false;
                }
               
            })
            
            
          
            
            $('.cc1 table, .cck table, .pay table').css({'margin-left':'50px', 'width':'400px'})
            
            
           // $('.cc1 td:eq(0), .cck  td:eq(0)').css('padding-left','50px')
            if($.trim($('#lblpaymn').text()) == "CC"){
                $('.cc1').show();
                $('.cck').hide();
                // $('.ppl').hide();
                
            }
            
//           else if($.trim($('#lblpaymn').text()) == "Paypal"){
//                $('.cc1').hide();
//                $('.cck').hide();
//                 $('.ppl').show();
//                 
//                 }
//                
            else{
                $('.cc1').hide();
                $('.cck').show();
                // $('.ppl').hide();
            }
            
              if($.trim($('#lblpamth').text()) == "Paypal"){
                $('.cc1').hide();
                $('.cck').hide();
                 $('.ppl').show();
                
            }
            else{
              
                 $('.ppl').hide();
            }
            
            
            if($.trim($('#lblpyschd').text()) != 'Full payment'){
                $('.pay').show();            
            }else{
                 $('.pay').hide();  
                                 
            }
            if($.trim($('#lblpyschd').text()) != 'None'){
                $('.pay').show();   
                          
            }else{
                 $('.pay').hide();  
                                  
            }
           
            
            
             
            
           /*
            $('.inner table:eq(1) tr').each(function(){
                $(this).children('td:eq(0)').css({ 'font-size':'14px' });
            })
            */
            
            
            
            // btnQuali Active Status
            /*
            VoiceFile
            VoiceQuality
            lblcustname
            PhnAdd
            
            
            Vehicl
            PackageAvailable
            Refund1
            Refund
            
            PayScSFullP
            Email
            Refund2
            Refund2
            custmserv
            CNumIss
            custA
            */
            
           
            calCount();
            
             for(i = 0; i<ar.length; i++){
                $('input[name='+$.trim(ar[i])+']').live('change', function(){
                    calCount();
                })
            }   
            
             
           
      });
        
      
        
       
           
        
        function calCount(){
            rCount = 0;
            for(i = 0; i<ar.length; i++){                             
                if($('input#'+$.trim(ar[i])+'Y').is(':checked') ){     
                    //console.log(ar[i]+'Y') ;               
                    rCount++;
                }                
            }
            console.log(rCount)
            if(rCount == 14){
                 $('#btnQuali').removeAttr('disabled');
                  $('#btnReject').removeAttr('disabled');
                   $('#btnReturn').removeAttr('disabled');
            }else{
                $('#btnQuali').attr('disabled',true);
                 $('#btnReject').attr('disabled',true);   
                  $('#btnReturn').attr('disabled',true);   
            }
        }
        
       
        
    </script>

    </form>
</body>
</html>
