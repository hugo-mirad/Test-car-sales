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
using System.Net.Mail;
using System.IO;

public partial class QCDataView : System.Web.UI.Page
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
                        if (Session["DsDropDown"] == null)
                        {
                            dsDropDown = objdropdownBL.Usp_Get_DropDown();
                            Session["DsDropDown"] = dsDropDown;
                        }
                        else
                        {
                            dsDropDown = (DataSet)Session["DsDropDown"];
                        }
                        //DataSet dsYears = objHotLeadBL.USP_GetNext12years();

                        //fillYears(dsYears);
                        FillYear();
                        //FillPackage();
                        //FillStates();
                        GetAllModels();
                        GetMakes();
                        GetModelsInfo();
                        //FillExteriorColor();
                        //FillInteriorColor();
                        //FillVoiceFileLocation();
                        //GetBody();
                        FillPayCancelReason();
                        //FillPaymentDate();
                        //FillPhotoSource();
                        //FillDescriptionSource();
                        Session["ViewQCStatus"] = "";
                        //FillBillingStates();
                        //FillCheckTypes();
                        if ((Session["AgentQCPostingID"] != null) && (Session["AgentQCPostingID"].ToString() != ""))
                        {
                            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
                            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingID);

                            if ((Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "8"))
                            {
                                btnEdit.Visible = false;
                                ddlQCStatus.Visible = false;
                                btnQCUpdate.Visible = true;
                                //btnQCPass.Visible = false;
                                //btnQCReject.Visible = false;
                                //btnQCPending.Visible = false;
                                //btnEdit2.Visible = false;
                                //btnQCPass2.Visible = false;
                                //btnQCReject2.Visible = false;
                                //btnQCPending2.Visible = false;
                            }
                            if ((Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "3") || (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == ""))
                            {
                                btnEdit.Visible = true;
                                ddlQCStatus.Visible = false;
                                btnQCUpdate.Visible = true;
                            }
                            if (((Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "3") || (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "")) && (Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() != "1") && (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "8"))
                            {
                                btnEdit.Visible = true;
                                ddlQCStatus.Visible = false;
                                btnQCUpdate.Visible = true;
                            }
                            if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) != 6)
                            {
                                ListItem liPaySt = new ListItem();
                                liPaySt.Text = "FullyPaid";
                                liPaySt.Value = "1";
                                //ddlPmntStatus.SelectedIndex = ddlPmntStatus.Items.IndexOf(liPaySt);
                                ddlPmntStatus.Items.Remove(liPaySt);
                            }
                            else
                            {
                                if (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() != "3")
                                {
                                    if (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() != "4")
                                    {
                                        ListItem liPaySt = new ListItem();
                                        liPaySt.Text = "FullyPaid";
                                        liPaySt.Value = "1";
                                        //ddlPmntStatus.SelectedIndex = ddlPmntStatus.Items.IndexOf(liPaySt);
                                        ddlPmntStatus.Items.Remove(liPaySt);
                                    }
                                }
                            }
                            Double PackCost2 = new Double();
                            PackCost2 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
                            string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
                            string PackName2 = Cardetais.Tables[0].Rows[0]["Description"].ToString();
                            //ListItem listPack = new ListItem();
                            //listPack.Value = Cardetais.Tables[0].Rows[0]["PackageID"].ToString();
                            //listPack.Text = PackName2 + " ($" + PackAmount2 + ")";
                            //ddlPackage.SelectedIndex = ddlPackage.Items.IndexOf(listPack);
                            Session["QCViewPackageID"] = Cardetais.Tables[0].Rows[0]["PackageID"].ToString();
                            lblPackage.Text = PackName2 + " ($" + PackAmount2 + ")";
                            lbldiscountpacka.Text = Cardetais.Tables[0].Rows[0]["DiscountId"].ToString();
                            if (lbldiscountpacka.Text == "0") lbldiscountpacka.Text = "";
                            else if (lbldiscountpacka.Text == "25") lbldiscountpacka.Text = "Discount 25$";
                            else if (lbldiscountpacka.Text == "50") lbldiscountpacka.Text = "Discount 50$";

                            if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "1")
                            {
                                lblQCStatus.Text = "QC Approved";
                                if ((Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() != "1") && ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "8")))
                                {
                                    btnMovedToSmartz.Visible = true;
                                }
                                else if ((Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() != "1") && ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "3") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "4")))
                                {
                                    if (Cardetais.Tables[0].Rows[0]["Amount1"].ToString() != "")
                                    {
                                        Double TotalAmount1 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Amount1"].ToString());
                                        string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                                        if (ChkAmount == "0.00")
                                        {
                                            btnMovedToSmartz.Visible = true;
                                        }
                                        else
                                        {
                                            btnMovedToSmartz.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        btnMovedToSmartz.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnMovedToSmartz.Visible = false;
                                }
                                if ((Cardetais.Tables[0].Rows[0]["SmartzStatus"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "7") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "8"))
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
                                    if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "3") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "4"))
                                    {
                                        if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 2) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 3) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 4))
                                        {
                                            if (Cardetais.Tables[0].Rows[0]["Amount1"].ToString() != "")
                                            {
                                                Double TotalAmount1 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Amount1"].ToString());
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
                                            if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                                            {
                                                if (Cardetais.Tables[0].Rows[0]["Amount1"].ToString() != "")
                                                {
                                                    Double TotalAmount1 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Amount1"].ToString());
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
                            else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "2")
                            {
                                lblQCStatus.Text = "QC Reject";
                                btnProcess.Enabled = false;
                                btnProcess.Visible = false;
                                btnMovedToSmartz.Visible = false;
                            }
                            else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "4")
                            {
                                lblQCStatus.Text = "QC Returned";
                                btnProcess.Enabled = false;
                                btnProcess.Visible = false;
                                btnMovedToSmartz.Visible = false;
                            }
                            else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "3")
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
                            lblPaymentStatusView.Text = Cardetais.Tables[0].Rows[0]["PSStatusName1"].ToString();
                            lblSaleID.Text = Cardetais.Tables[0].Rows[0]["carid"].ToString();
                            if (Cardetais.Tables[0].Rows[0]["SaleDate"].ToString() != "")
                            {
                                DateTime Saledt = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SaleDate"].ToString());
                                lblSaleDate.Text = Saledt.ToString("MM/dd/yyyy hh:mm tt");
                            }
                            lblLocation.Text = Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString();
                            lblSaleAgent.Text = Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString();
                            lblVerifierName.Text = Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString();
                            lblVerifierLocation.Text = Cardetais.Tables[0].Rows[0]["SaleverifierCenterCode"].ToString();
                            string OldNotesPay = Cardetais.Tables[0].Rows[0]["PaymentNotes"].ToString();
                            OldNotesPay = OldNotesPay.Replace("<br>", Environment.NewLine);
                            txtPaymentNotes.Text = OldNotesPay;

                            //ListItem liVoiceLocation = new ListItem();
                            //liVoiceLocation.Text = Cardetais.Tables[0].Rows[0]["VoiceFileLocationName"].ToString();
                            //liVoiceLocation.Value = Cardetais.Tables[0].Rows[0]["VoiceFileLocation"].ToString();
                            //ddlVoiceFileLocation.SelectedIndex = ddlVoiceFileLocation.Items.IndexOf(liVoiceLocation);
                            txtVoiceFileLocation.Text = Cardetais.Tables[0].Rows[0]["VoiceFileLocationName"].ToString();

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
                            //ListItem listState = new ListItem();
                            //listState.Value = Cardetais.Tables[0].Rows[0]["StateID"].ToString();
                            //listState.Text = Cardetais.Tables[0].Rows[0]["state"].ToString();
                            //ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(listState);
                            lblLocationState.Text = Cardetais.Tables[0].Rows[0]["state"].ToString();
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

                            //ListItem listBody = new ListItem();
                            //listBody.Value = Cardetais.Tables[0].Rows[0]["bodyTypeID"].ToString();
                            //listBody.Text = Cardetais.Tables[0].Rows[0]["bodyType"].ToString();
                            //ddlBodyStyle.SelectedIndex = ddlBodyStyle.Items.IndexOf(listBody);
                            txtBodyStyle.Text = Cardetais.Tables[0].Rows[0]["bodyType"].ToString();
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
                            }
                            else if (NumberOfCylinder == "4 Cylinder")
                            {
                                rdbtnCylinders2.Checked = true;
                            }
                            else if (NumberOfCylinder == "5 Cylinder")
                            {
                                rdbtnCylinders3.Checked = true;
                            }
                            else if (NumberOfCylinder == "6 Cylinder")
                            {
                                rdbtnCylinders4.Checked = true;
                            }
                            else if (NumberOfCylinder == "7 Cylinder")
                            {
                                rdbtnCylinders5.Checked = true;
                            }
                            else if (NumberOfCylinder == "8 Cylinder")
                            {
                                rdbtnCylinders6.Checked = true;
                            }
                            else
                            {
                                rdbtnCylinders7.Checked = true;
                            }

                            //ListItem list7 = new ListItem();
                            //list7.Value = Cardetais.Tables[0].Rows[0]["exteriorColor"].ToString();
                            //list7.Text = Cardetais.Tables[0].Rows[0]["exteriorColor"].ToString();
                            //ddlExteriorColor.SelectedIndex = ddlExteriorColor.Items.IndexOf(list7);
                            txtExteriorColor.Text = Cardetais.Tables[0].Rows[0]["exteriorColor"].ToString();

                            //ListItem list8 = new ListItem();
                            //list8.Text = Cardetais.Tables[0].Rows[0]["interiorColor"].ToString();
                            //list8.Value = Cardetais.Tables[0].Rows[0]["interiorColor"].ToString();
                            //ddlInteriorColor.SelectedIndex = ddlInteriorColor.Items.IndexOf(list8);
                            txtInteriorColor.Text = Cardetais.Tables[0].Rows[0]["interiorColor"].ToString();

                            string Transmission = Cardetais.Tables[0].Rows[0]["Transmission"].ToString();
                            if (Transmission == "Automatic")
                            {
                                rdbtnTrans1.Checked = true;
                            }
                            else if (Transmission == "Manual")
                            {
                                rdbtnTrans2.Checked = true;
                            }
                            else if (Transmission == "Tiptronic")
                            {
                                rdbtnTrans3.Checked = true;
                            }
                            else
                            {
                                rdbtnTrans4.Checked = true;
                            }
                            string NumberOfDoors = Cardetais.Tables[0].Rows[0]["numberOfDoors"].ToString();
                            if (NumberOfDoors == "Two Door")
                            {
                                rdbtnDoor2.Checked = true;
                            }
                            else if (NumberOfDoors == "Three Door")
                            {
                                rdbtnDoor3.Checked = true;
                            }
                            else if (NumberOfDoors == "Four Door")
                            {
                                rdbtnDoor4.Checked = true;
                            }

                            else if (NumberOfDoors == "Five Door")
                            {
                                rdbtnDoor5.Checked = true;
                            }
                            else
                            {
                                rdbtnDoor6.Checked = true;
                            }

                            string DriveTrain = Cardetais.Tables[0].Rows[0]["DriveTrain"].ToString();
                            if (DriveTrain == "2 wheel drive")
                            {
                                rdbtnDriveTrain1.Checked = true;
                            }
                            else if (DriveTrain == "2 wheel drive - front")
                            {
                                rdbtnDriveTrain2.Checked = true;
                            }
                            else if (DriveTrain == "All wheel drive")
                            {
                                rdbtnDriveTrain3.Checked = true;
                            }
                            else if (DriveTrain == "Rear wheel drive")
                            {
                                rdbtnDriveTrain4.Checked = true;
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
                            }
                            else if (FuelTypeID == 2)
                            {
                                rdbtnFuelType2.Checked = true;
                            }
                            else if (FuelTypeID == 3)
                            {
                                rdbtnFuelType3.Checked = true;
                            }
                            else if (FuelTypeID == 4)
                            {
                                rdbtnFuelType4.Checked = true;
                            }
                            else if (FuelTypeID == 5)
                            {
                                rdbtnFuelType5.Checked = true;
                            }
                            else if (FuelTypeID == 6)
                            {
                                rdbtnFuelType6.Checked = true;
                            }
                            else if (FuelTypeID == 7)
                            {
                                rdbtnFuelType7.Checked = true;
                            }
                            else
                            {
                                rdbtnFuelType8.Checked = true;
                            }
                            int ConditionID = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["vehicleConditionID"].ToString());
                            if (ConditionID == 1)
                            {
                                rdbtnCondition1.Checked = true;
                            }
                            else if (ConditionID == 2)
                            {
                                rdbtnCondition2.Checked = true;
                            }
                            else if (ConditionID == 3)
                            {
                                rdbtnCondition3.Checked = true;
                            }
                            else if (ConditionID == 4)
                            {
                                rdbtnCondition4.Checked = true;
                            }
                            else if (ConditionID == 5)
                            {
                                rdbtnCondition5.Checked = true;
                            }
                            else if (ConditionID == 6)
                            {
                                rdbtnCondition6.Checked = true;
                            }
                            else
                            {
                                rdbtnCondition7.Checked = true;
                            }


                            for (int i = 1; i < Cardetais.Tables[1].Rows.Count; i++)
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
                                }
                            }
                            if (Cardetais.Tables[1].Rows.Count > 36)
                            {
                                if (Cardetais.Tables[1].Rows[36]["Isactive"].ToString() == "True")
                                {
                                    rdbtnVinyl.Checked = true;
                                }
                            }
                            if (Cardetais.Tables[1].Rows.Count > 37)
                            {
                                if (Cardetais.Tables[1].Rows[37]["Isactive"].ToString() == "True")
                                {
                                    rdbtnCloth.Checked = true;
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
                            //ListItem liSourceofPhotos = new ListItem();
                            //liSourceofPhotos.Text = Cardetais.Tables[0].Rows[0]["SourceOfPhotosName"].ToString();
                            //liSourceofPhotos.Value = Cardetais.Tables[0].Rows[0]["SourceOfPhotosID"].ToString();
                            //ddlPhotosSource.SelectedIndex = ddlPhotosSource.Items.IndexOf(liSourceofPhotos);
                            txtPhotosSource.Text = Cardetais.Tables[0].Rows[0]["SourceOfPhotosName"].ToString();
                            //ListItem liSourceofDescription = new ListItem();
                            //liSourceofDescription.Text = Cardetais.Tables[0].Rows[0]["SourceOfDescriptionName"].ToString();
                            //liSourceofDescription.Value = Cardetais.Tables[0].Rows[0]["SourceOfDescriptionID"].ToString();
                            //ddlDescriptionSource.SelectedIndex = ddlDescriptionSource.Items.IndexOf(liSourceofDescription);
                            txtDescriptionSource.Text = Cardetais.Tables[0].Rows[0]["SourceOfDescriptionName"].ToString();

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

                                //ListItem liExpMnth = new ListItem();
                                //liExpMnth.Text = EXpDt[0].ToString();
                                //liExpMnth.Value = EXpDt[0].ToString();
                                //ExpMon.SelectedIndex = ExpMon.Items.IndexOf(liExpMnth);

                                txtExpMon.Text = EXpDt[0].ToString();
                                //ListItem liExpyear = new ListItem();
                                //liExpyear.Text = "20" + EXpDt[1].ToString();
                                //liExpyear.Value = EXpDt[1].ToString();
                                //CCExpiresYear.SelectedIndex = CCExpiresYear.Items.IndexOf(liExpyear);
                                txtCCExpiresYear.Text = "20" + EXpDt[1].ToString();
                                cvv.Text = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();

                                txtbillingaddress.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString());
                                txtbillingcity.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString());

                                //ListItem liBillST = new ListItem();
                                //liBillST.Value = Cardetais.Tables[0].Rows[0]["billingState"].ToString();
                                //liBillST.Text = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
                                //ddlbillingstate.SelectedIndex = ddlbillingstate.Items.IndexOf(liBillST);
                                txtbillingstate.Text = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();

                                txtbillingzip.Text = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
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
                                    txtPDDate.Text = PDDate.ToString("MM/dd/yyyy");
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
                            txtOldQcNotes.Text = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();

                            Session["AgentQCCarID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["carid"].ToString());
                            Session["AgentQCUID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["uid"].ToString());
                            Session["AgentQCPostingID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["postingID"].ToString());
                            Session["AgentQCUserPackID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["UserPackID"].ToString());
                            Session["AgentQCSellerID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["sellerID"].ToString());
                            Session["AgentQCPSID1"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSID1"].ToString());
                            Session["AgentQCPaymentTypeID"] = Cardetais.Tables[0].Rows[0]["pmntType"].ToString();
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
        try
        {
            if (lblQCStatus.Text == "QC Approved")
            {
                btnQCUpdate.Enabled = false;
            }
            else
            {
                btnQCUpdate.Enabled = true;
            }
        }
        catch { }
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // your code!
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
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
    //private void FillVoiceFileLocation()
    //{
    //    try
    //    {
    //        DataSet dsVoiceFileLocation = objCentralDBBL.GetVoiceFileLocation();
    //        ddlVoiceFileLocation.DataSource = dsVoiceFileLocation.Tables[0];
    //        ddlVoiceFileLocation.DataTextField = "VoiceFileLocationName";
    //        ddlVoiceFileLocation.DataValueField = "VoiceFileLocationID";
    //        ddlVoiceFileLocation.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //private void fillYears(DataSet dsYears)
    //{
    //    try
    //    {
    //        CCExpiresYear.Items.Clear();
    //        CCExpiresYear.DataSource = dsYears.Tables[0];
    //        CCExpiresYear.DataTextField = "YearNum";
    //        CCExpiresYear.DataValueField = "YearValue";
    //        CCExpiresYear.DataBind();
    //        CCExpiresYear.Items.Insert(0, new ListItem("Select Year", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //private void FillStates()
    //{
    //    try
    //    {
    //        ddlLocationState.DataSource = dsDropDown.Tables[1];
    //        ddlLocationState.DataTextField = "State_Code";
    //        ddlLocationState.DataValueField = "State_ID";
    //        ddlLocationState.DataBind();
    //        ddlLocationState.Items.Insert(0, new ListItem("Unspecified", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //private void FillBillingStates()
    //{
    //    try
    //    {
    //        ddlbillingstate.Items.Clear();
    //        if (Session["DsDropDown"] == null)
    //        {
    //            dsDropDown = objdropdownBL.Usp_Get_DropDown();
    //            Session["DsDropDown"] = dsDropDown;
    //        }
    //        else
    //        {
    //            dsDropDown = (DataSet)Session["DsDropDown"];
    //        }

    //        ddlbillingstate.DataSource = dsDropDown.Tables[1];
    //        ddlbillingstate.DataTextField = "State_Code";
    //        ddlbillingstate.DataValueField = "State_ID";
    //        ddlbillingstate.DataBind();
    //        ddlbillingstate.Items.Insert(0, new ListItem("Unspecified", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
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
    //private void FillPackage()
    //{
    //    try
    //    {
    //        for (int i = 1; i < dsDropDown.Tables[2].Rows.Count; i++)
    //        {
    //            Double PackCost = new Double();
    //            PackCost = Convert.ToDouble(dsDropDown.Tables[2].Rows[i]["Price"].ToString());
    //            string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
    //            string PackName = dsDropDown.Tables[2].Rows[i]["Description"].ToString();
    //            ListItem list = new ListItem();
    //            list.Text = PackName + " ($" + PackAmount + ")";
    //            list.Value = dsDropDown.Tables[2].Rows[i]["PackageID"].ToString();
    //            ddlPackage.Items.Add(list);
    //        }
    //        ddlPackage.Items.Insert(0, new ListItem("Select", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
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
    //private void FillInteriorColor()
    //{
    //    try
    //    {
    //        ddlInteriorColor.DataSource = dsDropDown.Tables[4];
    //        ddlInteriorColor.DataTextField = "InteriorColorName";
    //        ddlInteriorColor.DataValueField = "InteriorColorName";
    //        ddlInteriorColor.DataBind();
    //        ddlInteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private void FillExteriorColor()
    //{
    //    try
    //    {
    //        ddlExteriorColor.DataSource = dsDropDown.Tables[3];
    //        ddlExteriorColor.DataTextField = "ExteriorColorName";
    //        ddlExteriorColor.DataValueField = "ExteriorColorName";
    //        ddlExteriorColor.DataBind();
    //        ddlExteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //private void FillDescriptionSource()
    //{
    //    try
    //    {
    //        DataSet dsDescripSource = objHotLeadBL.GetMasterSourceOfDescription();
    //        ddlDescriptionSource.DataSource = dsDescripSource.Tables[0];
    //        ddlDescriptionSource.DataTextField = "SourceOfDescriptionName";
    //        ddlDescriptionSource.DataValueField = "SourceOfDescriptionID";
    //        ddlDescriptionSource.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //private void FillPhotoSource()
    //{
    //    try
    //    {
    //        DataSet dsDescripPhotos = objHotLeadBL.USP_GetMasterSourceOfPhotos();
    //        ddlPhotosSource.DataSource = dsDescripPhotos.Tables[0];
    //        ddlPhotosSource.DataTextField = "SourceOfPhotosName";
    //        ddlPhotosSource.DataValueField = "SourceOfPhotosID";
    //        ddlPhotosSource.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public void GetBody()
    //{
    //    try
    //    {
    //        var obj = new List<BodyInfo>();

    //        //MakesInfo objMakes = new MakesInfo();
    //        MakesBL objMakesBL = new MakesBL();

    //        if (Session[Constants.Bodys] == null)
    //        {
    //            obj = (List<BodyInfo>)objMakesBL.GetBodys();
    //            Session["Bodys"] = obj;
    //        }
    //        else
    //        {
    //            obj = (List<BodyInfo>)Session[Constants.Bodys];
    //        }


    //        ddlBodyStyle.DataSource = obj;
    //        ddlBodyStyle.DataTextField = "bodyType";
    //        ddlBodyStyle.DataValueField = "bodyTypeID";
    //        ddlBodyStyle.DataBind();
    //        ddlBodyStyle.Items.Insert(0, new ListItem("Unspecified", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}



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


    protected void lnkbtnPaymentHistory_Click(object sender, EventArgs e)
    {
        try
        {
            if ((Session["AgentQCPaymentTypeID"].ToString() == "1") || (Session["AgentQCPaymentTypeID"].ToString() == "2") || (Session["AgentQCPaymentTypeID"].ToString() == "3") || (Session["AgentQCPaymentTypeID"].ToString() == "4"))
            {
                divresults.Style["display"] = "block";
                divCheckResults.Style["display"] = "none";
                int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
                DataSet PayHistory = objHotLeadBL.GetPaymentTransactionData(PostingID);
                if (PayHistory.Tables.Count > 0)
                {
                    if (PayHistory.Tables[0].Rows.Count > 0)
                    {
                        grdIntroInfo.Visible = true;
                        grdIntroInfo.DataSource = PayHistory.Tables[0];
                        grdIntroInfo.DataBind();
                        lblPayTransSaleID.Text = Session["AgentQCCarID"].ToString();
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
                int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
                DataSet PayHistory = objHotLeadBL.GetPaymentTransactionDataForChecks(PostingID);
                if (PayHistory.Tables.Count > 0)
                {
                    if (PayHistory.Tables[0].Rows.Count > 0)
                    {
                        grdCheckResults.Visible = true;
                        grdCheckResults.DataSource = PayHistory.Tables[0];
                        grdCheckResults.DataBind();
                        lblPayTransSaleID.Text = Session["AgentQCCarID"].ToString();
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
            int Status = Convert.ToInt32(ddlQCStatus.SelectedItem.Value);
            Session["ViewQCStatus"] = Status;
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

    protected void btnQCPass_Click(object sender, EventArgs e)
    {
        try
        {
            int Status = Convert.ToInt32(1);
            UpdateQCStatus(Status);
            mpealteruserUpdated.Show();
            lblErrUpdated.Visible = true;
            lblErrUpdated.Text = "QC Details updated successfully";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnQCFail_Click(object sender, EventArgs e)
    {
        try
        {
            int Status = Convert.ToInt32(2);
            UpdateQCStatus(Status);
            mpealteruserUpdated.Show();
            lblErrUpdated.Visible = true;
            lblErrUpdated.Text = "QC Details updated successfully";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnQCPending_Click(object sender, EventArgs e)
    {
        try
        {
            int Status = Convert.ToInt32(3);
            UpdateQCStatus(Status);
            mpealteruserUpdated.Show();
            lblErrUpdated.Visible = true;
            lblErrUpdated.Text = "QC Details updated successfully";
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
            if ((Session["AgentQCQCID"] == null) || (Session["AgentQCQCID"].ToString() == ""))
            {
                QCID = Convert.ToInt32(0);
            }
            else
            {
                QCID = Convert.ToInt32(Session["AgentQCQCID"].ToString());
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
            int CarID = Convert.ToInt32(Session["AgentQCCarID"].ToString());
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(QCID, QCNotes, Status, CarID, QCBY, PostingID);
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
            if ((Session["ViewQCStatus"].ToString() != "0") || (Session["ViewQCStatus"].ToString() == ""))
            {
                if ((Session["ViewQCStatus"].ToString() != "1"))
                {
                    Session["ViewQCStatus"] = "";
                    Response.Redirect("QCReport.aspx");
                }
                else
                {
                    Session["ViewQCStatus"] = "";
                    mpealteruserUpdated.Hide();
                    Response.Redirect("QCDataView.aspx");
                }
            }
            else
            {
                mpealteruserUpdated.Hide();
                Response.Redirect("QCDataView.aspx");
            }
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
            Response.Redirect("QCDataEdit.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
  protected void MoveSmartz_Click(object sender, EventArgs e)
    {
        try
        {

            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
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
                                        mpealteruserUpdatedSmartz.Show();
                                        lblErrUpdatedSmartz.Visible = true;
                                        lblErrUpdatedSmartz.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
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
                                mpealteruserUpdatedSmartz.Show();
                                lblErrUpdatedSmartz.Visible = true;
                                lblErrUpdatedSmartz.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
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
                            mpealteruserUpdatedSmartz.Show();
                            lblErrUpdatedSmartz.Visible = true;
                            lblErrUpdatedSmartz.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
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
                                mpealteruserUpdatedSmartz.Show();
                                lblErrUpdatedSmartz.Visible = true;
                                lblErrUpdatedSmartz.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
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
                        mpealteruserUpdatedSmartz.Show();
                        lblErrUpdatedSmartz.Visible = true;
                        lblErrUpdatedSmartz.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
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
                    mpealteruserUpdatedSmartz.Show();
                    lblErrUpdatedSmartz.Visible = true;
                    lblErrUpdatedSmartz.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
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
    protected void btnYesUpdatedSmartz1_Click(object sender, EventArgs e)
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
            if (Session["NewUserPayStatus"].ToString() == "5")
            {
                DateTime PostDate = Convert.ToDateTime(Session["NewUserPDDate"].ToString());
                PDDate = PostDate.ToString("MM/dd/yyyy");
                text = format.SendRegistrationdetailsForPDSales(RegLogUserID, LoginPassword, UserDisName, ref text, PDDate);
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
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            // AuthorizePayment();
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
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
            DataSet dsChkRejectThere = objHotLeadBL.CheckResultPaymentReject(CCFirstName, CCLastName, CCAddress, CCZip, CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, PostingID);
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

    //PaymentSucces.aspx
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
        if (Session["QCViewPackageID"].ToString() == "5")
        {
            Package = "Gold Deluxe Promo Package – No cancellations allowed; All sales are final";
        }
        else if (Session["QCViewPackageID"].ToString() == "4")
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
        //Discount 21-11-2013 starts 
        string DiscountAmount = "";
        try
        {
           
            if (lbldiscountpacka.Text == "Discount 25$")
                DiscountAmount = "25";
            else
                DiscountAmount = "0";

        }
        catch { DiscountAmount = "0"; }
        //Discount 21-11-2013 Ends

        try
        {
            if (Convert.ToDouble(FinalAmount) != (Convert.ToDouble(txtPDAmountNow.Text) + Convert.ToDouble(DiscountAmount)))
            {
                Package = Package + "- Partial payment -";
            }
        }
        catch {
            if (Convert.ToDouble(FinalAmount) != (Convert.ToDouble(txtPDAmountNow.Text)))
            {
                Package = Package + "- Partial payment -";
            }
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
                try
                {
                    if (Convert.ToDouble(txtPDAmountNow.Text).ToString() == "0")
                    {
                        Result = "NoPayDue";
                    }

                    else if (Convert.ToDouble(FinalAmount1) != Convert.ToDouble(txtPDAmountNow.Text) + Convert.ToDouble(DiscountAmount))
                    {
                        Result = "PartialPaid";
                    }
                    else
                    {
                        Result = "Paid";
                    }
                }
                catch { }
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
                int PSID = Convert.ToInt32(Session["AgentQCPSID1"].ToString());
                int PaymentID = Convert.ToInt32(Session["AgentQCPaymentID"].ToString());
                int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                int PSStatusID = Convert.ToInt32(3);
                int PmntStatus = 1;
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

    private void SavePayTransInfo(string AuthNetTransID, string Result)
    {
        try
        {
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            int PayTryBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            string CardType = string.Empty;

            if (Session["AgentQCPaymentTypeID"].ToString() == "1")
            {
                CardType = "Visa";
            }
            else if (Session["AgentQCPaymentTypeID"].ToString() == "2")
            {
                CardType = "Mastercard";
            }
            else if (Session["AgentQCPaymentTypeID"].ToString() == "3")
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
            DataSet dsSavePayTrans = objHotLeadBL.SavePaymentHistoryData(PostingID, PayTryBy, CardType, CCardNumber, Address, City, State,
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
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            int PayTryBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            string CardType = string.Empty;
            CardType = "Check";
            string Address = txtAddress.Text;
            string City = txtCity.Text;
            string State = lblLocationState.Text;
            string Zip = txtZip.Text;
            string Amount = txtPDAmountNow.Text;
            string AccountHolderName = txtCustNameForCheck.Text;
            string AccountNumber = txtAccNumberForCheck.Text;
            string BankName = txtBankNameForCheck.Text;
            string RoutingNumber = txtRoutingNumberForCheck.Text;
            string AccountType = ddlAccType.SelectedItem.Text;

            DataSet dsSavePayTrans = objHotLeadBL.SavePaymentHistoryDataForChecks(PostingID, PayTryBy, CardType, Address, City, State,
                Zip, Amount, Result, AuthNetTransID, AccountHolderName, AccountNumber, BankName, RoutingNumber, AccountType);

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
            int PSID = Convert.ToInt32(Session["AgentQCPSID1"].ToString());
            int PaymentID = Convert.ToInt32(Session["AgentQCPaymentID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int PSStatusID = Convert.ToInt32(1);
            int PmntStatus = 2;
            if (Result == "NoPayDue")
            {
                PSStatusID = 8;
            }
            else if (Result == "PartialPaid")
            {
                PSStatusID = 7;
            }
            else
            {
                PSStatusID = 1;
            }
            string TransactionID = AuthNetTransID;
            string Amount = string.Empty;
            Amount = txtPDAmountNow.Text;
            string PaymentNotes = PayInfo;
            DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatusForProcessButton(PSID, PaymentID, PSStatusID, PmntStatus, TransactionID, Amount, UID, PaymentNotes);
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnMoveSmartzNo_Click(object sender, EventArgs e)
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
    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPmntStatus.SelectedItem.Value == "3")
            {
                Response.Redirect("QCDataView.aspx");
            }
            else
            {
                Response.Redirect("QCReport.aspx");
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
            int PSID = Convert.ToInt32(Session["AgentQCPSID1"].ToString());
            int PaymentID = Convert.ToInt32(Session["AgentQCPaymentID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            string ErrorString = Session["PayCancelError"].ToString();
            String UpdatedBy = Session[Constants.NAME].ToString();
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            ErrorString = UpdatedBy + "-" + dtNow.ToString("MM/dd/yyyy hh:mm tt") + " <br>" + ErrorString + " <br>" + "-------------------------------------------------";
            int PSStatusID = Convert.ToInt32(3);
            int PmntStatus = 1;
            string Result = "Pending";
            if (rdbtnPmntReturned.Checked == true)
            {
                PSStatusID = 5;
                PmntStatus = 6;
                Result = "Returned";
            }
            else if (rdbtnPmntReject.Checked == true)
            {
                PSStatusID = 2;
                PmntStatus = 6;
                Result = "Reject";
            }
            else
            {
                PSStatusID = 3;
                PmntStatus = 1;
                Result = "Pending";
            }
            DataSet dsUpdatePaynotes = objHotLeadBL.UpdateQCPayNotesForProcessButton(PSID, UID, ErrorString, PSStatusID, PmntStatus, PaymentID);
            string AuthNetTransID = "";
            if ((Session["AgentQCPaymentTypeID"].ToString() == "1") || (Session["AgentQCPaymentTypeID"].ToString() == "2") || (Session["AgentQCPaymentTypeID"].ToString() == "3") || (Session["AgentQCPaymentTypeID"].ToString() == "4"))
            {
                SavePayTransInfo(AuthNetTransID, Result);
            }
            else
            {
                SavePayTransInfoForChecks(AuthNetTransID, Result);
            }
            if (PSStatusID == 3)
            {
                Response.Redirect("QCDataView.aspx");
            }
            else
            {
                Response.Redirect("QCReport.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;

        AuthorizePayment();
        //if (CustomValidator1.ErrorMessage.Length > 0)
        //{
        //    args.IsValid = false;
        //}
        //else
        //{
        //    //Processed so send the user to a Thank You Page
        //    ///Response.Redirect("http:ThankYouPayment.aspx");
        //}
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


    protected void btnPmntUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int pmntID = Convert.ToInt32(Session["AgentQCPaymentID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int PSStatusID = Convert.ToInt32(ddlPmntStatus.SelectedItem.Value);
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
                TransactionID = txtPaytransID.Text;
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
            DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatus(pmntID, PSStatusID, PmntStatus, TransactionID, dtPayDate, PayCancelReason, Amount, UID, PaymentNotes);
            string AuthNetTransID = "";
            string Result = ddlPmntStatus.SelectedItem.Text;
            if ((Session["AgentQCPaymentTypeID"].ToString() == "1") || (Session["AgentQCPaymentTypeID"].ToString() == "2") || (Session["AgentQCPaymentTypeID"].ToString() == "3") || (Session["AgentQCPaymentTypeID"].ToString() == "4"))
            {
                SavePayTransInfo(AuthNetTransID, Result);
            }
            else if (Session["AgentQCPaymentTypeID"].ToString() == "6")
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
            if (Session["QCViewPackageID"].ToString() == "5")
            {
                Package = "Gold Deluxe Promo Package – No cancellations allowed; All sales are final";
            }
            else if (Session["QCViewPackageID"].ToString() == "4")
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
            post_values.Add("x_first_name", txtFirstName.Text);
            post_values.Add("x_last_name", txtLastName.Text);
            post_values.Add("x_address", txtAddress.Text);
            post_values.Add("x_state", lblLocationState.Text);
            post_values.Add("x_zip", txtZip.Text);
            post_values.Add("x_city", txtCity.Text);
            post_values.Add("x_phone", txtPhone.Text);
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
                int PSID = Convert.ToInt32(Session["AgentQCPSID1"].ToString());
                int PaymentID = Convert.ToInt32(Session["AgentQCPaymentID"].ToString());
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
    protected void btnAddAnotherCarYes_Click(object sender, EventArgs e)
    {
        try
        {
            int PostingID = Convert.ToInt32(Session["AgentQCMovingPostingID"].ToString());
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingID);
            SaveDataForMultiCar(Cardetais);
            MdepAddAnotherCarAlert.Hide();
            mpealteruserUpdatedSmartz.Show();
            lblErrUpdatedSmartz.Visible = true;
            lblErrUpdatedSmartz.Text = "Customer details saved successfully with carid " + Session["CarID"].ToString();
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
    protected void PhoneMatch_Click(object sender, EventArgs e)
    {
        try
        {
            string url = "http://www.whitepages.com/phone/1-" + txtPhone.Text;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
        }
        catch { }
    }
    protected void AddressMatch_Click(object sender, EventArgs e)
    {
        try
        {
            string Add = txtbillingaddress.Text;
            Add = Add.Replace(" ", "+");
            string States = txtbillingcity.Text.Trim() + ", " + txtbillingstate.Text + " " + txtbillingzip.Text;
            States = txtbillingstate.Text.Replace(" ", "+");
            States = txtbillingstate.Text.Replace(",", "%2C");

            string url = "http://www.whitepages.com/search/FindNearby?utf8=%E2%9C%93&street=" + txtbillingaddress.Text + "&where=" + States;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
        }
        catch { }
    }

    protected void GoogleAddressMatch_Click(object sender, EventArgs e)
    {
        try
        {
            string Address = txtbillingaddress.Text;
            Address = Address.Replace(" ", "+");
            string States = txtbillingcity.Text.Trim() + ", " + txtbillingstate.Text + " " + txtbillingzip.Text;
            States = txtbillingstate.Text.Replace(" ", "+");
            States = txtbillingstate.Text.Replace(",", "%2C");
            string url = "https://www.google.co.in/#q=" + Address + States;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
        }
        catch { }
    }
    protected void lnkAgentUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            qccarid.Text = lblSaleID.Text;
            lblactualAgent.Text=lblSaleAgent.Text;

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
                DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(Convert.ToInt32(centerId));
                ddlAgentUpdate.Items.Clear();
                ddlAgentUpdate.DataSource = dsverifier;
                ddlAgentUpdate.DataTextField = "AgentUFirstName";
                ddlAgentUpdate.DataValueField = "AgentUID";
                ddlAgentUpdate.DataBind();
                ddlAgentUpdate.Items.Insert(0, new ListItem("All", "0"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            MdlPopAgentUpdate.Show();
        }
        catch { }

    }
    protected void AgentUdfate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.USP_UpdateAgent(Convert.ToInt32(ddlAgentUpdate.SelectedValue), Convert.ToInt32(qccarid.Text));
            lblSaleAgent.Text = ddlAgentUpdate.SelectedItem.ToString();
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Agent is Updated Successfully.');", true);
            MdlPopAgentUpdate.Hide();
        }
        catch { }
    }
    protected void lnkVerifierName_Click(object sender, EventArgs e)
    {
        try
        {
            lblVerSaleId.Text = lblSaleID.Text;
            lblVerName.Text = lblVerifierName.Text;

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
                ddlVerifierUpdate.Items.Clear();
                ddlVerifierUpdate.DataSource = dsverifier;
                ddlVerifierUpdate.DataTextField = "AgentUFirstName";
                ddlVerifierUpdate.DataValueField = "AgentUID";
                ddlVerifierUpdate.DataBind();
                ddlVerifierUpdate.Items.Insert(0, new ListItem("All", "0"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            MdlPopVerifier.Show();
        }
        catch { }

    }
    protected void VUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.USP_UpdateVerifier(Convert.ToInt32(ddlVerifierUpdate.SelectedValue), Convert.ToInt32(lblVerSaleId.Text));
            lblVerifierName.Text = ddlVerifierUpdate.SelectedItem.ToString();
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Verifier is Updated Successfully.');", true);
            MdlPopVerifier.Hide();
        }
        catch { }
    }
    protected void leaddetails_Click(object sender, EventArgs e)
    {
        MDLPOPLeadsPhn.Show();
    }
    protected void btnPhoneOk_Click(object sender, EventArgs e)
    {
        try
        {
            MDLPOPLeadsPhn.Hide();
            DataSet QCUpdateds = objHotLeadBL.GetResultsFromLeadsDB(txtLoadPhone.Text);
            txtLeadPhnDeta.Text = QCUpdateds.Tables[0].Rows[0]["PhoneNo"].ToString();
            lblLeaddate.Text = QCUpdateds.Tables[0].Rows[0]["CollectedDate"].ToString();

            lblLeadPrice.Text = QCUpdateds.Tables[0].Rows[0]["Price"].ToString();
            lblLeadModel.Text = QCUpdateds.Tables[0].Rows[0]["Model"].ToString();

            lblLeadState.Text = QCUpdateds.Tables[0].Rows[0]["State_Name"].ToString();
            lblLeadCity.Text = QCUpdateds.Tables[0].Rows[0]["City"].ToString();
            lblLeadEmail.Text = QCUpdateds.Tables[0].Rows[0]["CusEmailId"].ToString();
            lblDescriptin.Text = QCUpdateds.Tables[0].Rows[0]["Description"].ToString();
            lnkLeadURL.Text = QCUpdateds.Tables[0].Rows[0]["Url"].ToString();
            MdlPopLeadDetails.Show();
        }
        catch { }
    }
}
