using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace Tasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ListboxDataPath = @"C:\Users\infin\source\repos\Tasks\ListboxData.txt";
        string ArchivedListboxDataPath = @"C:\Users\infin\source\repos\Tasks\ArchivedListboxData.txt";

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();

            LoadAllListboxData(ListboxDataPath, Tasks_ListBox);
            LoadAllListboxData(ArchivedListboxDataPath, ArchivedTasks_ListBox);

        }

        void LoadAllListboxData(string path, ListBox listBox)
        {
            string[] ListboxData = File.ReadAllLines(path);
            foreach (string task in ListboxData)
            {
                string[] task_elements = task.Split("\" \"");

                AddTaskToListbox(listBox, task_elements[0].Remove(0, 1), task_elements[1], task_elements[2].Remove(task_elements[2].Length - 1, 1));
            }
        }

        private void Add_Task_Button_Click(object sender, RoutedEventArgs e)
        {
            string task_text = Task_TextBox.Text.Trim();
            string description_text = Description_TextBox.Text.Trim();
            string due_date = Due_Date_DatePicker.SelectedDate.HasValue ? Due_Date_DatePicker.SelectedDate.Value.Date.ToShortDateString() : "false";

            if (!IsNullOrWhiteSpace(task_text) && !IsNullOrWhiteSpace(description_text) && due_date != "false")
            {

                if (TaskInfoStackPanel.Children[0] is TextBlock textBlock && textBlock.Name == "ErrorMessage_TextBlock")
                { TaskInfoStackPanel.Children.RemoveAt(0); }

                if (ContainsQuotes(task_text) || ContainsQuotes(description_text))
                {
                    TextBlock ErrorMessage = new TextBlock();
                    ErrorMessage.Name = "ErrorMessage_TextBlock";
                    ErrorMessage.Text = "Please do not use double quotes.";
                    ErrorMessage.Foreground = Brushes.Red;
                    ErrorMessage.Margin = new Thickness(25, 0, 0, 0);
                    TaskInfoStackPanel.Children.Insert(0, ErrorMessage);
                    return;
                }

                AddTaskToListbox(Tasks_ListBox, task_text, description_text, due_date);
                Task_TextBox.Clear();
                Description_TextBox.Clear();
                Save_Task(ListboxDataPath, task_text, description_text, due_date);
            }
        }
        
        bool ContainsQuotes(string _string) { return _string.Contains("\""); }

        bool IsNullOrWhiteSpace(string _string) { return string.IsNullOrWhiteSpace(_string); }

        void AddTaskToListbox(ListBox listBox, string task, string description, string due_date)
        {
            listBox.Items.Add(new NewTask()
            {
                Task = task,
                Description = description,
                Due_Date = due_date
            });
        }

        private void Save_Task(string path, string task, string description, string due_date)
        {
            using StreamWriter file = new(path, append: true);
            file.WriteLine($"\"{task}\" \"{description}\" \"{due_date}\"");
        }

        private void TasksDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteItem_Button_ContextMenu(ListboxDataPath, Tasks_ListBox);
        }

        private void ArchivedTasksDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteItem_Button_ContextMenu(ArchivedListboxDataPath, ArchivedTasks_ListBox);
        }

        void DeleteItem_Button_ContextMenu(string path, ListBox listBox)
        {
            var SelectedIndex = listBox.SelectedIndex;
            if (listBox.SelectedIndex == -1) return;
            listBox.Items.RemoveAt(SelectedIndex);

            List<string> ListboxData = File.ReadAllLines(path).ToList();
            ListboxData.RemoveAt(SelectedIndex);
            File.WriteAllLines(path, ListboxData);
        }

        private void ToggleTasksAndArchivedTasks_Button(object sender, RoutedEventArgs e) 
        {
            var Collapsed = Visibility.Collapsed;
            var Visible = Visibility.Visible;
            Tasks_ListBox.Visibility = Tasks_ListBox.Visibility == Collapsed ? Visible : Collapsed;
            ArchivedTasks_ListBox.Visibility = Tasks_ListBox.Visibility == Collapsed ? Visible : Collapsed;
            ToggleTasksAndArchivedTasksInfo.Text = Tasks_ListBox.Visibility == Collapsed ? "Archived Tasks" : "Tasks";
        }

        private void CompleteItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = Tasks_ListBox.SelectedIndex;
            string[] listboxData = File.ReadAllLines(ListboxDataPath);
            string[] itemData = listboxData[selectedIndex].Split("\" \"");

            // The Remove() method removes the extra double quotes that get added to the strings
            var task = itemData[0].Remove(0, 1);
            var due_date = itemData[2].Remove(itemData[2].Length - 1, 1)
                ;
            Save_Task(ArchivedListboxDataPath, task, itemData[1], due_date);
            AddTaskToListbox(ArchivedTasks_ListBox, task, itemData[1], due_date);
            DeleteItem_Button_ContextMenu(ListboxDataPath, Tasks_ListBox);
        }
      
    }

    public class NewTask
    {
        public string Task { get; set; }
        public string Description { get; set; }
        public string Due_Date { get; set; }

    }


}
