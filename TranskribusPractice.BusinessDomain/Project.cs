using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.BusinessDomain
{
    public class Project
    {
        public string JpgPath { get; set; }
        public ObservableCollection<TextRegion> TextRegions { get; set; }
        public Project(string jpgPath, ObservableCollection<TextRegion> textRegions) 
        {
            JpgPath = jpgPath;
            TextRegions = textRegions;
        }
        public Project() 
        {
            TextRegions = new ObservableCollection<TextRegion>();
        }
    }
}
