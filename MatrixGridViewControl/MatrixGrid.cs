/// (C) Storm23, 2015
/// Источник: https://www.cyberforum.ru/blogs/529033/blog3296.html
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MatrixGridViewControl
{
    /// <summary>
    /// Кликабельный контрол для отображения матриц
    /// </summary>
    public class MatrixGrid : UserControl
    {
        public Size GridSize { get; set; }
        public Point HoveredCell = new Point(-1, -1);

        public event EventHandler<CellNeededEventArgs> CellNeeded;
        public event EventHandler<CellClickEventArgs> CellClick;
        // добавлено 30.12.2022
        public event EventHandler<CellClickEventArgs> CellContext;

        public MatrixGrid()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;

            if (CellNeeded == null)
                return;

            var cw = ClientSize.Width / GridSize.Width;
            var ch = ClientSize.Height / GridSize.Height;

            for (int j = 0; j < GridSize.Height; j++)
                for (int i = 0; i < GridSize.Width; i++)
                {
                    var cell = new Point(i, j);

                    //получаем значение ячейки от пользователя
                    var ea = new CellNeededEventArgs(cell);
                    CellNeeded(this, ea);

                    //рисуем ячейку
                    var rect = new Rectangle(cw * i, ch * j, cw, ch);

                    // добавлено 29.12.2022
                    if (ea.Background != null)
                    {
                        gr.DrawImage(ea.Background, new Rectangle(rect.Location, ea.Background.Size));
                        if (ea.Action != null)
                        {
                            gr.DrawImage(ea.Action, new Rectangle(Point.Add(rect.Location, new Size(3, 3)), ea.Action.Size));
                        }
                    }
                    else
                    {
                        //фон
                        if (ea.BackColor != Color.Transparent)
                            using (var brush = new SolidBrush(ea.BackColor))
                                gr.FillRectangle(brush, rect);
                    }

                    rect.Inflate(-1, -1);

                    if (cell == HoveredCell &&
                        !ea.IsBorder)                       // добавлено 28.12.2022
                        gr.DrawRectangle(Pens.Red, rect);

                    //текст
                    if (!string.IsNullOrEmpty(ea.Value) &&
                        ea.IsBorder)                                    // добавлено 29.12.2022
                        using (var brush = new SolidBrush(ForeColor))   // добавлено 30.12.2022
                        {
                            gr.DrawString(ea.Value, Font, brush, rect, 
                                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        }
                }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var cell = PointToCell(e.Location);
            HoveredCell = cell;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                var cell = PointToCell(e.Location);
                OnCellClick(new CellClickEventArgs(cell));
                HoveredCell = cell;
            }
            else // Добавлено 30.12.2022
            if (e.Button == MouseButtons.Right)
            {
                var cell = PointToCell(e.Location);
                OnCellContext(new CellClickEventArgs(cell));
                HoveredCell = cell;
            }
        }

        protected virtual void OnCellClick(CellClickEventArgs cellClickEventArgs)
        {
            CellClick?.Invoke(this, cellClickEventArgs);
        }

        // Добавлено 30.12.2022
        protected virtual void OnCellContext(CellClickEventArgs cellContextEventArgs)
        {
            CellContext?.Invoke(this, cellContextEventArgs);
        }

        Point PointToCell(Point p)
        {
            var cw = ClientSize.Width / GridSize.Width;
            var ch = ClientSize.Height / GridSize.Height;
            return new Point(p.X / cw, p.Y / ch);
        }

        public class CellNeededEventArgs : EventArgs
        {
            public Point Cell { get; private set; }
            public string Value { get; set; }
            public Color BackColor { get; set; }

            // добавлено 28.12.2022
            public bool IsBorder { get; set; }
            public bool Visible { get; set; }

            // добавлено 29.12.2022
            public Image Background { get; set; }
            public Image Action { get; set; }

            public CellNeededEventArgs(Point cell)
            {
                Cell = cell;
            }
        }

        public class CellClickEventArgs : EventArgs
        {
            public Point Cell { get; private set; }

            public CellClickEventArgs(Point cell)
            {
                Cell = cell;
            }
        }

        /// <summary>
        /// Добавлено 28.12.2022
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HoveredCell = new Point(-1, -1);
            Invalidate();
        }
    }
}
