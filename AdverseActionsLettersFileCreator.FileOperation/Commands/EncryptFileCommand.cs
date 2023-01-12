using AdverseActionsLettersFileCreator.FileOperation.Models;
using MediatR;

namespace AdverseActionsLettersFileCreator.FileOperation.Commands
{
    public class EncryptFileCommand : IRequest<EncryptedFile>
    {
        public byte[] UnEncryptedData { get; set; }
        public byte[] PublicKey { get; set; }
        public string FileName { get; set; }

        public EncryptFileCommand(byte[] unEncryptedData, byte[] publicKey, string fileName)
        {
            UnEncryptedData = unEncryptedData;
            PublicKey = publicKey;
            FileName = fileName;
        }
    }
}
