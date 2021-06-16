﻿using Baetoti.Core.Entites.Base;
using System;

namespace Baetoti.Core.Entites
{
    public class AppException : BaseEntity
    {
        public string UserId { get; set; } //null if exception occur before authentication

        public string Url { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public string InnerException { get; set; }

        public string StackTrace { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
