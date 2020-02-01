using AuthenticationService.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain
{
    public class UserAudit : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public DateTimeOffset Timestamp { get; set; } = DateTime.UtcNow;

        public UserAuditEventType AuditEvent { get; set; }

        public string IpAddress { get; set; }

        public string Description { get; set; }

        public string Headers { get; set; }

        public static UserAudit CreateAuditEvent(string userId, UserAuditEventType auditEventType, string ipAddress, string description = "", string headers = "")
            => new UserAudit
            {
                UserId = userId,
                AuditEvent = auditEventType,
                IpAddress = ipAddress,
                Description = description,
                Headers = headers
            };
    }

    
}
