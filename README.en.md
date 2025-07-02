# 📝 gRPC-ToDoList - gRPC ToDo + WPF Client + Console Client

A demo project built with C# and .NET 8 showcasing a gRPC-based ToDo task manager with:
- CRUD operations
- Server-side streaming (live updates)
- Bidirectional streaming (chat support)
- WPF client with live task updates
- Docker containerization

---

## 🚀 Features

- Add tasks
- View tasks via reactive `stream`
- Mark tasks as done
- Real-time UI updates (via gRPC stream)
- Docker support


---

## 📦 Technologies

| Component        | Tech stack                              |
|------------------|------------------------------------------|
| Backend          | ASP.NET Core gRPC                        |
| Protocol         | Protocol Buffers (`.proto`)              |
| Clients          | WPF (.NET 8), Console (.NET 8)           |
| UI               | XAML (WPF)                               |
| Streaming        | `Server-side streaming`, `Bidirectional` |
| Docker           | Dockerfile + docker-compose              |
| Transport        | HTTP/2 (gRPC)                            |


---

## 📁 Project structure
```bash
/
├── ToDoListApp/                # gRPC server (ASP.NET Core)
│   ├── Services/ToDoService.cs # Service implementation
│   ├── Protos/todo.proto       # gRPC protocol
│   └── Program.cs              # Server entry point
├── ToDoListWpfClient/          # WPF client
│   └── MainWindow.xaml(.cs)    # UI and logic
├── gRPCTodoClient/             # Console client
│   ├── Protos/todo.proto       # gRPC protocol
│   └── Program.cs
└── README.md
```
---

## 📦 Getting Started

### 1. Clone the repo

git clone https://github.com/your-username/grpc-todo-app.git
cd grpc-todo-app


### 2. Run the server

cd ToDoListApp
dotnet run


### 3. Run the WPF client

cd ToDoListWpfClient
dotnet run

## 🧵 gRPC Streaming

    - Client connects to StreamTodos.

    - Server immediately sends the current task list.

    - On any task change, clients receive real-time updates.

    - Built with IServerStreamWriter<ToDoItem> and subscriber collection.

## 🙋‍♀️ Author
Ghalia Dahech
