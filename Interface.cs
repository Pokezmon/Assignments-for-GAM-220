using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixed_text_Adventure
{
    interface IInspectable
    {
        string Name { get; }
        string Description { get; }
        void Inspect();
    }

    interface IRoomResident : IInspectable { }
}

