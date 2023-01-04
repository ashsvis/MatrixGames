
namespace Sokoban.View
{
    partial class ucLevel
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNext = new System.Windows.Forms.LinkLabel();
            this.btnReset = new System.Windows.Forms.LinkLabel();
            this.btnPrev = new System.Windows.Forms.LinkLabel();
            this.kbdView = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.AutoSize = true;
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.Enabled = false;
            this.btnNext.Font = new System.Drawing.Font("Segoe Print", 12F);
            this.btnNext.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnNext.LinkColor = System.Drawing.Color.Teal;
            this.btnNext.Location = new System.Drawing.Point(660, 397);
            this.btnNext.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(175, 28);
            this.btnNext.TabIndex = 7;
            this.btnNext.TabStop = true;
            this.btnNext.Text = "Следующий уровень";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.AutoSize = true;
            this.btnReset.BackColor = System.Drawing.Color.Transparent;
            this.btnReset.Enabled = false;
            this.btnReset.Font = new System.Drawing.Font("Segoe Print", 12F);
            this.btnReset.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnReset.LinkColor = System.Drawing.Color.Teal;
            this.btnReset.Location = new System.Drawing.Point(403, 397);
            this.btnReset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(58, 28);
            this.btnReset.TabIndex = 5;
            this.btnReset.TabStop = true;
            this.btnReset.Text = "Сброс";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.AutoSize = true;
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Enabled = false;
            this.btnPrev.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPrev.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnPrev.LinkColor = System.Drawing.Color.Teal;
            this.btnPrev.Location = new System.Drawing.Point(471, 397);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(182, 28);
            this.btnPrev.TabIndex = 6;
            this.btnPrev.TabStop = true;
            this.btnPrev.Text = "Предыдущий уровень";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // kbdView
            // 
            this.kbdView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kbdView.BackColor = System.Drawing.Color.Gray;
            this.kbdView.Location = new System.Drawing.Point(379, 405);
            this.kbdView.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.kbdView.Multiline = true;
            this.kbdView.Name = "kbdView";
            this.kbdView.Size = new System.Drawing.Size(15, 15);
            this.kbdView.TabIndex = 4;
            // 
            // ucLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Sokoban.Properties.Resources.SokobanSpace;
            this.Controls.Add(this.kbdView);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPrev);
            this.Font = new System.Drawing.Font("Segoe Print", 12F);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "ucLevel";
            this.Size = new System.Drawing.Size(841, 432);
            this.Load += new System.EventHandler(this.ucLevel_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucLevel_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ucLevel_MouseMove);
            this.Resize += new System.EventHandler(this.ucLevel_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel btnNext;
        private System.Windows.Forms.LinkLabel btnReset;
        private System.Windows.Forms.LinkLabel btnPrev;
        private System.Windows.Forms.TextBox kbdView;
    }
}
