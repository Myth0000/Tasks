using System.Collections.Generic;
using System.Windows;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Configuration;


namespace Tasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            
        }

        private void Add_Task_Button_Click(object sender, RoutedEventArgs e)
        {
            string task_text = Task_TextBox.Text.Trim();
            string description_text = Description_TextBox.Text.Trim();
            string due_date = Due_Date_DatePicker.SelectedDate.Value.Date.ToShortDateString(); ;

            if (!string.IsNullOrWhiteSpace(task_text) && !string.IsNullOrWhiteSpace(description_text))
            {
                Tasks_ListBox.Items.Add(new NewTask()
                {
                    Task = task_text,
                    Description = description_text,
                    Due_Date = due_date
                });

                Task_TextBox.Clear();
                Description_TextBox.Clear();
            }
            List<string> task = new List<string>() { task_text, description_text, due_date };
;           SaveListBoxItems(task);
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static void SaveListBoxItems(List<string> listBoxItems)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Execute(
                    "insert into Tasks (Task, Description, Due_Date) values (@Task, @Description, @Due_Date)",
                    listBoxItems
                    );
            }
        }

    }
    public class NewTask
    {
        public string Task { get; set; }
        public string Description { get; set; }
        public string Due_Date { get; set; }

    }


}
