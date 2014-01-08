<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCVerifyReport.aspx.cs" Inherits="QCVerifyReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    
    <link href="css/css2.css" rel="stylesheet" type="text/css" />
    <link href="css/inputs.css" rel="stylesheet" type="text/css" />
    <style>
        .pageHead{ text-align:center; }
        .block
        {
            display: block;
            border: #999 1px solid;
            padding: 1px;
            margin-bottom:10px;
            box-shadow:0 2px 3px rgba(0,0,0,0.1)
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
        
        select, textarea, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input
        {
            height:18px;	
        	}
        
    </style>

    <script type="text/javascript">

function openpopup()
{
window.open("Ticker.aspx ")
}
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 500px;">
        <table style="width: 100%; border-collapse: collapse;">
            <tr>
                <td>                    
                    <h2 class="pageHead">QC Check List</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="block">
                        <h3 class="blockHead">
                            Package Details</h3>
                        <div class="inner">
                            Package: &nbsp;  &nbsp;<asp:CheckBox id="ChkPaY" runat="server" /> $99
                            &nbsp;  &nbsp;
                              <asp:CheckBox id="ChkPaN" runat="server" /> $199  
                              &nbsp;  &nbsp;
                              Other  &nbsp;  &nbsp;<asp:TextBox ID="PackEnter" runat="server" ></asp:TextBox>                          
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="block">
                        <h3 class="blockHead">
                            Seller Info</h3>
                        <div class="inner">
                        <table></table>
                            Seller Name&nbsp;  &nbsp;<asp:CheckBox ID="ChksellNameY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChksellNameN" runat="server" /> No<br />
                            
                             Phone &nbsp;  &nbsp;<asp:CheckBox ID="ChksellPhnY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChksellPhnN" runat="server" /> No<br />
                            
                             Email &nbsp;  &nbsp;<asp:CheckBox ID="ChkEmailY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkEmailN" runat="server" /> No &nbsp;  &nbsp;
                            <asp:TextBox ID="EmailEdit" runat="server" Visible="false"></asp:TextBox>
                             <br />
                            
                             Address &nbsp;  &nbsp;<asp:CheckBox ID="ChkAddrY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkAddrN" runat="server" /> No
                           
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="block">
                        <h3 class="blockHead">
                            Vehicle Information</h3>
                        <div class="inner">
                            Make and Model&nbsp;  &nbsp;<asp:CheckBox ID="chkmakmodlY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="chkmakmodlN" runat="server" /> No&nbsp;  &nbsp;
                            <asp:TextBox ID="txtmakmodlEdit" runat="server" Visible="false"></asp:TextBox>
                            
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="block">
                        <h3 class="blockHead">
                            Payment Details</h3>
                        <div class="inner">
                            
                             <strong style="width: 50px">Payment Type</strong><span style="font-weight: bold">
                               <asp:RadioButton ID="PaymentModeCC" CssClass="noLM" Text="" GroupName="Cylinders"
                                runat="server" oncheckedchanged="PaymentModeCC_CheckedChanged" AutoPostBack="true"/><span class="featNon">CC</span>
                               <asp:RadioButton ID="PaymentModeCheck" CssClass="noLM" Text="" GroupName="Cylinders"
                               runat="server" oncheckedchanged="PaymentModeCheck_CheckedChanged" AutoPostBack="true"  /><span class="featNon" AutoPostBack="true">Cheque</span>
                               </span>
                            
                            
                             
                         <%--   <asp:RadioButton ID="PaymentModeCC" runat="server"/>
                            <asp:RadioButton ID="PaymentModeCheck" runat="server"/>
                          --%>
                            <div id="dvcreditcard" runat="server" visible="false">
                                <!-- If Credit Card -->
                                Payment Method&nbsp;  &nbsp;<asp:CheckBox ID="chkPaymethodY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="chkPaymethodN" runat="server" /> No&nbsp;  &nbsp;<br />
                               
                                CardHolderName&nbsp;  &nbsp;<asp:CheckBox ID="ChkHnameY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkHnameN" runat="server" /> No&nbsp;  &nbsp;<br />
                                
                                CreditCard No.&nbsp;  &nbsp;<asp:CheckBox ID="ChkCredCardY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkCredCardN" runat="server" /> No&nbsp;  &nbsp;<br />
                               
                                Expiry Date&nbsp;  &nbsp;<asp:CheckBox ID="chkExpDateY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="chkExpDateN" runat="server" /> No&nbsp;  &nbsp;<br />
                               
                                CVV&nbsp;  &nbsp;<asp:CheckBox ID="chkCvvY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="chkCvvN" runat="server" /> No&nbsp;  &nbsp;<br />
                               
                                Billing Address&nbsp;  &nbsp;<asp:CheckBox ID="ChkBillAddY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkBillAddn" runat="server" /> No&nbsp;  &nbsp;
                            </div>
                            <div id="divcheque" runat="server" visible="false">
                            <br />
                                <!-- If Check -->
                                Account Holder Name&nbsp;  &nbsp;<asp:CheckBox ID="ckAccHNameY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ckAccHNameN" runat="server" /> No&nbsp;  &nbsp;<br />
                                Bank Name&nbsp;  &nbsp;<asp:CheckBox ID="ChkBankNameY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkBankNameN" runat="server" /> No&nbsp;  &nbsp;<br />
                                Account Name&nbsp;  &nbsp;<asp:CheckBox ID="ChkAccNameY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkAccNameN" runat="server" /> No&nbsp;  &nbsp;<br />
                                Routing Namee&nbsp;  &nbsp;<asp:CheckBox ID="chkRoutNoY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="chkRoutNoN" runat="server" /> No&nbsp;  &nbsp;<br />
                                Account Type&nbsp;  &nbsp;<asp:CheckBox ID="ChkAccTypY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="ChkAccTypN" runat="server" /> No
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="block">
                        <h3 class="blockHead">
                            Payment Schedule</h3>
                        <div class="inner">
                              <strong style="width: 50px">PaymentSchedule</strong><span style="font-weight: bold">
                             <asp:RadioButton ID="rbtFullPay" CssClass="noLM" Text="" GroupName="PaymentSchedule"
                                 runat="server" oncheckedchanged="rbtFullPay_CheckedChanged" AutoPostBack="true"/><span class="featNon">Full Payment</span>
                             <asp:RadioButton ID="rbtPartialPay" CssClass="noLM" Text="" GroupName="PaymentSchedule"
                                 runat="server" oncheckedchanged="rbtPartialPay_CheckedChanged" AutoPostBack="true"  /><span class="featNon">Partial payment</span>
                             
                         </span>
                            <div id="divfullpaym" runat="server" visible="false">
                          <%-- <asp:RadioButton id="rbtFullPay" runat="server"/>Full Payment--%>
                           &nbsp;  &nbsp;<asp:CheckBox ID="PayScSFullPY" runat="server" />Yes &nbsp;  &nbsp;
                            <asp:CheckBox ID="PayScSFullPN" runat="server" /> No
                            </div>
                            <br />
                             <%--<asp:RadioButton id="rbtPartialPay" runat="server"/>Partial payment--%>
                             <br />
                             <div id="divPartialpaym" runat="server" visible="false">
                            Tody's Payment <asp:TextBox id="txttodaypayment" runat="server"></asp:TextBox><br />
                            Next Payment <asp:TextBox id="txtnextpay" runat="server"></asp:TextBox>
                            </div>
                         
                            <br />
                            <br />
                            Voice File Confirmation:<asp:TextBox ID="txtvoicefileconf" runat="server"></asp:TextBox>
                            <br />
                            <br />
                            Notes<asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px;">
                    <div style="text-align: center">
                        <asp:Button ID="btnQuali" runat="server" Text="Qualified"  OnClick="btnQuali_Click" CssClass="btn btn-success" />
                        <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click" CssClass="btn btn-danger" />
                        <asp:Button ID="btnHold" runat="server" Text="Hold" OnClick="btnHold_Click" CssClass="btn btn-warning" />
                        <asp:Button ID="btnReturn" runat="server" Text="Return" OnClick="btnReturn_Click" CssClass="btn btn-info" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
