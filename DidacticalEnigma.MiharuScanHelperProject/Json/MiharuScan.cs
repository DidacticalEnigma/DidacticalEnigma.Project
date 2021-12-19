namespace DidacticalEnigma.MiharuScanHelperProject.Json;

public class MiharuScan
{
    public Header Header { get; }
    
    public ChapterJson Data { get; }

    public MiharuScan(Header header, ChapterJson data)
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
}