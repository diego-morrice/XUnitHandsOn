using System.Collections.Generic;
using System.Web.Http.Results;
using Bogus;
using FluentAssertions;
using Xunit;
using XUnitHandsOn.Commands;
using XUnitHandsOn.Controllers;
using XUnitHandsOn.Handlers;
using XUnitHandsOn.Models;
using XUnitHandsOn.Querys;

namespace XUnitHandsOnUnitTests
{

    public class PetControllerTest
    {
        private readonly PetController Controller;

       public PetControllerTest()
        {
            Controller = new PetController(new PetQuery(), new CreatePetCommandHandler(), new DeletePetCommandHandler());
        }

        //Atributo do XUnit
        [Fact]
        public async void Pet_Get_By_All_Should_Be_Ok()
        {
            //Chamada para a web API
            var pet = await Controller.Get();

            //Valida o tipo da resosta com FluentAssertations
            pet.Should().BeOfType<OkNegotiatedContentResult<List<PetModel>>>();
        }

        [Fact]
        public async void Pet_Get_By_All_Should__Have_Valid_Data()
        {
            var pet = await Controller.Get();

            pet.Content.Should().NotBeNullOrEmpty();

            foreach (var petItem in pet.Content)
            {
                petItem.Should().NotBeNull();
                petItem.Name.Should().NotBeNullOrEmpty();
                petItem.OwnerName.Should().NotBeNullOrEmpty();
                petItem.Type.Should().BeOfType<PetModel.PetType>();
                petItem.Gender.Should().BeOfType<PetModel.PetGender>();
            }
        }

        [Theory]
        [InlineData(0)]
        public async void Pet_Get_By_Id_Should_Be_Ok(int id)
        {
            var pet = await Controller.Get(id);

            pet.Should().BeOfType<OkNegotiatedContentResult<PetModel>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Pet_Get_By_Id_Should_Have_Valid_Data(int id)
        {
            var pet = await Controller.Get(id);

            pet.Content.Should().NotBeNull();
            pet.Content.Name.Should().NotBeNullOrEmpty();
            pet.Content.OwnerName.Should().NotBeNullOrEmpty();
            pet.Content.Type.Should().BeOfType<PetModel.PetType>();
            pet.Content.Gender.Should().BeOfType<PetModel.PetGender>();
        }

        [Theory]
        [MemberData(nameof(FakerPets), parameters:3)]
        public async void Pet_Put_Valid_Pet(CreatePetCommand pet)
        {
            var result = await Controller.Put(pet);
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void Pet_Trying_Put_Null_Pet()
        {
            var result = await Controller.Put(null);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Theory]
        [MemberData(nameof(FakerPetsInvalid), parameters: 3)]
        public async void Pet_Trying_Put_Invalid_Pet(CreatePetCommand pet)
        {
            var result = await Controller.Put(pet);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Pet_Delete_Valid_Pet(int id)
        {
            var result = await Controller.Delete(new DeletePetCommand()
            {
                Id = id
            });
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void Pet_Trying_Delete_Null_Pet()
        {
            var result = await Controller.Delete(null);
            result.Should().BeOfType<BadRequestResult>();
        }

        #region Fakers
        public static IEnumerable<object[]> FakerPets(int numTests)
        {
            var fakePets = new Faker<CreatePetCommand>(locale: "pt_BR").StrictMode(true)
                .RuleFor(p => p.Gender, f => f.PickRandom<PetModel.PetGender>().ToString())
                .RuleFor(p => p.OwnerName, (f, p) => f.Name.FirstName())
                .RuleFor(p => p.Type, f => f.PickRandom<PetModel.PetType>().ToString())
                .RuleFor(p => p.Name, (f, p) => f.Name.FirstName());

            var pets = fakePets.Generate(numTests);
            var result = new List<object[]>();

            pets.ForEach(f => result.Add(new object[] { f }));

            return result;
        }

        public static IEnumerable<object[]> FakerPetsInvalid(int numTests)
        {
            var fakePets = new Faker<CreatePetCommand>(locale: "pt_BR").StrictMode(false)
                .RuleFor(p => p.Name, (f, p) => f.Name.FirstName());

            var pets = fakePets.Generate(numTests);
            var result = new List<object[]>();

            pets.ForEach(f => result.Add(new object[] { f }));

            return result;
        }

        #endregion
    }

}
