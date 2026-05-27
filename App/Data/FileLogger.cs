using App.Data;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public sealed class FileLogger : ILogger, IDisposable
{
    private readonly BlockingCollection<string> _queue =
        new BlockingCollection<string>();

    private readonly Task _worker;
    private int _isDisposed = 0;

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
        if (Interlocked.CompareExchange(ref _isDisposed, 0, 0) == 1)
        {
            return;
        }
        try
        {
            if (!_queue.IsAddingCompleted)
                _queue.Add($"{DateTime.Now:HH:mm:ss.fff} {message}");
        }
        catch (ObjectDisposedException) { }
    }

public void Dispose()
{
    if (Interlocked.Exchange(ref _isDisposed, 1) == 1) return;
    _queue.CompleteAdding();
    _worker.Wait();
    _queue.Dispose();
}
}
