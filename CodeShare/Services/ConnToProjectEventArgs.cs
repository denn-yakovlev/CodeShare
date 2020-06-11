using CodeShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services
{
    public class ConnToProjectEventArgs : EventArgs
    {
        public string ProjectId { get; set; }
    }
}
