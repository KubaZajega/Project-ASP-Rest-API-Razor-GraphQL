using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jakub_Zajega_14987.Application.DTOs.GraphQL;
public class GqlUsersConnection { public List<GqlUserNode> Nodes { get; set; } = new(); }