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


public partial class WeeklySalesByDay : System.Web.UI.Page
{
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet CarsDetails = new DataSet();
    DataSet dsDropDown = new DataSet();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    UserRegistrationInfo objUserregInfo = new UserRegistrationInfo();
    HotLeadsBL objHotLeadBL = new HotLeadsBL();
    double PDTotal = 0, GDTotal = 0, SDTotal = 0, OtherTotal = 0, SubTotaTotal = 0, SubFri = 0, SubSat = 0;
    double FPDTotal = 0, FGDTotal = 0, FSDTotal = 0, FOtherTotal = 0, FSubTotaTotal = 0, FSubFri = 0, FSubSat = 0;

    double PDVRTotal = 0, GDVRTotal = 0, SDVRTotal = 0, OtherVRTotal = 0, SubVRTotaTotal = 0, SubVRFriTotal = 0, SubVRSatTotal = 0;
    double FVRPDTotal = 0, FVRGDTotal = 0, FVRSDTotal = 0, FVROtherTotal = 0, FVRSubTotaTotal = 0, FVRSubFriTotal = 0, FVRSubSatTotal = 0;
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
                try
                {
                    Session["PDTotal"] = null;
                    Session["GDTotal"] = null;
                    Session["SDTotal"] = null;
                    Session["OtherTotal"] = null;
                    Session["SubTotaTotal"] = null;
                    Session["FPDTotal"] = null;
                    Session["FGDTotal"] = null;
                    Session["FSDTotal"] = null;
                    Session["FOtherTotal"] = null;
                    Session["FSubTotaTotal"] = null;


                    Session["PDVRTotal"] = null;
                    Session["GDVRTotal"] = null;
                    Session["SDVRTotal"] = null;
                    Session["OtherVRTotal"] = null;
                    Session["SubVRTotaTotal"] = null;

                    Session["FVRPDTotal"] = null;
                    Session["FVRGDTotal"] = null;
                    Session["FVRSDTotal"] = null;
                    Session["FVROtherTotal"] = null;
                    Session["FVRSubTotaTotal"] = null;
                    Session["FVRSubFriTotal"] = null;
                    Session["FVRSubSatTotal"] = null;
                    Session["SubVRFriTotal"] = null;
                    Session["SubVRSatTotal"] = null;
                }
                catch { }


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
                DateTime d = DateTime.Today;

                int offset = d.DayOfWeek - DayOfWeek.Monday;

                DateTime lastMonday = d.AddDays(-offset);
                txtStartDate.Text = lastMonday.ToString("MM/dd/yyyy");
                txtEndDate.Text = lastMonday.AddDays(6).ToString("MM/dd/yyyy");

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

