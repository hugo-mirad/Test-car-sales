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

public partial class NewDealerSale : System.Web.UI.Page
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
                Session["CurrentPage"] = "New sale";

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

                    Session["NewDealerPaymentID"] = null;
                    Session["NewDealerContactID"] = null;
                    Session["NewDealerUID"] = null;
                    Session["NewDealerSaleID"] = null;
                    Session["NewDealerCoordinatorID"] = null;
                    lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");
                    DataSet dsYears = objHotLeadBL.USP_GetNext12years();

                    fillYears(dsYears);

                    DataSet dsDatetime = objHotLeadBL.GetDatetime();
                    FillStates();

                    FillPromotionOptions();
                    //FillPaymentDate();
                    //FillPDDate(dsDatetime);
                    FillPaymentDate(dsDatetime);
                    FillTargetDate(dsDatetime);
                    FillCallbackDate(dsDatetime);
                    FillContractSignDate(dsDatetime);
                    FillBillingStates();
                    FillInvoiceBillingStates();
                    FillAgents();
                    FillVoiceFileLocation();
                    // FillCheckTypes();
                    if ((Session["DealerEditSaleID"] != null) && (Session["DealerEditSaleID"].ToString() != ""))
                    {
                        DataSet GetDealerData = objHotLeadBL.GetDealerDetailsByDealerSaleID(Convert.ToInt32(Session["DealerEditSaleID"].ToString()));
                        if (GetDealerData.Tables.Count > 0)
                        {
                            if (GetDealerData.Tables[0].Rows.Count > 0)
                            {
                                if (GetDealerData.Tables[2].Rows.Count > 0)
                                {
                                    RepeaterSurveyEdit.Visible = true;
                                    RepeterSurvey.Visible = false;
                                    RepeaterSurveyEdit.DataSource = GetDealerData.Tables[2];
                                    RepeaterSurveyEdit.DataBind();
                                }
                                else
                                {
                                    FillSurveyQuestions();
                                }
                                if (GetDealerData.Tables[0].Rows[0]["PackageID"].ToString() != "0")
                                {
                                    Double PackCost2 = new Double();
                                    PackCost2 = Convert.ToDouble(GetDealerData.Tables[0].Rows[0]["Price"].ToString());
                                    string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
                                    string PackName2 = GetDealerData.Tables[0].Rows[0]["Description"].ToString();
                                    ListItem listPack = new ListItem();
                                    listPack.Value = GetDealerData.Tables[0].Rows[0]["PackageID"].ToString();
                                    listPack.Text = PackName2 + " ($" + PackAmount2 + ")";
                                    ddlPackage.SelectedIndex = ddlPackage.Items.IndexOf(listPack);
                                }

                                ListItem liSaleAgent = new ListItem();
                                liSaleAgent.Text = GetDealerData.Tables[0].Rows[0]["AgentUFirstName"].ToString();
                                liSaleAgent.Value = GetDealerData.Tables[0].Rows[0]["UCEDealerCoordinatorID"].ToString();
                                ddlSaleAgent.SelectedIndex = ddlSaleAgent.Items.IndexOf(liSaleAgent);
                                txtLeadSource.Text = GetDealerData.Tables[0].Rows[0]["LeadGeneratedBy"].ToString();
                                txtLeadAgent.Text = GetDealerData.Tables[0].Rows[0]["LeadAgent"].ToString();

                                Double PackCost1 = new Double();
                                PackCost1 = Convert.ToDouble(GetDealerData.Tables[0].Rows[0]["PromotionTakeAmount"].ToString());
                                string PackAmount1 = string.Format("{0:0.00}", PackCost1).ToString();
                                string PackName1 = GetDealerData.Tables[0].Rows[0]["PromotionName"].ToString();
                                ListItem list = new ListItem();
                                list.Text = PackName1 + " ($" + PackAmount1 + ")";
                                list.Value = GetDealerData.Tables[0].Rows[0]["PromotionID"].ToString();
                                ddlPromotionOptions.Items.Add(list);

                                txtDealerShipName.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["DealerShipName"].ToString());
                                txtPhone.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerPhone"].ToString());
                                txtFax.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerFax"].ToString());
                                txtWebAddress.Text = GetDealerData.Tables[0].Rows[0]["DealerWebAddress"].ToString();
                                txtDealerLicenseNumber.Text = GetDealerData.Tables[0].Rows[0]["DealerLicenseNumber"].ToString();
                                txtAddress.Text = GetDealerData.Tables[0].Rows[0]["DealerAddress"].ToString();
                                txtCity.Text = GetDealerData.Tables[0].Rows[0]["DealerCity"].ToString();

                                ListItem listState = new ListItem();
                                listState.Value = GetDealerData.Tables[0].Rows[0]["DealerStateID"].ToString();
                                listState.Text = GetDealerData.Tables[0].Rows[0]["State_Code"].ToString();
                                ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(listState);
                                txtZip.Text = GetDealerData.Tables[0].Rows[0]["DealerZip"].ToString();

                                txtDealerContactName.Text = objGeneralFunc.ToProper(GetDealerData.Tables[0].Rows[0]["DealerContactName"].ToString());
                                txtDealerJobTitle.Text = GetDealerData.Tables[0].Rows[0]["DealerJobTitle"].ToString();
                                txtContactPhone.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerContactPhone"].ToString());
                                txtContactMobileNumber.Text = objGeneralFunc.filPhnm(GetDealerData.Tables[0].Rows[0]["DealerMobilePhone"].ToString());
                                txtEmail.Text = GetDealerData.Tables[0].Rows[0]["DealerEmail"].ToString();
                                txtSaleNotes.Text = GetDealerData.Tables[0].Rows[0]["SaleNotes"].ToString();

                                Session["NewDealerSaleID"] = GetDealerData.Tables[0].Rows[0]["DealerSaleID"].ToString();
                                Session["NewDealerUID"] = GetDealerData.Tables[0].Rows[0]["DealerUID"].ToString();
                                Session["NewDealerContactID"] = GetDealerData.Tables[0].Rows[0]["DealerContactID"].ToString();
                                Session["NewDealerPaymentID"] = GetDealerData.Tables[0].Rows[0]["PaymentID"].ToString();
                                Session["NewDealerCoordinatorID"] = GetDealerData.Tables[0].Rows[0]["UCEDealerCoordinatorID"].ToString();

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
                            else
                            {
                                Session["DealerEditSaleID"] = null;
                                FillSurveyQuestions();
                            }
                        }
                        else
                        {
                            Session["DealerEditSaleID"] = null;
                            FillSurveyQuestions();
                        }
                    }
                    else
                    {
                        Session["DealerEditSaleID"] = null;
                        FillSurveyQuestions();
                    }
                }
            }
        }
    }

    private void FillPromotionOptions()
    {
        try
        {
            DataSet dsPromotionOptions = new DataSet();
            if (Session["dsPromotionOptions"] == null)
            {
                dsPromotionOptions = objHotLeadBL.GetPromotionOptions();
                Session["dsPromotionOptions"] = dsPromotionOptions;
            }
            else
            {
                dsPromotionOptions = Session["dsPromotionOptions"] as DataSet;
            }
            for (int i = 1; i < dsPromotionOptions.Tables[0].Rows.Count; i++)
            {
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(dsPromotionOptions.Tables[0].Rows[i]["PromotionTakeAmount"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = dsPromotionOptions.Tables[0].Rows[i]["PromotionName"].ToString();
                ListItem list = new ListItem();
                list.Text = PackName + " ($" + PackAmount + ")";
                list.Value = dsPromotionOptions.Tables[0].Rows[i]["PromotionID"].ToString();
                ddlPromotionOptions.Items.Add(list);
            }
            ddlPromotionOptions.Items.Insert(0, new ListItem("Select", "1"));

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillSurveyQuestions()
    {
        try
        {
            DataSet dsSurveyQuestion = new DataSet();
            if (Session["dsSurveyQuestions"] == null)
            {
                dsSurveyQuestion = objHotLeadBL.GetSurveyQuestions();
                Session["dsSurveyQuestions"] = dsSurveyQuestion;
            }
            else
            {
                dsSurveyQuestion = Session["dsSurveyQuestions"] as DataSet;
            }
            RepeterSurvey.DataSource = dsSurveyQuestion.Tables[0];
            RepeterSurvey.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillVoiceFileLocation()
    {
        try
        {
            DataSet dsVoiceFileLocation = objCentralDBBL.GetVoiceFileLocation();
            ddlVoiceFileLocation.DataSource = dsVoiceFileLocation.Tables[0];
            ddlVoiceFileLocation.DataTextField = "VoiceFileLocationName";
            ddlVoiceFileLocation.DataValueField = "VoiceFileLocationID";
            ddlVoiceFileLocation.DataBind();
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
            DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(Convert.ToInt32(Session[Constants.CenterCodeID].ToString()));
            ddlSaleAgent.Items.Clear();
            ddlSaleAgent.DataSource = dsverifier;
            ddlSaleAgent.DataTextField = "AgentUFirstName";
            ddlSaleAgent.DataValueField = "AgentUID";
            ddlSaleAgent.DataBind();
            ListItem li = new ListItem();
            li.Text = Session[Constants.NAME].ToString();
            li.Value = Session[Constants.USER_ID].ToString();
            ddlSaleAgent.SelectedIndex = ddlSaleAgent.Items.IndexOf(li);
        }
        catch (Exception ex)
        {
            throw ex;
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

                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Intromail")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkIntromail.Enabled = true;
                    }

                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "User management")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnAdmin.Enabled = true;

                    }

                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Agent report")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnReports.Enabled = true;
                        lnkbtnMyDealerRep.Enabled = true;
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
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "New sale")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnNewSale.Enabled = true;
                    }

                }

            }
        }


    }

    private void fillYears(DataSet dsYears)
    {
        try
        {
            CCExpiresYear.Items.Clear();
            CCExpiresYear.DataSource = dsYears.Tables[0];
            CCExpiresYear.DataTextField = "YearNum";
            CCExpiresYear.DataValueField = "YearValue";
            CCExpiresYear.DataBind();
            CCExpiresYear.Items.Insert(0, new ListItem("Select Year", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillStates()
    {
        try
        {
            ddlLocationState.DataSource = dsDropDown.Tables[1];
            ddlLocationState.DataTextField = "State_Code";
            ddlLocationState.DataValueField = "State_ID";
            ddlLocationState.DataBind();
            ddlLocationState.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillBillingStates()
    {
        try
        {
            ddlbillingstate.Items.Clear();
            if (Session["DsDropDown"] == null)
            {
                dsDropDown = objdropdownBL.Usp_Get_DropDown();
                Session["DsDropDown"] = dsDropDown;
            }
            else
            {
                dsDropDown = (DataSet)Session["DsDropDown"];
            }

            ddlbillingstate.DataSource = dsDropDown.Tables[1];
            ddlbillingstate.DataTextField = "State_Code";
            ddlbillingstate.DataValueField = "State_ID";
            ddlbillingstate.DataBind();
            ddlbillingstate.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillInvoiceBillingStates()
    {
        try
        {
            ddlInvoiceState.Items.Clear();
            if (Session["DsDropDown"] == null)
            {
                dsDropDown = objdropdownBL.Usp_Get_DropDown();
                Session["DsDropDown"] = dsDropDown;
            }
            else
            {
                dsDropDown = (DataSet)Session["DsDropDown"];
            }

            ddlInvoiceState.DataSource = dsDropDown.Tables[1];
            ddlInvoiceState.DataTextField = "State_Code";
            ddlInvoiceState.DataValueField = "State_ID";
            ddlInvoiceState.DataBind();
            ddlInvoiceState.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }


    //private void FillPDDate(DataSet dsDatetime)
    //{
    //    try
    //    {
    //        DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
    //        ddlPDDate.Items.Clear();
    //        for (int i = 0; i < 62; i++)
    //        {
    //            ListItem list = new ListItem();
    //            list.Text = dtNow.AddDays(i).ToString("MM/dd/yyyy");
    //            list.Value = dtNow.AddDays(i).ToString("MM/dd/yyyy");
    //            ddlPDDate.Items.Add(list);
    //        }
    //        ddlPDDate.Items.Insert(0, new ListItem("Select", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    private void FillPaymentDate(DataSet dsDatetime)
    {
        try
        {
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ddlPaymentDate.Items.Clear();
            for (int i = 0; i < 31; i++)
            {
                ListItem list = new ListItem();
                list.Text = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                list.Value = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                ddlPaymentDate.Items.Add(list);
            }
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

    private void FillContractSignDate(DataSet dsDatetime)
    {
        try
        {
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ddlContractDate.Items.Clear();
            for (int i = 0; i < 31; i++)
            {
                ListItem list = new ListItem();
                list.Text = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                list.Value = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                ddlContractDate.Items.Add(list);
            }
            ddlContractDate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnAdmin_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerEditSaleID"] = null;
            Response.Redirect("UserManagement.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void lnkbtnNewSale_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerEditSaleID"] = null;
            Response.Redirect("NewSale.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnMyDealerRep_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerEditSaleID"] = null;
            Response.Redirect("AgentDealerReport.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnReports_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerEditSaleID"] = null;
            Response.Redirect("AgentReport.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkbtnCentralReport_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerEditSaleID"] = null;
            Response.Redirect("CentralReport.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkIntromail_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerEditSaleID"] = null;
            Response.Redirect("Intromail.aspx");
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
            int Status = 1;
            string DealerContactPhone = txtContactPhone.Text;
            DealerContactPhone = DealerContactPhone.Replace("-", "");
            DealerContactPhone = DealerContactPhone.Replace("-", "");
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForDealerSave(DealerContactPhone);
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                if ((Session["DealerEditSaleID"] != null) && (Session["DealerEditSaleID"].ToString() != ""))
                {
                    if (Session["DealerEditSaleID"].ToString() == dsUserExists.Tables[0].Rows[0]["DealerSaleID"].ToString())
                    {
                        Session["NewDealerPaymentID"] = dsUserExists.Tables[0].Rows[0]["PaymentID"].ToString();
                        Session["NewDealerContactID"] = dsUserExists.Tables[0].Rows[0]["DealerContactID"].ToString();
                        Session["NewDealerUID"] = dsUserExists.Tables[0].Rows[0]["DealerUID"].ToString();
                        Session["NewDealerSaleID"] = dsUserExists.Tables[0].Rows[0]["DealerSaleID"].ToString();
                        Session["NewDealerCoordinatorID"] = dsUserExists.Tables[0].Rows[0]["UCEDealerCoordinatorID"].ToString();
                        SaveDealerInfo(Status);
                        mpealteruserUpdated.Show();
                        lblErrUpdated.Visible = true;
                        lblErrUpdated.Text = "The previous record is being updated and successfully saved with Dealer id " + Session["NewDealerUID"].ToString();
                    }
                    else
                    {
                        string Name = dsUserExists.Tables[0].Rows[0]["DealerContactName"].ToString();
                        mdepAlertExists.Show();
                        lblErrorExists.Visible = true;
                        lblErrorExists.Text = "Oops!<br />Record exists for this phone # <br />Phone: " + txtContactPhone.Text + "<br />Name: " + Name + "<br />Please change phone # to save";
                    }
                }
                else
                {
                    string Name = dsUserExists.Tables[0].Rows[0]["DealerContactName"].ToString();
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Oops!<br />Record exists for this phone # <br />Phone: " + txtContactPhone.Text + "<br />Name: " + Name + "<br />Please change phone # to save";
                }
            }
            else
            {
                SaveDealerInfo(Status);
                mpealteruserUpdated.Show();
                lblErrUpdated.Visible = true;
                lblErrUpdated.Text = "Customer details successfully saved with Dealer id " + Session["NewDealerUID"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int Status = Convert.ToInt32(ddlSaleStatus.SelectedItem.Value);
            string DealerContactPhone = txtContactPhone.Text;
            DealerContactPhone = DealerContactPhone.Replace("-", "");
            DealerContactPhone = DealerContactPhone.Replace("-", "");
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForDealerSave(DealerContactPhone);
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                if ((Session["DealerEditSaleID"] != null) && (Session["DealerEditSaleID"].ToString() != ""))
                {
                    if (Session["DealerEditSaleID"].ToString() == dsUserExists.Tables[0].Rows[0]["DealerSaleID"].ToString())
                    {
                        Session["NewDealerPaymentID"] = dsUserExists.Tables[0].Rows[0]["PaymentID"].ToString();
                        Session["NewDealerContactID"] = dsUserExists.Tables[0].Rows[0]["DealerContactID"].ToString();
                        Session["NewDealerUID"] = dsUserExists.Tables[0].Rows[0]["DealerUID"].ToString();
                        Session["NewDealerSaleID"] = dsUserExists.Tables[0].Rows[0]["DealerSaleID"].ToString();
                        Session["NewDealerCoordinatorID"] = dsUserExists.Tables[0].Rows[0]["UCEDealerCoordinatorID"].ToString();
                        SaveDealerInfo(Status);
                        mpealteruserUpdated.Show();
                        lblErrUpdated.Visible = true;
                        lblErrUpdated.Text = "The previous record is being updated and successfully saved with Dealer id " + Session["NewDealerUID"].ToString();
                    }
                    else
                    {
                        string Name = dsUserExists.Tables[0].Rows[0]["DealerContactName"].ToString();
                        mdepAlertExists.Show();
                        lblErrorExists.Visible = true;
                        lblErrorExists.Text = "Oops!<br />Record exists for this phone # <br />Phone: " + txtContactPhone.Text + "<br />Name: " + Name + "<br />Please change phone # to save";
                    }
                }
                else
                {
                    string Name = dsUserExists.Tables[0].Rows[0]["DealerContactName"].ToString();
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Oops!<br />Record exists for this phone # <br />Phone: " + txtContactPhone.Text + "<br />Name: " + Name + "<br />Please change phone # to save";
                }
            }
            else
            {
                SaveDealerInfo(Status);
                mpealteruserUpdated.Show();
                lblErrUpdated.Visible = true;
                lblErrUpdated.Text = "Customer details successfully saved with Dealer id " + Session["NewDealerUID"].ToString();
            }
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
            Session["DealerEditSaleID"] = null;
            Response.Redirect("NewDealerSale.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void SaveDealerInfo(int Status)
    {
        try
        {
            string DealerContactName = objGeneralFunc.ToProper(txtDealerContactName.Text.Trim());
            string DealerJobTitle = txtDealerJobTitle.Text.Trim();
            string DealerContactPhone = txtContactPhone.Text;
            DealerContactPhone = DealerContactPhone.Replace("-", "");
            DealerContactPhone = DealerContactPhone.Replace("-", "");
            string DealerMobilePhone = txtContactMobileNumber.Text;
            DealerMobilePhone = DealerMobilePhone.Replace("-", "");
            DealerMobilePhone = DealerMobilePhone.Replace("-", "");
            string DealerEmail = txtEmail.Text.Trim();
            int DealerUID = 0;
            if ((Session["NewDealerUID"] == null) || (Session["NewDealerUID"].ToString() == ""))
            {
                DealerUID = 0;
            }
            else
            {                
                DealerUID = Convert.ToInt32(Session["NewDealerUID"]);
            }
            int DealerContactID = 0;
            if ((Session["NewDealerContactID"] == null) || (Session["NewDealerContactID"].ToString() == ""))
            {
                DealerContactID = 0;
            }
            else
            {                
                DealerContactID = Convert.ToInt32(Session["NewDealerContactID"]);
            }
            string DealerShipName = objGeneralFunc.ToProper(txtDealerShipName.Text.Trim());
            string DealerAddress = txtAddress.Text.Trim();
            string DealerCity = objGeneralFunc.ToProper(txtCity.Text.Trim());
            int DealerStateID = Convert.ToInt32(ddlLocationState.SelectedItem.Value);
            string DealerZip = txtZip.Text.Trim();
            string DealerPhone = txtPhone.Text;
            DealerPhone = DealerPhone.Replace("-", "");
            DealerPhone = DealerPhone.Replace("-", "");
            string DealerFax = txtFax.Text;
            DealerFax = DealerFax.Replace("-", "");
            DealerFax = DealerFax.Replace("-", "");
            string DealerWebAddress = txtWebAddress.Text;
            string DealerLicenseNumber = txtDealerLicenseNumber.Text;
            int DealerSaleID = 0;
            if ((Session["NewDealerSaleID"] == null) || (Session["NewDealerSaleID"].ToString() == ""))
            {
                DealerSaleID = 0;
            }
            else
            {                
                DealerSaleID = Convert.ToInt32(Session["NewDealerSaleID"]);
            }
            int UCEDealerCoordinatorID = Convert.ToInt32(ddlSaleAgent.SelectedItem.Value);
            if ((Session["NewDealerCoordinatorID"] == null) || (Session["NewDealerCoordinatorID"].ToString() == ""))
            {
                UCEDealerCoordinatorID = Convert.ToInt32(ddlSaleAgent.SelectedItem.Value);
            }
            else
            {
                UCEDealerCoordinatorID = Convert.ToInt32(Session["NewDealerCoordinatorID"]);
            }
            string LeadGeneratedBy = txtLeadSource.Text;
            string LeadAgent = txtLeadAgent.Text;
            int PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value);
            int PromotionID = Convert.ToInt32(ddlPromotionOptions.SelectedItem.Value);
            string SaleNotes = txtSaleNotes.Text.Trim();
            int SaleEnteredBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            string strIp;
            string strHostName = Request.UserHostAddress.ToString();
            strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            DateTime TargetSignupDate = new DateTime();
            DateTime CallbackDate = new DateTime();
            if (Status == 1)
            {
                if (ddlTargetDate.SelectedItem.Text.ToString() == "Select")
                {
                    TargetSignupDate = Convert.ToDateTime("1/1/1990");
                }
                else
                {
                    TargetSignupDate = Convert.ToDateTime(ddlTargetDate.SelectedItem.Text);
                }
                if (ddlCallbackDate.SelectedItem.Text.ToString() == "Select")
                {
                    CallbackDate = Convert.ToDateTime("1/1/1990");
                }
                else
                {
                    CallbackDate = Convert.ToDateTime(ddlCallbackDate.SelectedItem.Text);
                }
            }
            else
            {
                TargetSignupDate = Convert.ToDateTime("1/1/1990");
                CallbackDate = Convert.ToDateTime("1/1/1990");
            }
            DataSet dsDealersInfo = objHotLeadBL.AddCarsalesDealerInformation(DealerContactID, DealerContactName, DealerJobTitle, DealerContactPhone, DealerMobilePhone, DealerEmail, DealerUID, DealerShipName,
                DealerAddress, DealerCity, DealerStateID, DealerZip, DealerPhone, DealerFax, DealerWebAddress, DealerLicenseNumber, DealerSaleID, UCEDealerCoordinatorID, LeadGeneratedBy, LeadAgent, PackageID, PromotionID,
                TargetSignupDate, CallbackDate, SaleNotes, SaleEnteredBy, strIp);
            DealerSaleID = Convert.ToInt32(dsDealersInfo.Tables[0].Rows[0]["DealerSaleID"].ToString());
            Session["NewDealerSaleID"] = DealerSaleID;
            DealerUID = Convert.ToInt32(dsDealersInfo.Tables[0].Rows[0]["DealerUID"].ToString());
            Session["NewDealerUID"] = DealerUID;
            DealerContactID = Convert.ToInt32(dsDealersInfo.Tables[0].Rows[0]["DealerContactID"].ToString());
            Session["NewDealerContactID"] = DealerContactID;
            int PaymentType = 0;
            string Cardtype = string.Empty;
            if (rdbtnPayVisa.Checked == true)
            {
                PaymentType = 1;
                Cardtype = "VisaCard";
            }
            else if (rdbtnPayMasterCard.Checked == true)
            {
                PaymentType = 2;
                Cardtype = "MasterCard";
            }
            else if (rdbtnPayDiscover.Checked == true)
            {
                PaymentType = 4;
                Cardtype = "DiscoverCard";
            }
            else if (rdbtnPayAmex.Checked == true)
            {
                PaymentType = 3;
                Cardtype = "AmExCard";
            }
            else if (rdbtnInvoice.Checked == true)
            {
                PaymentType = 8;
            }
            else if (rdbtnPayCheck.Checked == true)
            {
                PaymentType = 5;
            }
            string VoiceRecord = txtVoicefileConfirmNo.Text.Trim();
            int VoiceFileLocation = Convert.ToInt32(ddlVoiceFileLocation.SelectedItem.Value);
            DateTime ContractSignDate = new DateTime();
            if (ddlContractDate.SelectedItem.Text.ToString() == "Select")
            {
                ContractSignDate = Convert.ToDateTime("1/1/1990");
            }
            else
            {
                ContractSignDate = Convert.ToDateTime(ddlContractDate.SelectedItem.Text);
            }
            int ContractStatus = Convert.ToInt32(ddlContractStatus.SelectedItem.Value);
            int PaymentID;
            PaymentID = Convert.ToInt32(0);
            if ((Session["NewDealerPaymentID"] == null) || (Session["NewDealerPaymentID"].ToString() == ""))
            {
                PaymentID = 0; 
            }
            else
            {
                PaymentID = Convert.ToInt32(Session["NewDealerPaymentID"]);
            }
            if ((rdbtnPayVisa.Checked == true) || (rdbtnPayMasterCard.Checked == true) || (rdbtnPayDiscover.Checked == true) || (rdbtnPayAmex.Checked == true))
            {
                DateTime PaymentScheduleDate = Convert.ToDateTime(ddlPaymentDate.SelectedItem.Text);
                string Amount = txtPDAmountNow.Text;
                int pmntStatus = 4;
                string CCCardNumber = CardNumber.Text;
                string CardExpDt = ExpMon.SelectedValue + "/" + CCExpiresYear.SelectedValue;
                string CardholderName = objGeneralFunc.ToProper(txtCardholderName.Text);
                string CardholderLastName = objGeneralFunc.ToProper(txtCardholderLastName.Text);
                string CardCode = cvv.Text;
                string BillingAdd = objGeneralFunc.ToProper(txtbillingaddress.Text);
                string BillingCity = objGeneralFunc.ToProper(txtbillingcity.Text);
                string BillingState = ddlbillingstate.SelectedItem.Value;
                string BillingZip = txtbillingzip.Text;
                DataSet dsSaveCCInfo = objHotLeadBL.SaveCreditCardDataForDealer(DealerSaleID, PaymentScheduleDate, ContractSignDate, ContractStatus, Status, Amount, PaymentID, PaymentType, pmntStatus,
                    strIp, VoiceRecord, VoiceFileLocation, CCCardNumber, Cardtype, CardExpDt, CardholderName, CardholderLastName, CardCode, BillingZip, BillingAdd, BillingCity, BillingState);
                PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                Session["NewDealerPaymentID"] = PaymentID;
            }
            if (rdbtnPayCheck.Checked == true)
            {
                DateTime PaymentScheduleDate = Convert.ToDateTime(ddlPaymentDate.SelectedItem.Text);
                string Amount = txtPDAmountNow.Text;
                int pmntStatus = 4;
                int AccType = Convert.ToInt32(ddlAccType.SelectedItem.Value);
                string BankRouting = txtRoutingNumberForCheck.Text;
                string bankName = txtBankNameForCheck.Text;
                string AccNumber = txtAccNumberForCheck.Text;
                string AccHolderName = txtCustNameForCheck.Text;
                string CheckNumber = "";
                int CheckType = Convert.ToInt32(5);
                DataSet dsSaveCheckInfo = objHotLeadBL.SaveCheckDataForDealer(DealerSaleID, PaymentScheduleDate, ContractSignDate, ContractStatus, Status, Amount, PaymentID, PaymentType, pmntStatus, strIp,
                    VoiceRecord, VoiceFileLocation, AccType, BankRouting, bankName, AccNumber, AccHolderName, CheckType, CheckNumber);
                PaymentID = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                Session["NewDealerPaymentID"] = PaymentID;
            }
            if (rdbtnInvoice.Checked == true)
            {
                DateTime PaymentScheduleDate = Convert.ToDateTime(ddlPaymentDate.SelectedItem.Text);
                string Amount = txtPDAmountNow.Text;
                int pmntStatus = 4;
                string InvoiceAttentionTo = txtAttentionTo.Text;
                int SendInvoiceID;
                if (rdbtnInvoiceEmail.Checked == true)
                {
                    SendInvoiceID = 1;
                }
                else
                {
                    SendInvoiceID = 2;
                }
                string SendInvoiceEmail = txtInvoiceEmail.Text.Trim();
                string InvoiceBillingName = objGeneralFunc.ToProper(txtInvoiceBillingname.Text);
                string BillingAdd = objGeneralFunc.ToProper(tyxtInvoiceAddress.Text);
                string BillingCity = objGeneralFunc.ToProper(txtInvoiceCity.Text);
                string BillingState = ddlInvoiceState.SelectedItem.Value;
                string BillingZip = txtInvoiceZip.Text;
                DataSet dsSaveCCInfo = objHotLeadBL.SaveInvoiceDataForDealer(DealerSaleID, PaymentScheduleDate, ContractSignDate, ContractStatus, Status, Amount, PaymentID, PaymentType, pmntStatus,
                    strIp, VoiceRecord, VoiceFileLocation, SendInvoiceID, InvoiceAttentionTo, SendInvoiceEmail, InvoiceBillingName, BillingZip, BillingAdd, BillingCity, BillingState);
                PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                Session["NewDealerPaymentID"] = PaymentID;
            }
            int ProductOptionID;
            int ProductOptionStatus;
            string ProductOptionNotes = "";
            if (rdbtnWebsiteYes.Checked == true)
            {
                ProductOptionID = 1;
                ProductOptionStatus = 1;
                ProductOptionNotes = txtPreferredAddress.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            else
            {
                ProductOptionID = 1;
                ProductOptionStatus = 0;
                ProductOptionNotes = txtPreferredAddress.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            if (rdbtnCarsPromotionYes.Checked == true)
            {
                ProductOptionID = 2;
                ProductOptionStatus = 1;
                ProductOptionNotes = txtGetCarsFrom.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            else
            {
                ProductOptionID = 2;
                ProductOptionStatus = 0;
                ProductOptionNotes = txtGetCarsFrom.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            if (rdbtnLeadsYes.Checked == true)
            {
                ProductOptionID = 3;
                ProductOptionStatus = 1;
                ProductOptionNotes = txtLeadsToSend.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            else
            {
                ProductOptionID = 3;
                ProductOptionStatus = 0;
                ProductOptionNotes = txtLeadsToSend.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            if (rdbtnOthersYes.Checked == true)
            {
                ProductOptionID = 4;
                ProductOptionStatus = 1;
                ProductOptionNotes = txtOthersNotes.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            else
            {
                ProductOptionID = 4;
                ProductOptionStatus = 0;
                ProductOptionNotes = txtOthersNotes.Text.Trim();
                DataSet dsProductOptions = objHotLeadBL.InsertProductInformation(ProductOptionID, DealerUID, ProductOptionStatus, ProductOptionNotes);
            }
            if ((Session["DealerEditSaleID"] != null) && (Session["DealerEditSaleID"].ToString() != ""))
            {
                foreach (RepeaterItem ri in RepeaterSurveyEdit.Items)
                {
                    TextBox txtSurveyQuestionAnswers = (TextBox)ri.FindControl("txtSurveyQuestionAnswers");
                    HiddenField hdnSurveyQuestionID = (HiddenField)ri.FindControl("hdnSurveyQuestionID");
                    int SurveyQuestionID = Convert.ToInt32(hdnSurveyQuestionID.Value);
                    DataSet dsSurveyInfo = objHotLeadBL.InsertSurveyInformation(DealerUID, SurveyQuestionID, txtSurveyQuestionAnswers.Text.Trim());
                }
            }
            else
            {
                foreach (RepeaterItem ri in RepeterSurvey.Items)
                {
                    TextBox txtSurveyQuestionAnswers = (TextBox)ri.FindControl("txtSurveyQuestionAnswers");
                    HiddenField hdnSurveyQuestionID = (HiddenField)ri.FindControl("hdnSurveyQuestionID");
                    int SurveyQuestionID = Convert.ToInt32(hdnSurveyQuestionID.Value);
                    DataSet dsSurveyInfo = objHotLeadBL.InsertSurveyInformation(DealerUID, SurveyQuestionID, txtSurveyQuestionAnswers.Text.Trim());
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnAbandon_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DealerEditSaleID"] = null;
            Response.Redirect("NewDealerSale.aspx");
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
    protected void rdbtnPayVisa_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "block";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "none";

            CardType.Value = "VisaCard";
            CardNumber.Text = "";
            txtCardholderName.Text = "";
            CardNumber.Text = "";
            ListItem limonth = new ListItem();
            limonth.Text = "Select Month";
            limonth.Value = "0";
            ExpMon.SelectedIndex = ExpMon.Items.IndexOf(limonth);
            DataSet dsYears = objHotLeadBL.USP_GetNext12years();
            fillYears(dsYears);
            cvv.Text = "";
            //txtBillingname.Text = "";
            //txtbillingPhone.Text = "";
            txtbillingaddress.Text = "";
            txtbillingcity.Text = "";
            txtbillingzip.Text = "";
            FillBillingStates();
            //btnProcess.Visible = true;
            //chkboxlstPDsale.Enabled = true;
            //ddlPDDate.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnPayMasterCard_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "block";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "none";

            CardType.Value = "MasterCard";
            CardNumber.Text = "";
            txtCardholderName.Text = "";
            CardNumber.Text = "";
            ListItem limonth = new ListItem();
            limonth.Text = "Select Month";
            limonth.Value = "0";
            ExpMon.SelectedIndex = ExpMon.Items.IndexOf(limonth);
            DataSet dsYears = objHotLeadBL.USP_GetNext12years();
            fillYears(dsYears);
            cvv.Text = "";
            // txtBillingname.Text = "";
            // txtbillingPhone.Text = "";
            txtbillingaddress.Text = "";
            txtbillingcity.Text = "";
            txtbillingzip.Text = "";
            FillBillingStates();
            //btnProcess.Visible = true;
            //chkboxlstPDsale.Enabled = true;
            //ddlPDDate.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnPayDiscover_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "block";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "none";

            CardType.Value = "DiscoverCard";
            CardNumber.Text = "";
            txtCardholderName.Text = "";
            CardNumber.Text = "";
            ListItem limonth = new ListItem();
            limonth.Text = "Select Month";
            limonth.Value = "0";
            ExpMon.SelectedIndex = ExpMon.Items.IndexOf(limonth);
            DataSet dsYears = objHotLeadBL.USP_GetNext12years();
            fillYears(dsYears);
            cvv.Text = "";
            //txtBillingname.Text = "";
            //  txtbillingPhone.Text = "";
            txtbillingaddress.Text = "";
            txtbillingcity.Text = "";
            txtbillingzip.Text = "";
            FillBillingStates();
            //btnProcess.Visible = true;
            //chkboxlstPDsale.Enabled = true;
            //ddlPDDate.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnPayAmex_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "block";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "none";

            CardType.Value = "AmExCard";
            CardNumber.Text = "";
            txtCardholderName.Text = "";
            CardNumber.Text = "";
            ListItem limonth = new ListItem();
            limonth.Text = "Select Month";
            limonth.Value = "0";
            ExpMon.SelectedIndex = ExpMon.Items.IndexOf(limonth);
            DataSet dsYears = objHotLeadBL.USP_GetNext12years();
            fillYears(dsYears);
            cvv.Text = "";
            //txtBillingname.Text = "";
            //txtbillingPhone.Text = "";
            txtbillingaddress.Text = "";
            txtbillingcity.Text = "";
            txtbillingzip.Text = "";
            FillBillingStates();
            //btnProcess.Visible = true;
            //chkboxlstPDsale.Enabled = true;
            //ddlPDDate.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnInvoice_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "none";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "block";

            //FillPaymentDate();
            txtAttentionTo.Text = "";
            FillInvoiceBillingStates();
            rdbtnInvoiceEmail.Checked = true;
            txtInvoiceEmail.Text = "";
            //txtPayAmount.Text = "";
            txtInvoiceBillingname.Text = "";
            tyxtInvoiceAddress.Text = "";
            txtInvoiceCity.Text = "";
            txtInvoiceZip.Text = "";
            //btnProcess.Visible = true;
            //chkboxlstPDsale.Enabled = false;
            //ddlPDDate.Enabled = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnPayCheck_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "none";
            divcheck.Style["display"] = "block";
            divpaypal.Style["display"] = "none";

            txtBankNameForCheck.Text = "";
            ListItem liAccType = new ListItem();
            liAccType.Text = "Select";
            liAccType.Value = "0";
            ddlAccType.SelectedIndex = ddlAccType.Items.IndexOf(liAccType);
            ListItem liCheckType = new ListItem();
            liCheckType.Text = "Select";
            liCheckType.Value = "0";
            //ddlCheckType.SelectedIndex = ddlCheckType.Items.IndexOf(liCheckType);
            //txtCheckNumber.Text = "";
            txtCustNameForCheck.Text = "";
            txtAccNumberForCheck.Text = "";
            txtRoutingNumberForCheck.Text = "";
            //btnProcess.Visible = true;
            //chkboxlstPDsale.Enabled = true;
            //ddlPDDate.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}
