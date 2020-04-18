using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Share
{
    [Serializable]
    public class Packet
    {
        public List<string> PData;
        public int PNum;
        public string PSenderID;
        public PacketType PType;

        public Packet(PacketType _packetType, string _id)
        {
            this.PData = new List<string>();
            this.PSenderID = _id;
            this.PType = _packetType;
        }
        public Packet(byte[] _packetBytes)
        {

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(_packetBytes);

            Packet p = (Packet)bf.Deserialize(ms);
            ms.Close();
            this.PData = p.PData;
            this.PNum = p.PNum;
            this.PSenderID = p.PSenderID;
            this.PType = p.PType;
        }
        public byte[] ToBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, this);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
    }
}
