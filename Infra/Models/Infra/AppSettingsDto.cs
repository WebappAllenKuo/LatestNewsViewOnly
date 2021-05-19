using System.Collections.Generic;

namespace Infra.Models.Infra
{
    public class AppSettingsDto
    {
        public FileFolderSettings FileFolder { get; set; }
    }

    public class FileFolderSettings
    {
        public string[] Base { get; set; }

        public string[] Image { get; set; }

        public string[] Attachment { get; set; }
    }
}
