using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBattleGame
{
    public partial class ScoreUC : UserControl
    {
        public string Caption { get => lbCaption.Text; set => lbCaption.Text = value; }
        public int ShipDesk4 { get => int.Parse(lbFourDeskCount.Text); set => lbFourDeskCount.Text = value.ToString(); }
        public int ShipDesk3 { get => int.Parse(lbThreeDeskCount.Text); set => lbThreeDeskCount.Text = value.ToString(); }
        public int ShipDesk2 { get => int.Parse(lbTwoDeskCount.Text); set => lbTwoDeskCount.Text = value.ToString(); }
        public int ShipDesk1 { get => int.Parse(lbOneDeskCount.Text); set => lbOneDeskCount.Text = value.ToString(); }

        public ScoreUC()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        public void Clear()
        {
            ShipDesk1 = 0;
            ShipDesk2 = 0;
            ShipDesk3 = 0;
            ShipDesk4 = 0;
        }

        public int ShipDeskCount()
        {
            lbOneDeskCount.Visible = lbOneDeskCaption.Visible = ShipDesk1 > 0;
            lbTwoDeskCount.Visible = lbTwoDeskCaption.Visible = ShipDesk2 > 0;
            lbThreeDeskCount.Visible = lbThreeDeskCaption.Visible = ShipDesk3 > 0;
            lbFourDeskCount.Visible = lbFourDeskCaption.Visible = ShipDesk4 > 0;

            AnimateLabels();

            return ShipDesk1 + ShipDesk2 + ShipDesk3 + ShipDesk4;
        }

        private async void AnimateLabels()
        {
            var desks = new int[] { ShipDesk4, ShipDesk3, ShipDesk2 };

            for (var i = 0; i < desks.Length; i++)
            {
                if (desks[i] == 0 && tlpTable.RowStyles[i + 1].Height > 0)
                {
                    while (tlpTable.RowStyles[i + 1].Height > 0)
                    {
                        await Task.Delay(1);
                        tlpTable.RowStyles[i + 1].Height -= tlpTable.RowStyles[i + 1].Height / 8;
                        if (tlpTable.RowStyles[i + 1].Height < 0.1f)
                            tlpTable.RowStyles[i + 1].Height = 0;
                    }
                }
            }
        }
    }
}
