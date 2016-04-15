namespace OpenDental{
	partial class FormApptPrintSetup {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptPrintSetup));
			this.labelColumnsPerPage = new System.Windows.Forms.Label();
			this.labelFontSize = new System.Windows.Forms.Label();
			this.labelStartTime = new System.Windows.Forms.Label();
			this.labelStopTime = new System.Windows.Forms.Label();
			this.textFontSize = new OpenDental.ValidNum();
			this.butSave = new OpenDental.UI.Button();
			this.textColumnsPerPage = new OpenDental.ValidNumber();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.comboStart = new System.Windows.Forms.ComboBox();
			this.comboStop = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// labelColumnsPerPage
			// 
			this.labelColumnsPerPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelColumnsPerPage.Location = new System.Drawing.Point(12, 85);
			this.labelColumnsPerPage.Name = "labelColumnsPerPage";
			this.labelColumnsPerPage.Size = new System.Drawing.Size(128, 15);
			this.labelColumnsPerPage.TabIndex = 72;
			this.labelColumnsPerPage.Text = "Operatories per page";
			this.labelColumnsPerPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelFontSize
			// 
			this.labelFontSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelFontSize.Location = new System.Drawing.Point(45, 111);
			this.labelFontSize.Name = "labelFontSize";
			this.labelFontSize.Size = new System.Drawing.Size(95, 15);
			this.labelFontSize.TabIndex = 74;
			this.labelFontSize.Text = "Font size";
			this.labelFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStartTime
			// 
			this.labelStartTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelStartTime.Location = new System.Drawing.Point(45, 33);
			this.labelStartTime.Name = "labelStartTime";
			this.labelStartTime.Size = new System.Drawing.Size(95, 15);
			this.labelStartTime.TabIndex = 76;
			this.labelStartTime.Text = "Start time";
			this.labelStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStopTime
			// 
			this.labelStopTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelStopTime.Location = new System.Drawing.Point(45, 59);
			this.labelStopTime.Name = "labelStopTime";
			this.labelStopTime.Size = new System.Drawing.Size(95, 15);
			this.labelStopTime.TabIndex = 78;
			this.labelStopTime.Text = "Stop time";
			this.labelStopTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFontSize
			// 
			this.textFontSize.Location = new System.Drawing.Point(146, 108);
			this.textFontSize.MaxVal = 50;
			this.textFontSize.MinVal = 2;
			this.textFontSize.Name = "textFontSize";
			this.textFontSize.Size = new System.Drawing.Size(50, 20);
			this.textFontSize.TabIndex = 88;
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Location = new System.Drawing.Point(12, 168);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(75, 24);
			this.butSave.TabIndex = 82;
			this.butSave.Text = "Save";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// textColumnsPerPage
			// 
			this.textColumnsPerPage.Location = new System.Drawing.Point(146, 82);
			this.textColumnsPerPage.MaxVal = 255;
			this.textColumnsPerPage.MinVal = 0;
			this.textColumnsPerPage.Name = "textColumnsPerPage";
			this.textColumnsPerPage.Size = new System.Drawing.Size(50, 20);
			this.textColumnsPerPage.TabIndex = 73;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(197, 168);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(280, 168);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(202, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 15);
			this.label3.TabIndex = 89;
			this.label3.Text = "Between 2 and 50";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboStart
			// 
			this.comboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStart.FormattingEnabled = true;
			this.comboStart.Location = new System.Drawing.Point(147, 33);
			this.comboStart.Name = "comboStart";
			this.comboStart.Size = new System.Drawing.Size(142, 21);
			this.comboStart.TabIndex = 90;
			// 
			// comboStop
			// 
			this.comboStop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStop.FormattingEnabled = true;
			this.comboStop.Location = new System.Drawing.Point(146, 57);
			this.comboStop.Name = "comboStop";
			this.comboStop.Size = new System.Drawing.Size(143, 21);
			this.comboStop.TabIndex = 91;
			// 
			// FormApptPrintSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(367, 204);
			this.Controls.Add(this.comboStop);
			this.Controls.Add(this.comboStart);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textFontSize);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.labelStopTime);
			this.Controls.Add(this.labelStartTime);
			this.Controls.Add(this.labelFontSize);
			this.Controls.Add(this.textColumnsPerPage);
			this.Controls.Add(this.labelColumnsPerPage);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(383, 242);
			this.MinimumSize = new System.Drawing.Size(383, 242);
			this.Name = "FormApptPrintSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form Appt Print Setup";
			this.Load += new System.EventHandler(this.FormApptPrintSetup_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label labelColumnsPerPage;
		private ValidNumber textColumnsPerPage;
		private System.Windows.Forms.Label labelFontSize;
		private System.Windows.Forms.Label labelStartTime;
		private System.Windows.Forms.Label labelStopTime;
		private UI.Button butSave;
		private ValidNum textFontSize;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboStart;
		private System.Windows.Forms.ComboBox comboStop;
	}
}