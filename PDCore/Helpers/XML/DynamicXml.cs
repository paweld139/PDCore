using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PDCore.Helpers.XML
{
    public class DynamicXml : DynamicObject, IEnumerable
    {
        private readonly dynamic _xml;

        public DynamicXml(string fileName)
        {
            _xml = XDocument.Load(fileName);
        }

        public DynamicXml(dynamic xml)
        {
            _xml = xml;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var xml = _xml.Element(binder.Name);

            if (xml != null)
            {
                result = new DynamicXml(xml);

                return true;
            }

            result = null;

            return false;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var child in _xml.Elements())
            {
                yield return new DynamicXml(child);
            }
        }

        public static implicit operator string(DynamicXml xml)
        {
            return xml._xml.Value;
        }
    }
}
