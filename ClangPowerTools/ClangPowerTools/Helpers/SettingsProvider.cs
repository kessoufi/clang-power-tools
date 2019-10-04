﻿using ClangPowerTools.DialogPages;
using Microsoft.VisualStudio.Shell;

namespace ClangPowerTools
{
  public class SettingsProvider
  {
    #region Properties

    public static ClangGeneralOptionsView GeneralSettings { get; private set; }
    public static ClangTidyOptionsView TidySettings { get; private set; }
    public static ClangTidyCustomChecksOptionsView TidyCustomCheckes { get; private set; }
    public static ClangTidyPredefinedChecksOptionsView TidyPredefinedChecks { get; private set; }
    public static ClangFormatOptionsView ClangFormatSettings { get; private set; }

    #endregion


    #region Constructor

    public static void Initialize(AsyncPackage aPackage)
    {
      GeneralSettings = (ClangGeneralOptionsView)aPackage.GetDialogPage(typeof(ClangGeneralOptionsView));
      TidySettings = (ClangTidyOptionsView)aPackage.GetDialogPage(typeof(ClangTidyOptionsView));
      TidyCustomCheckes = (ClangTidyCustomChecksOptionsView)aPackage.GetDialogPage(typeof(ClangTidyCustomChecksOptionsView));
      TidyPredefinedChecks = (ClangTidyPredefinedChecksOptionsView)aPackage.GetDialogPage(typeof(ClangTidyPredefinedChecksOptionsView));
      ClangFormatSettings = (ClangFormatOptionsView)aPackage.GetDialogPage(typeof(ClangFormatOptionsView));
    }


    public static void SaveAll()
    {
      GeneralSettings.SaveSettingsToStorage();
      TidySettings.SaveSettingsToStorage();
      TidyCustomCheckes.SaveSettingsToStorage();
      TidyPredefinedChecks.SaveSettingsToStorage();
      ClangFormatSettings.SaveSettingsToStorage();
    }

    public static void SaveGeneralSettings() => GeneralSettings.SaveSettingsToStorage();

    public static void SaveTidySettings() => TidySettings.SaveSettingsToStorage();

    public static void SaveTidyCustomChecksSettings() => TidyCustomCheckes.SaveSettingsToStorage();

    public static void SaveTidyPredefinedChecksSettings() => TidyPredefinedChecks.SaveSettingsToStorage();

    public static void SaveFormatSettings() => ClangFormatSettings.SaveSettingsToStorage();

    #endregion

  }
}
