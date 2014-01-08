using System;
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
using HotLeadBL;
using HotLeadInfo;
using CarsBL.Transactions;
using CarsBL.CentralDBTransactions;
using CarsInfo;
using CarsBL.Masters;
using System.Collections.Generic;
using HotLeadBL.HotLeadsTran;

public partial class IPAddress : System.Web.UI.Page
{
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet CarsDetails = new DataSet();
    DataSet dsDropDown = new DataSet();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    UserRegistrationInfo objUserregInfo = new UserRegistrationInfo();
    HotLeadsBL objHotLeadBL = new HotLeadsBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.NAME] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                Session["CurrentPage"] = "Central report";

                if (LoadIndividualUserRights() == false)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    if (Session[Constants.NAME] == null)
                    {
                        lnkBtnLogout.Visible = false;
                        lblUserName.Visible = false;
                    }
                    else
                    {
                        LoadUserRights();
                        lnkBtnLogout.Visible = true;
                        lblUserName.Visible = true;
                        string LogUsername = Session[Constants.NAME].ToString();
                        string CenterCode = Session[Constants.CenterCode].ToString();
                        string UserLogName = Session[Constants.USER_NAME].ToString();
                        if (LogUsername.Length > 20)
                        {
                            lblUserName.Text = LogUsername.ToString().Substring(0, 20);
                            //if (CenterCode.Length > 5)
                            //{
                            //    lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString().Substring(0, 5) + ")";
                            //}
                            //else
                            //{
                            lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString() + ")-" + UserLogName.ToString();
                            //}
                        }
                        else
                        {
                            lblUserName.Text = LogUsername;
                            lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString() + ")-" + UserLogName.ToString();
                        }
                        lblCenterCode.Text = "Center code is " + CenterCode.ToString();
                        // lblAgentAddCenterCode.Text = "Center code is " + CenterCode.ToString();

                    }
                    lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");
                    FillIPAddress();
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "pageLoad();", true);

                }
            }
        }
    }

    private void FillIPAddress()
    {
        try
        {
            DataSet dsIpAddress = objHotLeadBL.GetAllIPAddressValues();
            if (dsIpAddress.Tables.Count > 0)
            {
                if (dsIpAddress.Tables[0].Rows.Count > 0)
                {
                    lblResHead.Text = "";
                    grdUserDetails.Visible = true;
                    grdUserDetails.DataSource = dsIpAddress.Tables[0];
                    grdUserDetails.DataBind();
                }
                else
                {
                    lblResHead.Text = "Results not found";
                    grdUserDetails.Visible = false;
                }
            }
            else
            {
                lblResHead.Text = "Results not found";
                grdUserDetails.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool LoadIndividualUserRights()
    {
        DataSet dsIndidivitualRights = new DataSet();
        bool bValid = false;
        //dsIndidivitualRights = objHotLeadBL.GetUserModules_ActiveInactive(Convert.ToInt32(Session[Constants.USER_ID]));
        if (Session["IndividualUserRights"] == null)
        {
            dsIndidivitualRights = objHotLeadBL.GetUserModules_ActiveInactive(Convert.ToInt32(Session[Constants.USER_ID]));
            Session["IndividualUserRights"] = dsIndidivitualRights;
        }
        else
        {
            dsIndidivitualRights = Session["IndividualUserRights"] as DataSet;
        }
        if (dsIndidivitualRights.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsIndidivitualRights.Tables[0].Rows.Count; i++)
            {

                if (dsIndidivitualRights.Tables[0].Rows[i]["ModuleName"].ToString() == Session["CurrentPage"].ToString())
                {
                    if (dsIndidivitualRights.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        bValid = true;
                        break;
                    }

                }


            }
        }
        return bValid;
    }
    private void LoadUserRights()
    {
        DataSet dsSession = new DataSet();
        dsSession = objHotLeadBL.GetUserSession(Convert.ToInt32(Session[Constants.USER_ID]));

        if (dsSession.Tables[0].Rows[0]["SessionID"].ToString() != HttpContext.Current.Session.SessionID.ToString())
        {
            // objUserlog.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 8);

            Session["SessionTimeOut"] = 1;
            Response.Redirect("Login.aspx");

        }

    }

    protected void lnkAddUSer_Click(object sender, EventArgs e)
    {
        try
        {
            txtIpAddress.Text = "";
            mpeAddNew.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            HotLeadsBL objHotLeadsBL = new HotLeadsBL();
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            objHotLeadsBL.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), dtNow, Convert.ToInt32(Session[Constants.USERLOG_ID]), 2);
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnAddCenter_Click(object sender, EventArgs e)
    {
        try
        {
            string IPAddress = txtIpAddress.Text.Trim();
            DataSet dsIPAddressExits = objHotLeadBL.CheckIPAddressExist(IPAddress);
            if (dsIPAddressExits.Tables.Count > 0)
            {
                if (dsIPAddressExits.Tables[0].Rows.Count > 0)
                {
                    Session["CkeckPopupCenter"] = 1;
                    mpelblUerExist.Show();
                    lblUerExist.Text = "IP address already exists.";
                    lblUerExist.Visible = true;
                }
                else
                {
                    DataSet newIPAddress = objHotLeadBL.InsertIPAddressValues(IPAddress);
                    mpelblUerExist.Show();
                    lblUerExist.Text = "IP address added successfully";
                    lblUerExist.Visible = true;
                    Session["CkeckPopupCenter"] = 2;
                }
            }
            else
            {
                DataSet newIPAddress = objHotLeadBL.InsertIPAddressValues(IPAddress);
                mpelblUerExist.Show();
                lblUerExist.Text = "IP address added successfully";
                lblUerExist.Visible = true;
                Session["CkeckPopupCenter"] = 2;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnlblUerExist_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(Session["CkeckPopupCenter"]) == 2)
            {
                mpelblUerExist.Hide();
                FillIPAddress();
            }
            else if (Convert.ToInt32(Session["CkeckPopupCenter"]) == 1)
            {
                mpeAddNew.Show();
                mpelblUerExist.Hide();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }






    protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "view")
            {
                string IPAddress = e.CommandArgument.ToString();
                DataSet dsDelete = objHotLeadBL.DeleteIPAddressExist(IPAddress);
                mpelblUerExist.Show();
                lblUerExist.Text = "IP address deleted successfully";
                lblUerExist.Visible = true;
                Session["CkeckPopupCenter"] = 2;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }





}
