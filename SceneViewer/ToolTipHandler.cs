using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using ToolTip = System.Windows.Controls.ToolTip;
using Label = System.Windows.Controls.Label;

namespace ArtilleryApplication
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

      var tooltip = new ToolTip
      {
        Background = Brushes.White,
        Width = mainImage.ActualWidth/2,
        Height = mainImage.ActualHeight/2,
      };

      var grid = new Grid();
      grid.Width = tooltip.Width;
      grid.Height = tooltip.Height;
      ColumnDefinition gridCol1 = new ColumnDefinition();

      RowDefinition gridRow1 = new RowDefinition();
      RowDefinition gridRow2 = new RowDefinition();
      RowDefinition gridRow3 = new RowDefinition();

      gridRow1.Height = GridLength.Auto;
      gridRow2.Height = GridLength.Auto;
      gridRow3.Height = GridLength.Auto;

      grid.RowDefinitions.Add(gridRow1);
      grid.RowDefinitions.Add(gridRow2);
      grid.RowDefinitions.Add(gridRow3);
      grid.ColumnDefinitions.Add(gridCol1);

      if (scene != null)
      {
        TextBlock headBlock = new TextBlock();
        headBlock.Text = scene.Header;
        headBlock.FontSize = 14;
        headBlock.FontWeight = FontWeights.Bold;
        headBlock.VerticalAlignment = VerticalAlignment.Top;
        Grid.SetRow(headBlock, 0);
        Grid.SetColumn(headBlock, 0);
        grid.Children.Add(headBlock);


        TextBlock textBlock = new TextBlock();
        textBlock.Text = scene.Text;
        textBlock.FontSize = 10;
        textBlock.FontWeight = FontWeights.Bold;
        textBlock.VerticalAlignment = VerticalAlignment.Stretch;
        textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
        textBlock.TextWrapping = 0;
        Grid.SetRow(textBlock, 1);
        Grid.SetColumn(textBlock, 0);
        grid.Children.Add(textBlock);

        if (scene.MediaLink != null)
        {
          var path = PathHelper.CombinePaths(Artillery.CurrentSceneLocation, scene.MediaLink);

          if (File.Exists(path) &&
              (path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
               path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
               path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
               path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)))
          {
            MediaElement mediaElement = new MediaElement()
            {
              Source = new Uri(path),
              HorizontalAlignment = HorizontalAlignment.Stretch,
              VerticalAlignment = VerticalAlignment.Stretch,
            };
            MediaTimeline mediaTimeline = new MediaTimeline()
            {
              Source = new Uri(path)
            };

            Grid.SetRow(mediaElement, 2);
            Grid.SetColumn(mediaElement, 0);
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

    public override IToolTipHandler CreateCopy(string currentSceneLocation = null)
    {
      var handler = new ToolTipHandler();
      handler.RelativeLocation = RelativeLocation;
      handler.RelativeSize = RelativeSize;
      if (currentSceneLocation == null)
      {
        handler.SceneLocation = SceneLocation;
      }
      else
      {
        var nextSceneUri = new Uri(PathHelper.CombinePaths(Artillery.CurrentSceneLocation, SceneLocation));
        var referenceUri = new Uri(currentSceneLocation);
        handler.SceneLocation = referenceUri.MakeRelativeUri(nextSceneUri).ToString();
      }

      return handler;
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
}