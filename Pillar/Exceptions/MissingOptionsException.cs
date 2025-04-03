namespace Pillar.Exceptions;

public class MissingOptionsException(string optionName,string searchPath) : Exception($"Missing option:{optionName} in IConfiguration: {searchPath}")
{
    
    public string OptionName { get; } = optionName;
    
    public string SearchPath { get; } = searchPath;
    
}