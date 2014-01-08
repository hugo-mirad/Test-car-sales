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

public partial class AddNewCenters : System.Web.UI.Page
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
                            lblUserName.Text = lblUserName.Text + " (" + CenterCode.ToString() + ")-" + UserLogName.ToString();
                        }
                        lblCenterCode.Text = "Center code is " + CenterCode.ToString();
                        // lblAgentAddCenterCode.Text = "Center code is " + CenterCode.ToString();

                    }
                    lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "pageLoad();", true);
                    GetUserModules_AllUsers();
                    LoadCenters();
                    FillCenterRights();
                    //LoadDDL();
                }
            }
        }
    }

    private void FillCenterRights()
    {
        try
        {
            DataSet dsAllRoles = new DataSet();
            if (Session["AllCenterroles"] == null)
            {
                dsAllRoles = objHotLeadBL.GetCentersRoles();
                Session["AllCenterroles"] = dsAllRoles;
            }
            else
            {
                dsAllRoles = Session["AllCenterroles"] as DataSet;
            }

            chbklstCenterRoles.DataSource = dsAllRoles.Tables[0];
            chbklstCenterRoles.DataTextField = "CenterRoleName";
            chbklstCenterRoles.DataValueField = "CenterRoleID";
            chbklstCenterRoles.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void lnkAddUSer_Click(object sender, EventArgs e)
    {
        try
        {
            txtFName.Text = "";
            txtCenterCode.Text = "";
            FillCenterRights();
            mpeAddNew.Show();
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

    protected void btnAddCenter_Click(object sender, EventArgs e)
    {
        try
        {
            Session["CkeckPopupCenter"] = 1;
            string centername = objGeneralFunc.ToProper(txtFName.Text.Trim());
            string CenterCode = txtCenterCode.Text.Trim();
            DataSet dsCenterExits = objHotLeadBL.CheckCenterExistsForNewCenter(CenterCode);
            if (dsCenterExits.Tables.Count > 0)
            {
                if (dsCenterExits.Tables[0].Rows.Count > 0)
                {
                    mpelblUerExist.Show();

                    lblUerExist.Text = "Center code already exists. Please choose another center code.";
                    lblUerExist.Visible = true;
                }
                else
                {
                    DataSet newCenter = objHotLeadBL.SaveNewCenter(CenterCode, centername);
                    int CenterID = Convert.ToInt32(newCenter.Tables[0].Rows[0]["AgentCenterID"].ToString());
                    for (int m = 0; m < chbklstCenterRoles.Items.Count; m++)
                    {
                        if (chbklstCenterRoles.Items[m].Selected == true)
                        {
                            DataSet dsCentRoles = objHotLeadBL.SaveNewCenterRolesRights(CenterID, Convert.ToInt32(chbklstCenterRoles.Items[m].Value), 1);
                        }
                        else
                        {
                            DataSet dsCentRoles = objHotLeadBL.SaveNewCenterRolesRights(CenterID, Convert.ToInt32(chbklstCenterRoles.Items[m].Value), 0);
                        }
                    }
                    LoadCenters();
                    mpealteruser.Show();
                    lblErr.Text = "Center details added successfully";
                    lblErr.Visible = true;
                }
            }
            else
            {
                DataSet newCenter = objHotLeadBL.SaveNewCenter(CenterCode, centername);
                int CenterID = Convert.ToInt32(newCenter.Tables[0].Rows[0]["AgentCenterID"].ToString());
                for (int m = 0; m < chbklstCenterRoles.Items.Count; m++)
                {
                    if (chbklstCenterRoles.Items[m].Selected == true)
                    {
                        DataSet dsCentRoles = objHotLeadBL.SaveNewCenterRolesRights(CenterID, Convert.ToInt32(chbklstCenterRoles.Items[m].Value), 1);
                    }
                    else
                    {
                        DataSet dsCentRoles = objHotLeadBL.SaveNewCenterRolesRights(CenterID, Convert.ToInt32(chbklstCenterRoles.Items[m].Value), 0);
                    }
                }
                LoadCenters();
                mpealteruser.Show();
                lblErr.Text = "Center details added successfully";
                lblErr.Visible = true;
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
            if ((Convert.ToInt32(Session[Constants.CenterCodeID])) == Convert.ToInt32(Session["CenterID_Update"]))
            {
                mpealteruser.Show();
                lblErr.Text = "This transaction is not an allowed transaction";
                lblErr.Visible = true;
            }
            else
            {
                int CenterStatus = Convert.ToInt32(ddlUpStatus.SelectedItem.Value);
                string centerName = objGeneralFunc.ToProper(txtUpFName.Text.Trim());
                int CenterID = Convert.ToInt32(Session["CenterID_Update"]);
                DataSet dsUpDetails = objHotLeadBL.UpdateCenterDetails(CenterID, centerName, CenterStatus);
                for (int m = 0; m < chkbxlistupdateCenterRoles.Items.Count; m++)
                {
                    if (chkbxlistupdateCenterRoles.Items[m].Selected == true)
                    {
                        DataSet dsCentRoles = objHotLeadBL.SaveNewCenterRolesRights(CenterID, Convert.ToInt32(chkbxlistupdateCenterRoles.Items[m].Value), 1);
                    }
                    else
                    {
                        DataSet dsCentRoles = objHotLeadBL.SaveNewCenterRolesRights(CenterID, Convert.ToInt32(chkbxlistupdateCenterRoles.Items[m].Value), 0);
                    }
                }
                LoadCenters();
                mpealteruser.Show();
                lblErr.Text = "Center details updated successfully";
                lblErr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnlblUerExist_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(Session["CkeckPopupCenter"]) == 2)
            {
                MPEUpdate.Show();
                mpelblUerExist.Hide();
            }
            else if (Convert.ToInt32(Session["CkeckPopupCenter"]) == 1)
            {
                mpeAddNew.Show();
                mpelblUerExist.Hide();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetUserModules_AllUsers()
    {
        try
        {
            DataSet dsAllUsersModuleRights = new DataSet();
            dsAllUsersModuleRights = objHotLeadBL.GetUsersModuleRites_All();
            Session["AllUsersModuleRights"] = dsAllUsersModuleRights;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void LoadCenters()
    {
        try
        {

            DataSet dsCenters = objHotLeadBL.GetAllCentersData();
            Session["CentersDetails"] = dsCenters;
            if (dsCenters.Tables.Count > 0)
            {
                if (dsCenters.Tables[0].Rows.Count > 0)
                {
                    lblResHead.Text = "Number of centers " + dsCenters.Tables[0].Rows.Count.ToString();
                    grdUserDetails.Visible = true;
                    grdUserDetails.DataSource = dsCenters.Tables[0];
                    grdUserDetails.DataBind();
                }
                else
                {
                    lblResHead.Text = "Results not found";
                    grdUserDetails.Visible = false;

                }
            }
            else
            {
                lblResHead.Text = "Results not found";
                grdUserDetails.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            DataSet dsVoice = new DataSet();
            DataTable dt = new DataTable();
            DataView dv = new DataView();

            DataSet dsModules = new DataSet();
            DataTable dt1 = new DataTable();
            DataView dv1 = new DataView();



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnCenterID");
                Label lblUserType = (Label)e.Row.FindControl("lblUserType");
                //  Label lblLocationRights = (Label)e.Row.FindControl("lblLocationRights");


                dsVoice = (DataSet)Session["CentersDetails"];
                dv = dsVoice.Tables[0].DefaultView;



                dv.RowFilter = "AgentCenterID=" + hdnId.Value + "";
                dt = dv.ToTable();

                if (dt.Rows.Count > 0)
                {
                    dsModules = (DataSet)Session["CentersDetails"];

                    dv1 = dsModules.Tables[1].DefaultView;
                    dv1.RowFilter = "AgentCenterID=" + hdnId.Value + "";
                    dt1 = dv1.ToTable();


                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                        {
                            if ((dt1.Rows[i]["CenterRoleID"].ToString() == "1") && (dt1.Rows[i]["CenterRoleName"].ToString() == "Sales") && (dt1.Rows[i]["CenterRightStatus"].ToString() == "1"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", Sales";
                                }
                                else
                                {
                                    lblUserType.Text = "Sales";
                                }

                            }
                            if ((dt1.Rows[i]["CenterRoleID"].ToString() == "2") && (dt1.Rows[i]["CenterRoleName"].ToString() == "Transfers Out") && (dt1.Rows[i]["CenterRightStatus"].ToString() == "1"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", Transfers Out";
                                }
                                else
                                {
                                    lblUserType.Text = "Transfers Out";
                                }
                            }
                            if ((dt1.Rows[i]["CenterRoleID"].ToString() == "3") && (dt1.Rows[i]["CenterRoleName"].ToString() == "Transfers In") && (dt1.Rows[i]["CenterRightStatus"].ToString() == "1"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", Transfers In";
                                }
                                else
                                {
                                    lblUserType.Text = "Transfers In";
                                }

                            }
                            if ((dt1.Rows[i]["CenterRoleID"].ToString() == "4") && (dt1.Rows[i]["CenterRoleName"].ToString() == "QC") && (dt1.Rows[i]["CenterRightStatus"].ToString() == "1"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", QC";
                                }
                                else
                                {
                                    lblUserType.Text = "QC";
                                }
                            }
                            if ((dt1.Rows[i]["CenterRoleID"].ToString() == "5") && (dt1.Rows[i]["CenterRoleName"].ToString() == "Central Reports") && (dt1.Rows[i]["CenterRightStatus"].ToString() == "1"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", Central Reports";
                                }
                                else
                                {
                                    lblUserType.Text = "Central Reports";
                                }
                            }

                        }

                    }

                }
                dv1.RowFilter = "";
                dv.RowFilter = "";
            }

        }
        catch (Exception ex)
        {
            throw ex;
            // throw ex;
        }
    }
    private void FillCenterRightsUpdate()
    {
        try
        {
            DataSet dsAllRoles = new DataSet();
            if (Session["AllCenterroles"] == null)
            {
                dsAllRoles = objHotLeadBL.GetCentersRoles();
                Session["AllCenterroles"] = dsAllRoles;
            }
            else
            {
                dsAllRoles = Session["AllCenterroles"] as DataSet;
            }

            chkbxlistupdateCenterRoles.DataSource = dsAllRoles.Tables[0];
            chkbxlistupdateCenterRoles.DataTextField = "CenterRoleName";
            chkbxlistupdateCenterRoles.DataValueField = "CenterRoleID";
            chkbxlistupdateCenterRoles.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataSet dsUser = new DataSet();
            dsUser = (DataSet)Session["CentersDetails"];

            if (e.CommandName == "view")
            {
                int CenterID = Convert.ToInt32(e.CommandArgument);

                DataView dv1 = new DataView();
                DataTable dt = new DataTable();

                dv1 = dsUser.Tables[0].DefaultView;

                dv1.RowFilter = "AgentCenterID=" + CenterID + "";
                Session["CenterID_Update"] = CenterID;

                dt = dv1.ToTable();

                if (dt.Rows.Count > 0)
                {
                    FillCenterRightsUpdate();

                    DataView dvRoles = new DataView();
                    DataTable dtRoles = new DataTable();
                    dvRoles = dsUser.Tables[1].DefaultView;
                    dvRoles.RowFilter = "AgentCenterID=" + CenterID + "";
                    dtRoles = dvRoles.ToTable();
                    if (dtRoles.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtRoles.Rows.Count; i++)
                        {
                            if (dtRoles.Rows[i]["CenterRightStatus"].ToString() == "1")
                            {
                                chkbxlistupdateCenterRoles.Items[i].Selected = true;
                            }
                        }
                    }
                    txtUpFName.Text = Convert.ToString(dt.Rows[0]["AgentCenterName"].ToString());

                    lblUpdateAgentCenterCode.Text = "Center code is " + dt.Rows[0]["AgentCenterCode"].ToString();


                    ListItem liUserStatus = new ListItem();
                    liUserStatus.Value = Convert.ToInt32(dt.Rows[0]["AgentCenterStatus"]).ToString();
                    liUserStatus.Text = Convert.ToString(dt.Rows[0]["status_name"].ToString());
                    ddlUpStatus.SelectedIndex = ddlUpStatus.Items.IndexOf(liUserStatus);

                }
                MPEUpdate.Show();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void ddlUpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlUpStatus.SelectedItem.Value) == 0)
            {
                if ((Convert.ToInt32(Session[Constants.CenterCodeID])) == Convert.ToInt32(Session["CenterID_Update"]))
                {
                    Session["CkeckPopupCenter"] = 2;
                    mpelblUerExist.Show();
                    lblUerExist.Text = "This transaction is not an allowed transaction";

                    lblUerExist.Visible = true;
                    ListItem liUserStatus = new ListItem();
                    liUserStatus.Value = "1";
                    liUserStatus.Text = "Active";
                    ddlUpStatus.SelectedIndex = ddlUpStatus.Items.IndexOf(liUserStatus);

                    MPEUpdate.Hide();
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void btnChangePW_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsChangePW = new DataSet();

            Session["ChangePass"] = 2;

            dsChangePW = objHotLeadBL.Get_Change_Password(txtNewPW.Text, Convert.ToInt32(Session["UserId_Update"]));

            if (dsChangePW != null)
            {
                if (dsChangePW.Tables.Count > 0)
                {
                    if (dsChangePW.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dsChangePW.Tables[0].Rows[0]["CHANGED"]) == 0)
                        {
                            //mpealteruser.Show();
                            //LoadUsers();
                            //lblErr.Text = "Password Changed Successfully";
                            //lblErr.Visible = true;

                            mpelblUerExist.Show();
                            lblUerExist.Text = "Password changed successfully";

                            lblUerExist.Visible = true;
                            //MPEUpdate.Hide();

                            //mpelblUerExist.Show();
                            //lblUerExist.Text = "Password Changed Successfully";
                            //lblUerExist.Visible = true;
                        }
                        else
                        {

                            //lblUerExist.Text = "Invalid Old Password";
                            //  lblUerExist.Visible = true;
                            //MPEUpdate.Show();
                            //mpeChangePW.Show();
                            //    mpelblUerExist.Show();
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
    protected void btnCancelPW_Click(object sender, EventArgs e)
    {
        try
        {
            MPEUpdate.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkCenterCode_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["CentersDetails"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentCenterCode";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkCenterCode.Text = "Center Code &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkCenterCode.Text = "Center Code &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkCenterCode.Text = "Center Code &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkCenterCode.Text = "Center Code &#8659";
            }

            lnkCenterName.Text = "Name &darr; &uarr;";
            lnkStatus.Text = "Status &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdUserDetails, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkCenterName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["CentersDetails"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentCenterName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkCenterName.Text = "Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkCenterName.Text = "Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkCenterName.Text = "Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkCenterName.Text = "Name &#8659";
            }

            lnkCenterCode.Text = "Center Code &darr; &uarr;";
            lnkStatus.Text = "Status &darr; &uarr;";
            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdUserDetails, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["CentersDetails"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentCenterStatus";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkStatus.Text = "Status &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkStatus.Text = "Status &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkStatus.Text = "Status &#8659";
            }

            lnkCenterCode.Text = "Center Code &darr; &uarr;";
            lnkCenterName.Text = "Name &darr; &uarr;";

            if (dt != null)
            {
                BizUtility.GridSortForReport(txthdnSortOrder, SortExp, grdUserDetails, 0, dt, Session["SortDirec"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}
