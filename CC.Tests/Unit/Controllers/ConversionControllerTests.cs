using CC.Application.Constants;
using CC.Application.Contracts;
using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;
using CC.Application.Interfaces;
using CC.Presentation.Controllers;
using FluentAssertions;
using Moq;
using System.Net;

namespace CC.Presentation.Tests.Controllers
{
    public class ConversionControllerTests
    {
        private readonly Mock<IResponseContract<ConvertLatestResponseContract>> _mockConvertResponse;
        private readonly Mock<IResponseContract<GetRateHistoryResponseContract>> _mockHistoryResponse;
        private readonly Mock<IResponseContract<GetLatestExRateResponseContract>> _mockLatestRateResponse;
        private readonly Mock<IConversionService> _mockConversionService;
        private readonly Mock<IConversionValidator> _mockValidator;
        private readonly ConversionController _controller;

        public ConversionControllerTests()
        {
            _mockConvertResponse = new Mock<IResponseContract<ConvertLatestResponseContract>>();
            _mockHistoryResponse = new Mock<IResponseContract<GetRateHistoryResponseContract>>();
            _mockLatestRateResponse = new Mock<IResponseContract<GetLatestExRateResponseContract>>();
            _mockConversionService = new Mock<IConversionService>();
            _mockValidator = new Mock<IConversionValidator>();

            _controller = new ConversionController(
                _mockConvertResponse.Object,
                _mockHistoryResponse.Object,
                _mockLatestRateResponse.Object,
                _mockConversionService.Object,
                _mockValidator.Object);
        }

