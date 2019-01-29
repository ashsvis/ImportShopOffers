using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImportShopOffers
{
    public partial class FlagPropertyEditor : UserControl, IEditor<ColumnInfo, XElement>
    {
        private ColumnInfo _columnInfo;
        private int _updating;

        public event EventHandler<ChangingEventArgs> StartChanging;
        public event EventHandler<EventArgs> Changed;

        public FlagPropertyEditor()
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

            nudFlag.Value = (int)columnInfo.Value;
            _updating--;
        }

        private void UpdateObject()
        {
            if (_updating > 0) return; // we are in updating mode

            // fire event
            StartChanging(this, new ChangingEventArgs("Flag Value Style"));

            // send values back from GUI to object
            _columnInfo.Value = (int)nudFlag.Value;

            // fire event
            Changed(this, EventArgs.Empty);
        }

        private void nudFlag_ValueChanged(object sender, EventArgs e)
        {
            UpdateObject();
        }
    }
}
