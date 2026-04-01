using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp4
{
    public partial class MainWindow : Window
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputBox.Text))
                return;

            tasks.Add(new TaskItem
            {
                Description = InputBox.Text,
                IsCompleted = false
            });

            InputBox.Text = "";
            Refresh();
        }

        private void Delete_Click(object sender, MouseButtonEventArgs e)
        {
            var task = (sender as TextBlock)?.DataContext as TaskItem;

            if (task != null)
            {
                tasks.Remove(task);
                Refresh();
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void SearchChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (TaskList == null) return;

            IEnumerable<TaskItem> result = tasks;

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                result = result.Where(t =>
                    t.Description.ToLower().Contains(SearchBox.Text.ToLower()));
            }

            var filter = (FilterBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (filter == "Активные")
                result = result.Where(t => !t.IsCompleted);

            if (filter == "Выполненные")
                result = result.Where(t => t.IsCompleted);

            result = result.OrderBy(t => t.IsCompleted);

            TaskList.ItemsSource = result.ToList();

            UpdateCounter();
        }

        private void UpdateCounter()
        {
            int total = tasks.Count;
            int done = tasks.Count(t => t.IsCompleted);
            int left = total - done;

            CounterText.Text = $"Всего: {total} | Выполнено: {done} | Осталось: {left}";
        }
    }
}
