using Grpc.Core;
using Grpc.Net.Client;
using ToDoListApp;

var channel = GrpcChannel.ForAddress("https://localhost:7094");
var client = new ToDo.ToDoClient(channel);

_ = Task.Run(async () =>
{
    var streamingCall = client.StreamTodos(new GetAllToDosRequest());
    await foreach (var item in streamingCall.ResponseStream.ReadAllAsync())
    {
        var status = item.IsDone ? "[+]" : "[ ]";
        Console.WriteLine($"{item.Id}. {item.Title} {status}");
    }
});

Console.WriteLine("Введите команды: add <текст> / done <id> / exit");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input)) continue;

    if (input.StartsWith("add ", StringComparison.OrdinalIgnoreCase))
    {
        var title = input.Substring(4).Trim();
        if (!string.IsNullOrEmpty(title))
        {
            var response = await client.AddToDoAsync(new AddToDoRequest { Title = title });
            Console.WriteLine($"Добавлено {response.Item.Id}: {response.Item.Title}");
        }
    }
    else if (input.StartsWith("done ", StringComparison.OrdinalIgnoreCase))
    {
        if (int.TryParse(input.Substring(5), out int id))
        {
            var result = await client.MarkDoneAsync(new MarkDoneRequest { Id = id });
            Console.WriteLine(result.Success ? "✓ Задача отмечена как выполненная." : "! Задача не найдена.");
        }
        else
        {
            Console.WriteLine("Введите корректный ID.");
        }
    }
    else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
    else
    {
        Console.WriteLine("Неизвестная команда. Используй: add / done / exit");
    }
}