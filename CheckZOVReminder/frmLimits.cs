using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace CheckZOVReminder
{
    public struct YaLimits
    {

        public String
            DateTimeFrom, //from
            DateTimeTo,   //to
            Quntity;      //Quntity

        public YaLimits(
                            String dtFrom,
                            String dtTo,
                            String iQuntity
                        )
        {
            this.DateTimeFrom = dtFrom;
            this.DateTimeTo = dtTo;
            this.Quntity = iQuntity;
        }
    }

    public partial class frmLimits : Form
    {

        public frmLimits()
        {
            InitializeComponent();
        }

        private void frmLimits_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        private List<YaLimits> CollectLimitsTo(XDocument xDocument)
        {
            //Лист структур YaSearchResult, который метод в итоге возвращает.
            List<YaLimits> ret = new List<YaLimits>();

            //из полученного XML'я выдираем все элементы с именем "time-interval" - это результаты поиска
            var groupQuery = from gr in xDocument.Elements().
                Elements("response").
                Elements("limits").
                Elements("time-interval")
                select gr;
            //DateTime dtFrom, dtTo;
            //каждый элемент group преобразовывается в объект SearchResult
            for (int i = 0; i < groupQuery.Count(); i++)
            {
                string strFrom = GetAttribute(groupQuery.ElementAt(i), "from");
                string strTo = GetAttribute(groupQuery.ElementAt(i), "to");
                //string strTo = GetValue(groupQuery.ElementAt(i), "time-interval");
                string iQuantity = GetValue(groupQuery.ElementAt(i), "");
                //string indexedTimeQuery = GetValue(groupQuery.ElementAt(i), "modtime");
                //string cacheUrlQuery = GetValue(groupQuery.ElementAt(i), "saved-copy-url");
                ret.Add(new YaLimits(strFrom, strTo, iQuantity));
            }

            return ret;


        }

        private static string GetValue(XElement group, string name)
        {
            try
            {
                return group.Value;
            }
                //это если в результате нету элемента с каким то именем,
                //то будет вместо значащей строчки возвращаться пустая.
            catch
            {
                return string.Empty;
            }
        }

        private static string GetAttribute(XElement group, string name)
        {
            try
            {
                return group.Attribute(name).Value;
            }
                //это если в результате нету элемента с каким то именем,
                //то будет вместо значащей строчки возвращаться пустая.
            catch
            {
                return string.Empty;
            }
        }

        private void FillListView(List<YaLimits> yaSearchResults)
        {
            DateTimeOffset localTime = new DateTimeOffset(DateTime.Now).ToOffset(TimeSpan.Zero);
            ListViewItem li;
            lvLimits.Items.Clear();
            foreach (YaLimits yaLimits in yaSearchResults)
            {
                li = lvLimits.Items.Add(yaLimits.DateTimeFrom);
                li.SubItems.Add(new ListViewItem.ListViewSubItem(li, yaLimits.DateTimeTo));
                li.SubItems.Add(new ListViewItem.ListViewSubItem(li, yaLimits.Quntity));
                DateTime dtFrom = DateTime.Parse(yaLimits.DateTimeFrom);
                DateTime dtTo = DateTime.Parse(yaLimits.DateTimeTo);
                li.Selected = ((dtFrom > localTime) && (localTime < dtTo));
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowData();
        }

        private void ShowData()
        {
            string url =
                "http://xmlsearch.yandex.ru/xmlsearch?action=limits-info&user={0}&key={1}";
            string yauser = "ryurik1978";
            string yakey = "03.108365218:d079b13c7f27666c7d0e8fcd01e84872";
            string completeUrl = String.Format(url, yauser, yakey);

            Trace.WriteLine(completeUrl);
            //Объект, отсылающий запрос.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(completeUrl);
            //Получение ответа.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            XmlReader xmlReader = XmlReader.Create(response.GetResponseStream());
            XDocument xmlResponse = XDocument.Load(xmlReader);

            List<YaLimits> yaSearchResults = CollectLimitsTo(xmlResponse);
            FillListView(yaSearchResults);
        }
    }
}