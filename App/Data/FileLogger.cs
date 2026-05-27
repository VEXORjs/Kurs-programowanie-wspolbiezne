using App.Data;
using System.Collections.Concurrent;
using System.IO;

public sealed class FileLogger : ILogger, IDisposable
{
    private readonly BlockingCollection<string> _queue =
        new BlockingCollection<string>();

    private readonly Task _worker;

    public FileLogger(string path = "diagnostics.log")
    {
        path = Path.Combine(
            Directory.GetCurrentDirectory(),
            path);
        //programowanie wspolbiezne(nowy worker)
        _worker = Task.Run(async () =>
        {
            //message logger
            using var writer = new StreamWriter(path, append: true);

            foreach (var message in _queue.GetConsumingEnumerable())
            {
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
        });
    }

    public void Log(string message)
    {
        if (!_queue.IsAddingCompleted)
        {
            _queue.Add($"{DateTime.Now:HH:mm:ss.fff} {message}");
        }
    }

    public void Dispose()
    {
        _queue.CompleteAdding();
        _worker.Wait();
        _queue.Dispose();
    }
}
