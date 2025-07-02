using Grpc.Net.Client;
using ToDoListApp;
using ToDoListApp;

var channel = GrpcChannel.ForAddress("https://localhost:7094");
var client = new ToDo.ToDoClient(channel);

Console.WriteLine("Введите задачу");
var title = Console.ReadLine();

var addResponse = await client.AddToDoAsync(new AddToDoRequest { Title = title });
Console.WriteLine($"Добавлено: {addResponse.Item.Id}: {addResponse.Item.Title}");

var list = await client.GetAllToDosAsync(new GetAllToDosRequest());
Console.WriteLine("Список задач:");
foreach (var item in list.Items)
{
    var status = item.IsDone ? "[✓]" : "[ ]";
    Console.WriteLine($"{item.Id}. {item.Title}: {status}");
}

Console.WriteLine("Ввудите ID задачи, чтобы отметить выполненной: ");
if (int.TryParse(Console.ReadLine(), out int idToMark))
{
    var result = await client.MarkDoneAsync(new MarkDoneRequest { Id = idToMark });
    Console.WriteLine(result.Success ? "Успешно!" : "Не найдено.");
}

Console.ReadLine();