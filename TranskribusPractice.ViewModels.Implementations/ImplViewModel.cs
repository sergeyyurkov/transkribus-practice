using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TranskribusPractice.BusinessDomain.AreaConcept;
using TranskribusPractice.ViewModels.Implementations.Commands;

namespace TranskribusPractice.ViewModels.Implementations
{
    public partial class ImplViewModel : AbstractViewModel, IMouseAware
    {
        private ObservableCollection<RectangleRegion> _allRegions;
        private ObservableCollection<TextRegion> _textRegions;
        private RelayCommand _setTextRegionModeCommand;
        private RelayCommand _setLineRegionModeCommand;
        private RelayCommand _setWordRegionModeCommand;
        public override string JpgPath { get; set; } = "test.jpg";
        public override string TextLeft { get; set; } = "Real Text Left";
        public override string TextRight { get; set; } = "Real Text Riht";
        public override string TextSelected { get; set; } = "Real Text Selected";
        public override ObservableCollection<TextRegion> TextRegions
        {
            get => _textRegions;
            set
            {
                if (_textRegions == value) return;
                _textRegions = value;
                NotifyPropertyChanged();
            }
        }
        public override ObservableCollection<RectangleRegion> AllRegions
        {
            get => _allRegions;
            set
            {
                if (_allRegions == value) return;
                _allRegions = value;
                NotifyPropertyChanged();
            }
        }
        public override ICommand SaveCommand => null;
        public override ICommand OpenCommand => null;
        public override ICommand SaveAsCommand => null;
        public override ICommand SetTextRegionModeCommand
        {
            get => _setTextRegionModeCommand ??
                    (_setTextRegionModeCommand = new RelayCommand((o) =>
                    {
                        Mode = Region.Text;
                    }));
        }
        public override ICommand SetLineRegionModeCommand
        {
            get => _setLineRegionModeCommand ??
                    (_setLineRegionModeCommand = new RelayCommand((o) =>
                    {
                        Mode = Region.Line;
                    }));
        }
        public override ICommand SetWordRegionModeCommand
        {
            get => _setWordRegionModeCommand ??
                    (_setWordRegionModeCommand = new RelayCommand((o) =>
                    {
                        Mode = Region.Word;
                    }));
        }

        public ImplViewModel() 
        {
            FillTextRegions();
            UpdateAllRegions();
        }
        private void UpdateAllRegions()
        {
            AllRegions = new ObservableCollection<RectangleRegion>();
            foreach (TextRegion text in TextRegions ?? Enumerable.Empty<TextRegion>())
            {
                AllRegions.Add(text);
                foreach (LineRegion line in text.Lines ?? Enumerable.Empty<LineRegion>())
                {
                    AllRegions.Add(line);
                    foreach (WordRegion word in line.Words ?? Enumerable.Empty<WordRegion>())
                    {
                        AllRegions.Add(word);
                    }
                }
            }
        }
        private void FillTextRegions()
        {
            TextRegions = new ObservableCollection<TextRegion>()
            {
                new TextRegion()
                {
                    Name = "Text region 1",
                    X = 100,
                    Y = 200,
                    Width = 50,
                    Height = 70,
                    Lines = new ObservableCollection<LineRegion>
                    {
                        new LineRegion()
                        {
                            Name = "Line 1",
                            X = 20,
                            Y = 20,
                            Width = 33,
                            Height = 22,
                            Words = new ObservableCollection<WordRegion>()
                            {
                                new WordRegion()
                                {
                                    Name = "Word 1",
                                    X = 60,
                                    Y = 20,
                                    Width = 44,
                                    Height = 55,
                                    Content = "Word 11111"
                                },
                                new WordRegion()
                                {
                                    Name = "Word 2",
                                    X = 50,
                                    Y = 20,
                                    Width = 44,
                                    Height = 55,
                                    Content = "Word 22222"
                                }
                            }
                        },
                        new LineRegion()
                        {
                            Name = "Line 2",
                            X = 170,
                            Y = 200,
                            Width = 20,
                            Height = 50
                        }
                    }
                },
                new TextRegion()
                {
                    Name = "Text region 2",
                    X = 150,
                    Y = 300,
                    Width = 20,
                    Height = 50,
                    Lines = new ObservableCollection<LineRegion> { new LineRegion() { Name = "Line 3" }, new LineRegion() { Name = "Line 2" } }
                },
                new TextRegion()
                {
                    Name = "Text region 3",
                    X = 150,
                    Y = 150,
                    Width = 60,
                    Height = 30,
                }
            };
        }
    }
}
