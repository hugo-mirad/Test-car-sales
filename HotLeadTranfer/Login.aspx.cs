﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CarsBL.Transactions;
using CarsBL.CentralDBTransactions;
using CarsInfo;
using HotLeadBL.HotLeadsTran;

public partial class Login : System.Web.UI.Page
{
    SmartzUserRegBL objSmartzBL = new SmartzUserRegBL();
    SmartzUserRegInfo objSmartzInfo = new SmartzUserRegInfo();
    UserLogInfo UserLogInfo = new UserLogInfo();
    UserLogBL objUserLog = new UserLogBL();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    HotLeadsBL objHotLeadsBL = new HotLeadsBL();

    public string strUserSessionID;

    public string strUserAuthCookieID;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session[Constants.USER_ID] = null;
            Session.Clear();
            Session.Abandon();
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtUserName.Focus();
            Session.Timeout = 180;
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsGetCenterInfo = new DataSet();
            String strHostName = Request.UserHostAddress.ToString();
            string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            dsGetCenterInfo = objHotLeadsBL.GetCenterData(txtCenterCode.Text);
            if (dsGetCenterInfo.Tables.Count > 0)
            {
                if (dsGetCenterInfo.Tables[0].Rows.Count > 0)
                {
                    if (dsGetCenterInfo.Tables[0].Rows[0]["AgentCenterStatus"].ToString() == "1")
                    {
                        DataSet dsUserDetails = new DataSet();
                        dsUserDetails = objHotLeadsBL.HotLeadsPerformLogin(txtUserName.Text, txtPassword.Text, txtCenterCode.Text, strIp);

                        if (dsUserDetails.Tables.Count > 0)
                        {
                            if (dsUserDetails.Tables[0].Rows.Count > 0)
                            {
                                Session[Constants.USER_ID] = dsUserDetails.Tables[0].Rows[0]["AgentUID"].ToString();
                                Session[Constants.USER_NAME] = dsUserDetails.Tables[0].Rows[0]["AgentLogUname"].ToString();
                                Session[Constants.NAME] = dsUserDetails.Tables[0].Rows[0]["AgentUFirstName"].ToString();
                                Session[Constants.CenterCode] = dsUserDetails.Tables[0].Rows[0]["AgentCenterCode"].ToString();
                                Session[Constants.CenterCodeID] = dsUserDetails.Tables[0].Rows[0]["AgentCenterID"].ToString();
                                Session[Constants.USER_TYPE_ID] = dsUserDetails.Tables[0].Rows[0]["AgentUtype_Id"].ToString();

                                CreateUserLog(1);
                                if (Convert.ToInt32(Session[Constants.USER_TYPE_ID].ToString()) == 7)
                                {
                                    Response.Redirect("LeadsUpload.aspx");
                                }
                                if (Convert.ToInt32(Session[Constants.USER_TYPE_ID].ToString()) == 4)
                                {
                                    Response.Redirect("QCReport.aspx");
                                }
                                if (Convert.ToInt32(Session[Constants.USER_TYPE_ID].ToString()) == 5)
                                {
                                    Response.Redirect("LiveTransfers.aspx");
                                }
                                if (Convert.ToInt32(Session[Constants.USER_TYPE_ID].ToString()) == 6)
                                {
                                    Response.Redirect("AllCentersReport.aspx");
                                }
                                DataSet dsModules = new DataSet();
                                dsModules = objHotLeadsBL.GetUsersModuleRites(Convert.ToInt32(dsUserDetails.Tables[0].Rows[0]["AgentUID"].ToString()));
                                Session[Constants.USER_TYPE_ID] = dsModules.Tables[0].Rows[0]["AgentUtype_Id"].ToString();

                                //  Session[Constants.USER_LocationID] = dsUserLocations;                    

                                Session[Constants.USER_Rights] = dsModules;
                                if (Convert.ToInt32(Session[Constants.USER_TYPE_ID].ToString()) == 1)
                                {
                                    Response.Redirect("NewSale.aspx");
                                }
                                else if (Convert.ToInt32(Session[Constants.USER_TYPE_ID].ToString()) == 2)
                                {
                                    Response.Redirect("UserManagement.aspx");
                                }
                                else if (Convert.ToInt32(Session[Constants.USER_TYPE_ID].ToString()) == 3)
                                {
                                    Response.Redirect("CentralReport.aspx");
                                }
                            }
                            else
                            {
                                Session[Constants.USER_NAME] = txtUserName.Text;
                                if (CreateUserLog(3))
                                {
                                    lblError.Text = "Invalid username or password or center code!";
                                    lblError.Visible = true;
                                    txtUserName.Text = "";
                                    txtPassword.Text = "";
                                    txtCenterCode.Text = "";
                                }
                            }
                        }
                        else
                        {
                            Session[Constants.USER_NAME] = txtUserName.Text;
                            if (CreateUserLog(3))
                            {
                                lblError.Text = "Invalid username or password or center code!";
                                lblError.Visible = true;
                                txtUserName.Text = "";
                                txtPassword.Text = "";
                                txtCenterCode.Text = "";
                            }
                        }
                    }
                    else
                    {
                        Session[Constants.USER_NAME] = txtUserName.Text;
                        if (CreateUserLog(3))
                        {
                            lblError.Text = "Center is not active!";
                            lblError.Visible = true;
                            txtUserName.Text = "";
                            txtPassword.Text = "";
                            txtCenterCode.Text = "";
                        }
                    }
                }
                else
                {
                    Session[Constants.USER_NAME] = txtUserName.Text;
                    if (CreateUserLog(3))
                    {
                        lblError.Text = "Invalid center code!";
                        lblError.Visible = true;
                        txtUserName.Text = "";
                        txtPassword.Text = "";
                        txtCenterCode.Text = "";
                    }
                }
            }
            else
            {
                Session[Constants.USER_NAME] = txtUserName.Text;
                if (CreateUserLog(3))
                {
                    lblError.Text = "Invalid center code!";
                    lblError.Visible = true;
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    txtCenterCode.Text = "";
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    private Boolean CreateUserLog(int LogoutType)
    {
        Boolean blnReturnValue = false;

        Boolean blnSuccess;
        Int64 lngReturn = -1;


        String strHostName = Request.UserHostAddress.ToString();

        string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();


        try
        {

            UserLogInfo.Login_Ip = strIp;

            UserLogInfo.Login_DateTime = DateTime.Now;

            Session["SessionId"] = HttpContext.Current.Session.SessionID;


            //Set current Login 
            UserLogInfo.User_Id = int.Parse(Convert.ToInt64(Session[Constants.USER_ID]).ToString());
            UserLogInfo.Log_Status_Id = LogoutType;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, txtUserName.Text.Trim().ToLower(), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToInt32(Constants.SESSIONEXPIRATIONTIME)), false, Session[Constants.USER_NAME].ToString().Trim(), FormsAuthentication.FormsCookiePath);

            // encrypt the ticket
            string sEncTicket = FormsAuthentication.Encrypt(ticket);

            // set the cookie
            HttpCookie httpAuthCookie;

            FormsAuthentication.SetAuthCookie(txtUserName.Text.Trim().ToLower(), false);

            httpAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, sEncTicket);

            Response.Cookies.Add(httpAuthCookie);

            strUserAuthCookieID = httpAuthCookie.Value;

            UserLogInfo.Session_Id = Session["SessionId"].ToString();
            UserLogInfo.CookieID = strUserAuthCookieID;
            UserLogInfo.Logout_time = Convert.ToDateTime("1/1/1990");
            //dsUserLog.Tables[0].Rows.Add(drUserLog);
            //blnSuccess = objUserLog.SaveUserLog(UserLogInfo, ref lngReturn, "");
            blnSuccess = objHotLeadsBL.SaveUserLog(UserLogInfo, ref lngReturn, "");
            if (blnSuccess == true && lngReturn > 0)
            {
                Session[Constants.USERLOG_ID] = lngReturn;
                blnReturnValue = true;
            }
        }
        catch (Exception ex)
        {
            // Response.Redirect("Error.aspx");


        }

        return blnReturnValue;
    }

}
