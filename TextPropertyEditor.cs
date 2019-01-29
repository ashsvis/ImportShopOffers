using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImportShopOffers
{
    public partial class TextPropertyEditor : UserControl, IEditor<ColumnInfo, XElement>
    {
        private ColumnInfo _columnInfo;
        private int _updating;

        public event EventHandler<ChangingEventArgs> StartChanging;
        public event EventHandler<EventArgs> Changed;

        public TextPropertyEditor()
        {
            InitializeComponent();
        }

        public void Build(ColumnInfo columnInfo, XElement content)
        {
            // remember editing object
            _columnInfo = columnInfo;
            groupBox1.Text = string.Format("Значение свойства {0}", columnInfo.Text);

            // copy properties of object to GUI
            _updating++;
            tbText.Text = (string)columnInfo.Value;
            nudSize.Value = columnInfo.Size;
            _updating--;
        }

        private void UpdateObject()
        {
            if (_updating > 0) return; // we are in updating mode

            // fire event
            StartChanging?.Invoke(this, new ChangingEventArgs("Text Value Style"));

            // send values back from GUI to object
            _columnInfo.Value = tbText.Text;
            _columnInfo.Size = (int)nudSize.Value;

            // fire event
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void cbSize_CheckedChanged(object sender, EventArgs e)
        {
            UpdateObject();
        }

        private void tbText_Leave(object sender, EventArgs e)
        {
            UpdateObject();
        }
    }
}
