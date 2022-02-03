﻿using System.Collections.Generic;
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

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();

            // Load up all the Tasks from the data
            string[] ListboxData = File.ReadAllLines(ListboxDataPath);
            foreach (string task in ListboxData)
            {
                string[] task_elements = task.Split("\" \"");

                AddTaskToListbox(task_elements[0].Remove(0, 1), task_elements[1], task_elements[2].Remove(task_elements[2].Length - 1, 1));
            }
        }

        private void Add_Task_Button_Click(object sender, RoutedEventArgs e)
        {
            string task_text = Task_TextBox.Text.Trim();
            string description_text = Description_TextBox.Text.Trim();
            string due_date = Due_Date_DatePicker.SelectedDate.HasValue ? Due_Date_DatePicker.SelectedDate.Value.Date.ToShortDateString() : "false";

            if (!IsNullOrWhiteSpace(task_text) && !IsNullOrWhiteSpace(description_text) && due_date != "false")
            {

                if (StackPanel.Children[0] is TextBlock textBlock && textBlock.Name == "ErrorMessage_TextBlock")
                { StackPanel.Children.RemoveAt(0); }

                if (ContainsQuotes(task_text) || ContainsQuotes(description_text))
                {
                    TextBlock ErrorMessage = new TextBlock();
                    ErrorMessage.Name = "ErrorMessage_TextBlock";
                    ErrorMessage.Text = "Please do not use double quotes.";
                    ErrorMessage.Foreground = Brushes.Red;
                    ErrorMessage.Margin = new Thickness(25, 0, 0, 0);
                    StackPanel.Children.Insert(0, ErrorMessage);
                    return;
                }

                AddTaskToListbox(task_text, description_text, due_date);
                Task_TextBox.Clear();
                Description_TextBox.Clear();
                Save_Task(ListboxDataPath, task_text, description_text, due_date);
            }
        }

        bool ContainsQuotes(string _string) { return _string.Contains("\""); }

        bool IsNullOrWhiteSpace(string _string) { return string.IsNullOrWhiteSpace(_string); }

        void AddTaskToListbox(string task, string description, string due_date)
        {
            Tasks_ListBox.Items.Add(new NewTask()
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var SelectedIndex = Tasks_ListBox.SelectedIndex;
            if (Tasks_ListBox.SelectedIndex == -1) return;
            Tasks_ListBox.Items.RemoveAt(SelectedIndex);

            List<string> ListboxData = File.ReadAllLines(ListboxDataPath).ToList();
            ListboxData.RemoveAt(SelectedIndex);
            File.WriteAllLines(ListboxDataPath, ListboxData);
        }

    }
    public class NewTask
    {
        public string Task { get; set; }
        public string Description { get; set; }
        public string Due_Date { get; set; }

    }


}
