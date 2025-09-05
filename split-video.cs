using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        string ffmpegPath = @"C:\Tools\ffmpeg\bin\ffmpeg.exe"; // đường dẫn ffmpeg
        string inputFile = @"C:\Videos\input.mp4";
        string outputFolder = @"C:\Videos\Output";
        string outputPrefix = "segment";

        // Tạo playlist HLS
        string playlistFile = System.IO.Path.Combine(outputFolder, "playlist.m3u8");

        // Chuẩn hóa output folder
        System.IO.Directory.CreateDirectory(outputFolder);

        // FFmpeg arguments
        // -hls_time 10 → mỗi segment 10 giây
        // -hls_playlist_type vod → tạo playlist cho video on-demand.
        string args = $"-i \"{inputFile}\" -c:v libx264 -c:a aac -f hls -hls_time 10 -hls_playlist_type vod \"{playlistFile}\"";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        Console.WriteLine("Video đã được cắt thành segment TS và tạo playlist HLS!");
    }
}
