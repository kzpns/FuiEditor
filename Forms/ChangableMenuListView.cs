using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuiEditor.Forms
{
    public class ChangableMenuListView : ListView
    {
        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
        {
            base.OnItemSelectionChanged(e);

            int selectedCount = SelectedIndices.Count;

            if(selectedCount > 1)
            {
                ContextMenuStrip = MultiSelectedContextMenuStrip;
            }
            else
            {
                ContextMenuStrip = SingleSelectedContextMenuStrip;
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public ContextMenuStrip MultiSelectedContextMenuStrip
        {
            get;
            set;
        }

        [Browsable(true)]
        [Category("Behavior")]
        public ContextMenuStrip SingleSelectedContextMenuStrip
        {
            get;
            set;
        }
    }
}
