using Azure.Storage.Queues;
using System.Text;
using System.Text.Json;

namespace ABCRetail.StorageWeb.Services;

public class QueueStorageService
{
    private readonly QueueServiceClient _queueServiceClient;
    private readonly StorageOptions _options;

    public QueueStorageService(QueueServiceClient queueServiceClient, StorageOptions options)
    {
        _queueServiceClient = queueServiceClient;
        _options = options;
    }

    public QueueClient GetQueue()
    {
        var queue = _queueServiceClient.GetQueueClient(_options.OrdersQueue);
        queue.CreateIfNotExists();
        return queue;
    }

    public async Task EnqueueAsync(object payload)
    {
        var queue = GetQueue();
        var json = JsonSerializer.Serialize(payload);
        await queue.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(json)));
    }

    public IEnumerable<string> PeekMessages(int max = 10)
    {
        var queue = GetQueue();
        var msgs = queue.PeekMessages(max);
        foreach (var m in msgs.Value)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(m.MessageText));
            yield return json;
        }
    }
}