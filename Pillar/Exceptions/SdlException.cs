namespace Pillar.Exceptions;

public class SdlException(string msg) : Exception($"Error Message: {msg}.SDL.GetLastError:{SdlLibrary.GetLastError()}")
{
    
}