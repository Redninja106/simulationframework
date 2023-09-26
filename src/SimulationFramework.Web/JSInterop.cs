using System.Runtime.InteropServices.JavaScript;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimulationFramework.Web;

public static partial class JSInterop
{
    [JSImport("globalThis.document.getElementById")]
    public static partial JSObject GetElementById(string id);

    [JSImport("globalThis.requestAnimationFrame")]
    public static partial void RequestAnimationFrame([JSMarshalAs<Function<Number>>] Action<double> callback);

    System.Runtime.InteropServices.JavaScript.JSType
}