<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ticker.aspx.cs" Inherits="Ticker" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="css/css2.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/WebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 380px; border: #777 2px solid; padding: 3px; box-shadow: 0 0 12px rgba(0,0,0,0.5)">
        <asp:UpdatePanel ID="updtpnlTotal" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Timer ID="IntervalTimer" OnTick="IntervalTimer_Tick" runat="server" Interval="180000">
                    </asp:Timer>
                </div>
                <table width="380px;" class="grid1">
                    <tr>
                        <td colspan="3" style="background: #666; border: #666 1px solid; line-height: 22px;
                            vertical-align: middle; color:#fff">
                            Date:<asp:Label ID="lblDatetime" runat="server"></asp:Label>
                            <span class="close" id="close" onclick="javascript: top.window.close();"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center; border: #666 1px solid; line-height: 22px;
                            font-weight: bold; text-transform: uppercase; background: #ccc;>
                            <asp:Label ID="lblCenterCode" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td width="110px" style="font-size: 14px; font-weight: bold">
                          
                        </td>
                        <td style="font-size: 14px; font-weight: bold" width="200px">
                           Agent
                        </td>
                         <td style="font-size: 14px; font-weight: bold" width="200px">
                           Verifier
                        </td>
                    </tr>
                    <tr>
                        <td  style="font-size: 14px; width:110px; font-weight: bold">
                            Sales:
                        </td>
                        <td style="font-size: 14px; font-weight: bold" width="200px">
                            <asp:Label ID="lblTotalsales" runat="server"></asp:Label>
                        </td>
                         <td style="font-size: 14px; font-weight: bold" width="200px">
                            <asp:Label ID="lblVTotalsales" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="border: none; padding: 0" colspan="3">
                            <table class="grid1">
                                <asp:Repeater ID="LiveSalesRepeater" runat="server" OnItemDataBound="LiveSalesRepeater_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width :84px;">
                                                <asp:Label ID="lblAgnetName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"SaleAgent"),11)%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAgnetSales" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnAgentSalesCount" runat="server" Value='<%# Eval("ACount") %>' />
                                                <asp:HiddenField ID="hdnAgentAmount" runat="server" Value='<%# Eval("ATotalAmount", "{0:0.00}") %>' />
                                                <asp:HiddenField ID="hdnAgentDsicount" runat="server" Value='<%# Eval("ADiscountAmount", "{0:0.00}") %>' />
                                              </td>
                                               <td >
                                                 <asp:Label ID="lblVSales1" runat="server"></asp:Label>
                                                  <asp:HiddenField ID="hdnVSalesCount" runat="server" Value='<%# Eval("VCount") %>' />
                                                <asp:HiddenField ID="hdnVAmount" runat="server" Value='<%# Eval("VTotalAmount", "{0:0.00}") %>' />
                                                  <asp:HiddenField ID="hdnVDiscount" runat="server" Value='<%# Eval("VDiscountAmount", "{0:0.00}") %>' />
                                                 
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr class="Repeat">
                        <td colspan="3" style="border: none; padding: 0;">
                            <table class="grid1">
                                <asp:Repeater ID="AllCenters" runat="server" OnItemDataBound="AllCenters_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width :84px; ">
                                                <asp:Label ID="lblCenterName" runat="server" Text='<%#objGeneralFunc.WrapTextByMaxCharacters(DataBinder.Eval(Container.DataItem,"Center"),11)%>'></asp:Label>
                                            </td>
                                            <td >
                                                <asp:Label ID="lblCenterSales" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnCenterSalesCount" runat="server" Value='<%# Eval("Count") %>' />
                                                <asp:HiddenField ID="hdnCenterAmount" runat="server" Value='<%# Eval("TotalAmount", "{0:0.00}") %>' />
                                                 <asp:HiddenField ID="hdnCenterDiscount" runat="server" Value='<%# Eval("Discount", "{0:0.00}") %>' />
                                                
                                               </td>
                                               <td>
                                                <asp:Label ID="lblCenterVSales" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnCenterVSalesCount" runat="server" Value='<%# Eval("VCount") %>' />
                                                <asp:HiddenField ID="hdnCenterVAmount" runat="server" Value='<%# Eval("VTotalAmount", "{0:0.00}") %>' />
                                                 <asp:HiddenField ID="hdnCenterVDiscount" runat="server" Value='<%# Eval("VDiscount", "{0:0.00}") %>' />
                                                
                                              </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="250px;">
                    <tr>
                        <td width="120px">
                            <asp:Button ID="btnSummaryRefresh" runat="server" CssClass="g-button g-button-submit"
                                Text="Refresh" OnClick="btnRefresh_Click" />
                            <asp:Button ID="btnAgentRefresh" runat="server" CssClass="g-button g-button-submit"
                                Text="Refresh" OnClick="btnAgentRefresh_Click" Visible="false" />
                        </td>
                        <td>
                            <asp:Button ID="btnDetails" runat="server" CssClass="g-button g-button-submit" Text="Details"
                                OnClick="btnAgentRefresh_Click" />
                            <asp:Button ID="btnSummary" runat="server" CssClass="g-button g-button-submit" Text="Summary"
                                Visible="false" OnClick="btnRefresh_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
