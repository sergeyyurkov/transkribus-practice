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

        public string SaveAs(Project project)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "project (*.xml)|*.xml",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            string projectPath = string.Empty;
            if (saveFileDialog.ShowDialog() == true)
            {
                projectPath = saveFileDialog.FileName;
            }
            XmlSerializer xs = new XmlSerializer(typeof(Project));
            if (projectPath != String.Empty)
            {
                using (StreamWriter wr = new StreamWriter(projectPath))
                {
                    xs.Serialize(wr, project);
                }
            }
            return projectPath;
        }

        public void ShowError()
        {
            MessageBox.Show("You can't save the project without choosing an image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
