# 📝 gRPC-ToDoList - gRPC ToDo + WPF Client + Console Client

Демонстрационный проект на C# и .NET 8, в котором реализован gRPC-сервис управления задачами (ToDo) с поддержкой:
- CRUD-операций
- server-streaming (реактивная подписка на обновления)
- bidirectional streaming (чат)
- WPF клиента с подпиской на задачи в реальном времени
- контейнеризации через Docker

---

## 🚀 Возможности

- Добавление задач
- Отображение всех задач с помощью реактивного `stream`
- Отметка задачи как выполненной
- Обновление UI в реальном времени (через gRPC stream)
- Запуск через Docker

---

## 📦 Используемые технологии

| Компонент        | Технологии                               |
|------------------|------------------------------------------|
| Backend          | ASP.NET Core gRPC                        |
| Протокол         | Protocol Buffers (`.proto`)              |
| Клиенты          | WPF (.NET 8), Console (.NET 8)           |
| UI               | XAML (WPF)                               |
| Стриминг         | `Server-side streaming` и подписка       |
| Docker           | Dockerfile + docker-compose              |
| Транспорт        | HTTP/2 (gRPC)                            |


---

## 📁 Структура проекта

```bash
/
├── ToDoListApp/                # gRPC-сервер (ASP.NET Core)
│   ├── Services/ToDoService.cs # Реализация сервиса
│   ├── Protos/todo.proto       # Протокол сообщений и сервисов
│   └── Program.cs              # Настройка хоста
├── ToDoListWpfClient/          # WPF-клиент
│   └── MainWindow.xaml(.cs)    # UI и логика клиента
├── gRPCTodoClient/             # Консольный клиент
│   ├── Protos/todo.proto       # Протокол сообщений и сервисов
│   └── Program.cs
└── README.md

---

## 📦 Установка и запуск

### 1. Клонировать репозиторий

```bash
git clone https://github.com/your-username/grpc-todo-app.git
cd grpc-todo-app


### 2. Запустить сервер

```bash
cd ToDoListApp
dotnet run


### 3. Запустить WPF-клиент
```bash
cd ToDoListWpfClient
dotnet run

## 🧵 gRPC Streaming

    - Клиент подключается к StreamTodos.

    - Сервер сразу отправляет все текущие задачи.

    - При добавлении/обновлении задач — подписчики мгновенно получают обновление.

    - Используется IServerStreamWriter<ToDoItem> и хранилище подписчиков на сервере.

## 🙋‍♀️ Автор
Ghalia Dahech