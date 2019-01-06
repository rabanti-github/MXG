# MXG
MGX - Minimalist XML Generator for C#

<b>Note: This is not production ready yet. Testing in progress.</b>

Exact Description: [TBD]

This project is intended as small, fast and simple library to create valid XML documents, that are according to the XML specifications.
A main Goal is to reduce the generation time of XML documents. This has some consequences:
* The library has no XML scheme validation
* The library does not cover special conditions or exceptions, when it comes to XML
* The library has a small set of methods and properties
* To get better performance, it is important to know the estimated / expected number of elements per document, sub-element per element and attributes per element
