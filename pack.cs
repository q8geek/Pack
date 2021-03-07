using System;

namespace Pack
{
    public class Entry
    {
        public string filename;
        public long fileSize;
        public List<byte> content;

        public string GetMD5()
        {
            byte[] hash = MD5.Create().ComputeHash(content.ToArray());
            string retString = "";
            foreach (byte b in hash)
            {
                uint u = Convert.ToUInt32(b);
                retString += u.ToString("X2");
            }
            return retString;
        }
    }

    public class Pack
    {
        public List<Entry> content;
        public string index;
        public string name;


        public long GetBiggestFile()
        {
            long retLong = 0;
            foreach (Entry entry in content)
            {
                if (entry.fileSize > retLong)
                    retLong = entry.fileSize;
            }

            return retLong;
        }


        public string GetMD5()
        {
            List<byte> allBytes = new List<byte>();
            foreach (Entry e in content)
            {
                allBytes.AddRange(e.content);
            }
            byte[] hash = MD5.Create().ComputeHash(allBytes.ToArray());
            string retString = "";
            foreach (byte b in hash)
            {
                uint u = Convert.ToUInt32(b);
                retString += u.ToString("X2");
            }
            return retString;
        }
    }


}
