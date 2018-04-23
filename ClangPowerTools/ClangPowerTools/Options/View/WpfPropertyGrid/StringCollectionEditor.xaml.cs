﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ClangPowerTools.Options.View.WpfPropertyGrid
{
  /// <summary>
  /// Interaction logic for StringCollectionEditor.xaml.
  /// </summary>
  [ProvideToolboxControl("ClangPowerTools.Options.View.WpfPropertyGrid.MultilineTextBox", true)]
  public partial class StringCollectionEditor : Window
  {
    #region Hide minimize and maximize buttons members

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    private const int GWL_STYLE = -16;
    private const int WS_MAXIMIZEBOX = 0x10000; //maximize button
    private const int WS_MINIMIZEBOX = 0x20000; //minimize button

    private IntPtr _windowHandle;

    #endregion


    public StringCollectionEditor()
    {
      InitializeComponent();
      this.SourceInitialized += MainWindowSourceInitialized;
    }

    #region Hide minimize and maximize buttons method

    private void MainWindowSourceInitialized(object sender, EventArgs e)
    {
      _windowHandle = new WindowInteropHelper(this).Handle;
      HideMinimizeAndMaximizeButtons();
    }

    protected void HideMinimizeAndMaximizeButtons()
    {
      if (_windowHandle == null)
        return; // The window has not yet been completely initialized

      SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
    }

    #endregion


  }
}
