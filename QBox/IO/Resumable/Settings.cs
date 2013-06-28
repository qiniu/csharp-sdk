using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox.IO.Resumable
{
    /// <summary>
    /// 
    /// </summary>
    public class Settings
    {
        int taskQsize;

        public int TaskQsize
        {
            get { return taskQsize; }
            set { taskQsize = value; }
        }
        int workers;

        public int Workers
        {
            get { return workers; }
            set { workers = value; }
        }
        int chunkSize;

        public int ChunkSize
        {
            get { return chunkSize; }
            set { chunkSize = value; }
        }
        int tryTimes;

        public int TryTimes
        {
            get { return tryTimes; }
            set { tryTimes = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskQSize"></param>
        /// <param name="workers"></param>
        /// <param name="chunkSize"></param>
        /// <param name="tryTimes"></param>
        public Settings(int taskQSize, int workers, int chunkSize=1<<18, int tryTimes=3)
        {
            this.taskQsize = taskQSize;
            this.workers = workers;
            this.chunkSize = chunkSize;
            this.tryTimes = tryTimes; 
        }
    }
}
