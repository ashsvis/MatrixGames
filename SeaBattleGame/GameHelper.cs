using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SeaBattleGame
{
    public static class GameHelper
    {
        private static readonly Random rand;
        private static readonly string[,] matrixPlayer;
        private static readonly string[,] matrixEnemy;

        private static readonly List<Ship> shipsPlayer = new List<Ship>();
        private static readonly List<Ship> shipsEnemy = new List<Ship>();

        public const int Side = 12; // бордюрная ячейка + 10 рабочих ячеек на стороне матрицы + бордюрная ячейка

        /// <summary>
        /// Маркер заведомо пустой ячейки, выстрел в которую ненужен.
        /// </summary>
        public const string ToggleMarker = "#";
        /// <summary>
        /// Маркер выстрела по ячейке, нет попадания, мимо.
        /// </summary>
        public const string ShotMarker = ".";
        /// <summary>
        /// Маркер попадания в палубу, попал.
        /// </summary>
        public const string HitMarker = "+";
        /// <summary>
        /// Маркер после затопления всего корабля.
        /// </summary>
        public const string SankMarker = "*";
        /// <summary>
        /// Маркер после выигыша для оставшихся кораблей.
        /// </summary>
        public const string LiveMarker = "!";

        // маркировка палуб соответственно размерам кораблей
        public const string Desk1Marker = "1";
        public const string Desk2Marker = "2";
        public const string Desk3Marker = "3";
        public const string Desk4Marker = "4";

        public const string RowHeaders = "АБВГДЕЖЗИК";

        static GameHelper()
        {
            rand = new Random();

            //создаем матрицу игрока
            matrixPlayer = new string[Side, Side];
            //создаем матрицу противника
            matrixEnemy = new string[Side, Side];
        }

        public static Size GridSize => new Size(matrixPlayer.GetLength(0), matrixPlayer.GetLength(1));

        public static string[,] MatrixPlayer => matrixPlayer;
        public static string[,] MatrixEnemy => matrixEnemy;

        public static List<Ship> ShipsPlayer => shipsPlayer;
        public static List<Ship> ShipsEnemy => shipsEnemy;

        /// <summary>
        /// Очистка строковой матрицы
        /// </summary>
        /// <param name="matrix"></param>
        public static void EmptyMatrix(string[,] matrix)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    matrix[i, j] = string.Empty;
                }
        }

        public static string SayShot(Point cell)
        {
            return $"{RowHeaders[cell.Y - 1]}{cell.X}";
        }

        public static bool IsUnknown(string[,] matrix, Point cell)
        {
            var value = matrix[cell.X, cell.Y];
            if (string.IsNullOrEmpty(value))
                return true;
            if (value == Desk1Marker || value == Desk2Marker || value == Desk3Marker || value == Desk4Marker)
                return true;
            return false;
        }

        /// <summary>
        /// Отметка об ударе или разрушении палубы
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="ships"></param>
        /// <param name="cell"></param>
        /// <param name="updateHits"></param>
        /// <returns>True - палуба разрушена, False - мимо</returns>
        public static bool SetShotOrHit(string[,] matrix, List<Ship> ships, Point cell, Action<Point, ActionKind> updateHits = null)
        {
            var value = matrix[cell.X, cell.Y];
            // если ячейка была пуста, то ставим отметку об ударе
            if (string.IsNullOrEmpty(value))
            {
                matrix[cell.X, cell.Y] = ShotMarker;
                updateHits?.Invoke(new Point(cell.X, cell.Y), ActionKind.Remove);
            }
            else
            // если ячейка содержала палубу корабля, то ставим отметку о разрушении
            if (value.StartsWith(Desk1Marker) || value.StartsWith(Desk2Marker) || value.StartsWith(Desk3Marker) || value.StartsWith(Desk4Marker))
            {
                matrix[cell.X, cell.Y] += HitMarker;
                MarkCorners(matrix, cell, updateHits);
                // поиск корабля по координтам палубы
                var ship = ships.FirstOrDefault(item => item.Coordinates.Contains(cell));
                if (ship != null)
                {
                    ship.Hits++;
                    // определение ориентации корабля по двум ударам
                    if (ship.Hits == 2)
                    {
                        if (matrix[cell.X - 1, cell.Y].EndsWith(HitMarker) || matrix[cell.X + 1, cell.Y].EndsWith(HitMarker))
                            ship.Orientation = Orientation.Horizontal;
                        else if (matrix[cell.X, cell.Y - 1].EndsWith(HitMarker) || matrix[cell.X, cell.Y + 1].EndsWith(HitMarker))
                            ship.Orientation = Orientation.Vertical;
                    }
                    var markers = CheckShipDestroyed(matrix, ships, ship);
                    if (markers.Count > 0)
                        markers.ForEach(point => updateHits?.Invoke(point, ActionKind.Remove));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Обстрел вокруг обнаруженной палубы с целью потопить весь корабль
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="cell"></param>
        /// <param name="updateHits"></param>
        public static void UpdateShotsAroundDesk(string[,] matrix, Point cell, Action<Point, ActionKind> updateHits)
        {
            var coords = new List<Point>();
            if (cell.X > 1 && IsUnknown(matrix, new Point(cell.X - 1, cell.Y)))
                coords.Add(new Point(cell.X - 1, cell.Y));
            if (cell.X < Side - 2 && IsUnknown(matrix, new Point(cell.X + 1, cell.Y)))
                coords.Add(new Point(cell.X + 1, cell.Y));
            if (cell.Y > 1 && IsUnknown(matrix, new Point(cell.X, cell.Y - 1)))
                coords.Add(new Point(cell.X, cell.Y - 1));
            if (cell.Y < Side - 2 && IsUnknown(matrix, new Point(cell.X, cell.Y + 1)))
                coords.Add(new Point(cell.X, cell.Y + 1));
            while (coords.Count > 0)
            {
                var index = rand.Next(coords.Count);
                updateHits(coords[index], ActionKind.Insert);
                coords.RemoveAt(index);
            }

        }

        /// <summary>
        /// Маркировка выживших кораблей для предъявления
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="ships"></param>
        public static void ShowLivedShips(string[,] matrix, List<Ship> ships)
        {
            foreach (var ship in ships)
                foreach (var cell in ship.Coordinates)
                {
                    if (matrix[cell.X, cell.Y].EndsWith(HitMarker))
                        matrix[cell.X, cell.Y] = matrix[cell.X, cell.Y].Replace(HitMarker, SankMarker);
                    else
                        matrix[cell.X, cell.Y] += LiveMarker;
                }
        }

        /// <summary>
        /// Проверка, что при очередном обстреле корабль однозначно потоплен
        /// </summary>
        /// <param name="matrix">Игровое поле</param>
        /// <param name="ships">Список кораблей</param>
        /// <param name="ship">Корабль с повреждениями</param>
        private static List<Point> CheckShipDestroyed(string[,] matrix, List<Ship> ships, Ship ship)
        {
            var markers = new List<Point>();
            if (ship.Desks == ship.Hits)
            {
                // замена маркера подбитой палубы на маркер затопленной палубы
                foreach (var cell in ship.Coordinates)
                    matrix[cell.X, cell.Y] = matrix[cell.X, cell.Y].Replace(HitMarker, SankMarker);
                // отметка свободных полей слева и справа от корабля
                if (ship.Orientation == Orientation.Horizontal || ship.Orientation == Orientation.Unknown)
                {
                    var cell = ship.Coordinates.First();
                    if (cell.X > 1 && matrix[cell.X - 1, cell.Y] == "")
                    {
                        matrix[cell.X - 1, cell.Y] = ToggleMarker;
                        markers.Add(new Point(cell.X - 1, cell.Y));
                    }
                    cell = ship.Coordinates.Last();
                    if (cell.X < Side - 2 && matrix[cell.X + 1, cell.Y] == "")
                    {
                        matrix[cell.X + 1, cell.Y] = ToggleMarker;
                        markers.Add(new Point(cell.X + 1, cell.Y));
                    }
                }
                if (ship.Orientation == Orientation.Vertical || ship.Orientation == Orientation.Unknown)
                {
                    var cell = ship.Coordinates.First();
                    if (cell.Y > 1 && matrix[cell.X, cell.Y - 1] == "")
                    {
                        matrix[cell.X, cell.Y - 1] = ToggleMarker;
                        markers.Add(new Point(cell.X, cell.Y - 1));
                    }
                    cell = ship.Coordinates.Last();
                    if (cell.Y < Side - 2 && matrix[cell.X, cell.Y + 1] == "")
                    {
                        matrix[cell.X, cell.Y + 1] = ToggleMarker;
                        markers.Add(new Point(cell.X, cell.Y + 1));
                    }
                }
                // удаление затопленных кораблей из списка
                ships.RemoveAll(item => string.Join(";", item.Coordinates) == string.Join(";", ship.Coordinates));
            }
            return markers;
        }

        /// <summary>
        /// Маркировка соседних немаркированных угловых ячеек при подбитии палубы
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="cell"></param>
        private static void MarkCorners(string[,] matrix, Point cell, Action<Point, ActionKind> updateHits)
        {
            var sideX = matrix.GetLength(1);
            var sideY = matrix.GetLength(0);
            if (cell.X - 1 > 0 && cell.Y - 1 > 0 && HasNoMakers(matrix[cell.X - 1, cell.Y - 1]))
            {
                matrix[cell.X - 1, cell.Y - 1] += ToggleMarker;
                updateHits?.Invoke(new Point(cell.X - 1, cell.Y - 1), ActionKind.Remove);
            }
            if (cell.X + 1 < sideX - 1 && cell.Y - 1 > 0 && HasNoMakers(matrix[cell.X + 1, cell.Y - 1]))
            {
                matrix[cell.X + 1, cell.Y - 1] += ToggleMarker;
                updateHits?.Invoke(new Point(cell.X + 1, cell.Y - 1), ActionKind.Remove);
            }
            if (cell.X - 1 > 0 && cell.Y + 1 < sideY - 1 && HasNoMakers(matrix[cell.X - 1, cell.Y + 1]))
            {
                matrix[cell.X - 1, cell.Y + 1] += ToggleMarker;
                updateHits?.Invoke(new Point(cell.X - 1, cell.Y + 1), ActionKind.Remove);
            }
            if (cell.X + 1 < sideX - 1 && cell.Y + 1 < sideY - 1 && HasNoMakers(matrix[cell.X + 1, cell.Y + 1]))
            {
                matrix[cell.X + 1, cell.Y + 1] += ToggleMarker;
                updateHits?.Invoke(new Point(cell.X + 1, cell.Y + 1), ActionKind.Remove);
            }
        }

        private static bool HasNoMakers(string value)
        {
            return !value.EndsWith(ToggleMarker) && !value.EndsWith(HitMarker) && value != ShotMarker;
        }


        #region Автоматическая расстановка кораблей.


        /// Источник: https://www.cyberforum.ru/cpp-beginners/thread246354.html#post1389806
        /// Адаптировано для C#

        private static bool ShipIsGood(int size, bool is_horiz, int row_top, int col_left, string[,] matrix)
        {
            if (is_horiz)
            {
                for (int i = Math.Max(0, row_top - 1); i <= Math.Min(10 - 1, row_top + 1); ++i)
                {
                    for (int j = Math.Max(0, col_left - 1); j <= Math.Min(10 - 1, col_left + size); ++j)
                    {
                        if (matrix[i + 1, j + 1] != "") return false;
                    }
                }
                return true;
            }
            else //вертикальный
            {
                for (int i = Math.Max(0, row_top - 1); i <= Math.Min(10 - 1, row_top + size); ++i)
                {
                    for (int j = Math.Max(0, col_left - 1); j <= Math.Min(10 - 1, col_left + 1); ++j)
                    {
                        if (matrix[i + 1, j + 1] != "") return false;
                    }
                }
                return true;
            }
        }

        private static Ship SetShipWithSize(int size, string[,] matrix)
        {
            bool is_horiz = rand.Next(2) == 0;
            int row_top;
            int col_left;

            do
            {
                do
                {
                    row_top = rand.Next(10);
                } while (!is_horiz
                && row_top > 10 - size);

                do
                {
                    col_left = rand.Next(10);
                } while (is_horiz
                       && col_left > 10 - size);
            } while (!ShipIsGood(size, is_horiz, row_top, col_left, matrix));

            var ship = new Ship() { Desks = size, Hits = 0, Orientation = Orientation.Unknown };
            var coordinates = new List<Point>();

            if (is_horiz)
            {
                for (int j = col_left; j < col_left + size; ++j)
                {
                    matrix[row_top + 1, j + 1] = size.ToString();
                    coordinates.Add(new Point(row_top + 1, j + 1));
                }
            }
            else //вертикальный
            {
                for (int i = row_top; i < row_top + size; ++i)
                {
                    matrix[i + 1, col_left + 1] = size.ToString();
                    coordinates.Add(new Point(i + 1, col_left + 1));
                }
            }

            ship.Coordinates = coordinates.ToArray();

            return ship;
        }

        public static List<Ship> SetShips(string[,] matrix)
        {
            var ships = new List<Ship>();
            for (int i = 0; i < 1; ++i)
                ships.Add(SetShipWithSize(4, matrix));
            for (int i = 0; i < 2; ++i)
                ships.Add(SetShipWithSize(3, matrix));
            for (int i = 0; i < 3; ++i)
                ships.Add(SetShipWithSize(2, matrix));
            for (int i = 0; i < 4; ++i)
                ships.Add(SetShipWithSize(1, matrix));
            return ships;
        }

        #endregion Автоматическая расстановка кораблей.


        /// <summary>
        /// Расстановка кораблей автоматическая
        /// </summary>
        public static void InitPlaceShips()
        {
            EmptyMatrix(matrixPlayer);
            shipsPlayer.Clear();
            shipsPlayer.AddRange(SetShips(matrixPlayer));
            EmptyMatrix(matrixEnemy);
            shipsEnemy.Clear();
            shipsEnemy.AddRange(SetShips(matrixEnemy));
        }
    }
}
