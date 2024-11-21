# FrequencyDictionary Application Setup and Usage Guide

This document provides instructions for setting up, running, and managing the `FrequencyDictionary` application, including required packages and configurations.

---

## **System Setup**

### 1. **Set the Startup Project**
- Open the project in Visual Studio.
- Right-click on your project in the **Solution Explorer**.
- Select **"Set as Startup Project"** to ensure this project runs when you press F5 or start debugging.

---

### 2. **Add Command-Line Arguments**
The application requires two command-line arguments:
1. The input file path (`input.txt`).
2. The output file path (`output.txt`).

#### Steps to Add Arguments:
1. Right-click on your project in **Solution Explorer** and select **"Properties"**.
2. Navigate to the **Debug** tab.
3. In the **Application Arguments** (or **Command-Line Arguments**) field, enter:
   ```
   input.txt output.txt
   ```

This ensures the application receives the required arguments during debugging.

---

## **Required Packages**

### 1. **System.Text.Encoding.CodePages**
This package provides support for code pages such as `windows-1252`.

#### Installation:
Use the following command in the **Package Manager Console** or terminal:
```
dotnet add package System.Text.Encoding.CodePages
```

#### Purpose:
- Enables handling of legacy encodings like `Windows-1252`.
- Required for reading and writing files with specific encoding.

---

## **Running the Application**

### 1. **Command-Line Execution**
Compile the project and run the executable in the terminal with the following command:
```
FrequencyDictionaryApp.exe input.txt output.txt
```

#### Explanation:
- `input.txt`: Path to the input file containing the text to analyze.
- `output.txt`: Path to the file where the word frequency results will be saved.

### 2. **Debugging in Visual Studio**
- Follow the steps in the **Add Command-Line Arguments** section.
- Press **F5** to start debugging.
- The program will read from `input.txt` and output the results to `output.txt`.

---

## **How the System Works**

1. **Input**:
   - Reads the content of the input file (`input.txt`) using `InputReader`.
   - Validates the file's existence and encoding (`Windows-1252`).

2. **Processing**:
   - Analyzes the file content with `FrequencyAnalyzer` to compute word frequencies.
   - Handles normalization (case-insensitivity) and parallelizes processing for scalability.

3. **Output**:
   - Writes the word frequencies to the specified output file (`output.txt`) using `OutputWriter`.
   - The output is in the format `{word},{frequency}`.

---

## **Supported Features**

- Reads files in `Windows-1252` encoding.
- Parallel processing for scalability on multicore systems.
- Comprehensive error handling:
  - Missing files.
  - Unsupported encoding.
  - Permission errors for output files.

---

## **System Requirements**

- **.NET 6.0 or later**.
- **NuGet Package**:
  - `System.Text.Encoding.CodePages`.

---

## **Testing**

The application includes unit tests to ensure robustness:
- **Framework**: xUnit.
- **Mocking**: Moq.
- **Assertions**: FluentAssertions.

Run tests with the following command:
```
dotnet test
```

---

## **Output Example**

### Input File (`input.txt`):
```
hello world hello
HELLO World
```

### Output File (`output.txt`):
```
hello,3
world,2
```

---

## **Notes**

- Ensure `input.txt` exists and is accessible before running the program.
- Verify permissions for writing to `output.txt`.
- For debugging, always set the correct **command-line arguments** in the Visual Studio Debug tab.


# FrequencyDictionary Application Architecture Documentation

This document provides an overview of the architecture and implementation details of the `FrequencyDictionary` application, focusing on its performance, scalability, modularity, unit tests, and extensibility.

---

## **1. Performance: Reasonable Economy of CPU and Memory Resources**

### Design Considerations:
- The application leverages efficient algorithms for word frequency analysis.
- Uses `ConcurrentDictionary` in the `FrequencyAnalyzer` for thread-safe, concurrent word frequency updates, minimizing locking overhead.
- Processes input files lazily (avoids holding unnecessary copies of data in memory).
- Sorting is deferred until the word frequencies have been aggregated, optimizing resource usage.

### Key Features:
- **Efficient Splitting**: Uses `String.Split` with specific delimiters to minimize processing overhead.
- **Lazy Evaluation**: Leverages `LINQ` to defer operations until absolutely necessary.
- **Resource Cleanup**: Proper use of `using` blocks ensures timely resource disposal.

