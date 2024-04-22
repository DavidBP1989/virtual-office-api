namespace cobach_api.Application.Dtos.Documentos
{
    public class InformacionCarpetasResponse
    {
        public string FolderName { get; set; } = null!;
        public int FolderId { get; set; }
        public int FilesPerFolder { get; set; }
    }
}
