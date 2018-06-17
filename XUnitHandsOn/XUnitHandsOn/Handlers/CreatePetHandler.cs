using System.Threading.Tasks;
using XUnitHandsOn.Commands;

namespace XUnitHandsOn.Handlers
{
    public class CreatePetCommandHandler
    {
        public CreatePetCommandHandler()
        {
            
        }

        public async Task<bool> Handle(CreatePetCommand command)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(command?.Name) || command.Name.Length < 3)
                    return false;
                if (string.IsNullOrEmpty(command?.OwnerName) || command.OwnerName.Length < 3)
                    return false;
                if (string.IsNullOrEmpty(command.Type) || command.Type.Length < 3)
                    return false;

                return true;
            });
        }
    }
}