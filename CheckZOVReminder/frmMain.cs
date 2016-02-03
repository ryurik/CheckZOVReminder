using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Hosting;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using _Excel = Microsoft.Office.Interop.Excel;


namespace CheckZOVReminder
{
    // яндекс.директ
    public struct YaSearchResult
    {

        public string DisplayUrl, //url
                      CacheUrl, //saved-copy-url
                      Title, //title
                      Description, //headline
                      IndexedTime; //modtime

        public YaSearchResult(string url,
                   string cacheUrl,
                   string title,
                   string description,
                   string indexedTime)
        {
            this.DisplayUrl = url;
            this.CacheUrl = cacheUrl;
            this.Title = title;
            this.Description = description;
            this.IndexedTime = indexedTime;
        }
    }
    // главный поиск
    public struct MainSearchResult
    {

        public string Url,//url
            Title,//title
            Description; //headline

        public MainSearchResult(string url,
                   string title,
                   string description)
        {
            this.Url = url;
            this.Title = title;
            this.Description = description;
        }
    }

    // работа с аргументами при старте
    public struct CommandLineArgs
    {
        private string[] m_arrArgs;
        public CommandLineArgs(string[] pArrArgs)
        {
            m_arrArgs = pArrArgs;
        }

        public string this[string pSArgumentName]
        {
            get
            {
                if (m_arrArgs == null) return null; // no args
                for (int i = 0; i < m_arrArgs.Length; i++)
                {
                    if (m_arrArgs[i] == pSArgumentName)
                    {
                        if (m_arrArgs.Length == i + 1) return null; // no value
                        if (m_arrArgs[i + 1].StartsWith("--")) return null; // no value
                        return m_arrArgs[i + 1]; // return value
                    }
                }
                return null; // key not found
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class frmMain : Form
    {
        public Boolean AutoClose = false;
//        private FirefoxDriver _ffDriver;
        private ChromeDriver _chromeDriver;
        private WebDriverWait _wait;
        private String _currentRegion;
        private String CurrentRegion {
            get
            {return (_currentRegion.Equals("Неопознан")) && (_chromeDriver != null)
                    ? _chromeDriver.FindElement(By.ClassName(Properties.Settings.Default.RegionChange)).Text
                    : _currentRegion;
            } 
            set { _currentRegion = value; }
        }
        private String _currentSearchText;
        private List<String> relatedWords = new List<string>(); // всегда вносится запрос на 0 позицию
        private DateTime _queryDateTime;
        private SqlConnection _sqlcon;
        private SqlCommand _sqlCmdInsertToZOVRu = new SqlCommand();
        private SqlCommand _sqlCmdInsertToZOVRuResults = new SqlCommand();
        private SqlCommand _sqlCmdInsertToZOVRuDirect = new SqlCommand();
        private Boolean _IsRyurik;


        ///
        ///                                   
        //public const string PathToRegions = "D:\\Work\\C#\\CheckZOVReminder\\CheckZOVReminder\\Regions.xlsx"; // HOME
        //public const string PathToRegions = "D:\\Work\\C#\\2048\\CheckZOVReminder\\CheckZOVReminder\\Regions.xlsx"; //WORK


        /// 
        /// 
        /// 

        //public const string btnClassName = ".button.suggest2-form__button.button_size_m.button_theme_normal.i-bem.button_js_inited";

        //public string topDirect = "descendant-or-self::div[contains(@class, 'serp-block serp-adv serp-adv_label_yes serp-adv_compact_no serp-block-')]";


        // класс для самих ссылок
        //public string firstblocklinksClassName = "descendant-or-self::div[contains(@class, 'serp-block serp-block-')]";
            // главные блоки (верхний и нижний)


        // элемент - главный текст - ПОЧЕМУ НЕ БЕРЕМ ВЕСЬ БЛОК, а потом в нем роемся? может по этому и тупит
        // ВЕСЬ БЛОК содержащий интересующюю информацию
        //public string[] MainClassName = {"descendant-or-self::a[contains(@class, 'b-link serp-item__title-link serp-item__title-link')]", "descendant-or-self::div[contains(@class, 'serp-item__wrap island clearfix')]"};

        // адрес сайта - поднимаемся на 2 уровня вверх от firstblocklinksClassName и ищем
        //public string[] UrlClassName = {"../../descendant-or-self::a[contains(@class, 'b-link serp-url__link')]", "descendant-or-self::a[contains(@class, 'b-link serp-url__link')]"};

        // описание - поднимаемся на 2 уровня вверх от Главного текста, попадаем в блок, содержащий всю хрень и ищем там. Чета как-то криво. почему не взять сразу блок?
        //public string[] DescriptionClassName = { "../../descendant-or-self::div[contains(@class, 'serp-item__text')]", "descendant-or-self::div[contains(@class, 'serp-item__text')]" };



        // descendant-or-self::div
        //*

        public frmMain()
        {
            // --autorun --yadirect --all --storetodb --autoclose --pageamounts 15
            CheckComputerName();

            InitializeComponent();
            #if DEBUG 

            // загружаем из файла список регионов, если это Москва - то выделяем
            LoadListOfRegions();
            // переносим выделенные регионы в активный поиск
            MoveSelectedItemsFromRegionsToWork();
            #else
            cbDataBase.Checked = cbDataBase.Enabled = cbDataBase.Visible = false;
            #endif

            CommandLineArgs argsWithValues = new CommandLineArgs(Environment.GetCommandLineArgs());
            String[] args = Environment.GetCommandLineArgs();

            //Директ
            cbYandexDirect.Checked = args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandYaDirect.ToUpper())).ToArray().Any(); // директ
            //Excel
            cbExcel.Checked = args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandExcel.ToUpper())).ToArray().Any(); // Excel
            //Всё
            cbProcessAllData.Checked = args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandAll.ToUpper())).ToArray().Any(); // All
            // Также искали
            cbRelatedWords.Checked = args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandRelatedWords.ToUpper())).ToArray().Any(); // RelatedWords
            // Внести в базу
            cbDataBase.Checked = args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandStoreToDB.ToUpper())).ToArray().Any(); // StoreToDB
            // фаразы
            string[] stringSeparators = new string[] { "\n", "\r" };
            if (Properties.Settings.Default.Words != "")
            {
                tbSearchByYandex.Items.Clear();
                tbSearchByYandex.Items.AddRange(Properties.Settings.Default.Words.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries));
            }
            // регионы
            if (Properties.Settings.Default.Regions != "")
            {
                cbRegion.Items.Clear();
                cbRegion.Items.AddRange(Properties.Settings.Default.Regions.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries));
            }

