<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCReportForDealer.aspx.cs"
    Inherits="QCReportForDealer" %>

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

    <script type='text/javascript' language="javascript" src='js/jquery.alphanumeric.pack.js'></script>

    <script src="js/jquery.formatCurrency-1.4.0.js" type="text/javascript"></script>

    <script src="Static/JS/calendar.js" type="text/javascript"></script>

    <link href="Static/Css/calender.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">        window.history.forward(1);</script>

    <script type="text/javascript" language="javascript">
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,height=420,width=300');
            if (window.focus) { newwindow.focus() }
        }
        function isNumberKey(evt) {
            debugger
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isNumberKeyForDt(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47)
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
    
    </script>

    <script type="text/javascript" language="javascript">
        function ClosePopup() {
            $find('<%= MPEUpdate.ClientID%>').hide();
            return false;
        }
        function PayInfoChanges() {
            debugger;
            if (document.getElementById('<%=hdnPopPayType.ClientID%>').value == "6") {
                document.getElementById('<%=divTransID.ClientID%>').style.display = "none";
                document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "none";
                document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "none";
            }
            else {
                if ((document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "1") || (document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "7") || (document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "8")) {
                    document.getElementById('<%=divTransID.ClientID%>').style.display = "block";
                    document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "block";
                    document.getElementById('<%=divReason.ClientID%>').style.display = "none";
                    document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "block";
                    if (document.getElementById('<%=hdnPophdnAmount.ClientID%>').value == "0") {
                        document.getElementById('<%=divTransID.ClientID%>').style.display = "none";
                        document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "none";
                        document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "none";
                    }
                }
                else {
                    document.getElementById('<%=divTransID.ClientID%>').style.display = "none";
                    document.getElementById('<%=divPaymentDate.ClientID%>').style.display = "none";
                    document.getElementById('<%=divPaymentAmount.ClientID%>').style.display = "none";
                    if (document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "2") {
                        document.getElementById('<%=divReason.ClientID%>').style.display = "block";
                    }
                    else {
                        document.getElementById('<%=divReason.ClientID%>').style.display = "none";
                    }
                }
            }
            return false;
        }
        function ValidateUpdate() {

            var valid = true;
            if ((document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "1") || (document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "7") || (document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "8")) {
                if (document.getElementById('<%=hdnPophdnAmount.ClientID%>').value != "0") {
                    if (document.getElementById('<%= ddlPaymentDate.ClientID%>').value == "0") {
                        document.getElementById('<%= ddlPaymentDate.ClientID%>').focus();
                        alert("Select payment date");
                        document.getElementById('<%=ddlPaymentDate.ClientID%>').focus()
                        valid = false;
                        return valid;
                    }
                    if (document.getElementById('<%= txtPaymentAmountInPop.ClientID%>').value.trim().length < 1) {
                        alert("Please enter amount paid");
                        valid = false;
                        document.getElementById('txtPaymentAmountInPop').focus();
                        return valid;
                    }
                    if (document.getElementById('<%= txtPaymentAmountInPop.ClientID%>').value.trim() != document.getElementById('<%= hdnPophdnAmount.ClientID%>').value) {
                        alert("Amount entered not match with amount need to be paid now");
                        valid = false;
                        document.getElementById('txtPaymentAmountInPop').focus();
                        return valid;
                    }
                    if (document.getElementById('<%= txtPaytransID.ClientID%>').value.trim().length < 1) {
                        alert("Please enter transaction id");
                        valid = false;
                        document.getElementById('txtPaytransID').focus();
                        return valid;
                    }
                }
            }

            if (document.getElementById('<%=ddlPaymentStatus.ClientID%>').value == "2") {
                if (document.getElementById('<%= ddlPayCancelReason.ClientID%>').value == "0") {
                    document.getElementById('<%= ddlPayCancelReason.ClientID%>').focus();
                    alert("Select payment reject reason");
                    document.getElementById('<%=ddlPayCancelReason.ClientID%>').focus()
                    valid = false;
                    return valid;
                }
                if (document.getElementById('<%= txtPaymentNewNotes.ClientID%>').value.trim().length < 1) {
                    alert("Please enter payment notes");
                    valid = false;
                    document.getElementById('txtPaymentNewNotes').focus();
                    return valid;
                }
            }
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
            <td style="width: 260px;">
                <a>
                    <img src="images/logo2.png" /></a>
            </td>
            <td>
                <h1 style="border-bottom: none; padding-top: 5px;">
                    UNITED CAR EXCHANGE <span>Dealer QC Report</span></h1>
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
                                    <asp:LinkButton ID="lnkbtnQCReport" runat="server" Text="QC Report" PostBackUrl="~/QCReport.aspx"></asp:LinkButton>
                                </li>
                                <li>
                                <asp:LinkButton ID="lnkbtnBulkReport" runat="server" Text="Bulk Process" PostBackUrl="~/BulkProcess.aspx"></asp:LinkButton>
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
    <div class="main" style="width: 90%; min-width: 1100px; margin: 0 10px 10px 10px">
        <asp:UpdatePanel ID="updtpnltblGrdcar" runat="server">
            <ContentTemplate>
                <table style="width: 100%; display: block;" id="tblGrdcar" runat="server">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <table style="width: 610px; padding: 10px; border-collapse: initial; border: #ccc 1px solid;"
                                        cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding: 0; width: 300px">
                                                <asp:RadioButton ID="rdbtnQCOpen" runat="server" Text="QC Open" GroupName="QCReport"
                                                    CssClass="noLM" Style="margin-left: 0;" />
                                                <asp:RadioButton ID="rdbtnQCDonepayopen" Text="QC Done pay open" runat="server" GroupName="QCReport" />
                                                <asp:RadioButton ID="rdbtnAll" runat="server" Text="All" GroupName="QCReport" Checked="true" />
                                            </td>
                                            <td style="text-align: left; padding-right: 25px; width: 150px">
                                                Center:
                                                <asp:DropDownList ID="ddlCenters" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updbtnSearch" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnGenerate" runat="server" CssClass="g-button g-button-submit" Text="Generate"
                                                            OnClick="btnGenerate_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 40%;">
                                        <%-- <h2 style="margin: 0; padding: 0;">
                                            Sale(s)</h2>--%>
                                        <asp:Label ID="lblResHead" runat="server"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdtpnldatResCounts" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblRes" Font-Size="12px" Font-Bold="true" ForeColor="Black" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" style="width: 40%;">
                                        <asp:Label ID="lblResCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div style="width: 100%;" id="divCarresults" runat="server">
                                <div style="width: 100%; position: relative; padding: 0 3px; height: 1px">
                                    <asp:UpdatePanel ID="UpdtpnlHeader" runat="server">
                                        <ContentTemplate>
                                            <table class="grid1 " cellpadding="0" cellspacing="0" style="position: absolute;
                                                top: 2px; width: 1588px; background: #fff; border-top: #fff 2px solid;">
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
                                                    <td width="160" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkAgentSort" runat="server" Text="Agent &darr; &uarr;" OnClick="lnkAgentSort_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="100" align="left">
                                                        <%--Package--%>
                                                        <asp:LinkButton ID="lnkPromotionSort" runat="server" Text="Promotion &darr; &uarr;"
                                                            OnClick="lnkPromotionSort_Click"></asp:LinkButton>
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
                                                    <td width="100" align="left">
                                                        <%--Agent--%>
                                                        <asp:LinkButton ID="lnkbtnPaymentStatus" runat="server" Text="Pmnt Status &darr; &uarr;"
                                                            OnClick="lnkbtnPaymentStatus_Click"></asp:LinkButton>
                                                    </td>
                                                    <td width="240" align="left">
                                                        Smartz St
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
                                <div style="width: 1610px; overflow-y: scroll; overflow-x: hidden; padding: 26px 3px 3px 3px;
                                    border: #ccc 1px solid; height: 420px">
                                    <asp:Panel ID="pnl1" Width="100%" runat="server">
                                        <asp:UpdatePanel ID="UpdPnlGrid" runat="server">
                                            <ContentTemplate>
                                                <input style="width: 91px" id="txthdnSortOrder" type="hidden" runat="server" enableviewstate="true" />
                                                <input style="width: 40px" id="txthdnSortColumnId" type="hidden" runat="server" enableviewstate="true" />
                                                <asp:GridView Width="1588px" ID="grdDealerSaleInfo" runat="server" CellSpacing="0"
                                                    CellPadding="0" CssClass="grid1" AutoGenerateColumns="False" GridLines="None"
                                                    ShowHeader="false" OnRowDataBound="grdDealerSaleInfo_RowDataBound" OnRowCommand="grdDealerSaleInfo_RowCommand">
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
                                                                <%--<asp:Label ID="lblDealerID" Text='<%# Eval("DealerUID")%>' runat="server"></asp:Label>--%>
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
                                                                <asp:Label ID="lblAgent" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnAgentID" runat="server" Value='<%# Eval("UCEDealerCoordinatorID")%>' />
                                                                <asp:HiddenField ID="hdnAgentName" runat="server" Value='<%# Eval("AgentUFirstName")%>' />
                                                                <asp:HiddenField ID="hdnAgentCenterID" runat="server" Value='<%# Eval("AgentCenterID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPromotion" runat="server" Text='<%# Eval("PromotionOptionCode")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerStatus" runat="server" Text='<%# Eval("DealerStatusName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQCStatus" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnQCStatusName" runat="server" Value='<%# Eval("QCStatusName")%>' />
                                                                <asp:HiddenField ID="hdnQCStatusID" runat="server" Value='<%# Eval("QCStatusID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnPaymentStatus" runat="server" CommandArgument='<%# Eval("DealerSaleID")%>'
                                                                    CommandName="EditPayInfo"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnPayStatus" runat="server" Value='<%# Eval("pmntStatus")%>' />
                                                                <asp:HiddenField ID="hdnPayStatusName" runat="server" Value='<%# Eval("PSStatusName")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnMoveSmartz" runat="server" CommandArgument='<%# Eval("DealerSaleID")%>'
                                                                    CommandName="MoveSmartz"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnSmartzStatus" runat="server" Value='<%# Eval("SmartzStatus")%>' />
                                                                <asp:HiddenField ID="hdnSmartzCarID" runat="server" Value='<%# Eval("SmartzDealerCode")%>' />
                                                                <asp:HiddenField ID="hdnSmartzMovedDate" runat="server" Value='<%# Eval("SmartzMovedDate")%>' />
                                                                <asp:HiddenField ID="hdnPSAmount" runat="server" Value='<%# Eval("Amount") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="240px" />
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
                <!-- <div class="cls">
            </div> -->
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
                                                            <b>Dealer ID</b> &nbsp;
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
                                            <td align="center">
                                                <!-- Credit Card Start  -->
                                                <div id="divcard" runat="server" style="display: block; height: auto; min-height: auto;
                                                    max-height: auto; margin-bottom: 15px;">
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
                                                    <table border="0" cellpadding="4" cellspacing="4" style="width: 47%; margin: 0; float: left;">
                                                        <tr>
                                                            <td colspan="2">
                                                                <h5 style="font-size: 15px; margin: 0; float: left; width: 130px;">
                                                                    Invoice Details</h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <strong style="width: 77px">Attention To </strong>
                                                                <asp:Label ID="lblInvoiceAttentionTo" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <strong>Send the Invoice by</strong><br />
                                                                <span>
                                                                    <asp:RadioButton ID="rdbtnInvoiceEmail" CssClass="noLM" Style="width: 40px" Text=""
                                                                        GroupName="InvoiceSend" runat="server" Checked="true" Enabled="false" /><span class="featNon"
                                                                            style="width: 50px;">Email</span>
                                                                    <asp:Label ID="lblInvoiceEmail" runat="server"></asp:Label><br />
                                                                </span><span style="margin-top: 6px;">
                                                                    <asp:RadioButton ID="rdbtnInvoicePostal" Style="width: 40px" CssClass="noLM" Text=""
                                                                        GroupName="InvoiceSend" runat="server" Enabled="false" /><span class="featNon" style="display: inline">Postal</span>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 37%; margin: 0; float: left">
                                                        <tr>
                                                            <td colspan="2">
                                                                <h5 style="font-size: 15px; margin: 0; display: inline-block">
                                                                    Billing Address</h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 70px;">
                                                                <span><strong style="width: 40px">Name</strong>
                                                                    <asp:Label ID="lblInvoiceBillingName" runat="server"></asp:Label>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span><strong style="width: 45px">Address</strong>
                                                                    <asp:Label ID="lblInvoiceBillingAddress" runat="server"></asp:Label>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span><strong style="width: 40px">City</strong>
                                                                    <asp:Label ID="lblInvoiceBillingCity" runat="server"></asp:Label>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div style="width: 45%; display: inline-block; float: left; margin-right: 10px;">
                                                                    <span><strong style="width: 40px">State</strong>
                                                                        <asp:Label ID="lblInvoiceBillingState" runat="server"></asp:Label>
                                                                    </span>
                                                                </div>
                                                                <div style="width: 45%; display: inline-block; float: left">
                                                                    <span><strong style="width: 40px">ZIP</strong>
                                                                        <asp:Label ID="lblInvoiceBillingZip" runat="server"></asp:Label>
                                                                    </span>
                                                                </div>
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
                                                                <asp:ListItem Value="4">Open</asp:ListItem>
                                                                <asp:ListItem Value="3">Pending</asp:ListItem>
                                                                <asp:ListItem Value="1">FullyPaid</asp:ListItem>
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
                                            <td colspan="4" style="padding-top: 15px;">
                                                <div style="width: 240px; margin: 0 auto;">
                                                    <asp:Button ID="btnUpdate" OnClientClick="return ValidateUpdate();" runat="server"
                                                        Text="Update" CssClass="g-button g-button-submit" OnClick="btnUpdate_Click" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnCancelUpdate" CssClass="g-button g-button-submit" runat="server"
                                                        Text="Cancel" OnClientClick="return ClosePopup();" />
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
            <asp:Button ID="btnDealerExistsClose" class="cls" runat="server" Text="" BorderWidth="0"
                OnClick="btnDealerExistsOK_Click" />
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
    </form>
</body>
</html>
