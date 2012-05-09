using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Devweb.MapEditor.Model;
using Microsoft.Win32;

namespace Devweb.MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constants

        public const string DEFAULT_TITLE = "CrapEditor";

        #endregion

        #region Private fields

        private Map _map;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Gets the view model.
        /// </summary>
        public MapViewModel ViewModel
        {
            get
            {
                return DataContext as MapViewModel;
            }
        }

        #region File Menu command methods

        private void Command_New(object sender, ExecutedRoutedEventArgs e)
        {
            DataContext = new MapViewModel(new Map());
        }

        private void Command_CanNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel == null;
        }

        private void Command_Open(object sender, ExecutedRoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Title = "Select map file",
                    Filter = "Map files (.m)|*.m"
                };
            
            if (openDialog.ShowDialog(this) == true)
            {
                var filename = openDialog.FileName;

                try
                {
                    DataContext = new MapViewModel(FileOps.Load(filename));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error loading: " + ex.Message);
                }
            }
        }

        private void Command_CanOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DataContext == null;
        }

        private void Command_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.FilePath))
            {
                Command_SaveAs(sender, e);
                return;
            }

            try
            {
                FileOps.Save(ViewModel, ViewModel.FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error saving: " + ex.Message);
            }
        }

        private void Command_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog() { CheckPathExists = true, Title = "Enter save location.", Filter = "Map files (.m)|*.m" };
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    FileOps.Save(ViewModel, saveDialog.FileName);
                    ViewModel.Name = Path.GetFileName(saveDialog.FileName);
                    ViewModel.FilePath = saveDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error saving: " + ex.Message);
                }
            }
        }

        private void Command_CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DataContext != null;
        }

        private void Command_Close(object sender, ExecutedRoutedEventArgs e)
        {
            DataContext = null;
        }

        private void Command_CanClose(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DataContext != null;
        }

        #endregion

        #region Grid event handlers

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null || grid.Tag == null) return;

            var tile = grid.Tag as Tile;
            if (tile != null)
            {
                tile.Definition = ViewModel.CurrentTile;
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var grid = sender as Grid;
            if (grid == null || grid.Tag == null) return;

            var tile = grid.Tag as Tile;
            if (tile != null)
            {
                tile.Definition = ViewModel.CurrentTile;
            }
        }

        #endregion
    }
}
