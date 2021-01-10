using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace StEn.FinCalcR.WinUi.Extensions
{
    public static class MethodInfoExtensions
    {
        [UsedImplicitly]
        public static async Task<T> InvokeAsync<T>(this MethodInfo methodInfo, object obj, params object[] parameters)
        {
            dynamic awaitable = methodInfo.Invoke(obj, parameters);
            await awaitable;
            return (T)awaitable.GetAwaiter().GetResult();
        }

        [UsedImplicitly]
        public static async Task InvokeAsync(this MethodInfo methodInfo, object obj, params object[] parameters)
        {
            dynamic awaitable = methodInfo.Invoke(obj, parameters);
            _ = await awaitable;
        }
    }
}
