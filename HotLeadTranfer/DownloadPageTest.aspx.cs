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

public partial class DownloadPageTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Type"].ToString() == "Excel")
        {
            DataSet ds = Session["ExcelDs"] as DataSet;
            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=ExcelName.xls");
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
            System.Web.UI.WebControls.DataGrid dg = new System.Web.UI.WebControls.DataGrid();
            dg.DataSource = ds.Tables[0];
            dg.DataBind();
            dg.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
        else
        {
            Response.ContentType = "application/pdf";
            string path = Server.MapPath("~/PDFS/TestM10.pdf");
            byte[] bts = System.IO.File.ReadAllBytes(path);
            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("Content-Type", "Application/octet-stream");//octet-stream
            Response.AddHeader("Content-Length", bts.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment;   filename=1.pdf");
            Response.BinaryWrite(bts);
            Response.Flush();
            Response.TransmitFile(path);
            Response.End();
        }
    }
}
