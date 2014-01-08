<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>..:: Car Sales System ::..</title>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/menu.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">        window.history.forward(1);</script>

    <script type="text/javascript" language="javascript" src="js/jquery-1.7.min.js"></script>

    <script type="text/javascript" language="javascript" src="js/sliding_effect.js"></script>

    <script type="text/javascript" language="javascript">
      
      function ValidateDetails()
        {
       
            var valid=true;
     
             if (document.getElementById("txtUserName").value.trim()=="")
            {
                alert('Please enter username')
                valid=false;
                document.getElementById("txtUserName").value= "";
                document.getElementById('txtUserName').focus();  
               if(document.getElementById('lblError') != null)
                {
                document.getElementById('lblError').outerText = "";    
                }
                             
                
            }
             
           else if(document.getElementById("txtPassword").value.trim()=="")
            {
                alert("Please enter password"); 
                valid=false;
                document.getElementById("txtPassword").value = "";
                document.getElementById('txtPassword').focus();  
              if(document.getElementById('lblError') != null)
                {
                document.getElementById('lblError').outerText = "";    
                } 
            }  
             else if(document.getElementById("txtCenterCode").value.trim()=="")
            {
                alert("Please enter center code"); 
                valid=false;
                document.getElementById("txtCenterCode").value = "";
                document.getElementById('txtCenterCode').focus();  
                if(document.getElementById('lblError') != null)
                {
                 document.getElementById('lblError').outerText = "";    
                } 
            }  
              
            return valid;
        }

    </script>

    <script type="text/javascript">
        if (/Chrome[\/\s](\d+\.\d+)/.test(navigator.userAgent)){ //test for Chrome/x.x 
         
        }
        else
         alert("This application has been optimized to be used with chrome browser. If you use any other browser your experience may vary");


    </script>

</head>
<body>
    <form id="form1" runat="server">
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
                <div class="loginbox">
                    <h2 class="loginHed">
                        Login</h2>
                    <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr id="methodOfContactEmail">
                            <td style="width: 120px; vertical-align: top; line-height: 30px">
                                User Name
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" CssClass="input1" Width="200px"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr id="methodOfContactConfirmEmail">
                            <td style="vertical-align: top; line-height: 30px">
                                Password
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="30" CssClass="input1" Width="200px"
                                            TextMode="Password"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr id="Tr1">
                            <td style="vertical-align: top; line-height: 30px">
                                Center Code
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCenterCode" runat="server" MaxLength="30" CssClass="input1" Width="200px"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="padding-top: 5px;">
                                <asp:Button ID="btnLogin" runat="server" CssClass="g-button g-button-submit" Text="Login"
                                    OnClick="btnLogin_Click" OnClientClick="return ValidateDetails();" />
                                <!--  <input type="button" class="g-button " value="Login" />  -->
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2" style="padding-top: 8px;">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                        &nbsp;</div>
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
