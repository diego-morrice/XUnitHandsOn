using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using XUnitHandsOn.Models;

namespace XUnitHandsOn.Querys
{
    public class PetQuery
    {

        public PetQuery()
        {
            Randomizer.Seed = new Random(123456789);
        }

        private async Task<List<PetModel>> Seed()
        {
            var petIds = 0;
            var fakePets = new Faker<PetModel>(locale: "pt_BR").StrictMode(true).RuleFor(p => p.Id, t => petIds++)
                .RuleFor(p => p.Gender, f => f.PickRandom<PetModel.PetGender>())
                .RuleFor(p => p.OwnerName, (f, p) => f.Name.FirstName())
                .RuleFor(p => p.Type, f => f.PickRandom<PetModel.PetType>())
                .RuleFor(p => p.Name, (f, p) => f.Name.FirstName());

            return await Task.Run(() => fakePets.Generate(10));
        }

        public async Task<PetModel> GetPetAsync(int id)
        {
            var result = await Task.Run(() => Seed().Result.Where(f => f.Id == id));
            return result.Any() ? result.First() : null;
        }

        public async Task<List<PetModel>> GetAllPetAsync()
        {
            return await Seed();
        }
    }
}