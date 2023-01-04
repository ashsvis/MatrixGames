using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Level
    {
        private int level = 0;
        private int count = 0;
        private Cell[] goalCells;

        public Cell[,] Cells { get; set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Level()
        {
            Generate(level);
        }

        public Level(int level)
        {
            this.level = level;
            Generate(level);
        }

        public static Size CalculateMaxSize()
        {
            var levels = new List<string>();
            levels.AddRange(Properties.Resources.levels.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            var size = new Size();
            for (var i = 0; i < levels.Count; i++)
            {
                var level = levels[i].Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var columns = level.Max(item => item.Length);
                var rows = level.Length;
                if (size.Width < columns) size.Width = columns;
                if (size.Height < rows) size.Height = rows;
            }
            return size;
        }

        /// <summary>
        /// Генерарация уровня по номеру индекса уровня
        /// </summary>
        /// <param name="levelIndex">индекс уровня, от 0 до 59</param>
        private void Generate(int levelIndex)
        {
            var levels = new List<string>();
            // загрузка уровней, каждый уровень в одной строке
            levels.AddRange(Properties.Resources.levels.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            count = levels.Count;
            if (levelIndex >= count) return;
            // строки в уровне отделены табуляцией
            var level = levels[levelIndex].Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            Columns = level.Max(item => item.Length);
            Rows = level.Length;
            var goals = new List<Cell>();
            // матрица ячеек
            Cells = new Cell[Rows, Columns];
            var rect = new Rectangle(0, 0, 36, 36);
            Width = rect.Width * Columns;
            Height = rect.Height * Rows;
            var r = 0;
            foreach (var row in level)
            {
                var c = 0;
                foreach (var ch in row.ToCharArray())
                {
                    var cell = new Cell(r, c);
                    cell.CellStyle += (o, e) => cellStyle?.Invoke(o, e);
                    Cells[r, c] = cell;
                    cell.Rectangle = new Rectangle(rect.Location, rect.Size);
                    switch (ch)
                    {
                        case 'X':
                            cell.Kind = CellKind.Wall;
                            break;
                        case '.':
                            cell.Kind = CellKind.Storage;
                            goals.Add(cell);
                            break;
                        case '*':
                            cell.Kind = CellKind.Box;
                            break;
                        case '=':
                            cell.Kind = CellKind.Boxed;
                            break;
                        case '@':
                            cell.Kind = CellKind.Docker;
                            break;
                        case ' ':
                            cell.Kind = CellKind.Floor;
                            break;
                        case '~':
                            cell.Kind = CellKind.Space;
                            break;
                    }
                    rect.Offset(rect.Width, 0);
                    c++;
                }
                rect.Offset(0, rect.Height);
                rect.X = 0;
                r++;
            }
            #region заполнение краёв уровня кодом внешнего фона
            for (var i = 0; i < Rows; i++)
            {
                var j = 0;
                while (Cells[i, j].Kind == CellKind.Floor || Cells[i, j].Kind == CellKind.Space)
                {
                    Cells[i, j].Kind = CellKind.Space;
                    j++;
                }
            }
            for (var i = 0; i < Rows; i++)
            {
                var j = Columns - 1;
                while (Cells[i, j].Kind == CellKind.Floor || Cells[i, j].Kind == CellKind.Space)
                {
                    Cells[i, j].Kind = CellKind.Space;
                    j--;
                }
            }
            for (var j = 0; j < Columns; j++)
            {
                var i = 0;
                while (Cells[i, j].Kind == CellKind.Floor || Cells[i, j].Kind == CellKind.Space)
                {
                    Cells[i, j].Kind = CellKind.Space;
                    i++;
                }
            }
            for (var j = 0; j < Columns; j++)
            {
                var i = Rows - 1;
                while (Cells[i, j].Kind == CellKind.Floor || Cells[i, j].Kind == CellKind.Space)
                {
                    Cells[i, j].Kind = CellKind.Space;
                    i--;
                }
            }
            #endregion
            goalCells = goals.ToArray();
        }

        private event CellStyleEventHandler cellStyle;

        public event CellStyleEventHandler CellStyle
        {
            add { cellStyle += value; }
            remove { cellStyle -= value; }
        }

        /// <summary>
        /// Передвижение грузчика из ячейки в ячейку
        /// </summary>
        /// <param name="fromCell">из ячейки</param>
        /// <param name="toCell">в ячейку</param>
        private async void GoFromTo(Cell fromCell, Cell toCell)
        {
            switch (toCell.Kind)
            {
                case CellKind.Floor:
                case CellKind.Storage:
                    // если целевая ячека пустая, то перемещаем туда грузчика
                    fromCell.Restore();
                    toCell.Kind = CellKind.Docker;
                    break;
                case CellKind.Box:
                case CellKind.Boxed:
                    // вычисляем, в какую сторону двигался грузчик
                    var rowOffset = toCell.Row - fromCell.Row;
                    var colOffset = toCell.Column - fromCell.Column;
                    // пробуем смещать груз
                    if (ShiftFromTo(toCell, Cells[toCell.Row + rowOffset, toCell.Column + colOffset]))
                    {
                        // если сместить груз удалось, то перемещаем на это место грузчика
                        fromCell.Restore();
                        toCell.Kind = CellKind.Docker;
                        if (CheckAllBoxPlaced())
                        {
                            await Task.Delay(100);
                            levelComplete?.Invoke(this, new EventArgs());
                        }
                    }
                    break;
            }
        }

        private event EventHandler levelComplete;

        public event EventHandler LevelComplete
        {
            add { levelComplete += value; }
            remove { levelComplete -= value; }
        }

        /// <summary>
        /// Проверка окончания уровня (все ящики на месте)
        /// </summary>
        /// <returns></returns>
        public bool CheckAllBoxPlaced()
        {
            return goalCells.All(cell => cell.Kind == CellKind.Boxed);
        }

        /// <summary>
        /// Смещение одиночного груза (за которым есть свободное пространство)
        /// </summary>
        /// <param name="fromCell">из ячейки</param>
        /// <param name="toCell">в ячейку</param>
        /// <returns>Возвращаем истину, если сместить груз удалось</returns>
        private bool ShiftFromTo(Cell fromCell, Cell toCell)
        {
            if (toCell.Kind == CellKind.Floor || toCell.Kind == CellKind.Storage)
            {
                fromCell.Restore();
                toCell.Kind = goalCells.Contains(toCell) ? CellKind.Boxed : CellKind.Box;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Перемещение грузчика вправо
        /// </summary>
        public void GoRight()
        {
            var cell = GetLoaderCell();
            GoFromTo(cell, Cells[cell.Row, cell.Column + 1]);
        }

        /// <summary>
        /// Перемещение грузчика влево
        /// </summary>
        public void GoLeft()
        {
            var cell = GetLoaderCell();
            GoFromTo(cell, Cells[cell.Row, cell.Column - 1]);
        }

        /// <summary>
        /// Перемещение грузчика вниз
        /// </summary>
        public void GoDown()
        {
            var cell = GetLoaderCell();
            GoFromTo(cell, Cells[cell.Row + 1, cell.Column]);
        }

        /// <summary>
        /// Перемещение грузчика вверх
        /// </summary>
        public void GoUp()
        {
            var cell = GetLoaderCell();
            GoFromTo(cell, Cells[cell.Row - 1, cell.Column]);
        }

        /// <summary>
        /// Получение ссылки на ячейку с грузчиком
        /// </summary>
        /// <returns></returns>
        private Cell GetLoaderCell()
        {
            for (var row = 0; row < Cells.GetLength(0); row++)
            {
                for (var col = 0; col < Cells.GetLength(1); col++)
                {
                    var cell = Cells[row, col];
                    if (cell.Kind == CellKind.Docker)
                        return cell;
                }
            }
            return null;
        }

        public int LevelsCount => count;

        public int CurrentLevel => level;

        /// <summary>
        /// Перемещение на начало предыдущего уровеня
        /// </summary>
        /// <returns></returns>
        public bool Prev()
        {
            if (level > 0)
                Generate(--level);
            return level > 0;
        }

        /// <summary>
        /// Перемещение на начало следующего уровня
        /// </summary>
        /// <returns></returns>
        public bool Next()
        {
            if (level < count - 1)
                Generate(++level);
            return level < count - 1;
        }

        /// <summary>
        /// Сброс текущего уровня к началу
        /// </summary>
        public void Reset()
        {
            Generate(level);
        }

        /// <summary>
        /// Рисование уровня
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="offset">смещение</param>
        public void Draw(Graphics graphics, Point offset)
        {
            for (var row = 0; row < Cells.GetLength(0); row++)
            {
                for (var col = 0; col < Cells.GetLength(1); col++)
                {
                    var cell = Cells[row, col];
                    cell.Draw(graphics, offset);
                }
            }
        }

        /// <summary>
        /// Получение ссылки на ячейку по координатам мыши с учётом центрирования рисунка уровня на форме
        /// </summary>
        /// <param name="point">Координаты указателя</param>
        /// <param name="offset">Смещение левого верхнего угла рисунка уровня</param>
        /// <returns></returns>
        public Cell CellAt(Point point, Point offset)
        {
            for (var row = 0; row < Cells.GetLength(0); row++)
            {
                for (var col = 0; col < Cells.GetLength(1); col++)
                {
                    var cell = Cells[row, col];
                    var rect = new Rectangle(cell.Rectangle.Location, cell.Rectangle.Size);
                    rect.Offset(offset);
                    if (rect.Contains(point))
                        return cell;
                }
            }
            return null;
        }
    }
}
