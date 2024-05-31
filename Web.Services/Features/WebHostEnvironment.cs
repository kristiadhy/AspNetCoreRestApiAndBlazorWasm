using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Services.Features;
public record WebHostEnvironment(bool IsDevelopment, bool IsProduction)
{
}
