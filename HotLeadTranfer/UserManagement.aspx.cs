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
public partial class UserManagement : System.Web.UI.Page
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
                Session["CurrentPage"] = "User management";

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
                        lblAgentAddCenterCode.Text = "Center code is " + CenterCode.ToString();
                        lblUpdateAgentCenterCode.Text = "Center code is " + CenterCode.ToString();
                    }
                    lnkTicker.Attributes.Add("href", "javascript:poptastic('Ticker.aspx?CID=" + Session[Constants.CenterCodeID] + "&CNAME=" + Session[Constants.CenterCode] + "');");
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "pageLoad();", true);
                    GetUserModules_AllUsers();
                    LoadUsers();
                    //LoadDDL();
                    FillNewUserType();
                    DataSet dsCenterRoles = objHotLeadBL.GetCenterRolesByID(Convert.ToInt32(Session[Constants.CenterCodeID].ToString()));
                    Session["dsCenterRoles"] = dsCenterRoles;
                    if (dsCenterRoles.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsCenterRoles.Tables[0].Rows.Count; i++)
                        {
                            if (dsCenterRoles.Tables[0].Rows[i]["CenterRoleID"].ToString() == "3")
                            {
                                if (dsCenterRoles.Tables[0].Rows[i]["CenterRightStatus"].ToString() == "0")
                                {
                                    ListItem liCenterRoles = new ListItem();
                                    liCenterRoles.Value = "5";
                                    liCenterRoles.Text = "Transfers";
                                    ddlAddUsertype.Items.Remove(liCenterRoles);
                                }
                            }
                            if (dsCenterRoles.Tables[0].Rows[i]["CenterRoleID"].ToString() == "4")
                            {
                                if (dsCenterRoles.Tables[0].Rows[i]["CenterRightStatus"].ToString() == "0")
                                {
                                    ListItem liCenterRoles = new ListItem();
                                    liCenterRoles.Value = "4";
                                    liCenterRoles.Text = "QC Module";
                                    ddlAddUsertype.Items.Remove(liCenterRoles);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void FillNewUserType()
    {
        try
        {
            DataSet dsFillNewUserType = new DataSet();
            dsFillNewUserType = objHotLeadBL.GetMasterUserType();
            Session["dsFillNewUserType"] = dsFillNewUserType;
            ddlAddUsertype.DataSource = dsFillNewUserType.Tables[0];
            ddlAddUsertype.DataTextField = "UserTypeName";
            ddlAddUsertype.DataValueField = "UtypeID";
            ddlAddUsertype.DataBind();
            ddlAddUsertype.Items.Insert(0, new ListItem("Select", "0"));
            ListItem liCenterRoles = new ListItem();
            liCenterRoles.Value = "6";
            liCenterRoles.Text = "Central admin";
            ddlAddUsertype.Items.Remove(liCenterRoles);
            ListItem liCenterRoles2 = new ListItem();
            liCenterRoles2.Value = "2";
            liCenterRoles2.Text = "Center admin";
            ddlAddUsertype.Items.Remove(liCenterRoles2);
            ListItem liCenterRoles4 = new ListItem();
            liCenterRoles4.Value = "7";
            liCenterRoles4.Text = "Leads";
            ddlAddUsertype.Items.Remove(liCenterRoles4);

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

        DataSet dsModules = new DataSet();
        dsModules = (DataSet)Session[Constants.USER_Rights];

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
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Agent report")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnReports.Enabled = true;
                        lnkbtnMyDealerRep.Enabled = true;
                    }

                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Intromail")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnIntromail.Enabled = true;

                    }
                }
                if (dsModules.Tables[0].Rows[i]["ModuleName"].ToString() == "Center report")
                {
                    if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                    {
                        lnkbtnCentralReport.Enabled = true;
                    }

                }

            }
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
    private void LoadUsers()
    {
        try
        {
            int CenterCodeID = Convert.ToInt32(Session[Constants.CenterCodeID].ToString());
            DataSet dsUsers = objHotLeadBL.GetUsersDetails(CenterCodeID);
            Session["UserDetails"] = dsUsers;
            if (dsUsers.Tables.Count > 0)
            {
                if (dsUsers.Tables[0].Rows.Count > 0)
                {
                    lblResHead.Text = "Number of users " + dsUsers.Tables[0].Rows.Count.ToString();
                    grdUserDetails.Visible = true;
                    grdUserDetails.DataSource = dsUsers.Tables[0];
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
                HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnUID");
                Label lblUserType = (Label)e.Row.FindControl("lblUserType");
                //  Label lblLocationRights = (Label)e.Row.FindControl("lblLocationRights");


                dsVoice = (DataSet)Session["UserDetails"];
                dv = dsVoice.Tables[0].DefaultView;



                dv.RowFilter = "AgentUID=" + hdnId.Value + "";
                dt = dv.ToTable();

                if (dt.Rows.Count > 0)
                {
                    dsModules = (DataSet)Session["AllUsersModuleRights"];

                    dv1 = dsModules.Tables[0].DefaultView;
                    dv1.RowFilter = "AgentUID=" + hdnId.Value + "";
                    dt1 = dv1.ToTable();


                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                        {
                            if ((dt1.Rows[i]["ModuleActive"].ToString() == "1") && (dt1.Rows[i]["modulename"].ToString() == "New sale"))
                            {
                                if (lblUserType.Text != "")
                                {

                                    lblUserType.Text = lblUserType.Text + ", New sale";
                                }
                                else
                                {
                                    lblUserType.Text = "New sale";
                                }

                            }
                            if ((dt1.Rows[i]["ModuleActive"].ToString() == "1") && (dt1.Rows[i]["modulename"].ToString() == "Agent report"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", Agent report";
                                }
                                else
                                {
                                    lblUserType.Text = "Agent report";
                                }
                            }
                            if ((dt1.Rows[i]["ModuleActive"].ToString() == "1") && (dt1.Rows[i]["modulename"].ToString() == "User management"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", User management";
                                }
                                else
                                {
                                    lblUserType.Text = "User management";
                                }

                            }
                            if ((dt1.Rows[i]["ModuleActive"].ToString() == "1") && (dt1.Rows[i]["modulename"].ToString() == "Intromail"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", Intromail";
                                }
                                else
                                {
                                    lblUserType.Text = "Intromail";
                                }
                            }
                            if ((dt1.Rows[i]["ModuleActive"].ToString() == "1") && (dt1.Rows[i]["modulename"].ToString() == "Center report"))
                            {
                                if (lblUserType.Text != "")
                                {
                                    lblUserType.Text = lblUserType.Text + ", Center report";
                                }
                                else
                                {
                                    lblUserType.Text = "Center report";
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
    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            int iUserId = 0;
            Session["CkeckPoPUP"] = 1;
            int CenterCode = Convert.ToInt32(Session[Constants.CenterCodeID]);
            ds = objHotLeadBL.SaveUserDetails(objGeneralFunc.ToProper(txtFName.Text), txtEmail.Text, Convert.ToInt32(ddlAddUsertype.SelectedItem.Value), txtUserName.Text, txtPassword.Text, 1, CenterCode);


            if (ds != null)
            {

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["Checks"]) == 1)
                {
                    mpelblUerExist.Show();

                    lblUerExist.Text = "User name already exists. Please choose another user name.";
                    lblUerExist.Visible = true;

                }
                else
                {
                    if (Convert.ToInt32(ddlAddUsertype.SelectedItem.Value) == 1)
                    {
                        for (int m = 1; m < 8; m++)
                        {
                            if (m == 3)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 5)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 6)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 7)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 1);
                            }

                        }
                    }
                    if (Convert.ToInt32(ddlAddUsertype.SelectedItem.Value) == 2)
                    {
                        for (int m = 1; m < 8; m++)
                        {
                            if (m == 5)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 6)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 7)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 1);
                            }

                        }
                    }
                    if (Convert.ToInt32(ddlAddUsertype.SelectedItem.Value) == 3)
                    {
                        for (int m = 1; m < 8; m++)
                        {
                            if (m == 6)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 7)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 1);
                            }
                        }
                    }
                    if (Convert.ToInt32(ddlAddUsertype.SelectedItem.Value) == 4)
                    {
                        for (int m = 1; m < 8; m++)
                        {
                            if (m == 6)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 1);
                            }
                            else
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                        }
                    }
                    if (Convert.ToInt32(ddlAddUsertype.SelectedItem.Value) == 5)
                    {
                        for (int m = 1; m < 8; m++)
                        {
                            if (m == 3)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 5)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else if (m == 6)
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 0);
                            }
                            else
                            {
                                objHotLeadBL.SaveModuleRights(Convert.ToInt32(ds.Tables[0].Rows[0]["UID"].ToString()), m, 1);
                            }
                        }
                    }
                    mpealteruser.Show();
                    GetUserModules_AllUsers();


                    LoadUsers();

                    //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('User Details Added Successfully.')", true);

                    lblErr.Text = "User details added successfully";
                    lblErr.Visible = true;

                }
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
            if ((Convert.ToInt32(Session[Constants.USER_ID])) == Convert.ToInt32(Session["UserId_Update"]))
            {
                mpealteruser.Show();
                lblErr.Text = "This transaction is not an allowed transaction";

                lblErr.Visible = true;

            }
            else
            {
                int Tran_By = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                objHotLeadBL.UpdateUser_Details(objGeneralFunc.ToProper(txtUpFName.Text), txtUpEmail.Text, Convert.ToInt32(ddlUpdateuserType.SelectedItem.Value), Convert.ToInt32(Session["UserId_Update"]), Convert.ToInt32(ddlUpStatus.SelectedItem.Value), Tran_By);

                if (Convert.ToInt32(ddlUpdateuserType.SelectedItem.Value) == 1)
                {
                    for (int m = 1; m < 8; m++)
                    {
                        if (m == 3)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 5)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 6)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 7)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 1);
                        }

                    }
                }
                if (Convert.ToInt32(ddlUpdateuserType.SelectedItem.Value) == 2)
                {
                    for (int m = 1; m < 8; m++)
                    {
                        if (m == 5)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 6)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 7)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 1);
                        }

                    }
                }
                if (Convert.ToInt32(ddlUpdateuserType.SelectedItem.Value) == 3)
                {
                    for (int m = 1; m < 8; m++)
                    {
                        if (m == 6)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 7)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 1);
                        }
                    }
                }
                if (Convert.ToInt32(ddlUpdateuserType.SelectedItem.Value) == 4)
                {
                    for (int m = 1; m < 8; m++)
                    {
                        if (m == 6)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 1);
                        }
                        else
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                    }
                }
                if (Convert.ToInt32(ddlUpdateuserType.SelectedItem.Value) == 5)
                {
                    for (int m = 1; m < 8; m++)
                    {
                        if (m == 3)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 5)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else if (m == 6)
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 0);
                        }
                        else
                        {
                            objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), m, 1);
                        }
                    }
                }
                //for (int m = 0; m < ddlLocationUpdate.Items.Count; m++)
                //{
                //    if (ddlLocationUpdate.Items[m].Selected == true)
                //    {
                //        objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), Convert.ToInt32(ddlLocationUpdate.Items[m].Value), 1);
                //    }
                //    else
                //    {
                //        objHotLeadBL.UpdateModuleRights(Convert.ToInt32(Session["UserId_Update"].ToString()), Convert.ToInt32(ddlLocationUpdate.Items[m].Value), 0);
                //    }
                //}


                mpealteruser.Show();
                lblErr.Text = "User details updated successfully";
                GetUserModules_AllUsers();

                LoadUsers();
                lblErr.Visible = true;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('User Details Updated Successfully.')", true);
            }
        }
        catch (Exception ex)
        {
            //throw ex;
            throw ex;
        }
    }
    protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataSet dsUser = new DataSet();
            dsUser = (DataSet)Session["UserDetails"];

            if (e.CommandName == "view")
            {
                int UserID = Convert.ToInt32(e.CommandArgument);

                DataView dv1 = new DataView();
                DataTable dt = new DataTable();

                dv1 = dsUser.Tables[0].DefaultView;

                dv1.RowFilter = "AgentUID=" + UserID + "";
                Session["UserId_Update"] = UserID;

                dt = dv1.ToTable();

                if (dt.Rows.Count > 0)
                {
                    FillUpdateUserType();
                    DataSet dsCenterRolesUpdate = Session["dsCenterRoles"] as DataSet;

                    if (dsCenterRolesUpdate.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsCenterRolesUpdate.Tables[0].Rows.Count; i++)
                        {
                            if (dsCenterRolesUpdate.Tables[0].Rows[i]["CenterRoleID"].ToString() == "3")
                            {
                                if (dsCenterRolesUpdate.Tables[0].Rows[i]["CenterRightStatus"].ToString() == "0")
                                {
                                    ListItem liCenterRoles = new ListItem();
                                    liCenterRoles.Value = "5";
                                    liCenterRoles.Text = "Transfers";
                                    ddlUpdateuserType.Items.Remove(liCenterRoles);
                                }
                            }
                            if (dsCenterRolesUpdate.Tables[0].Rows[i]["CenterRoleID"].ToString() == "4")
                            {
                                if (dsCenterRolesUpdate.Tables[0].Rows[i]["CenterRightStatus"].ToString() == "0")
                                {
                                    ListItem liCenterRoles = new ListItem();
                                    liCenterRoles.Value = "4";
                                    liCenterRoles.Text = "QC Module";
                                    ddlUpdateuserType.Items.Remove(liCenterRoles);
                                }
                            }
                        }
                    }
                    txtUpFName.Text = Convert.ToString(dt.Rows[0]["AgentUFirstName"].ToString());


                    txtUpEmail.Text = Convert.ToString(dt.Rows[0]["AgentUEmail"].ToString());

                    lblUnamePW.Text = dt.Rows[0]["AgentLogUname"].ToString();
                    lblUpdateUser.Text = dt.Rows[0]["AgentLogUname"].ToString();
                    ListItem liUserType = new ListItem();
                    liUserType.Value = Convert.ToString(dt.Rows[0]["AgentUtype_Id"].ToString());
                    liUserType.Text = Convert.ToString(dt.Rows[0]["UsertypeName"].ToString());
                    ddlUpdateuserType.SelectedIndex = ddlUpdateuserType.Items.IndexOf(liUserType);


                    ListItem liUserStatus = new ListItem();
                    liUserStatus.Value = Convert.ToInt32(dt.Rows[0]["AgentIsActive"]).ToString();
                    liUserStatus.Text = Convert.ToString(dt.Rows[0]["status_name"].ToString());
                    ddlUpStatus.SelectedIndex = ddlUpStatus.Items.IndexOf(liUserStatus);


                }



                DataSet dsModules = new DataSet();

                dsModules = objHotLeadBL.GetUsersModuleRites_All(UserID);

                //ddlLocationUpdate.DataSource = dsModules.Tables[0].DefaultView;
                //ddlLocationUpdate.DataTextField = "ModuleName";
                //ddlLocationUpdate.DataValueField = "ModuleID";
                ////  objliaLL.Attributes.Add("onclick", "return Check_All_CheckBoxClick(this)");
                //ddlLocationUpdate.DataBind();

                //for (int y = 0; y < ddlLocationUpdate.Items.Count; y++)
                //{
                //    ddlLocationUpdate.Items[y].Selected = false;
                //}


                //if (dsModules != null)
                //{
                //    if (dsModules.Tables[0].Rows.Count > 0)
                //    {
                //        if (dsModules.Tables[0].Rows.Count > 0)
                //        {
                //            for (int i = 0; i <= dsModules.Tables[0].Rows.Count - 1; i++)
                //            {
                //                if (dsModules.Tables[0].Rows[i]["ModuleActive"].ToString() == "1")
                //                {
                //                    ddlLocationUpdate.Items[i].Selected = true;

                //                }

                //            }
                //        }

                //    }
                //}
                MPEUpdate.Show();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillUpdateUserType()
    {
        DataSet dsFillUpdateUserType = Session["dsFillNewUserType"] as DataSet;
        ddlUpdateuserType.DataSource = dsFillUpdateUserType.Tables[0];
        ddlUpdateuserType.DataTextField = "UserTypeName";
        ddlUpdateuserType.DataValueField = "UtypeID";
        ddlUpdateuserType.DataBind();
        ddlUpdateuserType.Items.Insert(0, new ListItem("Select", "0"));
        ListItem liCenterRoles = new ListItem();
        liCenterRoles.Value = "6";
        liCenterRoles.Text = "Central admin";
        ddlUpdateuserType.Items.Remove(liCenterRoles);
        ListItem liCenterRoles2 = new ListItem();
        liCenterRoles2.Value = "2";
        liCenterRoles2.Text = "Center admin";
        ddlUpdateuserType.Items.Remove(liCenterRoles2);
        ListItem liCenterRoles4 = new ListItem();
        liCenterRoles4.Value = "7";
        liCenterRoles4.Text = "Leads";
        ddlUpdateuserType.Items.Remove(liCenterRoles4);
    }
    protected void btnlblUerExist_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(Session["CkeckPoPUP"]) == 2)
            {
                MPEUpdate.Show();
                mpelblUerExist.Hide();
            }
            else if (Convert.ToInt32(Session["ChangePass"]) == 2)
            {
                MPEUpdate.Show();
                //mpeChangePW.Show();
            }
            else if (Convert.ToInt32(Session["CkeckPoPUP"]) == 1)
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
    protected void ddlUpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlUpStatus.SelectedItem.Value) == 0)
            {
                if ((Convert.ToInt32(Session[Constants.USER_ID])) == Convert.ToInt32(Session["UserId_Update"]))
                {
                    Session["CkeckPoPUP"] = 2;
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

    protected void lnkUserName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["UserDetails"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentLogUname";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkUserName.Text = "User Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkUserName.Text = "User Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkUserName.Text = "User Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkUserName.Text = "User Name &#8659";
            }

            lnkName.Text = "Name &darr; &uarr;";
            lnkStatus.Text = "Status &darr; &uarr;";
            lnkUserRights.Text = "User Type &darr; &uarr;";
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

    protected void lnkName_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["UserDetails"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentUFirstName";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkName.Text = "Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkName.Text = "Name &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkName.Text = "Name &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkName.Text = "Name &#8659";
            }

            lnkUserName.Text = "User Name &darr; &uarr;";
            lnkStatus.Text = "Status &darr; &uarr;";
            lnkUserRights.Text = "User Type &darr; &uarr;";
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
            ds = Session["UserDetails"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "AgentIsActive";
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

            lnkUserName.Text = "User Name &darr; &uarr;";
            lnkName.Text = "Name &darr; &uarr;";
            lnkUserRights.Text = "User Type &darr; &uarr;";
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

    protected void lnkUserRights_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Timeout = 180;
            DataSet ds = new DataSet();
            ds = Session["UserDetails"] as DataSet;
            ds.Tables[0].DefaultView.RowFilter = "";
            DataTable dt = ds.Tables[0];
            string SortExp = "usertypename";
            if (Session["SortDirec"] == null)
            {
                Session["SortDirec"] = "Ascending";
                lnkUserRights.Text = "User Type &#8659";
            }
            else if (Session["SortDirec"].ToString() == "")
            {
                Session["SortDirec"] = "Ascending";
                lnkUserRights.Text = "User Type &#8659";
            }
            else if (Session["SortDirec"].ToString() == "Ascending")
            {
                Session["SortDirec"] = "Descending";
                lnkUserRights.Text = "User Type &#8657";
            }
            else
            {
                Session["SortDirec"] = "Ascending";
                lnkUserRights.Text = "User Type &#8659";
            }

            lnkUserName.Text = "User Name &darr; &uarr;";
            lnkName.Text = "Name &darr; &uarr;";
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

}
