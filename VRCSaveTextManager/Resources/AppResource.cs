using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace VRCSaveTextManager.Resources
{
    /// <summary>
    /// Resouce manager.
    /// </summary>
    internal static class AppResource
    {
        /// <summary>
        /// Prefix of resource path.
        /// </summary>
        private const string ResourcePathPrefix = "VRCSaveTextManager.Resources.";
        /// <summary>
        /// Resource cache dictionary.
        /// </summary>
        private static readonly Dictionary<string, string> _textResourceDict = [];


        /// <summary>
        /// Get text resource.
        /// </summary>
        /// <param name="resouceName">Resource name.</param>
        /// <returns>Text resource</returns>
        public static string GetText(string resouceName)
        {
            if (_textResourceDict.TryGetValue(resouceName, out var text))
            {
                return text;
            }

            var asm = Assembly.GetExecutingAssembly();
            var resourcePath = ResourcePathPrefix + resouceName;
            using (var stream = asm.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new ApplicationException("Failed to load embedded resource: " + resourcePath);
                }

                using (var sr = new StreamReader(stream))
                {
                    text = sr.ReadToEnd();
                }

                _textResourceDict.Add(resouceName, text);
                return text;
            }
        }
    }
}
