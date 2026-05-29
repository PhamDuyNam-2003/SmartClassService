using System;
using System.Collections.Generic;
using System.Text;

namespace auth_service.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
       = string.Empty;
    }
}
