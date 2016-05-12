using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace WpfApplication1
{
  [Serializable]
  public class DirectionToolTipHandler : IToolTipHandler
  {
    public override Button GetTooltipButton(Image image, Grid grid, List<Button> buttons, List<ToolTipHandler> handlers)
    {
      return null;
    }

    public override void RegisterResponse(Button button, MyResponse response)
    {
    }

    public override IToolTipHandler CreateCopy(string sceneLocation = null)
    {
      var handler = new DirectionToolTipHandler();

      if (sceneLocation == null)
      {
        handler.SceneLocation = SceneLocation;
      }
      else
      {
        var nextSceneUri = new Uri(PathHelper.CombinePaths(MainWindow.CurrentSceneLocation, SceneLocation));
        var referenceUri = new Uri(sceneLocation);
        handler.SceneLocation = referenceUri.MakeRelativeUri(nextSceneUri).ToString();
      }
      return handler;
    }
  }
}
