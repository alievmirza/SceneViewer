using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
  [Serializable]
  public abstract class IToolTipHandler
  {
    public string SceneLocation { get; set; }

    public abstract Button GetTooltipButton(Image image, Grid grid, List<Button> buttons, List<ToolTipHandler> handlers);

    public delegate void MyResponse(Scene scene, string path);

    public abstract void RegisterResponse(Button button, MyResponse response);

    public abstract IToolTipHandler CreateCopy(string path = null);
  }
}
