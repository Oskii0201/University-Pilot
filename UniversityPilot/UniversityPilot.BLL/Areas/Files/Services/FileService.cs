using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.Shared.Utilities;

namespace UniversityPilot.BLL.Areas.Files.Services
{
    internal class FileService : IFileService
    {
        public Dictionary<string, string> GetFileTypeDictionary()
        {
            return EnumHelper.GetEnumDescriptionDictionary<FileType>();
        }
    }
}