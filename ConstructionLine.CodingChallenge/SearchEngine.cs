using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly Shirt[] _shirts;
        private readonly Dictionary<Color, ColorCount> _colorCounts = new Dictionary<Color, ColorCount>(Color.All.Count);
        private readonly Dictionary<Size, SizeCount> _sizeCounts = new Dictionary<Size, SizeCount>(Size.All.Count);

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts.ToArray();

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            foreach (var color in Color.All)
                _colorCounts.Add(color, new ColorCount { Color = color, Count = 0 });
		
            foreach (var size in Size.All)
                _sizeCounts.Add(size, new SizeCount { Size = size, Count = 0 });
        }


        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            var foundShirts = new List<Shirt>();

            Action<Shirt, SearchOptions, List<Shirt>> searchAction;

            if (options.Sizes.Count != 0 && options.Colors.Count != 0)
                searchAction = PerformSizeAndColorSearch;

            else if (options.Sizes.Count == 0 && options.Colors.Count != 0)
                searchAction = PerformColorSearch;
                
            else
                searchAction = PerformSizeSearch;
            
            foreach (var shirt in _shirts)
                searchAction(shirt, options, foundShirts);

            return new SearchResults 
            { 
                ColorCounts = _colorCounts.Values.ToList(), 
                Shirts = foundShirts.ToList(), 
                SizeCounts = _sizeCounts.Values.ToList() 
            };
        }

        private void PerformSizeAndColorSearch(Shirt shirt, SearchOptions options, List<Shirt> foundShirts)
        {
            foreach (var size in options.Sizes)
                if (size == shirt.Size)
                    PerformColorSearch(shirt, options, foundShirts);
        }

        private void PerformColorSearch(Shirt shirt, SearchOptions options, List<Shirt> foundShirts)
        {
            foreach (var color in options.Colors)

                if (color == shirt.Color)
                {
                    foundShirts.Add(shirt);

                    _sizeCounts[shirt.Size].Count++;
                    _colorCounts[shirt.Color].Count++;
                }
        }

        private void PerformSizeSearch(Shirt shirt, SearchOptions options, List<Shirt> foundShirts)
        {
            foreach (var size in options.Sizes)

                if (size == shirt.Size)
                {
                    foundShirts.Add(shirt);

                    _sizeCounts[shirt.Size].Count++;
                    _colorCounts[shirt.Color].Count++;
                }
        }
    }
}