using Grpc.Core;
using System.Collections.Concurrent;
using ToDoListApp;

namespace ToDoListApp.Services
{
    public class ToDoService : ToDo.ToDoBase
    {
        private static readonly List<ToDoItem> _todos = new();
        private static readonly List<IServerStreamWriter<ToDoItem>> _subscribers = new();
        private static int _idCounter = 1;
        private static readonly object _lock = new();

        public override Task<AddToDoReply> AddToDo(AddToDoRequest request, ServerCallContext context)
        {
            ToDoItem item;
            lock (_lock)
            {
                item = new ToDoItem
                {
                    Id = _idCounter++,
                    Title = request.Title,
                    IsDone = false
                };
                _todos.Add(item);
            }

            NotifySubscribers(item);
            return Task.FromResult(new AddToDoReply { Item = item });
        }

        public override Task<GetAllToDosReply> GetAllToDos(GetAllToDosRequest request, ServerCallContext context)
        {
            var reply = new GetAllToDosReply();
            lock (_lock)
            {
                reply.Items.AddRange(_todos);
            }
            return Task.FromResult(reply);
        }

        public override Task<MarkDoneReply> MarkDone(MarkDoneRequest request, ServerCallContext context)
        {
            ToDoItem? item = null;

            lock (_lock)
            {
                item = _todos.FirstOrDefault(t => t.Id == request.Id);
                if (item != null)
                {
                    item.IsDone = true;
                }
            }

            if (item != null)
            {
                NotifySubscribers(item);
                return Task.FromResult(new MarkDoneReply { Success = true });
            }

            return Task.FromResult(new MarkDoneReply { Success = false });
        }

        public override async Task StreamTodos(GetAllToDosRequest request, IServerStreamWriter<ToDoItem> responseStream, ServerCallContext context)
        {
            lock (_lock)
            {
                foreach (var item in _todos)
                {
                    responseStream.WriteAsync(item);
                }
            }

            lock (_lock)
            {
                _subscribers.Add(responseStream);
            }

            try
            {
                await Task.Delay(Timeout.Infinite, context.CancellationToken);
            }
            catch (TaskCanceledException)
            {
                lock (_lock)
                {
                    _subscribers.Remove(responseStream);
                }
            }
        }

        private void NotifySubscribers(ToDoItem item)
        {
            lock (_lock)
            {
                foreach (var subscriber in _subscribers.ToList())
                {
                    try
                    {
                        subscriber.WriteAsync(item);
                    }
                    catch
                    {
                        _subscribers.Remove(subscriber);
                    }
                }
            }
        }
    }
}