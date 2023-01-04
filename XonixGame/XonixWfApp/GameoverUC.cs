using System;
using System.Windows.Forms;

namespace XonixWfApp
{
    public partial class GameoverUC : UserControl
    {
        public GameoverUC()
        {
            InitializeComponent();
        }

        public event EventHandler AfterClickResumeButton;

        private void resumeButton_Click(object sender, EventArgs e)
        {
            AfterClickResumeButton?.Invoke(this, new EventArgs());
        }
    }
}
