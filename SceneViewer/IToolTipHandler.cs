using System;

namespace SceneViewerNamespace
{
  [Serializable]
  public abstract class IToolTipHandler
  {
    public string SceneLocation { get; set; }
  }
}
