﻿using ClangPowerTools.MVVM.Commands;
using ClangPowerTools.Views;
using System.IO;
using System.Windows.Input;

namespace ClangPowerTools
{
  public class GeneralSettingsViewModel : CommonSettingsFunctionality
  {
    #region Members
    private SettingsHandler cptSettings = new SettingsHandler();

    private ICommand logoutCommand;
    private ICommand exportSettingsCommand;
    private ICommand importSettingsCommand;
    private ICommand resetSettingsCommand;
    #endregion

    #region Commands
    public ICommand LogoutCommand
    {
      get => logoutCommand ?? (logoutCommand = new RelayCommand(() => Logout(), () => CanExecute));
    }

    public ICommand ExportSettingsCommand
    {
      get => exportSettingsCommand ?? (exportSettingsCommand = new RelayCommand(() => ExportSettings(), () => CanExecute));
    }

    public ICommand ImportSettingssCommand
    {
      get => importSettingsCommand ?? (importSettingsCommand = new RelayCommand(() => ImportSettings(), () => CanExecute));
    }

    public ICommand ResetSettingsCommand
    {
      get => resetSettingsCommand ?? (resetSettingsCommand = new RelayCommand(() => ResetSettings(), () => CanExecute));
    }

    #endregion

    #region Properties
    public bool CanExecute
    {
      get
      {
        return true;
      }
    }
    #endregion


    #region Methods
    private void Logout()
    {
      var settingsPathBuilder = new SettingsPathBuilder();
      string path = settingsPathBuilder.GetPath("ctpjwt");

      if (File.Exists(path) == true)
      {
        File.Delete(path);
      }

      LoginView loginView = new LoginView();
      loginView.ShowDialog();
    }

    private void ExportSettings()
    {
      string path = SaveFile("settings", ".json", "Settings files (.json)|*.json");
      if (string.IsNullOrEmpty(path) == false)
      {
        cptSettings.SaveSettings(path);
      }
    }

    private void ImportSettings()
    {
      string path = OpenFile("settings", ".json", "Settings files (.json)|*.json");
      if (string.IsNullOrEmpty(path) == false)
      {
        cptSettings.LoadSettings(path);
      }
    }

    private void ResetSettings()
    {
      cptSettings.ResetSettings();
    }

    #endregion
  }
}