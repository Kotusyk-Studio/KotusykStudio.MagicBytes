using System.IO.Compression;

namespace KotusykStudio.MagicBytes;
public static class ByteSniffer
{
    public static string GetFileExtension(byte[] fileBytes)
    {
        foreach (var magicNumber in MagicNumbersExtensions)
        {
            if (fileBytes.Length < magicNumber.Key.Length + magicNumber.Value.offset)
                continue;

            var match = true;
            for (int i = 0; i < magicNumber.Key.Length; i++)
            {
                if (magicNumber.Key[i] != fileBytes[i + magicNumber.Value.offset])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                string extension = magicNumber.Value.extension;
                if (extension == ".zip")
                {
                    using var memoryStream = new MemoryStream(fileBytes);
                    using var zip = new ZipArchive(memoryStream);
                    if (zip.Entries.Any(x => x.FullName.Contains("_rels/.rels")))
                    {
                        if (zip.Entries.Any(x => x.FullName.Contains("word/document.xml")))
                        {
                            extension = ".docx";
                        }
                        else if (zip.Entries.Any(x => x.FullName.Contains("xl/workbook.xml")))
                        {
                            extension = ".xlsx";
                        }
                    }
                }
                return extension;
            }
        }

        return ".unknown";
    }

    public static readonly Dictionary<byte[], (string extension, int offset)> MagicNumbersExtensions = new()
    {
        //картинки
        { new byte[3] { 255, 216, 255 }, (".jpg", 0) },
        { new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 }, (".png", 0) },
        { new byte[6] { 71, 73, 70, 56, 55, 97 }, (".gif", 0) },
        { new byte[6] { 71, 73, 70, 56, 57, 97 }, (".gif", 0) },
        { new byte[2] { 66, 77 }, (".bmp", 0) },
        { new byte[4] { 87, 69, 66, 80 }, (".webp", 8) },
        //відео
        { new byte[8] { 102, 116, 121, 112, 105, 115, 111, 109 }, (".mp4", 4) },
        { new byte[4] { 0, 0, 1, 179 }, (".mpg", 0) },
        { new byte[4] { 0, 0, 1, 186 }, (".mpg", 0) },
        { new byte[4] { 65, 86, 73, 32 }, (".avi", 8) },
        { new byte[16] { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 }, (".wmv", 0) },
        { new byte[4] { 26, 69, 223, 163 }, (".webm", 0) },
        //аудіо
        { new byte[2] { 255, 251 }, (".mp3", 0) },
        { new byte[2] { 255, 243 }, (".mp3", 0) },
        { new byte[2] { 255, 242 }, (".mp3", 0) },
        { new byte[3] { 73, 68, 51 }, (".mp3", 0) },
        { new byte[4] { 87, 65, 86, 69 }, (".wav", 8) },
        //документи
        { new byte[5] { 37, 80, 68, 70, 45 }, (".pdf", 0) },
        { new byte[8] { 65, 84, 38, 84, 70, 79, 82, 77 }, (".djvu", 0) },
        { new byte[6] { 123, 92, 114, 116, 102, 49 }, (".rtf", 0) },
        { new byte[4] { 80, 75, 3, 4 }, (".zip", 0) },
        { new byte[8] { 208, 207, 17, 224, 161, 177, 26, 225 }, (".doc", 0) },
        //файли
        { new byte[6] { 82, 97, 114, 33, 26, 7 }, (".rar", 0) },
        { new byte[6] { 55, 122, 188, 175, 39, 28}, (".7z", 0) }
    };
}