//            CheckComputerName();
            cbDataBase.Visible = _IsRyurik;

            if (args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandPageAmounts.ToUpper())).ToArray().Any()) // PageAmounts
            {
                int i;

                numericYaPageCount.Value = int.TryParse(argsWithValues[Properties.Settings.Default.CommandPageAmounts], out i) ? i : 5;
            }
            if (args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandAutoRun.ToUpper())).ToArray().Any()) // Autorun
            {
                btnSearchByYandex_Click(this, null);
            }
            if (args.Where(tmp => tmp.ToUpper().Equals(Properties.Settings.Default.CommandAutoClose.ToUpper())).ToArray().Any()) // Autoclose
            {
                AutoClose = true;
                this.Close();
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckComputerName()
        {
            _IsRyurik = (System.Net.Dns.GetHostName().ToUpper().Contains("RYURIK".ToUpper()));

            Trace.WriteLine(System.Net.Dns.GetHostName());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            InitializeDriver(); // создаем сервер и браузер

            FindText("Москва", "Купить кухню в Москве"); // иницируем поиск строки (по умолчанию "Найти кухню")

            FillList(); // Заполняем список

        }

        private void InitializeDriver()
        {
            if (_chromeDriver == null)
            {
                String pathToChromeDriver;
                #if DEBUG 
                    Trace.WriteLine("Debug mode!");
                    pathToChromeDriver = Properties.Settings.Default.ChromeDriverPath; // "D:\\Work\\C#\\2048\\chromedriver_win32";
                #else

                    pathToChromeDriver = Properties.Settings.Default.ChromeDriverPath; //System.IO.Path.GetDirectoryName(Application.ExecutablePath); 
                #endif
                    _chromeDriver = new ChromeDriver(pathToChromeDriver); // ЗОВ
                //_chromeDriver = new ChromeDriver("D:\\Work\\C#\\chromedriver_win32"); // дом
                _chromeDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                _wait = new WebDriverWait(_chromeDriver, new TimeSpan(0, 0, 10)); //waits 10 secs
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private String SetRegion(String region)
        {
            InitializeDriver();
            String _newActiveRegion = "";
            String url = _chromeDriver.Url;
            _chromeDriver.Navigate().GoToUrl("http://tune.yandex.ru/region/");
            try
            {
                _wait.Until(d => d.FindElement(By.ClassName("b-form-input__input"))); // ждем пока появится элемент с 
                IWebElement webElement = _chromeDriver.FindElementByClassName("b-form-input__input");
                String activeRegion = webElement.GetAttribute("value");
                activeRegion = String.IsNullOrEmpty(activeRegion) ? "" : activeRegion;

                Trace.WriteLine(String.Format("Текущий регион:{0}", activeRegion));
                if ((!String.IsNullOrEmpty(region)) && ((!activeRegion.ToUpper().Equals(region.ToUpper()))))
                {
                    do
                    {
                        Trace.WriteLine(String.Format("Меняем регион '{0}' на '{1}'", activeRegion, region));

                        List<IWebElement> checkboxRegions = _chromeDriver.FindElements(By.XPath(String.Format("/descendant-or-self::*[contains(@class, '{0}')]", "b-form-checkbox__checkbox i-bem"))).ToList();


                        String webElementByDefaultValue = String.IsNullOrEmpty(checkboxRegions[0].GetAttribute("checked")) ? "false" : "true";
                        String webElementByDontCheckRegion = String.IsNullOrEmpty(checkboxRegions[1].GetAttribute("checked")) ? "false" : "true";
                        // снимаем галочку "по-умолчанию"
                        Trace.WriteLine(String.Format("webElementByDefaultValue:'{0}'", webElementByDefaultValue));
                        if (Boolean.Parse(webElementByDefaultValue))
                        {
                            checkboxRegions[0].Click();
                        }

                        // снимаем галочку с "не уточнять моё местоположение"
                        if (Boolean.Parse(webElementByDontCheckRegion))
                        {
                            checkboxRegions[1].Click();
                        }

                        // webElementByDefault.
                        webElement.Click();
                        webElement.Clear();
                        webElement.SendKeys(region);
                        IWebElement webElementChangeRegion = _chromeDriver.FindElementByClassName("b-form-button__input");
                        webElementChangeRegion.Click();
                        webElement.Click();
                        Thread.Sleep(500);

                        webElement.Submit(); // - не срабатывает почему-то. регион не меняется

                        Trace.WriteLine(String.Format("/descendant-or-self::*[contains(@href, '{0}')]", "region"));
                        //_wait.Until(d => d.FindElement(By.XPath(String.Format("/descendant-or-self::*[contains(@href, '{0}')]", "region"))));
                        while (!_chromeDriver.Title.Equals("Настройка Яндекса"))
                        {
                            try
                            {
                                webElement = _chromeDriver.FindElementByClassName("b-form-input__input");
                                webElement.Submit();
                            }
                            catch (Exception E)
                            {
                                Trace.WriteLine(String.Format("Ошибка: {0}", E.Message));
                            }
                            Thread.Sleep(500);
                        } 
                        

                        _chromeDriver.Navigate().GoToUrl("http://tune.yandex.ru/region/");
                        try
                        {
                            _wait.Until(d => d.FindElement(By.ClassName("b-form-input__input"))); // ждем пока появится элемент с 
                            webElement = _chromeDriver.FindElementByClassName("b-form-input__input");
                            activeRegion = webElement.GetAttribute("value");
                        }
                        catch (Exception E)
                        {
                            Trace.WriteLine(String.Format("Ошибка:{0}", E.Message));
                        }
                    } while (!activeRegion.Equals(region));

                }
                _newActiveRegion = webElement.GetAttribute("value");
                Trace.WriteLine(String.Format("Сменили на регион:{0}", _newActiveRegion));

            }
            catch
            {
                Trace.WriteLine("Не нашли элемент с регионом");
            }
            //_chromeDriver.Navigate().GoToUrl(url);
            return _newActiveRegion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_currentregion"></param>
        /// <param name="findText"></param>
        private void FindText(String currRegion, String findText)
        {
            if (_chromeDriver == null)
            {
                InitializeDriver();
            }
            _queryDateTime = DateTime.Now;

            _currentSearchText = findText;
            // настраиваем регион!
            CurrentRegion = SetRegion(currRegion);
            // настраиваем историю
            UncheckHistorySearching();

            _chromeDriver.Navigate().GoToUrl("http://yandex.ru");
            _chromeDriver.FindElementById("text").SendKeys(findText);
            _chromeDriver.FindElementById("text").Submit(); 

            // ждем, пока не появится элемент "Регион" / а может появится капча
            try
            {
                _wait.Until(d => d.FindElement(By.ClassName(Properties.Settings.Default.RegionChange)));
                CurrentRegion = _chromeDriver.FindElement(By.ClassName(Properties.Settings.Default.RegionChange)).Text;
            }
            catch
            {
                CurrentRegion = "Неопознан";
                try
                {
                    IWebElement capchaElement = _chromeDriver.FindElement(By.ClassName("b-captcha__input"));
                    do
                    {
                        MessageBox.Show("Введи капчу в браузере!!", "Внимание", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        capchaElement = _chromeDriver.FindElement(By.ClassName("b-captcha__input"));
                    } while (capchaElement != null);
                }
                catch (Exception)
                {
                    // и не капча и регион не споймали, лажа какая-то
                }
            }
            relatedWords.Clear();
            relatedWords.Add(findText);

            btnRefresh.Enabled = true;
            btnNextPage.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        private void UncheckHistorySearching()
        {
            InitializeDriver();
            // идем на страничку настроек
            _chromeDriver.Navigate().GoToUrl("http://yandex.ru/search/customize?&lr=213");
            try
            {
                // ищем Результаты поиска:	учитывать мою историю поиска
                IWebElement checkboxHistory = _chromeDriver.FindElement(By.XPath(String.Format(Properties.Settings.Default.XPathCheckMyStorySearch)));
                String webElementHistory = String.IsNullOrEmpty(checkboxHistory.GetAttribute("checked")) ? "false" : "true";
                if (Boolean.Parse(webElementHistory))
                {
                    checkboxHistory.Click(); // снимаем галочку, если она выбрана
                    // ищем кнопку сохранить
                    IWebElement btnSave = _chromeDriver.FindElement(By.XPath(String.Format(Properties.Settings.Default.XPathButtonsSaveSettings)));
                    btnSave.Click(); // сохранили настройки
                }

            }
            catch (Exception E)
            {
                Trace.WriteLine(String.Format("Ошибка при настройке яндекса - учитывать историю поиска:{0}", E.Message));
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_chromeDriver != null)
            {
                _chromeDriver.Quit();
                btnRefresh.Enabled = false;
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChromeDiverQuit();
        }

        private void ChromeDiverQuit()
        {
            try
            {
                if (_chromeDriver != null)
                {
                    _chromeDriver.Quit();
                    _chromeDriver.Dispose();
                    _chromeDriver = null;
                }
            }
            catch (Exception E)
            {
                Trace.WriteLine(E.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillList();
        }

        private void FillList()
        {
            //**************************************************************************
            // верхний рекламный блок - Директ
            // "serp-block serp-adv serp-adv_label_yes serp-adv_compact_no serp-block-x8"

            // первые результаты до картинок
            // "serp-block serp-block-jg"

            // второй блок с результатами
            // "serp-block serp-block-qw"

            // нижний рекламный блок - Директ
            // "serp-block serp-adv serp-adv_label_yes serp-adv_compact_no serp-block-l0"
            //**************************************************************************


            // 1. сам результат (полный):
            // "serp-item serp-item_first_yes serp-item_plain_yes i-bem serp-item_js_inited"
            // 1.1 текст ссылки
            // "serp-item__title clearfix"
            // --!! "b-link serp-item__title-link serp-item__title-link"
            // 1.2 Сайт
            // "serp-item__greenurl serp-url i-bem" -- полная ссылка с навигацией
            // "b-link serp-url__link" - чистый домен
            // 1.3 текст сайта
            // "serp-item__text"



            // "serp-item__label serp-item__label_before_yes"  директ



            //ListViewItem _li;

            List<IWebElement> myElements; // = new List<IWebElement>();
            List<YaSearchResult> YaDirect = new List<YaSearchResult>();
            List<MainSearchResult> mainSearch = new List<MainSearchResult>();

            int zovruId = 0;
            if (cbDataBase.Checked)
            {
                zovruId = insertToZOVRu(_queryDateTime,
                    CurrentRegion.Equals("Неопознан")
                        ? _chromeDriver.FindElement(By.ClassName(Properties.Settings.Default.RegionChange)).Text
                        : CurrentRegion,
                    relatedWords[0]);
            }

            lvResults.Items.Clear();

            int k = 0;

            do
            {
                
                if (cbYandexDirect.Checked) // заполняем директ
                {
                    // верхний директ

                    myElements = getWebElementsByXPath(_chromeDriver, Properties.Settings.Default.XPathDirect, Properties.Settings.Default.XPathADVBlock);

                    YaDirect.AddRange(myElements.Select(t => new YaSearchResult(GetSubElementText(t, Properties.Settings.Default.XPathUrl0), k.ToString(), t.Text, GetSubElementText(t, Properties.Settings.Default.XPathDescription0), DateTime.Now.ToString())));

                }

                // первый блок ссылок "serp-block serp-block-pk"

                myElements = getWebElementsByXPath(_chromeDriver, Properties.Settings.Default.XPathBlockLinksName, Properties.Settings.Default.XPathMain1);

                mainSearch.AddRange(myElements.Select(t => new MainSearchResult(GetSubElementText(t, Properties.Settings.Default.XPathUrl1), GetSubElementText(t, Properties.Settings.Default.XPathMain0), GetSubElementText(t, Properties.Settings.Default.XPathDescription1))));
                /*
                foreach (IWebElement t in myElements)
                {
                    InsertIntoList(j.ToString(), t.Text, GetSubElementText(t, UrlClassName[1]));
                    j++;
                }
                //*/
                // иногда проскакивают - ВМЕСТЕ С ... искали еще
                if (cbRelatedWords.Checked)
                {
                    try
                    {
                        IWebElement relatedElement = _chromeDriver.FindElement(By.XPath("/descendant-or-self::div[contains(@class, 'related_side_bottom')]"));
                        List<IWebElement> relatedElements =relatedElement.FindElements(By.XPath("descendant-or-self::div[contains(@class, 'related__item')]")).ToList();
                        foreach (IWebElement element in relatedElements)
                        {
                            if (!relatedWords.Contains(element.Text))
                            {
                                relatedWords.Add(element.Text);
                            }
                        }
                    }
                    catch (Exception E)
                    {
                        Trace.WriteLine(String.Format("Не смогли найти элемент RelatedWords:{0}", E.Message));
                    }
                }
                k++;
                if (k < numericYaPageCount.Value)
                {
                    PressNextButton();
                }
            } while (k < numericYaPageCount.Value);
            
            // export 
            if (cbExcel.Checked)
            {
                ExportToExcel(zovruId, mainSearch, YaDirect, relatedWords);
            }
            //ExportToExcelFromLV(YaDirect, relatedWords);

            if (zovruId > 0)
            {
                InsertToDB(zovruId, mainSearch, YaDirect, relatedWords);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ZOVRuID"></param>
        /// <param name="mainSearch"></param>
        /// <param name="yaDirect"></param>
        /// <param name="relatedWords"></param>
        private void InsertToDB(int ZOVRuID, List<MainSearchResult> mainSearch, List<YaSearchResult> yaDirect, List<string> relatedWords)
        {
            InitializeConnection();
            /*
            foreach (var relatedWord in relatedWords)
            {
                
            }
             --*/
            int i = 1;
            foreach (MainSearchResult mainSearchResult in mainSearch)
            {
                insertToZOVRuResults(ZOVRuID, i, mainSearchResult.Url, mainSearchResult.Title, mainSearchResult.Description);
                i++;
            }

            foreach (YaSearchResult yaSearchResult in yaDirect)
            {
                insertToZOVRuDirect(ZOVRuID, Int32.Parse(yaSearchResult.CacheUrl), yaSearchResult.DisplayUrl, yaSearchResult.Description);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private _Excel.Worksheet getWorksheet()
        {
            _Excel.Application oExcelApp;
            try
            {
                //Get reference to Excel.Application from the ROT.
                oExcelApp = (_Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");

            }
            catch (Exception ex)
            {
                oExcelApp = null;
                Debug.Print(ex.Message.ToString());
            }

            _Excel.Workbook workbook;
            _Excel.Worksheet worksheet;

            if (oExcelApp == null || oExcelApp.ActiveWorkbook == null)
            {
                oExcelApp = new Microsoft.Office.Interop.Excel.Application();
                workbook = oExcelApp.Workbooks.Add();
                worksheet = (_Excel.Worksheet)workbook.Sheets.Add();
                oExcelApp.Visible = true;
            }
            else
            {
                workbook = oExcelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet;
            }
            return worksheet;
        }

        private void ExportToExcel(int zovRuID = 0, List<MainSearchResult> mainSearchResults = null, List<YaSearchResult> YaDirectList = null, List<String> relatedWordsList = null)
        {
            _Excel.Worksheet worksheet = getWorksheet();

            // Оформление
            worksheet.Cells[1, 2] = _queryDateTime;
            worksheet.Cells[2, 2] = CurrentRegion;
            worksheet.Cells[3, 2] = _currentSearchText;

            worksheet.Cells[4, 1] = "Позиция";
            worksheet.Cells[4, 2] = "Сайт";
            worksheet.Cells[4, 3] = "Заголовок";
            worksheet.Cells[4, 4] = "Описание";

            worksheet.Cells[4, 1].Font.Bold = true;
            worksheet.Cells[4, 2].Font.Bold = true;
            worksheet.Cells[4, 3].Font.Bold = true;
            worksheet.Cells[4, 4].Font.Bold = true;

            int i = 5;
            if (mainSearchResults != null)
            {
                foreach (MainSearchResult mainSearch in mainSearchResults)
                {
                    worksheet.Cells[i, 1] = i - 4;
                    worksheet.Cells[i, 2] = mainSearch.Url;
                    worksheet.Cells[i, 3] = mainSearch.Title;
                    worksheet.Cells[i, 4] = mainSearch.Description;
                    i++;
                }
            }
            // Яндекс Директ
            if (YaDirectList != null)
            {
                for (i = 0; i < YaDirectList.Count; i++)
                {
                    worksheet.Cells[i + 5, 5] = YaDirectList[i].DisplayUrl;
                    worksheet.Cells[i + 5, 6] = YaDirectList[i].Title.Replace("=", "");
                    worksheet.Cells[i + 5, 7] = Int32.Parse(YaDirectList[i].CacheUrl) + 1;
                    worksheet.Cells[i + 5, 8] = YaDirectList[i].Description;

                    String s = YaDirectList[i].Title;
                    if (s.ToUpper().Contains("ЗОВ"))
                    {
                        worksheet.Cells[i + 5, 5].Font.Bold = true;
                        worksheet.Cells[i + 5, 6].Font.Bold = true;
                    }
                }
                worksheet.Cells[3, 5] = "Директ";
                worksheet.Cells[4, 5] = "Сайт";
                worksheet.Cells[4, 6] = "Текст";
                worksheet.Cells[4, 7] = "Страница/позиция";
                worksheet.Cells[4, 7] = "Описание";
                worksheet.Cells[4, 5].Font.Bold = true;
                worksheet.Cells[4, 6].Font.Bold = true;
                worksheet.Cells[4, 7].Font.Bold = true;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void ExportToExcelFromLV(List<YaSearchResult> YaDirectList = null, List<String> relatedWordsList = null)
        {
            _Excel.Worksheet worksheet = getWorksheet();

            int i = 5;
            foreach (ListViewItem li in lvResults.Items)
            {
                worksheet.Cells[i + li.Index, 1] = Int32.Parse(li.SubItems[0].Text) + 1;
                worksheet.Cells[i + li.Index, 2] = li.SubItems[2].Text; // сайт
                worksheet.Cells[i + li.Index, 3] = li.SubItems[1].Text; // описание

                String s = li.SubItems[1].Text;
                if (s.ToUpper().Contains("ЗОВ"))
                {
                    worksheet.Cells[i + li.Index, 1].Font.Bold = true;
                    worksheet.Cells[i + li.Index, 2].Font.Bold = true;
                    worksheet.Cells[i + li.Index, 3].Font.Bold = true;
                }
            }

            // Регион

            try
            {
                CurrentRegion = CurrentRegion.Equals("Неопознан")
                    ? _chromeDriver.FindElement(By.ClassName(Properties.Settings.Default.RegionChange)).Text
                    : CurrentRegion;
            }
            catch (Exception E)
            {
                Trace.WriteLine(String.Format("Ошибка: {0}", E.Message));
            }

            // Оформление
            worksheet.Cells[1, 2] = _queryDateTime;
            worksheet.Cells[2, 2] = CurrentRegion;
            worksheet.Cells[3, 2] = _currentSearchText;

            worksheet.Cells[4, 1] = "Позиция";
            worksheet.Cells[4, 2] = "Сайт";
            worksheet.Cells[4, 3] = "Описание/текст";

            worksheet.Name = CurrentRegion;

            // Яндекс Директ
            if (YaDirectList != null)
            {
                for (i = 0; i < YaDirectList.Count; i++)
                {
                    worksheet.Cells[i + 5, 5] = YaDirectList[i].DisplayUrl;
                    worksheet.Cells[i + 5, 6] = YaDirectList[i].Title.Replace("=", "");
                    worksheet.Cells[i + 5, 7] = Int32.Parse(YaDirectList[i].CacheUrl) + 1;
                    worksheet.Cells[i + 5, 8] = YaDirectList[i].Description;

                    String s = YaDirectList[i].Title;
                    if (s.ToUpper().Contains("ЗОВ"))
                    {
                        worksheet.Cells[i + 5, 5].Font.Bold = true;
                        worksheet.Cells[i + 5, 6].Font.Bold = true;
                    }
                }
                worksheet.Cells[3, 5] = "Директ";
                worksheet.Cells[4, 5] = "Сайт";
                worksheet.Cells[4, 6] = "Текст";
                worksheet.Cells[4, 7] = "Страница/позиция";
                worksheet.Cells[4, 7] = "Описание";
                worksheet.Cells[4, 5].Font.Bold = true;
                worksheet.Cells[4, 6].Font.Bold = true;
                worksheet.Cells[4, 7].Font.Bold = true;
            }

            // "Так же искали" - мифическая хуйня. не понимаю когда появляется
            if (relatedWordsList != null)
            {
                for (i = 1; i < relatedWordsList.Count; i++)
                {
                    worksheet.Cells[i + 4, 9] = relatedWordsList[i];

                    String s = relatedWordsList[i];
                    if (s.ToUpper().Contains("ЗОВ"))
                    {
                        worksheet.Cells[i + 5, 9].Font.Bold = true;
                    }
                }
                worksheet.Cells[4, 9] = String.Format("Так же с фразой {0} искали:", relatedWordsList[0]);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeConnection()
        {
            if ((_sqlcon != null) && (_sqlcon.State != ConnectionState.Open))
            {
                _sqlcon.Open();
            }
            else if (_sqlcon == null)
            {
                SqlConnectionStringBuilder sqlStr = new SqlConnectionStringBuilder();
                sqlStr.InitialCatalog = "globalbase";
                if (_IsRyurik)
                {
                    sqlStr.DataSource = @"zsdb\main";
//                    sqlStr.IntegratedSecurity = true;
                    sqlStr.IntegratedSecurity = false;
                    sqlStr.UserID = "ZOVRu";
                    sqlStr.Password = ",hetyjrcjk.iyc";
                    sqlStr.ConnectTimeout = 10;
                }
                else
                {
                    sqlStr.DataSource = @"82.209.219.231\main,10111";
                    sqlStr.IntegratedSecurity = false;
                    sqlStr.UserID = "ZOVRu";
                    sqlStr.Password = ",hetyjrcjk.iyc";
                    sqlStr.ConnectTimeout = 10;
                }

                _sqlcon = new SqlConnection(sqlStr.ToString());
                _sqlcon.Open();
            }

            if (_sqlcon.State == ConnectionState.Open)
            {
                InitializeCommandInsertToZOVRu();
                InitializeCommandInsertToZOVRuResults();
                InitializeCommandInsertToZOVRuDirect();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeCommandInsertToZOVRu()
        {
            if (_sqlCmdInsertToZOVRu == null)
            {
                _sqlCmdInsertToZOVRu = new SqlCommand();
            }
            _sqlCmdInsertToZOVRu.Connection = _sqlcon;
            _sqlCmdInsertToZOVRu.CommandType = CommandType.StoredProcedure;

            _sqlCmdInsertToZOVRu.CommandText = "dbo.SP_InsertDataToZOVRu";

            _sqlCmdInsertToZOVRu.Parameters.Clear();
            _sqlCmdInsertToZOVRu.Parameters.Add("@ZOVRuID", SqlDbType.Int);
            _sqlCmdInsertToZOVRu.Parameters.Add("@QueryTime", SqlDbType.DateTime);
            _sqlCmdInsertToZOVRu.Parameters.Add("@City", SqlDbType.NVarChar, 255);
            _sqlCmdInsertToZOVRu.Parameters.Add("@Words", SqlDbType.NVarChar, 255);

            _sqlCmdInsertToZOVRu.Parameters["@ZOVRuID"].Direction = ParameterDirection.Output;
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeCommandInsertToZOVRuResults()
        {
            if (_sqlCmdInsertToZOVRuResults == null)
            {
                _sqlCmdInsertToZOVRuResults = new SqlCommand();
            }
            _sqlCmdInsertToZOVRuResults.Connection = _sqlcon;
            _sqlCmdInsertToZOVRuResults.CommandType = CommandType.StoredProcedure;

            _sqlCmdInsertToZOVRuResults.CommandText = "dbo.[SP_InsertDataToZOVRuResults]";

            _sqlCmdInsertToZOVRuResults.Parameters.Clear();
            _sqlCmdInsertToZOVRuResults.Parameters.Add("@ZOVRuID", SqlDbType.Int);
            _sqlCmdInsertToZOVRuResults.Parameters.Add("@Positon", SqlDbType.Int);
            _sqlCmdInsertToZOVRuResults.Parameters.Add("@Site", SqlDbType.NVarChar, 255);
            _sqlCmdInsertToZOVRuResults.Parameters.Add("@Title", SqlDbType.NVarChar, 255);
            _sqlCmdInsertToZOVRuResults.Parameters.Add("@Description", SqlDbType.NVarChar, 1024);
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeCommandInsertToZOVRuDirect()
        {
            if (_sqlCmdInsertToZOVRuDirect == null)
            {
                _sqlCmdInsertToZOVRuDirect = new SqlCommand();
            }
            _sqlCmdInsertToZOVRuDirect.Connection = _sqlcon;
            _sqlCmdInsertToZOVRuDirect.CommandType = CommandType.StoredProcedure;

            _sqlCmdInsertToZOVRuDirect.CommandText = "dbo.[SP_InsertDataToZOVRuDirect]";

            _sqlCmdInsertToZOVRuDirect.Parameters.Clear();
            _sqlCmdInsertToZOVRuDirect.Parameters.Add("@ZOVRuID", SqlDbType.Int);
            _sqlCmdInsertToZOVRuDirect.Parameters.Add("@Page", SqlDbType.Int);
            _sqlCmdInsertToZOVRuDirect.Parameters.Add("@Site", SqlDbType.NVarChar, 255);
            _sqlCmdInsertToZOVRuDirect.Parameters.Add("@Description", SqlDbType.NVarChar, 1024);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="QueryTime"></param>
        /// <param name="City"></param>
        /// <param name="Words"></param>
        /// <returns></returns>
        private int insertToZOVRu(DateTime QueryTime, String City, String Words)
        {
            InitializeConnection();

            _sqlCmdInsertToZOVRu.Parameters["@QueryTime"].Value = QueryTime;
            _sqlCmdInsertToZOVRu.Parameters["@City"].Value = City;
            _sqlCmdInsertToZOVRu.Parameters["@Words"].Value = Words;

            try
            {
                _sqlCmdInsertToZOVRu.ExecuteScalar();
            }
            catch (Exception E)
            {
                Trace.WriteLine(String.Format("Произошла ошибка при выполении SP '{0}':{1}", _sqlCmdInsertToZOVRu.CommandText, E.Message));
            }

            int rerInt;
            try
            {
                rerInt = (int) _sqlCmdInsertToZOVRu.Parameters["@ZOVRuID"].Value;
            }
            catch
            {
                rerInt = 0;
            }
            return rerInt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ZOVRuID"></param>
        /// <param name="Positon"></param>
        /// <param name="Site"></param>
        /// <param name="Description"></param>

        private void insertToZOVRuResults(int ZOVRuID, int Positon, String Site, String Title, String Description)
        {
            InitializeConnection();

            _sqlCmdInsertToZOVRuResults.Parameters["@ZOVRuID"].Value = ZOVRuID;
            _sqlCmdInsertToZOVRuResults.Parameters["@Positon"].Value = Positon;
            _sqlCmdInsertToZOVRuResults.Parameters["@Site"].Value = Site;
            _sqlCmdInsertToZOVRuResults.Parameters["@Title"].Value = Title;
            _sqlCmdInsertToZOVRuResults.Parameters["@Description"].Value = Description;

            try
            {
                _sqlCmdInsertToZOVRuResults.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                Trace.WriteLine(String.Format("Произошла ошибка при выполении SP '{0}':{1}", _sqlCmdInsertToZOVRuResults.CommandText, E.Message));
            }

        }

        private void insertToZOVRuDirect(int ZOVRuID, int Page, String Site, String Description)
        {
            InitializeConnection();

            _sqlCmdInsertToZOVRuDirect.Parameters["@ZOVRuID"].Value = ZOVRuID;
            _sqlCmdInsertToZOVRuDirect.Parameters["@Page"].Value = Page;
            _sqlCmdInsertToZOVRuDirect.Parameters["@Site"].Value = Site;
            _sqlCmdInsertToZOVRuDirect.Parameters["@Description"].Value = Description;

            try
            {
                _sqlCmdInsertToZOVRuDirect.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                Trace.WriteLine(String.Format("Произошла ошибка при выполении SP '{0}':{1}", _sqlCmdInsertToZOVRuDirect.CommandText, E.Message));
            }

        }


        /// <summary>
        /// 
        /// </summary>
        private void PressNextButton()
        {
            InitializeDriver();
            try
            {
                //var elementF = _chromeDriver.FindElementByXPath("//div[@class='content content_type_main layout layout_type_serp i-bem content_js_inited']");

                IWebElement elementF = _chromeDriver.FindElementByXPath("//div[@class='pager__content']");
//                IWebElement elementF = _chromeDriver.FindElementByXPath("//div[@class='pager pager__instance pager__instance_position_bottom more-hidden serp pager__instance pager__instance_position_bottom more-hidden i-bem serp_js_inited pager_js_inited']");
                int ActivePage = 0;
                String classActivePage = "";

                Trace.WriteLine(String.Format("Бежим по страничкам:"));
                foreach (IWebElement webElement in elementF.FindElements(By.XPath("descendant::*[contains(@class, 'button')]")))
                {
                    String currClass = webElement.GetAttribute("class");
                    Trace.WriteLine(String.Format("{0}:{1}", webElement.Text, currClass));
                    if (currClass.ToUpper().Contains("button_checked_yes".ToUpper()))
                    {
                        classActivePage = currClass;
                        // определили какая страница сейчас активна
                        if (int.TryParse(webElement.Text, out ActivePage))
                        {
                            Trace.WriteLine(String.Format("Сейчас активна '{0}' страница", ActivePage));
                            ActivePage++;
                        }
                        else
                        {
                            ActivePage = 0;
                        }
                    }
                    // Если это кнопка "Следующая", то нажимаем ее
                    if (webElement.Text.ToUpper().CompareTo("Следующая".ToUpper()) == 0)
                    {
                        Trace.WriteLine(String.Format("Нажимаем!!!"));

                        webElement.Click();
                        // ждем появления region-change__text
                        WebDriverWait _wait5 = new WebDriverWait(_chromeDriver, new TimeSpan(0, 0, 5));
                            //waits 5 secs max

                        Trace.WriteLine(String.Format("/descendant-or-self::*[contains(@class, '{0}')]", classActivePage));
                        // Ждем появления активной кнопки следующей страницы [2] - второй элемент!!! потому как кнопка в "кнопке"
//                        _wait5.Until(d => d.FindElement(By.XPath(String.Format("/descendant-or-self::*[contains(@class, '{0}')][2]", classActivePage))).Text.Equals(ActivePage.ToString()));
                        _wait5.Until(d => d.FindElement(By.XPath(String.Format("/descendant-or-self::a[contains(@class, '{0}')]", classActivePage))).Text.Equals(ActivePage.ToString()));

                        break;
                    }
                }
            }
            catch (WebDriverException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromeDriver"></param>
        /// <param name="containerCssSelector"></param>
        /// <param name="linkCssSelector"></param>
        /// <returns></returns>
        private List<IWebElement> getWebElementsByCssSelector(ChromeDriver chromeDriver, String containerCssSelector,
            String linkCssSelector)
        {
            try
            {
                IWebElement webElement = (IWebElement) chromeDriver.FindElement(By.CssSelector(containerCssSelector));
                if (webElement != null)
                {
                    return webElement.FindElements(By.CssSelector(linkCssSelector)).ToList();
                }
            }
            catch (WebDriverException e)
            {
                System.Diagnostics.Trace.WriteLine(containerCssSelector + System.Environment.NewLine +
                                                   linkCssSelector + System.Environment.NewLine +
                                                   e.Message);
            }

            return new List<IWebElement>(); // возвращаем пустой массив
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromeDriver"></param>
        /// <param name="containerXPath"></param>
        /// <param name="linkXPath"></param>
        /// <returns></returns>
        private List<IWebElement> getWebElementsByXPath(ChromeDriver chromeDriver, String containerXPath, String linkXPath)
        {
            try
            {
                // ищем все контейнеры, которые содержат containerXPath
                List<IWebElement> webConteiners = chromeDriver.FindElements(By.XPath(containerXPath)).ToList();
                if (webConteiners != null)
                {
                    List<IWebElement> returnElements = new List<IWebElement>();
                    foreach (IWebElement t in webConteiners) // получаем все контейнеры
                    {
                        // нужно пробежать по всем контейнерам и сложить все элементы linkXPath в этих контейнерах
                        List<IWebElement> webLinks = t.FindElements(By.XPath(linkXPath)).ToList();
                        foreach (IWebElement l in webLinks)
                        {
                            returnElements.Add(l);
                        }

                    }
                    return returnElements;
                }
            }
            catch (WebDriverException e)
            {
                System.Diagnostics.Trace.WriteLine(containerXPath + System.Environment.NewLine +
                                                   linkXPath + System.Environment.NewLine +
                                                   e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(containerXPath + System.Environment.NewLine +
                                                   linkXPath + System.Environment.NewLine +
                                                   e.Message);
            }
            return new List<IWebElement>(); // возвращаем пустой массив
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="urlXPath"></param>
        /// <returns></returns>
        private string GetSubElementText(IWebElement webElement, String urlXPath)
        {
            try
            {
                IWebElement webLinks = webElement.FindElement(By.XPath(urlXPath));
                return webLinks.Text;
            }
            catch (WebDriverException e)
            {
                System.Diagnostics.Trace.WriteLine(urlXPath + System.Environment.NewLine + e.Message);
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstString"></param>
        /// <param name="secondString"></param>
        private void InsertIntoList(string firstString, string secondString, string thirdColumn)
        {
            ListViewItem li = lvResults.Items.Add(firstString);
            li.SubItems.Add(new ListViewItem.ListViewSubItem(li, secondString));
            li.SubItems.Add(new ListViewItem.ListViewSubItem(li, thirdColumn));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindText_Click(object sender, EventArgs e)
        {
            if (rbAPI.Checked)
            {
                FindTextWithAPI();
            }
            else
            {
                InitializeDriver();
                // 

                foreach (ListViewItem lvi in lvAllRegions.Items)
                {
                    if (lvi.Selected)
                    {
                    }
                }
                FindText("Гродно", tbFindText.Text);
            }
        }

        private void btnXPath_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeDriver();
                List<IWebElement> myElements = new List<IWebElement>();

                IWebElement webElement = _chromeDriver.FindElementByXPath(tbXPath.Text);
                richXPath.Text = webElement.Text;

                myElements = _chromeDriver.FindElementsByXPath(tbXPath.Text).ToList();

                richXPath.Text = "";
                foreach (IWebElement webEl in myElements)
                {
                    String webElementClass = webEl.GetAttribute("class");
                    richXPath.Text = richXPath.Text +
                                     String.Format("Class:{0} -- Text:{1}", webElementClass, webEl.Text) +
                                     System.Environment.NewLine;
                }
            }
            catch (WebDriverException wde)
            {
                System.Diagnostics.Trace.WriteLine(tbXPath.Text + System.Environment.NewLine + wde.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MoveSelectedItemsFromRegionsToWork();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FindTextWithAPI()
        {
            if (tabSearchEngine.SelectedTab.TabIndex == tbYandex.TabIndex) // Yandex
            {
                const String username = "ryurik1978";
                const String userkey = "03.108365218:d079b13c7f27666c7d0e8fcd01e84872";

                const String regionid = "1"; // Москва и область
                /*

                // POST метод
                // ryurik1978
                String url = String.Format(@"http://xmlsearch.yandex.ru/xmlsearch?user={0}&key={1}", username, userkey);

                // Текст запроса в формате XML
                String command =
        @"<?xml version=""1.0"" encoding=""UTF-8""?>   
          <request>   
           <query>Купить кухню</query>
           <groupings>
             <groupby attr=""d"" 
                    mode=""deep"" 
                    groups-on-page=""10"" 
                    docs-in-group=""1"" />   
           </groupings>   
          </request>";

                byte[] bytes = Encoding.UTF8.GetBytes(command);
                // Объект, с помощью которого будем отсылать запрос и получать ответ.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "text/xml";
                // Пишем наш XML-запрос в поток 
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                // Получаем ответ
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                 
                 --*/
                // GET метод
                //Шаблон запроса.
                string url = @"http://xmlsearch.yandex.ru/xmlsearch?
              query={0}&
              groupby=attr%3Dd.mode%3Ddeep.groups-on-page%3D20.docs-in-group%3D1&
              page=1
              user={1}&
              key={2}&
              lr={3}";
                // СУКИ, нужно зарегить пользователя на рабочий сайт, у которого есть рейтинг, только тогда можно пользоваться XML
                //Готовый текст запроса.
                string completeUrl = String.Format(url, tbFindText.Text, username, userkey, regionid);
                Trace.WriteLine(completeUrl);
                //Объект, отсылающий запрос.
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(completeUrl);
                //Получение ответа.
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                XmlReader xmlReader = XmlReader.Create(response.GetResponseStream());
                XDocument xmlResponse = XDocument.Load(xmlReader);

                List<YaSearchResult> yaSearchResults = CollectAllResulrsInList(xmlResponse);
                FillYandexResutsToExcel(yaSearchResults);
            }
            else // Google
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void LoadFromXML()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlReader xmlReader = XmlReader.Create(openFileDialog.FileName);
                XDocument xmlResponse = XDocument.Load(xmlReader);
                List<YaSearchResult> yaSearchResults = CollectAllResulrsInList(xmlResponse);
                FillYandexResutsToExcel(yaSearchResults);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadListOfRegions()
        {
            ImportFromExcel();
        }

        /// <summary>
        /// 
        /// </summary>

        private void ImportFromExcel()
        {

            System.Threading.Thread thisThread = System.Threading.Thread.CurrentThread;
            System.Globalization.CultureInfo originalCulture = thisThread.CurrentCulture;

            // Use an exception block to switch back in case of a run-time error.
            try
            {
                thisThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


                //Приложение
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;

                if (ExcelApp.Version == "14.0")
                {

                }

                ExcelWorkBook = ExcelApp.Workbooks.Open(Properties.Settings.Default.PathToRegions, false, true);
                ExcelWorkSheet = ExcelWorkBook.Worksheets[1];

                int i = 2;
                String region;
                while (ExcelWorkSheet.Cells[i, 1].Text != "")
                {
                    region = ExcelWorkSheet.Cells[i, 2].Text;
                    ListViewItem li = lvAllRegions.Items.Add(i.ToString(), ExcelWorkSheet.Cells[i, 1].Text);
                    li.SubItems.Add(new ListViewItem.ListViewSubItem(li, region));
                    li.SubItems.Add(new ListViewItem.ListViewSubItem(li, ExcelWorkSheet.Cells[i, 3].Text));

                    if (region.ToLower().StartsWith("Моск".ToLower()) || region.ToLower().StartsWith("Санк".ToLower()))
                    {
                        li.Selected = true;
                    }
                    i++;
                }
                ExcelWorkBook.Close(false);

                ExcelApp.Quit();
            }
            finally
            {
                // Restore the culture information for the thread after the
                // Excel calls have completed.
                thisThread.CurrentCulture = originalCulture;
            }
        }

        private void MoveSelectedItemsFromRegionsToWork()
        {
            foreach (ListViewItem lvi in lvAllRegions.Items)
            {
                if (lvi.Selected)
                {
                    lvAllRegions.Items.Remove(lvi);
                    lvWorking.Items.Add(lvi);
                    lvi.Selected = false;
                }
            }
        }

        private void MoveSelectedItemsFromWorkToRegions()
        {
            foreach (ListViewItem lvi in lvWorking.Items)
            {
                if (lvi.Selected)
                {
                    lvWorking.Items.Remove(lvi);
                    lvAllRegions.Items.Add(lvi);
                    lvi.Selected = false;
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            MoveSelectedItemsFromWorkToRegions();
        }

        public static string GetValue(XElement group, string name)
        {
            try
            {
                return group.Element("doc").Element(name).Value;
            }
                //это если в результате нету элемента с каким то именем,
                //то будет вместо значащей строчки возвращаться пустая.
            catch
            {
                return string.Empty;
            }
        }

        public static List<YaSearchResult> CollectAllResulrsInList(XDocument response)
        {

            //Лист структур YaSearchResult, который метод в итоге возвращает.
            List<YaSearchResult> ret = new List<YaSearchResult>();

            //из полученного XML'я выдираем все элементы с именем "group" - это результаты поиска
            var groupQuery = from gr in response.Elements().
                Elements("response").
                Elements("results").
                Elements("grouping").
                Elements("group")
                select gr;

            //каждый элемент group преобразовывается в объект SearchResult
            for (int i = 0; i < groupQuery.Count(); i++)
            {
                string urlQuery = GetValue(groupQuery.ElementAt(i), "url");
                string titleQuery = GetValue(groupQuery.ElementAt(i), "title");
                string descriptionQuery = GetValue(groupQuery.ElementAt(i), "headline");
                string indexedTimeQuery = GetValue(groupQuery.ElementAt(i), "modtime");
                string cacheUrlQuery = GetValue(groupQuery.ElementAt(i), "saved-copy-url");
                ret.Add(new YaSearchResult(urlQuery, cacheUrlQuery, titleQuery, descriptionQuery, indexedTimeQuery));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yaSearchResults"></param>
        public void FillYandexResutsToExcel(List<YaSearchResult> yaSearchResults)
        {
            System.Threading.Thread thisThread = System.Threading.Thread.CurrentThread;
            System.Globalization.CultureInfo originalCulture = thisThread.CurrentCulture;

            // Use an exception block to switch back in case of a run-time error.
            try
            {
                thisThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


                //Приложение
                _Excel.Application oExcelApp;
                _Excel.Workbook workbook;
                _Excel.Worksheet worksheet;

                try
                {
                    //Get reference to Excel.Application from the ROT.
                    oExcelApp =
                        (_Excel.Application) System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                }
                catch (Exception ex)
                {
                    oExcelApp = null;
                    Debug.Print(ex.Message.ToString());
                }

                if (oExcelApp == null || oExcelApp.ActiveWorkbook == null)
                {
                    oExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    workbook = oExcelApp.Workbooks.Add();
                    worksheet = (_Excel.Worksheet) workbook.Sheets.Add();
                    oExcelApp.Visible = true;
                }
                else
                {
                    workbook = oExcelApp.ActiveWorkbook;
                    worksheet = workbook.ActiveSheet;
                }
                if (oExcelApp.Version == "14.0")
                {

                }

                int i = 2;
                foreach (YaSearchResult yaSearchResult in yaSearchResults)
                {
                    worksheet.Cells[i, 1] = yaSearchResult.Title;
                    worksheet.Cells[i, 2] = yaSearchResult.Description;
                    worksheet.Cells[i, 3] = yaSearchResult.CacheUrl;
                    worksheet.Cells[i, 4] = yaSearchResult.DisplayUrl;
                    worksheet.Cells[i, 5] = yaSearchResult.IndexedTime;
                    i++;
                }
                oExcelApp.UserControl = true;
            }
            finally
            {
                // Restore the culture information for the thread after the
                // Excel calls have completed.
                thisThread.CurrentCulture = originalCulture;
            }

        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            PressNextButton();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //LoadFromXML();
            InitializeConnection();
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            frmLimits frmLimit = new frmLimits();
            frmLimit.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            InitializeDriver();


            richXPath.Clear();
            _chromeDriver.Navigate()
                .GoToUrl(
                    "http://www.2020.net/20-20University/Design_Level-101_v10/table_of_contents_table_of_contents.html");
//            _wait.Until(d => d.FindElement(By.ClassName("text15614Font1"))); // ждем пока появится элемент с 
            btnGetLink.Enabled = true;



            return;
/*
            //List<IWebElement> videoElements = _chromeDriver.FindElements(By.XPath(String.Format("/descendant-or-self::div[contains(@id, 'button')]"))).ToList();
            List<IWebElement> videoElements =
                _chromeDriver.FindElements(By.XPath(String.Format("/descendant-or-self::img[contains(@name, 'image')]")))
                    .ToList();
            for (int i = 0; i < videoElements.Count; i++)
            {
                List<IWebElement> currentElements = _chromeDriver.FindElements(By.XPath(String.Format("/descendant-or-self::img[contains(@name, 'image')]"))).ToList();
                //                List<IWebElement> currentElements = _chromeDriver.FindElements(By.XPath(String.Format("/descendant-or-self::div[contains(@id, 'button')]"))).ToList();

                foreach (IWebElement curWebElement in currentElements)
                {
                    String currentSrc = curWebElement.GetAttribute("src");
                    if (currentSrc.IndexOf("images/unchecked.jpg") > 0)
                    {
                        curWebElement.Click();
                        try
                        {
                            _wait.Until(d => d.FindElement(By.XPath("/descendant-or-self::embed[contains(@type, 'application/x-shockwave-flash')]")));

                            // ждем пока загрузиться 
                            try
                            {
                                IWebElement vidosWebElement = _chromeDriver.FindElement(By.XPath("/descendant-or-self::embed[contains(@type, 'application/x-shockwave-flash')]"));
                                String filePath = vidosWebElement.GetAttribute("flashvars");
                                filePath = filePath.Substring(1, filePath.IndexOf("&") - 1);
                                richXPath.Lines = new[]
                                {
                                    richXPath.Lines + System.Environment.NewLine +
                                    "http://www.2020.net/20-20University/Design_Level-101_v10/media/" + filePath
                                };
                                _chromeDriver.Navigate().Back();

                            }
                            catch
                            {
                                _chromeDriver.Navigate().Back();
                            }
                        }
                        catch
                        {
                            _chromeDriver.Navigate().Back();
                        }
                        break; // нужно по новому обновлять данные
                    }
                }
            }
            // */
        }

        private void btnGetLink_Click(object sender, EventArgs e)
        {
            IWebElement vidosWebElement =
                _chromeDriver.FindElement(
                    By.XPath(
                        "/descendant-or-self::embed[contains(@type, 'application/x-shockwave-flash')]"));
            String filePath = vidosWebElement.GetAttribute("flashvars");
            filePath = filePath.Substring(5, filePath.IndexOf("&") - 5);

            
            richXPath.Text = richXPath.Text + System.Environment.NewLine + "http://www.2020.net/20-20University/Design_Level-101_v10/media/" + filePath;
            _chromeDriver.Navigate().Back();

        }

        private void btn3_Click(object sender, EventArgs e)
        {
            Trace.WriteLine(SetRegion("Москва"));
             
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchByYandex_Click(object sender, EventArgs e)
        {
            // --autorun --yadirect --all --storetodb --autoclose --pageamounts 15
            /* */
            try // */
            {
                if (cbProcessAllData.Checked)
                {
                    InitializeDriver();
                    foreach (String t in cbRegion.Items)
                    {
                        cbRegion.Text = t;
                        Application.DoEvents();

                        if (!cbRegion.Text.StartsWith("!"))
                        {
                            foreach (String s in tbSearchByYandex.Items)
                            {
                                tbSearchByYandex.Text = s.Replace("%C", t);
                                Application.DoEvents();
                                if (!tbSearchByYandex.Text.StartsWith("!"))
                                {
                                    FindText(cbRegion.Text, tbSearchByYandex.Text);
                                    FillList();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!cbRegion.Text.StartsWith("!")) // смотрим чтобы небыло клиента 
                    {
                        InitializeDriver();
                        Application.DoEvents();

                        tbSearchByYandex.Text = tbSearchByYandex.Text.Replace("%C", cbRegion.Text);

                        FindText(cbRegion.Text, tbSearchByYandex.Text);
                        FillList();
                    }
                    else
                    {
                        MessageBox.Show("Выберите регион!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            } 
            /* */
            catch (Exception E)
            {
                Trace.WriteLine(E.Message);
            } //*/
            ChromeDiverQuit();
        }
    }
}
