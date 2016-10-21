using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Security;
using System.Xml.Serialization;
using SubSonic;

namespace Wcss
{
    public partial class Download 
    {
        public static void EnsureDownloadDirectories(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        private static string _baseDirectory = null;
        /// <summary>
        /// This is also the FTP directory
        /// </summary>
        public static string BaseDirectory
        {
            get
            {
                if (_baseDirectory == null)
                    _baseDirectory = string.Format(@"{0}\{1}\", _Config._DownloadDirectory, _Config.APPLICATION_NAME);
                return _baseDirectory;
            }
        }

        /// <summary>
        /// ensures directories are created
        /// </summary>
        /// <param name="currentAbsolutePath_NoFile"></param>
        public void MoveFileToCalculatedPath(string currentAbsolutePath_NoFile)
        {   
            string path = this.CalculatedPath;

            if (path != currentAbsolutePath_NoFile)
            {
                string mappedCurrent = string.Format(@"{0}{1}", currentAbsolutePath_NoFile, this.FileName);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (File.Exists(mappedCurrent))
                {
                    string mappedDestination = string.Format(@"{0}{1}", this.CalculatedPath, this.FileName);
                    File.Move(mappedCurrent, mappedDestination);
                }
                else
                    throw new FileNotFoundException();

                if (this.SampleFile != null)
                {
                    string mappedSample = string.Format(@"{0}{1}", currentAbsolutePath_NoFile, this.SampleFile);

                    if (File.Exists(mappedSample))
                    {
                        string mappedSampleDestination = string.Format(@"{0}{1}", this.CalculatedPath, this.SampleFile);
                        File.Move(mappedSample, mappedSampleDestination);
                    }
                    else
                        throw new FileNotFoundException();
                }
            }
        }

        //TrackNumber
        //Title
        //vcFileContext
        //vcTrackContext
        //vcGenre
        //vcKeywords
        //TActId
        //BaseStoragePath
        //Compilation
        //Artist
        //Album
        //FileName
        //vcFormat
        //FileBinary
        //iFileSeconds
        //iFileBytes
        //SampleFile
        //iSampleClick
        //iAttempted
        //iSuccessful
        //dtStamp
        //dtLastValidated
        //Application Id



        /// <summary>
        /// Tells us if it is a music file, data file, picture, report, etc
        /// </summary>
        [XmlAttribute("FileContext")]
        public _Enums.DownloadFileContext FileContext
        {
            get 
            { 
                return (this.VcFileContext != null) ? 
                (_Enums.DownloadFileContext)Enum.Parse(typeof(_Enums.DownloadFileContext), this.VcFileContext, true) : 
                _Enums.DownloadFileContext._NA; 
            }
            set
            {
                if (value == _Enums.DownloadFileContext._NA)
                    this.VcFileContext = null;
                else
                    this.VcFileContext = value.ToString();
            }
        }
        /// <summary>
        /// Refers to singletrack, fullalbum, side1, side2
        /// </summary>
        [XmlAttribute("TrackContext")]
        public _Enums.DownloadTrackContext TrackContext
        {
            get
            {
                return (this.VcTrackContext != null) ?
                (_Enums.DownloadTrackContext)Enum.Parse(typeof(_Enums.DownloadTrackContext), this.VcTrackContext, true) :
                _Enums.DownloadTrackContext._NA;
            }
            set
            {
                if (value == _Enums.DownloadTrackContext._NA)
                    this.VcTrackContext = null;
                else
                    this.VcTrackContext = value.ToString();
            }
        }

        private List<string> _genreList = null;
        /// <summary>
        /// A comma separated list of applicable genres
        /// </summary>
        [XmlAttribute("GenreList")]
        public List<string> GenreList
        {
            get
            {
                if (_genreList == null)
                {
                    _genreList = new List<string>();

                    if (this.VcGenre != null && this.VcGenre.Length > 0)
                    {
                        string[] parts = this.VcGenre.Split(',');
                        _genreList.AddRange(parts);
                    }
                }

                return _genreList;
            }
            set
            {
                if (value.Count == 0)
                    this.VcGenre = null;
                else
                    this.VcGenre = Utils.ParseHelper.SplitListIntoString<string>(value, false);

                _genreList = null;
            }
        }
        public void GenreList_Add(string genre)
        {
            if (!GenreList.Contains(genre))
            {
                GenreList.Add(genre);
                this.GenreList = GenreList;
            }
        }
        public void GenreList_Delete(string genre)
        {
            if (GenreList.Contains(genre))
            {
                GenreList.Remove(genre);
                this.GenreList = GenreList;
            }
        }

        private List<string> _keywordList = null;
        /// <summary>
        /// A comma separated list of keywords that are applicable to the download
        /// </summary>
        [XmlAttribute("KeywordList")]
        public List<string> KeywordList
        {
            get
            {
                if (_keywordList == null)
                {
                    _keywordList = new List<string>();

                    if (this.VcKeywords != null && this.VcKeywords.Length > 0)
                    {
                        string[] parts = this.VcKeywords.Split(',');
                        _keywordList.AddRange(parts);
                    }
                }

                return _keywordList;
            }
            set
            {
                if (value.Count == 0)
                    this.VcKeywords = null;
                else
                    this.VcKeywords = Utils.ParseHelper.SplitListIntoString<string>(value, false);

                _keywordList = null;
            }
        }
        public void KeywordList_Add(string keyword)
        {
            if (!KeywordList.Contains(keyword))
            {
                KeywordList.Add(keyword);
                this.KeywordList = KeywordList;
            }
        }
        public void KeywordList_Delete(string keyword)
        {
            if (KeywordList.Contains(keyword))
            {
                KeywordList.Remove(keyword);
                this.KeywordList = KeywordList;
            }
        }
        /// <summary>
        /// NOT Virtual - due to security concerns the config download directory is absolute. Defines where the file is stored - calculated from Base, Compilation, Artist, Album. 
        /// Does NOT include filename
        /// </summary>
        [XmlAttribute("CalculatedPath")]
        public string CalculatedPath
        {
            get 
            {
                return string.Format(@"{0}{1}{2}{3}", Download.BaseDirectory, 
                    (this.Compilation != null && this.Compilation.Trim().Length > 0) ? string.Format(@"{0}\", this.Compilation.Trim()) : string.Empty, 
                    (this.Artist != null && this.Artist.Trim().Length > 0) ? string.Format(@"{0}\", this.Artist.Trim()) : string.Empty, 
                    (this.Album != null && this.Album.Trim().Length > 0) ? string.Format(@"{0}\", this.Album.Trim()) : string.Empty
                    );
            }
        }
        /// <summary>
        /// The path and file name of the file
        /// </summary>
        [XmlAttribute("FileLocation")]
        public string FileLocation
        {
            get
            {
                return string.Format("{0}{1}", this.CalculatedPath, this.FileName);
            }
        }
        /// <summary>
        /// Csv, Mp3, Ogg Vorbis, jpg, tiff
        /// </summary>
        [XmlAttribute("Format")]
        public _Enums.DownloadFormat Format
        {
            get
            {
                return (this.VcFormat != null) ?
                (_Enums.DownloadFormat)Enum.Parse(typeof(_Enums.DownloadFormat), this.VcFormat, true) :
                _Enums.DownloadFormat._NA;
            }
            set
            {
                if (value == _Enums.DownloadFormat._NA)
                    this.VcFormat = null;
                else
                    this.VcFormat = value.ToString();
            }
        }
        ///// <summary>
        ///// For music files - tells us the time of the file
        ///// </summary>
        //[XmlAttribute("FileSeconds")]
        //public int FileSeconds
        //{
        //    get { return this.IFileSeconds; }
        //    set { this.IFileSeconds = value;  }
        //}
        /// <summary>
        /// Size of file in bytes
        /// </summary>
        [XmlAttribute("FileBytes")]
        public int FileBytes
        {
            get { return this.IFileBytes; }
            set { this.IFileBytes = value; }
        }
        /// <summary>
        /// Number of times a sample has been clicked on
        /// </summary>
        [XmlAttribute("SampleClick")]
        public int SampleClick
        {
            get { return this.ISampleClick; }
            set { this.ISampleClick = value; }
        }
        /// <summary>
        /// Number of download attempts
        /// </summary>
        [XmlAttribute("Attempted")]
        public int Attempted
        {
            get { return this.IAttempted; }
            set { this.IAttempted = value; }
        }
        /// <summary>
        /// Number of successful downloads
        /// </summary>
        [XmlAttribute("Successful")]
        public int Successful
        {
            get { return this.ISuccessful; }
            set { this.ISuccessful = value; }
        }
        /// <summary>
        /// Holds a date for the last time file associations, etc were verified. Use for house keeping
        /// </summary>
        [XmlAttribute("DateLastValidated")]
        public DateTime DateLastValidated
        {
            get
            {
                if (!this.DtLastValidated.HasValue)
                    return DateTime.MaxValue;//has never been validated

                return this.DtLastValidated.Value;
            }
            set { this.DtLastValidated = value; }
        }
    }
}
