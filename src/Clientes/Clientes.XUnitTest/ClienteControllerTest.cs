using Clientes.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Newtonsoft.Json;
using Clientes.Domain.Dto;
using Clientes.Domain.Shared;

namespace Clientes.XUnitTest
{
    public class ClienteControllerTest : IClassFixture<ClienteTest>
    {
        ClientesController clientesController;
        public ClienteControllerTest(ClienteTest fixture)
        {
            clientesController = fixture.clientesController;
        }

        [Fact]
        public void Get_WithoutParam_Ok_Test()
        {
            var result = clientesController.Get() as OkObjectResult;

            Assert.False((result.Value as BaseResponse<List<ClienteDto>>).Data.Count <= 0);
            Assert.Equal(200, result.StatusCode);
            Assert.True((result.Value as BaseResponse<List<ClienteDto>>).Success);
        }

        [Theory]
        [InlineData("078C89D3-7D66-4DE3-A117-A80D69391675")]
        public void GetCliente_WithTestData_ThenOk_Test(string id)
        {
            var result = clientesController.Get(new Guid(id)) as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
            Assert.False((result.Value as BaseResponse<ClienteDto>).Data == null);
            Assert.True((result.Value as BaseResponse<ClienteDto>).Success);
        }

        [Theory]
        [InlineData("2DCCDCDD-ABD4-4EC8-AF13-94D2B27A1407")]
        public void GetCliente_WithTestData_ThenBadRequest_NotFound_Test(string id)
        {
            try
            {
                var result = clientesController.Get(new Guid(id)) as OkObjectResult;
                Assert.Equal(400, result.StatusCode);
                Assert.True((result.Value as BaseResponse<ClienteDto>).Data == null);
                Assert.False((result.Value as BaseResponse<ClienteDto>).Success == false);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "Não encontrado.");
            }
        }

        [Fact]
        public void AddCliente_WithTestData_ThenOk_Test()
        {
            var newcliente = new ClienteDto() { Idade = 55, Nome = "Test Unit 2020" };
            var result = clientesController.Post(newcliente) as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
            Assert.False((result.Value as BaseResponse<ClienteDto>).Data == null);
            Assert.True((result.Value as BaseResponse<ClienteDto>).Success);
        }

        [Fact]
        public void AddCliente_WithTestData_ThenBadRequest_Idade_Test()
        {
            try
            {
                var newcliente = new ClienteDto() { Idade = 0, Nome = "Test Unit 2020" };
                var result = clientesController.Post(newcliente) as OkObjectResult;

                Assert.Equal(400, result.StatusCode);
                Assert.True((result.Value as BaseResponse<ClienteDto>).Data == null);
                Assert.False((result.Value as BaseResponse<ClienteDto>).Success == false);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "O campo [Idade] é obrigatório.");
            }
        }

        [Fact]
        public void AddCliente_WithTestData_ThenBadRequest_Nome_Test()
        {
            try
            {
                var newcliente = new ClienteDto() { Idade = 50, Nome = "" };
                var result = clientesController.Post(newcliente) as OkObjectResult;

                Assert.Equal(400, result.StatusCode);
                Assert.True((result.Value as BaseResponse<ClienteDto>).Data == null);
                Assert.False((result.Value as BaseResponse<ClienteDto>).Success == false);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "O campo [Nome] é obrigatório.");
            }
        }

        [Theory]
        [InlineData("2DCCDCDD-ABD4-4EC8-AF13-94D2B27A1407")]
        public void DeleteCliente_WithTestData_ThenBadRequest_NotFound_Test(string id)
        {
            try
            {
                var result = clientesController.Delete(new Guid(id)) as OkObjectResult;
                Assert.Equal(400, result.StatusCode);
                Assert.False((result.Value as BaseResponse).Success);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "Não encontrado.");
            }
        }

        [Fact]
        public void UpdateCliente_WithTestData_ThenOk_Test()
        {
            var newcliente = new ClienteDto() { Id = new Guid("078C89D3-7D66-4DE3-A117-A80D69391675"), Idade = 55, Nome = "Test Unit 2020" };
            var result = clientesController.Put(newcliente) as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
            Assert.False((result.Value as BaseResponse<ClienteDto>).Data == null);
            Assert.True((result.Value as BaseResponse<ClienteDto>).Success);
        }

        [Fact]
        public void UpdateCliente_WithTestData_ThenBadRequest_Idade_Test()
        {
            try
            {
                var newcliente = new ClienteDto() { Id = new Guid("078C89D3-7D66-4DE3-A117-A80D69391675"), Idade = 0, Nome = "Test Unit 2020" };
                var result = clientesController.Put(newcliente) as OkObjectResult;

                Assert.Equal(400, result.StatusCode);
                Assert.False((result.Value as BaseResponse).Success == false);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "O campo [Idade] é obrigatório.");
            }
        }

        [Fact]
        public void UpdateCliente_WithTestData_ThenBadRequest_Nome_Test()
        {
            try
            {
                var newcliente = new ClienteDto() { Id = new Guid("078C89D3-7D66-4DE3-A117-A80D69391675"), Idade = 55, Nome = "" };
                var result = clientesController.Put(newcliente) as OkObjectResult;

                Assert.Equal(400, result.StatusCode);
                Assert.True((result.Value as BaseResponse<ClienteDto>).Data == null);
                Assert.False((result.Value as BaseResponse<ClienteDto>).Success == false);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "O campo [Nome] é obrigatório.");
            }
        }

        [Fact]
        public void UpdateCliente_WithTestData_ThenBadRequest_NotFound_Test()
        {
            try
            {
                var newcliente = new ClienteDto() { Id = new Guid("B645EECA-72B9-44AE-8780-5898471D9BDB"), Idade = 55, Nome = "Test Unit 2020" };
                var result = clientesController.Put(newcliente) as OkObjectResult;

                Assert.Equal(400, result.StatusCode);
                Assert.True((result.Value as BaseResponse<ClienteDto>).Data == null);
                Assert.False((result.Value as BaseResponse<ClienteDto>).Success == false);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "Não encontrado.");
            }
        }

    }
}
