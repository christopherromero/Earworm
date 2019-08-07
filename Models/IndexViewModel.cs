using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Earworm.Models
{
    public class IndexViewModel
    {
        public string HelloWorld;

        public void OnGetAsync()
        {
            HelloWorld = "HelloWorld!";
        }
    }
}
