using System.Threading.Tasks;
using XUnitHandsOn.Commands;

namespace XUnitHandsOn.Handlers
{
    public class DeletePetCommandHandler
    {
        public DeletePetCommandHandler()
        {

        }

        public async Task<bool> Handle(DeletePetCommand command)
        {
            return await Task.Run(() =>
            {
                if (command == null)
                    return false;

                return command.Id != default(int);
            });
        }
    }
}