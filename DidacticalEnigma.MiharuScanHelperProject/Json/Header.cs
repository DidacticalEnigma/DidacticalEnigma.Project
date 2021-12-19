namespace DidacticalEnigma.MiharuScanHelperProject.Json;

public class Header
{
    public int Page { get; }
    
    public int Version { get; }

    public Header(int page, int version)
    {
        Page = page;
        Version = version;
    }
}