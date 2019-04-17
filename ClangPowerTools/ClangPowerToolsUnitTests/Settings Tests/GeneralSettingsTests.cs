﻿using Microsoft.VisualStudio.Shell;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ClangPowerTools.Tests.Settings
{
  [VsTestSettings(UIThread = true)]
  public class GeneralSettingsTests
  {
    [VsFact(Version = "2019")]
    public async Task ClangGeneralOptionsView_NotNullAsync()
    {
      //Arrange
      await UnitTestUtility.LoadPackageAsync();

      //Act
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Assert
      Assert.NotNull(generalSettings);
    }

    [VsFact(Version = "2019")]
    public async Task CompileFlags_ChangeValue_CompareViewToFileAsync()
    {
      //Arrange
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.ClangFlags = "-Wall";
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();
    
      //Assert
      Assert.Equal(generalSettings.ClangFlags, generalSettingsFromFile.ClangFlags);
    }

    [VsFact(Version = "2019")]
    public async Task FilesToIgnore_ChangeValue_CompareViewToFileAsync()
    {
      //Arrange
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.FilesToIgnore = "test.cpp";
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();

      //Assert
      Assert.Equal(generalSettings.FilesToIgnore, generalSettingsFromFile.FilesToIgnore);
    }

    [VsFact(Version = "2019")]
    public async Task ProjectToIgnore_ChangeValue_CompareViewToFileAsync()
    {
      //Arrange
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.ProjectsToIgnore = "TestProject";
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();

      //Assert
      Assert.Equal(generalSettings.ProjectsToIgnore, generalSettingsFromFile.ProjectsToIgnore);
    }

    [VsFact(Version = "2019")]
    public async Task AdditionalIncludes_ChangeValue_CompareViewToFileAsync()
    {
      //Arrange
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.AdditionalIncludes = ClangGeneralAdditionalIncludes.SystemIncludeDirectories;
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();

      //Assert
      Assert.Equal(generalSettings.AdditionalIncludes.Value, generalSettingsFromFile.AdditionalIncludes.Value);
    }

    [VsFact(Version = "2019")]
    public async Task TreatWarningsAsErrors_ChangeValue_CompareViewToFileAsync()
    {
      //Arrange
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.TreatWarningsAsErrors = true;
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();

      //Assert
      Assert.Equal(generalSettings.TreatWarningsAsErrors, generalSettingsFromFile.TreatWarningsAsErrors);
    }

    [VsFact(Version = "2019")]
    public async Task ContinueOnError_ChangeValue_CompareViewToFileAsync()
    {
      //Arrange
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.Continue = true;
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();

      //Assert
      Assert.Equal(generalSettings.Continue, generalSettingsFromFile.Continue);
    }


    [VsFact(Version = "2019")]
    public async Task ClangCompileAfterVsCompile_ChangeValue_CompareViewToFileAsync()
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.ClangCompileAfterVsCompile = true;
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();

      //Assert
      Assert.Equal(generalSettings.ClangCompileAfterVsCompile, generalSettingsFromFile.ClangCompileAfterVsCompile);
    }

    [VsFact(Version = "2019")]
    public async Task VerboseMode_ChangeValue_CompareViewToFileAsync()
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      await UnitTestUtility.LoadPackageAsync();
      UnitTestUtility.ResetClangGeneralOptionsView();
      ClangGeneralOptionsView generalSettings = SettingsProvider.GeneralSettings;

      //Act
      generalSettings.VerboseMode = true;
      UnitTestUtility.SaveClangOptions(generalSettings);
      ClangGeneralOptionsView generalSettingsFromFile = UnitTestUtility.GetClangGeneralOptionsViewFromFile();

      //Assert
      Assert.Equal(generalSettings.VerboseMode, generalSettingsFromFile.VerboseMode);
    }
  }
}
