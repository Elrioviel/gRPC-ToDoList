using Grpc.Core;
using System.Security.Cryptography.X509Certificates;

namespace ToDoListApp.Services
{
    public class ToDoService : ToDo.ToDoBase
    {
        private static readonly List<ToDoItem> _todos = new();
        private static int _idCounter = 1;

        public override Task<AddToDoReply> AddToDo(AddToDoRequest request, ServerCallContext context)
        {
            var item = new ToDoItem
            {
                Id = _idCounter++,
                Title = request.Title,
                IsDone = false
            };
            _todos.Add(item);

            return Task.FromResult(new AddToDoReply { Item = item });
        }

        public override Task<GetAllToDosReply> GetAllToDos(GetAllToDosRequest request, ServerCallContext context)
        {
            var reply = new GetAllToDosReply();
            reply.Items.AddRange(_todos);
            return Task.FromResult(reply);
        }

        public override Task<MarkDoneReply> MarkDone(MarkDoneRequest request, ServerCallContext context)
        {
            var item = _todos.FirstOrDefault(t => t.Id == request.Id);

            if (item != null)
            {
                item.IsDone = true;
                return Task.FromResult(new MarkDoneReply { Success = true });
            }

            return Task.FromResult(new MarkDoneReply { Success = false });
        }
    }
}
