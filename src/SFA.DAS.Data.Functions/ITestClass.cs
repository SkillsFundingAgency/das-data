using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

namespace SFA.DAS.Data.Functions
{
    public interface ITestClass
    {
        void Write();
    }

    public class TestClass : ITestClass
    {
        public void Write()
        {
            Console.WriteLine("Written using testclass");

        }
    }
}
