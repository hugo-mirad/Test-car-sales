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

public partial class CarSalesReport : System.Web.UI.Page
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
                            //if (CenterCode.Length > 5)
                            //{
                            //    lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString().Substring(0, 5) + ")";
                            //}
                            //else
                            //{
                            //    lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString() + ")";
                            //}
                            lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString() + ")-" + UserLogName.ToString();
                        }
                        lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");
                        DataSet dsDatetime = objHotLeadBL.GetDatetime();
                        DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
                        txtStartDate.Text = dtNow.AddDays(-6).ToString("MM/dd/yyyy");
                        txtEndDate.Text = dtNow.ToString("MM/dd/yyyy");
                        DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
                        DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
                        GetResults(StartDate, EndDate);
                    }
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
    private void GetResults(DateTime StartDate, DateTime EndDate)
    {
        try
        {
            // DateTime StartingDate = Convert.ToDateTime(StartDate.AddDays(-1).ToString("MM/dd/yyyy"));
            //DateTime EndingDate = Convert.ToDateTime(EndDate.AddDays(1).ToString("MM/dd/yyyy"));
            DateTime StartingDate = StartDate;
            DateTime EndingDate = EndDate;
            DataSet SalesData = new DataSet();
            SalesData = objHotLeadBL.GetCarSalesReport(StartingDate, EndingDate);
            Session["SalesReportData"] = SalesData;
            if (SalesData.Tables.Count > 0)
            {
                if (SalesData.Tables[0].Rows.Count > 0)
                {
                    lblResHead.Text = "Car sales report for the period " + StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");
                    lblRes.Text = "Total records found " + SalesData.Tables[0].Rows.Count.ToString();
                    grdWarmLeadInfo.Visible = true;
                    grdWarmLeadInfo.DataSource = SalesData.Tables[0];
                    grdWarmLeadInfo.DataBind();
                }
                else
                {
                    lblResHead.Text = "Car sales report for the period " + StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");
                    lblRes.Text = "No records found";
                    grdWarmLeadInfo.Visible = false;
                }
            }
            else
            {
                lblResHead.Text = "Car sales report for the period " + StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");
                lblRes.Text = "No records found";
                grdWarmLeadInfo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdWarmLeadInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAgent = (Label)e.Row.FindControl("lblAgent");
                HiddenField hdnAgentName = (HiddenField)e.Row.FindControl("hdnAgentName");
                HiddenField hdnVerifierID = (HiddenField)e.Row.FindControl("hdnVerifierID");
                Label lblVerifier = (Label)e.Row.FindControl("lblVerifier");
                HiddenField hdnVerifierName = (HiddenField)e.Row.FindControl("hdnVerifierName");
                HiddenField hdnAgentCenterCode = (HiddenField)e.Row.FindControl("hdnAgentCenterCode");
                HiddenField hdnVerifierCenterCode = (HiddenField)e.Row.FindControl("hdnVerifierCenterCode");

                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnPackName");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblPackage");
                Label lblQcStatus = (Label)e.Row.FindControl("lblQcStatus");
                HiddenField hdnQcStatus = (HiddenField)e.Row.FindControl("hdnQcStatus");
                Label lblPaid = (Label)e.Row.FindControl("lblPaid");
                HiddenField hdnAmount1 = (HiddenField)e.Row.FindControl("hdnAmount1");


                lblPaid.Text = string.Format("{0:0.00}", Convert.ToDouble(hdnAmount1.Value));
                if (hdnQcStatus.Value == "")
                {
                    lblQcStatus.Text = "QC Open";
                }
                else
                {
                    lblQcStatus.Text = hdnQcStatus.Value;
                }
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(hdnPackCost.Value.ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = hdnPackName.Value.ToString();
                lblPackage.Text = PackName + "($" + PackAmount + ")";

                lblAgent.Text = hdnAgentCenterCode.Value + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnAgentName.Value.ToString(), 15);
                lblVerifier.Text = hdnVerifierCenterCode.Value + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnVerifierName.Value.ToString(), 15);
            }
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
            GetResults(StartDate, EndDate);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ImgbtnExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataSet dsExcelSearch = new DataSet();
            dsExcelSearch = Session["SalesReportData"] as DataSet;
            string SelectedTyepe = "CarSalesReport";
            if (dsExcelSearch.Tables[0].Rows.Count > 0)
            {
                DataSetToExcel.Convert(dsExcelSearch, Response, SelectedTyepe);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
