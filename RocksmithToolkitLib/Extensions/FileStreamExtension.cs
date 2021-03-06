﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RocksmithToolkitLib.Extensions
{
    public class TempFileStream : FileStream
    {
        static int _buffer_size = 65536;
        public TempFileStream()
            : base(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite, FileShare.Read, _buffer_size, FileOptions.DeleteOnClose) { }

        public TempFileStream(FileAccess access)
            : base(Path.GetTempFileName(), FileMode.Create, access, FileShare.Read, _buffer_size, FileOptions.DeleteOnClose) { }

        public TempFileStream(FileAccess access, FileShare share)
            : base(Path.GetTempFileName(), FileMode.Create, access, share, _buffer_size, FileOptions.DeleteOnClose) { }

        public TempFileStream(FileAccess access, FileShare share, int bufferSize)
            : base(Path.GetTempFileName(), FileMode.Create, access, share, bufferSize, FileOptions.DeleteOnClose) { }
    }

    public class XmlStreamingDeserializer<T>
    {
        static XmlSerializerNamespaces _ns;
        XmlSerializer _serializer = new XmlSerializer(typeof(T));
        XmlReader _reader;

        static XmlStreamingDeserializer()
        {
            _ns = new XmlSerializerNamespaces();
            _ns.Add("", "");
        }

        private XmlStreamingDeserializer()
        {
            _serializer = new XmlSerializer(typeof(T));
        }
        
        public XmlStreamingDeserializer(TextReader reader)
            : this(XmlReader.Create(reader))
        {
        }
        
        public XmlStreamingDeserializer(XmlReader reader)
            : this()
        {
            _reader = reader;
        }
        
        public void Close()
        {
            _reader.Close();
        }
        
        public T Deserialize()
        {
            while (_reader.IsStartElement())
            {
                    XmlReader reader = _reader.ReadSubtree();
                    return (T)_serializer.Deserialize(reader);
            }
            return default(T);
        }
    }
}
