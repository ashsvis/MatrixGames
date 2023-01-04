using System;

namespace XonixModelLibrary
{
    public interface IPlayer
    {
        bool IsStopped { get; }
        Location Location { get; }
        PlayerMoveState MoveState { get; set; }
        int Lives { get; }
        void Footmarked();
        void MoveDown();
        void MoveLeft();
        void MoveRight();
        void MoveUp();
        void Offset(int dx, int dy);
        void ReturnLocation();
        void StopMove();
        void LiveLoss();
        void RestoreLivesTo(int lives);

        event EventHandler AfterAllLivesLossed;

    }
}