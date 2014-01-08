<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form2" runat="server">
    <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="Progress" runat="server" AssociatedUpdatePanelID="updtpnlMain"
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
                <h1 class="h11">
                    Car Sales System</h1>
            </td>
            <td style="width: 250px;">
            </td>
        </table>
        <div class="clear">
            &nbsp;</div>
    </div>
    <div class="main">
        <asp:UpdatePanel ID="updtpnlMain" runat="server">
            <ContentTemplate>
                <div class="clear">
                    &nbsp;</div>
                <div>
                </div>
                <div style="padding: 15px 10px; width: 95%;">
                    <div class="errorPage">
                        <div class="errorPageData">
                            <h2>
                                Have a problem
                                <br />
                                <asp:Label ID="lblError" runat="server"></asp:Label>
                            </h2>
                            <div>
                                <a href="http://carsales.unitedcarexchange.com/Login.aspx">click here</a> to login
                            </div>
                            <div class="clear">
                                &nbsp;</div>
                        </div>
                        <div class="clear">
                            &nbsp;</div>
                    </div>
                </div>
                <div class="clear">
                    &nbsp;</div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="footer">
        United Car Exchange © 2012</div>
    </form>
</body>
</html>
