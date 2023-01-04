namespace XonixWfApp
{
    partial class GameUC
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
            this.components = new System.ComponentModel.Container();
            this.stepTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // stepTimer
            // 
            this.stepTimer.Interval = 10;
            this.stepTimer.Tick += new System.EventHandler(this.stepTimer_Tick);
            // 
            // GameUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "GameUC";
            this.Load += new System.EventHandler(this.GameUC_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameUC_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer stepTimer;
    }
}
