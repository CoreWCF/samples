// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Xml;

namespace CoreWcf.Samples.CustomTextMessageEncoder
{
    public class CustomTextMessageEncoder : MessageEncoder
    {
        private readonly CustomTextMessageEncoderFactory _factory;
        private readonly XmlWriterSettings _writerSettings;
        private readonly string _contentType;

        public CustomTextMessageEncoder(CustomTextMessageEncoderFactory factory)
        {
            _factory = factory;
            _writerSettings = new XmlWriterSettings();
            _writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
            _contentType = string.Format("{0}; charset={1}", _factory.MediaType, _writerSettings.Encoding.HeaderName);
        }

        public override string ContentType => _contentType;

        public override string MediaType => _factory.MediaType;

        public override MessageVersion MessageVersion => _factory.MessageVersion;

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            return ReadMessage(msgContents, bufferManager);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            XmlReader reader = XmlReader.Create(stream);
            Message message = Message.CreateMessage(reader, maxSizeOfHeaders, MessageVersion);
            return message;
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, _writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, _writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }
    }
}
