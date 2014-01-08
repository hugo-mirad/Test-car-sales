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


public partial class QCReport : System.Web.UI.Page
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
            SingleAgentSales = objHotLeadBL.GetQCDataForAll(Status, CenterID);
             Session["AllSalesQCData"] = SingleAgentSales;
            lblResHead.Text = "Recent 400 sales are showing";
            if (SingleAgentSales.Tables[0].Rows.Count > 0)
            {
                grdWarmLeadInfo.Visible = true;
                lblResCount.Visible = true;
                lblRes.Visible = false;
                lblResCount.Text = "Total " + SingleAgentSales.Tables[0].Rows.Count.ToString() + " records found";
                grdWarmLeadInfo.DataSource = SingleAgentSales.Tables[0];
                grdWarmLeadInfo.DataBind();
            }
            else
            {
                grdWarmLeadInfo.Visible = false;
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
    protected void grdWarmLeadInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnAgentID = (HiddenField)e.Row.FindControl("hdnAgentID");
                Label lblAgent = (Label)e.Row.FindControl("lblAgent");
                HiddenField hdnAgentName = (HiddenField)e.Row.FindControl("hdnAgentName");
                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnPackName");
                HiddenField hdnPackDiscount = (HiddenField)e.Row.FindControl("hdnPackDiscount");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnPhoneNum");
                Label lblQCStatus = (Label)e.Row.FindControl("lblQCStatus");
                HiddenField hdnQCStatusName = (HiddenField)e.Row.FindControl("hdnQCStatusName");
                HiddenField hdnQCStatusID = (HiddenField)e.Row.FindControl("hdnQCStatusID");
                LinkButton lnkbtnPaymentStatus = (LinkButton)e.Row.FindControl("lnkbtnPaymentStatus");
                HiddenField hdnPSID1Status = (HiddenField)e.Row.FindControl("hdnPSID1Status");
                HiddenField hdnPSID1StatusName = (HiddenField)e.Row.FindControl("hdnPSID1StatusName");
                LinkButton lnkbtnMoveSmartz = (LinkButton)e.Row.FindControl("lnkbtnMoveSmartz");
                HiddenField hdnSmartzStatus = (HiddenField)e.Row.FindControl("hdnSmartzStatus");
                LinkButton lnkCarID = (LinkButton)e.Row.FindControl("lnkCarID");
                HiddenField hdnSmartzCarID = (HiddenField)e.Row.FindControl("hdnSmartzCarID");
                HiddenField hdnSmartzMovedDate = (HiddenField)e.Row.FindControl("hdnSmartzMovedDate");
                HiddenField hdnPSAmount = (HiddenField)e.Row.FindControl("hdnPSAmount");

                Label lblName = (Label)e.Row.FindControl("lblName");
                HiddenField hdnSellerName = (HiddenField)e.Row.FindControl("hdnSellerName");
                HiddenField hdnLastName = (HiddenField)e.Row.FindControl("hdnLastName");
                HiddenField hdnAgentCenterID = (HiddenField)e.Row.FindControl("hdnAgentCenterID");

                Label lblYear = (Label)e.Row.FindControl("lblYear");
                HiddenField hdnYear = (HiddenField)e.Row.FindControl("hdnYear");
                HiddenField hdnMake = (HiddenField)e.Row.FindControl("hdnMake");
                HiddenField hdnModel = (HiddenField)e.Row.FindControl("hdnModel");

                lblYear.Text = hdnYear.Value + "/" + hdnMake.Value + "/" + hdnModel.Value;
                string TransName = string.Empty;
                if (hdnLastName.Value != "")
                {
                    TransName = hdnLastName.Value + " " + hdnSellerName.Value;
                }
                else
                {
                    TransName = hdnSellerName.Value;
                }
                if (TransName.Length > 15)
                {
                    lblName.Text = objGeneralFunc.WrapTextByMaxCharacters(TransName, 15);
                }
                else
                {
                    lblName.Text = TransName;
                }

                Double PackCost = new Double();
                PackCost = Convert.ToDouble(hdnPackCost.Value.ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = hdnPackName.Value.ToString();
                lblPackage.Text = PackName + "($" + PackAmount + "";
                if (hdnPackDiscount.Value != "0")
                {
                    if(hdnPackDiscount.Value!="")
                    lblPackage.Text += "-" + hdnPackDiscount.Value + ")";
                }
                else
                    lblPackage.Text += ")";
               
                   
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
    protected void grdWarmLeadInfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditSale")
            {
                int PostingID = Convert.ToInt32(e.CommandArgument.ToString());
                Session["AgentQCPostingID"] = PostingID;
                Response.Redirect("QCDataView.aspx");
            }
            if (e.CommandName == "EditPayInfo")
            {
                int PostingID = Convert.ToInt32(e.CommandArgument.ToString());
                DataSet Cardetais = objHotLeadBL.GetInfoByPostingIDForPayInfo(PostingID);
                Session["QCPayUpPmntTypeID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString());
                Session["QCPayUpPmntID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PaymentID"].ToString());
                lblpaymentPopSaleID.Text = Cardetais.Tables[0].Rows[0]["CarID"].ToString();
                if (Cardetais.Tables[0].Rows[0]["phone"].ToString() == "")
                {
                    lblPayInfoPhone.Text = "";
                }
                else
                {
                    lblPayInfoPhone.Text = objGeneralFunc.filPhnm(Cardetais.Tables[0].Rows[0]["phone"].ToString());
                }
                lblPayInfoVoiceConfNo.Text = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString();
                lblPayInfoEmail.Text = Cardetais.Tables[0].Rows[0]["email"].ToString();
                if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString() != "")
                {
                    DateTime PaymentDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                    lblPoplblPayDate.Text = PaymentDate.ToString("MM/dd/yyyy");
                    Session["QCPayUpPmntDate"] = PaymentDate.ToString("MM/dd/yyyy");
                }
                lblPoplblPayAmount.Text = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = Cardetais.Tables[0].Rows[0]["Description"].ToString();
                lblPoplblPackage.Text = PackName + "($" + PackAmount + ")";
                hdnPophdnAmount.Value = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();

                string OldNotes = Cardetais.Tables[0].Rows[0]["PaymentNotes"].ToString();
                OldNotes = OldNotes.Replace("<br>", Environment.NewLine);
                txtPaymentNotes.Text = OldNotes;
                txtPaymentNewNotes.Text = "";
                if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString() != "")
                {
                    DateTime PDDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString());
                    lblPDDateForPop.Text = PDDate.ToString("MM/dd/yyyy");
                    trPopPDData.Style["display"] = "block";
                    lblPDAmountForPop.Text = Cardetais.Tables[0].Rows[0]["Amount2"].ToString();
                }
                else
                {
                    lblPDDateForPop.Text = "";
                    trPopPDData.Style["display"] = "none";
                    lblPDAmountForPop.Text = "";
                }

                FillPayCancelReason();
                hdnPopPayType.Value = Cardetais.Tables[0].Rows[0]["pmntType"].ToString();
                if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                {
                    divcard.Style["display"] = "none";
                    divcheck.Style["display"] = "block";
                    divpaypal.Style["display"] = "none";
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 8))
                    {
                        divTransID.Style["display"] = "block";
                        divPaymentDate.Style["display"] = "block";
                        divPaymentAmount.Style["display"] = "block";
                        divReason.Style["display"] = "none";
                        if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString() != "")
                        {
                            DateTime PaymentDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                            //ListItem liPayDate = new ListItem();
                            //liPayDate.Text = PaymentDate.ToString("MM/dd/yyyy");
                            //liPayDate.Value = PaymentDate.ToString("MM/dd/yyyy");
                            //ddlPaymentDate.SelectedIndex = ddlPaymentDate.Items.IndexOf(liPayDate);
                            ddlPaymentDate.Items.Clear();
                            ddlPaymentDate.Items.Insert(0, new ListItem(PaymentDate.ToString("MM/dd/yyyy"), PaymentDate.ToString("MM/dd/yyyy")));
                        }

                        txtPaytransID.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                        txtPaymentAmountInPop.Text = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
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
                        if (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "2")
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
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PSStatusName1"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString();
                    ddlPaymentStatus.SelectedIndex = ddlPaymentStatus.Items.IndexOf(liPayStatus);
                    //lblCheckNumber.Text = Cardetais.Tables[0].Rows[0]["BankCheckNumber"].ToString();
                    //lblCheckType.Text = Cardetais.Tables[0].Rows[0]["CheckTypeName"].ToString();


                }
                else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 6)
                {
                    divcard.Style["display"] = "none";
                    divcheck.Style["display"] = "none";
                    divpaypal.Style["display"] = "block";
                    divTransID.Style["display"] = "none";
                    divPaymentDate.Style["display"] = "none";
                    divPaymentAmount.Style["display"] = "none";
                    lblPaypalTranID.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                    lblPaypalEmail.Text = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                    ListItem liPayStatus = new ListItem();
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PSStatusName1"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString();
                    ddlPaymentStatus.SelectedIndex = ddlPaymentStatus.Items.IndexOf(liPayStatus);
                    if (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "2")
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
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 8))
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
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString()) == 8))
                    {
                        divTransID.Style["display"] = "block";
                        divPaymentDate.Style["display"] = "block";
                        divPaymentAmount.Style["display"] = "block";
                        if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString() != "")
                        {
                            DateTime PaymentDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                            ddlPaymentDate.Items.Clear();
                            ddlPaymentDate.Items.Insert(0, new ListItem(PaymentDate.ToString("MM/dd/yyyy"), PaymentDate.ToString("MM/dd/yyyy")));
                        }
                        txtPaytransID.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                        txtPaymentAmountInPop.Text = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
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
                        if (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "2")
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
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PSStatusName1"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString();
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
                int PostingID = Convert.ToInt32(e.CommandArgument.ToString());
                Session["AgentQCMovingPostingID"] = PostingID;
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingID);
                string RegPhone = Cardetais.Tables[0].Rows[0]["phone"].ToString();
                DataSet dsPhoneExists = objdropdownBL.ChkUserExistsPhoneNumber(RegPhone);
                string Email = Cardetais.Tables[0].Rows[0]["UserName"].ToString();
                string UserID;
                string FistName = Cardetais.Tables[0].Rows[0]["LastName"].ToString();
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
                UserID = FistName + RegPhone.ToString();
                int EmailExists = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["EmailExists"].ToString());
                if (dsPhoneExists.Tables.Count > 0)
                {
                    if (dsPhoneExists.Tables[0].Rows.Count > 0)
                    {
                        string PhoneNumber = dsPhoneExists.Tables[0].Rows[0]["PhoneNumber"].ToString();
                        string CustName = dsPhoneExists.Tables[0].Rows[0]["Name"].ToString();
                        string CustEmail = dsPhoneExists.Tables[0].Rows[0]["UserName"].ToString();
                        string Address = dsPhoneExists.Tables[0].Rows[0]["Address"].ToString();
                        Session["dsExitsUserForSmartz"] = dsPhoneExists;
                        //mdepAlertExists.Show();
                        //lblErrorExists.Visible = true;
                        //lblErrorExists.Text = "Phone number " + RegPhone + " already exists<br />You cannot move it to smartz";
                        MdepAddAnotherCarAlert.Show();
                        lblAddAnotherCarAlertError.Visible = true;
                        //lblAddAnotherCarAlertError.Text = "Phone number " + RegPhone + " already exists<br />Do you want to add another car?";
                        lblAddAnotherCarAlertError.Text = "Account already exist with <br />Phone number: " + PhoneNumber + "<br />Email: " + CustEmail + "<br />Name: " + CustName + " <br />Do you want to transfer and add to the same account?";
                    }
                    else
                    {
                        if (EmailExists == 1)
                        {
                            DataSet dsUserExists = objdropdownBL.USP_ChkUserExists(Email);
                            if (dsUserExists.Tables.Count > 0)
                            {
                                if (dsUserExists.Tables[0].Rows.Count > 0)
                                {
                                    string PhoneNumber = dsUserExists.Tables[0].Rows[0]["PhoneNumber"].ToString();
                                    string CustName = dsUserExists.Tables[0].Rows[0]["Name"].ToString();
                                    string CustEmail = dsUserExists.Tables[0].Rows[0]["UserName"].ToString();
                                    string Address = dsUserExists.Tables[0].Rows[0]["Address"].ToString();
                                    Session["dsExitsUserForSmartz"] = dsUserExists;
                                    //mdepAlertExists.Show();
                                    //lblErrorExists.Visible = true;
                                    //lblErrorExists.Text = "Email " + Email + " already exists<br />You cannot move it to smartz";
                                    MdepAddAnotherCarAlert.Show();
                                    lblAddAnotherCarAlertError.Visible = true;
                                    //lblAddAnotherCarAlertError.Text = "Email " + Email + " already exists<br />Do you want to add another car?";
                                    lblAddAnotherCarAlertError.Text = "Account already exist with <br />Phone number: " + PhoneNumber + "<br />Email: " + CustEmail + "<br />Name: " + CustName + " <br />Do you want to transfer and add to the same account?";
                                }
                                else
                                {
                                    DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                    if (dsUserIDExists.Tables.Count > 0)
                                    {
                                        if (dsUserExists.Tables[0].Rows.Count > 0)
                                        {
                                            UserID = UserID + s.ToString();
                                            SaveRegData(UserID, Email, Cardetais, EmailExists);
                                        }
                                        else
                                        {
                                            SaveRegData(UserID, Email, Cardetais, EmailExists);
                                        }
                                    }
                                    else
                                    {
                                        SaveRegData(UserID, Email, Cardetais, EmailExists);
                                    }
                                }
                            }
                            else
                            {
                                DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                if (dsUserIDExists.Tables.Count > 0)
                                {
                                    if (dsUserExists.Tables[0].Rows.Count > 0)
                                    {
                                        UserID = UserID + s.ToString();
                                        SaveRegData(UserID, Email, Cardetais, EmailExists);
                                    }
                                    else
                                    {
                                        SaveRegData(UserID, Email, Cardetais, EmailExists);
                                    }
                                }
                                else
                                {
                                    SaveRegData(UserID, Email, Cardetais, EmailExists);
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
                                    SaveRegData(UserID, Email, Cardetais, EmailExists);
                                }
                                else
                                {
                                    SaveRegData(UserID, Email, Cardetais, EmailExists);
                                }
                            }
                            else
                            {
                                SaveRegData(UserID, Email, Cardetais, EmailExists);
                            }
                        }
                    }
                }
                else
                {
                    if (EmailExists == 1)
                    {
                        DataSet dsUserExists = objdropdownBL.USP_ChkUserExists(Email);
                        if (dsUserExists.Tables.Count > 0)
                        {
                            if (dsUserExists.Tables[0].Rows.Count > 0)
                            {
                                string PhoneNumber = dsUserExists.Tables[0].Rows[0]["PhoneNumber"].ToString();
                                string CustName = dsUserExists.Tables[0].Rows[0]["Name"].ToString();
                                string CustEmail = dsUserExists.Tables[0].Rows[0]["UserName"].ToString();
                                string Address = dsUserExists.Tables[0].Rows[0]["Address"].ToString();
                                Session["dsExitsUserForSmartz"] = dsUserExists;
                                //mdepAlertExists.Show();
                                //lblErrorExists.Visible = true;
                                //lblErrorExists.Text = "Email " + Email + " already exists<br />You cannot move it to smartz";
                                MdepAddAnotherCarAlert.Show();
                                lblAddAnotherCarAlertError.Visible = true;
                                lblAddAnotherCarAlertError.Text = "Account already exist with <br />Phone number: " + PhoneNumber + "<br />Email: " + CustEmail + "<br />Name: " + CustName + " <br />Do you want to transfer and add to the same account?";
                            }
                            else
                            {
                                DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                                if (dsUserIDExists.Tables.Count > 0)
                                {
                                    if (dsUserExists.Tables[0].Rows.Count > 0)
                                    {
                                        UserID = UserID + s.ToString();
                                        SaveRegData(UserID, Email, Cardetais, EmailExists);
                                    }
                                    else
                                    {
                                        SaveRegData(UserID, Email, Cardetais, EmailExists);
                                    }
                                }
                                else
                                {
                                    SaveRegData(UserID, Email, Cardetais, EmailExists);
                                }

                            }
                        }
                        else
                        {
                            DataSet dsUserIDExists = objdropdownBL.ChkUserExistsUserID(UserID);
                            if (dsUserIDExists.Tables.Count > 0)
                            {
                                if (dsUserExists.Tables[0].Rows.Count > 0)
                                {
                                    UserID = UserID + s.ToString();
                                    SaveRegData(UserID, Email, Cardetais, EmailExists);
                                }
                                else
                                {
                                    SaveRegData(UserID, Email, Cardetais, EmailExists);
                                }
                            }
                            else
                            {
                                SaveRegData(UserID, Email, Cardetais, EmailExists);
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
                                SaveRegData(UserID, Email, Cardetais, EmailExists);
                            }
                            else
                            {
                                SaveRegData(UserID, Email, Cardetais, EmailExists);
                            }
                        }
                        else
                        {
                            SaveRegData(UserID, Email, Cardetais, EmailExists);
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

    private void SaveRegData(string UserID, string Email, DataSet Cardetais, int EmailExists)
    {
        try
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 5; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            string RegName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["sellerName"].ToString()).Trim() + " " + objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["LastName"].ToString()).Trim();
            string RegUserName = Email;
            string LastName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["LastName"].ToString()).Trim();
            if (LastName.Length > 4)
            {
                LastName = LastName.Substring(0, 4);
            }
            string Password = LastName + r.ToString();
            string RegPhone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
            string CouponCode = "";
            string ReferCode = "";
            string RegAddress = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["address1"].ToString()).Trim();
            string RegCity = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["city"].ToString()).Trim();
            int RegState = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["StateID"].ToString());
            string RegZip = Cardetais.Tables[0].Rows[0]["zip"].ToString();
            string BusinessName = "";
            string AltEmail = "";
            string RegAltPhone = "";
            int SalesAgentID = 0;
            int VerifierID = Convert.ToInt32(0);
            string VerifierCenterCode = Cardetais.Tables[0].Rows[0]["SaleverifierCenterCode"].ToString();
            string CenterCode = Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString();
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
                        if (VerifierCenterCode != "")
                        {
                            if (VerifierCenterCode == "INBH")
                            {
                                VerifierID = Convert.ToInt32(56);
                            }
                            if (VerifierCenterCode == "TEST")
                            {
                                VerifierID = Convert.ToInt32(35);
                            }
                            if (VerifierID == 0)
                            {
                                DataSet dsVerifCenter = objCentralDBBL.CheckAgentExists(VerifierCenterCode);
                                if (dsVerifCenter.Tables.Count > 0)
                                {
                                    if (dsVerifCenter.Tables[0].Rows.Count > 0)
                                    {
                                        VerifierID = Convert.ToInt32(dsVerifCenter.Tables[0].Rows[0]["Sale_Agent_Id"].ToString());
                                        int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
                                        DataSet dsUserInfo = objdropdownBL.Usp_SmartzSave_RegisterLogUser(RegName, RegUserName, Password, RegPhone, CouponCode, ReferCode, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID);
                                        Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                                        Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                                        Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                                        Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                                        Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                                        Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                                        Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                                        Session["RegEmailExists"] = dsUserInfo.Tables[0].Rows[0]["EmailExists"].ToString();
                                        Session["RegLogUserID"] = dsUserInfo.Tables[0].Rows[0]["UserID"].ToString();
                                        SaveData(Cardetais);
                                        mpealteruserUpdated.Show();
                                        lblErrUpdated.Visible = true;
                                        lblErrUpdated.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
                                    }
                                    else
                                    {
                                        mdepAlertExists.Show();
                                        lblErrorExists.Visible = true;
                                        lblErrorExists.Text = "Verifier not exist with center " + CenterCode + " in smartz. <br />Sale not able to move to smartz.";
                                    }
                                }
                                else
                                {
                                    mdepAlertExists.Show();
                                    lblErrorExists.Visible = true;
                                    lblErrorExists.Text = "Verifier not exist with center " + CenterCode + " in smartz. <br />Sale not able to move to smartz.";
                                }
                            }
                            else
                            {
                                int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
                                DataSet dsUserInfo = objdropdownBL.Usp_SmartzSave_RegisterLogUser(RegName, RegUserName, Password, RegPhone, CouponCode, ReferCode, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID);
                                Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                                Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                                Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                                Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                                Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                                Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                                Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                                Session["RegEmailExists"] = dsUserInfo.Tables[0].Rows[0]["EmailExists"].ToString();
                                Session["RegLogUserID"] = dsUserInfo.Tables[0].Rows[0]["UserID"].ToString();
                                SaveData(Cardetais);
                                mpealteruserUpdated.Show();
                                lblErrUpdated.Visible = true;
                                lblErrUpdated.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
                            }
                        }
                        else
                        {
                            int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
                            DataSet dsUserInfo = objdropdownBL.Usp_SmartzSave_RegisterLogUser(RegName, RegUserName, Password, RegPhone, CouponCode, ReferCode, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID);
                            Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                            Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                            Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                            Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                            Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                            Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                            Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                            Session["RegEmailExists"] = dsUserInfo.Tables[0].Rows[0]["EmailExists"].ToString();
                            Session["RegLogUserID"] = dsUserInfo.Tables[0].Rows[0]["UserID"].ToString();
                            SaveData(Cardetais);
                            mpealteruserUpdated.Show();
                            lblErrUpdated.Visible = true;
                            lblErrUpdated.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
                        }
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
            else
            {
                if (VerifierCenterCode != "")
                {
                    if (VerifierCenterCode == "INBH")
                    {
                        VerifierID = Convert.ToInt32(56);
                    }
                    if (VerifierCenterCode == "TEST")
                    {
                        VerifierID = Convert.ToInt32(35);
                    }
                    if (VerifierID == 0)
                    {
                        DataSet dsVerifCenter = objCentralDBBL.CheckAgentExists(VerifierCenterCode);
                        if (dsVerifCenter.Tables.Count > 0)
                        {
                            if (dsVerifCenter.Tables[0].Rows.Count > 0)
                            {
                                VerifierID = Convert.ToInt32(dsVerifCenter.Tables[0].Rows[0]["Sale_Agent_Id"].ToString());
                                int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
                                DataSet dsUserInfo = objdropdownBL.Usp_SmartzSave_RegisterLogUser(RegName, RegUserName, Password, RegPhone, CouponCode, ReferCode, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID);
                                Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                                Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                                Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                                Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                                Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                                Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                                Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                                Session["RegEmailExists"] = dsUserInfo.Tables[0].Rows[0]["EmailExists"].ToString();
                                Session["RegLogUserID"] = dsUserInfo.Tables[0].Rows[0]["UserID"].ToString();
                                SaveData(Cardetais);
                                mpealteruserUpdated.Show();
                                lblErrUpdated.Visible = true;
                                lblErrUpdated.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
                            }
                            else
                            {
                                mdepAlertExists.Show();
                                lblErrorExists.Visible = true;
                                lblErrorExists.Text = "Verifier not exist with center " + CenterCode + " in smartz. <br />Sale not able to move to smartz.";
                            }
                        }
                        else
                        {
                            mdepAlertExists.Show();
                            lblErrorExists.Visible = true;
                            lblErrorExists.Text = "Verifier not exist with center " + CenterCode + " in smartz. <br />Sale not able to move to smartz.";
                        }

                    }
                    else
                    {
                        int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
                        DataSet dsUserInfo = objdropdownBL.Usp_SmartzSave_RegisterLogUser(RegName, RegUserName, Password, RegPhone, CouponCode, ReferCode, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID);
                        Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                        Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                        Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                        Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                        Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                        Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                        Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                        Session["RegEmailExists"] = dsUserInfo.Tables[0].Rows[0]["EmailExists"].ToString();
                        Session["RegLogUserID"] = dsUserInfo.Tables[0].Rows[0]["UserID"].ToString();
                        SaveData(Cardetais);
                        mpealteruserUpdated.Show();
                        lblErrUpdated.Visible = true;
                        lblErrUpdated.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
                    }
                }
                else
                {
                    int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
                    DataSet dsUserInfo = objdropdownBL.Usp_SmartzSave_RegisterLogUser(RegName, RegUserName, Password, RegPhone, CouponCode, ReferCode, PackageID, RegState, RegCity, RegAddress, RegZip, BusinessName, AltEmail, RegAltPhone, SalesAgentID, VerifierID, EmailExists, UserID);
                    Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                    Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                    Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                    Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                    Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                    Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                    Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                    Session["RegEmailExists"] = dsUserInfo.Tables[0].Rows[0]["EmailExists"].ToString();
                    Session["RegLogUserID"] = dsUserInfo.Tables[0].Rows[0]["UserID"].ToString();
                    SaveData(Cardetais);
                    mpealteruserUpdated.Show();
                    lblErrUpdated.Visible = true;
                    lblErrUpdated.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SaveData(DataSet Cardetais)
    {
        try
        {
            int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
            string strIp;
            string strHostName = Request.UserHostAddress.ToString();
            strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            int YearOfMake = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString());
            Session["SelYear"] = Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString();
            Session["SelMake"] = Cardetais.Tables[0].Rows[0]["make"].ToString();
            Session["SelModel"] = Cardetais.Tables[0].Rows[0]["model"].ToString();
            int MakeModelID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["makeModelID"].ToString());
            int BodyTypeID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["bodyTypeID"].ToString());
            int CarID;
            string Price = Cardetais.Tables[0].Rows[0]["Carprice"].ToString();
            string Mileage = Cardetais.Tables[0].Rows[0]["mileage"].ToString();

            string ExteriorColor = Cardetais.Tables[0].Rows[0]["exteriorColor"].ToString();
            string InteriorColor = Cardetais.Tables[0].Rows[0]["interiorColor"].ToString();
            string Transmission = Cardetais.Tables[0].Rows[0]["Transmission"].ToString();
            string NumberOfDoors = Cardetais.Tables[0].Rows[0]["numberOfDoors"].ToString();
            string DriveTrain = Cardetais.Tables[0].Rows[0]["DriveTrain"].ToString();
            string VIN = Cardetais.Tables[0].Rows[0]["VIN"].ToString();
            string NumberOfCylinder = Cardetais.Tables[0].Rows[0]["numberOfCylinder"].ToString();
            int FuelTypeID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["fuelTypeID"].ToString());
            int ConditionID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["vehicleConditionID"].ToString());
            string SellerZip = string.Empty;
            //if (txtZip.Text.Length == 4)
            //{
            //    SellerZip = "0" + txtZip.Text;
            //}
            //else
            //{
            SellerZip = Cardetais.Tables[0].Rows[0]["zip"].ToString();
            //}
            string SellCity = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["city"].ToString());
            int SellStateID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["StateID"].ToString());
            string Condition = string.Empty;
            string Description = string.Empty;
            Description = Cardetais.Tables[0].Rows[0]["Cardescription"].ToString();
            Condition = Cardetais.Tables[0].Rows[0]["ConditionDescription"].ToString();
            String UpdatedBy = Session[Constants.NAME].ToString();
            DataSet dsDatetimeNw = objHotLeadBL.GetDatetime();
            DateTime dtNowNw = Convert.ToDateTime(dsDatetimeNw.Tables[0].Rows[0]["Datetime"].ToString());
            string InternalNotesNew = string.Empty;
            if (Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString() != "")
            {
                InternalNotesNew = Cardetais.Tables[0].Rows[0]["SaleNotes"].ToString().Trim() + "<br>Sale moved by " + UpdatedBy + " at " + dtNowNw.ToString("MM/dd/yyyy hh:mm tt") + "<br>Sale agent name " + Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString() + "<br>Verifier agent name " + Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString();
            }
            else
            {
                InternalNotesNew = Cardetais.Tables[0].Rows[0]["SaleNotes"].ToString().Trim() + "<br>Sale moved by " + UpdatedBy + " at " + dtNowNw.ToString("MM/dd/yyyy hh:mm tt") + "<br>Sale agent name " + Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString();
            }
            string Title = "";
            //DataSet dsCarsDetails = objdropdownBL.USP_SmartzSaveCarDetails(YearOfMake, MakeModelID, BodyTypeID, ConditionID, Price, Mileage, ExteriorColor, Transmission, InteriorColor, NumberOfDoors, VIN, NumberOfCylinder, FuelTypeID, SellerZip, SellCity, SellStateID, DriveTrain, Description, Condition, InternalNotesNew, Title);
            //Session["CarID"] = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0]["CarID"].ToString());
            //Session["UniqueID"] = dsCarsDetails.Tables[0].Rows[0]["CarUniqueID"].ToString();
            //CarID = Convert.ToInt32(Session["CarID"].ToString());
            int RegUID = Convert.ToInt32(Session["RegUSER_ID"].ToString());
            int FeatureID;
            int IsactiveFea;
            string SellerName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["sellerName"].ToString()).Trim() + " " + objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["LastName"].ToString()).Trim();
            string Address1 = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["address1"].ToString()).Trim();
            string CustPhone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
            string AltCustPhone = "";
            string CustState = Cardetais.Tables[0].Rows[0]["state"].ToString();
            string CustEmail = Cardetais.Tables[0].Rows[0]["email"].ToString();
            DateTime SaleDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SaleDate"].ToString());
            int SaleEnteredBy;
            string CenterCode = Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString();
            if (CenterCode == "INBH")
            {
                SaleEnteredBy = Convert.ToInt32(56);
            }
            else if (CenterCode == "TEST")
            {
                SaleEnteredBy = Convert.ToInt32(35);
            }
            else
            {
                DataSet dsCenter = objCentralDBBL.CheckAgentExists(CenterCode);
                if (dsCenter.Tables.Count > 0)
                {
                    if (dsCenter.Tables[0].Rows.Count > 0)
                    {
                        SaleEnteredBy = Convert.ToInt32(dsCenter.Tables[0].Rows[0]["Sale_Agent_Id"].ToString());
                    }
                    else
                    {
                        SaleEnteredBy = Convert.ToInt32(35);
                    }
                }
                else
                {
                    SaleEnteredBy = Convert.ToInt32(35);
                }
            }
            int SourceOfPhotos = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["SourceOfPhotosID"].ToString());
            Session["SourceOfPhotos"] = SourceOfPhotos;
            int SourceOfDescription = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["SourceOfDescriptionID"].ToString());
            Session["SourceOfDescription"] = SourceOfDescription;
            DataSet dsPosting = new DataSet();
            Session["CarSellerZip"] = SellerZip;
            int CarsalesID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["CarID"].ToString());
            dsPosting = objdropdownBL.USP_SmartzSaveCarDetailsFromCarSales(YearOfMake, MakeModelID, BodyTypeID, ConditionID, Price, Mileage, ExteriorColor, Transmission, InteriorColor, NumberOfDoors, VIN, NumberOfCylinder, FuelTypeID, SellerZip, SellCity, SellStateID, DriveTrain, Description, Condition, InternalNotesNew, Title, SellerName, Address1, CustState, CustPhone, AltCustPhone, CustEmail, RegUID, PackageID, SaleDate, SaleEnteredBy, strIp, SourceOfPhotos, SourceOfDescription, CarsalesID);
            Session["PostingID"] = Convert.ToInt32(dsPosting.Tables[0].Rows[0]["PostingID"].ToString());
            Session["CarID"] = Convert.ToInt32(dsPosting.Tables[0].Rows[0]["CarID"].ToString());
            Session["UniqueID"] = dsPosting.Tables[0].Rows[0]["CarUniqueID"].ToString();
            CarID = Convert.ToInt32(Session["CarID"].ToString());

            int PSStatus = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString());
            int PmntStatus;
            if (PSStatus == 1)
            {
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PaidNowAmount = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();

                PmntStatus = 2;
                //if (PackAmount != PaidNowAmount)
                //{
                //    PmntStatus = 5;
                //}
                //else
                //{
                //}
            }
            else if (PSStatus == 7)
            {
                PmntStatus = 7;
            }
            else if (PSStatus == 8)
            {
                PmntStatus = 8;
            }
            else
            {
                PmntStatus = 5;
            }
            Session["NewUserPayStatus"] = PmntStatus;
            //Session["NewUserPDDate"] = 0;
            int PmntType;
            string TransactionID;
            int AdActive;
            int UceStatus;
            int MultisiteStatus;
            string PayAmount;
            int ListingStatus;
            DateTime PDDate;
            int UserPackID = Convert.ToInt32(Session["RegUserPackID"].ToString());
            int PostingID = Convert.ToInt32(Session["PostingID"].ToString());
            string VoiceFileName = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString();
            int VoiceFileLocation = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["VoiceFileLocation"].ToString());
            PmntType = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString());
            TransactionID = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
            AdActive = Convert.ToInt32(1);
            PayAmount = Cardetais.Tables[0].Rows[0]["Amount"].ToString();
            string PendingAmount = Cardetais.Tables[0].Rows[0]["Amount2"].ToString();
            ListingStatus = 1;
            UceStatus = Convert.ToInt32(1);
            MultisiteStatus = Convert.ToInt32(1);

            if (PackageID != 1)
            {
                DateTime Paymentdate;
                if (PmntStatus == 5)
                {
                    AdActive = Convert.ToInt32(0);
                    PayAmount = Cardetais.Tables[0].Rows[0]["Amount"].ToString();
                    ListingStatus = 2;
                    UceStatus = Convert.ToInt32(0);
                    MultisiteStatus = Convert.ToInt32(0);
                    Paymentdate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                    if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString() != "")
                    {
                        PDDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString());
                    }
                    else
                    {
                        PDDate = Paymentdate;
                    }
                }
                else
                {
                    if (PmntStatus == 2)
                    {
                        AdActive = Convert.ToInt32(1);
                        ListingStatus = 1;
                        UceStatus = Convert.ToInt32(1);
                        MultisiteStatus = Convert.ToInt32(1);
                    }
                    else
                    {
                        if (PmntStatus == 7)
                        {
                            if (Convert.ToDouble(PayAmount) >= Convert.ToDouble("25.00"))
                            {
                                AdActive = Convert.ToInt32(1);
                                ListingStatus = 1;
                                UceStatus = Convert.ToInt32(1);
                                MultisiteStatus = Convert.ToInt32(1);
                            }
                            else
                            {
                                AdActive = Convert.ToInt32(0);
                                ListingStatus = 2;
                                UceStatus = Convert.ToInt32(0);
                                MultisiteStatus = Convert.ToInt32(0);
                            }
                        }
                        else
                        {
                            AdActive = Convert.ToInt32(0);
                            ListingStatus = 2;
                            UceStatus = Convert.ToInt32(0);
                            MultisiteStatus = Convert.ToInt32(0);
                        }
                    }
                    Paymentdate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                    if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString() != "")
                    {
                        PDDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString());
                    }
                    else
                    {
                        PDDate = Convert.ToDateTime("1/1/1990");
                    }
                }
                Session["NewUserPDDate"] = PDDate;
                string CCCardNumber = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
                string Cardtype = Cardetais.Tables[0].Rows[0]["Cardtype"].ToString();
                string CardExpDt = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
                string CardholderName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderName"].ToString());
                string CardholderLastName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString());
                string CardCode = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();
                string BillingAdd = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString());
                string BillingCity = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString());
                string BillingState = Cardetais.Tables[0].Rows[0]["billingState"].ToString();
                string BillingZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
                string BillingPhone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
                int AccType;
                if (Cardetais.Tables[0].Rows[0]["bankAccountType"].ToString() != "")
                {
                    AccType = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["bankAccountType"].ToString());
                }
                else
                {
                    AccType = 0;
                }
                string BankRouting = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
                string bankName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankName"].ToString());
                string AccNumber = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                string AccHolderName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
                string CheckNumber = "";
                int CheckType = Convert.ToInt32(5);
                string PayPalEmailAcc = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                bool bnewPay = objdropdownBL.USP_SmartzSavePmntDetailsForCarSales(PmntType, PmntStatus, TransactionID, strIp, RegUID, AdActive, PayAmount, Paymentdate, ListingStatus, PDDate, UserPackID, PostingID, VoiceFileName, UceStatus, MultisiteStatus, VoiceFileLocation, PendingAmount, CCCardNumber,
                                Cardtype, CardExpDt, CardholderName, CardholderLastName, CardCode, CardholderName, BillingPhone, BillingAdd, BillingCity, BillingState, BillingZip,
                                PayPalEmailAcc, CheckType, CheckNumber, AccType, BankRouting, bankName, AccNumber, AccHolderName);
            }
            else
            {
                bool bnewPay = objdropdownBL.USP_SmartzSavePmntDetailsForFreePackage(RegUID, AdActive, ListingStatus, UserPackID, PostingID, UceStatus, MultisiteStatus);
            }
            DataSet dsUpdateSmartzStatus = objHotLeadBL.UpdateSmartzMoveStatus(1, Convert.ToInt32(Session["AgentQCMovingPostingID"].ToString()), CarID);
            for (int i = 1; i < 52; i++)
            {
                if (Cardetais.Tables[1].Rows[i - 1]["Isactive"].ToString() == "True")
                {
                    IsactiveFea = 1;
                }
                else
                {
                    IsactiveFea = 0;
                }
                FeatureID = i;
                bool dsCarFeature = objdropdownBL.USP_SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            int UID;
            UID = 15;
            if (Session["CarSellerZip"].ToString() != "")
            {
                string SellerZipTick = Session["CarSellerZip"].ToString();
                DataSet dsZipExists = objdropdownBL.SmartzCheckZipExists(SellerZipTick);
                if (dsZipExists.Tables[0].Rows[0]["Result"].ToString() != "Yes")
                {
                    int CallType = Convert.ToInt32(8);
                    int CallReason = Convert.ToInt32(4);
                    int CallResolution = Convert.ToInt32(8);
                    string SpokeWith = "Internal Ticket";
                    string Notes = "Internal ticket for zip " + SellerZipTick.ToString() + " is not exists";
                    int TicketType = Convert.ToInt32(3);
                    int Priority = Convert.ToInt32(2);
                    int CallBackID = Convert.ToInt32(1);
                    string TicketDescription = "Internal ticket for zip " + SellerZipTick.ToString() + " is not exists";
                    bool bnew = objdropdownBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                }

            }
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            if ((Session["SourceOfPhotos"].ToString() == "2") || (Session["SourceOfPhotos"].ToString() == "3"))
            {
                int CallType = Convert.ToInt32(8);
                int CallReason = Convert.ToInt32(4);
                int CallResolution = Convert.ToInt32(8);
                string SpokeWith = "Internal Ticket";
                string Notes = string.Empty;
                if (Session["SourceOfPhotos"].ToString() == "2")
                {
                    Notes = "Get photos from craigslist";
                }
                else
                {
                    Notes = "Use stock photos";
                }
                int TicketType = Convert.ToInt32(3);
                int Priority = Convert.ToInt32(2);
                int CallBackID = Convert.ToInt32(1);
                string TicketDescription = Notes;
                bool bnew = objdropdownBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                string CenterCodeph = Session[Constants.CenterCode].ToString();
                UID = 15;
                String UpdatedByph = Session[Constants.NAME].ToString();
                string InternalNotesNewPh = Notes;
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedByph + "<br>";
                //InternalNotesNewPh = UpdateByWithDate + InternalNotesNewPh.Trim() + "<br>" + "-------------------------------------------------";
                InternalNotesNewPh = InternalNotesNewPh.Trim();
                DataSet dsNewIntNotes = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID, InternalNotesNewPh, UID);
            }
            if ((Session["SourceOfDescription"].ToString() == "2") || (Session["SourceOfDescription"].ToString() == "3"))
            {
                int CallType = Convert.ToInt32(8);
                int CallReason = Convert.ToInt32(4);
                int CallResolution = Convert.ToInt32(8);
                string SpokeWith = "Internal Ticket";
                string Notes = string.Empty;
                if (Session["SourceOfDescription"].ToString() == "2")
                {
                    Notes = "Get description from craigslist";
                }
                else
                {
                    Notes = "Use stock description";
                }
                int TicketType = Convert.ToInt32(3);
                int Priority = Convert.ToInt32(2);
                int CallBackID = Convert.ToInt32(1);
                string TicketDescription = Notes;
                bool bnew = objdropdownBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                string CenterCodeDesc = Session[Constants.CenterCode].ToString();
                UID = 15;
                String UpdatedByDesc = Session[Constants.NAME].ToString();
                string InternalNotesNewDesc = Notes;
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedByDesc + "<br>";
                //InternalNotesNewDesc = UpdateByWithDate + InternalNotesNewDesc.Trim() + "<br>" + "-------------------------------------------------";
                InternalNotesNewDesc = InternalNotesNewDesc.Trim();
                DataSet dsNewIntNotes = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID, InternalNotesNewDesc, UID);
            }
            int CarID1 = Convert.ToInt32(Session["CarID"].ToString());
            int UID1;
            string CenterCode1 = Session[Constants.CenterCode].ToString();
            UID1 = 15;
            string InternalNotesNew1 = string.Empty;
            InternalNotesNew1 = "-------------------------------------------------";
            DataSet dsNewIntNotes1 = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID1, InternalNotesNew1, UID1);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlPaymentStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int PmntType = Convert.ToInt32(Session["QCPayUpPmntTypeID"].ToString());
            int PmntStatus = Convert.ToInt32(ddlPaymentStatus.SelectedItem.Value);
            if (PmntType == 6)
            {
                divTransID.Style["display"] = "none";
                divPaymentDate.Style["display"] = "none";
                divPaymentAmount.Style["display"] = "none";
            }
            else
            {
                if ((PmntStatus == 1) || (PmntStatus == 7) || (PmntStatus == 8))
                {
                    divTransID.Style["display"] = "block";
                    divPaymentDate.Style["display"] = "block";
                    divPaymentAmount.Style["display"] = "block";
                    divReason.Style["display"] = "none";
                }
                else
                {
                    divTransID.Style["display"] = "none";
                    divPaymentDate.Style["display"] = "none";
                    divPaymentAmount.Style["display"] = "none";
                    if (PmntStatus == 2)
                    {
                        divReason.Style["display"] = "block";
                    }
                    else
                    {
                        divReason.Style["display"] = "none";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnYesUpdated_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["RegEmailExists"] != null)
            {
                if (Session["RegEmailExists"].ToString() != "")
                {
                    if (Session["RegEmailExists"].ToString() == "1")
                    {
                        ResendRegMail();
                        DataSet dsDatetime = objHotLeadBL.GetDatetime();
                        DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
                        int CarID = Convert.ToInt32(Session["CarID"].ToString());
                        int UID;
                        string CenterCode = Session[Constants.CenterCode].ToString();
                        UID = 15;
                        String UpdatedBy = Session[Constants.NAME].ToString();
                        string InternalNotesNew = "Welcome mail sent at " + dtNow.ToString() + " from carsales";
                        string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "<br>";
                        InternalNotesNew = UpdateByWithDate + InternalNotesNew.Trim() + "<br>" + "-------------------------------------------------";
                        DataSet dsNewIntNotes = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID, InternalNotesNew, UID);
                    }
                }
            }
            Response.Redirect("QCReport.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ResendRegMail()
    {
        try
        {
            string PDDate = string.Empty;
            string LoginPassword = Session["RegPassword"].ToString();
            string LoginName = Session["RegUserName"].ToString();
            string UserDisName = Session["RegName"].ToString();
            string RegLogUserID = Session["RegLogUserID"].ToString();

            string Year = Session["SelYear"].ToString();
            string Model = Session["SelModel"].ToString();
            string Make = Session["SelMake"].ToString();
            string UniqueID = Session["UniqueID"].ToString();
            Make = Make.Replace(" ", "%20");
            Model = Model.Replace(" ", "%20");
            Model = Model.Replace("&", "@");
            string Link = "http://unitedcarexchange.com/Buy-Sell-UsedCar/" + Year + "-" + Make + "-" + Model + "-" + UniqueID;
            string TermsLink = "http://unitedcarexchange.com/TermsandConditions.aspx";
            clsMailFormats format = new clsMailFormats();
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("info@unitedcarexchange.com");
            msg.To.Add(LoginName);
            msg.Bcc.Add("archive@unitedcarexchange.com");
            msg.Subject = "Registration Details From United Car Exchange For Car ID# " + Session["CarID"].ToString();
            msg.IsBodyHtml = true;
            string text = string.Empty;
            if (Session["NewUserPayStatus"] != null)
            {
                if (Session["NewUserPayStatus"].ToString() != "")
                {
                    if (Session["NewUserPayStatus"].ToString() == "5")
                    {
                        DateTime PostDate = Convert.ToDateTime(Session["NewUserPDDate"].ToString());
                        PDDate = PostDate.ToString("MM/dd/yyyy");
                        text = format.SendRegistrationdetailsForPDSales(RegLogUserID, LoginPassword, UserDisName, ref text, PDDate);
                    }
                }
            }
            else
            {
                text = format.SendRegistrationdetails(RegLogUserID, LoginPassword, UserDisName, ref text, Link, TermsLink);
            }
            msg.Body = text.ToString();
            SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.Credentials = new System.Net.NetworkCredential("satheesh.aakula@gmail.com", "hugomirad");
            //smtp.EnableSsl = true;
            //smtp.Send(msg);
            smtp.Host = "127.0.0.1";
            smtp.Port = 25;
            smtp.Send(msg);
        }
        catch (Exception ex)
        {
            //throw ex;
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            int CarID = Convert.ToInt32(Session["CarID"].ToString());
            int UID;
            string CenterCode = Session[Constants.CenterCode].ToString();
            UID = 15;
            String UpdatedBy = Session[Constants.NAME].ToString();
            string InternalNotesNew = "welcome email could not be sent from carsales";
            string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "<br>";
            InternalNotesNew = UpdateByWithDate + InternalNotesNew.Trim() + "<br>" + "-------------------------------------------------";
            DataSet dsNewIntNotes = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID, InternalNotesNew, UID);
            Response.Redirect("EmailServerError.aspx");
        }
    }

    protected void lnkCarIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "carid";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkCarIDSort.Text = "Sale ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkCarIDSort.Text = "Sale ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkCarIDSort.Text = "Sale ID &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkCarIDSort.Text = "Sale ID &#8659";
            }
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
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
            ds =  Session["AllSalesQCData"] as DataSet;
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
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
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
            ds =  Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleAgent";
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
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkYearSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds =  Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "yearOfMake";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkYearSort.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkYearSort.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkYearSort.Text = "Year/Make/Model &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkYearSort.Text = "Year/Make/Model &#8659";
            }
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
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
            ds =  Session["AllSalesQCData"] as DataSet;
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
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkNameSort.Text = "Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkNameSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds =  Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "sellerName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkNameSort.Text = "Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkNameSort.Text = "Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkNameSort.Text = "Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkNameSort.Text = "Name &#8659";
            }
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
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
            ds =  Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PhoneNum";
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
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Name &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
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
            ds =  Session["AllSalesQCData"] as DataSet;
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
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnPaymentStatus.Text = "Pmnt Status &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
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
            DataTable dt = new DataTable();
            try
            {
                ds =  Session["AllSalesQCData"] as DataSet;
                ds.Tables[0].DefaultView.RowFilter = "";
                dt = ds.Tables[0];
            }
            catch { }
        

            string SortExp = "PSStatusName1";
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
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";


            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdWarmLeadInfo, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnQCOpen_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string Status = "QCOpen";
            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            GetResults(Status, CenterID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
      
    }
    protected void rdbtnQCDonepayopen_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string Status = "QCDonePayOpen";
            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            GetResults(Status, CenterID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
    }
    protected void rdbtnAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string Status = "All";
            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            GetResults(Status, CenterID);
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
            //btnUpdate.Enabled = false;
            //btnUpdate.BackColor = System.Drawing.Color.Black;
            int pmntID = Convert.ToInt32(Session["QCPayUpPmntID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int PSStatusID = Convert.ToInt32(ddlPaymentStatus.SelectedItem.Value);
            int PmntStatus = 0;
            if (PSStatusID == 1)
            {
                PmntStatus = 2;
            }
            else if (PSStatusID == 2)
            {
                PmntStatus = 6;
            }
            else
            {
                PmntStatus = 1;
            }
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
            DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatus(pmntID, PSStatusID, PmntStatus, TransactionID, dtPayDate, PayCancelReason, Amount, UID, PaymentNotes);
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
      
        //UpdateProgress1.Visible = false;
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "$('#UpdateProgress1').hide();", true);

       
    }
    protected void btnAddAnotherCarYes_Click(object sender, EventArgs e)
    {
        try
        {
            int PostingID = Convert.ToInt32(Session["AgentQCMovingPostingID"].ToString());
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingID);
            SaveDataForMultiCar(Cardetais);
            MdepAddAnotherCarAlert.Hide();
            mpealteruserUpdated.Show();
            lblErrUpdated.Visible = true;
            lblErrUpdated.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
        }
        catch (Exception ex)
        {

        }
    }

    private void SaveDataForMultiCar(DataSet Cardetais)
    {
        try
        {
            DataSet dsUserInfo = Session["dsExitsUserForSmartz"] as DataSet;
            Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
            Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
            Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
            Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
            //Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
            Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
            //Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
            Session["RegEmailExists"] = dsUserInfo.Tables[0].Rows[0]["EmailExists"].ToString();
            Session["RegLogUserID"] = dsUserInfo.Tables[0].Rows[0]["UserID"].ToString();
            int PackageID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["packageID"].ToString());
            Session["PackageID"] = PackageID;
            string strIp;
            string strHostName = Request.UserHostAddress.ToString();
            strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            int YearOfMake = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString());
            Session["SelYear"] = Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString();
            Session["SelMake"] = Cardetais.Tables[0].Rows[0]["make"].ToString();
            Session["SelModel"] = Cardetais.Tables[0].Rows[0]["model"].ToString();
            int MakeModelID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["makeModelID"].ToString());
            int BodyTypeID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["bodyTypeID"].ToString());
            int CarID;
            string Price = Cardetais.Tables[0].Rows[0]["Carprice"].ToString();
            string Mileage = Cardetais.Tables[0].Rows[0]["mileage"].ToString();

            string ExteriorColor = Cardetais.Tables[0].Rows[0]["exteriorColor"].ToString();
            string InteriorColor = Cardetais.Tables[0].Rows[0]["interiorColor"].ToString();
            string Transmission = Cardetais.Tables[0].Rows[0]["Transmission"].ToString();
            string NumberOfDoors = Cardetais.Tables[0].Rows[0]["numberOfDoors"].ToString();
            string DriveTrain = Cardetais.Tables[0].Rows[0]["DriveTrain"].ToString();
            string VIN = Cardetais.Tables[0].Rows[0]["VIN"].ToString();
            string NumberOfCylinder = Cardetais.Tables[0].Rows[0]["numberOfCylinder"].ToString();
            int FuelTypeID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["fuelTypeID"].ToString());
            int ConditionID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["vehicleConditionID"].ToString());
            string SellerZip = string.Empty;
            //if (txtZip.Text.Length == 4)
            //{
            //    SellerZip = "0" + txtZip.Text;
            //}
            //else
            //{
            SellerZip = Cardetais.Tables[0].Rows[0]["zip"].ToString();
            //}
            string SellCity = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["city"].ToString());
            int SellStateID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["StateID"].ToString());
            string Condition = string.Empty;
            string Description = string.Empty;
            Description = Cardetais.Tables[0].Rows[0]["Cardescription"].ToString();
            Condition = Cardetais.Tables[0].Rows[0]["ConditionDescription"].ToString();
            String UpdatedBy = Session[Constants.NAME].ToString();
            DataSet dsDatetimeNw = objHotLeadBL.GetDatetime();
            DateTime dtNowNw = Convert.ToDateTime(dsDatetimeNw.Tables[0].Rows[0]["Datetime"].ToString());
            string InternalNotesNew = string.Empty;
            if (Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString() != "")
            {
                InternalNotesNew = Cardetais.Tables[0].Rows[0]["SaleNotes"].ToString().Trim() + "<br>Sale moved by " + UpdatedBy + " at " + dtNowNw.ToString("MM/dd/yyyy hh:mm tt") + "<br>Sale agent name " + Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString() + "<br>Verifier agent name " + Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString();
            }
            else
            {
                InternalNotesNew = Cardetais.Tables[0].Rows[0]["SaleNotes"].ToString().Trim() + "<br>Sale moved by " + UpdatedBy + " at " + dtNowNw.ToString("MM/dd/yyyy hh:mm tt") + "<br>Sale agent name " + Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString();
            }
            string Title = "";
            //DataSet dsCarsDetails = objdropdownBL.USP_SmartzSaveCarDetails(YearOfMake, MakeModelID, BodyTypeID, ConditionID, Price, Mileage, ExteriorColor, Transmission, InteriorColor, NumberOfDoors, VIN, NumberOfCylinder, FuelTypeID, SellerZip, SellCity, SellStateID, DriveTrain, Description, Condition, InternalNotesNew, Title);
            //Session["CarID"] = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0]["CarID"].ToString());
            //Session["UniqueID"] = dsCarsDetails.Tables[0].Rows[0]["CarUniqueID"].ToString();
            //CarID = Convert.ToInt32(Session["CarID"].ToString());
            int RegUID = Convert.ToInt32(Session["RegUSER_ID"].ToString());
            int FeatureID;
            int IsactiveFea;
            string SellerName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["sellerName"].ToString()).Trim() + " " + objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["LastName"].ToString()).Trim();
            string Address1 = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["address1"].ToString()).Trim();
            string CustPhone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
            string AltCustPhone = "";
            string CustState = Cardetais.Tables[0].Rows[0]["state"].ToString();
            string CustEmail = Cardetais.Tables[0].Rows[0]["email"].ToString();
            DateTime SaleDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SaleDate"].ToString());
            int SaleEnteredBy;
            string CenterCode = Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString();
            if (CenterCode == "INBH")
            {
                SaleEnteredBy = Convert.ToInt32(56);
            }
            else if (CenterCode == "TEST")
            {
                SaleEnteredBy = Convert.ToInt32(35);
            }
            else
            {
                DataSet dsCenter = objCentralDBBL.CheckAgentExists(CenterCode);
                if (dsCenter.Tables.Count > 0)
                {
                    if (dsCenter.Tables[0].Rows.Count > 0)
                    {
                        SaleEnteredBy = Convert.ToInt32(dsCenter.Tables[0].Rows[0]["Sale_Agent_Id"].ToString());
                    }
                    else
                    {
                        SaleEnteredBy = Convert.ToInt32(35);
                    }
                }
                else
                {
                    SaleEnteredBy = Convert.ToInt32(35);
                }
            }
            int SourceOfPhotos = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["SourceOfPhotosID"].ToString());
            Session["SourceOfPhotos"] = SourceOfPhotos;
            int SourceOfDescription = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["SourceOfDescriptionID"].ToString());
            Session["SourceOfDescription"] = SourceOfDescription;
            DataSet dsPosting = new DataSet();
            Session["CarSellerZip"] = SellerZip;
            int CarsalesID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["CarID"].ToString());
            dsPosting = objdropdownBL.SmartzSaveAnotherCarDetailsFromCarSales(YearOfMake, MakeModelID, BodyTypeID, ConditionID, Price, Mileage, ExteriorColor, Transmission, InteriorColor, NumberOfDoors, VIN, NumberOfCylinder, FuelTypeID, SellerZip, SellCity, SellStateID, DriveTrain, Description, Condition, InternalNotesNew, Title, SellerName, Address1, CustState, CustPhone, AltCustPhone, CustEmail, RegUID, PackageID, SaleDate, SaleEnteredBy, strIp, SourceOfPhotos, SourceOfDescription, CarsalesID);
            Session["PostingID"] = Convert.ToInt32(dsPosting.Tables[0].Rows[0]["PostingID"].ToString());
            Session["CarID"] = Convert.ToInt32(dsPosting.Tables[0].Rows[0]["CarID"].ToString());
            Session["UniqueID"] = dsPosting.Tables[0].Rows[0]["CarUniqueID"].ToString();
            Session["RegUserPackID"] = dsPosting.Tables[0].Rows[0]["UserPackID"].ToString();
            CarID = Convert.ToInt32(Session["CarID"].ToString());

            int PSStatus = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString());
            int PmntStatus;
            if (PSStatus == 1)
            {
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PaidNowAmount = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();

                PmntStatus = 2;
                //if (PackAmount != PaidNowAmount)
                //{
                //    PmntStatus = 5;
                //}
                //else
                //{
                //}
            }
            else if (PSStatus == 7)
            {
                PmntStatus = 7;
            }
            else if (PSStatus == 8)
            {
                PmntStatus = 8;
            }
            else
            {
                PmntStatus = 5;
            }
            Session["NewUserPayStatus"] = PmntStatus;
            //Session["NewUserPDDate"] = 0;
            int PmntType;
            string TransactionID;
            int AdActive;
            int UceStatus;
            int MultisiteStatus;
            string PayAmount;
            int ListingStatus;
            DateTime PDDate;
            int UserPackID = Convert.ToInt32(Session["RegUserPackID"].ToString());
            int PostingID = Convert.ToInt32(Session["PostingID"].ToString());
            string VoiceFileName = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString();
            int VoiceFileLocation = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["VoiceFileLocation"].ToString());
            PmntType = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString());
            TransactionID = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
            AdActive = Convert.ToInt32(1);
            PayAmount = Cardetais.Tables[0].Rows[0]["Amount"].ToString();
            string PendingAmount = Cardetais.Tables[0].Rows[0]["Amount2"].ToString();
            ListingStatus = 1;
            UceStatus = Convert.ToInt32(1);
            MultisiteStatus = Convert.ToInt32(1);

            if (PackageID != 1)
            {
                DateTime Paymentdate;
                if (PmntStatus == 5)
                {
                    AdActive = Convert.ToInt32(0);
                    PayAmount = Cardetais.Tables[0].Rows[0]["Amount"].ToString();
                    ListingStatus = 2;
                    UceStatus = Convert.ToInt32(0);
                    MultisiteStatus = Convert.ToInt32(0);
                    Paymentdate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                    if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString() != "")
                    {
                        PDDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString());
                    }
                    else
                    {
                        PDDate = Paymentdate;
                    }
                }
                else
                {
                    if (PmntStatus == 2)
                    {
                        AdActive = Convert.ToInt32(1);
                        ListingStatus = 1;
                        UceStatus = Convert.ToInt32(1);
                        MultisiteStatus = Convert.ToInt32(1);
                    }
                    else
                    {
                        if (PmntStatus == 7)
                        {
                            if (Convert.ToDouble(PayAmount) >= Convert.ToDouble("25.00"))
                            {
                                AdActive = Convert.ToInt32(1);
                                ListingStatus = 1;
                                UceStatus = Convert.ToInt32(1);
                                MultisiteStatus = Convert.ToInt32(1);
                            }
                            else
                            {
                                AdActive = Convert.ToInt32(0);
                                ListingStatus = 2;
                                UceStatus = Convert.ToInt32(0);
                                MultisiteStatus = Convert.ToInt32(0);
                            }
                        }
                        else
                        {
                            AdActive = Convert.ToInt32(0);
                            ListingStatus = 2;
                            UceStatus = Convert.ToInt32(0);
                            MultisiteStatus = Convert.ToInt32(0);
                        }
                    }
                    Paymentdate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                    if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString() != "")
                    {
                        PDDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString());
                    }
                    else
                    {
                        PDDate = Convert.ToDateTime("1/1/1990");
                    }
                }
                Session["NewUserPDDate"] = PDDate;
                string CCCardNumber = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
                string Cardtype = Cardetais.Tables[0].Rows[0]["Cardtype"].ToString();
                string CardExpDt = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
                string CardholderName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderName"].ToString());
                string CardholderLastName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString());
                string CardCode = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();
                string BillingAdd = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString());
                string BillingCity = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString());
                string BillingState = Cardetais.Tables[0].Rows[0]["billingState"].ToString();
                string BillingZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
                string BillingPhone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
                int AccType;
                if (Cardetais.Tables[0].Rows[0]["bankAccountType"].ToString() != "")
                {
                    AccType = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["bankAccountType"].ToString());
                }
                else
                {
                    AccType = 0;
                }
                string BankRouting = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
                string bankName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankName"].ToString());
                string AccNumber = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                string AccHolderName = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
                string CheckNumber = "";
                int CheckType = Convert.ToInt32(5);
                string PayPalEmailAcc = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                bool bnewPay = objdropdownBL.SmartzSavePmntDetailsOfAnotherCarForCarSales(PmntType, PmntStatus, TransactionID, strIp, RegUID, AdActive, PayAmount, Paymentdate, ListingStatus, PDDate, UserPackID, PostingID, VoiceFileName, UceStatus, MultisiteStatus, VoiceFileLocation, PendingAmount, CCCardNumber,
                                Cardtype, CardExpDt, CardholderName, CardholderLastName, CardCode, CardholderName, BillingPhone, BillingAdd, BillingCity, BillingState, BillingZip,
                                PayPalEmailAcc, CheckType, CheckNumber, AccType, BankRouting, bankName, AccNumber, AccHolderName);
            }
            else
            {
                bool bnewPay = objdropdownBL.SmartzSavePmntDetailsOfAnotherCarForFreePackage(RegUID, AdActive, ListingStatus, UserPackID, PostingID, UceStatus, MultisiteStatus);
            }
            DataSet dsUpdateSmartzStatus = objHotLeadBL.UpdateSmartzMoveStatus(1, Convert.ToInt32(Session["AgentQCMovingPostingID"].ToString()), CarID);
            for (int i = 1; i < 52; i++)
            {
                if (Cardetais.Tables[1].Rows[i - 1]["Isactive"].ToString() == "True")
                {
                    IsactiveFea = 1;
                }
                else
                {
                    IsactiveFea = 0;
                }
                FeatureID = i;
                bool dsCarFeature = objdropdownBL.USP_SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            int UID;
            UID = 15;
            if (Session["CarSellerZip"].ToString() != "")
            {
                string SellerZipTick = Session["CarSellerZip"].ToString();
                DataSet dsZipExists = objdropdownBL.SmartzCheckZipExists(SellerZipTick);
                if (dsZipExists.Tables[0].Rows[0]["Result"].ToString() != "Yes")
                {
                    int CallType = Convert.ToInt32(8);
                    int CallReason = Convert.ToInt32(4);
                    int CallResolution = Convert.ToInt32(8);
                    string SpokeWith = "Internal Ticket";
                    string Notes = "Internal ticket for zip " + SellerZipTick.ToString() + " is not exists";
                    int TicketType = Convert.ToInt32(3);
                    int Priority = Convert.ToInt32(2);
                    int CallBackID = Convert.ToInt32(1);
                    string TicketDescription = "Internal ticket for zip " + SellerZipTick.ToString() + " is not exists";
                    bool bnew = objdropdownBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                }

            }
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            if ((Session["SourceOfPhotos"].ToString() == "2") || (Session["SourceOfPhotos"].ToString() == "3"))
            {
                int CallType = Convert.ToInt32(8);
                int CallReason = Convert.ToInt32(4);
                int CallResolution = Convert.ToInt32(8);
                string SpokeWith = "Internal Ticket";
                string Notes = string.Empty;
                if (Session["SourceOfPhotos"].ToString() == "2")
                {
                    Notes = "Get photos from craigslist";
                }
                else
                {
                    Notes = "Use stock photos";
                }
                int TicketType = Convert.ToInt32(3);
                int Priority = Convert.ToInt32(2);
                int CallBackID = Convert.ToInt32(1);
                string TicketDescription = Notes;
                bool bnew = objdropdownBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                string CenterCodeph = Session[Constants.CenterCode].ToString();
                UID = 15;
                String UpdatedByph = Session[Constants.NAME].ToString();
                string InternalNotesNewPh = Notes;
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedByph + "<br>";
                //InternalNotesNewPh = UpdateByWithDate + InternalNotesNewPh.Trim() + "<br>" + "-------------------------------------------------";
                InternalNotesNewPh = InternalNotesNewPh.Trim();
                DataSet dsNewIntNotes = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID, InternalNotesNewPh, UID);
            }
            if ((Session["SourceOfDescription"].ToString() == "2") || (Session["SourceOfDescription"].ToString() == "3"))
            {
                int CallType = Convert.ToInt32(8);
                int CallReason = Convert.ToInt32(4);
                int CallResolution = Convert.ToInt32(8);
                string SpokeWith = "Internal Ticket";
                string Notes = string.Empty;
                if (Session["SourceOfDescription"].ToString() == "2")
                {
                    Notes = "Get description from craigslist";
                }
                else
                {
                    Notes = "Use stock description";
                }
                int TicketType = Convert.ToInt32(3);
                int Priority = Convert.ToInt32(2);
                int CallBackID = Convert.ToInt32(1);
                string TicketDescription = Notes;
                bool bnew = objdropdownBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                string CenterCodeDesc = Session[Constants.CenterCode].ToString();
                UID = 15;
                String UpdatedByDesc = Session[Constants.NAME].ToString();
                string InternalNotesNewDesc = Notes;
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedByDesc + "<br>";
                //InternalNotesNewDesc = UpdateByWithDate + InternalNotesNewDesc.Trim() + "<br>" + "-------------------------------------------------";
                InternalNotesNewDesc = InternalNotesNewDesc.Trim();
                DataSet dsNewIntNotes = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID, InternalNotesNewDesc, UID);
            }
            int CarID1 = Convert.ToInt32(Session["CarID"].ToString());
            int UID1;
            string CenterCode1 = Session[Constants.CenterCode].ToString();
            UID1 = 15;
            string InternalNotesNew1 = string.Empty;
            InternalNotesNew1 = "-------------------------------------------------";
            DataSet dsNewIntNotes1 = objdropdownBL.USP_UpdateCustomerInternalNotes(CarID1, InternalNotesNew1, UID1);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnQCSearch_Click(object sender, EventArgs e)
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


            string Search = txtQCSearch.Text;
            Search = Search.Replace("-", "");
            GetResults1(Status, CenterID, Search);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void GetResults1(string Status, int CenterID, string Search)
    {
        try
        {

            // DateTime StartingDate = Convert.ToDateTime(StartDate.AddDays(-1).ToString("MM/dd/yyyy"));
            DataSet SingleAgentSales = new DataSet();
            if (ddlQCSearch.Text == "SaleID")
            SingleAgentSales = objHotLeadBL.GetQCDataForAllSearch(Status, CenterID, Search);
            else  if (ddlQCSearch.Text == "Name")
                SingleAgentSales = objHotLeadBL.GetQCDataForAllSearch2(Status, CenterID, Search);
            else if (ddlQCSearch.Text == "Phone")
                SingleAgentSales = objHotLeadBL.GetQCDataForAllSearch3(Status, CenterID, Search);
            Session["AllSalesQCData"] = SingleAgentSales;
            lblResHead.Text = "Recent 400 sales are showing";
            if (SingleAgentSales.Tables[0].Rows.Count > 0)
            {
                grdWarmLeadInfo.Visible = true;
                lblResCount.Visible = true;
                lblRes.Visible = false;
                lblResCount.Text = "Total " + SingleAgentSales.Tables[0].Rows.Count.ToString() + " records found";
                grdWarmLeadInfo.DataSource = SingleAgentSales.Tables[0];
                grdWarmLeadInfo.DataBind();
            }
            else
            {
                grdWarmLeadInfo.Visible = false;
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
}
