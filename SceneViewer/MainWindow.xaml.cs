using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using Cursors = System.Windows.Input.Cursors;
using Image = System.Windows.Controls.Image;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Label = System.Windows.Forms.Label;
using Point = System.Windows.Point;
using TextBox = System.Windows.Forms.TextBox;
using ToolBar = System.Windows.Controls.ToolBar;

namespace WpfApplication1
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private List<Button> _buttons;
    private List<ToolTipHandler> _buttonsHandlers;

    //constants for width and height of button
    private double _buttonWidthConstant = 0.2;
    private double _buttonHeightConstant = 0.1;

    //last selected path of pictures
    public static string CurrentSceneLocation;
    private Point _startPositionOfMouseClick;
    private DirectionToolTipHandler _leftButtonHandler;
    private DirectionToolTipHandler _rightButtonHandler;
    private DirectionToolTipHandler _upButtonHandler;
    private DirectionToolTipHandler _downButtonHandler;

    private readonly NavigationToolTipHandlerFactory _navigationHandlerFactory;
    private readonly ToolTipHandlerFactory _handlerFactory;
    private ToolTipHandlerFactory _currentToolTipFactory;

    public MainWindow()
    {
      _buttons = new List<Button>();
      _buttonsHandlers = new List<ToolTipHandler>();

      InitializeComponent();

      ExitButton.Click += OnExitButtonClick;
      OpenImageButton.Click += OnOpenImageButtonCLick;
      SaveSceneButton.Click += OnSaveSceneButtonClick;
      OpenSceneButton.Click += OnOpenSceneButtonClick;
      MainToolBar.Loaded += OnToolBarLoaded; //use this to disable weird things like overflow button in TooLBar

      ApplicationMainWindow.KeyDown += OnKeyDown;

      //TODO: add custom styles for this buttons
      LeftButton.Click += OnLeftClick;
      RightButton.Click += OnRightClick;
      UpButton.Click += OnUpClick;
      DownButton.Click += OnDownClick;

      _leftButtonHandler = null;
      _rightButtonHandler = null;
      _upButtonHandler = null;
      _downButtonHandler = null;

      LeftButton.Visibility = Visibility.Hidden;
      RightButton.Visibility = Visibility.Hidden;
      UpButton.Visibility = Visibility.Hidden;
      DownButton.Visibility = Visibility.Hidden;

      MainImage.Focus();
      MainGrid.SizeChanged += OnSizeChanged; //resize buttons
      MainImage.SizeChanged += OnImageSizeChanged;

      CreateContextToolTip.Click += OnCreateContextToolTip;
      CreateNavigationToolTip.Click += OnCreateNavigationToolTip;
      CreateLeftToolTip.Click += OnCreateLeftToolTip;
      CreateRightToolTip.Click += OnCreateRightToolTip;
      CreateDownToolTip.Click += OnCreateDownToolTip;
      CreateUpToolTip.Click += OnCreateUpToolTip;

      MainImage.MouseLeftButtonDown += OnMouseLeftButtonPressed;
      MainImage.MouseLeftButtonUp += OnMouseLeftButtonReleased;

      _navigationHandlerFactory = new NavigationToolTipHandlerFactory();
      _handlerFactory = new ToolTipHandlerFactory();
      _currentToolTipFactory = null;

      OpenImage("D:\\Data\\Documents\\Visual Studio 2013\\Projects\\SceneViewer\\SceneViewer\\Data\\Example\\SceneData\\0.jpg");
    }

    private void OnCreateUpToolTip(object sender, RoutedEventArgs e)
    {
      if (CurrentSceneLocation == null)
        return;

      var openFileDialog = new OpenFileDialog
      {
        DefaultExt = ".sc",
        Filter = "Scenes (.sc)|*.sc"
      };
      // Default file extension
      // Filter files by extension
      if (CurrentSceneLocation != null && File.Exists(CurrentSceneLocation))
      {
        openFileDialog.FileName = CurrentSceneLocation;
      }

      // Display the openFile dialog.
      DialogResult result = openFileDialog.ShowDialog();
      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        var fileUri = new Uri(CurrentSceneLocation);
        var referenceUri = new Uri(openFileDialog.FileName);
        _upButtonHandler = new DirectionToolTipHandler()
        {
          SceneLocation = fileUri.MakeRelativeUri(referenceUri).ToString()
        };
        UpButton.Visibility = Visibility.Visible;
      }
    }

    private void OnCreateDownToolTip(object sender, RoutedEventArgs e)
    {
      if (CurrentSceneLocation == null)
        return;

      var openFileDialog = new OpenFileDialog
      {
        DefaultExt = ".sc",
        Filter = "Scenes (.sc)|*.sc"
      };
      // Default file extension
      // Filter files by extension
      if (CurrentSceneLocation != null && File.Exists(CurrentSceneLocation))
      {
        openFileDialog.FileName = CurrentSceneLocation;
      }

      // Display the openFile dialog.
      DialogResult result = openFileDialog.ShowDialog();
      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        var fileUri = new Uri(CurrentSceneLocation);
        var referenceUri = new Uri(openFileDialog.FileName);
        _downButtonHandler = new DirectionToolTipHandler()
        {
          SceneLocation = fileUri.MakeRelativeUri(referenceUri).ToString()
        };
        DownButton.Visibility = Visibility.Visible;
      }
    }

    private void OnCreateRightToolTip(object sender, RoutedEventArgs e)
    {
      if (CurrentSceneLocation == null)
        return;

      var openFileDialog = new OpenFileDialog
      {
        DefaultExt = ".sc",
        Filter = "Scenes (.sc)|*.sc"
      };
      // Default file extension
      // Filter files by extension
      if (CurrentSceneLocation != null && File.Exists(CurrentSceneLocation))
      {
        openFileDialog.FileName = CurrentSceneLocation;
      }

      // Display the openFile dialog.
      DialogResult result = openFileDialog.ShowDialog();
      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        var fileUri = new Uri(CurrentSceneLocation);
        var referenceUri = new Uri(openFileDialog.FileName);
        _rightButtonHandler = new DirectionToolTipHandler()
        {
          SceneLocation = fileUri.MakeRelativeUri(referenceUri).ToString()
        };
        RightButton.Visibility = Visibility.Visible;
      }
    }

    private void OnCreateLeftToolTip(object sender, RoutedEventArgs e)
    {
      if (CurrentSceneLocation == null)
        return;

      var openFileDialog = new OpenFileDialog
      {
        DefaultExt = ".sc",
        Filter = "Scenes (.sc)|*.sc"
      };
      // Default file extension
      // Filter files by extension
      if (CurrentSceneLocation != null && File.Exists(CurrentSceneLocation))
      {
        openFileDialog.FileName = CurrentSceneLocation;
      }

      // Display the openFile dialog.
      DialogResult result = openFileDialog.ShowDialog();
      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        var fileUri = new Uri(CurrentSceneLocation);
        var referenceUri = new Uri(openFileDialog.FileName);
        _leftButtonHandler = new DirectionToolTipHandler()
        {
          SceneLocation = fileUri.MakeRelativeUri(referenceUri).ToString()
        };
        LeftButton.Visibility = Visibility.Visible;
      }
    }

    private void OnCreateNavigationToolTip(object sender, RoutedEventArgs e)
    {
      _currentToolTipFactory = _navigationHandlerFactory;
    }

    private void OnCreateContextToolTip(object sender, RoutedEventArgs e)
    {
      _currentToolTipFactory = _handlerFactory;
    }

    private void OnOpenSceneButtonClick(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog();
      if (CurrentSceneLocation != null && File.Exists(CurrentSceneLocation))
      {
        openFileDialog.FileName = CurrentSceneLocation;
      }

      // Display the openFile dialog.
      DialogResult result = openFileDialog.ShowDialog();
      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        var path = openFileDialog.FileName;
        if (File.Exists(path))
        {
          Scene scene = Scene.GetSceneByPath(path);
          if (scene != null)
          {
            LoadScene(scene, path);
          }
        }
      }
    }

    private void ClearScene()
    {
      if (_buttons != null)
      {
        foreach (var button in _buttons)
        {
          MainGrid.Children.Remove(button);
        }
      }

      _buttons = new List<Button>();
      _buttonsHandlers = new List<ToolTipHandler>();

      _leftButtonHandler = null;
      _rightButtonHandler = null;
      _upButtonHandler = null;
      _downButtonHandler = null;

      LeftButton.Visibility = Visibility.Hidden;
      RightButton.Visibility = Visibility.Hidden;
      UpButton.Visibility = Visibility.Hidden;
      DownButton.Visibility = Visibility.Hidden;

      CurrentSceneLocation = null;
    }

    private void LoadScene(Scene scene, string path)
    {
      ClearScene();

      var absoluteMediaPath = PathHelper.CombinePaths(path, scene.MediaLink);

      if (!File.Exists(absoluteMediaPath) ||
          !(absoluteMediaPath.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
            || absoluteMediaPath.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
            || absoluteMediaPath.EndsWith("png", StringComparison.OrdinalIgnoreCase)
            || absoluteMediaPath.EndsWith("gif", StringComparison.OrdinalIgnoreCase)))
        return;

      CurrentSceneLocation = path;
      var image = new BitmapImage(new Uri(absoluteMediaPath));

      _leftButtonHandler = scene.LeftButtonHandler;
      LeftButton.Visibility = _leftButtonHandler == null ? Visibility.Hidden : Visibility.Visible;
      _rightButtonHandler = scene.RightButtonHandler;
      RightButton.Visibility = _rightButtonHandler == null ? Visibility.Hidden : Visibility.Visible;
      _upButtonHandler = scene.UpButtonHandler;
      UpButton.Visibility = _upButtonHandler == null ? Visibility.Hidden : Visibility.Visible;
      _downButtonHandler = scene.DownButtonHandler;
      DownButton.Visibility = _downButtonHandler == null ? Visibility.Hidden : Visibility.Visible;

      MainImage.Source = image;
      if (scene.ToolTipHandlers != null)
        foreach (var toolTipHandler in scene.ToolTipHandlers)
        {
          var button = toolTipHandler.GetTooltipButton(MainImage, MainGrid, _buttons, _buttonsHandlers);
          toolTipHandler.RegisterResponse(button, LoadScene);
          MainGrid.Children.Add(button);

          _buttons.Add(button);
          _buttonsHandlers.Add(toolTipHandler);
        }
    }

    private void OnSaveSceneButtonClick(object sender, RoutedEventArgs e)
    {
      if (MainImage.Source == null)
        return;

      // Configure save file dialog box
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
      {
        FileName = "unnamed",
        DefaultExt = ".sc",
        Filter = "Scenes (.sc)|*.sc"
      };
      // Default file name
      // Default file extension
      // Filter files by extension

      // Show save file dialog box
      var result = dlg.ShowDialog();

      // Process save file dialog box results
      if (result != true)
      {
        return;
      }
      var path = dlg.FileName;


      var sceneUri = new Uri(dlg.FileName);
      var mediaUri = ((BitmapImage) MainImage.Source).UriSource;
      var mediaRelativeUri = sceneUri.MakeRelativeUri(mediaUri);

      var scene = new Scene
      {
        MediaLink = mediaRelativeUri.ToString(),
        Header = Prompt.ShowDialog("Set description", "Set description"),
        Text = Prompt.ShowDialog("Set text", "Set text"),
        ToolTipHandlers = new List<ToolTipHandler>(),
        LeftButtonHandler =
          _leftButtonHandler != null ? _leftButtonHandler.CreateCopy(dlg.FileName) as DirectionToolTipHandler : null,
        RightButtonHandler =
          _rightButtonHandler != null ? _rightButtonHandler.CreateCopy(dlg.FileName) as DirectionToolTipHandler : null,
        UpButtonHandler =
          _upButtonHandler != null ? _upButtonHandler.CreateCopy(dlg.FileName) as DirectionToolTipHandler : null,
        DownButtonHandler =
          _downButtonHandler != null ? _downButtonHandler.CreateCopy(dlg.FileName) as DirectionToolTipHandler : null
      };

      for (var i = 0; i < _buttons.Count; i++)
      {
        var handler = _buttonsHandlers[i];

        var handlerToSave = handler.CreateCopy(dlg.FileName) as ToolTipHandler;

        if (handlerToSave == null)
          throw new InvalidCastException("handler should be ToolTipHandler");

        scene.ToolTipHandlers.Add(handlerToSave);
      }

      FileStream fs = new FileStream(path, FileMode.Create);

      // Construct a BinaryFormatter and use it to serialize the data to the stream.
      BinaryFormatter formatter = new BinaryFormatter();
      try
      {
        formatter.Serialize(fs, scene);
      }
      catch (SerializationException ex)
      {
        Console.WriteLine("Failed to serialize. Reason: " + ex.Message);
      }
      finally
      {
        fs.Close();
      }
    }

    private void OnImageSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (Math.Abs(e.PreviousSize.Height) < 0.000001 || Math.Abs(e.PreviousSize.Width) < 0.000001)
        return;

      foreach (var button in _buttons)
      {
        button.Width = button.ActualWidth*e.NewSize.Width/e.PreviousSize.Width;
        button.Height = button.ActualHeight*e.NewSize.Height/e.PreviousSize.Height;
      }
    }

    private void OnMouseLeftButtonPressed(object sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
      var image = sender as Image;
      if (image == null)
        return;

      if (_currentToolTipFactory == null)
        return;

      MainImage.Cursor = Cursors.Hand;
      _startPositionOfMouseClick = mouseButtonEventArgs.GetPosition(image);
    }

    private void OnMouseLeftButtonReleased(object sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
      var image = sender as Image;
      if (image == null)
        return;

      if (_currentToolTipFactory == null)
        return;

      MainImage.Cursor = Cursors.Arrow;
      CreateNewToolTip(image, _startPositionOfMouseClick, mouseButtonEventArgs.GetPosition(image));
      _currentToolTipFactory = null;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      var width = e.NewSize.Width;
      var height = e.NewSize.Height;
      LeftButton.Width = width*_buttonWidthConstant;
      LeftButton.Height = height*_buttonHeightConstant;
      RightButton.Width = width*_buttonWidthConstant;
      RightButton.Height = height*_buttonHeightConstant;
      UpButton.Width = width*_buttonWidthConstant;
      UpButton.Height = height*_buttonHeightConstant;
      DownButton.Width = width*_buttonWidthConstant;
      DownButton.Height = height*_buttonHeightConstant;

      var imagePosition = MainImage.TransformToAncestor(MainGrid).Transform(new Point(0, 0));

      for (var i = 0; i < _buttons.Count; i++)
      {
        var handler = _buttonsHandlers[i];
        _buttons[i].Margin = new Thickness(imagePosition.X + MainImage.ActualWidth*handler.RelativeLocation.X,
          imagePosition.Y + MainImage.ActualHeight*handler.RelativeLocation.Y, 0, 0);
      }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Left)
        OnLeftClick(sender, e);

      if (e.Key == Key.Right)
        OnRightClick(sender, e);

      if (e.Key == Key.Up)
        OnUpClick(sender, e);

      if (e.Key == Key.Down)
        OnDownClick(sender, e);

      if (e.Key == Key.Escape)
        OnExitButtonClick(sender, e);
    }

    private void OnLeftClick(object sender, RoutedEventArgs routedEventArgs)
    {
      if (_leftButtonHandler != null)
      {
        var absoluteScenePath = PathHelper.CombinePaths(CurrentSceneLocation, _leftButtonHandler.SceneLocation);
        var scene = Scene.GetSceneByPath(absoluteScenePath);
        LoadScene(scene, absoluteScenePath);
      }
    }

    private void OnRightClick(object sender, RoutedEventArgs routedEventArgs)
    {
      if (_rightButtonHandler != null)
      {
        var absoluteScenePath = PathHelper.CombinePaths(CurrentSceneLocation, _rightButtonHandler.SceneLocation);
        var scene = Scene.GetSceneByPath(absoluteScenePath);
        LoadScene(scene, absoluteScenePath);
      }
    }

    private void OnUpClick(object sender, RoutedEventArgs routedEventArgs)
    {
      if (_upButtonHandler != null)
      {
        var absoluteScenePath = PathHelper.CombinePaths(CurrentSceneLocation, _upButtonHandler.SceneLocation);
        var scene = Scene.GetSceneByPath(absoluteScenePath);
        LoadScene(scene, absoluteScenePath);
      }
    }

    private void OnDownClick(object sender, RoutedEventArgs routedEventArgs)
    {
      if (_downButtonHandler != null)
      {
        var absoluteScenePath = PathHelper.CombinePaths(CurrentSceneLocation, _downButtonHandler.SceneLocation);
        var scene = Scene.GetSceneByPath(absoluteScenePath);
        LoadScene(scene, absoluteScenePath);
      }
    }

    private void OnExitButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
      Application.Current.Shutdown();
    }

    private void OpenImage(string path)
    {
      if (!(path.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith("png", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith("gif", StringComparison.OrdinalIgnoreCase)))
        return;

      var image = new BitmapImage(new Uri(path));
      ClearScene();
      MainImage.Source = image;
      CurrentSceneLocation = path;
    }

    private void OnOpenImageButtonCLick(object sender, RoutedEventArgs routedEventArgs)
    {
      var openFileDialog = new OpenFileDialog();
      if (CurrentSceneLocation != null && File.Exists(CurrentSceneLocation))
      {
        openFileDialog.FileName = CurrentSceneLocation;
      }

      // Display the openFile dialog.
      DialogResult result = openFileDialog.ShowDialog();
      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        OpenImage(openFileDialog.FileName);
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

    private void CreateNewToolTip(Image image, Point startPosition, Point endPosition)
    {
      if (CurrentSceneLocation == null)
        return;

      var startX = Math.Min(startPosition.X, endPosition.X);
      var startY = Math.Min(startPosition.Y, endPosition.Y);
      var lengthX = Math.Abs(endPosition.X - startPosition.X);
      var lengthY = Math.Abs(endPosition.Y - startPosition.Y);

      var openFileDialog = new OpenFileDialog
      {
        DefaultExt = ".sc",
        Filter = "Scenes (.sc)|*.sc"
      };
      // Default file extension
      // Filter files by extension
      if (CurrentSceneLocation != null && File.Exists(CurrentSceneLocation))
      {
        openFileDialog.FileName = CurrentSceneLocation;
      }

      // Display the openFile dialog.
      DialogResult result = openFileDialog.ShowDialog();
      // OK button was pressed.
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        var sceneUri = new Uri(CurrentSceneLocation);
        var referenceUri = new Uri(openFileDialog.FileName);

        var tooltipHandler = _currentToolTipFactory.GetTooltipHandler(sceneUri.MakeRelativeUri(referenceUri).ToString(),
          new Point(startX/image.ActualWidth, startY/image.ActualHeight),
          new Point(lengthX/image.ActualWidth, lengthY/image.ActualHeight));
        var button = tooltipHandler.GetTooltipButton(image, MainGrid, _buttons, _buttonsHandlers);
        _buttonsHandlers.Add(tooltipHandler);
        tooltipHandler.RegisterResponse(button, LoadScene);
        _buttons.Add(button);

        MainGrid.Children.Add(button);
      }
    }
  }

  public static class Prompt
  {
    public static string ShowDialog(string text, string caption)
    {
      Form prompt = new Form()
      {
        Width = 200,
        Height = 150,
        FormBorderStyle = FormBorderStyle.FixedDialog,
        Text = caption,
        StartPosition = FormStartPosition.CenterScreen
      };
      Label textLabel = new Label() {Left = 50, Top = 20, Text = text};
      TextBox textBox = new TextBox() {Left = 50, Top = 50, Width = 400};
      System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button()
      {
        Text = "Ok",
        Left = 350,
        Width = 100,
        Top = 70,
        DialogResult = DialogResult.OK
      };
      confirmation.Click += (sender, e) => { prompt.Close(); };
      prompt.Controls.Add(textBox);
      prompt.Controls.Add(confirmation);
      prompt.Controls.Add(textLabel);
      prompt.AcceptButton = confirmation;

      return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
    }
  }

  public static class PathHelper
  {
    public static string CombinePaths(string absolutePath, string relativePath)
    {
      var folderPath = absolutePath.Substring(0, Math.Max(absolutePath.LastIndexOf('\\'), absolutePath.LastIndexOf('/')));
      return Path.Combine(folderPath, relativePath);
    }
  }
}