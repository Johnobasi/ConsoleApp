using ConsoleApp.Abstracts;
using ConsoleApp.Services;
using FluentAssertions;
using Moq;
using System.Text;

namespace ConsoleAppTest
{
    public class FrequencyDictionaryTests
    {
        private readonly Mock<IInputFileReader> _mockInputReader;
        private readonly Mock<IFrequencyProcessor> _mockFrequencyAnalyzer;
        private readonly Mock<IOutputFileWriter> _mockOutputWriter;
        private readonly InputFileReader _inputFileReader;
        private readonly FrequencyProcessor _frequencyProcessor;
        private readonly OutputFileWriter _outputFileWriter;
        public FrequencyDictionaryTests()
        {
            _mockInputReader = new Mock<IInputFileReader>();
            _mockFrequencyAnalyzer = new Mock<IFrequencyProcessor>();
            _mockOutputWriter = new Mock<IOutputFileWriter>();
            _inputFileReader = new InputFileReader();
            _frequencyProcessor = new FrequencyProcessor();
            _outputFileWriter = new OutputFileWriter();
        }

        [Fact]
        public async Task ReadFile_FileExists_ReturnsContent()
        {
            // Arrange
            var filePath = "test.txt";
            var fileContent = "Hello World";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(fileStream, Encoding.GetEncoding("Windows-1252")))
            {
                await writer.WriteAsync(fileContent);
            }

            try
            {
                // Act: Read the content using the InputFileReader
                var resultLines = new List<string>();
                await foreach (var line in _inputFileReader.ReadFileInChunksAsync(filePath))
                {
                    resultLines.Add(line);
                }

                var result = string.Join(" ", resultLines); // Combine lines into a single string for comparison

                // Assert
                result.Should().Be(fileContent);
            }
            finally
            {
                // Cleanup test file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Fact]
        public async Task ReadFile_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            var filePath = "nonexistent.txt";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FileNotFoundException>(async () =>
            {
                await foreach (var _ in _inputFileReader.ReadFileInChunksAsync(filePath))
                {
                    // This block won't execute because the exception will be thrown first
                }
            });

            //Assert
            exception.Message.Should().Be($"Input file not found: {filePath}");
        }

        [Fact]
        public async Task GetWordFrequencies_ValidContent_ReturnsCorrectFrequencies()
        {
            var content = GetAsyncEnumerable(["hello world hello"]);// "hello world hello";
            var expected = new Dictionary<string, int>
            {
                { "hello", 2 },
                { "world", 1 }
            };

            var result = await _frequencyProcessor.GetWordFrequenciesAsync(content);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWordFrequencies_SpecialCharacters_IgnoresWhitespace()
        {
            // Arrange
            var content = GetAsyncEnumerable(["hello    world\nhello"]);
            var expected = new Dictionary<string, int>
            {
                { "hello", 2 },
                { "world", 1 }
            };

            // Act
            var result = await _frequencyProcessor.GetWordFrequenciesAsync(content);

            // Assert
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }
        //program class test with mock
        [Fact]
        public async Task Main_ValidInputs_ProcessesSuccessfully()
        {
            // Arrange
            var mockInputReader = new Mock<IInputFileReader>();
            var mockFrequencyAnalyzer = new Mock<IFrequencyProcessor>();
            var mockOutputWriter = new Mock<IOutputFileWriter>();

            // Simulate IAsyncEnumerable<string> for file lines
            mockInputReader
                .Setup(reader => reader.ReadFileInChunksAsync("input.txt"))
                .Returns(GetAsyncEnumerable(new[] { "hello world", "hello" }));

            mockFrequencyAnalyzer
                .Setup(analyzer => analyzer.GetWordFrequenciesAsync(It.IsAny<IAsyncEnumerable<string>>()))
                .ReturnsAsync(new Dictionary<string, int>
                {
            { "hello", 2 },
            { "world", 1 }
                });

            mockOutputWriter
                .Setup(writer => writer.WriteFrequenciesAsync("output.txt", It.IsAny<Dictionary<string, int>>()))
                .Returns(Task.CompletedTask);

            // Act
            var fileStream = mockInputReader.Object.ReadFileInChunksAsync("input.txt");
            var frequencies = await mockFrequencyAnalyzer.Object.GetWordFrequenciesAsync(fileStream);
            await mockOutputWriter.Object.WriteFrequenciesAsync("output.txt", frequencies);
        }

        #region
        // Helper method to create an IAsyncEnumerable<string>
        private static async IAsyncEnumerable<string> GetAsyncEnumerable(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                await Task.Delay(1); // Simulate asynchronous operation
                yield return line;
            }
        }
        #endregion
    }
}