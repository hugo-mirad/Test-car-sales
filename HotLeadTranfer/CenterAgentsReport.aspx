<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CenterAgentsReport.aspx.cs"
    Inherits="CenterAgentsReport" %>

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

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/WebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <asp:UpdateProgress ID="Progress" runat="server" AssociatedUpdatePanelID="updtpnlTableShow"
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
                    UNITED CAR EXCHANGE <span>AGENT PERFORMANCE REPORT</span></h1>
            </td>
            <td style="width: 380px; padding-top: 10px;">
                <div class="loginStat">
                    Welcome &nbsp;<asp:Label ID="lblUserName" runat="server" Visible="false"></asp:Label>
                    <br />
                    <ul class="menu2">
                        <li>
                            <asp:LinkButton ID="lnkbtnAgentReport" runat="server" Text="My report" PostBackUrl="~/AgentReport.aspx"
                                Enabled="false"></asp:LinkButton>
                            <ul>
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
    <div class="main">
        <div class="clear">
            &nbsp;</div>
        <asp:UpdatePanel ID="updtpnlTableShow" runat="server">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 70%">
                            <div style="float: left; width: 100px; text-align: left; padding-top: 2px;">
                                <b>Agent Status: </b>
                            </div>
                            <div style="float: left; width: 350px; text-align: left;">
                                <asp:RadioButton ID="rdbtnActive" runat="server" Text="Active Now" GroupName="AgentStatus"
                                    OnCheckedChanged="rdbtnActive_Click" AutoPostBack="true" />
                                &nbsp; &nbsp;
                                <asp:RadioButton ID="rdbtnActivePeriod" runat="server" Text="Active During Period"
                                    GroupName="AgentStatus" OnCheckedChanged="rdbtnActivePeriod_Click" AutoPostBack="true" />
                                &nbsp; &nbsp;
                                <asp:RadioButton ID="rdbtnAll" runat="server" Text="All" GroupName="AgentStatus"
                                    Checked="true" OnCheckedChanged="rdbtnAll_Click" AutoPostBack="true" />
                            </div>
                            <div class="clear">
                                &nbsp;</div>
                            <br />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkbtnCenterReport" Text="Center Report" runat="server" PostBackUrl="~/CentralReport.aspx"></asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 270px; padding-top: 2px;">
                                        <table style="width: 270px; float: left; margin-left: 0px; margin-right: 13px;">
                                            <tr>
                                                <td colspan="3" align="left">
                                                    <div style="border-bottom: 1px #666 solid; text-align: center; width: 240px; margin: 0 auto 2px auto;
                                                        font-weight: bold; padding-bottom: 2px;">
                                                        Date range</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45%; text-align: right">
                                                    <asp:TextBox ID="txtStartDate" runat="server" class="input1 " MaxLength="10" onkeypress="return isNumberKeyForDt(event)"
                                                        Width="70px"></asp:TextBox>&nbsp;
                                                    <img id="imgcal" runat="server" style="border-right: 0px; border-top: 0px; border-left: 0px;
                                                        border-bottom: 0px" title="Calendar Control" onclick="displayCalendar(document.forms[0].txtStartDate,'mm/dd/yyyy',this);"
                                                        alt="Calendar Control" src="images/Calender.gif" width="18" />
                                                </td>
                                                <td style="width: 26px; text-align: center">
                                                    <b>to</b>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtEndDate" runat="server" class="input1 " MaxLength="10" onkeypress="return isNumberKeyForDt(event)"
                                                        Width="70px"></asp:TextBox>&nbsp;
                                                    <img id="img1" runat="server" style="border-right: 0px; border-top: 0px; border-left: 0px;
                                                        border-bottom: 0px" title="Calendar Control" onclick="displayCalendar(document.forms[0].txtEndDate,'mm/dd/yyyy',this);"
                                                        alt="Calendar Control" src="images/Calender.gif" width="18" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 105px; padding-top: 28px;">
                                        <div style="float: left; width: 100px;">
                                            <asp:UpdatePanel ID="updbtnSearch" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSearchMonth" runat="server" CssClass="g-button g-button-submit"
                                                        Text="Generate" OnClientClick="return ValidateData();" OnClick="btnSearchMonth_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updbtnSearch"
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
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="clear" style="height: 10px;">
            &nbsp;</div>
        <table style="width: 100%;">
            <tr>
                <td style="vertical-align: top;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table style="width: 90%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="80%">
                                        <asp:Label ID="lblResHead" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right; padding-right: 25px;">
                                        Report date:
                                        <asp:Label ID="lblResCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="agentSalesHolder">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table class="agentSales">
                                    <tr class="tbHed3">
                                        <th>
                                            Agent Name
                                        </th>
                                        <th>
                                            Vehicle Type
                                        </th>
                                        <th>
                                            Package
                                        </th>
                                        <th style="text-align: center">
                                            No Of Sales
                                        </th>
                                        <th style="text-align: center">
                                            Sale Amount<br />
                                            $
                                        </th>
                                        <th style="text-align: center">
                                            Paid Amount<br />
                                            $
                                        </th>
                                        <th style="text-align: center">
                                            Prev Sale Paid Amount<br />
                                            $
                                        </th>
                                        <th style="text-align: center">
                                            Total<br />
                                            $
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="8" style="width: 100%; padding: 0; margin: 0" class="hold1">
                                            <asp:PlaceHolder ID="plcholgrid" runat="server"></asp:PlaceHolder>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                    <tr class="grandTotal">
                                        <td class="agentName">
                                            Grand Total
                                        </td>
                                        <td class="vehicleType">
                                        </td>
                                        <td class="package">
                                        </td>
                                        <td class="noSales">
                                            <asp:Label ID="lblGrandNoOfSales" runat="server"></asp:Label>
                                        </td>
                                        <td class="slAmount">
                                            <asp:Label ID="lblGrandSaleAmount" runat="server"></asp:Label>
                                        </td>
                                        <td class="pAmount">
                                            <asp:Label ID="lblGrandPaidAmount" runat="server"></asp:Label>
                                        </td>
                                        <td class="prevS">
                                            <asp:Label ID="lblgrandPrevAmount" runat="server"></asp:Label>
                                        </td>
                                        <td class="total">
                                            <asp:Label ID="lblGrandTotalAmount" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
            &nbsp;</div>
    </div>
    </form>
</body>
</html>
