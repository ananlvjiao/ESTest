ESTest
======

ElasticSearch Test Project


ESTestSetUp
==============================


[Index and mappings set up ] (/docs/TSTestSetup.txt)


HyperLogLogTest
==============================

[Index and mappings set up ] (/docs/HyperLogLogTest.txt)

[Index stats] (/docs/hlltest_stats.json)

[Index dump] (/docs/hlltestdump.json)

C# implementation
--------------------------------
Use C# implementation from http://adnan-korkmaz.blogspot.com/2012/06/hyperloglog-c-implementation.html 

Error rate, map size, and numbers of cardinality 

[Test Result] (/docs/HyperLogLogTestResult.text)
The test result lists based on different standard error, what the map size is; and it also lists for different cardinality, (10 times) average error rate, min error rate and max error rate.

The C# implementation requires to pass the value of the param "stdError"(standard error) to initialize the HyperLogLog. The standard error and map size relationship is defined in paper http://algo.inria.fr/flajolet/Publications/FlFuGaMe07.pdf
stdError = 1.04/√m

The hash function output is 32bits in the C# implementation. 


Redis implementation 
----------------------------------
https://github.com/antirez/redis/blob/unstable/src/hyperloglog.c

using the params: m = 16384, 64 bits hash output, standard error = 0.0081


