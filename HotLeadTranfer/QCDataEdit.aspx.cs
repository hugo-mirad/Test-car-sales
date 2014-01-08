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

public partial class QCDataEdit : System.Web.UI.Page
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
            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
            if (!IsPostBack)
            {
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
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
                        if (Session["DsDropDown"] == null)
                        {
                            dsDropDown = objdropdownBL.Usp_Get_DropDown();
                            Session["DsDropDown"] = dsDropDown;
                        }
                        else
                        {
                            dsDropDown = (DataSet)Session["DsDropDown"];
                        }
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
                        FillVoiceFileLocation();
                        FillBillingStates();
                        FillPhotoSource();
                        FillDescriptionSource();
                        //FillCheckTypes();
                        FillDiscounts();
                      
                        if ((Session["AgentQCPostingID"] != null) && (Session["AgentQCPostingID"].ToString() != ""))
                        {
                            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
                            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingID);

                            Double PackCost2 = new Double();
                            PackCost2 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
                            string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
                            string PackName2 = Cardetais.Tables[0].Rows[0]["Description"].ToString();
                            ListItem listPack = new ListItem();
                            listPack.Value = Cardetais.Tables[0].Rows[0]["PackageID"].ToString();
                            listPack.Text = PackName2 + " ($" + PackAmount2 + ")";
                            ddlPackage.SelectedIndex = ddlPackage.Items.IndexOf(listPack);

                            lblSaleID.Text = Cardetais.Tables[0].Rows[0]["carid"].ToString();
                            if (Cardetais.Tables[0].Rows[0]["SaleDate"].ToString() != "")
                            {
                                DateTime Saledt = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SaleDate"].ToString());
                                lblSaleDate.Text = Saledt.ToString("MM/dd/yyyy hh:mm tt");
                            }
                            if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "1")
                            {
                                lblQCStatus.Text = "QC Approved";
                            }
                            else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "2")
                            {
                                lblQCStatus.Text = "QC Reject";
                            }
                            else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "3")
                            {
                                lblQCStatus.Text = "QC Pending";
                            }
                            else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "4")
                            {
                                lblQCStatus.Text = "QC Returned";
                            }
                            else
                            {
                                lblQCStatus.Text = "QC Open";
                            }
                            lblPaymentStatusView.Text = Cardetais.Tables[0].Rows[0]["PSStatusName1"].ToString();
                            lblLocation.Text = Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString();
                            lblSaleAgent.Text = Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString();
                            
                            lblVerifierName.Text = Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString();
                          
                            lblVerifierLocation.Text = Cardetais.Tables[0].Rows[0]["SaleverifierCenterCode"].ToString();

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
                            //Filldiscount Value DiscountType DisCountTypeId

                            ListItem listdiscount = new ListItem();
                            listdiscount.Value = Cardetais.Tables[0].Rows[0]["DiscountId"].ToString();
                            if (listdiscount.Value == "") listdiscount.Value = "0";
                            //DataSet Discouname = objdropdownBL.GetPackageText(Convert.ToInt32(listdiscount.Value));
                            //listdiscount.Text = Discouname.Tables[0].Rows[0]["DiscounName"].ToString();
                            if (ddlPackage.Text != "5")
                                discountchk.Visible = true;
                            else
                            {
                                discountchk.Visible = true;
                               
                                if (listdiscount.Text == "0") listdiscount.Value = "0";
                                else if (listdiscount.Text == "25") listdiscount.Value = "1";
                            }
                           
                            ddldiscount.SelectedIndex = Convert.ToInt32(listdiscount.Value);

                            ListItem listState = new ListItem();
                            listState.Value = Cardetais.Tables[0].Rows[0]["StateID"].ToString();
                            listState.Text = Cardetais.Tables[0].Rows[0]["state"].ToString();
                            ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(listState);
                            txtZip.Text = Cardetais.Tables[0].Rows[0]["zip"].ToString();

                            ListItem list2 = new ListItem();
                            list2.Value = Cardetais.Tables[0].Rows[0]["MakeID"].ToString();
                            list2.Text = Cardetais.Tables[0].Rows[0]["make"].ToString();
                            ddlMake.SelectedIndex = ddlMake.Items.IndexOf(list2);
                            GetModelsInfo();

                            ListItem list3 = new ListItem();
                            list3.Text = Cardetais.Tables[0].Rows[0]["model"].ToString();
                            list3.Value = Cardetais.Tables[0].Rows[0]["makeModelID"].ToString();
                            ddlModel.SelectedIndex = ddlModel.Items.IndexOf(list3);

                            ListItem list1 = new ListItem();
                            list1.Text = Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString();
                            list1.Value = Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString();
                            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(list1);

                            ListItem listBody = new ListItem();
                            listBody.Value = Cardetais.Tables[0].Rows[0]["bodyTypeID"].ToString();
                            listBody.Text = Cardetais.Tables[0].Rows[0]["bodyType"].ToString();
                            ddlBodyStyle.SelectedIndex = ddlBodyStyle.Items.IndexOf(listBody);

                            if (Cardetais.Tables[0].Rows[0]["Carprice"].ToString() == "0.0000")
                            {
                                txtAskingPrice.Text = "";
                            }
                            else
                            {
                                txtAskingPrice.Text = string.Format("{0:0}", Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Carprice"].ToString()));
                            }
                            if (txtAskingPrice.Text.Length > 6)
                            {
                                txtAskingPrice.Text = txtAskingPrice.Text.Substring(0, 6);
                            }

                            if (Cardetais.Tables[0].Rows[0]["mileage"].ToString() == "0.00")
                            {
                                txtMileage.Text = "";
                            }
                            else
                            {
                                txtMileage.Text = string.Format("{0:0}", Convert.ToDouble(Cardetais.Tables[0].Rows[0]["mileage"].ToString()));
                            }
                            if (txtMileage.Text.Length > 6)
                            {
                                txtMileage.Text = txtMileage.Text.Substring(0, 6);
                            }

                            string NumberOfCylinder = Cardetais.Tables[0].Rows[0]["numberOfCylinder"].ToString();
                            if (NumberOfCylinder == "3 Cylinder")
                            {
                                rdbtnCylinders1.Checked = true;
                                rdbtnCylinders7.Checked = false;
                            }
                            else if (NumberOfCylinder == "4 Cylinder")
                            {
                                rdbtnCylinders2.Checked = true;
                                rdbtnCylinders7.Checked = false;
                            }
                            else if (NumberOfCylinder == "5 Cylinder")
                            {
                                rdbtnCylinders3.Checked = true;
                                rdbtnCylinders7.Checked = false;
                            }
                            else if (NumberOfCylinder == "6 Cylinder")
                            {
                                rdbtnCylinders4.Checked = true;
                                rdbtnCylinders7.Checked = false;
                            }
                            else if (NumberOfCylinder == "7 Cylinder")
                            {
                                rdbtnCylinders5.Checked = true;
                                rdbtnCylinders7.Checked = false;
                            }
                            else if (NumberOfCylinder == "8 Cylinder")
                            {
                                rdbtnCylinders6.Checked = true;
                                rdbtnCylinders7.Checked = false;
                            }
                            else
                            {
                                rdbtnCylinders7.Checked = true;
                            }

                            ListItem list7 = new ListItem();
                            list7.Value = Cardetais.Tables[0].Rows[0]["exteriorColor"].ToString();
                            list7.Text = Cardetais.Tables[0].Rows[0]["exteriorColor"].ToString();
                            ddlExteriorColor.SelectedIndex = ddlExteriorColor.Items.IndexOf(list7);


                            ListItem list8 = new ListItem();
                            list8.Text = Cardetais.Tables[0].Rows[0]["interiorColor"].ToString();
                            list8.Value = Cardetais.Tables[0].Rows[0]["interiorColor"].ToString();
                            ddlInteriorColor.SelectedIndex = ddlInteriorColor.Items.IndexOf(list8);

                            string Transmission = Cardetais.Tables[0].Rows[0]["Transmission"].ToString();
                            if (Transmission == "Automatic")
                            {
                                rdbtnTrans1.Checked = true;
                                rdbtnTrans4.Checked = false;
                            }
                            else if (Transmission == "Manual")
                            {
                                rdbtnTrans2.Checked = true;
                                rdbtnTrans4.Checked = false;
                            }
                            else if (Transmission == "Tiptronic")
                            {
                                rdbtnTrans3.Checked = true;
                                rdbtnTrans4.Checked = false;
                            }
                            else
                            {
                                rdbtnTrans4.Checked = true;
                            }
                            string NumberOfDoors = Cardetais.Tables[0].Rows[0]["numberOfDoors"].ToString();
                            if (NumberOfDoors == "Two Door")
                            {
                                rdbtnDoor2.Checked = true;
                                rdbtnDoor6.Checked = false;
                            }
                            else if (NumberOfDoors == "Three Door")
                            {
                                rdbtnDoor3.Checked = true;
                                rdbtnDoor6.Checked = false;
                            }
                            else if (NumberOfDoors == "Four Door")
                            {
                                rdbtnDoor4.Checked = true;
                                rdbtnDoor6.Checked = false;
                            }

                            else if (NumberOfDoors == "Five Door")
                            {
                                rdbtnDoor5.Checked = true;
                                rdbtnDoor6.Checked = false;
                            }
                            else
                            {
                                rdbtnDoor6.Checked = true;
                            }

                            string DriveTrain = Cardetais.Tables[0].Rows[0]["DriveTrain"].ToString();
                            if (DriveTrain == "2 wheel drive")
                            {
                                rdbtnDriveTrain1.Checked = true;
                                rdbtnDriveTrain5.Checked = false;
                            }
                            else if (DriveTrain == "2 wheel drive - front")
                            {
                                rdbtnDriveTrain2.Checked = true;
                                rdbtnDriveTrain5.Checked = false;
                            }
                            else if (DriveTrain == "All wheel drive")
                            {
                                rdbtnDriveTrain3.Checked = true;
                                rdbtnDriveTrain5.Checked = false;
                            }
                            else if (DriveTrain == "Rear wheel drive")
                            {
                                rdbtnDriveTrain4.Checked = true;
                                rdbtnDriveTrain5.Checked = false;
                            }
                            else
                            {
                                rdbtnDriveTrain5.Checked = true;
                            }
                            txtVin.Text = Cardetais.Tables[0].Rows[0]["VIN"].ToString();

                            int FuelTypeID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["fuelTypeID"].ToString());
                            if (FuelTypeID == 1)
                            {
                                rdbtnFuelType1.Checked = true;
                                rdbtnFuelType8.Checked = false;
                            }
                            else if (FuelTypeID == 2)
                            {
                                rdbtnFuelType2.Checked = true;
                                rdbtnFuelType8.Checked = false;
                            }
                            else if (FuelTypeID == 3)
                            {
                                rdbtnFuelType3.Checked = true;
                                rdbtnFuelType8.Checked = false;
                            }
                            else if (FuelTypeID == 4)
                            {
                                rdbtnFuelType4.Checked = true;
                                rdbtnFuelType8.Checked = false;
                            }
                            else if (FuelTypeID == 5)
                            {
                                rdbtnFuelType5.Checked = true;
                                rdbtnFuelType8.Checked = false;
                            }
                            else if (FuelTypeID == 6)
                            {
                                rdbtnFuelType6.Checked = true;
                                rdbtnFuelType8.Checked = false;
                            }
                            else if (FuelTypeID == 7)
                            {
                                rdbtnFuelType7.Checked = true;
                                rdbtnFuelType8.Checked = false;
                            }
                            else
                            {
                                rdbtnFuelType8.Checked = true;
                            }
                            int ConditionID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["vehicleConditionID"].ToString());
                            if (ConditionID == 1)
                            {
                                rdbtnCondition1.Checked = true;
                                rdbtnCondition7.Checked = false;
                            }
                            else if (ConditionID == 2)
                            {
                                rdbtnCondition2.Checked = true;
                                rdbtnCondition7.Checked = false;
                            }
                            else if (ConditionID == 3)
                            {
                                rdbtnCondition3.Checked = true;
                                rdbtnCondition7.Checked = false;
                            }
                            else if (ConditionID == 4)
                            {
                                rdbtnCondition4.Checked = true;
                                rdbtnCondition7.Checked = false;
                            }
                            else if (ConditionID == 5)
                            {
                                rdbtnCondition5.Checked = true;
                                rdbtnCondition7.Checked = false;
                            }
                            else if (ConditionID == 6)
                            {
                                rdbtnCondition6.Checked = true;
                                rdbtnCondition7.Checked = false;
                            }
                            else
                            {
                                rdbtnCondition7.Checked = true;
                            }
                            try
                            {
                                
                                FillAgents();
                              
                                FillVerifiers();
                            }
                            catch { }
                            try
                            {
                                try
                                {
                                    ddlUpAgenU.SelectedItem.Text = lblSaleAgent.Text.Trim();
                                    
                                }
                                catch { }
                                try
                                {
                                    ddlVerfNamU.SelectedItem.Text = lblVerifierName.Text.Trim();
                                }
                                catch { }
                            }
                            catch { }
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
                                            if (Cardetais.Tables[1].Rows.Count >= i)
                                            {
                                                if (Cardetais.Tables[1].Rows[i - 1]["Isactive"].ToString() == "True")
                                                {
                                                    ChkedBox.Checked = true;
                                                }
                                                else
                                                {
                                                    ChkedBox.Checked = false;
                                                }
                                            }
                                            else
                                            {
                                                ChkedBox.Checked = false;
                                            }
                                        }
                                    }
                                }
                            }
                            if (Cardetais.Tables[1].Rows.Count > 9)
                            {
                                if (Cardetais.Tables[1].Rows[9]["Isactive"].ToString() == "True")
                                {
                                    rdbtnLeather.Checked = true;
                                    rdbtnInteriorNA.Checked = false;
                                }
                            }
                            if (Cardetais.Tables[1].Rows.Count > 36)
                            {
                                if (Cardetais.Tables[1].Rows[36]["Isactive"].ToString() == "True")
                                {
                                    rdbtnVinyl.Checked = true;
                                    rdbtnInteriorNA.Checked = false;
                                }
                            }
                            if (Cardetais.Tables[1].Rows.Count > 37)
                            {
                                if (Cardetais.Tables[1].Rows[37]["Isactive"].ToString() == "True")
                                {
                                    rdbtnCloth.Checked = true;
                                    rdbtnInteriorNA.Checked = false;
                                }
                            }
                            if (Cardetais.Tables[1].Rows.Count > 53)
                            {
                                if (Cardetais.Tables[1].Rows[53]["Isactive"].ToString() == "True")
                                {
                                    rdbtnInteriorNA.Checked = true;
                                }
                            }
                            txtDescription.Text = Cardetais.Tables[0].Rows[0]["Cardescription"].ToString();
                            string OldNotes = Cardetais.Tables[0].Rows[0]["SaleNotes"].ToString();
                            OldNotes = OldNotes.Replace("<br>", Environment.NewLine);
                            txtSaleNotes.Text = OldNotes;
                            ListItem liSourceofPhotos = new ListItem();
                            liSourceofPhotos.Text = Cardetais.Tables[0].Rows[0]["SourceOfPhotosName"].ToString();
                            liSourceofPhotos.Value = Cardetais.Tables[0].Rows[0]["SourceOfPhotosID"].ToString();
                            ddlPhotosSource.SelectedIndex = ddlPhotosSource.Items.IndexOf(liSourceofPhotos);

                            ListItem liVoiceLocation = new ListItem();
                            liVoiceLocation.Text = Cardetais.Tables[0].Rows[0]["VoiceFileLocationName"].ToString();
                            liVoiceLocation.Value = Cardetais.Tables[0].Rows[0]["VoiceFileLocation"].ToString();
                            ddlVoiceFileLocation.SelectedIndex = ddlVoiceFileLocation.Items.IndexOf(liVoiceLocation);

                            ListItem liSourceofDescription = new ListItem();
                            liSourceofDescription.Text = Cardetais.Tables[0].Rows[0]["SourceOfDescriptionName"].ToString();
                            liSourceofDescription.Value = Cardetais.Tables[0].Rows[0]["SourceOfDescriptionID"].ToString();
                            ddlDescriptionSource.SelectedIndex = ddlDescriptionSource.Items.IndexOf(liSourceofDescription);

                            if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 1)
                            {
                                rdbtnPayVisa.Checked = true;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 2)
                            {
                                rdbtnPayMasterCard.Checked = true;
                                rdbtnPayVisa.Checked = false;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 3)
                            {
                                rdbtnPayAmex.Checked = true;
                                rdbtnPayVisa.Checked = false;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 4)
                            {
                                rdbtnPayDiscover.Checked = true;
                                rdbtnPayVisa.Checked = false;
                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                            {
                                rdbtnPayCheck.Checked = true;
                                rdbtnPayVisa.Checked = false;
                            }
                            else
                            {
                                rdbtnPayPaypal.Checked = true;
                                rdbtnPayVisa.Checked = false;
                            }
                            if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1"))
                            {
                                rdbtnPayVisa.Enabled = false;
                                rdbtnPayMasterCard.Enabled = false;
                                rdbtnPayAmex.Enabled = false;
                                rdbtnPayDiscover.Enabled = false;
                                rdbtnPayCheck.Enabled = false;
                                rdbtnPayPaypal.Enabled = false;
                                lnkbtnCopyCheckName.Enabled = false;
                                lnkbtnCopySellerInfo.Enabled = false;
                                ddlPDDate.Enabled = false;
                                chkboxlstPDsale.Enabled = false;
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
                                ListItem liCheckType = new ListItem();
                                liCheckType.Text = Cardetais.Tables[0].Rows[0]["CheckTypeName"].ToString();
                                liCheckType.Value = Cardetais.Tables[0].Rows[0]["CheckTypeID"].ToString();
                                //ddlCheckType.SelectedIndex = ddlCheckType.Items.IndexOf(liCheckType);
                                //txtCheckNumber.Text = Cardetais.Tables[0].Rows[0]["BankCheckNumber"].ToString();
                                if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1"))
                                {
                                    txtCustNameForCheck.Enabled = false;
                                    txtAccNumberForCheck.Enabled = false;
                                    txtBankNameForCheck.Enabled = false;
                                    txtRoutingNumberForCheck.Enabled = false;
                                    ddlAccType.Enabled = false;
                                    //ddlCheckType.Enabled = false;
                                    //txtCheckNumber.Enabled = false;
                                }

                            }
                            else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 6)
                            {
                                divcard.Style["display"] = "none";
                                divcheck.Style["display"] = "none";
                                divpaypal.Style["display"] = "block";
                                txtPaytransID.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                                txtpayPalEmailAccount.Text = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                                if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1"))
                                {
                                    txtPaytransID.Enabled = false;
                                    txtpayPalEmailAccount.Enabled = false;
                                }
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
                                if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1"))
                                {
                                    txtCardholderName.Enabled = false;
                                    txtCardholderLastName.Enabled = false;
                                    CardNumber.Enabled = false;
                                    ExpMon.Enabled = false;
                                    CCExpiresYear.Enabled = false;
                                    cvv.Enabled = false;
                                    txtbillingaddress.Enabled = false;
                                    txtbillingcity.Enabled = false;
                                    ddlbillingstate.Enabled = false;
                                    txtbillingzip.Enabled = false;
                                }
                            }
                            if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString() != "")
                            {
                                DateTime PayDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString());
                                txtPaymentDate.Text = PayDate.ToString("MM/dd/yyyy");
                            }
                            txtPDAmountNow.Text = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
                            if (Cardetais.Tables[0].Rows[0]["PSID2"].ToString() != "")
                            {
                                if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString() != "")
                                {
                                    chkboxlstPDsale.Checked = true;
                                    DateTime PDDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString());
                                    ListItem liPDDate = new ListItem();
                                    liPDDate.Text = PDDate.ToString("MM/dd/yyyy");
                                    liPDDate.Value = PDDate.ToString("MM/dd/yyyy");
                                    ddlPDDate.SelectedIndex = ddlPDDate.Items.IndexOf(liPDDate);
                                }
                            }

                            txtPDAmountLater.Text = Cardetais.Tables[0].Rows[0]["Amount2"].ToString();
                            txtVoicefileConfirmNo.Text = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString();

                            double TotalAmount;
                            if (txtPDAmountLater.Text != "")
                            {
                                TotalAmount = Convert.ToDouble(txtPDAmountNow.Text) + Convert.ToDouble(txtPDAmountLater.Text);
                                txtTotalAmount.Text = string.Format("{0:0.00}", TotalAmount);
                            }
                            else
                            {
                                txtTotalAmount.Text = txtPDAmountNow.Text;
                            }
                            //txtQCNotes.Text = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();
                            if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1"))
                            {
                                txtPaymentDate.Enabled = false;
                                txtPDAmountNow.Enabled = false;
                                chkboxlstPDsale.Enabled = false;
                                txtPDAmountLater.Enabled = false;
                                ddlPDDate.Enabled = false;
                            }

                            Session["AgentQCCarID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["carid"].ToString());
                            Session["AgentQCUID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["uid"].ToString());
                            Session["AgentQCPostingID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["postingID"].ToString());
                            Session["AgentQCUserPackID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["UserPackID"].ToString());
                            Session["AgentQCSellerID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["sellerID"].ToString());
                            Session["AgentQCPSID1"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSID1"].ToString());
                            Session["AgentQCPSStatusID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString());
                            Session["AgentQCPaymentStatusID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntStatus"].ToString());
                            if (Cardetais.Tables[0].Rows[0]["PSID2"].ToString() != "")
                            {
                                Session["AgentQCPSID2"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSID2"].ToString());
                            }
                            else
                            {
                                Session["AgentQCPSID2"] = "";
                            }
                            if (Cardetais.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                            {
                                Session["AgentQCPaymentID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PaymentID"].ToString());
                            }
                            else
                            {
                                Session["AgentQCPaymentID"] = "";
                            }
                            if (Cardetais.Tables[0].Rows[0]["QCID"].ToString() != "")
                            {
                                Session["AgentQCQCID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["QCID"].ToString());
                            }
                            else
                            {
                                Session["AgentQCQCID"] = "";
                            }


                            // lblVoiceFile.Text = Cardetais.Tables[0].Rows[0]["ConditionDescription"].ToString();
                        }

                    }
                }
            }
        }
        
    }

    protected void FillVerifiers()
    {
        try
        {
            int centerId = 0;
            if (lblLocation.Text == "TEST") centerId = 1;
            else if (lblLocation.Text == "INDG") centerId = 2;
            else if (lblLocation.Text == "PH01") centerId = 3;
            else if (lblLocation.Text == "INBH") centerId = 4;
            else if (lblLocation.Text == "USMP") centerId = 5;
            else if (lblLocation.Text == "CENTRAL") centerId = 6;
            else if (lblLocation.Text == "USWB") centerId = 7;
            DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(centerId);
            ddlVerfNamU.Items.Clear();
            ddlVerfNamU.DataSource = dsverifier;
            ddlVerfNamU.DataTextField = "AgentUFirstName";
            ddlVerfNamU.DataValueField = "AgentUID";
            ddlVerfNamU.DataBind();
            ddlVerfNamU.Items.Insert(0, new ListItem("All", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillAgents()
    {
       
        try
        {
            int centerId=0;
            if (lblVerifierLocation.Text == "TEST") centerId = 1;
            else if (lblVerifierLocation.Text == "INDG") centerId = 2;
            else if (lblVerifierLocation.Text == "PH01") centerId = 3;
            else if (lblVerifierLocation.Text == "INBH") centerId = 4;
            else if (lblVerifierLocation.Text == "USMP") centerId = 5;
            else if (lblVerifierLocation.Text == "CENTRAL") centerId = 6;
            else if (lblVerifierLocation.Text == "USWB") centerId = 7;
            DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(centerId);
            ddlUpAgenU.Items.Clear();
            ddlUpAgenU.DataSource = dsverifier;
            ddlUpAgenU.DataTextField = "AgentUFirstName";
            ddlUpAgenU.DataValueField = "AgentUID";
            ddlUpAgenU.DataBind();
            ddlUpAgenU.Items.Insert(0, new ListItem("All", "0"));
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

    protected void btnQCUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int LeadStatus = Convert.ToInt32(1);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            int UID = Convert.ToInt32(Session["AgentQCUID"].ToString());
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForQCSale(SellerPhone, PostingID, UID);
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                mdepAlertExists.Show();
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "Phone " + txtPhone.Text + " already exists.<br />Please change phone # to save";
            }
            else
            {
                DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExistsForQCSaleOtherSales(UID, PostingID);
                if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                {
                    Session["dsUserDraftExists"] = dsUserDraftExists;
                    mdepDraftExistsShow.Show();
                    lblDraftExistsShow.Visible = true;
                    lblDraftExistsShow.Text = "It appears this customer has multiple cars listed under this account.<br />Any changes to seller information(include phone number) will impact data for all can listed under this customer.<br />Do you to proceed with updates?";
                }
                else
                {
                    SaveInfo(LeadStatus);
                    int Status = Convert.ToInt32(1);

                    try
                    {

                        if (ddlUpAgenU.Text != "0" && ddlVerfNamU.Text!= "0")
                        {
                            //Added Agent and Veriufier Updates
                            DataSet dsverifier = objHotLeadBL.UpdateAgentVerifier(Convert.ToInt32(ddlUpAgenU.SelectedValue), Convert.ToInt32(ddlVerfNamU.SelectedValue), Convert.ToInt32(lblSaleID.Text));
                        }
                    }
                    catch { }
                   


                    //UpdateQCStatus(Status);
                    mpealteruserUpdated.Show();
                    lblErrUpdated.Visible = true;
                    lblErrUpdated.Text = "Sale Details updated successfully";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SaveInfo(int LeadStatus)
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
            int SaleAgentID = Convert.ToInt32(Session[Constants.USER_ID]);
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
            int SourceOfPhotos = Convert.ToInt32(ddlPhotosSource.SelectedItem.Value);
            int SourceOfDescription = Convert.ToInt32(ddlDescriptionSource.SelectedItem.Value);


            string LastName = objGeneralFunc.ToProper(txtLastName.Text.Trim());
            int CarID;
            int RegUID;
            int PostingID;
            int UserPackID;
            int sellerID;

            if ((Session["AgentQCCarID"] == null) || (Session["AgentQCCarID"].ToString() == ""))
            {
                CarID = Convert.ToInt32(0);
            }
            else
            {
                CarID = Convert.ToInt32(Session["AgentQCCarID"].ToString());
            }
            if ((Session["AgentQCUID"] == null) || (Session["AgentQCUID"].ToString() == ""))
            {
                RegUID = Convert.ToInt32(0);
            }
            else
            {
                RegUID = Convert.ToInt32(Session["AgentQCUID"].ToString());
            }
            if ((Session["AgentQCPostingID"] == null) || (Session["AgentQCPostingID"].ToString() == ""))
            {
                PostingID = Convert.ToInt32(0);
            }
            else
            {
                PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            }
            if ((Session["AgentQCUserPackID"] == null) || (Session["AgentQCUserPackID"].ToString() == ""))
            {
                UserPackID = Convert.ToInt32(0);
            }
            else
            {
                UserPackID = Convert.ToInt32(Session["AgentQCUserPackID"].ToString());
            }
            if ((Session["AgentQCSellerID"] == null) || (Session["AgentQCSellerID"].ToString() == ""))
            {
                sellerID = Convert.ToInt32(0);
            }
            else
            {
                sellerID = Convert.ToInt32(Session["AgentQCSellerID"].ToString());
            }
            int EmailExists = 1;
            if (chkbxEMailNA.Checked == true)
            {
                EmailExists = 0;
            }
            int DiscountID=0;
            if (ddldiscount.Text == "0") DiscountID = 0;
            else if (ddldiscount.Text == "1") DiscountID = 25;
            DataSet dsdata = objHotLeadBL.SaveSaleStatusDataForQCEdit(objUserregInfo, SaleAgentID, PackageID, DiscountID, YearOfMake, MakeModelID, BodyTypeID, ConditionID, DriveTrain,
                        Price, Mileage, ExteriorColor, InteriorColor, Transmission, NumberOfDoors, VIN, NumberOfCylinder, FuelTypeID, Description, Condition, Title,
                        State, strIp, InternalNotesNew, LeadStatus, LastName, SourceOfPhotos, SourceOfDescription, CarID, RegUID, UserPackID, sellerID, PostingID, EmailExists);

            CarID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["CarID"].ToString());
            Session["AgentQCCarID"] = CarID;
            RegUID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["UID"].ToString());
            Session["AgentQCUID"] = RegUID;
            PostingID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["PostingID"].ToString());
            Session["AgentQCPostingID"] = PostingID;
            UserPackID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["UserPackID"].ToString());
            Session["AgentQCUserPackID"] = UserPackID;
            sellerID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["sellerID"].ToString());
            Session["AgentQCSellerID"] = sellerID;

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
            if ((Session["AgentQCPSID1"] == null) || (Session["AgentQCPSID1"].ToString() == ""))
            {
                PSID1 = Convert.ToInt32(0);
            }
            else
            {
                PSID1 = Convert.ToInt32(Session["AgentQCPSID1"].ToString());
            }
            int PSID2;
            if ((Session["AgentQCPSID2"] == null) || (Session["AgentQCPSID2"].ToString() == ""))
            {
                PSID2 = Convert.ToInt32(0);
            }
            else
            {
                PSID2 = Convert.ToInt32(Session["AgentQCPSID2"].ToString());
            }
            int PaymentID;
            if ((Session["AgentQCPaymentID"] == null) || (Session["AgentQCPaymentID"].ToString() == ""))
            {
                PaymentID = Convert.ToInt32(0);
            }
            else
            {
                PaymentID = Convert.ToInt32(Session["AgentQCPaymentID"].ToString());
            }

            if ((rdbtnPayVisa.Checked == true) || (rdbtnPayMasterCard.Checked == true) || (rdbtnPayDiscover.Checked == true) || (rdbtnPayAmex.Checked == true))
            {
                if (chkboxlstPDsale.Checked == true)
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID = Convert.ToInt32(Session["AgentQCPSStatusID"].ToString());
                    int pmntStatus = Convert.ToInt32(Session["AgentQCPaymentStatusID"].ToString());
                    if ((PSStatusID == 4) || (PSStatusID == 3))
                    {
                        if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                        {
                            PSStatusID = 8;
                            pmntStatus = 2;
                        }
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
                    Session["AgentQCPSID1"] = PSID1;
                    PSID2 = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PSID2"].ToString());
                    Session["AgentQCPSID2"] = PSID2;
                    PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AgentQCPaymentID"] = PaymentID;
                }
                else
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID = Convert.ToInt32(Session["AgentQCPSStatusID"].ToString());
                    int pmntStatus = Convert.ToInt32(Session["AgentQCPaymentStatusID"].ToString());
                    if ((PSStatusID == 4) || (PSStatusID == 3))
                    {
                        if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                        {
                            PSStatusID = 8;
                            pmntStatus = 2;
                        }
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
                    DataSet dsSaveCCInfo = objHotLeadBL.SaveCreditCardData(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                            pmntStatus, strIp, VoiceRecord, CCCardNumber, Cardtype, CardExpDt, CardholderName, CardholderLastName, CardCode, BillingZip, BillingAdd,
                                            BillingCity, BillingState, PostingID, VoiceFileLocation);

                    PSID1 = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PSID1"].ToString());
                    Session["AgentQCPSID1"] = PSID1;
                    PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AgentQCPaymentID"] = PaymentID;
                }
            }
            if (rdbtnPayPaypal.Checked == true)
            {
                DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                string Amount = txtPDAmountNow.Text;
                int PSStatusID = Convert.ToInt32(Session["AgentQCPSStatusID"].ToString());
                int pmntStatus = Convert.ToInt32(Session["AgentQCPaymentStatusID"].ToString());
                if ((PSStatusID == 4) || (PSStatusID == 3))
                {
                    if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                    {
                        PSStatusID = 8;
                        pmntStatus = 2;
                    }
                }
                string TransID = txtPaytransID.Text;
                string PayPalEmailAcc = txtpayPalEmailAccount.Text;
                DataSet dsSavePayPalInfo = objHotLeadBL.SavePayPalData(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                        pmntStatus, strIp, VoiceRecord, TransID, PayPalEmailAcc, PostingID, VoiceFileLocation);

                PSID1 = Convert.ToInt32(dsSavePayPalInfo.Tables[0].Rows[0]["PSID1"].ToString());
                Session["AgentQCPSID1"] = PSID1;
                PaymentID = Convert.ToInt32(dsSavePayPalInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                Session["AgentQCPaymentID"] = PaymentID;

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
                    int PSStatusID = Convert.ToInt32(Session["AgentQCPSStatusID"].ToString());
                    int pmntStatus = Convert.ToInt32(Session["AgentQCPaymentStatusID"].ToString());
                    if ((PSStatusID == 4) || (PSStatusID == 3))
                    {
                        if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                        {
                            PSStatusID = 8;
                            pmntStatus = 2;
                        }
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
                    Session["AgentQCPSID1"] = PSID1;
                    PSID2 = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PSID2"].ToString());
                    Session["AgentQCPSID2"] = PSID2;
                    PaymentID = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AgentQCPaymentID"] = PaymentID;
                }
                else
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID = Convert.ToInt32(Session["AgentQCPSStatusID"].ToString());
                    int pmntStatus = Convert.ToInt32(Session["AgentQCPaymentStatusID"].ToString());
                    if ((PSStatusID == 4) || (PSStatusID == 3))
                    {
                        if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                        {
                            PSStatusID = 8;
                            pmntStatus = 2;
                        }
                    }
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
                    Session["AgentQCPSID1"] = PSID1;
                    PaymentID = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["AgentQCPaymentID"] = PaymentID;

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
                            bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
                        }
                    }
                }
            }
            if (rdbtnLeather.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 10;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 10;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }
            if (rdbtnVinyl.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 37;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 37;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }
            if (rdbtnCloth.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 38;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 38;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }

            if (rdbtnInteriorNA.Checked == true)
            {
                IsactiveFea = 1;
                FeatureID = 54;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }
            else
            {
                IsactiveFea = 0;
                FeatureID = 54;
                bool dsCarFeature = objHotLeadBL.SmartzUpdateFeatures(CarID, FeatureID, IsactiveFea, SaleAgentID);
            }

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
            int LeadStatus = Convert.ToInt32(1);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            int UID = Convert.ToInt32(Session["AgentQCUID"].ToString());
            DataSet dsUserDraftExists = new DataSet();
            dsUserDraftExists = Session["dsUserDraftExists"] as DataSet;
            if (dsUserDraftExists.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsUserDraftExists.Tables[0].Rows.Count; i++)
                {
                    int EmailExists = 1;
                    if (chkbxEMailNA.Checked == true)
                    {
                        EmailExists = 0;
                    }
                    string Name = objGeneralFunc.ToProper(txtFirstName.Text);
                    string email = txtEmail.Text;
                    string Address = objGeneralFunc.ToProper(txtAddress.Text);
                    string City = objGeneralFunc.ToProper(txtCity.Text);
                    string state = ddlLocationState.SelectedItem.Text;
                    string Zip = txtZip.Text;
                    string LastName = objGeneralFunc.ToProper(txtLastName.Text);
                    int sellerID = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[i]["sellerID"].ToString());
                    int SaleAgentID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                    DataSet dsSellerInfoUpdate = objHotLeadBL.UpdateSellerInfoForQCEdit(Name, Address, City, state, Zip, SellerPhone, email, LastName, sellerID, SaleAgentID);
                }
            }
            SaveInfo(LeadStatus);
            int Status = Convert.ToInt32(1);
            //UpdateQCStatus(Status);
            mdepDraftExistsShow.Hide();
            mpealteruserUpdated.Show();
            lblErrUpdated.Visible = true;
            lblErrUpdated.Text = "Sale Details updated successfully";
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
            //int QCBY = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            //int QCID = 0;
            //if ((Session["AgentQCQCID"] == null) || (Session["AgentQCQCID"].ToString() == ""))
            //{
            //    QCID = Convert.ToInt32(0);
            //}
            //else
            //{
            //    QCID = Convert.ToInt32(Session["AgentQCQCID"].ToString());
            //}
            //string QCNotes = string.Empty;
            //QCNotes = txtQCNotes.Text;
            //int CarID = Convert.ToInt32(Session["AgentQCCarID"].ToString());
            //int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            //DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(QCID, QCNotes, Status, CarID, QCBY, PostingID);
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
            Response.Redirect("QCDataView.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void rdbtnPayVisa_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divcard.Style["display"] = "block";
            divcheck.Style["display"] = "none";
            divpaypal.Style["display"] = "none";

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
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillDiscounts()
    {
        try
        {
            DataSet dsDropDown1 = objdropdownBL.GetDiscountpacka();
            for (int i = 1; i < dsDropDown1.Tables[0].Rows.Count; i++)
            {
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(dsDropDown1.Tables[0].Rows[i]["DiscountAmount"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = dsDropDown1.Tables[0].Rows[i]["DiscountType"].ToString();
                ListItem list = new ListItem();
                if (PackName == "Mobile Appdown Discount")
                    PackName = "Mobile Appdown Discount (25$)";
                list.Text = PackName;// +" ($" + PackAmount + ")";
                list.Value = dsDropDown1.Tables[0].Rows[i]["DisCountTypeId"].ToString();
                ddldiscount.Items.Add(list);
            }
            ddldiscount.Items.Insert(0, new ListItem("Zero", "0"));

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
}
