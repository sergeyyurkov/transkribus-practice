using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using TranskribusPractice.BusinessDomain;

namespace TranskribusPractice.Services
{
    public class ProjectService : IProjectService
    {
        public string OpenProjectFile(out Project project )
        {
            project = null;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "project (*.xml)|*.xml",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            string projectPath = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                projectPath = openFileDialog.FileName;
            }
            XmlSerializer xs = new XmlSerializer(typeof(Project));
            if (projectPath != String.Empty)
            {
                using (StreamReader rd = new StreamReader(projectPath))
                {
                    project = xs.Deserialize(rd) as Project;
                }
            }
            return projectPath;
        }

        public void Save(string path, Project project)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Project));
            using (StreamWriter wr = new StreamWriter(path))
            {
                xs.Serialize(wr, project);
            }
        }

        public (bool, string) SaveAs(string oldProjectPath, Project project)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                FileName = oldProjectPath,
                Filter = "project (*.xml)|*.xml",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            string newProjectPath = string.Empty;
            if (saveFileDialog.ShowDialog() == true)
            {
                newProjectPath = saveFileDialog.FileName;
            }
            else 
            {
                return (false, oldProjectPath);
            }
            XmlSerializer xs = new XmlSerializer(typeof(Project));
            if (newProjectPath != String.Empty)
            {
                using (StreamWriter wr = new StreamWriter(newProjectPath))
                {
                    xs.Serialize(wr, project);
                }
            }
            else 
            {
                return (false, oldProjectPath);
            }
            return (true, newProjectPath);
        }

        public void ShowError()
        {
            MessageBox.Show("You can't save the project without choosing an image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public CustomMessageBoxResult ShowNotSavedWarning(string projectFileName)
        {
            var result = MessageBox.Show("Do you want to save changes to \"" + projectFileName+ '\"',
                                 "Warning",
                                 MessageBoxButton.YesNoCancel,
                                 MessageBoxImage.Question);
            switch (result) 
            {
                case MessageBoxResult.Yes: return CustomMessageBoxResult.Yes;
                case MessageBoxResult.No: return CustomMessageBoxResult.No;
                case MessageBoxResult.Cancel: return CustomMessageBoxResult.Cancel;
                default: return CustomMessageBoxResult.Cancel;
            }
        }
    }
}
