using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XonixWfApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var wellcome = new WellcomeUC();
            Controls.Add(wellcome);
            ClientSize = wellcome.ClientSize;
            wellcome.AfterClickStartButton += OnResumeGame;
        }

        private void OnResumeGame(object sender, EventArgs e)
        {
            var game = new GameUC(1, 3) { ClientSize = ClientSize };
            Controls.Add(game);
            ClientSize = game.ClientSize;
            Controls.RemoveAt(0);
            game.AfterLevelOver += Game_AfterLevelOver;
            game.AfterGameOver += Game_AfterGameOver;
        }

        private void Game_AfterLevelOver(object sender, XonixModelLibrary.LevelInfoEventArgs args)
        {
            var game = new GameUC(args.Level, args.Lives) { ClientSize = ClientSize };
            Controls.Add(game);
            ClientSize = game.ClientSize;
            Controls.RemoveAt(0);
            game.AfterLevelOver += Game_AfterLevelOver;
            game.AfterGameOver += Game_AfterGameOver;
        }

        private void Game_AfterGameOver(object sender, EventArgs e)
        {
            var gamover = new GameoverUC();
            Controls.Add(gamover);
            ClientSize = gamover.ClientSize;
            Controls.RemoveAt(0);
            gamover.AfterClickResumeButton += OnResumeGame;
        }
    }
}
