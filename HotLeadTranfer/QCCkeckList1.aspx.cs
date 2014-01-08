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

public partial class QCCkeckList1 : System.Web.UI.Page
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
            try
            {
                string salesId = Request.QueryString["CarId"].ToString().Trim();
                int PostingID = Convert.ToInt32(Session["AgentQCPostingID"].ToString());
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(PostingID));
                lblSalesId.Text = Cardetais.Tables[0].Rows[0]["carid"].ToString().Trim();
                DateTime Date = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["SaleDate"].ToString().Trim());
                string StrForm = string.Format("{0:MM/dd/yyyy}", Date);
                lblSalesDate.Text = StrForm.ToString().Trim();
                lblPhn.Text = objGeneralFunc.filPhnm(Cardetais.Tables[0].Rows[0]["phone"].ToString().Trim());
                lblcuName.Text = Cardetais.Tables[0].Rows[0]["sellerName"].ToString().Trim();
                lblcuName.Text += " " + Cardetais.Tables[0].Rows[0]["LastName"].ToString().Trim();
                try
                {
                    Session["CFName"] = Cardetais.Tables[0].Rows[0]["sellerName"].ToString().Trim();
                    Session["CLName"] = Cardetais.Tables[0].Rows[0]["LastName"].ToString().Trim();

                }
                catch { }

                lblVFNo.Text = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString().Trim().Trim() + "(" + Cardetais.Tables[0].Rows[0]["VoiceFileLocationName"].ToString().Trim() + ")";
                Session["VFLocation"] = Cardetais.Tables[0].Rows[0]["VoiceFileLocationName"].ToString().Trim();
                lblVF.Text = Cardetais.Tables[0].Rows[0]["VoiceRecord"].ToString().Trim();
                lblDate.Text = StrForm.ToString().Trim();
                lblcustname.Text = Cardetais.Tables[0].Rows[0]["sellerName"].ToString().Trim();
                lblcustname.Text += Cardetais.Tables[0].Rows[0]["LastName"].ToString().Trim();
                lblPn.Text = objGeneralFunc.filPhnm(Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString().Trim());
                Session["Phone"] = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString().Trim();
                lblAdd.Text = Cardetais.Tables[0].Rows[0]["address1"].ToString().Trim();
                string paymnType = Cardetais.Tables[0].Rows[0]["PaymentTypeName"].ToString().Trim();
                if (paymnType.Trim().StartsWith("Check"))
                {
                    lblpaymn.Text = "Check";
                }
                else
                    lblpaymn.Text = "CC";

                if (lblpaymn.Text == "Check")
                {
                    if (Cardetais.Tables[0].Rows[0]["city"].ToString().Trim() != "")
                        lblCity1.Text += Cardetais.Tables[0].Rows[0]["city"].ToString().Trim();
                    try
                    {
                        Session["lblCity"] = Cardetais.Tables[0].Rows[0]["city"].ToString().Trim();
                    }
                    catch { }
                    if (Cardetais.Tables[0].Rows[0]["State"].ToString() != "")
                        lblCity1.Text += ", " + Cardetais.Tables[0].Rows[0]["state"].ToString().Trim();
                    try
                    {
                        Session["State"] = Cardetais.Tables[0].Rows[0]["state"].ToString().Trim();
                    }
                    catch { }
                    if (Cardetais.Tables[0].Rows[0]["zip"].ToString() != "")
                        lblCity1.Text += " " + Cardetais.Tables[0].Rows[0]["zip"].ToString().Trim();
                    try
                    {
                        Session["Zip"] = Cardetais.Tables[0].Rows[0]["zip"].ToString().Trim();
                    }
                    catch { }

                }

                else
                {
                    if (Cardetais.Tables[0].Rows[0]["billingCity"].ToString() != "")
                        lblCity1.Text += Cardetais.Tables[0].Rows[0]["billingCity"].ToString().Trim();
                    else
                        lblCity1.Text += Cardetais.Tables[0].Rows[0]["city"].ToString().Trim();
                    try
                    {
                        Session["lblCity"] = Cardetais.Tables[0].Rows[0]["billingCity"].ToString().Trim();
                    }
                    catch { }
                    if (Cardetais.Tables[0].Rows[0]["State"].ToString() != "")
                        lblCity1.Text += ", " + Cardetais.Tables[0].Rows[0]["State_Code"].ToString().Trim();
                    try
                    {
                        Session["State"] = Cardetais.Tables[0].Rows[0]["State_Code"].ToString().Trim();
                    }
                    catch { }
                    if (Cardetais.Tables[0].Rows[0]["zip"].ToString() != "")
                        lblCity1.Text += " " + Cardetais.Tables[0].Rows[0]["zip"].ToString().Trim();
                    try
                    {
                        Session["Zip"] = Cardetais.Tables[0].Rows[0]["zip"].ToString().Trim();
                    }
                    catch { }
                }

                if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString().Trim()) == 1)
                {
                    lblpamth.Text = "Visa";
                }
                else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString().Trim()) == 2)
                {
                    lblpamth.Text = "Mastercard";
                }
                else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString().Trim()) == 3)
                {
                    lblpamth.Text = "Amex";
                }
                else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString().Trim()) == 4)
                {
                    lblpamth.Text = "Discover";
                }
                else if (Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString().Trim()) == 5)
                {
                    lblpamth.Text = "Check";
                }
                else
                {
                    lblpamth.Text = "Paypal";
                }
                lblPack.Text = "$" + Math.Round((Convert.ToDecimal(Cardetais.Tables[0].Rows[0]["Price"].ToString().Trim())), 2);
                lblCHN.Text = Cardetais.Tables[0].Rows[0]["Name"].ToString().Trim() + Cardetais.Tables[0].Rows[0]["lastname"].ToString().Trim();
                lblcrdCNo.Text = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString().Trim();
                try
                {
                    string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString().Trim();
                    string[] EXpDt = EXpDate.Split(new char[] { '/' });
                    lblExpDate.Text = EXpDt[0].ToString().Trim();
                    lblExpDate.Text += "/" + "20" + EXpDt[1].ToString().Trim();
                }
                catch { }
                lblcvv.Text = Cardetais.Tables[0].Rows[0]["cardCode"].ToString().Trim();
                lblbillAdd.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString().Trim());

                lnlbillcity.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString().Trim().Trim());
                lnlbillcity.Text += ", " + Cardetais.Tables[0].Rows[0]["State"].ToString().Trim();
                lnlbillcity.Text += " " + Cardetais.Tables[0].Rows[0]["zip"].ToString().Trim();

                //if cheque
                lblACHNam.Text = Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString().Trim();// +"," + Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString().Trim();
                Session["FName"] = Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString().Trim();
                Session["LName"] = ""; Cardetais.Tables[0].Rows[0]["cardholderLastName"].ToString().Trim();
                lblBankNam.Text = Cardetais.Tables[0].Rows[0]["bankName"].ToString().Trim();
                lblAName.Text = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString().Trim();
                lblRoutName.Text = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString().Trim();
                lblAccTyp.Text = Cardetais.Tables[0].Rows[0]["bankAccountType"].ToString().Trim();
                if (lblAccTyp.Text == "1")
                    lblAccTyp.Text = "CHECKING";
                else if (lblAccTyp.Text == "2")
                    lblAccTyp.Text = "SAVINGS";
                else if (lblAccTyp.Text == "3")
                    lblAccTyp.Text = "BUSINESSCHECKING";

                if ((Convert.ToInt32(Cardetais.Tables[0].Rows[0]["pmntType"].ToString()) == 6))
                {
                    lblTrnas.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                    lblpayemail.Text = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                }


                //lblpyschd
                string PackName2 = Cardetais.Tables[0].Rows[0]["Description"].ToString().Trim();
                Double PackCost2 = new Double();
                PackCost2 = Convert.ToDouble(Cardetais.Tables[0].Rows[0]["Price"].ToString().Trim());
                string PackAmount2 = string.Format("{0:0.00}", PackCost2).ToString().Trim();
                string lblPackag = PackName2 + " ($" + PackAmount2 + ")";
                string PackCost = lblPackag;
                string[] Pack = PackCost.Split('$');
                string[] FinalAmountSpl = Pack[1].Split(')');
                string FinalAmount = FinalAmountSpl[0].ToString().Trim();
                string txtPDAmountNow = Cardetais.Tables[0].Rows[0]["Amount1"].ToString().Trim();
                string Pstats = Cardetais.Tables[0].Rows[0]["PSStatusID1"].ToString().Trim();
                if (Pstats == "1")
                    lblpyschd.Text = "Full payment";
                else if (Pstats == "7")
                    lblpyschd.Text = "Partial payment";
                else
                    lblpyschd.Text = "None";

                txtPDDate.Text = ""; txtPDDate.Text = "";
                if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString().Trim() != "")
                {
                    DateTime PayDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate1"].ToString().Trim());
                    txtPaymentDate.Text = PayDate.ToString("MM/dd/yyyy");
                }
                if (Cardetais.Tables[0].Rows[0]["PSID2"].ToString().Trim() != "")
                {
                    if (Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString().Trim() != "")
                    {

                        DateTime PDDate = Convert.ToDateTime(Cardetais.Tables[0].Rows[0]["PaymentScheduledDate2"].ToString().Trim());
                        txtPDDate.Text = PDDate.ToString("MM/dd/yyyy");
                    }
                }
                txttodaypayment.Text = ""; txtPDAmountLater.Text = "";
                txttodaypayment.Text = txtPDAmountNow.ToString().Trim();
                if (Cardetais.Tables[0].Rows[0]["Amount2"].ToString().Trim() != "")
                    txtPDAmountLater.Text = Cardetais.Tables[0].Rows[0]["Amount2"].ToString().Trim();
                else
                    txtPDAmountLater.Text = "";
                lblEmail.Text = Cardetais.Tables[0].Rows[0]["email"].ToString().Trim();
                lblVehivleinfo.Text = Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString().Trim() + " " + Cardetais.Tables[0].Rows[0]["make"].ToString().Trim() + " " + Cardetais.Tables[0].Rows[0]["model"].ToString().Trim();
                txtNotes.Text = "";
                lblAgen.Text = Cardetais.Tables[0].Rows[0]["SaleAgent"].ToString().Trim() + "(" + Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString().Trim() + ")";
                Session["AgentLocation"] = Cardetais.Tables[0].Rows[0]["AgentCenterCode"].ToString().Trim();
                lblver.Text = Cardetais.Tables[0].Rows[0]["SaleVerifierName"].ToString() + "(" + Cardetais.Tables[0].Rows[0]["SaleverifierCenterCode"].ToString() + ")";
                Session["VerifierLocation"] = Cardetais.Tables[0].Rows[0]["SaleverifierCenterCode"].ToString();
                Session["Make"] = Cardetais.Tables[0].Rows[0]["make"].ToString();
                Session["Model"] = Cardetais.Tables[0].Rows[0]["model"].ToString();
                Session["year"] = Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString();
            }
            catch { }
        }
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
        try
        {
            string TypeAction = "Qualify";
            QCCheckListInsertData(TypeAction);
            //QC Status Update
            int status = 1;
            UpdateQCStatus(status, TypeAction);
          
        }
        catch { }
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Verified and Status is modified in QC Report.');", true);
        try
        {
            String x = "<script type='text/javascript'>window.opener.location.href='QCDataView.aspx';self.close();</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
        }
        catch { }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            string TypeAction = "Reject";
            QCCheckListInsertData(TypeAction);
            //QC Status Update
            int status = 1;
            UpdateQCStatus(status, TypeAction);

        }
        catch { }
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Rejected and Status is modified in QC Report.');", true);
        try
        {
            String x = "<script type='text/javascript'>window.opener.location.href='QCDataView.aspx';self.close();</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
        }
        catch { }

    }

    protected void btnHold_Click(object sender, EventArgs e)
    {
        try
        {
            string TypeAction = "Hold";
            QCCheckListInsertData(TypeAction);

            //QC Status Update
            int status = 3;
            UpdateQCStatus(status, TypeAction);
        }
        catch { }
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Hold and Status is modified in QC Report.');", true);
        try
        {
            String x = "<script type='text/javascript'>window.opener.location.href='QCDataView.aspx';self.close();</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
           
        }
        catch { }

    }
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            string TypeAction = "Return";
            QCCheckListInsertData(TypeAction);
            //QC Status Update
            int status = 4;
            UpdateQCStatus(status, TypeAction);
        }
        catch { }
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('QC CheckList is Return and Status is modified in QC Report.');", true);
        try
        {
            String x = "<script type='text/javascript'>window.opener.location.href='QCDataView.aspx';self.close();</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
        }
        catch { }

    }
    private void UpdateQCStatus(int Status, string TypeAction)
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

            string QCNotes = string.Empty; string txtOldQcNotes = "";
            DataSet dsDatetime = objHotLeadBL.GetDatetime();
            DateTime dtNow = Convert.ToDateTime(dsDatetime.Tables[0].Rows[0]["Datetime"].ToString());
            String UpdatedBy = Session[Constants.NAME].ToString();
            if (txtNotes.Text.Trim() != "")
            {
                string salesId = Request.QueryString["CarId"].ToString();
                DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(salesId));
                txtOldQcNotes = Cardetais.Tables[0].Rows[0]["QCNotes"].ToString();
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
            DataSet QCUpdateds = objHotLeadBL.UpdateQCStatus(QCID, QCNotes, Status, CarID, QCBY, PostingID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void QCCheckListInsertData(string Result)
    {
        try
        {
            string SalesId = ""; DateTime SaleDate; string VoiceFileNumber = "", CustomerName = "", Phone = "";
            string Package = ""; bool VoiceFileConfirmation = false; bool IsSaleDate = false, IsCustName = false, IsPhoneandAdd = false;
            string IsPhone = "", Address = ""; bool IsPackage = false, IsRefund = false, NoRefund = false;
            bool IsPayment = false; string paymentMode = "", CPamentmethod = "", CCardHolderName = "", CCreditCardNo = "";
            string CExpiryDat = ""; string CCvv = "", CBillingAddress = "", CKAccountHolderName = "", CKBankName = "";
            string CKAccountName = "", CKRoutingName = "", CKAccountType = "", PaymentScheduleType = ""; bool FullPayment = false;
            string PartialPayment = ""; string PartialToday = ""; DateTime PartialTodayDate, PartialNextDate; bool Email = false;
            bool IsRefund2 = false, NoRefund2 = false, IscustomerService = false, PartialDeposit = false; string notes = "";

            //1
            SalesId = lblSalesId.Text; SaleDate = Convert.ToDateTime(lblSalesDate.Text); VoiceFileNumber = lblVFNo.Text; CustomerName = lblcuName.Text; Phone = lblPhn.Text;
            //2
            Package = lblPack.Text;
            if (VoiceFileY.Checked == true) VoiceFileConfirmation = true; else if (VoiceFileN.Checked == true) VoiceFileConfirmation = false;
            if (VoiceQualityY.Checked == true) IsSaleDate = true; else if (VoiceQualityN.Checked == true) IsSaleDate = false;
            if (lblcustnameY.Checked == true) IsCustName = true; else if (lblcustnameN.Checked == true) IsCustName = false;
            if (PhnAddY.Checked == true) IsPhoneandAdd = true; else if (PhnAddN.Checked == true) IsPhoneandAdd = false;
            //3
            IsPhone = lblPn.Text; Address = lblAdd.Text;
            if (PackageAvailableY.Checked == true) IsPackage = true; else if (PackageAvailableN.Checked == true) IsPackage = false;
            if (Refund1Y.Checked == true) IsRefund = true; else if (Refund1N.Checked == true) IsRefund = false;
            if (RefundNY.Checked == true) NoRefund = true; else if (RefundNN.Checked == true) NoRefund = false;
            //4
            if (RefundY.Checked == true) IsPayment = true; else if (RefundN.Checked == true) IsPayment = false;
            paymentMode = lblpamth.Text; CPamentmethod = lblpamth.Text; CCardHolderName = lblCHN.Text; CCreditCardNo = lblcrdCNo.Text;
            //5
            try
            {
                CExpiryDat = lblExpDate.Text;
            }
            catch { }
            CCvv = lblcvv.Text; CBillingAddress = lblbillAdd.Text; CKAccountHolderName = lblACHNam.Text; CKBankName = lblBankNam.Text;
            //6
            CKAccountName = lblAName.Text; CKRoutingName = lblRoutName.Text; CKAccountType = lblAccTyp.Text;
            PaymentScheduleType = lblpyschd.Text; if (PayScSFullPY.Checked == true) FullPayment = true; else if (PayScSFullPY.Checked == true) FullPayment = false;

            //7
            PartialPayment = txtPDAmountLater.Text; PartialToday = txttodaypayment.Text;
            if (txtPaymentDate.Text != "")
                PartialTodayDate = Convert.ToDateTime(txtPaymentDate.Text);
            else
            {
                txtPaymentDate.Visible = false;
                PartialTodayDate = DateTime.Now;
            }

            if (txtPDDate.Text != "")
                PartialNextDate = Convert.ToDateTime(txtPDDate.Text);
            else
            {
                txtPDDate.Visible = false;
                PartialNextDate = DateTime.Now;
            }

            if (EmailY.Checked == true) Email = true; else if (EmailY.Checked == true) Email = false;

            //8
            if (Refund2Y.Checked == true) IsRefund2 = true; else if (Refund2N.Checked == true) IsRefund2 = false;
            if (NRefund2Y.Checked == true) NoRefund2 = true; else if (NRefund2N.Checked == true) NoRefund2 = false;
            if (custmservY.Checked == true) IscustomerService = true; else if (custmservN.Checked == true) IscustomerService = false;
            // if (RadioButton3.Checked == true) PartialDeposit = true; else if (RadioButton4.Checked == true) PartialDeposit = false;
            notes = txtNotes.Text;



            DataSet dsUserInfo = objdropdownBL.Usp_FinalSaveQCCheckList(SalesId, SaleDate, VoiceFileNumber, CustomerName, Phone,
                 Package, VoiceFileConfirmation, IsSaleDate, IsCustName, IsPhoneandAdd,
                IsPhone, Address, IsPackage, IsRefund, NoRefund,
                IsPayment, paymentMode, CPamentmethod, CCardHolderName, CCreditCardNo,
                 CExpiryDat, CCvv, CBillingAddress, CKAccountHolderName, CKBankName,
                 CKAccountName, CKRoutingName, CKAccountType, PaymentScheduleType, FullPayment,
                PartialPayment, PartialToday, PartialTodayDate, PartialNextDate, Email,
                 IsRefund2, NoRefund2, IscustomerService, PartialDeposit, notes);
        }
        catch { }
    }




    protected void BtnVehUpdaye_Click(object sender, EventArgs e)
    {
        try
        {
            string CarId = lblsaleId.Text;
            string PackageType = "";
            if (RadioButton3.Checked == true) PackageType = "99.99"; else if (RadioButton4.Checked == true) PackageType = "199.99"; else if (RadioButton4.Checked == true) PackageType = "299.99";
            DataSet QCUpdateds = objHotLeadBL.USP_PackageUpdate(Convert.ToInt32(CarId), PackageType);
        }
        catch { }
    }
    protected void lblPackEdit_Click(object sender, EventArgs e)
    {
        try
        {
            lblsaleId.Text = ""; SelectedPack.Text = "";
            lblsaleId.Text = lblSalesId.Text;
            SelectedPack.Text = lblPack.Text;
            MdlpackageEdit.Show();
        }
        catch { }
    }
    protected void lnkEmil_Click(object sender, EventArgs e)
    {
        try
        {
            Label1.Text = ""; Label1.Text = ""; lbloldemail.Text = "";
            Label1.Text = lblSalesId.Text;
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(lblSalesId.Text));
            lbloldemail.Text = Cardetais.Tables[0].Rows[0]["email"].ToString();
            txtEmail.Text = Cardetais.Tables[0].Rows[0]["email"].ToString();
            MdlEmailEdit.Show();
        }
        catch { }
    }
    public void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string CarId = Label1.Text;
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(Label1.Text));
            string email = Cardetais.Tables[0].Rows[0]["email"].ToString();

            DataSet QCUpdateds = objHotLeadBL.USP_EmailUpdate1(Convert.ToInt32(CarId), email.ToString());
            lblEmail.Text = txtEmail.Text;
        }
        catch { }
    }
    public void lnkPaymEdit_Click(object sender, EventArgs e)
    {
        try
        {
            chkcarid.Text = lblSalesId.Text;
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(lblSalesId.Text));

            string paymnType = Cardetais.Tables[0].Rows[0]["PaymentTypeName"].ToString();
            if (paymnType.Trim().StartsWith("Check"))
            {
                lblpaymn.Text = "Check";
            }
            else
                lblpaymn.Text = "CC";

            if (lblpaymn.Text == "Check" && lblpamth.Text != "Paypal")
            {

                chklabel.Text = Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString();
                chkbanknam.Text = Cardetais.Tables[0].Rows[0]["bankName"].ToString();
                chkaccno.Text = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                chkbankroutin.Text = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
                chkacctyp.Text = Cardetais.Tables[0].Rows[0]["bankAccountType"].ToString();


                txtchaccnam.Text = Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString();
                txtbanknam.Text = Cardetais.Tables[0].Rows[0]["bankName"].ToString();
                txtaccno.Text = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
                txtrou.Text = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();


                MdlPopCheck.Show();
            }

            else if (lblpaymn.Text != "Check" && lblpamth.Text != "Paypal")
            {
                lblCarNoP.Text = Cardetais.Tables[0].Rows[0]["cardNumber"].ToString();
                lblCVVP.Text = Cardetais.Tables[0].Rows[0]["cardCode"].ToString();
                try
                {
                    string EXpDate = Cardetais.Tables[0].Rows[0]["cardExpDt"].ToString();
                    string[] EXpDt = EXpDate.Split(new char[] { '/' });
                    lblExpP.Text = EXpDt[0].ToString();
                    lblExpP.Text += "/" + "20" + EXpDt[1].ToString();
                }
                catch { }
                lblNameP.Text = Cardetais.Tables[0].Rows[0]["Name"].ToString() + Cardetais.Tables[0].Rows[0]["lastname"].ToString();
                lblAddP1.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString());
                LblCityPP.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString().Trim());
                LblCityPP.Text += ", " + Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
                LblCityPP.Text += " " + Cardetais.Tables[0].Rows[0]["zip"].ToString();

                lblCarNoPC.Text = lblcrdCNo.Text;
                lblCVVPC.Text = lblcvv.Text;
                lblExpPC.Text = lblExpDate.Text;
                lblNamePC.Text = Cardetais.Tables[0].Rows[0]["Name"].ToString();
                tctLPName.Text = Cardetais.Tables[0].Rows[0]["lastname"].ToString();
                lblAddP1C.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingAdd"].ToString());
                LblCityPPC.Text = objGeneralFunc.ToProper(Cardetais.Tables[0].Rows[0]["billingCity"].ToString().Trim());
                FillStates();
                txtzipii.Text = Cardetais.Tables[0].Rows[0]["zip"].ToString();
                try
                {
                    txtStatePP.Text = Cardetais.Tables[0].Rows[0]["State_Code"].ToString();
                }
                catch { }
                MdlPaymEdit.Show();
            }
            if ((lblpamth.Text == "Paypal"))
            {
                lblpyaplsal.Text = lblSalesId.Text;
                lbltranpayno.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                Label8.Text = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                txttransano.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
                txtpayemail.Text = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
                MdlPPaypl.Show();
            }
        }
        catch { }
       
    }

    private void FillStates()
    {
        try
        {
            DataSet dsCenters = objdropdownBL.Usp_Get_DropDown1();
            txtStatePP.Items.Clear();
            for (int i = 0; i < dsCenters.Tables[0].Rows.Count; i++)
            {
                if (dsCenters.Tables[0].Rows[i]["State_ID"].ToString() != "0")
                {
                    ListItem list = new ListItem();
                    list.Text = dsCenters.Tables[0].Rows[i]["State_Code"].ToString();
                    list.Value = dsCenters.Tables[0].Rows[i]["State_ID"].ToString();
                    txtStatePP.Items.Add(list);
                }
            }
            txtStatePP.Items.Insert(0, new ListItem("All", "0"));
        }
        catch { }

    }
    public void lnkRecEdt_Click(object sender, EventArgs e)
    {
        try
        {
            lblSaleIdR.Text = lblSalesId.Text;
            lblVFNo1.Text = lblVF.Text;
            lblVFNoC1.Text = lblVF.Text;
            lblVFLoc1.Text = Session["VFLocation"].ToString();
            MDLVREdit.Show();
        }
        catch { }

    }
    public void lnkSaleDate_Click(object sender, EventArgs e)
    {
        try
        {
            lblSaleUp.Text = lblSalesId.Text;
            lblsaleDateU.Text = lblDate.Text;
            txtSaleDateC.Text = lblDate.Text;
            MDLSaleDateUpda.Show();
        }
        catch { }

    }
    protected void lnkCustNameC_Click(object sender, EventArgs e)
   {
       try
       {
           lblsalCus.Text = lblSalesId.Text;
           DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(lblSalesId.Text));

           lblCustNC.Text = lblcustname.Text;
           txtchanCusU.Text = Cardetais.Tables[0].Rows[0]["sellerName"].ToString();
           txtlatNamU.Text = Cardetais.Tables[0].Rows[0]["LastName"].ToString();
           MDlPopCustNameC.Show();
       }
       catch { }
    }
    protected void lnkPhnAdd_Click(object sender, EventArgs e)
    {

        try
        {
            lblsalddr.Text = lblSalesId.Text;

            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(lblSalesId.Text));
            lblPhnAdd.Text = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
            lbladdAddress.Text = Cardetais.Tables[0].Rows[0]["address1"].ToString();
            lbladdCity.Text = Cardetais.Tables[0].Rows[0]["city"].ToString().Trim() + ", " + Cardetais.Tables[0].Rows[0]["state"].ToString().Trim() + Cardetais.Tables[0].Rows[0]["zip"].ToString().Trim();

            txtcPhnC.Text = Cardetais.Tables[0].Rows[0]["PhoneNum"].ToString();
            txtCAddress.Text = Cardetais.Tables[0].Rows[0]["address1"].ToString();
            txtCityC.Text = Cardetais.Tables[0].Rows[0]["city"].ToString().Trim();
            txtSTateC.Text = Cardetais.Tables[0].Rows[0]["state"].ToString().Trim();
            TxtCZIp.Text = Cardetais.Tables[0].Rows[0]["zip"].ToString().Trim();

            MdlPopPhnAndAdd.Show();
        }
        catch { }
    }
    protected void lnkVeh_Click(object sender, EventArgs e)
    {
        try
        {
            lblvehsale.Text = lblSalesId.Text;
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(lblSalesId.Text));
            lblMake.Text = Cardetais.Tables[0].Rows[0]["make"].ToString();
            lblMaodel.Text = Cardetais.Tables[0].Rows[0]["model"].ToString(); ;
            lblYear.Text = Cardetais.Tables[0].Rows[0]["yearOfMake"].ToString();
            // txtcmake.Text = Session["Make"].ToString();
            txtyear.Text = Session["year"].ToString();
            //txtcmake
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
                txtcmake.DataSource = obj;
                txtcmake.DataTextField = "Make";
                txtcmake.DataValueField = "MakeID";
                txtcmake.DataBind();
                txtcmake.Items.Insert(0, new ListItem("Unspecified", "0"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            MdlPopVehUp.Show();
        }
        catch { }

    }

    protected void txtcmake_SelectedIndex(object sender, EventArgs e)
    {
      try
        {
            //var objModel = new List<MakesInfo>();
            //objModel = Session["AllModel"] as List<MakesInfo>;
            DataSet dsModels = Session[Constants.AllModel] as DataSet;
            int makeid = Convert.ToInt32(txtcmake.SelectedItem.Value);
            DataView dvModel = new DataView();
            DataTable dtModel = new DataTable();
            dvModel = dsModels.Tables[0].DefaultView;
            dvModel.RowFilter = "MakeID='" + makeid.ToString() + "'";
            dtModel = dvModel.ToTable();
            txtmodel.DataSource = dtModel;
            txtmodel.Items.Clear();
            txtmodel.DataTextField = "Model";
            txtmodel.DataValueField = "MakeModelID";
            txtmodel.DataBind();
            txtmodel.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
      MdlPopVehUp.Show();
     }
    protected void btnUpdateVehicl1_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.UpdateEVhicleinformation(Convert.ToInt32(txtmodel.Text), txtyear.Text, Convert.ToInt32(lblSalesId.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Vehicle Information is updated successfully.');", true);
            lblVehivleinfo.Text = txtyear.Text + " " + txtcmake.SelectedItem.ToString() + " " + txtmodel.SelectedItem.ToString();
        }
        catch { }
    }
    protected void linkPyDate_Click(object sender, EventArgs e)
    {
        lblpostUp.Text = lblSalesId.Text;
        try
        {
            lblAmountUps.Text = txttodaypayment.Text;
            lblDateUp.Text = txtPaymentDate.Text;
        }
        catch { }
        try
        {
            txtChDateUp.Text = txtPaymentDate.Text;
            txtChAmntUp.Text = txttodaypayment.Text;

        }
        catch { }
        try { 
       // lblPostDateUp.Text=
       //lblPostAmountUpse.Text=
        }
        catch { }
       // lblPostDateUp.Text = txtPDDate.Text;
        //if (txtPDAmountLater.Text != "")
        //    lblPostAmountUp.Text = "";
        //else
        //    lblPostAmountUp.Text = "";
        MdlPostDate.Show();
    }
    protected void lnkAgent_Click(object sender, EventArgs e)
    {
        try
        {
            lblAgnSale.Text = lblSalesId.Text;
            lblAgnt.Text = lblAgen.Text;
            lblVerfs.Text = lblver.Text;
            FillAgentCenters();
            FillVerifierCenters();
            MdlPAgentEdi.Show();
        }
        catch { }
    }
    private void FillAgentCenters()
    {
        try
        {
            DataSet dsCenters = objHotLeadBL.GetAllCentersData1();
            ddlAgntCents.Items.Clear();
            for (int i = 0; i < dsCenters.Tables[0].Rows.Count; i++)
            {
                if (dsCenters.Tables[0].Rows[i]["AgentCenterID"].ToString() != "0")
                {
                    ListItem list = new ListItem();
                    list.Text = dsCenters.Tables[0].Rows[i]["AgentCenterCode"].ToString();
                    list.Value = dsCenters.Tables[0].Rows[i]["AgentCenterID"].ToString();
                    ddlAgntCents.Items.Add(list);
                }
            }
            ddlAgntCents.Items.Insert(0, new ListItem("All", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillVerifierCenters()
    {
        try
        {
            DataSet dsCenters = objHotLeadBL.GetAllCentersData1();
            ddlVerifierCents.Items.Clear();
            for (int i = 0; i < dsCenters.Tables[0].Rows.Count; i++)
            {
                if (dsCenters.Tables[0].Rows[i]["AgentCenterID"].ToString() != "0")
                {
                    ListItem list = new ListItem();
                    list.Text = dsCenters.Tables[0].Rows[i]["AgentCenterCode"].ToString();
                    list.Value = dsCenters.Tables[0].Rows[i]["AgentCenterID"].ToString();
                    ddlVerifierCents.Items.Add(list);
                }
            }
            ddlVerifierCents.Items.Insert(0, new ListItem("All", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkVerifie_Click(object sender, EventArgs e)
    {
    }
    protected void ddlVerifierCents_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(Convert.ToInt32(ddlVerifierCents.SelectedValue));
            ddlverifiers.Items.Clear();
            ddlverifiers.DataSource = dsverifier;
            ddlverifiers.DataTextField = "AgentUFirstName";
            ddlverifiers.DataValueField = "AgentUID";
            ddlverifiers.DataBind();
            ddlverifiers.Items.Insert(0, new ListItem("All", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        MdlPAgentEdi.Show();

    }
    protected void ddlAgntCents_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            DataSet dsverifier = objHotLeadBL.GetAgentsForAgents(Convert.ToInt32(ddlAgntCents.SelectedItem.Value));
            ddlagents.Items.Clear();
            ddlagents.DataSource = dsverifier;
            ddlagents.DataTextField = "AgentUFirstName";
            ddlagents.DataValueField = "AgentUID";
            ddlagents.DataBind();
            ddlagents.Items.Insert(0, new ListItem("All", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        MdlPAgentEdi.Show();

    }
   
    public void Button20_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlAgntCents.Text != "0" || ddlVerifierCents.Text != "0")
            {
                int a = Convert.ToInt32(ddlagents.SelectedValue), b = Convert.ToInt32(ddlverifiers.SelectedValue), c = Convert.ToInt32(lblAgnSale.Text);
                DataSet dsverifier = objHotLeadBL.UpdateAgentVerifier(Convert.ToInt32(ddlagents.SelectedValue), Convert.ToInt32(ddlverifiers.SelectedValue), Convert.ToInt32(lblAgnSale.Text));
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Agent and Verifier are updated successfully.');", true);
                lblAgen.Text = ddlagents.SelectedItem.ToString() + "(" + ddlAgntCents.SelectedItem.ToString()+")";
                lblver.Text = ddlverifiers.SelectedItem.ToString() + "(" + ddlVerifierCents.SelectedItem.ToString() + ")";
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Please select Agent and Center details to change.');", true);
                MdlPAgentEdi.Show();
            }
        }
        catch { }
    }
    //USP_VoiceRecordUpdate
    public void Button5_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.VoiceRecordUpdate((lblVFNoC1.Text), Convert.ToInt16(lblSalesId.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Voice Record Number is updated successfully.');", true);
            lblVF.Text = lblVFNoC1.Text;
        }
        catch { }
    }
    public void BtnDaleDate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.SaleDateUpdate(Convert.ToDateTime(txtSaleDateC.Text), Convert.ToInt16(lblSalesId.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('SaleDate is updated successfully.');", true);
            lblDate.Text = txtSaleDateC.Text;
        }
        catch { }
    }
    public void BtncustnUpda_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.CustomerNameUpdates(txtchanCusU.Text, txtlatNamU.Text, Convert.ToInt16(lblSalesId.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Customer name is updated successfully.');", true);
            lblcustname.Text = txtchanCusU.Text;
            lblcustname.Text += " " + txtlatNamU.Text;
        }
        catch { }
    }
    public void btnCustAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.CusomerAddressUpdates(txtcPhnC.Text, txtCAddress.Text, txtCityC.Text, txtSTateC.Text, TxtCZIp.Text, Convert.ToInt16(lblSalesId.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Customer Address is updated successfully.');", true);
            
              lblPn.Text = objGeneralFunc.filPhnm(txtcPhnC.Text);
              lblAdd.Text =txtCAddress.Text;
              lblCity1.Text = txtCityC.Text + ", " + txtSTateC.Text + " " + TxtCZIp.Text;
        }
        catch { }
    }
    protected void BtnPacgeUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string Pack = "";
            if (RadioButton3.Checked == true)
            {
                Pack = "99.99"; lblPack.Text = "$99.99";
            }
            else if (RadioButton4.Checked == true) { Pack = "199.99"; lblPack.Text = "$199.99"; }
            else if (RadioButton5.Checked == true) { Pack = "299.99"; lblPack.Text = "$929.99"; }
            DataSet dsverifier = objHotLeadBL.UpdatePackages(Pack, Convert.ToInt16(lblSalesId.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Package Amount is updated successfully.');", true);
        }
        catch { }
        
    }
    //USP_PaymentDetailsData
    public void BtnPaymInfoUpdat_Click(object sendder, EventArgs e)
    {
        try
        {
            if (txtStatePP.SelectedValue.ToString() != "0")
            {
                try
                {
                    string EXpDate = lblExpPC.Text;
                    string[] EXpDt = EXpDate.Split(new char[] { '/' });

                    ListItem liExpMnth = new ListItem();
                    liExpMnth.Text = EXpDt[0].ToString();
                    liExpMnth.Value = EXpDt[0].ToString();
                    lblExpPC.Text = EXpDt[0].ToString() + EXpDt[1].ToString();
                    string year = EXpDt[1].ToString();
                    var result = year.Substring(year.Length - Math.Min(2, year.Length));
                    lblExpPC.Text = liExpMnth.Value + "/" + result.ToString();
                }
                catch { }
              
                DataSet dsverifier = objHotLeadBL.USP_PaymentDetailsData(lblCarNoPC.Text, lblCVVPC.Text, lblExpPC.Text, lblNamePC.Text,
                tctLPName.Text, lblAddP1C.Text, LblCityPPC.Text, txtStatePP.SelectedValue, txtzipii.Text, Convert.ToInt16(lblSalesId.Text));
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Payment Info is updated successfully.');", true);
                lblcrdCNo.Text = lblCarNoPC.Text;
                lblcvv.Text = lblCVVPC.Text;
                lblExpDate.Text = lblExpPC.Text;
                lblCHN.Text = lblNamePC.Text + " " + tctLPName.Text;
                lblbillAdd.Text = lblAddP1C.Text; lnlbillcity.Text = LblCityPPC.Text.Trim() + ", " + txtStatePP.SelectedValue + " " + txtzipii.Text;
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Please select state .');", true);
                MdlPaymEdit.Show();
            }
        }
        catch { }
    }
    public void BtnPostAmount_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.UpdatePaymentScheduledDate(Convert.ToDateTime(txtChDateUp.Text), txtChAmntUp.Text, Convert.ToInt16(lblSalesId.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Payment Amount Information is updated successfully.');", true);
            txtPaymentDate.Text = txtChDateUp.Text;
            txttodaypayment.Text = txtChAmntUp.Text;
        }
        catch { }
    }

    public void chkupdate_Click(object sender, EventArgs e)
    {

        try
        {
            DataSet dsverifier = objHotLeadBL.UpadeChyeckPay(txtchaccnam.Text, txtaccno.Text, txtaccno.Text, txtbanknam.Text, Convert.ToInt32(chkcarid.Text));

            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(chkcarid.Text));
            lblACHNam.Text = Cardetais.Tables[0].Rows[0]["bankAccountHolderName"].ToString();
            lblAName.Text = Cardetais.Tables[0].Rows[0]["bankAccountNumber"].ToString();
            lblBankNam.Text = Cardetais.Tables[0].Rows[0]["bankName"].ToString();
            lblRoutName.Text = Cardetais.Tables[0].Rows[0]["bankRouting"].ToString();
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Check Payment Information is updated successfully.');", true);
        }
        catch { }

    }
    protected void btnppayemail_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsverifier = objHotLeadBL.PaymentDetailsDataPaypal(txttransano.Text, txtpayemail.Text, Convert.ToInt16(lblpyaplsal.Text));
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "alert('Payment Paypal Info is updated successfully.');", true);
            DataSet Cardetais = objHotLeadBL.GetCarDetailsByPostingID(Convert.ToInt32(lblpyaplsal.Text));
            lblTrnas.Text = Cardetais.Tables[0].Rows[0]["TransactionID"].ToString();
            lblpayemail.Text = Cardetais.Tables[0].Rows[0]["PaypalEmail"].ToString();
        }
        catch { }
    } 
   
}