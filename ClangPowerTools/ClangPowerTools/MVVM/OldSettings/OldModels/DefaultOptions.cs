﻿namespace ClangPowerTools
{
  public class DefaultOptions
  {
    public const string ClangFlags = "-Wall;-fms-compatibility-version=19.10;-Wmicrosoft;-Wno-invalid-token-paste;-Wno-unknown-pragmas;-Wno-unused-value";
    public const string HeaderFilter = ".*";
    public const string FileExtensions = ".c;.cpp;.cxx;.cc;.tli;.tlh;.h;.hh;.hpp;.hxx;.hh";
    public const string IgnoreFiles = "resource.h";
  }
}
