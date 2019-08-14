﻿namespace ClangPowerTools
{
  class SettingsModelHandler
  {
    public static CompilerSettingsModel CompilerSettings { get; set; } = new CompilerSettingsModel();
    public static FormatSettingsModel FormatSettings { get; set; } = new FormatSettingsModel();
    public static TidySettingsModel TidySettings { get; set; } = new TidySettingsModel();
  }
}