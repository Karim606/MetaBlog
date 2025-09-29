using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveDataAttribute:Attribute
    {
        public string Mask { get; }
        public SensitiveDataAttribute(string mask="****Sensitive****") { 
        Mask = mask;
        }

    }
}
