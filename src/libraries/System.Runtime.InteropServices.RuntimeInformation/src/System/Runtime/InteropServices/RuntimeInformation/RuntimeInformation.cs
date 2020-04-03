// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Runtime.InteropServices
{
    public static partial class RuntimeInformation
    {
        private const string FrameworkName = ".NET Core";
        private static string? s_frameworkDescription;
        private static string? s_runtimeIdentifier;

        public static string FrameworkDescription
        {
            get
            {
                if (s_frameworkDescription == null)
                {
                    string? versionString = AppContext.GetData("FX_PRODUCT_VERSION") as string;

                    if (versionString == null)
                    {
                        // Use AssemblyInformationalVersionAttribute as fallback if the exact product version is not specified by the host
                        versionString = typeof(object).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

                        // Strip the git hash if there is one
                        if (versionString != null)
                        {
                            int plusIndex = versionString.IndexOf('+');
                            if (plusIndex != -1)
                            {
                                versionString = versionString.Substring(0, plusIndex);
                            }
                        }
                    }

                    s_frameworkDescription = !string.IsNullOrWhiteSpace(versionString) ? $"{FrameworkName} {versionString}" : FrameworkName;
                }

                return s_frameworkDescription;
            }
        }

        /// <summary>
        /// Returns an opaque string that identifies the platform on which an app is running.
        /// </summary>
        /// <remarks>
        /// The property returns a string that identifies the operating system, typically including version,
        /// and processor architecture of the currently executing process.
        /// Since this string is opaque, it is not recommended to parse the string into its constituent parts.
        ///
        /// For more information, see https://docs.microsoft.com/dotnet/core/rid-catalog.
        /// </remarks>
        public static string RuntimeIdentifier =>
            s_runtimeIdentifier ??= AppContext.GetData("RUNTIME_IDENTIFIER") as string ?? "unknown";
    }
}
