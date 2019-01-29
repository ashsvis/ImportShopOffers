using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImportShopOffers
{
    public partial class MainForm : Form
    {
        private OfferCategories _offerCategories;
        private AllOffers _allOffers;
        //private List<ColumnInfo> _columns;
        private PattertInfo _pattern;

        public MainForm()
        {
            InitializeComponent();
            _pattern = new PattertInfo();
            //_columns = new List<ColumnInfo>();
            FillColumns();
            _allOffers = new AllOffers();
            _offerCategories = new OfferCategories();
        }

        private void FillColumns()
        {
            _pattern.Columns = new List<ColumnInfo>
            {
                new ColumnInfo { Text = "Название", ColumnType = ColumnType.Value, Name = "name", Size = 70, Index = 0 },
                new ColumnInfo { Text = "Тип торгов", ColumnType = ColumnType.Flag, Value = 0, Index = 1 },
                new ColumnInfo { Text = "Описание", ColumnType = ColumnType.Value, Name = "description", Index = 2 },
                new ColumnInfo { Text = "Номер категории", ColumnType = ColumnType.Number, Value = 0, Index = 3 },
                new ColumnInfo { Text = "Фото 1", ColumnType = ColumnType.Collection, Name = "picture", Subindex = 0, Index = 4 },
                new ColumnInfo { Text = "Фото 2", ColumnType = ColumnType.Collection, Name = "picture", Subindex = 1, Index = 5 },
                new ColumnInfo { Text = "Фото 3", ColumnType = ColumnType.Collection, Name = "picture", Subindex = 2, Index = 6 },
                new ColumnInfo { Text = "Фото 4", ColumnType = ColumnType.Collection, Name = "picture", Subindex = 3, Index = 7 },
                new ColumnInfo { Text = "Фото 5", ColumnType = ColumnType.Collection, Name = "picture", Subindex = 4, Index = 8 },
                new ColumnInfo { Text = "Фото 6", ColumnType = ColumnType.Collection, Name = "picture", Subindex = 5, Index = 9 },
                new ColumnInfo { Text = "Фото 7", ColumnType = ColumnType.Collection, Name = "picture", Subindex = 6, Index = 10 },
                new ColumnInfo { Text = "Наличие", ColumnType = ColumnType.Flag, Value = 0, Index = 11 },
                new ColumnInfo { Text = "Состояние", ColumnType = ColumnType.Flag, Value = 0, Index = 12 },
                new ColumnInfo { Text = "Месторасположение", ColumnType = ColumnType.Text, Value = "", Index = 13 },
                new ColumnInfo { Text = "Длительность торгов", ColumnType = ColumnType.Number, Value = 1, Offset = 1, Size = 30, Index = 14 },
                new ColumnInfo { Text = "Количество", ColumnType = ColumnType.Number, Value = 0, Index = 15 },
                new ColumnInfo { Text = "Цена", ColumnType = ColumnType.Value, Name = "price", Index = 16 }
            };
        }

        private void OffersForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("Готово.");
        }

        private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            Cursor = Cursors.WaitCursor;
            try
            {
                HideLastPropertyEditor();
                tbDescriptor.Clear();
                UpdateStatus("Загрузка файла...");
                var fromFile = XElement.Load(openFileDialog1.FileName);
                UpdateStatus("Извлечение предложений товаров...");
                _allOffers.Load(fromFile.Element("shop").Element("offers"));
                UpdateStatus("Извлечение категорий товаров...");
                _offerCategories.Load(fromFile.Element("shop").Element("categories"));
                UpdateStatus("Привязка товаров к категориям...");
                _offerCategories.LinkOffers(_allOffers);
                UpdateStatus("Подготовка иерархического списка...");
                BuildCategoriesTree(_offerCategories);
                UpdateStatus("Сортировка...");
                tvCategories.Sort();
                UpdateStatus("Готово.");
                UpdateInterface();
                tsbExportToExcel.Enabled = true;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateStatus(string mess)
        {
            lbStatus.Text = mess;
            statusStrip1.Update();
        }

        private void BuildCategoriesTree(OfferCategories katalog)
        {
            try
            {
                lvOffers.Items.Clear();
                lvProperties.Items.Clear();
                tvCategories.BeginUpdate();
                tvCategories.Nodes.Clear();
                foreach (var category in katalog.Categories)
                {
                    var node = new TreeNode(category.Name) { Tag = category };
                    tvCategories.Nodes.Add(node);
                    AddChilds(category, node);
                }
            }
            finally
            {
                tvCategories.EndUpdate();
            }
        }

        private void AddChilds(OfferCategory category, TreeNode node)
        {
            foreach (var child in category.Childs)
            {
                var childNode = new TreeNode(child.Name) { Tag = child };
                node.Nodes.Add(childNode);
                AddChilds(child, childNode);
            }
        }

        private void tvCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;
            lvProperties.Items.Clear();
            HideLastPropertyEditor();
            tbDescriptor.Clear();
            UpdateInterface();
            try
            {
                lvOffers.BeginUpdate();
                lvOffers.Items.Clear();
                var category = e.Node.Tag as OfferCategory;
                if (category != null)
                {
                    foreach (var offer in category.Offers.OrderBy(item => item.Name))
                    {
                        var lvi = new ListViewItem(offer.Name);
                        lvi.Tag = offer;
                        lvOffers.Items.Add(lvi);
                    }
                }
            }
            finally
            {
                lvOffers.EndUpdate();
            }
        }

        private void lvOffers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
            FillProperties();
        }

        private void FillProperties()
        {
            try
            {
                lvProperties.BeginUpdate();
                lvProperties.Items.Clear();
                tbDescriptor.Clear();
                if (lvOffers.SelectedItems.Count == 0) return;
                var offer = lvOffers.SelectedItems[0].Tag as Offer;
                if (offer == null) return;
                foreach (var prop in _pattern.Columns.OrderBy(item => item.Index))
                {
                    var lvi = new ListViewItem((prop.Index + 1).ToString());
                    lvi.Tag = prop;
                    lvi.SubItems.Add(prop.ToString());
                    switch (prop.ColumnType)
                    {
                        case ColumnType.Flag:
                        case ColumnType.Number:
                            lvi.SubItems.Add(prop.Value.ToString());
                            break;
                        case ColumnType.Text:
                            lvi.SubItems.Add(TrimByLength(prop.Value.ToString(), prop.Size));
                            break;
                        case ColumnType.Collection:
                        case ColumnType.Value:
                            var content = offer.GetContent();
                            var name = prop.Name;
                            var value = content.Element(name).Value;
                            switch (name)
                            {
                                case "picture":
                                    var n = 0;
                                    foreach (var picture in content.Elements(name))
                                    {
                                        if (prop.Subindex == n++)
                                        {
                                            lvi.SubItems.Add(picture.Value);
                                            break;
                                        }
                                    }
                                    lvi.SubItems.Add("");
                                    break;
                                case "description":
                                    UpdateDescriptor();
                                    lvi.SubItems.Add("(смотри справа, в отдельном окне)");
                                    break;
                                case "name":
                                    lvi.SubItems.Add(TrimByLength(value, prop.Size));
                                    break;
                                default:
                                    if (prop.ColumnType != ColumnType.Collection)
                                        lvi.SubItems.Add(TrimByLength(value, prop.Size));
                                    else
                                        lvi.SubItems.Add(value);
                                    break;
                            }
                            break;
                        default:
                            lvi.SubItems.Add("(not defined)");
                            break;
                    }
                    lvProperties.Items.Add(lvi);
                }
            }
            finally
            {
                lvProperties.EndUpdate();
            }
        }

        private string TrimByLength(string value, int size)
        {
            if (size > 0)
            {
                var values = value.Split(new[] { ' ' });
                var words = new List<string>(values);
                var sb = new StringBuilder();
                var result = new List<string>();
                while (sb.Length < size && words.Count > 0)
                {
                    result.Add(words[0]);
                    sb.AppendFormat(" {0}", words[0]);
                    words.RemoveAt(0);
                }
                return string.Join(" ", result);
            }
            return value;
        }

        private void UpdateInterface()
        {
            tsbApply.Enabled = tsbCancel.Enabled = tsbMoveUp.Enabled = tsbMoveDown.Enabled = false;
            cbAddArticleBeforeDescription.Enabled = cbAddExtraFieldsAfterDescription.Enabled =
                cbAddParamAfterDescription.Enabled = cbCleanHtmlFromDescription.Enabled = 
                                lvOffers.SelectedItems.Count > 0;
        }

        private void lvProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProperties.SelectedItems.Count == 0)
            {
                HideLastPropertyEditor();
                return;
            }
            var index = lvProperties.SelectedIndices[0];
            tsbMoveUp.Enabled = index > 0;
            tsbMoveDown.Enabled = index < lvProperties.Items.Count - 1;
            ShowColumnPropertyEditor();
        }

        private void tsbMoveUp_Click(object sender, EventArgs e)
        {
            HideLastPropertyEditor();
            var item = lvProperties.SelectedItems[0];
            var index = item.Index - 1;
            lvProperties.Items.Remove(item);
            lvProperties.Items.Insert(index, item).Selected = true;
            tsbApply.Enabled = tsbCancel.Enabled = true;
        }

        private void tsbMoveDown_Click(object sender, EventArgs e)
        {
            HideLastPropertyEditor();
            var item = lvProperties.SelectedItems[0];
            var index = item.Index + 1;
            lvProperties.Items.Remove(item);
            lvProperties.Items.Insert(index, item).Selected = true;
            tsbApply.Enabled = tsbCancel.Enabled = true;
        }

        private void tsbApply_Click(object sender, EventArgs e)
        {
            HideLastPropertyEditor();
            Renumeration();
            FillProperties();
            UpdateInterface();
        }

        private void Renumeration()
        {
            for (var i = 0; i < lvProperties.Items.Count; i++)
            {
                var lvi = lvProperties.Items[i];
                var column = lvi.Tag as ColumnInfo;
                if (column != null)
                    column.Index = i;
            }
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            HideLastPropertyEditor();
            FillProperties();
            tsbApply.Enabled = tsbCancel.Enabled = false;
        }

        private void ShowColumnPropertyEditor()
        {
            var offer = (Offer)lvOffers.SelectedItems[0].Tag;
            IEditor<ColumnInfo, XElement> editor;
            HideLastPropertyEditor();
            var item = lvProperties.SelectedItems[0];
            var info = (ColumnInfo)item.Tag;
            editor = null;
            switch (info.ColumnType)
            {
                case ColumnType.Value:
                    var propEditor = new XmlValuePropertyEditor();
                    panelForProperty.Controls.Add(propEditor);
                    editor = propEditor;
                    break;
                case ColumnType.Flag:
                    var flagEditor = new FlagPropertyEditor();
                    panelForProperty.Controls.Add(flagEditor);
                    editor = flagEditor;
                    break;
                case ColumnType.Number:
                    var numberEditor = new NumberPropertyEditor();
                    panelForProperty.Controls.Add(numberEditor);
                    editor = numberEditor;
                    break;
                case ColumnType.Text:
                    var textEditor = new TextPropertyEditor();
                    panelForProperty.Controls.Add(textEditor);
                    editor = textEditor;
                    break;
                case ColumnType.Collection:
                    var collectEditor = new CollectionPropertyEditor();
                    panelForProperty.Controls.Add(collectEditor);
                    editor = collectEditor;
                    break;
            }
            if (editor != null)
            {
                editor.StartChanging += Editor_StartChanging;
                editor.Changed += Editor_Changed;
                editor.Build(info, offer.GetContent());
            }
        }

        private void HideLastPropertyEditor()
        {
            if (panelForProperty.Controls.Count > 0)
            {
                var editor = (IEditor<ColumnInfo, XElement>)panelForProperty.Controls[0];
                editor.StartChanging -= Editor_StartChanging;
                editor.Changed -= Editor_Changed;
            }
            panelForProperty.Controls.Clear();
        }

        private void Editor_Changed(object sender, EventArgs e)
        {
            if (lvOffers.SelectedItems.Count == 0) return;
            var offer = (Offer)lvOffers.SelectedItems[0].Tag;
            if (lvProperties.SelectedItems.Count == 0) return;
            var item = lvProperties.SelectedItems[0];
            var prop = item.Tag as ColumnInfo;
            if (prop == null) return;
            var value = CalculateValue(offer, _pattern, prop);
            if (item.SubItems.Count < 3) return;
            item.SubItems[2].Text = value;
            if (prop.ColumnType == ColumnType.Value && prop.Name == "description")
                UpdateDescriptor();
        }

        private string CalculateValue(Offer offer, PattertInfo pattern, ColumnInfo prop, bool withDescriptor = false)
        {
            switch (prop.ColumnType)
            {
                case ColumnType.Flag:
                case ColumnType.Number:
                    return prop.Value.ToString();
                case ColumnType.Text:
                    return TrimByLength(prop.Value.ToString(), prop.Size);
                case ColumnType.Collection:
                case ColumnType.Value:
                    var content = offer.GetContent();
                    var name = prop.Name;
                    var value = content.Element(name).Value;
                    switch (name)
                    {
                        case "picture":
                            var n = 0;
                            foreach (var picture in content.Elements(name))
                                if (prop.Subindex == n++)
                                    return picture.Value;
                            return "";
                        case "description":
                            var desc = "(смотри справа, в отдельном окне)";
                            if (withDescriptor)
                                desc = offer.GetDescriptorText(_pattern);
                            return desc;
                        case "name":
                            return TrimByLength(value, prop.Size);
                        default:
                            if (prop.ColumnType != ColumnType.Collection)
                                return TrimByLength(value, prop.Size);
                            else
                                return value;
                    }
                default:
                    return "(not defined)";
            }
        }

        private string _lastSelected;

        private void Editor_StartChanging(object sender, ChangingEventArgs e)
        {
            _lastSelected = lvProperties.SelectedItems.Count > 0 ? lvProperties.SelectedItems[0].Text : null;
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbExportToExcel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            var fileName = saveFileDialog1.FileName;
            if (File.Exists(fileName)) File.Delete(fileName);
            ExportToExcel(fileName, _allOffers, _pattern.Columns);
        }

        private void ExportToExcel(string fileName, AllOffers allOffers, List<ColumnInfo> columns)
        {
            if (columns.Count == 0) return;
            var s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var rowCount = allOffers.Offers.Count + 1;
            var colCount = columns.Count;
            object[,] arr = new object[rowCount, colCount];
            // добавим заголовки
            var n = 0;
            foreach (var column in columns)
                arr[0, n++] = column.Text;
            // основная таблица, построчно
            var row = 1;
            foreach (var offer in allOffers.Offers)
            {
                n = 0;
                foreach (var column in columns)
                    arr[row, n++] = CalculateValue(offer, _pattern, column, true);
                row++;
            }

            dynamic xl = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
            try
            {
                Cursor = Cursors.WaitCursor;
                var wb = xl.Workbooks.Add();
                var sheet = wb.Sheets[1];
                var range = sheet.Range(string.Format("A1:{0}{1}", s[colCount - 1], rowCount));
                range.Value = arr;
                wb.SaveAs(fileName);
                wb.Close();
            }
            finally
            {
                xl.Quit();
                xl = null;
                Cursor = Cursors.Default;

            }
        }

        private void cbCleanHtmlFromDescription_CheckedChanged(object sender, EventArgs e)
        {
            _pattern.CleanHtmlFromDescription = (sender as CheckBox).Checked;
            UpdateDescriptor();
        }

        private void cbAddParamAfterDescription_CheckedChanged(object sender, EventArgs e)
        {
            _pattern.AddParamAfterDescription = (sender as CheckBox).Checked;
            UpdateDescriptor();
        }

        private void cbAddArticleBeforeDescription_CheckedChanged(object sender, EventArgs e)
        {
            _pattern.AddArticleBeforeDescription = (sender as CheckBox).Checked;
            UpdateDescriptor();
        }

        private void cbAddExtraFieldsAfterDescription_CheckedChanged(object sender, EventArgs e)
        {
            _pattern.AddExtraFieldsAfterDescription = (sender as CheckBox).Checked;
            UpdateDescriptor();
        }

        private void UpdateDescriptor()
        {
            var offer = (Offer)lvOffers.SelectedItems[0].Tag;
            tbDescriptor.Text = offer.GetDescriptorText(_pattern);
        }
    }
}
