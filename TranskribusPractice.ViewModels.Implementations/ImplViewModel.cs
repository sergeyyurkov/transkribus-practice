using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TranskribusPractice.BusinessDomain.AreaConcept;
using TranskribusPractice.ViewModels.Implementations.Commands;

namespace TranskribusPractice.ViewModels.Implementations
{
    public partial class ImplViewModel : AbstractViewModel, IMouseAware
    {
        private string _textLeft;
        private string _textRight;
        private string _textSelected;
        private ObservableCollection<RectangleRegion> _allRegions;
        private ObservableCollection<TextRegion> _textRegions;
        private RelayCommand _setTextRegionModeCommand;
        private RelayCommand _setLineRegionModeCommand;
        private RelayCommand _setWordRegionModeCommand;
        private RelayCommand _setSelectionModeCommand;
        public override string JpgPath { get; set; } = "test.jpg";
        public override string TextLeft
        {
            get => _textLeft;
            set
            {
                if (_textLeft == value) return;
                _textLeft = value;
                NotifyPropertyChanged();
            }
        }
        public override string TextRight
        {
            get => _textRight;
            set
            {
                if (_textRight == value) return;
                _textRight = value;
                NotifyPropertyChanged();
            }
        }
        public override string TextSelected
        {
            get => _textSelected;
            set
            {
                if (_textSelected == value) return;
                _textSelected = value;
                NotifyPropertyChanged();
            }
        }
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
                        foreach (var region in AllRegions)
                        {
                            region.SelectionMode = false;
                        }
                        Mode = Region.Text;
                    }));
        }
        public override ICommand SetLineRegionModeCommand
        {
            get => _setLineRegionModeCommand ??
                    (_setLineRegionModeCommand = new RelayCommand((o) =>
                    {
                        foreach (var region in AllRegions)
                        {
                            region.SelectionMode = false;
                        }
                        Mode = Region.Line;
                    }));
        }
        public override ICommand SetWordRegionModeCommand
        {
            get => _setWordRegionModeCommand ??
                    (_setWordRegionModeCommand = new RelayCommand((o) =>
                    {
                        foreach (var region in AllRegions)
                        {
                            region.SelectionMode = false;
                        }
                        Mode = Region.Word;
                    }));
        }
        public override ICommand SetSelectionModeCommand
        {
            get => _setSelectionModeCommand ??
                    (_setSelectionModeCommand = new RelayCommand((o) =>
                    {
                        foreach (var region in AllRegions)
                        {
                            region.SelectionMode = true;
                        }
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
        public void BuildRichTextBox()
        {
            StringBuilder sbl = new StringBuilder();
            StringBuilder sbs = new StringBuilder();
            StringBuilder sbr = new StringBuilder();
            bool isSelection = false;
            bool wasSelection = false;
            int lineCounter = 1;
            foreach (var text in TextRegions)
            {
                if (SelectedRectangle == text)
                {
                    isSelection = true;
                }
                foreach (var line in text.Lines ?? Enumerable.Empty<LineRegion>())
                {
                    if (SelectedRectangle == line)
                    {
                        isSelection = true;
                    }
                    if (!isSelection && !wasSelection)
                    {
                        sbl.Append(lineCounter++ + ".");
                    }
                    else if (isSelection)
                    {
                        sbs.Append(lineCounter++ + ".");
                    }
                    else if (wasSelection)
                    {
                        sbr.Append(lineCounter++ + ".");
                    }
                    foreach (var word in line.Words ?? Enumerable.Empty<WordRegion>())
                    {
                        if (SelectedRectangle == word)
                        {
                            isSelection = true;
                        }
                        if (!isSelection && !wasSelection)
                        {
                            sbl.Append(' ' + word.Content);
                        }
                        else if (isSelection)
                        {
                            sbs.Append(' ' + word.Content);
                        }
                        else if (wasSelection)
                        {
                            sbr.Append(' ' + word.Content);
                        }
                        if (SelectedRectangle == word)
                        {
                            isSelection = false;
                            wasSelection = true;
                        }
                    }
                    if (!isSelection && !wasSelection)
                    {
                        sbl.Append(Environment.NewLine);
                    }
                    else if (isSelection)
                    {
                        sbs.Append(Environment.NewLine);
                    }
                    else if (wasSelection)
                    {
                        sbr.Append(Environment.NewLine);
                    }
                    if (SelectedRectangle == line)
                    {
                        isSelection = false;
                        wasSelection = true;
                    }
                }
                if (SelectedRectangle == text)
                {
                    isSelection = false;
                    wasSelection = true;
                }
            }
            TextLeft = sbl.ToString();
            TextSelected = sbs.ToString();
            TextRight = sbr.ToString();
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
