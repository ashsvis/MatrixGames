namespace Sokoban
{
    public enum CellKind
    {
        Floor,   // пуста ячейка
        Storage, // пустая ячейка с местом под ящик
        Box,     // ящик
        Boxed,   // ящик на месте
        Wall,    // стена
        Docker,  // грузчик
        Space,   // пустая ячейка вне игрового поля
    }
}
