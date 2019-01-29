using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImportShopOffers
{
    public partial class XmlValuePropertyEditor : UserControl, IEditor<ColumnInfo, XElement>
    {
        private ColumnInfo _columnInfo;
        private int _updating;

        public event EventHandler<ChangingEventArgs> StartChanging;
        public event EventHandler<EventArgs> Changed;

        public XmlValuePropertyEditor()
        {
            InitializeComponent();
        }

        public void Build(ColumnInfo columnInfo, XElement content)
        {
            // remember editing object
            _columnInfo = columnInfo;
            groupBox1.Text = string.Format("Значение свойства {0} из файла", columnInfo.Text);

            // copy properties of object to GUI
            _updating++;

            cbNames.Items.Clear();
            foreach (var item in content.Elements()
                                    .Select(x => x.Name.ToString())
                                    .Distinct()
                                    .OrderBy(name => name))
                cbNames.Items.Add(item);
            cbNames.Text = columnInfo.Name;
            nudSize.Value = columnInfo.Size;
            _updating--;
        }

        private void UpdateObject()
        {
            if (_updating > 0) return; // we are in updating mode

            // fire event
            StartChanging(this, new ChangingEventArgs("Xml Value Style"));

            // send values back from GUI to object
            _columnInfo.Name = cbNames.Text;
            _columnInfo.Size = (int)nudSize.Value;

            // fire event
            Changed(this, EventArgs.Empty);
        }

        private void cbSize_CheckedChanged(object sender, EventArgs e)
        {
            UpdateObject();
        }
    }
}
