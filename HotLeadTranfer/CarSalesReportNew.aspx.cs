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

public partial class CarSalesReportNew : System.Web.UI.Page
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
                        FillCenters();
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
            lblDateRange.Text = StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            lblReportDate.Text = dtNow.ToString("MM/dd/yyyy");
            int TypeID;
            if (rdbtnSales.Checked == true)
            {
                lblReportType.Text = "Sales";
                TypeID = 1;
            }
            else
            {
                lblReportType.Text = "Verifications";
                TypeID = 2;
            }
            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            int AgentID;
            if (CenterID.ToString() == "0")
            {
                AgentID = 0;
            }
            else
            {
                AgentID = Convert.ToInt32(ddlAgents.SelectedItem.Value);
            }
            SalesData = objHotLeadBL.GetCarsalesReportNewWithSummary(StartingDate, EndingDate, CenterID, AgentID, TypeID);
            Session["SalesReportData"] = SalesData;
            if (SalesData.Tables.Count > 0)
            {
                if (SalesData.Tables[0].Rows.Count > 0)
                {
                    //lblResHead.Text = "Car sales report for the period " + StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");
                    lblAgentRes.Text = "Total records found " + SalesData.Tables[0].Rows.Count.ToString();
                    grdAgentsInfo.Visible = true;
                    grdAgentsInfo.DataSource = SalesData.Tables[0];
                    grdAgentsInfo.DataBind();
                    TotalDataRepeater.Visible = true;
                    TotalDataRepeater.DataSource = SalesData.Tables[0];
                    TotalDataRepeater.DataBind();
                    ImgbtnExcel.Enabled = true;
                }
                else
                {
                    //lblResHead.Text = "Car sales report for the period " + StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");
                    lblAgentRes.Text = "No records found";
                    grdAgentsInfo.Visible = false;
                    TotalDataRepeater.Visible = false;
                    ImgbtnExcel.Enabled = false;
                }
            }
            else
            {
                //lblResHead.Text = "Car sales report for the period " + StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");
                lblAgentRes.Text = "No records found";
                grdAgentsInfo.Visible = false;
                TotalDataRepeater.Visible = false;
                ImgbtnExcel. Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void TotalDataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            DataSet dsData = new DataSet();
            DataTable dtData = new DataTable();
            DataView dvData = new DataView();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnFullAgentName = (HiddenField)e.Item.FindControl("hdnFullAgentName");
                HiddenField hdnFullAgentCenter = (HiddenField)e.Item.FindControl("hdnFullAgentCenter");
                Label lblFullDateRange = (Label)e.Item.FindControl("lblFullDateRange");
                Label lblFullReportDate = (Label)e.Item.FindControl("lblFullReportDate");
                Label lblDateRange = (Label)this.FindControl("lblDateRange");
                Label lblReportDate = (Label)this.FindControl("lblReportDate");
                RadioButton rdbtnSales = (RadioButton)this.FindControl("rdbtnSales");
                RadioButton rdbtnVerifications = (RadioButton)this.FindControl("rdbtnVerifications");

                lblFullDateRange.Text = lblDateRange.Text;
                lblFullReportDate.Text = lblReportDate.Text;

                dsData = (DataSet)Session["SalesReportData"];
                dvData = dsData.Tables[1].DefaultView;
                if (rdbtnSales.Checked == true)
                {
                    dvData.RowFilter = "SaleAgent='" + hdnFullAgentName.Value + "' and AgentCenterCode='" + hdnFullAgentCenter.Value + "'";
                }
                else
                {
                    dvData.RowFilter = "Verifier='" + hdnFullAgentName.Value + "' and VerifierCenter='" + hdnFullAgentCenter.Value + "'";
                }
                dtData = dvData.ToTable();
                if (dtData.Rows.Count > 0)
                {
                    grdWarmLeadInfo.DataSource = dtData;
                    grdWarmLeadInfo.DataBind();
                }
                dvData.RowFilter = "";
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
    private void FillCenters()
    {
        try
        {
            DataSet dsCenters = objHotLeadBL.GetAllCentersData();
            ddlCenters.Items.Clear();
            for (int i = 1; i < dsCenters.Tables[0].Rows.Count; i++)
            {
                if (dsCenters.Tables[0].Rows[i]["AgentCenterID"].ToString() != "0")
                {
                    ListItem list = new ListItem();
                    list.Text = dsCenters.Tables[0].Rows[i]["AgentCenterCode"].ToString();
                    list.Value = dsCenters.Tables[0].Rows[i]["AgentCenterID"].ToString();
                    ddlCenters.Items.Add(list);
                }
            }
            ddlCenters.Items.Insert(0, new ListItem("All", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillAgents()
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(Convert.ToInt32(ddlCenters.SelectedItem.Value));
            ddlAgents.Items.Clear();
            ddlAgents.DataSource = dsverifier;
            ddlAgents.DataTextField = "AgentUFirstName";
            ddlAgents.DataValueField = "AgentUID";
            ddlAgents.DataBind();
            ddlAgents.Items.Insert(0, new ListItem("All", "0"));
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
            if (dsExcelSearch.Tables[1].Rows.Count > 0)
            {
                DataSetToExcel.Convert(dsExcelSearch, Response, SelectedTyepe);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCenters_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCenters.SelectedItem.Value != "0")
            {
                tblAgents.Style["display"] = "block";
                FillAgents();
            }
            else
            {
                tblAgents.Style["display"] = "none";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
