using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using ToolTip = System.Windows.Controls.ToolTip;

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
          Text = scene.Header,
          VerticalAlignment = VerticalAlignment.Top
        });
        grid.Children.Add(new TextBlock()
        {
          Text = scene.Text,
          VerticalAlignment = VerticalAlignment.Center
        });
        if (scene.MediaLink != null)
        {
          if (File.Exists(scene.MediaLink) &&
            (scene.MediaLink.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
             scene.MediaLink.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
             scene.MediaLink.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
             scene.MediaLink.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)))
          {
            MediaElement mediaElement = new MediaElement()
            {
              Source = new Uri(scene.MediaLink),
              VerticalAlignment = VerticalAlignment.Bottom
            };
            grid.Children.Add(mediaElement);
          }
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
