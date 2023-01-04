namespace XonixModelLibrary
{
    /// <summary>
    /// Статусы движения "игрока"
    /// </summary>
    public enum PlayerMoveState
    {
        None = 0,   // игрок не движется
        Down = 1,   // движется вниз
        Left = 2,   // влево
        Right = 3,  // вправо
        Up = 4,     // вверх
    }
}
