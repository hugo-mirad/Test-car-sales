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
using Microsoft.Practices.EnterpriseLibrary.Data;
using CarsBL;
using System.Data.Common;
using System.Net;
using System.IO;



public partial class BulkProcess : System.Web.UI.Page
{


    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet CarsDetails = new DataSet();
    DataSet dsDropDown = new DataSet();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    UserRegistrationInfo objUserregInfo = new UserRegistrationInfo();
    HotLeadsBL objHotLeadBL = new HotLeadsBL();
    //Declared variables:
    int PostingID = 0; string BulkProcessResults = "";
    string CCFirstName = "", CCLastName = "", CCAddress = "", CCZip = "", CCNumber = "", CCCvv = "", CCExpiry = "",
     CCAmount = "", CCCity = "", CCState = "", CPhnNo = "", CEmail = "", VoiceRecord = "";

    int totalsales = 0, Qcopen = 0, QcPending = 0, PaymOpen = 0, PymPnding = 0, TrnsfPen = 0;
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
                        FillQCStatus();
                        FillPaymentStatus();
                        // GetResults(Status, "0", ddlPaymentStatus.SelectedItem.ToString());
                        GetResults("0", "All", "0", "0", "SaleDate");

                    }
                }
            }
        }
    }

    private void FillCenters()
    {
        try
        {
            DataSet dsCenters = objHotLeadBL.GetAllCentersData1();
            ddlCenters.Items.Clear();
            for (int i = 0; i < dsCenters.Tables[0].Rows.Count; i++)
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
    private void FillQCStatus()
    {
        try
        {
            DataSet QCStatus = objHotLeadBL.GetAllQCStatus();
            ddlQCStatus.Items.Clear();
            for (int i = 0; i < QCStatus.Tables[0].Rows.Count; i++)
            {

                ListItem list = new ListItem();
                list.Text = QCStatus.Tables[0].Rows[i]["QCStatusName"].ToString();
                list.Value = QCStatus.Tables[0].Rows[i]["QCStatusID"].ToString();
                ddlQCStatus.Items.Add(list);

            }
            ddlQCStatus.Items.Insert(0, new ListItem("QC Open", "4"));
            ddlQCStatus.Items.Insert(0, new ListItem("All", "0"));

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillPaymentStatus()
    {
        try
        {
            DataSet PaymStatus = objHotLeadBL.GetAllPaymentStatus();
            ddlPaymentsta.Items.Clear();
            for (int i = 0; i < PaymStatus.Tables[0].Rows.Count; i++)
            {

                ListItem list = new ListItem();
                list.Text = PaymStatus.Tables[0].Rows[i]["PSStatusName"].ToString();
                list.Value = PaymStatus.Tables[0].Rows[i]["PSStatusID"].ToString();
                ddlPaymentsta.Items.Add(list);

            }
            ddlPaymentsta.Items.Insert(0, new ListItem("All", "0"));
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

            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            //  GetResults(Status, CenterID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //private void GetResults(string Status,string CenterCode,string PaymentType)
    //{
    //    try
    //    {

    //        // DateTime StartingDate = Convert.ToDateTime(StartDate.AddDays(-1).ToString("MM/dd/yyyy"));
    //        DataSet SingleAgentSales = new DataSet();
    //        SingleAgentSales = objHotLeadBL.GetBulkDataAll(Status, CenterCode, PaymentType);
    //        Session["AllBulkDataData"] = SingleAgentSales;
    //        lblResHead.Text = "Recent 50 sales are showing";
    //        if (SingleAgentSales.Tables[0].Rows.Count > 0)
    //        {
    //            grdWarmLeadInfo.Visible = true;
    //           // lblResCount.Visible = true;
    //            lblRes.Visible = false;
    //            lblResCount.Text = "Total " + SingleAgentSales.Tables[0].Rows.Count.ToString() + " records found";
    //            grdWarmLeadInfo.DataSource = SingleAgentSales.Tables[0];
    //            grdWarmLeadInfo.DataBind();

    //        }
    //        else
    //        {
    //            grdWarmLeadInfo.Visible = false;
    //            lblResCount.Visible = false;
    //            lblRes.Visible = true;
    //            lblRes.Text = "No records exist";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    private void GetResults(string CenterCode, string Status, string PaymentType, string Transfer, string Orderby)
    {
        try
        {

            // DateTime StartingDate = Convert.ToDateTime(StartDate.AddDays(-1).ToString("MM/dd/yyyy"));
            DataSet SingleAgentSales = new DataSet();
            SingleAgentSales = objHotLeadBL.GetBulkDataAll(CenterCode, Status, PaymentType, Transfer, Orderby);
            Session["AllBulkDataData"] = SingleAgentSales;
            lblResHead.Text = "Recent " + SingleAgentSales.Tables[0].Rows.Count.ToString() + " sales are showing";
            if (SingleAgentSales.Tables[0].Rows.Count > 0)
            {
                grdWarmLeadInfo.Visible = true;
                // lblResCount.Visible = true;
                lblRes.Visible = false;
                lblResCount.Text = "Total " + SingleAgentSales.Tables[0].Rows.Count.ToString() + " records found";
                grdWarmLeadInfo.DataSource = SingleAgentSales.Tables[0];
                grdWarmLeadInfo.DataBind();

            }
            else
            {
                grdWarmLeadInfo.DataSource = null;
                grdWarmLeadInfo.DataBind();
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            DataSet dsTasks3 = (DataSet)Session["AllBulkDataData"];
            LinkButton CarId = (LinkButton)e.Row.FindControl("lnkCarID");

            DataView dv3 = new DataView();
            DataTable dt3 = new DataTable();

            Label lblSaleData = (Label)e.Row.FindControl("lblSaleData");
            Label lblAgent = (Label)e.Row.FindControl("lblAgent");
            Label lblVerifier = (Label)e.Row.FindControl("lblVerifier");
            LinkButton voiceFile = (LinkButton)e.Row.FindControl("lnkvoicefile");
            Label lblPackage = (Label)e.Row.FindControl("lblPackage");
            Label lblCTotaCars = (Label)e.Row.FindControl("lblCTotaCars");
            try
            {
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleDate"].ToString() != "")
                    lblSaleData.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleDate"].ToString();
                else
                    lblSaleData.Style["display"] = "none";
            }
            catch
            {
            }
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleAgent"].ToString() != "")
                lblAgent.Text = "A: " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleAgent"].ToString() + " (" + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["AgentCenterCode"].ToString() + ")";
            else
                lblAgent.Style["display"] = "none";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PackageCode"].ToString() != "")
                lblVerifier.Text = "V: " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["VerifierName"].ToString() + " (" + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["AgentCenterCode"].ToString() + ")";
            else
                lblVerifier.Style["display"] = "none";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["VoiceRecord"].ToString() != "")
                voiceFile.Text = "VF: " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["VoiceRecord"].ToString();

            string VoiceFileLoc = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["VoiceFileLocationName"].ToString();
            if (VoiceFileLoc == "Diamond voice")
            {
                VoiceFileLoc = "DV";
            }
            else
            {
                VoiceFileLoc = VoiceFileLoc;
            }
            voiceFile.Text = voiceFile.Text + " (" + VoiceFileLoc + ") ";
            if (voiceFile.Text == "")
                voiceFile.Style["display"] = "none";

            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PackageCode"].ToString() != "")
                lblPackage.Text = "Package: " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PackageCode"].ToString();
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Price"].ToString() != "")
                lblPackage.Text += " ($" + (Math.Round(Convert.ToDouble(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Price"].ToString()), 2)) + ")";
            if (lblPackage.Text == "")
                lblPackage.Style["display"] = "none";

            int TotalCars = Convert.ToInt32(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["TotalCars"].ToString());
            //if (TotalCars>=0)
            //{
            //if (TotalCars > 1)
            //    lblCTotaCars.Text = "Multiple Cars";
            //    else lblCTotaCars.Text = "Single Cars";

            //           }

            lblCTotaCars.Text = "No Of Cars: #" + TotalCars;

            //lblSaleData.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleDate"].ToString() == "" ? "" : dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleDate"].ToString();


            //For Payment Data:
            Label lblType = (Label)e.Row.FindControl("lblType");
            Label lblSecurity = (Label)e.Row.FindControl("lblSecurity");
            Label lblName = (Label)e.Row.FindControl("lblName");
            Label lblAdd1 = (Label)e.Row.FindControl("lblAdd1");
            Label lblCity = (Label)e.Row.FindControl("lblCity");
            Label lblToday = (Label)e.Row.FindControl("lblToday");
            string paymnType = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["pmntType"].ToString();
            if (paymnType.Trim().StartsWith("Check"))
            {
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["bankAccountNumber"].ToString() != "")
                    lblType.Text = "Chk: AC# " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["bankAccountNumber"].ToString();
                else
                    lblType.Style["display"] = "none";

                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["bankRouting"].ToString() != "")
                    lblSecurity.Text = "Rt# " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["bankRouting"].ToString();
                else
                    lblSecurity.Style["display"] = "none";

                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["bankAccountHolderName"].ToString() != "")
                    lblName.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["bankAccountHolderName"].ToString();
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardholderLastName"].ToString() != "")
                    lblName.Text += " " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardholderLastName"].ToString();
                else if (lblName.Text == "")
                    lblName.Style["display"] = "none";

                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingAdd"].ToString() != "")
                    lblAdd1.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingAdd"].ToString();
                else if (lblAdd1.Text == "")
                    lblAdd1.Style["display"] = "none";

                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString() != "")
                    lblCity.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString() + ", ";

                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString() != "")
                    lblCity.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString() + " ";

                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingZip"].ToString() != "")
                    lblCity.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingZip"].ToString();
                if (lblToday.Text == "")
                    lblCity.Style["display"] = "none";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Amount1"].ToString() != "")
                    lblToday.Text = "Today: $" + (Math.Round(Convert.ToDouble(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Amount1"].ToString()), 2)).ToString();
                else
                    lblToday.Style["display"] = "none";


            }
            else
            {
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["pmntType"].ToString() != "")
                    //lblType.Text =  dsTasks3.Tables[0].Rows[e.Row.RowIndex]["pmntType"].ToString();
                    lblType.Text = "Visa: ";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardNumber"].ToString() != "")
                    lblType.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardNumber"].ToString();
                else
                    lblType.Style["display"] = "none";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardCode"].ToString() != "")
                    lblSecurity.Text = "Sec Code/Exp: " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardCode"].ToString();
                // else  lblSecurity.Style["display"] = "none";

                try
                {
                    var vali = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardExpDt"].ToString();
                    var listSplit = vali.Split('/');
                    string month = listSplit[0];
                    string year = listSplit[1];
                    if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardExpDt"].ToString() != "")
                        lblSecurity.Text += " / " + month + "-" + year;
                    if (lblSecurity.Text == " / 0-0")
                        lblSecurity.Visible = true;

                }
                catch { }

                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardholderName"].ToString() != "")
                    lblName.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardholderName"].ToString() + " " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["cardholderLastName"].ToString();
                else lblName.Style["display"] = "none";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingAdd"].ToString() != "")
                    lblAdd1.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingAdd"].ToString();
                else lblAdd1.Style["display"] = "none";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString() != "")
                    lblCity.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString().Trim() + ", ";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString() != "")
                    lblCity.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString() + " ";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingZip"].ToString() != "")
                    lblCity.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingZip"].ToString();
                else
                    lblCity.Style["display"] = "none";
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Amount1"].ToString() != "")
                    lblToday.Text = "Today: $" + Math.Round(Convert.ToDouble(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Amount1"].ToString()), 2);

                //else
                //    lblToday.Style["display"] = "none";

                try
                {
                    if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PaymentDate"].ToString() != "")
                        lblToday.Text += "PD: " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PaymentDate"].ToString();
                    if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["A2"].ToString() != "")
                        lblToday.Text += " ($" + Math.Round(Convert.ToDouble(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["A2"].ToString()), 2) + ")";
                }
                catch { }

            }

            //For Customer Details
            Label lblCusPhn = (Label)e.Row.FindControl("lblCusPhn");
            Label lblCName = (Label)e.Row.FindControl("lblCName");
            Label lblcAdd = (Label)e.Row.FindControl("lblcAdd");
            Label lblCCity = (Label)e.Row.FindControl("lblCCity");
            Label LblCmail = (Label)e.Row.FindControl("LblCmail");


            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["phone"].ToString() != "")
            {
                lblCusPhn.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["phone"].ToString();
                lblCusPhn.Text = objGeneralFunc.filPhnm(lblCusPhn.Text);

            }

            else
                lblCusPhn.Style["display"] = "none";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Name"].ToString() != "")
                lblCName.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Name"].ToString() + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["LastName"].ToString();
            else lblCName.Style["display"] = "none";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Address"].ToString() != "")
                lblcAdd.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Address"].ToString();
            else lblcAdd.Style["display"] = "none";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString() != "")
                lblCCity.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString() + ", ";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString() != "")
                lblCCity.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString();

            // if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Zip"].ToString() != "")
            lblCCity.Text += " " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingZip"].ToString();

            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["UserName"].ToString() != "")
                LblCmail.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["UserName"].ToString();
            //else
            //    LblCmail.Text = "Email:None";
            //LblCmail.Style["display"] = "none";

            //For Car Details:
            Label LblTitle = (Label)e.Row.FindControl("LblTitle");
            Label LblPriMil = (Label)e.Row.FindControl("LblPriMil");

            Label lblMakemodl = (Label)e.Row.FindControl("lblMakemodl");
            LinkButton lblFeat = (LinkButton)e.Row.FindControl("lblfet");
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["make"].ToString() != "")
                LblTitle.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["make"].ToString().ToUpper() + ", " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["model"].ToString().ToUpper() + "," + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["yearOfMake"].ToString();
            else LblTitle.Style["display"] = "none";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["E1"].ToString() != "")
            {
                string valu = (Math.Round(Convert.ToDouble(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["E1"].ToString()), 2)).ToString();
                int valu1 = Convert.ToInt32(valu.ToString());
                LblPriMil.Text = "$" + valu1.ToString("#,##0");


            }
            else LblPriMil.Style["display"] = "none";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["mileage"].ToString() != "")
            {
                string valu2 = (Math.Round(Convert.ToDouble(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["mileage"].ToString()), 2)).ToString();
                int valu12 = Convert.ToInt32(valu2.ToString());
                LblPriMil.Text += "/ " + valu12.ToString("#,##0") + "mi";

            }
            // LblPriMil.Text += "/ m: " +Math.Ceiling(Convert.ToDouble(dsTasks3.Tables[0].Rows[e.Row.RowIndex]["mileage"].ToString()));


            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["yearOfMake"].ToString() != "")
                LblTitle.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["yearOfMake"].ToString();
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["make"].ToString() != "")
                LblTitle.Text += " " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["make"].ToString();
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["model"].ToString() != "")
                LblTitle.Text += " " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["model"].ToString();



            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString() != "")
                lblMakemodl.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingCity"].ToString() + ", ";
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString() != "")
                lblMakemodl.Text += dsTasks3.Tables[0].Rows[e.Row.RowIndex]["state"].ToString().Trim();
            // if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Zip"].ToString() != "")
            lblMakemodl.Text += " " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["billingZip"].ToString().Trim();
            //if (lblMakemodl.Text.StartsWith(","))
            //{
            //    lblMakemodl.Substring(10, lblMakemodl.Text.Length - 1);
            //}

            LinkButton lblvehcdesc = (LinkButton)e.Row.FindControl("lblDescr");
            string lblvehcdesc1 = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["E4"].ToString().Trim();
            if (lblvehcdesc1 != "")
            {
                lblvehcdesc.Enabled = true;
                lblvehcdesc.ForeColor = System.Drawing.Color.Orange;
                string sTable = CreateTable3(lblvehcdesc1);
                lblvehcdesc.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                lblvehcdesc.Attributes.Add("onmouseout", "return nd1(4000);");
            }
            else
            {
                lblvehcdesc.Enabled = false;
                lblvehcdesc.ForeColor = System.Drawing.Color.Black;
            }


            LinkButton lblpictres = (LinkButton)e.Row.FindControl("lblpictres");
            string lblpictres1 = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SourceOfPhotosName"].ToString();
            if (lblpictres1 != "")
            {
                lblpictres.Enabled = true;
                lblpictres.ForeColor = System.Drawing.Color.Orange;
                string sTable = CreateTable4(lblpictres1);
                lblpictres.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                lblpictres.Attributes.Add("onmouseout", "return nd1(4000);");
            }
            else
            {
                lblpictres.Enabled = false;
                lblpictres.ForeColor = System.Drawing.Color.Black;
            }


            LinkButton lblpicDesc = (LinkButton)e.Row.FindControl("lblpicDesc");
            string lblpicDesc1 = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SourceOfDescriptionName"].ToString();
            if (lblpicDesc1 != "")
            {
                lblpicDesc.Enabled = true;
                lblpicDesc.ForeColor = System.Drawing.Color.Orange;
                string sTable = CreateTable5(lblpicDesc1);
                lblpicDesc.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                lblpicDesc.Attributes.Add("onmouseout", "return nd1(4000);");
            }
            else
            {
                lblpicDesc.Enabled = false;
                lblpicDesc.ForeColor = System.Drawing.Color.Black;
            }



            //Process
            Label lblQcSatus = (Label)e.Row.FindControl("lblQcSatus");
            LinkButton QCProcessta = (LinkButton)e.Row.FindControl("QCProcessta");
            Label lb1PmtSta = (Label)e.Row.FindControl("lb1PmtSta");
            LinkButton lblTransfr = (LinkButton)e.Row.FindControl("lblTransfr");
            // label lblTransfr=(Label)e.Row.
            //LinkButton lbpmntQc = (LinkButton)e.Row.FindControl("lblQcPer");
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCStatusName"].ToString() == "")
            {
                lblQcSatus.Text = "QC Open";
                QCProcessta.Visible = true;
                QCProcessta.Enabled = true;
            }
            else
            {
                lblQcSatus.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCStatusName"].ToString();
                QCProcessta.Visible = false;
            }
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCDoneBy"].ToString() != "")
                lblQcSatus.Text += " (" + (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QDB"].ToString());
            try
            {
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCDate"].ToString() != "")
                    lblQcSatus.Text += ", " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCDate"].ToString() + ")";
                //else
                //    lblQcSatus.Style["display"] = "none";
            }
            catch { }

            String Paymntstsid = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PD1"].ToString();
            if ((Paymntstsid == "1") || (Paymntstsid == "7") || (Paymntstsid == "8"))
            {
                lb1PmtSta.ForeColor = System.Drawing.Color.Green;
            }
            else if (Paymntstsid == "2")
            {
                lb1PmtSta.ForeColor = System.Drawing.Color.Red;
            }
            else if (Paymntstsid == "3")
            {
                lb1PmtSta.ForeColor = System.Drawing.Color.Orange;
            }
            else if ((Paymntstsid == "4") || (Paymntstsid == "6"))
            {
                lb1PmtSta.ForeColor = System.Drawing.Color.Black;
            }
            else if (Paymntstsid == "5")
            {
                lb1PmtSta.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lb1PmtSta.ForeColor = System.Drawing.Color.Yellow;
            }
            if (Paymntstsid == "1")
            {
                lb1PmtSta.Enabled = true;
            }
            else
            {
                lb1PmtSta.Enabled = false;
            }

            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PS1"].ToString() != "")
                lb1PmtSta.Text = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PS1"].ToString();
            else
                lb1PmtSta.Style["display"] = "none";

            string paymentdate = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PaymentScheduledDate"].ToString();
            string paymentTransdctiid = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["transactionid"].ToString();
            if (paymentdate != "")
                lb1PmtSta.Text += paymentTransdctiid + "(" + paymentdate + ")";






            LinkButton lbpmtpprcs = (LinkButton)e.Row.FindControl("lbpmtpprcs");
            if (lb1PmtSta.Text == "Pending")
                lbpmtpprcs.Visible = true;
            else if ((lb1PmtSta.Text.StartsWith("Pending")))
                lbpmtpprcs.Visible = true;
            else
                lbpmtpprcs.Visible = false;

            if (lb1PmtSta.Text == "Reject" || lb1PmtSta.Text == "Pending" || lb1PmtSta.Text == "Return")
            {
                if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PaymentCancelReasonName"].ToString() != "")
                    lb1PmtSta.Text += " " + " Reason: " + dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PaymentCancelReasonName"].ToString();

            }

            string QCStaId = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCStatusID"].ToString();

            if (QCStaId == "")
            {
                lblQcSatus.Text = "QC Open";
                lblQcSatus.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                if (QCStaId == "1")
                {
                    lblQcSatus.ForeColor = System.Drawing.Color.Green;
                }
                if (QCStaId == "2")
                {
                    lblQcSatus.ForeColor = System.Drawing.Color.Red;
                }
                if (QCStaId == "3")
                {
                    lblQcSatus.ForeColor = System.Drawing.Color.Orange;
                }
                if (QCStaId == "4")
                {
                    lblQcSatus.ForeColor = System.Drawing.Color.Blue;
                }

            }

            //Smartz lblTransfr
            string SmartzStatus = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SmartzStatus"].ToString();
            string PaytatusID = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PD1"].ToString();
            string Smartzmoveddate = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SmartzMovedDate"].ToString();
            string SmartzAmount = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["Amount1"].ToString();
            string SmartzCarID = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SmartzCarID"].ToString();
            DateTime MovedDate = DateTime.Now;
            if (SmartzStatus == "1")
            {

                if (SmartzStatus != "")
                {
                    try
                    {
                        MovedDate = Convert.ToDateTime(Smartzmoveddate);
                        lblTransfr.Text = "Moved ( " + SmartzCarID + " " + MovedDate.ToString("MM/dd/yyyy hh:mm tt") + ")";
                    }
                    catch { }
                }
                else
                {
                    lblTransfr.Text = "Moved (" + SmartzCarID + ")";
                }
                lblTransfr.Enabled = false;
                lblTransfr.ForeColor = System.Drawing.Color.Green;
            }
            else if (((PaytatusID == "1") || (PaytatusID == "7") || (PaytatusID == "8")) && (PaytatusID == "1"))
            {

                if ((SmartzStatus == "") || (SmartzStatus == "0"))
                {
                    lblTransfr.Text = "Ready to move";
                    lblTransfr.Enabled = true;
                    lblTransfr.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    if (MovedDate != null)
                    {
                        MovedDate = Convert.ToDateTime(MovedDate);
                        lblTransfr.Text = "Moved (" + SmartzCarID + " " + MovedDate.ToString("MM/dd/yyyy hh:mm tt") + ")";
                    }
                    else
                    {
                        lblTransfr.Text = "Moved (" + SmartzCarID + ")";
                    }
                    lblTransfr.Enabled = false;
                    lblTransfr.ForeColor = System.Drawing.Color.Green;
                }
            }

            else
            {
                if ((SmartzStatus != "1") && (SmartzStatus == "1") && ((SmartzStatus == "3") || (SmartzStatus == "4")))
                {
                    if (SmartzAmount != "")
                    {
                        Double TotalAmount1 = Convert.ToDouble(SmartzAmount);
                        string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                        if (ChkAmount == "0.00")
                        {
                            lblTransfr.Text = "Ready to move";
                            lblTransfr.Enabled = true;
                            lblTransfr.ForeColor = System.Drawing.Color.Orange;
                        }
                        else
                        {
                            lblTransfr.Enabled = false;
                            lblTransfr.Text = "Not ready";
                            lblTransfr.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        lblTransfr.Enabled = false;
                        lblTransfr.Text = "Not ready";
                        lblTransfr.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    lblTransfr.Enabled = false;
                    lblTransfr.Text = "Not ready";
                    lblTransfr.ForeColor = System.Drawing.Color.Black;
                }
                //SmartzStatus
            }


            //Sale Notes
            LinkButton LblNotes = (LinkButton)e.Row.FindControl("lblnotels");
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleNotes"].ToString() == "")
            {
                LblNotes.ForeColor = System.Drawing.Color.Black;
                LblNotes.Enabled = false;

            }
            else
            {
                LblNotes.ForeColor = System.Drawing.Color.Orange;
                LblNotes.Enabled = true;
            }
            string salenote = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleNotes"].ToString();
            if (salenote != "")
            {
                string sTable = CreateTable(salenote);
                LblNotes.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                LblNotes.Attributes.Add("onmouseout", "return nd1(4000);");
            }
            //QC Notes
            LinkButton LblQcNotes = (LinkButton)e.Row.FindControl("lblQcNotes");
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCNotes"].ToString() == "")
            {
                LblQcNotes.ForeColor = System.Drawing.Color.Black;
                LblQcNotes.Enabled = false;

            }
            else
            {
                LblQcNotes.ForeColor = System.Drawing.Color.Orange;
                LblQcNotes.Enabled = true;
            }
            string QCNOtes = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCNotes"].ToString();
            if (QCNOtes != "")
            {
                string sTable = CreateTable1(QCNOtes);
                LblNotes.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                LblNotes.Attributes.Add("onmouseout", "return nd1(4000);");
            }

            //Payment Notes
            LinkButton LblPamnetNotes = (LinkButton)e.Row.FindControl("lblPaynotes");
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PaymentNotes"].ToString() == "")
            {
                LblPamnetNotes.ForeColor = System.Drawing.Color.Black;
                LblPamnetNotes.Enabled = false;

            }
            else
            {
                LblPamnetNotes.ForeColor = System.Drawing.Color.Orange;
                LblPamnetNotes.Enabled = true;
            }

            string PaymntNOtes = dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PaymentNotes"].ToString();
            if (PaymntNOtes != "")
            {
                string sTable = CreateTable2(PaymntNOtes);
                LblPamnetNotes.Attributes.Add("onmouseover", "return overlib1('" + sTable + "',STICKY, MOUSEOFF, CENTER, ABOVE,OFFSETX,30,  WIDTH, 100,  CSSCLASS,TEXTFONTCLASS,'summaryfontClass',FGCLASS,'summaryfgClass',BGCLASS,'summarybgClass',CAPTIONFONTCLASS,'summarycapfontClass', CLOSEFONTCLASS, 'summarycapfontClass');");
                LblPamnetNotes.Attributes.Add("onmouseout", "return nd1(4000);");
            }


            //For Total Counts:
            try
            {
                if ((dsTasks3.Tables[0].Rows[e.Row.RowIndex]["SaleDate"].ToString() == DateTime.Now.ToString()))
                    totalsales = totalsales + 1;
            }
            catch { }

            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCStatusName"].ToString() == "QC Open" || dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCStatusName"].ToString() == "")
                Qcopen = Qcopen + 1;
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["QCStatusName"].ToString() == "QC Pending")
                QcPending = QcPending + 1;
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PS1"].ToString() == "Open" || dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PS1"].ToString() == "")
                PaymOpen = PaymOpen + 1;
            if (dsTasks3.Tables[0].Rows[e.Row.RowIndex]["PS1"].ToString() == "Pending")
                PymPnding = PymPnding + 1;


        }
        Lbl_dialstat.Text = "" + totalsales + ";   QC:" + Qcopen + " Open," + QcPending + " Pending;  Payment:" + PaymOpen + " Open, " + PymPnding + " Pending;";

        //lblsearchdetails.Text = "Search Criteria:" + "QC Status:" + ddlQCStatus.SelectedItem.Text + " ,Payment Status:" +
        //ddlPaymentsta.SelectedItem.Text + " ,Process Status:" + DropDownList1.SelectedItem.Text + ",Transfer Status:" +
        //ddl_transtst.SelectedItem.Text + ",Center:" + ddlCenters.SelectedItem.Text + "";
        lblRefreshTime.Text = DateTime.Now.ToString();
    }
    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string FinalResult = "";
            string carids = hdncheck.Value;
            string fruit1 = carids;
            string[] split2 = fruit1.Split(',');
            int count = split2.Count();


            string txtbx = Session["EnterNotes"].ToString();
            string val = txtbx.Substring(txtbx.IndexOf(",") + 1);
            Session["EnterNotes"] = val;

            string[] splt = txtbx.Split(',');
            string txtbx1 = txtbx.Split(',')[0].Trim();

            Label salesid = (Label)e.Row.FindControl("lblsalesid");
            string carId = salesid.Text;
            TextBox EnterNotes = (TextBox)e.Row.FindControl("txtEnterNotes1");
            EnterNotes.Text = txtbx1.ToString();
            //  string Paymstst = Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString();
            HiddenField Paymst = (HiddenField)e.Row.FindControl("lblpaymStat");
            //lblpaynotes

            if (Paymst.Value == "1")
            {

                FinalResult = "Fail";

            }
            else
            {
                if (Paymst.Value != "1")
                {
                    DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(carId));
                    if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "1")
                        FinalResult = "QC Alerdy Approved";
                    else
                    {
                       // DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(QCID, QCNotes, Convert.ToInt32(selecteditem), PostingId, QCBY, PostingId);
                        FinalResult = "Success";
                    }
                }
            }
            Label QCResul = (Label)e.Row.FindControl("lblQcresu");
            QCResul.Text = FinalResult;


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
            if (e.CommandName == "CarFea")
            {
                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet dsUsers = new DataSet();
                string spNameString = string.Empty;
                Database dbDatabase = DatabaseFactory.CreateDatabase(Global.INSTANCE_NAME4);
                spNameString = "USP_CarFeatures";
                DbCommand dbCommand = null;
                dbCommand = dbDatabase.GetStoredProcCommand(spNameString);
                dbDatabase.AddInParameter(dbCommand, "@CarId", DbType.String, Carvalue);
                dsUsers = dbDatabase.ExecuteDataSet(dbCommand);
                DataSet dsTasks3 = (DataSet)dsUsers;
                CarID.Text = Carvalue.ToString();
                if (dsTasks3.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < dsTasks3.Tables[0].Rows.Count; k++)
                    {
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "1")
                        {
                            if (lblcomfrt.Text == "")
                            {
                                lblcomfrt.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblcomfrt.Text = lblcomfrt.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "2")
                        {
                            if (lblseats.Text == "")
                            {
                                lblseats.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblseats.Text = lblseats.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "3")
                        {
                            if (lblsafety.Text == "")
                            {
                                lblsafety.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblsafety.Text = lblsafety.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "4")
                        {
                            if (lblSound.Text == "")
                            {
                                lblSound.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblSound.Text = lblSound.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "5")
                        {
                            if (lblwindows.Text == "")
                            {
                                lblwindows.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblwindows.Text = lblwindows.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "6")
                        {
                            if (lblothers.Text == "")
                            {
                                lblothers.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblothers.Text = lblothers.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "7")
                        {
                            if (lblnew.Text == "")
                            {
                                lblnew.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblnew.Text = lblnew.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                        if (dsTasks3.Tables[0].Rows[k]["FeatureTypeID"].ToString() == "8")
                        {
                            if (lblspecls.Text == "")
                            {
                                lblspecls.Text = dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                            else
                            {
                                lblspecls.Text = lblspecls.Text + ", " + dsTasks3.Tables[0].Rows[k]["FeatureName"].ToString();
                            }
                        }
                    }
                }

                mdlCarFeature.Show();

            }


            else if (e.CommandName == "SaleNote")
            {

                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(Carvalue));
                if (lblsalesnot.Text == "") lblsalesnot.Text = Cardetais.Tables[0].Rows[0]["SaleNotes"].ToString();

                ModelSalesno.Show();
            }
            else if (e.CommandName == "QCNote")
            {


                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(Carvalue));
                if (lblQcNotesty.Text == "") lblQcNotesty.Text = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();

                ModelQCno.Show();
            }

            else if (e.CommandName == "PaymentNote")
            {

                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(Carvalue));
                if (lblpaynotes1.Text == "") lblpaynotes1.Text = Cardetais.Tables[0].Rows[0]["PaymentNotes"].ToString();
                ModelPaymtno.Show();
            }

            else if (e.CommandName == "PaymentHistory")
            {
                int Carvalue = Convert.ToInt32(e.CommandArgument);
                lblPayTransSaleID.Text = Carvalue.ToString();
                DataSet PayHistory = objHotLeadBL.GetPaymentTransactionData(Carvalue);
                if (PayHistory.Tables.Count > 0)
                {
                    if (PayHistory.Tables[0].Rows.Count > 0)
                    {
                        grdIntroInfo.Visible = true;
                        grdIntroInfo.DataSource = PayHistory.Tables[0];
                        grdIntroInfo.DataBind();
                        //  lblPayTransSaleID.Text = Session["AgentQCCarID"].ToString();
                        mpeaSalesData.Show();
                    }
                    else
                    {
                        //lblNotransError.Text = "Transaction history not available";
                        //lblNotransError.Visible = true;
                        mpeaSalesData.Show();
                    }
                }
                else
                {
                    //lblNotransError.Text = "Transaction history not available";
                    //lblNotransError.Visible = true;
                    mpeaSalesData.Show();
                }
            }
            else if (e.CommandName == "CarDesctiption")
            {

                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(Carvalue));
                if (txtcardesc.Text == "") txtcardesc.Text = Cardetais.Tables[0].Rows[0]["Cardescription"].ToString();


                MPCardescr.Show();
            }
            else if (e.CommandName == "CarAddFeat")
            {
                int Carvalue = Convert.ToInt32(e.CommandArgument);
                IDataReader IAcDetDatarReader = null;
                string spNameString = string.Empty;
                Database dbDatabase = DatabaseFactory.CreateDatabase(Global.INSTANCE_NAME4);
                spNameString = "[USP_GetCarDetailsByPostingID]";
                DbCommand dbCommand = null;

                try
                {
                    //Set stored procedure to the command object
                    dbCommand = dbDatabase.GetStoredProcCommand(spNameString);
                    dbDatabase.AddInParameter(dbCommand, "@postingID", DbType.String, Carvalue);
                    //Executing stored procedure
                    IAcDetDatarReader = dbDatabase.ExecuteReader(dbCommand);
                    while (IAcDetDatarReader.Read())
                    {



                        ddlMake.Text = IAcDetDatarReader["make"].ToString();
                        ddlModel.Text = IAcDetDatarReader["makeModelID"].ToString();
                        ddlYear1.Text = IAcDetDatarReader["yearOfMake"].ToString();

                        //ListItem listBody = new ListItem();
                        //listBody.Value = IAcDetDatarReader["bodyTypeID"].ToString();
                        //listBody.Text = IAcDetDatarReader["bodyType"].ToString();
                        //ddlBodyStyle.SelectedIndex = ddlBodyStyle.Items.IndexOf(listBody);
                        txtBodyStyle.Text = IAcDetDatarReader["bodyType"].ToString();
                        if (IAcDetDatarReader["Carprice"].ToString() == "0.0000")
                        {
                            txtAskingPrice.Text = "";
                        }
                        else
                        {
                            txtAskingPrice.Text = string.Format("{0:0}", Convert.ToDouble(IAcDetDatarReader["Carprice"].ToString()));
                        }
                        if (txtAskingPrice.Text.Length > 6)
                        {
                            txtAskingPrice.Text = txtAskingPrice.Text.Substring(0, 6);
                        }

                        if (IAcDetDatarReader["mileage"].ToString() == "0.00")
                        {
                            txtMileage.Text = "";
                        }
                        else
                        {
                            txtMileage.Text = string.Format("{0:0}", Convert.ToDouble(IAcDetDatarReader["mileage"].ToString()));
                        }
                        if (txtMileage.Text.Length > 6)
                        {
                            txtMileage.Text = txtMileage.Text.Substring(0, 6);
                        }

                        string NumberOfCylinder = IAcDetDatarReader["numberOfCylinder"].ToString();
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
                        //list7.Value = IAcDetDatarReader["exteriorColor"].ToString();
                        //list7.Text = IAcDetDatarReader["exteriorColor"].ToString();
                        //ddlExteriorColor.SelectedIndex = ddlExteriorColor.Items.IndexOf(list7);
                        txtExteriorColor.Text = IAcDetDatarReader["exteriorColor"].ToString();

                        //ListItem list8 = new ListItem();
                        //list8.Text = IAcDetDatarReader["interiorColor"].ToString();
                        //list8.Value = IAcDetDatarReader["interiorColor"].ToString();
                        //ddlInteriorColor.SelectedIndex = ddlInteriorColor.Items.IndexOf(list8);
                        txtInteriorColor.Text = IAcDetDatarReader["interiorColor"].ToString();

                        string Transmission = IAcDetDatarReader["Transmission"].ToString();
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
                        string NumberOfDoors = IAcDetDatarReader["numberOfDoors"].ToString();
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

                        string DriveTrain = IAcDetDatarReader["DriveTrain"].ToString();
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
                        txtVin.Text = IAcDetDatarReader["VIN"].ToString();

                        int FuelTypeID = Convert.ToInt32(IAcDetDatarReader["fuelTypeID"].ToString());
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
                        int ConditionID = Convert.ToInt32(IAcDetDatarReader["vehicleConditionID"].ToString());
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

                    }
                }

                catch (Exception ex)
                {

                }
                finally
                {
                    dbDatabase = null;
                }
                mdlPopAdl.Show();

            }

            else if (e.CommandName == "PicData")
            {
                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(Carvalue));
                if (txtpicdata.Text == "") txtpicdata.Text = Cardetais.Tables[0].Rows[0]["SourceOfPhotosName"].ToString();
                mdlPicdata.Show();


            }


            else if (e.CommandName == "PicDescd")
            {

                int Carvalue = Convert.ToInt32(e.CommandArgument);
                IDataReader IAcDetDatarReader = null;
                string spNameString = string.Empty;
                Database dbDatabase = DatabaseFactory.CreateDatabase(Global.INSTANCE_NAME4);
                spNameString = "[USP_GetCarDetailsByPostingID]";
                DbCommand dbCommand = null;

                try
                {
                    //Set stored procedure to the command object
                    dbCommand = dbDatabase.GetStoredProcCommand(spNameString);
                    dbDatabase.AddInParameter(dbCommand, "@postingID", DbType.String, Carvalue);
                    //Executing stored procedure
                    IAcDetDatarReader = dbDatabase.ExecuteReader(dbCommand);
                    if (IAcDetDatarReader.Read())
                    {
                        txtpicdesc.Text = IAcDetDatarReader["SourceOfDescriptionName"].ToString();

                    }
                }

                catch (Exception ex)
                {

                }
                finally
                {
                    dbDatabase = null;
                }
                Mdlpicdesc.Show();
            }

            else if (e.CommandName == "QCProcesstas")
            {

                string QcStatusIds = hdncheck.Value;
                QcStatusIds = QcStatusIds.Substring(0, QcStatusIds.Length - 1);
                string s = QcStatusIds;
                char[] c = s.ToCharArray();
                int count = 0;
                foreach (char ch in c)
                {
                    if (ch == ',')
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    MdlQcProcessta.Show();
                }
                else
                {
                    modelbulkQCProc.Show();
                    // txtmulticars.Text = "";// QcStatusIds;

                }


                int QCStatus1 = 0;
                qccarid.Text = (Convert.ToInt32(e.CommandArgument.ToString())).ToString();
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(qccarid.Text));
                try
                {
                    QCStatus1 = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString());
                }
                catch { QCStatus1 = 0; }
                if (QCStatus1 == 0)
                    DropDownList2.SelectedIndex = 1;
                else if (QCStatus1 == 1)
                    DropDownList2.Text = "QC Approved";
                else if (QCStatus1 == 2)
                    DropDownList2.Text = "QC Reject";
                else if (QCStatus1 == 3)
                    DropDownList2.Text = "QC Pending";
                else if (QCStatus1 == 4)
                    DropDownList2.Text = "QC Returned";


            }
            else if (e.CommandName == "EditSale")
            {
                int PostingID = Convert.ToInt32(e.CommandArgument.ToString());
                Session["AgentQCPostingID"] = PostingID;
                Response.Redirect("QCDataView.aspx");
            }
            else if (e.CommandName == "MoveSmartz")
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

            //PaymentProcess:
            else if (e.CommandName == "PaymentProcess")
            {
                int PostingID = Convert.ToInt32(e.CommandArgument.ToString());
                Session["PostingId"] = PostingID.ToString();
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
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

                            }
                            else
                            {
                                PaymentVisaProcess(PostingID);

                            }
                        }
                        else
                        {
                            PaymentVisaProcess(PostingID);
                        }
                    }

                    else
                    {

                        if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                        {
                            if (Cardetais.Tables[0].Rows[0]["Amount1"].ToString() != "")
                            {
                                Double TotalAmount1 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Amount1"].ToString());
                                string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                                if (ChkAmount == "0.00")
                                {

                                }
                                else
                                {
                                    PaymentCheckProcess(PostingID);
                                }
                            }
                            else
                            {
                                PaymentCheckProcess(PostingID);
                            }
                        }
                        else
                        {

                        }
                    }

                }


            }

            else if (e.CommandName == "EditPayInfo")
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
                    lblPDAmountForPop.Text = Cardetais.Tables[0].Rows[0]["A2"].ToString();
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
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 8))
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
                        if (Cardetais.Tables[0].Rows[0]["PD1"].ToString() == "2")
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
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PS1"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["PD1"].ToString();
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
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PS1"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["PD1"].ToString();
                    ddlPaymentStatus.SelectedIndex = ddlPaymentStatus.Items.IndexOf(liPayStatus);
                    if (Cardetais.Tables[0].Rows[0]["PD1"].ToString() == "2")
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
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 8))
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
                    if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 1) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 7) || (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString()) == 8))
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
                        if (Cardetais.Tables[0].Rows[0]["PD1"].ToString() == "2")
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
                    liPayStatus.Text = Cardetais.Tables[0].Rows[0]["PS1"].ToString();
                    liPayStatus.Value = Cardetais.Tables[0].Rows[0]["PD1"].ToString();
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

            if (e.CommandName == "AddressMatch")
            {
                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Carvalue);

                string Address = Cardetais.Tables[0].Rows[0]["address1"].ToString();
                string Staes = Cardetais.Tables[0].Rows[0]["city"].ToString() + ", " + Cardetais.Tables[0].Rows[0]["state"].ToString() + " " + Cardetais.Tables[0].Rows[0]["zip"].ToString();
                Address = Address.Replace(" ", "+");
                Staes = Staes.Replace(" ", "+");
                Staes = Staes.Replace(",", "%2C");
                string url = "http://www.whitepages.com/search/FindNearby?utf8=%E2%9C%93&street=" + Address + "&where=" + Staes;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
                // Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(url)));

            }
            if (e.CommandName == "PhoneMatch")
            {
                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Carvalue);

                string Phone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
                string url = "http://www.whitepages.com/phone/1-" + Phone;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
                //  Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(pageurl)));

            }
            if (e.CommandName == "GoogleAddressMatch")
            {
                int Carvalue = Convert.ToInt32(e.CommandArgument);
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Carvalue);

                string Address = Cardetais.Tables[0].Rows[0]["address1"].ToString();
                string Staes = Cardetais.Tables[0].Rows[0]["city"].ToString() + ", " + Cardetais.Tables[0].Rows[0]["state"].ToString() + " " + Cardetais.Tables[0].Rows[0]["zip"].ToString();
                Address = Address.Replace(" ", "+");
                Staes = Staes.Replace(" ", "+");
                Staes = Staes.Replace(",", "%2C");
                string Phone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
                string url = "https://www.google.co.in/#q=" + Address + Staes;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
                //  Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(pageurl)));

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PaymentCheckProcess(int postingid)
    {
        try
        {
            GoWithCheck(postingid);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PaymentVisaProcess(int PostingID)
    {
        try
        {
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
            // AuthorizePayment();
            CCFirstName = Cardetais.Tables[0].Rows[0]["cardholderName"].ToString().Trim();
            CCLastName = Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString().Trim();
            CCAddress = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString().Trim();
            CCZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString().Trim();
            CCNumber = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString().Trim();
            CCCvv = Cardetais.Tables[0].Rows[0]["cardCode"].ToString().Trim();
            string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString().Trim();
            string[] EXpDt = EXpDate.Split(new char[] { '/' });
            string txtExpMon = EXpDt[0].ToString();
            string txtCCExpiresYear = "20" + EXpDt[1].ToString().Trim();

            CCExpiry = txtExpMon + "/" + txtCCExpiresYear;
            CCAmount = Cardetais.Tables[0].Rows[0]["Amount1"].ToString().Trim();
            CCCity = Cardetais.Tables[0].Rows[0]["billingCity"].ToString().Trim();
            CCState = Cardetais.Tables[0].Rows[0]["State_Code"].ToString().Trim();
            CPhnNo = Cardetais.Tables[0].Rows[0]["phone"].ToString().Trim();
            CEmail = Cardetais.Tables[0].Rows[0]["UserName"].ToString().Trim();
            VoiceRecord = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString().Trim();
            Session["QCViewPackageID"] = Cardetais.Tables[0].Rows[0]["PackageID"].ToString().Trim();
            Session["AgentQCPaymentTypeID"] = Cardetais.Tables[0].Rows[0]["pmntType"].ToString().Trim();


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
                    AuthorizePayment(PostingID, CCFirstName, CCLastName, CCAddress, CCZip, CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, CPhnNo, CEmail, VoiceRecord);
                }

            }
            else
            {
                AuthorizePayment(PostingID, CCFirstName, CCLastName, CCAddress, CCZip, CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, CPhnNo, CEmail, VoiceRecord);
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

            int PSStatus=0;
            try
            {
                PSStatus = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString());
            }
            catch { }
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
            ds = Session["AllSalesQCData"] as DataSet;
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
            ds = Session["AllSalesQCData"] as DataSet;
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
            ds = Session["AllSalesQCData"] as DataSet;
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
            ds = Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "Price";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";

            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";

            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";

            }
            else
            {
                Session["SortDirec"] = "Ascending";

            }
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";



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
            ds = Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "sellerName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";

            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";

            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";

            }
            else
            {
                Session["SortDirec"] = "Ascending";

            }
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";




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
            ds = Session["AllSalesQCData"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "PhoneNum";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";

            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";

            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";

            }
            else
            {
                Session["SortDirec"] = "Ascending";

            }
            lnkCarIDSort.Text = "Sale ID &darr; &uarr;";
            lnkSaleDateSort.Text = "Sale Dt &darr; &uarr;";

            lnkAgentSort.Text = "Agent &darr; &uarr;";
            lnkYearSort.Text = "Year/Make/Model &darr; &uarr;";


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
            ds = Session["AllSalesQCData"] as DataSet;
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
                ds = Session["AllSalesQCData"] as DataSet;
                ds.Tables[0].DefaultView.RowFilter = "";
                dt = ds.Tables[0];
            }
            catch { }


            string SortExp = "PS1";
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
            // GetResults();
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
            // GetResults();
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
            // GetResults();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    int pmntID = Convert.ToInt32(Session["QCPayUpPmntID"].ToString());
        //    int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
        //    int PSStatusID = Convert.ToInt32(ddlPaymentStatus.SelectedItem.Value);
        //    int PmntStatus = 0;
        //    if (PSStatusID == 1)
        //    {
        //        PmntStatus = 2;
        //    }
        //    else if (PSStatusID == 2)
        //    {
        //        PmntStatus = 6;
        //    }
        //    else
        //    {
        //        PmntStatus = 1;
        //    }
        //    string TransactionID = txtPaytransID.Text;
        //    DateTime dtPayDate;
        //    if (PSStatusID == 1)
        //    {
        //        if (hdnPophdnAmount.Value != "0")
        //        {
        //            dtPayDate = Convert.ToDateTime(ddlPaymentDate.SelectedItem.Text);
        //        }
        //        else
        //        {
        //            dtPayDate = Convert.ToDateTime(Session["QCPayUpPmntDate"].ToString());
        //        }
        //    }
        //    else
        //    {
        //        dtPayDate = Convert.ToDateTime("1/1/1990");
        //    }
        //    string Amount = string.Empty;
        //    if (hdnPophdnAmount.Value != "0")
        //    {
        //        Amount = txtPaymentAmountInPop.Text;
        //    }
        //    else
        //    {
        //        Amount = "0";
        //    }
        //    int PayCancelReason = Convert.ToInt32(ddlPayCancelReason.SelectedItem.Value);
        //    string PaymentNotes = string.Empty;
        //    DataSet dsDatetime = objHotLeadBL.GetDatetime();
        //    DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
        //    String UpdatedBy = Session[Constants.NAME].ToString();
        //    if (txtPaymentNewNotes.Text.Trim() != "")
        //    {
        //        string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
        //        if (txtPaymentNotes.Text.Trim() != "")
        //        {
        //            PaymentNotes = txtPaymentNotes.Text.Trim() + "\n" + UpdateByWithDate + txtPaymentNewNotes.Text.Trim() + "\n" + "-------------------------------------------------";
        //        }
        //        else
        //        {
        //            PaymentNotes = UpdateByWithDate + txtPaymentNewNotes.Text.Trim() + "\n" + "-------------------------------------------------";
        //        }
        //    }
        //    else
        //    {
        //        PaymentNotes = txtPaymentNotes.Text.Trim();
        //    }
        //    DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatus(pmntID, PSStatusID, PmntStatus, TransactionID, dtPayDate, PayCancelReason, Amount, UID, PaymentNotes);
        //    MPEUpdate.Hide();

        //    int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
        //    // GetResults(Status, CenterID);
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        try
        {

            int PostingId = Convert.ToInt32(Session["PostingId"].ToString());
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingId);
            string pmntty = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
            Session["QCPayUpPmntTypeID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString());
            Session["QCPayUpPmntID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PaymentID"].ToString());
            lblpaymentPopSaleID.Text = Cardetais.Tables[0].Rows[0]["CarID"].ToString();




            int pmntID = Convert.ToInt32(Session["QCPayUpPmntID"].ToString());
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int PSStatusID = 1;
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
            string TransactionID = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
            DateTime dtPayDate;
            if (PSStatusID == 1)
            {
                if (hdnPophdnAmount.Value != "0")
                {

                    DataSet dsDatetime = objHotLeadBL.GetDatetime();
                    DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
                    Session["dtnow"] = dtNow.ToString();
                    dtPayDate = Convert.ToDateTime(Session["dtnow"].ToString());
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
                txtPaymentAmountInPop.Text = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
                Amount = txtPaymentAmountInPop.Text;
            }
            else
            {
                Amount = "0";

                int PayCancelReason = Convert.ToInt32(1);
                string PaymentNotes = string.Empty;
                string OldNotes = Cardetais.Tables[0].Rows[0]["PaymentNotes"].ToString();
                OldNotes = OldNotes.Replace("<br>", Environment.NewLine);
                txtPaymentNotes.Text = OldNotes;
                DataSet dsDatetime1 = objHotLeadBL.GetDatetime();
                DateTime dtNow1 = Convert.ToDateTime(dsDatetime1.Tables[0].Rows[0]["Datetime"].ToString());
                String UpdatedBy = Session[Constants.NAME].ToString();
                if (txtPaymentNewNotes.Text.Trim() != "")
                {
                    string UpdateByWithDate = dtNow1.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
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

                int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
                // GetResults(Status, CenterID);
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

            int PSStatus = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PD1"].ToString());
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
    private string CreateTable(string SalesNotes)
    {
        SalesNotes = SalesNotes.Replace("\n", "<br />");
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"330px\" id=\"SalesStatus\" style=\"display: block; box-shadow: 0 0 8px rgba(0,0,0,0.4);background-color: #fff; border: #999 1px solid; padding: 2px; height: 190px\">";
        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"text-align:center;background-color:#ccc;width: 330px;\"> Sales Notes";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += "<tr >";
        strTransaction += "<td  style=\"padding-left:10px;\" align=\"left\"> <div style=\"overflow: scroll; width: 310px; height: 150px;\">";
        strTransaction += SalesNotes;
        strTransaction += "</div></td>";
        strTransaction += " </tr>";
        strTransaction += "</table>";

        return strTransaction;

    }
    private string CreateTable1(string QcNotes)
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

    private string CreateTable2(string PaymNotes)
    {
        PaymNotes = PaymNotes.Replace("\n", "<br />");
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"330px\" id=\"SalesStatus\" style=\"display: block; box-shadow: 0 0 8px rgba(0,0,0,0.4);background-color: #fff; border: #999 1px solid; padding: 2px; height: 190px\">";
        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"text-align:center;background-color:#ccc;width: 330px;\"> Payment Notes";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += "<tr >";
        strTransaction += "<td  style=\"padding-left:10px;\" align=\"left\"> <div style=\"overflow: scroll; width: 310px; height: 150px;\">";
        strTransaction += PaymNotes;
        strTransaction += "</div></td>";
        strTransaction += " </tr>";
        strTransaction += "</table>";

        return strTransaction;

    }

    private string CreateTable3(string VehicleDesc)
    {
        VehicleDesc = VehicleDesc.Replace("\n", "<br />");
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"330px\" id=\"SalesStatus\" style=\"display: block; box-shadow: 0 0 8px rgba(0,0,0,0.4);background-color: #fff; border: #999 1px solid; padding: 2px; height: 190px\">";
        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"text-align:center;background-color:#ccc;width: 330px;\"> Vehicle Description";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += "<tr >";
        strTransaction += "<td  style=\"padding-left:10px;\" align=\"left\"> <div style=\"overflow: scroll; width: 310px; height: 150px;\">";
        strTransaction += VehicleDesc;
        strTransaction += "</div></td>";
        strTransaction += " </tr>";
        strTransaction += "</table>";
        return strTransaction;
    }

    private string CreateTable4(string Picdata)
    {
        Picdata = Picdata.Replace("\n", "<br />");
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"330px\" id=\"SalesStatus\" style=\"display: block; box-shadow: 0 0 8px rgba(0,0,0,0.4);background-color: #fff; border: #999 1px solid; padding: 2px; height: 190px\">";
        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"text-align:center;background-color:#ccc;width: 330px;\"> Picture Data";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += "<tr >";
        strTransaction += "<td  style=\"padding-left:10px;\" align=\"left\"> <div style=\"overflow: scroll; width: 310px; height: 150px;\">";
        strTransaction += Picdata;
        strTransaction += "</div></td>";
        strTransaction += " </tr>";
        strTransaction += "</table>";
        return strTransaction;

    }

    private string CreateTable5(string PicDescdata)
    {
        PicDescdata = PicDescdata.Replace("\n", "<br />");
        string strTransaction = string.Empty;
        strTransaction = "<table width=\"330px\" id=\"SalesStatus\" style=\"display: block; box-shadow: 0 0 8px rgba(0,0,0,0.4);background-color: #fff; border: #999 1px solid; padding: 2px; height: 190px\">";
        strTransaction += "<tr id=\"CampaignsBody3\">";
        strTransaction += "<td  style=\"text-align:center;background-color:#ccc;width: 330px;\"> Picture Description Data";
        strTransaction += "</td>";
        strTransaction += " </tr>";
        strTransaction += "<tr >";
        strTransaction += "<td  style=\"padding-left:10px;\" align=\"left\"> <div style=\"overflow: scroll; width: 310px; height: 150px;\">";
        strTransaction += PicDescdata;
        strTransaction += "</div></td>";
        strTransaction += " </tr>";
        strTransaction += "</table>";
        return strTransaction;

    }




    //PaymentSucces.aspx
    private bool AuthorizePayment(int PostingID, string CCFirstName, string CCLastName, string CCAddress, string CCZip,
        string CCNumber, string CCCvv, string CCExpiry, string CCAmount, string CCCity, string CCState, string CPhnNo, string CEmail, string VoiceRecord)
    {
        DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
        Double PackCost2 = new Double();
        PackCost2 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
        string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
        string PackName2 = Cardetais.Tables[0].Rows[0]["Description"].ToString();
        String lblPackage = PackName2 + " ($" + PackAmount2 + ")";

        string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
        string[] EXpDt = EXpDate.Split(new char[] { '/' });
        string txtExpMon = EXpDt[0].ToString();
        string txtCCExpiresYear = "20" + EXpDt[1].ToString();

        CCExpiry = txtExpMon + "/" + txtCCExpiresYear;

        //CustomValidator1.ErrorMessage = "";

                       //-- Main Details (Remove Comment when Checking)Padma ---//

        string AuthNetVersion = "3.1"; // Contains CCV support
        string AuthNetLoginID = "9FtTpx88g879"; //Set your AuthNetLoginID here
        string AuthNetTransKey = "9Gp3Au9t97Wvb784";  // Get this from your authorize.net merchant interface

        WebClient webClientRequest = new WebClient();
        System.Collections.Specialized.NameValueCollection InputObject = new System.Collections.Specialized.NameValueCollection(30);
        System.Collections.Specialized.NameValueCollection ReturnObject = new System.Collections.Specialized.NameValueCollection(30);

        byte[] ReturnBytes;
        string[] ReturnValues;
        string ErrorString = "";
                    //(TESTMODE) Bill To Company is required. (33) (Remove Comments while Testing Padma)

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
        InputObject.Add("x_first_name", CCFirstName);
        InputObject.Add("x_last_name", CCLastName);
        InputObject.Add("x_phone", CPhnNo);
        InputObject.Add("x_address", CCAddress);
        InputObject.Add("x_city", CCCity);
        InputObject.Add("x_state", CCState);
        InputObject.Add("x_zip", CCZip);

        if (CEmail!= "")
        {
            InputObject.Add("x_email", CEmail);
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
             Package = lblPackage;
        }
        //var string = $('#ddlPackage option:selected').text();
        //var p =string.split('$');
        //var pp = p[1].split(')');
        ////alert(pp[0]);
        ////pp[0] = parseInt(pp[0]);
        //pp[0] = parseFloat(pp[0]);
        //var selectedPack = pp[0].toFixed(2);
        string PackCost = lblPackage;
        string[] Pack = PackCost.Split('$');
        string[] FinalAmountSpl = Pack[1].Split(')');
        string FinalAmount = FinalAmountSpl[0].ToString();
        if (Convert.ToDouble(FinalAmount) !=Convert.ToDouble(CCAmount))
        {
            Package = Package + "- Partial payment -";
        }

        InputObject.Add("x_description", "Payment to " + Package);
        InputObject.Add("x_invoice_num", VoiceRecord);
        //string.Format("{0:00}", Convert.ToDecimal(lblAdPrice2.Text))) + "Dollars
        //Description of Purchase

        //lblPackDescrip.Text 
        //Card Details
        InputObject.Add("x_card_num", CCNumber);
        InputObject.Add("x_exp_date", txtExpMon + "/" + txtCCExpiresYear);
        InputObject.Add("x_card_code", CCCvv);

        InputObject.Add("x_method", "CC");
        InputObject.Add("x_type", "AUTH_CAPTURE");

        InputObject.Add("x_amount", string.Format("{0:c2}", Convert.ToDouble(CCAmount)));

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
                string PayNotes = UpdatedBy + "-" + dtNow.ToString("MM/dd/yyyy hh:mm tt") + " <br>Payment Successfully processed for $" + CCAmount + "  <br>Authorisation Code " + ReturnValues[4].Trim(char.Parse("|")) + " <br> TransID=" + ReturnValues[6].Trim(char.Parse("|")) + "<br> " + "-------------------------------------------------"; // Returned Authorisation Code;                
                string Result = "Paid";
                string PackCost1 =  lblPackage;
                string[] Pack1 = PackCost1.Split('$');
                string[] FinalAmountSpl1 = Pack1[1].Split(')');
                string FinalAmount1 = FinalAmountSpl1[0].ToString();
                if (Convert.ToDouble(CCAmount).ToString() == "0")
                {
                    Result = "NoPayDue";
                }
                else if (Convert.ToDouble(FinalAmount1) != Convert.ToDouble(CCAmount))
                {
                    Result = "PartialPaid";
                }
                else
                {
                    Result = "Paid";
                }
                SavePayInfo(AuthNetTransID, PayNotes, Result);
                SavePayTransInfo(AuthNetTransID, Result);
                string lblMoveSmartz = PayInfo;
                //Commented
               //Padma  lblMoveSmartz.Visible = true;
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
            DataSet dsUpdatePaynotes = objHotLeadBL.UpdateQCPayNotesForProcessButton(PSID, UID, ErrorString, PSStatusID, PmntStatus, PaymentID);
            string AuthNetTransID = "";
            string Result = "Pending";
             SavePayTransInfo(AuthNetTransID, Result);
            ErrorString = "Payment is DECLINED <br /> " + ErrorString;
            string lblErr = ErrorString;
            mpealteruser.Show();

                // ErrorString contains the actual error
                //CustomValidator1.ErrorMessage = ErrorString;
            return false;
            }
        }
        catch (Exception ex)
        {
           // CustomValidator1.ErrorMessage = ex.Message;
            return false;
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
            Amount = CCAmount;
            string PaymentNotes = PayInfo;
            DataSet dsUpPayData = objHotLeadBL.UpdateQCPayStatusForProcessButton(PSID, PaymentID, PSStatusID, PmntStatus, TransactionID, Amount, UID, PaymentNotes);
        }
        catch (Exception ex)
        {
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

            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
            // AuthorizePayment();
            string CCardNumber = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
            string Address = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString();
            string City = Cardetais.Tables[0].Rows[0]["billingCity"].ToString();
            string State = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
            string Zip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
            string Amount = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
            string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
            string[] EXpDt = EXpDate.Split(new char[] { '/' });
            string txtExpMon = EXpDt[0].ToString();
            string txtCCExpiresYear = "20" + EXpDt[1].ToString();
            string CCExpiryDate = txtExpMon + "/" + txtCCExpiresYear;
            string CardCvv = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();
            string CCFirstName = Cardetais.Tables[0].Rows[0]["cardholderName"].ToString();
            string CCLastName = Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString();
            DataSet dsSavePayTrans = objHotLeadBL.SavePaymentHistoryData(PostingID, PayTryBy, CardType, CCardNumber, Address, City, State,
             Zip, Amount, Result, AuthNetTransID, CCExpiryDate, CardCvv, CCFirstName, CCLastName);

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
    protected void btnRejectThereYes_Click(object sender, EventArgs e)
    {
        try
        {

            PostingID = Convert.ToInt32(Session["PostingId"].ToString());
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
            try
            {
                if (Cardetais.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                {
                    Session["AgentQCPaymentID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PaymentID"].ToString());
                }
                Session["AgentQCPSID1"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSID1"].ToString());
                Session["AgentQCCarID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["carid"].ToString());
                Session["AgentQCUID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["uid"].ToString());
                Session["AgentQCPostingID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["postingID"].ToString());
                Session["AgentQCUserPackID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["UserPackID"].ToString());
                Session["AgentQCSellerID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["sellerID"].ToString());
                Session["AgentQCPSID1"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSID1"].ToString());
                Session["AgentQCPaymentTypeID"] = Cardetais.Tables[0].Rows[0]["pmntType"].ToString();
            }
            catch { }
            // AuthorizePayment();
            CCFirstName = Cardetais.Tables[0].Rows[0]["cardholderName"].ToString();
            CCLastName = Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString();
            CCAddress = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString();
            CCZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
            CCNumber = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
            CCCvv = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();
            string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
            string[] EXpDt = EXpDate.Split(new char[] { '/' });
            string txtExpMon = EXpDt[0].ToString();
            string txtCCExpiresYear = "20" + EXpDt[1].ToString();

            CCExpiry = txtExpMon + "/" + txtCCExpiresYear;
            CCAmount = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
            CCCity = Cardetais.Tables[0].Rows[0]["billingCity"].ToString();
            CCState = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
            CPhnNo = Cardetais.Tables[0].Rows[0]["phone"].ToString();
            CEmail = Cardetais.Tables[0].Rows[0]["UserName"].ToString();
            VoiceRecord = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString();
            Session["QCViewPackageID"] = Cardetais.Tables[0].Rows[0]["PackageID"].ToString();
            Session["AgentQCPaymentTypeID"] = Cardetais.Tables[0].Rows[0]["pmntType"].ToString();



            AuthorizePayment(PostingID, CCFirstName, CCLastName, CCAddress, CCZip,
            CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, CPhnNo, CEmail, VoiceRecord);
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
            Response.Redirect("QCDataView.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCheckProcess_Click(object sender, EventArgs e)
    {

    }
    private void GoWithCheck(int postingid)
    {
        try
        {
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
            string txtRoutingNumberForCheck = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
            Double PackCost2 = new Double();
            PackCost2 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
            string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
            string PackName2 = Cardetais.Tables[0].Rows[0]["Description"].ToString();
            Session["QCViewPackageID"] = Cardetais.Tables[0].Rows[0]["PackageID"].ToString();
            string lblPackage = PackName2 + " ($" + PackAmount2 + ")";
            string txtFirstName = Cardetais.Tables[0].Rows[0]["cardholderName"].ToString();
            string txtLastName = Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString();
            string txtAddress = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString();
            string lblLocationState = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
            string txtZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
            string txtCity = Cardetais.Tables[0].Rows[0]["billingCity"].ToString();
            string txtPhone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();

            // By default, this sample code is designed to post to our test server for
            // developer accounts: https://test.authorize.net/gateway/transact.dll
            // for real accounts (even in test mode), please make sure that you are
            string post_url = "https://secure.authorize.net/gateway/transact.dll";
            //String post_url = "https://test.authorize.net/gateway/transact.dll";

            //The valid routing number of the customer’s bank 9 digits
            string sBankCode = string.Empty;
            sBankCode = txtRoutingNumberForCheck;

            //The customer’s valid bank account number Up to 20 digits The customer’s checking,
            string sBankaccountnumber = string.Empty;
            sBankaccountnumber = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
            //The type of bank account CHECKING,BUSINESSCHECKING,SAVINGS
            string sBankType = "SAVINGS";

            string txtBankNameForCheck = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankName"].ToString());
            //The name of the bank that holds the customer’s account Up to 50 characters
            string sbank_name = txtBankNameForCheck;

            string txtCustNameForCheck = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
            //The name of the bank that holds the customer’s account Up to 50 characters
            string sbank_acct_name = txtCustNameForCheck;
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
                Package = lblPackage;
            }

            string PackCost = lblPackage;
            string[] Pack = PackCost.Split('$');
            string[] FinalAmountSpl = Pack[1].Split(')');
            string FinalAmount = FinalAmountSpl[0].ToString();
            string txtPDAmountNow = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
            if (Convert.ToDouble(FinalAmount) != Convert.ToDouble(txtPDAmountNow))
            {
                Package = Package + "- Partial payment -";
            }

            post_values.Add("x_amount", string.Format("{0:c2}", Convert.ToDouble(txtPDAmountNow)));
            //post_values.Add("x_amount", txtPDAmountNow.Text);
            post_values.Add("x_description", Package);
            post_values.Add("x_merchant_email", "shravan@datumglobal.net");  //Emails Merchant
            post_values.Add("x_first_name", txtFirstName);
            post_values.Add("x_last_name", txtLastName);
            post_values.Add("x_address", txtAddress);
            post_values.Add("x_state", lblLocationState);
            post_values.Add("x_zip", txtZip);
            post_values.Add("x_city", txtCity);
            post_values.Add("x_phone", txtPhone);

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
                string PackCost1 = lblPackage;
                string[] Pack1 = PackCost1.Split('$');
                string[] FinalAmountSpl1 = Pack1[1].Split(')');
                string FinalAmount1 = FinalAmountSpl1[0].ToString();

                if (Convert.ToDouble(txtPDAmountNow).ToString() == "0")
                {
                    Result = "NoPayDue";
                }
                else if (Convert.ToDouble(FinalAmount1) != Convert.ToDouble(txtPDAmountNow))
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
    private void SavePayTransInfoForChecks(string AuthNetTransID, string Result)
    {
        try
        {
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            int PayTryBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
            string txtAddress = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString();
            string txtCity = Cardetais.Tables[0].Rows[0]["billingCity"].ToString();
            string lblLocationState = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
            string txtZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
            string txtPDAmountNow = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
            string txtCustNameForCheck = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
            string txtAccNumberForCheck = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
            string txtBankNameForCheck = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankName"].ToString());
            string txtRoutingNumberForCheck = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();

            string liAccType = Cardetais.Tables[0].Rows[0]["AccountTypeName"].ToString();
            string ddlAccType = liAccType;

            string CardType = string.Empty;
            CardType = "Check";
            string Address = txtAddress;
            string City = txtCity;
            string State = lblLocationState; ;
            string Zip = txtZip;
            string Amount = txtPDAmountNow;
            string AccountHolderName = txtCustNameForCheck;
            string AccountNumber = txtAccNumberForCheck;
            string BankName = txtBankNameForCheck;
            string RoutingNumber = txtRoutingNumberForCheck;
            string AccountType = ddlAccType;


            DataSet dsSavePayTrans = objHotLeadBL.SavePaymentHistoryDataForChecks(PostingID, PayTryBy, CardType, Address, City, State,
                Zip, Amount, Result, AuthNetTransID, AccountHolderName, AccountNumber, BankName, RoutingNumber, AccountType);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void rfsh_Click(object sender, EventArgs e)
    {
        totalsales = 0; Qcopen = 0; QcPending = 0; PaymOpen = 0; PymPnding = 0; TrnsfPen = 0;
        try
        {

            string Center = ddlCenters.SelectedValue;
            string trnfer = ddl_transtst.SelectedValue;
            string paymnt = ddlPaymentsta.SelectedValue;
            string Status = ddlQCStatus.SelectedItem.ToString();
            string sortBy = ddl_sortby.SelectedItem.ToString();
            if (sortBy == "SaleDate")
                sortBy = "SaleDate";
            else if (sortBy == "Sale Id")
                sortBy = "postingID";
            else if (sortBy == "Agent")
                sortBy = "AgentUserName";
            else if (sortBy == "Verifier")
                sortBy = "AgentUserName";
            else if (sortBy == "Sale Voice File")
                sortBy = "VoiceRecord";
            else if (sortBy == "Sale Package")
                sortBy = "PackageCode";
            else if (sortBy == "No.of Cars")
                sortBy = "TotalCars";
            else if (sortBy == "Payment type")
                sortBy = "dbo.Tbl_PmntType.pmntType";
            else if (sortBy == "Customer phone")
                sortBy = "phone";
            else if (sortBy == "Customer first name")
                sortBy = "sellerName";
            else if (sortBy == "Customer last name")
                sortBy = "LastName";
            else if (sortBy == "Customer city")
                sortBy = "city";
            else if (sortBy == "Package")
                sortBy = "PackageCode";
            else if (sortBy == "Make")
                sortBy = "make";

            else if (sortBy == "QC Status")
                sortBy = "QCStatusID";
            else if (sortBy == "Payment status")
                sortBy = "PSStatusID";
            else if (sortBy == "Transfer status")
                sortBy = "SmartzStatus";

            else if (sortBy == "QC Status")
                sortBy = "QCStatusID";
            else if (sortBy == "Payment status")
                sortBy = "PSStatusID";
            else if (sortBy == "Transfer status")
                sortBy = "SmartzStatus";
            else if (sortBy == "CarID")
                sortBy = "carid";






            int CenterID = Convert.ToInt32(ddlCenters.SelectedItem.Value);
            if (Status == "4") Status = "Open";
            GetResults(Center, Status, paymnt, trnfer, sortBy);
            int grdcount = grdWarmLeadInfo.Rows.Count;
            if (grdcount == 0)
            {
                divCarresults.Visible = false;
                UpdtpnlHeader.Visible = false;
                lblResHead.Visible = false;
                //Norecrds.Visible = true;
                //Norecrds.Text = "No Records Found";
                //Norecrds.Height = 40;
                //Norecrds.Width = 300;
                //Norecrds.BackColor = Color.Red;
                //Norecrds.ForeColor = Color.Blue;

            }
            else
            {
                divCarresults.Visible = true;
                UpdtpnlHeader.Visible = true;
                lblResHead.Visible = true;
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
    protected void grdIntroInfo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lnkbulkPr_Click(object sender, EventArgs e)
    {

        SetInitialRow();
        string QcStatusIds = "";
        btnOk1.Visible = false;
        btncancel1.Visible = true;
        btnproceed1.Visible = true;

        QcStatusIds = hdncheck.Value;
        QcStatusIds = QcStatusIds.Substring(0, QcStatusIds.Length - 1);
        string QcStatusIds1 = QcStatusIds;
        string[] QcStatusIds2 = QcStatusIds1.Split(',');
        int result = QcStatusIds2.Count(s => s != null);
        for (int i = 0; i < result; i++)
        {
            string[] QcStatusIds3 = QcStatusIds1.Split(',');
            string carid = QcStatusIds2[i].ToString();
            AddNewRowToGrid(Convert.ToInt32(carid));
        }



        if (ddltyprocs.SelectedItem.ToString() == "QC process")
        {
            if (result == 1)
            {
                // txtmulticars.Text = QcStatusIds.Replace(",",System.Environment.NewLine);
                MdlQcProcessta.Show();
            }
            else
            {
                //   txtmulticars.Text = QcStatusIds.Replace(",", System.Environment.NewLine);
                modelbulkQCProc.Show();

            }

        }
        else if (ddltyprocs.SelectedItem.ToString() == "Payment Process")
        {
            if (result == 1)
            {

            }
            else
            {

                string QcStatusIds5 = hdncheck.Value;
                QcStatusIds5 = QcStatusIds5.Substring(0, QcStatusIds5.Length - 1);
                string QcStatusIds6 = QcStatusIds5;
                string[] QcStatusIds7 = QcStatusIds6.Split(',');
                int result1 = QcStatusIds7.Count(s => s != null);
                for (int i = 0; i < result1; i++)
                {
                    string[] QcStatusIds3 = QcStatusIds1.Split(',');
                    string carid = QcStatusIds2[i].ToString();

                    int PostingID = Convert.ToInt32(carid);
                    DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
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

                                }
                                else
                                {
                                    PaymentVisaProcess(PostingID);

                                }
                            }
                            else
                            {
                                PaymentVisaProcess(PostingID);
                            }
                        }

                        else
                        {

                            if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                            {
                                if (Cardetais.Tables[0].Rows[0]["Amount1"].ToString() != "")
                                {
                                    Double TotalAmount1 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Amount1"].ToString());
                                    string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                                    if (ChkAmount == "0.00")
                                    {

                                    }
                                    else
                                    {
                                        PaymentCheckProcess(PostingID);
                                    }
                                }
                                else
                                {
                                    PaymentCheckProcess(PostingID);
                                }
                            }
                            else
                            {

                            }
                        }

                    }
                }
            }

        }


    }


    protected void Updatesta_Click(object sender, EventArgs e)
    {
        int Status = Convert.ToInt32(DropDownList2.SelectedItem.Value);
        UpdateQCStatus(Status);
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC Status is Updated Successfully.');", true);
    }
    private void UpdateQCStatus(int Status)
    {
        try
        {
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(qccarid.Text));
            txtOldQcNotes.Text = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();

            int QCBY = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            int QCID = 0;
            try
            {
                Session["AgentQCQCID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["QCID"].ToString());
            }
            catch { }
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

            //--details----//



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
            int CarID = Convert.ToInt32(qccarid.Text);
            int PostingID = Convert.ToInt32(qccarid.Text);

            DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(QCID, QCNotes, Status, CarID, QCBY, PostingID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BulkUpdatesta_Click(object sender, EventArgs e)
    {
        int Status = 0;// Convert.ToInt32(DropDownList3.SelectedItem.Value);
        UpdateBulkQCStatus(Status);

    }
    private void UpdateBulkQCStatus(int Status)
    {
        try
        {
            string carids = "";// txtmulticars.Text;
            string SuccessQc = "";
            string FailureQc = "";
            string QCStatusApp = "";
            string fruit = carids;
            string[] split1 = fruit.Split(',');
            int result = split1.Count(s => s != null);
            for (int i = 0; i < result; i++)
            {
                string fruit1 = carids;
                string[] split2 = fruit1.Split(',');
                string firstId = fruit1.Split(',')[i];
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
                int QCStatus = 0;
                //--details----//
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(firstId));
                txtOldQcNotes.Text = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();
                int PaymentStatus = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString());
                try
                {
                    QCStatus = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString());

                }
                catch { QCStatus = 0; }
                if (QCStatus == 1)
                {
                    QCStatusApp += firstId + ",";
                }
                if (PaymentStatus == 1)
                {


                    FailureQc += firstId + ",";


                }
                else
                {
                    int CarID = Convert.ToInt32(firstId);
                    int PostingID = Convert.ToInt32(firstId);

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

                    SuccessQc += CarID + ",";
                    Status = 0;// Convert.ToInt32(DropDownList3.SelectedValue);
                    //DataSet QCUpdateds = objHotLeadBL.UpdateBulkQCStatus(QCID, QCNotes, Status, CarID, QCBY, PostingID);
                }
            }
            if (SuccessQc.EndsWith(","))
                SuccessQc = SuccessQc.Remove(SuccessQc.Length - 1, 1);
            if (QCStatusApp.EndsWith(","))
                QCStatusApp = QCStatusApp.Remove(QCStatusApp.Length - 1, 1);
            if (FailureQc.EndsWith(","))
                FailureQc = FailureQc.Remove(FailureQc.Length - 1, 1);



            //if(SuccessQc!="")
            //QcSuccandfail.Text = "Sceesfully Changed QC Sataus are: '" + SuccessQc + "'";
            //if(QCStatusApp!="")
            // QcSuccandfail.Text += " and QCStatus Approveds are: '" + QCStatusApp + "'";
            //if(FailureQc!="")
            //    QcSuccandfail.Text += " Failure Qc Update are: '" + FailureQc + "'";
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        dt.Columns.Add(new DataColumn("Column4", typeof(string)));
        dt.Columns.Add(new DataColumn("Column5", typeof(string)));
        dt.Columns.Add(new DataColumn("Column6", typeof(string)));
        dt.Columns.Add(new DataColumn("Column7", typeof(string)));
        dt.Columns.Add(new DataColumn("Column8", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = 12;
        dr["Column2"] = string.Empty;
        dr["Column3"] = string.Empty;
        dt.Rows.Add(dr);

        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        BulkQc.DataSource = dt;
        BulkQc.DataBind();
    }

    private void AddNewRowToGrid(int Carid)
    {

        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    Label saleid = (Label)BulkQc.Rows[rowIndex].Cells[1].FindControl("saleid");
                    Label saledate = (Label)BulkQc.Rows[rowIndex].Cells[2].FindControl("saledate");
                    Label Agent = (Label)BulkQc.Rows[rowIndex].Cells[3].FindControl("Agent");
                    Label Customername = (Label)BulkQc.Rows[rowIndex].Cells[4].FindControl("Customername");
                    Label QCStatus = (Label)BulkQc.Rows[rowIndex].Cells[5].FindControl("QCStatus");
                    DropDownList desrdqcstat = (DropDownList)BulkQc.Rows[rowIndex].Cells[6].FindControl("desrdqcstat");
                    TextBox Notestoresult = (TextBox)BulkQc.Rows[rowIndex].Cells[7].FindControl("Notestoresult");
                    TextBox result = (TextBox)BulkQc.Rows[rowIndex].Cells[8].FindControl("result");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = "1";
                    dtCurrentTable.Rows[i - 1]["Column2"] = "2";
                    dtCurrentTable.Rows[i - 1]["Column3"] = "3";

                    dtCurrentTable.Rows[i - 1]["Column4"] = "4";
                    dtCurrentTable.Rows[i - 1]["Column5"] = "5";
                    dtCurrentTable.Rows[i - 1]["Column6"] = "6";
                    dtCurrentTable.Rows[i - 1]["Column7"] = "7";
                    dtCurrentTable.Rows[i - 1]["Column8"] = "8";

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                BulkQc.DataSource = dtCurrentTable;
                BulkQc.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    string carids = hdncheck.Value;
                    string fruit1 = carids;
                    string[] split2 = fruit1.Split(',');
                    string firstId = fruit1.Split(',')[i];
                    Label saleid = (Label)BulkQc.Rows[rowIndex].Cells[1].FindControl("saleid");
                    Label saledate = (Label)BulkQc.Rows[rowIndex].Cells[2].FindControl("saledate");
                    Label Agent = (Label)BulkQc.Rows[rowIndex].Cells[3].FindControl("Agent");
                    Label Customername = (Label)BulkQc.Rows[rowIndex].Cells[4].FindControl("Customername");
                    Label QCStatus = (Label)BulkQc.Rows[rowIndex].Cells[5].FindControl("QCStatus");
                    DropDownList desrdqcstat = (DropDownList)BulkQc.Rows[rowIndex].Cells[6].FindControl("desrdqcstat");
                    TextBox Notestoresult = (TextBox)BulkQc.Rows[rowIndex].Cells[7].FindControl("Notestoresult");
                    TextBox result = (TextBox)BulkQc.Rows[rowIndex].Cells[8].FindControl("result");


                    DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(firstId));
                    saleid.Text = firstId.ToString();
                    saledate.Text = Cardetais.Tables[0].Rows[0]["SaleDate"].ToString();
                    Agent.Text = Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString();
                    Customername.Text = Cardetais.Tables[0].Rows[0]["sellerName"].ToString();

                    if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "")
                    {
                        QCStatus.Text = "QC Open";

                    }
                    else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "1")
                        QCStatus.Text = "QC Approved";
                    else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "2")
                        QCStatus.Text = "QC Reject";
                    else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "3")
                        QCStatus.Text = "QC Pending";
                    else if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "4")
                        QCStatus.Text = "QC Returned";


                    rowIndex++;
                    BulkQc.Rows[dt.Rows.Count - 1].Visible = false;
                }
            }
        }
    }

    protected void btnproceed1_Click(object sender, EventArgs e)
    {

        UpdateQCBulkStatus();
        btnOk1.Visible = true;
        btncancel1.Visible = false;
        btnproceed1.Visible = false;
        // lblerrormsg1.Visible = false;


    }
    private void UpdateQCBulkStatus()
    {
        try
        {
            int count = BulkQc.Rows.Count;
            // BulkQc.Rows[0].Cells[8].Text = "Success";


            if (count <= 5)
            {
                for (int i = 0; i < count - 1; i++)
                {


                    Label CarID = (Label)BulkQc.Rows[i].Cells[2].FindControl("saleid");
                    string PostId = CarID.Text;
                    int PostingId = Convert.ToInt32(PostId.ToString());
                    int QCBY = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                    int QCID = 0;
                    DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingId);
                    try
                    {
                        Session["AgentQCQCID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["QCID"].ToString());
                    }
                    catch { Session["AgentQCQCID"] = null; }
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

                    //--details----//

                    string txtOldQcNotes = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();
                    DropDownList Status = (DropDownList)BulkQc.Rows[i].Cells[7].FindControl("desrdqcstat");
                    string selecteditem = Status.SelectedValue.ToString();

                    TextBox QcNotes = (TextBox)BulkQc.Rows[i].Cells[8].FindControl("Notestoresult");
                    string selectedStatus = QcNotes.Text;

                    Label OLDQCId = (Label)BulkQc.Rows[i].Cells[7].FindControl("QCStatus");
                    string QCID1 = OLDQCId.Text;
                    if (QCID1 == "")
                        QCID = 0;
                    else if (QCID1 == "QC Approved")
                        QCID = 1;
                    else if (QCID1 == "QC Reject")
                        QCID = 2;
                    else if (QCID1 == "QC Pending")
                        QCID = 3;
                    else if (QCID1 == "QC Returned")
                        QCID = 4;


                    if (selectedStatus != "")
                    {
                        string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
                        if (txtOldQcNotes != "")
                        {
                            QCNotes = txtOldQcNotes + "\n" + UpdateByWithDate + selectedStatus + "\n" + "-------------------------------------------------";
                        }
                        else
                        {
                            QCNotes = UpdateByWithDate + selectedStatus + "\n" + "-------------------------------------------------";
                        }
                    }
                    else
                    {
                        QCNotes = txtOldQcNotes;
                    }


                    string Paymstst = Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString();


                    if (Paymstst == "1")
                    {

                        BulkQc.Rows[i].Cells[8].Text = "Payment is Fully Paid.";

                    }
                    else
                    {
                        if (Paymstst != "1")
                        {
                            DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(QCID, QCNotes, Convert.ToInt32(selecteditem), PostingId, QCBY, PostingId);
                            BulkQc.Rows[i].Cells[8].Text = "Success.";
                        }
                    }
                }
            }
            else
            {
                //lblerrormsg1.Visible = true;
                //lblerrormsg1.Text = "You are selected more than 50 Records. So please select 50 records to update at a time";
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkwhitepa_Click(object sender, EventArgs e)
    {

    }
    protected void qcprocess_Click(object sender, EventArgs e)
    {
        string Str = "";

        for (int count = 0; count < grdWarmLeadInfo.Rows.Count; count++)
        {
            string txt = grdWarmLeadInfo.Rows[count].Cells[7].Text;
            if (txt != null)
            {
                TextBox Notes = (TextBox)grdWarmLeadInfo.Rows[count].Cells[7].FindControl("txtEnterNotes");
                string val1 = Notes.Text;
                if (val1 != "")
                    Str += Notes.Text + ",";

            }


        }
        if (Str.EndsWith(","))
            Str = Str.Remove(Str.Length - 1);

        Session["EnterNotes"] = Str;
        Session["EnterNotes1"] = Str;


        QCProcessForAll(1);
        string carids = hdncheck.Value;

        //TextBox QCEnterText = (TextBox)grdWarmLeadInfo.Rows[1].FindControl("txtEnterNotes").ToString();
        if (carids.EndsWith(","))
            carids = carids.Remove(carids.Length - 1, 1);
        DataSet SingleAgentSales = objHotLeadBL.USP_GetCarDetailsByPostingIDIN(carids);
        grid.DataSource = SingleAgentSales.Tables[0];
        grid.DataBind();
        lblQCProcessSaleID.Text = carids.ToString();
        MdlStatusResult.Show();
    }
    protected void qcreject_Click(object sender, EventArgs e)
    {
        string Str = "";

        for (int count = 0; count < grdWarmLeadInfo.Rows.Count; count++)
        {
            string txt = grdWarmLeadInfo.Rows[count].Cells[7].Text;
            if (txt != null)
            {
                TextBox Notes = (TextBox)grdWarmLeadInfo.Rows[count].Cells[7].FindControl("txtEnterNotes");
                string val1 = Notes.Text;
                if (val1 != "")
                    Str += Notes.Text + ",";

            }


        }
        if (Str.EndsWith(","))
            Str = Str.Remove(Str.Length - 1);

        Session["EnterNotes"] = Str;
        Session["EnterNotes1"] = Str;



        QCProcessForAll(2);
    }
    protected void qcreturn_Click(object sender, EventArgs e)
    {
        string Str = "";

        for (int count = 0; count < grdWarmLeadInfo.Rows.Count; count++)
        {
            string txt = grdWarmLeadInfo.Rows[count].Cells[7].Text;
            if (txt != null)
            {
                TextBox Notes = (TextBox)grdWarmLeadInfo.Rows[count].Cells[7].FindControl("txtEnterNotes");
                string val1 = Notes.Text;
                if (val1 != "")
                    Str += Notes.Text + ",";

            }


        }
        if (Str.EndsWith(","))
            Str = Str.Remove(Str.Length - 1);

        Session["EnterNotes"] = Str;
        Session["EnterNotes1"] = Str;


        QCProcessForAll(4);
    }
    public void QCProcessForAll(int QCUpdateSttua)
    {

        UpdateQCBulkStatus1(QCUpdateSttua);
        // MdlStatusResult.Show();

    }

    private void UpdateQCBulkStatus1(int QCUpdateSttua)
    {
        try
        {
            string FinalResult = "";
            string carids = hdncheck.Value;
            string fruit1 = carids;
            string[] split2 = fruit1.Split(',');
            int count = split2.Count();

            string txtbx = Session["EnterNotes1"].ToString();
            //string val = txtbx.Substring(txtbx.IndexOf(",") + 1);
            //Session["EnterNotes"] = val;
            //Session["EnterNotes1"] = val;
            //string[] splt = txtbx.Split(',');


            for (int i = 0; i < count - 1; i++)
            {

                string PostId = fruit1.Split(',')[i];
                int PostingId = Convert.ToInt32(PostId.ToString());
                int QCBY = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                int QCID = 0;
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingId);
                try
                {
                    Session["AgentQCQCID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["QCID"].ToString());
                }
                catch { Session["AgentQCQCID"] = null; }
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

                //--details----//

                string txtOldQcNotes = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();
                string selecteditem = QCUpdateSttua.ToString();

                //TextBox QcNotes = (TextBox)grdWarmLeadInfo.Rows[i].Cells[2].FindControl("txtEnterNotes");
                string selectedStatus = txtbx.Split(',')[0].Trim();

                string QCID1 = QCUpdateSttua.ToString();

                if (selectedStatus != "")
                {
                    string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
                    if (txtOldQcNotes != "")
                    {
                        QCNotes = txtOldQcNotes + "\n" + UpdateByWithDate + selectedStatus + "\n" + "-------------------------------------------------";
                    }
                    else
                    {
                        QCNotes = UpdateByWithDate + selectedStatus + "\n" + "-------------------------------------------------";
                    }
                }
                else
                {
                    QCNotes = txtOldQcNotes;
                }


                string Paymstst = Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString();


                if (Paymstst == "1")
                {

                    FinalResult = "Fail" + ",";

                }
                else
                {
                    if (Paymstst != "1")
                    {
                        if (Cardetais.Tables[0].Rows[0]["QCStatusID"].ToString() == "1")
                            FinalResult = "QC Alerdy Approved" + ",";
                        else
                        {
                            DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(QCID, QCNotes, Convert.ToInt32(selecteditem), PostingId, QCBY, PostingId);
                            FinalResult = "Approved" + ",";
                        }
                    }
                }
            }
            try
            {
                Session["FinalResult"] = FinalResult.ToString();

            }
            catch { }


        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void btnPPrc_Click(object sender, EventArgs e)
    {
        string FinalResult = "";
        string carids = "";
        for (int i = 0; i <= grid.Rows.Count - 1; i++)
        {
            Label Notes = (Label)grid.Rows[i].Cells[1].FindControl("lblsalesid");
            string Ids = Notes.Text;
            carids += Ids + ",";

        }
        if (carids.EndsWith(","))
            carids = carids.Remove(carids.Length - 1, 1);
        string fruit1 = carids;
        Session["PaymentCarId"] = carids;
        string[] split2 = fruit1.Split(',');
        int count = split2.Count();
        for (int i = 0; i <= count - 2; i++)
        {
            string PostingID1 = fruit1.Split(',')[i];
            int PostingID = Convert.ToInt32(PostingID1.ToString());

            Session["PostingId"] = PostingID.ToString();
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
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

                        }
                        else
                        {
                            PaymentVisaProcessBulk(PostingID);

                        }
                    }
                    else
                    {
                        PaymentVisaProcessBulk(PostingID);
                    }
                }

                else
                {

                    if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                    {
                        if (Cardetais.Tables[0].Rows[0]["Amount1"].ToString() != "")
                        {
                            Double TotalAmount1 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Amount1"].ToString());
                            string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                            if (ChkAmount == "0.00")
                            {

                            }
                            else
                            {
                                PaymentCheckProcessBulk(PostingID);

                            }
                        }
                        else
                        {
                            PaymentCheckProcessBulk(PostingID);
                        }
                    }
                    else
                    {

                    }
                }

            }


        }
        MdlStatusResult.Hide();
        //Payment Show

        DataSet SingleAgentSales = objHotLeadBL.USP_GetCarDetailsByPostingIDIN(carids);
        GridView1.DataSource = SingleAgentSales.Tables[0];
        GridView1.DataBind();
        MdlStatusResult.Show();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string carids = "";
            Label caridss = (Label)e.Row.FindControl("lblsalesid");
            carids = caridss.Text;
            string FinalResult = "";
            PostingID = Convert.ToInt32(carids);
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));

            if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "3") || (Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "4"))
            {
                Label caridss2 = (Label)e.Row.FindControl("Payment");
                caridss2.Text = "Pending status";
            }
            else
            {
                Label caridss1 = (Label)e.Row.FindControl("Payment");
                caridss1.Text = "Processed";

            }


            string fruit1 = carids;
            string[] split2 = fruit1.Split(',');
            int count = split2.Count();

            try
            {
                string txtbx = Session["PaymentCarId"].ToString();
                string val = txtbx.Substring(txtbx.IndexOf(",") + 1);

            }
            catch { }
        }
        MdlPayStat.Show();


    }
    protected void smartz_Click(object sender, EventArgs e)
    {
        string FinalResult = "", carids = "";
        for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
        {
            Label Ids = (Label)GridView1.Rows[i].Cells[1].FindControl("lblsalesid");
            string Cars = Ids.Text;
            carids += Cars + ",";

        }
        if (carids.EndsWith(","))
            carids = carids.Remove(carids.Length - 1, 1);

        string fruit1 = carids;
        string[] split2 = fruit1.Split(',');
        int count = split2.Count();


        for (int k = 0; k <= count - 1; k++)
        {
            int PostingID = Convert.ToInt32(fruit1.Split(',')[k]);
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
                           // MdepAddAnotherCarAlert.Show();
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

    protected void lblcuscheck_Click(object sender, EventArgs e)
    {
        string a = "630 E Front St";
        a = a.Replace(" ", "+");
        string b = "Farmersville, CA 93223";
        b = b.Replace(" ", "+");
        b = b.Replace(",", "%2C");
        string url = "http://www.whitepages.com/search/FindNearby?utf8=%E2%9C%93&street=" + a + "&where=" + b;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
    }
    protected void btnAddAnotherCarNoN1_Click(object sender, EventArgs e)
    {
        MdlPayStat.Show();
    }
    protected void PProcess_Click(object sender, EventArgs e)
    {

        BulkPayment();
    }
    public void BulkPayment()
    {
        string FinalResult = "";
        string carids = hdncheck.Value;
        string fruit1 = carids;
        string[] split2 = fruit1.Split(',');
        int count = split2.Count();

        for (int i = 0; i < count - 1; i++)
        {
            string PostingID1 = fruit1.Split(',')[i];
            int PostingID = Convert.ToInt32(PostingID1.ToString());
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
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

                        }
                        else
                        {

                            PaymentVisaProcessBulk(PostingID);

                        }
                    }
                    else
                    {
                        PaymentVisaProcessBulk(PostingID);
                    }
                }

                else
                {

                    if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 5)
                    {
                        if (Cardetais.Tables[0].Rows[0]["Amount1"].ToString() != "")
                        {
                            Double TotalAmount1 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Amount1"].ToString());
                            string ChkAmount = string.Format("{0:0.00}", TotalAmount1);
                            if (ChkAmount == "0.00")
                            {

                            }
                            else
                            {
                                PaymentCheckProcessBulk(PostingID);
                            }
                        }
                        else
                        {
                            PaymentCheckProcessBulk(PostingID);
                        }
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "1"))
                {
                    FinalResult += "Full Paid" + ",";
                }
                else if ((Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString() == "2"))
                {
                    FinalResult += "Reject" + ",";
                }


            }

        }
        DataSet SingleAgentSales = objHotLeadBL.USP_GetCarDetailsByPostingIDIN(carids);
        GridView1.DataSource = SingleAgentSales.Tables[0];
        GridView1.DataBind();
        MdlPayStat.Show();

    }

    private void PaymentVisaProcessBulk(int PostingID)
    {
        try
        {
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
            // AuthorizePayment();
            CCFirstName = Cardetais.Tables[0].Rows[0]["cardholderName"].ToString().Trim();
            CCLastName = Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString().Trim();
            CCAddress = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString().Trim();
            CCZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString().Trim();
            CCNumber = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString().Trim();
            CCCvv = Cardetais.Tables[0].Rows[0]["cardCode"].ToString().Trim();
            string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString().Trim();
            string[] EXpDt = EXpDate.Split(new char[] { '/' });
            string txtExpMon = EXpDt[0].ToString();
            string txtCCExpiresYear = "20" + EXpDt[1].ToString().Trim();

            CCExpiry = txtExpMon + "/" + txtCCExpiresYear;
            CCAmount = Cardetais.Tables[0].Rows[0]["Amount1"].ToString().Trim();
            CCCity = Cardetais.Tables[0].Rows[0]["billingCity"].ToString().Trim();
            CCState = Cardetais.Tables[0].Rows[0]["State_Code"].ToString().Trim();
            CPhnNo = Cardetais.Tables[0].Rows[0]["phone"].ToString().Trim();
            CEmail = Cardetais.Tables[0].Rows[0]["UserName"].ToString().Trim();
            VoiceRecord = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString().Trim();
            Session["QCViewPackageID"] = Cardetais.Tables[0].Rows[0]["PackageID"].ToString().Trim();
            Session["AgentQCPaymentTypeID"] = Cardetais.Tables[0].Rows[0]["pmntType"].ToString().Trim();


            DataSet dsChkRejectThere = objHotLeadBL.CheckResultPaymentReject(CCFirstName, CCLastName, CCAddress, CCZip, CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, PostingID);
            if (dsChkRejectThere.Tables.Count > 0)
            {
                if (dsChkRejectThere.Tables[0].Rows.Count > 0)
                {
                    DateTime dtTranDt = Convert.ToDateTime(dsChkRejectThere.Tables[0].Rows[0]["PayTryDatetime"].ToString());
                    string DtTranDate = dtTranDt.ToString("MM/dd/yy hh:mm tt");
                    lblRejectThereError.Visible = true;
                    lblRejectThereError.Text = "We have already attempted to process the payment earlier at " + DtTranDate + " with the same data. No payment information is updated since then. Do you want to try again?";
                    Session["PaymentBulkResults"] = lblRejectThereError.Text;
                    //mdepAlertRejectThere.Show();
                }
                else
                {
                    AuthorizePayment(PostingID, CCFirstName, CCLastName, CCAddress, CCZip, CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, CPhnNo, CEmail, VoiceRecord);
                }

            }
            else
            {
                AuthorizePayment(PostingID, CCFirstName, CCLastName, CCAddress, CCZip, CCNumber, CCCvv, CCExpiry, CCAmount, CCCity, CCState, CPhnNo, CEmail, VoiceRecord);
            }

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void PaymentCheckProcessBulk(int postingid)
    {
        try
        {
            GoWithCheckBulk(postingid);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GoWithCheckBulk(int postingid)
    {
        try
        {
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(postingid));
            string txtRoutingNumberForCheck = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
            Double PackCost2 = new Double();
            PackCost2 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
            string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
            string PackName2 = Cardetais.Tables[0].Rows[0]["Description"].ToString();
            Session["QCViewPackageID"] = Cardetais.Tables[0].Rows[0]["PackageID"].ToString();
            string lblPackage = PackName2 + " ($" + PackAmount2 + ")";
            string txtFirstName = Cardetais.Tables[0].Rows[0]["cardholderName"].ToString();
            string txtLastName = Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString();
            string txtAddress = Cardetais.Tables[0].Rows[0]["billingAdd"].ToString();
            string lblLocationState = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
            string txtZip = Cardetais.Tables[0].Rows[0]["billingZip"].ToString();
            string txtCity = Cardetais.Tables[0].Rows[0]["billingCity"].ToString();
            string txtPhone = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();

            // By default, this sample code is designed to post to our test server for
            // developer accounts: https://test.authorize.net/gateway/transact.dll
            // for real accounts (even in test mode), please make sure that you are
            string post_url = "https://secure.authorize.net/gateway/transact.dll";
            //String post_url = "https://test.authorize.net/gateway/transact.dll";

            //The valid routing number of the customer’s bank 9 digits
            string sBankCode = string.Empty;
            sBankCode = txtRoutingNumberForCheck;

            //The customer’s valid bank account number Up to 20 digits The customer’s checking,
            string sBankaccountnumber = string.Empty;
            sBankaccountnumber = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
            //The type of bank account CHECKING,BUSINESSCHECKING,SAVINGS
            string sBankType = "SAVINGS";

            string txtBankNameForCheck = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankName"].ToString());
            //The name of the bank that holds the customer’s account Up to 50 characters
            string sbank_name = txtBankNameForCheck;

            string txtCustNameForCheck = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString());
            //The name of the bank that holds the customer’s account Up to 50 characters
            string sbank_acct_name = txtCustNameForCheck;
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
                Package = lblPackage;
            }

            string PackCost = lblPackage;
            string[] Pack = PackCost.Split('$');
            string[] FinalAmountSpl = Pack[1].Split(')');
            string FinalAmount = FinalAmountSpl[0].ToString();
            string txtPDAmountNow = Cardetais.Tables[0].Rows[0]["Amount1"].ToString();
            if (Convert.ToDouble(FinalAmount) != Convert.ToDouble(txtPDAmountNow))
            {
                Package = Package + "- Partial payment -";
            }

            post_values.Add("x_amount", string.Format("{0:c2}", Convert.ToDouble(txtPDAmountNow)));
            //post_values.Add("x_amount", txtPDAmountNow.Text);
            post_values.Add("x_description", Package);
            post_values.Add("x_merchant_email", "shravan@datumglobal.net");  //Emails Merchant
            post_values.Add("x_first_name", txtFirstName);
            post_values.Add("x_last_name", txtLastName);
            post_values.Add("x_address", txtAddress);
            post_values.Add("x_state", lblLocationState);
            post_values.Add("x_zip", txtZip);
            post_values.Add("x_city", txtCity);
            post_values.Add("x_phone", txtPhone);

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
                string PackCost1 = lblPackage;
                string[] Pack1 = PackCost1.Split('$');
                string[] FinalAmountSpl1 = Pack1[1].Split(')');
                string FinalAmount1 = FinalAmountSpl1[0].ToString();

                if (Convert.ToDouble(txtPDAmountNow).ToString() == "0")
                {
                    Result = "NoPayDue";
                }
                else if (Convert.ToDouble(FinalAmount1) != Convert.ToDouble(txtPDAmountNow))
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
    protected void PReject_Click(object sender, EventArgs e)
    {
        try
        {
            int paymentId = 0, PostingId = 0;
            string carid = hdncheck.Value;
            string fruit1 = carid;
            string[] split2 = fruit1.Split(',');
            int count = split2.Count();
            for (int i = 0; i <= count - 2; i++)
            {
                paymentId = 2;
                PostingId = Convert.ToInt16(fruit1.Split(',')[i]);
                DataSet QCUpdateds = objHotLeadBL.UpdatePaymaentStus(paymentId, PostingId);
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Payment is rejected.');", true);
            }
        }
        catch { }
    }
    protected void Preturn_Click(object sender, EventArgs e)
    {
        try
        {
            int paymentId = 0, PostingId = 0;
            string carid = hdncheck.Value;
            string fruit1 = carid;
            string[] split2 = fruit1.Split(',');
            int count = split2.Count();
            for (int i = 0; i <= count - 2; i++)
            {
                paymentId = 5;
                PostingId = Convert.ToInt16(fruit1.Split(',')[i]);
                DataSet QCUpdateds = objHotLeadBL.UpdatePaymaentStus(paymentId, PostingId);
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Payment is returned.');", true);
            }

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