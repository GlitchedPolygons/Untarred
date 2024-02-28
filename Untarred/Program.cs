// See https://aka.ms/new-console-template for more information

using System.Formats.Tar;
using System.IO.Compression;

if (args.Length == 0)
{
    Console.WriteLine("No files specified for decompression.");
    Environment.Exit(1);
    return;
}

foreach (string arg in args)
{
    if (string.IsNullOrEmpty(arg) || !File.Exists(arg))
    {
        continue;
    }

    try
    {
        string outputDirectory = Path.GetFullPath(Path.GetFileNameWithoutExtension(arg));

        if (outputDirectory.EndsWith(".tar"))
        {
            outputDirectory = outputDirectory[..^4];
        }

        if (Directory.Exists(outputDirectory))
        {
            outputDirectory = $"{outputDirectory}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
        }

        if (Directory.Exists(outputDirectory))
        {
            outputDirectory = $"{outputDirectory}_{Guid.NewGuid():N}";
        }

        Directory.CreateDirectory(outputDirectory);
        
        await using FileStream inputFileStream = new(arg, FileMode.Open);
        await using GZipStream gzStream = new GZipStream(inputFileStream, CompressionMode.Decompress);
        await using TarReader tarReader = new TarReader(gzStream);

        for (;;)
        {
            TarEntry? tarEntry = await tarReader.GetNextEntryAsync();

            if (tarEntry is null)
            {
                break;
            }

            switch (tarEntry.EntryType)
            {
                case TarEntryType.Directory:
                {
                    // TODO
                    break;
                }
                case TarEntryType.RegularFile:
                {
                    // TODO
                    break;
                }
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to decompress \"{arg}\"! Thrown exception: {e.ToString()} - please ensure it is a valid .tar.gz archive!");
    }
}