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
using System.Net.Mail;

public partial class QCReportForDealer : System.Web.UI.Page
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
                Session["CurrentPage"] = "QC Module";

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
                        string Status = "All";
                        FillCenters();
                        GetResults(Status, 0);
                    }
                }
            }
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

                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Central report")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnAllCentersReport.Enabled = true;
                        lnkbtnAddCenters.Enabled = true;
                        lnkbtnAllusersmgmnt.Enabled = true;
                        lnkbtnIPAddress.Enabled = true;
                        lnkbtnSalesreport.Enabled = true;
                        lnkbtnLeadsAssign.Enabled = true;
                        lnkbtnLeadsDownLoad.Enabled = true;
                    }
                }
            }
        }


    }
    private bool LoadIndividualUserRights()
    {
        DataSet dsIndidivitualRights = new DataSet();
        bool bValid = false;

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
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            HotLeadsBL objHotLeadsBL = new HotLeadsBL();
            objHotLeadsBL.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), dtNow, Convert.ToInt32(Session[Constants.USERLOG_ID]), 2);
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            string Status;
            if (rdbtnAll.Checked == true)
            {
                Status = "All";
            }
            else if (rdbtnQCOpen.Checked == true)
            {
                Status = "QCOpen";
            }
            else
            {
                Status = "QCDonePayOpen";
            }
            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            GetResults(Status, CenterID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void GetResults(string Status, int CenterID)
    {
        try
        {
            // DateTime StartingDate = Convert.ToDateTime(StartDate.AddDays(-1).ToString("MM/dd/yyyy"));
            DataSet SingleAgentSales = new DataSet();
            SingleAgentSales = objHotLeadBL.GetQCDataForDealersData(Status, CenterID);
            Session["AllDealersSalesQCData"] = SingleAgentSales;
            lblResHead.Text = "Recent 400 sales are showing";
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
                HiddenField hdnAgentID = (HiddenField)e.Row.FindControl("hdnAgentID");
                Label lblAgent = (Label)e.Row.FindControl("lblAgent");
                HiddenField hdnAgentName = (HiddenField)e.Row.FindControl("hdnAgentName");
                HiddenField hdnAgentCenterID = (HiddenField)e.Row.FindControl("hdnAgentCenterID");
                Label lblQCStatus = (Label)e.Row.FindControl("lblQCStatus");
                HiddenField hdnQCStatusName = (HiddenField)e.Row.FindControl("hdnQCStatusName");
                HiddenField hdnQCStatusID = (HiddenField)e.Row.FindControl("hdnQCStatusID");
                LinkButton lnkbtnPaymentStatus = (LinkButton)e.Row.FindControl("lnkbtnPaymentStatus");
                HiddenField hdnPSID1Status = (HiddenField)e.Row.FindControl("hdnPayStatus");
                HiddenField hdnPSID1StatusName = (HiddenField)e.Row.FindControl("hdnPayStatusName");
                LinkButton lnkbtnMoveSmartz = (LinkButton)e.Row.FindControl("lnkbtnMoveSmartz");
                HiddenField hdnSmartzStatus = (HiddenField)e.Row.FindControl("hdnSmartzStatus");
                HiddenField hdnSmartzCarID = (HiddenField)e.Row.FindControl("hdnSmartzCarID");
                HiddenField hdnSmartzMovedDate = (HiddenField)e.Row.FindControl("hdnSmartzMovedDate");
                HiddenField hdnPSAmount = (HiddenField)e.Row.FindControl("hdnPSAmount");

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
                if (hdnAgentID.Value.ToString() != "0")
                {
                    if (hdnAgentName.Value != "")
                    {
                        lblAgent.Text = objGeneralFunc.GetCenterCode(hdnAgentCenterID.Value) + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnAgentName.Value.ToString(), 15);
                    }
                }
                else
                {
                    lblAgent.Text = "";
                }
                if (hdnQCStatusID.Value == "")
                {
                    lblQCStatus.Text = "QC Open";
                    lblQCStatus.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    if (hdnQCStatusID.Value == "1")
                    {
                        lblQCStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    if (hdnQCStatusID.Value == "2")
                    {
                        lblQCStatus.ForeColor = System.Drawing.Color.Red;
                    }
                    if (hdnQCStatusID.Value == "3")
                    {
                        lblQCStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                    if (hdnQCStatusID.Value == "4")
                    {
                        lblQCStatus.ForeColor = System.Drawing.Color.Blue;
                    }
                    lblQCStatus.Text = hdnQCStatusName.Value;
                }
                lnkbtnPaymentStatus.Text = hdnPSID1StatusName.Value;
                if ((hdnPSID1Status.Value == "1") || (hdnPSID1Status.Value == "7") || (hdnPSID1Status.Value == "8"))
                {
                    lnkbtnPaymentStatus.ForeColor = System.Drawing.Color.Green;
                }
                else if (hdnPSID1Status.Value == "2")
                {
                    lnkbtnPaymentStatus.ForeColor = System.Drawing.Color.Red;
                }
                else if (hdnPSID1Status.Value == "3")
                {
                    lnkbtnPaymentStatus.ForeColor = System.Drawing.Color.Orange;
                }
                else if ((hdnPSID1Status.Value == "4") || (hdnPSID1Status.Value == "6"))
                {
                    lnkbtnPaymentStatus.ForeColor = System.Drawing.Color.Black;
                }
                else if (hdnPSID1Status.Value == "5")
                {
                    lnkbtnPaymentStatus.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    lnkbtnPaymentStatus.ForeColor = System.Drawing.Color.Yellow;
                }
                if (hdnQCStatusID.Value == "1")
                {
                    lnkbtnPaymentStatus.Enabled = true;
                }
                else
                {
                    lnkbtnPaymentStatus.Enabled = false;
                }

                if (hdnSmartzStatus.Value == "1")
                {
                    if (hdnSmartzMovedDate.Value != "")
                    {
                        DateTime MovedDate = Convert.ToDateTime(hdnSmartzMovedDate.Value);
                        lnkbtnMoveSmartz.Text = "Moved (" + hdnSmartzCarID.Value + " " + MovedDate.ToString("MM/dd/yyyy hh:mm tt") + ")";
                    }
                    else
                    {
                        lnkbtnMoveSmartz.Text = "Moved (" + hdnSmartzCarID.Value + ")";
                    }
                    lnkbtnMoveSmartz.Enabled = false;
                    lnkbtnMoveSmartz.ForeColor = System.Drawing.Color.Green;
                }
                else if (((hdnPSID1Status.Value == "1") || (hdnPSID1Status.Value == "7") || (hdnPSID1Status.Value == "8")) && (hdnQCStatusID.Value == "1"))
                {

                    if ((hdnSmartzStatus.Value == "") || (hdnSmartzStatus.Value == "0"))
                    {
                        lnkbtnMoveSmartz.Text = "Ready to move";
                        lnkbtnMoveSmartz.Enabled = true;
                        lnkbtnMoveSmartz.ForeColor = System.Drawing.Color.Orange;
                    }
                    else
                    {
                        if (hdnSmartzMovedDate.Value != "")
                        {
                            DateTime MovedDate = Convert.ToDateTime(hdnSmartzMovedDate.Value);
                            lnkbtnMoveSmartz.Text = "Moved (" + hdnSmartzCarID.Value + " " + MovedDate.ToString("MM/dd/yyyy hh:mm tt") + ")";
                        }
                        else
                        {
                            lnkbtnMoveSmartz.Text = "Moved (" + hdnSmartzCarID.Value + ")";
                        }
                        lnkbtnMoveSmartz.Enabled = false;
                        lnkbtnMoveSmartz.ForeColor = System.Drawing.Color.Green;
                    }
                }

                else
                {
                    if ((hdnSmartzStatus.Value != "1") && (hdnQCStatusID.Value == "1") && ((hdnPSID1Status.Value == "3") || (hdnPSID1Status.Value == "4")))
                    {
                        if (hdnPSAmount.Value != "")
                        {
                            Double TotalAmount1 = Convert.ToDouble(hdnPSAmount.Value);
                            string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                            if (ChkAmount == "0.00")
                            {
                                lnkbtnMoveSmartz.Text = "Ready to move";
                                lnkbtnMoveSmartz.Enabled = true;
                                lnkbtnMoveSmartz.ForeColor = System.Drawing.Color.Orange;
                            }
                            else
                            {
                                lnkbtnMoveSmartz.Enabled = false;
                                lnkbtnMoveSmartz.Text = "Not ready";
                                lnkbtnMoveSmartz.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        else
                        {
                            lnkbtnMoveSmartz.Enabled = false;
                            lnkbtnMoveSmartz.Text = "Not ready";
                            lnkbtnMoveSmartz.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        lnkbtnMoveSmartz.Enabled = false;
                        lnkbtnMoveSmartz.Text = "Not ready";
                        lnkbtnMoveSmartz.ForeColor = System.Drawing.Color.Black;
                    }
                    //SmartzStatus
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillPayCancelReason()
    {
        try
        {
            DataSet dsReason = new DataSet();
            ddlPayCancelReason.Items.Clear();
            if (Session["CancellationReason"] == null)
            {
                dsReason = objHotLeadBL.GetPmntCancelReasons();
                Session["CancellationReason"] = dsReason;
            }
            else
            {
                dsReason = (DataSet)Session["CancellationReason"];
            }
            ddlPayCancelReason.DataSource = dsReason.Tables[0];
            ddlPayCancelReason.DataTextField = "PaymentCancelReasonName";
            ddlPayCancelReason.DataValueField = "PaymentCancelReasonID";
            ddlPayCancelReason.DataBind();
            ddlPayCancelReason.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillPaymentDate()
    {
        try
        {
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ddlPaymentDate.Items.Clear();
            for (int i = 0; i < 14; i++)
            {
                ListItem list = new ListItem();
                list.Text = dtNow.AddDays(-i).ToString("MM/dd/yyyy");
                list.Value = dtNow.AddDays(-i).ToString("MM/dd/yyyy");
                ddlPaymentDate.Items.Add(list);
            }
            ddlPaymentDate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdDealerSaleInfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditSale")
            {
                int DealerSaleID = Convert.ToInt32(e.CommandArgument.ToString());
                Session["DealerQCDealerSaleID"] = DealerSaleID;
                Response.Redirect("QCDataViewForDealer.aspx");
            }
            if (e.CommandName == "EditPayInfo")
            {
                int DealerSaleID = Convert.ToInt32(e.CommandArgument.ToString());
                DataSet Cardetais = objHotLeadBL.GetInfoByDealerSaleIDForPayInfo(DealerSaleID);
                Session["QCPayUpPmntTypeID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString());
                Session["QCPayUpPmntID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PaymentID"].ToString());
                lblpaymentPopSaleID.Text = Cardetais.Tables[0].Rows[0]["DealerSaleID"].ToString();
                if (Cardetais.Tables[0].Rows[0]["DealerContactPhone"].ToString() == "")
                {
                    lblPayInfoPhone.Text = "";
                }
                else
                {
                    lblPayInfoPhone.Text = objGeneralFunc.filPhnm(Cardetais.Tables[0].Rows[0]["DealerContactPhone"].ToString());
                }
                lblPayInfoVoiceConfNo.Text = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString();
                lblPayInfoEmail.Text = Cardetais.Tables[0].Rows[0]["DealerEmail"].ToString();
                if (Cardetais.Tables[0].Rows[0]["SchedulePaymentDate"].ToString() != "")
                {
                    DateTime PaymentDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SchedulePaymentDate"].ToString());
                    lblPoplblPayDate.Text = PaymentDate.ToString("MM/dd/yyyy");
                    Session["QCPayUpPmntDate"] = PaymentDate.ToString("MM/dd/yyyy");
                }
                lblPoplblPayAmount.Text = Cardetais.Tables[0].Rows[0]["Amount"].ToString();
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = Cardetais.Tables[0].Rows[0]["Description"].ToString();
                lblPoplblPackage.Text = PackName + "($" + PackAmount + ")";
                hdnPophdnAmount.Value = Cardetais.Tables[0].Rows[0]["Amount"].ToString();

                string OldNotes = Cardetais.Tables[0].Rows[0]["PaymentNotes"].ToString();
                OldNotes = OldNotes.Replace("<br>", Environment.NewLine);
                txtPaymentNotes.Text = OldNotes;
                txtPaymentNewNotes.Text = "";

                FillPayCancelReason();
                hdnPopPayType.Value = Cardetais.Tables[0].Rows[0]["pmntType"].ToString();
                if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                {
                    divcard.Style["display"] = "none";
                    divcheck.Style["display"] = "block";
                    divpaypal.Style["display"] = "none";
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 8))
                    {
                        divTransID.Style["display"] = "block";
                        divPaymentDate.Style["display"] = "block";
                        divPaymentAmount.Style["display"] = "block";
                        divReason.Style["display"] = "none";
                        if (Cardetais.Tables[0].Rows[0]["SchedulePaymentDate"].ToString() != "")
                        {
                            DateTime PaymentDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SchedulePaymentDate"].ToString());
                            //ListItem liPayDate = new ListItem();
                            //liPayDate.Text = PaymentDate.ToString("MM/dd/yyyy");
                            //liPayDate.Value = PaymentDate.ToString("MM/dd/yyyy");
                            //ddlPaymentDate.SelectedIndex = ddlPaymentDate.Items.IndexOf(liPayDate);
                            ddlPaymentDate.Items.Clear();
                            ddlPaymentDate.Items.Insert(0, new ListItem(PaymentDate.ToString("MM/dd/yyyy"), PaymentDate.ToString("MM/dd/yyyy")));
                        }

                        txtPaytransID.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                        txtPaymentAmountInPop.Text = Cardetais.Tables[0].Rows[0]["Amount"].ToString();
                        txtPaytransID.Enabled = false;
                        ddlPaymentDate.Enabled = false;
                        ddlPaymentStatus.Enabled = false;
                        btnUpdate.Enabled = false;
                        txtPaymentAmountInPop.Enabled = false;
                        txtPaymentNewNotes.Enabled = false;
                    }
                    else
                    {
                        txtPaymentNewNotes.Enabled = true;
                        ddlPaymentStatus.Enabled = true;
                        btnUpdate.Enabled = true;
                        divTransID.Style["display"] = "none";
                        divPaymentDate.Style["display"] = "none";
                        divPaymentAmount.Style["display"] = "none";
                        txtPaytransID.Text = "";
                        FillPaymentDate();
                        txtPaytransID.Enabled = true;
                        ddlPaymentDate.Enabled = true;
                        txtPaymentAmountInPop.Text = "";
                        txtPaymentAmountInPop.Enabled = true;
                        if (Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString() == "2")
                        {
                            divReason.Style["display"] = "block";
                            ListItem liPayReason = new ListItem();
                            liPayReason.Text = Cardetais.Tables[0].Rows[0]["PaymentCancelReasonName"].ToString();
                            liPayReason.Value = Cardetais.Tables[0].Rows[0]["PaymentCancelReasonID"].ToString();
                            ddlPayCancelReason.SelectedIndex = ddlPayCancelReason.Items.IndexOf(liPayReason);
                        }
                        else
                        {
                            divReason.Style["display"] = "none";
                        }
                    }
                    lblAccHolderName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
                    lblAccNumber.Text = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                    lblBankName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankName"].ToString());
                    lblRouting.Text = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
                    lblAccType.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["AccountTypeName"].ToString());
                    ListItem liPayStatus = new ListItem();
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PSStatusName"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString();
                    ddlPaymentStatus.SelectedIndex = ddlPaymentStatus.Items.IndexOf(liPayStatus);
                    //lblCheckNumber.Text = Cardetais.Tables[0].Rows[0]["BankCheckNumber"].ToString();
                    //lblCheckType.Text = Cardetais.Tables[0].Rows[0]["CheckTypeName"].ToString();


                }
                else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 8)
                {
                    divcard.Style["display"] = "none";
                    divcheck.Style["display"] = "none";
                    divpaypal.Style["display"] = "block";
                    divTransID.Style["display"] = "none";
                    divPaymentDate.Style["display"] = "none";
                    divPaymentAmount.Style["display"] = "none";
                    lblInvoiceAttentionTo.Text = Cardetais.Tables[0].Rows[0]["InvoiceAttentionTo"].ToString();
                    if (Cardetais.Tables[0].Rows[0]["SendInvoiceID"].ToString() == "1")
                    {
                        rdbtnInvoiceEmail.Checked = true;
                    }
                    else
                    {
                        rdbtnInvoicePostal.Checked = true;
                    }
                    lblInvoiceEmail.Text = Cardetais.Tables[0].Rows[0]["SendInvoiceEmail"].ToString();
                    lblInvoiceBillingName.Text = Cardetais.Tables[0].Rows[0]["billingName"].ToString();
                    lblInvoiceBillingAddress.Text = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString();
                    lblInvoiceBillingCity.Text = Cardetais.Tables[0].Rows[0]["billingCity"].ToString();
                    lblInvoiceBillingState.Text = Cardetais.Tables[0].Rows[0]["BillingStateName"].ToString();
                    lblInvoiceBillingZip.Text = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
                    //lblInvoiceBillingName.Text = Cardetais.Tables[0].Rows[0]["billingName"].ToString();
                    ListItem liPayStatus = new ListItem();
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PSStatusName"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString();
                    ddlPaymentStatus.SelectedIndex = ddlPaymentStatus.Items.IndexOf(liPayStatus);
                    if (Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString() == "2")
                    {
                        divReason.Style["display"] = "block";
                        FillPayCancelReason();
                        ListItem liPayReason = new ListItem();
                        liPayReason.Text = Cardetais.Tables[0].Rows[0]["PaymentCancelReasonName"].ToString();
                        liPayReason.Value = Cardetais.Tables[0].Rows[0]["PaymentCancelReasonID"].ToString();
                        ddlPayCancelReason.SelectedIndex = ddlPayCancelReason.Items.IndexOf(liPayReason);
                    }
                    else
                    {
                        FillPayCancelReason();
                        divReason.Style["display"] = "none";
                    }
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 8))
                    {
                        ddlPaymentStatus.Enabled = false;
                        btnUpdate.Enabled = false;
                    }
                    else
                    {
                        ddlPaymentStatus.Enabled = true;
                        btnUpdate.Enabled = true;
                    }
                }
                else
                {
                    divcard.Style["display"] = "block";
                    divcheck.Style["display"] = "none";
                    divpaypal.Style["display"] = "none";
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString()) == 8))
                    {
                        divTransID.Style["display"] = "block";
                        divPaymentDate.Style["display"] = "block";
                        divPaymentAmount.Style["display"] = "block";
                        if (Cardetais.Tables[0].Rows[0]["SchedulePaymentDate"].ToString() != "")
                        {
                            DateTime PaymentDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SchedulePaymentDate"].ToString());
                            ddlPaymentDate.Items.Clear();
                            ddlPaymentDate.Items.Insert(0, new ListItem(PaymentDate.ToString("MM/dd/yyyy"), PaymentDate.ToString("MM/dd/yyyy")));
                        }
                        txtPaytransID.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                        txtPaymentAmountInPop.Text = Cardetais.Tables[0].Rows[0]["Amount"].ToString();
                        txtPaytransID.Enabled = false;
                        ddlPaymentDate.Enabled = false;
                        ddlPaymentStatus.Enabled = false;
                        btnUpdate.Enabled = false;
                        txtPaymentAmountInPop.Enabled = false;
                        txtPaymentNewNotes.Enabled = false;
                    }
                    else
                    {
                        ddlPaymentStatus.Enabled = true;
                        txtPaymentNewNotes.Enabled = true;
                        btnUpdate.Enabled = true;
                        divTransID.Style["display"] = "none";
                        divPaymentDate.Style["display"] = "none";
                        divPaymentAmount.Style["display"] = "none";
                        txtPaytransID.Text = "";
                        txtPaymentAmountInPop.Text = "";
                        txtPaymentAmountInPop.Enabled = true;
                        FillPaymentDate();
                        txtPaytransID.Enabled = true;
                        ddlPaymentDate.Enabled = true;
                        if (Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString() == "2")
                        {
                            divReason.Style["display"] = "block";
                            FillPayCancelReason();
                            ListItem liPayReason = new ListItem();
                            liPayReason.Text = Cardetais.Tables[0].Rows[0]["PaymentCancelReasonName"].ToString();
                            liPayReason.Value = Cardetais.Tables[0].Rows[0]["PaymentCancelReasonID"].ToString();
                            ddlPayCancelReason.SelectedIndex = ddlPayCancelReason.Items.IndexOf(liPayReason);
                        }
                        else
                        {
                            FillPayCancelReason();
                            divReason.Style["display"] = "none";
                        }
                    }
                    //lblCCCardType.Text = Cardetais.Tables[0].Rows[0]["lblCCCardType"].ToString();
                    lblCardHolderName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderName"].ToString());
                    lblCCCardType.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardType"].ToString());
                    lblCardHolderLastName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString());
                    lblCCNumber.Text = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
                    string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
                    string[] EXpDt = EXpDate.Split(new char[] { '/' });
                    lblCCExpiryDate.Text = EXpDt[0].ToString() + "/" + "20" + EXpDt[1].ToString();
                    lblCvv.Text = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();
                    lblBillingAddress.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString());
                    lblBillingCity.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString());
                    lblBillingState.Text = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
                    lblBillingZip.Text = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
                    ListItem liPayStatus = new ListItem();
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PSStatusName"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString();
                    ddlPaymentStatus.SelectedIndex = ddlPaymentStatus.Items.IndexOf(liPayStatus);
                }
                if (hdnPophdnAmount.Value == "0")
                {
                    divTransID.Style["display"] = "none";
                    divPaymentDate.Style["display"] = "none";
                    divPaymentAmount.Style["display"] = "none";
                }

                MPEUpdate.Show();
            }
            if (e.CommandName == "MoveSmartz")
            {
                int DealerSaleID = Convert.ToInt32(e.CommandArgument.ToString());
                Session["DealerQCDealerSaleID"] = DealerSaleID;
                MdepEnterDEalerCode.Show();
                txtDealerCode.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int pmntID = Convert.ToInt32(Session["QCPayUpPmntID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int PSStatusID = Convert.ToInt32(ddlPaymentStatus.SelectedItem.Value);
            string TransactionID = txtPaytransID.Text;
            DateTime dtPayDate;
            if (PSStatusID == 1)
            {
                if (hdnPophdnAmount.Value != "0")
                {
                    dtPayDate = Convert.ToDateTime(ddlPaymentDate.SelectedItem.Text);
                }
                else
                {
                    dtPayDate = Convert.ToDateTime(Session["QCPayUpPmntDate"].ToString());
                }
            }
            else
            {
                dtPayDate = Convert.ToDateTime("1/1/1990");
            }
            string Amount = string.Empty;
            if (hdnPophdnAmount.Value != "0")
            {
                Amount = txtPaymentAmountInPop.Text;
            }
            else
            {
                Amount = "0";
            }
            int PayCancelReason = Convert.ToInt32(ddlPayCancelReason.SelectedItem.Value);
            string PaymentNotes = string.Empty;
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            String UpdatedBy = Session[Constants.NAME].ToString();
            if (txtPaymentNewNotes.Text.Trim() != "")
            {
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
                if (txtPaymentNotes.Text.Trim() != "")
                {
                    PaymentNotes = txtPaymentNotes.Text.Trim() + "\n" + UpdateByWithDate + txtPaymentNewNotes.Text.Trim() + "\n" + "-------------------------------------------------";
                }
                else
                {
                    PaymentNotes = UpdateByWithDate + txtPaymentNewNotes.Text.Trim() + "\n" + "-------------------------------------------------";
                }
            }
            else
            {
                PaymentNotes = txtPaymentNotes.Text.Trim();
            }
            DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatusForDealer(pmntID, PSStatusID, TransactionID, dtPayDate, PayCancelReason, Amount, UID, PaymentNotes);
            MPEUpdate.Hide();
            string Status = "All";
            if (rdbtnAll.Checked == true)
            {
                Status = "All";
            }
            else if (rdbtnQCDonepayopen.Checked == true)
            {
                Status = "QCDonePayOpen";
            }
            else if (rdbtnQCOpen.Checked == true)
            {
                Status = "QCOpen";
            }
            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            GetResults(Status, CenterID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnPopMovetoSmartz_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet GetDealerData = objHotLeadBL.GetDealerDetailsByDealerSaleID(Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString()));
            Session["GetDealerData"] = GetDealerData;
            string RegName = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["DealerContactName"].ToString());
            string RegUserName = GetDealerData.Tables[0].Rows[0]["DealerEmail"].ToString();
            string Password = GetDealerData.Tables[0].Rows[0]["DealerContactPhone"].ToString();
            string RegPhone = GetDealerData.Tables[0].Rows[0]["DealerContactPhone"].ToString();
            string RegAddress = GetDealerData.Tables[0].Rows[0]["DealerAddress"].ToString();
            string RegCity = GetDealerData.Tables[0].Rows[0]["DealerCity"].ToString();
            int RegState = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["DealerStateID"].ToString());
            string RegZip = string.Empty;
            string BusinessName = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["DealerShipName"].ToString());
            string AltEmail = "";
            string RegAltPhone = GetDealerData.Tables[0].Rows[0]["DealerPhone"].ToString();
            int SalesAgentID = 0;
            string CenterCode = objGeneralFunc.GetCenterCode(GetDealerData.Tables[0].Rows[0]["AgentCenterID"].ToString());
            if (CenterCode == "INBH")
            {
                SalesAgentID = Convert.ToInt32(56);
            }
            if (CenterCode == "TEST")
            {
                SalesAgentID = Convert.ToInt32(35);
            }
            if (SalesAgentID == 0)
            {
                DataSet dsCenter = objCentralDBBL.CheckAgentExists(CenterCode);
                if (dsCenter.Tables.Count > 0)
                {
                    if (dsCenter.Tables[0].Rows.Count > 0)
                    {
                        SalesAgentID = Convert.ToInt32(dsCenter.Tables[0].Rows[0]["Sale_Agent_Id"].ToString());
                    }
                    else
                    {
                        mdepAlertExists.Show();
                        lblErrorExists.Visible = true;
                        lblErrorExists.Text = "Agnet not exist with center " + CenterCode + " in smartz. <br />Sale not able to move to smartz.";
                    }
                }
                else
                {
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Agnet not exist with center " + CenterCode + " in smartz. <br />Sale not able to move to smartz.";
                }
            }

            int VerifierID = Convert.ToInt32(SalesAgentID);
            RegZip = GetDealerData.Tables[0].Rows[0]["DealerZip"].ToString();
            string UserID;
            string FistName = RegName;
            if (FistName.Length > 3)
            {
                FistName = FistName.Substring(0, 3);
            }
            string s = "";
            int j;
            Random random1 = new Random();
            for (j = 1; j < 4; j++)
            {
                s += random1.Next(0, 9).ToString();
            }
            UserID = FistName + RegPhone;

            int EmailExists = 1;
            int PackageID = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["PackageID"].ToString());
            string DealerID = txtDealerCode.Text.Trim();
            DataSet dsDealerIDExists = objdropdownBL.ChkUserExistsDealerCode(DealerID);
            DataSet dsPhoneExists = objdropdownBL.ChkUserExistsPhoneNumber(RegPhone);
            if (dsDealerIDExists.Tables.Count > 0)
            {
                if (dsDealerIDExists.Tables[0].Rows.Count > 0)
                {
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Dealer id " + DealerID + " already exists";
                }
                else
                {
                    if (dsPhoneExists.Tables.Count > 0)
                    {
                        if (dsPhoneExists.Tables[0].Rows.Count > 0)
                        {
                            mdepAlertExists.Show();
                            lblErrorExists.Visible = true;
                            lblErrorExists.Text = "Phone number " + RegPhone + " already exists";
                        }
                        else
                        {
                            if (RegUserName != "")
                            {
                                EmailExists = 1;
                                DataSet dsUserExists = objdropdownBL.USP_ChkUserExists(RegUserName);
                                if (dsUserExists.Tables.Count > 0)
                                {
                                    if (dsUserExists.Tables[0].Rows.Count > 0)
                                    {
                                        Session.Timeout = 180;
                                        mdepAlertExists.Show();
                                        lblErrorExists.Visible = true;
                                        lblErrorExists.Text = "Email " + RegUserName + " already exists";
                                    }
                                    else
                                    {
                                        DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                        if (dsUserIDExists.Tables.Count > 0)
                                        {
                                            if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                            {
                                                UserID = UserID + s.ToString();
                                                DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                                string m = "";
                                                int l;
                                                if (dsUserIDExists2.Tables.Count > 0)
                                                {
                                                    if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (l = 1; l < 2; l++)
                                                        {
                                                            m += random1.Next(0, 9).ToString();
                                                        }
                                                        UserID = UserID + m.ToString();
                                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                    }
                                                    else
                                                    {
                                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                    }
                                                }
                                                else
                                                {
                                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                }
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                }
                                else
                                {
                                    DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                    if (dsUserIDExists.Tables.Count > 0)
                                    {
                                        if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                        {
                                            UserID = UserID + s.ToString();
                                            DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                            string m = "";
                                            int l;
                                            if (dsUserIDExists2.Tables.Count > 0)
                                            {
                                                if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                                {
                                                    for (l = 1; l < 2; l++)
                                                    {
                                                        m += random1.Next(0, 9).ToString();
                                                    }
                                                    UserID = UserID + m.ToString();
                                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                }
                                                else
                                                {
                                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                }
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }

                            }
                            else
                            {
                                EmailExists = 0;
                                DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                if (dsUserIDExists.Tables.Count > 0)
                                {
                                    if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                    {
                                        UserID = UserID + s.ToString();
                                        DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                        string m = "";
                                        int l;
                                        if (dsUserIDExists2.Tables.Count > 0)
                                        {
                                            if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                            {
                                                for (l = 1; l < 2; l++)
                                                {
                                                    m += random1.Next(0, 9).ToString();
                                                }
                                                UserID = UserID + m.ToString();
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (RegUserName != "")
                        {
                            EmailExists = 1;
                            DataSet dsUserExists = objdropdownBL.USP_ChkUserExists(RegUserName);
                            if (dsUserExists.Tables.Count > 0)
                            {
                                if (dsUserExists.Tables[0].Rows.Count > 0)
                                {
                                    Session.Timeout = 180;
                                    mdepAlertExists.Show();
                                    lblErrorExists.Visible = true;
                                    lblErrorExists.Text = "Email " + RegUserName + " already exists";
                                }
                                else
                                {
                                    DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                    if (dsUserIDExists.Tables.Count > 0)
                                    {
                                        if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                        {
                                            UserID = UserID + s.ToString();
                                            DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                            string m = "";
                                            int l;
                                            if (dsUserIDExists2.Tables.Count > 0)
                                            {
                                                if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                                {
                                                    for (l = 1; l < 2; l++)
                                                    {
                                                        m += random1.Next(0, 9).ToString();
                                                    }
                                                    UserID = UserID + m.ToString();
                                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                }
                                                else
                                                {
                                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                }
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                            }
                            else
                            {
                                DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                if (dsUserIDExists.Tables.Count > 0)
                                {
                                    if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                    {
                                        UserID = UserID + s.ToString();
                                        DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                        string m = "";
                                        int l;
                                        if (dsUserIDExists2.Tables.Count > 0)
                                        {
                                            if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                            {
                                                for (l = 1; l < 2; l++)
                                                {
                                                    m += random1.Next(0, 9).ToString();
                                                }
                                                UserID = UserID + m.ToString();
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }

                        }
                        else
                        {
                            EmailExists = 0;
                            DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                            if (dsUserIDExists.Tables.Count > 0)
                            {
                                if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                {
                                    UserID = UserID + s.ToString();
                                    DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                    string m = "";
                                    int l;
                                    if (dsUserIDExists2.Tables.Count > 0)
                                    {
                                        if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                        {
                                            for (l = 1; l < 2; l++)
                                            {
                                                m += random1.Next(0, 9).ToString();
                                            }
                                            UserID = UserID + m.ToString();
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }
                            else
                            {
                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                            }
                        }
                    }
                }
            }
            else
            {
                if (dsPhoneExists.Tables.Count > 0)
                {
                    if (dsPhoneExists.Tables[0].Rows.Count > 0)
                    {
                        mdepAlertExists.Show();
                        lblErrorExists.Visible = true;
                        lblErrorExists.Text = "Phone number " + RegPhone + " already exists";
                    }
                    else
                    {
                        if (RegUserName != "")
                        {
                            EmailExists = 1;
                            DataSet dsUserExists = objdropdownBL.USP_ChkUserExists(RegUserName);
                            if (dsUserExists.Tables.Count > 0)
                            {
                                if (dsUserExists.Tables[0].Rows.Count > 0)
                                {
                                    Session.Timeout = 180;
                                    mdepAlertExists.Show();
                                    lblErrorExists.Visible = true;
                                    lblErrorExists.Text = "Email " + RegUserName + " already exists";
                                }
                                else
                                {
                                    DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                    if (dsUserIDExists.Tables.Count > 0)
                                    {
                                        if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                        {
                                            UserID = UserID + s.ToString();
                                            DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                            string m = "";
                                            int l;
                                            if (dsUserIDExists2.Tables.Count > 0)
                                            {
                                                if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                                {
                                                    for (l = 1; l < 2; l++)
                                                    {
                                                        m += random1.Next(0, 9).ToString();
                                                    }
                                                    UserID = UserID + m.ToString();
                                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                }
                                                else
                                                {
                                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                                }
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                            }
                            else
                            {
                                DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                if (dsUserIDExists.Tables.Count > 0)
                                {
                                    if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                    {
                                        UserID = UserID + s.ToString();
                                        DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                        string m = "";
                                        int l;
                                        if (dsUserIDExists2.Tables.Count > 0)
                                        {
                                            if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                            {
                                                for (l = 1; l < 2; l++)
                                                {
                                                    m += random1.Next(0, 9).ToString();
                                                }
                                                UserID = UserID + m.ToString();
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }

                        }
                        else
                        {
                            EmailExists = 0;
                            DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                            if (dsUserIDExists.Tables.Count > 0)
                            {
                                if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                {
                                    UserID = UserID + s.ToString();
                                    DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                    string m = "";
                                    int l;
                                    if (dsUserIDExists2.Tables.Count > 0)
                                    {
                                        if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                        {
                                            for (l = 1; l < 2; l++)
                                            {
                                                m += random1.Next(0, 9).ToString();
                                            }
                                            UserID = UserID + m.ToString();
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }
                            else
                            {
                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                            }
                        }
                    }
                }
                else
                {
                    if (RegUserName != "")
                    {
                        EmailExists = 1;
                        DataSet dsUserExists = objdropdownBL.USP_ChkUserExists(RegUserName);
                        if (dsUserExists.Tables.Count > 0)
                        {
                            if (dsUserExists.Tables[0].Rows.Count > 0)
                            {
                                Session.Timeout = 180;
                                mdepAlertExists.Show();
                                lblErrorExists.Visible = true;
                                lblErrorExists.Text = "Email " + RegUserName + " already exists";
                            }
                            else
                            {
                                DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                if (dsUserIDExists.Tables.Count > 0)
                                {
                                    if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                    {
                                        UserID = UserID + s.ToString();
                                        DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                        string m = "";
                                        int l;
                                        if (dsUserIDExists2.Tables.Count > 0)
                                        {
                                            if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                            {
                                                for (l = 1; l < 2; l++)
                                                {
                                                    m += random1.Next(0, 9).ToString();
                                                }
                                                UserID = UserID + m.ToString();
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                            else
                                            {
                                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                            }
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }
                        }
                        else
                        {
                            DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                            if (dsUserIDExists.Tables.Count > 0)
                            {
                                if (dsUserIDExists.Tables[0].Rows.Count > 0)
                                {
                                    UserID = UserID + s.ToString();
                                    DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                    string m = "";
                                    int l;
                                    if (dsUserIDExists2.Tables.Count > 0)
                                    {
                                        if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                        {
                                            for (l = 1; l < 2; l++)
                                            {
                                                m += random1.Next(0, 9).ToString();
                                            }
                                            UserID = UserID + m.ToString();
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                        else
                                        {
                                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }
                            else
                            {
                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                            }
                        }

                    }
                    else
                    {
                        EmailExists = 0;
                        DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                        if (dsUserIDExists.Tables.Count > 0)
                        {
                            if (dsUserIDExists.Tables[0].Rows.Count > 0)
                            {
                                UserID = UserID + s.ToString();
                                DataSet dsUserIDExists2 = objdropdownBL.ChkUserExistsUserID(UserID);
                                string m = "";
                                int l;
                                if (dsUserIDExists2.Tables.Count > 0)
                                {
                                    if (dsUserIDExists2.Tables[0].Rows.Count > 0)
                                    {
                                        for (l = 1; l < 2; l++)
                                        {
                                            m += random1.Next(0, 9).ToString();
                                        }
                                        UserID = UserID + m.ToString();
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                    else
                                    {
                                        SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                    }
                                }
                                else
                                {
                                    SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                                }
                            }
                            else
                            {
                                SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                            }
                        }
                        else
                        {
                            SaveRegDetails(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void SaveRegDetails(string RegName, string RegUserName, string Password, string RegPhone, int PackageID, int RegState, string RegCity, string RegAddress, string RegZip, string BusinessName, string AltEmail, string RegAltPhone, int SalesAgentID, int VerifierID, int EmailExists, string UserID, string DealerID)
    {
        try
        {
            DataSet GetDealerData = Session["GetDealerData"] as DataSet;
            DateTime Saledate = Convert.ToDateTime(GetDealerData.Tables[0].Rows[0]["SaleDate"].ToString());
            string SellerName = "";
            string SellerBusinessName = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["DealerShipName"].ToString());
            string SellerPhNum = GetDealerData.Tables[0].Rows[0]["DealerContactPhone"].ToString();
            string SellerAltNum = "";
            string SellerEmail = GetDealerData.Tables[0].Rows[0]["DealerEmail"].ToString();
            string SellerAltEmail = "";
            string SellerAddress = "";
            string SellerCity = GetDealerData.Tables[0].Rows[0]["DealerCity"].ToString();
            string SellerState = GetDealerData.Tables[0].Rows[0]["State_Code"].ToString();
            string SellerZip = GetDealerData.Tables[0].Rows[0]["DealerZip"].ToString();
            string strIp;
            string strHostName = Request.UserHostAddress.ToString();
            strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            string SpecialNotes = "";
            string UserNotes = GetDealerData.Tables[0].Rows[0]["SaleNotes"].ToString();

            String UpdatedBy = Session[Constants.NAME].ToString();
            string InternalNotesNew = UserNotes;
            string UpdateByWithDate = System.DateTime.Now.ToUniversalTime().AddHours(-4).ToString() + "-" + UpdatedBy + "<br>";
            InternalNotesNew = UpdateByWithDate + InternalNotesNew.Trim() + "<br>" + "-------------------------------------------------";
            int CarsalesUserID = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["DealerUID"].ToString());
            DataSet dsUserInfo = objdropdownBL.SaveDealerRegDataForCarSales(RegName, RegUserName, Password, RegPhone, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID, DealerID,
                Saledate, SellerName, SellerPhNum, SellerAltNum, SellerEmail, SellerAltEmail, SellerAddress, SellerCity, SellerState, SellerZip, SellerBusinessName, SpecialNotes, InternalNotesNew, CarsalesUserID);
            int UID = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UID"].ToString());
            Session["DealerUID"] = UID;
            int UserPackID = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString());
            Session["DealerUserPackID"] = UserPackID;
            int SellerID = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["SellerID"].ToString());
            Session["DealerSellerID"] = SellerID;

            int PaymentType = 0;
            string Cardtype = string.Empty;

            PaymentType = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString());
            Cardtype = GetDealerData.Tables[0].Rows[0]["cardType"].ToString();
            Session["SavePaymentType"] = PaymentType;



            string VoiceRecord = GetDealerData.Tables[0].Rows[0]["VoiceRecord"].ToString();
            int VoiceFileLocation = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["VoiceFileLocation"].ToString());
            int PSStatus = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString());
            String TransactionID = String.Empty;
            int pmntStatus;
            DateTime PaymentDate = new DateTime();
            if (PSStatus == 1)
            {
                pmntStatus = 2;
            }
            else if (PSStatus == 7)
            {
                pmntStatus = 7;
            }
            else if (PSStatus == 8)
            {
                pmntStatus = 8;
            }
            else
            {
                pmntStatus = 5;
            }
            if (pmntStatus == 2)
            {
                PaymentDate = Convert.ToDateTime(GetDealerData.Tables[0].Rows[0]["SchedulePaymentDate"].ToString());
                TransactionID = GetDealerData.Tables[0].Rows[0]["TransactionID"].ToString();
            }
            else
            {
                PaymentDate = Convert.ToDateTime("1/1/1990");
                TransactionID = "";
            }
            string Amount = GetDealerData.Tables[0].Rows[0]["Amount"].ToString();

            if ((GetDealerData.Tables[0].Rows[0]["pmntType"].ToString() == "1") || (GetDealerData.Tables[0].Rows[0]["pmntType"].ToString() == "2") || (GetDealerData.Tables[0].Rows[0]["pmntType"].ToString() == "3") || (GetDealerData.Tables[0].Rows[0]["pmntType"].ToString() == "4"))
            {
                //PaymentDate = Convert.ToDateTime(ddlPaymentDate.SelectedItem.Value);
                //string Amount = txtPaymentAmountIn.Text;
                string CCCardNumber = GetDealerData.Tables[0].Rows[0]["cardNumber"].ToString();
                string CardExpDt = GetDealerData.Tables[0].Rows[0]["cardExpDt"].ToString();
                string CardholderName = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["cardholderName"].ToString());
                string CardholderLastName = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["cardholderLastName"].ToString());
                string CardCode = GetDealerData.Tables[0].Rows[0]["cardCode"].ToString();
                string BillingAdd = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["billingAdd"].ToString());
                string BillingCity = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["billingCity"].ToString());
                string BillingState = GetDealerData.Tables[0].Rows[0]["billingState"].ToString();
                string BillingZip = GetDealerData.Tables[0].Rows[0]["billingZip"].ToString();
                DataSet dspayData = objdropdownBL.SaveCreditCardInfoForDealerPaid(PaymentDate, Amount, PaymentType, pmntStatus, strIp, VoiceRecord, VoiceFileLocation, CCCardNumber,
                                    Cardtype, CardExpDt, CardholderName, CardholderLastName, CardCode, BillingZip, BillingAdd, BillingCity, BillingState, UserPackID, UID, TransactionID);
            }
            if (GetDealerData.Tables[0].Rows[0]["pmntType"].ToString() == "8")
            {
                PaymentType = 6;
                string TransID = "Invoice";
                string PayPalEmailAcc = GetDealerData.Tables[0].Rows[0]["SendInvoiceEmail"].ToString();
                DataSet dsSavePayPalInfo = objdropdownBL.SavePayPalDataForDealerPaid(PaymentDate, Amount, PaymentType, pmntStatus, strIp, VoiceRecord, VoiceFileLocation, TransID, PayPalEmailAcc, UserPackID, UID);
            }
            if (GetDealerData.Tables[0].Rows[0]["pmntType"].ToString() == "5")
            {
                int AccType = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["bankAccountType"].ToString());
                string BankRouting = GetDealerData.Tables[0].Rows[0]["bankRouting"].ToString();
                string bankName = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["bankName"].ToString());
                string AccNumber = GetDealerData.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                string AccHolderName = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
                string CheckNumber = "";
                int CheckType = Convert.ToInt32(5);
                DataSet dsSaveCheckInfo = objdropdownBL.SaveCheckDataFordealerPaid(PaymentDate, Amount, PaymentType, pmntStatus, strIp, VoiceRecord, VoiceFileLocation, AccType, BankRouting, bankName, AccNumber,
                    AccHolderName, CheckType, CheckNumber, TransactionID, UserPackID, UID);
            }
            DataSet dsUpdateSmartzStatus = objHotLeadBL.UpdateSmartzMoveStatusForDealer(1, Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString()), UID, DealerID);
            //MPEUpdate.Show();
            //Session.Timeout = 180;
            mpealteruserUpdated.Show();
            lblErrUpdated.Visible = true;
            lblErrUpdated.Text = "Customer details saved successfully<br />Dealer id: " + DealerID.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnDealerExistsOK_Click(object sender, EventArgs e)
    {
        try
        {
            mdepDealerExists.Hide();
            MdepEnterDEalerCode.Show();
            txtDealerCode.Text = "";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnClsUpdated_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerQCDealerSaleID"] = null;

            Session["DealerEditSaleID"] = null;
            Session["ViewDealerQCStatus"] = "";
            Response.Redirect("QCReportForDealer.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkDealerIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
            ds = Session["AllDealersSalesQCData"] as DataSet;
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
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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

    protected void lnkbtnPaymentStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllDealersSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PSStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPaymentStatus.Text = "Pmnt Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPaymentStatus.Text = "Pmnt Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnPaymentStatus.Text = "Pmnt Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPaymentStatus.Text = "Pmnt Status &#8659";
            }
            lnkDealerIDSort.Text = "Dealer ID &darr; &uarr;";
            lnkDealerShipName.Text = "Dlrship Name &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            //lnkTargetDateSort.Text = "Target Dt &darr; &uarr;";
            lnkPromotionSort.Text = "Promotion &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkbtnDealerStatus.Text = "Status &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnContactName.Text = "Contact Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnCitySort.Text = "City &darr; &uarr;";
            lnkbtnZipSort.Text = "Zip &darr; &uarr;";
            //lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";
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
}
