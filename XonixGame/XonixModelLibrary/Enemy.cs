using System;
using System.Collections.Generic;

namespace XonixModelLibrary
{
    public class Enemy : IEnemy
    {
        public Location Location { get; set; }
        public EnemyMoveState MoveState { get; set; }
        public int Offset { get; set; }

        /// <summary>
        /// Подготовка (проверка возможности) к перемещению
        /// </summary>
        /// <param name="area">рабочая область</param>
        /// <param name="operations">накопительный список</param>
        public void PrepareEnemies(Area area, List<NextOperation> operations)
        {
            // на основании текщего направления перемещения
            switch (MoveState)
            {
                case EnemyMoveState.LeftUp:
                    // проверка возможности продолжения движения влево-вверх
                    if (LeftUpMovePossible(area, Location))
                        PrepareEnemyCell(operations, EnemyMoveState.LeftUp);
                    else
                    // проверка возможности отскока во всех других направлениях
                        if (LeftDownMovePossible(area, Location) && RightUpMovePossible(area, Location) && RightDownMovePossible(area, Location))
                            PrepareEnemyCell(operations, EnemyMoveState.RightDown);
                        else if (LeftDownMovePossible(area, Location))
                            PrepareEnemyCell(operations, EnemyMoveState.LeftDown); // проверка отскока влево-вниз
                        else if (RightUpMovePossible(area, Location))
                            PrepareEnemyCell(operations, EnemyMoveState.RightUp); // проверка отскока вправо-вверх
                        else
                            PrepareEnemyCell(operations, EnemyMoveState.RightDown); // отскок назад
                    break;
                case EnemyMoveState.LeftDown:
                    // проверка возможности продолжения движения влево-вниз
                    if (LeftDownMovePossible(area, Location))
                        PrepareEnemyCell(operations, EnemyMoveState.LeftDown);
                    else
                        // проверка возможности отскока во всех других направлениях
                        if (RightDownMovePossible(area, Location) && LeftUpMovePossible(area, Location) && RightUpMovePossible(area, Location))
                            PrepareEnemyCell(operations, EnemyMoveState.RightUp);
                        else if (RightDownMovePossible(area, Location)) // проверка отскока вправо-вниз
                            PrepareEnemyCell(operations, EnemyMoveState.RightDown);
                        else if (LeftUpMovePossible(area, Location)) // проверка отскока влево-вверх
                            PrepareEnemyCell(operations, EnemyMoveState.LeftUp);
                        else
                            PrepareEnemyCell(operations, EnemyMoveState.RightUp); // отскок назад
                    break;
                case EnemyMoveState.RightUp:
                    // проверка возможности продолжения движения вправо-вверх
                    if (RightUpMovePossible(area, Location))
                        PrepareEnemyCell(operations, EnemyMoveState.RightUp);
                    else
                    // проверка возможности отскока во всех других направлениях
                        if (LeftUpMovePossible(area, Location) && RightDownMovePossible(area, Location) && LeftDownMovePossible(area, Location))
                            PrepareEnemyCell(operations, EnemyMoveState.LeftDown);
                        else if (LeftUpMovePossible(area, Location)) // проверка отскока влево-вверх
                            PrepareEnemyCell(operations, EnemyMoveState.LeftUp);
                        else if (RightDownMovePossible(area, Location)) // проверка отскока вправо-вниз
                            PrepareEnemyCell(operations, EnemyMoveState.RightDown);
                        else
                            PrepareEnemyCell(operations, EnemyMoveState.LeftDown); // отскок назад
                    break;
                case EnemyMoveState.RightDown:
                    // проверка возможности продолжения движения вправо-вниз
                    if (RightDownMovePossible(area, Location))
                        PrepareEnemyCell(operations, EnemyMoveState.RightDown);
                    else
                    // проверка возможности отскока во всех других направлениях
                        if (LeftDownMovePossible(area, Location) && RightUpMovePossible(area, Location) && LeftUpMovePossible(area, Location))
                            PrepareEnemyCell(operations, EnemyMoveState.LeftUp);
                        else if (LeftDownMovePossible(area, Location)) // проверка отскока влево-вниз
                            PrepareEnemyCell(operations, EnemyMoveState.LeftDown);
                        else if (RightUpMovePossible(area, Location)) // проверка отскока вправо-вверх
                            PrepareEnemyCell(operations, EnemyMoveState.RightUp);
                        else
                            PrepareEnemyCell(operations, EnemyMoveState.LeftUp); // отскок назад
                    break;
            }
        }

