namespace KotusykStudio.MagicBytes;

public static class ByteArrayExtensions
{
    /// <summary>
    /// Try to get file extension by magic numbers in byte array.
    /// </summary>
    /// <param name="fileBytes">your file</param>
    /// <param name="extension">founded extension<br/>
    /// if unable to identify extension then variable will be ".unknown"</param>
    /// <returns>true if extension was successfully found<br/>
    /// false if extension is unknown</returns>
    public static bool TryGetFileExtension(this byte[] fileBytes, out string extension)
    {
        extension = GetFileExtension(fileBytes);
        return extension != Extensions.Unknown;
    }

    /// <summary>
    /// Get file extension by magic numbers in byte array.
    /// </summary>
    /// <param name="fileBytes">your file</param>
    /// <returns>string extension with dot at beginning <br/>
    /// if unable to identify extension then return ".unknown"</returns>
    public static string GetFileExtension(this byte[] fileBytes) => ByteSniffer.GetFileExtension(fileBytes);
}
