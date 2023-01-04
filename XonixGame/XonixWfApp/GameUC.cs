using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XonixModelLibrary;

namespace XonixWfApp
{
    public partial class GameUC : UserControl
    {
        const int step = 10;            // число пикселей на сторону базового квадратика игрового поля
        private Area Area;              // игровое поле, двухмерный массив статусов
        private IPlayer player;         // "игрок" с позицией и направлением движения
        private List<IEnemy> enemies;   // список "врагов" с позициями и направлениями движения

        // хранение числа пропусков при обработке в таймере
        const int skipGoal = 3;
        private int skip;

        const int ResultPanelHeight = 30;
        private readonly int enemyCount = 1;
        private readonly int lives;

        public event LevelInfoEventHandler AfterLevelOver;
        public event EventHandler AfterGameOver;

        public GameUC(int level, int lives)
        {
            InitializeComponent();
            // включаем двойную буферизацию, иначе всё будет страшно мерцать
            DoubleBuffered = true;

            enemyCount = level;
            this.lives = lives;
        }

        /// <summary>
        /// Подготовка рабочего поля модели
        /// </summary>
        private void InitArea()
        {
            // выделяем рабочее поле
            Area = new Area(ClientSize.Width / step, ClientSize.Height / step);
            Area.AfterAreaFilled += Player_AfterAreaFilled;
            Area.After75PrecentFilled += Area_After75PrecentFilled;

            // позиция "игрока" занимает среднюю позицию свеху на "суше"
            player = new Player(lives, Area.MaxRows() / 2, 0);
            player.AfterAllLivesLossed += Player_AfterAllLivesLossed;

            // подготовка генератора случайных чисел
            var rand = new Random();
            // подготовка списка "врагов"
            enemies = new List<IEnemy>();
            // заполняем список случайными позициями и направлениями движения "врагов"
            for (var i = 0; i < enemyCount; i++)
            {
                var enemy = new Enemy()
                {
                    // позиция "врага" может располагаться внутри рамки с "сушей"
                    Location = new Location(rand.Next(2, Area.MaxRows() - 2), rand.Next(2, Area.MaxColumns() - 2)),
                    // направление движения любое, кроме начального никакого
                    MoveState = (EnemyMoveState)rand.Next(1, 5),
                };
                enemy.FootmarkFound += Enemy_FootmarkFound;
                enemies.Add(enemy);
            }
            // по списку назначаем признаки "врагов" в игровом поле
            enemies.ForEach(enemy => Area[enemy.Location.X, enemy.Location.Y] = (int)CellState.Enemy);
        }

        private async void Area_After75PrecentFilled(object sender, EventArgs e)
        {
            await Pause();
            AfterLevelOver?.Invoke(this, new LevelInfoEventArgs() { Level = enemyCount + 1, Lives = player.Lives});
        }

        private void Player_AfterAllLivesLossed(object sender, EventArgs e)
        {
            AfterGameOver?.Invoke(this, new EventArgs());
        }

        private void Player_AfterAreaFilled(object sender, EventArgs e)
        {
            Invalidate();
        }

        private bool footmarkFound;

        /// <summary>
        /// Обработчик события, наступающего при пересечении следа "игрока"
        /// </summary>
        /// <param name="sender">ссылка на объект enemy</param>
        /// <param name="e"></param>
        private void Enemy_FootmarkFound(object sender, EventArgs e)
        {
            footmarkFound = true; // взводим признак пересечения "следа"
        }

        private async Task Pause()
        {
            stepTimer.Stop();
            await Task.Delay(1000);
            stepTimer.Start();
        }

        /// <summary>
        /// Метод выполняется один раз после загрузки компонента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameUC_Load(object sender, EventArgs e)
        {
            // подготовка рабочего поля модели
            InitArea();
            // корректируем размер клиентской области приложения
            var size = new Size(Area.MaxRows() * step, Area.MaxColumns() * step);
            // снизу добавляем полосу для отображения результатов
            size.Height += ResultPanelHeight;
            ClientSize = size;
            // запускаем таймер
            stepTimer.Interval = 10;
            stepTimer.Enabled = true;
        }

        /// <summary>
        /// Метод обработки события таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void stepTimer_Tick(object sender, EventArgs e)
        {
            // если кадр ключевой
            if (skip == 0)
            {
                // обновление игрового статусов поля
                Area.UpdateLocation(player, enemies);

                // список перемещений врагов, подготовленный для групповой операции
                var nextEnemyMoves = new List<NextOperation>();
                // сбрасываем признак пересечения "следа" перед подготовкой перемещений
                footmarkFound = false;
                // подготавливаем перемещения
                enemies.ForEach(enemy => enemy.PrepareEnemies(Area, nextEnemyMoves));
                // при подготовке перемещений "врагов" могло быть пересечение "следа" игрока
                if (footmarkFound)
                {
                    // сбрасываем признак пересечения "следа"
                    footmarkFound = false;

                    await Pause();

                    // очищаем список подготовленных перемещений "врагов"
                    nextEnemyMoves.Clear();
                    // очищаем игровое поле от пересечённого "следа"
                    Area.ReturnPlayerAfterLoss(player);
                    // автоматическое движение игрока прекращаем
                    player.StopMove();
                }
                // собственно перемещаем
                nextEnemyMoves.ForEach(item => Area.UpdateEnemyCell(item.Enemy, item.Direct, step));
                skip = skipGoal;
            }
            else
            {
                skip--;
                // для промежуточных кадров просто уменьшаем внутреннее смещение фишек "врагов" для более плавного их перемещения
                enemies.ForEach(enemy => enemy.DecrementEnemiesOffset(step / skipGoal));
            }
            Invalidate();
        }

