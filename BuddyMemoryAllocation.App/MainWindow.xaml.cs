using System;
using System.Collections.ObjectModel;
using System.Windows;
using BuddyMemoryAllocation.Model;
using BuddyMemoryAllocation.Model.Contract;
using BuddyMemoryAllocation.Storage;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace BuddyMemoryAllocation.App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _operationMemory = new OperationMemory(1024, 64, Units.Kb);
            _managerMemory = new Bma(_operationMemory);
            _visualizator = new VisualizationMemory(Canvas, _operationMemory);
            _listProcess = new ObservableCollection<Process>();
            dgListProccess.ItemsSource = _listProcess;
        }

        private ObservableCollection<Process> _listProcess;
        private IStorage<Process> _storage;
        private readonly OperationMemory _operationMemory;
        private readonly IMemoryManager _managerMemory;
        private readonly VisualizationMemory _visualizator;

        private void btnOkConfigMemory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAllocate_Click(object sender, RoutedEventArgs e)
        {
            if (tbNameNewProccess.Text == String.Empty || nUpDownSizeProccess == null) return;
            var nameProcess = tbNameNewProccess.Text;
            if (nUpDownSizeProccess.Value != null)
            {
                var sizeProcess = (int) nUpDownSizeProccess.Value;
                var process = new Process(nameProcess, sizeProcess);
                try
                {
                    _managerMemory.Allocate(process);
                    _listProcess.Add(process);
                }
                catch (OutOfMemoryException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            _visualizator.RefreshAsync();
        }

        private void btnDeallocate_Click(object sender, RoutedEventArgs e)
        {
            var selectProcess = (Process) dgListProccess.SelectedValue;
            if (selectProcess == null) return;
            _listProcess.Remove(selectProcess);
            _managerMemory.Deallocate(selectProcess.Id);
            _visualizator.RefreshAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _visualizator.RefreshAsync();
        }

        private void btnSaveList_Click(object sender, RoutedEventArgs e)
        {
            var pathFile = SaveFile();
            _storage = new StorageFileXml<Process>();
            _storage.Write(_listProcess,pathFile);
        }

        private void btnLoadList_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = OpenFile();
            _storage = new StorageFileXml<Process>();
            _listProcess = new ObservableCollection<Process>(_storage.Read(pathFile));
            foreach (var item in _listProcess)
            {
                _managerMemory.Allocate(item);
            }
            dgListProccess.ItemsSource = _listProcess;
            _visualizator.RefreshAsync();
        }

        /// Выбор файла с данными 
        private string OpenFile()
        {
            var fname = String.Empty;
            var myDialog = new OpenFileDialog
            {
                Filter = "txt files(*.txt)|*.txt|xml files(*.xml)|*.xml|All files(*.*)|*.*",
                FilterIndex = 2,
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true
            };
            //Проверка существования файла с заданным именем
            if (myDialog.ShowDialog() == true)
            {
                fname = myDialog.FileName;
            }
            return fname;
        }

        private string SaveFile()
        {
            var pathFile = String.Empty;
            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
                AddExtension = true,
                CheckPathExists = true,
                OverwritePrompt = true
            };

            while(saveFileDialog1.ShowDialog() == true)
            {
                pathFile = saveFileDialog1.FileName;
                if (pathFile.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1) break;  
                    MessageBox.Show("Error!!! Filename is wrong");
            }
            return pathFile;
        }
    }
}