        #region Convert Tests

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task Convert_WhenCurrenciesSame_ReturnsSameAmount()
        {
            // Arrange
            var request = new ConvertLatestRequestContract
            {
                Amount = 100,
                FromCurrency = "USD",
                ToCurrency = "USD"
            };

            var expectedResponse = new ResponseContract<ConvertLatestResponseContract>
            {
                Data = new ConvertLatestResponseContract(100, "USD"),
                IsSuccess = true
            };

            _mockConvertResponse.Setup(x => x.ProcessSuccessResponse(It.IsAny<ConvertLatestResponseContract>()))
                .Returns(expectedResponse);

            // Act
            var result = await _controller.Convert(request);

            // Assert
            result.Data.Amount.Should().Be(100);
            result.Data.Currency.Should().Be("USD");
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task Convert_WhenValidationFails_ReturnsErrorResponse()
        {
            // Arrange
            var request = new ConvertLatestRequestContract
            {
                Amount = 100,
                FromCurrency = "USD",
                ToCurrency = "EUR"
            };

            var errorResponse = new ResponseContract<ConvertLatestResponseContract>
            {
                IsSuccess = false,
                Messages = new List<string> { "Invalid currency" },
                ErrorCode = "VALIDATION_ERROR"
            };

            _mockValidator.Setup(x => x.Validate(request)).Returns(new ResponseContract<object>
            {
                IsSuccess = false,
                Data = new object(), // or whatever mock object you expect
                Messages = ["Invalid currency"],
                ErrorCode = ErrorCodes.INVALID_REQUEST
            });

            _mockConvertResponse.Setup(x => x.ProcessErrorResponse(It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(errorResponse);

            // Act
            var result = await _controller.Convert(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Messages.Should().Contain("Invalid currency");
            result.ErrorCode.Should().Be("VALIDATION_ERROR");
        }

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task Convert_WhenServiceReturnsSuccess_ReturnsConvertedAmount()
        {
            // Arrange
            var request = new ConvertLatestRequestContract
            {
                Amount = 100,
                FromCurrency = "USD",
                ToCurrency = "EUR"
            };

            var serviceResponse = new ResponseContract<ConvertLatestResponseContract>
            {
                Data = new ConvertLatestResponseContract(85, "EUR"),
                IsSuccess = true
            };

            _mockValidator.Setup(x => x.Validate(request)).Returns(new ResponseContract<object>
            {
                IsSuccess = true,
                Data = new object(),
                Messages = new List<string>(),
                ErrorCode = string.Empty
            });

            _mockConversionService.Setup(x => x.ConvertAsync(request)).ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.Convert(request);

            // Assert
            result.Data.Amount.Should().Be(85);
            result.Data.Currency.Should().Be("EUR");
            result.IsSuccess.Should().BeTrue();
        }

        #endregion

        #region GetLatestExchangeRate Tests

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task GetLatestExchangeRate_WhenValidationFails_ReturnsErrorResponse()
        {
            // Arrange
            var request = new GetLatestExRateRequestContract { Currency = "INVALID" };
            var errorResponse = new ResponseContract<GetLatestExRateResponseContract>
            {
                IsSuccess = false,
                Messages = new List<string> { "Invalid currency" },
                ErrorCode = "VALIDATION_ERROR"
            };

            _mockValidator.Setup(x => x.Validate(request)).Returns(new ResponseContract<object>
            {
                IsSuccess = false,
                Data = new object(),
                Messages = new List<string>(),
                ErrorCode = ErrorCodes.INVALID_REQUEST
            });


            _mockLatestRateResponse.Setup(x => x.ProcessErrorResponse(It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(errorResponse);

            // Act
            var result = await _controller.GetLatestExchangeRate(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Messages.Should().Contain("Invalid currency");
            result.ErrorCode.Should().Be("VALIDATION_ERROR");
        }

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task GetLatestExchangeRate_WhenServiceThrowsException_ReturnsErrorResponse()
        {
            // Arrange
            var request = new GetLatestExRateRequestContract { Currency = "USD" };
            var exception = new Exception("Service error");
            var errorResponse = new ResponseContract<GetLatestExRateResponseContract>
            {
                IsSuccess = false,
                Messages = new List<string> { "Service error" },
                ErrorCode = "SERVICE_ERROR"
            };

            _mockValidator.Setup(x => x.Validate(request)).Returns(new ResponseContract<object>
            {
                IsSuccess = true,
                Data = new object(),
                Messages = new List<string>(),
                ErrorCode = string.Empty
            });
            
            _mockLatestRateResponse.Setup(x => x.HandleException(exception))
                .Returns(errorResponse);

            _mockConversionService.Setup(x => x.GetLatestExchangeRateAsync(request))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.GetLatestExchangeRate(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Messages.Should().Contain("Service error");
            result.ErrorCode.Should().Be("SERVICE_ERROR");
        }

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task GetLatestExchangeRate_WhenSuccessful_ReturnsRates()
        {
            // Arrange
            var request = new GetLatestExRateRequestContract { Currency = "USD" };
            var rates = new Dictionary<string, decimal> { { "EUR", 0.85m }, { "GBP", 0.75m } };
            var serviceResponse = new ResponseContract<GetLatestExRateResponseContract>
            {
                Data = new GetLatestExRateResponseContract { Rates = rates },
                IsSuccess = true
            };

            _mockValidator.Setup(x => x.Validate(request)).Returns(new ResponseContract<object>
            {
                IsSuccess = true,
                Data = new object(),
                Messages = new List<string>(),
                ErrorCode = string.Empty
            });
            _mockConversionService.Setup(x => x.GetLatestExchangeRateAsync(request))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.GetLatestExchangeRate(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Rates.Should().ContainKeys("EUR", "GBP");
        }

        #endregion

        #region GetRateHistory Tests

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task GetRateHistory_WhenValidationFails_ReturnsErrorResponse()
        {
            // Arrange
            var request = new GetRateHistoryRequestContract
            {
                Currency = "INVALID",
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now
            };

            var errorResponse = new ResponseContract<GetRateHistoryResponseContract>
            {
                IsSuccess = false,
                Messages = new List<string> { "Invalid currency" },
                ErrorCode = "VALIDATION_ERROR"
            };

            _mockValidator.Setup(x => x.Validate(request)).Returns(new ResponseContract<object>
            {
                IsSuccess = false,
                Data = new object(),
                Messages = new List<string>(),
                ErrorCode = ErrorCodes.INVALID_REQUEST
            });
            
            _mockHistoryResponse.Setup(x => x.ProcessErrorResponse(It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(errorResponse);

            // Act
            var result = await _controller.GetRateHistory(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Messages.Should().Contain("Invalid currency");
            result.ErrorCode.Should().Be("VALIDATION_ERROR");
        }

        [Fact]
        [Trait("Coverage", "Critical")]
        public async Task GetRateHistory_WhenServiceReturnsSuccess_ReturnsHistoricalData()
        {
            // Arrange
            var request = new GetRateHistoryRequestContract
            {
                Currency = "USD",
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now,
                PageNumber = 1,
                PageSize = 10
            };

            var historicalData = new GetRateHistoryResponseContract
            {
                Rates = new Dictionary<string, Dictionary<string, decimal>>
                {
                    { "2023-01-01", new Dictionary<string, decimal> { { "EUR", 0.85m } } }
                }
            };

            var serviceResponse = new ResponseContract<GetRateHistoryResponseContract>
            {
                Data = historicalData,
                IsSuccess = true
            };

            _mockValidator.Setup(x => x.Validate(request)).Returns(new ResponseContract<object>
            {
                IsSuccess = true,
                Data = new object(),
                Messages = new List<string>(),
                ErrorCode = string.Empty
            });
            
            _mockConversionService.Setup(x => x.GetRateHistoryAsync(request))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.GetRateHistory(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Rates.Should().ContainKey("2023-01-01");
        }

        #endregion

        // Helper class since we don't have the actual ValidationResponse
        private class ValidationResponse
        {
            public bool IsSuccess { get; }
            public List<string> Messages { get; }
            public string ErrorCode { get; }

            public ValidationResponse(bool isSuccess, List<string> messages = null, string errorCode = null)
            {
                IsSuccess = isSuccess;
                Messages = messages ?? new List<string>();
                ErrorCode = errorCode;
            }
        }
    }
}