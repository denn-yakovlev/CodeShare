using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.Model.Entities
{
    // основная сущность - сессия совместной работы
    public class Session
    {
        public string Id { get; set; }

        public Task? CurrentTask { get; set; }

        public ICollection<User> Collaborators { get; set; }
    }
}
