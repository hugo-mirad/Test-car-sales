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

public partial class Ticker : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            string CenterIDGet = Request.QueryString["CID"].ToString();
            string CenterCodeGet = Request.QueryString["CNAME"].ToString();
            string CenterCode = CenterCodeGet;
            int CenterID = Convert.ToInt32(CenterIDGet.ToString());
            getdetails(CenterCode, CenterID);
        }
    }

    private void getdetails(string CenterCode, int CenterID)
     {
        try
        {
            ViewState["OpenStatus"] = "0";
            DataSet dsData = objHotLeadBL.GetAllSalesByCenterForTicker1(CenterID);
            lblCenterCode.Text = CenterCode.ToString();
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            lblDatetime.Text = dtNow.ToString("MM/dd/yyyy hh:mm tt");
           //P int TotalSales = Convert.ToInt32(dsData.Tables[0].Compute("sum(Count)", ""));
            int TotalSales = Convert.ToInt32(dsData.Tables[0].Compute("sum(ACount)", ""));
           //p Double TotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(TotalAmount)", ""));
            Double TotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(ATotalAmount)", ""));
            //Discount amount 
            Double DiscountTotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(ADiscountAmount)", ""));

            lblTotalsales.Text = TotalSales.ToString() + " ($" + string.Format("{0:0.00}", TotalAmount).ToString() + ")";
            if (DiscountTotalAmount  != 0.0)
             lblTotalsales.Text += "- ($"+ string.Format("{0:0.00}", DiscountTotalAmount).ToString() + ")";

            int VTotalSales = Convert.ToInt32(dsData.Tables[0].Compute("sum(VCount)", ""));
            Double VTotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(VTotalAmount)", ""));
            //Verifier total Discount Amount
            Double DiscountVTotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(VDiscountAmount)", ""));
            lblVTotalsales.Text = VTotalSales.ToString() + " ($" + string.Format("{0:0.00}", VTotalAmount).ToString() + ")";
            if (DiscountVTotalAmount != 0.0)
                lblVTotalsales.Text += "- ($" + string.Format("{0:0.00}", DiscountVTotalAmount).ToString() + ")";

            LiveSalesRepeater.Visible = false;
            AllCenters.Visible = false;
            btnAgentRefresh.Visible = false;
            btnSummary.Visible = false;
            btnSummaryRefresh.Visible = true;
            btnDetails.Visible = true;
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
            string CenterIDGet = Request.QueryString["CID"].ToString();
            string CenterCodeGet = Request.QueryString["CNAME"].ToString();
            string CenterCode = CenterCodeGet;
            int CenterID = Convert.ToInt32(CenterIDGet.ToString());
            getdetails(CenterCode, CenterID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnAgentRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            string CenterIDGet = Request.QueryString["CID"].ToString();
            string CenterCodeGet = Request.QueryString["CNAME"].ToString();
            string CenterCode = CenterCodeGet;
            int CenterID = Convert.ToInt32(CenterIDGet.ToString());
            getDetailsInfo(CenterCode, CenterID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void getDetailsInfo(string CenterCode, int CenterID)
    {
        try
        {
            ViewState["OpenStatus"] = "1";
            DataSet dsData = objHotLeadBL.GetAllSalesByCenterForTicker1(CenterID);
            DataSet dsAllCenters = objHotLeadBL.GetAllCenterSalesByCenterForTicker1(CenterID);
            lblCenterCode.Text = CenterCode.ToString();
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            lblDatetime.Text = dtNow.ToString("MM/dd/yyyy hh:mm tt");
            int TotalSales = Convert.ToInt32(dsData.Tables[0].Compute("sum(ACount)", ""));
            Double TotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(ATotalAmount)", ""));
            //For Calculating discount amount
            Double DiscountTotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(ADiscountAmount)", ""));
            lblTotalsales.Text = TotalSales.ToString() + " ($" + string.Format("{0:0.00}", TotalAmount).ToString() + ")";
            if(DiscountTotalAmount != 0.00 )
               lblTotalsales.Text +=   "- ($" + string.Format("{0:0.00}", DiscountTotalAmount).ToString() + ")";
            int VTotalSales = Convert.ToInt32(dsData.Tables[0].Compute("sum(VCount)", ""));
            Double VTotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(VTotalAmount)", ""));
            //Discount amount for verifier
            Double DiscountVTotalAmount = Convert.ToDouble(dsData.Tables[0].Compute("sum(VDiscountAmount)", ""));

             lblVTotalsales.Text = VTotalSales.ToString() + " ($" + string.Format("{0:0.00}", VTotalAmount).ToString() + ")"; 
            if(DiscountVTotalAmount != 0.00)
                lblVTotalsales.Text += "-" + string.Format("{0:0.00}", DiscountVTotalAmount).ToString() + ")";
            LiveSalesRepeater.Visible = true;
            btnAgentRefresh.Visible = true;
            btnSummary.Visible = true;
            btnSummaryRefresh.Visible = false;
            btnDetails.Visible = false;
            AllCenters.Visible = true;
            if (dsData.Tables[0].Rows.Count > 0)
            {
                LiveSalesRepeater.DataSource = dsData.Tables[0];
                LiveSalesRepeater.DataBind();
            }
            if (dsAllCenters.Tables[0].Rows.Count > 0)
            {
                AllCenters.DataSource = dsAllCenters.Tables[0];
                AllCenters.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void LiveSalesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblAgnetSales = (Label)e.Item.FindControl("lblAgnetSales");
                HiddenField hdnAgentSalesCount = (HiddenField)e.Item.FindControl("hdnAgentSalesCount");
                HiddenField hdnAgentAmount = (HiddenField)e.Item.FindControl("hdnAgentAmount");
                HiddenField hdnAgentDiscount = (HiddenField)e.Item.FindControl("hdnAgentDsicount");
                lblAgnetSales.Text = hdnAgentSalesCount.Value + " ($" + string.Format("{0:0.00}", hdnAgentAmount.Value).ToString() + ")";
                    if(hdnAgentDiscount.Value!="0.00")
                        lblAgnetSales.Text += "- ($" + string.Format("{0:0.00}", hdnAgentDiscount.Value).ToString() + ")";

                Label lblVSales = (Label)e.Item.FindControl("lblVSales1");
                HiddenField hdnVSalesCount = (HiddenField)e.Item.FindControl("hdnVSalesCount");
                HiddenField hdnVAmount = (HiddenField)e.Item.FindControl("hdnVAmount");
                HiddenField hdnVDiscount = (HiddenField)e.Item.FindControl("hdnVDiscount");

                lblVSales.Text = hdnVSalesCount.Value + " ($" + string.Format("{0:0.00}", hdnVAmount.Value).ToString() + ")";
                    if(hdnVDiscount.Value!="0.00")
                  lblVSales.Text += "- ($" +string.Format("{0:0.00}", hdnVDiscount.Value).ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void AllCenters_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblAgnetSales = (Label)e.Item.FindControl("lblCenterSales");
                HiddenField hdnAgentSalesCount = (HiddenField)e.Item.FindControl("hdnCenterSalesCount");
                HiddenField hdnAgentAmount = (HiddenField)e.Item.FindControl("hdnCenterAmount");
                HiddenField hdnAgentDiscount = (HiddenField)e.Item.FindControl("hdnCenterDiscount");

                lblAgnetSales.Text = hdnAgentSalesCount.Value + " ($" + string.Format("{0:0.00}", hdnAgentAmount.Value.ToString()).ToString() + ")";
                    if(hdnAgentDiscount.Value!="0.00")
                   lblAgnetSales.Text +=  "- ($"+ string.Format("{0:0.00}", hdnAgentDiscount.Value.ToString()).ToString() +")";
                
                Label lblVSales = (Label)e.Item.FindControl("lblCenterVSales");
                HiddenField hdnVSalesCount = (HiddenField)e.Item.FindControl("hdnCenterVSalesCount");
                HiddenField hdnVAmount = (HiddenField)e.Item.FindControl("hdnCenterVAmount");
                HiddenField hdnVDiscount = (HiddenField)e.Item.FindControl("hdnCenterVDiscount");
                lblVSales.Text = hdnVSalesCount.Value + "($" + string.Format("{0:0.00}", hdnVAmount.Value.ToString()) + ")";
                  if(hdnVDiscount.Value!="0.00")  
                   lblVSales.Text += "- ($" + string.Format("{0:0.00}", hdnVDiscount.Value.ToString()) + ")";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void IntervalTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["OpenStatus"].ToString() == "1")
            {
                string CenterIDGet = Request.QueryString["CID"].ToString();
                string CenterCodeGet = Request.QueryString["CNAME"].ToString();
                string CenterCode = CenterCodeGet;
                int CenterID = Convert.ToInt32(CenterIDGet.ToString());
                getDetailsInfo(CenterCode, CenterID);
            }
            else
            {
                string CenterIDGet = Request.QueryString["CID"].ToString();
                string CenterCodeGet = Request.QueryString["CNAME"].ToString();
                string CenterCode = CenterCodeGet;
                int CenterID = Convert.ToInt32(CenterIDGet.ToString());
                getdetails(CenterCode, CenterID);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
