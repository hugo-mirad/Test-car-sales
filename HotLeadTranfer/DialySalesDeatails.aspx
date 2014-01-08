<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DialySalesDeatails.aspx.cs"
    Inherits="DialySalesDeatails" %>

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

    <link href="css/css2.css" rel="stylesheet" type="text/css" />
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link href="css/tabbed.css" rel="stylesheet" type="text/css" />

    <script type='text/javascript' language="javascript" src='js/jquery.alphanumeric.pack.js'></script>

    <script src="js/jquery.formatCurrency-1.4.0.js" type="text/javascript"></script>

    <script src="Static/JS/calendar.js" type="text/javascript"></script>

    <script src="js/jquery.idTabs.min.js" type="text/javascript"></script>

    <link href="Static/Css/calender.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">window.history.forward(1);</script>

    <script type="text/javascript">
 function pageLoad()
   { 
      //InitializeTimer();     
       // Tabs Enabling
            var settings = { start: 0, change: false };
            $("#adv1 ul").idTabs(settings, true); 
             
        $('.arrowRight').click(function(){
             var arr = $('#txtStartDate').val().split('/')
            var date = new Date(parseInt(arr[2]), parseInt(arr[0])-1, parseInt(arr[1]) - 1);
            $('#txtStartDate').val((date.getMonth()+1)+'/'+date.getDate()+'/'+date.getFullYear()); 
        });
        
        $('.arrowLeft').click(function(){
            var arr = $('#txtStartDate').val().split('/')
            var date = new Date(parseInt(arr[2]), parseInt(arr[0])-1, parseInt(arr[1]) + 1);
            $('#txtStartDate').val((date.getMonth()+1)+'/'+date.getDate()+'/'+date.getFullYear());            
            
        });
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
      <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updbtnSearch"
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
                    UNITED CAR EXCHANGE <span>Daily Sales Details</span></h1>
            </td>
            <td sstyle="width: 490px; padding-top: 10px;">
                <div class="loginStat">
                    Welcome &nbsp;<asp:Label ID="lblUserName" runat="server" Visible="false"></asp:Label>
                    <br />
                    <ul id="example3" class="dropdown-menu " data-options='{"sub_indicators":"true","drop_shadows":"true"}'>
                        <li style="background: none"><a href="#">Menu</a>
                            <ul>
                                <li>
                                    <asp:LinkButton ID="lnkTicker" runat="server" Text="Sales Ticker"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAgentReport" runat="server" Text="My report" PostBackUrl="~/AgentReport.aspx"
                                        Enabled="false"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnIntromail" runat="server" Text="Intromail" PostBackUrl="~/Intromail.aspx"
                                        Enabled="false"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAdmin" runat="server" Text="User mgmt" PostBackUrl="~/UserManagement.aspx"
                                        Enabled="false"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnNewSale" runat="server" Text="New sale" PostBackUrl="~/NewSale.aspx"
                                        Enabled="false"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnLeadsDownLoad" runat="server" Text="Leads Download" PostBackUrl="~/CenterWiseLeadDownload.aspx"
                                        Enabled="false"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnDealerSale" runat="server" Text="New dealer sale" PostBackUrl="~/NewDealerSale.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnMyDealerRep" runat="server" Text="My dealer report" PostBackUrl="~/AgentDealerReport.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li><a href="#">Dialy Sales</a>
                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="lnkbtnDialySales" runat="server" Text="Daily Sales Count" PostBackUrl="~/DialySalesCount.aspx"></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="linkButtonDetails" runat="server" Text="Daily Sales Details"
                                                PostBackUrl="~/DialySalesDeatails.aspx"></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkbtnsalesbyweekly" runat="server" Text="Weekly Sales By Package"
                                                PostBackUrl="~/WeeklySalesByPacakage.aspx"></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkbtnsalesbyweelday" runat="server" Text="Weekly Sales By Day"
                                                PostBackUrl="~/WeeklySalesByDay.aspx"></asp:LinkButton>
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
    <div class="main" style="width: 90%; min-width: 1100px; margin:10px">
        <table  style="width: auto; padding: 10px; border-collapse: initial; border: #ccc 1px solid;
            float: left; height: 61px;" runat="server">
            <tr>
                <td align="right" style="vertical-align: middle">
                    <b><asp:Label ID="lbldate" Text="Date:" runat="server"></asp:Label></b>
                </td>
                <td align="left" style="vertical-align: middle">
                    <asp:Label ID="stselectedDate" runat="server"></asp:Label>
                </td>
                <td style="padding: 0; text-align: left; vertical-align: middle">
                    <asp:TextBox ID="txtStartDate" runat="server" class="input1 " MaxLength="10" onkeypress="return isNumberKeyForDt(event)"
                        Width="70px" BorderColor="Black"></asp:TextBox>&nbsp;
                   <span id="img1" runat="server" class="arrows arrowRight">&laquo;</span>
                </td>
                <td style="width: 0px; text-align: center; padding: 0;">
                </td>
                <td style="text-align: left; padding: 0 20px 0 0; vertical-align: middle">
                    <asp:TextBox ID="txtEndDate" runat="server" class="input1 " MaxLength="10" onkeypress="return isNumberKeyForDt(event)"
                        Width="70px" Style="display: none;"></asp:TextBox>&nbsp;
                    <span id="imgcal" runat="server" class="arrows arrowLeft">&raquo;</span>
                </td>
                <td style="vertical-align: middle; padding-left: 15px;">
                    <b><span id="lblAge" runat="server">Centers:</span></b>
                    <asp:DropDownList ID="ddlCenters" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="vertical-align: middle">
                  <asp:UpdatePanel ID="updbtnSearch" runat="server">
                        <ContentTemplate>
                    <asp:Button ID="Generate" runat="server" Text="Generate" CssClass="g-button g-button-submit"
                        OnClick="Change_Click" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td style="vertical-align: middle; padding-left: 5px;">
                    Report DateTime:<asp:Label ID="dtreportTime" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
          <div class="clear">
        </div>
    </div>
     <div class="clear">
    </div>
    <div>
        <asp:UpdatePanel ID="updtpnlTotal" runat="server">
            <ContentTemplate>
                <div>
                    <!-- Tab Start  -->
                    <div id="adv1" class="usual">
                        <ul style="padding: 0">
                            <li class="tab1"><a href="#tabs-1">Agents</a></li>
                            <li class="tab1"><a href="#tabs-2">Verifiers</a></li>
                        </ul>
                        <div class="clear">
                            &nbsp;</div>
                        <div id="tabs-1">
                            <div>
                                <%-- <table style="width: 100%;">--%>
                                <tr>
                                    <td>
                                        <asp:Repeater ID="Rpt_Agents" runat="server" OnItemDataBound="Rpt_Agents_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="AgentName" runat="server" Text='<%# Eval("AgentUserName") %>'></asp:Label>
                                                <asp:HiddenField ID="AgentUid" runat="server" Value='<%# Eval("AgentUID") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="hdnagcentr" runat="server" Value='<%# Eval("AgentCenterID") %>'>
                                                </asp:HiddenField>
                                                <asp:Repeater ID="Rpt_Customer" runat="server" OnItemDataBound="Rpt_Customer_ItemDataBound">
                                                    <HeaderTemplate>
                                                   <table style="width: 100%;" class="grid1">
                                                        <tr class="tbHed">
                                                            <td style="width:55px;">
                                                                SNo
                                                            </td>
                                                            <td style="width:180px;">
                                                                Customer Name
                                                            </td>
                                                            <td style="width:80px;">
                                                                Verifier
                                                            </td>
                                                            <td style="width:100px;">
                                                                Package
                                                            </td>
                                                            <td style="width:50px;">
                                                                $
                                                            </td>
                                                            <td style="width:60px;">
                                                                Type
                                                            </td>
                                                            <td style="width:80px;">
                                                                Status
                                                            </td>
                                                        </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                         <table style="width: 100%;" class="grid1">
                                                        <tr >
                                                            <td style="width:55px;">
                                                            
                                                                <asp:Label ID="SNo" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width:180px;">
                                                                <asp:Label ID="lblCustName" runat="server" Text='<%# Eval("FName") %>'></asp:Label>
                                                            </td>
                                                            <td style="width:80px;">
                                                                <asp:Label ID="lblLName" runat="server" Text='<%# Eval("VerifierAgent") %>'></asp:Label>
                                                            </td>
                                                            <td style="width:100px;">
                                                                <asp:Label ID="lblPackId" runat="server" Text='<%# Eval("PackageId") %>'></asp:Label>
                                                            </td>
                                                            <td  style="width:50px;">
                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                                <asp:HiddenField ID="HdnAmount" runat="server" Value='<%# Eval("Amount2") %>' />
                                                            </td>
                                                            <td style="width:60px;">
                                                                <asp:Label ID="lblPadTyp" runat="server" Text='<%# Eval("pmntType") %>'></asp:Label>
                                                            </td>
                                                            <td style="width:80px;">
                                                                <asp:Label ID="lblPaidSat" runat="server" Text='<%# Eval("PSStatusID") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                     </table>
                                                      </ItemTemplate>
                                                      <FooterTemplate>
                                                       <table style="width: 100%;" class="grid1">
                                                        <tr>
                                                            <td colspan="7" align="center">
                                                                <asp:Label ID="lblsubmit1" runat="server" Text="Submitted"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7" align="center">
                                                                <asp:Label ID="lblpaid1" runat="server" Text="Paid"></asp:Label>
                                                            </td>
                                                        </tr>
                                                         
                                                       </table>
                                                      </FooterTemplate>
                                                </asp:Repeater>
                                              
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                </table>
                            </div>
                            <!-- Tab End  -->
                        </div>
                        <div id="tabs-2">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Repeater ID="Rpt_Verifiers" runat="server" OnItemDataBound="Rpt_Verifiers_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="AgentName" runat="server" Text='<%# Eval("AgentUserName") %>'></asp:Label>
                                                <asp:HiddenField ID="AgentUid" runat="server" Value='<%# Eval("AgentUID") %>'></asp:HiddenField>
                                                <asp:Repeater ID="Rpt_VerifierCustomer" runat="server" OnItemDataBound="Rpt_VerifierCustomer_ItemDataBound">
                                                   <HeaderTemplate>
                                                        <table style="width: 100%;" class="grid1">
                                                            <tr class="tbHed">
                                                                <td style="width:55px;">
                                                                    SNo
                                                                </td>
                                                                <td style="width:180px;">
                                                                    Customer Name
                                                                </td>
                                                                <td style="width:80px;">
                                                                    Verifier
                                                                </td>
                                                                <td style="width:100px;">
                                                                    Package
                                                                </td>
                                                                <td style="width:50px;">
                                                                    $
                                                                </td>
                                                                <td style="width:60px;">
                                                                    Type
                                                                </td>
                                                                <td style="width:80px;">
                                                                    Status
                                                                </td>
                                                            </tr>
                                                            </HeaderTemplate>
                                                             <ItemTemplate>
                                                         <table style="width: 100%;" class="grid1">
                                                            <tr>
                                                                <td style="width:55px;">
                                                                    <asp:Label ID="VSNo" runat="server"></asp:Label>
                                                                </td>
                                                                <td style="width:180px;">
                                                                    <asp:Label ID="VlblCustName" runat="server" Text='<%# Eval("FName") %>'></asp:Label>
                                                                </td>
                                                                <td style="width:80px;">
                                                                    <asp:Label ID="VlblLName" runat="server" Text='<%# Eval("VerifierAgent") %>'></asp:Label>
                                                                </td>
                                                                <td style="width:100px;">
                                                                    <asp:Label ID="VlblPackId" runat="server" Text='<%# Eval("PackageId") %>'></asp:Label>
                                                                </td>
                                                                <td style="width:50px;">
                                                                    <asp:Label ID="VlblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                                    <asp:HiddenField ID="VHdnAmount" runat="server" Value='<%# Eval("Amount2") %>' />
                                                                </td>
                                                                <td style="width:60px;">
                                                                    <asp:Label ID="VlblPadTyp" runat="server" Text='<%# Eval("pmntType") %>'></asp:Label>
                                                                </td>
                                                                <td style="width:80px;">
                                                                    <asp:Label ID="VlblPaidSat" runat="server" Text='<%# Eval("PSStatusID") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            </table>
                                                            </ItemTemplate>
                                                              <FooterTemplate>
                                                       <table style="width: 100%;" class="grid1">
                                                            <tr>
                                                                <td colspan="7" align="center">
                                                                    <asp:Label ID="Vlblsubmit1" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="7" align="center">
                                                                    <asp:Label ID="Vlblpaid1" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                           </table>
                                                      </FooterTemplate>
                                                      
                                                  
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
