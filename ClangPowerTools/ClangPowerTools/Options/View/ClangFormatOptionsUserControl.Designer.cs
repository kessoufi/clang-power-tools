﻿namespace ClangPowerTools.Options.ViewModel
{
  partial class ClangFormatOptionsUserControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.clangFormatOptionsElementHost = new System.Windows.Forms.Integration.ElementHost();
      this.SuspendLayout();
      // 
      // clangFormatOptionsElementHost
      // 
      this.clangFormatOptionsElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
      this.clangFormatOptionsElementHost.Location = new System.Drawing.Point(0, 0);
      this.clangFormatOptionsElementHost.Name = "clangFormatOptionsElementHost";
      this.clangFormatOptionsElementHost.Size = new System.Drawing.Size(1814, 974);
      this.clangFormatOptionsElementHost.TabIndex = 0;
      this.clangFormatOptionsElementHost.Text = "elementHost1";
      this.clangFormatOptionsElementHost.Child = null;
      // 
      // ClangFormatOptionsUserControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.clangFormatOptionsElementHost);
      this.Name = "ClangFormatOptionsUserControl";
      this.Size = new System.Drawing.Size(1814, 974);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Integration.ElementHost clangFormatOptionsElementHost;
  }
}
