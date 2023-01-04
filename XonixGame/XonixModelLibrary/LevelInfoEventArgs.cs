using System;

namespace XonixModelLibrary
{
    public class LevelInfoEventArgs : EventArgs
    {
        public int Level { get; set; }
        public int Lives { get; set; }

    }

    public delegate void LevelInfoEventHandler(object sender, LevelInfoEventArgs args);
}
