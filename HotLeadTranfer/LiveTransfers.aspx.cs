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

public partial class LiveTransfers : System.Web.UI.Page
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
            ServiceReference objServiceReference = new ServiceReference();

            ScriptReference objScriptReference = new ScriptReference();

            objServiceReference.Path = "~/WebService.asmx";

            objScriptReference.Path = "~/Static/Js/CarsJScript.js";

            scrptmgr.Services.Add(objServiceReference);
            scrptmgr.Scripts.Add(objScriptReference);

            if (!IsPostBack)
            {
                Session["CurrentPage"] = "Transfers";

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
                        Session["selectIndexLive"] = null;
                        Session["selectIndexCallback"] = null;
                        DataSet dsdata = objHotLeadBL.GetLiveTransfersData();
                        DataSet dsAgents = objHotLeadBL.GetLoginAgentCount();
                        if (dsAgents.Tables[0].Rows[0]["CountAll"].ToString() == "0")
                        {
                            lblAgentsCount.Text = "Agent(s) are not available";
                        }
                        else
                        {
                            lblAgentsCount.Text = dsAgents.Tables[0].Rows[0]["CountAll"].ToString() + " Agent(s) are available";
                            DataSet dsTransferAgents = objHotLeadBL.GetAllLoginAgentDetails();
                            if (dsTransferAgents.Tables[0].Rows.Count > 0)
                            {
                                grdTest.DataSource = dsTransferAgents.Tables[0];
                                grdTest.DataBind();
                            }
                        }
                        lblCountScrolls.Text = dsdata.Tables[0].Rows.Count.ToString() + " Transfers";
                        LiveTransferRepeater.DataSource = dsdata;
                        LiveTransferRepeater.DataBind();
                        DataSet dsCBData = objHotLeadBL.GetCallbacksTransfersData();
                        lblCountCallbacks.Text = dsCBData.Tables[0].Rows.Count.ToString() + " Callbacks";
                        repeaterCallback.DataSource = dsCBData;
                        repeaterCallback.DataBind();
                        DataSet dsUpdateID = objHotLeadBL.UpdateUserLogDataTakenField(Convert.ToInt32(Session[Constants.USER_ID].ToString()));
                        Session["LiveTransferPostingID"] = "";
                        Session["PageOpen"] = 0;

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


    protected void lnkbtnIntromail_Click(object sender, EventArgs e)
    {
        try
        {
            Session["TransRedirect"] = "TransRedirect";
            Response.Redirect("Intromail.aspx");
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
            Session["TransRedirect"] = "TransRedirect";
            Response.Redirect("NewSale.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnAgentReports_Click(object sender, EventArgs e)
    {
        try
        {
            Session["TransRedirect"] = "TransRedirect";
            Response.Redirect("AgentReport.aspx");
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
            if (hdnItemSelect.Value == "1")
            {
                Tabselect.Value = "1";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CheckAlert();", true);
                //ClientScript.RegisterStartupScript(typeof(Page), "KyAUIDFCS", "<script language='javascript' type='text/javascript'>CheckAlert();</script>");
                mdepAlertExists.Show();
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "Please dispose the current car";
            }
            else
            {
                if (Session["LiveTransferPostingID"].ToString() != "")
                {
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                }
                HotLeadsBL objHotLeadsBL = new HotLeadsBL();
                DataSet dsDatetime = objHotLeadBL.GetDatetime();
                DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
                objHotLeadsBL.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), dtNow, Convert.ToInt32(Session[Constants.USERLOG_ID]), 2);
                Session.Abandon();
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void LiveTransferRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkbtnRepPhoneNumber = (LinkButton)e.Item.FindControl("lnkbtnRepPhoneNumber");
                HiddenField hdnRepPhoneNumber = (HiddenField)e.Item.FindControl("hdnRepPhoneNumber");
                Label lblRepSellerName = (Label)e.Item.FindControl("lblRepSellerName");
                HiddenField hdnRepSellerName = (HiddenField)e.Item.FindControl("hdnRepSellerName");
                Label lblRepLastName = (Label)e.Item.FindControl("lblRepLastName");
                HiddenField hdnRepLastName = (HiddenField)e.Item.FindControl("hdnRepLastName");
                Label lblRepYear = (Label)e.Item.FindControl("lblRepYear");
                HiddenField hdnRepYear = (HiddenField)e.Item.FindControl("hdnRepYear");
                Label lblRepPrice = (Label)e.Item.FindControl("lblRepPrice");
                HiddenField hdnRepPrice = (HiddenField)e.Item.FindControl("hdnRepPrice");
                Label lblRepCenterCode = (Label)e.Item.FindControl("lblRepCenterCode");
                HiddenField hdnRepTrandatetime = (HiddenField)e.Item.FindControl("hdnRepTrandatetime");
                Label lblRepTransDate = (Label)e.Item.FindControl("lblRepTransDate");
                HiddenField hdnRepCenterCode = (HiddenField)e.Item.FindControl("hdnRepCenterCode");
                Label lblRepAgent = (Label)e.Item.FindControl("lblRepAgent");
                HiddenField hdnRepAgent = (HiddenField)e.Item.FindControl("hdnRepAgent");
                Image ImgLockImage = (Image)e.Item.FindControl("ImgLockImage");
                HiddenField hdnLockStatus = (HiddenField)e.Item.FindControl("hdnLockStatus");


                lnkbtnRepPhoneNumber.Text = objGeneralFunc.filPhnm(hdnRepPhoneNumber.Value);
                if (hdnRepSellerName.Value.Trim().ToString() != "")
                {
                    if (hdnRepSellerName.Value.Trim().Length > 10)
                    {
                        lblRepSellerName.Text = objGeneralFunc.WrapTextByMaxCharacters(hdnRepSellerName.Value.ToString(), 10);
                    }
                    else
                    {
                        lblRepSellerName.Text = hdnRepSellerName.Value.ToString();
                    }
                }
                else
                {
                    lblRepSellerName.Text = "Unspecified";
                }
                if (hdnRepLastName.Value.Trim().ToString() != "")
                {
                    if (hdnRepLastName.Value.Trim().Length > 10)
                    {
                        lblRepLastName.Text = objGeneralFunc.WrapTextByMaxCharacters(hdnRepLastName.Value.ToString(), 10);
                    }
                    else
                    {
                        lblRepLastName.Text = hdnRepLastName.Value.ToString();
                    }
                }
                else
                {
                    lblRepLastName.Text = "Unspecified";
                }

                if (hdnRepYear.Value != "0")
                {
                    lblRepYear.Text = hdnRepYear.Value.ToString();
                }
                else
                {
                    lblRepYear.Text = "Unspecified";
                }
                string Price = string.Empty;
                if (hdnRepPrice.Value != "0.0000")
                {
                    Price = "$" + string.Format("{0:0}", Convert.ToDouble(hdnRepPrice.Value));
                }
                else
                {
                    Price = "Unspecified";
                }
                lblRepPrice.Text = Price;

                lblRepCenterCode.Text = hdnRepCenterCode.Value.ToString();

                if (hdnRepAgent.Value.ToString() != "")
                {
                    lblRepAgent.Text = hdnRepAgent.Value.ToString();
                }
                else
                {
                    lblRepAgent.Text = "Unspecified";
                }
                if (hdnRepTrandatetime.Value != "")
                {
                    DateTime Transferdate = Convert.ToDateTime(hdnRepTrandatetime.Value);
                    lblRepTransDate.Text = Transferdate.ToString("MM/dd/yyyy hh:mm tt");
                }
                else
                {
                    lblRepTransDate.Text = "Unspecified";
                }
                if ((Session["selectIndexLive"] == null) || (Session["selectIndexLive"].ToString() == ""))
                {
                    lnkbtnRepPhoneNumber.ForeColor = System.Drawing.Color.Blue;
                    HtmlControl divRepeaterLive = (HtmlGenericControl)e.Item.FindControl("divRepeaterLive");
                    divRepeaterLive.Attributes.Remove("Style");
                }
                else
                {
                    if (Session["selectIndexLive"].ToString() == e.Item.ItemIndex.ToString())
                    {
                        lnkbtnRepPhoneNumber.ForeColor = System.Drawing.Color.Red;
                        //string Div = "LiveTransferRepeater_ctl0" + e.Item.ItemIndex.ToString() + "_divRepeaterLive";
                        //HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("divRepeaterLive");
                        HtmlControl divRepeaterLive = (HtmlGenericControl)e.Item.FindControl("divRepeaterLive");
                        divRepeaterLive.Attributes.Add("Style", "background-color:#dbdbdb !important;");
                        ImgLockImage.Attributes.Add("src", "images/Lock-icon.png");
                    }
                }
                if (hdnLockStatus.Value == "1")
                {
                    ImgLockImage.ImageUrl = "~/images/Lock-icon.png";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void LiveTransferRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "GetData")
            {
                if (hdnItemSelect.Value == "1")
                {
                    Tabselect.Value = "1";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CheckAlert();", true);
                    //ClientScript.RegisterStartupScript(typeof(Page), "KyAUIDFCS", "<script language='javascript' type='text/javascript'>CheckAlert();</script>");
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Please dispose the current car";
                }
                else
                {

                    if (Session["LiveTransferPostingID"].ToString() != "")
                    {
                        int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                        int IsLockedOld = 0;
                        DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    }
                    LinkButton lnkbtnRepPhoneNumberSel = (LinkButton)e.Item.FindControl("lnkbtnRepPhoneNumber");
                    Image ImgLockImage = (Image)e.Item.FindControl("ImgLockImage");
                    if (Session["selectIndexLive"] != null)
                    {
                        int OldIndexForImg = Convert.ToInt32(Session["selectIndexLive"].ToString());
                        Image ImgLockImageOld = (Image)LiveTransferRepeater.Items[OldIndexForImg].FindControl("ImgLockImage");
                        ImgLockImageOld.Attributes.Add("src", "");
                    }

                    if (Session["selectIndexCallback"] != null)
                    {
                        int CallbackIndexForImg = Convert.ToInt32(Session["selectIndexCallback"].ToString());
                        Image ImgLockImageOldCB = (Image)repeaterCallback.Items[CallbackIndexForImg].FindControl("ImgLockImageCB");
                        ImgLockImageOldCB.Attributes.Add("src", "");
                        HtmlControl divRepeaterCB = (HtmlGenericControl)repeaterCallback.Items[CallbackIndexForImg].FindControl("divRepeaterCallback");
                        divRepeaterCB.Attributes.Remove("Style");
                        LinkButton lnkbtnRepPhoneNumberCB = (LinkButton)repeaterCallback.Items[CallbackIndexForImg].FindControl("lnkbtnRepPhoneNumberCB");
                        lnkbtnRepPhoneNumberCB.ForeColor = System.Drawing.Color.Blue;
                    }
                    for (int i = 0; i < LiveTransferRepeater.Items.Count; i++)
                    {
                        LinkButton lnkbtnRepPhoneNumber = (LinkButton)LiveTransferRepeater.Items[i].FindControl("lnkbtnRepPhoneNumber");
                        lnkbtnRepPhoneNumber.ForeColor = System.Drawing.Color.Blue;
                        HtmlControl divRepeaterLive1 = (HtmlGenericControl)LiveTransferRepeater.Items[i].FindControl("divRepeaterLive");
                        divRepeaterLive1.Attributes.Remove("Style");
                    }

                    int PostingID = Convert.ToInt32(e.CommandArgument);
                    DataSet dsStatus = objHotLeadBL.Get_Customer_LockStatus(PostingID);
                    if (dsStatus.Tables[0].Rows[0]["Column1"].ToString() == "UnLocked")
                    {
                        Session["LiveTransferPostingID"] = PostingID;
                        lnkbtnRepPhoneNumberSel.ForeColor = System.Drawing.Color.Red;
                        HtmlControl divRepeaterLive = (HtmlGenericControl)e.Item.FindControl("divRepeaterLive");
                        divRepeaterLive.Attributes.Add("Style", "background-color:#dbdbdb !important;");
                        ImgLockImage.Attributes.Add("src", "images/Lock-icon.png");
                        //ImgLockImage.ImageUrl = "~/images/Lock-icon.png";
                        //ImgLockImage.Attributes.Add("style", "display:block;");
                        Session["selectIndexLive"] = e.Item.ItemIndex;
                        int IsLocked = 1;
                        DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingID, IsLocked);
                        tdDataDetails.Style["display"] = "block";
                        FillData();
                        hdnItemSelect.Value = "1";
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
                    }
                    else
                    {
                        if (Session["selectIndexLive"] != null)
                        {
                            int OldIndex = Convert.ToInt32(Session["selectIndexLive"].ToString());
                            LinkButton lnkbtnPhoOld = (LinkButton)LiveTransferRepeater.Items[OldIndex].FindControl("lnkbtnRepPhoneNumber");
                            lnkbtnPhoOld.ForeColor = System.Drawing.Color.Red;
                            HtmlControl divRepeaterLive = (HtmlGenericControl)LiveTransferRepeater.Items[OldIndex].FindControl("divRepeaterLive");
                            divRepeaterLive.Attributes.Add("Style", "background-color:#dbdbdb !important;");
                            Image ImgLockImageOld = (Image)LiveTransferRepeater.Items[OldIndex].FindControl("ImgLockImage");
                            ImgLockImageOld.Attributes.Add("src", "images/Lock-icon.png");
                        }
                        mpealteruser.Show();
                        lblErr.Visible = true;
                        lblErr.Text = "CarID locked by the another user you cannot edit the details";
                    }
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void repeaterCallback_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkbtnRepPhoneNumber = (LinkButton)e.Item.FindControl("lnkbtnRepPhoneNumberCB");
                HiddenField hdnRepPhoneNumber = (HiddenField)e.Item.FindControl("hdnRepPhoneNumberCB");
                Label lblRepSellerName = (Label)e.Item.FindControl("lblRepSellerNameCB");
                HiddenField hdnRepSellerName = (HiddenField)e.Item.FindControl("hdnRepSellerNameCB");
                Label lblRepLastName = (Label)e.Item.FindControl("lblRepLastNameCB");
                HiddenField hdnRepLastName = (HiddenField)e.Item.FindControl("hdnRepLastNameCB");
                Label lblRepYear = (Label)e.Item.FindControl("lblRepYearCB");
                HiddenField hdnRepYear = (HiddenField)e.Item.FindControl("hdnRepYearCB");
                Label lblRepPrice = (Label)e.Item.FindControl("lblRepPriceCB");
                HiddenField hdnRepPrice = (HiddenField)e.Item.FindControl("hdnRepPriceCB");
                Label lblRepCenterCode = (Label)e.Item.FindControl("lblRepCenterCodeCB");
                HiddenField hdnRepTrandatetime = (HiddenField)e.Item.FindControl("hdnRepTrandatetimeCB");
                Label lblRepTransDate = (Label)e.Item.FindControl("lblRepTransDateCB");
                HiddenField hdnRepCenterCode = (HiddenField)e.Item.FindControl("hdnRepCenterCodeCB");
                Label lblRepAgent = (Label)e.Item.FindControl("lblRepAgentCB");
                HiddenField hdnRepAgent = (HiddenField)e.Item.FindControl("hdnRepAgentCB");
                Image ImgLockImage = (Image)e.Item.FindControl("ImgLockImageCB");
                HiddenField hdnLockStatus = (HiddenField)e.Item.FindControl("hdnLockStatusCB");
                Label lblVeriedByCB = (Label)e.Item.FindControl("lblVeriedByCB");
                Label lblCallbackTimeCB = (Label)e.Item.FindControl("lblCallbackTimeCB");
                HiddenField hdnVeriedByCB = (HiddenField)e.Item.FindControl("hdnVeriedByCB");
                HiddenField hdnCallbackTimeCB = (HiddenField)e.Item.FindControl("hdnCallbackTimeCB");

                lnkbtnRepPhoneNumber.Text = objGeneralFunc.filPhnm(hdnRepPhoneNumber.Value);
                if (hdnRepSellerName.Value.Trim().ToString() != "")
                {
                    if (hdnRepSellerName.Value.Trim().Length > 10)
                    {
                        lblRepSellerName.Text = objGeneralFunc.WrapTextByMaxCharacters(hdnRepSellerName.Value.ToString(), 10);
                    }
                    else
                    {
                        lblRepSellerName.Text = hdnRepSellerName.Value.ToString();
                    }
                }
                else
                {
                    lblRepSellerName.Text = "Unspecified";
                }
                if (hdnRepLastName.Value.Trim().ToString() != "")
                {
                    if (hdnRepLastName.Value.Trim().Length > 10)
                    {
                        lblRepLastName.Text = objGeneralFunc.WrapTextByMaxCharacters(hdnRepLastName.Value.ToString(), 10);
                    }
                    else
                    {
                        lblRepLastName.Text = hdnRepLastName.Value.ToString();
                    }
                }
                else
                {
                    lblRepLastName.Text = "Unspecified";
                }

                if (hdnRepYear.Value != "0")
                {
                    lblRepYear.Text = hdnRepYear.Value.ToString();
                }
                else
                {
                    lblRepYear.Text = "Unspecified";
                }
                string Price = string.Empty;
                if (hdnRepPrice.Value != "0.0000")
                {
                    Price = "$" + string.Format("{0:0}", Convert.ToDouble(hdnRepPrice.Value));
                }
                else
                {
                    Price = "Unspecified";
                }
                lblRepPrice.Text = Price;

                lblRepCenterCode.Text = hdnRepCenterCode.Value.ToString();

                if (hdnRepAgent.Value.ToString() != "")
                {
                    lblRepAgent.Text = hdnRepAgent.Value.ToString();
                }
                else
                {
                    lblRepAgent.Text = "Unspecified";
                }
                if (hdnRepTrandatetime.Value != "")
                {
                    DateTime Transferdate = Convert.ToDateTime(hdnRepTrandatetime.Value);
                    lblRepTransDate.Text = Transferdate.ToString("MM/dd/yyyy hh:mm tt");
                }
                else
                {
                    lblRepTransDate.Text = "Unspecified";
                }
                if ((Session["selectIndexCallback"] == null) || (Session["selectIndexCallback"].ToString() == ""))
                {
                    lnkbtnRepPhoneNumber.ForeColor = System.Drawing.Color.Blue;
                    HtmlControl divRepeaterCallback = (HtmlGenericControl)e.Item.FindControl("divRepeaterCallback");
                    divRepeaterCallback.Attributes.Remove("Style");
                }
                else
                {
                    if (Session["selectIndexCallback"].ToString() == e.Item.ItemIndex.ToString())
                    {
                        lnkbtnRepPhoneNumber.ForeColor = System.Drawing.Color.Red;
                        //string Div = "LiveTransferRepeater_ctl0" + e.Item.ItemIndex.ToString() + "_divRepeaterLive";
                        //HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("divRepeaterLive");
                        HtmlControl divRepeaterCallback = (HtmlGenericControl)e.Item.FindControl("divRepeaterCallback");
                        divRepeaterCallback.Attributes.Add("Style", "background-color:#dbdbdb !important;");
                        ImgLockImage.Attributes.Add("src", "images/Lock-icon.png");
                    }
                }
                if (hdnLockStatus.Value == "1")
                {
                    ImgLockImage.ImageUrl = "~/images/Lock-icon.png";
                }
                if (hdnVeriedByCB.Value.ToString() != "")
                {
                    lblVeriedByCB.Text = hdnVeriedByCB.Value.ToString();
                }
                else
                {
                    lblVeriedByCB.Text = "Unspecified";
                }
                if (hdnCallbackTimeCB.Value != "")
                {
                    DateTime Calbackdate = Convert.ToDateTime(hdnCallbackTimeCB.Value);
                    lblCallbackTimeCB.Text = Calbackdate.ToString("MM/dd/yyyy hh:mm tt");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void repeaterCallback_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "GetData")
            {
                if (hdnItemSelect.Value == "1")
                {
                    Tabselect.Value = "1";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CheckAlert();", true);
                    //ClientScript.RegisterStartupScript(typeof(Page), "KyAUIDFCS", "<script language='javascript' type='text/javascript'>CheckAlert();</script>");
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Please dispose the current car";
                }
                else
                {

                    if (Session["LiveTransferPostingID"].ToString() != "")
                    {
                        int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                        int IsLockedOld = 0;
                        DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    }
                    LinkButton lnkbtnRepPhoneNumberSel = (LinkButton)e.Item.FindControl("lnkbtnRepPhoneNumberCB");
                    Image ImgLockImage = (Image)e.Item.FindControl("ImgLockImageCB");
                    if (Session["selectIndexCallback"] != null)
                    {
                        int OldIndexForImg = Convert.ToInt32(Session["selectIndexCallback"].ToString());
                        Image ImgLockImageOld = (Image)repeaterCallback.Items[OldIndexForImg].FindControl("ImgLockImageCB");
                        ImgLockImageOld.Attributes.Add("src", "");
                    }
                    if (Session["selectIndexLive"] != null)
                    {
                        int OldIndexForImgLive = Convert.ToInt32(Session["selectIndexLive"].ToString());
                        Image ImgLockImageOldLive = (Image)LiveTransferRepeater.Items[OldIndexForImgLive].FindControl("ImgLockImage");
                        ImgLockImageOldLive.Attributes.Add("src", "");
                        LinkButton lnkbtnRepPhoneNumberLive = (LinkButton)LiveTransferRepeater.Items[OldIndexForImgLive].FindControl("lnkbtnRepPhoneNumber");
                        lnkbtnRepPhoneNumberLive.ForeColor = System.Drawing.Color.Blue;
                        HtmlControl divRepeaterLive = (HtmlGenericControl)LiveTransferRepeater.Items[OldIndexForImgLive].FindControl("divRepeaterLive");
                        divRepeaterLive.Attributes.Remove("Style");
                    }

                    for (int i = 0; i < repeaterCallback.Items.Count; i++)
                    {
                        LinkButton lnkbtnRepPhoneNumber = (LinkButton)repeaterCallback.Items[i].FindControl("lnkbtnRepPhoneNumberCB");
                        lnkbtnRepPhoneNumber.ForeColor = System.Drawing.Color.Blue;
                        HtmlControl divRepeaterLive1 = (HtmlGenericControl)repeaterCallback.Items[i].FindControl("divRepeaterCallback");
                        divRepeaterLive1.Attributes.Remove("Style");
                    }

                    int PostingID = Convert.ToInt32(e.CommandArgument);
                    DataSet dsStatus = objHotLeadBL.Get_Customer_LockStatus(PostingID);
                    if (dsStatus.Tables[0].Rows[0]["Column1"].ToString() == "UnLocked")
                    {
                        Session["LiveTransferPostingID"] = PostingID;
                        lnkbtnRepPhoneNumberSel.ForeColor = System.Drawing.Color.Red;
                        HtmlControl divRepeaterLive = (HtmlGenericControl)e.Item.FindControl("divRepeaterCallback");
                        divRepeaterLive.Attributes.Add("Style", "background-color:#dbdbdb !important;");
                        ImgLockImage.Attributes.Add("src", "images/Lock-icon.png");
                        //ImgLockImage.ImageUrl = "~/images/Lock-icon.png";
                        //ImgLockImage.Attributes.Add("style", "display:block;");
                        Session["selectIndexCallback"] = e.Item.ItemIndex;
                        int IsLocked = 1;
                        DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingID, IsLocked);
                        tdDataDetails.Style["display"] = "block";
                        FillData();
                        hdnItemSelect.Value = "1";
                        Session["PageOpen"] = 1;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
                    }
                    else
                    {
                        if (Session["selectIndexCallback"] != null)
                        {
                            int OldIndex = Convert.ToInt32(Session["selectIndexCallback"].ToString());
                            LinkButton lnkbtnPhoOld = (LinkButton)LiveTransferRepeater.Items[OldIndex].FindControl("lnkbtnRepPhoneNumberCB");
                            lnkbtnPhoOld.ForeColor = System.Drawing.Color.Red;
                            HtmlControl divRepeaterLive = (HtmlGenericControl)LiveTransferRepeater.Items[OldIndex].FindControl("divRepeaterCallback");
                            divRepeaterLive.Attributes.Add("Style", "background-color:#dbdbdb !important;");
                            Image ImgLockImageOld = (Image)LiveTransferRepeater.Items[OldIndex].FindControl("ImgLockImageCB");
                            ImgLockImageOld.Attributes.Add("src", "images/Lock-icon.png");
                        }
                        mpealteruser.Show();
                        lblErr.Visible = true;
                        lblErr.Text = "CarID locked by the another user you cannot edit the details";
                    }
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void IntervalTimer_Tick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Session["PageOpen"].ToString() == "1")
    //        {
    //            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
    //        }
    //        else
    //        {
    //            DataSet dsUpdateCheck = objHotLeadBL.CheckForUpdatesData(Convert.ToInt32(Session[Constants.USER_ID].ToString()));
    //            if (dsUpdateCheck.Tables[0].Rows[0]["UpdateID"].ToString() == "1")
    //            {
    //                DataSet dsdata = objHotLeadBL.GetLiveTransfersData();

    //                lblCountScrolls.Text = dsdata.Tables[0].Rows.Count.ToString() + " Transfers";
    //                LiveTransferRepeater.DataSource = dsdata;
    //                LiveTransferRepeater.DataBind();
    //                DataSet dsCBData = objHotLeadBL.GetCallbacksTransfersData();
    //                lblCountCallbacks.Text = dsCBData.Tables[0].Rows.Count.ToString() + " Callbacks";
    //                repeaterCallback.DataSource = dsCBData;
    //                repeaterCallback.DataBind();
    //                DataSet dsUpdateID = objHotLeadBL.UpdateUserLogDataTakenField(Convert.ToInt32(Session[Constants.USER_ID].ToString()));
    //                Tabselect.Value = "1";
    //                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
    //            }
    //            else
    //            {
    //                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    private void FillData()
    {
        try
        {
            if (Session["DsDropDown"] == null)
            {
                dsDropDown = objdropdownBL.Usp_Get_DropDown();
                Session["DsDropDown"] = dsDropDown;
            }
            else
            {
                dsDropDown = (DataSet)Session["DsDropDown"];
            }
            Session["NewSaleCarID"] = null;
            Session["NewSaleUID"] = null;
            Session["NewSalePostingID"] = null;
            Session["NewSaleUserPackID"] = null;
            Session["NewSaleSellerID"] = null;
            Session["NewSalePSID1"] = null;
            Session["NewSalePSID2"] = null;
            Session["NewSalePaymentID"] = null;

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
            FillVoiceFileLocation();
            //FillPaymentDate();
            FillPDDate();
            FillBillingStates();
            FillPhotoSource();
            FillDescriptionSource();
            // FillCheckTypes();
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            txtPaymentDate.Text = dtNow.ToString("MM/dd/yyyy");
            if ((Session["LiveTransferPostingID"] != null) && (Session["LiveTransferPostingID"].ToString() != ""))
            {
                int PostingID = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(PostingID);
                lblSaleAgentTran.Text = objGeneralFunc.WrapTextByMaxCharacters(Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString(), 10);
                lblSaleCenterTran.Text = Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString();
                if (Cardetais.Tables[0].Rows[0]["TransferDate"].ToString() != "")
                {
                    DateTime Transferdate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["TransferDate"].ToString());
                    lblTransferDateTran.Text = Transferdate.ToString("MM/dd/yyyy hh:mm tt");
                }


                Double PackCost2 = new Double();
                PackCost2 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString());
                string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString();
                string PackName2 = Cardetais.Tables[0].Rows[0]["Description"].ToString();
                ListItem listPack = new ListItem();
                listPack.Value = Cardetais.Tables[0].Rows[0]["PackageID"].ToString();
                listPack.Text = PackName2 + " ($" + PackAmount2 + ")";
                ddlPackage.SelectedIndex = ddlPackage.Items.IndexOf(listPack);

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
                Session["oldintNotestrans"] = OldNotes;
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

                Session["NewSaleCarID"] = Cardetais.Tables[0].Rows[0]["carid"].ToString();
                Session["NewSaleUID"] = Cardetais.Tables[0].Rows[0]["uid"].ToString();
                Session["NewSalePostingID"] = Cardetais.Tables[0].Rows[0]["postingID"].ToString();
                Session["NewSaleUserPackID"] = Cardetais.Tables[0].Rows[0]["UserPackID"].ToString();
                Session["NewSaleSellerID"] = Cardetais.Tables[0].Rows[0]["sellerID"].ToString();
                Session["NewSalePSID1"] = Cardetais.Tables[0].Rows[0]["PSID1"].ToString();
                if (Cardetais.Tables[0].Rows[0]["PSID2"].ToString() != "")
                {
                    Session["NewSalePSID2"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PSID2"].ToString());
                }
                if (Cardetais.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                {
                    Session["NewSalePaymentID"] = Convert.ToInt32(Cardetais.Tables[0].Rows[0]["PaymentID"].ToString());
                }
            }
            //Page.ClientScript.RegisterClientScriptBlock(tyPage, "script", "TabsScripting()");
            //ClientScript.RegisterStartupScript(typeof(Page), "KyAUIDFCS", "<script language='javascript' type='text/javascript'>TabsScripting();</script>");
            Tabselect.Value = "0";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
    private void FillYear()
    {
        try
        {
            ddlYear.Items.Clear();
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
            ddlLocationState.Items.Clear();
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
    private void FillVoiceFileLocation()
    {
        try
        {
            ddlVoiceFileLocation.Items.Clear();
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
    private void FillPackage()
    {
        try
        {
            ddlPackage.Items.Clear();
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
            ddlInteriorColor.Items.Clear();
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
            Session.Timeout = 180;
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
            int DispositionID = Convert.ToInt32(0);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(SellerPhone);
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
                mdepAlertExists.Show();
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "Phone " + txtPhone.Text + " already exists.<br />Please change phone # to save";
            }
            else
            {
                hdnItemSelect.Value = "0";
                DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(SellerPhone);
                if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                {
                    Session["NewSaleCarID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["carid"].ToString());
                    Session["NewSaleUID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["uid"].ToString());
                    Session["NewSalePostingID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString());
                    Session["NewSaleUserPackID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["UserPackID"].ToString());
                    Session["NewSaleSellerID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["sellerID"].ToString());
                    Session["NewSalePSID1"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID1"].ToString());
                    if (dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                    {
                        Session["NewSalePSID2"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString());
                    }
                    if (dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                    {
                        Session["NewSalePaymentID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString());
                    }
                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    mpealteruserUpdated.Show();
                    lblErrUpdated.Visible = true;
                    lblErrUpdated.Text = "Customer details saved successfully";
                }
                else
                {
                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    mpealteruserUpdated.Show();
                    lblErrUpdated.Visible = true;
                    lblErrUpdated.Text = "Customer details saved successfully";
                }
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
            Session["AbandonSalePostingID"] = null;
            tdDataDetails.Style["display"] = "none";
            hdnItemSelect.Value = "0";
            int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
            int IsLockedOld = 0;
            Session["selectIndexLive"] = null;
            Session["selectIndexCallback"] = null;
            DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);

            DataSet dsdata = objHotLeadBL.GetLiveTransfersData();

            lblCountScrolls.Text = dsdata.Tables[0].Rows.Count.ToString() + " Transfers";
            LiveTransferRepeater.DataSource = dsdata;
            LiveTransferRepeater.DataBind();
            DataSet dsCBData = objHotLeadBL.GetCallbacksTransfersData();
            lblCountCallbacks.Text = dsCBData.Tables[0].Rows.Count.ToString() + " Callbacks";
            repeaterCallback.DataSource = dsCBData;
            repeaterCallback.DataBind();
            DataSet dsUpdateID = objHotLeadBL.UpdateUserLogDataTakenField(Convert.ToInt32(Session[Constants.USER_ID].ToString()));
            Session["PageOpen"] = 0;

            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
            int LeadStatus = Convert.ToInt32(2);
            int DispositionID = Convert.ToInt32(0);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(SellerPhone);
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                int IsLockedOld = 0;
                DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                Session["AbandonSalePostingID"] = null;
                Response.Redirect("LiveTransfers.aspx");
            }
            else
            {
                DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(SellerPhone);
                if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                {
                    Session["NewSaleCarID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["carid"].ToString());
                    Session["NewSaleUID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["uid"].ToString());
                    Session["NewSalePostingID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString());
                    Session["NewSaleUserPackID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["UserPackID"].ToString());
                    Session["NewSaleSellerID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["sellerID"].ToString());
                    Session["NewSalePSID1"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID1"].ToString());
                    if (dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                    {
                        Session["NewSalePSID2"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString());
                    }
                    if (dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                    {
                        Session["NewSalePaymentID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString());
                    }
                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    Response.Redirect("LiveTransfers.aspx");
                }
                else
                {
                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    Response.Redirect("LiveTransfers.aspx");
                }
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

    protected void btnNotInterest_Click(object sender, EventArgs e)
    {
        try
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
            mpeNotInterest.Show();
            fillNIDdl();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void fillNIDdl()
    {
        try
        {
            DataSet dsNIReason = new DataSet();
            if (Session["NIReason"] == null)
            {
                dsNIReason = objHotLeadBL.GetNotInterestReason();
                Session["NIReason"] = dsNIReason;
            }
            else
            {
                dsNIReason = Session["NIReason"] as DataSet;
            }

            ddlNIReason.Items.Clear();
            ddlNIReason.DataSource = dsNIReason;
            ddlNIReason.DataTextField = "NotInterestReasonName";
            ddlNIReason.DataValueField = "NotInterestReasonID";
            ddlNIReason.DataBind();
            ddlNIReason.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnSaveNotInterestPopup_Click(object sender, EventArgs e)
    {
        try
        {
            mpeNotInterest.Hide();
            int LeadStatus = Convert.ToInt32(5);
            int DispositionID = Convert.ToInt32(0);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(SellerPhone);

            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
                mdepAlertExists.Show();
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "Phone " + txtPhone.Text + " already exists.<br />Please change phone # to save";
            }
            else
            {
                hdnItemSelect.Value = "0";
                DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(SellerPhone);
                if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                {
                    Session["NewSaleCarID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["carid"].ToString());
                    Session["NewSaleUID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["uid"].ToString());
                    Session["NewSalePostingID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString());
                    Session["NewSaleUserPackID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["UserPackID"].ToString());
                    Session["NewSaleSellerID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["sellerID"].ToString());
                    Session["NewSalePSID1"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID1"].ToString());
                    if (dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                    {
                        Session["NewSalePSID2"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString());
                    }
                    if (dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                    {
                        Session["NewSalePaymentID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString());
                    }

                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    Response.Redirect("LiveTransfers.aspx");
                }
                else
                {
                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    Response.Redirect("LiveTransfers.aspx");
                }
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
    protected void btnNotAvailable_Click(object sender, EventArgs e)
    {
        try
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
            mpeCallback.Show();
            fillCallbackddl();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void fillCallbackddl()
    {
        try
        {
            DataSet dsCallbackTime = new DataSet();
            if (Session["CallbackTimes"] == null)
            {
                dsCallbackTime = objHotLeadBL.GetCallbackTimings();
                Session["CallbackTimes"] = dsCallbackTime;
            }
            else
            {
                dsCallbackTime = Session["CallbackTimes"] as DataSet;
            }

            ddlCallbackPop.Items.Clear();
            ddlCallbackPop.DataSource = dsCallbackTime;
            ddlCallbackPop.DataTextField = "CallbackStatusName";
            ddlCallbackPop.DataValueField = "CallbackStatusID";
            ddlCallbackPop.DataBind();
            ddlCallbackPop.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCallbackSavePop_Click(object sender, EventArgs e)
    {
        try
        {
            mpeCallback.Hide();
            int LeadStatus = Convert.ToInt32(4);
            int DispositionID = Convert.ToInt32(1);
            string SellerPhone = txtPhone.Text;
            SellerPhone = SellerPhone.Replace("-", "");
            SellerPhone = SellerPhone.Replace("-", "");
            DataSet dsUserExists = objHotLeadBL.ChkUserPhoneNumberExistsForSale(SellerPhone);
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
                mdepAlertExists.Show();
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "Phone " + txtPhone.Text + " already exists.<br />Please change phone # to save";
            }
            else
            {
                hdnItemSelect.Value = "0";
                DataSet dsUserDraftExists = objHotLeadBL.ChkUserPhoneNumberExists(SellerPhone);
                if (dsUserDraftExists.Tables[0].Rows.Count > 0)
                {
                    Session["NewSaleCarID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["carid"].ToString());
                    Session["NewSaleUID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["uid"].ToString());
                    Session["NewSalePostingID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["postingID"].ToString());
                    Session["NewSaleUserPackID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["UserPackID"].ToString());
                    Session["NewSaleSellerID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["sellerID"].ToString());
                    Session["NewSalePSID1"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID1"].ToString());
                    if (dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString() != "")
                    {
                        Session["NewSalePSID2"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PSID2"].ToString());
                    }
                    if (dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString() != "")
                    {
                        Session["NewSalePaymentID"] = Convert.ToInt32(dsUserDraftExists.Tables[0].Rows[0]["PaymentID"].ToString());
                    }
                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    Response.Redirect("LiveTransfers.aspx");
                }
                else
                {
                    SaveInfo(LeadStatus, DispositionID);
                    int PostingIDOld = Convert.ToInt32(Session["LiveTransferPostingID"].ToString());
                    int IsLockedOld = 0;
                    DataSet dsLockCust = objHotLeadBL.USP_Lock_Customer(PostingIDOld, IsLockedOld);
                    Session["AbandonSalePostingID"] = null;
                    Response.Redirect("LiveTransfers.aspx");
                }
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
    private void SaveInfo(int LeadStatus, int DispositionID)
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

            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            string Title = "";
            string State = ddlLocationState.SelectedItem.Text;
            String UpdatedBy = Session[Constants.NAME].ToString();
            string InternalNotesNew = txtSaleNotes.Text.Trim();
            InternalNotesNew = InternalNotesNew.Trim();
            string UpdateByWithDate = dtNow.ToString() + "-" + UpdatedBy + "<br>";

            if (LeadStatus == 5)
            {
                if (InternalNotesNew != "")
                {
                    //InternalNotesNew = UpdateByWithDate + InternalNotesNew.Trim() + "<br>" + "---------------------------------";
                    InternalNotesNew = InternalNotesNew + "<br>" + UpdatedBy + " calls customer and customer said not interested due to " + ddlNIReason.SelectedItem.Text;// +"<br>" + "---------------------------------";
                }
                else
                {
                    InternalNotesNew = UpdatedBy + " calls customer and customer said not interested due to " + ddlNIReason.SelectedItem.Text;// +"<br>" + "---------------------------------";
                }
            }
            if (LeadStatus == 4)
            {
                if (InternalNotesNew != "")
                {
                    //InternalNotesNew = UpdateByWithDate + InternalNotesNew.Trim() + "<br>" + "---------------------------------";
                    InternalNotesNew = InternalNotesNew + "<br>" + UpdatedBy + " calls customer and customer said callback after " + ddlCallbackPop.SelectedItem.Text;// +"<br>" + "---------------------------------";
                }
                else
                {
                    InternalNotesNew = UpdatedBy + " calls customer and customer said callback after " + ddlCallbackPop.SelectedItem.Text;// +"<br>" + "---------------------------------";
                }
            }
            int SourceOfPhotos = Convert.ToInt32(ddlPhotosSource.SelectedItem.Value);
            int SourceOfDescription = Convert.ToInt32(ddlDescriptionSource.SelectedItem.Value);

            string LastName = objGeneralFunc.ToProper(txtLastName.Text.Trim());
            int CarID;
            int RegUID;
            int PostingID;
            int UserPackID;
            int sellerID;

            if ((Session["NewSaleCarID"] == null) || (Session["NewSaleCarID"].ToString() == ""))
            {
                CarID = Convert.ToInt32(0);
            }
            else
            {
                CarID = Convert.ToInt32(Session["NewSaleCarID"].ToString());
            }
            if ((Session["NewSaleUID"] == null) || (Session["NewSaleUID"].ToString() == ""))
            {
                RegUID = Convert.ToInt32(0);
            }
            else
            {
                RegUID = Convert.ToInt32(Session["NewSaleUID"].ToString());
            }
            if ((Session["NewSalePostingID"] == null) || (Session["NewSalePostingID"].ToString() == ""))
            {
                PostingID = Convert.ToInt32(0);
            }
            else
            {
                PostingID = Convert.ToInt32(Session["NewSalePostingID"].ToString());
            }
            if ((Session["NewSaleUserPackID"] == null) || (Session["NewSaleUserPackID"].ToString() == ""))
            {
                UserPackID = Convert.ToInt32(0);
            }
            else
            {
                UserPackID = Convert.ToInt32(Session["NewSaleUserPackID"].ToString());
            }
            if ((Session["NewSaleSellerID"] == null) || (Session["NewSaleSellerID"].ToString() == ""))
            {
                sellerID = Convert.ToInt32(0);
            }
            else
            {
                sellerID = Convert.ToInt32(Session["NewSaleSellerID"].ToString());
            }
            int EmailExists = 1;
            if (chkbxEMailNA.Checked == true)
            {
                EmailExists = 0;
            }

            DataSet dsdata = objHotLeadBL.SaveSaleStatusDataForTransfers(objUserregInfo, SaleAgentID, PackageID, YearOfMake, MakeModelID, BodyTypeID, ConditionID, DriveTrain,
                        Price, Mileage, ExteriorColor, InteriorColor, Transmission, NumberOfDoors, VIN, NumberOfCylinder, FuelTypeID, Description, Condition, Title,
                        State, strIp, InternalNotesNew, LeadStatus, LastName, SourceOfPhotos, SourceOfDescription, CarID, RegUID, UserPackID, sellerID, PostingID, DispositionID, EmailExists);

            CarID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["CarID"].ToString());
            Session["NewSaleCarID"] = CarID;
            RegUID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["UID"].ToString());
            Session["NewSaleUID"] = RegUID;
            PostingID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["PostingID"].ToString());
            Session["NewSalePostingID"] = PostingID;
            UserPackID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["UserPackID"].ToString());
            Session["NewSaleUserPackID"] = UserPackID;
            sellerID = Convert.ToInt32(dsdata.Tables[0].Rows[0]["sellerID"].ToString());
            Session["NewSaleSellerID"] = sellerID;
            if (LeadStatus == 5)
            {
                int notInterestReason = Convert.ToInt32(ddlNIReason.SelectedItem.Value);
                DataSet dsNiData = objHotLeadBL.SaveNotInterestData(CarID, notInterestReason);
            }
            if (LeadStatus == 4)
            {
                int CallbackReasonID = Convert.ToInt32(ddlCallbackPop.SelectedItem.Value);
                DataSet dsCallbackData = objHotLeadBL.SaveCallbackTimingData(CarID, CallbackReasonID);
            }
            DataSet dsSaveAgentLog = objHotLeadBL.SaveAgentCallsLog(CarID, LeadStatus);


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
            if ((Session["NewSalePSID1"] == null) || (Session["NewSalePSID1"].ToString() == ""))
            {
                PSID1 = Convert.ToInt32(0);
            }
            else
            {
                PSID1 = Convert.ToInt32(Session["NewSalePSID1"].ToString());
            }
            int PSID2;
            if ((Session["NewSalePSID2"] == null) || (Session["NewSalePSID2"].ToString() == ""))
            {
                PSID2 = Convert.ToInt32(0);
            }
            else
            {
                PSID2 = Convert.ToInt32(Session["NewSalePSID2"].ToString());
            }
            int PaymentID;
            if ((Session["NewSalePaymentID"] == null) || (Session["NewSalePaymentID"].ToString() == ""))
            {
                PaymentID = Convert.ToInt32(0);
            }
            else
            {
                PaymentID = Convert.ToInt32(Session["NewSalePaymentID"].ToString());
            }

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
                    Session["NewSalePSID1"] = PSID1;
                    PSID2 = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PSID2"].ToString());
                    Session["NewSalePSID2"] = PSID2;
                    PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["NewSalePaymentID"] = PaymentID;
                    DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForTransfers(PostingID, strIp, DispositionID, LeadStatus);
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
                    Session["NewSalePSID1"] = PSID1;
                    PaymentID = Convert.ToInt32(dsSaveCCInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["NewSalePaymentID"] = PaymentID;
                    DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForTransfers(PostingID, strIp, DispositionID, LeadStatus);
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
                Session["NewSalePSID1"] = PSID1;
                PaymentID = Convert.ToInt32(dsSavePayPalInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                Session["NewSalePaymentID"] = PaymentID;
                DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForTransfers(PostingID, strIp, DispositionID, LeadStatus);
            }
            if (rdbtnPayCheck.Checked == true)
            {
                if (chkboxlstPDsale.Checked == true)
                {
                    DateTime PaymentScheduleDate = Convert.ToDateTime(txtPaymentDate.Text);
                    string Amount = txtPDAmountNow.Text;
                    int PSStatusID;
                    int pmntStatus;

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
                    Session["NewSalePSID1"] = PSID1;
                    PSID2 = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PSID2"].ToString());
                    Session["NewSalePSID2"] = PSID2;
                    PaymentID = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["NewSalePaymentID"] = PaymentID;
                    DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForTransfers(PostingID, strIp, DispositionID, LeadStatus);
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
                    string CheckNumber = "";//txtCheckNumber.Text;
                    int CheckType = Convert.ToInt32(5);
                    DataSet dsSaveCheckInfo = objHotLeadBL.SaveCheckData(PSID1, PackageID, CarID, PaymentScheduleDate, Amount, PSStatusID, PaymentID, SaleAgentID, PaymentType,
                                            pmntStatus, strIp, VoiceRecord, PostingID, AccType, BankRouting, bankName, AccNumber, AccHolderName, VoiceFileLocation, CheckType, CheckNumber);
                    PSID1 = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PSID1"].ToString());
                    Session["NewSalePSID1"] = PSID1;
                    PaymentID = Convert.ToInt32(dsSaveCheckInfo.Tables[0].Rows[0]["PaymentID"].ToString());
                    Session["NewSalePaymentID"] = PaymentID;
                    DataSet dsSaveLeadStatus = objHotLeadBL.SaveLeadStatusForTransfers(PostingID, strIp, DispositionID, LeadStatus);
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
    protected void BtnClsUpdated_Click(object sender, EventArgs e)
    {
        try
        {
            Session["AbandonSalePostingID"] = null;
            Response.Redirect("LiveTransfers.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsUpdateCheck = objHotLeadBL.CheckForUpdatesData(Convert.ToInt32(Session[Constants.USER_ID].ToString()));
            if (dsUpdateCheck.Tables[0].Rows[0]["UpdateID"].ToString() == "1")
            {
                Session["AbandonSalePostingID"] = null;
                DataSet dsdata = objHotLeadBL.GetLiveTransfersData();
                lblCountScrolls.Text = dsdata.Tables[0].Rows.Count.ToString() + " Transfers";
                LiveTransferRepeater.DataSource = dsdata;
                LiveTransferRepeater.DataBind();
                DataSet dsCBData = objHotLeadBL.GetCallbacksTransfersData();
                lblCountCallbacks.Text = dsCBData.Tables[0].Rows.Count.ToString() + " Callbacks";
                repeaterCallback.DataSource = dsCBData;
                repeaterCallback.DataBind();
                DataSet dsUpdateID = objHotLeadBL.UpdateUserLogDataTakenField(Convert.ToInt32(Session[Constants.USER_ID].ToString()));
                Tabselect.Value = "1";
            }
            DataSet dsAgents = objHotLeadBL.GetLoginAgentCount();
            if (dsAgents.Tables[0].Rows[0]["CountAll"].ToString() == "0")
            {
                lblAgentsCount.Text = "Agent(s) are not available";
                grdTest.Visible = false;
            }
            else
            {
                lblAgentsCount.Text = dsAgents.Tables[0].Rows[0]["CountAll"].ToString() + " Agent(s) are available";
                DataSet dsTransferAgents = objHotLeadBL.GetAllLoginAgentDetails();
                if (dsTransferAgents.Tables[0].Rows.Count > 0)
                {
                    grdTest.Visible = true;
                    grdTest.DataSource = dsTransferAgents.Tables[0];
                    grdTest.DataBind();
                }
            }
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TransfersInfoBinding();", true);
            //tdDataDetails.Style["display"] = "none";
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
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
            Tabselect.Value = "1";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "TabsScripting();", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