        /// <summary>
        /// Метод прорисовки на поверхности формы, вызывается системой по мере неоходимости или при вызове метода Invalidate() формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameUC_Paint(object sender, PaintEventArgs e)
        {
            // базовый квадратик для рисования мозаики игрового поля
            var rect = new Rectangle(0, 0, step, step);
            // для всех ячеек игрового поля
            foreach (var cell in Area)
            {
                // смещаем базовый квадратик в позицию ячейки игрового поля
                rect.Location = new Point(cell.Row * step, cell.Column * step);
                // в зависимости от типа (статуса) игоровй ячеки окрашиваем поверхность:
                switch (cell.State)
                {
                    case CellState.Wall: // область "суши"
                        e.Graphics.FillRectangle(Brushes.CadetBlue, rect);
                        break;
                    case CellState.Footmark: // область следа "игрока" в моменты перемещения по пустой области
                        e.Graphics.FillRectangle(Brushes.BlueViolet, rect);
                        break;
                    case CellState.Enemy: // область нахождения "чужаков" (врагов)
                    case CellState.None: // пустая область, ещё не занятая "сушей"
                        e.Graphics.FillRectangle(Brushes.Black, rect);
                        break;
                }
            }

            // рисуем игрока
            rect = new Rectangle(new Point(player.Location.X * step, player.Location.Y * step), new Size(step, step));
            e.Graphics.FillRectangle(Brushes.BlueViolet, rect);
            rect.Inflate(-3, -3);
            e.Graphics.FillRectangle(Brushes.White, rect);
            rect.Inflate(3, 3);

            // рисуем "врагов" отдельно, по списку
            foreach (var enemy in enemies)
            {
                // смещаем базовый квадратик в позицию ячейки игрового поля
                rect = new Rectangle(new Point(enemy.Location.X * step, enemy.Location.Y * step), new Size(step, step));
                // дополнительное смещение enemy после "оттяжки"
                var location = enemy.CalculateOffet();
                rect.Offset(location.X, location.Y);
                rect.Inflate(1, 1);
                e.Graphics.FillRectangle(Brushes.Transparent, rect);
                e.Graphics.FillEllipse(Brushes.White, rect);
                rect.Inflate(-2, -2);
                e.Graphics.FillEllipse(Brushes.CadetBlue, rect);
            }

            // рисуем панель с результатами
            rect = new Rectangle(new Point(0, ClientSize.Height - ResultPanelHeight), new Size(ClientSize.Width, ResultPanelHeight));
            e.Graphics.FillRectangle(Brushes.Black, rect);

            // отображение счётчика "Заполнено"
            var r = rect;
            r.Width = rect.Width / 3;
            r.Offset(2 * rect.Width / 3, 0);
            // получаем значение счётчика из модели игры
            var filled = Area.Filled;
            var filledText = $"Заполнено: {filled,3}%";
            using (var font = new Font("Segoe UI", 12f, FontStyle.Bold))
            {
                var size = Size.Ceiling(e.Graphics.MeasureString(filledText, font));
                e.Graphics.DrawString(filledText, font, Brushes.White, new Point(r.Left + r.Width - size.Width, r.Top + (r.Height - size.Height) / 2));
            }

            // отображение счётчика "Жизни"
            r = rect;
            r.Width = rect.Width / 3;
            r.Offset(rect.Width / 3, 0);
            // получаем значение счётчика из модели игры
            var lives = player.Lives;
            var livesText = $"Жизни: {lives}";
            using (var font = new Font("Segoe UI", 12f, FontStyle.Bold))
            {
                var size = Size.Ceiling(e.Graphics.MeasureString(livesText, font));
                e.Graphics.DrawString(livesText, font, Brushes.White, new Point(r.Left + (r.Width - size.Width) / 2, r.Top + (r.Height - size.Height) / 2));
            }
        }

        const int WM_KEYDOWN = 0x100;

        /// <summary>
        /// Метод для реакции алгоритма игры на нажатия кнопок клавиатуры
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == WM_KEYDOWN)
            {
                switch (keyData)
                {
                    // нажато для движения вниз
                    case Keys.S:
                    case Keys.Down:
                        player.MoveDown();
                        return true;
                    // нажато для движения вверх
                    case Keys.W:
                    case Keys.Up:
                        player.MoveUp();
                        return true;
                    // нажато для движения влево
                    case Keys.A:
                    case Keys.Left:
                        player.MoveLeft();
                        return true;
                    // нажато для движения вправо
                    case Keys.D:
                    case Keys.Right:
                        player.MoveRight();
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
