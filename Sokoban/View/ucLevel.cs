using Sokoban.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sokoban.View
{
    public partial class ucLevel : UserControl
    {
        private readonly Level level;

        public ucLevel(int number = 0)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            level = new Level(number);
            level.LevelComplete += Level_LevelComplete;
            DoubleBuffered = true;
            kbdView.KeyDown += KbdView_KeyDown;
            btnPrev.Enabled = number > 0;
        }

        private void KbdView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    level.GoUp();
                    btnReset.Enabled = true;
                    break;
                case Keys.Down:
                    level.GoDown();
                    btnReset.Enabled = true;
                    break;
                case Keys.Left:
                    level.GoLeft();
                    btnReset.Enabled = true;
                    break;
                case Keys.Right:
                    level.GoRight();
                    btnReset.Enabled = true;
                    break;
                case Keys.Home:
                    level.Reset();
                    btnReset.Enabled = false;
                    return;
                default:
                    return;
            }
            Invalidate();
        }

        private event LevelNavigateEventHandler levelNavigate;

        public event LevelNavigateEventHandler LevelNavigate
        {
            add { levelNavigate += value; }
            remove { levelNavigate -= value; }
        }

        private void ucLevel_Load(object sender, EventArgs e)
        {
            // источник картинок для ресурсов: https://sokoban.info/?1_1
            level.CellStyle += (o, arg) =>
            {
                var cell = (Cell)o;
                switch (cell.Kind)
                {
                    case CellKind.Space:
                        arg.BackImage = Resources.SokobanSpace;
                        break;
                    case CellKind.Floor:
                        arg.BackImage = Resources.SokobanFloor;
                        break;
                    case CellKind.Wall:
                        arg.BackImage = Resources.SokobanWall;
                        break;
                    case CellKind.Storage:
                        arg.BackImage = Resources.SokobanStorage;
                        break;
                    case CellKind.Box:
                        arg.BackImage = Resources.SokobanBox;
                        break;
                    case CellKind.Boxed:
                        arg.BackImage = Resources.SokobanBoxed;
                        break;
                    case CellKind.Docker:
                        arg.BackImage = Resources.SokobanDocker;
                        break;
                }
            };
            kbdView.Top = Height + 5;
            kbdView.Focus();

            btnNext.Enabled = false;
        }

        private void Level_LevelComplete(object sender, EventArgs e)
        {
            btnNext.Enabled = level.CurrentLevel < level.LevelsCount - 1;
        }

        private void ucLevel_Paint(object sender, PaintEventArgs e)
        {
            var offset = new Point((ClientSize.Width - level.Width) / 2, (ClientSize.Height - level.Height) / 2);
            level.Draw(e.Graphics, offset);
        }

        private void ucLevel_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ucLevel_MouseMove(object sender, MouseEventArgs e)
        {
            var offset = new Point((ClientSize.Width - level.Width) / 2, (ClientSize.Height - level.Height) / 2);
            var cell = level.CellAt(e.Location, offset);
            Cursor = (cell != null && cell.Kind == CellKind.Docker) ? Cursors.Hand : Cursors.Default;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            kbdView.Focus();
            if (level.CurrentLevel < level.LevelsCount - 1)
                levelNavigate?.Invoke(this, new LevelNavigateEventArgs() { Command = LevelNavigateCommand.Next, Level = level.CurrentLevel });
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            kbdView.Focus();
            if (level.CurrentLevel > 0)
                levelNavigate?.Invoke(this, new LevelNavigateEventArgs() { Command = LevelNavigateCommand.Prev, Level = level.CurrentLevel });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = false;
            btnNext.Enabled = false;
            kbdView.Focus();
            levelNavigate?.Invoke(this, new LevelNavigateEventArgs() { Command = LevelNavigateCommand.Reset, Level = level.CurrentLevel });
        }

        public void Reset()
        {
            level.Reset();
            Invalidate();
        }
    }

    public delegate void LevelNavigateEventHandler(object sender, LevelNavigateEventArgs e);

    public enum LevelNavigateCommand
    {
        Reset,
        Next,
        Prev
    }

    public class LevelNavigateEventArgs : EventArgs
    {
        public LevelNavigateEventArgs() { }
        public LevelNavigateEventArgs(LevelNavigateCommand command, int level) 
        {
            Command = command;
            Level = level;
        }

        public LevelNavigateCommand Command { get; set; }
        public int Level { get; set; }
    }
}
