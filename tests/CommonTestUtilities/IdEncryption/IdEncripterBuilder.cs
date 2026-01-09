using Sqids;

namespace CommonTestUtilities.IdEncryption
{
    public class IdEncripterBuilder
    {
        public static SqidsEncoder<long> Build()
        {
            return new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = "dYq5nvuMR4IUkZ6FtzHEOVKAGDmBy3sJfgCjP1ebLXQ28iN7h9paWToSrwl0xc"
            });
        }
    }
}
