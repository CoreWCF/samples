// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml;

namespace CoreWcf.Samples.CustomTextMessageEncoder
{
    public class CustomTextMessageBindingElement : MessageEncodingBindingElement
    {
        private MessageVersion _msgVersion;
        private string _mediaType;
        private string _encoding;
        private readonly XmlDictionaryReaderQuotas _readerQuotas;

        public CustomTextMessageBindingElement(CustomTextMessageBindingElement binding)
            : this(binding.Encoding, binding.MediaType, binding.MessageVersion)
        {
            _readerQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.CopyTo(_readerQuotas);
        }

        public CustomTextMessageBindingElement(string encoding, string mediaType, MessageVersion msgVersion)
        {
            _msgVersion = msgVersion ?? throw new ArgumentNullException(nameof(msgVersion));
            _mediaType = mediaType ?? throw new ArgumentNullException(nameof(mediaType));
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _readerQuotas = new XmlDictionaryReaderQuotas();
        }

        public CustomTextMessageBindingElement(string encoding, string mediaType)
            : this(encoding, mediaType, MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.WSAddressing10))
        {
        }

        public CustomTextMessageBindingElement(string encoding)
            : this(encoding, "text/xml")
        {
        }

        public CustomTextMessageBindingElement()
            : this("UTF-8")
        {
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return _msgVersion;
            }

            set
            {
                _msgVersion = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public string MediaType
        {
            get
            {
                return _mediaType;
            }

            set
            {
                _mediaType = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public string Encoding
        {
            get
            {
                return _encoding;
            }

            set
            {
                _encoding = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        // This encoder does not enforces any quotas for the unsecure messages. The 
        // quotas are enforced for the secure portions of messages when this encoder
        // is used in a binding that is configured with security. 
        public XmlDictionaryReaderQuotas ReaderQuotas => _readerQuotas;

        #region IMessageEncodingBindingElement Members

        public override MessageEncoderFactory CreateMessageEncoderFactory() => new CustomTextMessageEncoderFactory(MediaType, Encoding, MessageVersion);

        #endregion

        public override BindingElement Clone() => new CustomTextMessageBindingElement(this);

        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return (T)(object)_readerQuotas;
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

    }
}
