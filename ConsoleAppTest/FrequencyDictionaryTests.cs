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
        public FrequencyDictionaryTests()
        {
            _mockInputReader = new Mock<IInputFileReader>();
            _mockFrequencyAnalyzer = new Mock<IFrequencyProcessor>();
            _mockOutputWriter = new Mock<IOutputFileWriter>();
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

            var inputReader = new InputFileReader();

            try
            {
                // Act
                var result = await inputReader.ReadFileAsync(filePath);

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
            var inputReader = new InputFileReader();
            var filePath = "nonexistent.txt";

            await Assert.ThrowsAsync<FileNotFoundException>(() => inputReader.ReadFileAsync(filePath));
        }

        [Fact]
        public async Task GetWordFrequencies_ValidContent_ReturnsCorrectFrequencies()
        {
            var content = "hello world hello";
            var expected = new Dictionary<string, int>
            {
                { "hello", 2 },
                { "world", 1 }
            };

            var frequencyAnalyzer = new FrequencyProcessor();

            var result = await frequencyAnalyzer.GetWordFrequenciesAsync(content);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWordFrequencies_SpecialCharacters_IgnoresWhitespace()
        {
            // Arrange
            var content = "hello    world\nhello";
            var expected = new Dictionary<string, int>
            {
                { "hello", 2 },
                { "world", 1 }
            };

            var frequencyAnalyzer = new FrequencyProcessor();

            // Act
            var result = await frequencyAnalyzer.GetWordFrequenciesAsync(content);

            // Assert
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }


        //program class test with mock
        [Fact]
        public async Task Main_ValidInputs_ProcessesSuccessfully()
        {
            // Arrange
            _mockInputReader.Setup(reader => reader.ReadFileAsync("input.txt")).ReturnsAsync("hello world hello");
            _mockFrequencyAnalyzer
                .Setup(analyzer => analyzer.GetWordFrequenciesAsync("hello world hello"))
                    .ReturnsAsync(new Dictionary<string, int>
                    {
                        { "hello", 2 },
                        { "world", 1 }
                    });

            // Act
            string content = await _mockInputReader.Object.ReadFileAsync("input.txt");
            var frequencies = await _mockFrequencyAnalyzer.Object.GetWordFrequenciesAsync(content);
            await _mockOutputWriter.Object.WriteFrequenciesAsync("output.txt", frequencies);

            // Assert
            _mockInputReader.Verify(reader => reader.ReadFileAsync("input.txt"), Times.Once);
            _mockFrequencyAnalyzer.Verify(analyzer => analyzer.GetWordFrequenciesAsync("hello world hello"), Times.Once);
            _mockOutputWriter.Verify(writer => writer.WriteFrequenciesAsync("output.txt", It.IsAny<Dictionary<string, int>>()), Times.Once);
        }
    }
}