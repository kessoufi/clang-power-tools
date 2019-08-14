﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClangPowerTools
{
  public class CPTSettings
  {
    private string settingsPath = string.Empty;
    private readonly string SettingsFileName = "settings.json";
    private readonly string GeneralConfigurationFileName = "GeneralConfiguration.config";
    private readonly string FormatConfigurationFileName = "FormatConfiguration.config";
    private readonly string TidyOptionsConfigurationFileName = "TidyOptionsConfiguration.config";
    private readonly string TidyPredefinedChecksConfigurationFileName = "TidyPredefinedChecksConfiguration.config";

    public CPTSettings()
    {
      SettingsPathBuilder settingsPathBuilder = new SettingsPathBuilder();
      settingsPath = settingsPathBuilder.GetPath("");
    }

    public void SerializeSettings()
    {
      List<object> models = CreateModelsList();

      using (StreamWriter file = File.CreateText(GetSettingsFilePath(settingsPath, SettingsFileName)))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Formatting = Formatting.Indented;
        serializer.Serialize(file, models);
      }
    }

    public void DeserializeSettings()
    {
      using (StreamReader sw = new StreamReader(GetSettingsFilePath(settingsPath, SettingsFileName)))
      {
        string json = sw.ReadToEnd();
        JsonSerializer serializer = new JsonSerializer();
        List<object> models = JsonConvert.DeserializeObject<List<object>>(json);

        //TODO handle error deserialization

        SettingsModelHandler.CompilerSettings = JsonConvert.DeserializeObject<CompilerSettingsModel>(models[0].ToString());
        SettingsModelHandler.FormatSettings = JsonConvert.DeserializeObject<FormatSettingsModel>(models[1].ToString());
        SettingsModelHandler.TidySettings = JsonConvert.DeserializeObject<TidySettingsModel>(models[2].ToString());
      }
    }

    public bool CheckIfSettingsFileExists()
    {
      string path = GetSettingsFilePath(settingsPath, SettingsFileName);
      return File.Exists(path);
    }

    public bool CheckOldGeneralSettingsExists()
    {
      string path = GetSettingsFilePath(settingsPath, GeneralConfigurationFileName);
      return File.Exists(path);
    }

    public void MapOldSettings()
    {
      ClangOptions clangOptions = LoadOldSettingsFromFile(new ClangOptions(), GeneralConfigurationFileName);
      MapClangOptionsToCompilerSettings(clangOptions);

      ClangFormatOptions clangFormatOptions = LoadOldSettingsFromFile(new ClangFormatOptions(), FormatConfigurationFileName);
      MapClangFormatOptionsToFormatSettings(clangFormatOptions);

      ClangTidyOptions clangTidyOptions = LoadOldSettingsFromFile(new ClangTidyOptions(), TidyOptionsConfigurationFileName);
      MapClangTidyOptionsToTidyettings(clangTidyOptions);

      ClangTidyPredefinedChecksOptions clangTidyPredefinedChecksOptions = LoadOldSettingsFromFile(new ClangTidyPredefinedChecksOptions(), TidyPredefinedChecksConfigurationFileName);
      MapTidyPredefinedChecksToTidyettings(clangTidyPredefinedChecksOptions);

      SerializeSettings();
    }

    private T LoadOldSettingsFromFile<T>(T settings, string settingsFileName) where T : new()
    {
      string path = GetSettingsFilePath(settingsPath, settingsFileName);

      if (File.Exists(path))
      {
        SerializeSettings(path, ref settings);
      }
      return settings;
    }

    private void DeleteOldSettings()
    {
      string[] files = Directory.GetFiles(settingsPath, "*.config");
      foreach (var file in files)
      {
        File.Delete(file);
      }
    }

    private void DeleteSettings()
    {
      string[] file = Directory.GetFiles(settingsPath, SettingsFileName);
      if (file.Length > 0)
      {
        File.Delete(file[0]);
      }
    }

    public void ResetSettings()
    {
      SettingsModelHandler.CompilerSettings = new CompilerSettingsModel();
      SettingsModelHandler.FormatSettings = new FormatSettingsModel();
    }

    private static List<object> CreateModelsList()
    {
      List<object> models = new List<object>();
      models.Add(SettingsModelHandler.CompilerSettings);
      models.Add(SettingsModelHandler.FormatSettings);
      models.Add(SettingsModelHandler.TidySettings);
      return models;
    }

    private string GetSettingsFilePath(string path, string fileName)
    {
      return Path.Combine(path, fileName);
    }

    private void SerializeSettings<T>(string path, ref T config) where T : new()
    {
      XmlSerializer serializer = new XmlSerializer();
      config = serializer.DeserializeFromFile<T>(path);
    }

    private void MapClangOptionsToCompilerSettings(ClangOptions clangOptions)
    {
      SettingsModelHandler.CompilerSettings.CompileFlags = clangOptions.ClangFlagsCollection;
      SettingsModelHandler.CompilerSettings.FilesToIgnore = clangOptions.FilesToIgnore;
      SettingsModelHandler.CompilerSettings.ProjectsToIgnore = clangOptions.ProjectsToIgnore;
      SettingsModelHandler.CompilerSettings.AdditionalIncludes = clangOptions.AdditionalIncludes;
      SettingsModelHandler.CompilerSettings.WarningsAsErrors = clangOptions.TreatWarningsAsErrors;
      SettingsModelHandler.CompilerSettings.ContinueOnError = clangOptions.Continue;
      SettingsModelHandler.CompilerSettings.ClangCompileAfterMSCVCompile = clangOptions.ClangCompileAfterVsCompile;
      SettingsModelHandler.CompilerSettings.VerboseMode = clangOptions.VerboseMode;
      SettingsModelHandler.CompilerSettings.Version = clangOptions.Version;
    }

    private void MapClangFormatOptionsToFormatSettings(ClangFormatOptions clangFormat)
    {
      SettingsModelHandler.FormatSettings.FileExtensions = clangFormat.FileExtensions;
      SettingsModelHandler.FormatSettings.FilesToIgnore = clangFormat.SkipFiles;
      SettingsModelHandler.FormatSettings.AssumeFilename = clangFormat.AssumeFilename;
      SettingsModelHandler.FormatSettings.CustomExecutable = clangFormat.ClangFormatPath.Value;
      SettingsModelHandler.FormatSettings.Style = clangFormat.Style;
      SettingsModelHandler.FormatSettings.FallbackStyle = clangFormat.FallbackStyle;
      SettingsModelHandler.FormatSettings.FormatOnSave = clangFormat.EnableFormatOnSave;
    }

    private void MapClangTidyOptionsToTidyettings(ClangTidyOptions clangTidy)
    {
      SettingsModelHandler.TidySettings.HeaderFilter = clangTidy.HeaderFilter;
      SettingsModelHandler.TidySettings.Checks = clangTidy.TidyChecksCollection;
      SettingsModelHandler.TidySettings.CustomExecutable = clangTidy.ClangTidyPath.Value;
      SettingsModelHandler.TidySettings.FormatAfterTidy = clangTidy.FormatAfterTidy;
      SettingsModelHandler.TidySettings.TidyOnSave = clangTidy.AutoTidyOnSave;
    }

    private void MapTidyPredefinedChecksToTidyettings(ClangTidyPredefinedChecksOptions clangTidyPredefinedChecksOptions)
    {
      PropertyInfo[] properties = typeof(ClangTidyPredefinedChecksOptions).GetProperties();

      foreach (PropertyInfo propertyInfo in properties)
      {
        bool isChecked = (bool)propertyInfo.GetValue(new ClangTidyPredefinedChecksOptions(), null);

        if (isChecked)
        {
          SettingsModelHandler.TidySettings.Checks += string.Concat(FormatTidyCheckName(propertyInfo.Name), ";");
        }
      }
    }

    private string FormatTidyCheckName(string name)
    {
      StringBuilder stringBuilder = new StringBuilder();

      stringBuilder.Append(name[0]);
      for (int i = 1; i < name.Length; i++)
      {
        if(Char.IsUpper(name[i]))
        {
          stringBuilder.Append(name[i]).Append("-");
        }
        stringBuilder.Append(name[i]);
      }

      return stringBuilder.ToString().ToLower();
    }
  }
}