                // getdetails("All", 5);

            }
        }
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
    private void getdetails(string CenterCode, int CenterID)
    {
        try
        {
            ViewState["OpenStatus"] = "0";
            DateTime dtstart = Convert.ToDateTime(txtStartDate.Text.ToString());
            DateTime dtEnd = Convert.ToDateTime(txtEndDate.Text.ToString());
            DataSet dsData = objHotLeadBL.GetTotalSalesBycenterByAgentForWeekByDays(CenterID, dtstart, dtEnd);
            DataSet dsDataVerifier = objHotLeadBL.GetTotalSalesBycenterByVerifierForweekDays(CenterID, dtstart, dtEnd);
            

            if (dsData.Tables[0].Rows.Count > 0)
            {
                Rpt_Salescount.DataSource = dsData.Tables[0];
                Rpt_Salescount.DataBind();
                VerifierTotal.Visible = true;
            }

            if (dsDataVerifier.Tables[0].Rows.Count > 0)
            {
                Rpt_Verifierscount.DataSource = dsDataVerifier.Tables[0];
                Rpt_Verifierscount.DataBind();
                VerifierTotal.Visible = true;
            }
            else
            {
                Rpt_Verifierscount.DataSource = null;
                Rpt_Verifierscount.DataBind();
                VerifierTotal.Visible = false;
            }

         


            try
            {
                //Total Submitted Sum
                lblTotalPD.Text = Session["PDTotal"].ToString();
                lblTotalGD.Text = Session["GDTotal"].ToString();
                lblTotalSD.Text = Session["SDTotal"].ToString();
                lblTotalother.Text = Session["OtherTotal"].ToString();
                lblTotalTotal.Text = Session["SubTotaTotal"].ToString();
                lblFri.Text = Session["SubFri"].ToString();
                lblSat.Text = Session["SubSat"].ToString();

                //Total Fully Paid Sum
                lblFTotalPD.Text = Session["FPDTotal"].ToString();
                lblFTotalGD.Text = Session["FGDTotal"].ToString();
                lblFTotalSD.Text = Session["FSDTotal"].ToString();
                lblFTotalother.Text = Session["FOtherTotal"].ToString();
                lblFTotalTotal.Text = Session["FSubTotaTotal"].ToString();
                lblFFrid.Text = Session["FSubFri"].ToString();
                lblFSat.Text = Session["FSubSat"].ToString();
                //Verifier Total Submitted Sum
                lblVRsubtPD.Text = Session["PDVRTotal"].ToString();
                lblVRsubtGD.Text = Session["GDVRTotal"].ToString();
                lblVRsubtSD.Text = Session["SDVRTotal"].ToString();
                lblVRsubtothr.Text = Session["OtherVRTotal"].ToString();
                lblVRsubtottota.Text = Session["SubVRTotaTotal"].ToString();
                lblVRsubtFri.Text = Session["SubVRFriTotal"].ToString();
                lblVRsubtSat.Text = Session["SubVRSatTotal"].ToString();

                //Verifier  Fully Paid Sum
                lblVRFsubtPD.Text = Session["FVRPDTotal"].ToString();
                lblVRFsubtGD.Text = Session["FVRGDTotal"].ToString();
                lblVRFsubtSD.Text = Session["FVRSDTotal"].ToString();
                lblVRFsubtothr.Text = Session["FVROtherTotal"].ToString();
                lblVRFsubtFri.Text = Session["FVRSubFriTotal"].ToString();
                lblVRFsubtsat.Text = Session["FVRSubSatTotal"].ToString();
                lblVRFsubtottota.Text = Session["FVRSubTotaTotal"].ToString();
            }
            catch { }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Rpt_Salescount_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblsubPD = (Label)e.Item.FindControl("lblsubPD");
                Label lblsubGD = (Label)e.Item.FindControl("lblsubGD");
                Label lblsubSD = (Label)e.Item.FindControl("lblsubSD");
                Label lblsubothr = (Label)e.Item.FindControl("lblsubothr");
                Label lbltots = (Label)e.Item.FindControl("lbltots");
                Label lblsubFri = (Label)e.Item.FindControl("lblsubFri");
                Label lblsubSat = (Label)e.Item.FindControl("lblsubSat");




                Label lblpaidPD = (Label)e.Item.FindControl("lblpaidPD");
                Label lblpaidGD = (Label)e.Item.FindControl("lblpaidGD");
                Label lblpaidSD = (Label)e.Item.FindControl("lblpaidSD");
                Label lblpaidothr = (Label)e.Item.FindControl("lblpaidothr");
                Label lblpaidtot = (Label)e.Item.FindControl("lblpaidtot");
                Label lblpaidfri = (Label)e.Item.FindControl("lblpaidfri");
                Label lblpaidsat = (Label)e.Item.FindControl("lblpaidsat");




                Label lblTotalPD = (Label)e.Item.FindControl("lblTotalPD");
                Label lblTotalGD = (Label)e.Item.FindControl("lblTotalGD");
                Label lblTotalSD = (Label)e.Item.FindControl("lblTotalSD");
                Label lblTotalother = (Label)e.Item.FindControl("lblTotalother");
                Label lblTotalTotal = (Label)e.Item.FindControl("lblTotalTotal");




                //Sum   PDTotal = 0, GDTotal = 0, SDTotal = 0, OtherTotal = 0, SubTotaTotal = 0;
                Double PackCost = Convert.ToDouble(lblsubPD.Text.ToString());
                PDTotal += Convert.ToDouble(PackCost);
                Session["PDTotal"] = PDTotal.ToString();

                Double PackCost1 = Convert.ToDouble(lblsubGD.Text.ToString());
                GDTotal += Convert.ToDouble(PackCost1);
                Session["GDTotal"] = GDTotal.ToString();

                Double PackCost2 = Convert.ToDouble(lblsubSD.Text.ToString());
                SDTotal += Convert.ToDouble(PackCost2);
                Session["SDTotal"] = SDTotal.ToString();

                Double PackCost3 = Convert.ToDouble(lblsubothr.Text.ToString());
                OtherTotal += Convert.ToDouble(PackCost3);
                Session["OtherTotal"] = OtherTotal.ToString();

                Double PackCost4 = Convert.ToDouble(lbltots.Text.ToString());
                SubTotaTotal += Convert.ToDouble(PackCost4);
                Session["SubTotaTotal"] = SubTotaTotal.ToString();

                Double PackCost5 = Convert.ToDouble(lblsubFri.Text.ToString());
                SubFri += Convert.ToDouble(PackCost5);
                Session["SubFri"] = SubFri.ToString();


                Double PackCost6 = Convert.ToDouble(lblsubSat.Text.ToString());
                SubSat += Convert.ToDouble(PackCost6);
                Session["SubSat"] = SubSat.ToString();


                //Fully Paid:double FPDTotal = 0, FGDTotal = 0, FSDTotal = 0, FOtherTotal = 0, FSubTotaTotal = 0;
                //lblFTotalPD lblFTotalGD lblFTotalSD lblFTotalother  lblFTotalTotal
                Double FPackCost = Convert.ToDouble(lblpaidPD.Text.ToString());
                FPDTotal += Convert.ToDouble(FPackCost);
                Session["FPDTotal"] = FPDTotal.ToString();

                Double FPackCost1 = Convert.ToDouble(lblpaidGD.Text.ToString());
                FGDTotal += Convert.ToDouble(FPackCost1);
                Session["FGDTotal"] = FGDTotal.ToString();

                Double FPackCost2 = Convert.ToDouble(lblpaidSD.Text.ToString());
                FSDTotal += Convert.ToDouble(FPackCost2);
                Session["FSDTotal"] = FSDTotal.ToString();

                Double FPackCost3 = Convert.ToDouble(lblpaidothr.Text.ToString());
                FOtherTotal += Convert.ToDouble(FPackCost3);
                Session["FOtherTotal"] = FOtherTotal.ToString();

                Double FPackCost4 = Convert.ToDouble(lblpaidtot.Text.ToString());
                FSubTotaTotal += Convert.ToDouble(FPackCost4);
                Session["FSubTotaTotal"] = FSubTotaTotal.ToString();

                Double FPackCost5 = Convert.ToDouble(lblpaidfri.Text.ToString());
                FSubFri += Convert.ToDouble(FPackCost5);
                Session["FSubFri"] = FSubFri.ToString();

                Double FPackCost6 = Convert.ToDouble(lblpaidsat.Text.ToString());
                FSubSat += Convert.ToDouble(FPackCost6);
                Session["FSubSat"] = FSubSat.ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    protected void Rpt_Verifierscount_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataSet dsTasks3 = (DataSet)Session["AllBulkDataData"];
                Label lblVRsubPD = (Label)e.Item.FindControl("lblVRsubPD");
                Label lblVRsubGD = (Label)e.Item.FindControl("lblVRsubGD");
                Label lblVRsubSD = (Label)e.Item.FindControl("lblVRsubSD");
                Label lblVRsubothr = (Label)e.Item.FindControl("lblVRsubothr");
                Label lblVRsubFri = (Label)e.Item.FindControl("lblVRsubFri");
                Label lblVRsubSat = (Label)e.Item.FindControl("lblVRsubSat");
                Label lblVRtots = (Label)e.Item.FindControl("lblVRtots");


                Label lblVRpaidPD = (Label)e.Item.FindControl("lblVRpaidPD");
                Label lblVRpaidGD = (Label)e.Item.FindControl("lblVRpaidGD");
                Label lblVRpaidSD = (Label)e.Item.FindControl("lblVRpaidSD");
                Label lblVRpaidothr = (Label)e.Item.FindControl("lblVRpaidothr");
                Label lblVRpaidFri = (Label)e.Item.FindControl("lblVRpaidFri");
                Label lblVRpaidSat = (Label)e.Item.FindControl("lblVRpaidSat");
                Label lblVRpaidtot = (Label)e.Item.FindControl("lblVRpaidtot");


                //Sum   PDTotal = 0, GDTotal = 0, SDTotal = 0, OtherTotal = 0, SubTotaTotal = 0;
                Double PackCost = Convert.ToDouble(lblVRsubPD.Text.ToString());
                PDVRTotal += Convert.ToDouble(PackCost);
                Session["PDVRTotal"] = PDVRTotal.ToString();

                Double PackCost1 = Convert.ToDouble(lblVRsubGD.Text.ToString());
                GDVRTotal += Convert.ToDouble(PackCost1);
                Session["GDVRTotal"] = GDVRTotal.ToString();

                Double PackCost2 = Convert.ToDouble(lblVRsubSD.Text.ToString());
                SDVRTotal += Convert.ToDouble(PackCost2);
                Session["SDVRTotal"] = SDVRTotal.ToString();

                Double PackCost3 = Convert.ToDouble(lblVRsubothr.Text.ToString());
                OtherVRTotal += Convert.ToDouble(PackCost3);
                Session["OtherVRTotal"] = OtherVRTotal.ToString();



                Double PackCost4 = Convert.ToDouble(lblVRtots.Text.ToString());
                SubVRTotaTotal += Convert.ToDouble(PackCost4);
                Session["SubVRTotaTotal"] = SubVRTotaTotal.ToString();

                Double PackCost5 = Convert.ToDouble(lblVRsubFri.Text.ToString());
                SubVRFriTotal += Convert.ToDouble(PackCost5);
                Session["SubVRFriTotal"] = SubVRFriTotal.ToString();


                Double PackCost6 = Convert.ToDouble(lblVRsubSat.Text.ToString());
                SubVRSatTotal += Convert.ToDouble(PackCost6);
                Session["SubVRSatTotal"] = SubVRSatTotal.ToString();


                //Fully Paid:double FPDTotal = 0, FGDTotal = 0, FSDTotal = 0, FOtherTotal = 0, FSubTotaTotal = 0;
                //lblFTotalPD lblFTotalGD lblFTotalSD lblFTotalother  lblFTotalTotal
                Double FPackCost = Convert.ToDouble(lblVRpaidPD.Text.ToString());
                FVRPDTotal += Convert.ToDouble(FPackCost);
                Session["FVRPDTotal"] = FVRPDTotal.ToString();

                Double FPackCost1 = Convert.ToDouble(lblVRpaidGD.Text.ToString());
                FVRGDTotal += Convert.ToDouble(FPackCost1);
                Session["FVRGDTotal"] = FVRGDTotal.ToString();

                Double FPackCost2 = Convert.ToDouble(lblVRpaidSD.Text.ToString());
                FVRSDTotal += Convert.ToDouble(FPackCost2);
                Session["FVRSDTotal"] = FVRSDTotal.ToString();

                Double FPackCost3 = Convert.ToDouble(lblVRpaidothr.Text.ToString());
                FVROtherTotal += Convert.ToDouble(FPackCost3);
                Session["FVROtherTotal"] = FVROtherTotal.ToString();

                Double FPackCost4 = Convert.ToDouble(lblVRpaidtot.Text.ToString());
                FVRSubTotaTotal += Convert.ToDouble(FPackCost4);
                Session["FVRSubTotaTotal"] = FVRSubTotaTotal.ToString();


                Double FPackCost5 = Convert.ToDouble(lblVRpaidFri.Text.ToString());
                FVRSubFriTotal += Convert.ToDouble(FPackCost5);
                Session["FVRSubFriTotal"] = FVRSubFriTotal.ToString();


                Double FPackCost6 = Convert.ToDouble(lblVRpaidSat.Text.ToString());
                FVRSubSatTotal += Convert.ToDouble(FPackCost6);
                Session["FVRSubSatTotal"] = FVRSubSatTotal.ToString();

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
        dtreportTime.Text = DateTime.Now.ToString();
        try
        {
            Session["PDTotal"] = null;
            Session["GDTotal"] = null;
            Session["SDTotal"] = null;
            Session["OtherTotal"] = null;
            Session["SubTotaTotal"] = null;
            Session["FPDTotal"] = null;
            Session["FGDTotal"] = null;
            Session["FSDTotal"] = null;
            Session["FOtherTotal"] = null;
            Session["FSubTotaTotal"] = null;


            Session["PDVRTotal"] = null;
            Session["GDVRTotal"] = null;
            Session["SDVRTotal"] = null;
            Session["OtherVRTotal"] = null;
            Session["SubVRTotaTotal"] = null;

            Session["FVRPDTotal"] = null;
            Session["FVRGDTotal"] = null;
            Session["FVRSDTotal"] = null;
            Session["FVROtherTotal"] = null;
            Session["FVRSubTotaTotal"] = null;
            Session["FVRSubFriTotal"] = null;
            Session["FVRSubSatTotal"] = null;
            Session["SubVRFriTotal"] = null;
            Session["SubVRSatTotal"] = null;
        }
        catch { }
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
