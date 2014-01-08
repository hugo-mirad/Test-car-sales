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


public partial class CenterAgentsReport : System.Web.UI.Page
{
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet CarsDetails = new DataSet();
    DataSet dsDropDown = new DataSet();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    UserRegistrationInfo objUserregInfo = new UserRegistrationInfo();
    HotLeadsBL objHotLeadBL = new HotLeadsBL();
    public DataSet dsTotdata = new DataSet();
    int TotalBasic;
    int TotalStandard;
    int TotalEnhanced;
    int TotalSilver;
    int TotalGold;
    int TotalPlatinum;
    int TotalTotal;
    Double TotalMoney;
    Double ActiveMoney;
    CentralDBMainBL objCentarlDBBL = new CentralDBMainBL();

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
                Session["CurrentPage"] = "Center report";

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
                    }
                    DataSet dsDatetime = objHotLeadBL.GetDatetime();
                    DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
                    txtStartDate.Text = dtNow.AddDays(-6).ToString("MM/dd/yyyy");
                    txtEndDate.Text = dtNow.ToString("MM/dd/yyyy");
                    DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
                    DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
                    int Active = 0;
                    GetResults(StartDate, EndDate, Active);
                }
            }
        }
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

        DataSet dsModules = new DataSet();
        dsModules = (DataSet)Session[Constants.USER_Rights];

        Session[Constants.USER_Rights] = dsModules;
        if (dsModules.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dsModules.Tables[0].Rows.Count; i++)
            {
                //lnkHome,userAdmin,lbtnManager,lnkReports

                //

                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "New sale")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnNewSale.Enabled = true;
                    }

                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "User management")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnAdmin.Enabled = true;

                    }

                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Intromail")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnIntromail.Enabled = true;

                    }
                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Agent report")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnAgentReport.Enabled = true;
                    }

                }

            }
        }


    }
    private bool LoadIndividualUserRights()
    {
        DataSet dsIndidivitualRights = new DataSet();
        bool bValid = false;

        // dsIndidivitualRights = objHotLeadBL.GetUserModules_ActiveInactive(Convert.ToInt32(Session[Constants.USER_ID]));
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
    private void GetResults(DateTime StartDate, DateTime EndDate, int Active)
    {
        try
        {

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    protected void btnSearchMonth_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            int Active;
            if (rdbtnActive.Checked == true)
            {
                Active = 1;
            }
            else if (rdbtnActivePeriod.Checked == true)
            {
                Active = 2;
            }
            else
            {
                Active = 0;
            }
            GetResults(StartDate, EndDate, Active);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void rdbtnActive_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            int Active = 1;
            GetResults(StartDate, EndDate, Active);

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void rdbtnAll_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            int Active = 0;
            GetResults(StartDate, EndDate, Active);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void rdbtnActivePeriod_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            int Active = 2;
            GetResults(StartDate, EndDate, Active);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void grdAgentData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataSet dsData = new DataSet();
            dsData = (DataSet)Session["NewAgentRepData"];
            if (e.CommandName == "ShowSales")
            {

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdIntroInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSaleDtInd = (Label)e.Row.FindControl("lblSaleDtInd");
                HiddenField hdnSaledate = (HiddenField)e.Row.FindControl("hdnSaledate");
                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnPackName");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnPhoneNum");
                HiddenField hdnStatus = (HiddenField)e.Row.FindControl("hdnStatus");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Image ImgStatus = (Image)e.Row.FindControl("ImgStatus");
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(hdnPackCost.Value.ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = hdnPackName.Value.ToString();
                lblPackage.Text = PackName + "($" + PackAmount + ")";

                if (hdnSaledate.Value.ToString() != "")
                {
                    DateTime dtSaleDate = Convert.ToDateTime(hdnSaledate.Value.ToString());
                    lblSaleDtInd.Text = dtSaleDate.ToString("MM/dd/yy");
                }
                else
                {
                    lblSaleDtInd.Text = "";
                }

                if (hdnPhoneNum.Value.ToString() == "")
                {
                    lblPhone.Text = "";
                }
                else
                {
                    lblPhone.Text = objGeneralFunc.filPhnm(hdnPhoneNum.Value);
                }
                if (hdnStatus.Value.ToString() == "1")
                {

                    ImgStatus.ImageUrl = "~/images/check.gif";
                }
                else if (hdnStatus.Value.ToString() == "2")
                {

                    ImgStatus.ImageUrl = "~/images/lock.gif";
                }
                else if (hdnStatus.Value.ToString() == "3")
                {

                    ImgStatus.ImageUrl = "~/images/icon13.png";
                }
                else if (hdnStatus.Value.ToString() == "4")
                {
                    ImgStatus.ImageUrl = "~/images/icon14.gif";
                }
                else if (hdnStatus.Value.ToString() == "5")
                {
                    ImgStatus.ImageUrl = "~/images/red_x.png";
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdAgentData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnSalesAmount = (HiddenField)e.Row.FindControl("hdnSalesAmount");
                Label lblSalesAmount = (Label)e.Row.FindControl("lblSalesAmount");
                HiddenField hdnPaidAmount = (HiddenField)e.Row.FindControl("hdnPaidAmount");
                Label lblPaidAmount = (Label)e.Row.FindControl("lblPaidAmount");
                HiddenField hdnPrevSalePaidAmount = (HiddenField)e.Row.FindControl("hdnPrevSalePaidAmount");
                Label lblPrevSalePaidAmount = (Label)e.Row.FindControl("lblPrevSalePaidAmount");
                HiddenField hdnTotalPaidAmount = (HiddenField)e.Row.FindControl("hdnTotalPaidAmount");
                Label lblTotalPaidAmount = (Label)e.Row.FindControl("lblTotalPaidAmount");
                if (hdnSalesAmount.Value.ToString() == "0")
                {
                    lblSalesAmount.Text = "0";
                }
                else
                {
                    Double SalesAmount = Convert.ToDouble(hdnSalesAmount.Value.ToString());
                    lblSalesAmount.Text = string.Format("{0:0.00}", SalesAmount);
                }
                if (hdnPaidAmount.Value.ToString() == "0")
                {
                    lblPaidAmount.Text = "0";
                }
                else
                {
                    Double PaidAmount = Convert.ToDouble(hdnPaidAmount.Value.ToString());
                    lblPaidAmount.Text = string.Format("{0:0.00}", PaidAmount);
                }
                if (hdnPrevSalePaidAmount.Value.ToString() == "0")
                {
                    lblPrevSalePaidAmount.Text = "0";
                }
                else
                {
                    Double PrevSalesPaidAmount = Convert.ToDouble(hdnPrevSalePaidAmount.Value.ToString());
                    lblPrevSalePaidAmount.Text = string.Format("{0:0.00}", PrevSalesPaidAmount);
                }
                if (hdnTotalPaidAmount.Value.ToString() == "0")
                {
                    lblTotalPaidAmount.Text = "0";
                }
                else
                {
                    Double TotalPaidAmount = Convert.ToDouble(hdnTotalPaidAmount.Value.ToString());
                    lblTotalPaidAmount.Text = string.Format("{0:0.00}", TotalPaidAmount);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BtnClsSendRegMail_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime StartDate = Convert.ToDateTime(Session["AgentReportStartDate"].ToString());
            DateTime EndDate = Convert.ToDateTime(Session["AgentReportEndDate"].ToString());
            int Active = Convert.ToInt32(Session["AgentReportActive"].ToString());
            GetResults(StartDate, EndDate, Active);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private string CreateTableHeader()
    {
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"120px\" id=\"SalesStatus\" style=\"display: block; background-color:#F3D9F6;border:2px;border-color:Black;height:110px \">";

        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"padding-left:10px;\" >";
        strTransaction += "Active:";
        strTransaction += "</td>";
        strTransaction += "<td>";
        strTransaction += "<img src=\"images/check.gif\" />";
        strTransaction += "</td>";
        strTransaction += "</tr>";
        strTransaction += " <tr id=\"CampaignsBody1\">";
        strTransaction += " <td style=\"padding-left:10px;\">";
        strTransaction += "Preliminary:";
        strTransaction += "</td>";
        strTransaction += " <td>";
        strTransaction += " <img src=\"images/lock.gif\" />";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += " <tr id=\"CampaignsBody1\">";
        strTransaction += " <td style=\"padding-left:10px;\">";
        strTransaction += "Withdraw:";
        strTransaction += "</td>";
        strTransaction += " <td>";
        strTransaction += " <img src=\"images/icon13.png\" />";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += " <tr id=\"CampaignsBody1\">";
        strTransaction += " <td style=\"padding-left:10px;\">";
        strTransaction += "Sold:";
        strTransaction += "</td>";
        strTransaction += " <td>";
        strTransaction += " <img src=\"images/icon14.gif\" />";
        strTransaction += "</td>";
        strTransaction += " </tr>";

        strTransaction += " <tr id=\"CampaignsBody1\">";
        strTransaction += " <td style=\"padding-left:10px;\">";
        strTransaction += "Admin Cancel:";
        strTransaction += "</td>";
        strTransaction += " <td>";
        strTransaction += " <img src=\"images/red_x.png\" />";
        strTransaction += "</td>";
        strTransaction += " </tr>";


        strTransaction += "<tr id=\"CampaignsTitle11\">";
        strTransaction += "<td colspan=\"2\">";
        strTransaction += "<br />";
        strTransaction += "<br />";
        strTransaction += "</td>";
        strTransaction += "</tr>";
        strTransaction += "</table>";

        return strTransaction;

    }


}
