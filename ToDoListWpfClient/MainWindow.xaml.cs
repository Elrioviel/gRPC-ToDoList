using Grpc.Core;
using Grpc.Net.Client;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using ToDoListApp;

namespace ToDoListWpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ToDo.ToDoClient _client;
        private readonly ObservableCollection<ToDoItemViewModel> _items = new();

        public MainWindow()
        {
            InitializeComponent();
            TasksList.ItemsSource = _items;

            //var channel = GrpcChannel.ForAddress("https://localhost:7094");
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel = GrpcChannel.ForAddress("https://localhost:7094", new GrpcChannelOptions
            {
                HttpHandler = handler
            });

            _client = new ToDo.ToDoClient(channel);

            LoadInitialTasks();
            StartStreaming();
        }

        private async void StartStreaming()
        {
            try
            {
                var stream = _client.StreamTodos(new GetAllToDosRequest());
                await foreach (var item in stream.ResponseStream.ReadAllAsync())
                {
                    Dispatcher.Invoke(() =>
                    {
                        var vm = new ToDoItemViewModel
                        {
                            Id = item.Id,
                            Title = item.Title,
                            IsDone = item.IsDone
                        };

                        var existing = _items.FirstOrDefault(i => i.Id == vm.Id);
                        if (existing != null)
                        {
                            existing.IsDone = vm.IsDone;
                        }
                        else
                        {
                            _items.Add(vm);
                        }

                        TasksList.Items.Refresh();
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка стриминга: {ex.Message}");
            }
        }

        private async void LoadInitialTasks()
        {
            try
            {
                var response = await _client.GetAllToDosAsync(new GetAllToDosRequest());
                foreach (var item in response.Items)
                {
                    _items.Add(new ToDoItemViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        IsDone = item.IsDone
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки задач: " + ex.Message);
            }
        }

        private async void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var title = TaskInput.Text.Trim();
            if (string.IsNullOrEmpty(title)) return;

            await _client.AddToDoAsync(new AddToDoRequest { Title = title });
            TaskInput.Text = string.Empty;
        }

        private async void MarkDone_Click(object sender, RoutedEventArgs e)
        {
            if (TasksList.SelectedItem is not ToDoItemViewModel selected) return;

            var result = await _client.MarkDoneAsync(new MarkDoneRequest { Id = selected.Id });
            if (!result.Success)
            {
                MessageBox.Show("Не удалось отметить.");
            }
        }
    }
}