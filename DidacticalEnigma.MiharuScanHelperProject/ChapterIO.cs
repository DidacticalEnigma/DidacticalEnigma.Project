using DidacticalEnigma.MiharuScanHelperProject.Json;
using Newtonsoft.Json;

namespace DidacticalEnigma.MiharuScanHelperProject;

public class ChapterIO
{
    private static readonly JsonSerializer serializer = new JsonSerializer();
    
    public static void Write(TextWriter writer, MiharuScan chapter)
    {
        WriteHeader(writer, chapter.Header);
        using var jsonWriter = new JsonTextWriter(writer)
        {
            CloseOutput = false
        };
        serializer.Serialize(jsonWriter, chapter.Data);
    }

    public static MiharuScan Read(TextReader reader)
    {
        var header = ReadHeader(reader);
        using var jsonReader = new JsonTextReader(reader)
        {
            CloseInput = false
        };
        var chapter = serializer.Deserialize<ChapterJson>(jsonReader) ?? throw new InvalidDataException("missing data");

        return new MiharuScan(header, chapter);
    }

    public static void WriteHeader(TextWriter writer, Header header)
    {
        if (header.Version <= 0)
        {
            throw new InvalidDataException("invalid version");
        }
        
        if (header.Version == 1)
        {
            return;
        }

        if (header.Version == 2)
        {
            writer.WriteLine(header.Page);
            return;
        }

        if (header.Version >= 5)
        {
            throw new InvalidDataException("version not supported");
        }
        
        writer.Write('v');
        writer.WriteLine(header.Version);
        writer.WriteLine(header.Page);
    }
    
    public static Header ReadHeader(TextReader reader)
    {
        if (reader.Peek() == '{')
        {
            return new Header(0, 1);
        }

        var line = reader.ReadLine();
        if (line == null)
        {
            throw new InvalidDataException("unexpected end of file");
        }

        if (!line.StartsWith("v"))
        {
            if (int.TryParse(line, out int page))
            {
                return new Header(page, 2);
            }
            else
            {
                throw new InvalidDataException("invalid header");
            }
        }
        else
        {
            bool versionParseSuccess = int.TryParse(line.Substring(1), out int version);
            if (!versionParseSuccess)
            {
                throw new InvalidDataException("invalid header");
            }

            if (version >= 5)
            {
                throw new InvalidDataException("version not supported");
            }
            
            var pageLine = reader.ReadLine();
            if (pageLine == null)
            {
                throw new InvalidDataException("unexpected end of file");
            }
            
            if (int.TryParse(pageLine, out int page))
            {
                return new Header(page, version);
            }
            else
            {
                throw new InvalidDataException("invalid header");
            }
        }
    }
}