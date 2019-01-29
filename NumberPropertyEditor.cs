using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImportShopOffers
{
    public partial class NumberPropertyEditor : UserControl, IEditor<ColumnInfo, XElement>
    {
        private ColumnInfo _columnInfo;
        private int _updating;

        public event EventHandler<ChangingEventArgs> StartChanging;
        public event EventHandler<EventArgs> Changed;

        public NumberPropertyEditor()
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

            nudMaximum.Value = columnInfo.Size > 0 ? columnInfo.Size : 32767;
            nudMinimum.Value = columnInfo.Offset > 0 ? columnInfo.Offset : 0;

            nudNumber.Maximum = columnInfo.Size > 0 ? columnInfo.Size :32767;
            nudNumber.Minimum = columnInfo.Offset > 0 ? columnInfo.Offset : 0;
            var value = (int)columnInfo.Value;
            nudNumber.Value = value < nudNumber.Minimum 
                ? nudNumber.Minimum 
                : value > nudNumber.Maximum 
                        ? nudNumber.Maximum 
                        : value;
            _updating--;
        }

        private void UpdateObject()
        {
            if (_updating > 0) return; // we are in updating mode

            // fire event
            StartChanging(this, new ChangingEventArgs("Number Value Style"));

            // send values back from GUI to object
            _columnInfo.Value = (int)nudNumber.Value;
            _columnInfo.Offset = (int)nudMinimum.Value;
            _columnInfo.Size = (int)nudMaximum.Value;

            // fire event
            Changed(this, EventArgs.Empty);
        }

        private void nudNumber_ValueChanged(object sender, EventArgs e)
        {
            UpdateObject();
        }
    }
}
