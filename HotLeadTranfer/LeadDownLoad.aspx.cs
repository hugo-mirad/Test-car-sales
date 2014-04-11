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
using HotLeadBL.Leads;
using HotLeadBL.Masters;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;

public partial class LeadDownLoad : System.Web.UI.Page
{
    int c = 0; int k = 0;
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet CarsDetails = new DataSet();
    DataSet dsDropDown = new DataSet();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    UserRegistrationInfo objUserregInfo = new UserRegistrationInfo();
    HotLeadsBL objHotLeadBL = new HotLeadsBL();
    LeadsDownloadBL objLeadDownload = new LeadsDownloadBL();
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
                    //LoadVehicletype();
                    FillCenters();
                    FillStates();
                    //FillCenterRights();
                    //LoadDDL();
                }
            }
        }
    }

    //protected override void OnUnload(EventArgs e)
    //{

    //}

    private void LoadVehicletype()
    {
        try
        {
            VehicleTypeBL objVehicleTypeBL = new VehicleTypeBL();

            DataSet dsVehicleTypes = new DataSet();

            if (Cache["VehicleType"] == null)
            {
                dsVehicleTypes = objVehicleTypeBL.GetVehicleType();
                Cache["VehicleType"] = dsVehicleTypes;
            }
            else
            {
                dsVehicleTypes = (DataSet)Cache["VehicleType"];
            }



            ddlVehicleType.DataValueField = "VehicleTypeID";
            ddlVehicleType.DataTextField = "VehicleType";
            ddlVehicleType.DataSource = dsVehicleTypes.Tables[0];
            ddlVehicleType.DataBind();


        }
        catch (Exception ex)
        {

        }
    }

    private void FillCenters()
    {
        try
        {
            CenterBL objCenterBL = new CenterBL();

            DataSet dsVehicleTypes = new DataSet();

            if (Cache["Centers"] == null)
            {
                dsVehicleTypes = objCenterBL.GetCenters();
                Cache["Centers"] = dsVehicleTypes;
            }
            else
            {
                dsVehicleTypes = (DataSet)Cache["Centers"];
            }



            ddlCenter.DataValueField = "AgentCenterID";
            ddlCenter.DataTextField = "AgentCenterCode";
            ddlCenter.DataSource = dsVehicleTypes.Tables[0];
            ddlCenter.DataBind();


        }
        catch (Exception ex)
        {

        }
    }




    private void FillStates()
    {
        try
        {
            CenterBL objCenterBL = new CenterBL();
            DataSet dsVehicleTypes = new DataSet();


            dsVehicleTypes = objCenterBL.GetStates();

            ddlstates.DataValueField = "StateName";
            ddlstates.DataTextField = "State_Code";
            ddlstates.DataSource = dsVehicleTypes.Tables[0];
            ddlstates.DataBind();
            ddlstates.Items.Insert(0, "Select");


        }
        catch (Exception ex)
        {

        }
    }


    #region LoadUserRight

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

    #endregion LoadUserRight



    protected void btnSubmit_Click(object sender, EventArgs e)
    {


        //for (int index = 0; index < grdStateWiseLeadsAllocation.Rows.Count; index++)
        //{
        //    //
        //}

    }
    protected void btnGet_Click(object sender, EventArgs e)
    {
        try
        {
            GetResultsData();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void GetResultsData()
    {
        try
        {


            DataSet dsLeadsStats = objLeadDownload.GetAllcotedLeadsConsolidated(ddlVehicleType.SelectedValue, ddlCenter.SelectedValue, txtStartDate.Text, txtEndDate.Text);
            if (dsLeadsStats.Tables[0].Rows.Count > 0)
            {
                dvLeads.Style["display"] = "block";
                rptrDownload.DataSource = dsLeadsStats.Tables[0];
                rptrDownload.DataBind();
                dvLeads.Visible = true;
                lblResHead.Visible = false;
            }
            else
            {
                dvLeads.Style["display"] = "none";
                dvLeads.Visible = false;
                lblResHead.Visible = true;
                lblResHead.Text = "No Leads found to Download";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void rptrDownload_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        LeadsDownloadBL objLeadDownload = new LeadsDownloadBL();

        HiddenField lblStateID = (HiddenField)e.Item.FindControl("lblStateID");

        Label lblStateCode = (Label)e.Item.FindControl("lblStateCode");


        DataSet dsLeadsDownload = objLeadDownload.GetAllcotedLeadsDownload(ddlVehicleType.SelectedItem.Value, ddlCenter.SelectedItem.Value, txtStartDate.Text, txtEndDate.Text, lblStateID.Value, Session[Constants.USER_ID].ToString());


        //DataSetToExcel.Convert(dsLeadsDownload, Response, ddlVehicleType.SelectedItem.Value + lblStateCode.Text);

        DataSetToExcel.Convert(dsLeadsDownload, Response, "UCE-CarLeads-" + ddlCenter.SelectedItem.Text + "-" + String.Format("{0:yyyy-MM-dd}", System.DateTime.Now.ToString()) + ".xls");



    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        LeadsDownloadBL objLeadDownload = new LeadsDownloadBL();
        DataSet dsTotal = new DataSet();
        string strStateID = string.Empty;
        int i = 0;
        for (int index = 0; index < rptrDownload.Items.Count; index++)
        {
            HiddenField lblStateID = (HiddenField)rptrDownload.Items[index].FindControl("lblStateID");

            Label lblStateCode = (Label)rptrDownload.Items[index].FindControl("lblStateCode");

            CheckBox chk = (CheckBox)rptrDownload.Items[index].FindControl("chk");
            Label lblTobeDownload = (Label)rptrDownload.Items[index].FindControl("lblTobeDownload");

            if (chk.Checked == true)
            {
                if (lblTobeDownload.Text != "0")
                {
                    if (i == 0)
                    {
                        strStateID = "'" + lblStateID.Value + "'";
                        i = +1;
                    }
                    else
                    {
                        strStateID = strStateID + ",'" + lblStateID.Value + "'";
                    }
                }
            }
        }
        if (strStateID != "")
        {
            DataSet dsLeadsDownload = objLeadDownload.GetAllcotedLeadsDownload(ddlVehicleType.SelectedItem.Value, ddlCenter.SelectedItem.Value, txtStartDate.Text, txtEndDate.Text, strStateID, Session[Constants.USER_ID].ToString());

            if (dsLeadsDownload != null)
            {
                if (dsLeadsDownload.Tables.Count > 0)
                {
                    if (dsLeadsDownload.Tables[0].Rows.Count > 0)
                    {
                        lblLeadCnt.Text = dsLeadsDownload.Tables.Count.ToString();
                        DataView dv = GetTopDataViewRows(dsLeadsDownload.Tables[0].DefaultView, 25);

                        rptrLeads.DataSource = dv.ToTable();
                        rptrLeads.DataBind();

                        lblLeadCnt.Text = dv.ToTable().Rows.Count.ToString();
                        MpeShowLeads.Show();
                    }
                    else
                    {
                        lblErrorExists.Visible = true;
                        lblErrorExists.Text = "No leads are available for view/download";
                        mdepAlertExists.Show();
                    }
                }
                else
                {
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "No leads are available for view/download";
                    mdepAlertExists.Show();
                }
            }
            else
            {
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "No leads are available for view/download";
                mdepAlertExists.Show();
            }
        }
        else
        {
            lblErrorExists.Visible = true;
            lblErrorExists.Text = "No leads are available for view/download";
            mdepAlertExists.Show();
        }
    }

    private DataView GetTopDataViewRows(DataView dv, Int32 n)
    {
        DataTable dt = dv.Table.Clone();

        for (int i = 0; i < n - 1; i++)
        {
            if (i >= dv.Count)
            {
                break;
            }
            dt.ImportRow(dv[i].Row);
        }
        return new DataView(dt, dv.RowFilter, dv.Sort, dv.RowStateFilter);
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        LeadsDownloadBL objLeadDownload = new LeadsDownloadBL();
        DataSet dsTotal = new DataSet();
        string strStateID = string.Empty;


        for (int index = 0; index < rptrDownload.Items.Count; index++)
        {
            HiddenField lblStateID = (HiddenField)rptrDownload.Items[index].FindControl("lblStateID");

            Label lblStateCode = (Label)rptrDownload.Items[index].FindControl("lblStateCode");

            CheckBox chk = (CheckBox)rptrDownload.Items[index].FindControl("chk");

            int i = 0;
            if (chk.Checked == true)
            {
                if (i == 0)
                {
                    strStateID = lblStateID.Value;
                    i = +1;
                }
                else
                {
                    strStateID = strStateID + "," + lblStateID.Value;
                }
            }
        }

        DataSet dsLeadsDownload = objLeadDownload.GetAllcotedLeadsDownload(ddlVehicleType.SelectedItem.Value, ddlCenter.SelectedItem.Value, txtStartDate.Text, txtEndDate.Text, strStateID, Session[Constants.USER_ID].ToString());

        if (dsLeadsDownload != null)
        {
            if (dsLeadsDownload.Tables.Count > 0)
            {
                if (dsLeadsDownload.Tables[0].Rows.Count > 0)
                {
                    objLeadDownload.UpdateLeadsAllcotedLeadsStatus(ddlVehicleType.SelectedItem.Value, ddlCenter.SelectedItem.Value, txtStartDate.Text, txtEndDate.Text, strStateID, Session[Constants.USER_ID].ToString(), dsLeadsDownload.Tables[0].Rows.Count.ToString());
                    //GetResultsData();
                    //DataSetToExcel.Convert(dsLeadsDownload, "UCE-CarLeads-" + ddlCenter.SelectedItem.Text + "-" + String.Format("{0:yyyy-MM-dd}", System.DateTime.Now.ToString()) + ".xls");
                    MpeShowLeads.Hide();
                    GetResultsData();
                    Session["PageDown"] = "LeadsDownload";
                    Session["LeadsDownDataset"] = dsLeadsDownload;
                    Session["LeadsdownloadName"] = "UCE-CarLeads-" + ddlCenter.SelectedItem.Text + "-" + String.Format("{0:yyyy-MM-dd}", System.DateTime.Now.ToString()) + ".xls";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "javascript:poptasticTest('GetExcelDownload.aspx');", true);
                }
                else
                {
                    GetResultsData();
                    MpeShowLeads.Hide();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "No leads are available for view/download";
                    mdepAlertExists.Show();
                }
            }
            else
            {
                GetResultsData();
                MpeShowLeads.Hide();
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "No leads are available for view/download";
                mdepAlertExists.Show();
            }
        }
        else
        {
            GetResultsData();
            MpeShowLeads.Hide();
            lblErrorExists.Visible = true;
            lblErrorExists.Text = "No leads are available for view/download";
            mdepAlertExists.Show();
        }

    }
    protected void AllDownloaded_Click(object sender, EventArgs e)
    {

        LeadsDownloadBL objLeadDownload = new LeadsDownloadBL();
        DataSet dsTotal = new DataSet();
        string strStateID = string.Empty;

        int i = 0;

        for (int index = 0; index < rptrDownload.Items.Count; index++)
        {
            HiddenField lblStateID = (HiddenField)rptrDownload.Items[index].FindControl("lblStateID");

            Label lblStateCode = (Label)rptrDownload.Items[index].FindControl("lblStateCode");

            CheckBox chk = (CheckBox)rptrDownload.Items[index].FindControl("chk");

            if (i == 0)
            {
                strStateID = lblStateID.Value;
                i = +1;
            }
            else
            {
                strStateID = strStateID + "," + lblStateID.Value;
            }

        }

        DataSet dsLeadsDownload = objLeadDownload.GetAllcotedLeadsDownload(ddlVehicleType.SelectedItem.Value, ddlCenter.SelectedItem.Value, txtStartDate.Text, txtEndDate.Text, strStateID, Session[Constants.USER_ID].ToString());



        if (dsLeadsDownload != null)
        {
            if (dsLeadsDownload.Tables.Count > 0)
            {
                if (dsLeadsDownload.Tables[0].Rows.Count > 0)
                {

                    objLeadDownload.UpdateLeadsAllcotedLeadsStatus(ddlVehicleType.SelectedItem.Value, ddlCenter.SelectedItem.Value, txtStartDate.Text, txtEndDate.Text, strStateID, Session[Constants.USER_ID].ToString(), dsLeadsDownload.Tables[0].Rows.Count.ToString());
                    //GetResultsData();
                    //DataSetToExcel.Convert(dsLeadsDownload, "UCE-CarLeads-" + ddlCenter.SelectedItem.Text + "-" + String.Format("{0:yyyy-MM-dd}", System.DateTime.Now.ToString()) + ".xls");
                    GetResultsData();
                    Session["PageDown"] = "LeadsDownload";
                    Session["LeadsDownDataset"] = dsLeadsDownload;
                    Session["LeadsdownloadName"] = "UCE-CarLeads-" + ddlCenter.SelectedItem.Text + "-" + String.Format("{0:yyyy-MM-dd}", System.DateTime.Now.ToString()) + ".xls";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "javascript:poptasticTest('GetExcelDownload.aspx');", true);

                }
                else
                {
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "No leads are available for view/download";
                    mdepAlertExists.Show();
                }
            }
            else
            {
                lblErrorExists.Visible = true;
                lblErrorExists.Text = "No leads are available for view/download";
                mdepAlertExists.Show();
            }
        }
        else
        {
            lblErrorExists.Visible = true;
            lblErrorExists.Text = "No leads are available for view/download";
            mdepAlertExists.Show();
        }

    }
    protected void rptrDownload_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
    protected void rptrLeads_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //Label lbllblLeadQcStatus = (Label)e.Item.FindControl("lblLeadQcStatus");

            //if (lbllblLeadQcStatus.Text == "0")
            //{
            //    lbllblLeadQcStatus.Text = "Passed";
            //}


        }
    }

    public void btnpdf_Click(object sende, EventArgs e)
    {

        divck.Visible = false;
        using (var pdfDoc = new Document(PageSize.A4))
        {
            pdfDoc.SetMargins(15f, 15f, 30f, 0f);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("~/PDFS/TestM10.pdf"), FileMode.Create));
            pdfDoc.Open();
            var table1 = new PdfPTable(4);

            var table3 = new PdfPTable(3);
            var table2 = new PdfPTable(2);
            var table0 = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();


            table1.WidthPercentage = 100;
            table1.HorizontalAlignment = 0;
            table1.SpacingAfter = 10;
            float[] sglTblHdWidths = new float[3];
            sglTblHdWidths[0] = 150f;
            sglTblHdWidths[1] = 200f;
            sglTblHdWidths[2] = 385f;


            DataSet ds = getData();


            int a = ds.Tables[0].Rows.Count % 20;
            int b = 20 - a;

            for (int j = 0; j < b; j++)
            {
                ds.Tables[0].Rows.Add();

            }

            var cou = ds.Tables[0].Rows.Count;
            int pgnum = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                iTextSharp.text.Font fntTableFont1 = FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fntTableFont = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fntTableFont2 = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                //PdfPCell CellZero = new PdfPCell(new Phrase((ds.Tables[0].Rows[i]["LeadId"].ToString()), fntTableFont));
                PdfPTable stab = new PdfPTable(1);
                //stab.DefaultCell.Border = Rectangle.NO_BORDER;
                //stab.DefaultCell.BorderColor = iTextSharp.text.BaseColor.WHITE;


                PdfPCell c1 = new PdfPCell();
                if (ds.Tables[0].Rows[i]["LeadId"].ToString() != "")
                {
                    c1 = new PdfPCell(new Phrase(("#" + ds.Tables[0].Rows[i]["LeadId"].ToString()), fntTableFont));
                    c1.HorizontalAlignment = Element.ALIGN_LEFT;
                    c1.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c1);
                }
                else
                {
                    c1 = new PdfPCell(new Phrase((""), fntTableFont));
                    c1.HorizontalAlignment = Element.ALIGN_LEFT;
                    c1.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c1);
                    table1.DefaultCell.Border = Rectangle.NO_BORDER;
                }

                PdfPCell c2 = new PdfPCell();
                if (ds.Tables[0].Rows[i]["Model"].ToString() != "")
                {
                    c2 = new PdfPCell(new Phrase((ds.Tables[0].Rows[i]["Model"].ToString().Substring(0, ds.Tables[0].Rows[i]["Model"].ToString().Length - 1)), fntTableFont));
                    c2.HorizontalAlignment = Element.ALIGN_CENTER;
                    c2.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c2);
                }
                else
                {
                    c2 = new PdfPCell(new Phrase((""), fntTableFont));
                    c2.HorizontalAlignment = Element.ALIGN_CENTER;
                    c2.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c2);
                    table1.DefaultCell.Border = Rectangle.NO_BORDER;
                }

                PdfPCell c3 = new PdfPCell();
                if (ds.Tables[0].Rows[i]["LeadId"].ToString() != "")
                {
                    c3 = new PdfPCell(new Phrase((ds.Tables[0].Rows[i]["PhoneNumer"].ToString()), fntTableFont1));
                    c3.HorizontalAlignment = Element.ALIGN_CENTER;
                    c3.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c3);
                }
                else
                {

                    c3 = new PdfPCell(new Phrase((""), fntTableFont1));
                    c3.HorizontalAlignment = Element.ALIGN_CENTER;
                    c3.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c3);
                    table1.DefaultCell.Border = Rectangle.NO_BORDER;
                }


                PdfPTable c4 = new PdfPTable(2);
                c4.DefaultCell.Border = Rectangle.NO_BORDER;
                if (ds.Tables[0].Rows[i]["LeadId"].ToString() != "")
                {

                    string str1 = ds.Tables[0].Rows[i]["State"].ToString();
                    string str2 = "$" + ds.Tables[0].Rows[i]["price"].ToString();
                    if (str2.Contains('.'))
                        str2 = str2.Substring(0, str2.LastIndexOf('.'));
                    // str1=str1+str2;
                    PdfPCell pce1 = new PdfPCell(new Phrase(str1, fntTableFont));
                    PdfPCell pce2 = new PdfPCell(new Phrase(str2, fntTableFont));
                    pce1.HorizontalAlignment = Element.ALIGN_LEFT;
                    pce2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pce1.Border = Rectangle.NO_BORDER;
                    pce2.Border = Rectangle.NO_BORDER;
                    c4.AddCell(pce1);
                    c4.AddCell(pce2);
                    stab.DefaultCell.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c4);
                }
                else
                {

                    string str1 = "";
                    string str2 = "";

                    PdfPCell pce1 = new PdfPCell(new Phrase(str1, fntTableFont));
                    PdfPCell pce2 = new PdfPCell(new Phrase(str2, fntTableFont));
                    pce1.HorizontalAlignment = Element.ALIGN_LEFT;
                    pce2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pce1.Border = Rectangle.NO_BORDER;
                    pce2.Border = Rectangle.NO_BORDER;
                    stab.DefaultCell.Border = Rectangle.NO_BORDER;
                    c4.AddCell(pce1);
                    c4.AddCell(pce2);
                    stab.AddCell(c4);
                    table1.DefaultCell.Border = Rectangle.NO_BORDER;
                }


                PdfPCell c5 = new PdfPCell();
                if (ds.Tables[0].Rows[i]["LeadId"].ToString() != "")
                {
                    c5 = new PdfPCell(new Phrase(((ds.Tables[0].Rows[i]["Descriptions"].ToString() + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n")), fntTableFont));
                    c5.HorizontalAlignment = Element.ALIGN_CENTER;
                    c5.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c5);
                }
                else
                {
                    c5 = new PdfPCell(new Phrase((("")), fntTableFont));
                    c5.HorizontalAlignment = Element.ALIGN_CENTER;
                    c5.Border = Rectangle.NO_BORDER;
                    stab.AddCell(c5);
                    table1.DefaultCell.Border = Rectangle.NO_BORDER;
                }

                table1.AddCell(stab);


                if (i > 0 && table1.Rows.Count % 5 == 0 && table1.Rows.Count > 0)
                {
                    //if (k == 0)
                    //{
                    //    pdfDoc.Add(new Phrase("LEADS FOR WINNERS                          " + ddlstates.SelectedValue.ToString() + "                                           " + DateTime.Now.ToString("dd/MM/yyyy")));
                    //    k = k + 1;
                    //}

                    PdfPTable ptab = new PdfPTable(3);

                    ptab.WidthPercentage = 100;
                    PdfPCell pcell1 = new PdfPCell(new Phrase("LEADS FOR WINNERS"));
                    PdfPCell pcell2 = new PdfPCell(new Phrase(ddlstates.SelectedValue.ToString()));
                    PdfPCell pcell3 = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy") + "   " + "Page :" + (++pgnum).ToString()));
                    pcell1.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    pcell1.HorizontalAlignment = Rectangle.ALIGN_MIDDLE;
                    pcell2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    pcell3.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    pcell1.Border = Rectangle.NO_BORDER;
                    pcell2.Border = Rectangle.NO_BORDER;
                    pcell3.Border = Rectangle.NO_BORDER;
                    ptab.DefaultCell.Border = Rectangle.NO_BORDER;
                    ptab.AddCell(pcell1);
                    ptab.AddCell(pcell2);
                    ptab.AddCell(pcell3);





                    PdfDiv pdiv = new PdfDiv();
                    pdiv.ContentHeight = 100;
                    pdiv.Content.Add(new Chunk("      "));
                    pdfDoc.Add(ptab);
                    pdfDoc.Add(pdiv);
                    pdfDoc.Add(table1);

                    pdfDoc.NewPage();
                    table1.Rows.Clear();

                    c = c + 20;


                }

                table1.WidthPercentage = 100;
            }
        }
        Session["Type"] = "PDF";

        Response.Redirect("DownloadPage1Test.aspx");
      

   
    }

    public DataSet getData()
    {

        DateTime dateStart = Convert.ToDateTime(txtStartDate.Text);
        DateTime dateEnd = Convert.ToDateTime(txtEndDate.Text);
        DataSet ds = objLeadDownload.LeadsPDFGetdata(@dateEnd, dateEnd, ddlstates.SelectedItem.ToString());
        return ds;

    }


    public void btnexcel_Click(object sender, EventArgs e)
    {

        DataSet ds = getData();
        Session["Type"] = "Excel";
        //first let's clean up the response.object
        Session["ExcelDs"] = ds;
        Response.Redirect("DownloadPageTest.aspx");

    }
}