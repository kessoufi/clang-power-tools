﻿using System.Windows.Controls;

namespace ClangPowerTools.Views
{
  /// <summary>
  /// Interaction logic for GeneralOptionsView.xaml
  /// </summary>
  public partial class CompilerSettingsView : UserControl
  {
    public CompilerSettingsView()
    {
      InitializeComponent();
      DataContext = new CompilerSettingsViewModel();
    }
  }
}
