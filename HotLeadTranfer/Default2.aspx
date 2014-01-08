<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <table id="Table7" runat="server" align="center" cellpadding="0" cellspacing="0"
                    style="width: 99%; margin: 0 auto;" class="nopad gridView">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; margin: 0 auto;"
                                        class="noPad">
                                        <tr>
                                            <td>
                                                <table width="100%" style="margin-top: 10px;">
                                                     <table style="width: 100%;">
                                        <tr>
                                            <td>
                                              
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Make:</strong><br />
                                                   
                                                        <asp:TextBox id="ddlMake" runat="server" ></asp:TextBox>
                                             
                                            </td>
                                            </tr>
                                            <tr>
                                            <td style="width: 50px">
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Model:</strong>
                                                   <br />
                                                        <asp:TextBox ID="ddlModel" runat="server"></asp:TextBox>
                                                       
                                            </td>
                                            </tr>
                                            <tr>
                                            <td>
                                                
                                                    <span class="star" style="color: Red">*</span><strong style="width: 40px;">Year:</strong>
                                                    <asp:TextBox ID="ddlYear" runat="server"></asp:TextBox>

                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td style="padding: 0;">
                                                <table style="padding: 0; width: 99%;">
                                                    <tr>
                                                        <td>
                                                            
                                                                <span class="star" style="color: Red">*</span><strong style="width: 45px">Price $:</strong>
                                                                <%--  <input type="text" style="width: 212px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtAskingPrice" runat="server" MaxLength="6" class="sample4" Enabled="false"></asp:TextBox>
                                                          
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                           
                                                                <strong style="width: 40px">Mileage:</strong>
                                                                <%-- <input type="text" style="width: 119px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtMileage" runat="server" MaxLength="6" class="sample4" Enabled="false"></asp:TextBox>
                                                          
                                                        </td>
                                                           </tr>
                                                           <tr>
                                                        <td>
                                                        
                                                           
                                                                <strong style="width: 50px">Cylinders:</strong><span style="font-weight: bold">
                                                                    <asp:RadioButton ID="rdbtnCylinders1" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">3</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders2" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">4</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders3" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">5</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders4" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">6</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders5" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">7</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders6" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">8</span>
                                                                    <asp:RadioButton ID="rdbtnCylinders7" CssClass="noLM" Text="" GroupName="Cylinders"
                                                                        runat="server" Checked="true" Enabled="false" /><br /><span class="featNon">NA</span>
                                                                </span>
                                                            
                                                        </td>
                                                         </tr>
                                                       
                                                 
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               
                                                    <strong style="width: 65px">Body style:</strong>
                                                  
                                                    <asp:TextBox ID="txtBodyStyle" runat="server" Enabled="false"></asp:TextBox>
                                              
                                            </td>
                                            </tr>
                                            <tr>
                                            <td>
                                             
                                                    <strong style="width: 80px">Exterior color:</strong>
                                                   
                                                    <asp:TextBox ID="txtExteriorColor" runat="server" Enabled="false"></asp:TextBox>
                                             
                                            </td>
                                            </tr>
                                            <tr>
                                            <td> 
                                                    <strong style="width: 80px">Interior color:</strong>
                                                    
                                                    <asp:TextBox ID="txtInteriorColor" runat="server" Enabled="false"></asp:TextBox>
                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  style="padding: 0">
                                                <table style="width: 100px" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                          
                                                                <strong style="width: 45px">Transmission:</strong><span style="font-weight: bold">
                                                                    <asp:RadioButton ID="rdbtnTrans1" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Enabled="false" /><span class="featNon">Auto</span>
                                                                    <asp:RadioButton ID="rdbtnTrans2" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Enabled="false" /><span class="featNon">Manual</span>
                                                                    <asp:RadioButton ID="rdbtnTrans3" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Enabled="false" /><span class="featNon">Tiptronic</span>
                                                                    <asp:RadioButton ID="rdbtnTrans4" CssClass="noLM" Text="" GroupName="Transmission"
                                                                        runat="server" Checked="true" Enabled="false" /><span class="featNon">NA</span>
                                                                </span>
                                                           
                                                        </td></tr>
                                                        <tr>
                                                        <td>
                                                          
                                                                <strong style="width: 45px">Doors:</strong><span>
                                                                 
                                                                    <asp:RadioButton ID="rdbtnDoor2" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">2</span>
                                                                    <asp:RadioButton ID="rdbtnDoor3" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">3</span>
                                                                    <asp:RadioButton ID="rdbtnDoor4" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">4</span>
                                                                    <asp:RadioButton ID="rdbtnDoor5" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Enabled="false" /><span class="featNon">5</span>
                                                                    <asp:RadioButton ID="rdbtnDoor6" CssClass="noLM" Text="" GroupName="Doors" runat="server"
                                                                        Checked="true" Enabled="false" /><span class="featNon">NA</span> </span>
                                                          
                                                        </td></tr>
                                                    
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding: 0;">
                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                         
                                                                <strong style="width: 65px">Drive train:</strong><br /><span style="font-weight: bold">
                                                                    <asp:RadioButton ID="rdbtnDriveTrain1" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">2WD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain2" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">FWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain3" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">AWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain4" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Enabled="false" /><br /><span class="featNon">RWD</span>
                                                                    <asp:RadioButton ID="rdbtnDriveTrain5" CssClass="noLM" Text="" GroupName="DriveTrain"
                                                                        runat="server" Checked="true" Enabled="false" /><br /><span class="featNon">NA</span>
                                                                </span>
                                                           
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                         
                                                                <strong>VIN #:</strong>
                                                                <%--<input type="text" style="width: 409px" class="sample4" />--%>
                                                                <asp:TextBox ID="txtVin" runat="server" Style="width: 409px" MaxLength="20" Enabled="false"></asp:TextBox>
                                                          
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              
                                                    <strong style="width: 55px">Fuel type:</strong><br /><span style="font-weight: bold">
                                                        <asp:RadioButton ID="rdbtnFuelType1" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><br /><span class="featNon">Diesel</span>
                                                        <asp:RadioButton ID="rdbtnFuelType2" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><br /><span class="featNon">Petrol</span>
                                                        <asp:RadioButton ID="rdbtnFuelType3" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><br /><span class="featNon">Hybrid</span>
                                                        <asp:RadioButton ID="rdbtnFuelType4" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><br /><span class="featNon">Electric</span>
                                                        <asp:RadioButton ID="rdbtnFuelType5" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><br /><span class="featNon">Gasoline</span>
                                                        <asp:RadioButton ID="rdbtnFuelType6" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><br /><span class="featNon">E-85</span>
                                                        <asp:RadioButton ID="rdbtnFuelType7" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Enabled="false" /><br /><span class="featNon">Gasoline-Hybrid</span>
                                                        <asp:RadioButton ID="rdbtnFuelType8" CssClass="noLM" Text="" GroupName="Fuels" runat="server"
                                                            Checked="true" Enabled="false" /><br /><span class="featNon">NA</span> </span>
                                              
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              
                                                    <strong style="width: 45px">Condition:</strong><br /><span style="font-weight: bold">
                                                     
                                                        <asp:RadioButton ID="rdbtnCondition1" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><br /><span class="featNon">Excellent</span>
                                                        <asp:RadioButton ID="rdbtnCondition2" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><br /><span class="featNon">Very good</span>
                                                        <asp:RadioButton ID="rdbtnCondition3" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><br /><span class="featNon">Good</span>
                                                        <asp:RadioButton ID="rdbtnCondition4" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><br /><span class="featNon">Fair</span>
                                                        <asp:RadioButton ID="rdbtnCondition5" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><br /><span class="featNon">Poor</span>
                                                        <asp:RadioButton ID="rdbtnCondition6" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Enabled="false" /><br /><span class="featNon">Parts or salvage</span>
                                                        <asp:RadioButton ID="rdbtnCondition7" CssClass="noLM" Text="" GroupName="Condition"
                                                            runat="server" Checked="true" Enabled="false" /><br /><span class="featNon">NA</span>
                                                    </span>
                                              
                                            </td>
                                        </tr>
                                    </table>
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
                           
                        </td>
                    </tr>
                </table>
    </div>
    </form>
</body>
</html>
