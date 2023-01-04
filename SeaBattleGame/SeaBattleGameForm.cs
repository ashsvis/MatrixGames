using MatrixGridViewControl;
using SeaBattleGame.Properties;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBattleGame
{
    public partial class SeaBattleGameForm : Form
    {
        public SeaBattleGameForm()
        {
            InitializeComponent();
            
            // базовый размер ячейки зависит от размера картинки для фона пустой ячейки
            var cellSize = Resources.DefaultCellImage.Size;

            //создаем грид игрока
            var gridPlayer = new MatrixGrid
            {
                Parent = this,
                Size = new Size(cellSize.Width * GameHelper.Side, cellSize.Height * GameHelper.Side),
                Location = new Point(cellSize.Width, cellSize.Height),
                GridSize = GameHelper.GridSize,
                Cursor = Cursors.No
            };

            playerScore.Caption = "Ваша эскадра";
            playerScore.Width = 178 + cellSize.Width;
            playerScore.ShipDesk4 = 1;
            playerScore.ShipDesk3 = 2;
            playerScore.ShipDesk2 = 3;
            playerScore.ShipDesk1 = 4;

            //создаем грид противника
            var gridEnemy = new MatrixGrid
            {
                Parent = this,
                Size = new Size(cellSize.Width * GameHelper.Side, cellSize.Height * GameHelper.Side),
                Location = new Point(gridPlayer.Right + cellSize.Width, gridPlayer.Top),
                GridSize = GameHelper.GridSize,
                Cursor = Cursors.Hand
            };

            enemyScore.Caption = "Противник";
            enemyScore.Width = 178 + cellSize.Width;
            enemyScore.ShipDesk4 = 1;
            enemyScore.ShipDesk3 = 2;
            enemyScore.ShipDesk2 = 3;
            enemyScore.ShipDesk1 = 4;

            //присваиваем событие, в котором будем отдавать текст ячейки грида игрока и ее цвет
            gridPlayer.CellNeeded += (o, e) =>
            {
                e.Visible = true;
                e.Value = GameHelper.MatrixPlayer[e.Cell.X, e.Cell.Y];
                DefaultBackgroundDrawing(e);
                DrawShipDesks(GameHelper.MatrixPlayer, e);
                DrawShipDeskActions(e);
            };

            //присваиваем событие, в котором будем отдавать текст ячейки грида противника и ее цвет
            gridEnemy.CellNeeded += (o, e) =>
            {
                e.Value = GameHelper.MatrixEnemy[e.Cell.X, e.Cell.Y];
                e.Visible = e.Value.EndsWith(GameHelper.SankMarker) || e.Value.EndsWith(GameHelper.LiveMarker);
                DefaultBackgroundDrawing(e);
                DrawShipDesks(GameHelper.MatrixEnemy, e);
                DrawShipDeskActions(e);
            };

            //обрабатываем событие клика по ячейке грида игрока
            gridPlayer.CellClick += (o, e) =>
            {
                if (e.Cell.X == 0 || e.Cell.Y == 0 || e.Cell.X == GameHelper.Side - 1 || e.Cell.Y == GameHelper.Side - 1) return;
            };

            //обрабатываем событие клика по ячейке грида противника
            gridEnemy.CellClick += async (o, e) =>
            {
                // стреляем только по рабочим ячейкам
                if (e.Cell.X == 0 || e.Cell.Y == 0 || e.Cell.X == GameHelper.Side - 1 || e.Cell.Y == GameHelper.Side - 1) return;
                // по ранее маркированным ячейкам более не стреляем
                if (!GameHelper.IsUnknown(GameHelper.MatrixEnemy, e.Cell)) return;
                var isHit = GameHelper.SetShotOrHit(GameHelper.MatrixEnemy, GameHelper.ShipsEnemy, e.Cell);
                // заполнение информации о счёте для эскадры противника
                enemyScore.Clear();
                foreach (var ship in GameHelper.ShipsEnemy)
                {
                    switch (ship.Desks)
                    {
                        case 1: enemyScore.ShipDesk1 += 1; break;
                        case 2: enemyScore.ShipDesk2 += 1; break;
                        case 3: enemyScore.ShipDesk3 += 1; break;
                        case 4: enemyScore.ShipDesk4 += 1; break;
                    }
                }
                if (enemyScore.ShipDeskCount() == 0)
                {
                    lbGameStatus.Text = "Ура! Вы выиграли!";
                    lbGameStatus.ForeColor = Color.Red;
                    gridEnemy.Enabled = false;
                    gridPlayer.Enabled = false;
                    btnRestart.Visible = true;
                    return;
                }
                if (isHit)
                {
                    lbGameStatus.Text = "Продолжайте стрелять.";
                    lbGameStatus.ForeColor = Color.Red;
                    return;
                }
                gridEnemy.Enabled = false;
                lbGameStatus.Text = "Противник стреляет...";
                lbGameStatus.ForeColor = Color.FromArgb(74, 48, 173);
                while (true)
                {
                    // ответный удар противника
                    var hp = GameController.NextHit();
                    await Task.Delay(500);
                    if (hp.IsEmpty) break;
                    GameController.UpdateFutureHits(hp, ActionKind.Remove);
                    isHit = GameHelper.SetShotOrHit(GameHelper.MatrixPlayer, GameHelper.ShipsPlayer, hp, GameController.UpdateFutureHits);
                    gridPlayer.Invalidate();
                    if (!isHit)
                    {
                        lbGameStatus.Text = "Ваш выстрел.";
                        lbGameStatus.ForeColor = Color.Red;
                        break;
                    }

                    // добавление обстрела обнаруженной палубы
                    GameHelper.UpdateShotsAroundDesk(GameHelper.MatrixPlayer, hp, GameController.UpdateFutureHits);

                    // заполнение информации о счёте для эскадры игрока
                    playerScore.Clear();
                    foreach (var ship in GameHelper.ShipsPlayer)
                    {
                        switch (ship.Desks)
                        {
                            case 1: playerScore.ShipDesk1 += 1; break;
                            case 2: playerScore.ShipDesk2 += 1; break;
                            case 3: playerScore.ShipDesk3 += 1; break;
                            case 4: playerScore.ShipDesk4 += 1; break;
                        }
                    }
                    await Task.Delay(1000);
                    if (playerScore.ShipDeskCount() == 0)
                    {
                        GameHelper.ShowLivedShips(GameHelper.MatrixEnemy, GameHelper.ShipsEnemy);
                        gridEnemy.Invalidate();
                        lbGameStatus.Text = "Вы проиграли!";
                        lbGameStatus.ForeColor = Color.FromArgb(74, 48, 173);
                        gridPlayer.Enabled = false;
                        btnRestart.Visible = true;
                        return;
                    }
                }
                gridEnemy.Enabled = true;
            };

            // обрабатываем событие по правой кнопке мыши по ячейке грида противника
            gridEnemy.CellContext += (o, e) =>
            {
                if (e.Cell.X == 0 || e.Cell.Y == 0 || e.Cell.X == GameHelper.Side - 1 || e.Cell.Y == GameHelper.Side - 1) return;

            };

            ClientSize = new Size(gridEnemy.Right + playerScore.Width, gridEnemy.Bottom + cellSize.Height * 2);

            gridPlayer.Enabled = false;
            DoubleBuffered = true;
        }

        /// <summary>
        /// Рисование действий на палубах кораблей
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="e"></param>
        private void DrawShipDeskActions(MatrixGrid.CellNeededEventArgs e)
        {
            if (e.IsBorder) return;
            if (e.Value.StartsWith(GameHelper.ShotMarker))
                e.Action = Resources.AddShotImage;
            else if (e.Value.EndsWith(GameHelper.HitMarker) || e.Value.EndsWith(GameHelper.SankMarker))
                e.Action = Resources.AddDestroyImage;
            else if (e.Value.EndsWith(GameHelper.ToggleMarker))
                e.Action = Resources.AddMarkImage;
        }

        /// <summary>
        /// Рисование палуб кораблей
        /// </summary>
        /// <param name="e"></param>
        private void DrawShipDesks(string[,] matrix, MatrixGrid.CellNeededEventArgs e)
        {
            if (e.IsBorder || !e.Visible) return;
            if (e.Value.StartsWith(GameHelper.Desk1Marker))
                e.Background = Resources.OneDeskShipCellImage;
            else if (e.Value.StartsWith(GameHelper.Desk2Marker))
            {
                if (matrix[e.Cell.X + 1, e.Cell.Y].StartsWith(GameHelper.Desk2Marker))
                    e.Background = Resources.LeftDeskShipCellImage;
                else if (matrix[e.Cell.X - 1, e.Cell.Y].StartsWith(GameHelper.Desk2Marker))
                    e.Background = Resources.RightDeskShipCellImage;
                else if (matrix[e.Cell.X, e.Cell.Y + 1].StartsWith(GameHelper.Desk2Marker))
                    e.Background = Resources.TopDeskShipCellImage;
                else if (matrix[e.Cell.X, e.Cell.Y - 1].StartsWith(GameHelper.Desk2Marker))
                    e.Background = Resources.BottomDeskShipCellImage;
            }
            else if (e.Value.StartsWith(GameHelper.Desk3Marker) || e.Value.StartsWith(GameHelper.Desk4Marker))
            {
                var value = e.Value.Substring(0, 1);
                if (matrix[e.Cell.X - 1, e.Cell.Y].StartsWith(value) &&
                    matrix[e.Cell.X + 1, e.Cell.Y].StartsWith(value))
                    e.Background = Resources.HorizDeskShipCellImage;
                else if (matrix[e.Cell.X - 1, e.Cell.Y].StartsWith(value))
                    e.Background = Resources.RightDeskShipCellImage;
                else if (matrix[e.Cell.X + 1, e.Cell.Y].StartsWith(value))
                    e.Background = Resources.LeftDeskShipCellImage;
                else if (matrix[e.Cell.X, e.Cell.Y - 1].StartsWith(value) &&
                         matrix[e.Cell.X, e.Cell.Y + 1].StartsWith(value))
                    e.Background = Resources.VertDeskShipCellImage;
                else if (matrix[e.Cell.X, e.Cell.Y - 1].StartsWith(value))
                    e.Background = Resources.BottomDeskShipCellImage;
                else if (matrix[e.Cell.X, e.Cell.Y + 1].StartsWith(value))
                    e.Background = Resources.TopDeskShipCellImage;
            }
        }

        /// <summary>
        /// Рисование фона ячеек для общих ситуаций
        /// </summary>
        /// <param name="e"></param>
        private static void DefaultBackgroundDrawing(MatrixGrid.CellNeededEventArgs e)
        {
            if (e.Cell.X == 0 && e.Cell.Y == 0)
                e.Background = Resources.TopLeftCellImage;
            else if (e.Cell.X == GameHelper.Side - 1 && e.Cell.Y == 0)
                e.Background = Resources.TopRightCellImage;
            else if (e.Cell.X == GameHelper.Side - 1 && e.Cell.Y == GameHelper.Side - 1)
                e.Background = Resources.BottomRightCellImage;
            else if (e.Cell.X == 0 && e.Cell.Y == GameHelper.Side - 1)
                e.Background = Resources.BottomLeftCellImage;
            else if (e.Cell.X > 0 && e.Cell.Y == 0)
                e.Background = Resources.TopBorderCellImage;
            else if (e.Cell.X == 0 && e.Cell.Y > 0)
                e.Background = Resources.LeftBorderCellImage;
            else if (e.Cell.X == GameHelper.Side - 1 && e.Cell.Y > 0)
                e.Background = Resources.RightBorderCellImage;
            else if (e.Cell.X > 0 && e.Cell.Y == GameHelper.Side - 1)
                e.Background = Resources.BottomBorderCellImage;
            else
                e.Background = Resources.DefaultCellImage;
            if (e.Cell.X == 0 || e.Cell.Y == 0 || e.Cell.X == GameHelper.Side - 1 || e.Cell.Y == GameHelper.Side - 1)
            {
                e.BackColor = Color.Transparent;
                e.IsBorder = true;
            }
            if (e.Cell.X > 0 && e.Cell.X < GameHelper.Side - 1 && e.Cell.Y == 0)
            {
                e.Value = e.Cell.X.ToString();
            }
            if (e.Cell.Y > 0 && e.Cell.Y < GameHelper.Side - 1 && e.Cell.X == 0)
            {
                e.Value = GameHelper.RowHeaders[e.Cell.Y - 1].ToString();
            }
        }

        private void SeaBattleGameForm_Load(object sender, EventArgs e)
        {
            GameHelper.InitPlaceShips();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

    }
}
