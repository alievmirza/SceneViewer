using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApplication1
{
  [Serializable]
  public class ToolTipHandler : IToolTipHandler
  {
    public Point RelativeLocation { get; set; }
    public Point RelativeSize { get; set; }

    //TODO: set style of tooltips
    public override Button GetTooltipButton(Image mainImage, Grid mainGrid, List<Button> buttons,
      List<ToolTipHandler> handlers)
    {
      var button = new Button
      {
        Width = mainImage.ActualWidth*RelativeSize.X,
        Height = mainImage.ActualHeight*RelativeSize.Y,
        Opacity = 0.3,
        VerticalAlignment = VerticalAlignment.Top,
        HorizontalAlignment = HorizontalAlignment.Left
      };

      var imagePosition = mainImage.TransformToAncestor(mainGrid).Transform(new Point(0, 0));

      button.Margin = new Thickness(imagePosition.X + mainImage.ActualWidth*RelativeLocation.X,
        imagePosition.Y + mainImage.ActualHeight*RelativeLocation.Y, 0, 0);

      ToolTipService.SetInitialShowDelay(button, 0);
      ToolTipService.SetShowDuration(button, 300000);
      var scene = Scene.GetSceneByPath(SceneLocation);
      var grid = new Grid();
      var tooltip = new ToolTip
      {
        Background = Brushes.White,
        Height = 100,
        Width = 100
      };

      if (scene != null)
      {
        grid.Children.Add(new TextBlock()
        {
          Text = scene.Header
        });
        grid.Children.Add(new TextBlock()
        {
          Text = scene.Text
        });
        if (scene.MediaLink != null && File.Exists(scene.MediaLink) &&
            (scene.MediaLink.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
             (scene.MediaLink.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) ||
             (scene.MediaLink.EndsWith(".png", StringComparison.OrdinalIgnoreCase))))
        {
          var image = new Image {Source = new BitmapImage(new Uri(scene.MediaLink))};

          grid.Children.Add(image);
        }
      }
      tooltip.Content = grid;
      button.ToolTip = tooltip;
      return button;
    }

    public override void RegisterResponse(Button button, MyResponse response)
    {
      
    }
  }

  public class ToolTipHandlerFactory
  {
    public virtual ToolTipHandler GetTooltipHandler(string sceneLocation, Point relativeLocation, Point relativeSize)
    {
      return new ToolTipHandler()
      {
        RelativeLocation = relativeLocation,
        RelativeSize = relativeSize,
        SceneLocation = sceneLocation
      };
    }
  }
}
