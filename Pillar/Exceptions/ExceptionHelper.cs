namespace Pillar.Exceptions;

internal static class ExceptionHelper
{
    public static InitializationException FailedToInit(string library,string reason)
    {
        return new InitializationException(library, reason);
    }

    public static SdlException SdlFailed(string reason)
    {
        return new SdlException(reason);
    }
}