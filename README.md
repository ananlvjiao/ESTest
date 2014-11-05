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


Use C# implementation from http://adnan-korkmaz.blogspot.com/2012/06/hyperloglog-c-implementation.html

For 100,000 deviceIDs (GUID), when set the standard error to be 0.02, the size of hyperloglog dictionary in binary file is 70KB; error rate is around 0.3%,the size of the index is 20MB
