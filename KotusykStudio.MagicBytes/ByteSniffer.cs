using System.IO.Compression;

namespace KotusykStudio.MagicBytes;

public static class ByteSniffer
{
    private const string _relsFile = "_rels/.rels";
    private const string _wordDocumentFile = "word/document.xml";
    private const string _workbookFile = "xl/workbook.xml";

    /// <summary>
    /// Try to get file extension by magic numbers in byte array.
    /// </summary>
    /// <param name="fileBytes">your file</param>
    /// <param name="extension">founded extension<br/>
    /// if unable to identify extension then variable will be ".unknown"</param>
    /// <returns>true if extension was successfully found<br/>
    /// false if extension is unknown</returns>
    public static bool TryGetFileExtension(byte[] fileBytes, out string extension)
    {
        extension = GetFileExtension(fileBytes);
        return extension != FileExtensions.Unknown;
    }

    /// <summary>
    /// Get file extension by magic numbers in byte array.
    /// </summary>
    /// <param name="fileBytes">your file</param>
    /// <returns>string extension with dot at beginning <br/>
    /// if unable to identify extension then return ".unknown"</returns>
    public static string GetFileExtension(byte[] fileBytes)
    {
        foreach (var magicNumber in _magicNumbersExtensions)
        {
            if (fileBytes.Length < magicNumber.Key.Length + magicNumber.Value.offset)
                continue;

            var match = true;
            for (var i = 0; i < magicNumber.Key.Length; i++)
            {
                if (magicNumber.Key[i] != fileBytes[i + magicNumber.Value.offset])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                var extension = magicNumber.Value.extension;
                if (extension == FileExtensions.Zip)
                {
                    using var memoryStream = new MemoryStream(fileBytes);
                    using var zip = new ZipArchive(memoryStream);
                    if (zip.Entries.Any(x => x.FullName.Contains(_relsFile)))
                    {
                        if (zip.Entries.Any(x => x.FullName.Contains(_wordDocumentFile)))
                        {
                            extension = FileExtensions.Docx;
                        }
                        else if (zip.Entries.Any(x => x.FullName.Contains(_workbookFile)))
                        {
                            extension = FileExtensions.Xlsx;
                        }
                    }
                }
                return extension;
            }
        }

        return FileExtensions.Unknown;
    }

    private static readonly Dictionary<byte[], (string extension, int offset)> _magicNumbersExtensions = new()
        {
            #region Images

            { new byte[3] { 255, 216, 255 }, (FileExtensions.Jpg, 0) },
            { new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 }, (FileExtensions.Png, 0) },
            { new byte[6] { 71, 73, 70, 56, 55, 97 }, (FileExtensions.Gif, 0) },
            { new byte[6] { 71, 73, 70, 56, 57, 97 }, (FileExtensions.Gif, 0) },
            { new byte[2] { 66, 77 }, (FileExtensions.Bmp, 0) },
            { new byte[4] { 87, 69, 66, 80 }, (FileExtensions.Webp, 8) },

            #endregion

            #region Videos

            { new byte[8] { 102, 116, 121, 112, 105, 115, 111, 109 }, (FileExtensions.Mp4, 4) },
            { new byte[4] { 0, 0, 1, 179 }, (FileExtensions.Mpg, 0) },
            { new byte[4] { 0, 0, 1, 186 }, (FileExtensions.Mpg, 0) },
            { new byte[4] { 65, 86, 73, 32 }, (FileExtensions.Avi, 8) },
            { new byte[16] { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 }, (FileExtensions.Wmv, 0) },
            { new byte[4] { 26, 69, 223, 163 }, (FileExtensions.Webm, 0) },

            #endregion

            #region Audio

            { new byte[2] { 255, 251 }, (FileExtensions.Mp3, 0) },
            { new byte[2] { 255, 243 }, (FileExtensions.Mp3, 0) },
            { new byte[2] { 255, 242 }, (FileExtensions.Mp3, 0) },
            { new byte[3] { 73, 68, 51 }, (FileExtensions.Mp3, 0) },
            { new byte[4] { 87, 65, 86, 69 }, (FileExtensions.Wav, 8) },

            #endregion

            #region Documents

            { new byte[5] { 37, 80, 68, 70, 45 }, (FileExtensions.Pdf, 0) },
            { new byte[8] { 65, 84, 38, 84, 70, 79, 82, 77 }, (FileExtensions.Djvu, 0) },
            { new byte[6] { 123, 92, 114, 116, 102, 49 }, (FileExtensions.Rtf, 0) },
            { new byte[8] { 208, 207, 17, 224, 161, 177, 26, 225 }, (FileExtensions.Doc, 0) },

            #endregion

            #region Archives

            { new byte[4] { 80, 75, 3, 4 }, (FileExtensions.Zip, 0) },
            { new byte[6] { 82, 97, 114, 33, 26, 7 }, (FileExtensions.Rar, 0) },
            { new byte[6] { 55, 122, 188, 175, 39, 28}, (FileExtensions.SevenZ, 0) }

            #endregion
        };
}
