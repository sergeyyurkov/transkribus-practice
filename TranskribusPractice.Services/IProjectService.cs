using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TranskribusPractice.BusinessDomain;

namespace TranskribusPractice.Services
{
    public interface IProjectService
    {
        string OpenProjectFile(out Project project);
        void Save(string path, Project project);
        (bool, string) SaveAs(string oldProjectPath, Project project);
        void ShowError();
        CustomMessageBoxResult ShowNotSavedWarning(string projectFileName);
    }
}
