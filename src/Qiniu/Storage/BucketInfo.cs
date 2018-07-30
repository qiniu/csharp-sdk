namespace Qiniu.Storage
{
    /// <summary>
    ///     bucket info
    /// </summary>
    public class BucketInfo
    {
        /// <summary>
        ///     bucket name
        /// </summary>
        public string Tbl { get; set; }

        /// <summary>
        ///     itbl
        /// </summary>
        public long Itbl { get; set; }

        /// <summary>
        ///     deprecated
        /// </summary>
        public string Phy { get; set; }

        /// <summary>
        ///     id
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        ///     zone
        /// </summary>
        public string Zone { get; set; }

        /// <summary>
        ///     region
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        ///     isGlobal
        /// </summary>
        public bool Global { get; set; }

        /// <summary>
        ///     isLineStorage
        /// </summary>
        public bool Line { get; set; }

        /// <summary>
        ///     creationTime
        /// </summary>
        public long Ctime { get; set; }
    }
}
