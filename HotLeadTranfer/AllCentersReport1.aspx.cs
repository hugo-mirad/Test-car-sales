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

public partial class AllCentersReport1 : System.Web.UI.Page
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
                        FillCenters();
                        FillPayments();
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
        dsIndidivitualRights = objHotLeadBL.GetUserModules_ActiveInactive(Convert.ToInt32(Session[Constants.USER_ID]));

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


    private void FillPayments()
    {
        //try
        //{
        //    DataSet dsCenters = objHotLeadBL.GetAllPaymentStatus();
        //    ddlpaymen.Items.Clear();
        //    for (int i = 1; i < dsCenters.Tables[0].Rows.Count; i++)
        //    {
        //        if (dsCenters.Tables[0].Rows[i]["PSStatusID"].ToString() != "0")
        //        {
        //            ListItem list = new ListItem();
        //            list.Text = dsCenters.Tables[0].Rows[i]["PSStatusName"].ToString();
        //            list.Value = dsCenters.Tables[0].Rows[i]["PSStatusID"].ToString();
        //            ddlpaymen.Items.Add(list);
        //        }
        //    }
        //   ddlpaymen.Items.Insert(0, new ListItem("All", "0"));
        //   ddlpaymen.SelectedIndex = 2;
           
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        ddlpaymen.SelectedIndex = 1;
    }
    private void GetResults(DateTime StartDate, DateTime EndDate)
    {
        try
        {
            // DateTime StartingDate = Convert.ToDateTime(StartDate.AddDays(-1).ToString("MM/dd/yyyy"));
            //DateTime EndingDate = Convert.ToDateTime(EndDate.AddDays(1).ToString("MM/dd/yyyy"));
            DateTime StartingDate = StartDate;
            DateTime EndingDate = EndDate;
            int SaleAgentID = Convert.ToInt32(Session[Constants.USER_ID]);
            int CenterCode = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            int paymentid = Convert.ToInt32(ddlpaymen.SelectedItem.Value);
            DataSet SingleAgentSales = new DataSet();
            SingleAgentSales = objHotLeadBL.GetAllCentersAgentsSalesDataNewPending(StartingDate, EndingDate, CenterCode, paymentid);
            DataSet AbandonSales = objHotLeadBL.GetAllCentersAgentsAbandonSalesData(StartingDate, EndingDate, CenterCode);
            DataSet dsTransferIn = objHotLeadBL.GetAllCentersAgentsTransfersOutSalesData(StartingDate, EndingDate, CenterCode);
            DataSet dsVerifiesData = objHotLeadBL.GetAllCentersVerifiesSalesData(StartingDate, EndingDate, CenterCode);
            Session["AllCentersAgentTransferOutSales"] = dsTransferIn;
            tblTransfersIN.Style["display"] = "block";
            Session["AllCentersAgentAbandonSales"] = AbandonSales;
            Session["AllCentersAgentSales"] = SingleAgentSales;
            Session["AllCentersVerifiesSales"] = dsVerifiesData;
            lblResHead.Text = "All centers performance report for the period " + StartDate.ToString("MM/dd/yyyy") + " to " + EndDate.ToString("MM/dd/yyyy");

            if (SingleAgentSales.Tables[0].Rows.Count > 0)
            {
                lblTotSales.Text = SingleAgentSales.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lblTotSales.Text = "0";
            }
            if (AbandonSales.Tables[0].Rows.Count > 0)
            {
                lblTotAbandon.Text = AbandonSales.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lblTotAbandon.Text = "0";
            }
            if (dsVerifiesData.Tables[0].Rows.Count > 0)
            {
                lblTotVerif.Text = dsVerifiesData.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lblTotVerif.Text = "0";
            }
            if (dsTransferIn.Tables[0].Rows.Count > 0)
            {
                lblTotTransfers.Text = dsTransferIn.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lblTotTransfers.Text = "0";
            }

            if (rdbtnSales.Checked == true)
            {
                tblSales.Style["display"] = "block";
                tblVerifies.Style["display"] = "none";
                tblAbandon.Style["display"] = "none";
                tblTransfersIN.Style["display"] = "none";
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
            if (rdbtnAbandon.Checked == true)
            {
                tblSales.Style["display"] = "none";
                tblVerifies.Style["display"] = "none";
                tblAbandon.Style["display"] = "block";
                tblTransfersIN.Style["display"] = "none";
                if (AbandonSales.Tables[0].Rows.Count > 0)
                {
                    grdAbandInfo.Visible = true;
                    lblAbandonResCount.Visible = true;
                    lblAbandonRes.Visible = false;
                    lblAbandonResCount.Text = "Total " + AbandonSales.Tables[0].Rows.Count.ToString() + " records found";
                    grdAbandInfo.DataSource = AbandonSales.Tables[0];
                    grdAbandInfo.DataBind();
                }
                else
                {
                    grdAbandInfo.Visible = false;
                    lblAbandonResCount.Visible = false;
                    lblAbandonRes.Visible = true;
                    lblAbandonRes.Text = "No records exist";
                }
            }
            if (rdbtnTransfers.Checked == true)
            {
                tblSales.Style["display"] = "none";
                tblVerifies.Style["display"] = "none";
                tblAbandon.Style["display"] = "none";
                tblTransfersIN.Style["display"] = "block";
                if (dsTransferIn.Tables[0].Rows.Count > 0)
                {
                    grdTransfersIn.Visible = true;
                    lblTranferResCount.Visible = true;
                    lblTransferRes.Visible = false;
                    lblTranferResCount.Text = "Total " + dsTransferIn.Tables[0].Rows.Count.ToString() + " transfer out records found";
                    grdTransfersIn.DataSource = dsTransferIn.Tables[0];
                    grdTransfersIn.DataBind();
                }
                else
                {
                    grdTransfersIn.Visible = false;
                    lblTranferResCount.Visible = false;
                    lblTransferRes.Visible = true;
                    lblTransferRes.Text = "No records exist";
                }
            }
            if (rdbtnVerifications.Checked == true)
            {
                tblSales.Style["display"] = "none";
                tblVerifies.Style["display"] = "block";
                tblAbandon.Style["display"] = "none";
                tblTransfersIN.Style["display"] = "none";
                if (dsVerifiesData.Tables[0].Rows.Count > 0)
                {
                    grdVerifierData.Visible = true;
                    lblVerifyResCount.Visible = true;
                    lblVerifyRes.Visible = false;
                    lblVerifyResCount.Text = "Total " + dsVerifiesData.Tables[0].Rows.Count.ToString() + " records found";
                    grdVerifierData.DataSource = dsVerifiesData.Tables[0];
                    grdVerifierData.DataBind();
                }
                else
                {
                    grdVerifierData.Visible = false;
                    lblVerifyResCount.Visible = false;
                    lblVerifyRes.Visible = true;
                    lblVerifyRes.Text = "No records exist";
                }
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
    protected void grdWarmLeadInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnAgentID = (HiddenField)e.Row.FindControl("hdnAgentID");
                Label lblAgent = (Label)e.Row.FindControl("lblAgent");
                HiddenField hdnAgentName = (HiddenField)e.Row.FindControl("hdnAgentName");
                HiddenField hdnVerifierID = (HiddenField)e.Row.FindControl("hdnVerifierID");
                Label lblVerifier = (Label)e.Row.FindControl("lblVerifier");
                HiddenField hdnVerifierName = (HiddenField)e.Row.FindControl("hdnVerifierName");

                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnPackName");
                HiddenField hdnPackDiscount = (HiddenField)e.Row.FindControl("hdnPackDiscount");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnPhoneNum");

                Label lblPaid = (Label)e.Row.FindControl("lblPaid");
                Label lblPending = (Label)e.Row.FindControl("lblPending");
                HiddenField hdnPSID1Status = (HiddenField)e.Row.FindControl("hdnPSID1Status");
                HiddenField hdnPSID2Status = (HiddenField)e.Row.FindControl("hdnPSID2Status");
                HiddenField hdnAmount1 = (HiddenField)e.Row.FindControl("hdnAmount1");
                HiddenField hdnAmount2 = (HiddenField)e.Row.FindControl("hdnAmount2");
                Label lblQcStatus = (Label)e.Row.FindControl("lblQcStatus");
                HiddenField hdnQcStatus = (HiddenField)e.Row.FindControl("hdnQcStatus");
                HiddenField hdnQCNotes = (HiddenField)e.Row.FindControl("hdnQCNotes");

                Label lblPmntStatus = (Label)e.Row.FindControl("lblPmntStatus");
                HiddenField hdnPmntStatus = (HiddenField)e.Row.FindControl("hdnPmntStatus");
                HiddenField hdnPmntReason = (HiddenField)e.Row.FindControl("hdnPmntReason");
                Label lnkCarID = (Label)e.Row.FindControl("lnkCarID");
                HiddenField hdnPSIDNotes = (HiddenField)e.Row.FindControl("hdnPSIDNotes");
                Label lblName = (Label)e.Row.FindControl("lblName");
                HiddenField hdnSellerName = (HiddenField)e.Row.FindControl("hdnSellerName");
                HiddenField hdnLastName = (HiddenField)e.Row.FindControl("hdnLastName");
                HiddenField hdnQCStatusID = (HiddenField)e.Row.FindControl("hdnQCStatusID");
                HiddenField hdnAgentCenterCode = (HiddenField)e.Row.FindControl("hdnAgentCenterCode");
                HiddenField hdnVerifierCenterCode = (HiddenField)e.Row.FindControl("hdnVerifierCenterCode");

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



                //Discount 21-11-2013 starts 
                //Double PackCost = new Double();
                //PackCost = Convert.ToDouble(hdnPackCost.Value.ToString());
                //string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                //string PackName = hdnPackName.Value.ToString();
                //lblPackage.Text = PackName + "($" + PackAmount + ")";
                //Discount 21-11-2013 Ends
              
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
                    lblAgent.Text = objGeneralFunc.GetCenterCode(hdnAgentCenterCode.Value) + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnAgentName.Value.ToString(), 15);
                }
                else
                {
                    lblAgent.Text = "";
                }

                if (hdnVerifierID.Value.ToString() != "0")
                {
                    lblVerifier.Text = objGeneralFunc.GetCenterCode(hdnVerifierCenterCode.Value) + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnVerifierName.Value.ToString(), 15);
                }
                else
                {
                    lblVerifier.Text = "";
                }
                double PSID1AmountPaid = Convert.ToDouble("0.00");
                double PSID1AmountPend = Convert.ToDouble("0.00");
                if ((hdnPSID1Status.Value == "1") || (hdnPSID1Status.Value == "7"))
                {
                    //lblPrice.Text = "$" + string.Format("{0:0.00}", Convert.ToDouble(CarsDetails.Tables[0].Rows[0]["price"].ToString()));
                    PSID1AmountPaid = PSID1AmountPaid + Convert.ToDouble(hdnAmount1.Value);
                }
                else
                {
                    if (hdnAmount1.Value != "")
                    {
                        PSID1AmountPend = PSID1AmountPend + Convert.ToDouble(hdnAmount1.Value);
                    }
                }
                if (hdnPSID2Status.Value == "1")
                {
                    if (hdnAmount2.Value != "")
                    {
                        PSID1AmountPaid = PSID1AmountPaid + Convert.ToDouble(hdnAmount2.Value);
                    }
                }
                else
                {
                    if (hdnAmount2.Value != "")
                    {
                        PSID1AmountPend = PSID1AmountPend + Convert.ToDouble(hdnAmount2.Value);
                    }
                }
                lblPaid.Text = string.Format("{0:0.00}", PSID1AmountPaid);
                lblPending.Text = string.Format("{0:0.00}", PSID1AmountPend);
                if (hdnQcStatus.Value == "")
                {
                    lblQcStatus.Text = "QC Open";
                    lblQcStatus.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    if (hdnQCStatusID.Value == "1")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    if (hdnQCStatusID.Value == "2")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Red;
                    }
                    if (hdnQCStatusID.Value == "3")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                    if (hdnQCStatusID.Value == "4")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Blue;
                    }
                    lblQcStatus.Text = hdnQcStatus.Value;
                }
                if (hdnQCNotes.Value.Trim() != "")
                {
                    string sTable = CreateTable(hdnQCNotes.Value);
                    lblQcStatus.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                    lblQcStatus.Attributes.Add("onmouseout", "return nd1(4000);");
                }
                lblPmntStatus.Text = hdnPmntStatus.Value;
                if ((hdnPSID1Status.Value == "2") || (hdnPSIDNotes.Value != ""))
                {
                    string NotesText = string.Empty;
                    if (hdnPmntReason.Value != "")
                    {
                        if (hdnPSID1Status.Value == "2")
                        {
                            NotesText = hdnPmntReason.Value + "<br />" + hdnPSIDNotes.Value;
                        }
                        else
                        {
                            NotesText = hdnPSIDNotes.Value;
                        }
                    }
                    else
                    {
                        NotesText = hdnPSIDNotes.Value;
                    }
                    if (NotesText.Trim() != "")
                    {
                        string sTable1 = CreateTable2(NotesText.Trim());
                        lblPmntStatus.Attributes.Add("onmouseover", "return overlib1('" + sTable1 + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                        lblPmntStatus.Attributes.Add("onmouseout", "return nd1(4000);");
                    }
                }
                if ((hdnPSID1Status.Value == "5") || (hdnQCStatusID.Value == "4"))
                {
                    lnkCarID.Enabled = true;
                }
                else
                {
                    lnkCarID.Enabled = false;
                }
                if ((hdnPSID1Status.Value == "1") || (hdnPSID1Status.Value == "7") || (hdnPSID1Status.Value == "8"))
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Green;
                }
                else if (hdnPSID1Status.Value == "2")
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Red;
                }
                else if (hdnPSID1Status.Value == "3")
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Orange;
                }
                else if ((hdnPSID1Status.Value == "4") || (hdnPSID1Status.Value == "6"))
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Black;
                }
                else if (hdnPSID1Status.Value == "5")
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Yellow;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdVerifierData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnAgentID = (HiddenField)e.Row.FindControl("hdnVerifyAgentID");
                Label lblAgent = (Label)e.Row.FindControl("lblVerifyAgent");
                HiddenField hdnAgentName = (HiddenField)e.Row.FindControl("hdnVerifyAgentName");
                HiddenField hdnVerifierID = (HiddenField)e.Row.FindControl("hdnVerifyVeriferID");
                Label lblVerifier = (Label)e.Row.FindControl("lblVerifyVerifer");
                HiddenField hdnVerifierName = (HiddenField)e.Row.FindControl("hdnVerifyVeriferName");

                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnVerifyPackName");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnVerifyPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblVerifyPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblVerifyPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnVerifyPhoneNum");

                Label lblPaid = (Label)e.Row.FindControl("lblVerifyPaid");
                Label lblPending = (Label)e.Row.FindControl("lblVerifyPending");
                HiddenField hdnPSID1Status = (HiddenField)e.Row.FindControl("hdnVerifyPSID1Status");
                HiddenField hdnPSID2Status = (HiddenField)e.Row.FindControl("hdnVerifyPSID2Status");
                HiddenField hdnAmount1 = (HiddenField)e.Row.FindControl("hdnVerifyAmount1");
                HiddenField hdnAmount2 = (HiddenField)e.Row.FindControl("hdnVerifyAmount2");
                Label lblQcStatus = (Label)e.Row.FindControl("lblVerifyQcStatus");
                HiddenField hdnQcStatus = (HiddenField)e.Row.FindControl("hdnVerifyQcStatus");
                HiddenField hdnQCNotes = (HiddenField)e.Row.FindControl("hdnVerifyQCNotes");

                Label lblPmntStatus = (Label)e.Row.FindControl("lblVerifyPmntStatus");
                HiddenField hdnPmntStatus = (HiddenField)e.Row.FindControl("hdnVerifyPmntStatus");
                HiddenField hdnPmntReason = (HiddenField)e.Row.FindControl("hdnVerifyPmntReason");
                Label lnkCarID = (Label)e.Row.FindControl("lnkVerifyCarID");
                HiddenField hdnPSIDNotes = (HiddenField)e.Row.FindControl("hdnVerifyPSIDNotes");
                Label lblName = (Label)e.Row.FindControl("lblVerifyName");
                HiddenField hdnSellerName = (HiddenField)e.Row.FindControl("hdnVerifySellerName");
                HiddenField hdnLastName = (HiddenField)e.Row.FindControl("hdnVerifyLastName");
                HiddenField hdnQCStatusID = (HiddenField)e.Row.FindControl("hdnVerifyQCStatusID");
                HiddenField hdnVerifyAgentCenterCode = (HiddenField)e.Row.FindControl("hdnVerifyAgentCenterCode");
                HiddenField hdnVerifyVerifierCenterCode = (HiddenField)e.Row.FindControl("hdnVerifyVerifierCenterCode");

                Label lblYear = (Label)e.Row.FindControl("lblVerifyYear");
                HiddenField hdnYear = (HiddenField)e.Row.FindControl("hdnVerifyYear");
                HiddenField hdnMake = (HiddenField)e.Row.FindControl("hdnVerifyMake");
                HiddenField hdnModel = (HiddenField)e.Row.FindControl("hdnVerifyModel");

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
                    lblAgent.Text = objGeneralFunc.GetCenterCode(hdnVerifyAgentCenterCode.Value) + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnAgentName.Value.ToString(), 15);
                }
                else
                {
                    lblAgent.Text = "";
                }

                if (hdnVerifierID.Value.ToString() != "0")
                {
                    lblVerifier.Text = objGeneralFunc.GetCenterCode(hdnVerifyVerifierCenterCode.Value) + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnVerifierName.Value.ToString(), 15);
                }
                else
                {
                    lblVerifier.Text = "";
                }
                double PSID1AmountPaid = Convert.ToDouble("0.00");
                double PSID1AmountPend = Convert.ToDouble("0.00");
                if ((hdnPSID1Status.Value == "1") || (hdnPSID1Status.Value == "7"))
                {
                    //lblPrice.Text = "$" + string.Format("{0:0.00}", Convert.ToDouble(CarsDetails.Tables[0].Rows[0]["price"].ToString()));
                    PSID1AmountPaid = PSID1AmountPaid + Convert.ToDouble(hdnAmount1.Value);
                }
                else
                {
                    if (hdnAmount1.Value != "")
                    {
                        PSID1AmountPend = PSID1AmountPend + Convert.ToDouble(hdnAmount1.Value);
                    }
                }
                if (hdnPSID2Status.Value == "1")
                {
                    if (hdnAmount2.Value != "")
                    {
                        PSID1AmountPaid = PSID1AmountPaid + Convert.ToDouble(hdnAmount2.Value);
                    }
                }
                else
                {
                    if (hdnAmount2.Value != "")
                    {
                        PSID1AmountPend = PSID1AmountPend + Convert.ToDouble(hdnAmount2.Value);
                    }
                }
                lblPaid.Text = string.Format("{0:0.00}", PSID1AmountPaid);
                lblPending.Text = string.Format("{0:0.00}", PSID1AmountPend);
                if (hdnQcStatus.Value == "")
                {
                    lblQcStatus.Text = "QC Open";
                    lblQcStatus.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    if (hdnQCStatusID.Value == "1")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    if (hdnQCStatusID.Value == "2")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Red;
                    }
                    if (hdnQCStatusID.Value == "3")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                    if (hdnQCStatusID.Value == "4")
                    {
                        lblQcStatus.ForeColor = System.Drawing.Color.Blue;
                    }
                    lblQcStatus.Text = hdnQcStatus.Value;
                }
                if (hdnQCNotes.Value.Trim() != "")
                {
                    string sTable = CreateTable(hdnQCNotes.Value);
                    lblQcStatus.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                    lblQcStatus.Attributes.Add("onmouseout", "return nd1(4000);");
                }
                lblPmntStatus.Text = hdnPmntStatus.Value;
                if ((hdnPSID1Status.Value == "2") || (hdnPSIDNotes.Value != ""))
                {
                    string NotesText = string.Empty;
                    if (hdnPmntReason.Value != "")
                    {
                        if (hdnPSID1Status.Value == "2")
                        {
                            NotesText = hdnPmntReason.Value + "<br />" + hdnPSIDNotes.Value;
                        }
                        else
                        {
                            NotesText = hdnPSIDNotes.Value;
                        }
                    }
                    else
                    {
                        NotesText = hdnPSIDNotes.Value;
                    }
                    if (NotesText.Trim() != "")
                    {
                        string sTable1 = CreateTable2(NotesText.Trim());
                        lblPmntStatus.Attributes.Add("onmouseover", "return overlib1('" + sTable1 + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                        lblPmntStatus.Attributes.Add("onmouseout", "return nd1(4000);");
                    }
                }
                if ((hdnPSID1Status.Value == "5") || (hdnQCStatusID.Value == "4"))
                {
                    lnkCarID.Enabled = true;
                }
                else
                {
                    lnkCarID.Enabled = false;
                }
                if ((hdnPSID1Status.Value == "1") || (hdnPSID1Status.Value == "7") || (hdnPSID1Status.Value == "8"))
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Green;
                }
                else if (hdnPSID1Status.Value == "2")
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Red;
                }
                else if (hdnPSID1Status.Value == "3")
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Orange;
                }
                else if ((hdnPSID1Status.Value == "4") || (hdnPSID1Status.Value == "6"))
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Black;
                }
                else if (hdnPSID1Status.Value == "5")
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    lblPmntStatus.ForeColor = System.Drawing.Color.Yellow;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdAbandInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnAbandonAgentID = (HiddenField)e.Row.FindControl("hdnAbandonAgentID");
                Label lblAbandonAgent = (Label)e.Row.FindControl("lblAbandonAgent");
                HiddenField hdnAbandonAgentName = (HiddenField)e.Row.FindControl("hdnAbandonAgentName");
                HiddenField hdnAbandonPackName = (HiddenField)e.Row.FindControl("hdnAbandonPackName");
                HiddenField hdnAbandonPackCost = (HiddenField)e.Row.FindControl("hdnAbandonPackCost");
                Label lblAbandonPackage = (Label)e.Row.FindControl("lblAbandonPackage");
                Label lblAbandonPhone = (Label)e.Row.FindControl("lblAbandonPhone");
                HiddenField hdnAbandonPhoneNum = (HiddenField)e.Row.FindControl("hdnAbandonPhoneNum");
                Label lblAbandonName = (Label)e.Row.FindControl("lblAbandonName");
                HiddenField hdnAbandonSellerName = (HiddenField)e.Row.FindControl("hdnAbandonSellerName");
                HiddenField hdnAbandonLastName = (HiddenField)e.Row.FindControl("hdnAbandonLastName");

                Label lblYear = (Label)e.Row.FindControl("lblAbandonYear");
                HiddenField hdnYear = (HiddenField)e.Row.FindControl("hdnAbandonYear");
                HiddenField hdnMake = (HiddenField)e.Row.FindControl("hdnAbandonMake");
                HiddenField hdnModel = (HiddenField)e.Row.FindControl("hdnAbandonModel");

                lblYear.Text = hdnYear.Value + "/" + hdnMake.Value + "/" + hdnModel.Value;
                string TransName = string.Empty;
                if (hdnAbandonLastName.Value != "")
                {
                    TransName = hdnAbandonLastName.Value + " " + hdnAbandonSellerName.Value;
                }
                else
                {
                    TransName = hdnAbandonSellerName.Value;
                }
                if (TransName.Length > 15)
                {
                    lblAbandonName.Text = objGeneralFunc.WrapTextByMaxCharacters(TransName, 15);
                }
                else
                {
                    lblAbandonName.Text = TransName;
                }

                Double PackCost = new Double();
                PackCost = Convert.ToDouble(hdnAbandonPackCost.Value.ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = hdnAbandonPackName.Value.ToString();
                lblAbandonPackage.Text = PackName + "($" + PackAmount + ")";
                if (hdnAbandonPhoneNum.Value.ToString() == "")
                {
                    lblAbandonPhone.Text = "";
                }
                else
                {
                    lblAbandonPhone.Text = objGeneralFunc.filPhnm(hdnAbandonPhoneNum.Value);
                }
                if (hdnAbandonAgentID.Value.ToString() != "0")
                {
                    lblAbandonAgent.Text = objGeneralFunc.WrapTextByMaxCharacters(hdnAbandonAgentName.Value.ToString(), 15);
                }
                else
                {
                    lblAbandonAgent.Text = "";
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdTransfersIn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnAgentID = (HiddenField)e.Row.FindControl("hdnTransAgentID");
                Label lblAgent = (Label)e.Row.FindControl("lblTransAgent");
                HiddenField hdnAgentName = (HiddenField)e.Row.FindControl("hdnTransAgentName");
                HiddenField hdnPackName = (HiddenField)e.Row.FindControl("hdnTransPackName");
                HiddenField hdnPackCost = (HiddenField)e.Row.FindControl("hdnTransPackCost");
                Label lblPackage = (Label)e.Row.FindControl("lblTransPackage");
                Label lblPhone = (Label)e.Row.FindControl("lblTransPhone");
                HiddenField hdnPhoneNum = (HiddenField)e.Row.FindControl("hdnTransPhoneNum");

                HiddenField hdnTransVerifierID = (HiddenField)e.Row.FindControl("hdnTransVerifierID");
                Label lblTransVerifier = (Label)e.Row.FindControl("lblTransVerifier");
                HiddenField hdnTransVerifierName = (HiddenField)e.Row.FindControl("hdnTransVerifierName");
                Label lblTransStatus = (Label)e.Row.FindControl("lblTransStatus");
                HiddenField hdnTransStatusName = (HiddenField)e.Row.FindControl("hdnTransStatusName");
                HiddenField hdnTransStatusID = (HiddenField)e.Row.FindControl("hdnTransStatusID");
                HiddenField hdnTransDisposID = (HiddenField)e.Row.FindControl("hdnTransDisposID");
                HiddenField hdnTransDisposName = (HiddenField)e.Row.FindControl("hdnTransDisposName");
                Label lblTransName = (Label)e.Row.FindControl("lblTransName");
                HiddenField hdnTransSellerName = (HiddenField)e.Row.FindControl("hdnTransSellerName");
                HiddenField hdnTransLastName = (HiddenField)e.Row.FindControl("hdnTransLastName");
                HiddenField hdnTransAgentCenterCode = (HiddenField)e.Row.FindControl("hdnTransAgentCenterCode");
                HiddenField hdnTransVerifierCenterCode = (HiddenField)e.Row.FindControl("hdnTransVerifierCenterCode");

                Label lblYear = (Label)e.Row.FindControl("lblTransYear");
                HiddenField hdnYear = (HiddenField)e.Row.FindControl("hdnTransYear");
                HiddenField hdnMake = (HiddenField)e.Row.FindControl("hdnTransMake");
                HiddenField hdnModel = (HiddenField)e.Row.FindControl("hdnTransModel");

                lblYear.Text = hdnYear.Value + "/" + hdnMake.Value + "/" + hdnModel.Value;

                string TransName = string.Empty;
                if (hdnTransLastName.Value != "")
                {
                    TransName = hdnTransLastName.Value + " " + hdnTransSellerName.Value;
                }
                else
                {
                    TransName = hdnTransSellerName.Value;
                }
                if (TransName.Length > 15)
                {
                    lblTransName.Text = objGeneralFunc.WrapTextByMaxCharacters(TransName, 15);
                }
                else
                {
                    lblTransName.Text = TransName;
                }
                if (hdnTransStatusID.Value == "4")
                {
                    if (hdnTransDisposName.Value != "")
                    {
                        lblTransStatus.Text = hdnTransDisposName.Value;
                    }
                    else
                    {
                        lblTransStatus.Text = hdnTransStatusName.Value;
                    }
                }
                else
                {
                    lblTransStatus.Text = hdnTransStatusName.Value;
                }


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
                    lblAgent.Text = objGeneralFunc.GetCenterCode(hdnTransAgentCenterCode.Value) + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnAgentName.Value.ToString(), 15);
                }
                else
                {
                    lblAgent.Text = "";
                }
                if (hdnTransVerifierID.Value.ToString() != "0")
                {
                    if (hdnTransVerifierName.Value.Trim() != "")
                    {
                        lblTransVerifier.Text = objGeneralFunc.GetCenterCode(hdnTransVerifierCenterCode.Value) + ":" + objGeneralFunc.WrapTextByMaxCharacters(hdnTransVerifierName.Value.ToString(), 15);
                    }
                }
                else
                {
                    lblTransVerifier.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void rdbtnSales_CheckedChanged(object sender, EventArgs e)
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
    protected void rdbtnVerifications_CheckedChanged(object sender, EventArgs e)
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
    protected void rdbtnAbandon_CheckedChanged(object sender, EventArgs e)
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
    protected void rdbtnTransfers_CheckedChanged(object sender, EventArgs e)
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

    protected void lnkbtnPmntStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PSStatusName1";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPmntStatus.Text = "Pmnt Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPmntStatus.Text = "Pmnt Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnPmntStatus.Text = "Pmnt Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnPmntStatus.Text = "Pmnt Status &#8659";
            }
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";
            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";


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
            ds = Session["AllCentersAgentSales"] as DataSet;
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
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";

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

    protected void lnkCarIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentSales"] as DataSet;
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
            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";


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
            ds = Session["AllCentersAgentSales"] as DataSet;
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
            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";

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
            ds = Session["AllCentersAgentSales"] as DataSet;
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
            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";

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

    protected void lnkVerifierSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "VerifierName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifierSort.Text = "Verifier &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifierSort.Text = "Verifier &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifierSort.Text = "Verifier &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifierSort.Text = "Verifier &#8659";
            }
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";

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
            ds = Session["AllCentersAgentSales"] as DataSet;
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
            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";

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
            ds = Session["AllCentersAgentSales"] as DataSet;
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

            lnkNameSort.Text = "Cust Name &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";

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
            ds = Session["AllCentersAgentSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "sellerName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkNameSort.Text = "Cust Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkNameSort.Text = "Cust Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkNameSort.Text = "Cust Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkNameSort.Text = "Cust Name &#8659";
            }
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkPackageSort.Text = "Package &darr; &uarr;";
            lnkPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";

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
            ds = Session["AllCentersAgentSales"] as DataSet;
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
            lnkNameSort.Text = "Cust Name &darr; &uarr;";

            lnkbtnQCStatus.Text = "QC Status &darr; &uarr;";
            lnkbtnPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifierSort.Text = "Verifier &darr; &uarr;";

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





    protected void lnkAbandonCenterCode_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "carid";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonCenterCode.Text = "Cen Code &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonCenterCode.Text = "Cen Code &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonCenterCode.Text = "Cen Code &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonCenterCode.Text = "Cen Code &#8659";
            }
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";
            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkAbandonCarIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "carid";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonCarIDSort.Text = "Sale ID &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonCarIDSort.Text = "Sale ID &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonCarIDSort.Text = "Sale ID &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonCarIDSort.Text = "Sale ID &#8659";
            }
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";
            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkAbandonSaleDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleDate";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonSaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonSaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonSaleDateSort.Text = "Sale Dt &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonSaleDateSort.Text = "Sale Dt &#8659";
            }
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";
            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkAbandonAgentSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleAgent";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonAgentSort.Text = "Agent &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonAgentSort.Text = "Agent &#8659";
            }
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkAbandonYearSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "yearOfMake";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonYearSort.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonYearSort.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonYearSort.Text = "Year/Make/Model &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonYearSort.Text = "Year/Make/Model &#8659";
            }
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";

            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkAbandonPackageSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "Price";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonPackageSort.Text = "Package &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonPackageSort.Text = "Package &#8659";
            }
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";
            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkAbandonNameSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "sellerName";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonNameSort.Text = "Cust Name &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonNameSort.Text = "Cust Name &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonNameSort.Text = "Cust Name &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonNameSort.Text = "Cust Name &#8659";
            }
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";
            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";
            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkAbandonPhoneSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PhoneNum";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonPhoneSort.Text = "Phone &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonPhoneSort.Text = "Phone &#8659";
            }
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";
            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonEmailSort.Text = "Email &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkAbandonEmailSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentAbandonSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "email";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonEmailSort.Text = "Email &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonEmailSort.Text = "Email &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkAbandonEmailSort.Text = "Email &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkAbandonEmailSort.Text = "Email &#8659";
            }
            lnkAbandonCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkAbandonSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAbandonAgentSort.Text = "Agent &darr; &uarr;";
            lnkAbandonYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkAbandonPackageSort.Text = "Package &darr; &uarr;";
            lnkAbandonNameSort.Text = "Cust Name &darr; &uarr;";
            lnkAbandonPhoneSort.Text = "Phone &darr; &uarr;";
            lnkAbandonCenterCode.Text = "Cen Code &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdAbandInfo, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }





    protected void lnkbtnTransSaleID_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "carid";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransSaleID.Text = "Sale ID &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransSaleID.Text = "Sale ID &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransSaleID.Text = "Sale ID &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransSaleID.Text = "Sale ID &#8659";
            }
            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";

            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnTransSaleDt_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "TransferDate";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransSaleDt.Text = "Trans Dt &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransSaleDt.Text = "Trans Dt &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransSaleDt.Text = "Trans Dt &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransSaleDt.Text = "Trans Dt &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnTransAgent_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleAgent";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransAgent.Text = "Agent &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransAgent.Text = "Agent &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransAgent.Text = "Agent &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransAgent.Text = "Agent &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnTransVerifier_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "VerifierName";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransVerifier.Text = "Verifier &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransVerifier.Text = "Verifier &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransVerifier.Text = "Verifier &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransVerifier.Text = "Verifier &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnTransStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "LeadStatusName";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransStatus.Text = "Status &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransStatus.Text = "Status &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransStatus.Text = "Status &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransStatus.Text = "Status &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnTransYear_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "yearOfMake";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransYear.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransYear.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransYear.Text = "Year/Make/Model &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransYear.Text = "Year/Make/Model &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkbtnTransPackage_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "Price";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransPackage.Text = "Package &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransPackage.Text = "Package &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransPackage.Text = "Package &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransPackage.Text = "Package &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnTransName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "sellerName";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransName.Text = "Cust Name &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransName.Text = "Cust Name &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransName.Text = "Cust Name &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransName.Text = "Cust Name &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnTransPhone_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PhoneNum";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransPhone.Text = "Phone &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransPhone.Text = "Phone &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransPhone.Text = "Phone &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransPhone.Text = "Phone &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransEmail.Text = "Email &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnTransEmail_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersAgentTransferOutSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "email";
            if (Session["SortAbandonDirec"] == null)
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransEmail.Text = "Email &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "")
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransEmail.Text = "Email &#8659";
            }
            else if (Session["SortAbandonDirec"].ToString() == "Ascending")
            {
                Session["SortAbandonDirec"] = "Descending";
                lnkbtnTransEmail.Text = "Email &#8657";
            }
            else
            {
                Session["SortAbandonDirec"] = "Ascending";
                lnkbtnTransEmail.Text = "Email &#8659";
            }
            lnkbtnTransSaleID.Text = "Sale ID &darr; &uarr;";

            lnkbtnTransSaleDt.Text = "Trans Dt &darr; &uarr;";
            lnkbtnTransAgent.Text = "Agent &darr; &uarr;";
            lnkbtnTransVerifier.Text = "Verifier &darr; &uarr;";
            lnkbtnTransStatus.Text = "Status &darr; &uarr;";
            lnkbtnTransYear.Text = "Year/Make/Model &darr; &uarr;";

            lnkbtnTransPackage.Text = "Package &darr; &uarr;";
            lnkbtnTransName.Text = "Cust Name &darr; &uarr;";
            lnkbtnTransPhone.Text = "Phone &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdTransfersIn, 0, dt, Session["SortAbandonDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private string CreateTable(string QcNotes)
    {

        
        QcNotes = QcNotes.Replace("\n", "<br />");
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"330px\" id=\"SalesStatus\" style=\"display: block; box-shadow: 0 0 8px rgba(0,0,0,0.4);background-color: #fff; border: #999 1px solid; padding: 2px; height: 190px\">";
        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"text-align:center;background-color:#ccc;width: 330px;\"> QC Notes";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += "<tr >";
        strTransaction += "<td  style=\"padding-left:10px;\" align=\"left\"> <div style=\"overflow: scroll; width: 310px; height: 150px;\">";
        strTransaction += QcNotes;
        strTransaction += "</div></td>";
        strTransaction += " </tr>";
        strTransaction += "</table>";

        return strTransaction;

    }
    private string CreateTable2(string PmntStatusReason)
    {
        PmntStatusReason = PmntStatusReason.Replace("\n", "<br />");
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"330px\" id=\"PmntStatus\" style=\"display: block; box-shadow: 0 0 8px rgba(0,0,0,0.4);background-color: #fff; border: #999 1px solid; padding: 2px; height: 190px\">";
        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"text-align:center;background-color:#ccc;width: 330px;\"> Payment notes";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += "<tr >";
        strTransaction += "<td  style=\"padding-left:10px;\" align=\"left\"> <div style=\"overflow: scroll; width: 310px; height: 150px;\">";
        strTransaction += PmntStatusReason;
        strTransaction += "</div></td>";
        strTransaction += " </tr>";
        strTransaction += "</table>";

        return strTransaction;

    }




    protected void lnkVerifyCarIDSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "carid";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyCarIDSort.Text = "Sale ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyCarIDSort.Text = "Sale ID &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifyCarIDSort.Text = "Sale ID &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyCarIDSort.Text = "Sale ID &#8659";
            }
            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkVerifySaleDateSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleDate";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifySaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifySaleDateSort.Text = "Sale Dt &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifySaleDateSort.Text = "Sale Dt &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifySaleDateSort.Text = "Sale Dt &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkVerifyAgentSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleAgent";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyAgentSort.Text = "Agent &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifyAgentSort.Text = "Agent &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyAgentSort.Text = "Agent &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkVerifyVerifierSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "SaleVerifierName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyVerifierSort.Text = "Verifier &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyVerifierSort.Text = "Verifier &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifyVerifierSort.Text = "Verifier &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyVerifierSort.Text = "Verifier &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";
            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkVerifyYearSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "yearOfMake";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyYearSort.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyYearSort.Text = "Year/Make/Model &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifyYearSort.Text = "Year/Make/Model &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyYearSort.Text = "Year/Make/Model &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkVerifyPackageSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "Price";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyPackageSort.Text = "Package &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifyPackageSort.Text = "Package &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyPackageSort.Text = "Package &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkVerifyNameSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "sellerName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyNameSort.Text = "Cust Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyNameSort.Text = "Cust Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifyNameSort.Text = "Cust Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyNameSort.Text = "Cust Name &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";

            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkVerifyPhoneSort_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PhoneNum";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyPhoneSort.Text = "Phone &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkVerifyPhoneSort.Text = "Phone &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkVerifyPhoneSort.Text = "Phone &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnVerifyQCStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "QCStatusName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnVerifyQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnVerifyQCStatus.Text = "QC Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnVerifyQCStatus.Text = "QC Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnVerifyQCStatus.Text = "QC Status &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkbtnVerifyPmntStatus.Text = "Pmnt Status &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnVerifyPmntStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["AllCentersVerifiesSales"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PSStatusName1";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnVerifyPmntStatus.Text = "Pmnt Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnVerifyPmntStatus.Text = "Pmnt Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkbtnVerifyPmntStatus.Text = "Pmnt Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkbtnVerifyPmntStatus.Text = "Pmnt Status &#8659";
            }
            lnkVerifyCarIDSort.Text = "Sale ID &darr; &uarr;";

            lnkVerifySaleDateSort.Text = "Sale Dt &darr; &uarr;";
            lnkVerifyAgentSort.Text = "Agent &darr; &uarr;";
            lnkVerifyVerifierSort.Text = "Verifier &darr; &uarr;";
            lnkVerifyYearSort.Text = "Year/Make/Model &darr; &uarr;";

            lnkVerifyPackageSort.Text = "Package &darr; &uarr;";
            lnkVerifyNameSort.Text = "Cust Name &darr; &uarr;";
            lnkbtnVerifyQCStatus.Text = "QC Status &darr; &uarr;";
            lnkVerifyPhoneSort.Text = "Phone &darr; &uarr;";


            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdVerifierData, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PaymentChange_click(object sender, EventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
        DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
        GetResults(StartDate, EndDate);
    }
}
