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

public partial class QCVerifyReport : System.Web.UI.Page
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
       
    }

    private void getdetails(string CenterCode, int CenterID)
    {
        
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
    protected void btnQuali_Click(object sender, EventArgs e)
    {
        string TypeAction = "Qualify";
        QCCheckListInsertData(TypeAction);
        //QC Status Update
        int status = 1;
        UpdateQCStatus(status, TypeAction);
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Verified and Status is modified in QC Report.');", true);
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {

        string TypeAction = "Reject";
        QCCheckListInsertData(TypeAction);
      
        //QC Status Update
        int status = 1;
        UpdateQCStatus(status, TypeAction);
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Rejected and Status is modified in QC Report.');", true);


    }
  
    protected void btnHold_Click(object sender, EventArgs e)
    {
        string TypeAction = "Hold";
        QCCheckListInsertData(TypeAction);
       
        //QC Status Update
        int status = 3;
        UpdateQCStatus(status, TypeAction);
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Hold and Status is modified in QC Report.');", true);

    }
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        string TypeAction = "Return";
        QCCheckListInsertData(TypeAction);
        //QC Status Update
        int status = 4;
        UpdateQCStatus(status, TypeAction);
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Return and Status is modified in QC Report.');", true);

    }
    public void QCCheckListInsertData(string Result)
    {
        string Package = ""; bool sellerName = false, sellerPhone = false, Email = false, Address = false;
        bool makeModel = false;

        string paymentMode = ""; bool CPamentmethod = false, CCardHolderName = false, CCreditCardNo = false,
        CExpiryDate = false, CCvv = false, CBillingAddress = false;

        bool CheckaccounHName = false, CKBankName = false, CKAccountName = false, CKRoutingName = false, CKAccountType = false;
        bool PaymentScheduleType = false, FullPayment = false, PartialPayment = false; string PartialToday = "", PartialNext = "";

        string txtvoicefileconf = "", Notes = "", TypeofAction = "";
        //4 Fields
        string salesId = Request.QueryString["CarId"].ToString();
        int QCVId = 1;
        if (ChkPaY.Checked == true) Package = "99Y"; else if (ChkPaN.Checked == true) Package = "199Y";
        string PackageText = PackEnter.Text;

        //4 fields
        if (ChksellNameY.Checked == true) sellerName = true; else if (ChksellNameN.Checked == true) sellerName = false;
        if (ChksellPhnY.Checked == true) sellerPhone = true; else if (ChksellPhnN.Checked == true) sellerPhone = false;
        if (ChkEmailY.Checked == true) Email = true; else if (ChkEmailN.Checked == true) Email = false;
        if (ChkAddrY.Checked == true) Address = true; else if (ChkAddrN.Checked == true) Address = false;

        //makeModel
        if (chkmakmodlY.Checked == true) makeModel = true; else if (chkmakmodlY.Checked == true) makeModel = false;

        //7 fileds
        if (PaymentModeCC.Checked == true) paymentMode = "CC"; else if (PaymentModeCheck.Checked == false) paymentMode = "Check";
        if (chkPaymethodY.Checked == true) CPamentmethod = true; else if (chkPaymethodN.Checked == true) CPamentmethod = false;
        if (ChkHnameY.Checked == true) CCardHolderName = true; else if (ChkHnameN.Checked == true) CCardHolderName = false;
        if (ChkCredCardY.Checked == true) CCreditCardNo = true; else if (ChkCredCardN.Checked == true) CCreditCardNo = false;
        if (chkExpDateY.Checked == true) CExpiryDate = true; else if (chkExpDateN.Checked == true) CExpiryDate = false;
        if (chkCvvY.Checked == true) CCvv = true; else if (chkCvvN.Checked == true) CCvv = false;
        if (ChkBillAddY.Checked == true) CBillingAddress = true; else if (ChkBillAddn.Checked == true) CBillingAddress = false;

        //5 fields
        if (ckAccHNameY.Checked == true) CheckaccounHName = true; else if (ckAccHNameN.Checked == true) CheckaccounHName = false;
        if (ChkBankNameY.Checked == true) CKBankName = true; else if (ChkBankNameN.Checked == true) CKBankName = false;
        if (ChkAccNameY.Checked == true) CKAccountName = true; else if (ChkAccNameN.Checked == true) CKAccountName = false;
        if (chkRoutNoY.Checked == true) CKRoutingName = true; else if (chkRoutNoN.Checked == true) CKRoutingName = false;
        if (ChkAccTypY.Checked == true) CKAccountType = true; else if (ChkAccTypN.Checked == true) CKAccountType = false;


        //5 fields
        if (rbtFullPay.Checked == true) PaymentScheduleType = true; else if (rbtPartialPay.Checked == false) PaymentScheduleType = false;
        if (PayScSFullPY.Checked == true) FullPayment = true; else if (PayScSFullPN.Checked == true) FullPayment = false;
        if (ChkAccNameY.Checked == true) PartialPayment = true; else if (ChkAccNameN.Checked == true) PartialPayment = false;
        PartialToday = txttodaypayment.Text;
        PartialNext = txtnextpay.Text;


        //2 fields
        txtvoicefileconf = txtnextpay.Text; Notes = txtNotes.Text; TypeofAction = "Qualified";

        //DataSet dsUserInfo = objdropdownBL.Usp_SaveQCCheckList(salesId, Package, PackageText,
        //                                                       sellerName, sellerPhone, Email, Address,
        //                                                       makeModel,
        //                paymentMode, CPamentmethod, CCardHolderName, CCreditCardNo, CExpiryDate, CCvv, CBillingAddress,
        //                CheckaccounHName, CKBankName, CKAccountName, CKRoutingName, CKAccountType,
        //                FullPayment, PartialPayment, PartialToday, PartialNext,
        //                txtvoicefileconf, Notes, Result);
    }
    private void UpdateQCStatus(int Status,string TypeAction)
    {

        // 1 QC Approved   2 QC Reject 3 QC Pending 4 QC Returned
        if (TypeAction == "Qualify")
            Status = 1;
        else if (TypeAction == "Reject")
            Status = 2;
        else if (TypeAction == "Hold")
            Status = 3;
        if (TypeAction == "Return")
            Status = 4;
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
            string QCNotes = string.Empty; string txtOldQcNotes = "";
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            String UpdatedBy = Session[Constants.NAME].ToString();
            if (txtNotes.Text.Trim() != "")
            {
                string salesId = Request.QueryString["CarId"].ToString();
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(salesId));
                txtOldQcNotes = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();
                string UpdateByWithDate = dtNow.ToString("MM/dd/yyyy hh:mm tt") + "-" + UpdatedBy + "\n";
                if (txtOldQcNotes.Trim() != "")
                {
                    QCNotes = txtOldQcNotes.Trim() + "\n" + UpdateByWithDate + txtNotes.Text.Trim() + "\n" + "-------------------------------------------------";
                }
                else
                {
                    QCNotes = UpdateByWithDate + txtNotes.Text.Trim() + "\n" + "-------------------------------------------------";
                }
            }
            else
            {
                QCNotes = txtOldQcNotes.Trim();
            }
            int CarID = Convert.ToInt32(Session["AgentQCCarID"].ToString());
            int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
            DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(CarID, QCNotes, Status, CarID, QCBY, CarID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    protected void PaymentModeCC_CheckedChanged(object sender, EventArgs e)
    {
        if (PaymentModeCC.Checked == true)
            dvcreditcard.Visible = true;
            divcheque.Visible = false;

    }
    protected void PaymentModeCheck_CheckedChanged(object sender, EventArgs e)
    {
        if (PaymentModeCheck.Checked == true)
            divcheque.Visible = true;
            dvcreditcard.Visible = false;
    }
    protected void rbtFullPay_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtFullPay.Checked == true)
            divfullpaym.Visible = true;
            divPartialpaym.Visible = false;
       
    }
    protected void rbtPartialPay_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtPartialPay.Checked == true)
            divPartialpaym.Visible = true;
            divfullpaym.Visible = false;
       
    }
    
}