---

## **2. Scalability on Multicore/Multiprocessor Systems**

### Design Considerations:
- The `Parallel.ForEach` construct is used in the `FrequencyAnalyzer` to distribute word processing across multiple CPU cores.
- Thread-safe operations are ensured through the use of `ConcurrentDictionary`, enabling parallel updates without performance bottlenecks.

### Benefits:
- Parallel execution enables faster processing for large files.
- Scales with the number of available cores, ensuring performance improvements on multicore systems.

---

## **3. Modularity and Unit Tests**

### Modular Design:
- The application adheres to the **Single Responsibility Principle (SRP)** by separating concerns across distinct components:
  1. **`InputReader`**: Handles file reading and validation.
  2. **`FrequencyAnalyzer`**: Processes text to compute word frequencies.
  3. **`OutputWriter`**: Manages writing the frequency dictionary to a file.

### Unit Testing:
- Comprehensive test coverage is implemented for each module, ensuring functionality and robustness.
- Mocking is used in integration tests to isolate dependencies and verify interactions between components.
- Frameworks:
  - **xUnit**: For writing unit tests.
  - **Moq**: For mocking dependencies.
  - **FluentAssertions**: For improved readability of test assertions.

---

## **4. Convenient Architecture for Future Expansion**

### Extensibility:
The design enables easy addition of new features without modifying existing code:
- **Input Sources**: Additional implementations of `IInputReader` can be added for new data sources, such as network streams or databases.
- **Analyzers**: New text analysis tools (e.g., character frequency analysis) can be integrated by implementing a new analyzer interface.
- **Output Formats**: Additional formats (e.g., JSON, XML) can be supported by creating new implementations of `IOutputWriter`.

### Dependency Injection:
The use of interfaces allows for seamless integration of new components while maintaining loose coupling.

---

## **5. Description of the Architecture**

### Architectural Overview:

#### Components:
1. **`InputReader`**:
   - Reads file content and ensures valid input (existence, encoding).
   - Encapsulates file operations to separate concerns from other components.
   
2. **`FrequencyAnalyzer`**:
   - Processes text to compute word frequencies.
   - Handles normalization (case-insensitivity) and efficient sorting.

3. **`OutputWriter`**:
   - Writes the frequency dictionary to an output file in the format `{word},{frequency}
`.
   - Validates the file path and handles permission errors.

#### Data Flow:
1. **File Input**:
   - `InputReader` reads the text from the input file.
   
2. **Frequency Analysis**:
   - `FrequencyAnalyzer` processes the text and calculates word frequencies.

3. **File Output**:
   - `OutputWriter` writes the results to the specified output file.

---

## **Conclusion**

The `FrequencyDictionary` application is designed with the following principles:
1. **Performance**: Optimized for both CPU and memory efficiency.
2. **Scalability**: Leverages multicore processing to handle large datasets.
3. **Modularity**: Ensures clean separation of concerns, enhancing testability and maintainability.
4. **Extensibility**: Easily adaptable for new features and functionalities.
5. **Robustness**: Backed by comprehensive unit tests to ensure reliability.

# Frequency Dictionary Tests Documentation

This README documents the test suite for the `FrequencyDictionary` application, detailing each test case, its purpose, and the results.

## Test Documentation Table

