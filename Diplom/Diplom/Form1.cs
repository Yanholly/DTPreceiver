using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diplom;
using Diplom.Models;
using Newtonsoft.Json;

namespace Diplom
{
  
    public partial class Form1 : Form
    {
      

        public Form1()
        {
            InitializeComponent();
            
        }

        List<string> Districts = new List<string>(18) { "40262", "40263", "40265", "40270", "40273", "40276", "40277", "40278", "40279", "40280", "40281", "40284", "40285", "40288", "40290", "40294", "40296", "40298" };

        #region UI Event Handlers

        public string CreateRequestJson(int month, int year, string district)
        {
            GIBDDresponse sendData = new GIBDDresponse();
            sendData.data = "{\"date\":[\"YEAR:2018\"],\"ParReg\":\"40\",\"order\":{\"type\":\"1\",\"fieldName\":\"dat\"},\"reg\":\"" + district + "\",\"ind\":\"1\",\"st\":\"1\",\"en\":\"100\"}";//"{\"date\":[\"MONTHS:" + month.ToString() + "."+ year.ToString()+ "\"],\"ParReg\":\"40\",\"order\":{\"type\":\"1\",\"fieldName\":\"dat\"},\"reg\":\"" + district + "\",\"ind\":\"1\",\"st\":\"1\",\"en\":\"100\"}";
            return JsonConvert.SerializeObject(sendData);
        }

        HashSet<string> dtpndu = new HashSet<string>();

        private void cmdGo_Click(object sender, EventArgs e)
        {
            for (int year = 2015; year < 2016; year++)
               {
                   for (int month = 1; month < 2; month++)
                   {


                       foreach (var district in Districts)
                       {


                           RestClient rClient = new RestClient();
                           rClient.endPoint = txtInput.Text;
                           rClient.postJSON = CreateRequestJson(month, year, district);
                           debugOutput($"REST Client Created {DateTime.Now}||{year}|{month}||{Districts.IndexOf(district)}");
                           string strResponse = String.Empty;
                           strResponse = rClient.makeRequest();
                           GIBDDresponse gibddJson = JsonConvert.DeserializeObject<GIBDDresponse>(strResponse);
                           var desJson = JsonConvert.DeserializeObject<dynamic>(gibddJson.data);
                           //debugOutput(desJson[0].Data[0].Dt[0].dtp.val.ToString());
                           var dtpamount = 0;
                           HashSet<string> dtptypes = new HashSet<string>();
                           if(desJson != null)
                           foreach (var dtp in desJson.tab)
                           {
                               
                                   dtpndu.Add(dtp.ToString());

                               
                               /*

                           dtpamount++;
                          foreach (var varndu in dtp.infoDtp.s_pch)
                          {
                              dtpndu.Add(varndu.ToString());
                           }
                           */

                           }
                       }
                   }
               }

            foreach (var nduDtp in dtpndu)
            {
                debugOutput(nduDtp);
            }
           

            txtAmount.Text = dtpndu.Count.ToString();
         

        }

        private void cmdPaste_Click(object sender, EventArgs e)
        {
            //txtPOSTData.Text = "{\"data\": \"{\\\"date\\\":[\\\"YEAR:2018\\\"],\\\"ParReg\\\":\\\"40\\\",\\\"order\\\":{\\\"type\\\":\\\"1\\\",\\\"fieldName\\\":\\\"dat\\\"},\\\"reg\\\":\\\"40290\\\",\\\"ind\\\":\\\"1\\\",\\\"st\\\":\\\"1\\\",\\\"en\\\":\\\"300\\\"}\"}";
            txtInput.Text = "http://stat.gibdd.ru/map/getDTPCardData";
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtResponse.Text = string.Empty;
        }

        #endregion

        private void debugOutput(string strDebugText)
        {
            try
            {
                System.Diagnostics.Debug.Write(strDebugText + Environment.NewLine);
                txtResponse.Text = txtResponse.Text + strDebugText + Environment.NewLine;
                txtResponse.SelectionStart = txtResponse.TextLength;
                txtResponse.ScrollToCaret();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message, ToString() + Environment.NewLine);
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
