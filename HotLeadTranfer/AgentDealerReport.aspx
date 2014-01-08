<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentDealerReport.aspx.cs"
    Inherits="AgentDealerReport" %>

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

    <script type="text/javascript" language="javascript">window.history.forward(1);</script>

    <script type="text/javascript">
    
    function poptastic(url)
{
	newwindow=window.open(url,'name','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,height=420,width=300');
	    if (window.focus) {newwindow.focus()}
    }
    
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
            <td style="width: 250px;">
                <a>
                    <img src="images/logo2.png" /></a>
            </td>
            <td>
                <h1 style="border-bottom: none; padding-top: 5px;">
                    UNITED CAR EXCHANGE <span>Agent Dealer Sales Report</span></h1>
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
                                    <asp:LinkButton ID="lnkbtnCentralReport" runat="server" Text="Center report" PostBackUrl="~/CentralReport.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnIntromail" runat="server" Text="Intromail" PostBackUrl="~/Intromail.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnAdmin" runat="server" Text="User mgmt" PostBackUrl="~/UserManagement.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnNewSale" runat="server" Text="New sale" PostBackUrl="~/NewSale.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnTransfers" runat="server" Text="Transfers" PostBackUrl="~/LiveTransfers.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnDealerSale" runat="server" Text="New dealer sale" PostBackUrl="~/NewDealerSale.aspx"
                                        Enabled="false"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkbtnReports" runat="server" Text="My report" PostBackUrl="~/AgentReport.aspx"
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
    <div style="height: 10px;">
    </div>
    <div class="main" style="width: 90%; min-width: 1100px; margin: 0 10px 10px 10px;">
        <asp:UpdatePanel ID="updtpnltblGrdcar" runat="server">
            <ContentTemplate>
                <table style="width: 100%; display: block;" id="tblGrdcar" runat="server">
                    <tr>
                        <td>
                            <table style="float: left; width: 533px; border-collapse: initial; padding: 10px 0 15px 0;
                                border: #ccc 1px solid; margin-right: 15px; height: 70px;">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdbtnSales" Text="Sale(s)" GroupName="Option" runat="server"
                                            Checked="true" OnCheckedChanged="rdbtnSales_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rdbtnProspects" Text="Prospect(s)" GroupName="Option" runat="server"
                                            OnCheckedChanged="rdbtnProspects_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rdbtnPromotionals" Text="Promotional(s)" GroupName="Option"
                                            runat="server" OnCheckedChanged="rdbtnPromotionals_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="border-collapse: collapse; float: right;
                                border-collapse: initial; padding: 10px; border: #ccc 1px solid; height: 70px;"
                                class="table12">
                                <tr>
                                    <td style="width: 140px;">
                                        <b>Total Sale(s):</b>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:Label ID="lblTotSales" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 140px;">
                                        <b>Total Prospect(s):</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotProspects" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Total Promotional(s):</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotPromotionals" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        &nbsp;
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
                                            <td>
                                                <asp:Label ID="lblResHead" runat="server"></asp:Label>
                                                <%--<a style="display:inline-block; text-align:right; float:right"  href="javascript:poptastic('Ticker.aspx?CID=7&CNAME=USWB');">Sales Ticker</a>--%>
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
                                    <td align="left" style="width: 25%;">
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
                                    <td align="left" style="width: 25%;">
                                        <asp:Label ID="lblResCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div style="width: 100%;" id="divDealerSaleInfo" runat="server">
                                <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                    <asp:UpdatePanel ID="UpdtpnlHeader" runat="server">
                                        <ContentTemplate>
                                            <table class="grid1 " cellpadding="0" cellspacing="0" style="position: absolute;
                                                top: 2px; width: 1328px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed">
                                                    <td width="80" align="left">
                                                        <asp:LinkButton ID="lnkDealerIDSort" runat="server" Text="Dealer ID &darr; &uarr;"
                                                            OnClick="lnkDealerIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkDealerShipName" runat="server" Text="Dlrship Name &darr; &uarr;"
                                                            OnClick="lnkDealerShipName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkPackageSort" runat="server" Text="Package &darr; &uarr;" OnClick="lnkPackageSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkSaleDateSort" runat="server" Text="Sale Dt &#8657" OnClick="lnkSaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkTargetDateSort" runat="server" Text="Target Dt &darr; &uarr;"
                                                            OnClick="lnkTargetDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkPromotionSort" runat="server" Text="Promotion &darr; &uarr;"
                                                            OnClick="lnkPromotionSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkAgentSort" runat="server" Text="Agent &darr; &uarr;" OnClick="lnkAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnDealerStatus" runat="server" Text="Status &darr; &uarr;"
                                                            OnClick="lnkbtnDealerStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnQCStatus" runat="server" Text="QC Status &darr; &uarr;"
                                                            OnClick="lnkbtnQCStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkbtnContactName" runat="server" Text="Contact Name &darr; &uarr;"
                                                            OnClick="lnkbtnContactName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="90" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkPhoneSort" runat="server" Text="Phone &darr; &uarr;" OnClick="lnkPhoneSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="60" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkbtnCitySort" runat="server" Text="City &darr; &uarr;" OnClick="lnkbtnCitySort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="60" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkbtnZipSort" runat="server" Text="Zip &darr; &uarr;" OnClick="lnkbtnZipSort_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="width: 1350px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 420px">
                                    <asp:Panel ID="pnl1" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdPnlGrid" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="txthdnSortOrder" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="txthdnSortColumnId" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1328px" ID="grdDealerSaleInfo" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                    ShowHeader="false" OnRowDataBound="grdDealerSaleInfo_RowDataBound">
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
                                                                <%--<asp:LinkButton ID="lnkDealerID" runat="server" Text='<%# Eval("DealerUID")%>' CommandArgument='<%# Eval("DealerSaleID")%>'
                                                                    CommandName="EditSale"></asp:LinkButton>--%>
                                                                <asp:Label ID="lblDealerID" Text='<%# Eval("DealerUID")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerShipName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerShipName"),20)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPackage" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPackName" runat="server" Value='<%# Eval("PackageCode")%>' />
                                                                <asp:HiddenField ID="hdnPackCost" runat="server" Value='<%# Eval("Price")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSaleDt" runat="server" Text='<%# Bind("SaleDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTargetDt" runat="server" Text='<%# Bind("TargetSignupDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPromotion" runat="server" Text='<%# Eval("PromotionOptionCode")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgent" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"AgentUFirstName"),13)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerStatus" runat="server" Text='<%# Eval("DealerStatusName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQCStatus" runat="server" Text='<%# Eval("QCStatusName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerContactName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerContactName"),13)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPhoneNum" runat="server" Value='<%# Eval("DealerContactPhone")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerCity" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerCity"),10)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerZip" runat="server" Text='<%# Eval("DealerZip")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdDealerSaleInfo" EventName="Sorting" />
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
                <table style="width: 100%; display: block;" id="tblProspects" runat="server">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 25%;">
                                        <h2 style="margin: 0; padding: 0;">
                                            Prospect(s)</h2>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblProspectRes" Font-Size="12px" Font-Bold="true" ForeColor="Black"
                                                    runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" style="width: 25%;">
                                        <asp:Label ID="lblProspectResCount" runat="server"></asp:Label>
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
                                                top: 2px; width: 1328px; background: #fff; border-top: #fff 2px solid;">
                                                <tr class="tbHed">
                                                    <td width="80" align="left">
                                                        <asp:LinkButton ID="lnkProsDealerIDSort" runat="server" Text="Dealer ID &darr; &uarr;" OnClick="lnkProsDealerIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkProsDealerShipName" runat="server" Text="Dlrship Name &darr; &uarr;" OnClick="lnkProsDealerShipName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkProsPackageSort" runat="server" Text="Package &darr; &uarr;" OnClick="lnkProsPackageSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkProsSaleDateSort" runat="server" Text="Enter Dt &#8657" OnClick="lnkProsSaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkProsTargetDateSort" runat="server" Text="Target Dt &darr; &uarr;" OnClick="lnkProsTargetDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkProsPromotionSort" runat="server" Text="Promotion &darr; &uarr;" OnClick="lnkProsPromotionSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkProsAgentSort" runat="server" Text="Agent &darr; &uarr;" OnClick="lnkProsAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnProsDealerStatus" runat="server" Text="Status &darr; &uarr;" OnClick="lnkbtnProsDealerStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnProsQCStatus" runat="server" Text="QC Status &darr; &uarr;" OnClick="lnkbtnProsQCStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkbtnProsContactName" runat="server" Text="Contact Name &darr; &uarr;" OnClick="lnkbtnProsContactName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="90" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkProsPhoneSort" runat="server" Text="Phone &darr; &uarr;" OnClick="lnkProsPhoneSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="60" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkbtnProsCitySort" runat="server" Text="City &darr; &uarr;" OnClick="lnkbtnProsCitySort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="60" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkbtnProsZipSort" runat="server" Text="Zip &darr; &uarr;" OnClick="lnkbtnProsZipSort_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="width: 1350px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 420px">
                                    <asp:Panel ID="Panel1" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="Hidden1" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="Hidden2" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1328px" ID="grdDealerProsInfo" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                    ShowHeader="false" OnRowDataBound="grdDealerProsInfo_RowDataBound" OnRowCommand="grdDealerProsInfo_RowCommand">
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
                                                                <asp:LinkButton ID="lnkDealerID" runat="server" Text='<%# Eval("DealerUID")%>' CommandArgument='<%# Eval("DealerSaleID")%>'
                                                                    CommandName="EditSale"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerShipName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerShipName"),20)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPackage" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPackName" runat="server" Value='<%# Eval("PackageCode")%>' />
                                                                <asp:HiddenField ID="hdnPackCost" runat="server" Value='<%# Eval("Price")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSaleDt" runat="server" Text='<%# Bind("DealerEnterDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTargetDt" runat="server" Text='<%# Bind("TargetSignupDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPromotion" runat="server" Text='<%# Eval("PromotionOptionCode")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgent" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"AgentUFirstName"),13)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerStatus" runat="server" Text='<%# Eval("DealerStatusName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQCStatus" runat="server" Text='<%# Eval("QCStatusName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerContactName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerContactName"),13)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPhoneNum" runat="server" Value='<%# Eval("DealerContactPhone")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerCity" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerCity"),10)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerZip" runat="server" Text='<%# Eval("DealerZip")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdDealerProsInfo" EventName="Sorting" />
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
                <table style="width: 100%; display: block;" id="tblPromotionals" runat="server">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 25%;">
                                        <h2 style="margin: 0; padding: 0;">
                                            Promotional(s)</h2>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblPromotionalRes" Font-Size="12px" Font-Bold="true" ForeColor="Black"
                                                    runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" style="width: 25%;">
                                        <asp:Label ID="lblPromotionalResCount" runat="server"></asp:Label>
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
                                                    <td width="80" align="left">
                                                        <asp:LinkButton ID="lnkPromotDealerIDSort" runat="server" Text="Dealer ID &darr; &uarr;"
                                                            OnClick="lnkPromotDealerIDSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkPromotDealerShipName" runat="server" Text="Dlrship Name &darr; &uarr;"
                                                            OnClick="lnkPromotDealerShipName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="80" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkPromotPackageSort" runat="server" Text="Package &darr; &uarr;"
                                                            OnClick="lnkPromotPackageSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkPromotSaleDateSort" runat="server" Text="Sale Dt &#8657" OnClick="lnkPromotSaleDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <asp:LinkButton ID="lnkPromotTargetDateSort" runat="server" Text="Target Dt &darr; &uarr;"
                                                            OnClick="lnkPromotTargetDateSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkPromotPromotionSort" runat="server" Text="Promotion &darr; &uarr;"
                                                            OnClick="lnkPromotPromotionSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="110" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkPromotAgentSort" runat="server" Text="Agent &darr; &uarr;"
                                                            OnClick="lnkPromotAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnPromotDealerStatus" runat="server" Text="Status &darr; &uarr;"
                                                            OnClick="lnkbtnPromotDealerStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnPromotQCStatus" runat="server" Text="QC Status &darr; &uarr;"
                                                            OnClick="lnkbtnPromotQCStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <%--Name--%>
                                                        <asp:LinkButton ID="lnkbtnPromotContactName" runat="server" Text="Contact Name &darr; &uarr;"
                                                            OnClick="lnkbtnPromotContactName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="90" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkPromotPhoneSort" runat="server" Text="Phone &darr; &uarr;"
                                                            OnClick="lnkPromotPhoneSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="60" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkbtnPromotCitySort" runat="server" Text="City &darr; &uarr;"
                                                            OnClick="lnkbtnPromotCitySort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="60" align="left">
                                                        <%--Phone--%>
                                                        <asp:LinkButton ID="lnkbtnPromotZipSort" runat="server" Text="Zip &darr; &uarr;"
                                                            OnClick="lnkbtnPromotZipSort_Click"></asp:LinkButton>
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
                                                <asp:GridView Width="1328px" ID="grdDealerPromotInfo" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                    ShowHeader="false" OnRowDataBound="grdDealerPromotInfo_RowDataBound">
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
                                                                <%--<asp:LinkButton ID="lnkDealerID" runat="server" Text='<%# Eval("DealerUID")%>' CommandArgument='<%# Eval("DealerSaleID")%>'
                                                                    CommandName="EditSale"></asp:LinkButton>--%>
                                                                <asp:Label ID="lblDealerID" Text='<%# Eval("DealerUID")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerShipName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerShipName"),20)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPackage" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPackName" runat="server" Value='<%# Eval("PackageCode")%>' />
                                                                <asp:HiddenField ID="hdnPackCost" runat="server" Value='<%# Eval("Price")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSaleDt" runat="server" Text='<%# Bind("SaleDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTargetDt" runat="server" Text='<%# Bind("TargetSignupDate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPromotion" runat="server" Text='<%# Eval("PromotionOptionCode")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgent" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"AgentUFirstName"),13)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerStatus" runat="server" Text='<%# Eval("DealerStatusName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQCStatus" runat="server" Text='<%# Eval("QCStatusName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerContactName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerContactName"),13)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnPhoneNum" runat="server" Value='<%# Eval("DealerContactPhone")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerCity" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"DealerCity"),10)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerZip" runat="server" Text='<%# Eval("DealerZip")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdDealerPromotInfo" EventName="Sorting" />
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
