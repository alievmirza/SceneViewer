using System;
using System.Collections.Generic;
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

      return button;
    }

    public override void RegisterResponse(Button button, MyResponse response)
    {
      var scene = Scene.GetSceneByPath(SceneLocation);

      if (scene != null)
        button.Click += (sender, args) => response(scene);
    }
  }

  public class NavigationToolTipHandlerFactory : ToolTipHandlerFactory
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
