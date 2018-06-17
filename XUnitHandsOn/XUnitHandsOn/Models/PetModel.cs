using XUnitHandsOn.Querys;

namespace XUnitHandsOn.Models
{
    public class PetModel
    {
        public enum PetGender
        {
            Macho,
            Femea
        }

        public enum PetType
        {
            Cachorro,
            Gato,
            Hamster,
            Peixe,
            Passarinho,
            Porco
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public PetGender Gender { get; set; }
        public string OwnerName { get; set; }
    }
}