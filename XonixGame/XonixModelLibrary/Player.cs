using System;

namespace XonixModelLibrary
{
    public class Player : IPlayer
    {
        private PlayerMoveState moveState;
        private Location lastLocation;
        private Location location;
        private bool footmarked;

        public event EventHandler AfterAllLivesLossed;

        public Player(int lives, int x, int y)
        {
            location.X = x;
            location.Y = y;
            lastLocation = location;
            footmarked = false;
            // изначальный счётчик жизней
            Lives = lives;
        }

        /// <summary>
        /// текущая позиция "игрока"
        /// </summary>
        public Location Location { get => location; }

        /// <summary>
        /// режим перемещения "игрока"
        /// </summary>
        public PlayerMoveState MoveState { get => moveState; set => moveState = value; }

        public int Lives { get; private set; }

        public void MoveUp()
        {
            if (moveState != PlayerMoveState.Down)
                moveState = PlayerMoveState.Up;
        }

        public void MoveDown()
        {
            if (moveState != PlayerMoveState.Up)
                moveState = PlayerMoveState.Down;
        }

        public void MoveLeft()
        {
            if (moveState != PlayerMoveState.Right)
                moveState = PlayerMoveState.Left;
        }

        public void MoveRight()
        {
            if (moveState != PlayerMoveState.Left)
                moveState = PlayerMoveState.Right;
        }

        /// <summary>
        /// Признак, что игрок остановлен (или не двигается автоматически)
        /// </summary>
        public bool IsStopped => MoveState == PlayerMoveState.None;

        public void StopMove()
        {
            // сбрасываем признак движения player
            MoveState = PlayerMoveState.None;
            // стираем очередь "следов" игрока
            //footmarkQueue.Clear();
            footmarked = false;
        }

        /// <summary>
        /// Смещение позиции игрока
        /// </summary>
        /// <param name="dx">по горизонтали</param>
        /// <param name="dy">по вретикали</param>
        public void Offset(int dx, int dy)
        {
            // запоминаем, где были, только если двигались только по "суше"
            if (!footmarked)
                lastLocation = location;
            // сдвигаем локацию
            location.Offset(dx, dy);
        }

        /// <summary>
        /// Возвращаем позицию при неудаче
        /// </summary>
        public void ReturnLocation()
        {
            location = lastLocation;
        }

        /// <summary>
        /// Начинаем оставлять "след"
        /// </summary>
        public void Footmarked()
        {
            footmarked = true;
        }


        /// <summary>
        /// Игровая жизнь потеряна
        /// </summary>
        public void LiveLoss()
        {
            Lives--;
            if (Lives == 0)
                AfterAllLivesLossed?.Invoke(this, new EventArgs());
        }

        public void RestoreLivesTo(int lives)
        {
            Lives = lives;
        }
    }
}
