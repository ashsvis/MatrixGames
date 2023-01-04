using System.Collections.Generic;

namespace XonixModelLibrary
{
    public interface IEnemy
    {
        Location Location { get; set; }
        EnemyMoveState MoveState { get; set; }
        int Offset { get; set; }

        Location CalculateOffet();
        void DecrementEnemiesOffset(int decrement);
        void PrepareEnemies(Area area, List<NextOperation> operations);
    }
}