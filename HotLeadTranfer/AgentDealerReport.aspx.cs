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

public partial class AgentDealerReport : System.Web.UI.Page
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
                Session["CurrentPage"] = "Agent report";

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
                        lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");
                        int SaleAgentID = Convert.ToInt32(Session[Constants.USER_ID]);
                        DataSet SingleAgentSales = new DataSet();
                        DataSet SinglAgentProspects = new DataSet();
                        DataSet SingleAgentPromotionals = new DataSet();
                        SingleAgentSales = objHotLeadBL.GetEachAgentSalesDataForDealers(SaleAgentID);
                        Session["SingleAgentSalesForDealer"] = SingleAgentSales;
                        SinglAgentProspects = objHotLeadBL.GetEachAgentProspectsDataForDealers(SaleAgentID);
                        Session["SingleAgentProspectsForDealer"] = SinglAgentProspects;
                        SingleAgentPromotionals = objHotLeadBL.GetEachAgentPromotionalsDataForDealers(SaleAgentID);
                        Session["SingleAgentPromotionalsForDealer"] = SingleAgentPromotionals;
                        tblSales.Style["display"] = "block";
                        tblProspects.Style["display"] = "none";
                        tblPromotionals.Style["display"] = "none";
                        lblResHead.Text = "Last one month records are showing";

                        if (SingleAgentSales.Tables[0].Rows.Count > 0)
                        {
                            lblTotSales.Text = SingleAgentSales.Tables[0].Rows.Count.ToString();
                        }
                        else
                        {
                            lblTotSales.Text = "0";
                        }
                        if (SinglAgentProspects.Tables[0].Rows.Count > 0)
                        {
                            lblTotProspects.Text = SinglAgentProspects.Tables[0].Rows.Count.ToString();
                        }
                        else
                        {
                            lblTotProspects.Text = "0";
                        }
                        if (SingleAgentPromotionals.Tables[0].Rows.Count > 0)
                        {
                            lblTotPromotionals.Text = SingleAgentPromotionals.Tables[0].Rows.Count.ToString();
                        }
                        else
                        {
                            lblTotPromotionals.Text = "0";
                        }

                        if (SingleAgentSales.Tables[0].Rows.Count > 0)
                        {
                            grdDealerSaleInfo.Visible = true;
                            lblResCount.Visible = true;
                            lblRes.Visible = false;
                            lblResCount.Text = "Total " + SingleAgentSales.Tables[0].Rows.Count.ToString() + " records found";
                            grdDealerSaleInfo.DataSource = SingleAgentSales.Tables[0];
                            grdDealerSaleInfo.DataBind();
                        }
                        else
                        {
                            grdDealerSaleInfo.Visible = false;
                            lblResCount.Visible = false;
                            lblRes.Visible = true;
                            lblRes.Text = "No records exist";
                        }
                    }
                }
            }
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

        DataSet dsModules = new DataSet();
        dsModules = objHotLeadBL.GetUsersModuleRites(Convert.ToInt32(Session[Constants.USER_ID]));

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
                        lnkbtnDealerSale.Enabled = true;
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
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Center report")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnCentralReport.Enabled = true;
                    }

                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Transfers")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnTransfers.Enabled = true;
                        //lnkbtnNewSale.Visible = false;
                        //lnkbtnAdmin.Visible = false;
                        //lnkbtnIntromail.Visible = false;
                        //lnkbtnCentralReport.Visible = false;
                    }

                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Agent report")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnReports.Enabled = true;
                    }
                }

            }
        }


    }
    protected void rdbtnSales_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int SaleAgentID = Convert.ToInt32(Session[Constants.USER_ID]);
            DataSet SingleAgentSales = new DataSet();
            SingleAgentSales = Session["SingleAgentSalesForDealer"] as DataSet;
            tblSales.Style["display"] = "block";
            tblProspects.Style["display"] = "none";
            tblPromotionals.Style["display"] = "none";
            if (SingleAgentSales.Tables[0].Rows.Count > 0)
            {
                grdDealerSaleInfo.Visible = true;
                lblResCount.Visible = true;
                lblRes.Visible = false;
                lblResCount.Text = "Total " + SingleAgentSales.Tables[0].Rows.Count.ToString() + " records found";
                grdDealerSaleInfo.DataSource = SingleAgentSales.Tables[0];
                grdDealerSaleInfo.DataBind();
            }
            else
            {
                grdDealerSaleInfo.Visible = false;
                lblResCount.Visible = false;
                lblRes.Visible = true;
                lblRes.Text = "No records exist";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnProspects_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int SaleAgentID = Convert.ToInt32(Session[Constants.USER_ID]);
            DataSet SinglAgentProspects = new DataSet();
            SinglAgentProspects = Session["SingleAgentProspectsForDealer"] as DataSet;
            tblProspects.Style["display"] = "block";
            tblSales.Style["display"] = "none";
            tblPromotionals.Style["display"] = "none";
            if (SinglAgentProspects.Tables[0].Rows.Count > 0)
            {
                grdDealerProsInfo.Visible = true;
                lblProspectResCount.Visible = true;
                lblProspectRes.Visible = false;
                lblProspectResCount.Text = "Total " + SinglAgentProspects.Tables[0].Rows.Count.ToString() + " records found";
                grdDealerProsInfo.DataSource = SinglAgentProspects.Tables[0];
                grdDealerProsInfo.DataBind();
            }
            else
            {
                grdDealerProsInfo.Visible = false;
                lblProspectResCount.Visible = false;
                lblProspectRes.Visible = true;
                lblProspectRes.Text = "No records exist";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnPromotionals_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int SaleAgentID = Convert.ToInt32(Session[Constants.USER_ID]);
            DataSet SingleAgentPromotionals = new DataSet();
            SingleAgentPromotionals = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            tblProspects.Style["display"] = "none";
            tblSales.Style["display"] = "none";
            tblPromotionals.Style["display"] = "block";
            if (SingleAgentPromotionals.Tables[0].Rows.Count > 0)
            {
                grdDealerPromotInfo.Visible = true;
                lblPromotionalResCount.Visible = true;
                lblPromotionalRes.Visible = false;
                lblPromotionalResCount.Text = "Total " + SingleAgentPromotionals.Tables[0].Rows.Count.ToString() + " records found";
                grdDealerPromotInfo.DataSource = SingleAgentPromotionals.Tables[0];
                grdDealerPromotInfo.DataBind();
            }
            else
            {
                grdDealerPromotInfo.Visible = false;
                lblPromotionalResCount.Visible = false;
                lblPromotionalRes.Visible = true;
                lblPromotionalRes.Text = "No records exist";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdDealerSaleInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnPackName");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnPhoneNum");

                Double PackCost = new Double();
                PackCost = Convert.ToDouble(hdnPackCost.Value.ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = hdnPackName.Value.ToString();
                lblPackage.Text = PackName + "($" + PackAmount + ")";
                if (hdnPhoneNum.Value.ToString() == "")
                {
                    lblPhone.Text = "";
                }
                else
                {
                    lblPhone.Text = objGeneralFunc.filPhnm(hdnPhoneNum.Value);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdDealerProsInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnPackName");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnPhoneNum");

                Double PackCost = new Double();
                PackCost = Convert.ToDouble(hdnPackCost.Value.ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = hdnPackName.Value.ToString();
                lblPackage.Text = PackName + "($" + PackAmount + ")";
                if (hdnPhoneNum.Value.ToString() == "")
                {
                    lblPhone.Text = "";
                }
                else
                {
                    lblPhone.Text = objGeneralFunc.filPhnm(hdnPhoneNum.Value);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdDealerPromotInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnPackName");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnPhoneNum");

                Double PackCost = new Double();
                PackCost = Convert.ToDouble(hdnPackCost.Value.ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = hdnPackName.Value.ToString();
                lblPackage.Text = PackName + "($" + PackAmount + ")";
                if (hdnPhoneNum.Value.ToString() == "")
                {
                    lblPhone.Text = "";
                }
                else
                {
                    lblPhone.Text = objGeneralFunc.filPhnm(hdnPhoneNum.Value);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdDealerProsInfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditSale")
        {
            int DealerSaleID = Convert.ToInt32(e.CommandArgument.ToString());
            Session["DealerEditSaleID"] = DealerSaleID;
            Response.Redirect("NewDealerSale.aspx");
        }
    }


    protected void lnkDealerIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerUID";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkDealerIDSort.Text = "Dealer ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkDealerIDSort.Text = "Dealer ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkDealerIDSort.Text = "Dealer ID &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkDealerIDSort.Text = "Dealer ID &#8659";
            }
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkDealerShipName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerShipName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkDealerShipName.Text = "Dlrship Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkDealerShipName.Text = "Dlrship Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkDealerShipName.Text = "Dlrship Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkDealerShipName.Text = "Dlrship Name &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPackageSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "Price";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPackageSort.Text = "Package &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPackageSort.Text = "Package &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkSaleDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleDate";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkSaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkSaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkSaleDateSort.Text = "Sale Dt &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkSaleDateSort.Text = "Sale Dt &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkTargetDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "TargetSignupDate";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkTargetDateSort.Text = "Target Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkTargetDateSort.Text = "Target Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkTargetDateSort.Text = "Target Dt &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkTargetDateSort.Text = "Target Dt &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotionSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PromotionOptionCode";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotionSort.Text = "Promotion &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotionSort.Text = "Promotion &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotionSort.Text = "Promotion &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotionSort.Text = "Promotion &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkAgentSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentUFirstName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkAgentSort.Text = "Agent &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkAgentSort.Text = "Agent &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnDealerStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnDealerStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnDealerStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnDealerStatus.Text = "Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnDealerStatus.Text = "Status &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnQCStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "QCStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnQCStatus.Text = "QC Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnQCStatus.Text = "QC Status &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnContactName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerContactName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnContactName.Text = "Contact Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnContactName.Text = "Contact Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnContactName.Text = "Contact Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnContactName.Text = "Contact Name &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPhoneSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerContactPhone";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPhoneSort.Text = "Phone &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPhoneSort.Text = "Phone &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnCitySort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerCity";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnCitySort.Text = "City &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnCitySort.Text = "City &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnCitySort.Text = "City &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnCitySort.Text = "City &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnZipSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentSalesForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerZip";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnZipSort.Text = "Zip &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnZipSort.Text = "Zip &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnZipSort.Text = "Zip &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnZipSort.Text = "Zip &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerSaleInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkPromotDealerIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerUID";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotDealerIDSort.Text = "Dealer ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotDealerIDSort.Text = "Dealer ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotDealerIDSort.Text = "Dealer ID &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotDealerIDSort.Text = "Dealer ID &#8659";
            }
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotDealerShipName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerShipName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotDealerShipName.Text = "Dlrship Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotDealerShipName.Text = "Dlrship Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotDealerShipName.Text = "Dlrship Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotDealerShipName.Text = "Dlrship Name &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotPackageSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "Price";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotPackageSort.Text = "Package &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPackageSort.Text = "Package &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotSaleDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleDate";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotSaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotSaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotSaleDateSort.Text = "Sale Dt &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotSaleDateSort.Text = "Sale Dt &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotTargetDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "TargetSignupDate";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotTargetDateSort.Text = "Target Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotTargetDateSort.Text = "Target Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotTargetDateSort.Text = "Target Dt &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotTargetDateSort.Text = "Target Dt &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotPromotionSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PromotionOptionCode";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPromotionSort.Text = "Promotion &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPromotionSort.Text = "Promotion &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotPromotionSort.Text = "Promotion &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPromotionSort.Text = "Promotion &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotAgentSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentUFirstName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotAgentSort.Text = "Agent &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotAgentSort.Text = "Agent &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnPromotDealerStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotDealerStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotDealerStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnPromotDealerStatus.Text = "Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotDealerStatus.Text = "Status &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnPromotQCStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "QCStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnPromotQCStatus.Text = "QC Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotQCStatus.Text = "QC Status &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnPromotContactName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerContactName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotContactName.Text = "Contact Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotContactName.Text = "Contact Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnPromotContactName.Text = "Contact Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotContactName.Text = "Contact Name &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPromotPhoneSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerContactPhone";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkPromotPhoneSort.Text = "Phone &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkPromotPhoneSort.Text = "Phone &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnPromotCitySort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerCity";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotCitySort.Text = "City &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotCitySort.Text = "City &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnPromotCitySort.Text = "City &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotCitySort.Text = "City &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnPromotZipSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentPromotionalsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerZip";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotZipSort.Text = "Zip &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotZipSort.Text = "Zip &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnPromotZipSort.Text = "Zip &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPromotZipSort.Text = "Zip &#8659";
            }
            lnkPromotDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkPromotDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPromotPackageSort.Text = "Package &darr; &uarr;";
            lnkPromotSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkPromotTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkPromotAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnPromotDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnPromotQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPromotContactName.Text = "Contact Name &darr; &uarr;";
            lnkPromotPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnPromotCitySort.Text = "City &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerPromotInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkProsDealerIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerUID";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsDealerIDSort.Text = "Dealer ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsDealerIDSort.Text = "Dealer ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsDealerIDSort.Text = "Dealer ID &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsDealerIDSort.Text = "Dealer ID &#8659";
            }
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkProsDealerShipName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerShipName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsDealerShipName.Text = "Dlrship Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsDealerShipName.Text = "Dlrship Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsDealerShipName.Text = "Dlrship Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsDealerShipName.Text = "Dlrship Name &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkProsPackageSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "Price";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsPackageSort.Text = "Package &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPackageSort.Text = "Package &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkProsSaleDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerEnterDate";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsSaleDateSort.Text = "Enter Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsSaleDateSort.Text = "Enter Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsSaleDateSort.Text = "Enter Dt &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsSaleDateSort.Text = "Enter Dt &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkProsTargetDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "TargetSignupDate";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsTargetDateSort.Text = "Target Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsTargetDateSort.Text = "Target Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsTargetDateSort.Text = "Target Dt &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsTargetDateSort.Text = "Target Dt &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkProsPromotionSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PromotionOptionCode";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPromotionSort.Text = "Promotion &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPromotionSort.Text = "Promotion &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsPromotionSort.Text = "Promotion &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPromotionSort.Text = "Promotion &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkProsAgentSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentUFirstName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsAgentSort.Text = "Agent &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsAgentSort.Text = "Agent &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnProsDealerStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsDealerStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsDealerStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnProsDealerStatus.Text = "Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsDealerStatus.Text = "Status &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnProsQCStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "QCStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnProsQCStatus.Text = "QC Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsQCStatus.Text = "QC Status &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnProsContactName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerContactName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsContactName.Text = "Contact Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsContactName.Text = "Contact Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnProsContactName.Text = "Contact Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsContactName.Text = "Contact Name &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkProsPhoneSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerContactPhone";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkProsPhoneSort.Text = "Phone &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkProsPhoneSort.Text = "Phone &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnProsCitySort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerCity";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsCitySort.Text = "City &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsCitySort.Text = "City &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnProsCitySort.Text = "City &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsCitySort.Text = "City &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsZipSort.Text = "Zip &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnProsZipSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["SingleAgentProspectsForDealer"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "DealerZip";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsZipSort.Text = "Zip &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsZipSort.Text = "Zip &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnProsZipSort.Text = "Zip &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnProsZipSort.Text = "Zip &#8659";
            }
            lnkProsDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkProsDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkProsPackageSort.Text = "Package &darr; &uarr;";
            lnkProsSaleDateSort.Text = "Enter Dt &darr; &uarr;";
            lnkProsTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkProsPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkProsAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnProsDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnProsQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnProsContactName.Text = "Contact Name &darr; &uarr;";
            lnkProsPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnProsCitySort.Text = "City &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdDealerProsInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}

