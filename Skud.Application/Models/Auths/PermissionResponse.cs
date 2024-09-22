using Skud.Application.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skud.Application.Models.Auths;
public class PermissionResponse : BaseDateResponse
{
    public int Id { get; set; } 
    public string Name { get; set; }
}
