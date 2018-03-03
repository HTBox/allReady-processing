using System;

namespace AllReady.Processing
{
    public static class Configuration
    {
        public static string GetEnvironmentVariable(string key, string defaultValue = "") =>
            Environment.GetEnvironmentVariable(key) ?? defaultValue;

        public static int GetEnvironmentVariableAsInt(string key, int defaultValue = 0) =>
            int.Parse(GetEnvironmentVariable(key, defaultValue.ToString()));
    }
}