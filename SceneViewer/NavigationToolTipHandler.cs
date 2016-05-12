using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
  [Serializable]
  public class NavigationToolTipHandler : ToolTipHandler
  {
    public override Button GetTooltipButton(Image mainImage, Grid mainGrid, List<Button> buttons,
      List<ToolTipHandler> handlers)
    {
      var button = new Button
      {
        Width = mainImage.ActualWidth * RelativeSize.X,
        Height = mainImage.ActualHeight * RelativeSize.Y,
        Opacity = 0.3,
        VerticalAlignment = VerticalAlignment.Top,
        HorizontalAlignment = HorizontalAlignment.Left
      };

      var imagePosition = mainImage.TransformToAncestor(mainGrid).Transform(new Point(0, 0));

      button.Margin = new Thickness(imagePosition.X + mainImage.ActualWidth * RelativeLocation.X,
        imagePosition.Y + mainImage.ActualHeight * RelativeLocation.Y, 0, 0);

      var scene = Scene.GetSceneByPath(SceneLocation);

      ToolTipService.SetInitialShowDelay(button, 0);
      ToolTipService.SetShowDuration(button, 300000);

      button.ToolTip = scene != null ? scene.Header : "";

      return button;
    }

    public override void RegisterResponse(Button button, MyResponse response)
    {
      var scene = Scene.GetSceneByPath(SceneLocation);

      if (scene != null)
        button.Click += (sender, args) =>
        {
          var absoluteScenePath = PathHelper.CombinePaths(
          MainWindow.CurrentSceneLocation, SceneLocation);
          response(scene, absoluteScenePath);
        };
    }

    public override IToolTipHandler CreateCopy(string currentSceneLocation = null)
    {
      var handler = new NavigationToolTipHandler();
      handler.RelativeLocation = RelativeLocation;
      handler.RelativeSize = RelativeSize;
      if (currentSceneLocation == null)
      {
        handler.SceneLocation = SceneLocation;
      }
      else
      {
        var nextSceneUri = new Uri(PathHelper.CombinePaths(MainWindow.CurrentSceneLocation, SceneLocation));
        var referenceUri = new Uri(currentSceneLocation);
        handler.SceneLocation = referenceUri.MakeRelativeUri(nextSceneUri).ToString();
      }

      return handler;
    }
  }

  public class NavigationToolTipHandlerFactory : ToolTipHandler.ToolTipHandlerFactory
  {
    public override ToolTipHandler GetTooltipHandler(string sceneLocation, Point relativeLocation, Point relativeSize)
    {
      return new NavigationToolTipHandler()
      {
        RelativeLocation = relativeLocation,
        RelativeSize = relativeSize,
        SceneLocation = sceneLocation
      };
    }
  }
}
