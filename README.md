# WebScraper framework
---

Extracting data from Web easily. Web scraping framework for .NET
## About
What you are loking at is framework for extracting data from web pages. It aim is to provide simple but flexible tools for extracting, processing and storing data from web sites.
## Basic concepts
This framework gives you simple building blocks to manage data extraction from Web:
- Provider
- Extractor
- Transformer
- Splitter
- Aggregator
- Consumer

This blocks in common have input and output (some of them have only input, some of them have only output), that connects to each other. All of them connected makes a Task - data extracting unit. 
Input or output of such a block is Payload.

### Provider
Provides data to be splitted, processed further way. It's main goal is to generate data for processing, usual it will get page and return it's html for further processing.
It gets nothing on input and provides collection of Payloads. Usually it's starting point of task.

### Extractor
Extracts data from input using some extracting rules, for example CSS selector.
For input it gets one Payload and provides collection of Payloads for output.

### Transformer
Transforms Payload of one type to another type, using some mapping logic.
For input it gets one Payload and produces one Payload for output.

### Splitter
Splits input Payload to collection of Payloads, using some inner logic.
For input it gets one Payload and produces collection of Payloads for output.

### Aggregator
Aggregates input collection of Payloads to one Payload, using some inner logic.
For input it gets collection fo Payloads and produces one Payload for output.

### Consumer
Consumes input Payload and usually do some serialization of it (in file or database for example).
For input it gets Payload and produces nothing. Usually it's ending point of task.

## Roadmap
1. Make solution projects structure
2. Make interfaces for main blocks
3. Make basic implementations for task processing
4. Make sample implementations for some blocks and some working example.
5. ...
6. Make UI for visual creating tasks, testing and executing them.

## Disclaimer
For now project have just me as a developer, so it's quite slow-burner.
