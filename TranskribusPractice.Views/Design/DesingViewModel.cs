using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.Views.Design
{
    internal class DesingViewModel : ViewModels.AbstractViewModel
    {
        private ObservableCollection<RectangleRegion> _allRegions;
        private ObservableCollection<TextRegion> _textRegions;
        public override string JpgPath { get; set; } = "test.jpg";
        public override string ProjectPath { get ; set; } = "test.xml";
        public override string TextLeft { get; set; } = "Example Text Left";
        public override string TextRight { get; set; } = "Example Text Riht";
        public override string TextSelected { get; set; } = "Example Text Selected";
        public override double RectangleWidth { get; set; }
        public override double RectangleHeight { get; set; }
        public override double RectangleCanvasLeft { get; set; }
        public override double RectangleCanvasTop { get; set; }
        public override Region Mode { get; set; }
        public override bool RectangleVisibility { get; set; }
        public override RectangleRegion SelectedRectangle { get; set; }
        public override ObservableCollection<RectangleRegion> AllRegions
        {
            get => _allRegions ?? (_allRegions = new ObservableCollection<RectangleRegion>());
            set => _allRegions = value;
        }
        public override ObservableCollection<TextRegion> TextRegions 
        {
            get => _textRegions ?? (_textRegions = new ObservableCollection<TextRegion>());
            set => _textRegions = value;
        }
        public override ICommand OpenJpgFileCommand => null;
        public override ICommand CreateNewProjectCommand => null;
        public override ICommand OpenProjectFileCommand => null;
        public override ICommand SaveProjectCommand => null;
        public override ICommand SaveAsProjectCommand => null;
        public override ICommand SetTextRegionModeCommand => null;
        public override ICommand SetLineRegionModeCommand => null;
        public override ICommand SetWordRegionModeCommand => null;
        public override ICommand SetSelectionModeCommand => null;

    }
}
