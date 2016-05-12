using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

            var tooltip = new ToolTip
            {
                Background = Brushes.White,
                Width = mainImage.ActualWidth / 2,
                Height = mainImage.ActualHeight / 2,
            };



            var grid = new Grid();
            grid.Width = tooltip.Width;
            grid.Height = tooltip.Height;
            ColumnDefinition gridCol1 = new ColumnDefinition();
            //ColumnDefinition gridCol2 = new ColumnDefinition();
            //gridCol1.Width = new GridLength(grid.Width/2);
            //gridCol2.Width = new GridLength(grid.Width/2);
            RowDefinition gridRow1 = new RowDefinition();
            RowDefinition gridRow2 = new RowDefinition();
            RowDefinition gridRow3 = new RowDefinition();

            gridRow1.Height = new GridLength(grid.Height*0.10);
            //gridRow2.Height = new GridLength(grid.Height*0.60);
            //gridRow3.Height = new GridLength(grid.Height*0.25);
            grid.RowDefinitions.Add(gridRow1);
            grid.RowDefinitions.Add(gridRow2);
            grid.RowDefinitions.Add(gridRow3);
            grid.ColumnDefinitions.Add(gridCol1);
            
            /*
            TextBlock txtBlock1 = new TextBlock();
            txtBlock1.Text = "Author Name";
            txtBlock1.FontSize = 14;
            txtBlock1.FontWeight = FontWeights.Bold;
            txtBlock1.Foreground = new SolidColorBrush(Colors.Green);
            txtBlock1.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(txtBlock1, 0);
            Grid.SetColumn(txtBlock1, 0);
            grid.Children.Add(txtBlock1);
            */





            /*if (scene != null)
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
            }*/

            
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
                   if (File.Exists(scene.MediaLink) &&
                       (scene.MediaLink.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        scene.MediaLink.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        scene.MediaLink.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        scene.MediaLink.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)))
                   {
                       MediaElement mediaElement = new MediaElement()
                       {
                           Source = new Uri(scene.MediaLink),
                           HorizontalAlignment = HorizontalAlignment.Stretch,
                           VerticalAlignment = VerticalAlignment.Stretch,
                           
                       };
                       MediaTimeline mediaTimeline = new MediaTimeline()
                       {
                           Source = new Uri(scene.MediaLink)

                       };

                        Grid.SetRow(mediaElement, 1);
                        Grid.SetColumn(mediaElement, 0);
                        Grid.SetRowSpan(mediaElement, 2);
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
