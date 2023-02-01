// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Configuration;
using System.Xml;

namespace CoreWcf.Samples.CustomTextMessageEncoder
{
    public class CustomTextMessageEncodingElement : BindingElementExtensionElement
    {
        public CustomTextMessageEncodingElement()
        {
        }

        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            CustomTextMessageBindingElement binding = (CustomTextMessageBindingElement)bindingElement;
            binding.MessageVersion = MessageVersion;
            binding.MediaType = MediaType;
            binding.Encoding = Encoding;
            ApplyConfiguration(binding.ReaderQuotas);
        }

        private void ApplyConfiguration(XmlDictionaryReaderQuotas readerQuotas)
        {
            if (readerQuotas == null)
                throw new ArgumentNullException(nameof(readerQuotas));

            if (ReaderQuotasElement.MaxDepth != 0)
            {
                readerQuotas.MaxDepth = ReaderQuotasElement.MaxDepth;
            }
            if (ReaderQuotasElement.MaxStringContentLength != 0)
            {
                readerQuotas.MaxStringContentLength = ReaderQuotasElement.MaxStringContentLength;
            }
            if (ReaderQuotasElement.MaxArrayLength != 0)
            {
                readerQuotas.MaxArrayLength = ReaderQuotasElement.MaxArrayLength;
            }
            if (ReaderQuotasElement.MaxBytesPerRead != 0)
            {
                readerQuotas.MaxBytesPerRead = ReaderQuotasElement.MaxBytesPerRead;
            }
            if (ReaderQuotasElement.MaxNameTableCharCount != 0)
            {
                readerQuotas.MaxNameTableCharCount = ReaderQuotasElement.MaxNameTableCharCount;
            }
        }

        public override Type BindingElementType => typeof(CustomTextMessageBindingElement);

        protected override BindingElement CreateBindingElement()
        {
            CustomTextMessageBindingElement binding = new CustomTextMessageBindingElement();
            ApplyConfiguration(binding);
            return binding;
        }

        [ConfigurationProperty(ConfigurationStrings.MessageVersion,
            DefaultValue = ConfigurationStrings.DefaultMessageVersion)]
        [TypeConverter(typeof(MessageVersionConverter))]
        public MessageVersion MessageVersion
        {
            get
            {
                return (MessageVersion)base[ConfigurationStrings.MessageVersion];
            }
            set
            {
                base[ConfigurationStrings.MessageVersion] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.MediaType,
            DefaultValue = ConfigurationStrings.DefaultMediaType)]
        public string MediaType
        {
            get
            {
                return (string)base[ConfigurationStrings.MediaType];
            }

            set
            {
                base[ConfigurationStrings.MediaType] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.Encoding,
           DefaultValue = ConfigurationStrings.DefaultEncoding)]
        public string Encoding
        {
            get
            {
                return (string)base[ConfigurationStrings.Encoding];
            }

            set
            {
                base[ConfigurationStrings.Encoding] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.ReaderQuotas)]
        public XmlDictionaryReaderQuotasElement ReaderQuotasElement
        {
            get
            {
                return (XmlDictionaryReaderQuotasElement)base[ConfigurationStrings.ReaderQuotas];
            }
        }
    }
}
