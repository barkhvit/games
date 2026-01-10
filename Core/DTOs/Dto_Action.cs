
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.DTOs
{
    public static class Dto_Action
    {
        //views
        public static string Show { get; } = nameof(Show);

        //REQUESTS
        public static string Go { get; } = nameof(Go);
        public static string Stop { get; } = nameof(Stop);
    }
}
