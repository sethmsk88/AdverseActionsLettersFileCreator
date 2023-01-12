using AdverseActionsLettersFileCreator.FileOperation.Commands;
using AdverseActionsLettersFileCreator.FileOperation.Models;
using MediatR;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace AdverseActionsLettersFileCreator.FileOperation.CommandHandlers
{
    public class EncryptFileCommandHandler : IRequestHandler<EncryptFileCommand, EncryptedFile>
    {
        private const string CannotFindKeyInKeyRingMessage = "Can't find encryption key in key ring.";

        public async Task<EncryptedFile> Handle(EncryptFileCommand request, CancellationToken cancellationToken)
        {
            var algorithm = CompressionAlgorithmTag.ZLib;
            var compression = 6;
            using (var publicKeyStream = new MemoryStream(request.PublicKey))
            {
                var publicKey = ReadPublicKey(publicKeyStream);

                var processedData = Compress(request.UnEncryptedData,
                    string.IsNullOrWhiteSpace(request.FileName) ?
                        PgpLiteralData.Console :
                        request.FileName,
                    algorithm,
                    compression);

                using (var memoryStream = new MemoryStream())
                {
                    Stream outputStream = memoryStream;
                    var encryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, false);
                    encryptedDataGenerator.AddMethod(publicKey);

                    using (var encryptedDataStream = encryptedDataGenerator.Open(outputStream, processedData.Length))
                    {
                        encryptedDataStream.Write(processedData, 0, processedData.Length);
                        encryptedDataStream.Close();
                    }
                    return new EncryptedFile { FileByte = memoryStream.ToArray() };
                }
            }
        }

        private static PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);

            var publicKeyRingBundle = new PgpPublicKeyRingBundle(inputStream);

            // we just loop through the collection until we find a key suitable for encryption, in the real
            // world you would probably want to be a bit smarter about this.
            // iterate through the key rings.
            foreach (PgpPublicKeyRing keyRing in publicKeyRingBundle.GetKeyRings())
            {
                foreach (PgpPublicKey publicKey in keyRing.GetPublicKeys())
                {
                    if (publicKey.IsEncryptionKey)
                    {
                        return publicKey;
                    }
                }
            }

            throw new ArgumentException(CannotFindKeyInKeyRingMessage);
        }
        private static byte[] Compress(byte[] clearData, string fileName, CompressionAlgorithmTag algorithm, int? compression)
        {
            using (var memoryStream = new MemoryStream())
            {
                var compressedDataGenerator = compression.HasValue
                    ? new PgpCompressedDataGenerator(algorithm, compression.Value)
                    : new PgpCompressedDataGenerator(algorithm);

                using (var compressedDataStream =
                    compressedDataGenerator.Open(memoryStream)) // open it with the final destination
                {
                    var literalDataGenerator = new PgpLiteralDataGenerator();

                    using (var literalDataStream =
                        literalDataGenerator.Open(
                            compressedDataStream, // the compressed output stream
                            PgpLiteralData.Binary,
                            fileName, // "filename" to store
                            clearData.Length, // length of clear data
                            DateTime.UtcNow // current time
                        ))
                    {
                        literalDataStream.Write(clearData, 0, clearData.Length);
                        literalDataStream.Close();
                    }

                    compressedDataGenerator.Close();

                    return memoryStream.ToArray();
                }
            }
        }
    }
}
