
namespace CryEngine.RunTime.Testing.Reports
{
    partial class ReportForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.uxTestTree = new System.Windows.Forms.TreeView();
            this.uxTreeImages = new System.Windows.Forms.ImageList(this.components);
            this.uxTimeMessage = new System.Windows.Forms.Label();
            this.uxTopGroup = new System.Windows.Forms.GroupBox();
            this.uxIgnoredCount = new System.Windows.Forms.Label();
            this.uxFailureCount = new System.Windows.Forms.Label();
            this.uxSuccessCount = new System.Windows.Forms.Label();
            this.uxRerunTests = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uxTestResult = new System.Windows.Forms.TextBox();
            this.uxTopGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxTestTree
            // 
            this.uxTestTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxTestTree.ImageIndex = 0;
            this.uxTestTree.ImageList = this.uxTreeImages;
            this.uxTestTree.Location = new System.Drawing.Point(12, 12);
            this.uxTestTree.Name = "uxTestTree";
            this.uxTestTree.SelectedImageIndex = 0;
            this.uxTestTree.Size = new System.Drawing.Size(382, 428);
            this.uxTestTree.TabIndex = 0;
            this.uxTestTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeSelect);
            // 
            // uxTreeImages
            // 
            this.uxTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("uxTreeImages.ImageStream")));
            this.uxTreeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.uxTreeImages.Images.SetKeyName(0, "TestSuccess.png");
            this.uxTreeImages.Images.SetKeyName(1, "TestFailed.png");
            this.uxTreeImages.Images.SetKeyName(2, "TestIgnored.png");
            // 
            // uxTimeMessage
            // 
            this.uxTimeMessage.AutoSize = true;
            this.uxTimeMessage.Location = new System.Drawing.Point(6, 16);
            this.uxTimeMessage.Name = "uxTimeMessage";
            this.uxTimeMessage.Size = new System.Drawing.Size(55, 13);
            this.uxTimeMessage.TabIndex = 1;
            this.uxTimeMessage.Text = "Tests time";
            // 
            // uxTopGroup
            // 
            this.uxTopGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxTopGroup.Controls.Add(this.uxIgnoredCount);
            this.uxTopGroup.Controls.Add(this.uxFailureCount);
            this.uxTopGroup.Controls.Add(this.uxSuccessCount);
            this.uxTopGroup.Controls.Add(this.uxTimeMessage);
            this.uxTopGroup.Location = new System.Drawing.Point(400, 12);
            this.uxTopGroup.Name = "uxTopGroup";
            this.uxTopGroup.Size = new System.Drawing.Size(395, 93);
            this.uxTopGroup.TabIndex = 2;
            this.uxTopGroup.TabStop = false;
            this.uxTopGroup.Text = "Test Run Information";
            // 
            // uxIgnoredCount
            // 
            this.uxIgnoredCount.AutoSize = true;
            this.uxIgnoredCount.Location = new System.Drawing.Point(6, 57);
            this.uxIgnoredCount.Name = "uxIgnoredCount";
            this.uxIgnoredCount.Size = new System.Drawing.Size(43, 13);
            this.uxIgnoredCount.TabIndex = 5;
            this.uxIgnoredCount.Text = "Ignored";
            // 
            // uxFailureCount
            // 
            this.uxFailureCount.AutoSize = true;
            this.uxFailureCount.Location = new System.Drawing.Point(6, 72);
            this.uxFailureCount.Name = "uxFailureCount";
            this.uxFailureCount.Size = new System.Drawing.Size(43, 13);
            this.uxFailureCount.TabIndex = 4;
            this.uxFailureCount.Text = "Failures";
            // 
            // uxSuccessCount
            // 
            this.uxSuccessCount.AutoSize = true;
            this.uxSuccessCount.Location = new System.Drawing.Point(6, 42);
            this.uxSuccessCount.Name = "uxSuccessCount";
            this.uxSuccessCount.Size = new System.Drawing.Size(59, 13);
            this.uxSuccessCount.TabIndex = 3;
            this.uxSuccessCount.Text = "Successes";
            // 
            // uxRerunTests
            // 
            this.uxRerunTests.Location = new System.Drawing.Point(400, 111);
            this.uxRerunTests.Name = "uxRerunTests";
            this.uxRerunTests.Size = new System.Drawing.Size(87, 23);
            this.uxRerunTests.TabIndex = 3;
            this.uxRerunTests.Text = "Run Tests";
            this.uxRerunTests.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.uxTestResult);
            this.groupBox1.Location = new System.Drawing.Point(400, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 300);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Test";
            // 
            // uxTestResult
            // 
            this.uxTestResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxTestResult.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxTestResult.Location = new System.Drawing.Point(9, 19);
            this.uxTestResult.Multiline = true;
            this.uxTestResult.Name = "uxTestResult";
            this.uxTestResult.ReadOnly = true;
            this.uxTestResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxTestResult.Size = new System.Drawing.Size(380, 275);
            this.uxTestResult.TabIndex = 7;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 452);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.uxRerunTests);
            this.Controls.Add(this.uxTopGroup);
            this.Controls.Add(this.uxTestTree);
            this.Name = "ReportForm";
            this.Text = "CryMono Unit Test Report";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClose);
            this.uxTopGroup.ResumeLayout(false);
            this.uxTopGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView uxTestTree;
        private System.Windows.Forms.ImageList uxTreeImages;
        private System.Windows.Forms.Label uxTimeMessage;
        private System.Windows.Forms.GroupBox uxTopGroup;
        private System.Windows.Forms.Label uxIgnoredCount;
        private System.Windows.Forms.Label uxFailureCount;
        private System.Windows.Forms.Label uxSuccessCount;
        private System.Windows.Forms.Button uxRerunTests;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox uxTestResult;
    }
}