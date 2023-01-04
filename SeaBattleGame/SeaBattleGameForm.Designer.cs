namespace SeaBattleGame
{
    partial class SeaBattleGameForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeaBattleGameForm));
            this.lbGameCaption = new System.Windows.Forms.Label();
            this.lbGameStatus = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.enemyScore = new SeaBattleGame.ScoreUC();
            this.playerScore = new SeaBattleGame.ScoreUC();
            this.SuspendLayout();
            // 
            // lbGameCaption
            // 
            this.lbGameCaption.BackColor = System.Drawing.Color.Transparent;
            this.lbGameCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbGameCaption.Font = new System.Drawing.Font("Segoe Print", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbGameCaption.Location = new System.Drawing.Point(0, 0);
            this.lbGameCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGameCaption.Name = "lbGameCaption";
            this.lbGameCaption.Size = new System.Drawing.Size(1056, 36);
            this.lbGameCaption.TabIndex = 0;
            this.lbGameCaption.Text = "Морской бой между эскадрами";
            this.lbGameCaption.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbGameStatus
            // 
            this.lbGameStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbGameStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbGameStatus.Font = new System.Drawing.Font("Segoe Print", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbGameStatus.ForeColor = System.Drawing.Color.Red;
            this.lbGameStatus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbGameStatus.Location = new System.Drawing.Point(0, 435);
            this.lbGameStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGameStatus.Name = "lbGameStatus";
            this.lbGameStatus.Size = new System.Drawing.Size(1056, 44);
            this.lbGameStatus.TabIndex = 0;
            this.lbGameStatus.Text = "Ваш выстрел.";
            this.lbGameStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRestart
            // 
            this.btnRestart.BackgroundImage = global::SeaBattleGame.Properties.Resources.DefaultCellImage;
            this.btnRestart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(48)))), ((int)(((byte)(173)))));
            this.btnRestart.FlatAppearance.BorderSize = 2;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Location = new System.Drawing.Point(889, 427);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(159, 40);
            this.btnRestart.TabIndex = 2;
            this.btnRestart.Text = "Играть заново";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Visible = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // enemyScore
            // 
            this.enemyScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enemyScore.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("enemyScore.BackgroundImage")));
            this.enemyScore.Caption = "Эскадра";
            this.enemyScore.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold);
            this.enemyScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(48)))), ((int)(((byte)(173)))));
            this.enemyScore.Location = new System.Drawing.Point(845, 231);
            this.enemyScore.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.enemyScore.Name = "enemyScore";
            this.enemyScore.ShipDesk1 = 0;
            this.enemyScore.ShipDesk2 = 0;
            this.enemyScore.ShipDesk3 = 0;
            this.enemyScore.ShipDesk4 = 0;
            this.enemyScore.Size = new System.Drawing.Size(211, 162);
            this.enemyScore.TabIndex = 1;
            // 
            // playerScore
            // 
            this.playerScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playerScore.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("playerScore.BackgroundImage")));
            this.playerScore.Caption = "Эскадра";
            this.playerScore.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold);
            this.playerScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(48)))), ((int)(((byte)(173)))));
            this.playerScore.Location = new System.Drawing.Point(845, 33);
            this.playerScore.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.playerScore.Name = "playerScore";
            this.playerScore.ShipDesk1 = 0;
            this.playerScore.ShipDesk2 = 0;
            this.playerScore.ShipDesk3 = 0;
            this.playerScore.ShipDesk4 = 0;
            this.playerScore.Size = new System.Drawing.Size(211, 162);
            this.playerScore.TabIndex = 1;
            // 
            // SeaBattleGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SeaBattleGame.Properties.Resources.DefaultCellImage;
            this.ClientSize = new System.Drawing.Size(1056, 479);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.enemyScore);
            this.Controls.Add(this.playerScore);
            this.Controls.Add(this.lbGameStatus);
            this.Controls.Add(this.lbGameCaption);
            this.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(48)))), ((int)(((byte)(173)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "SeaBattleGameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Морской бой (пример использования кастомного пользовательского контрола)";
            this.Load += new System.EventHandler(this.SeaBattleGameForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbGameCaption;
        private System.Windows.Forms.Label lbGameStatus;
        private ScoreUC playerScore;
        private ScoreUC enemyScore;
        private System.Windows.Forms.Button btnRestart;
    }
}

