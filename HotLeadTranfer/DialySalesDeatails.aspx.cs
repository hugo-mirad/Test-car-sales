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


public partial class DialySalesDeatails : System.Web.UI.Page
{
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet CarsDetails = new DataSet();
    DataSet dsDropDown = new DataSet();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    UserRegistrationInfo objUserregInfo = new UserRegistrationInfo();
    HotLeadsBL objHotLeadBL = new HotLeadsBL();
    int k = 1; int gd = 0, pd = 0, sd = 0, other = 0;
    int gd1 = 0, pd1 = 0, sd1 = 0, other1 = 0; DateTime dt;
    int gd2 = 0, pd2 = 0, sd2 = 0, other2 = 0;
    int gd3 = 0, pd3 = 0, sd3 = 0, other3 = 0; 
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

                FillCenters();
                string CenterCode = Session[Constants.CenterCode].ToString();
                int CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString());
                if (CenterID == 6)
                {
                    ddlCenters.Visible = true;
                    lblAge.Visible = true;
                    CenterID = 0;
                }
                else
                {
                    ddlCenters.Visible = false;
                    lblAge.Visible = false;
                }
                txtStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
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
                    //string CenterCode = Session[Constants.CenterCode].ToString();
                    string UserLogName = Session[Constants.USER_NAME].ToString();
                    if (LogUsername.Length > 20)
                    {
                        lblUserName.Text = LogUsername.ToString().Substring(0, 20);

                        lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString() + ")-" + UserLogName.ToString();
                        //}
                    }
                    else
                    {
                        lblUserName.Text = LogUsername;
                        lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString() + ")-" + UserLogName.ToString();
                    }
                }
                dtreportTime.Text = DateTime.Now.ToString();
                getdetails(CenterCode, CenterID);



            }
        }
    }
    private void LoadUserRights()
    {
        try
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
                    if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Agent report")
                    {
                        if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                        {
                            lnkbtnAgentReport.Enabled = true;
                            lnkbtnMyDealerRep.Enabled = true;
                        }

                    }
                    if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Center report")
                    {
                        if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                        {
                            lnkbtnLeadsDownLoad.Enabled = true;
                        }

                    }

                }
            }

        }
        catch { }

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
    private void FillCenters()
    {
        try
        {
            DataSet dsCenters = objHotLeadBL.GetAllCentersData1();
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
    private void getdetails(string CenterCode, int CenterID)
    {
          
            ViewState["OpenStatus"] = "0";
            dt =Convert.ToDateTime(txtStartDate.Text.ToString());
            DataSet dsData = objHotLeadBL.GetAllAgentsByCenterID1(CenterID);
          


            if (dsData.Tables[0].Rows.Count > 0)
            {
                Rpt_Agents.DataSource = dsData.Tables[0];
                Rpt_Agents.DataBind();
               
            }


            if (dsData.Tables[0].Rows.Count > 0)
            {
                Rpt_Verifiers.DataSource = dsData.Tables[0];
                Rpt_Verifiers.DataBind();
            }
           
    }

    protected void Rpt_Agents_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //string CenterCode = Session[Constants.CenterCode].ToString();
                //int CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString());
                string CenterCode = Session[Constants.CenterCode].ToString();
                int CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString());
                if (CenterID == 6)
                {
                    ddlCenters.Visible = true;
                    lblAge.Visible = true;
                    CenterID = 0;
                }
                else
                {
                    ddlCenters.Visible = false;
                    lblAge.Visible = false;
                }
                dt = Convert.ToDateTime(txtStartDate.Text.ToString());
                 Label lblsubPD = (Label)e.Item.FindControl("AgentName");
                 HiddenField AgentUid = (HiddenField)e.Item.FindControl("AgentUid");
                 string Agentlue = AgentUid.Value;
                 HiddenField hdnagcentr = (HiddenField)e.Item.FindControl("hdnagcentr");
                 string Agentcenter = hdnagcentr.Value;
                 DataSet dsDataVerifier = objHotLeadBL.GetSalesByDetails(Convert.ToInt32(Agentcenter), dt, Convert.ToInt32(Agentlue));
                var repeater2 = (Repeater)e.Item.FindControl("Rpt_Customer");

                if (dsDataVerifier.Tables[0].Rows.Count > 0)
                {
                    repeater2.DataSource = dsDataVerifier;
                    for (int i = 0; i <= dsDataVerifier.Tables[0].Rows.Count-1; i++)
                    {
                        string dc = dsDataVerifier.Tables[0].Rows[i].ItemArray[5].ToString();
                      string lblPaidSat = dsDataVerifier.Tables[0].Columns[5].ToString();

                      if (lblPaidSat != "5")
                      {
                          if (dc == "6") pd = pd + 1;
                          else if (dc == "5") gd = gd + 1;
                          else if (dc == "5") sd = sd + 1;
                          else other = other + 1;
                          Session["Sub"] = "Submitted PD (" + pd + "), GD(" + gd + "), SD(" + sd + "), Other (" + other + ") ";
                        
                      }
                      else
                      {
                          if (dc == "6") pd1 = pd1 + 1;
                          else if (dc == "5") gd1 = gd1 + 1;
                          else if (dc == "5") sd1 = sd1 + 1;
                          else other1 = other1 + 1;
                          Session["Paid"] = "Paid PD (" + pd1 + "), GD(" + gd1 + "), SD(" + sd1 + "), Other (" + other1 + ") ";

                      }
                        
                    }

                    repeater2.DataBind();


                    k = 1;  gd = 0; pd = 0; sd = 0;other = 0;
                    gd1 = 0; pd1 = 0; sd1 = 0; other1 = 0;
                }
                else
                {
                    lblsubPD.Visible = false;
                    k = 1; gd = 0; pd = 0; sd = 0; other = 0;
                    gd1 = 0; pd1 = 0; sd1 = 0; other1 = 0;
                }
               
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }


    protected void Rpt_Customer_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblSno = (Label)e.Item.FindControl("SNo");
                Label lblCustName=(Label)e.Item.FindControl("lblCustName");
                Label lblLName = (Label)e.Item.FindControl("lblLName");
                Label lblPackId = (Label)e.Item.FindControl("lblPackId");
                Label lblAmount = (Label)e.Item.FindControl("lblAmount");
                Label lblPadTyp = (Label)e.Item.FindControl("lblPadTyp");
                Label lblPaidSat = (Label)e.Item.FindControl("lblPaidSat");
                HiddenField HdnAmount=(HiddenField)e.Item.FindControl("HdnAmount");
              
                string FName = lblCustName.Text; string LName = lblLName.Text;
                FName = FName + " " + LName.ToString();
                lblCustName.Text = FName;

                string Packageid = lblPackId.Text;
                if (Packageid == "6") Packageid = "PD";
                else if (Packageid == "5") Packageid = "GD";
                else if (Packageid == "4") Packageid = "SD";
                else  Packageid = "Other";
                lblPackId.Text = Packageid;
                string Amount = lblAmount.Text;
                string HidAmount = HdnAmount.Value;
                if (Amount == "0.0") lblAmount.Text = HdnAmount.Value;

                if (lblPadTyp.Text == "1" || lblPadTyp.Text == "2" || lblPadTyp.Text == "3" || lblPadTyp.Text == "4")
                    lblPadTyp.Text = "CC";
                else if (lblPadTyp.Text == "5")
                    lblPadTyp.Text = "Check";
                else if (lblPadTyp.Text == "6")
                    lblPadTyp.Text = "Paypal";
                else if (lblPadTyp.Text == "7")
                    lblPadTyp.Text = "Cash";
                else if (lblPadTyp.Text == "8")
                    lblPadTyp.Text = "Invoice";

                if (lblPadTyp.Text == "CC" || lblPadTyp.Text == "Paypal" || lblPadTyp.Text == "Cash" || lblPadTyp.Text == "Cash") lblPaidSat.Text = "Submitted";
                else if (lblPadTyp.Text == "Check") lblPaidSat.Text = "Paid";


                if (lblSno.Text != null)
                   lblSno.Text =  e.Item.ItemIndex + 1 + "";
              
                Session["lblPaidSat"] = lblPaidSat.Text;
                Session["Packageid"] = Packageid;
               
               
            }

          

            if (e.Item.ItemType == ListItemType.Footer)
            {

              
                //string lblPaidSat= Session["lblPaidSat"].ToString();
                //string Packageid = Session["Packageid"].ToString();
                Label lblsubmit1 = (Label)e.Item.FindControl("lblsubmit1");
                Label lblPaidSum = (Label)e.Item.FindControl("lblpaid1");
                //if (lblPaidSat == "Submitted")
                //{
                //    if (Packageid == "PD") pd = pd + 1;
                //    else if (Packageid == "GD") gd = gd + 1;
                //    else if (Packageid == "SD") sd = sd + 1;
                //    else other = other + 1;
                //    lblsubmit1.Text = "Submitted PD (" + pd + "), GD(" + gd + "), SD(" + sd + "), Other (" + other + ") ";


                //}
                //else
                //{
                //    if (Packageid == "PD") pd1 = pd1 + 1;
                //    else if (Packageid == "GD") gd1 = gd1 + 1;
                //    else if (Packageid == "SD") sd1 = sd1 + 1;
                //    else other1 = other1 + 1;
                //    lblPaidSum.Text = "Paid PD (" + pd1 + "), GD(" + gd1 + "), SD(" + sd1 + "), Other (" + other1 + ") ";
                //}
                if(Session["Sub"]!=null)
                lblsubmit1.Text = Session["Sub"].ToString();
                if (lblsubmit1.Text == "Submitted" || lblsubmit1.Text == "Submitted")
                    lblsubmit1.Text = "Submitted PD (0), GD(0), SD(0), Other (0) ";
                if (Session["Paid"]!= null)
                lblPaidSum.Text = Session["Paid"].ToString();
                if (lblPaidSum.Text == "Paid" || lblPaidSum.Text == "Paid")
                    lblPaidSum.Text = "Paid PD (0), GD(0), SD(0), Other (0) ";
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    protected void Rpt_Verifiers_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //string CenterCode = Session[Constants.CenterCode].ToString();
                //int CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString());
                string CenterCode = Session[Constants.CenterCode].ToString();
                int CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString());
                if (CenterID == 6)
                {
                    ddlCenters.Visible = true;
                    lblAge.Visible = true;
                    CenterID = 0;
                }
                else
                {
                    ddlCenters.Visible = false;
                    lblAge.Visible = false;
                }
                dt = Convert.ToDateTime(txtStartDate.Text.ToString());
                Label lblsubPD = (Label)e.Item.FindControl("AgentName");
                Label Label7 = (Label)e.Item.FindControl("AgentName");
                HiddenField AgentUid = (HiddenField)e.Item.FindControl("AgentUid");
                string Agentlue = AgentUid.Value;
                DataSet dsDataVerifier = objHotLeadBL.GetVerifierSalesByDetails(CenterID, dt, Convert.ToInt32(Agentlue));
                var repeater2 = (Repeater)e.Item.FindControl("Rpt_VerifierCustomer");

                if (dsDataVerifier.Tables[0].Rows.Count > 0)
                {
                    repeater2.DataSource = dsDataVerifier;

                    for (int i = 0; i <= dsDataVerifier.Tables[0].Rows.Count - 1; i++)
                    {
                        string dc1 = dsDataVerifier.Tables[0].Rows[i].ItemArray[5].ToString();
                        string lblPaidSat1 = dsDataVerifier.Tables[0].Columns[5].ToString();

                        if (lblPaidSat1 != "5")
                        {
                            if (dc1== "6") pd2 = pd2 + 1;
                            else if (dc1 == "5") gd2 = gd2 + 1;
                            else if (dc1 == "5") sd2 = sd2 + 1;
                            else other = other + 1;
                            Session["Sub"] = "Submitted PD (" + pd2 + "), GD(" + gd2 + "), SD(" + sd2 + "), Other (" + other2 + ") ";

                        }
                        else
                        {
                            if (dc1 == "6") pd3 = pd3 + 1;
                            else if (dc1 == "5") gd3 = gd3 + 1;
                            else if (dc1 == "5") sd3 = sd3 + 1;
                            else other1 = other3 + 1;
                            Session["Paid"] = "Paid PD (" + pd3 + "), GD(" + gd3 + "), SD(" + sd3 + "), Other (" + other3 + ") ";

                        }

                    }
                    repeater2.DataBind();
                    gd2 = 0; pd2 = 0; sd2 = 0; other2 = 0;
                    gd3 = 0; pd3 = 0; sd3 = 0; other3 = 0; 
                }
                else
                {
                    lblsubPD.Visible = false;
                     gd2 = 0; pd2 = 0; sd2 = 0; other2 = 0;
                     gd3 = 0; pd3 = 0; sd3 = 0; other3 = 0; 
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    protected void Rpt_VerifierCustomer_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label VlblSno = (Label)e.Item.FindControl("VSNo");
                Label VlblCustName = (Label)e.Item.FindControl("VlblCustName");
                Label VlblLName = (Label)e.Item.FindControl("VlblLName");
                Label VlblPackId = (Label)e.Item.FindControl("VlblPackId");
                Label VlblAmount = (Label)e.Item.FindControl("VlblAmount");
                Label VlblPadTyp = (Label)e.Item.FindControl("VlblPadTyp");
                Label VlblPaidSat = (Label)e.Item.FindControl("VlblPaidSat");
                HiddenField VHdnAmount = (HiddenField)e.Item.FindControl("VHdnAmount");
                
                string FName = VlblCustName.Text; string LName = VlblLName.Text;
                FName = FName + " " + LName.ToString();
                VlblCustName.Text = FName;

                string Packageid = VlblPackId.Text;
                if (Packageid == "6") Packageid = "PD";
                else if (Packageid == "5") Packageid = "GD";
                else if (Packageid == "4") Packageid = "SD";
                else Packageid = "Other";
                VlblPackId.Text = Packageid;
                string Amount = VlblAmount.Text;
                string HidAmount = VHdnAmount.Value;
                if (Amount == "0.0") VlblAmount.Text = VHdnAmount.Value;

                if (VlblPadTyp.Text == "1" || VlblPadTyp.Text == "2" || VlblPadTyp.Text == "3" || VlblPadTyp.Text == "4")
                    VlblPadTyp.Text = "CC";
                else if (VlblPadTyp.Text == "5")
                    VlblPadTyp.Text = "Check";
                else if (VlblPadTyp.Text == "6")
                    VlblPadTyp.Text = "Paypal";
                else if (VlblPadTyp.Text == "7")
                    VlblPadTyp.Text = "Cash";
                else if (VlblPadTyp.Text == "8")
                    VlblPadTyp.Text = "Invoice";

                if (VlblPadTyp.Text == "CC" || VlblPadTyp.Text == "Paypal" || VlblPadTyp.Text == "Cash" || VlblPadTyp.Text == "Cash") VlblPaidSat.Text = "Submitted";
                else if (VlblPadTyp.Text == "Check") VlblPaidSat.Text = "Paid";

               
                if (VlblSno.Text != null)
                    VlblSno.Text = e.Item.ItemIndex + 1 + "";

                Session["VlblPaidSat"] = VlblPaidSat.Text;
                Session["Packageid"] = Packageid;
            }

            if (e.Item.ItemType == ListItemType.Footer )
            {
                Label Vlblsubmit1 = (Label)e.Item.FindControl("Vlblsubmit1");
                Label VlblPaidSum = (Label)e.Item.FindControl("Vlblpaid1");
                //string VlblPaidSat = Session["VlblPaidSat"].ToString();
                //string Packageid = Session["Packageid"].ToString();
                //if (VlblPaidSat == "Submitted")
                //{
                //    if (Packageid == "PD") pd2 = pd2 + 1;
                //    else if (Packageid == "GD") gd2 = gd2 + 1;
                //    else if (Packageid == "SD") sd2 = sd2 + 1;
                //    else other = other + 1;
                //    Vlblsubmit1.Text = "Submitted PD (" + pd2 + "), GD(" + gd2 + "), SD(" + sd2 + "), Other (" + other2 + ") ";

                //}
                //else
                //{
                //    if (Packageid == "PD") pd3 = pd3 + 1;
                //    else if (Packageid == "GD") gd3 = gd3 + 1;
                //    else if (Packageid == "SD") sd3 = sd3 + 1;
                //    else other1 = other1 + 1;
                //    VlblPaidSum.Text = "Paid PD (" + pd3 + "), GD(" + gd3 + "), SD(" + sd3 + "), Other (" + other1 + ") ";
                //}
                if (Session["Sub"] != null)
                    Vlblsubmit1.Text = Session["Sub"].ToString();
               
                if (Session["Paid"] != null)
                    VlblPaidSum.Text = Session["Paid"].ToString();
             

                if (Vlblsubmit1.Text == "Submitted" || Vlblsubmit1.Text == "")
                    Vlblsubmit1.Text = "Submitted PD (0), GD(0), SD(0), Other (0) ";
                if (VlblPaidSum.Text == "Paid" || VlblPaidSum.Text == "" )
                    VlblPaidSum.Text = "Paid PD (0), GD(0), SD(0), Other (0) ";

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
    protected void Change_Click(object sender, EventArgs e)
    {
       gd2 = 0; pd2 = 0; sd2 = 0; other2 = 0;
       gd3 = 0; pd3 = 0; sd3 = 0; other3 = 0; 
        dtreportTime.Text = DateTime.Now.ToString();
        string CenterCode = Session[Constants.CenterCode].ToString();
        int CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString());
        try
        {
            if (ddlCenters.Visible == true)
                CenterID = Convert.ToInt32(ddlCenters.SelectedValue);
            else
                CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString());
        }
        catch { CenterID = Convert.ToInt16(Session[Constants.CenterCodeID].ToString()); }
        getdetails(CenterCode, CenterID);
    }
}
