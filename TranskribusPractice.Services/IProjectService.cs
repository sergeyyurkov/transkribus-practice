using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranskribusPractice.BusinessDomain;

namespace TranskribusPractice.Services
{
    public interface IProjectService
    {
        string OpenProjectFile(out Project project);
        void Save(string path, Project project);
        string SaveAs(Project project);
    }
}
