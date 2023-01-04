using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace XonixModelLibrary
{
    public class Area : IEnumerable<Cell>
    {
        private readonly int[,] area;

        private int underPlayerCellState;   // статус ячейки, с которой перемещается "игрок"

        public Area(int iSize, int jSize)
        {
            // выделяем рабочее поле
            area = new int[iSize, jSize];
            Init();
        }

        /// <summary>
        /// Подготовка рабочего поля модели
        /// </summary>
        private void Init()
        {
            // две верхние и две нижние полосы будут уже заняты "сушей"
            for (var i = 0; i < MaxRows(); i++)
            {
                area[i, 0] = (int)CellState.Wall;
                area[i, 1] = (int)CellState.Wall;
                area[i, MaxColumns() - 2] = (int)CellState.Wall;
                area[i, MaxColumns() - 1] = (int)CellState.Wall;
            }
            // две левые и две правые полосы будут уже заняты "сушей"
            for (var j = 0; j < MaxColumns(); j++)
            {
                area[0, j] = (int)CellState.Wall;
                area[1, j] = (int)CellState.Wall;
                area[MaxRows() - 2, j] = (int)CellState.Wall;
                area[MaxRows() - 1, j] = (int)CellState.Wall;
            }
            // сохраняем признак того, что под "игроком" сейчас "суша"
            underPlayerCellState = (int)CellState.Wall;
        }

        /// <summary>
        /// Доступ к ячейкам через индексатор
        /// </summary>
        /// <param name="i">индекс строк</param>
        /// <param name="j">индекс столбцов</param>
        /// <returns>возврат статуса ячейки</returns>
        public int this[int i, int j]
        {
            get { return area[i, j]; }
            set { area[i, j] = value; }
        }

        /// <summary>
        /// Количество строк матрицы
        /// </summary>
        /// <returns></returns>
        public int MaxRows()
        { 
            return area.GetLength(0); 
        }

        /// <summary>
        /// Количество столбцов матрицы
        /// </summary>
        /// <returns></returns>
        public int MaxColumns()
        {
            return area.GetLength(1);
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (var i = 0; i < MaxRows(); i++)
                for (var j = 0; j < MaxColumns(); j++)
                    yield return new Cell(this)
                    {
                        Row = i,
                        Column = j,
                        State = (CellState)area[i, j]
                    };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Перемещение позиции "игрока" либо заливка "сушей" захваченного участка пустого места
        /// </summary>
        public void UpdateLocation(IPlayer player, IEnumerable<IEnemy> enemies)
        {
            switch (player.MoveState)
            {
                case PlayerMoveState.Up:
                    if (player.Location.Y > 0)
                        UpdatePlayerCell(player, enemies, 0, -1);
                    else
                        ReplaceFootmarkToWall(player, enemies);
                    break;
                case PlayerMoveState.Down:
                    if (player.Location.Y < MaxColumns() - 1)
                        UpdatePlayerCell(player, enemies, 0, 1);
                    else
                        ReplaceFootmarkToWall(player, enemies);
                    break;
                case PlayerMoveState.Left:
                    if (player.Location.X > 0)
                        UpdatePlayerCell(player, enemies, -1, 0);
                    else
                        ReplaceFootmarkToWall(player, enemies);
                    break;
                case PlayerMoveState.Right:
                    if (player.Location.X < MaxRows() - 1)
                        UpdatePlayerCell(player, enemies, 1, 0);
                    else
                        ReplaceFootmarkToWall(player, enemies);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Собственно перемещение маркера "игрока" по игровому полю
        /// </summary>
        /// <param name="dx">смещение по горизонтали</param>
        /// <param name="dy">смещение по вертикали</param>
        private void UpdatePlayerCell(IPlayer player, IEnumerable<IEnemy> enemies, int dx, int dy)
        {
            // в текущую позицию под игроком помещаем запомненный маркер
            area[player.Location.X, player.Location.Y] = underPlayerCellState;
            // если это не "суша", то помещаем в эту позицию маркер "следа игрока"
            if (area[player.Location.X, player.Location.Y] != (int)CellState.Wall)
            {
                area[player.Location.X, player.Location.Y] = (int)CellState.Footmark;
                player.Footmarked();
            }
            // перемещаем позицию игрока на заданное смещение
            player.Offset(dx, dy);
            // запоминаем, что было в этой позиции, куда "наступит" "игрок"
            var beforeWasEmpty = underPlayerCellState == (int)CellState.None;
            underPlayerCellState = area[player.Location.X, player.Location.Y];
            // если под "игроком", куда он "наступил", была "суша", которая наступила сразу после пустого поля,
            if (beforeWasEmpty && underPlayerCellState == (int)CellState.Wall)
                ReplaceFootmarkToWall(player, enemies); // то заливаем его "след" на "суше" - "сушей"
        }

        /// <summary>
        /// Действия для очистки игрового поля при неудаче игрока со "следом"
        /// </summary>
        /// <param name="player"></param>
        public void ReturnPlayerAfterLoss(IPlayer player)
        {
            // место под игроком очищаем
            area[player.Location.X, player.Location.Y] = (int)CellState.None;
            // место под "следами" очищаем
            foreach (var cell in this.Where(item => item.State == CellState.Footmark))
                cell.State = CellState.None;
            // возврат игрока в позицию на "суше", откуда начал
            player.ReturnLocation();
            // статус ячейки под игроком - это "суша", так как верули его на "сушу"
            underPlayerCellState = (int)CellState.Wall;
            // забор жизни
            player.LiveLoss();
        }

        /// <summary>
        /// Метод для занятия сушей Wall после окончания движения player
        /// </summary>
        private void ReplaceFootmarkToWall(IPlayer player, IEnumerable<IEnemy> enemies)
        {
            if (player.IsStopped) return; // если player не двигался, выходим
            player.StopMove(); // сбрасываем признак движения player
            var wave = -1; // "затравка" цифровой волны
            // заполнение мест нахождения enemy "затравкой" цифровой волны
            foreach (var enemy in enemies)
                area[enemy.Location.X, enemy.Location.Y] = wave;
            // заполняем поэтапно области цифровой волной, пока есть свободное место для волны
            while (true)
            {
                // wave получает следующее отрицательное значение после окончания итерации
                if (!MakeLeeWaveCycle(ref wave))
                    break;
            }
            // если области поля не были заполнены цифровой волной (в т.ч. след от player),
            // то заполняем эти области признаком Wall
            var count = 0;
            foreach (var cell in this.Where(item => item.State == CellState.Footmark || item.State == CellState.None))
            {
                cell.State = CellState.Wall;
                count++;
            }

            Filled += (int)Math.Round(count * 100.0 / GamedAreaCount());
            // заполнение остатков цифровой волны признаками пустого поля
            foreach (var cell in this.Where(item => (int)item.State < 0))
                cell.State = CellState.None;
            // заполнение позиций enemy признаками enemy
            foreach (var enemy in enemies)
                area[enemy.Location.X, enemy.Location.Y] = (int)CellState.Enemy;

            AfterAreaFilled?.Invoke(this, new EventArgs());

            if (Filled >= 75)
                After75PrecentFilled?.Invoke(this, new EventArgs());
        }

        public event EventHandler After75PrecentFilled;

        /// <summary>
        /// Вычисление общего количества ячеек, свободных с начала игры,
        /// сюда не включаются ячейки изначального бордера с "сушей"
        /// </summary>
        /// <returns></returns>
        private int GamedAreaCount()
        {
            return (MaxRows() - 4) * (MaxColumns() - 4);
        }

        /// <summary>
        /// Счётчик процента заполненности "сушей" игрового поля
        /// </summary>
        public int Filled { get; set; }

        public event EventHandler AfterAreaFilled;

        /// <summary>
        /// Метод итерации цифровой волны
        /// </summary>
        /// <param name="wave">текущий номер итерации (отрицательное число)</param>
        /// <returns>Возвращается признак, того, что волна ещё была размещена во время этой итерации</returns>
        private bool MakeLeeWaveCycle(ref int wave)
        {
            var changed = false;
            // обходим матрицу по всем ячейкам
            for (var i = 0; i < MaxRows(); i++)
                for (var j = 0; j < MaxColumns(); j++)
                    if (area[i, j] == wave) // если текущая ячейка содержит крайний маркер цифровой волны, то
                    {
                        // ставим следующий маркер цифровой волны в ячейках вокруг текущей
                        if (i > 0 && area[i - 1, j] == (int)CellState.None)
                        {
                            area[i - 1, j] = wave - 1;
                            changed = true; // да, волна продолжает распространяться
                        }
                        if (i < MaxRows() - 1 && area[i + 1, j] == (int)CellState.None)
                        {
                            area[i + 1, j] = wave - 1;
                            changed = true; // да, волна продолжает распростаняться
                        }
                        if (j > 0 && area[i, j - 1] == (int)CellState.None)
                        {
                            area[i, j - 1] = wave - 1;
                            changed = true; // да, волна продолжает распространяться
                        }
                        if (j < MaxColumns() - 1 && area[i, j + 1] == (int)CellState.None)
                        {
                            area[i, j + 1] = wave - 1;
                            changed = true; // да, волна продолжает распространяться
                        }
                    }
            // обновляем маркер цифровой волны для следующей итерации
            wave--;
            return changed;
        }


        /// <summary>
        /// Перемещение enemy в заданном направлении
        /// </summary>
        /// <param name="area">ссылка на рабочее поле</param>
        /// <param name="newDirect">указатель направления</param>
        /// <param name="step">размер стороны базового квадратика</param>
        public void UpdateEnemyCell(IEnemy enemy, EnemyMoveState newDirect, int step)
        {
            // в текущую позицию enemy помещаем метку пустого поля
            area[enemy.Location.X, enemy.Location.Y] = (int)CellState.None;
            // корректируем локацию enemy
            var p = enemy.Location;
            switch (newDirect)
            {
                case EnemyMoveState.LeftUp:
                    p.Offset(-1, -1);
                    break;
                case EnemyMoveState.LeftDown:
                    p.Offset(-1, 1);
                    break;
                case EnemyMoveState.RightUp:
                    p.Offset(1, -1);
                    break;
                case EnemyMoveState.RightDown:
                    p.Offset(1, 1);
                    break;
            }
            enemy.Location = p;
            enemy.MoveState = newDirect;
            enemy.Offset = step;
            // в скорректированную текущюю позицию enemy помещаем метку enemy
            area[enemy.Location.X, enemy.Location.Y] = (int)CellState.Enemy;
        }
    }
}
