using System.Collections.Generic;

namespace Pack
{
    public class Entry
    {
        public string filename;
        public long fileSize;
        public List<byte> content;
    }

    public class Pack
    {
        public List<Entry> content;
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
    }


}
