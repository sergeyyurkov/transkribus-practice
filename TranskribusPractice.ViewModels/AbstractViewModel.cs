using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TranskribusPractice.BusinessDomain.AreaConcept;
namespace TranskribusPractice.ViewModels
{
    public abstract class AbstractViewModel : BaseViewModel
    {
        public abstract string JpgPath { get; set; }
        public abstract string ProjectPath { get; set; }
        public abstract string TextLeft { get; set; }
        public abstract string TextRight { get; set; }
        public abstract string TextSelected { get; set; }
        public abstract double RectangleWidth { get; set; }
        public abstract double RectangleHeight { get; set; }
        public abstract double RectangleCanvasLeft { get; set; }
        public abstract double RectangleCanvasTop { get; set; }
        public abstract Region Mode { get; set; }
        public abstract bool RectangleVisibility { get; set; }
        public abstract RectangleRegion SelectedRectangle { get; set; }
        public abstract ObservableCollection<RectangleRegion> AllRegions { get; set; }
        public abstract ObservableCollection<TextRegion> TextRegions { get; set; }
        public abstract ICommand OpenJpgFileCommand { get; }
        public abstract ICommand CreateNewProjectCommand { get; }
        public abstract ICommand OpenProjectFileCommand { get; }
        public abstract ICommand SaveProjectCommand { get; }
        public abstract ICommand SaveAsProjectCommand { get; }
        public abstract ICommand SetTextRegionModeCommand { get; }
        public abstract ICommand SetLineRegionModeCommand { get; }
        public abstract ICommand SetWordRegionModeCommand { get; }
        public abstract ICommand SetSelectionModeCommand { get; }
    }
}
