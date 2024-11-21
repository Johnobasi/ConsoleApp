# ConsoleApp

# FrequencyDictionary Application Setup and Usage Guide
This readme file provides instructions for setting up and running `FrequencyDictionary` application which also included packages and configurations.

### System Setup

#### 1. Set the Startup Project
- Open visual studio
- Ensure .Net 8 is installed

#### 2. Add Command-Line Arguments
The application requires two command-line arguments required arguments during debugging:
1. The input file path (`input.txt`).
2. The output file path (`output.txt`).

#### Steps i used to add Arguments:
1. I right-click on your project in 'Solution Explorer' and select 'Properties'.
2. I navigate to the 'Debug' tab.
3. In the 'Application Arguments' (or 'Command-Line Arguments') field, i entered:
   ```
   input.txt output.txt
   ```
## Required Packages
```
dotnet add package System.Text.Encoding.CodePages
```
#### Purpose:
- I added below package
- dotnet add package System.Text.Encoding.CodePages
- Reason is to enables handling of encodings `Windows-1252` as speocfied in the requirement.


### UML Diagram for the project
+---------------------+          +---------------------+          +---------------------+
|    IInputFileReader |          | IFrequencyProcessor|          |   IOutputFileWriter |
+---------------------+          +---------------------+          +---------------------+
| + ReadFileInChunksAsync() |    | + GetWordFrequenciesAsync()|   | + WriteFrequenciesAsync() |
+---------------------+          +---------------------+          +---------------------+
         ^                               ^                                ^
         |                               |                                |
+---------------------+          +---------------------+          +---------------------+
|   InputFileReader   |          |  FrequencyProcessor |          |   OutputFileWriter  |
+---------------------+          +---------------------+          +---------------------+
| + ReadFileInChunksAsync() |    | + GetWordFrequenciesAsync()|   | + WriteFrequenciesAsync() |
|                          |     | + Regex pattern matching   |   | + WriteBatchAsync() |
+---------------------+          +---------------------+          +---------------------+

                                     +----------------+
                                     |     Program     |
                                     +----------------+
                                     | + Main()        |
                                     |                 |
                                     +----------------+


### Explanation of the Classes and escription of the Architecture
##. IInputFileReader Interface:
- Defines all interfaces used for reading files in chunks as asynchronous streams (IAsyncEnumerable<string>).

#Implementation:
- InputFileReader reads a file line by line and supports multiple encodings.
- It handles Windows-1252 encoding and falls back to UTF-8 when necessary.
- While implementing InputFileReader, I encountered issues with files encoded in an unexpected format. 
Adding a fallback to UTF-8 resolved the problem.

##. IFrequencyProcessor Interface:
- Defines the contract for processing wor frequencie.

#Implementation:
FrequencyProcessor:
- Processes asynchronous streams of lines.
- Matches words using a Unicode-compatible regex (\b\p{L}+\b).
- Uses ConcurrentDictionary for thread-safety.
- Aggregates word frequencies using a thread-safe ConcurrentDictionary.
- Logs unsupported words or characters for debugging purposes.
- Leverages parallel processing (Parallel.ForEachAsync) to improve performance on large datasets.
- For example, when processing a file with the following content: Hello World Hello, the output will correctly show hello: 2, world: 1.

##3. IOutputFileWriter Interface:
Defines the contract for writing word frequencies to a file.

#Implementation:
OutputFileWriter:
- Writes word frequencies to a file in batches for optimized performance.
- Uses StreamWriter and Task.WhenAll to write concurrently.
- Supports customizable batch sizes

##4. Program

#Purpose:
- Serve as entry point for the application.
- Dependency injection (DI) to resolve IInputFileReader, IFrequencyProcessor, and IOutputFileWriter implementations.
- Reads file chunks.
- Processes word frequencies.
- Writes results to the output file.


###Design Principles used
- Separation of Concerns : Logisc for read and write and seperated
- Dependency Injection : injected the interfaces in DI
- Uses asynchronous streams (IAsyncEnumerable) and parallel processing to handle large files efficiently.

##Future Expansion
- Support for Additional Input and output format
- Enhanced Processing
- UI