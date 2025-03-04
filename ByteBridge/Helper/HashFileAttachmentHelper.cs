namespace ByteBridge.Helper
{
    public static class HashFileAttachmentHelper
    {
        public static async Task<string> CalculateFileHashAsync(Stream fileAttachment)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(fileAttachment);
            return BitConverter.ToString(hashBytes, 0, hashBytes.Length).Replace("-", "").ToLower();
        }
    }
}