| **Test Name**                                             | **Description**                                                                                                                   | **Expected Result**                                                                                                           | **Actual Result**                                                                                     | **Test Status** |
|-----------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------
| `ReadFile_FileExists_ReturnsContent`                      | Verifies that `InputReader.ReadFile` successfully reads content from an existing file encoded in Windows-1252 encoding.           | The method returns the exact content of the file.                                                                             | The method returns the exact content of the file.                                                     | Yes             |
| `ReadFile_FileNotFound_ThrowsFileNotFoundException`       | Checks that `InputReader.ReadFile` throws a `FileNotFoundException` when the specified input file does not exist.                 | A `FileNotFoundException` is thrown.                                                                                          | A `FileNotFoundException` is thrown.                                                                  | Yes             |
| `GetWordFrequencies_ValidContent_ReturnsCorrectFrequencies` | Ensures that `FrequencyAnalyzer.GetWordFrequencies` correctly calculates word frequencies from valid input text.                | Returns a dictionary with correct word counts: `{ "hello": 2, "world": 1 }`.                                                  | Returns a dictionary with correct word counts: `{ "hello": 2, "world": 1 }`.                          | Yes             |
| `GetWordFrequencies_EmptyContent_ThrowsArgumentException` | Tests that `FrequencyAnalyzer.GetWordFrequencies` throws an `ArgumentException` when provided with empty content.                 | An `ArgumentException` is thrown.                                                                                             | An `ArgumentException` is thrown.                                                                     | Yes             |
| `GetWordFrequencies_SpecialCharacters_IgnoresWhitespace`  | Validates that `FrequencyAnalyzer.GetWordFrequencies` correctly processes text with irregular whitespace and newline characters.  | Returns a dictionary with correct word counts ignoring extra whitespace: `{ "hello": 2, "world": 1 }`.                        | Returns a dictionary with correct word counts ignoring extra whitespace: `{ "hello": 2, "world": 1 }`.| Yes             |
| `Main_ValidInputs_ProcessesSuccessfully`                  | Simulates the main program flow using mocks to ensure that all components interact correctly when provided with valid inputs.     | All methods (`ReadFile`, `GetWordFrequencies`, `WriteFrequencies`) are called once, and the process completes successfully.   | All methods are called once, and the process completes successfully without exceptions.               | Yes             |

---

## Detailed Test Descriptions

### 1. `ReadFile_FileExists_ReturnsContent`
- **Description**: Checks if `InputReader.ReadFile` can read a file that exists and is encoded in Windows-1252.
- **Expected Result**: The content read from the file should exactly match the content written to it.
- **Actual Result**: The method reads the file and returns the exact content.
- **Test Status**: **Yes** (Pass)

---

### 2. `ReadFile_FileNotFound_ThrowsFileNotFoundException`
- **Description**: Ensures `InputReader.ReadFile` throws the appropriate exception when the input file does not exist.
- **Expected Result**: A `FileNotFoundException` is thrown with a relevant error message.
- **Actual Result**: The method throws a `FileNotFoundException`.
- **Test Status**: **Yes** (Pass)

---

### 3. `GetWordFrequencies_ValidContent_ReturnsCorrectFrequencies`
- **Description**: Verifies that `FrequencyAnalyzer.GetWordFrequencies` returns the correct word frequencies for valid input text.
- **Expected Result**: A dictionary `{ "hello": 2, "world": 1 }`.
- **Actual Result**: The method returns `{ "hello": 2, "world": 1 }`.
- **Test Status**: **Yes** (Pass)

---

### 4. `GetWordFrequencies_EmptyContent_ThrowsArgumentException`
- **Description**: Tests how `FrequencyAnalyzer.GetWordFrequencies` handles empty input.
- **Expected Result**: An `ArgumentException` is thrown indicating that the input is invalid.
- **Actual Result**: The method throws an `ArgumentException`.
- **Test Status**: **Yes** (Pass)

---

### 5. `GetWordFrequencies_SpecialCharacters_IgnoresWhitespace`
- **Description**: Checks if the method correctly processes input text with extra spaces and newline characters.
- **Expected Result**: Word frequencies are calculated correctly, ignoring extra whitespace.
- **Actual Result**: The method returns `{ "hello": 2, "world": 1 }`, correctly ignoring the extra whitespace.
- **Test Status**: **Yes** (Pass)

---

### 6. `Main_ValidInputs_ProcessesSuccessfully`
- **Description**: Uses mock objects to simulate the main program execution and verify that all components work together as expected.
- **Expected Result**: All methods (`ReadFile`, `GetWordFrequencies`, `WriteFrequencies`) are called exactly once with the correct parameters.
- **Actual Result**: All methods are invoked as expected, and the process completes without errors.
- **Test Status**: **Yes** (Pass)

---

## Notes
- **Testing Environment**: The tests are written using the xUnit framework along with Moq for mocking dependencies and FluentAssertions for more readable assertions.
- **Dependencies**:
  - **xUnit**: A unit testing framework for .NET.
  - **Moq**: A mocking library for .NET to create mock objects.
  - **FluentAssertions**: Provides more intuitive assertion syntax.
- **Encoding Handling**: The tests ensure that encoding is properly handled by registering the code pages encoding provider where necessary.

---


                //await so that the thread reading text can be released
                //use stream reader to read content

                //this will improve performamnce

