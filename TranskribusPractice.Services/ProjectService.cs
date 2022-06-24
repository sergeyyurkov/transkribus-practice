using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TranskribusPractice.BusinessDomain;

namespace TranskribusPractice.Services
{
    public class ProjectService : IProjectService
    {
        public string OpenProjectFile(out Project project)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "project (*.xml)|*.xml",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            string path = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
            }
            XmlSerializer xs = new XmlSerializer(typeof(Project));
            using (StreamReader rd = new StreamReader(path))
            {
                project = xs.Deserialize(rd) as Project;
            }
            return path;
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
    }
}
