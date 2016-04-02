using System;
using System.Collections.Generic;
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
  }
}
