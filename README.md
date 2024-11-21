# ConsoleApp

# FrequencyDictionary Application Setup and Usage Guide
This dreadme file provides instructions for setting up and running `FrequencyDictionary` application, including required packages and configurations.

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