        /// <summary>
        /// Подготовка перемещения enemy в заданном направлении
        /// </summary>
        /// <param name="enemy">ссылка на объект enemy</param>
        /// <param name="direct">указатель направления</param>
        private void PrepareEnemyCell(List<NextOperation> operations, EnemyMoveState direct)
        {
            operations.Add(new NextOperation() { Enemy = this, Direct = direct });
        }

        private bool RightDownMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(1, 1);
            // проверка возможности движения вправо-вниз
            var possible = test.X < area.MaxRows() && test.Y < area.MaxColumns() && IsEmptyCell(area, test);
            return possible && RightMovePossible(area, p) && DownMovePossible(area, p);
        }

        private bool LeftDownMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(-1, 1);
            // проверка возможности движения влево-вниз
            var possible = test.X > 0 && test.Y < area.MaxColumns() && IsEmptyCell(area, test);
            return possible && LeftMovePossible(area, p) && DownMovePossible(area, p);
        }

        private bool RightUpMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(1, -1);
            // проверка возможности движения вправо-вверх
            var possible = test.X < area.MaxRows() && test.Y > 0 && IsEmptyCell(area, test);
            return possible && RightMovePossible(area, p) && UpMovePossible(area, p);
        }

        private bool LeftUpMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(-1, -1);
            // проверка возможности движения влево-вверх
            var possible = test.X > 0 && test.Y > 0 && IsEmptyCell(area, test);
            return possible && LeftMovePossible(area, p) && UpMovePossible(area, p);
        }

        private bool LeftMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(-1, 0);
            // проверка возможности движения влево
            var possible = test.X > 0 && IsEmptyCell(area, test);
            return possible;
        }

        private bool UpMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(0, -1);
            // проверка возможности движения вверх
            var possible = test.Y > 0 && IsEmptyCell(area, test);
            return possible;
        }

        private bool RightMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(1, 0);
            // проверка возможности движения вправо
            var possible = test.X < area.MaxRows() && test.Y > 0 && IsEmptyCell(area, test);
            return possible;
        }

        private bool DownMovePossible(Area area, Location p)
        {
            var test = p;
            test.Offset(0, 1);
            // проверка возможности движения вниз
            var possible = test.Y < area.MaxColumns() && IsEmptyCell(area, test);
            return possible;
        }

        public event EventHandler FootmarkFound;

        private bool IsEmptyCell(Area area, Location p)
        {
            if (area[p.X, p.Y] == (int)CellState.Footmark)
                FootmarkFound?.Invoke(this, new EventArgs());
            return area[p.X, p.Y] == (int)CellState.None;
        }

        public void DecrementEnemiesOffset(int decrement)
        {
            if (Offset > 0)
            {
                // уменьшить величину смещения
                Offset -= decrement;
                if (Offset < 0) Offset = 0;
            }
        }

        public Location CalculateOffet()
        {
            var p = new Location();
            var o = Offset;
            switch (MoveState)
            {
                case EnemyMoveState.LeftUp:
                    p.Offset(o, o);
                    break;
                case EnemyMoveState.LeftDown:
                    p.Offset(o, -o);
                    break;
                case EnemyMoveState.RightUp:
                    p.Offset(-o, o);
                    break;
                case EnemyMoveState.RightDown:
                    p.Offset(-o, -o);
                    break;
                default:
                    break;
            }
            return p;
        }
    }
}
