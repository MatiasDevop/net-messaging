using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingContracts
{
    public record MovieCreated(Guid Id,
    string Title,
    string Slug,
    int YearOfRelease,
    List<string> Genres);
}
