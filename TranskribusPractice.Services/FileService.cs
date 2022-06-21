using System;
using Microsoft.Win32;


namespace TranskribusPractice.Services
{
    public class FileService : IFileService
    {
        public string OpenJpgFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "jpg files (*.jpg)|*.jpg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            string filename = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
            }
            return filename;
        }
    }
}
