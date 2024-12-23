using System;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Nwwz.Mvc.Testing;

internal static class Resources
{
  private static ResourceManager s_resourceManager;
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (object.Equals(null, s_resourceManager)) {
        System.Resources.ResourceManager temp = new System.Resources.ResourceManager("IntegrationTests.CustomTestSdkExtenstion.Resources1", typeof(Resources1).Assembly);
        s_resourceManager = temp;
      }
      return s_resourceManager;
    }
  }

  internal static CultureInfo Culture { get; set; }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static string GetResourceString(string resourceKey, string defaultValue = null)
  {
    return Resources.ResourceManager.GetString(resourceKey, Resources.Culture);
  }
    
  internal static string FormatInvalidAssemblyEntryPoint(object p0)
  {
    return string.Format((IFormatProvider) Resources.Culture, Resources.GetResourceString("InvalidAssemblyEntryPoint"), p0);
  }

  internal static string MissingBuilderMethod
  {
    get => Resources.GetResourceString(nameof (MissingBuilderMethod));
  }

  internal static string FormatMissingBuilderMethod(
    object p0,
    object p1,
    object p2,
    object p3,
    object p4,
    object p5)
  {
    return string.Format((IFormatProvider) Resources.Culture, 
      Resources.GetResourceString("MissingBuilderMethod"), p0, p1, p2, p3, p4, p5);
  }

  internal static string MissingDepsFile => Resources.GetResourceString(nameof (MissingDepsFile));

  internal static string FormatMissingDepsFile(object p0, object p1)
  {
    return string.Format((IFormatProvider) Resources.Culture, Resources.GetResourceString("MissingDepsFile"), p0, p1);
  }
}