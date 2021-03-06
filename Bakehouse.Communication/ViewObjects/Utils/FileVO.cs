namespace Bakehouse.Communication.ViewObjects.Utils
{
    public class FileVO
    {
        public string File { get; set; }
        public string Name { get; set; }
        public bool Disable { get; set; }
    }

    public class FileManySaveVO
    {
        public string MainName { get; set; }
        public string GalleryName { get; set; }

        public FileManySaveVO()
        {
            MainName = "";
            GalleryName = "";
        }
    }
}