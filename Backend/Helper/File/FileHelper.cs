using System;

namespace Helper.File
{
    public class FileHelper
    {
        /// <summary>
        /// Check if a given filename is a valid one
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsValidFileName(string filename)
        {
            System.IO.FileInfo fi = null;
            try
            {
                fi = new System.IO.FileInfo(filename);
            }
            catch (ArgumentException) { }
            catch (System.IO.PathTooLongException) { }
            catch (NotSupportedException) { }
            return !ReferenceEquals(fi, null);
        }
    }
}
