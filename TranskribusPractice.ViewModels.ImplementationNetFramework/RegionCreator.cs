using System;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.ViewModels.ImplementationsNetFramework
{
    internal class RegionCreator
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public RegionCreator(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public TextRegion CreateTextRegion(string name)
        {
            return new TextRegion()
            {
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Name = name
            };
        }
        public LineRegion CreateLineRegion(string name)
        {
            return new LineRegion()
            {
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Name = name
            };
        }
        public WordRegion CreateWordRegion(string name)
        {
            return new WordRegion()
            {
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Name = name
            };
        }
    }
}
