# MXG
MGX - Minimalist XML Generator for .NET

**Note: This is not production ready yet. Testing in progress.**

## Description

This project is intended as small, fast and simple .NET (currently 4.6 or higher) library to create valid XML documents, that are according to the XML specifications and applicable to MSXML.

A main Goal is to reduce the generation time of XML documents. This has some **consequences**:
* The library has no XML scheme validation
* The library does not cover special conditions or exceptions, when it comes to XML
* The library has a small set of methods and properties
* To get better performance, it is important to know the estimated / expected number of elements per document, sub-element per element and attributes per element

## Performance

First benchmarks revealed a performance advantage of MXG over System.Xml, depending on the document size:
* **2-4 times faster** (small documents with few elements)
* **8-9 times faster** (large documents with many elements)

In general, MXG seems to be more efficient, when it comes to the handling of XML attributes. Large documents with many attributes result in a even higher performance.
However, these figures may vary, depending on hardware, software or test content.

The tests were performed by generating the same documents once by System.Xml and once by MXG.

## Known Issues
* Currently, a very high number of XML elements (>5000) in one XMl document can cause a crash of MXG. This issue is currently under investigation
* The library shows a better performance, when handling many attributes in few elements. This performance ratio flips on many elements and few attributes. This is under investigation

## License
MXG is published under the MIT license.
