﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ESTest
{
    class Program
    {
        private static void Main(string[] args)
        {
            //ESTestSetup is super slow
            ESTestSetup esTest = new ESTestSetup();
            esTest.SetUp();

            HyperLogLogTest hllTest = new HyperLogLogTest();
            hllTest.SetUp();

            AlgorithmTest algTest = new AlgorithmTest();
            algTest.SetUp();
        }
    }
}
