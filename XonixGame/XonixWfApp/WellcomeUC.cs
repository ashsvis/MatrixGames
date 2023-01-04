using System;
using System.Windows.Forms;

namespace XonixWfApp
{
    public partial class WellcomeUC : UserControl
    {
        public WellcomeUC()
        {
            InitializeComponent();
        }

        public event EventHandler AfterClickStartButton;

        private void startButton_Click(object sender, EventArgs e)
        {
            AfterClickStartButton?.Invoke(this, new EventArgs());
        }
    }
}
