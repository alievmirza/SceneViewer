using System;
using System.Windows;

namespace SceneViewerNamespace
{
  [Serializable]
  public class ToolTipHandler : IToolTipHandler
  {
    public Point RelativeLocation { get; set; }
    public Point RelativeSize { get; set; }
  }
}
