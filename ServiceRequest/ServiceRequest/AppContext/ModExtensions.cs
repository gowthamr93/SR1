using System;

namespace ServiceRequest.AppContext
{
    public static class ModExtensions
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        private const int length = 26;
		private const int asciiValue = 65;

        #endregion

		#region Public Function
        ///------------------------------------------------------------------------------------------
		/// <summary>
		/// Name : Indexs to alphabet.
		/// To covert the numbers into its corresponding character
		/// </summary>
		/// <returns>The to alphabet.</returns>
		/// <param name="index">Index.</param>
		///------------------------------------------------------------------------------------------

        public static string IndexToAlphabet(this int index)
        {
           string str = "";
			char achar;
			int mod;
			while (true)
			{
				mod = (index % length) + asciiValue;
				index = (int)(index / length);
				achar = (char)mod;
				str = achar + str;
				if (index > 0) index--;
				else if (index == 0) break;
			}
			return str;
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		GetContentType
        /// 
        /// <summary>	Gets the content type mime for the particular file extension.
        /// </summary>
        /// <param name="fileExtension">	The file extension.</param>
        /// 
        /// <returns>	string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string GetContentType(this string fileExtension)
        {
            try
            {
                string sContentType;
                //
                switch (fileExtension)
                {
                    case "bmp":
                        sContentType = "image/bmp";
                        break;
                    case "csv":
                        sContentType = "text/plain";
                        break;
                    case "doc":
                    case "docx":
                        sContentType = "application/msword";
                        break;
                    case "dxf":
                        sContentType = "application/dxf";
                        break;
                    case "dwg":
                        sContentType = "application/dwg";
                        break;
                    case "dwf":
                        sContentType = "application/dwf";
                        break;
                    case "eps":
                        sContentType = "image/eps";
                        break;
                    case "gif":
                        sContentType = "image/gif";
                        break;
                    case "jpeg":
                    case "jpg":
                        sContentType = "image/jpeg";
                        break;
                    case "pdf":
                        sContentType = "application/pdf";
                        break;
                    case "png":
                        sContentType = "image/png";
                        break;
                    case "rtf":
                        sContentType = "application/rtf";
                        break;
                    case "tif":
                    case "tiff":
                        sContentType = "image/tiff";
                        break;
                    case "mp3":
                    case "wav":
                        sContentType = "audio/mp3";
                        break;
                    case "mp4":
                        sContentType = "video/mp4";
                        break;
                    case "txt":
                        sContentType = "text/plain";
                        break;
                    case "xls":
                    case "xlsx":
                        sContentType = "application/msexcel";
                        break;
                    default:
                        sContentType = "text/plain";
                        break;
                }
                return sContentType;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocumentType
        /// 
        /// <summary>	Gets the document type mime for the particular file extension.
        /// </summary>
        /// <param name="fileExtension">	The file extension.</param>
        /// 
        /// <returns>	string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string GetDocumentType(this string fileExtension)
        {
            try
            {
                string source;
                //
                switch (fileExtension)
                {
                    
                    case ".txt":
                    case ".csv":
                        source = "txt.png";
                        break;
                    case ".doc":
                    case ".docx":
                        source = "doc.png";
                        break;
                    case ".gif":
                    case ".jpeg":
                    case ".jpg":
                    case ".png":
                    case ".tiff":
                    case ".tif":
                    case ".bmp":
                        source = "png.png";
                        break;
                    case ".pdf":
                        source = "pdf.png";
                        break;
                    case ".mp3":
                    case ".wav":
                        source = "mp3.png";
                        break;
                    case ".mp4":
                        source = "mp4.png";
                        break;
                    case ".xls":
                    case ".xlsx":
                        source = "xls.png";
                        break;
                    default:
                        source = "others.png";
                        break;
                }
                return source;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
