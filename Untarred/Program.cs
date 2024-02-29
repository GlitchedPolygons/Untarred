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
        
        await using FileStream inputFileStream = new(arg, FileMode.Open);
        await using GZipStream gzStream = new GZipStream(inputFileStream, CompressionMode.Decompress);
        await using TarReader tarReader = new TarReader(gzStream);

        bool outputDirectoryCreated = false;
        
        for (;;)
        {
            TarEntry? tarEntry = await tarReader.GetNextEntryAsync();

            if (tarEntry is null)
            {
                break;
            }

            if (!outputDirectoryCreated)
            {
                outputDirectoryCreated = true;
                Directory.CreateDirectory(outputDirectory);
            }

            switch (tarEntry.EntryType)
            {
                case TarEntryType.Directory:
                case TarEntryType.DirectoryList:
                {
                    Directory.CreateDirectory(Path.Combine(outputDirectory, tarEntry.Name));
                    break;
                }
                case TarEntryType.HardLink:
                case TarEntryType.SymbolicLink:
                case TarEntryType.SparseFile:
                case TarEntryType.RegularFile:
                case TarEntryType.V7RegularFile:
                case TarEntryType.ContiguousFile:
                {
                    await tarEntry.ExtractToFileAsync(Path.Combine(outputDirectory, tarEntry.Name), true);
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