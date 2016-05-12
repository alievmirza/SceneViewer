using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WpfApplication1
{
  [Serializable]
  public class Scene
  {
    public string MediaLink { get; set; }
    public string Header { get; set; }
    public string Text { get; set; }
    public List<ToolTipHandler> ToolTipHandlers { get; set; }
    public DirectionToolTipHandler LeftButtonHandler { get; set; }
    public DirectionToolTipHandler RightButtonHandler { get; set; }
    public DirectionToolTipHandler UpButtonHandler { get; set; }
    public DirectionToolTipHandler DownButtonHandler { get; set; }

    public Scene()
    {
      ToolTipHandlers = new List<ToolTipHandler>();
    }

    public static Scene GetSceneByPath(string path)
    {
      if (path.Contains(":/") || path.Contains(":\\"))
      {
        if (File.Exists(path))
        {
          FileStream fs = new FileStream(path, FileMode.Open);
          Scene scene;
          // Construct a BinaryFormatter and use it to serialize the data to the stream.
          BinaryFormatter formatter = new BinaryFormatter();
          try
          {
            scene = (Scene) formatter.Deserialize(fs);
          }
          catch (SerializationException ex)
          {
            Console.WriteLine("Failed to serialize. Reason: " + ex.Message);
            throw;
          }
          finally
          {
            fs.Close();
          }
          return scene;
        }
        return null;
      }
      else
      {
        path = PathHelper.CombinePaths(MainWindow.CurrentSceneLocation, path);
        if (File.Exists(path))
        {
          FileStream fs = new FileStream(path, FileMode.Open);
          Scene scene;
          // Construct a BinaryFormatter and use it to serialize the data to the stream.
          BinaryFormatter formatter = new BinaryFormatter();
          try
          {
            scene = (Scene)formatter.Deserialize(fs);
          }
          catch (SerializationException ex)
          {
            Console.WriteLine("Failed to serialize. Reason: " + ex.Message);
            throw;
          }
          finally
          {
            fs.Close();
          }
          return scene;
        }
        return null;
      }
    }
  }
}
