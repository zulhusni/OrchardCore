using System;
using System.Threading.Tasks;
using OrchardCore.Environment.Shell.Scope;

namespace OrchardCore.Environment.Shell
{
    public static class ShellHostExtensions
    {
        /// <summary>
        /// Creates a standalone service scope that can be used to resolve local services.
        /// </summary>
        public static Task<ShellScope> GetScopeAsync(this IShellHost shellHost, string tenant)
        {
            return shellHost.GetScopeAsync(shellHost.GetSettings(tenant));
        }

        /// <summary>
        /// Reloads all shells so that their settings will be reloaded before creating new ones
        /// for subsequent requests, while existing requests get flushed.
        /// </summary>
        public async static Task ReloadAllShellContextsAsync(this IShellHost shellHost)
        {
            foreach (var setting in shellHost.GetAllSettings())
            {
                await shellHost.ReloadShellContextAsync(setting);
            }
        }

        /// <summary>
        /// Releases all shells so that new ones will be created for subsequent requests.
        /// Note: Can be used to free up resources after a given period of inactivity.
        /// </summary>
        public async static Task ReleaseAllShellContextsAsync(this IShellHost shellHost)
        {
            foreach (var setting in shellHost.GetAllSettings())
            {
                await shellHost.ReleaseShellContextAsync(setting);
            }
        }

        /// <summary>
        /// Retrieves the shell settings associated with the specified tenant.
        /// </summary>
        public static ShellSettings GetSettings(this IShellHost shellHost, string tenant)
        {
            if (!shellHost.TryGetSettings(tenant, out var settings))
            {
                throw new ArgumentException("The specified tenant name is not valid.", nameof(tenant));
            }

            return settings;
        }
    }
}
