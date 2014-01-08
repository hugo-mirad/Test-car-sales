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
using System.Net;
using System.IO;

public partial class QCDataViewForDealer : System.Web.UI.Page
{
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet CarsDetails = new DataSet();
    DataSet dsDropDown = new DataSet();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    UserRegistrationInfo objUserregInfo = new UserRegistrationInfo();
    HotLeadsBL objHotLeadBL = new HotLeadsBL();
    DataSet dsDatetime = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.NAME] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
            if (!IsPostBack)
            {
                Session["CurrentPage"] = "QC Module";

                if (LoadIndividualUserRights() == false)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    ServiceReference objServiceReference = new ServiceReference();

                    ScriptReference objScriptReference = new ScriptReference();

                    objServiceReference.Path = "~/WebService.asmx";

                    objScriptReference.Path = "~/Static/Js/CarsJScript.js";

                    scrptmgr.Services.Add(objServiceReference);
                    scrptmgr.Scripts.Add(objScriptReference);

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
                    if (Session["DsDropDown"] == null)
                    {
                        dsDropDown = objdropdownBL.Usp_Get_DropDown();
                        Session["DsDropDown"] = dsDropDown;
                    }
                    else
                    {
                        dsDropDown = (DataSet)Session["DsDropDown"];
                    }
                    //lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");

                    DataSet dsDatetime = objHotLeadBL.GetDatetime();

                    //FillPaymentDate();
                    //FillPDDate(dsDatetime);
                    Session["ViewDealerQCStatus"] = "";
                    FillTargetDate(dsDatetime);
                    FillCallbackDate(dsDatetime);
                    //FillContractSignDate(dsDatetime);
                    //FillBillingStates();
                    //FillInvoiceBillingStates();
                    FillPayCancelReason();
                    //FillVoiceFileLocation();
                    // FillCheckTypes();
                    if ((Session["DealerQCDealerSaleID"] != null) && (Session["DealerQCDealerSaleID"].ToString() != ""))
                    {
                        DataSet GetDealerData = objHotLeadBL.GetDealerDetailsByDealerSaleID(Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString()));
                        if (GetDealerData.Tables.Count > 0)
                        {
                            if (GetDealerData.Tables[0].Rows.Count > 0)
                            {
                                RepeaterSurveyEdit.Visible = true;
                                RepeaterSurveyEdit.DataSource = GetDealerData.Tables[2];
                                RepeaterSurveyEdit.DataBind();
                            }
                            if ((GetDealerData.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "1") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "7") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "8"))
                            {
                                btnEdit.Visible = false;
                                ddlQCStatus.Visible = false;
                                btnQCUpdate.Visible = false;
                            }
                            if ((GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == "3") || (GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == ""))
                            {
                                btnEdit.Visible = true;
                                ddlQCStatus.Visible = true;
                                btnQCUpdate.Visible = true;
                            }
                            if (((GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == "3") || (GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == "")) && (GetDealerData.Tables[0].Rows[0]["SmartzStatus"].ToString() != "1") && (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "8"))
                            {
                                btnEdit.Visible = true;
                                ddlQCStatus.Visible = true;
                                btnQCUpdate.Visible = true;
                            }
                            if (GetDealerData.Tables[0].Rows[0]["QCStatusName"].ToString() == "")
                            {
                                lblQCStatus.Text = "QC Open";
                            }
                            else
                            {
                                lblQCStatus.Text = GetDealerData.Tables[0].Rows[0]["QCStatusName"].ToString();
                            }
                            txtDealerShipName.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["DealerShipName"].ToString());
                            txtPhone.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerPhone"].ToString());
                            txtFax.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerFax"].ToString());
                            txtWebAddress.Text = GetDealerData.Tables[0].Rows[0]["DealerWebAddress"].ToString();
                            txtDealerLicenseNumber.Text = GetDealerData.Tables[0].Rows[0]["DealerLicenseNumber"].ToString();
                            txtAddress.Text = GetDealerData.Tables[0].Rows[0]["DealerAddress"].ToString();
                            txtCity.Text = GetDealerData.Tables[0].Rows[0]["DealerCity"].ToString();
                            txtLocationState.Text = GetDealerData.Tables[0].Rows[0]["State_Code"].ToString();
                            txtDealerContactName.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["DealerContactName"].ToString());
                            txtDealerJobTitle.Text = GetDealerData.Tables[0].Rows[0]["DealerJobTitle"].ToString();
                            txtContactPhone.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerContactPhone"].ToString());
                            txtContactMobileNumber.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerMobilePhone"].ToString());
                            txtEmail.Text = GetDealerData.Tables[0].Rows[0]["DealerEmail"].ToString();
                            txtSaleNotes.Text = GetDealerData.Tables[0].Rows[0]["SaleNotes"].ToString();

                            if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 1)
                            {
                                rdbtnPayVisa.Checked = true;
                            }
                            else if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 2)
                            {
                                rdbtnPayMasterCard.Checked = true;
                            }
                            else if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 3)
                            {
                                rdbtnPayAmex.Checked = true;
                            }
                            else if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 4)
                            {
                                rdbtnPayDiscover.Checked = true;
                            }
                            else if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                            {
                                rdbtnPayCheck.Checked = true;
                            }
                            else
                            {
                                rdbtnInvoice.Checked = true;
                            }
                            if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                            {
                                divcard.Style["display"] = "none";
                                divcheck.Style["display"] = "block";
                                divpaypal.Style["display"] = "none";
                                txtCustNameForCheck.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
                                txtAccNumberForCheck.Text = GetDealerData.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                                txtBankNameForCheck.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["bankName"].ToString());
                                txtRoutingNumberForCheck.Text = GetDealerData.Tables[0].Rows[0]["bankRouting"].ToString();
                                //lblAccountType.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["AccountTypeName"].ToString());
                                ListItem liAccType = new ListItem();
                                liAccType.Text = GetDealerData.Tables[0].Rows[0]["AccountTypeName"].ToString();
                                liAccType.Value = GetDealerData.Tables[0].Rows[0]["bankAccountType"].ToString();
                                ddlAccType.SelectedIndex = ddlAccType.Items.IndexOf(liAccType);

                                //ListItem liCheckType = new ListItem();
                                //liCheckType.Text = Cardetais.Tables[0].Rows[0]["CheckTypeName"].ToString();
                                //liCheckType.Value = Cardetais.Tables[0].Rows[0]["CheckTypeID"].ToString();
                                //ddlCheckType.SelectedIndex = ddlCheckType.Items.IndexOf(liCheckType);
                                //txtCheckNumber.Text = Cardetais.Tables[0].Rows[0]["BankCheckNumber"].ToString();
                            }
                            else if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 8)
                            {
                                divcard.Style["display"] = "none";
                                divcheck.Style["display"] = "none";
                                divpaypal.Style["display"] = "block";
                                txtAttentionTo.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["InvoiceAttentionTo"].ToString());
                                if (GetDealerData.Tables[0].Rows[0]["SendInvoiceID"].ToString() == "1")
                                {
                                    rdbtnInvoiceEmail.Checked = true;
                                }
                                else
                                {
                                    rdbtnInvoiceEmail.Checked = false;
                                }
                                txtInvoiceEmail.Text = GetDealerData.Tables[0].Rows[0]["SendInvoiceEmail"].ToString();
                                txtInvoiceBillingname.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["billingName"].ToString());
                                tyxtInvoiceAddress.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["billingAdd"].ToString());
                                txtInvoiceCity.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["billingCity"].ToString());
                                txtInvoiceState.Text = GetDealerData.Tables[0].Rows[0]["billingState"].ToString();
                                txtInvoiceZip.Text = GetDealerData.Tables[0].Rows[0]["billingZip"].ToString();
                            }
                            else
                            {
                                divcard.Style["display"] = "block";
                                divcheck.Style["display"] = "none";
                                divpaypal.Style["display"] = "none";
                                txtCardholderName.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["cardholderName"].ToString());
                                txtCardholderLastName.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["cardholderLastName"].ToString());
                                CardNumber.Text = GetDealerData.Tables[0].Rows[0]["cardNumber"].ToString();
                                string EXpDate = GetDealerData.Tables[0].Rows[0]["cardExpDt"].ToString();
                                string[] EXpDt = EXpDate.Split(new char[] { '/' });

                                txtExpMon.Text = EXpDt[0].ToString();
                                txtCCExpiresYear.Text = "20" + EXpDt[1].ToString();
                                cvv.Text = GetDealerData.Tables[0].Rows[0]["cardCode"].ToString();

                                txtbillingaddress.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["billingAdd"].ToString());
                                txtbillingcity.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["billingCity"].ToString());
                                txtbillingstate.Text = GetDealerData.Tables[0].Rows[0]["State_Code"].ToString();

                                txtbillingzip.Text = GetDealerData.Tables[0].Rows[0]["billingZip"].ToString();
                            }
                            if (GetDealerData.Tables[0].Rows[0]["SchedulePaymentDate"].ToString() != "")
                            {
                                DateTime PayDate = Convert.ToDateTime(GetDealerData.Tables[0].Rows[0]["SchedulePaymentDate"].ToString());
                                txtPaymentDate.Text = PayDate.ToString("MM/dd/yyyy");
                            }
                            txtPDAmountNow.Text = GetDealerData.Tables[0].Rows[0]["Amount"].ToString();
                            txtVoicefileConfirmNo.Text = GetDealerData.Tables[0].Rows[0]["VoiceRecord"].ToString();
                            txtVoiceFileLocation.Text = GetDealerData.Tables[0].Rows[0]["VoiceFileLocationName"].ToString();
                            lblSaleID.Text = GetDealerData.Tables[0].Rows[0]["DealerUID"].ToString();
                            if (GetDealerData.Tables[0].Rows[0]["SaleDate"].ToString() != "")
                            {
                                DateTime Saledt = Convert.ToDateTime(GetDealerData.Tables[0].Rows[0]["SaleDate"].ToString());
                                lblSaleDate.Text = Saledt.ToString("MM/dd/yyyy hh:mm tt");
                            }
                            lblLocation.Text = objGeneralFunc.GetCenterCode(GetDealerData.Tables[0].Rows[0]["AgentCenterID"].ToString());
                            lblSaleAgent.Text = GetDealerData.Tables[0].Rows[0]["AgentUFirstName"].ToString();
                            if (GetDealerData.Tables[0].Rows[0]["QCID"].ToString() != "")
                            {
                                Session["AgentDealerQCQCID"] = Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["QCID"].ToString());
                            }
                            else
                            {
                                Session["AgentDealerQCQCID"] = "";
                            }
                            Session["DealerQCDealerUID"] = GetDealerData.Tables[0].Rows[0]["DealerUID"].ToString();
                            Session["AgentDealerQCPaymentID"] = GetDealerData.Tables[0].Rows[0]["PaymentID"].ToString();
                            Session["AgentDealerQCPaymentTypeID"] = GetDealerData.Tables[0].Rows[0]["pmntType"].ToString();

                            lblLeadSource.Text = objGeneralFunc.WrapTextByMaxCharacters(GetDealerData.Tables[0].Rows[0]["LeadGeneratedBy"].ToString(), 20);
                            lblLeadAgent.Text = objGeneralFunc.WrapTextByMaxCharacters(GetDealerData.Tables[0].Rows[0]["LeadAgent"].ToString(), 20);
                            lblPromotionOption.Text = GetDealerData.Tables[0].Rows[0]["PromotionOptionCode"].ToString();
                            //lblQCStatus.Text = GetDealerData.Tables[0].Rows[0]["QCStatusName"].ToString();
                            lblPaymentStatus.Text = GetDealerData.Tables[0].Rows[0]["PSStatusName"].ToString();
                            lblSaleStatus.Text = GetDealerData.Tables[0].Rows[0]["DealerStatusName"].ToString();
                            string OldNotesPay = GetDealerData.Tables[0].Rows[0]["PaymentNotes"].ToString();
                            OldNotesPay = OldNotesPay.Replace("<br>", Environment.NewLine);
                            txtPaymentNotes.Text = OldNotesPay;
                            Session["DealerQCViewPackageID"] = GetDealerData.Tables[0].Rows[0]["PackageID"].ToString();
                            if (GetDealerData.Tables[0].Rows[0]["PackageID"].ToString() != "0")
                            {
                                Double PackCost2 = new Double();
                                PackCost2 = Convert.ToDouble(GetDealerData.Tables[0].Rows[0]["Price"].ToString());
                                string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
                                string PackName2 = GetDealerData.Tables[0].Rows[0]["Description"].ToString();

                                lblPackage.Text = PackName2 + " ($" + PackAmount2 + ")";

                            }

                            if (GetDealerData.Tables[0].Rows[0]["TargetSignupDate"].ToString() != "")
                            {
                                DateTime Saledt = Convert.ToDateTime(GetDealerData.Tables[0].Rows[0]["TargetSignupDate"].ToString());
                                lblTargetDate.Text = Saledt.ToString("MM/dd/yyyy hh:mm tt");
                            }
                            if (GetDealerData.Tables[0].Rows[0]["CallbackDate"].ToString() != "")
                            {
                                DateTime Saledt = Convert.ToDateTime(GetDealerData.Tables[0].Rows[0]["CallbackDate"].ToString());
                                lblCallbackDate.Text = Saledt.ToString("MM/dd/yyyy hh:mm tt");
                            }
                            if (GetDealerData.Tables[0].Rows[0]["ContractSignDate"].ToString() != "")
                            {
                                DateTime PayDate = Convert.ToDateTime(GetDealerData.Tables[0].Rows[0]["ContractSignDate"].ToString());
                                txtContractDate.Text = PayDate.ToString("MM/dd/yyyy");
                            }
                            txtContractStatus.Text = GetDealerData.Tables[0].Rows[0]["ContractSignStatusName"].ToString();
                            txtOldQcNotes.Text = GetDealerData.Tables[0].Rows[0]["QCNotes"].ToString();
                            if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) != 8)
                            {
                                ListItem liPaySt = new ListItem();
                                liPaySt.Text = "FullyPaid";
                                liPaySt.Value = "1";
                                //ddlPmntStatus.SelectedIndex = ddlPmntStatus.Items.IndexOf(liPaySt);
                                ddlPmntStatus.Items.Remove(liPaySt);
                            }
                            else
                            {
                                if (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() != "3")
                                {
                                    if (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() != "4")
                                    {
                                        ListItem liPaySt = new ListItem();
                                        liPaySt.Text = "FullyPaid";
                                        liPaySt.Value = "1";
                                        //ddlPmntStatus.SelectedIndex = ddlPmntStatus.Items.IndexOf(liPaySt);
                                        ddlPmntStatus.Items.Remove(liPaySt);
                                    }
                                }

                            }
                            if (GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == "1")
                            {
                                lblQCStatus.Text = "QC Approved";
                                if ((GetDealerData.Tables[0].Rows[0]["SmartzStatus"].ToString() != "1") && ((GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "1") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "7") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "8")))
                                {
                                    btnMovedToSmartz.Visible = true;
                                }
                                else if ((GetDealerData.Tables[0].Rows[0]["SmartzStatus"].ToString() != "1") && ((GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "3") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "4")))
                                {
                                    btnMovedToSmartz.Visible = false;
                                }
                                else
                                {
                                    btnMovedToSmartz.Visible = false;
                                }
                                if ((GetDealerData.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "1") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "7") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "8"))
                                {
                                    btnProcess.Enabled = false;
                                    btnProcess.Visible = false;
                                    ddlPmntStatus.Visible = false;
                                    btnPmntUpdate.Visible = false;
                                    btnCheckProcess.Visible = false;
                                    btnCheckProcess.Enabled = false;
                                }
                                else
                                {
                                    if ((GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "3") || (GetDealerData.Tables[0].Rows[0]["pmntStatus"].ToString() == "4"))
                                    {
                                        if ((Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 1) || (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 2) || (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 3) || (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 4))
                                        {
                                            if (GetDealerData.Tables[0].Rows[0]["Amount"].ToString() != "")
                                            {
                                                Double TotalAmount1 = Convert.ToDouble(GetDealerData.Tables[0].Rows[0]["Amount"].ToString());
                                                string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                                                if (ChkAmount == "0.00")
                                                {
                                                    btnProcess.Enabled = false;
                                                    btnProcess.Visible = false;
                                                    btnCheckProcess.Visible = false;
                                                    btnCheckProcess.Enabled = false;
                                                }
                                                else
                                                {
                                                    btnProcess.Enabled = true;
                                                    btnProcess.Visible = true;
                                                    btnCheckProcess.Visible = false;
                                                    btnCheckProcess.Enabled = false;
                                                }
                                            }
                                            else
                                            {
                                                btnProcess.Enabled = true;
                                                btnProcess.Visible = true;
                                                btnCheckProcess.Visible = false;
                                                btnCheckProcess.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            btnProcess.Enabled = false;
                                            btnProcess.Visible = false;
                                            if (Convert.ToInt32(GetDealerData.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                                            {
                                                if (GetDealerData.Tables[0].Rows[0]["Amount"].ToString() != "")
                                                {
                                                    Double TotalAmount1 = Convert.ToDouble(GetDealerData.Tables[0].Rows[0]["Amount"].ToString());
                                                    string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                                                    if (ChkAmount == "0.00")
                                                    {
                                                        btnCheckProcess.Visible = false;
                                                        btnCheckProcess.Enabled = false;
                                                    }
                                                    else
                                                    {
                                                        btnCheckProcess.Visible = true;
                                                        btnCheckProcess.Enabled = true;
                                                    }
                                                }
                                                else
                                                {
                                                    btnCheckProcess.Visible = true;
                                                    btnCheckProcess.Enabled = true;
                                                }
                                            }
                                            else
                                            {
                                                btnCheckProcess.Visible = false;
                                                btnCheckProcess.Enabled = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        btnProcess.Enabled = false;
                                        btnProcess.Visible = false;
                                        btnCheckProcess.Visible = false;
                                        btnCheckProcess.Enabled = false;
                                    }
                                    ddlPmntStatus.Visible = true;
                                    btnPmntUpdate.Visible = true;
                                }

                            }
                            else if (GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == "2")
                            {
                                lblQCStatus.Text = "QC Reject";
                                btnProcess.Enabled = false;
                                btnProcess.Visible = false;
                                btnMovedToSmartz.Visible = false;
                            }
                            else if (GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == "4")
                            {
                                lblQCStatus.Text = "QC Returned";
                                btnProcess.Enabled = false;
                                btnProcess.Visible = false;
                                btnMovedToSmartz.Visible = false;
                            }
                            else if (GetDealerData.Tables[0].Rows[0]["QCStatusID"].ToString() == "3")
                            {
                                lblQCStatus.Text = "QC Pending";
                                btnProcess.Enabled = false;
                                btnProcess.Visible = false;
                                btnMovedToSmartz.Visible = false;
                            }
                            else
                            {
                                lblQCStatus.Text = "QC Open";
                                btnProcess.Enabled = false;
                                btnProcess.Visible = false;
                                btnMovedToSmartz.Visible = false;
                            }
                            if (GetDealerData.Tables[1].Rows.Count > 0)
                            {
                                for (int i = 0; i < GetDealerData.Tables[1].Rows.Count; i++)
                                {
                                    if (GetDealerData.Tables[1].Rows[i]["ProductOptionName"].ToString() == "Website")
                                    {
                                        if (GetDealerData.Tables[1].Rows[i]["ProductOptionStatus"].ToString() == "True")
                                        {
                                            rdbtnWebsiteYes.Checked = true;
                                        }
                                        else
                                        {
                                            rdbtnWebsiteNo.Checked = true;
                                        }
                                        txtPreferredAddress.Text = GetDealerData.Tables[1].Rows[i]["ProductOptionNotes"].ToString();
                                    }
                                    else if (GetDealerData.Tables[1].Rows[i]["ProductOptionName"].ToString() == "Cars promotion")
                                    {
                                        if (GetDealerData.Tables[1].Rows[i]["ProductOptionStatus"].ToString() == "True")
                                        {
                                            rdbtnCarsPromotionYes.Checked = true;
                                        }
                                        else
                                        {
                                            rdbtnCarsPromotionNo.Checked = true;
                                        }
                                        txtGetCarsFrom.Text = GetDealerData.Tables[1].Rows[i]["ProductOptionNotes"].ToString();
                                    }
                                    else if (GetDealerData.Tables[1].Rows[i]["ProductOptionName"].ToString() == "Leads")
                                    {
                                        if (GetDealerData.Tables[1].Rows[i]["ProductOptionStatus"].ToString() == "True")
                                        {
                                            rdbtnLeadsYes.Checked = true;
                                        }
                                        else
                                        {
                                            rdbtnLeadsNo.Checked = true;
                                        }
                                        txtLeadsToSend.Text = GetDealerData.Tables[1].Rows[i]["ProductOptionNotes"].ToString();
                                    }
                                    else if (GetDealerData.Tables[1].Rows[i]["ProductOptionName"].ToString() == "Other")
                                    {
                                        if (GetDealerData.Tables[1].Rows[i]["ProductOptionStatus"].ToString() == "True")
                                        {
                                            rdbtnOthersYes.Checked = true;
                                        }
                                        else
                                        {
                                            rdbtnOthersNo.Checked = true;
                                        }
                                        txtOthersNotes.Text = GetDealerData.Tables[1].Rows[i]["ProductOptionNotes"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }







    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // your code!
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
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


    private void FillPayCancelReason()
    {
        try
        {
            ddlPayCancelReason.Items.Clear();
            DataSet dsReason = objHotLeadBL.GetPmntCancelReasons();
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






    private void FillTargetDate(DataSet dsDatetime)
    {
        try
        {
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ddlTargetDate.Items.Clear();
            for (int i = 0; i < 31; i++)
            {
                ListItem list = new ListItem();
                list.Text = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                list.Value = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                ddlTargetDate.Items.Add(list);
            }
            ddlTargetDate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillCallbackDate(DataSet dsDatetime)
    {
        try
        {
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ddlCallbackDate.Items.Clear();
            for (int i = 0; i < 31; i++)
            {
                ListItem list = new ListItem();
                list.Text = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                list.Value = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                ddlCallbackDate.Items.Add(list);
            }
            ddlCallbackDate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnProspectSave_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("QCDataEditForDealer.aspx");
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
            if ((Session["ViewDealerQCStatus"].ToString() != "0") || (Session["ViewDealerQCStatus"].ToString() == ""))
            {
                if ((Session["ViewDealerQCStatus"].ToString() != "1"))
                {
                    Session["DealerEditSaleID"] = null;
                    Session["ViewDealerQCStatus"] = "";
                    Response.Redirect("QCReportForDealer.aspx");
                }
                else
                {
                    Session["DealerEditSaleID"] = null;
                    Session["ViewDealerQCStatus"] = "";
                    mpealteruserUpdated.Hide();
                    Response.Redirect("QCDataViewForDealer.aspx");
                }
            }
            else
            {
                Session["DealerEditSaleID"] = null;
                mpealteruserUpdated.Hide();
                Response.Redirect("QCDataViewForDealer.aspx");
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerQCDealerSaleID"] = null;
            Response.Redirect("QCReportForDealer.aspx");
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
            Session["DealerEditSaleID"] = null;
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
    protected void btnQCUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int Status = Convert.ToInt32(ddlQCStatus.SelectedItem.Value);
            Session["ViewDealerQCStatus"] = Status;
            if (Status != 0)
            {
                UpdateQCStatus(Status);
                mpealteruserUpdated.Show();
                lblErrUpdated.Visible = true;
                lblErrUpdated.Text = "QC Details updated successfully";
            }
            else
            {
                mpealteruserUpdated.Show();
                lblErrUpdated.Visible = true;
                lblErrUpdated.Text = "Please select qc status to update";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void UpdateQCStatus(int Status)
    {
        try
        {
            int QCBY = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int QCID = 0;
            if ((Session["AgentDealerQCQCID"] == null) || (Session["AgentDealerQCQCID"].ToString() == ""))
            {
                QCID = Convert.ToInt32(0);
            }
            else
            {
                QCID = Convert.ToInt32(Session["AgentDealerQCQCID"].ToString());
            }
            string QCNotes = string.Empty;
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            String UpdatedBy = Session[Constants.NAME].ToString();
            if (txtQCNotes.Text.Trim() != "")
            {
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
                if (txtOldQcNotes.Text.Trim() != "")
                {
                    QCNotes = txtOldQcNotes.Text.Trim() + "\n" + UpdateByWithDate + txtQCNotes.Text.Trim() + "\n" + "-------------------------------------------------";
                }
                else
                {
                    QCNotes = UpdateByWithDate + txtQCNotes.Text.Trim() + "\n" + "-------------------------------------------------";
                }
            }
            else
            {
                QCNotes = txtOldQcNotes.Text.Trim();
            }
            int DealerSaleID = Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString());
            int DealerUID = Convert.ToInt32(Session["DealerQCDealerUID"].ToString());
            DataSet QCUpdateds = objHotLeadBL.UpdateQCStatusForDealer(QCID, QCNotes, Status, DealerSaleID, QCBY, DealerUID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnPmntUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int pmntID = Convert.ToInt32(Session["AgentDealerQCPaymentID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int PSStatusID = Convert.ToInt32(ddlPmntStatus.SelectedItem.Value);
            string TransactionID = "";
            DateTime dtPayDate;
            dtPayDate = Convert.ToDateTime("1/1/1990");
            string Amount = string.Empty;
            Amount = "0";
            int PayCancelReason = Convert.ToInt32(ddlPayCancelReason.SelectedItem.Value);
            string PaymentNotes = string.Empty;
            String UpdatedBy = Session[Constants.NAME].ToString();
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            if (PSStatusID == 1)
            {
                TransactionID = "";
                dtPayDate = Convert.ToDateTime(txtPaymentDate.Text);
                Amount = txtPDAmountNow.Text;
            }
            if (txtPaymentNotesNew.Text.Trim() != "")
            {
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
                if (txtPaymentNotes.Text.Trim() != "")
                {
                    PaymentNotes = txtPaymentNotes.Text.Trim() + "\n" + UpdateByWithDate + txtPaymentNotesNew.Text.Trim() + "\n" + "-------------------------------------------------";
                }
                else
                {
                    PaymentNotes = UpdateByWithDate + txtPaymentNotesNew.Text.Trim() + "\n" + "-------------------------------------------------";
                }
            }
            else
            {
                PaymentNotes = txtPaymentNotes.Text.Trim();
            }
            DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatusForDealer(pmntID, PSStatusID, TransactionID, dtPayDate, PayCancelReason, Amount, UID, PaymentNotes);
            string AuthNetTransID = "";
            string Result = ddlPmntStatus.SelectedItem.Text;
            if ((Session["AgentDealerQCPaymentTypeID"].ToString() == "1") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "2") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "3") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "4"))
            {
                SavePayTransInfo(AuthNetTransID, Result);
            }
            else if (Session["AgentDealerQCPaymentTypeID"].ToString() == "8")
            {
            }
            else
            {
                SavePayTransInfoForChecks(AuthNetTransID, Result);
            }
            mdepAlertExists.Show();
            lblErrorExists.Visible = true;
            lblErrorExists.Text = "Payment status updated successfully";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void SavePayTransInfo(string AuthNetTransID, string Result)
    {
        try
        {
            int DealerSaleID = Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString());
            int PayTryBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            string CardType = string.Empty;

            if (Session["AgentDealerQCPaymentTypeID"].ToString() == "1")
            {
                CardType = "Visa";
            }
            else if (Session["AgentDealerQCPaymentTypeID"].ToString() == "2")
            {
                CardType = "Mastercard";
            }
            else if (Session["AgentDealerQCPaymentTypeID"].ToString() == "3")
            {
                CardType = "Amex";
            }
            else
            {
                CardType = "Discover";
            }
            string CCardNumber = CardNumber.Text;
            string Address = txtbillingaddress.Text;
            string City = txtbillingcity.Text;
            string State = txtbillingstate.Text;
            string Zip = txtbillingzip.Text;
            string Amount = txtPDAmountNow.Text;
            string CCExpiryDate = txtExpMon.Text + "/" + txtCCExpiresYear.Text;
            string CardCvv = cvv.Text;
            string CCFirstName = txtCardholderName.Text;
            string CCLastName = txtCardholderLastName.Text;
            DataSet dsSavePayTrans = objHotLeadBL.SavePaymentHistoryDataForDealer(DealerSaleID, PayTryBy, CardType, CCardNumber, Address, City, State,
                Zip, Amount, Result, AuthNetTransID, CCExpiryDate, CardCvv, CCFirstName, CCLastName);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void SavePayTransInfoForChecks(string AuthNetTransID, string Result)
    {
        try
        {
            int DealerSaleID = Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString());
            int PayTryBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            string CardType = string.Empty;
            CardType = "Check";
            string Address = txtAddress.Text;
            string City = txtCity.Text;
            string State = txtbillingstate.Text;
            string Zip = txtZip.Text;
            string Amount = txtPDAmountNow.Text;
            string AccountHolderName = txtCustNameForCheck.Text;
            string AccountNumber = txtAccNumberForCheck.Text;
            string BankName = txtBankNameForCheck.Text;
            string RoutingNumber = txtRoutingNumberForCheck.Text;
            string AccountType = ddlAccType.SelectedItem.Text;

            DataSet dsSavePayTrans = objHotLeadBL.SavePaymentHistoryDataForChecksForDealer(DealerSaleID, PayTryBy, CardType, Address, City, State,
                Zip, Amount, Result, AuthNetTransID, AccountHolderName, AccountNumber, BankName, RoutingNumber, AccountType);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            // AuthorizePayment();
            int DealerSaleID = Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString());
            string CCFirstName = txtCardholderName.Text;
            string CCLastName = txtCardholderLastName.Text;
            string CCAddress = txtbillingaddress.Text;
            string CCZip = txtbillingzip.Text;
            string CCNumber = CardNumber.Text;
            string CCCvv = cvv.Text;
            string CCExpiry = txtExpMon.Text + "/" + txtCCExpiresYear.Text;
            string CCAmount = txtPDAmountNow.Text;
            string CCCity = txtbillingcity.Text;
            string CCState = txtbillingstate.Text;
            DataSet dsChkRejectThere = objHotLeadBL.CheckResultPaymentRejectForDealer(CCFirstName, CCLastName, CCAddress, CCZip, CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, DealerSaleID);
            if (dsChkRejectThere.Tables.Count > 0)
            {
                if (dsChkRejectThere.Tables[0].Rows.Count > 0)
                {
                    DateTime dtTranDt = Convert.ToDateTime(dsChkRejectThere.Tables[0].Rows[0]["PayTryDatetime"].ToString());
                    string DtTranDate = dtTranDt.ToString("MM/dd/yy hh:mm tt");
                    lblRejectThereError.Visible = true;
                    lblRejectThereError.Text = "We have already attempted to process the payment earlier at " + DtTranDate + " with the same data. No payment information is updated since then. Do you want to try again?";
                    mdepAlertRejectThere.Show();
                }
                else
                {
                    AuthorizePayment();
                }
            }
            else
            {
                AuthorizePayment();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnRejectThereYes_Click(object sender, EventArgs e)
    {
        try
        {
            AuthorizePayment();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private bool AuthorizePayment()
    {
        //CustomValidator1.ErrorMessage = "";
        string AuthNetVersion = "3.1"; // Contains CCV support
        string AuthNetLoginID = "9FtTpx88g879"; //Set your AuthNetLoginID here
        string AuthNetTransKey = "9Gp3Au9t97Wvb784";  // Get this from your authorize.net merchant interface

        WebClient webClientRequest = new WebClient();
        System.Collections.Specialized.NameValueCollection InputObject = new System.Collections.Specialized.NameValueCollection(30);
        System.Collections.Specialized.NameValueCollection ReturnObject = new System.Collections.Specialized.NameValueCollection(30);

        byte[] ReturnBytes;
        string[] ReturnValues;
        string ErrorString;
        //(TESTMODE) Bill To Company is required. (33) 

        InputObject.Add("x_version", AuthNetVersion);
        InputObject.Add("x_delim_data", "True");
        InputObject.Add("x_login", AuthNetLoginID);
        InputObject.Add("x_tran_key", AuthNetTransKey);
        InputObject.Add("x_relay_response", "False");

        //----------------------Set to False to go Live--------------------
        InputObject.Add("x_test_request", "False");
        //---------------------------------------------------------------------
        InputObject.Add("x_delim_char", ",");
        InputObject.Add("x_encap_char", "|");

        //Billing Address
        InputObject.Add("x_first_name", txtCardholderName.Text);
        InputObject.Add("x_last_name", txtCardholderLastName.Text);
        InputObject.Add("x_phone", txtPhone.Text);
        InputObject.Add("x_address", txtbillingaddress.Text);
        InputObject.Add("x_city", txtbillingcity.Text);
        InputObject.Add("x_state", txtbillingstate.Text);
        InputObject.Add("x_zip", txtbillingzip.Text);

        if (txtEmail.Text != "")
        {
            InputObject.Add("x_email", txtEmail.Text);
        }
        else
        {
            InputObject.Add("x_email", "info@unitedcarexchange.com");
        }

        InputObject.Add("x_email_customer", "TRUE");                     //Emails Customer
        InputObject.Add("x_merchant_email", "shravan@datumglobal.net");  //Emails Merchant
        InputObject.Add("x_country", "USA");
        InputObject.Add("x_customer_ip", Request.UserHostAddress);  //Store Customer IP Address

        //Amount
        string Package = string.Empty;
        if (Session["DealerQCViewPackageID"].ToString() == "5")
        {
            Package = "Gold Deluxe Promo Package – No cancellations allowed; All sales are final";
        }
        else if (Session["DealerQCViewPackageID"].ToString() == "4")
        {
            Package = "Silver Deluxe Promo Package – no cancellations and no refunds allowed; All sales are final";
        }
        else
        {
            Package = lblPackage.Text;
        }
        //var string = $('#ddlPackage option:selected').text();
        //var p =string.split('$');
        //var pp = p[1].split(')');
        ////alert(pp[0]);
        ////pp[0] = parseInt(pp[0]);
        //pp[0] = parseFloat(pp[0]);
        //var selectedPack = pp[0].toFixed(2);
        string PackCost = lblPackage.Text;
        string[] Pack = PackCost.Split('$');
        string[] FinalAmountSpl = Pack[1].Split(')');
        string FinalAmount = FinalAmountSpl[0].ToString();
        if (Convert.ToDouble(FinalAmount) != Convert.ToDouble(txtPDAmountNow.Text))
        {
            Package = Package + "- Partial payment -";
        }

        InputObject.Add("x_description", "Payment to " + Package);
        InputObject.Add("x_invoice_num", txtVoicefileConfirmNo.Text);
        //string.Format("{0:00}", Convert.ToDecimal(lblAdPrice2.Text))) + "Dollars
        //Description of Purchase

        //lblPackDescrip.Text 
        //Card Details
        InputObject.Add("x_card_num", CardNumber.Text);
        InputObject.Add("x_exp_date", txtExpMon.Text + "/" + txtCCExpiresYear.Text);
        InputObject.Add("x_card_code", cvv.Text);

        InputObject.Add("x_method", "CC");
        InputObject.Add("x_type", "AUTH_CAPTURE");

        InputObject.Add("x_amount", string.Format("{0:c2}", Convert.ToDouble(txtPDAmountNow.Text)));

        //InputObject.Add("x_amount", string.Format("{0:c2}", lblAdPrice2));
        // Currency setting. Check the guide for other supported currencies
        InputObject.Add("x_currency_code", "USD");


        try
        {
            //Actual Server
            //Set above Testmode=off to go live
            webClientRequest.BaseAddress = "https://secure.authorize.net/gateway/transact.dll";

            ReturnBytes = webClientRequest.UploadValues(webClientRequest.BaseAddress, "POST", InputObject);
            ReturnValues = System.Text.Encoding.ASCII.GetString(ReturnBytes).Split(",".ToCharArray());

            if (ReturnValues[0].Trim(char.Parse("|")) == "1")
            {

                ///Successs 

                string AuthNetCode = ReturnValues[4].Trim(char.Parse("|")); // Returned Authorisation Code
                string AuthNetTransID = ReturnValues[6].Trim(char.Parse("|")); // Returned Transaction ID

                //Response.Redirect("PaymentSucces.aspx?NetCode=" + ReturnValues[4].Trim(char.Parse("|")) + "&tx=" + ReturnValues[6].Trim(char.Parse("|")) + "&amt=" + txtPDAmountNow.Text + "&item_number=" + Session["PackageID"].ToString() + "");

                string PayInfo = "Authorisation Code" + ReturnValues[4].Trim(char.Parse("|")) + "</br>TransID=" + ReturnValues[6].Trim(char.Parse("|")) + "</br>Do you want to move the sale to smartz?"; // Returned Authorisation Code;
                String UpdatedBy = Session[Constants.NAME].ToString();
                DataSet dsDatetime = objHotLeadBL.GetDatetime();
                DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
                string PayNotes = UpdatedBy + "-" + dtNow.ToString("MM/dd/yyyy hh:mm tt") + " <br>Payment Successfully processed for $" + txtPDAmountNow.Text + "  <br>Authorisation Code " + ReturnValues[4].Trim(char.Parse("|")) + " <br> TransID=" + ReturnValues[6].Trim(char.Parse("|")) + "<br> " + "-------------------------------------------------"; // Returned Authorisation Code;                
                string Result = "Paid";
                string PackCost1 = lblPackage.Text;
                string[] Pack1 = PackCost1.Split('$');
                string[] FinalAmountSpl1 = Pack1[1].Split(')');
                string FinalAmount1 = FinalAmountSpl1[0].ToString();
                if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                {
                    Result = "NoPayDue";
                }
                else if (Convert.ToDouble(FinalAmount1) != Convert.ToDouble(txtPDAmountNow.Text))
                {
                    Result = "PartialPaid";
                }
                else
                {
                    Result = "Paid";
                }
                SavePayInfo(AuthNetTransID, PayNotes, Result);
                SavePayTransInfo(AuthNetTransID, Result);
                lblMoveSmartz.Text = PayInfo;
                lblMoveSmartz.Visible = true;
                mdepalertMoveSmartz.Show();
                return true;
            }
            else
            {

                ///Failure
                // Error!
                ErrorString = ReturnValues[3].Trim(char.Parse("|")) + " (" + ReturnValues[2].Trim(char.Parse("|")) + ") " + ReturnValues[4].Trim(char.Parse("|"));

                if (ReturnValues[2].Trim(char.Parse("|")) == "44")
                {
                    // CCV transaction decline
                    ErrorString += "Credit Card Code Verification (CCV) returned the following error: ";

                    switch (ReturnValues[38].Trim(char.Parse("|")))
                    {
                        case "N":
                            ErrorString += "Card Code does not match.";
                            break;
                        case "P":
                            ErrorString += "Card Code was not processed.";
                            break;
                        case "S":
                            ErrorString += "Card Code should be on card but was not indicated.";
                            break;
                        case "U":
                            ErrorString += "Issuer was not certified for Card Code.";
                            break;
                    }
                }

                if (ReturnValues[2].Trim(char.Parse("|")) == "45")
                {
                    if (ErrorString.Length > 1)
                        ErrorString += "<br />n";

                    // AVS transaction decline
                    ErrorString += "Address Verification System (AVS) " +
                      "returned the following error: ";

                    switch (ReturnValues[5].Trim(char.Parse("|")))
                    {
                        case "A":
                            ErrorString += " the zip code entered does not match the billing address.";
                            break;
                        case "B":
                            ErrorString += " no information was provided for the AVS check.";
                            break;
                        case "E":
                            ErrorString += " a general error occurred in the AVS system.";
                            break;
                        case "G":
                            ErrorString += " the credit card was issued by a non-US bank.";
                            break;
                        case "N":
                            ErrorString += " neither the entered street address nor zip code matches the billing address.";
                            break;
                        case "P":
                            ErrorString += " AVS is not applicable for this transaction.";
                            break;
                        case "R":
                            ErrorString += " please retry the transaction; the AVS system was unavailable or timed out.";
                            break;
                        case "S":
                            ErrorString += " the AVS service is not supported by your credit card issuer.";
                            break;
                        case "U":
                            ErrorString += " address information is unavailable for the credit card.";
                            break;
                        case "W":
                            ErrorString += " the 9 digit zip code matches, but the street address does not.";
                            break;
                        case "Z":
                            ErrorString += " the zip code matches, but the address does not.";
                            break;
                    }
                }

                Session["PayCancelError"] = ErrorString;
                int PaymentID = Convert.ToInt32(Session["AgentDealerQCPaymentID"].ToString());
                int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                int PSStatusID = Convert.ToInt32(3);
                //DataSet dsUpdatePaynotes = objHotLeadBL.UpdateQCPayNotesForProcessButton(PSID, UID, ErrorString, PSStatusID, PmntStatus, PaymentID);
                string AuthNetTransID = "";
                string Result = "Pending";
                // SavePayTransInfo(AuthNetTransID, Result);
                ErrorString = "Payment is DECLINED <br /> " + ErrorString;
                lblErr.Text = ErrorString;
                mpealteruser.Show();

                // ErrorString contains the actual error
                //CustomValidator1.ErrorMessage = ErrorString;
                return false;
            }
        }
        catch (Exception ex)
        {
            //CustomValidator1.ErrorMessage = ex.Message;
            return false;
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int PaymentID = Convert.ToInt32(Session["AgentDealerQCPaymentID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            string ErrorString = Session["PayCancelError"].ToString();
            String UpdatedBy = Session[Constants.NAME].ToString();
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ErrorString = UpdatedBy + "-" + dtNow.ToString("MM/dd/yyyy hh:mm tt") + " <br>" + ErrorString + " <br>" + "-------------------------------------------------";
            int PSStatusID = Convert.ToInt32(3);
            string Result = "Pending";
            if (rdbtnPmntReturned.Checked == true)
            {
                PSStatusID = 5;
                Result = "Returned";
            }
            else if (rdbtnPmntReject.Checked == true)
            {
                PSStatusID = 2;
                Result = "Reject";
            }
            else
            {
                PSStatusID = 3;
                Result = "Pending";
            }
            DataSet dsUpdatePaynotes = objHotLeadBL.UpdateQCPayNotesForProcessButtonForDealers(UID, ErrorString, PSStatusID, PaymentID);
            string AuthNetTransID = "";
            if ((Session["AgentDealerQCPaymentTypeID"].ToString() == "1") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "2") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "3") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "4"))
            {
                SavePayTransInfo(AuthNetTransID, Result);
            }
            else
            {
                SavePayTransInfoForChecks(AuthNetTransID, Result);
            }
            if (PSStatusID == 3)
            {
                Response.Redirect("QCDataViewForDealer.aspx");
            }
            else
            {
                Response.Redirect("AgentDealerReport.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void SavePayInfo(string AuthNetTransID, string PayInfo, string Result)
    {
        try
        {
            int PaymentID = Convert.ToInt32(Session["AgentDealerQCPaymentID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int PSStatusID = Convert.ToInt32(1);
            string TransactionID = AuthNetTransID;
            string Amount = string.Empty;
            Amount = txtPDAmountNow.Text;
            string PaymentNotes = PayInfo;
            DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatusForProcessButtonForDealer(PaymentID, PSStatusID, TransactionID, Amount, UID, PaymentNotes);
        }
        catch (Exception ex)
        {
        }
    }
    protected void MoveSmartz_Click(object sender, EventArgs e)
    {
        try
        {
            MdepEnterDEalerCode.Show();
            txtDealerCode.Text = "";
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
    protected void btnMoveSmartzNo_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("QCDataViewForDealer.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCheckProcess_Click(object sender, EventArgs e)
    {
        try
        {
            GoWithCheck();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void GoWithCheck()
    {
        try
        {
            // By default, this sample code is designed to post to our test server for
            // developer accounts: https://test.authorize.net/gateway/transact.dll
            // for real accounts (even in test mode), please make sure that you are
            string post_url = "https://secure.authorize.net/gateway/transact.dll";
            //String post_url = "https://test.authorize.net/gateway/transact.dll";

            //The valid routing number of the customer’s bank 9 digits
            string sBankCode = string.Empty;
            sBankCode = txtRoutingNumberForCheck.Text;

            //The customer’s valid bank account number Up to 20 digits The customer’s checking,
            string sBankaccountnumber = string.Empty;
            sBankaccountnumber = txtAccNumberForCheck.Text;
            //The type of bank account CHECKING,BUSINESSCHECKING,SAVINGS
            string sBankType = ddlAccType.SelectedItem.Text;


            //The name of the bank that holds the customer’s account Up to 50 characters
            string sbank_name = txtBankNameForCheck.Text;

            //The name of the bank that holds the customer’s account Up to 50 characters
            string sbank_acct_name = txtCustNameForCheck.Text;
            //The type of electronic check payment request.Types," page 10 of this document.
            //ARC, BOC, CCD, PPD, TEL,WEB
            string echeck_type = "TEL";

            string sbank_check_number = "";




            string AuthNetVersion = "3.1"; // Contains CCV support
            string AuthNetLoginID = "9FtTpx88g879"; //Set your AuthNetLoginID here
            string AuthNetTransKey = "9Gp3Au9t97Wvb784";  // Get this from your authorize.net merchant interface


            Dictionary<string, string> post_values = new Dictionary<string, string>();
            //the API Login ID and Transaction Key must be replaced with valid values

            post_values.Add("x_login", AuthNetLoginID);
            post_values.Add("x_tran_key", AuthNetTransKey);
            post_values.Add("x_delim_data", "TRUE");
            post_values.Add("x_delim_char", "|");
            post_values.Add("x_relay_response", "FALSE");

            post_values.Add("x_type", "AUTH_CAPTURE");
            post_values.Add("x_method", "ECHECK");
            post_values.Add("x_bank_aba_code", sBankCode);
            post_values.Add("x_bank_acct_num", sBankaccountnumber);
            post_values.Add("x_bank_acct_type", sBankType);

            post_values.Add("x_bank_name", sbank_name);
            post_values.Add("x_bank_acct_name", sbank_acct_name);
            post_values.Add("x_echeck_type", echeck_type);
            post_values.Add("x_bank_check_number", sbank_check_number);

            post_values.Add("x_recurring_billing", "False");

            string Package = string.Empty;
            if (Session["DealerQCViewPackageID"].ToString() == "5")
            {
                Package = "Gold Deluxe Promo Package – No cancellations allowed; All sales are final";
            }
            else if (Session["DealerQCViewPackageID"].ToString() == "4")
            {
                Package = "Silver Deluxe Promo Package – no cancellations and no refunds allowed; All sales are final";
            }
            else
            {
                Package = lblPackage.Text;
            }

            string PackCost = lblPackage.Text;
            string[] Pack = PackCost.Split('$');
            string[] FinalAmountSpl = Pack[1].Split(')');
            string FinalAmount = FinalAmountSpl[0].ToString();
            if (Convert.ToDouble(FinalAmount) != Convert.ToDouble(txtPDAmountNow.Text))
            {
                Package = Package + "- Partial payment -";
            }

            post_values.Add("x_amount", string.Format("{0:c2}", Convert.ToDouble(txtPDAmountNow.Text)));
            //post_values.Add("x_amount", txtPDAmountNow.Text);
            post_values.Add("x_description", Package);
            post_values.Add("x_merchant_email", "shravan@datumglobal.net");  //Emails Merchant
            post_values.Add("x_first_name", txtCustNameForCheck.Text);
            post_values.Add("x_last_name", txtDealerContactName.Text);
            post_values.Add("x_address", txtAddress.Text);
            post_values.Add("x_state", txtLocationState.Text);
            post_values.Add("x_zip", txtZip.Text);
            post_values.Add("x_city", txtCity.Text);
            post_values.Add("x_phone", txtContactPhone.Text);
            // Additional fields can be added here as outlined in the AIM integration
            // guide at: http://developer.authorize.net

            // This section takes the input fields and converts them to the proper format
            // for an http post.  For example: "x_login=username&x_tran_key=a1B2c3D4"
            String post_string = "";

            foreach (KeyValuePair<string, string> post_value in post_values)
            {
                post_string += post_value.Key + "=" + HttpUtility.UrlEncode(post_value.Value) + "&";
            }
            post_string = post_string.TrimEnd('&');

            // The following section provides an example of how to add line item details to
            // the post string.  Because line items may consist of multiple values with the
            // same key/name, they cannot be simply added into the above array.
            //
            // This section is commented out by default.
            /*
            string[] line_items = {
                "item1<|>golf balls<|><|>2<|>18.95<|>Y",
                "item2<|>golf bag<|>Wilson golf carry bag, red<|>1<|>39.99<|>Y",
                "item3<|>book<|>Golf for Dummies<|>1<|>21.99<|>Y"};
            foreach( string value in line_items )
            {
                post_string += "&x_line_item=" + HttpUtility.UrlEncode(value);
            }
            */

            // create an HttpWebRequest object to communicate with Authorize.net
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(post_url);
            objRequest.Method = "POST";
            objRequest.ContentLength = post_string.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            // post data is sent as a stream
            StreamWriter myWriter = null;
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(post_string);
            myWriter.Close();

            // returned values are returned as a stream, then read into a string
            String post_response;
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
            {
                post_response = responseStream.ReadToEnd();
                responseStream.Close();
            }

            // the response string is broken into an array
            // The split character specified here must match the delimiting character specified above
            Array response_array = post_response.Split('|');
            string resultSpan = string.Empty;
            // the results are output to the screen in the form of an html numbered list.
            resultSpan += response_array.GetValue(3) + "(Response Code " + response_array.GetValue(0) + ")" + "(Response Reason Code " + response_array.GetValue(2) + ")";
            //foreach (string value in response_array)
            //{
            //    resultSpan += "<LI>" + value + "&nbsp;</LI> \n";
            //}
            //resultSpan += "</OL> \n";
            // individual elements of the array could be accessed to read certain response
            // fields.  For example, response_array[0] would return the Response Code,
            // response_array[2] would return the Response Reason Code.
            // for a list of response fields, please review the AIM Implementation Guide
            if (response_array.GetValue(0).ToString() == "1")
            {
                //Success
                //string AuthNetCode = ReturnValues[4].Trim(char.Parse("|")); // Returned Authorisation Code
                string AuthNetTransID = response_array.GetValue(6).ToString(); // Returned Transaction ID

                //Response.Redirect("PaymentSucces.aspx?NetCode=" + ReturnValues[4].Trim(char.Parse("|")) + "&tx=" + ReturnValues[6].Trim(char.Parse("|")) + "&amt=" + txtPDAmountNow.Text + "&item_number=" + Session["PackageID"].ToString() + "");

                string PayInfo = "TransID=" + AuthNetTransID + "</br>Do you want to move the sale to smartz?"; // Returned Authorisation Code;
                string PayNotes = "TransID=" + AuthNetTransID; // Returned Authorisation Code;                
                string Result = "Paid";
                string PackCost1 = lblPackage.Text;
                string[] Pack1 = PackCost1.Split('$');
                string[] FinalAmountSpl1 = Pack1[1].Split(')');
                string FinalAmount1 = FinalAmountSpl1[0].ToString();
                if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                {
                    Result = "NoPayDue";
                }
                else if (Convert.ToDouble(FinalAmount1) != Convert.ToDouble(txtPDAmountNow.Text))
                {
                    Result = "PartialPaid";
                }
                else
                {
                    Result = "Paid";
                }
                SavePayInfo(AuthNetTransID, PayNotes, Result);
                SavePayTransInfoForChecks(AuthNetTransID, Result);
                lblMoveSmartz.Text = PayInfo;
                lblMoveSmartz.Visible = true;
                mdepalertMoveSmartz.Show();
                //return true;
            }
            else
            {
                Session["PayCancelError"] = resultSpan;
                int PaymentID = Convert.ToInt32(Session["AgentDealerQCPaymentID"].ToString());
                int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                int PSStatusID = Convert.ToInt32(3);
                int PmntStatus = 1;
                //DataSet dsUpdatePaynotes = objHotLeadBL.UpdateQCPayNotesForProcessButton(PSID, UID, ErrorString, PSStatusID, PmntStatus, PaymentID);
                string AuthNetTransID = "";
                string Result = "Pending";
                // SavePayTransInfo(AuthNetTransID, Result);
                resultSpan = "Payment is DECLINED <br /> " + resultSpan;
                lblErr.Text = resultSpan;
                mpealteruser.Show();

                // ErrorString contains the actual error
                //CustomValidator1.ErrorMessage = ErrorString;
                //return false;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnPaymentHistory_Click(object sender, EventArgs e)
    {
        try
        {
            if ((Session["AgentDealerQCPaymentTypeID"].ToString() == "1") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "2") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "3") || (Session["AgentDealerQCPaymentTypeID"].ToString() == "4"))
            {
                divresults.Style["display"] = "block";
                divCheckResults.Style["display"] = "none";
                int DealerSaleID = Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString());
                DataSet PayHistory = objHotLeadBL.GetPaymentTransactionDataForDealer(DealerSaleID);
                if (PayHistory.Tables.Count > 0)
                {
                    if (PayHistory.Tables[0].Rows.Count > 0)
                    {
                        grdIntroInfo.Visible = true;
                        grdIntroInfo.DataSource = PayHistory.Tables[0];
                        grdIntroInfo.DataBind();
                        lblPayTransSaleID.Text = Session["DealerQCDealerUID"].ToString();
                        mpeaSalesData.Show();
                    }
                    else
                    {
                        lblNotransError.Text = "Transaction history not available";
                        lblNotransError.Visible = true;
                        mdepNoTransHis.Show();
                    }
                }
                else
                {
                    lblNotransError.Text = "Transaction history not available";
                    lblNotransError.Visible = true;
                    mdepNoTransHis.Show();
                }
            }
            else
            {
                divresults.Style["display"] = "none";
                divCheckResults.Style["display"] = "block";
                int DealerSaleID = Convert.ToInt32(Session["DealerQCDealerSaleID"].ToString());
                DataSet PayHistory = objHotLeadBL.GetPaymentTransactionDataForChecksForDealer(DealerSaleID);
                if (PayHistory.Tables.Count > 0)
                {
                    if (PayHistory.Tables[0].Rows.Count > 0)
                    {
                        grdCheckResults.Visible = true;
                        grdCheckResults.DataSource = PayHistory.Tables[0];
                        grdCheckResults.DataBind();
                        lblPayTransSaleID.Text = Session["DealerQCDealerUID"].ToString();
                        mpeaSalesData.Show();
                    }
                    else
                    {
                        lblNotransError.Text = "Transaction history not available";
                        lblNotransError.Visible = true;
                        mdepNoTransHis.Show();
                    }
                }
                else
                {
                    lblNotransError.Text = "Transaction history not available";
                    lblNotransError.Visible = true;
                    mdepNoTransHis.Show();
                }
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

                HiddenField hdnTransCardNum = (HiddenField)e.Row.FindControl("hdnTransCardNum");
                Label lblTransCardNum = (Label)e.Row.FindControl("lblTransCardNum");
                string CardNumber = hdnTransCardNum.Value;
                if (CardNumber.Length > 6)
                {
                    CardNumber = CardNumber.Substring(CardNumber.Length - 6, 6);
                }
                lblTransCardNum.Text = CardNumber;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdCheckResults_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HiddenField hdnTransCardNum = (HiddenField)e.Row.FindControl("hdnCheckTransAccNum");
                Label lblTransCardNum = (Label)e.Row.FindControl("lblCheckTransAccNum");
                string CardNumber = hdnTransCardNum.Value;
                //if (CardNumber.Length > 6)
                //{
                //    CardNumber = CardNumber.Substring(CardNumber.Length - 6, 6);
                //}
                lblTransCardNum.Text = CardNumber;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
