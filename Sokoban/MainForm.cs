using Sokoban.View;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sokoban
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<int, ucLevel> levels = new Dictionary<int, ucLevel>();

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var size = Level.CalculateMaxSize();

            ucLevel level = new ucLevel() { Dock = DockStyle.Fill };
            Controls.Add(level);
            level.LevelNavigate += Level_LevelNavigate;
            levels.Add(0, level);

            ClientSize = new System.Drawing.Size((size.Width + 2) * 36, (size.Height + 2) * 36);
            CenterToScreen();
        }

        private void Level_LevelNavigate(object sender, LevelNavigateEventArgs e)
        {
            ucLevel level;
            int key = 0;
            switch (e.Command)
            {
                case LevelNavigateCommand.Next:
                    key = e.Level + 1;
                    if (levels.ContainsKey(key))
                        level = levels[key];
                    else
                    {
                        level = new ucLevel(key) { Dock = DockStyle.Fill };
                        level.LevelNavigate += Level_LevelNavigate;
                        levels.Add(key, level);
                    }
                    Controls.Add(level);
                    Controls.RemoveAt(0);
                    break;
                case LevelNavigateCommand.Prev:
                    key = e.Level - 1;
                    if (levels.ContainsKey(key))
                        level = levels[key];
                    else
                    {
                        level = new ucLevel(key) { Dock = DockStyle.Fill };
                        level.LevelNavigate += Level_LevelNavigate;
                        levels.Add(key, level);
                    }
                    Controls.Add(level);
                    Controls.RemoveAt(0);
                    break;
                case LevelNavigateCommand.Reset:
                    key = e.Level;
                    if (levels.ContainsKey(key))
                    {
                        level = levels[key];
                        level.Reset();
                    }
                    break;
            }
            Text = $"Sokoban (Level {key + 1})";
        }
    }
}
