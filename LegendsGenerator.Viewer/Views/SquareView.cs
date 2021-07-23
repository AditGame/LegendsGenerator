// -------------------------------------------------------------------------------------------------
// <copyright file="SquareView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// A view into an World Square.
    /// </summary>
    public class SquareView : INotifyPropertyChanged
    {
        /// <summary>
        /// Mapping of definition to color.
        /// </summary>
        private static readonly IReadOnlyDictionary<string, Brush> ColorMap = new Dictionary<string, Brush>()
        {
            { "Ocean", Brushes.DarkBlue },
            { "Lake", Brushes.Blue },
            { "Mountain", Brushes.DarkSlateGray },
            { "Mountain Heights", Brushes.SlateGray },
            { "Mountain Peeks", Brushes.LightSlateGray },
            { "Sand Desert", Brushes.Goldenrod },
            { "Flat Rocky Wasteland", Brushes.RosyBrown },
            { "Hilly Rocky Wasteland", Brushes.RosyBrown },
            { "Badlands", Brushes.IndianRed },
            { "Flat Grasslands", Brushes.PaleGoldenrod },
            { "Hilly Grasslands", Brushes.PaleGoldenrod },
            { "Flat Savanna", Brushes.Green },
            { "Hilly Savanna", Brushes.Green },
            { "Marsh", Brushes.SeaGreen },
            { "Flat Shrubland", Brushes.OliveDrab },
            { "Hilly Shrubland", Brushes.Olive },
            { "Swamp", Brushes.DarkSeaGreen },
            { "Conifer Forest", Brushes.ForestGreen },
            { "Broadleft Forest", Brushes.DarkGreen },
        };

        /// <summary>
        /// Mapping of definition to image.
        /// </summary>
        private static readonly IReadOnlyDictionary<string, Brush> ImageMap = new Dictionary<string, Brush>()
        {
            { "City", new ImageBrush(new BitmapImage(new Uri(@"resources\City.png", UriKind.Relative))) },
            { "Town", new ImageBrush(new BitmapImage(new Uri(@"resources\Town.png", UriKind.Relative))) },
            { "Settler", new ImageBrush(new BitmapImage(new Uri(@"resources\Settler.png", UriKind.Relative))) },
            { "Hamlet", new ImageBrush(new BitmapImage(new Uri(@"resources\Hamlet.png", UriKind.Relative))) },
            { "Megaopolis", new ImageBrush(new BitmapImage(new Uri(@"resources\Megaopolis.png", UriKind.Relative))) },
            { "Metropolis", new ImageBrush(new BitmapImage(new Uri(@"resources\Metropolis.png", UriKind.Relative))) },
            { "Village", new ImageBrush(new BitmapImage(new Uri(@"resources\Village.png", UriKind.Relative))) },
            { "Abandoned Population Center", new ImageBrush(new BitmapImage(new Uri(@"resources\Abandoned Population Center.png", UriKind.Relative))) },
        };

        /// <summary>
        /// Backing field for Selected property.
        /// </summary>
        private bool selected;

        /// <summary>
        /// The inner grid square.
        /// </summary>
        private GridSquare inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SquareView"/> class.
        /// </summary>
        /// <param name="inner">The inner grid square.</param>
        public SquareView(GridSquare inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the Left position to display this in the viewer.
        /// </summary>
        public int ViewerX => this.inner.X * WorldViewer.GridSize;

        /// <summary>
        /// Gets the Top position to display this in the viewer.
        /// </summary>
        public int ViewerY => this.inner.Y * WorldViewer.GridSize;

        /// <summary>
        /// Gets the X coord of this in the world.
        /// </summary>
        public int X => this.inner.X;

        /// <summary>
        /// Gets the Y coord of this in the world.
        /// </summary>
        public int Y => this.inner.Y;

        /// <summary>
        /// Gets or sets a value indicating whether this square is selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                return this.selected;
            }

            set
            {
                if (this.selected == value)
                {
                    return;
                }

                this.selected = value;

                this.OnPropertyChanged(nameof(this.Selected));
                this.OnPropertyChanged(nameof(this.BackgroundColor));
            }
        }

        /// <summary>
        /// Gets the name of this tile.
        /// </summary>
        public string Name
        {
            get
            {
                int count = this.inner.GetThings().Count() - 1;
                if (count == 0)
                {
                    return string.Empty;
                }

                return count.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the text color to use for the title.
        /// </summary>
        public Brush BackgroundColor
        {
            get
            {
                string? name = this.inner.SquareDefinition?.Definition.Name;

                if (name == null || !ColorMap.TryGetValue(name, out Brush? value))
                {
                    value = Brushes.Green;
                }

                return value;
            }
        }

        /// <summary>
        /// Gets the foreground of this square.
        /// </summary>
        public Brush? Foreground
        {
            get
            {
                Brush? image = null;
                foreach (BaseThing thing in this.inner.GetThings())
                {
                    if (ImageMap.TryGetValue(thing.BaseDefinition.Name, out image))
                    {
                        break;
                    }
                }

                image ??= Brushes.Transparent;

                return image;
            }
        }

        /// <summary>
        /// Fires property changed events.
        /// </summary>
        /// <param name="propertyName">True when property changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
