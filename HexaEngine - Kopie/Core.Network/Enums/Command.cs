namespace HexaEngine.Core.Network.Enums
{
    public enum Command : int
    {
        None = 0,
        Reqest = 8,
        Auth = 16,
        Close = 32,
        Message = 64,
        Command = 128,
    }
}