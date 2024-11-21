using System;

namespace AppAPI.Models.Domain
{
    public class UserAudit
    {
        public Guid UserAuditId { get; set; }

        public Guid UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public User User { get; set; }
    }
}
