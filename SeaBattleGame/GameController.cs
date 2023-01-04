using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattleGame
{
    public static class GameController
    {
        const int Side = 10;                                    // рабочих ячеек на стороне матрицы
        private static readonly Random rand;
        private static readonly List<Point> coordsFixedHit;     // список фиксированных координат для логики обстрела компьютером
        private static readonly List<Point> coordsRandomHit;    // список случайных координат для логики обстрела компьютером

        static GameController()
        {
            rand = new Random();

            // изначально список фиксированных координат содержит 48 рабочих ячеек в определённом порядке
            // источник: http://cleanjs.ru/articles/igra-morskoj-boj-na-javascript-vystrel-kompyutera.html
            var coords = new List<Point>();
            var pt = new Point(1, 3);
            for (var i = 0; i < 8; i++)
            {
                coords.Add(pt);
                pt.Offset(1, 1);
            }
            pt = new Point(3, 1);
            for (var i = 0; i < 8; i++)
            {
                coords.Add(pt);
                pt.Offset(1, 1);
            }
            pt = new Point(7, 1);
            for (var i = 0; i < 4; i++)
            {
                coords.Add(pt);
                pt.Offset(1, 1);
            }
            pt = new Point(1, 7);
            for (var i = 0; i < 4; i++)
            {
                coords.Add(pt);
                pt.Offset(1, 1);
            }

            pt = new Point(8, 1);
            for (var i = 0; i < 8; i++)
            {
                coords.Add(pt);
                pt.Offset(-1, 1);
            }
            pt = new Point(10, 3);
            for (var i = 0; i < 8; i++)
            {
                coords.Add(pt);
                pt.Offset(-1, 1);
            }
            pt = new Point(4, 1);
            for (var i = 0; i < 4; i++)
            {
                coords.Add(pt);
                pt.Offset(-1, 1);
            }
            pt = new Point(10, 7);
            for (var i = 0; i < 4; i++)
            {
                coords.Add(pt);
                pt.Offset(-1, 1);
            }

            //// перемешиваем случайно фиксированные координаты
            //coordsFixedHit = new List<Point>();
            //while (coords.Count > 0)
            //{
            //    var index = rand.Next(coords.Count);
            //    coordsFixedHit.Add(coords[index]);
            //    coords.RemoveAt(index);
            //}

            coordsFixedHit = new List<Point>(coords);

            // изначально список случайных координат содержит все рабочие ячейки
            coordsRandomHit = new List<Point>();
            for (var j = 1; j <= Side; j++) 
                for (var i = 1; i <= Side; i++)
                    coordsRandomHit.Add(new Point(i, j));

        }

        /// <summary>
        /// Освобожнение очереди задуманных ударов от уже совершённых или уже ненужных ударов
        /// </summary>
        /// <param name="point"></param>
        public static void UpdateFutureHits(Point point, ActionKind kind = ActionKind.Remove)
        {
            switch (kind)
            {
                case ActionKind.Insert:
                    coordsFixedHit.RemoveAll(item => item == point);
                    coordsFixedHit.Insert(0, point);
                    break;
                default:
                    coordsFixedHit.RemoveAll(item => item == point);
                    coordsRandomHit.RemoveAll(item => item == point);
                    break;
            }
        }

        /// <summary>
        /// Возвращает координату следующего удара
        /// </summary>
        /// <returns></returns>
        public static Point NextHit()
        {
            var hitPoint = Point.Empty;
            if (coordsFixedHit.Count > 0)
            {
                hitPoint = coordsFixedHit[0];
                coordsFixedHit.RemoveAt(0);
            }
            else if (coordsRandomHit.Count > 0)
            {
                var index = rand.Next(coordsRandomHit.Count);
                hitPoint = coordsRandomHit[index];
                coordsRandomHit.RemoveAt(index);
            }
            return hitPoint;
        }
    }

    public enum ActionKind
    {
        Remove,
        Insert
    }
}
