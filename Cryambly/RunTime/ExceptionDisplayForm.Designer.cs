namespace CryCil.RunTime
{
	partial class ExceptionDisplayForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.ExceptionTypeBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ExceptionMessageBox = new System.Windows.Forms.TextBox();
			this.ExceptionTraceBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Exception Type:";
			// 
			// ExceptionTypeBox
			// 
			this.ExceptionTypeBox.Font = new System.Drawing.Font("Consolas", 8.25F);
			this.ExceptionTypeBox.Location = new System.Drawing.Point(102, 6);
			this.ExceptionTypeBox.Name = "ExceptionTypeBox";
			this.ExceptionTypeBox.ReadOnly = true;
			this.ExceptionTypeBox.Size = new System.Drawing.Size(302, 20);
			this.ExceptionTypeBox.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(43, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Message:";
			// 
			// ExceptionMessageBox
			// 
			this.ExceptionMessageBox.Font = new System.Drawing.Font("Consolas", 8.25F);
			this.ExceptionMessageBox.Location = new System.Drawing.Point(102, 32);
			this.ExceptionMessageBox.Name = "ExceptionMessageBox";
			this.ExceptionMessageBox.ReadOnly = true;
			this.ExceptionMessageBox.Size = new System.Drawing.Size(302, 20);
			this.ExceptionMessageBox.TabIndex = 3;
			// 
			// ExceptionTraceBox
			// 
			this.ExceptionTraceBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ExceptionTraceBox.Location = new System.Drawing.Point(102, 58);
			this.ExceptionTraceBox.MaxLength = 5000000;
			this.ExceptionTraceBox.Multiline = true;
			this.ExceptionTraceBox.Name = "ExceptionTraceBox";
			this.ExceptionTraceBox.ReadOnly = true;
			this.ExceptionTraceBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ExceptionTraceBox.Size = new System.Drawing.Size(555, 367);
			this.ExceptionTraceBox.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 61);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Exception Trace:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(410, 6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 21);
			this.button1.TabIndex = 6;
			this.button1.Text = "Continue";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.AttemptToContinue);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(410, 32);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 20);
			this.button2.TabIndex = 7;
			this.button2.Text = "Quit";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Terminate);
			// 
			// ExceptionDisplayForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(669, 437);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ExceptionTraceBox);
			this.Controls.Add(this.ExceptionMessageBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ExceptionTypeBox);
			this.Controls.Add(this.label1);
			this.Name = "ExceptionDisplayForm";
			this.Text = "An unhandled exception has been caught";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ExceptionTypeBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox ExceptionMessageBox;
		private System.Windows.Forms.TextBox ExceptionTraceBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}