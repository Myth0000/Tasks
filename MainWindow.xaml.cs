using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

    }
    public class NewTask
    {
        public string Task { get; set; }
        public string Description { get; set; }
        public string Due_Date { get; set; }

    }


}
