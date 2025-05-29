using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WpfApp;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.Tests
{
    public class MainWindowViewModelTests
    {
        [Fact]
        public async Task Load_ShouldPopulateLists()
        {
            // Arrange
            var mockApi = new Mock<IApiClient>();
            var fakeClients = new List<Client>
            {
                new Client { Id = 1, Name = "Alice", Email = "alice@test.com", Phonenumber = "123" },
                new Client { Id = 2, Name = "Bob", Email = "bob@test.com", Phonenumber = "456" }
            };
            mockApi.Setup(api => api.List()).ReturnsAsync(fakeClients);

            var vm = new MainWindowViewModel(mockApi.Object);

            // Act
            await vm.Load();

            // Assert
            Assert.Equal(2, vm.Lists.Count);
            Assert.Equal("Alice", vm.Lists[0].Name);
        }

        [Fact]
        public async Task SaveCommand_ShouldCallApiClientSave_WhenSelectedItemIsNotNull()
        {
            // Arrange
            var mockApi = new Mock<IApiClient>();
            var vm = new MainWindowViewModel(mockApi.Object);
            vm.SelectedItem = new Client { Id = 1, Name = "Test", Email = "t@test.com", Phonenumber = "123" };

            // Act
            await Task.Run(() => vm.SaveCommand.Execute(null));

            // Assert
            mockApi.Verify(api => api.Save(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public void NewCommand_ShouldSetSelectedItemToNewClient()
        {
            // Arrange
            var mockApi = new Mock<IApiClient>();
            var vm = new MainWindowViewModel(mockApi.Object);

            // Act
            vm.NewCommand.Execute(null);

            // Assert
            Assert.NotNull(vm.SelectedItem);
            Assert.IsType<Client>(vm.SelectedItem);
        }

        [Fact]
        public async Task DeleteCommand_ShouldCallDelete_WhenConfirmed()
        {
            // Arrange
            var mockApi = new Mock<IApiClient>();
            var vm = new MainWindowViewModel(mockApi.Object);
            var client = new Client { Id = 5, Name = "Delete Me", Email = "delete@test.com", Phonenumber = "000" };

            vm.Lists.Add(client);
            vm.SelectedItem = client;
            vm.ConfirmDelete = _ => true;

            // Act
            await Task.Run(() => vm.DeleteCommand.Execute(null));

            // Assert
            mockApi.Verify(api => api.Delete(5), Times.Once);
            Assert.DoesNotContain(client, vm.Lists);
            Assert.Null(vm.SelectedItem);
        }

        [Fact]
        public async Task DeleteCommand_ShouldNotDelete_WhenNotConfirmed()
        {
            // Arrange
            var mockApi = new Mock<IApiClient>();
            var vm = new MainWindowViewModel(mockApi.Object);
            var client = new Client { Id = 3, Name = "Stay", Email = "stay@test.com", Phonenumber = "111" };

            vm.Lists.Add(client);
            vm.SelectedItem = client;
            vm.ConfirmDelete = _ => false;

            // Act
            await Task.Run(() => vm.DeleteCommand.Execute(null));

            // Assert
            mockApi.Verify(api => api.Delete(It.IsAny<int>()), Times.Never);
            Assert.Contains(client, vm.Lists);
            Assert.Equal(client, vm.SelectedItem);
        }

        [Fact]
        public void SaveCommand_CanExecute_ShouldReturnFalse_IfSelectedItemIsNull()
        {
            // Arrange
            var mockApi = new Mock<IApiClient>();
            var vm = new MainWindowViewModel(mockApi.Object);

            // Act
            var canExecute = vm.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void SaveCommand_CanExecute_ShouldReturnTrue_IfSelectedItemIsNotNull()
        {
            // Arrange
            var mockApi = new Mock<IApiClient>();
            var vm = new MainWindowViewModel(mockApi.Object)
            {
                SelectedItem = new Client()
            };

            // Act
            var canExecute = vm.SaveCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }
    }
}
