using Sokoban.Properties;
using System;
using System.Drawing;

namespace Sokoban
{
    public class Cell
    {
        private CellKind kind;
        private CellKind prevKind;

        public int Row { get; private set; }
        public int Column { get; private set; }

        public CellKind Kind
        {
            get { return kind; }
            set
            {
                prevKind = kind;
                kind = value;
            }
        }
        public Rectangle Rectangle { get; set; }

        private event CellStyleEventHandler cellStyle;

        public event CellStyleEventHandler CellStyle
        {
            add { cellStyle += value; }
            remove { cellStyle -= value; }
        }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Метод отрисовки одиночной ячейки
        /// </summary>
        /// <param name="graphics">где рисуем</param>
        /// <param name="offset">смещение</param>
        /// 
        public void Draw(Graphics graphics, Point offset)
        {
            var rect = new Rectangle(Rectangle.Location, Rectangle.Size);
            rect.Offset(offset);

            var style = new CellStyleEventArgs();
            DefineDefault(this.kind, style);

            cellStyle?.Invoke(this, style);
            graphics.DrawImage(style.BackImage, rect);
        }

        /// <summary>
        /// Определение стилей по умолчанию
        /// </summary>
        /// <param name="kind">тип ячейки</param>
        /// <param name="style">стиль ячейки</param>
        /// 
        private void DefineDefault(CellKind kind, CellStyleEventArgs style)
        {
            // источник картинок для ресурсов: https://sokoban.info/?1_1
            switch (kind)
            {
                case CellKind.Space:
                    style.BackImage = Resources.SokobanSpace;
                    break;
                case CellKind.Floor:
                    style.BackImage = Resources.SokobanFloor;
                    break;
                case CellKind.Wall:
                    style.BackImage = Resources.SokobanWall;
                    break;
                case CellKind.Storage:
                    style.BackImage = Resources.SokobanStorage;
                    break;
                case CellKind.Box:
                    style.BackImage = Resources.SokobanBox;
                    break;
                case CellKind.Boxed:
                    style.BackImage = Resources.SokobanBoxed;
                    break;
                case CellKind.Docker:
                    style.BackImage = Resources.SokobanDocker;
                    break;
            }
        }

        public void Restore()
        {
            kind = prevKind;
        }
    }

    public delegate void CellStyleEventHandler(object sender, CellStyleEventArgs e);

    public class CellStyleEventArgs : EventArgs
    {
        public CellStyleEventArgs() { }
        public Image BackImage { get; set; }
    }
}
