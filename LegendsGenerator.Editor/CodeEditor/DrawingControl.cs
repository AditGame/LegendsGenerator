// -------------------------------------------------------------------------------------------------
// <copyright file="DrawingControl.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.CodeEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;

    public class DrawingControl : FrameworkElement
    {
        private VisualCollection visuals;
        private DrawingVisual visual;

        public DrawingControl()
        {
            this.visual = new DrawingVisual();
            this.visuals = new VisualCollection(this);
            this.visuals.Add(this.visual);
        }

        public DrawingContext GetContext()
        {
            return this.visual.RenderOpen();
        }

        protected override int VisualChildrenCount
        {
            get { return this.visuals.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= this.visuals.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this.visuals[index];
        }
    }
}
