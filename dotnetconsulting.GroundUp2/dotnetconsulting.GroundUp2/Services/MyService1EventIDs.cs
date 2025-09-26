using Microsoft.Extensions.Logging;

namespace dotnetconsulting.GroundUp2.Services;

internal static class MyService1EventIDs
{
    public const int BaseID = 10000;

    public static readonly EventId Event1 = new(BaseID + 1, "");
    public static readonly EventId Event2 = new(BaseID + 2);
}