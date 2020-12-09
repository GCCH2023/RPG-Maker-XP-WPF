using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    public class File
    {
        internal static File open(string filename, string mode)
        {
            return new File(filename, mode);
        }

        byte[] fileData;
        File(string filename, string mode)
        {
            if (mode == "r" || mode == "rb")
            {
                this.fileData = System.IO.File.ReadAllBytes(filename);

                unsafe
                {
                    fixed (byte* p = this.fileData)
                    {
                        this.position = p;
                    }
                }

                FileInfo fi = new FileInfo(filename);
                this.mtime = fi.CreationTime;
            }
        }

        internal void close()
        {
            
        }

        internal static bool is_exist(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        public DateTime mtime { get; set; }

        public unsafe byte* position;
    }
}
