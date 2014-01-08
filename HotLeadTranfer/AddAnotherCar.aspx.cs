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

public partial class AddAnotherCar : System.Web.UI.Page
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
                    Session["AddAnotherCarSaleCarID"] = null;
                    Session["AddAnotherCarSalePostingID"] = null;
                    Session["AddAnotherCarSaleUserPackID"] = null;
                    Session["AddAnotherCarSaleSellerID"] = null;
                    Session["AddAnotherCarSalePSID1"] = null;
                    Session["AddAnotherCarSalePSID2"] = null;
                    Session["AddAnotherCarSalePaymentID"] = null;
                    lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");
                    DataSet dsYears = objHotLeadBL.USP_GetNext12years();

                    fillYears(dsYears);
                    FillYear();
                    FillPackage();
                    FillStates();
                    GetAllModels();
                    GetMakes();
                    GetModelsInfo();
                    FillExteriorColor();
                    FillInteriorColor();
                    GetBody();
                    //FillPaymentDate();
                    FillPDDate();
                    FillBillingStates();
                    FillPhotoSource();
                    FillDescriptionSource();
                    // FillVerifier();
                    //FillAgents();
                    FillVoiceFileLocation();
                    // FillCheckTypes();
                    DataSet dsTransfers = objHotLeadBL.GetLoginTransferAgentsCount();
                    if (dsTransfers.Tables[0].Rows[0]["CountAll"].ToString() == "0")
                    {
                        lblTransferAgentsCount.Text = "Transfer agent(s) are not available";
                    }
                    else
                    {
                        lblTransferAgentsCount.Text = dsTransfers.Tables[0].Rows[0]["CountAll"].ToString() + " Transfer agent(s) available";
                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "lll();", true);
                        DataSet dsTransferAgents = objHotLeadBL.GetAllLoginTransferAgentsDetails();
                        if (dsTransferAgents.Tables[0].Rows.Count > 0)
                        {
                            grdTest.DataSource = dsTransferAgents.Tables[0];
                            grdTest.DataBind();
                        }
                    }
                    DataSet dsDatetime = objHotLeadBL.GetDatetime();
                    DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
                    txtPaymentDate.Text = dtNow.ToString("MM/dd/yyyy");
                    DataSet dsCenterRoles = objHotLeadBL.GetCenterRolesByID(Convert.ToInt32(Session[Constants.CenterCodeID].ToString()));
                    Session["dsCenterRoles"] = dsCenterRoles;
                    if (dsCenterRoles.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsCenterRoles.Tables[0].Rows.Count; i++)
                        {
                            if (dsCenterRoles.Tables[0].Rows[i]["CenterRoleID"].ToString() == "1")
                            {
                                if (dsCenterRoles.Tables[0].Rows[i]["CenterRightStatus"].ToString() == "0")
                                {
                                    btnSale.Visible = false;
                                    btnSale2.Visible = false;
                                }
                                else
                                {
                                    btnSale.Visible = true;
                                    btnSale2.Visible = true;
                                }
                            }
                            if (dsCenterRoles.Tables[0].Rows[i]["CenterRoleID"].ToString() == "2")
                            {
                                if (dsCenterRoles.Tables[0].Rows[i]["CenterRightStatus"].ToString() == "0")
                                {
                                    //btnTransfer.Visible = false;
                                    //btnTransfer2.Visible = false;
                                    lblTransferAgentsCount.Visible = false;
                                }
                                else
                                {
                                    //btnTransfer.Visible = true;
                                    //btnTransfer2.Visible = true;
                                    lblTransferAgentsCount.Visible = true;
                                }
                            }
                        }
                    }
                    if ((Session["AddAnotherCarSaleUID"] != null) && (Session["AddAnotherCarSaleUID"].ToString() != ""))
                    {
                        int UID = Convert.ToInt32(Session["AddAnotherCarSaleUID"].ToString());
                        DataSet Cardetais = objHotLeadBL.GetUserByExistUserID(UID);
                        int CarsCount = Convert.ToInt32(Cardetais.Tables[0].Rows.Count.ToString());
                        CarsCount = CarsCount + 1;
                        string CarsHeadText = CarsCount.ToString() + " car details";
                        lblHeadingForAddCar.Text = CarsHeadText.ToString();
                        //ListItem liVer = new ListItem();
                        //liVer.Text = Cardetais.Tables[0].Rows[0]["VerifierAgent"].ToString();
                        //liVer.Value = Cardetais.Tables[0].Rows[0]["VerifierID"].ToString();
                        //ddlVerifier.SelectedIndex = ddlVerifier.Items.IndexOf(liVer);
                        lblVerifier.Text = Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString();
                        lblSaleAgent.Text = Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString();
                        Session["AddAnotherCarVerifierID"] = Cardetais.Tables[0].Rows[0]["SaleVerifierID"].ToString();
                        Session["AddAnotherCarSaleAgentID"] = Cardetais.Tables[0].Rows[0]["SaleAgentID"].ToString();
                        txtFirstName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["sellerName"].ToString());
                        txtLastName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["LastName"].ToString());
                        txtPhone.Text = objGeneralFunc.filPhnm(Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString());
                        txtEmail.Text = Cardetais.Tables[0].Rows[0]["email"].ToString();
                        txtAddress.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["address1"].ToString());
                        txtCity.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["city"].ToString());
                        if (Cardetais.Tables[0].Rows[0]["EmailExists"].ToString() == "0")
                        {
                            chkbxEMailNA.Checked = true;
                        }
                        else
                        {
                            chkbxEMailNA.Checked = false;
                        }

                        ListItem listState = new ListItem();
                        listState.Value = Cardetais.Tables[0].Rows[0]["StateID"].ToString();
                        listState.Text = Cardetais.Tables[0].Rows[0]["state"].ToString();
                        ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(listState);
                        txtZip.Text = Cardetais.Tables[0].Rows[0]["zip"].ToString();
                        Session["AddAnotherCarSaleUID"] = Cardetais.Tables[0].Rows[0]["uid"].ToString();
                        if (Session["AddCarRedirectFrom"].ToString() == "FromNewCar")
                        {
                            if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 1)
                            {
                                rdbtnPayVisa.Checked = true;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 2)
                            {
                                rdbtnPayMasterCard.Checked = true;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 3)
                            {
                                rdbtnPayAmex.Checked = true;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 4)
                            {
                                rdbtnPayDiscover.Checked = true;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                            {
                                rdbtnPayCheck.Checked = true;
                            }
                            else
                            {
                                rdbtnPayPaypal.Checked = true;
                            }
                            if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                            {
                                divcard.Style["display"] = "none";
                                divcheck.Style["display"] = "block";
                                divpaypal.Style["display"] = "none";
                                txtCustNameForCheck.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
                                txtAccNumberForCheck.Text = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                                txtBankNameForCheck.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankName"].ToString());
                                txtRoutingNumberForCheck.Text = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
                                //lblAccountType.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["AccountTypeName"].ToString());
                                ListItem liAccType = new ListItem();
                                liAccType.Text = Cardetais.Tables[0].Rows[0]["AccountTypeName"].ToString();
                                liAccType.Value = Cardetais.Tables[0].Rows[0]["bankAccountType"].ToString();
                                ddlAccType.SelectedIndex = ddlAccType.Items.IndexOf(liAccType);

                                //ListItem liCheckType = new ListItem();
                                //liCheckType.Text = Cardetais.Tables[0].Rows[0]["CheckTypeName"].ToString();
                                //liCheckType.Value = Cardetais.Tables[0].Rows[0]["CheckTypeID"].ToString();
                                //ddlCheckType.SelectedIndex = ddlCheckType.Items.IndexOf(liCheckType);
                                //txtCheckNumber.Text = Cardetais.Tables[0].Rows[0]["BankCheckNumber"].ToString();
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 6)
                            {
                                divcard.Style["display"] = "none";
                                divcheck.Style["display"] = "none";
                                divpaypal.Style["display"] = "block";
                                txtPaytransID.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                                txtpayPalEmailAccount.Text = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                            }
                            else
                            {
                                divcard.Style["display"] = "block";
                                divcheck.Style["display"] = "none";
                                divpaypal.Style["display"] = "none";
                                txtCardholderName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderName"].ToString());
                                //    lblCardType.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardType"].ToString());
                                txtCardholderLastName.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString());
                                CardNumber.Text = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
                                string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
                                string[] EXpDt = EXpDate.Split(new char[] { '/' });

                                ListItem liExpMnth = new ListItem();
                                liExpMnth.Text = EXpDt[0].ToString();
                                liExpMnth.Value = EXpDt[0].ToString();
                                ExpMon.SelectedIndex = ExpMon.Items.IndexOf(liExpMnth);


                                ListItem liExpyear = new ListItem();
                                liExpyear.Text = "20" + EXpDt[1].ToString();
                                liExpyear.Value = EXpDt[1].ToString();
                                CCExpiresYear.SelectedIndex = CCExpiresYear.Items.IndexOf(liExpyear);

                                cvv.Text = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();

                                txtbillingaddress.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString());
                                txtbillingcity.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString());

                                ListItem liBillST = new ListItem();
                                liBillST.Value = Cardetais.Tables[0].Rows[0]["billingState"].ToString();
                                liBillST.Text = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
                                ddlbillingstate.SelectedIndex = ddlbillingstate.Items.IndexOf(liBillST);
                                txtbillingzip.Text = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("NewSale.aspx");
                    }
                }
            }
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
            //DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(Convert.ToInt32(Session[Constants.CenterCodeID].ToString()));
            //ddlSaleAgent.Items.Clear();
            //ddlSaleAgent.DataSource = dsverifier;
            //ddlSaleAgent.DataTextField = "AgentUFirstName";
            //ddlSaleAgent.DataValueField = "AgentUID";
            //ddlSaleAgent.DataBind();
            //ListItem li = new ListItem();
            //li.Text = Session[Constants.NAME].ToString();
            //li.Value = Session[Constants.USER_ID].ToString();
            //ddlSaleAgent.SelectedIndex = ddlSaleAgent.Items.IndexOf(li);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillVerifier()
    {
        try
        {
            // DataSet dsverifier = objHotLeadBL.GetAgentsForVerifier(Convert.ToInt32(Session[Constants.CenterCodeID].ToString()));
            //DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(Convert.ToInt32(Session[Constants.CenterCodeID].ToString()));
            //ddlVerifier.Items.Clear();
            //ddlVerifier.DataSource = dsverifier;
            //ddlVerifier.DataTextField = "AgentUFirstName";
            //ddlVerifier.DataValueField = "AgentUID";
            //ddlVerifier.DataBind();
            //ListItem li = new ListItem();
            //li.Text = Session[Constants.NAME].ToString();
            //li.Value = Session[Constants.USER_ID].ToString();
            //ddlVerifier.SelectedIndex = ddlVerifier.Items.IndexOf(li);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void FillDescriptionSource()
    {
        try
        {
            DataSet dsDescripSource = objHotLeadBL.GetMasterSourceOfDescription();
            ddlDescriptionSource.DataSource = dsDescripSource.Tables[0];
            ddlDescriptionSource.DataTextField = "SourceOfDescriptionName";
            ddlDescriptionSource.DataValueField = "SourceOfDescriptionID";
            ddlDescriptionSource.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillPhotoSource()
    {
        try
        {
            DataSet dsDescripPhotos = objHotLeadBL.USP_GetMasterSourceOfPhotos();
            ddlPhotosSource.DataSource = dsDescripPhotos.Tables[0];
            ddlPhotosSource.DataTextField = "SourceOfPhotosName";
            ddlPhotosSource.DataValueField = "SourceOfPhotosID";
            ddlPhotosSource.DataBind();
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
    private void FillYear()
    {
        try
        {
            DataSet dsYears = new DataSet();
            if (Session["CarsYears"] == null)
            {
                dsYears = objHotLeadBL.GetYears();
                Session["CarsYears"] = dsYears;
            }
            else
            {
                dsYears = Session["CarsYears"] as DataSet;
            }
            ddlYear.DataSource = dsYears.Tables[0];
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
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
    //private void FillCheckTypes()
    //{
    //    try
    //    {
    //        DataSet dsCheckTypes = new DataSet();
    //        dsCheckTypes = objHotLeadBL.GetAllCheckTypes();
    //        ddlCheckType.DataSource = dsCheckTypes.Tables[0];
    //        ddlCheckType.DataTextField = "CheckTypeName";
    //        ddlCheckType.DataValueField = "CheckTypeID";
    //        ddlCheckType.DataBind();
    //        ddlCheckType.Items.Insert(0, new ListItem("Select", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    private void FillPackage()
    {
        try
        {
            for (int i = 1; i < dsDropDown.Tables[2].Rows.Count; i++)
            {
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(dsDropDown.Tables[2].Rows[i]["Price"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = dsDropDown.Tables[2].Rows[i]["Description"].ToString();
                ListItem list = new ListItem();
                list.Text = PackName + " ($" + PackAmount + ")";
                list.Value = dsDropDown.Tables[2].Rows[i]["PackageID"].ToString();
                ddlPackage.Items.Add(list);
            }
            ddlPackage.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void GetAllModels()
    {
        try
        {
            DataSet dsAllModels = new DataSet();

            if (Session[Constants.AllModel] == null)
            {

                dsAllModels = objdropdownBL.USP_GetAllModels(0);
                Session[Constants.AllModel] = dsAllModels;
            }
            else
            {
                dsAllModels = (DataSet)Session[Constants.AllModel];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetMakes()
    {
        try
        {
            var obj = new List<MakesInfo>();


            MakesBL objMakesBL = new MakesBL();

            if (Session[Constants.Makes] == null)
            {
                obj = (List<MakesInfo>)objMakesBL.GetMakes();
                Session[Constants.Makes] = obj;
            }
            else
            {
                obj = (List<MakesInfo>)Session[Constants.Makes];
            }
            ddlMake.DataSource = obj;
            ddlMake.DataTextField = "Make";
            ddlMake.DataValueField = "MakeID";
            ddlMake.DataBind();
            ddlMake.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetModelsInfo()
    {
        try
        {
            //var objModel = new List<MakesInfo>();
            //objModel = Session["AllModel"] as List<MakesInfo>;
            DataSet dsModels = Session[Constants.AllModel] as DataSet;
            int makeid = Convert.ToInt32(ddlMake.SelectedItem.Value);
            DataView dvModel = new DataView();
            DataTable dtModel = new DataTable();
            dvModel = dsModels.Tables[0].DefaultView;
            dvModel.RowFilter = "MakeID='" + makeid.ToString() + "'";
            dtModel = dvModel.ToTable();
            ddlModel.DataSource = dtModel;
            ddlModel.Items.Clear();
            ddlModel.DataTextField = "Model";
            ddlModel.DataValueField = "MakeModelID";
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillInteriorColor()
    {
        try
        {
            ddlInteriorColor.DataSource = dsDropDown.Tables[4];
            ddlInteriorColor.DataTextField = "InteriorColorName";
            ddlInteriorColor.DataValueField = "InteriorColorName";
            ddlInteriorColor.DataBind();
            ddlInteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillExteriorColor()
    {
        try
        {
            ddlExteriorColor.DataSource = dsDropDown.Tables[3];
            ddlExteriorColor.DataTextField = "ExteriorColorName";
            ddlExteriorColor.DataValueField = "ExteriorColorName";
            ddlExteriorColor.DataBind();
            ddlExteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    public void GetBody()
    {
        try
        {
            var obj = new List<BodyInfo>();

            //MakesInfo objMakes = new MakesInfo();
            MakesBL objMakesBL = new MakesBL();

            if (Session[Constants.Bodys] == null)
            {
                obj = (List<BodyInfo>)objMakesBL.GetBodys();
                Session["Bodys"] = obj;
            }
            else
            {
                obj = (List<BodyInfo>)Session[Constants.Bodys];
            }


            ddlBodyStyle.DataSource = obj;
            ddlBodyStyle.DataTextField = "bodyType";
            ddlBodyStyle.DataValueField = "bodyTypeID";
            ddlBodyStyle.DataBind();
            ddlBodyStyle.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    ////private void FillCylinders()
    ////{
    ////    try
    ////    {
    ////        ddlCylinderCount.DataSource = dsDropDown.Tables[5];
    ////        ddlCylinderCount.DataTextField = "CylindersName";
    ////        ddlCylinderCount.DataValueField = "CylindersName";
    ////        ddlCylinderCount.DataBind();
    ////        ddlCylinderCount.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////    }
    ////}
    private void FillPDDate()
    {
        try
        {
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ddlPDDate.Items.Clear();
            for (int i = 0; i < 21; i++)
            {
                ListItem list = new ListItem();
                list.Text = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                list.Value = dtNow.AddDays(i).ToString("MM/dd/yyyy");
                ddlPDDate.Items.Add(list);
            }
            ddlPDDate.Items.Insert(0, new ListItem("Select", "0"));
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
            //ddlPaymentdate.Items.Clear();
            //for (int i = 0; i < 7; i++)
            //{
            //    ListItem list = new ListItem();
            //    list.Text = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(-i).ToString("MM/dd/yyyy");
            //    list.Value = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(-i).ToString("MM/dd/yyyy");
            //    ddlPaymentdate.Items.Add(list);
            //}
            //ddlPaymentdate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetModelsInfo();
            Session.Timeout = 180;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //private void FillDriveTrain()
    //{
    //    try
    //    {
    //        ddlDriveTrain.DataSource = dsDropDown.Tables[6];
    //        ddlDriveTrain.DataTextField = "NoOfDoorsName";
    //        ddlDriveTrain.DataValueField = "NoOfDoorsName";
    //        ddlDriveTrain.DataBind();
    //        ddlDriveTrain.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private void FillTransmissions()
    //{
    //    try
    //    {
    //        ddlTransmission.DataSource = dsDropDown.Tables[7];
    //        ddlTransmission.DataTextField = "TransmissionName";
    //        ddlTransmission.DataValueField = "TransmissionName";
    //        ddlTransmission.DataBind();
    //        ddlTransmission.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    protected void lnkbtnAdmin_Click(object sender, EventArgs e)
    {
        try
        {
            Session["AddAnotherCarSaleUID"] = null;
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
            Session["AddAnotherCarSaleUID"] = null;
            Response.Redirect("NewSale.aspx");
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
            Session["AddAnotherCarSaleUID"] = null;
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
            Session["AddAnotherCarSaleUID"] = null;
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
            Session["AddAnotherCarSaleUID"] = null;
            Response.Redirect("Intromail.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSale_Click(object sender, EventArgs e)
    {
        try
        {
            int LeadStatus = Convert.ToInt32(1);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            int SaleAgentID = Convert.ToInt32(Session["AddAnotherCarSaleAgentID"].ToString());
            SaveInfo(LeadStatus, SaleAgentID);
            Session["AbandonSalePostingID"] = null;
            mdepDraftExistsShow.Show();
            lblDraftExistsShow.Visible = true;
            lblDraftExistsShow.Text = "Customer details saved successfully with sale id " + Session["AddAnotherCarSaleCarID"].ToString() + "<br />Do you want to add another car?";
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
            if (txtPhone.Text != "")
            {
                int LeadStatus = Convert.ToInt32(2);
                string SellerPhone = txtPhone.Text;
                SellerPhone = SellerPhone.Replace("-", "");
                SellerPhone = SellerPhone.Replace("-", "");
                DataSet dsReturnExists = objHotLeadBL.ChkUserPhoneNumberExistsForReturned(SellerPhone);
                if (dsReturnExists.Tables[0].Rows.Count > 0)
                {
                    Session["NewSaleCarID"] = dsReturnExists.Tables[0].Rows[0]["carid"].ToString();
                    Session["NewSaleUID"] = dsReturnExists.Tables[0].Rows[0]["uid"].ToString();
                    Session["NewSalePostingID"] = dsReturnExists.Tables[0].Rows[0]["postingID"].ToString();
                    Session["NewSaleUserPackID"] = dsReturnExists.Tables[0].Rows[0]["UserPackID"].ToString();
                    Session["NewSaleSellerID"] = dsReturnExists.Tables[0].Rows[0]["sellerID"].ToString();
                    Session["NewSalePSID1"] = dsReturnExists.Tables[0].Rows[0]["PSID1"].ToString();
                    if (dsReturnExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                    {
                        Session["NewSalePSID2"] = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["PSID2"].ToString());
                    }
                    if (dsReturnExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                    {
                        Session["NewSalePaymentID"] = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["PaymentID"].ToString());
                    }
                    int SaleAgentID = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["SaleAgentID"].ToString());
                    SaveInfo(LeadStatus, SaleAgentID);
                    Session["AbandonSalePostingID"] = null;
                    Response.Redirect("NewSale.aspx");
                }
                else
                {
                    DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(SellerPhone);
                    if (dsUserExists.Tables[0].Rows.Count > 0)
                    {
                        Session["AbandonSalePostingID"] = null;
                        Response.Redirect("NewSale.aspx");
                    }
                    else
                    {
                        DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(SellerPhone);
                        if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                        {
                            Session["NewSaleCarID"] = dsUserDraftExists.Tables[0].Rows[0]["carid"].ToString();
                            Session["NewSaleUID"] = dsUserDraftExists.Tables[0].Rows[0]["uid"].ToString();
                            Session["NewSalePostingID"] = dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString();
                            Session["NewSaleUserPackID"] = dsUserDraftExists.Tables[0].Rows[0]["UserPackID"].ToString();
                            Session["NewSaleSellerID"] = dsUserDraftExists.Tables[0].Rows[0]["sellerID"].ToString();
                            Session["NewSalePSID1"] = dsUserDraftExists.Tables[0].Rows[0]["PSID1"].ToString();
                            if (dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                            {
                                Session["NewSalePSID2"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString());
                            }
                            if (dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                            {
                                Session["NewSalePaymentID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString());
                            }
                            int SaleAgentID = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["SaleAgentID"].ToString());
                            SaveInfo(LeadStatus, SaleAgentID);
                            Session["AbandonSalePostingID"] = null;
                            Response.Redirect("NewSale.aspx");
                        }
                        else
                        {
                            int SaleAgentID = 0;
                            SaveInfo(LeadStatus, SaleAgentID);
                            Session["AbandonSalePostingID"] = null;
                            Response.Redirect("NewSale.aspx");
                        }
                    }
                }
            }
            else
            {
                Session["AbandonSalePostingID"] = null;
                Response.Redirect("NewSale.aspx");
            }
            //mpealteruserUpdated.Show();
            //Response.Redirect("NewSale.aspx");
            //lblErrUpdated.Visible = true;
            //lblErrUpdated.Text = "Customer details saved successfully";

            // }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSavedraft_Click(object sender, EventArgs e)
    {
        try
        {
            int LeadStatus = Convert.ToInt32(3);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            DataSet dsReturnExists = objHotLeadBL.ChkUserPhoneNumberExistsForReturned(SellerPhone);
            if (dsReturnExists.Tables[0].Rows.Count > 0)
            {
                Session["NewSaleCarID"] = dsReturnExists.Tables[0].Rows[0]["carid"].ToString();
                Session["NewSaleUID"] = dsReturnExists.Tables[0].Rows[0]["uid"].ToString();
                Session["NewSalePostingID"] = dsReturnExists.Tables[0].Rows[0]["postingID"].ToString();
                Session["NewSaleUserPackID"] = dsReturnExists.Tables[0].Rows[0]["UserPackID"].ToString();
                Session["NewSaleSellerID"] = dsReturnExists.Tables[0].Rows[0]["sellerID"].ToString();
                Session["NewSalePSID1"] = dsReturnExists.Tables[0].Rows[0]["PSID1"].ToString();
                if (dsReturnExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                {
                    Session["NewSalePSID2"] = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["PSID2"].ToString());
                }
                if (dsReturnExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                {
                    Session["NewSalePaymentID"] = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["PaymentID"].ToString());
                }
                int SaleAgentID = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["SaleAgentID"].ToString());
                SaveInfo(LeadStatus, SaleAgentID);
                Session["AbandonSalePostingID"] = null;
            }
            else
            {
                DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(SellerPhone);
                if (dsUserExists.Tables[0].Rows.Count > 0)
                {
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Phone " + txtPhone.Text + " already exists.<br />Please change phone # to save";
                }
                else
                {
                    DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(SellerPhone);
                    if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                    {
                        Session["NewSaleCarID"] = dsUserDraftExists.Tables[0].Rows[0]["carid"].ToString();
                        Session["NewSaleUID"] = dsUserDraftExists.Tables[0].Rows[0]["uid"].ToString();
                        Session["NewSalePostingID"] = dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString();
                        Session["NewSaleUserPackID"] = dsUserDraftExists.Tables[0].Rows[0]["UserPackID"].ToString();
                        Session["NewSaleSellerID"] = dsUserDraftExists.Tables[0].Rows[0]["sellerID"].ToString();
                        Session["NewSalePSID1"] = dsUserDraftExists.Tables[0].Rows[0]["PSID1"].ToString();
                        if (dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                        {
                            Session["NewSalePSID2"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString());
                        }
                        if (dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                        {
                            Session["NewSalePaymentID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString());
                        }
                        int SaleAgentID = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["SaleAgentID"].ToString());
                        SaveInfo(LeadStatus, SaleAgentID);
                        Session["AbandonSalePostingID"] = null;
                    }
                    else
                    {
                        int SaleAgentID = 0;
                        SaveInfo(LeadStatus, SaleAgentID);
                        Session["AbandonSalePostingID"] = null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        try
        {
            int LeadStatus = Convert.ToInt32(4);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            DataSet dsReturnExists = objHotLeadBL.ChkUserPhoneNumberExistsForReturned(SellerPhone);
            if (dsReturnExists.Tables[0].Rows.Count > 0)
            {
                Session["NewSaleCarID"] = dsReturnExists.Tables[0].Rows[0]["carid"].ToString();
                Session["NewSaleUID"] = dsReturnExists.Tables[0].Rows[0]["uid"].ToString();
                Session["NewSalePostingID"] = dsReturnExists.Tables[0].Rows[0]["postingID"].ToString();
                Session["NewSaleUserPackID"] = dsReturnExists.Tables[0].Rows[0]["UserPackID"].ToString();
                Session["NewSaleSellerID"] = dsReturnExists.Tables[0].Rows[0]["sellerID"].ToString();
                Session["NewSalePSID1"] = dsReturnExists.Tables[0].Rows[0]["PSID1"].ToString();
                if (dsReturnExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                {
                    Session["NewSalePSID2"] = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["PSID2"].ToString());
                }
                if (dsReturnExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                {
                    Session["NewSalePaymentID"] = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["PaymentID"].ToString());
                }
                int SaleAgentID = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["SaleAgentID"].ToString());
                SaveInfo(LeadStatus, SaleAgentID);
                Session["AbandonSalePostingID"] = null;
                mpealteruserUpdated.Show();
                lblErrUpdated.Visible = true;
                lblErrUpdated.Text = "The previous record is being updated and successfully saved with sale id " + Session["NewSaleCarID"].ToString();
            }
            else
            {
                DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(SellerPhone);
                if (dsUserExists.Tables[0].Rows.Count > 0)
                {
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Phone " + txtPhone.Text + " already exists.<br />Please change phone # to transfer";
                }
                else
                {
                    DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(SellerPhone);
                    if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                    {
                        Session["NewSaleCarID"] = dsUserDraftExists.Tables[0].Rows[0]["carid"].ToString();
                        Session["NewSaleUID"] = dsUserDraftExists.Tables[0].Rows[0]["uid"].ToString();
                        Session["NewSalePostingID"] = dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString();
                        Session["NewSaleUserPackID"] = dsUserDraftExists.Tables[0].Rows[0]["UserPackID"].ToString();
                        Session["NewSaleSellerID"] = dsUserDraftExists.Tables[0].Rows[0]["sellerID"].ToString();
                        Session["NewSalePSID1"] = dsUserDraftExists.Tables[0].Rows[0]["PSID1"].ToString();
                        if (dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                        {
                            Session["NewSalePSID2"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString());
                        }
                        if (dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                        {
                            Session["NewSalePaymentID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString());
                        }
                        int SaleAgentID = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["SaleAgentID"].ToString());
                        SaveInfo(LeadStatus, SaleAgentID);
                        Session["AbandonSalePostingID"] = null;
                        mpealteruserUpdated.Show();
                        lblErrUpdated.Visible = true;
                        lblErrUpdated.Text = "The previous record is being updated and successfully saved with sale id " + Session["NewSaleCarID"].ToString();
                    }
                    else
                    {
                        int SaleAgentID = 0;
                        SaveInfo(LeadStatus, SaleAgentID);
                        Session["AbandonSalePostingID"] = null;
                        Response.Redirect("NewSale.aspx");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SaveInfo(int LeadStatus, int SaleAgentID)
    {
        try
        {
            objUserregInfo.Name = objGeneralFunc.ToProper(txtFirstName.Text);
            objUserregInfo.UserName = txtEmail.Text;
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            objUserregInfo.PhoneNumber = SellerPhone;
            objUserregInfo.Address = objGeneralFunc.ToProper(txtAddress.Text);
            objUserregInfo.City = objGeneralFunc.ToProper(txtCity.Text);
            objUserregInfo.StateID = Convert.ToInt32(ddlLocationState.SelectedItem.Value);
            objUserregInfo.Zip = txtZip.Text;
            if (SaleAgentID == 0)
            {
                SaleAgentID = Convert.ToInt32(Session["AddAnotherCarSaleAgentID"].ToString());
            }
            int PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value);
            string strIp;
            string strHostName = Request.UserHostAddress.ToString();
            strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            int YearOfMake = Convert.ToInt32(ddlYear.SelectedItem.Value);
            Session["SelYear"] = ddlYear.SelectedItem.Text;
            Session["SelMake"] = ddlMake.SelectedItem.Text;
            Session["SelModel"] = ddlModel.SelectedItem.Text;
            int MakeModelID = Convert.ToInt32(ddlModel.SelectedItem.Value);
            int BodyTypeID = Convert.ToInt32(ddlBodyStyle.SelectedItem.Value);
            string Price = string.Empty;
            if (txtAskingPrice.Text == "")
            {
                Price = "0";
            }
            else
            {
                Price = txtAskingPrice.Text;
                Price = Price.Replace(",", "");
            }
            string Mileage = string.Empty;
            if (txtMileage.Text == "")
            {
                Mileage = "0";
            }
            else
            {
                Mileage = txtMileage.Text;
                Mileage = Mileage.Replace(",", "");
                Mileage = Mileage.Replace("mi", "");
                Mileage = Mileage.Replace(" ", "");
            }
            string ExteriorColor = ddlExteriorColor.SelectedItem.Text;
            string InteriorColor = ddlInteriorColor.SelectedItem.Text;
            string Transmission = "Unspecified";
            if (rdbtnTrans1.Checked == true)
            {
                Transmission = "Automatic";
            }
            else if (rdbtnTrans2.Checked == true)
            {
                Transmission = "Manual";
            }
            else if (rdbtnTrans3.Checked == true)
            {
                Transmission = "Tiptronic";
            }
            else if (rdbtnTrans4.Checked == true)
            {
                Transmission = "Unspecified";
            }

            string NumberOfDoors = string.Empty;
            if (rdbtnDoor2.Checked == true)
            {
                NumberOfDoors = "Two Door";
            }
            else if (rdbtnDoor3.Checked == true)
            {
                NumberOfDoors = "Three Door";
            }
            else if (rdbtnDoor4.Checked == true)
            {
                NumberOfDoors = "Four Door";
            }
            else if (rdbtnDoor5.Checked == true)
            {
                NumberOfDoors = "Five Door";
            }
            else
            {
                NumberOfDoors = "Unspecified";
            }
            string DriveTrain = "Unspecified";
            if (rdbtnDriveTrain1.Checked == true)
            {
                DriveTrain = "2 wheel drive";
            }
            else if (rdbtnDriveTrain2.Checked == true)
            {
                DriveTrain = "2 wheel drive - front";
            }
            else if (rdbtnDriveTrain3.Checked == true)
            {
                DriveTrain = "All wheel drive";
            }
            else if (rdbtnDriveTrain4.Checked == true)
            {
                DriveTrain = "Rear wheel drive";
            }
            else if (rdbtnDriveTrain5.Checked == true)
            {
                DriveTrain = "Unspecified";
            }

            string VIN = txtVin.Text;
            string NumberOfCylinder = "Unspecified";
            if (rdbtnCylinders1.Checked == true)
            {
                NumberOfCylinder = "3 Cylinder";
            }
            else if (rdbtnCylinders2.Checked == true)
            {
                NumberOfCylinder = "4 Cylinder";
            }
            else if (rdbtnCylinders3.Checked == true)
            {
                NumberOfCylinder = "5 Cylinder";
            }
            else if (rdbtnCylinders4.Checked == true)
            {
                NumberOfCylinder = "6 Cylinder";
            }
            else if (rdbtnCylinders5.Checked == true)
            {
                NumberOfCylinder = "7 Cylinder";
            }
            else if (rdbtnCylinders6.Checked == true)
            {
                NumberOfCylinder = "8 Cylinder";
            }

            int FuelTypeID = Convert.ToInt32(0);
            if (rdbtnFuelType1.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(1);
            }
            else if (rdbtnFuelType2.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(2);
            }
            else if (rdbtnFuelType3.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(3);
            }
            else if (rdbtnFuelType4.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(4);
            }
            else if (rdbtnFuelType5.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(5);
            }
            else if (rdbtnFuelType6.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(6);
            }
            else if (rdbtnFuelType7.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(7);
            }
            else if (rdbtnFuelType8.Checked == true)
            {
                FuelTypeID = Convert.ToInt32(0);
            }

            int ConditionID = Convert.ToInt32(0);
            string Condition = "Unspecified";
            if (rdbtnCondition1.Checked == true)
            {
                ConditionID = Convert.ToInt32(1);
                Condition = "Excellent";
            }
            else if (rdbtnCondition2.Checked == true)
            {
                ConditionID = Convert.ToInt32(2);
                Condition = "Very Good";
            }
            else if (rdbtnCondition3.Checked == true)
            {
                ConditionID = Convert.ToInt32(3);
                Condition = "Good";
            }
            else if (rdbtnCondition4.Checked == true)
            {
                ConditionID = Convert.ToInt32(4);
                Condition = "Fair";
            }
            else if (rdbtnCondition5.Checked == true)
            {
                ConditionID = Convert.ToInt32(5);
                Condition = "Poor";
            }
            else if (rdbtnCondition6.Checked == true)
            {
                ConditionID = Convert.ToInt32(6);
                Condition = "Parts or Salvage";
            }
            else if (rdbtnCondition7.Checked == true)
            {
                ConditionID = Convert.ToInt32(0);
                Condition = "Unspecified";
            }


            string Description = string.Empty;
            Description = txtDescription.Text;

            string Title = "";
            string State = ddlLocationState.SelectedItem.Text;
            String UpdatedBy = Session[Constants.NAME].ToString();
            string InternalNotesNew = txtSaleNotes.Text.Trim();
            InternalNotesNew = InternalNotesNew.Trim();
            //string UpdateByWithDate = System.DateTime.Now.ToUniversalTime().AddHours(-4).ToString() + "-" + UpdatedBy + "<br>";
            //if (InternalNotesNew != "")
            //{
            //    InternalNotesNew = UpdateByWithDate + InternalNotesNew.Trim() + "<br>" + "---------------------------------";
            //}
            //else
            //{
            //    InternalNotesNew = InternalNotesNew.Trim();
            int SourceOfPhotos = Convert.ToInt32(ddlPhotosSource.SelectedItem.Value);
            int SourceOfDescription = Convert.ToInt32(ddlDescriptionSource.SelectedItem.Value);

            string LastName = objGeneralFunc.ToProper(txtLastName.Text.Trim());
            int CarID;
            int RegUID;
            int PostingID;
            int UserPackID;
            int sellerID;
            CarID = Convert.ToInt32(0);
            RegUID = Convert.ToInt32(Session["AddAnotherCarSaleUID"].ToString());
            PostingID = Convert.ToInt32(0);
            UserPackID = Convert.ToInt32(0);
            sellerID = Convert.ToInt32(0);
            int VerifierID = Convert.ToInt32(Session["AddAnotherCarVerifierID"].ToString());
            int SaleEnteredBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int EmailExists = 1;
            if (chkbxEMailNA.Checked == true)
            {
                EmailExists = 0;
            }
            DataSet dsdata = objHotLeadBL.SaveSaleStatusDataForMultiCar(objUserregInfo, SaleAgentID, PackageID, YearOfMake, MakeModelID, BodyTypeID, ConditionID, DriveTrain,
                        Price, Mileage, ExteriorColor, InteriorColor, Transmission, NumberOfDoors, VIN, NumberOfCylinder, FuelTypeID, Description, Condition, Title,
                        State, strIp, InternalNotesNew, LeadStatus, LastName, SourceOfPhotos, SourceOfDescription, CarID, RegUID, UserPackID, sellerID, PostingID, VerifierID, SaleEnteredBy, EmailExists);

            CarID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["CarID"].ToString());
            Session["AddAnotherCarSaleCarID"] = CarID;
            RegUID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["UID"].ToString());
            Session["AddAnotherCarSaleUID"] = RegUID;
            PostingID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["PostingID"].ToString());
            Session["AddAnotherCarSalePostingID"] = PostingID;
            UserPackID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["UserPackID"].ToString());
            Session["AddAnotherCarSaleUserPackID"] = UserPackID;
            sellerID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["sellerID"].ToString());
            Session["AddAnotherCarSaleSellerID"] = sellerID;

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
            else if (rdbtnPayPaypal.Checked == true)
            {
                PaymentType = 6;
            }
            else if (rdbtnPayCheck.Checked == true)
            {
                PaymentType = 5;
            }
            string VoiceRecord = txtVoicefileConfirmNo.Text.Trim();
            int VoiceFileLocation = Convert.ToInt32(ddlVoiceFileLocation.SelectedItem.Value);
            int PSID1;
            PSID1 = Convert.ToInt32(0);
            int PSID2;
            PSID2 = Convert.ToInt32(0);
            int PaymentID;
            PaymentID = Convert.ToInt32(0);
            if ((rdbtnPayVisa.Checked == true) || (rdbtnPayMasterCard.Checked == true) || (rdbtnPayDiscover.Checked == true) || (rdbtnPayAmex.Checked == true))
            {
                if (chkboxlstPDsale.Checked == true)
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID;
                    int pmntStatus;
                    //int PSStatusID = 4;
                    //int pmntStatus = 1;
                    if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                    {
                        PSStatusID = 8;
                        pmntStatus = 2;
                    }
                    else
                    {
                        PSStatusID = 4;
                        pmntStatus = 1;
                    }
                    string CCCardNumber = CardNumber.Text;
                    string CardExpDt = ExpMon.SelectedValue + "/" + CCExpiresYear.SelectedValue;
                    string CardholderName = objGeneralFunc.ToProper(txtCardholderName.Text);
                    string CardholderLastName = objGeneralFunc.ToProper(txtCardholderLastName.Text);
                    string CardCode = cvv.Text;
                    string BillingAdd = objGeneralFunc.ToProper(txtbillingaddress.Text);
                    string BillingCity = objGeneralFunc.ToProper(txtbillingcity.Text);
                    string BillingState = ddlbillingstate.SelectedItem.Value;
                    string BillingZip = txtbillingzip.Text;
                    DateTime PDDate = Convert.ToDateTime(ddlPDDate.SelectedItem.Text);
                    //string PDPayAmountNow = txtPDAmountNow.Text;
                    string PDPayAmountLater = txtPDAmountLater.Text;
                    DataSet dsSaveCCInfo = objHotLeadBL.SaveCreditCardDataForPDSale(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                            pmntStatus, strIp, VoiceRecord, CCCardNumber, Cardtype, CardExpDt, CardholderName, CardholderLastName, CardCode, BillingZip, BillingAdd,
                                            BillingCity, BillingState, PostingID, PSID2, PDDate, PDPayAmountLater, VoiceFileLocation);
                    PSID1 = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PSID1"].ToString());
                    Session["AddAnotherCarSalePSID1"] = PSID1;
                    PSID2 = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PSID2"].ToString());
                    Session["AddAnotherCarSalePSID2"] = PSID2;
                    PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AddAnotherCarSalePaymentID"] = PaymentID;
                    if (VoiceRecord.ToString() != "")
                    {
                        DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                    }
                    else
                    {
                        if (LeadStatus != 1)
                        {
                            DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                        }
                    }    
                }
                else
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID = 4;
                    int pmntStatus = 1;
                    string CCCardNumber = CardNumber.Text;
                    string CardExpDt = ExpMon.SelectedValue + "/" + CCExpiresYear.SelectedValue;
                    string CardholderName = objGeneralFunc.ToProper(txtCardholderName.Text);
                    string CardholderLastName = objGeneralFunc.ToProper(txtCardholderLastName.Text);
                    string CardCode = cvv.Text;
                    string BillingAdd = objGeneralFunc.ToProper(txtbillingaddress.Text);
                    string BillingCity = objGeneralFunc.ToProper(txtbillingcity.Text);
                    string BillingState = ddlbillingstate.SelectedItem.Value;
                    string BillingZip = txtbillingzip.Text;
                    DataSet dsSaveCCInfo = objHotLeadBL.SaveCreditCardData(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                            pmntStatus, strIp, VoiceRecord, CCCardNumber, Cardtype, CardExpDt, CardholderName, CardholderLastName, CardCode, BillingZip, BillingAdd,
                                            BillingCity, BillingState, PostingID, VoiceFileLocation);

                    PSID1 = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PSID1"].ToString());
                    Session["AddAnotherCarSalePSID1"] = PSID1;
                    PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AddAnotherCarSalePaymentID"] = PaymentID;
                    if (VoiceRecord.ToString() != "")
                    {
                        DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                    }
                    else
                    {
                        if (LeadStatus != 1)
                        {
                            DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                        }
                    }    
                }
            }
            if (rdbtnPayPaypal.Checked == true)
            {
                DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                string Amount = txtPDAmountNow.Text;
                int PSStatusID = 4;
                int pmntStatus = 1;
                string TransID = txtPaytransID.Text;
                string PayPalEmailAcc = txtpayPalEmailAccount.Text;
                DataSet dsSavePayPalInfo = objHotLeadBL.SavePayPalData(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                        pmntStatus, strIp, VoiceRecord, TransID, PayPalEmailAcc, PostingID, VoiceFileLocation);

                PSID1 = Convert.ToInt32(dsSavePayPalInfo.Tables[0].Rows[0]["PSID1"].ToString());
                Session["AddAnotherCarSalePSID1"] = PSID1;
                PaymentID = Convert.ToInt32(dsSavePayPalInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                Session["AddAnotherCarSalePaymentID"] = PaymentID;
                if (VoiceRecord.ToString() != "")
                {
                    DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                }
                else
                {
                    if (LeadStatus != 1)
                    {
                        DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                    }
                }    
                //    if (chkboxlstPDsale.Checked == true)
                //    {
                //        int pmntStatus = 1;
                //        string Amount = txtPayAmount.Text;
                //        DateTime PayDate = Convert.ToDateTime(ddlPaymentdate.SelectedItem.Text);
                //        string TransID = txtPaytransID.Text;
                //        string PayPalEmailAcc = txtpayPalEmailAccount.Text;
                //        DateTime PDDate = Convert.ToDateTime(ddlPDDate.SelectedItem.Text);
                //        string PDPayAmountNow = txtPDAmountNow.Text;
                //        string PDPayAmountLater = txtPDAmountLater.Text;
                //        DataSet dsSaveCCInfo = objHotLeadBL.SavePayPalDataForPDSale(UserPackID, PaymentType, pmntStatus, strIp, Amount, VoiceRecord, PostingID, PayDate, TransID, PayPalEmailAcc, PDDate, PDPayAmountNow, PDPayAmountLater);
                //    }
                //    else
                //    {
                //        int pmntStatus = 2;
                //        string Amount = txtPayAmount.Text;
                //        DateTime PayDate = Convert.ToDateTime(ddlPaymentdate.SelectedItem.Text);
                //        string TransID = txtPaytransID.Text;
                //        string PayPalEmailAcc = txtpayPalEmailAccount.Text;
                //        DataSet dsSaveCCInfo = objHotLeadBL.SavePayPalData(UserPackID, PaymentType, pmntStatus, strIp, Amount, VoiceRecord, PostingID, PayDate, TransID, PayPalEmailAcc);
                //    }
            }
            if (rdbtnPayCheck.Checked == true)
            {
                if (chkboxlstPDsale.Checked == true)
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID;
                    int pmntStatus;
                    //int PSStatusID = 4;
                    //int pmntStatus = 1;
                    if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                    {
                        PSStatusID = 8;
                        pmntStatus = 2;
                    }
                    else
                    {
                        PSStatusID = 4;
                        pmntStatus = 1;
                    }
                    int AccType = Convert.ToInt32(ddlAccType.SelectedItem.Value);
                    string BankRouting = txtRoutingNumberForCheck.Text;
                    string bankName = txtBankNameForCheck.Text;
                    string AccNumber = txtAccNumberForCheck.Text;
                    string AccHolderName = txtCustNameForCheck.Text;
                    DateTime PDDate = Convert.ToDateTime(ddlPDDate.SelectedItem.Text);
                    //string PDPayAmountNow = txtPDAmountNow.Text;
                    string PDPayAmountLater = txtPDAmountLater.Text;
                    string CheckNumber = "";
                    int CheckType = Convert.ToInt32(5);
                    DataSet dsSaveCheckInfo = objHotLeadBL.SaveCheckDataForPDSale(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                            pmntStatus, strIp, VoiceRecord, PostingID, AccType, BankRouting, bankName, AccNumber, AccHolderName, PSID2, PDDate, PDPayAmountLater, VoiceFileLocation, CheckType, CheckNumber);
                    PSID1 = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PSID1"].ToString());
                    Session["AddAnotherCarSalePSID1"] = PSID1;
                    PSID2 = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PSID2"].ToString());
                    Session["AddAnotherCarSalePSID2"] = PSID2;
                    PaymentID = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AddAnotherCarSalePaymentID"] = PaymentID;
                    if (VoiceRecord.ToString() != "")
                    {
                        DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                    }
                    else
                    {
                        if (LeadStatus != 1)
                        {
                            DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                        }
                    }    
                }
                else
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID = 4;
                    int pmntStatus = 1;
                    int AccType = Convert.ToInt32(ddlAccType.SelectedItem.Value);
                    string BankRouting = txtRoutingNumberForCheck.Text;
                    string bankName = txtBankNameForCheck.Text;
                    string AccNumber = txtAccNumberForCheck.Text;
                    string AccHolderName = txtCustNameForCheck.Text;
                    string CheckNumber = "";
                    int CheckType = Convert.ToInt32(5);
                    DataSet dsSaveCheckInfo = objHotLeadBL.SaveCheckData(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                            pmntStatus, strIp, VoiceRecord, PostingID, AccType, BankRouting, bankName, AccNumber, AccHolderName, VoiceFileLocation, CheckType, CheckNumber);
                    PSID1 = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PSID1"].ToString());
                    Session["AddAnotherCarSalePSID1"] = PSID1;
                    PaymentID = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AddAnotherCarSalePaymentID"] = PaymentID;
                    if (VoiceRecord.ToString() != "")
                    {
                        DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                    }
                    else
                    {
                        if (LeadStatus != 1)
                        {
                            DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForSales(PostingID, LeadStatus);
                        }
                    }    
                }
            }
            int FeatureID;
            int IsactiveFea;
            for (int i = 1; i < 54; i++)
            {
                if (i != 10)
                {
                    if (i != 37)
                    {
                        if (i != 38)
                        {
                            string ChkBoxID = "chkFeatures" + i.ToString();
                            CheckBox ChkedBox = (CheckBox)form1.FindControl(ChkBoxID);
                            if (ChkedBox.Checked == true)
                            {
                                IsactiveFea = 1;
                            }
                            else
                            {
                                IsactiveFea = 0;
                            }
                            FeatureID = i;
                            bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
                        }
                    }
                }
            }
            if (rdbtnLeather.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 10;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 10;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            if (rdbtnVinyl.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 37;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 37;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            if (rdbtnCloth.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 38;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 38;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }

            if (rdbtnInteriorNA.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 54;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 54;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, RegUID);
            }
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
            int LeadStatus = Convert.ToInt32(3);
            int SaleAgentID = 0;
            SaveInfo(LeadStatus, SaleAgentID);
            mpealteruserUpdated.Show();
            lblErrUpdated.Visible = true;
            lblErrUpdated.Text = "Customer details saved successfully with sale id " + Session["NewSaleCarID"].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnDraftPhNoYes_Click(object sender, EventArgs e)
    {
        try
        {
            int LeadStatus;
            if (Session["NewAbandonRedirect"].ToString() == "Draft")
            {
                LeadStatus = Convert.ToInt32(3);
                DataSet dsUsers = Session["dsUserExists"] as DataSet;
                Session["NewSaleCarID"] = dsUsers.Tables[0].Rows[0]["carid"].ToString();
                Session["NewSaleUID"] = dsUsers.Tables[0].Rows[0]["uid"].ToString();
                Session["NewSalePostingID"] = dsUsers.Tables[0].Rows[0]["postingID"].ToString();
                Session["NewSaleUserPackID"] = dsUsers.Tables[0].Rows[0]["UserPackID"].ToString();
                Session["NewSaleSellerID"] = dsUsers.Tables[0].Rows[0]["sellerID"].ToString();
                Session["NewSalePSID1"] = dsUsers.Tables[0].Rows[0]["PSID1"].ToString();
                Session["NewSalePSID2"] = dsUsers.Tables[0].Rows[0]["PSID2"].ToString();
                Session["NewSalePaymentID"] = dsUsers.Tables[0].Rows[0]["PaymentID"].ToString();
                int SaleAgentID = 0;
                SaveInfo(LeadStatus, SaleAgentID);
            }
            if (Session["NewAbandonRedirect"].ToString() == "Abandon")
            {
                LeadStatus = Convert.ToInt32(2);
                DataSet dsUsers = Session["dsUserExists"] as DataSet;
                Session["NewSaleCarID"] = dsUsers.Tables[0].Rows[0]["carid"].ToString();
                Session["NewSaleUID"] = dsUsers.Tables[0].Rows[0]["uid"].ToString();
                Session["NewSalePostingID"] = dsUsers.Tables[0].Rows[0]["postingID"].ToString();
                Session["NewSaleUserPackID"] = dsUsers.Tables[0].Rows[0]["UserPackID"].ToString();
                Session["NewSaleSellerID"] = dsUsers.Tables[0].Rows[0]["sellerID"].ToString();
                Session["NewSalePSID1"] = dsUsers.Tables[0].Rows[0]["PSID1"].ToString();
                Session["NewSalePSID2"] = dsUsers.Tables[0].Rows[0]["PSID2"].ToString();
                Session["NewSalePaymentID"] = dsUsers.Tables[0].Rows[0]["PaymentID"].ToString();
                int SaleAgentID = 0;
                SaveInfo(LeadStatus, SaleAgentID);
                Response.Redirect("NewSale.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void btnDraftPhNoNo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    protected void BtnClsUpdated_Click(object sender, EventArgs e)
    {
        try
        {
            Session["AbandonSalePostingID"] = null;
            Response.Redirect("NewSale.aspx");
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
    protected void rdbtnPayVisa_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "block";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "none";
            if (chkboxlstPDsale.Checked == true)
            {
                // divpdDate.Style["display"] = "block";
            }
            else
            {
                // divpdDate.Style["display"] = "none";
            }
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
            btnProcess.Visible = true;
            chkboxlstPDsale.Enabled = true;
            ddlPDDate.Enabled = true;
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
            if (chkboxlstPDsale.Checked == true)
            {
                // divpdDate.Style["display"] = "block";
            }
            else
            {
                // divpdDate.Style["display"] = "none";
            }
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
            btnProcess.Visible = true;
            chkboxlstPDsale.Enabled = true;
            ddlPDDate.Enabled = true;
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
            if (chkboxlstPDsale.Checked == true)
            {
                //divpdDate.Style["display"] = "block";
            }
            else
            {
                //divpdDate.Style["display"] = "none";
            }
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
            btnProcess.Visible = true;
            chkboxlstPDsale.Enabled = true;
            ddlPDDate.Enabled = true;
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
            if (chkboxlstPDsale.Checked == true)
            {
                //divpdDate.Style["display"] = "block";
            }
            else
            {
                // divpdDate.Style["display"] = "none";
            }
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
            btnProcess.Visible = true;
            chkboxlstPDsale.Enabled = true;
            ddlPDDate.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rdbtnPayPaypal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "none";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "block";
            if (chkboxlstPDsale.Checked == true)
            {
                // divpdDate.Style["display"] = "block";
            }
            else
            {
                //divpdDate.Style["display"] = "none";
            }
            //FillPaymentDate();
            txtPaytransID.Text = "";
            //txtPayAmount.Text = "";
            txtpayPalEmailAccount.Text = "";
            btnProcess.Visible = true;
            chkboxlstPDsale.Enabled = false;
            ddlPDDate.Enabled = false;
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
            if (chkboxlstPDsale.Checked == true)
            {
                //divpdDate.Style["display"] = "block";
            }
            else
            {
                // divpdDate.Style["display"] = "none";
            }
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
            btnProcess.Visible = true;
            chkboxlstPDsale.Enabled = true;
            ddlPDDate.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void chkboxlstPDsale_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //if (chkboxlstPDsale.Checked == true)
            //{
            //    divpdDate.Style["display"] = "block";
            //    FillPDDate();
            //    txtPDAmountNow.Text = "";
            //    txtPDAmountLater.Text = "";
            //    btnProcess.Visible = true;
            //}
            //else
            //{
            //    divpdDate.Style["display"] = "none";
            //    btnProcess.Visible = true;
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnPhoneOk_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(txtLoadPhone.Text.Trim());
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                string SaleID = dsUserExists.Tables[0].Rows[0]["carid"].ToString();
                MPEUpdate.Hide();
                MdepAddAnotherCarAlert.Show();
                lblAddAnotherCarAlertError.Visible = true;
                if (dsUserExists.Tables[0].Rows.Count > 1)
                {
                    lblAddAnotherCarAlertError.Text = dsUserExists.Tables[0].Rows.Count.ToString() + " Sale(s) already exists with this phone " + txtLoadPhone.Text.Trim() + "<br />Do you want add another car?";
                }
                else
                {
                    lblAddAnotherCarAlertError.Text = "Sale already exists with this phone " + txtLoadPhone.Text.Trim() + " as <br />sale id " + SaleID + "<br />Do you want add another car?";
                }
            }
            else
            {
                DataSet dsReturnExists = objHotLeadBL.ChkUserPhoneNumberExistsForReturned(txtLoadPhone.Text.Trim());
                if (dsReturnExists.Tables[0].Rows.Count > 0)
                {
                    Session["AbandonSalePostingID"] = Convert.ToInt32(dsReturnExists.Tables[0].Rows[0]["postingID"].ToString());
                    MPEUpdate.Hide();
                    mdepDraftExistsShow.Show();
                    lblDraftExistsShow.Visible = true;
                    lblDraftExistsShow.Text = "Return sale already exists with this phone " + txtLoadPhone.Text.Trim() + "<br />Do you want to get details?";
                }
                else
                {
                    DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(txtLoadPhone.Text.Trim());
                    if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                    {
                        Session["AbandonSalePostingID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString());
                        MPEUpdate.Hide();
                        mdepDraftExistsShow.Show();
                        lblDraftExistsShow.Visible = true;
                        lblDraftExistsShow.Text = "Draft/Abandon sale already exists with this phone " + txtLoadPhone.Text.Trim() + "<br />Do you want to get details?";
                    }
                    else
                    {
                        MPEUpdate.Hide();
                        txtPhone.Text = objGeneralFunc.filPhnm(txtLoadPhone.Text.Trim());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnAlertExistShow_Click(object sender, EventArgs e)
    {
        try
        {
            mdepExistsAlertShow.Hide();
            MPEUpdate.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnDraftExistsShowYes_Click(object sender, EventArgs e)
    {
        try
        {
            Session["AddCarRedirectFrom"] = "FromNewCar";
            Response.Redirect("AddAnotherCar.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnDraftExistsShowNo_Click(object sender, EventArgs e)
    {
        try
        {
            Session["AbandonSalePostingID"] = null;
            Session["AddAnotherCarSaleUID"] = null;
            Response.Redirect("NewSale.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnAddAnotherCarYes_Click(object sender, EventArgs e)
    {
        try
        {
            //Response.Redirect("NewSale.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnAddAnotherCarNo_Click(object sender, EventArgs e)
    {
        try
        {
            mdepExistsAlertShow.Show();
            MdepAddAnotherCarAlert.Hide();
            lblExistsAlertShow.Visible = true;
            lblExistsAlertShow.Text = "Sale already exists with this phone " + txtLoadPhone.Text.Trim() + "<br />Please change phone #";
            //Session["AbandonSalePostingID"] = null;
            //MPEUpdate.Show();
            //txtPhone.Text = objGeneralFunc.filPhnm(txtLoadPhone.Text.Trim());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
