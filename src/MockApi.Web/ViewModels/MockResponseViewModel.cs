using MockApi.Web.Models;

namespace MockApi.Web.ViewModels
{
    public class MockResponseViewModel
    {
        public Mock Mock { get; set; }
        public MockResponse MockResponse { get; set; }

        public MockResponseViewModel()
        {
            
        }

        public MockResponseViewModel(Mock mock, MockResponse mockResponse)
        {
            Mock = mock;
            MockResponse = mockResponse;
        }
    }
}