using Infrastructure.Extractors.Interfaces;
using System.Diagnostics;
using System.IO.Compression;

namespace Infrastructure.Extractors;

public sealed class FfmpegFrameExtractor : IFfmpegFrameExtractor
{
    public async Task GenerateZipAsync(
        string videoUrl,
        Stream output,
        CancellationToken ct)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments =
                    $"-i \"{videoUrl}\" -vf fps=1/3 -f image2pipe -vcodec mjpeg -",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        try
        {
            process.Start();
        }
        catch (Exception ex) when (ex is System.ComponentModel.Win32Exception or FileNotFoundException)
        {
            throw new InvalidOperationException(
                "FFmpeg not found. Make sure it is installed and available in PATH.", ex);
        }

        var stderrTask = process.StandardError.ReadToEndAsync(ct);

        using var zip = new ZipArchive(output, ZipArchiveMode.Create, true);

        var stdout = process.StandardOutput.BaseStream;
        var index = 0;

        while (true)
        {
            var jpeg = await ReadNextJpeg(stdout, ct);
            if (jpeg == null)
                break;

            var entry = zip.CreateEntry($"frame_{index:D5}.jpg");

            await using var entryStream = entry.Open();
            await entryStream.WriteAsync(jpeg, ct);

            index++;
        }

        await process.WaitForExitAsync(ct);

        if (process.ExitCode != 0)
        {
            var stderr = await stderrTask;
            throw new InvalidOperationException($"FFmpeg exited with code {process.ExitCode}: {stderr}");
        }
    }

    private static async Task<byte[]?> ReadNextJpeg(
        Stream stream,
        CancellationToken ct)
    {
        using var ms = new MemoryStream();

        bool started = false;
        int prev = -1;

        while (true)
        {
            int b = stream.ReadByte();

            if (b == -1)
                return ms.Length > 0 ? ms.ToArray() : null;

            if (!started)
            {
                if (prev == 0xFF && b == 0xD8)
                {
                    started = true;
                    ms.WriteByte(0xFF);
                    ms.WriteByte(0xD8);
                }
            }
            else
            {
                ms.WriteByte((byte)b);

                if (prev == 0xFF && b == 0xD9)
                    return ms.ToArray();
            }

            prev = b;
        }
    }
}