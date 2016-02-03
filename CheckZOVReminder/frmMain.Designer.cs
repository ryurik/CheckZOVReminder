namespace CheckZOVReminder
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvResults = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tbXPath = new System.Windows.Forms.TextBox();
            this.btnXPath = new System.Windows.Forms.Button();
            this.richXPath = new System.Windows.Forms.RichTextBox();
            this.tbFindText = new System.Windows.Forms.TextBox();
            this.btnFindText = new System.Windows.Forms.Button();
            this.tabSearchEngine = new System.Windows.Forms.TabControl();
            this.tbYandex = new System.Windows.Forms.TabPage();
            this.cbProcessAllData = new System.Windows.Forms.CheckBox();
            this.cbExcel = new System.Windows.Forms.CheckBox();
            this.cbRelatedWords = new System.Windows.Forms.CheckBox();
            this.cbDataBase = new System.Windows.Forms.CheckBox();
            this.tbSearchByYandex = new System.Windows.Forms.ComboBox();
            this.cbRegion = new System.Windows.Forms.ComboBox();
            this.btnSearchByYandex = new System.Windows.Forms.Button();
            this.labSearchText = new System.Windows.Forms.Label();
            this.labRegion = new System.Windows.Forms.Label();
            this.labYaPageCount = new System.Windows.Forms.Label();
            this.numericYaPageCount = new System.Windows.Forms.NumericUpDown();
            this.cbYandexDirect = new System.Windows.Forms.CheckBox();
            this.tpGoogle = new System.Windows.Forms.TabPage();
            this.gbMethod = new System.Windows.Forms.GroupBox();
            this.rbAPI = new System.Windows.Forms.RadioButton();
            this.rbWebDriver = new System.Windows.Forms.RadioButton();
            this.lvAllRegions = new System.Windows.Forms.ListView();
            this.chID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRegion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCountry = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvWorking = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnTest2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btnGetLink = new System.Windows.Forms.Button();
            this.tabSearchEngine.SuspendLayout();
            this.tbYandex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericYaPageCount)).BeginInit();
            this.gbMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(354, 189);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(33, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(354, 218);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(33, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(11, 264);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(79, 24);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Начать";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(96, 265);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvResults
            // 
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(13, 306);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(689, 371);
            this.lvResults.TabIndex = 5;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "№";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Link";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 100;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Enabled = false;
            this.btnRefresh.Location = new System.Drawing.Point(177, 264);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tbXPath
            // 
            this.tbXPath.Location = new System.Drawing.Point(742, 304);
            this.tbXPath.Name = "tbXPath";
            this.tbXPath.Size = new System.Drawing.Size(231, 20);
            this.tbXPath.TabIndex = 7;
            this.tbXPath.Text = "//*[contains(@class, \'serp-block serp-block-\')]";
            // 
            // btnXPath
            // 
            this.btnXPath.Location = new System.Drawing.Point(981, 304);
            this.btnXPath.Name = "btnXPath";
            this.btnXPath.Size = new System.Drawing.Size(46, 20);
            this.btnXPath.TabIndex = 8;
            this.btnXPath.Text = ">";
            this.btnXPath.UseVisualStyleBackColor = true;
            this.btnXPath.Click += new System.EventHandler(this.btnXPath_Click);
            // 
            // richXPath
            // 
            this.richXPath.HideSelection = false;
            this.richXPath.Location = new System.Drawing.Point(743, 330);
            this.richXPath.Name = "richXPath";
            this.richXPath.Size = new System.Drawing.Size(285, 330);
            this.richXPath.TabIndex = 9;
            this.richXPath.Text = "";
            // 
            // tbFindText
            // 
            this.tbFindText.Location = new System.Drawing.Point(398, 74);
            this.tbFindText.Name = "tbFindText";
            this.tbFindText.Size = new System.Drawing.Size(231, 20);
            this.tbFindText.TabIndex = 7;
            this.tbFindText.Text = "Купить кухню";
            // 
            // btnFindText
            // 
            this.btnFindText.Location = new System.Drawing.Point(637, 70);
            this.btnFindText.Name = "btnFindText";
            this.btnFindText.Size = new System.Drawing.Size(46, 23);
            this.btnFindText.TabIndex = 8;
            this.btnFindText.Text = ">";
            this.btnFindText.UseVisualStyleBackColor = true;
            this.btnFindText.Click += new System.EventHandler(this.btnFindText_Click);
            // 
            // tabSearchEngine
            // 
            this.tabSearchEngine.Controls.Add(this.tbYandex);
            this.tabSearchEngine.Controls.Add(this.tpGoogle);
            this.tabSearchEngine.Location = new System.Drawing.Point(13, 10);
            this.tabSearchEngine.Name = "tabSearchEngine";
            this.tabSearchEngine.SelectedIndex = 0;
            this.tabSearchEngine.Size = new System.Drawing.Size(337, 130);
            this.tabSearchEngine.TabIndex = 10;
            // 
            // tbYandex
            // 
            this.tbYandex.Controls.Add(this.cbProcessAllData);
            this.tbYandex.Controls.Add(this.cbExcel);
            this.tbYandex.Controls.Add(this.cbRelatedWords);
            this.tbYandex.Controls.Add(this.cbDataBase);
            this.tbYandex.Controls.Add(this.tbSearchByYandex);
            this.tbYandex.Controls.Add(this.cbRegion);
            this.tbYandex.Controls.Add(this.btnSearchByYandex);
            this.tbYandex.Controls.Add(this.labSearchText);
            this.tbYandex.Controls.Add(this.labRegion);
            this.tbYandex.Controls.Add(this.labYaPageCount);
            this.tbYandex.Controls.Add(this.numericYaPageCount);
            this.tbYandex.Controls.Add(this.cbYandexDirect);
            this.tbYandex.Location = new System.Drawing.Point(4, 22);
            this.tbYandex.Name = "tbYandex";
            this.tbYandex.Padding = new System.Windows.Forms.Padding(3);
            this.tbYandex.Size = new System.Drawing.Size(329, 104);
            this.tbYandex.TabIndex = 0;
            this.tbYandex.Text = "Yandex";
            this.tbYandex.UseVisualStyleBackColor = true;
            // 
            // cbProcessAllData
            // 
            this.cbProcessAllData.AutoSize = true;
            this.cbProcessAllData.Checked = true;
            this.cbProcessAllData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProcessAllData.Location = new System.Drawing.Point(147, 6);
            this.cbProcessAllData.Name = "cbProcessAllData";
            this.cbProcessAllData.Size = new System.Drawing.Size(45, 17);
            this.cbProcessAllData.TabIndex = 14;
            this.cbProcessAllData.Text = "Всё";
            this.cbProcessAllData.UseVisualStyleBackColor = true;
            // 
            // cbExcel
            // 
            this.cbExcel.AutoSize = true;
            this.cbExcel.Checked = true;
            this.cbExcel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExcel.Location = new System.Drawing.Point(147, 29);
            this.cbExcel.Name = "cbExcel";
            this.cbExcel.Size = new System.Drawing.Size(52, 17);
            this.cbExcel.TabIndex = 14;
            this.cbExcel.Text = "Excel";
            this.cbExcel.UseVisualStyleBackColor = true;
            // 
            // cbRelatedWords
            // 
            this.cbRelatedWords.AutoSize = true;
            this.cbRelatedWords.Location = new System.Drawing.Point(226, 6);
            this.cbRelatedWords.Name = "cbRelatedWords";
            this.cbRelatedWords.Size = new System.Drawing.Size(98, 17);
            this.cbRelatedWords.TabIndex = 14;
            this.cbRelatedWords.Text = "Также искали";
            this.cbRelatedWords.UseVisualStyleBackColor = true;
            // 
            // cbDataBase
            // 
            this.cbDataBase.AutoSize = true;
            this.cbDataBase.Checked = true;
            this.cbDataBase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDataBase.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbDataBase.Location = new System.Drawing.Point(226, 29);
            this.cbDataBase.Name = "cbDataBase";
            this.cbDataBase.Size = new System.Drawing.Size(97, 17);
            this.cbDataBase.TabIndex = 14;
            this.cbDataBase.Text = "Внести в базу";
            this.cbDataBase.UseVisualStyleBackColor = true;
            // 
            // tbSearchByYandex
            // 
            this.tbSearchByYandex.FormattingEnabled = true;
            this.tbSearchByYandex.Items.AddRange(new object[] {
            "купить кухню",
            "купить кухню в %C",
            "белорусские кухни",
            "белорусские кухни зов",
            "кухни в %C",
            "кухни %C",
            "кухни зов",
            "кухни зов %C",
            "кухни на заказ в %C недорого",
            "кухня"});
            this.tbSearchByYandex.Location = new System.Drawing.Point(52, 77);
            this.tbSearchByYandex.MaxDropDownItems = 20;
            this.tbSearchByYandex.Name = "tbSearchByYandex";
            this.tbSearchByYandex.Size = new System.Drawing.Size(229, 21);
            this.tbSearchByYandex.TabIndex = 13;
            this.tbSearchByYandex.Text = "Купить кухню";
            // 
            // cbRegion
            // 
            this.cbRegion.FormattingEnabled = true;
            this.cbRegion.Items.AddRange(new object[] {
            "Москва",
            "Санкт-Петербург",
            "Тула",
            "Тверь",
            "Тольятти",
            "Рязань",
            "Нижний Новгород",
            "! Гуринович !",
            "Липецк",
            "Ярославль",
            "Курск"});
            this.cbRegion.Location = new System.Drawing.Point(52, 55);
            this.cbRegion.MaxDropDownItems = 20;
            this.cbRegion.Name = "cbRegion";
            this.cbRegion.Size = new System.Drawing.Size(229, 21);
            this.cbRegion.TabIndex = 13;
            this.cbRegion.Text = "Москва";
            // 
            // btnSearchByYandex
            // 
            this.btnSearchByYandex.Location = new System.Drawing.Point(287, 56);
            this.btnSearchByYandex.Name = "btnSearchByYandex";
            this.btnSearchByYandex.Size = new System.Drawing.Size(36, 42);
            this.btnSearchByYandex.TabIndex = 12;
            this.btnSearchByYandex.Text = ">";
            this.btnSearchByYandex.UseVisualStyleBackColor = true;
            this.btnSearchByYandex.Click += new System.EventHandler(this.btnSearchByYandex_Click);
            // 
            // labSearchText
            // 
            this.labSearchText.AutoSize = true;
            this.labSearchText.Location = new System.Drawing.Point(13, 80);
            this.labSearchText.Name = "labSearchText";
            this.labSearchText.Size = new System.Drawing.Size(37, 13);
            this.labSearchText.TabIndex = 11;
            this.labSearchText.Text = "Текст";
            // 
            // labRegion
            // 
            this.labRegion.AutoSize = true;
            this.labRegion.Location = new System.Drawing.Point(7, 56);
            this.labRegion.Name = "labRegion";
            this.labRegion.Size = new System.Drawing.Size(43, 13);
            this.labRegion.TabIndex = 10;
            this.labRegion.Text = "Регион";
            // 
            // labYaPageCount
            // 
            this.labYaPageCount.AutoSize = true;
            this.labYaPageCount.Location = new System.Drawing.Point(3, 31);
            this.labYaPageCount.Name = "labYaPageCount";
            this.labYaPageCount.Size = new System.Drawing.Size(88, 13);
            this.labYaPageCount.TabIndex = 2;
            this.labYaPageCount.Text = "Кол-во страниц:";
            // 
            // numericYaPageCount
            // 
            this.numericYaPageCount.Location = new System.Drawing.Point(94, 29);
            this.numericYaPageCount.Name = "numericYaPageCount";
            this.numericYaPageCount.Size = new System.Drawing.Size(47, 20);
            this.numericYaPageCount.TabIndex = 1;
            this.numericYaPageCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // cbYandexDirect
            // 
            this.cbYandexDirect.AutoSize = true;
            this.cbYandexDirect.Checked = true;
            this.cbYandexDirect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbYandexDirect.Location = new System.Drawing.Point(3, 6);
            this.cbYandexDirect.Name = "cbYandexDirect";
            this.cbYandexDirect.Size = new System.Drawing.Size(64, 17);
            this.cbYandexDirect.TabIndex = 0;
            this.cbYandexDirect.Text = "Директ";
            this.cbYandexDirect.UseVisualStyleBackColor = true;
            // 
            // tpGoogle
            // 
            this.tpGoogle.Location = new System.Drawing.Point(4, 22);
            this.tpGoogle.Name = "tpGoogle";
            this.tpGoogle.Padding = new System.Windows.Forms.Padding(3);
            this.tpGoogle.Size = new System.Drawing.Size(329, 104);
            this.tpGoogle.TabIndex = 1;
            this.tpGoogle.Text = "Google";
            this.tpGoogle.UseVisualStyleBackColor = true;
            // 
            // gbMethod
            // 
            this.gbMethod.Controls.Add(this.rbAPI);
            this.gbMethod.Controls.Add(this.rbWebDriver);
            this.gbMethod.Location = new System.Drawing.Point(398, 10);
            this.gbMethod.Name = "gbMethod";
            this.gbMethod.Size = new System.Drawing.Size(285, 58);
            this.gbMethod.TabIndex = 11;
            this.gbMethod.TabStop = false;
            // 
            // rbAPI
            // 
            this.rbAPI.AutoSize = true;
            this.rbAPI.Location = new System.Drawing.Point(11, 32);
            this.rbAPI.Name = "rbAPI";
            this.rbAPI.Size = new System.Drawing.Size(42, 17);
            this.rbAPI.TabIndex = 1;
            this.rbAPI.Text = "API";
            this.rbAPI.UseVisualStyleBackColor = true;
            this.rbAPI.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbWebDriver
            // 
            this.rbWebDriver.AutoSize = true;
            this.rbWebDriver.Checked = true;
            this.rbWebDriver.Location = new System.Drawing.Point(11, 15);
            this.rbWebDriver.Name = "rbWebDriver";
            this.rbWebDriver.Size = new System.Drawing.Size(76, 17);
            this.rbWebDriver.TabIndex = 0;
            this.rbWebDriver.TabStop = true;
            this.rbWebDriver.Text = "WebDriver";
            this.rbWebDriver.UseVisualStyleBackColor = true;
            this.rbWebDriver.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // lvAllRegions
            // 
            this.lvAllRegions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chID,
            this.chRegion,
            this.chCountry});
            this.lvAllRegions.FullRowSelect = true;
            this.lvAllRegions.GridLines = true;
            this.lvAllRegions.HideSelection = false;
            this.lvAllRegions.LabelEdit = true;
            this.lvAllRegions.Location = new System.Drawing.Point(11, 189);
            this.lvAllRegions.Name = "lvAllRegions";
            this.lvAllRegions.ShowGroups = false;
            this.lvAllRegions.Size = new System.Drawing.Size(339, 70);
            this.lvAllRegions.TabIndex = 12;
            this.lvAllRegions.UseCompatibleStateImageBehavior = false;
            this.lvAllRegions.View = System.Windows.Forms.View.Details;
            // 
            // chID
            // 
            this.chID.Text = "ID";
            this.chID.Width = 0;
            // 
            // chRegion
            // 
            this.chRegion.Text = "Регион";
            this.chRegion.Width = 263;
            // 
            // chCountry
            // 
            this.chCountry.Text = "Страна";
            this.chCountry.Width = 120;
            // 
            // lvWorking
            // 
            this.lvWorking.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvWorking.FullRowSelect = true;
            this.lvWorking.GridLines = true;
            this.lvWorking.HideSelection = false;
            this.lvWorking.LabelEdit = true;
            this.lvWorking.Location = new System.Drawing.Point(398, 189);
            this.lvWorking.Name = "lvWorking";
            this.lvWorking.ShowGroups = false;
            this.lvWorking.Size = new System.Drawing.Size(283, 70);
            this.lvWorking.TabIndex = 12;
            this.lvWorking.UseCompatibleStateImageBehavior = false;
            this.lvWorking.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "ID";
            this.columnHeader4.Width = 0;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Регион";
            this.columnHeader5.Width = 300;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Страна";
            this.columnHeader6.Width = 120;
            // 
            // btnNextPage
            // 
            this.btnNextPage.Enabled = false;
            this.btnNextPage.Location = new System.Drawing.Point(707, 420);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(28, 23);
            this.btnNextPage.TabIndex = 2;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(707, 304);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(29, 23);
            this.btnTest.TabIndex = 13;
            this.btnTest.Text = "1";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnTest2
            // 
            this.btnTest2.Location = new System.Drawing.Point(708, 333);
            this.btnTest2.Name = "btnTest2";
            this.btnTest2.Size = new System.Drawing.Size(29, 23);
            this.btnTest2.TabIndex = 13;
            this.btnTest2.Text = "2";
            this.btnTest2.UseVisualStyleBackColor = true;
            this.btnTest2.Click += new System.EventHandler(this.btnTest2_Click);
            // 
            // btn3
            // 
            this.btn3.Location = new System.Drawing.Point(707, 362);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(29, 23);
            this.btn3.TabIndex = 13;
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btnGetLink
            // 
            this.btnGetLink.Enabled = false;
            this.btnGetLink.Location = new System.Drawing.Point(706, 391);
            this.btnGetLink.Name = "btnGetLink";
            this.btnGetLink.Size = new System.Drawing.Size(29, 23);
            this.btnGetLink.TabIndex = 13;
            this.btnGetLink.Text = "4";
            this.btnGetLink.UseVisualStyleBackColor = true;
            this.btnGetLink.Click += new System.EventHandler(this.btnGetLink_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 152);
            this.Controls.Add(this.btnGetLink);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btnTest2);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lvWorking);
            this.Controls.Add(this.lvAllRegions);
            this.Controls.Add(this.gbMethod);
            this.Controls.Add(this.tabSearchEngine);
            this.Controls.Add(this.richXPath);
            this.Controls.Add(this.btnFindText);
            this.Controls.Add(this.btnXPath);
            this.Controls.Add(this.tbFindText);
            this.Controls.Add(this.tbXPath);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lvResults);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Name = "frmMain";
            this.Text = "ZOV";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.tabSearchEngine.ResumeLayout(false);
            this.tbYandex.ResumeLayout(false);
            this.tbYandex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericYaPageCount)).EndInit();
            this.gbMethod.ResumeLayout(false);
            this.gbMethod.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        public System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox tbXPath;
        private System.Windows.Forms.Button btnXPath;
        private System.Windows.Forms.RichTextBox richXPath;
        private System.Windows.Forms.TextBox tbFindText;
        private System.Windows.Forms.Button btnFindText;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TabControl tabSearchEngine;
        private System.Windows.Forms.TabPage tbYandex;
        private System.Windows.Forms.CheckBox cbYandexDirect;
        private System.Windows.Forms.TabPage tpGoogle;
        private System.Windows.Forms.Label labYaPageCount;
        private System.Windows.Forms.NumericUpDown numericYaPageCount;
        private System.Windows.Forms.GroupBox gbMethod;
        private System.Windows.Forms.RadioButton rbAPI;
        private System.Windows.Forms.RadioButton rbWebDriver;
        private System.Windows.Forms.ListView lvAllRegions;
        private System.Windows.Forms.ColumnHeader chID;
        private System.Windows.Forms.ColumnHeader chRegion;
        private System.Windows.Forms.ColumnHeader chCountry;
        private System.Windows.Forms.ListView lvWorking;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnTest2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btnGetLink;
        private System.Windows.Forms.Button btnSearchByYandex;
        private System.Windows.Forms.Label labSearchText;
        private System.Windows.Forms.Label labRegion;
        private System.Windows.Forms.ComboBox cbRegion;
        private System.Windows.Forms.CheckBox cbDataBase;
        private System.Windows.Forms.CheckBox cbRelatedWords;
        private System.Windows.Forms.CheckBox cbExcel;
        private System.Windows.Forms.ComboBox tbSearchByYandex;
        private System.Windows.Forms.CheckBox cbProcessAllData;
    }
}

