﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Interfaces
{
    public interface ICurrentRequestContext
    {
        string? IpAddress { get; }
        string? DeviceInfo { get; }

    }
}
