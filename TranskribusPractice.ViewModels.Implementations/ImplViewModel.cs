using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TranskribusPractice.BusinessDomain;
using TranskribusPractice.BusinessDomain.AreaConcept;
using TranskribusPractice.Services;
using TranskribusPractice.ViewModels.Implementations.Commands;

namespace TranskribusPractice.ViewModels.Implementations
{
    public partial class ImplViewModel : AbstractViewModel, IMouseAware, IKeyboardAware, IFocusAware
    {
        private string _jpgPath;
        private string _textLeft;
        private string _textRight;
        private string _textSelected;
        private bool _isFocusable;
        private ObservableCollection<RectangleRegion> _allRegions = new ObservableCollection<RectangleRegion>();
        private ObservableCollection<TextRegion> _textRegions = new ObservableCollection<TextRegion>();
        private ObservableCollection<TextRegion> _savedTextRegions;
        private RelayCommand _openJpgFileCommand;
        private RelayCommand _createNewProjectCommand;
        private RelayCommand _openProjectFileCommand;
        private RelayCommand _saveProjectCommand;
        private RelayCommand _saveAsProjectCommand;
        private RelayCommand _setTextRegionModeCommand;
        private RelayCommand _setLineRegionModeCommand;
        private RelayCommand _setWordRegionModeCommand;
        private RelayCommand _setSelectionModeCommand;
        private readonly IServiceProvider _serviceProvider = new ServiceProvider();
        public override string JpgPath
        {
            get => _jpgPath;
            set
            {
                if (_jpgPath == value) return;
                _jpgPath = value;
                NotifyPropertyChanged();
            }
        }
        public override string ProjectPath{ get; set; }
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
        public override bool IsFocusable 
        {
            get => _isFocusable;
            set
            {
                if (_isFocusable == value) return;
                _isFocusable = value;
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
        public override ICommand OpenJpgFileCommand
        {
            get => _openJpgFileCommand ??
                    (_openJpgFileCommand = new RelayCommand((o) =>
                    {
                        var fileService = (IFileService)_serviceProvider.GetService(typeof(IFileService));
                        string path = fileService.OpenJpgFile();
                        if (path != null && path != string.Empty)
                        {
                            JpgPath = path;
                        }
                    }));
        }
        public override ICommand CreateNewProjectCommand
        {
            //TODO add saved check
            get => _createNewProjectCommand ??
                    (_createNewProjectCommand = new RelayCommand((o) =>
                    {
                        TextRegions.Clear();
                        AllRegions.Clear();
                        ProjectPath = string.Empty;
                        JpgPath = string.Empty;
                    }));
        }
        public override ICommand OpenProjectFileCommand
        {
            //TODO add saved check
            get => _openProjectFileCommand ??
                    (_openProjectFileCommand = new RelayCommand((o) =>
                    {
                        var projectService = (IProjectService)_serviceProvider.GetService(typeof(IProjectService));
                        Project project;
                        string path = projectService.OpenProjectFile(out project);
                        if ((path != string.Empty && path != null) && !(project is null)) 
                        {
                            ProjectPath = path;
                            JpgPath = project.JpgPath;
                            TextRegions = project.TextRegions;
                            UpdateAllRegions();
                            BuildRichTextBox();
                        }
                    }));
        }
        public override ICommand SaveProjectCommand
        {
            get => _saveProjectCommand ??
                    (_saveProjectCommand = new RelayCommand((o) =>
                    {
                        var projectService = (IProjectService)_serviceProvider.GetService(typeof(IProjectService));
                        if ((ProjectPath != string.Empty && ProjectPath != null)
                            && (JpgPath != string.Empty && JpgPath != null))
                        {
                            var copiedTextRegions = CopyTextRegions();
                            projectService.Save(ProjectPath, new Project(JpgPath, copiedTextRegions));
                            _savedTextRegions = copiedTextRegions;

                        }
                        else if (JpgPath != string.Empty && JpgPath != null)
                        {
                            var oldProjectPath = ProjectPath;
                            var copiedTextRegions = CopyTextRegions();
                            ProjectPath = projectService.SaveAs(new Project(JpgPath, copiedTextRegions));
                            if(ProjectPath != oldProjectPath && oldProjectPath != string.Empty) 
                            {
                                _savedTextRegions = copiedTextRegions;
                            }
                        }
                        else 
                        {
                            projectService.ShowError();
                        }

                    }));
        }
        public override ICommand SaveAsProjectCommand
        {
            get => _saveAsProjectCommand ??
                    (_saveAsProjectCommand = new RelayCommand((o) =>
                    {
                        var projectService = (IProjectService)_serviceProvider.GetService(typeof(IProjectService));
                        if (JpgPath != string.Empty && JpgPath != null) 
                        {
                            var oldProjectPath = ProjectPath;
                            var copiedTextRegions = CopyTextRegions();
                            ProjectPath = projectService.SaveAs(new Project(JpgPath, copiedTextRegions));
                            if (ProjectPath != oldProjectPath && oldProjectPath != string.Empty)
                            {
                                _savedTextRegions = copiedTextRegions;
                            }
                        }
                        else 
                        {
                            projectService.ShowError();
                        }
                    }));
        }
        public override ICommand SetTextRegionModeCommand
        {
            get => _setTextRegionModeCommand ??
                    (_setTextRegionModeCommand = new RelayCommand((o) =>
                    {
                        IsFocusable = false;
                        Mode = Region.Text;
                    }));
        }
        public override ICommand SetLineRegionModeCommand
        {
            get => _setLineRegionModeCommand ??
                    (_setLineRegionModeCommand = new RelayCommand((o) =>
                    {
                        IsFocusable = false;
                        Mode = Region.Line;
                    }));
        }
        public override ICommand SetWordRegionModeCommand
        {
            get => _setWordRegionModeCommand ??
                    (_setWordRegionModeCommand = new RelayCommand((o) =>
                    {
                        IsFocusable = false;
                        Mode = Region.Word;
                        
                    }));
        }
        public override ICommand SetSelectionModeCommand
        {
            get => _setSelectionModeCommand ??
                    (_setSelectionModeCommand = new RelayCommand((o) =>
                    {
                        IsFocusable = true;
                        Mode = Region.Undefined;
                    }));
        }
        public ImplViewModel() 
        {
            IsFocusable = true;
        }
        public void DeleteSelectedRectangle()
        {
            if (SelectedRectangle is TextRegion)
            {
                TextRegions.Remove((TextRegion)SelectedRectangle);
            }
            if (SelectedRectangle is LineRegion)
            {
                foreach (var text in TextRegions)
                {
                    text.Lines.Remove((LineRegion)SelectedRectangle);
                }
            }
            if (SelectedRectangle is WordRegion)
            {
                foreach (var text in TextRegions)
                {
                    foreach (var line in text.Lines)
                    {
                        line.Words.Remove((WordRegion)SelectedRectangle);
                    }
                }
            }
            SelectedRectangle = null;
            UpdateAllRegions();
        }
        public void LoseFocus() 
        {
            SelectedRectangle = null;
        }
        private bool IsSaved() 
        {
            if (_savedTextRegions.Count != TextRegions.Count)
            {
                return false;
            }
            for (int i = 0; i < _savedTextRegions.Count; i++)
            {
                if (_savedTextRegions[i].Lines.Count != TextRegions[i].Lines.Count) 
                {
                    return false;
                }
                for (int j = 0; j < _savedTextRegions[i].Lines.Count; j++)
                {
                    if (_savedTextRegions[i].Lines[j].Words.Count != TextRegions[i].Lines[j].Words.Count)
                    {
                        return false;
                    }
                    for (int k = 0; k < _savedTextRegions[i].Lines[j].Words.Count; k++)
                    {
                        if (_savedTextRegions[i].Lines[j].Words[k].Content
                            != TextRegions[i].Lines[j].Words[k].Content) 
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
       
        private ObservableCollection<TextRegion> CopyTextRegions() 
        {
            var textRegions = new ObservableCollection<TextRegion>();
            foreach (var text in TextRegions)
            {
                textRegions.Add((TextRegion)text.Clone());
            }
            return textRegions;
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
        private void BuildRichTextBox()
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
                        sbl.Append(lineCounter++ + ". ");
                    }
                    else if (isSelection)
                    {
                        sbs.Append(lineCounter++ + ". ");
                    }
                    else if (wasSelection)
                    {
                        sbr.Append(lineCounter++ + ". ");
                    }
                    foreach (var word in line.Words ?? Enumerable.Empty<WordRegion>())
                    {
                        if (SelectedRectangle == word)
                        {
                            isSelection = true;
                        }
                        if (!isSelection && !wasSelection)
                        {
                            sbl.Append(word.Content + ' ');
                        }
                        else if (isSelection)
                        {
                            sbs.Append(word.Content + ' ');
                        }
                        else if (wasSelection)
                        {
                            sbr.Append(word.Content + ' ');
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
