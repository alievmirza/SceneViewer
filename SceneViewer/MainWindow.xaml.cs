using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using Cursor = System.Windows.Input.Cursor;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using ToolBar = System.Windows.Controls.ToolBar;

namespace WpfApplication1
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    //images
    private readonly List<BitmapImage> _imageSources;

    private int _currentIndex;    //index of current image from _imageSources
    private int _totalCount;      //_imageSources.Count; why use separate variable? Because FUCK YOU, that's why

    //constants for width and height of button location
    private double _widthConstant = 0.25;
    private double _heigthConstant = 0.85;

    //constants for width and height of button
    private double _buttonWidthConstant = 0.2;
    private double _buttonHeightConstant = 0.1;

    //last position of cursor on grid
    private Point _lastPosition;
    //default cursor in application
    private readonly Cursor _defaultCursor;

    //last selected path of pictures
    private string _lastSelectedPath;

    public MainWindow()
    {
      _imageSources = new List<BitmapImage>();

      InitializeComponent();

      _defaultCursor = MainImage.Cursor; //get default cursor
      LeftButton.Click += OnLeftClick;
      RightButton.Click += OnRightClick;
      ExitButton.Click += OnExitButtonClick;
      OpenButton.Click += OnOpenButtonCLick;
      MainToolBar.Loaded += OnToolBarLoaded; //use this to disable weird things like overflow button in TooLBar

      //use it to turn scene without hotkeys
      MainImage.MouseMove += OnMouseMove;
      MainImage.MouseLeftButtonDown += OnMouseClick;
      MainImage.MouseLeftButtonUp += OnMouseReleased;
      ApplicationMainWindow.KeyDown += OnKeyDown;

      MainImage.Focus();
      MainGrid.SizeChanged += OnSizeChanged;  //resize buttons
    }

    private void OnMouseReleased(object sender, MouseEventArgs e)
    {
      MainImage.Cursor = _defaultCursor;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      var width = e.NewSize.Width;
      var height = e.NewSize.Height;
      LeftButton.Width = width*_buttonWidthConstant;
      LeftButton.Height = height*_buttonHeightConstant;
      LeftButton.Margin = new Thickness(width*_widthConstant, height*_heigthConstant, 0, 0);
      RightButton.Width = width*_buttonWidthConstant;

      RightButton.Height = height*_buttonHeightConstant;
      RightButton.Margin = new Thickness(width*(1 - _widthConstant) - RightButton.Width, height*_heigthConstant, 0, 0);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Left)
        OnLeftClick(sender, e);

      if (e.Key == Key.Right)
        OnRightClick(sender, e);
    }

    private void LoadImage(int index)
    {
      if (index < 0 || index >= _totalCount)
        return;

      MainImage.Source = _imageSources[index];
    }

    private void OnLeftClick(object sender, RoutedEventArgs routedEventArgs)
    {
      if (_totalCount == 0)
        return;

      if (_currentIndex < 1)
        _currentIndex = _totalCount - 1;
      else
        _currentIndex = (_currentIndex - 1)%_totalCount;
      LoadImage(_currentIndex);
    }

    private void OnRightClick(object sender, RoutedEventArgs routedEventArgs)
    {
      if (_totalCount == 0)
        return;

      _currentIndex = (_currentIndex + 1)%_totalCount;
      LoadImage(_currentIndex);
    }

    private void OnExitButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
      Application.Current.Shutdown();
    }

    private void OnOpenButtonCLick(object sender, RoutedEventArgs routedEventArgs)
    {
      var openFolderDialog = new FolderBrowserDialog();
      if (_lastSelectedPath != null && Directory.Exists(_lastSelectedPath))
      {
        openFolderDialog.SelectedPath = _lastSelectedPath;
      }

      // Display the openFile dialog.
      DialogResult result = openFolderDialog.ShowDialog();

      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        _lastSelectedPath = openFolderDialog.SelectedPath;

        var files = Directory.GetFiles(_lastSelectedPath).Where(file => 
          file.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
          || file.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
          || file.EndsWith("png", StringComparison.OrdinalIgnoreCase)).ToArray();

        foreach (var file in files)
        {
          var image = new BitmapImage();
          image.BeginInit();
          image.CacheOption = BitmapCacheOption.OnLoad;
          image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
          image.UriSource = new Uri(file);
          image.EndInit();
          _imageSources.Add(image);
        }
        _totalCount = _imageSources.Count;

        LoadImage(0);
      }

      // Cancel button was pressed.
      else if (result == System.Windows.Forms.DialogResult.Cancel)
      {
      }
    }

    //manually remove all overflow buttons on toolbar
    private void OnToolBarLoaded(object sender, RoutedEventArgs e)
    {
      var mainToolBar = sender as ToolBar;
      if (mainToolBar == null)
        return;

      foreach (FrameworkElement a in mainToolBar.Items)
      {
        ToolBar.SetOverflowMode(a, OverflowMode.Never);
      }
      var overflowGrid = mainToolBar.Template.FindName("OverflowGrid", mainToolBar) as FrameworkElement;

      if (overflowGrid != null)
      {
        overflowGrid.Visibility = Visibility.Hidden;
      }

      mainToolBar.Width = MainGrid.Width;
    }

    private void OnMouseClick(object sender, MouseEventArgs e)
    {
      var image = sender as Image;
      if (image == null)
        return;

      _lastPosition = e.GetPosition(this);
      var textBlock = (TextBlock)image.Resources["CursorGrabbing"];
      MainImage.Cursor = textBlock.Cursor;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Released)
      {
        MainImage.Cursor = _defaultCursor;
        return;
      }

      var position = e.GetPosition(this);
      var moveRight = position.X - _lastPosition.X > 0;

      if (moveRight)
        OnRightClick(sender, e);
      else
        OnLeftClick(sender, e);

      _lastPosition = position;
    }
  }
}