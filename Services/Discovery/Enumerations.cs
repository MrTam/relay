namespace Relay.Services.Discovery
{
    /// <summary>
    /// HDHomeRun UDP packet type
    /// </summary>
    internal enum PacketType : ushort
    {
        DiscoverRequest = 2,
        DiscoverReply = 3,
        GetSetRequest = 4,
        GetSetReply = 5,
        UpgradeRequest = 6,
        UpgradeReply = 7
    }

    /// <summary>
    /// HDHomeRun tag types
    /// </summary>
    internal enum Tag : byte
    {
        DeviceType = 0x1,
        DeviceId = 0x2,
        GetSetName = 0x3,
        GetSetValue = 0x4,
        ErrorMessage = 0x5,
        TunerCount = 0x10,
        GetSetLockKey = 0x15,
        LineupUrl = 0x27,
        StorageUrl = 0x28,
        BaseUrl = 0x2A,
        DeviceAuth = 0x2B,
        StorageId = 0x2C
    }
}