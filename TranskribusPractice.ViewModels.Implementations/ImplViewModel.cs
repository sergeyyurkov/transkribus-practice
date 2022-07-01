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
        private const string DefaultProjectName = "new project.xml";
        private string _jpgPath;
        private string _textLeft;
        private string _textRight;
        private string _textSelected;
        private bool _isFocusable = true;
        private bool _isNewProject = true;
        private ObservableCollection<RectangleRegion> _allRegions = new ObservableCollection<RectangleRegion>();
        private ObservableCollection<TextRegion> _textRegions = new ObservableCollection<TextRegion>();
        private ObservableCollection<TextRegion> _savedTextRegions = new ObservableCollection<TextRegion>();
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
        public override string ProjectPath { get; set; } = DefaultProjectName;
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
            get => _createNewProjectCommand ??
                    (_createNewProjectCommand = new RelayCommand(CreateNewProjectCommandExecution));
        }
        public override ICommand OpenProjectFileCommand
        {
            get => _openProjectFileCommand ??
                    (_openProjectFileCommand = new RelayCommand(OpenProjectFileCommandExecution));
        }
        public override ICommand SaveProjectCommand
        {
            get => _saveProjectCommand ??
                    (_saveProjectCommand = new RelayCommand(SaveProjectCommandExecution));
        }
        public override ICommand SaveAsProjectCommand
        {
            get => _saveAsProjectCommand ??
                    (_saveAsProjectCommand = new RelayCommand(SaveAsProjectCommandExecution));
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
        private void CreateNewProjectCommandExecution(object param)
        {
            var projectService = (IProjectService)_serviceProvider.GetService(typeof(IProjectService));
            if (!IsSaved())
            {
                var result = projectService.ShowNotSavedWarning(ProjectPath);
                if (result == CustomMessageBoxResult.Yes)
                {
                    SaveProjectCommandExecution(null);
                }
                else if (result == CustomMessageBoxResult.Cancel) 
                {
                    return;
                }
            }
            _isNewProject = true;
            _savedTextRegions.Clear();
            TextRegions.Clear();
            AllRegions.Clear();
            ProjectPath = DefaultProjectName;
            JpgPath = string.Empty;
        }
        private void OpenProjectFileCommandExecution(object param)
        {
            var projectService = (IProjectService)_serviceProvider.GetService(typeof(IProjectService));
            if (!IsSaved())
            {
                var result = projectService.ShowNotSavedWarning(ProjectPath);
                if (result == CustomMessageBoxResult.Yes)
                {
                    SaveProjectCommandExecution(null);
                }
                else if (result == CustomMessageBoxResult.Cancel)
                {
                    return;
                }
            }
            Project project;
            string path = projectService.OpenProjectFile(out project);
            if ((path != string.Empty && path != null) && !(project is null))
            {
                ProjectPath = path;
                JpgPath = project.JpgPath;
                _savedTextRegions = project.TextRegions;
                TextRegions = project.TextRegions;
                _isNewProject = false;
                UpdateAllRegions();
                BuildRichTextBox();
            }
        }
        private void SaveProjectCommandExecution(object param) 
        {
            var projectService = (IProjectService)_serviceProvider.GetService(typeof(IProjectService));
            if ((ProjectPath != string.Empty && ProjectPath != null)
                && (JpgPath != string.Empty && JpgPath != null))
            {
                var copiedTextRegions = CopyTextRegions();
                if (_isNewProject)
                {
                    _isNewProject = !SaveAsProject(projectService);
                }
                else
                {
                    projectService.Save(ProjectPath, new Project(JpgPath, copiedTextRegions));
                }
                _savedTextRegions = copiedTextRegions;
            }
            else if (JpgPath != string.Empty && JpgPath != null)
            {
                SaveAsProject(projectService);
            }
            else
            {
                projectService.ShowError();
            }
        }
        private void SaveAsProjectCommandExecution(object param)
        {
            var projectService = (IProjectService)_serviceProvider.GetService(typeof(IProjectService));
            if (JpgPath != string.Empty && JpgPath != null)
            {
                SaveAsProject(projectService);
            }
            else
            {
                projectService.ShowError();
            }
        }
        private bool SaveAsProject(IProjectService projectService) 
        {
            bool isSaved;
            var copiedTextRegions = CopyTextRegions();
            (isSaved, ProjectPath) = projectService.SaveAs(ProjectPath, new Project(JpgPath, copiedTextRegions));
            if (isSaved)
            {
                _savedTextRegions = copiedTextRegions;
            }
            return isSaved;
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
        public ImplViewModel() { }
        public void DeleteSelectedRectangle()
        {
            if (SelectedRectangle is TextRegion tr)
            {
                TextRegions.Remove(tr);
            }
            else if (SelectedRectangle is LineRegion lr)
            {
                foreach (var text in TextRegions)
                {
                    text.Lines.Remove(lr);
                }
            }
            else if (SelectedRectangle is WordRegion wr)
            {
                foreach (var text in TextRegions)
                {
                    foreach (var line in text.Lines)
                    {
                        line.Words.Remove(wr);
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
    }
}
