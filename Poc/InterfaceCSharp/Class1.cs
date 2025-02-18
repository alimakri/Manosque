using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceCSharp
{
    public interface IA
    {
        public void M1();
        public int M2(string s);
    }
    public class A : IA
    {
        public void M1()
        {

        }

        public int M2(string t)
        {
            return 0;
        }
    }
    public class B : IA
    {
        public void M1()
        {

        }

        public int M2(string t)
        {
            return 0;
        }
    }
}
